using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	public enum GraphicsType
	{
		D3D9,
		D3D11,
		OGL
	}

	[CompilerGenerated]
	private sealed class _003CWaitAndSetResolution_003Ed__507 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int waitForFrames;

		private int _003CframesAtStart_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CWaitAndSetResolution_003Ed__507(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private static GraphicsType graphicsVersion;

	public string configFilePath;

	public string controlsLayoutPath;

	public string launcherFilePath;

	public bool overrideSettings;

	protected Dictionary<string, ConfigNode> layoutConfigNodes;

	public static string SETTINGS_FILE_VERSION;

	public static string LANGUAGE;

	public static bool TUTORIALS_EDITOR_ENABLE;

	public static bool TUTORIALS_FLIGHT_ENABLE;

	public static bool TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED;

	public static bool TUTORIALS_MISSION_BUILDER_ENTERED;

	public static bool TUTORIALS_ESA_MISSION_SCREEN_TUTORIAL_COMPLETED;

	public static bool VAB_USE_CLICK_PLACE;

	public static bool VAB_USE_ANGLE_SNAP;

	public static bool VAB_ANGLE_SNAP_INCLUDE_VERTICAL;

	public static float VAB_FINE_OFFSET_THRESHOLD;

	public static float VAB_CAMERA_ORBIT_SENS;

	public static float VAB_CAMERA_ZOOM_SENS;

	public static float FLT_CAMERA_ORBIT_SENS;

	public static float FLT_CAMERA_ZOOM_SENS;

	public static float FLT_CAMERA_WOBBLE;

	public static float FLT_CAMERA_CHASE_SHARPNESS;

	public static bool FLT_CAMERA_CHASE_USEVELOCITYVECTOR;

	public static bool FLT_VESSEL_LABELS;

	public static bool ADDITIONAL_ACTION_GROUPS;

	public static bool ADVANCED_TWEAKABLES;

	public static bool ADVANCED_MESSAGESAPP;

	public static bool CONFIRM_MESSAGE_DELETION;

	public static bool AUTOSTRUT_SYMMETRY;

	public static int VAB_CRAFTNAME_CHAR_LIMIT;

	public static int EDITOR_UNDO_REDO_LIMIT;

	public static float SPACENAV_CAMERA_SENS_ROT;

	public static float SPACENAV_CAMERA_SENS_LIN;

	public static float SPACENAV_CAMERA_SHARPNESS_LIN;

	public static float SPACENAV_CAMERA_SHARPNESS_ROT;

	public static bool CAMERA_DOUBLECLICK_MOUSELOOK;

	public static float DOUBLECLICK_MOUSESPEED;

	public static bool IVA_RETAIN_CONTROL_POINT;

	public static float CAMERA_FX_EXTERNAL;

	public static float CAMERA_FX_INTERNAL;

	public static bool SIMULATE_IN_BACKGROUND;

	public static float PHYSICS_FRAME_DT_LIMIT;

	public static int MAX_VESSELS_BUDGET;

	public static bool DECLUTTER_KSC;

	public static int CONIC_PATCH_DRAW_MODE;

	public static int CONIC_PATCH_LIMIT;

	public static bool ALWAYS_SHOW_TARGET_APPROACH_MARKERS;

	public static float ORBIT_FADE_STRENGTH;

	public static bool ORBIT_FADE_DIRECTION_INV;

	public static bool ORBIT_WARP_DOWN_AT_SOI;

	public static bool ORBIT_DRIFT_COMPENSATION;

	public static bool PHYSICS_EASE;

	public static bool LEGACY_ORBIT_TARGETING;

	public static bool SHOW_PWARP_WARNING;

	public static TimeWarp.MaxRailsRateMode ORBIT_WARP_MAXRATE_MODE;

	public static double ORBIT_WARP_PEMODE_SURFACE_MARGIN;

	public static float ORBIT_WARP_ALTMODE_LIMIT_MODIFIER;

	public static bool RADAR_ALTIMETER_EXTENDED_CALCS;

	public static bool EVA_ROTATE_ON_MOVE;

	public static bool EVA_SHOW_PORTRAIT;

	public static bool EVA_DEFAULT_HELMET_ON;

	public static bool EVA_DEFAULT_NECKRING_ON;

	public static bool EVA_DIES_WHEN_UNSAFE_HELMET;

	public static bool EVA_INHERIT_PART_TEMPERATURE;

	public static float EVA_SCREEN_MESSAGE_X;

	public static float EVA_SCREEN_MESSAGE_Y;

	public static bool EVA_LADDER_CHECK_END;

	public static bool EVA_LADDER_JOINT_WHEN_IDLE;

	public static double EVA_LADDER_JOINT_BREAK_VELOCITY;

	public static double EVA_LADDER_JOINT_BREAK_ACCELERATION;

	public static float EVA_MAX_SLOPE_ANGLE;

	public static float EVA_INVENTORY_RANGE;

	public static float EVA_CONSTRUCTION_RANGE;

	public static bool EVA_CONSTRUCTION_COMBINE_ENABLED;

	public static bool EVA_CONSTRUCTION_COMBINE_NONENGINEERS;

	public static float EVA_CONSTRUCTION_COMBINE_RANGE;

	public static float PART_REPAIR_MASS_PER_KIT;

	public static int PART_REPAIR_MAX_KIT_AMOUNT;

	public static float SPACENAV_FLIGHT_SENS_ROT;

	public static float SPACENAV_FLIGHT_SENS_LIN;

	public static bool KERBIN_TIME;

	public static bool SHOW_EXIT_TO_MENU_CONFIRMATION;

	public static bool SHOW_WRONG_VESSEL_TYPE_CONFIRMATION;

	public static bool SHOW_VERSION_WATERMARK;

	public static bool SHOW_ANALYTICS_DIALOG;

	internal static bool SHOW_ANALYTICS_DIALOG_SettingExists;

	public static bool SHOW_WHATSNEW_DIALOG;

	internal static bool SHOW_WHATSNEW_DIALOG_SettingExists;

	public static string SHOW_WHATSNEW_DIALOG_VersionsShown;

	public static bool CALL_HOME_PROMPT;

	public static bool DONT_SEND_IP;

	public static bool SEND_PROGRESS_DATA;

	public static bool CHECK_FOR_UPDATES;

	public static bool VERBOSE_DEBUG_LOG;

	public static bool SHOW_CONSOLE_ON_ERROR;

	public static int CONSOLE_BUFFER_SIZE;

	public static float AUTOSAVE_INTERVAL;

	public static float AUTOSAVE_SHORT_INTERVAL;

	public static int SAVE_BACKUPS;

	public static bool SHOW_SPACE_CENTER_CREW;

	public static int MAP_MAX_ORBIT_BEFORE_FORCE2D;

	public static bool KERBNET_ALIGNS_WITH_ORBIT;

	public static float KERBNET_REFRESH_FAST_INTERVAL;

	public static float KERBNET_REFRESH_SLOW_INTERVAL;

	public static bool KERBNET_BACKGROUND_FLUFF;

	public static float WHEEL_WEIGHT_STRESS_MULTIPLIER;

	public static float WHEEL_SLIP_STRESS_MULTIPLIER;

	public static int WHEEL_SUBSTEPS_ACTIVE;

	public static int WHEEL_SUBSTEPS_INACTIVE;

	public static bool WHEEL_AUTO_SPRINGDAMPER;

	public static bool WHEEL_AUTO_STEERINGADJUST;

	public static bool LEGS_ADVANCED_SUSPENSIONDAMPER;

	public static bool WHEEL_DAMAGE_IMPACTCOLLIDER_ENABLED;

	public static bool WHEEL_DAMAGE_WHEELCOLLIDER_ENABLED;

	public static float UI_SCALE;

	public static float UI_OPACITY;

	public static bool UI_MAINCANVAS_PIXEL_PERFECT;

	public static bool UI_ACTIONCANVAS_PIXEL_PERFECT;

	public static bool UI_TOOLTIPCANVAS_PIXEL_PERFECT;

	public static bool AUTOHIDE_NAVBALL;

	public static bool EXTENDED_BURNTIME;

	public static float WARP_TO_MANNODE_MARGIN;

	public static bool DELTAV_CALCULATIONS_ENABLED;

	public static float DELTAV_BURN_PERCENTAGE;

	public static bool DELTAV_BURN_ESTIMATE_COLORS;

	public static bool DELTAV_BURN_TIME_COLORS;

	public static float DELTAV_ACTIVE_STAGE_UPDATE_SECS;

	public static float DELTAV_ALL_STAGES_UPDATE_SECS;

	public static float DELTAV_VESSEL_EVENT_DELAY_SECS;

	public static float DELTAV_ACTIVE_VESSEL_TIMESTEP;

	public static float DELTAV_CALCULATIONS_TIMESTEP;

	public static float DELTAV_CALCULATIONS_BIGTIMESTEP;

	public static bool DELTAV_USE_TIMED_VESSELCALCS;

	public static bool DELTAV_APP_ENABLED;

	public static bool DELTAV_APP_TWOCOLUMN_MODE;

	public static string STAGE_GROUP_INFO_ITEMS;

	public static int STAGE_GROUP_INFO_WIDTH_EDITOR;

	public static int STAGE_GROUP_INFO_WIDTH_FLIGHT;

	public static float STAGE_GROUP_INFO_NAME_PERCENTAGE;

	public static bool CRAFT_STEAM_UNSUBSCRIBE_WARNING;

	public static bool CONTROLPOINT_VISUALS_ENABLED;

	public static float CONTROLPOINT_ARROWLENGTH;

	public static string CONTROLPOINT_COLOR_FORWARD;

	public static string CONTROLPOINT_COLOR_UP;

	public static string CONTROLPOINT_COLOR_RIGHT;

	internal static Color COLOR_CONTROLPOINT_COLOR_FORWARD;

	internal static Color COLOR_CONTROLPOINT_COLOR_UP;

	internal static Color COLOR_CONTROLPOINT_COLOR_RIGHT;

	public static bool UIELEMENTSCALINGENABLED;

	internal static bool UI_SCALE_DISABLEDMODEANDSTAGE;

	public static float UI_SCALE_TIME;

	public static float UI_SCALE_ALTIMETER;

	public static float UI_SCALE_MAPOPTIONS;

	public static float UI_SCALE_APPS;

	public static float UI_SCALE_STAGINGSTACK;

	public static float UI_SCALE_MODE;

	public static float UI_SCALE_NAVBALL;

	public static float UI_SCALE_CREW;

	public static float UI_POS_NAVBALL;

	public static string UI_COLOR_INACTIVE_TEXT;

	public static string UI_COLOR_ACTIVE_TEXT;

	public static string UI_COLOR_INACTIVE_MINISETTNIGS_TEXT;

	public static float UI_POS_ALTIMETER_SLIDEDOWN_HOVER_HEIGHT;

	public static float MAPNODE_BEHINDBODY_OPACITY;

	public static float COMMNET_LOWCOLOR_BRIGHTNESSFACTOR;

	public static bool SHOW_VESSEL_NAMING_IN_FLIGHT;

	public static int VESSEL_NAMING_PRIORTY_LEVEL_MAX;

	public static int VESSEL_NAMING_PRIORTY_LEVEL_DEFAULT;

	public static bool SCIENCE_EXPERIMENT_SHOW_TRANSFER_WARNING;

	public static bool NAVIGATION_GHOSTING;

	public static float VESSEL_ANCHOR_VELOCITY_THRESHOLD;

	public static float VESSEL_ANCHOR_TIME_THRESHOLD;

	public static float VESSEL_ANCHOR_ANGLE_CHANGE_THRESHOLD;

	public static float VESSEL_ANCHOR_ANGLE_TIME_THRESHOLD;

	public static float VESSEL_ANCHOR_BREAK_FORCE_FACTOR;

	public static float VESSEL_ANCHOR_BREAK_TORQUE;

	public static bool MISSION_SHOW_CREATE_VESSEL_WARNING;

	public static bool MISSION_SHOW_TEST_MISSION_WARNING;

	public static bool MISSION_SHOW_NO_BRIEFING_WARNING;

	public static bool MISSION_STEAM_UNSUBSCRIBE_WARNING;

	public static bool MISSION_SNAP_TO_GRID;

	public static float MISSION_BUILDER_GAPHEIGHT;

	public static bool MISSION_SHOW_STOCK_PACKS_IN_BRIEFING;

	public static bool MISSION_LOG_NODE_ACTIVATIONS;

	public static bool MISSION_SHOW_EXPANSION_INFO;

	public static float MISSION_MINIMUM_CANVAS_ZOOM;

	public static bool MISSION_GAP_CAMERA_VAB_CONTROLS;

	public static bool MISSION_DELETE_REMOVES_IN_PROGRESS_MISSIONS;

	public static bool MISSION_NAVIGATION_GHOSTING;

	public static bool SERENITY_SHOW_EXPANSION_INFO;

	public static float SERENITY_ROCS_VISUAL_SPEED;

	public static bool SERENITY_CONTROLLER_IGNORES_VESSEL;

	public static float PART_HIGHLIGHTER_BRIGHTNESSFACTOR;

	public static string COLOR_PART_HIGHLIGHT;

	public static string COLOR_PART_EDITORATTACHED;

	public static string COLOR_PART_EDITORDETACHED;

	public static string COLOR_PART_ACTIONGROUP_SELECTED;

	public static string COLOR_PART_ACTIONGROUP_HIGHLIGHT;

	public static string COLOR_PART_ROOTTOOL_HIGHLIGHT;

	public static string COLOR_PART_ROOTTOOL_HIGHLIGHTEDGE;

	public static string COLOR_PART_ROOTTOOL_HOVER;

	public static string COLOR_PART_ROOTTOOL_HOVEREDGE;

	public static string COLOR_PART_ENGINEERAPP_HIGHLIGHT;

	public static string COLOR_PART_TRANSFER_SOURCE_HIGHLIGHT;

	public static string COLOR_PART_TRANSFER_SOURCE_HOVER;

	public static string COLOR_PART_TRANSFER_DEST_HIGHLIGHT;

	public static string COLOR_PART_TRANSFER_DEST_HOVER;

	public static string COLOR_PART_INVENTORY_CONTAINER;

	public static string COLOR_PART_INVENTORY_NOSPACE;

	public static string COLOR_PART_CONSTRUCTION_VALID;

	public static string COLOR_RD_SEARCH_NODE_HIGHLIGHT;

	public static string COLOR_RD_SEARCH_PART_HIGHLIGHT;

	public static string COLOR_LIGHT_PRESET_1;

	public static string COLOR_LIGHT_PRESET_2;

	public static string COLOR_LIGHT_PRESET_3;

	public static string COLOR_LIGHT_PRESET_4;

	public static string COLOR_LIGHT_PRESET_5;

	public static string COLOR_FIREWORK_PRESET_GREEN;

	public static string COLOR_FIREWORK_PRESET_LIGHT_BLUE;

	public static string COLOR_FIREWORK_PRESET_BLUE;

	public static string COLOR_FIREWORK_PRESET_PURPLE;

	public static string COLOR_FIREWORK_PRESET_PINK;

	private static List<Color> lightPresetColors;

	private static List<Color> fireworkPresetColors;

	public static int TEMPERATURE_GAUGES_MODE;

	public static float MASTER_VOLUME;

	public static float SHIP_VOLUME;

	public static float AMBIENCE_VOLUME;

	public static float MUSIC_VOLUME;

	public static float UI_VOLUME;

	public static float VOICE_VOLUME;

	public static bool SOUND_NORMALIZER_ENABLED;

	public static float SOUND_NORMALIZER_THRESHOLD;

	public static float SOUND_NORMALIZER_RESPONSIVENESS;

	public static int SOUND_NORMALIZER_SKIPSAMPLES;

	public static int SCREEN_RESOLUTION_WIDTH;

	public static int SCREEN_RESOLUTION_HEIGHT;

	public static bool FULLSCREEN;

	public static bool CELESTIAL_BODIES_CAST_SHADOWS;

	public static int QUALITY_PRESET;

	public static int ANTI_ALIASING;

	public static int TEXTURE_QUALITY;

	public static int SYNC_VBL;

	public static int LIGHT_QUALITY;

	public static int SHADOWS_QUALITY;

	public static int FRAMERATE_LIMIT;

	public static int SHADOWS_FLIGHT_PROJECTION;

	public static int SHADOWS_KSC_PROJECTION;

	public static int SHADOWS_TRACKING_PROJECTION;

	public static int SHADOWS_EDITORS_PROJECTION;

	public static int SHADOWS_MAIN_PROJECTION;

	public static int SHADOWS_DEFAULT_PROJECTION;

	public static float AMBIENTLIGHT_BOOSTFACTOR;

	public static float AMBIENTLIGHT_BOOSTFACTOR_MAPONLY;

	public static float AMBIENTLIGHT_BOOSTFACTOR_EDITONLY;

	public static bool PLANET_SCATTER;

	public static float PLANET_SCATTER_FACTOR;

	public static double WATERLEVEL_BASE_OFFSET;

	public static double WATERLEVEL_MAXLEVEL_MULT;

	public static bool UNSUPPORTED_LEGACY_SHADER_TERRAIN;

	public static int AERO_FX_QUALITY;

	public static bool SURFACE_FX;

	public static bool INFLIGHT_HIGHLIGHT;

	public static bool COMET_REENTRY_FRAGMENT;

	public static int REFLECTION_PROBE_REFRESH_MODE;

	public static int REFLECTION_PROBE_TEXTURE_RESOLUTION;

	public static int TERRAIN_SHADER_QUALITY;

	public static int FALLBACK_UNDERWATER_MODE;

	public static int SCREENSHOT_SUPERSIZE;

	public static bool SHOW_DEADLINES_AS_DATES;

	public static double DEFAULT_KERBAL_RESPAWN_TIMER;

	public static bool HIGHLIGHT_FX;

	public static bool COMET_SHOW_GEYSERS;

	public static int COMET_MAXIMUM_GEYSERS;

	public static bool COMET_SHOW_NEAR_DUST;

	public static int COMET_MAXIMUM_NEAR_DUST_EMITTERS;

	public static float INPUT_KEYBOARD_SENSIVITITY;

	public static float PRELAUNCH_DEFAULT_THROTTLE;

	public static float MIN_DISTANCE_FROM_OTHER_SPLASHES;

	public static float MIN_TIME_BETWEEN_SPLASHES;

	public static string CURRENT_LAYOUT_SETTINGS;

	public static InputDevices INPUT_DEVICES;

	public static float AxisSensitivityMin;

	public static float AxisSensitivityMax;

	public static KeyBinding PITCH_DOWN;

	public static KeyBinding PITCH_UP;

	public static KeyBinding YAW_LEFT;

	public static KeyBinding YAW_RIGHT;

	public static KeyBinding ROLL_LEFT;

	public static KeyBinding ROLL_RIGHT;

	public static KeyBinding THROTTLE_UP;

	public static KeyBinding THROTTLE_DOWN;

	public static KeyBinding SAS_HOLD;

	public static KeyBinding SAS_TOGGLE;

	public static KeyBinding LAUNCH_STAGES;

	public static KeyBinding Docking_toggleRotLin;

	public static KeyBinding CAMERA_MODE;

	public static KeyBinding CAMERA_NEXT;

	public static KeyBinding PAUSE;

	public static KeyBinding PRECISION_CTRL;

	public static KeyBinding ZOOM_IN;

	public static KeyBinding ZOOM_OUT;

	public static KeyBinding SCROLL_VIEW_UP;

	public static KeyBinding SCROLL_VIEW_DOWN;

	public static KeyBinding SCROLL_ICONS_UP;

	public static KeyBinding SCROLL_ICONS_DOWN;

	public static KeyBinding CAMERA_ORBIT_UP;

	public static KeyBinding CAMERA_ORBIT_DOWN;

	public static KeyBinding CAMERA_ORBIT_LEFT;

	public static KeyBinding CAMERA_ORBIT_RIGHT;

	public static KeyBinding CAMERA_RESET;

	public static KeyBinding CAMERA_MOUSE_TOGGLE;

	public static KeyBinding TIME_WARP_INCREASE;

	public static KeyBinding TIME_WARP_DECREASE;

	public static KeyBinding TIME_WARP_STOP;

	public static KeyBinding MAP_VIEW_TOGGLE;

	public static KeyBinding NAVBALL_TOGGLE;

	public static KeyBinding EVA_CONSTRUCTION_MODE_TOGGLE;

	public static KeyBinding UIMODE_STAGING;

	public static KeyBinding UIMODE_DOCKING;

	public static KeyBinding TRANSLATE_DOWN;

	public static KeyBinding TRANSLATE_UP;

	public static KeyBinding TRANSLATE_LEFT;

	public static KeyBinding TRANSLATE_RIGHT;

	public static KeyBinding TRANSLATE_FWD;

	public static KeyBinding TRANSLATE_BACK;

	public static KeyBinding RCS_TOGGLE;

	public static KeyBinding FOCUS_NEXT_VESSEL;

	public static KeyBinding FOCUS_PREV_VESSEL;

	public static KeyBinding TOGGLE_UI;

	public static KeyBinding TOGGLE_STATUS_SCREEN;

	public static KeyBinding TAKE_SCREENSHOT;

	public static KeyBinding TOGGLE_LABELS;

	public static KeyBinding TOGGLE_TEMP_GAUGES;

	public static KeyBinding TOGGLE_TEMP_OVERLAY;

	public static KeyBinding TOGGLE_FLIGHT_FORCES;

	public static KeyBinding QUICKSAVE;

	public static KeyBinding QUICKLOAD;

	public static KeyBinding THROTTLE_CUTOFF;

	public static KeyBinding THROTTLE_FULL;

	public static KeyBinding LANDING_GEAR;

	public static KeyBinding HEADLIGHT_TOGGLE;

	public static KeyBinding BRAKES;

	public static KeyBinding TOGGLE_SPACENAV_FLIGHT_CONTROL;

	public static KeyBinding TOGGLE_SPACENAV_ROLL_LOCK;

	public static KeyBinding WHEEL_STEER_LEFT;

	public static KeyBinding WHEEL_STEER_RIGHT;

	public static KeyBinding WHEEL_THROTTLE_DOWN;

	public static KeyBinding WHEEL_THROTTLE_UP;

	public static KeyBinding EVA_forward;

	public static KeyBinding EVA_back;

	public static KeyBinding EVA_left;

	public static KeyBinding EVA_right;

	public static KeyBinding EVA_yaw_left;

	public static KeyBinding EVA_yaw_right;

	public static KeyBinding EVA_Pack_forward;

	public static KeyBinding EVA_Pack_back;

	public static KeyBinding EVA_Pack_left;

	public static KeyBinding EVA_Pack_right;

	public static KeyBinding EVA_Pack_up;

	public static KeyBinding EVA_Pack_down;

	public static KeyBinding EVA_Jump;

	public static KeyBinding EVA_Run;

	public static KeyBinding EVA_ToggleMovementMode;

	public static KeyBinding EVA_TogglePack;

	public static KeyBinding EVA_Use;

	public static KeyBinding EVA_Board;

	public static KeyBinding EVA_Orient;

	public static KeyBinding EVA_Lights;

	public static KeyBinding EVA_Helmet;

	public static KeyBinding EVA_ChuteDeploy;

	public static KeyBinding Editor_pitchUp;

	public static KeyBinding Editor_pitchDown;

	public static KeyBinding Editor_yawLeft;

	public static KeyBinding Editor_yawRight;

	public static KeyBinding Editor_rollLeft;

	public static KeyBinding Editor_rollRight;

	public static KeyBinding Editor_resetRotation;

	public static KeyBinding Editor_modePlace;

	public static KeyBinding Editor_modeOffset;

	public static KeyBinding Editor_modeRotate;

	public static KeyBinding Editor_modeRoot;

	public static KeyBinding Editor_coordSystem;

	public static KeyBinding Editor_toggleSymMethod;

	public static KeyBinding Editor_toggleSymMode;

	public static KeyBinding Editor_toggleAngleSnap;

	public static KeyBinding Editor_fineTweak;

	public static KeyBinding Editor_partSearch;

	public static KeyBinding Editor_zoomScrollModifier;

	public static AxisBinding AXIS_PITCH;

	public static AxisBinding AXIS_ROLL;

	public static AxisBinding AXIS_YAW;

	public static AxisBinding AXIS_THROTTLE;

	public static AxisBinding AXIS_THROTTLE_INC;

	public static AxisBinding AXIS_CAMERA_HDG;

	public static AxisBinding AXIS_CAMERA_PITCH;

	public static AxisBinding AXIS_TRANSLATE_X;

	public static AxisBinding AXIS_TRANSLATE_Y;

	public static AxisBinding AXIS_TRANSLATE_Z;

	public static AxisBinding AXIS_WHEEL_STEER;

	public static AxisBinding AXIS_WHEEL_THROTTLE;

	public static AxisKeyBindingList AXIS_CUSTOM;

	public static AxisBinding axis_EVA_translate_x;

	public static AxisBinding axis_EVA_translate_y;

	public static AxisBinding axis_EVA_translate_z;

	public static AxisBinding axis_EVA_pitch;

	public static AxisBinding axis_EVA_yaw;

	public static AxisBinding axis_EVA_roll;

	public static AxisBinding AXIS_MOUSEWHEEL;

	public static KeyBinding MODIFIER_KEY;

	public static KeyBinding AbortActionGroup;

	public static KeyBinding CustomActionGroup1;

	public static KeyBinding CustomActionGroup2;

	public static KeyBinding CustomActionGroup3;

	public static KeyBinding CustomActionGroup4;

	public static KeyBinding CustomActionGroup5;

	public static KeyBinding CustomActionGroup6;

	public static KeyBinding CustomActionGroup7;

	public static KeyBinding CustomActionGroup8;

	public static KeyBinding CustomActionGroup9;

	public static KeyBinding CustomActionGroup10;

	public static KeyBinding AGROUP_SELECT_NEXT;

	public static KeyBinding AGROUP_SELECT_PREV;

	public static string AXIS_INCREMENTAL_SPEED_MULTIPLIER_STORAGE;

	public static float AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT;

	public static List<float> AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES;

	public static bool dontShowLauncher;

	public static bool TRACKIR_ENABLED;

	public static TrackIR.Settings TRACKIR;

	public static float FEMALE_EYE_OFFSET_X;

	public static float FEMALE_EYE_OFFSET_Y;

	public static float FEMALE_EYE_OFFSET_Z;

	public static float FEMALE_EYE_OFFSET_SCALE;

	public static bool FI_LOG_TEMP_ERROR;

	public static bool FI_LOG_OVERTEMP;

	public static bool LOG_INSTANT_FLUSH;

	public static bool CAN_ALWAYS_QUICKSAVE;

	public static float QUICKSAVE_MINIMUM_ALTITUDE;

	public static bool LOG_ERRORS_TO_SCREEN;

	public static bool LOG_EXCEPTIONS_TO_SCREEN;

	public static bool LOG_JOINT_BREAK_EVENT;

	public static bool LOG_MISSING_KEYS_TO_FILE;

	public static bool SHOW_TRANSLATION_KEYS_ON_SCREEN;

	public static bool LOG_DELTAV_VERBOSE;

	public static bool LOG_FXMONGER_VERBOSE;

	public static bool COLLECT_ROC_STATS;

	public static double DEBUG_MAX_SETPOSITION_ALTITUDE;

	public static bool DEBUG_AERO_GUI;

	public static bool DEBUG_AERO_DATA_PAWS;

	public static string PAW_COLLAPSED_GROUP_NAMES;

	internal static List<string> collpasedPAWGroups;

	public static bool PAW_NUMERIC_SLIDERS;

	public static float PAW_PREFERRED_HEIGHT;

	public static float PAW_SCREEN_OFFSET_X;

	public static float MANEUVER_TOOL_TRANSFER_DEGREES;

	public static float MANEUVER_TOOL_CALC_TIMEOUT;

	public static float MANEUVER_TOOL_CB_COLLISION_ADJUSTMENT;

	public static bool SHOW_DELETE_ALARM_CONFIRMATION;

	public static bool ALARM_ROW_DISPLAYED_FLIGHT;

	public static ValidatorMode MISSION_VALIDATOR_MODE;

	public static bool MISSION_TEST_AUTOMATIC_CHECKPOINTS;

	private static GameSettings fetch;

	private static bool saveOnGameSave;

	public static GraphicsType GraphicsVersion
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool Ready
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Dictionary<string, ConfigNode> KeyboardLayouts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GameSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetDefaultValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadLayoutKeyBindings(string layoutSettings, bool overrideSettings = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string DetectKeyboardLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadGameSettingsOnly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveGameSettingsOnly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reload Settings")]
	public void ReloadSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Save Settings")]
	public void WriteSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ParseLayoutsCfg(string[] layoutFiles)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode ParseLayoutRecursive(ConfigNode node, string layoutFile, int depth = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WriteCfg()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WriteLauncherCFG()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ParseLauncherCfg()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ParseCfg()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeDefaultValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCollapsedPawGroupNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCollapsedPawGroupString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetHighlighterColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLightPresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFireworkPresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color LoadColor(string colorValues, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Color> GetLightPresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Color> GetFireworkPresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUITextColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetRDSearchHighlightColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool SetHighlighterColor(ref Color c, string colorValues, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCPVisualColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void InitializeAxisGroupbindings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<float> GetAxisIncrementalSpeedMultiplierValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddIncrementalSpeedMultiplierValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetAxisSpeedMultiplier(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetAxiSpeedMultiplierIndex(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyEngineSettings(bool wait = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndSetResolution_003Ed__507))]
	private IEnumerator WaitAndSetResolution(int waitForFrames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSave(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveSettingsOnNextGameSave()
	{
		throw null;
	}
}
