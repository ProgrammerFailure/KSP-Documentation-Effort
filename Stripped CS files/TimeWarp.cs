using System;
using System.Collections;
using ns9;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
	public enum Modes
	{
		HIGH,
		const_1
	}

	public enum MaxRailsRateMode
	{
		VesselAltitude,
		PeAltitude
	}

	public float[] warpRates;

	public float[] altitudeLimits;

	public float[] physicsWarpRates;

	public int maxPhysicsRate_index;

	public int maxModeSwitchRate_index;

	public Modes Mode;

	public float textDuration = 3f;

	public float last_rate;

	public float curr_rate;

	public int current_rate_index;

	public ScreenMessage screenText;

	public static TimeWarp fetch;

	public ScreenMessage warpMessage;

	public float LerpStartTime;

	public bool controlsLocked;

	public bool autoWarpEngaged;

	public bool showPhyWarpWarning;

	public static bool saveShowPhysWarp = true;

	public double shipAltitude;

	public int soiRate;

	public int cRateIndex;

	public double minPeAllowed;

	public static double GThreshold = 0.1;

	public bool setAutoWarp;

	public double warpToUT;

	public double warpToMaxWarping;

	public double warpToMinWarping;

	public static string cacheAutoLOC_481056;

	public static string cacheAutoLOC_6001931;

	public static string cacheAutoLOC_6001932;

	public static string cacheAutoLOC_6001934;

	public static string cacheAutoLOC_6001936;

	public static Modes WarpMode
	{
		get
		{
			if (!fetch)
			{
				return Modes.HIGH;
			}
			return fetch.Mode;
		}
	}

	public float tgt_rate { get; set; }

	public static float CurrentRate
	{
		get
		{
			if (!fetch)
			{
				return 1f;
			}
			return fetch.curr_rate;
		}
	}

	public static int CurrentRateIndex
	{
		get
		{
			if (!fetch)
			{
				return 1;
			}
			return fetch.current_rate_index;
		}
	}

	public static bool CurrentRateIsTargetRate
	{
		get
		{
			if (!fetch)
			{
				return false;
			}
			return fetch.curr_rate.Equals(fetch.current_rate_index);
		}
	}

	public static float MaxRate
	{
		get
		{
			if (!fetch)
			{
				return 100000f;
			}
			return fetch.warpRates[fetch.warpRates.Length - 1];
		}
	}

	public static float MaxPhysicsRate
	{
		get
		{
			if (!fetch)
			{
				return 4f;
			}
			return fetch.warpRates[fetch.maxPhysicsRate_index];
		}
	}

	public static float deltaTime
	{
		get
		{
			if ((bool)fetch)
			{
				if (WarpMode != 0)
				{
					return Time.deltaTime;
				}
				return Time.deltaTime * fetch.curr_rate;
			}
			return Time.deltaTime;
		}
	}

	public static float fixedDeltaTime
	{
		get
		{
			if ((bool)fetch)
			{
				if (WarpMode != 0)
				{
					return Time.fixedDeltaTime;
				}
				return Time.fixedDeltaTime * fetch.curr_rate;
			}
			return Time.fixedDeltaTime;
		}
	}

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		fetch = this;
		screenText = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void PostScreenMessage(string text = null)
	{
		if (HighLogic.LoadedScene != GameScenes.MAINMENU)
		{
			if (text != null)
			{
				screenText.message = text;
			}
			ScreenMessages.PostScreenMessage(screenText);
		}
	}

	public void Start()
	{
		if (HighLogic.LoadedScene != GameScenes.FLIGHT || FlightDriver.flightStarted)
		{
			setRate(Mathf.Clamp(current_rate_index, 0, warpRates.Length), instantChange: false);
			curr_rate = tgt_rate;
			last_rate = tgt_rate;
			updateRate(curr_rate);
			showPhyWarpWarning = GameSettings.SHOW_PWARP_WARNING;
		}
	}

	public void btnSetHighRate(int rate)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.TIMEWARP))
		{
			if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpHigh)
			{
				PostScreenMessage(cacheAutoLOC_481056);
			}
			else if (setMode(Modes.HIGH))
			{
				setRate(rate, instantChange: false);
			}
		}
	}

	public void btnSetLowRate(int rate)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.TIMEWARP))
		{
			if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpLow)
			{
				PostScreenMessage(cacheAutoLOC_481056);
			}
			else if (setMode(Modes.const_1))
			{
				setRate(rate, instantChange: false);
			}
		}
	}

	public void Update()
	{
		if (setAutoWarp)
		{
			setAutoWarp = false;
			double warpDeltaTime = warpToUT - Planetarium.GetUniversalTime();
			getMaxWarpRateForTravel(warpDeltaTime, warpToMaxWarping, warpToMinWarping, out var rateIdx);
			setRate(Mathf.Min(current_rate_index + 1, rateIdx), instantChange: false);
			StartCoroutine(autoWarpTo(warpToUT, warpToMaxWarping, warpToMinWarping, rateIdx));
		}
		else
		{
			if (InputLockManager.IsLocked(ControlTypes.TIMEWARP))
			{
				return;
			}
			if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && current_rate_index <= maxModeSwitchRate_index && curr_rate == tgt_rate)
			{
				Vessel activeVessel = FlightGlobals.ActiveVessel;
				CelestialBody mainBody = activeVessel.mainBody;
				if (Mode == Modes.HIGH)
				{
					if (!FlightGlobals.ActiveVessel.LandedOrSplashed)
					{
						double num = FlightGlobals.getAltitudeAtPos(activeVessel.transform.position, mainBody);
						if (double.IsInfinity(num))
						{
							Debug.Break();
						}
						if (FlightGlobals.getStaticPressure(num, mainBody) > 0.0)
						{
							setMode(Modes.const_1);
						}
					}
					else if (FlightGlobals.ship_srfVelocity.sqrMagnitude > GThreshold)
					{
						setMode(Modes.const_1);
					}
				}
				if (Mode == Modes.const_1)
				{
					double num2 = FlightGlobals.getAltitudeAtPos(activeVessel.transform.position, activeVessel.mainBody);
					double staticPressure = FlightGlobals.getStaticPressure(num2);
					if (double.IsInfinity(num2))
					{
						Debug.Break();
					}
					if ((activeVessel.LandedOrSplashed && FlightGlobals.ship_srfVelocity.sqrMagnitude < GThreshold) || staticPressure == 0.0)
					{
						setMode(Modes.HIGH);
					}
				}
			}
			if (GameSettings.TIME_WARP_INCREASE.GetKeyDown())
			{
				if (GameSettings.MODIFIER_KEY.GetKey() && HighLogic.LoadedSceneIsFlight)
				{
					if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpLow)
					{
						PostScreenMessage(cacheAutoLOC_481056);
						return;
					}
					if (setMode(Modes.const_1))
					{
						if (showPhyWarpWarning && current_rate_index + 1 == 2)
						{
							ShowPWarpWarning();
						}
						else
						{
							setRate(Mathf.Min(current_rate_index + 1, physicsWarpRates.Length - 1), instantChange: false);
						}
					}
				}
				else
				{
					if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpHigh)
					{
						PostScreenMessage(cacheAutoLOC_481056);
						return;
					}
					if (Mode == Modes.HIGH)
					{
						setRate(Mathf.Min(current_rate_index + 1, warpRates.Length - 1), instantChange: false);
					}
					if (Mode == Modes.const_1)
					{
						if (showPhyWarpWarning && current_rate_index + 1 == 2)
						{
							ShowPWarpWarning();
						}
						else
						{
							setRate(Mathf.Min(current_rate_index + 1, physicsWarpRates.Length - 1), instantChange: false);
						}
					}
				}
			}
			if (GameSettings.TIME_WARP_DECREASE.GetKeyDown())
			{
				if (GameSettings.MODIFIER_KEY.GetKey() && HighLogic.LoadedSceneIsFlight)
				{
					if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpLow)
					{
						PostScreenMessage(cacheAutoLOC_481056);
						return;
					}
					if (setMode(Modes.const_1))
					{
						setRate(Mathf.Max(current_rate_index - 1, 0), instantChange: false);
					}
				}
				else
				{
					if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpHigh)
					{
						PostScreenMessage(cacheAutoLOC_481056);
						return;
					}
					setRate(Mathf.Max(current_rate_index - 1, 0), instantChange: false);
				}
			}
			if (GameSettings.TIME_WARP_STOP.GetKeyDown() && current_rate_index != 0)
			{
				CancelAutoWarp();
				setRate(0, instantChange: false);
			}
		}
	}

	public void ShowPWarpWarning()
	{
		FlightDriver.SetPause(pauseState: true);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("PhysWarpWarning", Localizer.Format("#autoLOC_6003092"), Localizer.Format("#autoLOC_455453"), null, new DialogGUIToggle(() => !showPhyWarpWarning, Localizer.Format("#autoLOC_360842"), delegate
		{
			showPhyWarpWarning = !showPhyWarpWarning;
		}, 120f), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), OnPWarpWarningDismiss)), persistAcrossScenes: false, null).OnDismiss = OnPWarpWarningDismiss;
	}

	public void OnPWarpWarningDismiss()
	{
		if (saveShowPhysWarp)
		{
			GameSettings.SHOW_PWARP_WARNING = showPhyWarpWarning;
			GameSettings.SaveSettings();
		}
		showPhyWarpWarning = false;
		FlightDriver.SetPause(pauseState: false);
	}

	public void FixedUpdate()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (HighLogic.LoadedSceneIsFlight && Mode == Modes.HIGH && curr_rate > warpRates[maxPhysicsRate_index] && !activeVessel.LandedOrSplashed)
		{
			setRate(current_rate_index, instantChange: false, instantIfLower: true, forceSwitch: false);
		}
		if (Planetarium.TimeScale != (double)tgt_rate)
		{
			curr_rate = Mathf.Lerp(last_rate, tgt_rate, Time.time - LerpStartTime);
			updateRate(curr_rate);
		}
	}

	public void updateRate(float r, bool postScreenMessage = true)
	{
		Planetarium.TimeScale = r;
		Time.timeScale = ((Mode == Modes.const_1) ? r : 1f);
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		if (postScreenMessage)
		{
			PostScreenMessage(Localizer.Format("#autoLOC_7003002", (curr_rate >= 1f) ? r.ToString("0") : r.ToString("0.0")));
		}
		if (r <= MaxPhysicsRate && controlsLocked)
		{
			InputLockManager.RemoveControlLock("TimeWarpLock");
			controlsLocked = false;
		}
	}

	public double GetAltitudeLimit(int i, CelestialBody body)
	{
		return body.timeWarpAltitudeLimits[i];
	}

	public static void SetRate(int rate_index, bool instant, bool postScreenMessage = true)
	{
		if ((bool)fetch)
		{
			rate_index = Mathf.Clamp(rate_index, 0, fetch.warpRates.Length);
			fetch.setRate(rate_index, instant, instantIfLower: false, forceSwitch: true, postScreenMessage);
		}
	}

	public bool setRate(int rateIdx, bool instantChange, bool instantIfLower = false, bool forceSwitch = true, bool postScreenMessage = true)
	{
		int num = rateIdx;
		if (HighLogic.LoadedSceneIsFlight && Mode == Modes.HIGH && rateIdx > maxPhysicsRate_index)
		{
			num = getMaxOnRailsRateIdx(rateIdx, lookAhead: true, out var reason);
			if (num < rateIdx && instantIfLower && (reason == ClearToSaveStatus.NOT_IN_ATMOSPHERE || reason == ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH))
			{
				instantChange = true;
			}
			if (postScreenMessage)
			{
				switch (reason)
				{
				case ClearToSaveStatus.NOT_IN_ATMOSPHERE:
					PostScreenMessage(Localizer.Format("#autoLOC_6001925", warpRates[maxPhysicsRate_index].ToString("0")));
					break;
				case ClearToSaveStatus.NOT_UNDER_ACCELERATION:
					PostScreenMessage(Localizer.Format("#autoLOC_6001926", warpRates[maxPhysicsRate_index].ToString("0")));
					break;
				case ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE:
					PostScreenMessage(Localizer.Format("#autoLOC_6001927", warpRates[maxPhysicsRate_index].ToString("0")));
					break;
				case ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH:
					if (GameSettings.ORBIT_WARP_MAXRATE_MODE == MaxRailsRateMode.VesselAltitude)
					{
						PostScreenMessage(Localizer.Format("#autoLOC_6001928", warpRates[num].ToString("0"), GetAltitudeLimit(Mathf.Min(num + 1, altitudeLimits.Length), FlightGlobals.currentMainBody).ToString("0")));
					}
					else
					{
						PostScreenMessage(Localizer.Format("#autoLOC_8003368", warpRates[num].ToString("0"), GetAltitudeLimit(Mathf.Min(num + 1, altitudeLimits.Length), FlightGlobals.currentMainBody).ToString("0"), (minPeAllowed + GameSettings.ORBIT_WARP_PEMODE_SURFACE_MARGIN).ToString("0")));
					}
					break;
				case ClearToSaveStatus.NOT_WHILE_ON_A_LADDER:
					PostScreenMessage(Localizer.Format("#autoLOC_6001929", warpRates[maxPhysicsRate_index].ToString("0")));
					break;
				}
			}
			if (num > maxPhysicsRate_index)
			{
				InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS_ALLOW_UIMODE, "TimeWarpLock");
				controlsLocked = true;
			}
		}
		Modes mode = Mode;
		num = ((mode == Modes.HIGH || mode != Modes.const_1) ? Mathf.Clamp(num, 0, warpRates.Length - 1) : Mathf.Clamp(num, 0, physicsWarpRates.Length - 1));
		if (current_rate_index != num || forceSwitch)
		{
			current_rate_index = num;
			assumeWarpRate((Mode == Modes.HIGH) ? warpRates[num] : physicsWarpRates[num], instantChange, postScreenMessage);
		}
		return num == rateIdx;
	}

	public int getMaxOnRailsRateIdx(int tgtRateIdx, bool lookAhead, out ClearToSaveStatus reason)
	{
		reason = ClearToSaveStatus.CLEAR;
		if (tgtRateIdx > 0 && HighLogic.LoadedSceneIsFlight && (bool)FlightGlobals.ActiveVessel)
		{
			for (int i = 0; i < FlightGlobals.VesselsLoaded.Count; i++)
			{
				Vessel vessel = FlightGlobals.VesselsLoaded[i];
				if (vessel.isEVA && vessel.GetComponent<KerbalEVA>().OnALadder)
				{
					reason = ClearToSaveStatus.NOT_WHILE_ON_A_LADDER;
					return 0;
				}
			}
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			CelestialBody mainBody = activeVessel.mainBody;
			if (!activeVessel.LandedOrSplashed)
			{
				double altitude = FlightGlobals.getAltitudeAtPos(activeVessel.transform.position, mainBody);
				double staticPressure = FlightGlobals.getStaticPressure(altitude, mainBody);
				_ = activeVessel.orbit.PeA;
				double val = 0.0;
				double val2 = 0.0;
				if (activeVessel.mainBody.hasSolidSurface)
				{
					val = activeVessel.mainBody.pqsController.radiusMax - activeVessel.mainBody.pqsController.radius;
					val2 = activeVessel.mainBody.pqsController.meshVertMax;
				}
				double val3 = (activeVessel.mainBody.atmosphere ? activeVessel.mainBody.atmosphereDepth : 0.0);
				minPeAllowed = Math.Max(val, val3);
				minPeAllowed = Math.Max(minPeAllowed, val2);
				if (staticPressure > 0.0)
				{
					reason = ClearToSaveStatus.NOT_IN_ATMOSPHERE;
					return 0;
				}
				if (activeVessel.geeForce > GThreshold)
				{
					reason = ClearToSaveStatus.NOT_UNDER_ACCELERATION;
					return 0;
				}
				if (GameSettings.ORBIT_WARP_MAXRATE_MODE == MaxRailsRateMode.VesselAltitude && activeVessel.altitude < GetAltitudeLimit(tgtRateIdx, mainBody))
				{
					reason = ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH;
					tgtRateIdx = GetMaxRateForAltitude(activeVessel.altitude, mainBody);
				}
				if (lookAhead && tgtRateIdx > maxPhysicsRate_index)
				{
					if (tgtRateIdx > warpRates.Length - 1)
					{
						tgtRateIdx = warpRates.Length - 1;
					}
					double num = Time.fixedDeltaTime;
					Orbit orbit = activeVessel.orbit;
					double universalTime = Planetarium.GetUniversalTime();
					double radius = mainBody.Radius;
					double num2 = num * (double)warpRates[tgtRateIdx];
					double num3 = orbit.timeToPe;
					if (num3 < 0.0)
					{
						num3 = double.MaxValue;
					}
					double peA = orbit.PeA;
					if (num2 > num3)
					{
						num2 = num3;
						altitude = peA;
					}
					else
					{
						altitude = orbit.getRelativePositionAtUT(universalTime + num2).magnitude - radius;
					}
					staticPressure = mainBody.GetPressure(altitude);
					while (tgtRateIdx > maxPhysicsRate_index && (staticPressure > 0.0 || !(altitude >= 0.0)))
					{
						tgtRateIdx--;
						double num4 = num * (double)warpRates[tgtRateIdx];
						if (num4 < num3)
						{
							num2 = num4;
							altitude = orbit.getRelativePositionAtUT(universalTime + num2).magnitude - radius;
						}
						else
						{
							num2 = num3;
							altitude = peA;
						}
						staticPressure = mainBody.GetPressure(altitude);
						reason = ClearToSaveStatus.NOT_IN_ATMOSPHERE;
					}
					if (GameSettings.ORBIT_WARP_MAXRATE_MODE != MaxRailsRateMode.PeAltitude || !(peA > minPeAllowed + GameSettings.ORBIT_WARP_PEMODE_SURFACE_MARGIN))
					{
						while (tgtRateIdx > maxPhysicsRate_index && !(altitude >= GetAltitudeLimit(tgtRateIdx, mainBody)))
						{
							tgtRateIdx--;
							double num5 = num * (double)warpRates[tgtRateIdx];
							if (num5 < num3)
							{
								num2 = num5;
								altitude = orbit.getRelativePositionAtUT(universalTime + num2).magnitude - radius;
							}
							else
							{
								num2 = num3;
								altitude = peA;
							}
							reason = ClearToSaveStatus.NOT_WHILE_ABOUT_TO_CRASH;
						}
					}
				}
			}
			else if (activeVessel.srf_velocity.sqrMagnitude > 0.09000000357627869)
			{
				reason = ClearToSaveStatus.NOT_WHILE_MOVING_OVER_SURFACE;
				return 0;
			}
			if (GameSettings.ORBIT_WARP_DOWN_AT_SOI && activeVessel.PatchedConicsAttached && activeVessel.orbit.patchEndTransition != Orbit.PatchTransitionType.FINAL)
			{
				int num6 = ClampRateToOrbitTransitions(tgtRateIdx, activeVessel.orbit, 3, 50);
				if (num6 != tgtRateIdx)
				{
					reason = ClearToSaveStatus.ORBIT_EVENT_IMMINENT;
					Debug.Log("Orbit event imminent. Dropping TimeWarp to max limit: " + warpRates[3] + "x");
					return num6;
				}
			}
		}
		return tgtRateIdx;
	}

	public int GetMaxRateForAltitude(double altitude, CelestialBody cb)
	{
		int num = 0;
		int num2 = altitudeLimits.Length;
		while (true)
		{
			if (num < num2)
			{
				if (GetAltitudeLimit(num, cb) >= altitude)
				{
					break;
				}
				num++;
				continue;
			}
			return warpRates.Length - 1;
		}
		return Mathf.Max(0, num - 1);
	}

	public int ClampRateToOrbitTransitions(int rate, Orbit obt, int maxAllowedSOITransitionRate, int secondsBeforeSOItransition)
	{
		if (obt.patchEndTransition != Orbit.PatchTransitionType.FINAL && rate > maxAllowedSOITransitionRate)
		{
			double rateChangeTravel = getRateChangeTravel(Planetarium.TimeScale, warpRates[maxAllowedSOITransitionRate], getWarpAccel(Planetarium.TimeScale, warpRates[maxAllowedSOITransitionRate], 1.0));
			if (obt.EndUT - Planetarium.GetUniversalTime() < rateChangeTravel + (double)secondsBeforeSOItransition)
			{
				return maxAllowedSOITransitionRate;
			}
		}
		return rate;
	}

	public void assumeWarpRate(float rate, bool instant, bool postScreenMessage = true)
	{
		last_rate = tgt_rate;
		tgt_rate = rate;
		LerpStartTime = Time.time;
		if (instant)
		{
			curr_rate = rate;
			updateRate(curr_rate, postScreenMessage);
		}
		FlightLogger.IgnoreGeeForces(1f);
		GameEvents.onTimeWarpRateChanged.Fire();
		if (postScreenMessage)
		{
			PostScreenMessage();
		}
	}

	public bool setMode(Modes mode)
	{
		if (Mode != mode)
		{
			if (current_rate_index > maxModeSwitchRate_index)
			{
				PostScreenMessage(Localizer.Format("#autoLOC_6001930", ((Mode == Modes.HIGH) ? warpRates[maxModeSwitchRate_index] : physicsWarpRates[maxModeSwitchRate_index]).ToString("0.0")));
				return false;
			}
			Mode = mode;
		}
		GameEvents.onTimeWarpRateChanged.Fire();
		return true;
	}

	public void OnDestroy()
	{
		InputLockManager.RemoveControlLock("TimeWarpLock");
		InputLockManager.RemoveControlLock("TimeWarpTo");
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void WarpTo(double double_0, double maxTimeWarping = 8.0, double minTimeWarping = 2.5)
	{
		if (autoWarpEngaged)
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6001931, 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		setAutoWarp = true;
		warpToUT = double_0;
		warpToMaxWarping = maxTimeWarping;
		warpToMinWarping = minTimeWarping;
	}

	public bool CancelAutoWarp(int rateIdx = -1, bool delay_unlock = false)
	{
		if (autoWarpEngaged)
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6001932, warpMessage);
			if (delay_unlock)
			{
				StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
				{
					InputLockManager.RemoveControlLock("TimeWarpTo");
				}));
			}
			else
			{
				InputLockManager.RemoveControlLock("TimeWarpTo");
			}
			autoWarpEngaged = false;
			if (rateIdx >= 0)
			{
				setRate(rateIdx, instantChange: false);
			}
			return true;
		}
		return false;
	}

	public IEnumerator autoWarpTo(double tgtUT, double maxTimeWarping, double minTimeWarping, int rateIdx)
	{
		autoWarpEngaged = true;
		warpMessage = new ScreenMessage("", 5f, ScreenMessageStyle.LOWER_CENTER);
		InputLockManager.SetControlLock(ControlTypes.WARPTO_LOCK, "TimeWarpTo");
		Debug.Log("Warping to UT:" + tgtUT.ToString("0.0") + ". Max Rate Allowed: " + tgt_rate.ToString("0.0") + "x.");
		double num;
		do
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001933", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - tgtUT, 3, explicitPositive: true), GameSettings.TIME_WARP_STOP.name), warpMessage);
			yield return new WaitForFixedUpdate();
			if (!autoWarpEngaged)
			{
				yield break;
			}
			if (current_rate_index < rateIdx)
			{
				ClearToSaveStatus reason;
				int maxOnRailsRateIdx = getMaxOnRailsRateIdx(rateIdx, lookAhead: true, out reason);
				if (maxOnRailsRateIdx <= 0)
				{
					ScreenMessages.PostScreenMessage(cacheAutoLOC_6001934, warpMessage);
				}
				if (maxOnRailsRateIdx > current_rate_index)
				{
					setRate(maxOnRailsRateIdx, instantChange: false);
				}
			}
			double warpAccel = getWarpAccel(Planetarium.TimeScale, 1.0, 1.0);
			double rateChangeTravel = getRateChangeTravel(Planetarium.TimeScale, 1.0, warpAccel);
			num = tgtUT - rateChangeTravel;
			if (GameSettings.TIME_WARP_STOP.GetKeyDown())
			{
				CancelAutoWarp(0, delay_unlock: true);
				yield break;
			}
			if (GameSettings.TIME_WARP_DECREASE.GetKeyDown() && current_rate_index > 0 && setRate(current_rate_index - 1, instantChange: false))
			{
				CancelAutoWarp();
				yield break;
			}
			if (GameSettings.TIME_WARP_INCREASE.GetKeyDown() && (float)current_rate_index < warpRates[warpRates.Length - 1] && setRate(current_rate_index + 1, instantChange: false))
			{
				CancelAutoWarp();
				yield break;
			}
		}
		while (Planetarium.GetUniversalTime() < num && current_rate_index > 0);
		if (current_rate_index > 0)
		{
			double num2 = Planetarium.GetUniversalTime() - num;
			Debug.Log("Warping down...  UT: " + Planetarium.GetUniversalTime().ToString("0.0") + " DecelStart at " + num.ToString("0.0") + " Overshoot is " + num2.ToString("0.0") + "s.");
			Planetarium.SetUniversalTime(num - (double)fixedDeltaTime * 0.5);
			setRate(0, instantChange: false);
			last_rate = (float)Planetarium.TimeScale;
			while (curr_rate != tgt_rate)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001935", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - tgtUT, 3, explicitPositive: true)), warpMessage);
				yield return new WaitForFixedUpdate();
			}
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6001936, warpMessage);
		}
		double num3 = Planetarium.GetUniversalTime() - tgtUT;
		Debug.Log("Real-time resumed. Current UT is: " + Planetarium.GetUniversalTime().ToString("0.0") + ". TgtUT Error is " + num3.ToString("0.0###") + "s. " + ((num3 > 0.0) ? "(Overshot)" : "(Undershot)"));
		yield return null;
		InputLockManager.RemoveControlLock("TimeWarpTo");
		autoWarpEngaged = false;
		warpMessage = null;
	}

	public double getMaxWarpRateForTravel(double warpDeltaTime, double minTimeInWarp, double maxTimeInWarp, out int rateIdx)
	{
		double num = warpDeltaTime;
		double num2 = 1.0;
		rateIdx = 0;
		if (warpDeltaTime < minTimeInWarp)
		{
			return 1.0;
		}
		while (!(num <= maxTimeInWarp) && rateIdx < warpRates.Length - 1)
		{
			num2 = warpRates[++rateIdx];
			num = warpDeltaTime / num2;
		}
		if (num < minTimeInWarp)
		{
			return warpRates[--rateIdx];
		}
		return num2;
	}

	public double getWarpAccel(double v0, double v, double t)
	{
		if (t == 0.0)
		{
			Debug.LogError("GetWarpAccel: Time Scale cannot be zero!");
		}
		return (v - v0) / t;
	}

	public double getRateChangeTravel(double v0, double v, double a)
	{
		if (a == 0.0)
		{
			return 0.0;
		}
		return (v * v - v0 * v0) / (2.0 * a);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_481056 = Localizer.Format("#autoLOC_481056");
		cacheAutoLOC_6001931 = Localizer.Format("#autoLOC_6001931");
		cacheAutoLOC_6001932 = Localizer.Format("#autoLOC_6001932");
		cacheAutoLOC_6001934 = Localizer.Format("#autoLOC_6001934");
		cacheAutoLOC_6001936 = Localizer.Format("#autoLOC_6001936");
	}
}
