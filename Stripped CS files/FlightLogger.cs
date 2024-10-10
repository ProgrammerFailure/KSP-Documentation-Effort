using System;
using System.Collections;
using System.Collections.Generic;
using ns17;
using ns2;
using ns9;
using UnityEngine;

public class FlightLogger : MonoBehaviour
{
	public double highestAltitude;

	public double groundDistance;

	public double totalDistance;

	public double highestGee;

	public double instantGee;

	public double sustainedGee;

	public double highestSpeed;

	public double highestSpeedOverLand;

	public double missionTime;

	public bool liftOff;

	public bool missionEnd;

	public int partsLost;

	public int kerbalsKilled;

	public FlightEndModes flightEndMode;

	public Dictionary<FlightEndModes, string> endMessages = new Dictionary<FlightEndModes, string>();

	public static List<string> eventLog;

	public static FlightLogger fetch;

	public bool okToSetSpeed;

	public static bool LogGees;

	public static string lastReport = "";

	public float endFlightWait = 4f;

	public float attemptSwitchWait = 0.5f;

	public static bool LiftOff
	{
		get
		{
			if (!fetch)
			{
				return false;
			}
			return fetch.liftOff;
		}
	}

	public static double met => fetch.missionTime;

	public string missionTimestamp => KSPUtil.PrintTimeStamp(missionTime);

	public string missionTimestampWithDay => KSPUtil.PrintTimeStamp(missionTime, days: true);

