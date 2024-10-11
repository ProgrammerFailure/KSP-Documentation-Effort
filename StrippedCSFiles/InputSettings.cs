using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputSettings : DialogGUIVerticalLayout, ISettings
{
	private enum ListenModes
	{
		OFF,
		KEY_PRIMARY,
		KEY_SECONDARY,
		AXIS_PRIMARY,
		AXIS_SECONDARY
	}

	internal enum InputPages
	{
		FLIGHT,
		VESSEL,
		CHARACTER,
		MISC,
		OTHER
	}

	internal class InputPage : DialogGUIHorizontalLayout
	{
		protected InputSettings parent;

		protected InputPages type;

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal InputPage()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Update()
		{
			throw null;
		}
	}

	internal class FlightInputPage : InputPage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal FlightInputPage(InputSettings parent)
		{
			throw null;
		}
	}

	internal class VesselInputPage : InputPage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal VesselInputPage(InputSettings parent)
		{
			throw null;
		}
	}

	internal class MiscInputPage : InputPage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal MiscInputPage(InputSettings parent)
		{
			throw null;
		}
	}

	internal class KerbalInputPage : InputPage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal KerbalInputPage(InputSettings parent)
		{
			throw null;
		}
	}

	internal class OtherInputPage : InputPage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal OtherInputPage(InputSettings parent)
		{
			throw null;
		}
	}

	private KeyBinding pitchDown;

	private KeyBinding pitchUp;

	private KeyBinding yawLeft;

	private KeyBinding yawRight;

	private KeyBinding rollLeft;

	private KeyBinding rollRight;

	private KeyBinding throttleUp;

	private KeyBinding throttleDown;

	private KeyBinding sasMomentairly;

	private KeyBinding sasToggled;

	private KeyBinding launchStages;

	private KeyBinding cameraMode;

	private KeyBinding cameraNext;

	private KeyBinding cameraReset;

	private KeyBinding pause;

	private KeyBinding precision;

	private KeyBinding scrollViewUp;

	private KeyBinding scrollViewDown;

	private KeyBinding zoomViewIn;

	private KeyBinding zoomViewOut;

	private KeyBinding scrollIconsUp;

	private KeyBinding scrollIconsDown;

	private KeyBinding viewOrbitUp;

	private KeyBinding viewOrbitDown;

	private KeyBinding viewOrbitLeft;

	private KeyBinding viewOrbitRight;

	private KeyBinding timeWarpIncrease;

	private KeyBinding timeWarpDecrease;

	private KeyBinding mapViewToggle;

	private KeyBinding translateUp;

	private KeyBinding translateDown;

	private KeyBinding translateLeft;

	private KeyBinding translateRight;

	private KeyBinding translateFwd;

	private KeyBinding translateBack;

	private KeyBinding rcsToggle;

	private KeyBinding focusNextVessel;

	private KeyBinding focusPrevVessel;

	private KeyBinding toggleUi;

	private KeyBinding toggleStatusScreen;

	private KeyBinding takeScreenshot;

	private KeyBinding toggleLabels;

	private KeyBinding quicksave;

	private KeyBinding quickload;

	private KeyBinding toggleAeroForces;

	private KeyBinding toggleThermalGauges;

	private KeyBinding toggleThermalOverlay;

	private KeyBinding meco;

	private KeyBinding throttleFull;

	private KeyBinding gears;

	private KeyBinding brakes;

	private KeyBinding lights;

	private KeyBinding EVA_forward;

	private KeyBinding EVA_back;

	private KeyBinding EVA_left;

	private KeyBinding EVA_right;

	private KeyBinding EVA_yaw_left;

	private KeyBinding EVA_yaw_right;

	private KeyBinding EVA_Pack_forward;

	private KeyBinding EVA_Pack_back;

	private KeyBinding EVA_Pack_left;

	private KeyBinding EVA_Pack_right;

	private KeyBinding EVA_Pack_up;

	private KeyBinding EVA_Pack_down;

	private KeyBinding EVA_Jump;

	private KeyBinding EVA_Run;

	private KeyBinding EVA_ToggleMovementMode;

	private KeyBinding EVA_TogglePack;

	private KeyBinding EVA_Use;

	private KeyBinding EVA_Board;

	private KeyBinding EVA_Orient;

	private KeyBinding EVA_lights;

	private KeyBinding EVA_Helmet;

	private KeyBinding EVA_ChuteDeploy;

	private KeyBinding Docking_toggleLinRot;

	private KeyBinding Wheel_steer_left;

	private KeyBinding Wheel_steer_right;

	private KeyBinding Wheel_throttle_down;

	private KeyBinding Wheel_throttle_up;

	private AxisBinding axis_wheel_steer;

	private AxisBinding axis_wheel_throttle;

	private KeyBinding AbortActionGroup;

	private KeyBinding CustomActionGroup1;

	private KeyBinding CustomActionGroup2;

	private KeyBinding CustomActionGroup3;

	private KeyBinding CustomActionGroup4;

	private KeyBinding CustomActionGroup5;

	private KeyBinding CustomActionGroup6;

	private KeyBinding CustomActionGroup7;

	private KeyBinding CustomActionGroup8;

	private KeyBinding CustomActionGroup9;

	private KeyBinding CustomActionGroup10;

	private KeyBinding Editor_pitch_up;

	private KeyBinding Editor_pitch_down;

	private KeyBinding Editor_roll_left;

	private KeyBinding Editor_roll_right;

	private KeyBinding Editor_yaw_left;

	private KeyBinding Editor_yaw_right;

	private KeyBinding Editor_reset_rotation;

	private KeyBinding Editor_coordSystem;

	private KeyBinding Editor_symMethod;

	private KeyBinding Editor_symMode;

	private KeyBinding Editor_angleSnap;

	private KeyBinding Editor_modePlace;

	private KeyBinding Editor_modeOffset;

	private KeyBinding Editor_modeRotate;

	private KeyBinding Editor_modeRoot;

	private KeyBinding Editor_partSearch;

	private AxisBinding axis_pitch;

	private AxisBinding axis_yaw;

	private AxisBinding axis_roll;

	private AxisBinding axis_translate_x;

	private AxisBinding axis_translate_y;

	private AxisBinding axis_translate_z;

	private AxisBinding axis_throttle;

	private AxisBinding axis_throttle_inc;

	private AxisBinding axis_EVA_translate_x;

	private AxisBinding axis_EVA_translate_y;

	private AxisBinding axis_EVA_translate_z;

	private AxisBinding axis_EVA_pitch;

	private AxisBinding axis_EVA_yaw;

	private AxisBinding axis_EVA_roll;

	private AxisBinding axis_camera_hdg;

	private AxisBinding axis_camera_pitch;

	private bool EVAorientOnMove;

	private float mouseWheelSensitivity;

	private KeyBinding SpaceNavigatorFunctionToggle;

	private KeyBinding SpaceNavigatorRollLockToggle;

	private float spaceNavCamSensRot;

	private float spaceNavCamSensLin;

	private float spaceNavCamSharpnessRot;

	private float spaceNavCamSharpnessLin;

	private float spaceNavFltSensRot;

	private float spaceNavFltSensLin;

	private bool trackIRenabled;

	private bool trackIRactiveFlight;

	private bool trackIRactiveIVA;

	private bool trackIRactiveEVA;

	private bool trackIRactiveMap;

	private bool trackIRactiveKSC;

	private bool trackIRactiveTrackingStation;

	private bool trackIRactiveEditors;

	private string[] joynames;

	private int deviceCount;

	private int axisCount;

	private float[] initialAxisValues;

	private Event keyEvent;

	private Vector2 KeyScrollPos;

	private Vector2 AxisScrollPos;

	private ListenModes listenMode;

	private KeyBinding keyTarget;

	private KeyBinding potentialKey;

	private AxisBinding axisTarget;

	private AxisBinding potentialAxis;

	private InputBindingModes targetSwitchState;

	private InputBindingModes newSwitchState;

	private string actionTargetName;

	private bool has6DOFDevice;

	internal InputPages inputPage;

	private static KeyCode[] unbindableKeys;

	private static List<KeyCode> keyValues;

	public UISkinDef skin;

	private InputPage[] inputPages;

	private MultiOptionDialog listenModeMODialog;

	private PopupDialog listenModeDialog;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InputSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static InputSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<KeyCode> GetValidKeys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool isEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drawInputPage(InputPages page, InputPage parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drawOtherPage(InputPage parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnListenModeDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] drawListenModeWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDismissListenModeDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase DrawKeyBinding(string name, KeyBinding keyRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase DrawAxisBinding(string name, AxisBinding axisRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ListenToKey(KeyBinding target, bool isPrimary, string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ListenToAxis(AxisBinding target, bool primary, string actionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetAxisValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetKey(KeyCodeExtended key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAxis(string axis, string name, int axisIdx, int deviceIdx, ListenModes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyAxisBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyKeyBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIBase[] DrawMiniSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}
}
