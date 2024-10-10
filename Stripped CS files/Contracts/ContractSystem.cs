using System;
using System.Collections;
using System.Collections.Generic;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using FinePrint;
using FinePrint.Contracts;
using FinePrint.Utilities;
using ModuleWheels;
using UnityEngine;
using Upgradeables;

namespace Contracts;

[KSPScenario((ScenarioCreationOptions)96, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class ContractSystem : ScenarioModule
{
	public static bool loaded;

	public static List<Type> ContractTypes = null;

	public static List<Type> MandatoryTypes = null;

	public static List<Type> ParameterTypes = null;

	public static List<Type> PredicateTypes = null;

	[SerializeField]
	public int finishedContractIDCheck = 5;

	[SerializeField]
	public List<Contract> contracts = new List<Contract>();

	[SerializeField]
	public List<Contract> contractsFinished = new List<Contract>();

	[SerializeField]
	public double lastUpdate = -1000.0;

	[SerializeField]
	public string version = "-1";

	public static int generateContractIterations = 50;

	public static float maxWarpFactorForUpdate = 100f;

	public static double updateInterval = 10.0;

	public bool updateDaemonRunning;

	public bool requireRefresh;

	public static Dictionary<string, int> ContractWeights = null;

	public static int WeightDefault = 30;

	public static int WeightMinimum = 10;

	public static int WeightMaximum = 90;

	public static int WeightAcceptDelta = 12;

	public static int WeightDeclineDelta = -8;

	public static int WeightWithdrawReadDelta = -2;

	public static int WeightWithdrawSeenDelta = -1;

	public Vessel returnVessel;

	public string errorString = "";

	public ConfigNode vesselConfigNode;

	public static ContractSystem Instance { get; set; }

	public List<Contract> Contracts => contracts;

	public List<Contract> ContractsFinished => contractsFinished;

	public static void GenerateTypes()
	{
		GenerateContractTypes();
		GenerateParameterTypes();
		GeneratePredicateTypes();
		GenerateMandatoryTypes();
	}

	public static void GenerateContractTypes()
	{
		if (ContractTypes != null)
		{
			return;
		}
		ContractTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(Contract)) && !(t == typeof(Contract)) && (string.IsNullOrEmpty(t.Namespace) || !t.Namespace.Contains("Expansions.Serenity.Contracts") || ExpansionsLoader.IsExpansionInstalled("Serenity")))
			{
				ContractTypes.Add(t);
			}
		});
		Debug.Log("[ContractSystem]: Found " + ContractTypes.Count + " contract types");
	}

	public static Type GetContractType(string typeName)
	{
		int num = 0;
		int count = ContractTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (ContractTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return ContractTypes[num];
	}

	public static void GenerateParameterTypes()
	{
		if (ParameterTypes != null)
		{
			return;
		}
		ParameterTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(ContractParameter)) && !(t == typeof(ContractParameter)) && (string.IsNullOrEmpty(t.Namespace) || !t.Namespace.Contains("Expansions.Serenity.Contracts") || ExpansionsLoader.IsExpansionInstalled("Serenity")))
			{
				ParameterTypes.Add(t);
			}
		});
		Debug.Log("[ContractSystem]: Found " + ParameterTypes.Count + " parameter types");
	}

	public static Type GetParameterType(string typeName)
	{
		int num = 0;
		int count = ParameterTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (ParameterTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return ParameterTypes[num];
	}

	public static void GeneratePredicateTypes()
	{
		if (PredicateTypes != null)
		{
			return;
		}
		PredicateTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(ContractPredicate)) && !(t == typeof(ContractPredicate)))
			{
				PredicateTypes.Add(t);
			}
		});
		Debug.Log("[ContractSystem]: Found " + PredicateTypes.Count + " predicate types");
	}

	public static Type GetPredicateType(string typeName)
	{
		int num = 0;
		int count = PredicateTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (PredicateTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return PredicateTypes[num];
	}

	public static void GenerateMandatoryTypes()
	{
		if (MandatoryTypes == null)
		{
			MandatoryTypes = new List<Type> { typeof(ExplorationContract) };
		}
	}

	public override void OnAwake()
	{
		loaded = false;
		Instance = this;
		ContractDefs.LoadContractCraftDefs();
		ContractDefs.LoadConstructionPartsList();
		GenerateTypes();
		if (!HighLogic.LoadedSceneIsEditor)
		{
			StartCoroutine(UpdateDaemon());
		}
	}

	public void OnDestroy()
	{
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			contracts[i].Unregister();
			contracts[i] = null;
		}
		int j = 0;
		for (int count2 = contractsFinished.Count; j < count2; j++)
		{
			contractsFinished[j] = null;
		}
		contracts.Clear();
		contractsFinished.Clear();
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
		GameEvents.onFlightReady.Remove(OnFlightReady);
		GameEvents.OnFlightGlobalsReady.Remove(OnFlightGlobalsReady);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.OnProgressReached.Remove(OnNodeReached);
		GameEvents.OnReputationChanged.Remove(OnReputationChanged);
		GameEvents.onGUIMissionControlSpawn.Remove(OnMissionControlSpawned);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnLoad(ConfigNode gameNode)
	{
		StartCoroutine(OnLoadRoutine(gameNode));
	}

	public IEnumerator OnLoadRoutine(ConfigNode gameNode)
	{
		yield return null;
		SystemUtilities.LoadNode(gameNode, "ContractSystem", "version", ref version, "-1", logging: false);
		if (CompatibilityUtilities.OldCareerSave(Game.Modes.CAREER, version))
		{
			CompatibilityUtilities.UpdateCareerSave(gameNode);
			version = VersioningBase.GetVersionString();
		}
		LoadContractWeights(gameNode);
		contracts.Clear();
		ConfigNode node = gameNode.GetNode("CONTRACTS");
		if (node == null)
		{
			yield break;
		}
		ConfigNode[] nodes = node.GetNodes("CONTRACT");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ConfigNode cNode = nodes[i];
			Contract contract = LoadContract(cNode);
			if (contract != null)
			{
				contracts.Add(contract);
			}
		}
		nodes = node.GetNodes("CONTRACT_FINISHED");
		int j = 0;
		for (int num2 = nodes.Length; j < num2; j++)
		{
			ConfigNode cNode = nodes[j];
			Contract contract2 = LoadContract(cNode);
			if (contract2 != null)
			{
				contractsFinished.Add(contract2);
			}
			else
			{
				Debug.LogError("[ContractSystem]: Contract " + j + " is invalid");
			}
		}
		if (node.HasValue("update"))
		{
			lastUpdate = double.Parse(node.GetValue("update"));
		}
		RegisterContracts();
		GameEvents.Contract.onContractsLoaded.Fire();
		ResetContracts();
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
		GameEvents.onFlightReady.Add(OnFlightReady);
		GameEvents.OnFlightGlobalsReady.Add(OnFlightGlobalsReady);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.OnProgressReached.Add(OnNodeReached);
		GameEvents.OnReputationChanged.Add(OnReputationChanged);
		GameEvents.onGUIMissionControlSpawn.Add(OnMissionControlSpawned);
		loaded = true;
	}

	public Contract LoadContract(ConfigNode cNode)
	{
		string value = cNode.GetValue("type");
		if (value == null)
		{
			Debug.LogError("[ContractSystem]: Contract config is invalid");
			return null;
		}
		cNode.RemoveValues("type");
		Type contractType = GetContractType(value);
		if (contractType == null)
		{
			Debug.LogError("[ContractSystem]: Contract type '" + value + "' not found");
			return null;
		}
		return Contract.Load((Contract)Activator.CreateInstance(contractType), cNode);
	}

	public void RegisterContracts()
	{
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			if (contracts[i].ContractState == Contract.State.Active)
			{
				contracts[i].Register();
			}
		}
	}

	public override void OnSave(ConfigNode gameNode)
	{
		gameNode.AddNode(SaveContractWeights());
		ConfigNode configNode = gameNode.AddNode("CONTRACTS");
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			Contract contract = contracts[i];
			ConfigNode node = configNode.AddNode("CONTRACT");
			contract.Save(node);
		}
		int j = 0;
		for (int count2 = contractsFinished.Count; j < count2; j++)
		{
			Contract contract2 = contractsFinished[j];
			ConfigNode node = configNode.AddNode("CONTRACT_FINISHED");
			contract2.Save(node);
		}
		gameNode.AddValue("update", lastUpdate.ToString("G17"));
		gameNode.AddValue("version", version);
	}

	public void OnNodeReached(ProgressNode node)
	{
		RefreshContracts();
	}

	public void OnReputationChanged(float newRep, TransactionReasons reason)
	{
		RefreshContracts();
	}

	[ContextMenu("Refresh Contracts")]
	public void RefreshContracts()
	{
		long ticks = KSPUtil.SystemDateTime.DateTimeNow().Ticks;
		int seed = (int)(ticks & 0x7FFFFFFFL) ^ (int)((ticks & 0x7FFFFFFF00000000L) >> 8);
		GetContractCounts(Reputation.UnitRep, ContractDefs.AverageAvailableContracts, out var tier, out var tier2, out var tier3);
		bool flag = false;
		if (WithdrawSurplusContracts(Contract.ContractPrestige.Trivial, tier))
		{
			flag = true;
		}
		if (WithdrawSurplusContracts(Contract.ContractPrestige.Significant, tier2))
		{
			flag = true;
		}
		if (WithdrawSurplusContracts(Contract.ContractPrestige.Exceptional, tier3))
		{
			flag = true;
		}
		if (GenerateContracts(ref seed, Contract.ContractPrestige.Trivial, tier - CountContracts(Contract.ContractPrestige.Trivial)))
		{
			flag = true;
		}
		if (GenerateContracts(ref seed, Contract.ContractPrestige.Significant, tier2 - CountContracts(Contract.ContractPrestige.Significant)))
		{
			flag = true;
		}
		if (GenerateContracts(ref seed, Contract.ContractPrestige.Exceptional, tier3 - CountContracts(Contract.ContractPrestige.Exceptional)))
		{
			flag = true;
		}
		if (flag)
		{
			GameEvents.Contract.onContractsListChanged.Fire();
		}
		requireRefresh = false;
		lastUpdate = Planetarium.fetch.time;
	}

	public static void GetContractCounts(float rep, int avgContracts, out int tier1, out int tier2, out int tier3)
	{
		float num = (float)avgContracts * Mathf.Lerp(0.5f, 1.5f, Mathf.InverseLerp(-1f, 1f, rep));
		float num2 = Mathf.InverseLerp(-1f, 1f, rep);
		float num3 = Mathf.InverseLerp(-0.33f, 0.6f, rep);
		float num4 = Mathf.InverseLerp(0.1f, 1f, rep);
		float num5 = Mathf.Floor(Mathf.Lerp(num, 2f, num2 - num3 * 0.5f));
		float num6 = Mathf.Floor(Mathf.Lerp(0f, num, num3 - num4 * 0.5f));
		float num7 = Mathf.Floor(Mathf.Lerp(0f, num, num4));
		float num8 = 1f / ((num5 + num6 + num7) / num);
		tier1 = Mathf.RoundToInt(num5 * num8);
		tier2 = Mathf.RoundToInt(num6 * num8);
		tier3 = Mathf.RoundToInt(num7 * num8);
	}

	public int CountContracts(Contract.ContractPrestige difficulty)
	{
		int num = 0;
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			if (contracts[i].Prestige == difficulty)
			{
				num++;
			}
		}
		return num;
	}

	public bool GenerateContracts(ref int seed, Contract.ContractPrestige difficulty, int count)
	{
		bool result = false;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				Contract contract = GenerateContract(ref seed, difficulty);
				if (contract == null)
				{
					break;
				}
				contract.Offer();
				contracts.Add(contract);
				if (contract.AutoAccept)
				{
					contract.Accept();
				}
				result = true;
				num++;
				continue;
			}
			return result;
		}
		return result;
	}

	public Contract GenerateContract(ref int seed, Contract.ContractPrestige difficulty)
	{
		Contract contract = null;
		int num = 0;
		while (contract == null && ++num < generateContractIterations)
		{
			if (seed == int.MaxValue)
			{
				seed = int.MinValue;
			}
			else
			{
				seed++;
			}
			contract = GenerateContract(seed, difficulty);
			if (contract == null)
			{
				continue;
			}
			int i = 0;
			for (int count = contracts.Count; i < count; i++)
			{
				if (contract.ContractID == contracts[i].ContractID)
				{
					contract.GenerateFailed();
					contract = null;
					break;
				}
			}
			if (contract == null)
			{
				continue;
			}
			for (int j = Mathf.Max(0, contractsFinished.Count - finishedContractIDCheck); j < contractsFinished.Count; j++)
			{
				if (contract.ContractID == contractsFinished[j].ContractID)
				{
					contract.GenerateFailed();
					contract = null;
					break;
				}
			}
		}
		return contract;
	}

	public Contract GenerateContract(int seed, Contract.ContractPrestige difficulty, Type contractType = null)
	{
		UnityEngine.Random.InitState(seed);
		if (contractType == null)
		{
			contractType = WeightedContractChoice();
		}
		return Contract.Generate(contractType, difficulty, seed, Contract.State.Generated);
	}

	public bool WithdrawSurplusContracts(Contract.ContractPrestige level, int maxAllowed)
	{
		bool result = false;
		int num = 0;
		int count = contracts.Count;
		while (count-- > 0)
		{
			Contract contract = contracts[count];
			if (contract.Prestige == level && contract.ContractState == Contract.State.Offered)
			{
				num++;
			}
		}
		int num2 = num - maxAllowed;
		int num3 = 0;
		while (num2 > 0 && num3 < contracts.Count)
		{
			Contract contract2 = contracts[num3];
			if (contract2.CanBeCancelled() && contract2.Prestige == level && contract2.ContractState == Contract.State.Offered)
			{
				contract2.Withdraw();
				result = true;
				num2--;
				Debug.Log("[ContractSystem]: Contract " + contract2.Title + " is no longer being offered by " + contract2.Agent.Name);
			}
			else
			{
				num3++;
			}
		}
		return result;
	}

	public IEnumerator UpdateDaemon()
	{
		yield return null;
		yield return null;
		if (updateDaemonRunning)
		{
			yield break;
		}
		updateDaemonRunning = true;
		while ((bool)this)
		{
			UpdateContracts();
			if (TimeWarp.CurrentRate < maxWarpFactorForUpdate && (Planetarium.fetch.time > lastUpdate + updateInterval || requireRefresh))
			{
				RefreshContracts();
			}
			yield return null;
		}
	}

	public void UpdateContracts()
	{
		int count = contracts.Count;
		while (count-- > 0)
		{
			Contract contract = contracts[count];
			if (contract.IsFinished())
			{
				contract.Unregister();
				if (contract.ContractState == Contract.State.Completed || contract.ContractState == Contract.State.DeadlineExpired || contract.ContractState == Contract.State.Failed || contract.ContractState == Contract.State.Cancelled)
				{
					contractsFinished.Add(contract);
				}
				contracts.RemoveAt(count);
				requireRefresh = true;
			}
			else
			{
				contract.Update();
			}
		}
		if (contracts.Count == 0)
		{
			requireRefresh = true;
		}
	}

	public void OnSceneLoadRequested(GameScenes scene)
	{
		ResetContracts();
	}

	public void OnFlightReady()
	{
		ResetContracts();
	}

	public void OnFlightGlobalsReady(bool ready)
	{
		ResetContracts();
	}

	public void OnVesselChange(Vessel vessel)
	{
		ResetContracts();
	}

	public void ResetContracts()
	{
		int count = contracts.Count;
		while (count-- > 0)
		{
			Contract contract = contracts[count];
			if (contract.ContractState == Contract.State.Active)
			{
				contract.Reset();
			}
		}
	}

	public Contract GetContractByGuid(Guid guid)
	{
		int count = contracts.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (contracts[num].ContractGuid == guid)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return contracts[num];
	}

	public T[] GetCurrentContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		List<T> list = new List<T>();
		int count = contracts.Count;
		for (int i = 0; i < count; i++)
		{
			if (contracts[i] is T val && (where == null || where(val)))
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	public bool AnyCurrentContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		int count = contracts.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(contracts[count] is T arg) || (where != null && !where(arg)));
		return true;
	}

	public T[] GetCurrentActiveContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		List<T> list = new List<T>();
		int count = contracts.Count;
		for (int i = 0; i < count; i++)
		{
			if (contracts[i] is T val && val.ContractState == Contract.State.Active && (where == null || where(val)))
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	public bool AnyCurrentActiveContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		int count = contracts.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(contracts[count] is T val) || val.ContractState != Contract.State.Active || (where != null && !where(val)));
		return true;
	}

	public bool HasCompletedContract(Type type)
	{
		int num = 0;
		int count = contractsFinished.Count;
		while (true)
		{
			if (num < count)
			{
				Contract contract = contractsFinished[num];
				if (contract.GetType() == type && contract.ContractState == Contract.State.Completed)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public T[] GetCompletedContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		List<T> list = new List<T>();
		int count = contractsFinished.Count;
		for (int i = 0; i < count; i++)
		{
			if (contractsFinished[i] is T val && val.ContractState == Contract.State.Completed && (where == null || where(val)))
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	public bool AnyCompletedContracts<T>(Func<T, bool> where = null) where T : Contract
	{
		int count = contractsFinished.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(contractsFinished[count] is T val) || val.ContractState != Contract.State.Completed || (where != null && !where(val)));
		return true;
	}

	public int GetActiveContractCount()
	{
		int num = 0;
		int count = contracts.Count;
		while (count-- > 0)
		{
			Contract contract = contracts[count];
			if (contract.ContractState == Contract.State.Active && !contract.AutoAccept)
			{
				num++;
			}
		}
		return num;
	}

	public void RebuildContracts()
	{
		List<Type> list = new List<Type>();
		List<int> list2 = new List<int>();
		List<Contract.ContractPrestige> list3 = new List<Contract.ContractPrestige>();
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			list.Add(contracts[i].GetType());
			list2.Add(contracts[i].MissionSeed);
			list3.Add(contracts[i].Prestige);
			if (contracts[i].ContractState == Contract.State.Active)
			{
				contracts[i].Cancel();
			}
		}
		contracts.Clear();
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			Contract contract = GenerateContract(list2[j], list3[j], list[j]);
			if (contract != null)
			{
				contracts.Add(contract);
				contract.Offer();
			}
		}
	}

	public void ClearContractsCurrent()
	{
		int i = 0;
		for (int count = contracts.Count; i < count; i++)
		{
			contracts[i].Kill();
		}
		contracts.Clear();
	}

	public void ClearContractsFinished()
	{
		contractsFinished.Clear();
	}

	public static int ClampedWeight(int weight)
	{
		return Math.Min(Math.Max(weight, WeightMinimum), WeightMaximum);
	}

	public static int TryGetWeight(string name)
	{
		int value = WeightDefault;
		if (ContractWeights != null)
		{
			ContractWeights.TryGetValue(name, out value);
		}
		return value;
	}

	public static int TryGetWeight(string name, out bool success)
	{
		int value = WeightDefault;
		success = ContractWeights != null && ContractWeights.TryGetValue(name, out value);
		return value;
	}

	public void LoadContractWeights(ConfigNode gameNode)
	{
		WeightAcceptDelta = ContractDefs.WeightAcceptDelta;
		WeightDeclineDelta = ContractDefs.WeightDeclineDelta;
		WeightWithdrawReadDelta = ContractDefs.WeightWithdrawReadDelta;
		WeightWithdrawSeenDelta = ContractDefs.WeightWithdrawSeenDelta;
		WeightMinimum = ContractDefs.WeightMinimum;
		WeightMinimum = ((WeightMinimum >= 0) ? WeightMinimum : 0);
		WeightMaximum = ContractDefs.WeightMaximum;
		WeightMaximum = ((WeightMaximum >= 0) ? WeightMaximum : 0);
		WeightDefault = ContractDefs.WeightDefault;
		WeightDefault = ((WeightDefault < WeightMinimum || WeightDefault > WeightMaximum) ? ((WeightMinimum + WeightMaximum) / 2) : WeightDefault);
		WeightDefault = ContractDefs.WeightDefault;
		WeightDefault = ((WeightDefault < WeightMinimum || WeightDefault > WeightMaximum) ? ((WeightMinimum + WeightMaximum) / 2) : WeightDefault);
		if (WeightMinimum > WeightMaximum)
		{
			int weightMinimum = WeightMinimum;
			WeightMinimum = WeightMaximum;
			WeightMaximum = weightMinimum;
		}
		if (WeightDefault > 0 && WeightMaximum > 0)
		{
			if (WeightMinimum <= 0 && (WeightAcceptDelta < 0 || WeightDeclineDelta < 0))
			{
				Debug.LogWarning("[ContractSystem]: Contract weight configuration settings can render contract types permanently inaccessible");
			}
		}
		else
		{
			Debug.LogWarning("[ContractSystem]: Contract weight configuration settings will not ever allow contracts to generate");
		}
		ContractWeights = new Dictionary<string, int>();
		if (ContractTypes == null)
		{
			return;
		}
		ConfigNode node = gameNode.GetNode("WEIGHTS");
		int count = ContractTypes.Count;
		while (count-- > 0)
		{
			string key = ContractTypes[count].Name;
			int result = WeightDefault;
			if (node != null && node.HasValue(key))
			{
				int.TryParse(node.GetValue(key), out result);
			}
			ContractWeights[key] = result;
		}
		if (FlightGlobals.Bodies == null)
		{
			return;
		}
		int count2 = FlightGlobals.Bodies.Count;
		while (count2-- > 0)
		{
			string key2 = FlightGlobals.Bodies[count2].name;
			int result2 = WeightDefault;
			if (node != null && node.HasValue(key2))
			{
				int.TryParse(node.GetValue(key2), out result2);
			}
			ContractWeights[key2] = result2;
		}
	}

	public ConfigNode SaveContractWeights()
	{
		ConfigNode configNode = new ConfigNode("WEIGHTS");
		if (ContractTypes == null)
		{
			return configNode;
		}
		int count = ContractTypes.Count;
		while (count-- > 0)
		{
			string text = ContractTypes[count].Name;
			int value = TryGetWeight(text);
			configNode.AddValue(text, value);
		}
		if (FlightGlobals.Bodies == null)
		{
			return configNode;
		}
		int count2 = FlightGlobals.Bodies.Count;
		while (count2-- > 0)
		{
			string text2 = FlightGlobals.Bodies[count2].name;
			int value2 = TryGetWeight(text2);
			configNode.AddValue(text2, value2);
		}
		return configNode;
	}

	public static void ResetWeights()
	{
		if (ContractTypes == null || ContractWeights == null)
		{
			return;
		}
		Debug.LogError("[ContractSystem]: Contract weights have been reset to default");
		int count = ContractTypes.Count;
		while (count-- > 0)
		{
			ContractWeights[ContractTypes[count].Name] = WeightDefault;
		}
		if (FlightGlobals.Bodies != null)
		{
			int count2 = FlightGlobals.Bodies.Count;
			while (count2-- > 0)
			{
				ContractWeights[FlightGlobals.Bodies[count2].name] = WeightDefault;
			}
		}
	}

	public static Type WeightedContractChoice()
	{
		if (ContractTypes != null && ContractTypes.Count > 0)
		{
			int num = 0;
			int count = ContractTypes.Count;
			while (count-- > 0)
			{
				num += TryGetWeight(ContractTypes[count].Name);
			}
			int num2 = UnityEngine.Random.Range(0, num);
			int count2 = ContractTypes.Count;
			while (count2-- > 0)
			{
				int num3 = TryGetWeight(ContractTypes[count2].Name);
				if (num2 >= num3)
				{
					num2 -= num3;
					continue;
				}
				num2 = count2;
				break;
			}
			return ContractTypes[num2];
		}
		Debug.LogError("[ContractSystem]: Attempted to generate contract with no contract types available");
		return null;
	}

	public static CelestialBody WeightedBodyChoice(IList<CelestialBody> bodies, System.Random generator = null)
	{
		KSPRandom kSPRandom = null;
		if (generator != null)
		{
			kSPRandom = generator as KSPRandom;
		}
		if (bodies != null && bodies.Count > 0)
		{
			int num = 0;
			int count = bodies.Count;
			while (count-- > 0)
			{
				num += TryGetWeight(bodies[count].name);
			}
			int num2 = kSPRandom?.Next(0, num) ?? UnityEngine.Random.Range(0, num);
			int count2 = bodies.Count;
			while (count2-- > 0)
			{
				int num3 = TryGetWeight(bodies[count2].name);
				if (num2 >= num3)
				{
					num2 -= num3;
					continue;
				}
				num2 = count2;
				break;
			}
			return bodies[num2];
		}
		Debug.LogError("[ContractSystem]: Attempted to generate contract with no celestial bodies available");
		return null;
	}

	public static void WeightAssignment(string name, int amount, bool ignoreLimits = false)
	{
		if (ContractWeights != null && ContractWeights.ContainsKey(name))
		{
			ContractWeights[name] = (ignoreLimits ? amount : ClampedWeight(amount));
		}
	}

	public static void WeightAdjustment(string name, int delta, bool ignoreLimits = false)
	{
		bool success;
		int num = TryGetWeight(name, out success);
		if (success)
		{
			int num2 = num + delta;
			ContractWeights[name] = (ignoreLimits ? num2 : ClampedWeight(num2));
		}
	}

	public static void AdjustWeight(string name, Contract contract)
	{
		switch (contract.ContractState)
		{
		case Contract.State.OfferExpired:
		case Contract.State.Withdrawn:
			if (contract.ContractViewed == Contract.Viewed.Read)
			{
				WeightAdjustment(name, WeightWithdrawReadDelta);
			}
			else if (contract.ContractViewed == Contract.Viewed.Seen)
			{
				WeightAdjustment(name, WeightWithdrawSeenDelta);
			}
			break;
		case Contract.State.Declined:
			WeightAdjustment(name, WeightDeclineDelta);
			break;
		case Contract.State.Active:
			WeightAdjustment(name, WeightAcceptDelta);
			break;
		}
	}

	public void OnMissionControlSpawned()
	{
		GenerateMandatoryContracts();
	}

	public void GenerateMandatoryContracts()
	{
		bool flag = false;
		for (int num = MandatoryTypes.Count - 1; num >= 0; num--)
		{
			Type type = MandatoryTypes[num];
			bool flag2 = true;
			int num2 = Contracts.Count - 1;
			while (num2 >= 0)
			{
				if (!(Contracts[num2].GetType() == type))
				{
					num2--;
					continue;
				}
				flag2 = false;
				break;
			}
			if (flag2)
			{
				Array values = Enum.GetValues(typeof(Contract.ContractPrestige));
				Contract.ContractPrestige difficulty = (Contract.ContractPrestige)values.GetValue(UnityEngine.Random.Range(0, values.Length));
				Contract contract = GenerateContract(UnityEngine.Random.Range(int.MinValue, int.MaxValue), difficulty, type);
				if (contract != null)
				{
					contract.Offer();
					contracts.Add(contract);
					if (contract.AutoAccept)
					{
						contract.Accept();
					}
					flag = true;
				}
			}
		}
		if (flag)
		{
			GameEvents.Contract.onContractsListChanged.Fire();
		}
	}

	public void SpawnPreBuiltShip(PreBuiltCraftDefinition craftDef, VesselSituation vesselSit)
	{
		StartCoroutine(ConstructShip(craftDef, vesselSit));
	}

	public IEnumerator ConstructShip(PreBuiltCraftDefinition craftDef, VesselSituation vesselSituation)
	{
		string value = "";
		craftDef.craftNode.TryGetValue("shipName", ref value);
		Debug.LogFormat("[ContractSystem]: Constructing Ship ({0})", value);
		uint uniquepersistentId = FlightGlobals.GetUniquepersistentId();
		returnVessel = null;
		vesselConfigNode = null;
		ConfigNode node = new ConfigNode();
		ShipConstruct shipOut = new ShipConstruct();
		string message = "";
		bool resetCBRequired = false;
		bool originalCBInverseRotation = false;
		bool flag = false;
		Vessel vessel = null;
		bool flag2 = false;
		Vector3d vector3d = Vector3d.zero;
		double alt = 0.0;
		float num = 0f;
		float num2 = 0f;
		if (!shipOut.LoadShip(craftDef.craftNode, uniquepersistentId, returnErrors: true, out message))
		{
			Debug.LogError("[ContractSystem] Failed to load vessel: " + value);
			Debug.LogError(message);
			yield break;
		}
		if (!vesselSituation.playerCreated)
		{
			shipOut.shipName = vesselSituation.vesselName;
			if (vesselSituation.location.situation == MissionSituation.VesselStartSituations.ORBITING && !EditorLogic.MissionCheckLaunchClamps(shipOut, shipOut.parts[0].localRoot, vesselSituation, out shipOut))
			{
				Debug.LogError("[MissionsExpansion] Failed to build mission vessel: " + vesselSituation.vesselName + " as it contains launch clamps and has a starting situation of Orbiting.");
				yield break;
			}
		}
		VesselCrewManifest crewManifest = new VesselCrewManifest();
		string text = "";
		string displaylandedAt = "";
		bool flag3;
		bool flag4 = (flag3 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.LANDED) && vesselSituation.location.vesselGroundLocation.splashed;
		bool flag5 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH;
		bool flag6 = vesselSituation.location.situation == MissionSituation.VesselStartSituations.ORBITING;
		if (flag6)
		{
			MissionSystem.setBodyRotation(vesselSituation.location.orbitSnapShot.Body, out resetCBRequired, out originalCBInverseRotation);
		}
		Orbit orbit = vesselSituation.location.orbitSnapShot.RelativeOrbit(null);
		if (flag3 || flag5)
		{
			if (flag5)
			{
				CelestialBody launchSiteBody = PSystemSetup.Instance.GetLaunchSiteBody(vesselSituation.location.launchSite);
				if (launchSiteBody != null)
				{
					vesselSituation.location.vesselGroundLocation.targetBody = launchSiteBody;
				}
			}
			orbit.referenceBody = vesselSituation.location.vesselGroundLocation.targetBody;
		}
		if (flag3)
		{
			Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
			Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
			Vector3d surfaceNVector = LatLon.GetSurfaceNVector(cf, vesselSituation.location.vesselGroundLocation.latitude, vesselSituation.location.vesselGroundLocation.longitude);
			GameObject gameObject = new GameObject("TempObject " + vesselSituation.vesselName);
			gameObject.transform.SetParent(vesselSituation.location.vesselGroundLocation.targetBody.transform);
			Vector3d vector3d2 = surfaceNVector * vesselSituation.location.vesselGroundLocation.targetBody.pqsController.radius;
			gameObject.transform.localPosition = vector3d2;
			alt = vesselSituation.location.vesselGroundLocation.targetBody.pqsController.GetSurfaceHeight(surfaceNVector, overrideQuadBuildCheck: true) - vesselSituation.location.vesselGroundLocation.targetBody.Radius;
			if (flag4)
			{
				if (alt < 0.0)
				{
					alt = 0.0;
					EditorDriver.editorFacility = vesselSituation.location.facility;
					if (!EditorLogic.MissionCheckLaunchClamps(shipOut, shipOut.parts[0].localRoot, vesselSituation, out shipOut, overrideSituationCheck: true))
					{
						Debug.LogError("[MissionsExpansion] Failed to build mission vessel: " + value + " as it contains launch clamps and has a starting situation of Orbiting.");
						yield break;
					}
				}
				else
				{
					flag4 = false;
				}
			}
			CelestialBody targetBody = vesselSituation.location.vesselGroundLocation.targetBody;
			num = (float)vesselSituation.location.vesselGroundLocation.latitude;
			num2 = (float)vesselSituation.location.vesselGroundLocation.longitude;
			Vector3d worldSurfacePosition = vesselSituation.location.vesselGroundLocation.targetBody.GetWorldSurfacePosition(num, num2, alt);
			Vector3d relSurfacePosition = targetBody.GetRelSurfacePosition(num, (double)num2 + targetBody.directRotAngle, alt);
			Quaternion quaternion = vesselSituation.location.vesselGroundLocation.rotation.quaternion;
			Quaternion quaternion2 = ((craftDef.craftNode.GetValue("type") == "VAB") ? Quaternion.Euler(-90f, -90f, -90f) : Quaternion.Euler(0f, -90f, -90f));
			QuaternionD quaternionD = Quaternion.LookRotation(relSurfacePosition) * quaternion * quaternion2;
			MissionSystem.putShiptoGround(shipOut, vesselSituation, null, worldSurfacePosition, quaternionD);
			double num3 = Vector3d.Distance(worldSurfacePosition, Vector3d.zero);
			if (num3 > 10000.0)
			{
				flag2 = true;
				vector3d = worldSurfacePosition;
				Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} Setting pos to {2}", shipOut.shipName, num3.ToString("N2"), vector3d);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		if (flag5)
		{
			text = vesselSituation.location.launchSite;
			displaylandedAt = PSystemSetup.Instance.GetLaunchSiteDisplayName(text);
			PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(vesselSituation.location.launchSite);
			if (spaceCenterFacility != null)
			{
				PQSCity componentInParent = spaceCenterFacility.facilityTransform.GetComponentInParent<PQSCity>();
				componentInParent.Orientate();
				UpgradeableFacility component = spaceCenterFacility.facilityTransform.GetComponent<UpgradeableFacility>();
				component.SetupLevels();
				component.SetLevel(MissionsUtils.GetFacilityLimit(vesselSituation.location.launchSite, component.MaxLevel + 1) - 1);
				spaceCenterFacility.SetSpawnPointsLatLonAlt();
				PSystemSetup.SpaceCenterFacility.SpawnPoint spawnPoint = spaceCenterFacility.GetSpawnPoint(vesselSituation.location.launchSite);
				if (spawnPoint == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find Facility spawn point for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				Transform spawnPointTransform = spawnPoint.GetSpawnPointTransform();
				if (spawnPointTransform != null)
				{
					MissionSystem.putShiptoGround(shipOut, vesselSituation, spawnPointTransform, Vector3.zero, Quaternion.identity);
					float num4 = Vector3.Distance(spawnPointTransform.position, Vector3.zero);
					if (num4 > 10000f)
					{
						flag2 = true;
						vector3d = componentInParent.sphere.PrecisePosition + componentInParent.PlanetRelativePosition + spawnPointTransform.localPosition;
						Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} KSC spawnPosition {2} Setting pos to {3}", shipOut.shipName, num4.ToString("N2"), spawnPointTransform.position, vector3d);
					}
					double lat = 0.0;
					double lon = 0.0;
					if (spawnPoint.latlonaltSet)
					{
						num = (float)spawnPoint.latitude;
						num2 = (float)spawnPoint.longitude;
						alt = (float)spawnPoint.altitude;
						Debug.LogFormat("[MissionSystem]: Vessel {0} Set Lat:{1}, Lon:{2}, Alt:{3} to Space Center Facility Spawn Point.", shipOut.shipName, num, num2, alt);
					}
					else
					{
						componentInParent.celestialBody.GetLatLonAlt(vector3d, out lat, out lon, out alt);
						Debug.LogFormat("[MissionSystem]: Vessel {0} Set Lat:{1}, Lon:{2}, Alt:{3} to Space Center Facility Point.", shipOut.shipName, num, num2, alt);
					}
				}
			}
			else
			{
				LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(vesselSituation.location.launchSite);
				if (launchSite == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find LaunchSite for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				if (launchSite.pqsCity != null)
				{
					launchSite.pqsCity.Orientate();
				}
				else
				{
					if (!(launchSite.pqsCity2 != null))
					{
						Debug.LogError("[MissionSystem]: Unable to find PQSCity for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
						yield break;
					}
					launchSite.pqsCity2.Orientate();
				}
				LaunchSite.SpawnPoint spawnPoint2 = launchSite.GetSpawnPoint(vesselSituation.location.launchSite);
				if (spawnPoint2 == null)
				{
					Debug.LogError("[MissionSystem]: Unable to find Facility spawn point for Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
					yield break;
				}
				Transform spawnPointTransform2 = spawnPoint2.GetSpawnPointTransform();
				if (spawnPointTransform2 != null)
				{
					MissionSystem.putShiptoGround(shipOut, vesselSituation, spawnPointTransform2, Vector3.zero, Quaternion.identity);
					float num5 = Vector3.Distance(spawnPointTransform2.position, Vector3.zero);
					if (num5 > 10000f)
					{
						flag2 = true;
						if (launchSite.isPQSCity)
						{
							vector3d = launchSite.pqsCity.sphere.PrecisePosition + launchSite.pqsCity.PlanetRelativePosition + spawnPointTransform2.localPosition;
							Debug.LogFormat("[MissionSystem]: VesselPosToSet: Sphere Pos {0} City Pos {1} spawnpoint LocalPos {2}", launchSite.pqsCity.sphere.PrecisePosition, launchSite.pqsCity.PlanetRelativePosition, spawnPointTransform2.localPosition);
						}
						else if (launchSite.isPQSCity2)
						{
							vector3d = launchSite.pqsCity2.sphere.PrecisePosition + launchSite.pqsCity2.PlanetRelativePosition + spawnPointTransform2.localPosition;
							Debug.LogFormat("[MissionSystem]: VesselPosToSet: Sphere Pos {0} City Pos {1} spawnpoint LocalPos {2}", launchSite.pqsCity2.sphere.PrecisePosition, launchSite.pqsCity2.PlanetRelativePosition, spawnPointTransform2.localPosition);
						}
						Debug.LogFormat("[MissionSystem]: Vessel {0}. Distance {1} spawnPosition {2} Setting pos to {3}", shipOut.shipName, num5.ToString("N2"), spawnPointTransform2.position, vector3d);
					}
					if (launchSite.isPQSCity)
					{
						if (spawnPoint2.latlonaltSet)
						{
							num = (float)spawnPoint2.latitude;
							num2 = (float)spawnPoint2.longitude;
							if (flag2)
							{
								alt = (float)spawnPoint2.altitude;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to SpawnPoint coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
						else
						{
							num = (float)launchSite.pqsCity.lat;
							num2 = (float)launchSite.pqsCity.lon;
							if (flag2)
							{
								alt = (float)launchSite.pqsCity.alt;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to PQSCity coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
					}
					if (launchSite.isPQSCity2)
					{
						if (spawnPoint2.latlonaltSet)
						{
							num = (float)spawnPoint2.latitude;
							num2 = (float)spawnPoint2.longitude;
							if (flag2)
							{
								alt = (float)spawnPoint2.altitude;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to SpawnPoint coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
						else
						{
							num = (float)launchSite.pqsCity2.lat;
							num2 = (float)launchSite.pqsCity2.lon;
							if (flag2)
							{
								alt = (float)launchSite.pqsCity2.alt;
							}
							Debug.LogFormat("[MissionSystem]: Ground GPS set to PQSCity2 coordinates. Lat:{0} Lon:{1} Alt:{2}", num, num2, alt);
						}
					}
				}
			}
		}
		Vessel vessel2;
		try
		{
			vessel2 = ShipConstruction.AssembleForLaunch(shipOut, text, displaylandedAt, (shipOut.missionFlag != string.Empty) ? shipOut.missionFlag : HighLogic.CurrentGame.flagURL, HighLogic.CurrentGame, crewManifest, fromShipAssembly: true, setActiveVessel: false, flag3 || flag5, preCreate: false, orbit, flag6, flag4);
			vessel2.gameObject.SetActive(value: false);
			MissionSystem.ToggleVesselComponents(vessel2, toggleValue: false);
		}
		catch (Exception)
		{
			Debug.LogError("[MissionSystem]: Unable to Assemble Vessel :" + vesselSituation.vesselName + " Vessel spawning Failed");
			yield break;
		}
		if (flag4)
		{
			vessel2.protoVessel.altitude = alt;
			vessel2.protoVessel.situation = Vessel.Situations.SPLASHED;
			vessel2.protoVessel.skipGroundPositioning = true;
			vessel2.protoVessel.vesselSpawning = false;
		}
		else
		{
			vessel2.protoVessel.situation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), vesselSituation.location.situation.ToString());
			vessel2.protoVessel.skipGroundPositioning = vessel2.skipGroundPositioning;
			vessel2.protoVessel.vesselSpawning = vessel2.vesselSpawning;
		}
		if (flag3 || flag5)
		{
			if (flag4)
			{
				vessel2.protoVessel.landed = false;
				vessel2.protoVessel.splashed = true;
			}
			else
			{
				vessel2.protoVessel.landed = true;
				vessel2.protoVessel.landedAt = vessel2.landedAt;
				vessel2.protoVessel.displaylandedAt = vessel2.displaylandedAt;
				vessel2.protoVessel.PQSminLevel = 0;
				vessel2.PQSminLevel = 0;
				vessel2.protoVessel.PQSmaxLevel = 0;
				vessel2.PQSmaxLevel = 0;
			}
			vessel2.latitude = (vessel2.protoVessel.latitude = num);
			vessel2.longitude = (vessel2.protoVessel.longitude = num2);
			if (flag2)
			{
				vessel2.altitude = (vessel2.protoVessel.altitude = alt);
			}
		}
		if (flag5)
		{
			vessel2.launchedFrom = text;
			vessel2.protoVessel.launchedFrom = text;
		}
		List<ModuleWheelBrakes> list = vessel2.FindPartModulesImplementing<ModuleWheelBrakes>();
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].brakeInput = (vesselSituation.location.brakesOn ? 1f : 0f);
			}
			if (vesselSituation.location.brakesOn)
			{
				vessel2.ActionGroups.SetGroup(KSPActionGroup.Brakes, active: true);
				int groupIndex = BaseAction.GetGroupIndex(KSPActionGroup.Brakes);
				vessel2.ActionGroups.cooldownTimes[groupIndex] = 0.0;
				vessel2.protoVessel.actionGroups = new ConfigNode();
				vessel2.ActionGroups.Save(vessel2.protoVessel.actionGroups);
			}
		}
		vessel2.protoVessel.ResetProtoPartSnapShots();
		int num6 = 0;
		for (int j = 0; j < vessel2.protoVessel.protoPartSnapshots.Count; j++)
		{
			ProtoPartSnapshot protoPartSnapshot = vessel2.protoVessel.protoPartSnapshots[j];
			protoPartSnapshot.flagURL = ((shipOut.missionFlag != string.Empty) ? shipOut.missionFlag : HighLogic.CurrentGame.flagURL);
			if (protoPartSnapshot.inverseStageIndex > num6)
			{
				num6 = protoPartSnapshot.inverseStageIndex;
			}
		}
		vessel2.protoVessel.stage = num6 + 1;
		vessel2.protoVessel.refTransform = vessel2.referenceTransformId;
		vessel2.vesselType = shipOut.vesselType;
		vessel2.protoVessel.vesselType = shipOut.vesselType;
		vessel2.protoVessel.Save(node);
		FlightGlobals.RemoveVessel(vessel2);
		if (HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			UnityEngine.Object.DestroyImmediate(vessel2.gameObject);
		}
		if (flag)
		{
			Vector3d offset = ((!(vessel != null)) ? FloatingOrigin.ReverseOffset : vessel.CoMD);
			FloatingOrigin.SetOffset(offset);
		}
		if (resetCBRequired)
		{
			if (flag6 && !HighLogic.LoadedSceneIsFlight)
			{
				vesselSituation.location.orbitSnapShot.Body.inverseRotation = originalCBInverseRotation;
				vesselSituation.location.orbitSnapShot.Body.CBUpdate();
			}
			else if (!flag6)
			{
				vesselSituation.location.vesselGroundLocation.targetBody.inverseRotation = originalCBInverseRotation;
				vesselSituation.location.vesselGroundLocation.targetBody.CBUpdate();
			}
		}
		vessel2.protoVessel.Load(HighLogic.CurrentGame.flightState, vessel2);
		if (flag6 && HighLogic.LoadedSceneIsFlight)
		{
			Instance.setVesselOrbit(vessel2);
		}
		if (HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			vessel2.protoVessel.persistentId = FlightGlobals.CheckVesselpersistentId(vessel2.protoVessel.persistentId, null, removeOldId: false, addNewId: true);
			vessel2.persistentId = vessel2.protoVessel.persistentId;
			for (int k = 0; k < vessel2.protoVessel.protoPartSnapshots.Count; k++)
			{
				vessel2.protoVessel.protoPartSnapshots[k].persistentId = FlightGlobals.CheckProtoPartSnapShotpersistentId(vessel2.protoVessel.protoPartSnapshots[k].persistentId, null, removeOldId: false, addNewId: true);
			}
			HighLogic.CurrentGame.flightState.protoVessels.Add(vessel2.protoVessel);
			if (vesselSituation.focusonSpawn)
			{
				HighLogic.CurrentGame.flightState.activeVesselIdx = HighLogic.CurrentGame.flightState.protoVessels.IndexOf(vessel2.protoVessel);
			}
			GameEvents.onNewVesselCreated.Fire(vessel2);
		}
		returnVessel = vessel2.protoVessel.vesselRef;
		if (HighLogic.LoadedSceneIsFlight && !vesselSituation.focusonSpawn)
		{
			MissionSystem.ToggleVesselComponents(vessel2, toggleValue: false);
			vessel2.GoOnRails();
			if (flag2)
			{
				vessel2.SetPosition(vector3d);
				vessel2.CoMD = vector3d;
				vessel2.CoM = vector3d;
				vessel2.protoVessel.CoM = vessel2.CoM;
			}
		}
		if (vessel2 != null && vessel2.gameObject != null)
		{
			vessel2.gameObject.SetActive(value: true);
			StartCoroutine(ReactivateComponents(vessel2));
		}
		vesselConfigNode = node;
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			StartCoroutine(MissionSystem.ResetKSCMarkers());
		}
		GameEvents.Contract.onContractPreBuiltVesselSpawned.Fire(returnVessel);
	}

	public IEnumerator ReactivateComponents(Vessel vessel)
	{
		yield return new WaitForEndOfFrame();
		if (vessel != null && vessel.parts != null && vessel.parts.Count > 0)
		{
			MissionSystem.ToggleVesselComponents(vessel, toggleValue: true);
		}
	}

	public void setVesselOrbit(Vessel vessel)
	{
		FlightGlobals.overrideOrbit = true;
		Invoke("disableOverride", 2f);
		MissionSystem.setVesselOrbitPosition(vessel);
		vessel.ignoreCollisionsFrames = 60;
		CollisionEnhancer.bypass = true;
		vessel.protoVessel = new ProtoVessel(vessel, preCreate: true);
	}

	public void disableOverride()
	{
		CollisionEnhancer.bypass = false;
		FlightGlobals.overrideOrbit = false;
	}
}
