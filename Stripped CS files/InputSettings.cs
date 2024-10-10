using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : DialogGUIVerticalLayout, ISettings
{
	public enum ListenModes
	{
		const_0,
		KEY_PRIMARY,
		KEY_SECONDARY,
		AXIS_PRIMARY,
		AXIS_SECONDARY
	}

	public enum InputPages
	{
		FLIGHT,
		VESSEL,
		CHARACTER,
		MISC,
		OTHER
	}

	public class InputPage : DialogGUIHorizontalLayout
	{
		public InputSettings parent;

		public InputPages type;

		public InputPage()
		{
			OptionEnabledCondition = null;
		}

		public override void Update()
		{
			base.Update();
			uiItem.SetActive(parent.inputPage == type);
		}
	}

	public class FlightInputPage : InputPage
	{
		public FlightInputPage(InputSettings parent)
		{
			parent.drawInputPage(InputPages.FLIGHT, this);
			base.parent = parent;
			type = InputPages.FLIGHT;
		}
	}

	public class VesselInputPage : InputPage
	{
		public VesselInputPage(InputSettings parent)
		{
			parent.drawInputPage(InputPages.VESSEL, this);
			base.parent = parent;
			type = InputPages.VESSEL;
		}
	}

	public class MiscInputPage : InputPage
	{
		public MiscInputPage(InputSettings parent)
		{
			parent.drawInputPage(InputPages.MISC, this);
			base.parent = parent;
			type = InputPages.MISC;
		}
	}

	public class KerbalInputPage : InputPage
	{
		public KerbalInputPage(InputSettings parent)
		{
			parent.drawInputPage(InputPages.CHARACTER, this);
			base.parent = parent;
			type = InputPages.CHARACTER;
		}
	}

	public class OtherInputPage : InputPage
	{
		public OtherInputPage(InputSettings parent)
		{
			parent.drawOtherPage(this);
			base.parent = parent;
			type = InputPages.OTHER;
		}
	}

	public KeyBinding pitchDown;

	public KeyBinding pitchUp;

	public KeyBinding yawLeft;

	public KeyBinding yawRight;

	public KeyBinding rollLeft;

	public KeyBinding rollRight;

	public KeyBinding throttleUp;

	public KeyBinding throttleDown;

	public KeyBinding sasMomentairly;

	public KeyBinding sasToggled;

	public KeyBinding launchStages;

	public KeyBinding cameraMode;

	public KeyBinding cameraNext;

	public KeyBinding cameraReset;

	public KeyBinding pause;

	public KeyBinding precision;

	public KeyBinding scrollViewUp;

	public KeyBinding scrollViewDown;

	public KeyBinding zoomViewIn;

	public KeyBinding zoomViewOut;

	public KeyBinding scrollIconsUp;

	public KeyBinding scrollIconsDown;

	public KeyBinding viewOrbitUp;

	public KeyBinding viewOrbitDown;

	public KeyBinding viewOrbitLeft;

	public KeyBinding viewOrbitRight;

	public KeyBinding timeWarpIncrease;

	public KeyBinding timeWarpDecrease;

	public KeyBinding mapViewToggle;

	public KeyBinding translateUp;

	public KeyBinding translateDown;

	public KeyBinding translateLeft;

	public KeyBinding translateRight;

	public KeyBinding translateFwd;

	public KeyBinding translateBack;

	public KeyBinding rcsToggle;

	public KeyBinding focusNextVessel;

	public KeyBinding focusPrevVessel;

	public KeyBinding toggleUi;

	public KeyBinding toggleStatusScreen;

	public KeyBinding takeScreenshot;

	public KeyBinding toggleLabels;

	public KeyBinding quicksave;

	public KeyBinding quickload;

	public KeyBinding toggleAeroForces;

	public KeyBinding toggleThermalGauges;

	public KeyBinding toggleThermalOverlay;

	public KeyBinding meco;

	public KeyBinding throttleFull;

	public KeyBinding gears;

	public KeyBinding brakes;

	public KeyBinding lights;

	public KeyBinding EVA_forward;

	public KeyBinding EVA_back;

	public KeyBinding EVA_left;

	public KeyBinding EVA_right;

	public KeyBinding EVA_yaw_left;

	public KeyBinding EVA_yaw_right;

	public KeyBinding EVA_Pack_forward;

	public KeyBinding EVA_Pack_back;

	public KeyBinding EVA_Pack_left;

	public KeyBinding EVA_Pack_right;

	public KeyBinding EVA_Pack_up;

	public KeyBinding EVA_Pack_down;

	public KeyBinding EVA_Jump;

	public KeyBinding EVA_Run;

	public KeyBinding EVA_ToggleMovementMode;

	public KeyBinding EVA_TogglePack;

	public KeyBinding EVA_Use;

	public KeyBinding EVA_Board;

	public KeyBinding EVA_Orient;

	public KeyBinding EVA_lights;

	public KeyBinding EVA_Helmet;

	public KeyBinding EVA_ChuteDeploy;

	public KeyBinding Docking_toggleLinRot;

	public KeyBinding Wheel_steer_left;

	public KeyBinding Wheel_steer_right;

	public KeyBinding Wheel_throttle_down;

	public KeyBinding Wheel_throttle_up;

	public AxisBinding axis_wheel_steer;

	public AxisBinding axis_wheel_throttle;

	public KeyBinding AbortActionGroup;

	public KeyBinding CustomActionGroup1;

	public KeyBinding CustomActionGroup2;

	public KeyBinding CustomActionGroup3;

	public KeyBinding CustomActionGroup4;

	public KeyBinding CustomActionGroup5;

	public KeyBinding CustomActionGroup6;

	public KeyBinding CustomActionGroup7;

	public KeyBinding CustomActionGroup8;

	public KeyBinding CustomActionGroup9;

	public KeyBinding CustomActionGroup10;

	public KeyBinding Editor_pitch_up;

	public KeyBinding Editor_pitch_down;

	public KeyBinding Editor_roll_left;

	public KeyBinding Editor_roll_right;

	public KeyBinding Editor_yaw_left;

	public KeyBinding Editor_yaw_right;

	public KeyBinding Editor_reset_rotation;

	public KeyBinding Editor_coordSystem;

	public KeyBinding Editor_symMethod;

	public KeyBinding Editor_symMode;

	public KeyBinding Editor_angleSnap;

	public KeyBinding Editor_modePlace;

	public KeyBinding Editor_modeOffset;

	public KeyBinding Editor_modeRotate;

	public KeyBinding Editor_modeRoot;

	public KeyBinding Editor_partSearch;

	public AxisBinding axis_pitch;

	public AxisBinding axis_yaw;

	public AxisBinding axis_roll;

	public AxisBinding axis_translate_x;

	public AxisBinding axis_translate_y;

	public AxisBinding axis_translate_z;

	public AxisBinding axis_throttle;

	public AxisBinding axis_throttle_inc;

	public AxisBinding axis_EVA_translate_x;

	public AxisBinding axis_EVA_translate_y;

	public AxisBinding axis_EVA_translate_z;

	public AxisBinding axis_EVA_pitch;

	public AxisBinding axis_EVA_yaw;

	public AxisBinding axis_EVA_roll;

	public AxisBinding axis_camera_hdg;

	public AxisBinding axis_camera_pitch;

	public bool EVAorientOnMove;

	public float mouseWheelSensitivity;

	public KeyBinding SpaceNavigatorFunctionToggle;

	public KeyBinding SpaceNavigatorRollLockToggle;

	public float spaceNavCamSensRot;

	public float spaceNavCamSensLin;

	public float spaceNavCamSharpnessRot;

	public float spaceNavCamSharpnessLin;

	public float spaceNavFltSensRot;

	public float spaceNavFltSensLin;

	public bool trackIRenabled;

	public bool trackIRactiveFlight;

	public bool trackIRactiveIVA;

	public bool trackIRactiveEVA;

	public bool trackIRactiveMap;

	public bool trackIRactiveKSC;

	public bool trackIRactiveTrackingStation;

	public bool trackIRactiveEditors;

	public string[] joynames;

	public int deviceCount = 11;

	public int axisCount = 20;

	public float[] initialAxisValues;

	public Event keyEvent;

	public Vector2 KeyScrollPos;

	public Vector2 AxisScrollPos;

	public ListenModes listenMode;

	public KeyBinding keyTarget;

	public KeyBinding potentialKey;

	public AxisBinding axisTarget;

	public AxisBinding potentialAxis;

	public InputBindingModes targetSwitchState;

	public InputBindingModes newSwitchState;

	public string actionTargetName;

	public bool has6DOFDevice;

	public InputPages inputPage;

	public static KeyCode[] unbindableKeys = new KeyCode[3]
	{
		KeyCode.Mouse0,
		KeyCode.Mouse1,
		KeyCode.Mouse2
	};

	public static List<KeyCode> keyValues = GetValidKeys();

	public UISkinDef skin;

	public InputPage[] inputPages = new InputPage[5];

	public MultiOptionDialog listenModeMODialog;

	public PopupDialog listenModeDialog;

	public static List<KeyCode> GetValidKeys()
	{
		List<KeyCode> list = new List<KeyCode>(Enum.GetValues(typeof(KeyCode)) as KeyCode[]);
		int num = unbindableKeys.Length - 1;
		int count = list.Count;
		while (count-- > 0)
		{
			int num2 = num;
			while (num2 >= 0)
			{
				if (unbindableKeys[num2] != list[count])
				{
					num2--;
					continue;
				}
				list.RemoveAt(count);
				break;
			}
		}
		for (int i = 0; i < 127; i++)
		{
			list.Add((KeyCode)(i + 128));
		}
		return list;
	}

	public void GetSettings()
	{
		pitchDown = GameSettings.PITCH_DOWN;
		pitchUp = GameSettings.PITCH_UP;
		yawLeft = GameSettings.YAW_LEFT;
		yawRight = GameSettings.YAW_RIGHT;
		rollLeft = GameSettings.ROLL_LEFT;
		rollRight = GameSettings.ROLL_RIGHT;
		throttleUp = GameSettings.THROTTLE_UP;
		throttleDown = GameSettings.THROTTLE_DOWN;
		sasMomentairly = GameSettings.SAS_HOLD;
		sasToggled = GameSettings.SAS_TOGGLE;
		launchStages = GameSettings.LAUNCH_STAGES;
		cameraMode = GameSettings.CAMERA_MODE;
		cameraNext = GameSettings.CAMERA_NEXT;
		pause = GameSettings.PAUSE;
		precision = GameSettings.PRECISION_CTRL;
		zoomViewIn = GameSettings.ZOOM_IN;
		zoomViewOut = GameSettings.ZOOM_OUT;
		scrollViewUp = GameSettings.SCROLL_VIEW_UP;
		scrollViewDown = GameSettings.SCROLL_VIEW_DOWN;
		scrollIconsUp = GameSettings.SCROLL_ICONS_UP;
		scrollIconsDown = GameSettings.SCROLL_ICONS_DOWN;
		viewOrbitUp = GameSettings.CAMERA_ORBIT_UP;
		viewOrbitDown = GameSettings.CAMERA_ORBIT_DOWN;
		viewOrbitLeft = GameSettings.CAMERA_ORBIT_LEFT;
		viewOrbitRight = GameSettings.CAMERA_ORBIT_RIGHT;
		cameraReset = GameSettings.CAMERA_RESET;
		timeWarpIncrease = GameSettings.TIME_WARP_INCREASE;
		timeWarpDecrease = GameSettings.TIME_WARP_DECREASE;
		mapViewToggle = GameSettings.MAP_VIEW_TOGGLE;
		translateUp = GameSettings.TRANSLATE_UP;
		translateDown = GameSettings.TRANSLATE_DOWN;
		translateLeft = GameSettings.TRANSLATE_LEFT;
		translateRight = GameSettings.TRANSLATE_RIGHT;
		translateFwd = GameSettings.TRANSLATE_FWD;
		translateBack = GameSettings.TRANSLATE_BACK;
		rcsToggle = GameSettings.RCS_TOGGLE;
		focusNextVessel = GameSettings.FOCUS_NEXT_VESSEL;
		focusPrevVessel = GameSettings.FOCUS_PREV_VESSEL;
		toggleUi = GameSettings.TOGGLE_UI;
		toggleStatusScreen = GameSettings.TOGGLE_STATUS_SCREEN;
		toggleLabels = GameSettings.TOGGLE_LABELS;
		takeScreenshot = GameSettings.TAKE_SCREENSHOT;
		quicksave = GameSettings.QUICKSAVE;
		quickload = GameSettings.QUICKLOAD;
		meco = GameSettings.THROTTLE_CUTOFF;
		throttleFull = GameSettings.THROTTLE_FULL;
		gears = GameSettings.LANDING_GEAR;
		brakes = GameSettings.BRAKES;
		lights = GameSettings.HEADLIGHT_TOGGLE;
		toggleAeroForces = GameSettings.TOGGLE_FLIGHT_FORCES;
		toggleThermalGauges = GameSettings.TOGGLE_TEMP_GAUGES;
		toggleThermalOverlay = GameSettings.TOGGLE_TEMP_OVERLAY;
		EVA_forward = GameSettings.EVA_forward;
		EVA_back = GameSettings.EVA_back;
		EVA_left = GameSettings.EVA_left;
		EVA_right = GameSettings.EVA_right;
		EVA_yaw_left = GameSettings.EVA_yaw_left;
		EVA_yaw_right = GameSettings.EVA_yaw_right;
		EVA_Pack_forward = GameSettings.EVA_Pack_forward;
		EVA_Pack_back = GameSettings.EVA_Pack_back;
		EVA_Pack_left = GameSettings.EVA_Pack_left;
		EVA_Pack_right = GameSettings.EVA_Pack_right;
		EVA_Pack_up = GameSettings.EVA_Pack_up;
		EVA_Pack_down = GameSettings.EVA_Pack_down;
		EVA_Jump = GameSettings.EVA_Jump;
		EVA_Run = GameSettings.EVA_Run;
		EVA_Use = GameSettings.EVA_Use;
		EVA_Board = GameSettings.EVA_Board;
		EVA_TogglePack = GameSettings.EVA_TogglePack;
		EVA_ToggleMovementMode = GameSettings.EVA_ToggleMovementMode;
		EVA_Orient = GameSettings.EVA_Orient;
		EVA_lights = GameSettings.EVA_Lights;
		EVA_Helmet = GameSettings.EVA_Helmet;
		EVA_ChuteDeploy = GameSettings.EVA_ChuteDeploy;
		Docking_toggleLinRot = GameSettings.Docking_toggleRotLin;
		Editor_pitch_up = GameSettings.Editor_pitchUp;
		Editor_pitch_down = GameSettings.Editor_pitchDown;
		Editor_yaw_left = GameSettings.Editor_yawLeft;
		Editor_yaw_right = GameSettings.Editor_yawRight;
		Editor_roll_left = GameSettings.Editor_rollLeft;
		Editor_roll_right = GameSettings.Editor_rollRight;
		Editor_reset_rotation = GameSettings.Editor_resetRotation;
		Editor_angleSnap = GameSettings.Editor_toggleAngleSnap;
		Editor_coordSystem = GameSettings.Editor_coordSystem;
		Editor_symMethod = GameSettings.Editor_toggleSymMethod;
		Editor_symMode = GameSettings.Editor_toggleSymMode;
		Editor_modePlace = GameSettings.Editor_modePlace;
		Editor_modeOffset = GameSettings.Editor_modeOffset;
		Editor_modeRotate = GameSettings.Editor_modeRotate;
		Editor_modeRoot = GameSettings.Editor_modeRoot;
		Editor_partSearch = GameSettings.Editor_partSearch;
		axis_pitch = GameSettings.AXIS_PITCH;
		axis_roll = GameSettings.AXIS_ROLL;
		axis_throttle = GameSettings.AXIS_THROTTLE;
		axis_throttle_inc = GameSettings.AXIS_THROTTLE_INC;
		axis_yaw = GameSettings.AXIS_YAW;
		axis_camera_hdg = GameSettings.AXIS_CAMERA_HDG;
		axis_camera_pitch = GameSettings.AXIS_CAMERA_PITCH;
		axis_translate_x = GameSettings.AXIS_TRANSLATE_X;
		axis_translate_y = GameSettings.AXIS_TRANSLATE_Y;
		axis_translate_z = GameSettings.AXIS_TRANSLATE_Z;
		axis_EVA_translate_x = GameSettings.axis_EVA_translate_x;
		axis_EVA_translate_y = GameSettings.axis_EVA_translate_y;
		axis_EVA_translate_z = GameSettings.axis_EVA_translate_z;
		axis_EVA_pitch = GameSettings.axis_EVA_pitch;
		axis_EVA_yaw = GameSettings.axis_EVA_yaw;
		axis_EVA_roll = GameSettings.axis_EVA_roll;
		AbortActionGroup = GameSettings.AbortActionGroup;
		CustomActionGroup1 = GameSettings.CustomActionGroup1;
		CustomActionGroup2 = GameSettings.CustomActionGroup2;
		CustomActionGroup3 = GameSettings.CustomActionGroup3;
		CustomActionGroup4 = GameSettings.CustomActionGroup4;
		CustomActionGroup5 = GameSettings.CustomActionGroup5;
		CustomActionGroup6 = GameSettings.CustomActionGroup6;
		CustomActionGroup7 = GameSettings.CustomActionGroup7;
		CustomActionGroup8 = GameSettings.CustomActionGroup8;
		CustomActionGroup9 = GameSettings.CustomActionGroup9;
		CustomActionGroup10 = GameSettings.CustomActionGroup10;
		axis_wheel_steer = GameSettings.AXIS_WHEEL_STEER;
		axis_wheel_throttle = GameSettings.AXIS_WHEEL_THROTTLE;
		Wheel_steer_left = GameSettings.WHEEL_STEER_LEFT;
		Wheel_steer_right = GameSettings.WHEEL_STEER_RIGHT;
		Wheel_throttle_up = GameSettings.WHEEL_THROTTLE_UP;
		Wheel_throttle_down = GameSettings.WHEEL_THROTTLE_DOWN;
		EVAorientOnMove = GameSettings.EVA_ROTATE_ON_MOVE;
		mouseWheelSensitivity = GameSettings.AXIS_MOUSEWHEEL.primary.scale;
		spaceNavFltSensLin = GameSettings.SPACENAV_FLIGHT_SENS_LIN;
		spaceNavFltSensRot = GameSettings.SPACENAV_FLIGHT_SENS_ROT;
		spaceNavCamSensLin = GameSettings.SPACENAV_CAMERA_SENS_LIN;
		spaceNavCamSensRot = GameSettings.SPACENAV_CAMERA_SENS_ROT;
		spaceNavCamSharpnessLin = GameSettings.SPACENAV_CAMERA_SHARPNESS_LIN;
		spaceNavCamSharpnessRot = GameSettings.SPACENAV_CAMERA_SHARPNESS_ROT;
		SpaceNavigatorFunctionToggle = GameSettings.TOGGLE_SPACENAV_FLIGHT_CONTROL;
		SpaceNavigatorRollLockToggle = GameSettings.TOGGLE_SPACENAV_ROLL_LOCK;
		trackIRenabled = GameSettings.TRACKIR_ENABLED;
		trackIRactiveFlight = GameSettings.TRACKIR.Instance.activeFlight;
		trackIRactiveIVA = GameSettings.TRACKIR.Instance.activeIVA;
		trackIRactiveEVA = GameSettings.TRACKIR.Instance.activeEVA;
		trackIRactiveMap = GameSettings.TRACKIR.Instance.activeMap;
		trackIRactiveKSC = GameSettings.TRACKIR.Instance.activeKSC;
		trackIRactiveTrackingStation = GameSettings.TRACKIR.Instance.activeTrackingStation;
		trackIRactiveEditors = GameSettings.TRACKIR.Instance.activeEditors;
		inputPage = InputPages.FLIGHT;
		has6DOFDevice = !(SpaceNavigator.Instance is SpaceNavigatorNoDevice);
		joynames = Input.GetJoystickNames();
		string text = "Joystick Names:";
		int i = 0;
		for (int num = joynames.Length; i < num; i++)
		{
			if (joynames[i] != string.Empty)
			{
				joynames[i] = InputDevices.TrimDeviceName(joynames[i]);
			}
			else
			{
				joynames[i] = "Joystick " + i;
			}
			text = text + "\n" + i + ": " + joynames[i];
		}
		Debug.Log(text);
		initialAxisValues = new float[deviceCount * axisCount];
	}

	public bool isEnabled()
	{
		return listenMode == ListenModes.const_0;
	}

	public void DrawSettings()
	{
		padding = new RectOffset(8, 8, 8, 0);
		inputPages[0] = new FlightInputPage(this);
		inputPages[1] = new VesselInputPage(this);
		inputPages[2] = new KerbalInputPage(this);
		inputPages[3] = new MiscInputPage(this);
		inputPages[4] = new OtherInputPage(this);
		AddChild(new DialogGUIBox("CONTROL MAPPING", -1f, 18f, null));
		DialogGUIBase dialogGUIBase = new DialogGUIHorizontalLayout(-1f, 30f, 4f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUIToggleGroup(new DialogGUIToggleButton(inputPage == InputPages.FLIGHT, "Flight", delegate(bool b)
		{
			if (b)
			{
				inputPage = InputPages.FLIGHT;
			}
			inputPages[0].uiItem.SetActive(b);
		}, 110f, 30f), new DialogGUIToggleButton(inputPage == InputPages.VESSEL, "Vessel", delegate(bool b)
		{
			if (b)
			{
				inputPage = InputPages.VESSEL;
			}
			inputPages[0].uiItem.SetActive(b);
		}, 110f, 30f), new DialogGUIToggleButton(inputPage == InputPages.CHARACTER, "Kerbals", delegate(bool b)
		{
			if (b)
			{
				inputPage = InputPages.CHARACTER;
			}
			inputPages[0].uiItem.SetActive(b);
		}, 110f, 30f), new DialogGUIToggleButton(inputPage == InputPages.MISC, "Game", delegate(bool b)
		{
			if (b)
			{
				inputPage = InputPages.MISC;
			}
			inputPages[0].uiItem.SetActive(b);
		}, 110f, 30f), new DialogGUIToggleButton(inputPage == InputPages.OTHER, "Other", delegate(bool b)
		{
			if (b)
			{
				inputPage = InputPages.OTHER;
			}
			inputPages[0].uiItem.SetActive(b);
		}, 110f, 30f)));
		DialogGUIBase dialogGUIBase2 = new DialogGUIVerticalLayout(dialogGUIBase);
		dialogGUIBase2.OptionInteractableCondition = isEnabled;
		DialogGUIBase[] list = inputPages;
		dialogGUIBase2.AddChild(new DialogGUIHorizontalLayout(list));
		AddChild(dialogGUIBase2);
	}

	public void drawInputPage(InputPages page, InputPage parent)
	{
		parent.AddChild(new DialogGUIFlexibleSpace());
		DialogGUIBase dialogGUIBase = new DialogGUIVerticalLayout(sw: true);
		parent.AddChild(dialogGUIBase);
		dialogGUIBase.AddChild(new DialogGUIBox("Key/Button Bindings", -1f, 18f, null));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 2f, new RectOffset(4, 20, 0, 0), TextAnchor.UpperLeft);
		switch (page)
		{
		case InputPages.FLIGHT:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Rotation", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Pitch Down", pitchDown));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Pitch Up", pitchUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Yaw Left", yawLeft));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Yaw Right", yawRight));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Roll Left", rollLeft));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Roll Right", rollRight));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Translation", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Up", translateUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Down", translateDown));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Left", translateLeft));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Right", translateRight));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Forward", translateFwd));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Translate Backward", translateBack));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Throttle", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Throttle Up", throttleUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Throttle Down", throttleDown));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Other", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Launch/Stages", launchStages));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Switch Translation/Rotation", Docking_toggleLinRot));
			break;
		case InputPages.VESSEL:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Systems", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Landing Gear", gears));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Brakes", brakes));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("SAS Hold", sasMomentairly));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("SAS Toggle", sasToggled));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("RCS Toggle", rcsToggle));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Throttle Cut-off", meco));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Full Throttle", throttleFull));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Precision Controls Toggle", precision));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Lights", lights));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Emergency abort", AbortActionGroup));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 1", CustomActionGroup1));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 2", CustomActionGroup2));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 3", CustomActionGroup3));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 4", CustomActionGroup4));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 5", CustomActionGroup5));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 6", CustomActionGroup6));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 7", CustomActionGroup7));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 8", CustomActionGroup8));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 9", CustomActionGroup9));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Custom action 10", CustomActionGroup10));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Wheels", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Steer Left", Wheel_steer_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Steer Right", Wheel_steer_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Drive Forward", Wheel_throttle_up));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Drive Back", Wheel_throttle_down));
			break;
		case InputPages.CHARACTER:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Character Controls", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Move Forward", EVA_forward));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Move Back", EVA_back));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Move Left", EVA_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Move Right", EVA_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Turn Left", EVA_yaw_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Turn Right", EVA_yaw_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Forward", EVA_Pack_forward));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Back", EVA_Pack_back));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Left", EVA_Pack_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Right", EVA_Pack_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Up", EVA_Pack_up));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("EVAPack Down", EVA_Pack_down));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Jump", EVA_Jump));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Run", EVA_Run));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Use/Grab", EVA_Use));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Board", EVA_Board));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle EVAPack", EVA_TogglePack));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Movement Mode", EVA_ToggleMovementMode));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Orient to View", EVA_Orient));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Lights", EVA_lights));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Helmet", EVA_Helmet));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Deploy Parachute", EVA_ChuteDeploy));
			break;
		case InputPages.MISC:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("General", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Orbital Map View", mapViewToggle));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Increase Time Warp", timeWarpIncrease));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Decrease Time Warp", timeWarpDecrease));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Focus Next Vessel", focusNextVessel));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Focus Prev Vessel", focusPrevVessel));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Take Screenshot", takeScreenshot));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Pause", pause));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Quicksave", quicksave));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Quickload", quickload));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Camera", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Camera Mode", cameraMode));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Camera Next", cameraNext));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Camera Reset", cameraReset));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Zoom In", zoomViewIn));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Zoom Out", zoomViewOut));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("View Up", viewOrbitUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("View Down", viewOrbitDown));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("View Left", viewOrbitLeft));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("View Right", viewOrbitRight));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Editor", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Scroll View Up", scrollViewUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Scroll View Down", scrollViewDown));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Pitch Part Down", Editor_pitch_down));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Pitch Part Up", Editor_pitch_up));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Yaw Part Left", Editor_yaw_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Yaw Part Right", Editor_yaw_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Roll Part Left", Editor_roll_left));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Roll Part Right", Editor_roll_right));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Reset Part Rotation", Editor_reset_rotation));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Place Mode", Editor_modePlace));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Offset Mode", Editor_modeOffset));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Rotate Mode", Editor_modeRotate));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Root Mode", Editor_modeRoot));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Local/Absolute", Editor_coordSystem));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Angle Snap", Editor_angleSnap));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Toggle Symmetry Method", Editor_symMethod));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Cycle Symmetry Count", Editor_symMode));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Search All Parts", Editor_partSearch));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("UI", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Flight UI", toggleUi));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Scroll Icons Up", scrollIconsUp));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Scroll Icons Down", scrollIconsDown));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Vessel Labels", toggleLabels));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Flight Log", toggleStatusScreen));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Flight Forces", toggleAeroForces));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Temp. Gauges", toggleThermalGauges));
			dialogGUIVerticalLayout.AddChild(DrawKeyBinding("Show/Hide Temp. Overlay", toggleThermalOverlay));
			break;
		}
		dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true));
		dialogGUIBase.AddChild(new DialogGUIScrollList(new Vector2(350f, -1f), hScroll: false, vScroll: true, dialogGUIVerticalLayout));
		dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 2f, new RectOffset(4, 20, 0, 0), TextAnchor.UpperLeft);
		dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true));
		dialogGUIBase = new DialogGUIVerticalLayout(sw: true);
		parent.AddChild(dialogGUIBase);
		dialogGUIBase.AddChild(new DialogGUIBox("Axis Bindings", -1f, 18f, null));
		switch (page)
		{
		case InputPages.FLIGHT:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Rotation", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Pitch Axis", axis_pitch));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Roll Axis", axis_roll));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Yaw Axis", axis_yaw));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Translation", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Left/Right", axis_translate_x));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Up/Down", axis_translate_y));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Forward/Back", axis_translate_z));
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Throttle", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Throttle Axis", axis_throttle));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Throttle (Incremental)", axis_throttle_inc));
			break;
		case InputPages.VESSEL:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Wheels", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Steering Axis", axis_wheel_steer));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Throttle Axis", axis_wheel_throttle));
			break;
		case InputPages.CHARACTER:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Character", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Forward/Back", axis_EVA_translate_z));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Left/Right", axis_EVA_translate_x));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Translate Up/Down", axis_EVA_translate_y));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Pitch (Analogue)", axis_EVA_pitch));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Yaw   (Analogue)", axis_EVA_yaw));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Roll  (Analogue)", axis_EVA_roll));
			break;
		case InputPages.MISC:
			dialogGUIVerticalLayout.AddChild(new DialogGUIBox("Camera", -1f, 18f, null));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Camera Horizontal", axis_camera_hdg));
			dialogGUIVerticalLayout.AddChild(DrawAxisBinding("Camera Vertical", axis_camera_pitch));
			break;
		}
		dialogGUIBase.AddChild(new DialogGUIScrollList(new Vector2(350f, -1f), hScroll: false, vScroll: true, dialogGUIVerticalLayout));
		parent.AddChild(new DialogGUIFlexibleSpace());
	}

	public void drawOtherPage(InputPage parent)
	{
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(new DialogGUILabel("Active in: "), new DialogGUIToggle(trackIRactiveFlight, "Flight External", delegate(bool b)
		{
			trackIRactiveFlight = b;
		}, 60f), new DialogGUIToggle(trackIRactiveIVA, "Flight Internal", delegate(bool b)
		{
			trackIRactiveIVA = b;
		}, 60f), new DialogGUIToggle(trackIRactiveEVA, "Flight EVA", delegate(bool b)
		{
			trackIRactiveEVA = b;
		}, 60f), new DialogGUIToggle(trackIRactiveMap, "Flight Map View", delegate(bool b)
		{
			trackIRactiveMap = b;
		}, 60f), new DialogGUIToggle(trackIRactiveKSC, "Space Center", delegate(bool b)
		{
			trackIRactiveKSC = b;
		}, 60f), new DialogGUIToggle(trackIRactiveTrackingStation, "Tracking Station", delegate(bool b)
		{
			trackIRactiveTrackingStation = b;
		}, 60f), new DialogGUIToggle(trackIRactiveEditors, "Editors", delegate(bool b)
		{
			trackIRactiveEditors = b;
		}, 60f));
		parent.AddChild(new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIVerticalLayout(800f, -1f, new DialogGUIBox("Other Control Settings and Device Options", -1f, 18f, null), new DialogGUIHorizontalLayout(-1f, 400f, new DialogGUIVerticalLayout(new DialogGUIBox("Other Settings", 300f, 18f, null), new DialogGUIToggle(EVAorientOnMove, "EVAs Auto-Rotate to Camera", delegate(bool b)
		{
			EVAorientOnMove = b;
		}, 250f), new DialogGUISpace(15f), new DialogGUILabel(() => "Mouse Wheel Sensitivity: " + mouseWheelSensitivity.ToString("0.000")), new DialogGUISlider(() => mouseWheelSensitivity, 0f, 1f, wholeNumbers: false, -1f, -1f, delegate(float f)
		{
			mouseWheelSensitivity = f;
		}), new DialogGUISpace(15f), new DialogGUIBox("Track IR", -1f, 18f, null), new DialogGUIToggle(trackIRenabled, () => (!trackIRenabled) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			trackIRenabled = b;
		}, 60f), dialogGUIVerticalLayout)))));
		dialogGUIVerticalLayout.OptionInteractableCondition = () => trackIRenabled;
		parent.AddChild(new DialogGUISpace(15f));
		DialogGUIVerticalLayout dialogGUIVerticalLayout2 = new DialogGUIVerticalLayout();
		parent.AddChild(dialogGUIVerticalLayout2);
		dialogGUIVerticalLayout2.OptionInteractableCondition = () => has6DOFDevice && listenMode == ListenModes.const_0;
		dialogGUIVerticalLayout2.AddChild(new DialogGUIBox("6-DOF Device:", -1f, 18f, null));
		if (!has6DOFDevice)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel("A 6-DOF Input Device is a hardware controller that allows manipulating objects and views in 3D applications.\n\nCurrently, this is only supported on the Windows version of KSP.", expandW: true, expandH: true));
		}
		dialogGUIVerticalLayout2.AddChild(DrawKeyBinding("Toggle Camera/Flight Control", SpaceNavigatorFunctionToggle));
		dialogGUIVerticalLayout2.AddChild(DrawKeyBinding("Lock/Unlock Roll Axis", SpaceNavigatorRollLockToggle));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIBox("6-DOF Camera Control:", -1f, 18f, null));
		dialogGUIVerticalLayout2.AddChild(new DialogGUILabel("<b>Pitch, Yaw and Roll</b>"));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sensitivity: " + spaceNavCamSensRot.ToString("0.0"), 100f), new DialogGUILabel("Slow", 50f), new DialogGUISlider(() => spaceNavCamSensRot, 1f, 50f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavCamSensRot = f;
		}), new DialogGUILabel("Fast", 50f)));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sharpness: " + spaceNavCamSharpnessRot.ToString("0.0"), 100f), new DialogGUILabel("Smooth", 50f), new DialogGUISlider(() => spaceNavCamSharpnessRot, 0.1f, 20f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavCamSharpnessRot = f;
		}), new DialogGUILabel("Precise", 50f)));
		dialogGUIVerticalLayout2.AddChild(new DialogGUISpace(5f));
		dialogGUIVerticalLayout2.AddChild(new DialogGUILabel("<b>XYZ Translation</b>"));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sensitivity: " + spaceNavCamSensLin.ToString("0.0"), 100f), new DialogGUILabel("Slow", 50f), new DialogGUISlider(() => spaceNavCamSensLin, 1f, 50f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavCamSensLin = f;
		}), new DialogGUILabel("Fast", 50f)));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sharpness: " + spaceNavCamSharpnessLin.ToString("0.0"), 100f), new DialogGUILabel("Smooth", 50f), new DialogGUISlider(() => spaceNavCamSharpnessLin, 0.1f, 20f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavCamSharpnessLin = f;
		}), new DialogGUILabel("Precise", 50f)));
		dialogGUIVerticalLayout2.AddChild(new DialogGUISpace(10f));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIBox("6-DOF Flight Control:", -1f, 18f, null));
		dialogGUIVerticalLayout2.AddChild(new DialogGUILabel("<b>Pitch, Yaw and Roll</b>"));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sensitivity: " + spaceNavFltSensRot.ToString("0.0"), 100f), new DialogGUILabel("Slow", 50f), new DialogGUISlider(() => spaceNavFltSensRot, 0.1f, 10f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavFltSensRot = f;
		}), new DialogGUILabel("Fast", 50f)));
		dialogGUIVerticalLayout2.AddChild(new DialogGUISpace(5f));
		dialogGUIVerticalLayout2.AddChild(new DialogGUILabel("<b>XYZ Translation</b>"));
		dialogGUIVerticalLayout2.AddChild(new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Sensitivity: " + spaceNavFltSensLin.ToString("0.0"), 100f), new DialogGUILabel("Slow", 50f), new DialogGUISlider(() => spaceNavFltSensLin, 0.1f, 10f, wholeNumbers: false, -1f, 20f, delegate(float f)
		{
			spaceNavFltSensLin = f;
		}), new DialogGUILabel("Fast", 50f)));
		parent.AddChild(new DialogGUIFlexibleSpace());
	}

	public void SpawnListenModeDialog()
	{
		string text = actionTargetName;
		switch (listenMode)
		{
		case ListenModes.KEY_PRIMARY:
		case ListenModes.AXIS_PRIMARY:
			text += " (Primary)";
			break;
		case ListenModes.KEY_SECONDARY:
		case ListenModes.AXIS_SECONDARY:
			text += " (Secondary)";
			break;
		}
		listenModeMODialog = new MultiOptionDialog("InputSettingsListener", "", text, skin, 600f, drawListenModeWindow());
		listenModeDialog = PopupDialog.SpawnPopupDialog(listenModeMODialog, persistAcrossScenes: false, skin);
		listenModeDialog.OnDismiss = OnDismissListenModeDialog;
	}

	public DialogGUIBase[] drawListenModeWindow()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		list.Add(new DialogGUIHorizontalLayout());
		list[0].OptionInteractableCondition = () => listenMode != ListenModes.const_0;
		list.Add(new DialogGUIFlexibleSpace());
		if (listenMode == ListenModes.KEY_PRIMARY || listenMode == ListenModes.KEY_SECONDARY)
		{
			list.Add(new DialogGUIVerticalLayout(new DialogGUILabel("Press the key or joystick button to use for <b>" + actionTargetName + "</b>"), new DialogGUISpace(8f), new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Current Assignment: <b>" + ((listenMode == ListenModes.KEY_PRIMARY) ? potentialKey.primary.ToString() : potentialKey.secondary.ToString()) + "</b>"), new DialogGUISpace(8f))));
		}
		if (listenMode == ListenModes.AXIS_PRIMARY || listenMode == ListenModes.AXIS_SECONDARY)
		{
			list.Add(new DialogGUIVerticalLayout(new DialogGUILabel("Move the joystick axis to use for <b>" + actionTargetName + "</b>"), new DialogGUISpace(8f), new DialogGUIHorizontalLayout(new DialogGUILabel(() => "Current Assignment: <b>" + ((listenMode == ListenModes.AXIS_PRIMARY) ? potentialAxis.primary.title : potentialAxis.secondary.title) + "</b>"), new DialogGUISpace(8f))));
		}
		list.Add(new DialogGUIFlexibleSpace());
		list.Add(new DialogGUILayoutEnd());
		list.Add(new DialogGUIFlexibleSpace());
		DialogGUIToggle stagingToggle = new DialogGUIToggle(() => (targetSwitchState & InputBindingModes.Staging) != 0, "Staging", delegate
		{
		}, 100f);
		DialogGUIToggle dockingTransToggle = new DialogGUIToggle(() => (targetSwitchState & InputBindingModes.Docking_Translation) != 0, "Docking (Translation)", delegate
		{
		}, 180f);
		DialogGUIToggle dockingRotToggle = new DialogGUIToggle(() => (targetSwitchState & InputBindingModes.Docking_Rotation) != 0, "Docking (Rotation)", delegate
		{
		}, 160f);
		DialogGUIBase layout = new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUIFlexibleSpace(), new DialogGUILabel("Use in Modes: ", 100f), stagingToggle, dockingTransToggle, dockingRotToggle, new DialogGUIFlexibleSpace());
		layout.OnUpdate = delegate
		{
			layout.uiItem.SetActive(inputPage == InputPages.FLIGHT || inputPage == InputPages.VESSEL);
			if (inputPage == InputPages.FLIGHT || inputPage == InputPages.VESSEL)
			{
				newSwitchState = InputBindingModes.None;
				if ((bool)stagingToggle.uiItem.GetComponent<Toggle>())
				{
					newSwitchState |= InputBindingModes.Staging;
				}
				if ((bool)dockingTransToggle.uiItem.GetComponent<Toggle>())
				{
					newSwitchState |= InputBindingModes.Docking_Translation;
				}
				if ((bool)dockingRotToggle.uiItem.GetComponent<Toggle>())
				{
					newSwitchState |= InputBindingModes.Docking_Rotation;
				}
				targetSwitchState = newSwitchState;
			}
		};
		list.Add(layout);
		list.Add(new DialogGUIFlexibleSpace());
		list.Add(new DialogGUIHorizontalLayout());
		list.Add(new DialogGUIButton("Clear Assignment", delegate
		{
			if (listenMode == ListenModes.KEY_PRIMARY || listenMode == ListenModes.KEY_SECONDARY)
			{
				SetKey(new KeyCodeExtended());
			}
			if (listenMode == ListenModes.AXIS_PRIMARY || listenMode == ListenModes.AXIS_SECONDARY)
			{
				SetAxis("None", "None", -1, -1, listenMode);
			}
			ResetAxisValues();
		}, () => true, 120f, 30f, dismissOnSelect: false, skin.customStyles[0]));
		list.Add(new DialogGUIFlexibleSpace());
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_190323"), OnDismissListenModeDialog, () => true, 80f, 30f, dismissOnSelect: false, skin.customStyles[0]));
		list.Add(new DialogGUIButton(Localizer.Format("#autoLOC_190328"), delegate
		{
			switch (listenMode)
			{
			case ListenModes.AXIS_PRIMARY:
			case ListenModes.AXIS_SECONDARY:
				ApplyAxisBinding();
				break;
			case ListenModes.KEY_PRIMARY:
			case ListenModes.KEY_SECONDARY:
				ApplyKeyBinding();
				break;
			}
			listenModeDialog.Dismiss();
		}, () => true, 80f, 30f, dismissOnSelect: true, skin.customStyles[0]));
		list.Add(new DialogGUILayoutEnd());
		return list.ToArray();
	}

	public void OnDismissListenModeDialog()
	{
		potentialKey = null;
		potentialAxis = null;
		listenMode = ListenModes.const_0;
		listenModeDialog.Dismiss();
	}

	public DialogGUIBase DrawKeyBinding(string name, KeyBinding keyRef)
	{
		return new DialogGUIHorizontalLayout(false, false, 2f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(name + ": ", skin.customStyles[1], expandW: true), new DialogGUIFlexibleSpace(), new DialogGUIButton(() => string.Concat(keyRef.primary), delegate
		{
			ListenToKey(keyRef, isPrimary: true, name);
		}, 100f, 30f, false), new DialogGUIButton(() => string.Concat(keyRef.secondary), delegate
		{
			ListenToKey(keyRef, isPrimary: false, name);
		}, 100f, 30f, false));
	}

	public DialogGUIBase DrawAxisBinding(string name, AxisBinding axisRef)
	{
		return new DialogGUIVerticalLayout(new DialogGUILabel(name + ": ", expandW: true), new DialogGUILabel("Primary:", expandW: true, expandH: true), new DialogGUIHorizontalLayout(new DialogGUIButton(() => axisRef.primary.title, delegate
		{
			ListenToAxis(axisRef, primary: true, name);
		}, 250f, 30f, false), new DialogGUIToggle(() => axisRef.primary.inverted, "Invert", delegate(bool b)
		{
			axisRef.primary.inverted = b;
		}, 60f), new DialogGUIFlexibleSpace()), new DialogGUIHorizontalLayout(new DialogGUILabel("Sensitivity ", skin.customStyles[1], expandW: true, expandH: true), new DialogGUISlider(delegate
		{
			if (Mathf.Abs(axisRef.primary.sensitivity - 1f) < 0.1f)
			{
				axisRef.primary.sensitivity = 1f;
			}
			return axisRef.primary.sensitivity;
		}, 0.3f, 3f, wholeNumbers: false, -1f, -1f, delegate(float f)
		{
			axisRef.primary.sensitivity = f;
		})), new DialogGUIHorizontalLayout(new DialogGUILabel("Dead Zone ", skin.customStyles[1], expandW: true, expandH: true), new DialogGUISlider(() => axisRef.primary.deadzone, 0f, 1f, wholeNumbers: false, -1f, -1f, delegate(float f)
		{
			axisRef.primary.deadzone = f;
		})), new DialogGUISpace(15f), new DialogGUILabel("Secondary:", expandW: true, expandH: true), new DialogGUIHorizontalLayout(new DialogGUIButton(() => axisRef.secondary.title, delegate
		{
			ListenToAxis(axisRef, primary: false, name);
		}, 250f, 30f, false), new DialogGUIToggle(() => axisRef.secondary.inverted, "Invert", delegate(bool b)
		{
			axisRef.secondary.inverted = b;
		}, 60f), new DialogGUIFlexibleSpace()), new DialogGUIHorizontalLayout(new DialogGUILabel("Sensitivity ", skin.customStyles[1], expandW: true, expandH: true), new DialogGUISlider(delegate
		{
			if (Mathf.Abs(axisRef.secondary.sensitivity - 1f) < 0.1f)
			{
				axisRef.secondary.sensitivity = 1f;
			}
			return axisRef.secondary.sensitivity;
		}, 0.3f, 3f, wholeNumbers: false, -1f, -1f, delegate(float f)
		{
			axisRef.secondary.sensitivity = f;
		})), new DialogGUIHorizontalLayout(new DialogGUILabel("Dead Zone ", skin.customStyles[1], expandW: true, expandH: true), new DialogGUISlider(() => axisRef.secondary.deadzone, 0f, 1f, wholeNumbers: false, -1f, -1f, delegate(float f)
		{
			axisRef.secondary.deadzone = f;
		})), new DialogGUISpace(30f));
	}

	public void ListenToKey(KeyBinding target, bool isPrimary, string actionName)
	{
		listenMode = (isPrimary ? ListenModes.KEY_PRIMARY : ListenModes.KEY_SECONDARY);
		targetSwitchState = (isPrimary ? target.switchState : target.switchStateSecondary);
		keyTarget = target;
		potentialKey = new KeyBinding();
		ConfigNode node = new ConfigNode();
		keyTarget.Save(node);
		potentialKey.Load(node);
		actionTargetName = actionName;
		SpawnListenModeDialog();
	}

	public void ListenToAxis(AxisBinding target, bool primary, string actionName)
	{
		listenMode = (primary ? ListenModes.AXIS_PRIMARY : ListenModes.AXIS_SECONDARY);
		axisTarget = target;
		potentialAxis = new AxisBinding();
		ConfigNode node = new ConfigNode();
		axisTarget.Save(node);
		potentialAxis.Load(node);
		ResetAxisValues();
		targetSwitchState = (primary ? target.primary.switchState : target.secondary.switchState);
		actionTargetName = actionName;
		SpawnListenModeDialog();
	}

	public void ResetAxisValues()
	{
		int i = 0;
		for (int num = initialAxisValues.Length; i < num; i++)
		{
			initialAxisValues[i] = Input.GetAxis("joy" + i / axisCount + "." + i % axisCount);
		}
	}

	public void SetKey(KeyCodeExtended key)
	{
		if (listenMode != 0 && listenMode != ListenModes.AXIS_PRIMARY && listenMode != ListenModes.AXIS_SECONDARY)
		{
			if (listenMode == ListenModes.KEY_PRIMARY)
			{
				potentialKey.primary = key;
				potentialKey.switchState = targetSwitchState;
			}
			if (listenMode == ListenModes.KEY_SECONDARY)
			{
				potentialKey.secondary = key;
				potentialKey.switchStateSecondary = targetSwitchState;
			}
			Debug.Log("listen target set to: " + key.ToString());
		}
	}

	public void SetAxis(string axis, string name, int axisIdx, int deviceIdx, ListenModes mode)
	{
		AxisBinding_Single axisBinding_Single = ((mode == ListenModes.AXIS_PRIMARY || mode != ListenModes.AXIS_SECONDARY) ? potentialAxis.primary : potentialAxis.secondary);
		axisBinding_Single.idTag = axis;
		axisBinding_Single.name = name;
		axisBinding_Single.title = name + ((axisIdx != -1) ? (" Axis " + axisIdx) : "");
		axisBinding_Single.deviceIdx = deviceIdx;
		axisBinding_Single.axisIdx = axisIdx;
		axisBinding_Single.switchState = targetSwitchState;
		Debug.Log("[InputSettings]: listen target set to " + axis + " - " + name + "; Axis " + axisIdx + ", Device " + deviceIdx);
		ResetAxisValues();
	}

	public void ApplyAxisBinding()
	{
		ConfigNode node = new ConfigNode();
		potentialAxis.Save(node);
		axisTarget.Load(node);
		potentialAxis = null;
		Debug.Log("listen target set to: " + ((listenMode == ListenModes.AXIS_PRIMARY) ? axisTarget.primary.name : axisTarget.secondary.name));
		listenMode = ListenModes.const_0;
	}

	public void ApplyKeyBinding()
	{
		ConfigNode node = new ConfigNode();
		potentialKey.Save(node);
		keyTarget.Load(node);
		potentialKey = null;
		Debug.Log("listen target set to: " + keyTarget.name);
		listenMode = ListenModes.const_0;
	}

	public new void OnUpdate()
	{
		Update();
		if (listenMode == ListenModes.const_0)
		{
			return;
		}
		if (listenMode == ListenModes.AXIS_PRIMARY || listenMode == ListenModes.AXIS_SECONDARY)
		{
			for (int i = 0; i < deviceCount; i++)
			{
				for (int j = 0; j < axisCount; j++)
				{
					if (Mathf.Abs(Input.GetAxis("joy" + i + "." + j) - initialAxisValues[i * axisCount + j]) > 0.5f)
					{
						SetAxis("joy" + i + "." + j, joynames[i], j, i, listenMode);
					}
				}
			}
		}
		if ((listenMode == ListenModes.KEY_PRIMARY || listenMode == ListenModes.KEY_SECONDARY) && ExtendedInput.DetectKeyDown(keyValues, out var key))
		{
			SetKey(key);
		}
	}

	public DialogGUIBase[] DrawMiniSettings()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		if (!FlightGlobals.ActiveVessel.isEVA && !MapView.MapIsEnabled)
		{
			list.Add(new DialogGUIBox("#autoLOC_900783", -1f, 18f, null));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900771", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.THROTTLE_UP.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900772", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.THROTTLE_DOWN.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900791", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.THROTTLE_FULL.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900790", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.THROTTLE_CUTOFF.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900845", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.CAMERA_MODE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900846", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.CAMERA_NEXT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900758", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.PITCH_UP.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900757", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.PITCH_DOWN.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900761", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.ROLL_LEFT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900762", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.ROLL_RIGHT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900759", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.YAW_LEFT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900760", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.YAW_RIGHT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900787", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.SAS_HOLD.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900788", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.SAS_TOGGLE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148071", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.LAUNCH_STAGES.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900789", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.RCS_TOGGLE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900768", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_FWD.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900769", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_BACK.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900765", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_DOWN.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900766", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_LEFT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900764", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_UP.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900767", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TRANSLATE_RIGHT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148942", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.NAVBALL_TOGGLE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900837", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TAKE_SCREENSHOT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148195", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_UI.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148199", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_STATUS_SCREEN.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148198", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_LABELS.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900840", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.QUICKSAVE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900842", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.QUICKLOAD.name), new DialogGUIFlexibleSpace()));
		}
		else if (MapView.MapIsEnabled)
		{
			list.Add(new DialogGUIBox("#autoLOC_308130", -1f, 18f, null));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900831", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.MAP_VIEW_TOGGLE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900847", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.CAMERA_RESET.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900832", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TIME_WARP_INCREASE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900833", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TIME_WARP_DECREASE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148942", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.NAVBALL_TOGGLE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900837", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TAKE_SCREENSHOT.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148195", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_UI.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148199", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_STATUS_SCREEN.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148198", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.TOGGLE_LABELS.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900840", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.QUICKSAVE.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900842", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.QUICKLOAD.name), new DialogGUIFlexibleSpace()));
		}
		else if (FlightGlobals.ActiveVessel.isEVA)
		{
			list.Add(new DialogGUIBox("#autoLOC_900811", -1f, 18f, null));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_148129", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Use.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900812", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_forward.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900813", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_back.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900814", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_left.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900815", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_right.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900819", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Run.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900818", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Jump.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900824", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_TogglePack.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900816", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_yaw_left.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900817", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_yaw_right.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900825", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Pack_up.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900826", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Pack_down.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900823", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Lights.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_8003231", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_Helmet.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_900821", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_ToggleMovementMode.name), new DialogGUIFlexibleSpace()));
			list.Add(new DialogGUIHorizontalLayout(new DialogGUILabel("#autoLOC_8004163", 240f), new DialogGUISpace(30f), new DialogGUILabel(GameSettings.EVA_ChuteDeploy.name), new DialogGUIFlexibleSpace()));
		}
		return list.ToArray();
	}

	public void ApplySettings()
	{
		GameSettings.PITCH_DOWN = pitchDown;
		GameSettings.PITCH_UP = pitchUp;
		GameSettings.YAW_LEFT = yawLeft;
		GameSettings.YAW_RIGHT = yawRight;
		GameSettings.ROLL_LEFT = rollLeft;
		GameSettings.ROLL_RIGHT = rollRight;
		GameSettings.THROTTLE_UP = throttleUp;
		GameSettings.THROTTLE_DOWN = throttleDown;
		GameSettings.SAS_HOLD = sasMomentairly;
		GameSettings.SAS_TOGGLE = sasToggled;
		GameSettings.LAUNCH_STAGES = launchStages;
		GameSettings.CAMERA_MODE = cameraMode;
		GameSettings.CAMERA_NEXT = cameraNext;
		GameSettings.CAMERA_RESET = cameraReset;
		GameSettings.PAUSE = pause;
		GameSettings.PRECISION_CTRL = precision;
		GameSettings.ZOOM_IN = zoomViewIn;
		GameSettings.ZOOM_OUT = zoomViewOut;
		GameSettings.SCROLL_VIEW_UP = scrollViewUp;
		GameSettings.SCROLL_VIEW_DOWN = scrollViewDown;
		GameSettings.SCROLL_ICONS_UP = scrollIconsUp;
		GameSettings.SCROLL_ICONS_DOWN = scrollIconsDown;
		GameSettings.CAMERA_ORBIT_UP = viewOrbitUp;
		GameSettings.CAMERA_ORBIT_DOWN = viewOrbitDown;
		GameSettings.CAMERA_ORBIT_LEFT = viewOrbitLeft;
		GameSettings.CAMERA_ORBIT_RIGHT = viewOrbitRight;
		GameSettings.TIME_WARP_INCREASE = timeWarpIncrease;
		GameSettings.TIME_WARP_DECREASE = timeWarpDecrease;
		GameSettings.MAP_VIEW_TOGGLE = mapViewToggle;
		GameSettings.TRANSLATE_UP = translateUp;
		GameSettings.TRANSLATE_DOWN = translateDown;
		GameSettings.TRANSLATE_LEFT = translateLeft;
		GameSettings.TRANSLATE_RIGHT = translateRight;
		GameSettings.TRANSLATE_FWD = translateFwd;
		GameSettings.TRANSLATE_BACK = translateBack;
		GameSettings.RCS_TOGGLE = rcsToggle;
		GameSettings.FOCUS_NEXT_VESSEL = focusNextVessel;
		GameSettings.FOCUS_PREV_VESSEL = focusPrevVessel;
		GameSettings.TAKE_SCREENSHOT = takeScreenshot;
		GameSettings.TOGGLE_LABELS = toggleLabels;
		GameSettings.TOGGLE_STATUS_SCREEN = toggleStatusScreen;
		GameSettings.TOGGLE_UI = toggleUi;
		GameSettings.QUICKSAVE = quicksave;
		GameSettings.QUICKLOAD = quickload;
		GameSettings.THROTTLE_CUTOFF = meco;
		GameSettings.THROTTLE_FULL = throttleFull;
		GameSettings.LANDING_GEAR = gears;
		GameSettings.BRAKES = brakes;
		GameSettings.HEADLIGHT_TOGGLE = lights;
		GameSettings.EVA_forward = EVA_forward;
		GameSettings.EVA_back = EVA_back;
		GameSettings.EVA_left = EVA_left;
		GameSettings.EVA_right = EVA_right;
		GameSettings.EVA_yaw_left = EVA_yaw_left;
		GameSettings.EVA_yaw_right = EVA_yaw_right;
		GameSettings.EVA_Pack_forward = EVA_Pack_forward;
		GameSettings.EVA_Pack_back = EVA_Pack_back;
		GameSettings.EVA_Pack_left = EVA_Pack_left;
		GameSettings.EVA_Pack_right = EVA_Pack_right;
		GameSettings.EVA_Pack_up = EVA_Pack_up;
		GameSettings.EVA_Pack_down = EVA_Pack_down;
		GameSettings.EVA_Jump = EVA_Jump;
		GameSettings.EVA_Run = EVA_Run;
		GameSettings.EVA_ToggleMovementMode = EVA_ToggleMovementMode;
		GameSettings.EVA_TogglePack = EVA_TogglePack;
		GameSettings.EVA_Use = EVA_Use;
		GameSettings.EVA_Board = EVA_Board;
		GameSettings.EVA_Orient = EVA_Orient;
		GameSettings.EVA_Lights = EVA_lights;
		GameSettings.EVA_Helmet = EVA_Helmet;
		GameSettings.EVA_ChuteDeploy = EVA_ChuteDeploy;
		GameSettings.Docking_toggleRotLin = Docking_toggleLinRot;
		GameSettings.Editor_pitchUp = Editor_pitch_up;
		GameSettings.Editor_pitchDown = Editor_pitch_down;
		GameSettings.Editor_yawLeft = Editor_yaw_left;
		GameSettings.Editor_yawRight = Editor_yaw_right;
		GameSettings.Editor_rollLeft = Editor_roll_left;
		GameSettings.Editor_rollRight = Editor_roll_right;
		GameSettings.Editor_resetRotation = Editor_reset_rotation;
		GameSettings.Editor_modePlace = Editor_modePlace;
		GameSettings.Editor_modeOffset = Editor_modeOffset;
		GameSettings.Editor_modeRotate = Editor_modeRotate;
		GameSettings.Editor_modeRoot = Editor_modeRoot;
		GameSettings.Editor_coordSystem = Editor_coordSystem;
		GameSettings.Editor_toggleAngleSnap = Editor_angleSnap;
		GameSettings.Editor_toggleSymMethod = Editor_symMethod;
		GameSettings.Editor_toggleSymMode = Editor_symMode;
		GameSettings.Editor_partSearch = Editor_partSearch;
		GameSettings.AXIS_WHEEL_STEER = axis_wheel_steer;
		GameSettings.AXIS_WHEEL_THROTTLE = axis_wheel_throttle;
		GameSettings.WHEEL_STEER_LEFT = Wheel_steer_left;
		GameSettings.WHEEL_STEER_RIGHT = Wheel_steer_right;
		GameSettings.WHEEL_THROTTLE_DOWN = Wheel_throttle_down;
		GameSettings.WHEEL_THROTTLE_UP = Wheel_throttle_up;
		GameSettings.AXIS_PITCH = axis_pitch;
		GameSettings.AXIS_ROLL = axis_roll;
		GameSettings.AXIS_THROTTLE = axis_throttle;
		GameSettings.AXIS_THROTTLE_INC = axis_throttle_inc;
		GameSettings.AXIS_YAW = axis_yaw;
		GameSettings.AXIS_CAMERA_HDG = axis_camera_hdg;
		GameSettings.AXIS_CAMERA_PITCH = axis_camera_pitch;
		GameSettings.AXIS_TRANSLATE_X = axis_translate_x;
		GameSettings.AXIS_TRANSLATE_Y = axis_translate_y;
		GameSettings.AXIS_TRANSLATE_Z = axis_translate_z;
		GameSettings.axis_EVA_translate_x = axis_EVA_translate_x;
		GameSettings.axis_EVA_translate_y = axis_EVA_translate_y;
		GameSettings.axis_EVA_translate_z = axis_EVA_translate_z;
		GameSettings.axis_EVA_pitch = axis_EVA_pitch;
		GameSettings.axis_EVA_yaw = axis_EVA_yaw;
		GameSettings.axis_EVA_roll = axis_EVA_roll;
		GameSettings.AbortActionGroup = AbortActionGroup;
		GameSettings.CustomActionGroup1 = CustomActionGroup1;
		GameSettings.CustomActionGroup2 = CustomActionGroup2;
		GameSettings.CustomActionGroup3 = CustomActionGroup3;
		GameSettings.CustomActionGroup4 = CustomActionGroup4;
		GameSettings.CustomActionGroup5 = CustomActionGroup5;
		GameSettings.CustomActionGroup6 = CustomActionGroup6;
		GameSettings.CustomActionGroup7 = CustomActionGroup7;
		GameSettings.CustomActionGroup8 = CustomActionGroup8;
		GameSettings.CustomActionGroup9 = CustomActionGroup9;
		GameSettings.CustomActionGroup10 = CustomActionGroup10;
		GameSettings.EVA_ROTATE_ON_MOVE = EVAorientOnMove;
		GameSettings.AXIS_MOUSEWHEEL.primary.scale = mouseWheelSensitivity;
		GameSettings.SPACENAV_FLIGHT_SENS_LIN = spaceNavFltSensLin;
		GameSettings.SPACENAV_FLIGHT_SENS_ROT = spaceNavFltSensRot;
		GameSettings.SPACENAV_CAMERA_SENS_LIN = spaceNavCamSensLin;
		GameSettings.SPACENAV_CAMERA_SENS_ROT = spaceNavCamSensRot;
		GameSettings.SPACENAV_CAMERA_SHARPNESS_LIN = spaceNavCamSharpnessLin;
		GameSettings.SPACENAV_CAMERA_SHARPNESS_ROT = spaceNavCamSharpnessRot;
		GameSettings.TOGGLE_SPACENAV_FLIGHT_CONTROL = SpaceNavigatorFunctionToggle;
		GameSettings.TOGGLE_SPACENAV_ROLL_LOCK = SpaceNavigatorRollLockToggle;
		GameSettings.TRACKIR_ENABLED = trackIRenabled;
		GameSettings.TRACKIR.Instance.activeFlight = trackIRactiveFlight;
		GameSettings.TRACKIR.Instance.activeIVA = trackIRactiveIVA;
		GameSettings.TRACKIR.Instance.activeEVA = trackIRactiveEVA;
		GameSettings.TRACKIR.Instance.activeMap = trackIRactiveMap;
		GameSettings.TRACKIR.Instance.activeKSC = trackIRactiveKSC;
		GameSettings.TRACKIR.Instance.activeTrackingStation = trackIRactiveTrackingStation;
		GameSettings.TRACKIR.Instance.activeEditors = trackIRactiveEditors;
	}

	public string GetName()
	{
		return "Input";
	}
}
