using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;
using FinePrint.Utilities;
using UnityEngine;

namespace SentinelMission;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER
})]
public class SentinelScenario : ScenarioModule
{
	[KSPField(isPersistant = true)]
	public double NextSpawnTime = double.MinValue;

	public KSPRandom generator;

	public List<Vessel> deployedSentinels;

	public static SentinelScenario Instance { get; set; }

	public int MaxSentinelUntrackedSpaceObjects => deployedSentinels.Count * SentinelUtilities.perSentinelObjectLimit;

	public override void OnAwake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[SentinelScenario]: Instance already exists!", Instance.gameObject);
			UnityEngine.Object.Destroy(Instance);
		}
		Instance = this;
		deployedSentinels = new List<Vessel>();
	}

	public void Start()
	{
		if (generator == null)
		{
			generator = new KSPRandom();
		}
		deployedSentinels = GetDeployedSentinels();
		StartCoroutine(SentinelRoutine());
	}

	public override void OnLoad(ConfigNode node)
	{
		LoadGameDBSettings();
	}

	public void LoadGameDBSettings()
	{
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("SPACEOBJECTS_LIMITS");
		ConfigNode configNode = null;
		if (configNodes.Length != 0)
		{
			configNode = configNodes[0];
			if (configNodes.Length > 1)
			{
				Debug.LogWarning("[ScenarioDiscoverabelObjects]: Found more than one SPACEOBJECTS_LIMITS node using the first one");
			}
		}
		if (configNode == null)
		{
			Debug.LogWarning("[ScenarioDiscoverabelObjects]: No SPACEOBJECTS_LIMITS node found. Using default values");
		}
		else
		{
			configNode.TryGetValue("perSentinelSpawnGroupMaxLimit", ref SentinelUtilities.perSentinelObjectLimit);
		}
	}

	public void OnDestroy()
	{
		deployedSentinels = null;
	}

	public IEnumerator SentinelRoutine()
	{
		while ((bool)this)
		{
			if (Planetarium.GetUniversalTime() >= NextSpawnTime)
			{
				deployedSentinels = GetDeployedSentinels();
				for (int i = 0; i < deployedSentinels.Count; i++)
				{
					Vessel v = deployedSentinels[i];
					StartCoroutine(ProcessSentinelScan(v));
				}
				NextSpawnTime = Planetarium.GetUniversalTime() + (double)KSPUtil.dateTimeFormatter.Day * SentinelUtilities.RandomRange(generator, 0.5, 1.5);
			}
			while (Planetarium.GetUniversalTime() < NextSpawnTime)
			{
				yield return null;
			}
		}
	}

	public IEnumerator ProcessSentinelScan(Vessel v)
	{
		if (generator.NextDouble() > (double)SentinelUtilities.SpawnChance)
		{
			yield break;
		}
		int dayTime = KSPUtil.dateTimeFormatter.Day;
		double processTime = Planetarium.GetUniversalTime() + SentinelUtilities.RandomRange(generator, 0.0, (double)dayTime / 3.0);
		while (Planetarium.GetUniversalTime() < processTime)
		{
			yield return null;
		}
		int num = deployedSentinels.Count * SentinelUtilities.perSentinelObjectLimit;
		if (FlightGlobals.CountSpaceObjects() - ScenarioDiscoverableObjects.Instance.untrackedObjectIDs.Count >= num)
		{
			yield return null;
		}
		if (CometManager.ShouldSpawnComet())
		{
			SpawnComet(v);
			yield break;
		}
		SentinelUtilities.FindInnerAndOuterBodies(v.orbit.semiMajorAxis, out var _, out var outerBody);
		Orbit orbit = SentinelAsteroidOrbit(v.orbit);
		UntrackedObjectClass untrackedObjectClass = SentinelUtilities.WeightedAsteroidClass(generator);
		DiscoverableObjectsUtil.SpawnAsteroid(DiscoverableObjectsUtil.GenerateAsteroidName(), orbit, (uint)SentinelUtilities.RandomRange(generator), untrackedObjectClass, SentinelUtilities.RandomRange(generator, dayTime * 4, dayTime * 80), dayTime * 80);
		if (ContractSystem.Instance != null)
		{
			SentinelContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<SentinelContract>();
			for (int i = 0; i < currentContracts.Length; i++)
			{
				currentContracts[i].GetParameter<SentinelParameter>()?.DiscoverAsteroid(untrackedObjectClass, orbit.eccentricity, orbit.inclination, outerBody);
			}
		}
	}

	[ContextMenu("Spawn a Comet")]
	public void SpawnComet()
	{
		if (deployedSentinels.Count > 0)
		{
			SpawnComet(deployedSentinels[0]);
		}
	}

	public void SpawnComet(Vessel sentinel)
	{
		CometOrbitType cometType = CometManager.GenerateWeightedCometType();
		SpawnComet(sentinel, cometType);
	}

	public void SpawnComet(Vessel sentinel, string typeName)
	{
		CometOrbitType cometOrbitType = CometManager.GetCometOrbitType(typeName);
		if (cometOrbitType == null)
		{
			Debug.LogWarning("[SentinelScenario] Unable to find Spawn Comet Type: " + typeName);
		}
		else
		{
			SpawnComet(sentinel, cometOrbitType);
		}
	}

	public void SpawnComet(Vessel sentinel, CometOrbitType cometType)
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		UnityEngine.Random.InitState(seed);
		double lifeTime = (double)UnityEngine.Random.Range(ScenarioDiscoverableObjects.Instance.minUntrackedLifetime, ScenarioDiscoverableObjects.Instance.maxUntrackedLifetime) * 24.0 * 60.0 * 60.0;
		double lifeTimeMax = (double)ScenarioDiscoverableObjects.Instance.maxUntrackedLifetime * 24.0 * 60.0 * 60.0;
		UntrackedObjectClass randomObjClass = cometType.GetRandomObjClass();
		CometDefinition cometDefinition = CometManager.GenerateDefinition(cometType, randomObjClass, seed);
		double minDiscoveryDistance = FlightGlobals.GetHomeBody().orbit.semiMajorAxis * cometDefinition.vfxStartDistance;
		bool validOrbit;
		Orbit o = cometType.CalculateSentinelOrbit(sentinel.orbit, minDiscoveryDistance, out validOrbit);
		if (validOrbit)
		{
			DiscoverableObjectsUtil.SpawnComet("UnknownComet", o, cometDefinition, (uint)SentinelUtilities.RandomRange(generator), randomObjClass, lifeTime, lifeTimeMax, optimizedCollider: false, 0f);
			GameEvents.onSentinelCometDetected.Fire(sentinel);
		}
	}

	public List<Vessel> GetDeployedSentinels()
	{
		List<Vessel> list = new List<Vessel>();
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			if (vessel.loaded)
			{
				for (int j = 0; j < vessel.parts.Count; j++)
				{
					Part part = vessel.parts[j];
					if (!(part.name != SentinelUtilities.SentinelPartName))
					{
						SentinelModule sentinelModule = part.FindModuleImplementing<SentinelModule>();
						if (sentinelModule != null && sentinelModule.isTracking)
						{
							list.Add(vessel);
							break;
						}
					}
				}
				continue;
			}
			bool flag = false;
			for (int k = 0; k < vessel.protoVessel.protoPartSnapshots.Count; k++)
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[k];
				if (protoPartSnapshot.partName != SentinelUtilities.SentinelPartName)
				{
					continue;
				}
				if (flag)
				{
					break;
				}
				for (int l = 0; l < protoPartSnapshot.modules.Count; l++)
				{
					ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[l];
					if (!(protoPartSnapshot.partName != SentinelUtilities.SentinelPartName) && !(protoPartModuleSnapshot.moduleName != SentinelUtilities.SentinelModuleName))
					{
						bool value = false;
						SystemUtilities.LoadNode(protoPartModuleSnapshot.moduleValues, SentinelUtilities.SentinelModuleName, "isTracking", ref value, defaultValue: false);
						if (value)
						{
							list.Add(vessel);
							flag = true;
							break;
						}
					}
				}
			}
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!SentinelUtilities.SentinelCanScan(list[num]))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	public bool ActiveSentinelMatchingOrbit(Orbit orbit, double deviationWindow)
	{
		List<Vessel> list = new List<Vessel>();
		list = GetDeployedSentinels();
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				Vessel vessel = list[num];
				if ((vessel.loaded ? vessel.orbit : vessel.protoVessel.orbitSnapShot.Load()) != null && VesselUtilities.VesselAtOrbit(orbit, deviationWindow, vessel))
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

	public void ResetSentinels()
	{
		List<Vessel> list = GetDeployedSentinels();
		for (int i = 0; i < list.Count; i++)
		{
			Vessel vessel = list[i];
			if (vessel.loaded)
			{
				for (int j = 0; j < vessel.parts.Count; j++)
				{
					Part part = vessel.parts[j];
					if (!(part.name != SentinelUtilities.SentinelPartName))
					{
						SentinelModule sentinelModule = part.FindModuleImplementing<SentinelModule>();
						if (sentinelModule != null)
						{
							sentinelModule.isTracking = false;
						}
					}
				}
				continue;
			}
			for (int k = 0; k < vessel.protoVessel.protoPartSnapshots.Count; k++)
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[k];
				if (protoPartSnapshot.partName != SentinelUtilities.SentinelPartName)
				{
					continue;
				}
				for (int l = 0; l < protoPartSnapshot.modules.Count; l++)
				{
					ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[l];
					if (!(protoPartSnapshot.partName != SentinelUtilities.SentinelPartName))
					{
						ConfigNode configNode = protoPartModuleSnapshot.moduleValues.CreateCopy();
						configNode.SetValue("isTracking", newValue: false);
						configNode.CopyTo(protoPartModuleSnapshot.moduleValues, overwrite: true);
					}
				}
			}
		}
	}

	public Orbit SentinelAsteroidOrbit(Orbit orbit)
	{
		Orbit orbit2 = new Orbit(orbit.inclination, orbit.eccentricity, orbit.semiMajorAxis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, orbit.referenceBody);
		orbit2.UpdateFromStateVectors(orbit.pos, orbit.vel, orbit.referenceBody, Planetarium.GetUniversalTime());
		if (SentinelUtilities.FindInnerAndOuterBodies(orbit.semiMajorAxis, out var _, out var outerBody))
		{
			orbit2.semiMajorAxis = Math.Max(orbit2.semiMajorAxis, outerBody.orbit.semiMajorAxis + SentinelUtilities.RandomRange(generator, 0f - (float)outerBody.orbit.semiMajorAxis * 0.1f, (float)outerBody.orbit.semiMajorAxis * 0.1f));
		}
		orbit2.Init();
		orbit2.UpdateFromUT(Planetarium.GetUniversalTime());
		Vector3d vel;
		if (SystemUtilities.CoinFlip(generator))
		{
			double num = SentinelUtilities.WeightedRandom(generator) * SentinelUtilities.GetProgradeBurnAllowance(orbit2);
			vel = orbit2.vel + orbit2.getOrbitalVelocityAtUT(Planetarium.GetUniversalTime()).normalized * num;
		}
		else
		{
			double num2 = SentinelUtilities.WeightedRandom(generator) * SentinelUtilities.GetRetrogradeBurnAllowance(orbit2);
			vel = orbit2.vel + orbit2.getOrbitalVelocityAtUT(Planetarium.GetUniversalTime()).normalized * (0.0 - num2);
		}
		orbit2.UpdateFromStateVectors(orbit2.pos, vel, orbit2.referenceBody, Planetarium.GetUniversalTime());
		double num3 = SentinelUtilities.AdjustedSentinelViewAngle(orbit, outerBody.orbit) / 2.0;
		orbit2.meanAnomalyAtEpoch += (SystemUtilities.CoinFlip(generator) ? SentinelUtilities.WeightedRandom(generator, num3 * (Math.PI / 180.0)) : (0.0 - SentinelUtilities.WeightedRandom(generator, num3 * (Math.PI / 180.0))));
		orbit2.inclination += (SystemUtilities.CoinFlip(generator) ? SentinelUtilities.WeightedRandom(generator, num3) : (0.0 - SentinelUtilities.WeightedRandom(generator, num3)));
		return orbit2;
	}
}
