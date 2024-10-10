using System;
using ns11;
using ns9;
using UnityEngine;

public class FlightInputHandler : MonoBehaviour
{
	public static FlightCtrlState state;

	[Obsolete("Use vessel.OnFlyByWire instead.")]
	public static FlightInputCallback OnFlyByWire = delegate
	{
	};

	public static FlightInputCallback OnRawAxisInput = delegate
	{
	};

	public float throttleResponsiveness = 2f;

	public float rcsDeadZone = 0.001f;

	public float throttle;

	public float axisThrottle;

	public float lastAxisThrottle;

	public float precisionPitch;

	public float precisionRoll;

	public float precisionYaw;

	public float precisionX;

	public float precisionY;

	public float precisionZ;

	public float precisionWheelSteer;

	public float precisionWheelThrottle;

	public float[] precision_custom_axis;

	public static FlightInputHandler fetch;

	public static int currentTarget = 0;

	public uint controlLockMask;

	public bool stageLock;

	public bool rcslock = true;

	public bool precisionMode;

	public bool hasFocus = true;

	public static bool SPACENAV_USE_AS_FLIGHT_CONTROL = false;

	public bool hasSpaceNavDevice;

	public bool linRotSwitchHold;

	public bool throttleFocus;

	public bool refocusThrottle;

	public static bool RCSLock
	{
		get
		{
			if (!fetch)
			{
				return false;
			}
			return fetch.rcslock;
		}
	}

	public void Awake()
	{
		fetch = this;
		state = new FlightCtrlState();
	}

