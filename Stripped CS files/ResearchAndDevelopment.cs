using System;
using System.Collections.Generic;
using FinePrint;
using FinePrint.Utilities;
using ns11;
using ns19;
using ns9;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3112, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class ResearchAndDevelopment : ScenarioModule
{
	public static ResearchAndDevelopment Instance;

	public static Dictionary<string, ScienceExperiment> experiments;

	public static Dictionary<string, MiniBiome> minibiomesbyScienceID;

	public static Dictionary<string, MiniBiome> minibiomesbyUnityTag;

	public Dictionary<string, ProtoTechNode> protoTechNodes;

	public static Dictionary<string, string> techTitles = new Dictionary<string, string>();

	public Dictionary<string, ScienceSubject> scienceSubjects;

	public EditorPartListFilter<AvailablePart> unresearchedTechFilter;

	public EditorPartListFilter<AvailablePart> unpurchasedPartFilter;

	public Dictionary<AvailablePart, int> experimentalPartsStock;

	public ScreenMessage message;

	public List<ScienceSubjectWidget> recoveredDataWidgets;

	public float science;

	public PSystemSetup.SpaceCenterFacility RnDFacility;

	public static Func<float, ScienceSubject, float> GetReferenceDataValueFunc = (float dataAmount, ScienceSubject subject) => dataAmount / subject.dataScale * subject.subjectValue;

	public static Func<float, ScienceSubject, float> GetSubjectValueFunc = (float subjectScience, ScienceSubject subject) => Mathf.Max(0f, 1f - subjectScience / subject.scienceCap);

	public float Science => science;

	public void AddScience(float value, TransactionReasons reason)
	{
		science += value;
		if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().AllowNegativeCurrency)
		{
			science = Math.Max(science, 0f);
		}
		CurrencyModifierQuery data = new CurrencyModifierQuery(reason, 0f, value, 0f);
		GameEvents.Modifiers.OnCurrencyModifierQuery.Fire(data);
		GameEvents.Modifiers.OnCurrencyModified.Fire(data);
		GameEvents.OnScienceChanged.Fire(science, reason);
	}

	public void SetScience(float value, TransactionReasons reason)
	{
		science = value;
		if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().AllowNegativeCurrency)
		{
			science = Math.Max(science, 0f);
		}
		GameEvents.OnScienceChanged.Fire(science, reason);
	}

	public static bool CanAfford(float amount)
	{
		if (Instance != null)
		{
			return UtilMath.RoundToPlaces(Instance.Science, 1) >= UtilMath.RoundToPlaces(amount, 1);
		}
		return true;
	}

	public override void OnAwake()
	{
		if (Instance != null)
		{
			Debug.LogError("[ResearchAndDevelopment Module]: Instance already exists!", Instance.gameObject);
		}
		Instance = this;
		protoTechNodes = new Dictionary<string, ProtoTechNode>();
		scienceSubjects = new Dictionary<string, ScienceSubject>();
		recoveredDataWidgets = new List<ScienceSubjectWidget>();
		experimentalPartsStock = new Dictionary<AvailablePart, int>();
		message = new ScreenMessage("", 4f, ScreenMessageStyle.UPPER_LEFT);
		if (HighLogic.CurrentGame != null)
		{
			science = HighLogic.CurrentGame.Parameters.Career.StartingScience;
		}
		GameEvents.onGameSceneLoadRequested.Add(OnLeavingScene);
		GameEvents.onVesselRecoveryProcessing.Add(onVesselRecoveryProcessing);
		GameEvents.Modifiers.OnCurrencyModified.Add(OnCurrenciesModified);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnLeavingScene);
		GameEvents.onVesselRecoveryProcessing.Remove(onVesselRecoveryProcessing);
		GameEvents.Modifiers.OnCurrencyModified.Remove(OnCurrenciesModified);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("sci"))
		{
			science = float.Parse(node.GetValue("sci"));
		}
		protoTechNodes.Clear();
		techTitles.Clear();
		if (node.GetNode("Tech") == null)
		{
			Debug.Log("[R&D]: No tech nodes defined. Assuming initial state", base.gameObject);
			node.AddNode(getInitialTech());
		}
		ConfigNode[] nodes = node.GetNodes("Tech");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ProtoTechNode protoTechNode = new ProtoTechNode(nodes[i]);
			protoTechNodes.Add(protoTechNode.techID, protoTechNode);
		}
		ConfigNode[] nodes2 = node.GetNodes("Science");
		num = nodes2.Length;
		for (int j = 0; j < num; j++)
		{
			ScienceSubject scienceSubject = new ScienceSubject(nodes2[j]);
			scienceSubjects.Add(scienceSubject.id, scienceSubject);
		}
		experimentalPartsStock.Clear();
		if (node.HasNode("ExpParts"))
		{
			ConfigNode.ValueList values = node.GetNode("ExpParts").values;
			num = values.Count;
			for (int k = 0; k < num; k++)
			{
				ConfigNode.Value value = values[k];
				experimentalPartsStock.Add(PartLoader.getPartInfoByName(value.name), int.Parse(value.value));
			}
		}
	}

	public static void loadExperiments()
	{
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("EXPERIMENT_DEFINITION");
		int num = configNodes.Length;
		for (int i = 0; i < num; i++)
		{
			ScienceExperiment scienceExperiment = new ScienceExperiment();
			scienceExperiment.Load(configNodes[i]);
			if (!experiments.ContainsKey(scienceExperiment.id))
			{
				experiments.Add(scienceExperiment.id, scienceExperiment);
			}
			else
			{
				Debug.LogWarning("[R&D]: Error Duplicate found adding Experiment Definition: " + scienceExperiment.id);
			}
		}
		if (!experiments.ContainsKey("recovery"))
		{
			ScienceExperiment scienceExperiment2 = new ScienceExperiment();
			scienceExperiment2.id = "recovery";
			scienceExperiment2.experimentTitle = Localizer.Format("#autoLOC_300438");
			scienceExperiment2.scienceCap = 0f;
			scienceExperiment2.dataScale = 0f;
			experiments.Add(scienceExperiment2.id, scienceExperiment2);
		}
	}

	public static void loadMiniBiomes()
	{
		minibiomesbyScienceID = new Dictionary<string, MiniBiome>();
		minibiomesbyUnityTag = new Dictionary<string, MiniBiome>();
		int count = FlightGlobals.Bodies.Count;
		for (int i = 0; i < count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (celestialBody.MiniBiomes == null)
			{
				continue;
			}
			for (int j = 0; j < celestialBody.MiniBiomes.Length; j++)
			{
				if (!minibiomesbyScienceID.ContainsKey(celestialBody.MiniBiomes[j].TagKeyID))
				{
					minibiomesbyScienceID.Add(celestialBody.MiniBiomes[j].TagKeyID, celestialBody.MiniBiomes[j]);
				}
				if (!minibiomesbyUnityTag.ContainsKey(celestialBody.MiniBiomes[j].TagKey))
				{
					minibiomesbyUnityTag.Add(celestialBody.MiniBiomes[j].TagKey, celestialBody.MiniBiomes[j]);
				}
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("sci", science);
		Dictionary<string, ProtoTechNode>.ValueCollection.Enumerator enumerator = protoTechNodes.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ProtoTechNode current = enumerator.Current;
			if (current.state == RDTech.State.Available)
			{
				current.Save(node.AddNode("Tech"));
			}
		}
		Dictionary<string, ScienceSubject>.ValueCollection.Enumerator enumerator2 = scienceSubjects.Values.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			enumerator2.Current.Save(node.AddNode("Science"));
		}
		if (experimentalPartsStock.Count > 0)
		{
			ConfigNode configNode = node.AddNode("ExpParts");
			Dictionary<AvailablePart, int>.Enumerator enumerator3 = experimentalPartsStock.GetEnumerator();
			while (enumerator3.MoveNext())
			{
				KeyValuePair<AvailablePart, int> current2 = enumerator3.Current;
				configNode.AddValue(current2.Key.name, current2.Value);
			}
		}
	}

	public ConfigNode getInitialTech()
	{
		ConfigNode configNode = new ConfigNode("Tech");
		configNode.AddValue("id", "start");
		configNode.AddValue("state", RDTech.State.Available.ToString());
		List<AvailablePart> list = ((PartLoader.Instance != null) ? PartLoader.LoadedPartsList : RDTestSceneLoader.LoadedPartsList);
		string text = "start";
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = list[i];
			if (availablePart.TechRequired == text)
			{
				configNode.AddValue("part", availablePart.name);
			}
		}
		return configNode;
	}

	public void Start()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			unresearchedTechFilter = new EditorPartListFilter<AvailablePart>("unresearchedPartTech", partTechAvailable);
			unpurchasedPartFilter = new EditorPartListFilter<AvailablePart>("unpurchasedParts", partModelPurchased, Localizer.Format("#autoLOC_6002378"));
			EditorPartList.Instance.ExcludeFilters.AddFilter(unresearchedTechFilter);
			EditorPartList.Instance.GreyoutFilters.AddFilter(unpurchasedPartFilter);
			EditorPartList.Instance.Refresh();
		}
		loadMiniBiomes();
		RnDFacility = PSystemSetup.Instance.GetSpaceCenterFacility("RnD");
		if ((!HighLogic.LoadedSceneIsGame || !HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch) && !(Funding.Instance == null))
		{
			return;
		}
		int count = PartLoader.LoadedPartsList.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[i];
			if (protoTechNodes.ContainsKey(availablePart.TechRequired) && protoTechNodes[availablePart.TechRequired].state == RDTech.State.Available && !protoTechNodes[availablePart.TechRequired].partsPurchased.Contains(availablePart))
			{
				protoTechNodes[availablePart.TechRequired].partsPurchased.Add(availablePart);
			}
		}
	}

	public static void AddExperimentalPart(AvailablePart ap)
	{
		if (Instance != null)
		{
			if (Instance.experimentalPartsStock.ContainsKey(ap))
			{
				Instance.experimentalPartsStock[ap]++;
			}
			else
			{
				Instance.experimentalPartsStock.Add(ap, 1);
			}
		}
	}

	public static bool IsExperimentalPart(AvailablePart ap)
	{
		if (Instance != null && ap != null)
		{
			return Instance.experimentalPartsStock.ContainsKey(ap);
		}
		return false;
	}

	public static void RemoveExperimentalPart(AvailablePart ap)
	{
		if (Instance != null && Instance.experimentalPartsStock.ContainsKey(ap))
		{
			Instance.experimentalPartsStock[ap]--;
			if (Instance.experimentalPartsStock[ap] == 0)
			{
				Instance.experimentalPartsStock.Remove(ap);
			}
		}
	}

	public void OnLeavingScene(GameScenes scene)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(unresearchedTechFilter);
			EditorPartList.Instance.GreyoutFilters.RemoveFilter(unpurchasedPartFilter);
		}
	}

	public void onVesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		if (pv == null)
		{
			return;
		}
		bool flag = mrDialog != null;
		float num = 0f;
		recoveredDataWidgets.Clear();
		List<ProtoPartSnapshot> allProtoPartsIncludingCargo = pv.GetAllProtoPartsIncludingCargo();
		for (int i = 0; i < allProtoPartsIncludingCargo.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = allProtoPartsIncludingCargo[i];
			for (int j = 0; j < protoPartSnapshot.modules.Count; j++)
			{
				ConfigNode moduleValues = protoPartSnapshot.modules[j].moduleValues;
				if (!moduleValues.HasNode("ScienceData"))
				{
					continue;
				}
				ConfigNode[] nodes = moduleValues.GetNodes("ScienceData");
				int k = 0;
				for (int num2 = nodes.Length; k < num2; k++)
				{
					ConfigNode configNode = nodes[k];
					ScienceData scienceData = new ScienceData(configNode);
					if (scienceSubjects.ContainsKey(scienceData.subjectID))
					{
						ScienceSubject scienceSubject = scienceSubjects[scienceData.subjectID];
						scienceSubject.title = scienceData.title;
						float num3 = 0f;
						float value = 1f;
						if (configNode.HasValue("data"))
						{
							num3 = float.Parse(configNode.GetValue("data"));
						}
						configNode.TryGetValue("scienceValueRatio", ref value);
						if (num3 != 0f)
						{
							num = SubmitScienceData(num3, value, scienceSubject, 1f, pv);
							if (flag)
							{
								recoveredDataWidgets.Add(ScienceSubjectWidget.Create(scienceSubject, num3, num, mrDialog));
							}
						}
						else if (flag)
						{
							Debug.LogWarning("Research & Development]: Experiment in " + protoPartSnapshot.partName + " has subject " + scienceData.subjectID + " defined, but no data was gathered.");
						}
					}
					else if (flag)
					{
						if (scienceData.subjectID != string.Empty)
						{
							Debug.LogError("[Research & Development]: Experiment in " + protoPartSnapshot.partName + " has scienceSubject " + scienceData.subjectID + " defined, but no such subject exists.");
						}
						else
						{
							Debug.Log("[Research & Development]: Experiment in " + protoPartSnapshot.partName + " has no scienceSubject defined, it seems no science was done here.");
						}
					}
				}
			}
		}
		reverseEngineerRecoveredVessel(pv, mrDialog);
		ModuleScienceLab.RecoverScienceLabs(pv, mrDialog);
		if (!flag)
		{
			return;
		}
		mrDialog.RnDOperational = RnDFacility.GetFacilityDamage() < 100f;
		if (recoveredDataWidgets.Count > 0)
		{
			int l = 0;
			for (int count = recoveredDataWidgets.Count; l < count; l++)
			{
				mrDialog.AddDataWidget(recoveredDataWidgets[l]);
				mrDialog.scienceEarned += recoveredDataWidgets[l].scienceAmount;
			}
		}
	}

	public void OnCurrenciesModified(CurrencyModifierQuery query)
	{
		float effectDelta = query.GetEffectDelta(Currency.Science);
		science += effectDelta;
		if (effectDelta != 0f)
		{
			GameEvents.OnScienceChanged.Fire(science, query.reason);
		}
	}

	public void reverseEngineerRecoveredVessel(ProtoVessel pv, MissionRecoveryDialog mrDialog)
	{
		VesselTripLog vesselTripLog = VesselTripLog.FromProtoVessel(pv);
		List<ScienceSubject> list = new List<ScienceSubject>();
		string homeBodyName = FlightGlobals.GetHomeBodyName();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		list3.Add(homeBodyName);
		CelestialBody bodyByName = FlightGlobals.GetBodyByName(homeBodyName);
		if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Orbit, homeBodyName))
		{
			list.AddRange(reverseEngineerPartsFrom(list3, list2, 10f, "Orbited", Localizer.Format("#autoLOC_300432", bodyByName.GetDisplayName())));
		}
		if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Suborbit, homeBodyName))
		{
			list.AddRange(reverseEngineerPartsFrom(list3, list2, 8f, "SubOrbited", Localizer.Format("#autoLOC_300435")));
		}
		if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Flight, homeBodyName))
		{
			list.AddRange(reverseEngineerPartsFrom(list3, list2, 5f, "Flew", Localizer.Format("#autoLOC_300438")));
		}
		if (!list2.Contains(homeBodyName))
		{
			list2.Add(homeBodyName);
		}
		list.AddRange(reverseEngineerPartsFrom(vesselTripLog.Log.GetDistinctTargets(FlightLog.EntryType.Land), list2, 15f, "Surfaced", "#autoLOC_6001096"));
		list.AddRange(reverseEngineerPartsFrom(vesselTripLog.Log.GetDistinctTargets(FlightLog.EntryType.Flight), list2, 12f, "Flew", "#autoLOC_6001097"));
		list.AddRange(reverseEngineerPartsFrom(vesselTripLog.Log.GetDistinctTargets(FlightLog.EntryType.Suborbit), list2, 10f, "SubOrbited", "#autoLOC_6001098"));
		list.AddRange(reverseEngineerPartsFrom(vesselTripLog.Log.GetDistinctTargets(FlightLog.EntryType.Orbit), list2, 8f, "Orbited", "#autoLOC_6001099"));
		list.AddRange(reverseEngineerPartsFrom(vesselTripLog.Log.GetDistinctTargets(FlightLog.EntryType.Flyby), list2, 6f, "FlewBy", "#autoLOC_6001100"));
		if (list.Count <= 0)
		{
			return;
		}
		ScienceSubject subject = null;
		float num = float.MinValue;
		int count = list.Count;
		while (count-- > 0)
		{
			ScienceSubject scienceSubject = list[count];
			if (scienceSubject.subjectValue > num)
			{
				subject = scienceSubject;
				num = scienceSubject.subjectValue;
			}
		}
		ScienceSubject scienceSubject2 = getScienceSubject(subject);
		float scienceAmount = SubmitScienceData(1f, scienceSubject2, 1f, pv, reverseEngineered: true);
		if (mrDialog != null)
		{
			recoveredDataWidgets.Add(ScienceSubjectWidget.Create(scienceSubject2, 1f, scienceAmount, mrDialog));
		}
	}

	public List<ScienceSubject> reverseEngineerPartsFrom(List<string> fromCBs, List<string> ignoreCBs, float subValue, string idVerb, string returnedFrom)
	{
		List<ScienceSubject> list = new List<ScienceSubject>();
		int count = fromCBs.Count;
		for (int i = 0; i < count; i++)
		{
			string text = fromCBs[i];
			CelestialBody bodyByName = FlightGlobals.GetBodyByName(text);
			if (!(bodyByName == null) && !ignoreCBs.Contains(text))
			{
				ignoreCBs.Add(text);
				list.Add(new ScienceSubject("recovery@" + text + idVerb, Localizer.Format(returnedFrom, bodyByName.bodyName), 1f, bodyByName.scienceValues.RecoveryValue * subValue, bodyByName.scienceValues.RecoveryValue * subValue * 1.2f));
			}
		}
		return list;
	}

	public bool partTechAvailable(AvailablePart ap)
	{
		if (ap.TechRequired == string.Empty)
		{
			return false;
		}
		if (protoTechNodes.ContainsKey(ap.TechRequired))
		{
			return protoTechNodes[ap.TechRequired].state == RDTech.State.Available;
		}
		if (experimentalPartsStock.ContainsKey(ap))
		{
			return experimentalPartsStock[ap] > 0;
		}
		return false;
	}

	public bool partModelPurchased(AvailablePart ap)
	{
		if (partTechAvailable(ap))
		{
			if (protoTechNodes.ContainsKey(ap.TechRequired) && protoTechNodes[ap.TechRequired].partsPurchased.Contains(ap))
			{
				return true;
			}
			if (experimentalPartsStock.ContainsKey(ap))
			{
				return experimentalPartsStock[ap] > 0;
			}
			return false;
		}
		return false;
	}

	public ProtoTechNode GetTechState(string techID)
	{
		if (protoTechNodes.ContainsKey(techID))
		{
			return protoTechNodes[techID];
		}
		return null;
	}

	public void SetTechState(string techID, ProtoTechNode techNode)
	{
		if (protoTechNodes.ContainsKey(techID))
		{
			protoTechNodes[techID] = techNode;
		}
		else
		{
			protoTechNodes.Add(techID, techNode);
		}
	}

	public static RDTech.State GetTechnologyState(string techID)
	{
		if (Instance == null)
		{
			return RDTech.State.Available;
		}
		if (Instance.protoTechNodes.ContainsKey(techID))
		{
			return Instance.protoTechNodes[techID].state;
		}
		return RDTech.State.Unavailable;
	}

	public static string GetTechnologyTitle(string techID)
	{
		if (techTitles.Count == 0)
		{
			RDTechTree.LoadTechTitles((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.Parameters.Career.TechTreeUrl : RDTechTree.backupTechTreeUrl, techTitles, loadFromDatabase: true);
		}
		if (techTitles.TryGetValue(techID, out var value))
		{
			return value;
		}
		return string.Empty;
	}

	public static bool PartTechAvailable(AvailablePart ap)
	{
		if (Instance != null)
		{
			return Instance.partTechAvailable(ap);
		}
		return true;
	}

	public static bool PartModelPurchased(AvailablePart ap)
	{
		if (Instance != null)
		{
			return Instance.partModelPurchased(ap);
		}
		return true;
	}

	public static ScienceExperiment GetExperiment(string experimentID)
	{
		if (experiments == null)
		{
			experiments = new Dictionary<string, ScienceExperiment>();
			loadExperiments();
		}
		if (experiments.ContainsKey(experimentID))
		{
			return experiments[experimentID];
		}
		Debug.LogWarning("[R&D]: No Experiment definition found with id " + experimentID);
		return null;
	}

	public static ScienceSubject GetExperimentSubject(ScienceExperiment experiment, ExperimentSituations situation, CelestialBody body, string biome, string displaybiome)
	{
		ScienceSubject scienceSubject = new ScienceSubject(experiment, situation, body, biome, displaybiome);
		if (!Instance)
		{
			return scienceSubject;
		}
		return Instance.getScienceSubject(scienceSubject);
	}

	public static ScienceSubject GetExperimentSubject(ScienceExperiment experiment, ExperimentSituations situation, string sourceUId, string sourceTitle, CelestialBody body, string biome, string displaybiome)
	{
		ScienceSubject scienceSubject = new ScienceSubject(experiment, situation, sourceUId, sourceTitle, body, biome, displaybiome);
		if (!Instance)
		{
			return scienceSubject;
		}
		return Instance.getScienceSubject(scienceSubject);
	}

	public static ScienceSubject GetSubjectByID(string subjectID)
	{
		if (Instance != null)
		{
			if (Instance.scienceSubjects.ContainsKey(subjectID))
			{
				return Instance.scienceSubjects[subjectID];
			}
			return null;
		}
		return null;
	}

	public static float GetReferenceDataValue(float dataAmount, ScienceSubject subject)
	{
		return GetReferenceDataValueFunc(dataAmount, subject);
	}

	public static float GetSubjectValue(float subjectScience, ScienceSubject subject)
	{
		return GetSubjectValueFunc(subjectScience, subject);
	}

	public static float GetScienceValue(float dataAmount, ScienceSubject subject, float xmitScalar = 1f)
	{
		return GetScienceValue(dataAmount, 1f, subject, xmitScalar);
	}

	public static float GetScienceValue(float dataAmount, float scienceValueRatio, ScienceSubject subject, float xmitScalar = 1f)
	{
		float referenceDataValue = GetReferenceDataValue(dataAmount, subject);
		float num = Mathf.Min(referenceDataValue * subject.scientificValue * xmitScalar, subject.scienceCap * scienceValueRatio);
		float b = Mathf.Lerp(referenceDataValue, subject.scienceCap * scienceValueRatio, xmitScalar) * xmitScalar;
		return Mathf.Max(Mathf.Min(subject.science + num, b) - subject.science, 0f);
	}

	public static float GetNextScienceValue(float dataAmount, ScienceSubject subject, float xmitScalar = 1f)
	{
		float num = subject.science + GetScienceValue(dataAmount, subject, xmitScalar);
		float num2 = subject.scientificValue;
		if (subject.applyScienceScale)
		{
			num2 = GetSubjectValue(num, subject);
		}
		float num3 = Mathf.Min(GetReferenceDataValue(dataAmount, subject) * num2 * xmitScalar, subject.scienceCap);
		float b = Mathf.Lerp(GetReferenceDataValue(dataAmount, subject), subject.scienceCap, xmitScalar) * xmitScalar;
		return Mathf.Max(Mathf.Min(num + num3, b) - num, 0f);
	}

	public ScienceSubject getScienceSubject(ScienceSubject subject)
	{
		if (scienceSubjects.ContainsKey(subject.id))
		{
			ScienceSubject scienceSubject = scienceSubjects[subject.id];
			scienceSubject.title = subject.title;
			scienceSubject.scienceCap = subject.scienceCap;
			scienceSubject.subjectValue = subject.subjectValue;
			scienceSubject.dataScale = subject.dataScale;
			return scienceSubject;
		}
		subject.scientificValue = 1f;
		scienceSubjects.Add(subject.id, subject);
		return subject;
	}

	public float SubmitScienceData(float dataAmount, ScienceSubject subject, float xmitScalar = 1f, ProtoVessel source = null, bool reverseEngineered = false)
	{
		return SubmitScienceData(dataAmount, 1f, subject, xmitScalar, source, reverseEngineered);
	}

	public float SubmitScienceData(float dataAmount, float scienceValueRatio, ScienceSubject subject, float xmitScalar = 1f, ProtoVessel source = null, bool reverseEngineered = false)
	{
		if (RnDFacility.GetFacilityDamage() >= 100f)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_300831"), message);
			}
			return 0f;
		}
		float scienceValue = GetScienceValue(dataAmount, scienceValueRatio, subject, xmitScalar);
		subject.science += scienceValue;
		if (subject.applyScienceScale)
		{
			subject.scientificValue = GetSubjectValue(subject.science, subject);
		}
		scienceValue *= HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
		TransactionReasons reason = ((xmitScalar == 1f) ? TransactionReasons.VesselRecovery : TransactionReasons.ScienceTransmission);
		string text = ScienceTransmissionRewardString(scienceValue, reason);
		Debug.Log("[Research & Development]: +" + dataAmount + " data on " + subject.title + "." + text + " Subject value is " + subject.scientificValue.ToString("0.00"));
		AddScience(scienceValue, reason);
		if (HighLogic.LoadedSceneIsFlight)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_300854", dataAmount, subject.title, text), message);
		}
		GameEvents.OnScienceRecieved.Fire(scienceValue, subject, source, reverseEngineered);
		return scienceValue;
	}

	public static List<ScienceSubject> GetSubjects()
	{
		if (!Instance)
		{
			return null;
		}
		return new List<ScienceSubject>(Instance.scienceSubjects.Values);
	}

	public static List<string> GetExperimentIDs()
	{
		if (experiments == null)
		{
			experiments = new Dictionary<string, ScienceExperiment>();
			loadExperiments();
		}
		return new List<string>(experiments.Keys);
	}

	public static List<string> GetSituationTagsDescriptions()
	{
		List<string> list = new List<string>();
		ExperimentSituations[] array = (ExperimentSituations[])Enum.GetValues(typeof(ExperimentSituations));
		for (int i = 0; i < array.Length; i++)
		{
			string item = array[i].Description();
			list.Add(item);
		}
		return list;
	}

	public static List<string> GetSituationTags()
	{
		return new List<string>(Enum.GetNames(typeof(ExperimentSituations)));
	}

	public static List<string> GetBiomeTagsLocalized(CelestialBody cb, bool includeMiniBiomes)
	{
		List<string> list = new List<string>();
		if (!(cb.BiomeMap == null) && cb.BiomeMap.Attributes != null && cb.BiomeMap.Attributes.Length != 0)
		{
			int num = cb.BiomeMap.Attributes.Length;
			for (int i = 0; i < num; i++)
			{
				list.Add(cb.BiomeMap.Attributes[i].displayname);
			}
			if (includeMiniBiomes)
			{
				list.AddRange(GetMiniBiomeTagsLocalized(cb));
			}
			return list;
		}
		return list;
	}

	public static List<string> GetMiniBiomeTagsLocalized(CelestialBody cb)
	{
		List<string> list = new List<string>();
		if (cb.MiniBiomes != null && cb.MiniBiomes.Length != 0)
		{
			int num = cb.MiniBiomes.Length;
			for (int i = 0; i < num; i++)
			{
				string getDisplayName = cb.MiniBiomes[i].GetDisplayName;
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (getDisplayName == list[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(getDisplayName);
				}
			}
			return list;
		}
		return list;
	}

	public static List<string> GetBiomeTags(CelestialBody cb, bool includeMiniBiomes)
	{
		List<string> list = new List<string>();
		if (!(cb.BiomeMap == null) && cb.BiomeMap.Attributes != null && cb.BiomeMap.Attributes.Length != 0)
		{
			int num = cb.BiomeMap.Attributes.Length;
			for (int i = 0; i < num; i++)
			{
				list.Add(cb.BiomeMap.Attributes[i].name.Replace(" ", string.Empty));
			}
			if (includeMiniBiomes)
			{
				list.AddRange(GetMiniBiomeTags(cb));
			}
			return list;
		}
		return list;
	}

	public static List<string> GetMiniBiomeTags(CelestialBody cb)
	{
		List<string> list = new List<string>();
		if (cb.MiniBiomes != null && cb.MiniBiomes.Length != 0)
		{
			int num = cb.MiniBiomes.Length;
			for (int i = 0; i < num; i++)
			{
				string tagKeyID = cb.MiniBiomes[i].TagKeyID;
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (tagKeyID == list[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(tagKeyID);
				}
			}
			return list;
		}
		return list;
	}

	public static string GetMiniBiomedisplayNameByScienceID(string TagID, bool formatted)
	{
		if (minibiomesbyScienceID == null)
		{
			loadMiniBiomes();
		}
		if (minibiomesbyScienceID.ContainsKey(TagID))
		{
			if (formatted)
			{
				return minibiomesbyScienceID[TagID].GetDisplayName;
			}
			return minibiomesbyScienceID[TagID].LocalizedTag;
		}
		return TagID;
	}

	public static string GetMiniBiomedisplayNameByUnityTag(string TagID, bool formatted)
	{
		if (minibiomesbyUnityTag == null)
		{
			loadMiniBiomes();
		}
		string text = MiniBiome.ConvertTagtoLandedAt(TagID);
		if (minibiomesbyUnityTag.ContainsKey(TagID))
		{
			if (formatted)
			{
				return minibiomesbyUnityTag[TagID].GetDisplayName;
			}
			return minibiomesbyUnityTag[TagID].LocalizedTag;
		}
		if (!string.IsNullOrEmpty(text) && minibiomesbyUnityTag.ContainsKey(text))
		{
			if (formatted)
			{
				return minibiomesbyUnityTag[text].GetDisplayName;
			}
			return minibiomesbyUnityTag[text].LocalizedTag;
		}
		return text;
	}

	public static string GetResults(string subjectID)
	{
		string[] array = subjectID.Split('@');
		string experimentID = array[0];
		string text = array[1];
		ScienceExperiment experiment = GetExperiment(experimentID);
		if (experiment != null && text != null)
		{
			string empty = string.Empty;
			if (experiment.Results.ContainsKey(text))
			{
				return experiment.Results[text];
			}
			List<string> list = new List<string>();
			Dictionary<string, string>.KeyCollection.Enumerator enumerator = experiment.Results.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (text.Contains(current.TrimEnd('*')))
				{
					list.Add(current);
				}
			}
			if (list.Count > 0)
			{
				return experiment.Results[list[UnityEngine.Random.Range(0, list.Count)]];
			}
			if (experiment.Results.ContainsKey("default"))
			{
				return experiment.Results["default"];
			}
			return experiment.experimentTitle + Localizer.Format("#autoLOC_6002268");
		}
		return string.Empty;
	}

	public static string CheckForMissingParts()
	{
		if (PartLoader.Instance == null && RDTestSceneLoader.Instance == null)
		{
			return Localizer.Format("#autoLOC_300989");
		}
		string text = string.Empty;
		bool flag = true;
		List<AvailablePart> list = ((PartLoader.Instance != null) ? PartLoader.LoadedPartsList : RDTestSceneLoader.LoadedPartsList);
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = list[i];
			if (availablePart.TechRequired == string.Empty)
			{
				text += Localizer.Format("#autoLOC_301005", availablePart.name);
				flag = false;
			}
		}
		if (flag)
		{
			text = Localizer.Format("#autoLOC_301012");
		}
		return text;
	}

	public static string PartAssignmentSummary()
	{
		if (PartLoader.Instance == null && RDTestSceneLoader.Instance == null)
		{
			return Localizer.Format("#autoLOC_301030");
		}
		List<string> list = new List<string>();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		List<AvailablePart> list2 = ((PartLoader.Instance != null) ? PartLoader.LoadedPartsList : RDTestSceneLoader.LoadedPartsList);
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = list2[i];
			string text = ((availablePart.TechRequired == string.Empty) ? Localizer.Format("#autoLOC_301043") : availablePart.TechRequired);
			if (!list.Contains(text))
			{
				list.Add(text);
				dictionary.Add(text, availablePart.name);
			}
			else
			{
				Dictionary<string, string> dictionary2 = dictionary;
				string key = text;
				dictionary2[key] = dictionary2[key] + ", " + availablePart.name;
			}
		}
		string text2 = string.Empty;
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			string text3 = list[j];
			int num = dictionary[text3].Split(',').Length;
			text2 = text2 + text3 + " (" + num + "): " + dictionary[text3] + "\n\n";
		}
		return Localizer.Format("#autoLOC_301069") + Localizer.Format("#autoLOC_301070", list.Count) + Localizer.Format("#autoLOC_301071", text2);
	}

	public void CheatAddScience(float sci)
	{
		AddScience(sci, TransactionReasons.Cheating);
	}

	public void UnlockProtoTechNode(ProtoTechNode node)
	{
		bool num = Funding.Instance == null || HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch;
		List<AvailablePart> list = new List<AvailablePart>();
		if (num)
		{
			int count = PartLoader.Instance.loadedParts.Count;
			for (int i = 0; i < count; i++)
			{
				AvailablePart availablePart = PartLoader.Instance.loadedParts[i];
				if (availablePart.TechRequired == node.techID)
				{
					list.Add(availablePart);
				}
			}
		}
		ProtoTechNode protoTechNode = new ProtoTechNode
		{
			techID = node.techID,
			state = RDTech.State.Available,
			scienceCost = node.scienceCost,
			partsPurchased = list
		};
		if (protoTechNodes.ContainsKey(protoTechNode.techID))
		{
			protoTechNodes[protoTechNode.techID] = protoTechNode;
		}
		else
		{
			protoTechNodes.Add(protoTechNode.techID, protoTechNode);
		}
	}

	public static void RefreshTechTreeUI()
	{
		RDTechTree[] array = UnityEngine.Object.FindObjectsOfType<RDTechTree>();
		int num = array.Length;
		while (num-- > 0)
		{
			array[num].SpawnTechTreeNodes();
		}
	}

	public void CheatTechnology()
	{
		ProtoTechNode[] treeTechs = AssetBase.RnDTechTree.GetTreeTechs();
		int num = treeTechs.Length;
		while (num-- > 0)
		{
			UnlockProtoTechNode(treeTechs[num]);
		}
		RefreshTechTreeUI();
		ContractDefs.Instance.TechResearched(default(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult>));
	}

	public static string CountUniversalScience()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		if (Instance != null)
		{
			if (experiments == null)
			{
				experiments = new Dictionary<string, ScienceExperiment>();
				loadExperiments();
			}
			num4 = experiments.Count;
			Dictionary<string, ScienceExperiment>.ValueCollection.Enumerator enumerator = experiments.Values.GetEnumerator();
			ExperimentSituations[] array = (ExperimentSituations[])Enum.GetValues(typeof(ExperimentSituations));
			int num8 = array.Length;
			while (enumerator.MoveNext())
			{
				ScienceExperiment current = enumerator.Current;
				int count = FlightGlobals.Bodies.Count;
				for (int i = 0; i < count; i++)
				{
					CelestialBody celestialBody = FlightGlobals.Bodies[i];
					for (int j = 0; j < num8; j++)
					{
						ExperimentSituations experimentSituations = array[j];
						float num9 = 0f;
						if (((ulong)current.situationMask & (ulong)experimentSituations) != 0L)
						{
							switch (experimentSituations)
							{
							case ExperimentSituations.FlyingHigh:
								num9 += current.scienceCap * celestialBody.scienceValues.FlyingHighDataValue;
								break;
							case ExperimentSituations.SrfLanded:
								num9 += current.scienceCap * celestialBody.scienceValues.LandedDataValue;
								break;
							case ExperimentSituations.SrfSplashed:
								num9 += current.scienceCap * celestialBody.scienceValues.SplashedDataValue;
								break;
							case ExperimentSituations.FlyingLow:
								num9 += current.scienceCap * celestialBody.scienceValues.FlyingLowDataValue;
								break;
							case ExperimentSituations.InSpaceHigh:
								num9 += current.scienceCap * celestialBody.scienceValues.InSpaceHighDataValue;
								break;
							case ExperimentSituations.InSpaceLow:
								num9 += current.scienceCap * celestialBody.scienceValues.InSpaceLowDataValue;
								break;
							}
							num5++;
							if (((ulong)current.biomeMask & (ulong)experimentSituations) != 0L)
							{
								num6++;
								if (celestialBody.BiomeMap != null)
								{
									num9 *= (float)celestialBody.BiomeMap.Attributes.Length;
								}
							}
						}
						num += num9;
					}
				}
			}
			num2 = num;
			int count2 = FlightGlobals.Bodies.Count;
			for (int k = 0; k < count2; k++)
			{
				CelestialBody celestialBody2 = FlightGlobals.Bodies[k];
				if (celestialBody2.BiomeMap != null)
				{
					num7 += celestialBody2.BiomeMap.Attributes.Length;
				}
				if (celestialBody2.isHomeWorld)
				{
					float[] array2 = new float[3] { 5f, 8f, 10f };
					int num10 = array2.Length;
					for (int l = 0; l < num10; l++)
					{
						num3 += celestialBody2.scienceValues.RecoveryValue * array2[l] * 1.2f;
					}
					continue;
				}
				float[] array3 = new float[5] { 6f, 8f, 10f, 12f, 15f };
				int num11 = array3.Length;
				for (int m = 0; m < num11; m++)
				{
					num3 += celestialBody2.scienceValues.RecoveryValue * array3[m] * 1.2f;
				}
			}
			num += num3;
		}
		else
		{
			num = 0f;
		}
		return Localizer.Format("#autoLOC_301288") + "\n" + Localizer.Format("#autoLOC_301289", num.ToString("0.0")) + "\n" + Localizer.Format("#autoLOC_301290", num2.ToString("0.0")) + "\n" + Localizer.Format("#autoLOC_301291", num3.ToString("0.0")) + "\n" + Localizer.Format("#autoLOC_301292", num4) + "\n" + Localizer.Format("#autoLOC_301293", num5) + "\n" + Localizer.Format("#autoLOC_301294", num6) + "\n" + Localizer.Format("#autoLOC_301295", num7);
	}

	public static string ScienceTransmissionRewardString(float amount, TransactionReasons reason = TransactionReasons.ScienceTransmission)
	{
		List<string> list = StringUtilities.FormattedCurrencies(0f, amount, 0f, symbols: false, verbose: false, reason, CurrencyModifierQuery.TextStyling.None);
		if (list.Count > 0)
		{
			return " " + Localizer.Format("#autoLOC_301303", StringUtilities.ThisThisAndThat(list));
		}
		return string.Empty;
	}

	public static bool ResearchedValidContractObjectives(params string[] objectiveTypes)
	{
		return ResearchedValidContractObjectives(new List<string>(objectiveTypes), copy: false);
	}

	public static bool ResearchedValidContractObjectives(List<string> objectiveTypes, bool copy = true)
	{
		if (objectiveTypes != null && objectiveTypes.Count > 0)
		{
			List<string> list = (copy ? new List<string>(objectiveTypes) : objectiveTypes);
			int count = PartLoader.LoadedPartsList.Count;
			while (count-- > 0)
			{
				AvailablePart availablePart = PartLoader.LoadedPartsList[count];
				if (availablePart.partPrefab == null || availablePart.partPrefab.Modules == null || !PartTechAvailable(availablePart))
				{
					continue;
				}
				int count2 = availablePart.partPrefab.Modules.Count;
				while (count2-- > 0)
				{
					for (int num = list.Count - 1; num >= 0; num--)
					{
						if (availablePart.partPrefab.Modules[count2].IsValidContractObjective(list[num]))
						{
							list.RemoveAt(num);
							if (list.Count <= 0)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		return false;
	}
}
