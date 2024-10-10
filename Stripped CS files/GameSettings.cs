using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Expansions.Missions.Editor;
using Highlighting;
using ns11;
using ns2;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	public enum GraphicsType
	{
		D3D9,
		D3D11,
		const_2
	}

	public static GraphicsType graphicsVersion = GraphicsType.D3D9;

	public string configFilePath;

	public string controlsLayoutPath;

	public string launcherFilePath;

	public bool overrideSettings;

	public Dictionary<string, ConfigNode> layoutConfigNodes = new Dictionary<string, ConfigNode>();

	public static string SETTINGS_FILE_VERSION;

	public static string LANGUAGE = "en-us";

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

	public static bool SHOW_ANALYTICS_DIALOG_SettingExists;

	public static bool SHOW_WHATSNEW_DIALOG;

	public static bool SHOW_WHATSNEW_DIALOG_SettingExists;

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

	public static Color COLOR_CONTROLPOINT_COLOR_FORWARD;

	public static Color COLOR_CONTROLPOINT_COLOR_UP;

	public static Color COLOR_CONTROLPOINT_COLOR_RIGHT;

	public static bool UIELEMENTSCALINGENABLED;

	public static bool UI_SCALE_DISABLEDMODEANDSTAGE;

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

	public static List<Color> lightPresetColors;

	public static List<Color> fireworkPresetColors;

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

	public static List<string> collpasedPAWGroups;

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

	public static GameSettings fetch;

	public static bool saveOnGameSave;

	public static GraphicsType GraphicsVersion => graphicsVersion;

	public static bool Ready => fetch != null;

	public static Dictionary<string, ConfigNode> KeyboardLayouts => fetch.layoutConfigNodes;

	public static void SetDefaultValues()
	{
		SETTINGS_FILE_VERSION = "1.3.0";
		TUTORIALS_EDITOR_ENABLE = false;
		TUTORIALS_FLIGHT_ENABLE = false;
		TUTORIALS_MISSION_SCREEN_TUTORIAL_COMPLETED = false;
		TUTORIALS_MISSION_BUILDER_ENTERED = false;
		TUTORIALS_ESA_MISSION_SCREEN_TUTORIAL_COMPLETED = false;
		MISSION_BUILDER_GAPHEIGHT = 400f;
		VAB_USE_CLICK_PLACE = true;
		VAB_USE_ANGLE_SNAP = false;
		VAB_ANGLE_SNAP_INCLUDE_VERTICAL = false;
		VAB_FINE_OFFSET_THRESHOLD = 20f;
		VAB_CAMERA_ORBIT_SENS = 0.04f;
		VAB_CAMERA_ZOOM_SENS = 0.1f;
		FLT_CAMERA_ORBIT_SENS = 0.04f;
		FLT_CAMERA_ZOOM_SENS = 0.5f;
		FLT_CAMERA_WOBBLE = 0.1f;
		FLT_CAMERA_CHASE_SHARPNESS = 1.5f;
		FLT_CAMERA_CHASE_USEVELOCITYVECTOR = true;
		FLT_VESSEL_LABELS = true;
		ADDITIONAL_ACTION_GROUPS = false;
		ADVANCED_TWEAKABLES = false;
		ADVANCED_MESSAGESAPP = true;
		CONFIRM_MESSAGE_DELETION = true;
		AUTOSTRUT_SYMMETRY = true;
		VAB_CRAFTNAME_CHAR_LIMIT = 128;
		EDITOR_UNDO_REDO_LIMIT = 32;
		SPACENAV_CAMERA_SENS_ROT = 30f;
		SPACENAV_CAMERA_SENS_LIN = 20f;
		SPACENAV_CAMERA_SHARPNESS_LIN = 8f;
		SPACENAV_CAMERA_SHARPNESS_ROT = 10f;
		CAMERA_DOUBLECLICK_MOUSELOOK = false;
		DOUBLECLICK_MOUSESPEED = 0.2f;
		IVA_RETAIN_CONTROL_POINT = false;
		CAMERA_FX_EXTERNAL = 1f;
		CAMERA_FX_INTERNAL = 1f;
		SIMULATE_IN_BACKGROUND = true;
		PHYSICS_FRAME_DT_LIMIT = 0.04f;
		MAX_VESSELS_BUDGET = 250;
		DECLUTTER_KSC = true;
		CONIC_PATCH_DRAW_MODE = 3;
		CONIC_PATCH_LIMIT = 3;
		ALWAYS_SHOW_TARGET_APPROACH_MARKERS = false;
		ORBIT_FADE_STRENGTH = 1f;
		ORBIT_FADE_DIRECTION_INV = false;
		ORBIT_WARP_DOWN_AT_SOI = true;
		ORBIT_DRIFT_COMPENSATION = true;
		PHYSICS_EASE = true;
		LEGACY_ORBIT_TARGETING = false;
		SHOW_PWARP_WARNING = true;
		RADAR_ALTIMETER_EXTENDED_CALCS = true;
		ORBIT_WARP_MAXRATE_MODE = TimeWarp.MaxRailsRateMode.PeAltitude;
		ORBIT_WARP_PEMODE_SURFACE_MARGIN = 250.0;
		ORBIT_WARP_ALTMODE_LIMIT_MODIFIER = 1f;
		EVA_ROTATE_ON_MOVE = true;
		EVA_SHOW_PORTRAIT = true;
		EVA_DEFAULT_HELMET_ON = true;
		EVA_DEFAULT_NECKRING_ON = true;
		EVA_DIES_WHEN_UNSAFE_HELMET = true;
		EVA_INHERIT_PART_TEMPERATURE = false;
		EVA_LADDER_CHECK_END = true;
		EVA_LADDER_JOINT_WHEN_IDLE = true;
		EVA_LADDER_JOINT_BREAK_VELOCITY = 100.0;
		EVA_LADDER_JOINT_BREAK_ACCELERATION = 12.0;
		EVA_MAX_SLOPE_ANGLE = 45f;
		EVA_INVENTORY_RANGE = 5f;
		EVA_CONSTRUCTION_RANGE = 7f;
		EVA_CONSTRUCTION_COMBINE_ENABLED = true;
		EVA_CONSTRUCTION_COMBINE_NONENGINEERS = true;
		EVA_CONSTRUCTION_COMBINE_RANGE = 7f;
		PART_REPAIR_MASS_PER_KIT = 0.05f;
		PART_REPAIR_MAX_KIT_AMOUNT = 4;
		EVA_SCREEN_MESSAGE_X = 0f;
		EVA_SCREEN_MESSAGE_Y = 200f;
		SPACENAV_FLIGHT_SENS_ROT = 5f;
		SPACENAV_FLIGHT_SENS_LIN = 1f;
		KERBIN_TIME = true;
		SHOW_EXIT_TO_MENU_CONFIRMATION = true;
		SHOW_WRONG_VESSEL_TYPE_CONFIRMATION = true;
		SHOW_VERSION_WATERMARK = false;
		SHOW_ANALYTICS_DIALOG = true;
		SHOW_ANALYTICS_DIALOG_SettingExists = false;
		SHOW_WHATSNEW_DIALOG = true;
		SHOW_WHATSNEW_DIALOG_SettingExists = false;
		SHOW_WHATSNEW_DIALOG_VersionsShown = "";
		CALL_HOME_PROMPT = false;
		DONT_SEND_IP = false;
		SEND_PROGRESS_DATA = false;
		CHECK_FOR_UPDATES = true;
		VERBOSE_DEBUG_LOG = false;
		SHOW_CONSOLE_ON_ERROR = false;
		CONSOLE_BUFFER_SIZE = 128;
		AUTOSAVE_INTERVAL = 300f;
		AUTOSAVE_SHORT_INTERVAL = 30f;
		SAVE_BACKUPS = 5;
		SHOW_SPACE_CENTER_CREW = true;
		MAP_MAX_ORBIT_BEFORE_FORCE2D = 150;
		KERBNET_ALIGNS_WITH_ORBIT = true;
		KERBNET_REFRESH_FAST_INTERVAL = 3.5f;
		KERBNET_REFRESH_SLOW_INTERVAL = 7f;
		KERBNET_BACKGROUND_FLUFF = true;
		WHEEL_WEIGHT_STRESS_MULTIPLIER = 1f;
		WHEEL_SLIP_STRESS_MULTIPLIER = 1f;
		WHEEL_SUBSTEPS_ACTIVE = 8;
		WHEEL_SUBSTEPS_INACTIVE = 4;
		WHEEL_AUTO_SPRINGDAMPER = true;
		WHEEL_AUTO_STEERINGADJUST = true;
		LEGS_ADVANCED_SUSPENSIONDAMPER = true;
		WHEEL_DAMAGE_IMPACTCOLLIDER_ENABLED = true;
		WHEEL_DAMAGE_WHEELCOLLIDER_ENABLED = true;
		CRAFT_STEAM_UNSUBSCRIBE_WARNING = true;
		UI_SCALE = 1f;
		UI_OPACITY = 0.5f;
		UI_MAINCANVAS_PIXEL_PERFECT = false;
		UI_ACTIONCANVAS_PIXEL_PERFECT = false;
		UI_TOOLTIPCANVAS_PIXEL_PERFECT = false;
		AUTOHIDE_NAVBALL = false;
		EXTENDED_BURNTIME = false;
		WARP_TO_MANNODE_MARGIN = 60f;
		DELTAV_CALCULATIONS_ENABLED = true;
		DELTAV_BURN_PERCENTAGE = 0.5f;
		DELTAV_BURN_ESTIMATE_COLORS = true;
		DELTAV_BURN_TIME_COLORS = true;
		DELTAV_USE_TIMED_VESSELCALCS = false;
		DELTAV_ACTIVE_STAGE_UPDATE_SECS = 0.2f;
		DELTAV_ALL_STAGES_UPDATE_SECS = 4f;
		DELTAV_VESSEL_EVENT_DELAY_SECS = 1f;
		DELTAV_ACTIVE_VESSEL_TIMESTEP = 5f;
		DELTAV_CALCULATIONS_TIMESTEP = 5f;
		DELTAV_CALCULATIONS_BIGTIMESTEP = 100f;
		DELTAV_APP_ENABLED = true;
		DELTAV_APP_TWOCOLUMN_MODE = false;
		STAGE_GROUP_INFO_ITEMS = "ISP,THRUST,TWR,BURNTIME";
		STAGE_GROUP_INFO_WIDTH_EDITOR = 100;
		STAGE_GROUP_INFO_WIDTH_FLIGHT = 120;
		STAGE_GROUP_INFO_NAME_PERCENTAGE = 0.5f;
		MAPNODE_BEHINDBODY_OPACITY = 0.5f;
		CONTROLPOINT_VISUALS_ENABLED = false;
		CONTROLPOINT_ARROWLENGTH = 2.5f;
		CONTROLPOINT_COLOR_FORWARD = "0.125, 0.125, 0.86, 1.0";
		CONTROLPOINT_COLOR_UP = "0.125, 0.86, 0.125, 1.0";
		CONTROLPOINT_COLOR_RIGHT = "0.86, 0.125, 0.125, 1.0";
		UIELEMENTSCALINGENABLED = true;
		UI_SCALE_DISABLEDMODEANDSTAGE = false;
		UI_SCALE_TIME = 1f;
		UI_SCALE_ALTIMETER = 1f;
		UI_SCALE_MAPOPTIONS = 1f;
		UI_SCALE_APPS = 1f;
		UI_SCALE_STAGINGSTACK = 1f;
		UI_SCALE_MODE = 1f;
		UI_SCALE_NAVBALL = 1f;
		UI_SCALE_CREW = 1f;
		UI_POS_NAVBALL = 0f;
		UI_POS_ALTIMETER_SLIDEDOWN_HOVER_HEIGHT = 15f;
		UI_COLOR_INACTIVE_TEXT = "0.901, 0.901, 0.901, 1.0";
		UI_COLOR_ACTIVE_TEXT = "0.890, 0.890, 0.890, 1.0";
		UI_COLOR_INACTIVE_MINISETTNIGS_TEXT = "0.749, 1.0, 0.0, 1.0";
		COMMNET_LOWCOLOR_BRIGHTNESSFACTOR = 0.5f;
		SHOW_VESSEL_NAMING_IN_FLIGHT = true;
		VESSEL_NAMING_PRIORTY_LEVEL_MAX = 20;
		VESSEL_NAMING_PRIORTY_LEVEL_DEFAULT = 10;
		SCIENCE_EXPERIMENT_SHOW_TRANSFER_WARNING = true;
		NAVIGATION_GHOSTING = false;
		VESSEL_ANCHOR_VELOCITY_THRESHOLD = 0.05f;
		VESSEL_ANCHOR_TIME_THRESHOLD = 1.5f;
		VESSEL_ANCHOR_ANGLE_CHANGE_THRESHOLD = 20f;
		VESSEL_ANCHOR_ANGLE_TIME_THRESHOLD = 0.1f;
		VESSEL_ANCHOR_BREAK_FORCE_FACTOR = 1.05f;
		VESSEL_ANCHOR_BREAK_TORQUE = 100f;
		MISSION_SHOW_CREATE_VESSEL_WARNING = true;
		MISSION_SHOW_TEST_MISSION_WARNING = true;
		MISSION_SHOW_NO_BRIEFING_WARNING = true;
		MISSION_STEAM_UNSUBSCRIBE_WARNING = true;
		MISSION_SNAP_TO_GRID = false;
		MISSION_SHOW_STOCK_PACKS_IN_BRIEFING = false;
		MISSION_LOG_NODE_ACTIVATIONS = true;
		MISSION_SHOW_EXPANSION_INFO = true;
		MISSION_MINIMUM_CANVAS_ZOOM = 0.2f;
		MISSION_GAP_CAMERA_VAB_CONTROLS = true;
		MISSION_DELETE_REMOVES_IN_PROGRESS_MISSIONS = true;
		MISSION_NAVIGATION_GHOSTING = true;
		SERENITY_SHOW_EXPANSION_INFO = true;
		SERENITY_ROCS_VISUAL_SPEED = 500f;
		SERENITY_CONTROLLER_IGNORES_VESSEL = false;
		PART_HIGHLIGHTER_BRIGHTNESSFACTOR = 1f;
		COLOR_PART_HIGHLIGHT = "0, 1.0, 0, 1.0";
		COLOR_PART_EDITORATTACHED = "0, 1.0, 0, 1.0";
		COLOR_PART_EDITORDETACHED = "1.0, 0, 0, 1.0";
		COLOR_PART_ACTIONGROUP_SELECTED = "0, 0, 1.0, 1.0";
		COLOR_PART_ACTIONGROUP_HIGHLIGHT = "0, 0, 1.0, 1.0";
		COLOR_PART_ROOTTOOL_HIGHLIGHT = "0.03921569, 0.5333334, 0.5411765, 0.3";
		COLOR_PART_ROOTTOOL_HIGHLIGHTEDGE = "0, 1.0, 1.0, 0.5";
		COLOR_PART_ROOTTOOL_HOVER = "0.254902, 0.9921569, 0.9960784, 1.0";
		COLOR_PART_ROOTTOOL_HOVEREDGE = "0.04313726, 0.9764706, 0.9176471, 1.0";
		COLOR_PART_ENGINEERAPP_HIGHLIGHT = "1.0, 0, 0, 1.0";
		COLOR_PART_TRANSFER_SOURCE_HIGHLIGHT = "0.7764706, 0.3176471, 0.007843138, 1.0";
		COLOR_PART_TRANSFER_SOURCE_HOVER = "1.0, 0.694, 0, 1.0";
		COLOR_PART_TRANSFER_DEST_HIGHLIGHT = "0.5490196, 1.0, 0.8588235, 1.0";
		COLOR_PART_TRANSFER_DEST_HOVER = "0.04313726, 0.9764706, 0.9176471, 1.0";
		COLOR_PART_INVENTORY_CONTAINER = "0.254902, 0.9921569, 0.9960784, 1.0";
		COLOR_PART_INVENTORY_NOSPACE = "1.0, 1.0, 0, 1.0";
		COLOR_PART_CONSTRUCTION_VALID = "0, 1.0, 0, 1.0";
		COLOR_RD_SEARCH_NODE_HIGHLIGHT = "1.0, 1.0, 0.25, 0.6";
		COLOR_RD_SEARCH_PART_HIGHLIGHT = "1.0, 1.0, 0.25, 0.6";
		COLOR_LIGHT_PRESET_1 = "1.0, 1.0, 1.0, 1.0";
		COLOR_LIGHT_PRESET_2 = "0.9333, 0.0039, 0.1215, 1.0 ";
		COLOR_LIGHT_PRESET_3 = "0.0352, 0.8666, 0.0392, 1.0 ";
		COLOR_LIGHT_PRESET_4 = "1.0, 0.9294, 0.0, 1.0 ";
		COLOR_LIGHT_PRESET_5 = "0, 0.6039, 1.0, 1.0 ";
		COLOR_FIREWORK_PRESET_GREEN = "0.317, 0.831, 0.1607, 1";
		COLOR_FIREWORK_PRESET_LIGHT_BLUE = "0.145, 0.752, 0.886, 1";
		COLOR_FIREWORK_PRESET_BLUE = "0.2, 0.454, 0.9803, 1";
		COLOR_FIREWORK_PRESET_PURPLE = "0.478, 0.396, 0.9607, 1";
		COLOR_FIREWORK_PRESET_PINK = "0.831, 0.235, 0.8509, 1";
		TEMPERATURE_GAUGES_MODE = 3;
		MASTER_VOLUME = 0.5f;
		SHIP_VOLUME = 0.5f;
		AMBIENCE_VOLUME = 0.5f;
		MUSIC_VOLUME = 0.35f;
		UI_VOLUME = 0.5f;
		VOICE_VOLUME = 0.5f;
		SOUND_NORMALIZER_ENABLED = true;
		SOUND_NORMALIZER_THRESHOLD = 1f;
		SOUND_NORMALIZER_RESPONSIVENESS = 16f;
		SOUND_NORMALIZER_SKIPSAMPLES = 0;
		SCREEN_RESOLUTION_WIDTH = 1280;
		SCREEN_RESOLUTION_HEIGHT = 720;
		FULLSCREEN = false;
		CELESTIAL_BODIES_CAST_SHADOWS = true;
		QUALITY_PRESET = 5;
		ANTI_ALIASING = 2;
		TEXTURE_QUALITY = 1;
		SYNC_VBL = 1;
		LIGHT_QUALITY = 8;
		SHADOWS_QUALITY = 4;
		FRAMERATE_LIMIT = 120;
		SHADOWS_FLIGHT_PROJECTION = 0;
		SHADOWS_KSC_PROJECTION = 0;
		SHADOWS_TRACKING_PROJECTION = 0;
		SHADOWS_EDITORS_PROJECTION = 1;
		SHADOWS_MAIN_PROJECTION = 1;
		SHADOWS_DEFAULT_PROJECTION = 0;
		AMBIENTLIGHT_BOOSTFACTOR = 0f;
		AMBIENTLIGHT_BOOSTFACTOR_MAPONLY = 0f;
		AMBIENTLIGHT_BOOSTFACTOR_EDITONLY = 0f;
		PLANET_SCATTER = false;
		PLANET_SCATTER_FACTOR = 0.5f;
		WATERLEVEL_BASE_OFFSET = 1.5;
		WATERLEVEL_MAXLEVEL_MULT = 0.1;
		UNSUPPORTED_LEGACY_SHADER_TERRAIN = false;
		AERO_FX_QUALITY = 3;
		SURFACE_FX = true;
		INFLIGHT_HIGHLIGHT = true;
		REFLECTION_PROBE_REFRESH_MODE = 0;
		REFLECTION_PROBE_TEXTURE_RESOLUTION = 1;
		COMET_REENTRY_FRAGMENT = true;
		FALLBACK_UNDERWATER_MODE = 1;
		SCREENSHOT_SUPERSIZE = 0;
		SHOW_DEADLINES_AS_DATES = false;
		DEFAULT_KERBAL_RESPAWN_TIMER = 7200.0;
		HIGHLIGHT_FX = true;
		COMET_SHOW_GEYSERS = true;
		COMET_MAXIMUM_GEYSERS = 50;
		COMET_MAXIMUM_NEAR_DUST_EMITTERS = 50;
		COMET_SHOW_NEAR_DUST = true;
		MIN_DISTANCE_FROM_OTHER_SPLASHES = 1.5f;
		MIN_TIME_BETWEEN_SPLASHES = 0.025f;
		INPUT_KEYBOARD_SENSIVITITY = 2f;
		PRELAUNCH_DEFAULT_THROTTLE = 0f;
		AxisSensitivityMin = 0.25f;
		AxisSensitivityMax = 5f;
		PITCH_DOWN = new KeyBinding(KeyCode.W, InputBindingModes.RotationModes);
		PITCH_UP = new KeyBinding(KeyCode.S, InputBindingModes.RotationModes);
		YAW_LEFT = new KeyBinding(KeyCode.A, InputBindingModes.RotationModes);
		YAW_RIGHT = new KeyBinding(KeyCode.D, InputBindingModes.RotationModes);
		ROLL_LEFT = new KeyBinding(KeyCode.Q);
		ROLL_RIGHT = new KeyBinding(KeyCode.E);
		THROTTLE_UP = new KeyBinding(KeyCode.LeftShift, InputBindingModes.Staging);
		THROTTLE_DOWN = new KeyBinding(KeyCode.LeftControl, InputBindingModes.Staging);
		SAS_HOLD = new KeyBinding(KeyCode.F);
		SAS_TOGGLE = new KeyBinding(KeyCode.T);
		LAUNCH_STAGES = new KeyBinding(KeyCode.Space, InputBindingModes.Staging);
		Docking_toggleRotLin = new KeyBinding(KeyCode.Space, InputBindingModes.DockingModes);
		CAMERA_MODE = new KeyBinding(KeyCode.C);
		CAMERA_NEXT = new KeyBinding(KeyCode.V);
		PAUSE = new KeyBinding(KeyCode.Escape);
		PRECISION_CTRL = new KeyBinding(KeyCode.CapsLock);
		ZOOM_IN = new KeyBinding(KeyCode.KeypadPlus);
		ZOOM_OUT = new KeyBinding(KeyCode.KeypadMinus);
		SCROLL_VIEW_UP = new KeyBinding(KeyCode.PageUp);
		SCROLL_VIEW_DOWN = new KeyBinding(KeyCode.PageDown);
		SCROLL_ICONS_UP = new KeyBinding(KeyCode.Home);
		SCROLL_ICONS_DOWN = new KeyBinding(KeyCode.End);
		CAMERA_ORBIT_UP = new KeyBinding(KeyCode.UpArrow);
		CAMERA_ORBIT_DOWN = new KeyBinding(KeyCode.DownArrow);
		CAMERA_ORBIT_LEFT = new KeyBinding(KeyCode.LeftArrow);
		CAMERA_ORBIT_RIGHT = new KeyBinding(KeyCode.RightArrow);
		CAMERA_RESET = new KeyBinding(KeyCode.BackQuote);
		CAMERA_MOUSE_TOGGLE = new KeyBinding(KeyCode.Backslash);
		TIME_WARP_INCREASE = new KeyBinding(KeyCode.Period);
		TIME_WARP_DECREASE = new KeyBinding(KeyCode.Comma);
		TIME_WARP_STOP = new KeyBinding(KeyCode.Slash);
		MAP_VIEW_TOGGLE = new KeyBinding(KeyCode.M);
		NAVBALL_TOGGLE = new KeyBinding(KeyCode.KeypadPeriod);
		EVA_CONSTRUCTION_MODE_TOGGLE = new KeyBinding(KeyCode.I, ControlTypes.EVA_INPUT);
		UIMODE_STAGING = new KeyBinding(KeyCode.Insert);
		UIMODE_DOCKING = new KeyBinding(KeyCode.Delete);
		TRANSLATE_DOWN = new KeyBinding(KeyCode.I, KeyCode.W, InputBindingModes.Any, InputBindingModes.Docking_Translation);
		TRANSLATE_UP = new KeyBinding(KeyCode.K, KeyCode.S, InputBindingModes.Any, InputBindingModes.Docking_Translation);
		TRANSLATE_LEFT = new KeyBinding(KeyCode.J, KeyCode.A, InputBindingModes.Any, InputBindingModes.Docking_Translation);
		TRANSLATE_RIGHT = new KeyBinding(KeyCode.L, KeyCode.D, InputBindingModes.Any, InputBindingModes.Docking_Translation);
		TRANSLATE_FWD = new KeyBinding(KeyCode.H, KeyCode.LeftShift, InputBindingModes.Any, InputBindingModes.DockingModes);
		TRANSLATE_BACK = new KeyBinding(KeyCode.N, KeyCode.LeftControl, InputBindingModes.Any, InputBindingModes.DockingModes);
		RCS_TOGGLE = new KeyBinding(KeyCode.R);
		FOCUS_NEXT_VESSEL = new KeyBinding(KeyCode.RightBracket);
		FOCUS_PREV_VESSEL = new KeyBinding(KeyCode.LeftBracket);
		TOGGLE_UI = new KeyBinding(KeyCode.F2);
		TOGGLE_STATUS_SCREEN = new KeyBinding(KeyCode.F3);
		TAKE_SCREENSHOT = new KeyBinding(KeyCode.F1);
		TOGGLE_LABELS = new KeyBinding(KeyCode.F4);
		TOGGLE_TEMP_GAUGES = new KeyBinding(KeyCode.F10);
		TOGGLE_TEMP_OVERLAY = new KeyBinding(KeyCode.F11);
		TOGGLE_FLIGHT_FORCES = new KeyBinding(KeyCode.F12);
		QUICKSAVE = new KeyBinding(KeyCode.F5);
		QUICKLOAD = new KeyBinding(KeyCode.F9);
		THROTTLE_CUTOFF = new KeyBinding(KeyCode.X);
		THROTTLE_FULL = new KeyBinding(KeyCode.Z);
		LANDING_GEAR = new KeyBinding(KeyCode.G);
		HEADLIGHT_TOGGLE = new KeyBinding(KeyCode.U);
		BRAKES = new KeyBinding(KeyCode.B);
		TOGGLE_SPACENAV_FLIGHT_CONTROL = new KeyBinding(KeyCode.ScrollLock);
		TOGGLE_SPACENAV_ROLL_LOCK = new KeyBinding(KeyCode.None);
		WHEEL_STEER_LEFT = new KeyBinding(KeyCode.A, InputBindingModes.RotationModes);
		WHEEL_STEER_RIGHT = new KeyBinding(KeyCode.D, InputBindingModes.RotationModes);
		WHEEL_THROTTLE_DOWN = new KeyBinding(KeyCode.S, InputBindingModes.RotationModes);
		WHEEL_THROTTLE_UP = new KeyBinding(KeyCode.W, InputBindingModes.RotationModes);
		EVA_forward = new KeyBinding(KeyCode.W, ControlTypes.EVA_INPUT);
		EVA_back = new KeyBinding(KeyCode.S, ControlTypes.EVA_INPUT);
		EVA_left = new KeyBinding(KeyCode.A, ControlTypes.EVA_INPUT);
		EVA_right = new KeyBinding(KeyCode.D, ControlTypes.EVA_INPUT);
		EVA_yaw_left = new KeyBinding(KeyCode.Q, ControlTypes.EVA_INPUT);
		EVA_yaw_right = new KeyBinding(KeyCode.E, ControlTypes.EVA_INPUT);
		EVA_Pack_forward = new KeyBinding(KeyCode.W, ControlTypes.EVA_INPUT);
		EVA_Pack_back = new KeyBinding(KeyCode.S, ControlTypes.EVA_INPUT);
		EVA_Pack_left = new KeyBinding(KeyCode.A, ControlTypes.EVA_INPUT);
		EVA_Pack_right = new KeyBinding(KeyCode.D, ControlTypes.EVA_INPUT);
		EVA_Pack_up = new KeyBinding(KeyCode.LeftShift, ControlTypes.EVA_INPUT);
		EVA_Pack_down = new KeyBinding(KeyCode.LeftControl, ControlTypes.EVA_INPUT);
		EVA_Jump = new KeyBinding(KeyCode.Space, ControlTypes.EVA_INPUT);
		EVA_Run = new KeyBinding(KeyCode.LeftShift, ControlTypes.EVA_INPUT);
		EVA_ToggleMovementMode = new KeyBinding(KeyCode.LeftAlt, ControlTypes.EVA_INPUT);
		EVA_TogglePack = new KeyBinding(KeyCode.R, ControlTypes.EVA_INPUT);
		EVA_Use = new KeyBinding(KeyCode.F, ControlTypes.EVA_INPUT);
		EVA_Board = new KeyBinding(KeyCode.B, ControlTypes.EVA_INPUT);
		EVA_Orient = new KeyBinding(KeyCode.Space, ControlTypes.EVA_INPUT);
		EVA_Lights = new KeyBinding(KeyCode.U, ControlTypes.EVA_INPUT);
		EVA_Helmet = new KeyBinding(KeyCode.O, ControlTypes.EVA_INPUT);
		EVA_ChuteDeploy = new KeyBinding(KeyCode.P, ControlTypes.EVA_INPUT);
		Editor_pitchUp = new KeyBinding(KeyCode.S);
		Editor_pitchDown = new KeyBinding(KeyCode.W);
		Editor_yawLeft = new KeyBinding(KeyCode.A);
		Editor_yawRight = new KeyBinding(KeyCode.D);
		Editor_rollLeft = new KeyBinding(KeyCode.Q);
		Editor_rollRight = new KeyBinding(KeyCode.E);
		Editor_resetRotation = new KeyBinding(KeyCode.Space);
		Editor_modePlace = new KeyBinding(KeyCode.Alpha1);
		Editor_modeOffset = new KeyBinding(KeyCode.Alpha2);
		Editor_modeRotate = new KeyBinding(KeyCode.Alpha3);
		Editor_modeRoot = new KeyBinding(KeyCode.Alpha4);
		Editor_coordSystem = new KeyBinding(KeyCode.F);
		Editor_toggleSymMethod = new KeyBinding(KeyCode.R);
		Editor_toggleSymMode = new KeyBinding(KeyCode.X);
		Editor_toggleAngleSnap = new KeyBinding(KeyCode.C);
		Editor_fineTweak = new KeyBinding(KeyCode.LeftShift);
		Editor_partSearch = new KeyBinding(KeyCode.BackQuote);
		Editor_zoomScrollModifier = new KeyBinding(KeyCode.LeftShift, KeyCode.RightShift);
		AXIS_PITCH = new AxisBinding();
		AXIS_ROLL = new AxisBinding();
		AXIS_YAW = new AxisBinding();
		AXIS_THROTTLE = new AxisBinding(-1f);
		AXIS_THROTTLE_INC = new AxisBinding();
		AXIS_CAMERA_HDG = new AxisBinding();
		AXIS_CAMERA_PITCH = new AxisBinding();
		AXIS_TRANSLATE_X = new AxisBinding();
		AXIS_TRANSLATE_Y = new AxisBinding();
		AXIS_TRANSLATE_Z = new AxisBinding();
		AXIS_WHEEL_STEER = new AxisBinding();
		AXIS_WHEEL_THROTTLE = new AxisBinding();
		AXIS_CUSTOM = new AxisKeyBindingList(4);
		axis_EVA_translate_x = new AxisBinding();
		axis_EVA_translate_y = new AxisBinding();
		axis_EVA_translate_z = new AxisBinding();
		axis_EVA_pitch = new AxisBinding();
		axis_EVA_yaw = new AxisBinding();
		axis_EVA_roll = new AxisBinding();
		AXIS_MOUSEWHEEL = new AxisBinding("Mouse ScrollWheel", "Mouse ScrollWheel", isInverted: false, 1f, 0f, 1f);
		MODIFIER_KEY = new KeyBinding(KeyCode.LeftAlt, KeyCode.RightAlt);
		AbortActionGroup = new KeyBinding(KeyCode.Backspace);
		CustomActionGroup1 = new KeyBinding(KeyCode.Alpha1);
		CustomActionGroup2 = new KeyBinding(KeyCode.Alpha2);
		CustomActionGroup3 = new KeyBinding(KeyCode.Alpha3);
		CustomActionGroup4 = new KeyBinding(KeyCode.Alpha4);
		CustomActionGroup5 = new KeyBinding(KeyCode.Alpha5);
		CustomActionGroup6 = new KeyBinding(KeyCode.Alpha6);
		CustomActionGroup7 = new KeyBinding(KeyCode.Alpha7);
		CustomActionGroup8 = new KeyBinding(KeyCode.Alpha8);
		CustomActionGroup9 = new KeyBinding(KeyCode.Alpha9);
		CustomActionGroup10 = new KeyBinding(KeyCode.Alpha0);
		AGROUP_SELECT_NEXT = new KeyBinding(KeyCode.F7);
		AGROUP_SELECT_PREV = new KeyBinding(KeyCode.F6);
		AXIS_INCREMENTAL_SPEED_MULTIPLIER_STORAGE = "0.01,0.02,0.0333,0.05,0.0666,0.08,0.10,0.125,0.15,0.2,0.25,0.333,0.4,0.5,0.666,1,2,3";
		AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT = 0.2f;
		dontShowLauncher = false;
		TRACKIR_ENABLED = false;
		TRACKIR = new TrackIR.Settings();
		FEMALE_EYE_OFFSET_X = 0f;
		FEMALE_EYE_OFFSET_Y = 0.009083331f;
		FEMALE_EYE_OFFSET_Z = 0.03378779f;
		FEMALE_EYE_OFFSET_SCALE = 2.03f;
		FI_LOG_TEMP_ERROR = false;
		FI_LOG_OVERTEMP = false;
		LOG_INSTANT_FLUSH = false;
		CAN_ALWAYS_QUICKSAVE = false;
		QUICKSAVE_MINIMUM_ALTITUDE = 500f;
		LOG_ERRORS_TO_SCREEN = false;
		LOG_EXCEPTIONS_TO_SCREEN = false;
		LOG_JOINT_BREAK_EVENT = false;
		LOG_MISSING_KEYS_TO_FILE = false;
		SHOW_TRANSLATION_KEYS_ON_SCREEN = false;
		LOG_DELTAV_VERBOSE = false;
		LOG_FXMONGER_VERBOSE = false;
		COLLECT_ROC_STATS = false;
		DEBUG_MAX_SETPOSITION_ALTITUDE = 10000000000000.0;
		DEBUG_AERO_GUI = false;
		DEBUG_AERO_DATA_PAWS = false;
		MISSION_VALIDATOR_MODE = ValidatorMode.Manual;
		MISSION_TEST_AUTOMATIC_CHECKPOINTS = true;
		PAW_COLLAPSED_GROUP_NAMES = "";
		collpasedPAWGroups = new List<string>();
		PAW_NUMERIC_SLIDERS = false;
		PAW_PREFERRED_HEIGHT = 10000f;
		PAW_SCREEN_OFFSET_X = 200f;
		MANEUVER_TOOL_TRANSFER_DEGREES = 10f;
		MANEUVER_TOOL_CALC_TIMEOUT = 10f;
		MANEUVER_TOOL_CB_COLLISION_ADJUSTMENT = 5f;
		SHOW_DELETE_ALARM_CONFIRMATION = true;
		ALARM_ROW_DISPLAYED_FLIGHT = false;
	}

	public static void LoadLayoutKeyBindings(string layoutSettings, bool overrideSettings = false)
	{
		if (!fetch.layoutConfigNodes.ContainsKey(layoutSettings))
		{
			return;
		}
		ConfigNode configNode = fetch.layoutConfigNodes[layoutSettings];
		Debug.Log("GameSettings: Loading Keyboard Layout... " + layoutSettings);
		int i = 0;
		for (int count = configNode.nodes.Count; i < count; i++)
		{
			ConfigNode configNode2 = configNode.nodes[i];
			FieldInfo field = fetch.GetType().GetField(configNode2.name);
			if (!(field == null) && field.GetValue(fetch) is IConfigNode configNode3)
			{
				configNode3.Load(configNode2);
			}
		}
		if (overrideSettings)
		{
			CURRENT_LAYOUT_SETTINGS = layoutSettings;
		}
		Debug.Log($"GameSettings: {layoutSettings} Layout Loaded!");
	}

	public static string DetectKeyboardLayout()
	{
		Dictionary<string, ConfigNode> dictionary = fetch.layoutConfigNodes;
		KeyboardLayout currentLayout = KeyboardLayout.GetKeyboardLayout();
		string text = $"{currentLayout.Type}_{currentLayout.Locale.Name}.cfg";
		Debug.Log("Layout detected: " + text);
		if (text.Equals("Qwerty_en-US.cfg", StringComparison.InvariantCultureIgnoreCase))
		{
			text = "Qwerty.cfg";
		}
		else if (text.Equals("Azerty_fr-FR.cfg", StringComparison.InvariantCultureIgnoreCase))
		{
			text = "Azerty.cfg";
		}
		else if (text.Equals("Qwertz_de-DE.cfg", StringComparison.InvariantCultureIgnoreCase))
		{
			text = "Qwertz.cfg";
		}
		if (dictionary.ContainsKey(text))
		{
			return text;
		}
		text = dictionary.Keys.Where((string t) => t.StartsWith($"{currentLayout.Type}_{currentLayout.Locale.TwoLetterISOLanguageName}", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
		if (text != null)
		{
			return text;
		}
		text = dictionary.Keys.Where((string t) => t.Equals($"{currentLayout.Type}.cfg", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
		if (text != null)
		{
			return text;
		}
		return "Qwerty.cfg";
	}

	public static void SaveSettings()
	{
		fetch.WriteCfg();
		fetch.WriteLauncherCFG();
	}

	public static void ResetSettings()
	{
		SetDefaultValues();
		LoadLayoutKeyBindings(DetectKeyboardLayout(), overrideSettings: true);
		PQSCache.CreateDefaultPresetList();
		SaveSettings();
		ApplySettings();
	}

	public static void LoadGameSettingsOnly()
	{
		fetch.ParseCfg();
	}

	public static void SaveGameSettingsOnly()
	{
		fetch.WriteCfg();
	}

	[ContextMenu("Reload Settings")]
	public void ReloadSettings()
	{
		ParseCfg();
		ParseLauncherCfg();
	}

	[ContextMenu("Save Settings")]
	public void WriteSettings()
	{
		WriteCfg();
		WriteLauncherCFG();
	}

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		fetch = this;
		string text = SystemInfo.graphicsDeviceVersion.ToLower();
		if (text.Contains("direct3d") || text.Contains("directx"))
		{
			if (!text.Contains("direct3d 11") && !text.Contains("directx 11"))
			{
				if (text.Contains("opengl"))
				{
					graphicsVersion = GraphicsType.const_2;
				}
			}
			else
			{
				graphicsVersion = GraphicsType.D3D11;
			}
		}
		SetDefaultValues();
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		if (INPUT_DEVICES == null)
		{
			INPUT_DEVICES = new InputDevices();
		}
		configFilePath = ((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../settings.cfg") : Path.Combine(Application.dataPath, "../settings.cfg"));
		launcherFilePath = ((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../Launcher.app/Contents/E5150.cfg.bak") : Path.Combine(Application.dataPath, "../Launcher_Data/E5150.cfg.bak"));
		controlsLayoutPath = ((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../" + controlsLayoutPath) : Path.Combine(Application.dataPath, "../" + controlsLayoutPath));
		if (Directory.Exists(controlsLayoutPath))
		{
			ParseLayoutsCfg(Directory.GetFiles(controlsLayoutPath, "*.cfg", SearchOption.AllDirectories));
		}
		if (File.Exists(configFilePath) && !overrideSettings)
		{
			ParseCfg();
			WriteCfg();
		}
		else
		{
			LoadLayoutKeyBindings(DetectKeyboardLayout(), overrideSettings: true);
			InitializeDefaultValues();
			WriteCfg();
		}
		if (!File.Exists(launcherFilePath))
		{
			dontShowLauncher = true;
		}
		else
		{
			ParseLauncherCfg();
		}
		saveOnGameSave = false;
		GameEvents.onGameStateSaved.Add(OnGameSave);
	}

	public void Start()
	{
		ApplyEngineSettings();
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
		GameEvents.onGameStateSaved.Remove(OnGameSave);
	}

	public void ParseLayoutsCfg(string[] layoutFiles)
	{
		int i = 0;
		for (int num = layoutFiles.Length; i < num; i++)
		{
			layoutConfigNodes.Add(Path.GetFileName(layoutFiles[i]), ParseLayoutRecursive(new ConfigNode(), layoutFiles[i]));
		}
	}

	public ConfigNode ParseLayoutRecursive(ConfigNode node, string layoutFile, int depth = 0)
	{
		ConfigNode configNode = ConfigNode.Load(layoutFile);
		if (configNode == null)
		{
			Debug.LogWarning("GameSettings: Cannot find keyboard layout " + layoutFile);
			return node;
		}
		if (!configNode.HasNode("KEYBOARD_LAYOUT"))
		{
			Debug.LogWarning($"GameSettings: {layoutFile} invalid format, missing KEYBOARD_LAYOUT node!");
			return node;
		}
		ConfigNode node2 = configNode.GetNode("KEYBOARD_LAYOUT");
		if (node2.HasValue("dependency") && depth < 5)
		{
			depth++;
			node = ((!layoutConfigNodes.ContainsKey(node2.GetValue("dependency"))) ? ParseLayoutRecursive(node, node2.GetValue("dependency"), depth) : layoutConfigNodes[node2.GetValue("dependency")].CreateCopy());
		}
		if (configNode.HasNode("KEY_MAP") && node.HasNode("KEY_MAP"))
		{
			configNode.GetNode("KEY_MAP").CopyTo(node.GetNode("KEY_MAP"), overwrite: true);
			configNode.RemoveNode("KEY_MAP");
		}
		configNode.CopyTo(node, overwrite: true);
		return node;
	}

	public void WriteCfg()
	{
		SetCollapsedPawGroupString();
		PDebug.Log("GameSettings: Writing cfg...", PDebug.DebugLevel.GameSettings);
		ConfigNode configNode = new ConfigNode();
		FieldInfo[] fields = GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.IsStatic && fieldInfo.IsPublic)
			{
				if (fieldInfo.GetValue(this) is IConfigNode configNode2)
				{
					PDebug.Log(fieldInfo.Name, PDebug.DebugLevel.GameSettings);
					configNode2.Save(configNode.AddNode(fieldInfo.Name));
					continue;
				}
				string text = fieldInfo.Name;
				string text2 = Convert.ToString(fieldInfo.GetValue(this), CultureInfo.InvariantCulture);
				PDebug.Log("Setting: " + text + ", " + text2, PDebug.DebugLevel.GameSettings);
				configNode.AddValue(text, text2);
			}
		}
		PQSCache.SavePresetList(configNode);
		configNode.Save(configFilePath, "KSP Game Settings");
		GameEvents.OnGameSettingsWritten.Fire();
		PDebug.Log("GameSettings: Written!", PDebug.DebugLevel.GameSettings);
	}

	public void WriteLauncherCFG()
	{
		if (!File.Exists(launcherFilePath))
		{
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = File.ReadAllLines(launcherFilePath);
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			string key = array[i].Substring(0, array[i].IndexOf("="));
			string value = array[i].Substring(array[i].IndexOf("=") + 1, array[i].Length - array[i].IndexOf("=") - 1);
			dictionary.Add(key, value);
		}
		Debug.Log("Launcher here(in write): " + dontShowLauncher);
		dictionary["DONT_SHOW_LAUNCHER "] = dontShowLauncher.ToString();
		int num2 = 0;
		string[] array2 = new string[dictionary.Count];
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			array2[num2] = item.Key + "=" + item.Value;
			num2++;
		}
		File.WriteAllLines(launcherFilePath, array2);
		dictionary.Clear();
		dictionary = null;
		array = null;
		array2 = null;
	}

	public void ParseLauncherCfg()
	{
		string[] array = File.ReadAllLines(launcherFilePath);
		foreach (string text in array)
		{
			if (text.Contains("DONT"))
			{
				dontShowLauncher = Convert.ToBoolean(text.Substring(text.IndexOf("=") + 1, text.Length - text.IndexOf("=") - 1));
				Debug.Log("Launcher disabled? " + dontShowLauncher);
			}
		}
	}

	public void ParseCfg()
	{
		PDebug.Log("GameSettings: Loading...", PDebug.DebugLevel.GameSettings);
		ConfigNode configNode = ConfigNode.Load(configFilePath);
		if (configNode == null)
		{
			Debug.LogWarning("GameSettings: Cannot find settings");
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (configNode.HasValue("SETTINGS_FILE_VERSION"))
		{
			if (KSPUtil.CheckVersion(configNode.GetValue("SETTINGS_FILE_VERSION"), 1, 1, 0) != VersionCompareResult.COMPATIBLE)
			{
				Debug.LogWarning("[GAME SETTINGS]: WARNING - settings.cfg file is incompatible with this version", base.gameObject);
				return;
			}
			SETTINGS_FILE_VERSION = configNode.GetValue("SETTINGS_FILE_VERSION");
			if (KSPUtil.CheckVersion(configNode.GetValue("SETTINGS_FILE_VERSION"), 1, 2, 0) != VersionCompareResult.COMPATIBLE)
			{
				flag = true;
			}
			if (KSPUtil.CheckVersion(configNode.GetValue("SETTINGS_FILE_VERSION"), 1, 3, 0) != VersionCompareResult.COMPATIBLE)
			{
				flag2 = true;
			}
			SETTINGS_FILE_VERSION = "1.3.0";
			if (!configNode.HasValue("SHOW_VERSION_WATERMARK"))
			{
				SHOW_VERSION_WATERMARK = false;
			}
			SHOW_ANALYTICS_DIALOG_SettingExists = configNode.HasValue("SHOW_ANALYTICS_DIALOG");
			SHOW_WHATSNEW_DIALOG_SettingExists = configNode.HasValue("SHOW_WHATSNEW_DIALOG");
			int i = 0;
			for (int count = configNode.nodes.Count; i < count; i++)
			{
				ConfigNode configNode2 = configNode.nodes[i];
				FieldInfo field = GetType().GetField(configNode2.name);
				if (!(field == null) && field.GetValue(this) is IConfigNode configNode3)
				{
					configNode3.Load(configNode2);
				}
			}
			foreach (ConfigNode.Value value in configNode.values)
			{
				if (value.name == "SETTINGS_FILE_VERSION")
				{
					continue;
				}
				FieldInfo field2 = GetType().GetField(value.name);
				if (field2 == null)
				{
					continue;
				}
				if (field2.FieldType == typeof(string))
				{
					field2.SetValue(this, value.value);
				}
				else if (field2.FieldType == typeof(int))
				{
					field2.SetValue(this, int.Parse(value.value, CultureInfo.InvariantCulture));
				}
				else if (field2.FieldType == typeof(float))
				{
					field2.SetValue(this, float.Parse(value.value, CultureInfo.InvariantCulture));
				}
				else if (field2.FieldType == typeof(bool))
				{
					field2.SetValue(this, bool.Parse(value.value));
				}
				else if (field2.FieldType == typeof(double))
				{
					field2.SetValue(this, double.Parse(value.value, CultureInfo.InvariantCulture));
				}
				else if (field2.FieldType == typeof(Vector2))
				{
					string[] array = value.value.Split(',');
					if (array.Length < 2)
					{
						continue;
					}
					field2.SetValue(this, new Vector2(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture)));
				}
				else if (field2.FieldType == typeof(Vector3))
				{
					string[] array2 = value.value.Split(',');
					if (array2.Length < 3)
					{
						continue;
					}
					field2.SetValue(this, new Vector3(float.Parse(array2[0], CultureInfo.InvariantCulture), float.Parse(array2[1], CultureInfo.InvariantCulture), float.Parse(array2[2], CultureInfo.InvariantCulture)));
				}
				else if (field2.FieldType == typeof(KeyCode))
				{
					field2.SetValue(this, Enum.Parse(typeof(KeyCode), value.value));
				}
				else if (field2.FieldType == typeof(ValidatorMode))
				{
					field2.SetValue(this, Enum.Parse(typeof(ValidatorMode), value.value));
				}
				PDebug.Log(value.name + ": " + ((field2.GetValue(this) != null) ? field2.GetValue(this).ToString() : "NULL"), PDebug.DebugLevel.GameSettings);
			}
			PQSCache.LoadPresetList(configNode);
			InitializeDefaultValues();
			bool flag3 = false;
			if (flag)
			{
				SHADOWS_DEFAULT_PROJECTION = 1;
				SHADOWS_FLIGHT_PROJECTION = 1;
				SHADOWS_KSC_PROJECTION = 1;
				SHADOWS_TRACKING_PROJECTION = 1;
				SHADOWS_EDITORS_PROJECTION = 1;
				flag3 = true;
			}
			if (flag2)
			{
				SHOW_VESSEL_NAMING_IN_FLIGHT = true;
				flag3 = true;
			}
			if (flag3)
			{
				WriteCfg();
			}
			PDebug.Log("GameSettings: Loaded!", PDebug.DebugLevel.GameSettings);
		}
		else
		{
			Debug.LogWarning("[GAME SETTINGS]: WARNING - settings.cfg file is incompatible with this version", base.gameObject);
		}
	}

	public void InitializeDefaultValues()
	{
		SetCollapsedPawGroupNames();
		SetHighlighterColors();
		SetUITextColors();
		SetRDSearchHighlightColors();
		Highlighter.HighlighterLimit = PART_HIGHLIGHTER_BRIGHTNESSFACTOR;
		SetCPVisualColors();
		InitializeAxisGroupbindings();
		SetLightPresetColors();
		SetFireworkPresetColors();
	}

	public void SetCollapsedPawGroupNames()
	{
		collpasedPAWGroups.Clear();
		string[] array = PAW_COLLAPSED_GROUP_NAMES.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				collpasedPAWGroups.Add(array[i]);
			}
		}
	}

	public void SetCollapsedPawGroupString()
	{
		PAW_COLLAPSED_GROUP_NAMES = string.Join(",", collpasedPAWGroups.ToArray());
	}

	public void SetHighlighterColors()
	{
		SetHighlighterColor(ref Highlighter.colorPartHighlightDefault, COLOR_PART_HIGHLIGHT, "COLOR_PART_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartEditorAttached, COLOR_PART_EDITORATTACHED, "COLOR_PART_EDITORATTACHED");
		SetHighlighterColor(ref Highlighter.colorPartEditorDetached, COLOR_PART_EDITORDETACHED, "COLOR_PART_EDITORDETACHED");
		SetHighlighterColor(ref Highlighter.colorPartEditorActionSelected, COLOR_PART_ACTIONGROUP_SELECTED, "COLOR_PART_ACTIONGROUP_SELECTED");
		SetHighlighterColor(ref Highlighter.colorPartEditorActionHighlight, COLOR_PART_ACTIONGROUP_HIGHLIGHT, "COLOR_PART_ACTIONGROUP_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartRootToolHighlight, COLOR_PART_ROOTTOOL_HIGHLIGHT, "COLOR_PART_ROOTTOOL_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartRootToolHighlightEdge, COLOR_PART_ROOTTOOL_HIGHLIGHTEDGE, "COLOR_PART_ROOTTOOL_HIGHLIGHTEDGE");
		SetHighlighterColor(ref Highlighter.colorPartRootToolHover, COLOR_PART_ROOTTOOL_HOVER, "COLOR_PART_ROOTTOOL_HOVER");
		SetHighlighterColor(ref Highlighter.colorPartRootToolHoverEdge, COLOR_PART_ROOTTOOL_HOVEREDGE, "COLOR_PART_ROOTTOOL_HOVEREDGE");
		SetHighlighterColor(ref Highlighter.colorPartEngineerAppHighlight, COLOR_PART_ENGINEERAPP_HIGHLIGHT, "COLOR_PART_ENGINEERAPP_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartTransferSourceHighlight, COLOR_PART_TRANSFER_SOURCE_HIGHLIGHT, "COLOR_PART_TRANSFER_SOURCE_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartTransferSourceHover, COLOR_PART_TRANSFER_SOURCE_HOVER, "COLOR_PART_TRANSFER_SOURCE_HOVER");
		SetHighlighterColor(ref Highlighter.colorPartTransferDestHighlight, COLOR_PART_TRANSFER_DEST_HIGHLIGHT, "COLOR_PART_TRANSFER_DEST_HIGHLIGHT");
		SetHighlighterColor(ref Highlighter.colorPartTransferDestHover, COLOR_PART_TRANSFER_DEST_HOVER, "COLOR_PART_TRANSFER_DEST_HOVER");
		SetHighlighterColor(ref Highlighter.colorPartInventoryContainer, COLOR_PART_INVENTORY_CONTAINER, "COLOR_PART_INVENTORY_CONTAINER");
		SetHighlighterColor(ref Highlighter.colorPartInventoryUnavailableSpace, COLOR_PART_INVENTORY_NOSPACE, "COLOR_PART_INVENTORY_NOSPACE");
		SetHighlighterColor(ref Highlighter.colorPartConstructionValid, COLOR_PART_CONSTRUCTION_VALID, "COLOR_PART_CONSTRUCTION_VALID");
	}

	public void SetLightPresetColors()
	{
		lightPresetColors = new List<Color>();
		lightPresetColors.Add(LoadColor(COLOR_LIGHT_PRESET_1, "COLOR_LIGHT_PRESET_1"));
		lightPresetColors.Add(LoadColor(COLOR_LIGHT_PRESET_2, "COLOR_LIGHT_PRESET_2"));
		lightPresetColors.Add(LoadColor(COLOR_LIGHT_PRESET_3, "COLOR_LIGHT_PRESET_3"));
		lightPresetColors.Add(LoadColor(COLOR_LIGHT_PRESET_4, "COLOR_LIGHT_PRESET_4"));
		lightPresetColors.Add(LoadColor(COLOR_LIGHT_PRESET_5, "COLOR_LIGHT_PRESET_5"));
	}

	public void SetFireworkPresetColors()
	{
		fireworkPresetColors = new List<Color>();
		fireworkPresetColors.Add(LoadColor(COLOR_FIREWORK_PRESET_GREEN, "COLOR_FIREWORK_PRESET_GREEN"));
		fireworkPresetColors.Add(LoadColor(COLOR_FIREWORK_PRESET_LIGHT_BLUE, "COLOR_FIREWORK_PRESET_LIGHT_BLUE"));
		fireworkPresetColors.Add(LoadColor(COLOR_FIREWORK_PRESET_BLUE, "COLOR_FIREWORK_PRESET_BLUE"));
		fireworkPresetColors.Add(LoadColor(COLOR_FIREWORK_PRESET_PURPLE, "COLOR_FIREWORK_PRESET_PURPLE"));
		fireworkPresetColors.Add(LoadColor(COLOR_FIREWORK_PRESET_PINK, "COLOR_FIREWORK_PRESET_PINK"));
	}

	public Color LoadColor(string colorValues, string name)
	{
		if (!ConfigNode.CheckAndParseColor(colorValues, out var color))
		{
			Debug.LogWarning($"Invalid color supplied for {name}: {colorValues}");
		}
		return color;
	}

	public static List<Color> GetLightPresetColors()
	{
		return lightPresetColors;
	}

	public static List<Color> GetFireworkPresetColors()
	{
		return fireworkPresetColors;
	}

	public void SetUITextColors()
	{
		SetHighlighterColor(ref MenuNavigation.inactiveTextColor, UI_COLOR_INACTIVE_TEXT, "UI_COLOR_DEFAULT_TEXT");
		SetHighlighterColor(ref MenuNavigation.activeTextColor, UI_COLOR_ACTIVE_TEXT, "UI_COLOR_HIGHLIGHT_TEXT");
		SetHighlighterColor(ref MenuNavigation.inactiveTextMiniSettingsColor, UI_COLOR_INACTIVE_MINISETTNIGS_TEXT, "UI_COLOR_HIGHLIGHT_GREEN_TEXT");
	}

	public void SetRDSearchHighlightColors()
	{
		SetHighlighterColor(ref RDTechTreeSearchBar.searchSelectionNodeColor, COLOR_RD_SEARCH_NODE_HIGHLIGHT, "COLOR_RD_SEARCH_NODE_HIGHLIGHT");
		SetHighlighterColor(ref RDTechTreeSearchBar.searchSelectionPartColor, COLOR_RD_SEARCH_PART_HIGHLIGHT, "COLOR_RD_SEARCH_PART_HIGHLIGHT");
	}

	public static bool SetHighlighterColor(ref Color c, string colorValues, string name)
	{
		if (ConfigNode.CheckAndParseColor(colorValues, out var color))
		{
			c = color;
			return true;
		}
		Debug.LogWarning($"Invalid color supplied for {name}: {colorValues}");
		return false;
	}

	public void SetCPVisualColors()
	{
		SetHighlighterColor(ref COLOR_CONTROLPOINT_COLOR_FORWARD, CONTROLPOINT_COLOR_FORWARD, "COLOR_CONTROLPOINT_COLOR_FORWARD");
		SetHighlighterColor(ref COLOR_CONTROLPOINT_COLOR_UP, CONTROLPOINT_COLOR_UP, "COLOR_CONTROLPOINT_COLOR_UP");
		SetHighlighterColor(ref COLOR_CONTROLPOINT_COLOR_RIGHT, CONTROLPOINT_COLOR_RIGHT, "COLOR_CONTROLPOINT_COLOR_RIGHT");
	}

	public static void InitializeAxisGroupbindings()
	{
		AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES = new List<float>();
		string[] array = AXIS_INCREMENTAL_SPEED_MULTIPLIER_STORAGE.Split(',');
		if (array == null)
		{
			return;
		}
		float result = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			if (float.TryParse(array[i], out result))
			{
				AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Add(result);
			}
		}
	}

	public static List<float> GetAxisIncrementalSpeedMultiplierValues()
	{
		return AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES;
	}

	public static void AddIncrementalSpeedMultiplierValue(float value)
	{
		if (AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES != null)
		{
			AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Add(value);
			return;
		}
		AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES = new List<float>();
		AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Add(value);
	}

	public static float GetAxisSpeedMultiplier(int index)
	{
		float num = 0f;
		if (AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES != null && index < AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Count)
		{
			return AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES[index];
		}
		return AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT;
	}

	public static int GetAxiSpeedMultiplierIndex(float value)
	{
		if (AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES == null)
		{
			InitializeAxisGroupbindings();
			return GetAxiSpeedMultiplierIndex(value);
		}
		if (!AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Contains(value))
		{
			if (!AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Contains(AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT))
			{
				AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Add(AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT);
				AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Sort();
			}
			return AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.IndexOf(value);
		}
		return AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.IndexOf(value);
	}

	public static void ApplySettings()
	{
		QualitySettings.SetQualityLevel(QUALITY_PRESET, applyExpensiveChanges: true);
		QualitySettings.antiAliasing = ANTI_ALIASING;
		QualitySettings.masterTextureLimit = TEXTURE_QUALITY;
		QualitySettings.vSyncCount = SYNC_VBL;
		QualitySettings.pixelLightCount = LIGHT_QUALITY;
		QualitySettings.shadowCascades = SHADOWS_QUALITY;
		Time.maximumDeltaTime = PHYSICS_FRAME_DT_LIMIT;
		if (Screen.width != SCREEN_RESOLUTION_WIDTH || Screen.height != SCREEN_RESOLUTION_HEIGHT || Screen.fullScreen != FULLSCREEN)
		{
			Screen.SetResolution(SCREEN_RESOLUTION_WIDTH, SCREEN_RESOLUTION_HEIGHT, FULLSCREEN);
		}
		UIMasterController.Instance.SetScale(UI_SCALE);
		AudioListener.volume = MASTER_VOLUME;
		HighlightingSystem.FxEnabled = HIGHLIGHT_FX;
	}

	public void ApplyEngineSettings(bool wait = true)
	{
		QualitySettings.SetQualityLevel(QUALITY_PRESET, applyExpensiveChanges: true);
		QualitySettings.antiAliasing = ANTI_ALIASING;
		QualitySettings.masterTextureLimit = TEXTURE_QUALITY;
		QualitySettings.vSyncCount = SYNC_VBL;
		QualitySettings.pixelLightCount = LIGHT_QUALITY;
		QualitySettings.shadowCascades = SHADOWS_QUALITY;
		Time.maximumDeltaTime = PHYSICS_FRAME_DT_LIMIT;
		Application.runInBackground = SIMULATE_IN_BACKGROUND;
		UIMasterController.Instance.SetScale(UI_SCALE);
		UIMasterController.Instance.SetAppScale(UI_SCALE_APPS * UI_SCALE);
		AudioListener.volume = MASTER_VOLUME;
		if (wait)
		{
			StartCoroutine(WaitAndSetResolution(10));
		}
	}

	public IEnumerator WaitAndSetResolution(int waitForFrames)
	{
		int framesAtStart = Time.frameCount;
		while (Time.frameCount - framesAtStart < waitForFrames)
		{
			yield return new WaitForEndOfFrame();
		}
		Screen.SetResolution(SCREEN_RESOLUTION_WIDTH, SCREEN_RESOLUTION_HEIGHT, FULLSCREEN);
	}

	public void OnGameSave(Game game)
	{
		if (saveOnGameSave)
		{
			saveOnGameSave = false;
			SaveGameSettingsOnly();
		}
	}

	public static void SaveSettingsOnNextGameSave()
	{
		saveOnGameSave = true;
	}
}