	public void Start()
	{
		rcslock = true;
		hasSpaceNavDevice = SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice);
		GameEvents.OnVesselOverrideGroupChanged.Add(OnVesselOverrideGroupChanged);
	}

	public void OnVesselOverrideGroupChanged(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			ResumeVesselCtrlState(v);
		}
	}

	public static void SetNeutralControls()
	{
		state = new FlightCtrlState();
		FlightGlobals.ActiveVessel.ctrlState.NeutralizeAll();
		if ((bool)fetch)
		{
			fetch.throttle = -1f;
			fetch.rcslock = true;
		}
	}

	public static void ResumeVesselCtrlState(Vessel v)
	{
		v.GetControlState(state);
		FlightGlobals.ActiveVessel.ActionGroups.CopyFrom(v.ActionGroups);
		if ((bool)fetch)
		{
			fetch.throttle = state.mainThrottle * 2f - 1f;
			fetch.refocusThrottle = true;
		}
	}

	public static void SetLaunchCtrlState()
	{
		SetNeutralControls();
		state.mainThrottle = GameSettings.PRELAUNCH_DEFAULT_THROTTLE;
		if ((bool)fetch)
		{
			fetch.refocusThrottle = true;
		}
	}

	public void OnApplicationFocus(bool focus)
	{
		hasFocus = focus;
		if (focus)
		{
			refocusThrottle = true;
		}
	}

	public void Update()
	{
		if (!FlightGlobals.ready || FlightDriver.Pause)
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!activeVessel.isEVA)
		{
			if (InputLockManager.IsUnlocked(ControlTypes.MISC))
			{
				if (GameSettings.MODIFIER_KEY.GetKey() && Input.GetKeyDown(KeyCode.L))
				{
					stageLock = !stageLock;
					if (stageLock)
					{
						InputLockManager.SetControlLock(ControlTypes.STAGING, "manualStageLock");
					}
					else
					{
						InputLockManager.RemoveControlLock("manualStageLock");
					}
					MonoBehaviour.print("staging lock/unlock");
				}
				if (GameSettings.PRECISION_CTRL.GetKeyDown())
				{
					precisionMode = !precisionMode;
					GameEvents.Input.OnPrecisionModeToggle.Fire(precisionMode);
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.flag_27) && GameSettings.RCS_TOGGLE.GetKeyDown())
			{
				activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.flag_5);
				if (!activeVessel.ActionControlBlocked(KSPActionGroup.flag_5))
				{
					rcslock = !rcslock;
					MonoBehaviour.print("RCS lock/unlock");
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.GROUP_GEARS))
			{
				if (GameSettings.LANDING_GEAR.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Gear);
					state.gearDown = true;
					state.gearUp = true;
				}
				else
				{
					state.gearDown = false;
					state.gearUp = false;
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.GROUP_LIGHTS))
			{
				if (GameSettings.HEADLIGHT_TOGGLE.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Light);
					state.headlight = true;
				}
				else
				{
					state.headlight = false;
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.THROTTLE_CUT_MAX) && GameSettings.THROTTLE_CUTOFF.GetKeyDown() && !GameSettings.MODIFIER_KEY.GetKey())
			{
				state.mainThrottle = 0f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.THROTTLE_CUT_MAX) && GameSettings.THROTTLE_FULL.GetKeyDown() && !GameSettings.MODIFIER_KEY.GetKey())
			{
				state.mainThrottle = 1f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.STAGING) && GameSettings.LAUNCH_STAGES.GetKeyDown())
			{
				if (!activeVessel.ActionControlBlocked(KSPActionGroup.Stage))
				{
					StageManager.ActivateNextStage();
				}
				activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Stage);
			}
			if (GameSettings.Docking_toggleRotLin.GetDoubleTapDown())
			{
				linRotSwitchHold = !linRotSwitchHold;
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003261", linRotSwitchHold ? Localizer.Format("#autoLOC_7003262") : Localizer.Format("#autoLOC_7003263")), 2f, ScreenMessageStyle.LOWER_CENTER);
			}
			else if (linRotSwitchHold)
			{
				if (GameSettings.Docking_toggleRotLin.GetKeyDown())
				{
					InputBinding.linRotState = true;
				}
				if (GameSettings.Docking_toggleRotLin.GetKeyUp())
				{
					InputBinding.linRotState = false;
				}
			}
			else if (GameSettings.Docking_toggleRotLin.GetKeyDown())
			{
				InputBinding.linRotState = !InputBinding.linRotState;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.flag_8))
			{
				if (GameSettings.SAS_HOLD.GetKeyDown())
				{
					if (!activeVessel.ActionControlBlocked(KSPActionGroup.flag_6))
					{
						state.killRot = !state.killRot;
					}
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.flag_6);
				}
				if (GameSettings.SAS_HOLD.GetKeyUp())
				{
					if (!activeVessel.ActionControlBlocked(KSPActionGroup.flag_6))
					{
						state.killRot = !state.killRot;
					}
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.flag_6);
				}
				if (GameSettings.SAS_TOGGLE.GetKeyDown())
				{
					if (!activeVessel.ActionControlBlocked(KSPActionGroup.flag_6))
					{
						state.killRot = !state.killRot;
					}
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.flag_6);
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.CUSTOM_ACTION_GROUPS))
			{
				if (ActionGroupsApp.Instance != null)
				{
					if (GameSettings.AGROUP_SELECT_NEXT.GetKeyDown())
					{
						ActionGroupsApp.Instance.SelectNext();
					}
					if (GameSettings.AGROUP_SELECT_PREV.GetKeyDown())
					{
						ActionGroupsApp.Instance.SelectPrev();
					}
				}
				if (GameSettings.CustomActionGroup1.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom01);
				}
				if (GameSettings.CustomActionGroup2.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom02);
				}
				if (GameSettings.CustomActionGroup3.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom03);
				}
				if (GameSettings.CustomActionGroup4.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom04);
				}
				if (GameSettings.CustomActionGroup5.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom05);
				}
				if (GameSettings.CustomActionGroup6.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom06);
				}
				if (GameSettings.CustomActionGroup7.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom07);
				}
				if (GameSettings.CustomActionGroup8.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom08);
				}
				if (GameSettings.CustomActionGroup9.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom09);
				}
				if (GameSettings.CustomActionGroup10.GetKeyDown())
				{
					activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Custom10);
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.GROUP_ABORT) && GameSettings.AbortActionGroup.GetKeyDown())
			{
				activeVessel.ActionGroups.ToggleGroup(KSPActionGroup.Abort);
			}
			if (InputLockManager.IsUnlocked(ControlTypes.GROUP_BRAKES))
			{
				if (GameSettings.BRAKES.GetKeyDown())
				{
					activeVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, active: true);
				}
				if (GameSettings.BRAKES.GetKeyUp())
				{
					activeVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, active: false);
				}
			}
		}
		else if (InputLockManager.IsUnlocked(ControlTypes.UI_DIALOGS) && GameSettings.SAS_TOGGLE.GetKeyDown())
		{
			GameSettings.EVA_ROTATE_ON_MOVE = !GameSettings.EVA_ROTATE_ON_MOVE;
		}
		if (hasSpaceNavDevice && GameSettings.TOGGLE_SPACENAV_FLIGHT_CONTROL.GetKeyUp())
		{
			if (SPACENAV_USE_AS_FLIGHT_CONTROL)
			{
				SPACENAV_USE_AS_FLIGHT_CONTROL = false;
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_175414"), 3f, ScreenMessageStyle.LOWER_CENTER);
			}
			else
			{
				SPACENAV_USE_AS_FLIGHT_CONTROL = true;
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_175419"), 3f, ScreenMessageStyle.LOWER_CENTER);
			}
		}
	}

	public void ProcessAxis(AxisBinding axisBinding, KeyBinding plusKeyBinding, KeyBinding minusKeyBinding, ref float axisValue, ref float precisionAxis)
	{
		axisValue = axisBinding.GetAxis();
		if (precisionMode)
		{
			precisionAxis = (minusKeyBinding.GetKey() ? Mathf.Max(-1f, precisionAxis - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (plusKeyBinding.GetKey() ? Mathf.Min(1f, precisionAxis + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : axisValue));
			axisValue = precisionAxis;
		}
		else
		{
			axisValue = (minusKeyBinding.GetKey() ? (-1f) : (plusKeyBinding.GetKey() ? 1f : axisValue));
			precisionAxis = axisValue;
		}
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready)
		{
			return;
		}
		if (!FlightDriver.Pause && !FlightGlobals.ActiveVessel.isEVA)
		{
			throttle = state.mainThrottle * 2f - 1f;
			if (InputLockManager.IsUnlocked(ControlTypes.THROTTLE))
			{
				throttle = (GameSettings.THROTTLE_UP.GetKey() ? Mathf.Min(1f, throttle + 1f * Time.deltaTime) : (GameSettings.THROTTLE_DOWN.GetKey() ? Mathf.Max(-1f, throttle - 1f * Time.deltaTime) : throttle));
				throttle = Mathf.Clamp(throttle + GameSettings.AXIS_THROTTLE_INC.GetAxis() * Time.deltaTime, -1f, 1f);
				axisThrottle = GameSettings.AXIS_THROTTLE.GetAxis();
				if (refocusThrottle)
				{
					lastAxisThrottle = axisThrottle;
					refocusThrottle = false;
					throttleFocus = false;
				}
				if (throttleFocus ? (axisThrottle != lastAxisThrottle) : (Mathf.Abs(axisThrottle - lastAxisThrottle) > 0.25f))
				{
					throttle = axisThrottle;
					lastAxisThrottle = throttle;
					throttleFocus = true;
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.ROLL))
			{
				state.roll = GameSettings.AXIS_ROLL.GetAxis();
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					if (GameSettings.ROLL_LEFT.GetKey())
					{
						state.rollTrim = Mathf.Clamp(state.rollTrim - 0.1f * Time.deltaTime, -1f, 1f);
					}
					if (GameSettings.ROLL_RIGHT.GetKey())
					{
						state.rollTrim = Mathf.Clamp(state.rollTrim + 0.1f * Time.deltaTime, -1f, 1f);
					}
				}
				else
				{
					if (SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
					{
						state.roll -= SpaceNavigator.Rotation.Roll() * GameSettings.SPACENAV_FLIGHT_SENS_ROT;
					}
					if (precisionMode)
					{
						precisionRoll = (GameSettings.ROLL_LEFT.GetKey() ? Mathf.Max(-1f, precisionRoll - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (GameSettings.ROLL_RIGHT.GetKey() ? Mathf.Min(1f, precisionRoll + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : state.roll));
						state.roll = precisionRoll;
					}
					else
					{
						state.roll = (GameSettings.ROLL_LEFT.GetKey() ? (-1f) : (GameSettings.ROLL_RIGHT.GetKey() ? 1f : state.roll));
						precisionRoll = state.roll;
					}
				}
			}
			else
			{
				state.roll = 0f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.PITCH))
			{
				state.pitch = GameSettings.AXIS_PITCH.GetAxis();
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					if (GameSettings.PITCH_DOWN.GetKey())
					{
						state.pitchTrim = Mathf.Clamp(state.pitchTrim - 0.1f * Time.deltaTime, -1f, 1f);
					}
					if (GameSettings.PITCH_UP.GetKey())
					{
						state.pitchTrim = Mathf.Clamp(state.pitchTrim + 0.1f * Time.deltaTime, -1f, 1f);
					}
				}
				else
				{
					if (SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
					{
						state.pitch -= SpaceNavigator.Rotation.Pitch() * GameSettings.SPACENAV_FLIGHT_SENS_ROT;
					}
					if (precisionMode)
					{
						precisionPitch = (GameSettings.PITCH_DOWN.GetKey() ? Mathf.Max(-1f, precisionPitch - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (GameSettings.PITCH_UP.GetKey() ? Mathf.Min(1f, precisionPitch + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : state.pitch));
						state.pitch = precisionPitch;
					}
					else
					{
						state.pitch = (GameSettings.PITCH_DOWN.GetKey() ? (-1f) : (GameSettings.PITCH_UP.GetKey() ? 1f : state.pitch));
						precisionPitch = state.pitch;
					}
				}
			}
			else
			{
				state.pitch = 0f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.flag_5))
			{
				state.yaw = GameSettings.AXIS_YAW.GetAxis();
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					if (GameSettings.YAW_LEFT.GetKey())
					{
						state.yawTrim = Mathf.Clamp(state.yawTrim - 0.1f * Time.deltaTime, -1f, 1f);
					}
					if (GameSettings.YAW_RIGHT.GetKey())
					{
						state.yawTrim = Mathf.Clamp(state.yawTrim + 0.1f * Time.deltaTime, -1f, 1f);
					}
				}
				else
				{
					if (SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
					{
						state.yaw += SpaceNavigator.Rotation.Yaw() * GameSettings.SPACENAV_FLIGHT_SENS_ROT;
					}
					if (precisionMode)
					{
						precisionYaw = (GameSettings.YAW_LEFT.GetKey() ? Mathf.Max(-1f, precisionYaw - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (GameSettings.YAW_RIGHT.GetKey() ? Mathf.Min(1f, precisionYaw + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : state.yaw));
						state.yaw = precisionYaw;
					}
					else
					{
						state.yaw = (GameSettings.YAW_LEFT.GetKey() ? (-1f) : (GameSettings.YAW_RIGHT.GetKey() ? 1f : state.yaw));
						precisionYaw = state.yaw;
					}
				}
			}
			else
			{
				state.yaw = 0f;
			}
			if (GameSettings.MODIFIER_KEY.GetKey() && Input.GetKeyDown(KeyCode.X))
			{
				if (InputLockManager.IsUnlocked(ControlTypes.PITCH))
				{
					state.pitchTrim = 0f;
				}
				if (InputLockManager.IsUnlocked(ControlTypes.ROLL))
				{
					state.rollTrim = 0f;
				}
				if (InputLockManager.IsUnlocked(ControlTypes.flag_5))
				{
					state.yawTrim = 0f;
				}
				if (InputLockManager.IsUnlocked(ControlTypes.WHEEL_STEER))
				{
					state.wheelSteerTrim = 0f;
				}
				if (InputLockManager.IsUnlocked(ControlTypes.WHEEL_THROTTLE))
				{
					state.wheelThrottleTrim = 0f;
				}
			}
			if (InputLockManager.IsUnlocked(ControlTypes.LINEAR))
			{
				state.float_0 = 0f - state.float_0;
				state.float_1 = 0f - state.float_1;
				state.float_2 = 0f - state.float_2;
				ProcessAxis(GameSettings.AXIS_TRANSLATE_X, GameSettings.TRANSLATE_RIGHT, GameSettings.TRANSLATE_LEFT, ref state.float_0, ref precisionX);
				ProcessAxis(GameSettings.AXIS_TRANSLATE_Y, GameSettings.TRANSLATE_DOWN, GameSettings.TRANSLATE_UP, ref state.float_1, ref precisionY);
				state.float_1 = 0f - state.float_1;
				ProcessAxis(GameSettings.AXIS_TRANSLATE_Z, GameSettings.TRANSLATE_FWD, GameSettings.TRANSLATE_BACK, ref state.float_2, ref precisionZ);
				if (SPACENAV_USE_AS_FLIGHT_CONTROL)
				{
					state.float_0 += SpaceNavigator.Translation.x * GameSettings.SPACENAV_FLIGHT_SENS_LIN;
					state.float_1 += SpaceNavigator.Translation.y * GameSettings.SPACENAV_FLIGHT_SENS_LIN;
					state.float_2 += SpaceNavigator.Translation.z * GameSettings.SPACENAV_FLIGHT_SENS_LIN;
				}
			}
			else
			{
				state.float_0 = 0f;
				state.float_1 = 0f;
				state.float_2 = 0f;
			}
			state.mainThrottle = throttle;
			if (InputLockManager.IsUnlocked(ControlTypes.WHEEL_STEER))
			{
				state.wheelSteer = GameSettings.AXIS_WHEEL_STEER.GetAxis();
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					if (GameSettings.WHEEL_STEER_RIGHT.GetKey())
					{
						state.wheelSteerTrim = Mathf.Clamp(state.wheelSteerTrim - 0.1f * Time.deltaTime, -1f, 1f);
					}
					if (GameSettings.WHEEL_STEER_LEFT.GetKey())
					{
						state.wheelSteerTrim = Mathf.Clamp(state.wheelSteerTrim + 0.1f * Time.deltaTime, -1f, 1f);
					}
				}
				else
				{
					if (SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
					{
						state.wheelSteer -= SpaceNavigator.Rotation.Yaw() * GameSettings.SPACENAV_FLIGHT_SENS_ROT;
					}
					if (precisionMode)
					{
						precisionWheelSteer = (GameSettings.WHEEL_STEER_RIGHT.GetKey() ? Mathf.Max(-1f, precisionWheelSteer - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (GameSettings.WHEEL_STEER_LEFT.GetKey() ? Mathf.Min(1f, precisionWheelSteer + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : state.wheelSteer));
						state.wheelSteer = precisionWheelSteer;
					}
					else
					{
						state.wheelSteer = (GameSettings.WHEEL_STEER_RIGHT.GetKey() ? (-1f) : (GameSettings.WHEEL_STEER_LEFT.GetKey() ? 1f : state.wheelSteer));
						precisionWheelSteer = state.wheelSteer;
					}
				}
			}
			else
			{
				state.wheelSteer = 0f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.WHEEL_THROTTLE))
			{
				state.wheelThrottle = GameSettings.AXIS_WHEEL_THROTTLE.GetAxis();
				if (GameSettings.MODIFIER_KEY.GetKey())
				{
					if (GameSettings.WHEEL_THROTTLE_DOWN.GetKey())
					{
						state.wheelThrottleTrim = Mathf.Clamp(state.wheelThrottleTrim - 0.1f * Time.deltaTime, -1f, 1f);
					}
					if (GameSettings.WHEEL_THROTTLE_UP.GetKey())
					{
						state.wheelThrottleTrim = Mathf.Clamp(state.wheelThrottleTrim + 0.1f * Time.deltaTime, -1f, 1f);
					}
				}
				else
				{
					if (SPACENAV_USE_AS_FLIGHT_CONTROL)
					{
						state.wheelThrottle += SpaceNavigator.Translation.z * GameSettings.SPACENAV_FLIGHT_SENS_LIN;
					}
					if (precisionMode)
					{
						precisionWheelThrottle = (GameSettings.WHEEL_THROTTLE_DOWN.GetKey() ? Mathf.Max(-1f, precisionWheelThrottle - GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : (GameSettings.WHEEL_THROTTLE_UP.GetKey() ? Mathf.Min(1f, precisionWheelThrottle + GameSettings.INPUT_KEYBOARD_SENSIVITITY * Time.deltaTime) : state.wheelThrottle));
						state.wheelThrottle = precisionWheelThrottle;
					}
					else
					{
						state.wheelThrottle = (GameSettings.WHEEL_THROTTLE_DOWN.GetKey() ? (-1f) : (GameSettings.WHEEL_THROTTLE_UP.GetKey() ? 1f : state.wheelThrottle));
						precisionWheelThrottle = state.wheelThrottle;
					}
				}
			}
			else
			{
				state.wheelThrottle = 0f;
			}
			if (InputLockManager.IsUnlocked(ControlTypes.CUSTOM_ACTION_GROUPS))
			{
				int num = GameSettings.AXIS_CUSTOM.Length;
				if (num > state.custom_axes.Length)
				{
					num = state.custom_axes.Length;
				}
				if (precision_custom_axis == null || precision_custom_axis.Length < num)
				{
					precision_custom_axis = new float[num];
				}
				for (int i = 0; i < num; i++)
				{
					AxisBinding axisBinding = GameSettings.AXIS_CUSTOM[i].axisBinding;
					KeyBinding plusKeyBinding = GameSettings.AXIS_CUSTOM[i].plusKeyBinding;
					KeyBinding minusKeyBinding = GameSettings.AXIS_CUSTOM[i].minusKeyBinding;
					ProcessAxis(axisBinding, plusKeyBinding, minusKeyBinding, ref state.custom_axes[i], ref precision_custom_axis[i]);
				}
			}
			OnRawAxisInput(state);
			state.mainThrottle = (state.mainThrottle + 1f) * 0.5f;
			state.roll += state.rollTrim;
			state.pitch += state.pitchTrim;
			state.yaw += state.yawTrim;
			state.wheelSteer += state.wheelSteerTrim;
			state.wheelThrottle += state.wheelThrottleTrim;
			state.float_0 = 0f - state.float_0;
			state.float_2 = 0f - state.float_2;
		}
		else
		{
			state.pitch = 0f;
			state.yaw = 0f;
			state.roll = 0f;
			state.float_0 = 0f;
			state.float_1 = 0f;
			state.float_2 = 0f;
		}
		if (!(FlightGlobals.ActiveVessel != null))
		{
			return;
		}
		for (int j = 0; j < FlightGlobals.VesselsLoaded.Count; j++)
		{
			Vessel vessel = FlightGlobals.VesselsLoaded[j];
			if (!vessel.packed)
			{
				if (vessel == FlightGlobals.ActiveVessel)
				{
					vessel.SetControlState(state);
				}
				else
				{
					vessel.ctrlState.pitch = 0f + vessel.ctrlState.pitchTrim;
					vessel.ctrlState.yaw = 0f + vessel.ctrlState.yawTrim;
					vessel.ctrlState.roll = 0f + vessel.ctrlState.rollTrim;
					vessel.ctrlState.float_0 = 0f;
					vessel.ctrlState.float_1 = 0f;
					vessel.ctrlState.float_2 = 0f;
				}
				vessel.FeedInputFeed();
			}
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnVesselOverrideGroupChanged.Remove(OnVesselOverrideGroupChanged);
		state.mainThrottle = 0f;
		state.pitch = 0f;
		state.yaw = 0f;
		state.roll = 0f;
		state.float_0 = 0f;
		state.float_1 = 0f;
		state.float_2 = 0f;
		InputLockManager.RemoveControlLock("manualStageLock");
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}
}
