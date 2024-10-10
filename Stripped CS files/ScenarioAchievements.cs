using System.Collections;
using System.Collections.Generic;
using Contracts;
using FinePrint.Utilities;
using KSPAchievements;
using ns11;
using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class ScenarioAchievements : ScenarioModule
{
	public interface IAchievementProvider
	{
		void SetAchievementComplete(string id);

		bool GetAchievementComplete(string id);
	}

	public class DebugAchievementProvider : IAchievementProvider
	{
		public Dictionary<string, bool> achievements = new Dictionary<string, bool>();

		public void SetAchievementComplete(string id)
		{
			achievements[id] = true;
			Debug.Log("[DebugAchievementProvider]: Completing achievement \"" + id + "\"!");
		}

		public bool GetAchievementComplete(string id)
		{
			bool flag = achievements.ContainsKey(id) && achievements[id];
			Debug.Log("[DebugAchievementProvider]: Achievement \"" + id + "\" is " + (flag ? "" : "not") + " complete...");
			return flag;
		}
	}

	public static IAchievementProvider provider;

	public static Dictionary<string, bool> achievements;

	public bool eventsAdded;

	public override void OnLoad(ConfigNode node)
	{
	}

	public override void OnSave(ConfigNode node)
	{
	}

	public override void OnAwake()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE || HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			Object.Destroy(this);
		}
		achievements = new Dictionary<string, bool>
		{
			{ "FirstContract", false },
			{ "FirstCrewFromSpace", false },
			{ "FirstEelooFlyby", false },
			{ "FirstEVA", false },
			{ "FirstLaunchPadLaunch", false },
			{ "FirstMohoLanding", false },
			{ "FirstMunWalk", false },
			{ "FirstOrbitalDock", false },
			{ "FirstRover", false },
			{ "FirstRunwayLaunch", false },
			{ "FirstSatelliteLaunch", false },
			{ "MunFlagPlant", false },
			{ "TargetedLaunchPadLanding", false },
			{ "FlybyBop", false },
			{ "FlybyDres", false },
			{ "FlybyDuna", false },
			{ "FlybyEve", false },
			{ "FlybyGilly", false },
			{ "FlybyIke", false },
			{ "FlybyJool", false },
			{ "FlybyKerbin", false },
			{ "FlybyLaythe", false },
			{ "FlybyMinmus", false },
			{ "FlybyMoho", false },
			{ "FlybyMun", false },
			{ "FlybyPol", false },
			{ "FlybyTylo", false },
			{ "FlybyVall", false },
			{ "ReturnFromBop", false },
			{ "ReturnFromDres", false },
			{ "ReturnFromDuna", false },
			{ "ReturnFromEeloo", false },
			{ "ReturnFromEve", false },
			{ "ReturnFromGilly", false },
			{ "ReturnFromIke", false },
			{ "ReturnFromKerbin", false },
			{ "ReturnFromLaythe", false },
			{ "ReturnFromMinmus", false },
			{ "ReturnFromMoho", false },
			{ "ReturnFromMun", false },
			{ "ReturnFromPol", false },
			{ "ReturnFromTylo", false },
			{ "ReturnFromVall", false },
			{ "ReturnFromJool", false }
		};
		GameEvents.VesselSituation.onLaunch.Add(OnLaunch);
		GameEvents.VesselSituation.onFlyBy.Add(OnFlyby);
		GameEvents.VesselSituation.onOrbit.Add(OnOrbit);
		GameEvents.VesselSituation.onLand.Add(OnLanding);
		GameEvents.VesselSituation.onReturnFromOrbit.Add(OnReturnFromOrbit);
		GameEvents.VesselSituation.onTargetedLanding.Add(OnTargetedLanding);
		GameEvents.Contract.onCompleted.Add(OnContractCompleted);
		GameEvents.onVesselRecoveryProcessing.Add(OnVesselRecovered);
		GameEvents.onCrewOnEva.Add(OnCrewEVA);
		GameEvents.onFlagPlant.Add(OnFlagPlant);
		GameEvents.onPartCouple.Add(OnPartsCoupled);
		eventsAdded = true;
	}

	public void Start()
	{
		StartCoroutine(EndInTutorials());
	}

	public void OnDestroy()
	{
		if (eventsAdded)
		{
			GameEvents.VesselSituation.onLaunch.Remove(OnLaunch);
			GameEvents.VesselSituation.onFlyBy.Remove(OnFlyby);
			GameEvents.VesselSituation.onOrbit.Remove(OnOrbit);
			GameEvents.VesselSituation.onLand.Remove(OnLanding);
			GameEvents.VesselSituation.onReturnFromOrbit.Remove(OnReturnFromOrbit);
			GameEvents.VesselSituation.onTargetedLanding.Remove(OnTargetedLanding);
			GameEvents.Contract.onCompleted.Remove(OnContractCompleted);
			GameEvents.onVesselRecoveryProcessing.Remove(OnVesselRecovered);
			GameEvents.onCrewOnEva.Remove(OnCrewEVA);
			GameEvents.onFlagPlant.Remove(OnFlagPlant);
			GameEvents.onPartCouple.Remove(OnPartsCoupled);
		}
	}

	public static void SetProvider(IAchievementProvider ap)
	{
		provider = ap;
	}

	public void CheckAchievement(string id)
	{
		bool value = default(bool);
		if (!(provider == null || !achievements.TryGetValue(id, out value) || value))
		{
			if (!provider.GetAchievementComplete(id))
			{
				provider.SetAchievementComplete(id);
			}
			achievements[id] = true;
		}
	}

	public IEnumerator EndInTutorials()
	{
		yield return null;
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.scenarios == null)
		{
			yield break;
		}
		List<ProtoScenarioModule> scenarios = HighLogic.CurrentGame.scenarios;
		int count = scenarios.Count;
		TutorialScenario tutorialScenario;
		do
		{
			if (count-- > 0)
			{
				tutorialScenario = scenarios[count].moduleRef as TutorialScenario;
				continue;
			}
			yield break;
		}
		while (!(tutorialScenario != null) || !tutorialScenario.ExclusiveTutorial);
		Object.Destroy(this);
	}

	public void OnLaunch(Vessel vessel)
	{
		if (!(vessel == null))
		{
			string landedAtLast = vessel.landedAtLast;
			if (landedAtLast.Contains("LaunchPad"))
			{
				CheckAchievement("FirstLaunchPadLaunch");
			}
			else if (landedAtLast.Contains("Runway"))
			{
				CheckAchievement("FirstRunwayLaunch");
			}
		}
	}

	public void OnFlyby(Vessel vessel, CelestialBody body)
	{
		if (!(body == Planetarium.fetch.Sun))
		{
			if (body.name == "Eeloo")
			{
				CheckAchievement("FirstEelooFlyby");
			}
			else
			{
				CheckAchievement("Flyby" + body.name);
			}
		}
	}

	public void OnOrbit(Vessel vessel, CelestialBody body)
	{
		if (body.isHomeWorld)
		{
			int num = VesselUtilities.ActualCrewCapacity();
			int num2 = (vessel.loaded ? vessel.GetCrewCount() : vessel.protoVessel.GetVesselCrew().Count);
			if ((vessel.loaded ? vessel.IsControllable : vessel.protoVessel.wasControllable) && num + num2 == 0)
			{
				CheckAchievement("FirstSatelliteLaunch");
			}
		}
	}

	public void OnLanding(Vessel vessel, CelestialBody body)
	{
		if (!body.hasSolidSurface || vessel == null)
		{
			return;
		}
		if (body.name == "Moho")
		{
			CheckAchievement("FirstMohoLanding");
		}
		if (body.isHomeWorld)
		{
			VesselTripLog vesselTripLog = VesselTripLog.FromVessel(vessel);
			if (vesselTripLog.Log.HasEntry(FlightLog.EntryType.Suborbit, body.name) || vesselTripLog.Log.HasEntry(FlightLog.EntryType.Orbit, body.name) || vesselTripLog.Log.HasEntry(FlightLog.EntryType.Escape, body.name))
			{
				CheckAchievement("ReturnFromKerbin");
			}
		}
		if (!body.isHomeWorld && (vessel.loaded ? vessel.vesselType : vessel.protoVessel.vesselType) == VesselType.Rover)
		{
			CheckAchievement("FirstRover");
		}
	}

	public void OnReturnFromOrbit(Vessel vessel, CelestialBody body)
	{
		if (!body.isHomeWorld && !(body == Planetarium.fetch.Sun))
		{
			CheckAchievement("ReturnFrom" + body.name);
		}
	}

	public void OnTargetedLanding(Vessel vessel, string to, ReturnFrom from)
	{
		if (to.Contains("LaunchPad") && (from == ReturnFrom.Orbit || from == ReturnFrom.SubOrbit))
		{
			CheckAchievement("TargetedLaunchPadLanding");
		}
	}

	public void OnContractCompleted(Contract contract)
	{
		CheckAchievement("FirstContract");
	}

	public void OnVesselRecovered(ProtoVessel protoVessel, MissionRecoveryDialog dialog, float amount)
	{
		List<ProtoCrewMember> vesselCrew = protoVessel.GetVesselCrew();
		int count = vesselCrew.Count;
		while (count-- > 0)
		{
			FlightLog flightLog = vesselCrew[count].flightLog;
			int count2 = flightLog.Count;
			while (count2-- > 0)
			{
				string type = flightLog[count2].type;
				if (type == "Orbit" || type == "Suborbit")
				{
					CheckAchievement("FirstCrewFromSpace");
					return;
				}
			}
		}
	}

	public void OnCrewEVA(GameEvents.FromToAction<Part, Part> action)
	{
		if (!(action.to == null) && !(action.from == null))
		{
			CheckAchievement("FirstEVA");
			Vessel vessel = action.from.vessel;
			CelestialBody celestialBody = (vessel.loaded ? vessel.mainBody : FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]);
			if ((vessel.loaded ? vessel.situation : vessel.protoVessel.situation) == Vessel.Situations.LANDED && celestialBody.name == "Mun")
			{
				CheckAchievement("FirstMunWalk");
			}
		}
	}

	public void OnFlagPlant(Vessel vessel)
	{
		if (!(vessel == null) && (vessel.loaded ? vessel.mainBody : FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]).name == "Mun")
		{
			CheckAchievement("MunFlagPlant");
		}
	}

	public void OnPartsCoupled(GameEvents.FromToAction<Part, Part> action)
	{
		if (!(action.from == null) && !(action.to == null) && !action.from.vessel.isEVA && !action.to.vessel.isEVA && action.from.missionID != action.to.missionID)
		{
			Vessel vessel = action.to.vessel;
			if ((vessel.loaded ? vessel.situation : vessel.protoVessel.situation) == Vessel.Situations.ORBITING)
			{
				CheckAchievement("FirstOrbitalDock");
			}
		}
	}
}