	public void Start()
	{
		if (fetch != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		fetch = this;
		endMessages.Add(FlightEndModes.ABORTED, Localizer.Format("#autoLOC_135142"));
		endMessages.Add(FlightEndModes.CATASTROPHIC_FAILURE, Localizer.Format("#autoLOC_135143"));
		endMessages.Add(FlightEndModes.SUCCESS, Localizer.Format("#autoLOC_135144"));
		endMessages.Add(FlightEndModes.SUCCESSFUL_FAILURE, Localizer.Format("#autoLOC_135145"));
		eventLog = new List<string>();
		highestAltitude = 0.0;
		groundDistance = 0.0;
		totalDistance = 0.0;
		highestGee = 0.0;
		highestSpeed = 0.0;
		highestSpeedOverLand = 0.0;
		okToSetSpeed = false;
		liftOff = false;
		missionEnd = false;
		GameEvents.onJointBreak.Add(onJointBreak);
		GameEvents.onCrash.Add(onCrash);
		GameEvents.onCrashSplashdown.Add(onCrashSplashdown);
		GameEvents.onCollision.Add(onCollision);
		GameEvents.onOverheat.Add(onOverheat);
		GameEvents.onOverPressure.Add(onOverPressure);
		GameEvents.onOverG.Add(onOverG);
		GameEvents.onStageSeparation.Add(onStageSeparation);
		GameEvents.onCrewOnEva.Add(onCrewOnEva);
		GameEvents.onCrewKilled.Add(onCrewKilled);
		GameEvents.onCrewBoardVessel.Add(onCrewBoardVessel);
		GameEvents.onLaunch.Add(onLaunch);
		GameEvents.onUndock.Add(onUndock);
		GameEvents.onSplashDamage.Add(onSplashDamage);
	}

	public void OnDestroy()
	{
		GameEvents.onJointBreak.Remove(onJointBreak);
		GameEvents.onCrash.Remove(onCrash);
		GameEvents.onCrashSplashdown.Remove(onCrashSplashdown);
		GameEvents.onCollision.Remove(onCollision);
		GameEvents.onOverheat.Remove(onOverheat);
		GameEvents.onOverPressure.Remove(onOverPressure);
		GameEvents.onOverG.Remove(onOverG);
		GameEvents.onStageSeparation.Remove(onStageSeparation);
		GameEvents.onCrewOnEva.Remove(onCrewOnEva);
		GameEvents.onCrewKilled.Remove(onCrewKilled);
		GameEvents.onCrewBoardVessel.Remove(onCrewBoardVessel);
		GameEvents.onLaunch.Remove(onLaunch);
		GameEvents.onUndock.Remove(onUndock);
		GameEvents.onSplashDamage.Remove(onSplashDamage);
		lastReport = "";
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Update()
	{
		if (GameSettings.TOGGLE_STATUS_SCREEN.GetKeyDown())
		{
			if (FlightResultsDialog.isDisplaying)
			{
				FlightResultsDialog.Close();
				return;
			}
			FlightResultsDialog.showExitControls = false;
			FlightResultsDialog.allowClosingDialog = true;
			FlightResultsDialog.Display(Localizer.Format("#autoLOC_135218", Vessel.GetSituationString(FlightGlobals.ActiveVessel)));
		}
		else if (Input.GetKeyUp(KeyCode.Escape) && FlightResultsDialog.isDisplaying && !UIMasterController.Instance.CameraMode)
		{
			FlightResultsDialog.Close();
		}
	}

	public void LateUpdate()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel == null) && activeVessel.state != Vessel.State.DEAD && !FlightDriver.Pause && FlightGlobals.ready)
		{
			missionTime = activeVessel.missionTime;
			highestAltitude = Math.Max(FlightGlobals.ship_altitude, highestAltitude);
			highestSpeed = Math.Max(activeVessel.srfSpeed, highestSpeed);
			if (LogGees)
			{
				instantGee = FlightGlobals.ship_geeForce;
				highestGee = Math.Max(highestGee, instantGee);
			}
			totalDistance += activeVessel.srfSpeed * (double)TimeWarp.fixedDeltaTime;
			groundDistance += activeVessel.horizontalSrfSpeed * (double)TimeWarp.fixedDeltaTime;
		}
	}

	public void FixedUpdate()
	{
		bool flag = okToSetSpeed;
		okToSetSpeed = false;
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel == null) && activeVessel.state != Vessel.State.DEAD && !FlightDriver.Pause && FlightGlobals.ready)
		{
			okToSetSpeed = flag;
			double num = TimeWarp.fixedDeltaTime;
			totalDistance += activeVessel.srfSpeed * num;
			groundDistance += activeVessel.horizontalSrfSpeed * num;
			if (!okToSetSpeed)
			{
				okToSetSpeed = true;
			}
			else
			{
				highestSpeedOverLand = Math.Max(activeVessel.horizontalSrfSpeed, highestSpeedOverLand);
			}
		}
	}

	public static void IgnoreGeeForces(float duration)
	{
		if ((bool)fetch)
		{
			LogGees = false;
			if (fetch.IsInvoking("ResumeGForceReading"))
			{
				fetch.CancelInvoke("ResumeGForceReading");
			}
			fetch.Invoke("ResumeGForceReading", Mathf.Max(duration, 0.01f));
		}
	}

	public void ResumeGForceReading()
	{
		LogGees = true;
	}

	public static string getMissionStats()
	{
		return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("" + Localizer.Format("#autoLOC_135285", KSPUtil.PrintTimeLong(fetch.missionTime)), Localizer.Format("#autoLOC_135287", fetch.highestAltitude.ToString("N0"))), Localizer.Format("#autoLOC_135288", fetch.highestSpeed.ToString("N0"))), Localizer.Format("#autoLOC_135289", fetch.highestSpeedOverLand.ToString("N0"))), Localizer.Format("#autoLOC_135291", fetch.groundDistance.ToString("N0"))), Localizer.Format("#autoLOC_135292", fetch.totalDistance.ToString("N0"))), Localizer.Format("#autoLOC_135294", fetch.highestGee.ToString("0.0")));
	}

	public void onJointBreak(EventReport report)
	{
		LogEvent(Localizer.Format("#autoLOC_135304", report.sender, report.other), report);
	}

	public void onCrash(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135311", report.sender, report.other), report);
	}

	public void onCrashSplashdown(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135318", report.sender), report);
	}

	public void onCollision(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135325", report.sender, report.other), report);
	}

	public void onOverheat(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135332", report.sender, report.msg), report);
	}

	public void onOverPressure(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135339", report.sender, report.msg), report);
	}

	public void onOverG(EventReport report)
	{
		fetch.checkMissionEnd(report.origin, FlightEndModes.CATASTROPHIC_FAILURE);
		fetch.partsLost++;
		LogEvent(Localizer.Format("#autoLOC_135346", report.sender, report.msg), report);
	}

	public void onStageSeparation(EventReport report)
	{
		LogEvent(Localizer.Format("#autoLOC_135351", report.stage), report);
	}

	public void onCrewOnEva(GameEvents.FromToAction<Part, Part> fv)
	{
		if (fv.to.vessel.GetComponent<KerbalEVA>().part.protoModuleCrew[0].gender == ProtoCrewMember.Gender.Female)
		{
			if (fv.from != null)
			{
				LogEvent(Localizer.Format("#autoLOC_135358", fv.to.vessel.vesselName + " ^F", fv.from.vessel.GetDisplayName()), new EventReport(FlightEvents.CREW_ON_EVA, fv.to, fv.to.vessel.vesselName, fv.from.vessel.GetDisplayName()));
			}
			else
			{
				LogEvent(Localizer.Format("#autoLOC_135358", fv.to.vessel.vesselName + "^F"), new EventReport(FlightEvents.CREW_ON_EVA, fv.to, fv.to.vessel.vesselName));
			}
		}
		else if (fv.from != null)
		{
			LogEvent(Localizer.Format("#autoLOC_135358", fv.to.vessel.vesselName, fv.from.vessel.GetDisplayName()), new EventReport(FlightEvents.CREW_ON_EVA, fv.to, fv.to.vessel.vesselName, fv.from.vessel.GetDisplayName()));
		}
		else
		{
			LogEvent(Localizer.Format("#autoLOC_135358", fv.to.vessel.vesselName), new EventReport(FlightEvents.CREW_ON_EVA, fv.to, fv.to.vessel.vesselName));
		}
	}

	public void onCrewKilled(EventReport report)
	{
		fetch.kerbalsKilled++;
		LogEvent(Localizer.Format("#autoLOC_135371", report.sender), report);
	}

	public void onCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		LogEvent(Localizer.Format("#autoLOC_135376", fp.from.vessel.vesselName, fp.to.partInfo.title, fp.to.vessel.GetDisplayName()), new EventReport(FlightEvents.CREW_BOARD_VESSEL, fp.to, fp.from.vessel.vesselName, fp.to.vessel.GetDisplayName()));
	}

	public void onLaunch(EventReport report)
	{
		if (!fetch.liftOff)
		{
			fetch.liftOff = true;
			LogEvent(Localizer.Format("#autoLOC_135384"), report);
		}
	}

	public void onUndock(EventReport report)
	{
		LogEvent(Localizer.Format("#autoLOC_135389", report.sender, report.other), report);
	}

	public void onSplashDamage(EventReport report)
	{
		LogEvent(Localizer.Format("#autoLOC_135394", report.sender, report.other), report);
	}

	public void LogEvent(string message, EventReport report)
	{
		string text = "[" + missionTimestamp + "]: " + message;
		if (!(text == lastReport))
		{
			lastReport = text;
			eventLog.Add(text);
			if (GameSettings.VERBOSE_DEBUG_LOG)
			{
				Debug.Log("[F: " + Time.frameCount + "]: " + text, report.origin ? report.origin.gameObject : null);
			}
		}
	}

	public void LogEvent(string message)
	{
		string text = "[" + missionTimestamp + "]: " + message;
		if (!(text == lastReport))
		{
			lastReport = text;
			eventLog.Add(text);
			if (GameSettings.VERBOSE_DEBUG_LOG)
			{
				Debug.Log("[F: " + Time.frameCount + "]: " + text);
			}
		}
	}

	public bool checkMissionEnd(Part p, FlightEndModes endMode)
	{
		if (p == FlightGlobals.ActiveVessel.rootPart)
		{
			if (!IsInvoking("AttemptVesselSwitch"))
			{
				Invoke("AttemptVesselSwitch", attemptSwitchWait);
			}
			if (!missionEnd)
			{
				StartCoroutine(waitAndEndFlight(p.vessel, endMode));
				missionEnd = true;
			}
			return true;
		}
		return false;
	}

	public void AttemptVesselSwitch()
	{
		Vessel vessel = FlightGlobals.FindNearestControllableVessel(FlightGlobals.ActiveVessel);
		if (vessel != null)
		{
			missionEnd = false;
			FlightGlobals.ForceSetActiveVessel(vessel);
			FlightInputHandler.ResumeVesselCtrlState(vessel);
		}
	}

	public IEnumerator waitAndEndFlight(Vessel v, FlightEndModes endMode)
	{
		float t = Time.realtimeSinceStartup + endFlightWait;
		while (!(Time.realtimeSinceStartup >= t) && !Input.GetMouseButton(0) && !Input.GetKeyUp(KeyCode.Escape) && !Input.GetKeyUp(KeyCode.Space))
		{
			if (FlightGlobals.ActiveVessel != v)
			{
				yield break;
			}
			yield return null;
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE || HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			HighLogic.CurrentGame.Status = Game.GameStatus.FAILED_OR_ABORTED;
		}
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			FlightResultsDialog.showExitControls = true;
			FlightResultsDialog.allowClosingDialog = true;
			FlightResultsDialog.Display(Localizer.Format("#autoLOC_135586", endMessages[endMode]));
		}
	}
}
