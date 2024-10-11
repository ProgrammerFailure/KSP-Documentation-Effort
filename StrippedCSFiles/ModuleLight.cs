using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleLight : PartModule, IResourceConsumer, IScalarModule
{
	public List<Light> lights;

	public List<float> brightnessLevels;

	public Transform movementTransform;

	public Transform movementTransformPitch;

	public Transform movementTransformRotate;

	[KSPField]
	public string lightName;

	[KSPField]
	public string lightMeshRendererName;

	[KSPField]
	public string flareRendererName;

	private Renderer flareRenderer;

	[KSPField(isPersistant = true)]
	public bool isOn;

	[KSPField(isPersistant = true)]
	public bool uiWriteLock;

	[KSPField]
	public bool useResources;

	[KSPField]
	public string resourceName;

	[KSPField]
	public string animationName;

	[KSPField]
	public float resourceAmount;

	[KSPField]
	public bool useAnimationDim;

	[KSPField]
	public bool useAutoDim;

	[KSPField]
	public float lightBrightenSpeed;

	[KSPField]
	public float lightDimSpeed;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001401")]
	public string displayStatus;

	[KSPField]
	public string status;

	[KSPField(isPersistant = true)]
	public bool disableColorPicker;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001402")]
	public float lightR;

	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001403")]
	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	public float lightG;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001404")]
	public float lightB;

	[UI_ColorPicker]
	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_8003454")]
	public string colorChanger;

	[KSPField(advancedTweakable = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002640")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool castLight;

	[KSPField(isPersistant = true)]
	public string movementTransformName;

	[KSPField]
	public string movementRotateTransformName;

	[KSPField]
	public string movementPitchTransformName;

	[KSPField]
	public bool canRotate;

	[KSPField]
	public string rotateAxisName;

	[KSPField]
	public bool canPitch;

	[KSPField]
	public string pitchAxisName;

	[KSPField]
	public float rotateMin;

	[KSPField]
	public float rotateMax;

	[KSPField]
	public float pitchMin;

	[KSPField]
	public float pitchMax;

	[KSPField]
	public bool canBlink;

	[KSPField(isPersistant = true)]
	public bool isBlinking;

	[KSPField]
	public float blinkMin;

	[KSPField]
	public float blinkMax;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 360f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 360f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = 0f, guiName = "#autoLOC_6011165")]
	public float rotationAngle;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 45f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = -45f, guiName = "#autoLOC_6011166")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 45f, minValue = -45f, affectSymCounterparts = UI_Scene.All)]
	public float pitchAngle;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 2f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = 0.2f, guiName = "#autoLOC_6011167")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 2f, minValue = 0.2f, affectSymCounterparts = UI_Scene.All)]
	public float blinkRate;

	private double recieved;

	private float currentLight;

	private float targetLight;

	private float resourceFraction;

	private Animation anim;

	private bool isStarted;

	private Color lightColor;

	private Color lastColor;

	private Color defaultColor;

	private Vector3 rotationVector;

	private float currentBlinkTime;

	private float currentTime;

	private Color currentColor;

	public bool blinkState;

	protected UI_FloatRange rotationAngleUIField;

	protected BaseAxisField rotationAngleAxisField;

	protected UI_FloatRange pitchAngleUIField;

	protected BaseAxisField pitchAngleAxisField;

	protected UI_FloatRange blinkRateUIField;

	protected BaseAxisField blinkRateAxisField;

	private UIPartActionColorPicker colorPickerWindow;

	private MeshRenderer lightMeshRenderer;

	public List<LightStruct> lightsList;

	private Color tempOnLightColor;

	private float lastBlinkRate;

	private List<PartResourceDefinition> consumedResources;

	[KSPField]
	public string moduleID;

	private EventData<float, float> OnMove;

	private EventData<float> OnStopped;

	private List<AdjusterLightBase> adjusterCache;

	private static string cacheAutoLOC_220477;

	private static string cacheAutoLOC_219034;

	private static string cacheAutoLOC_6003046;

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleLight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001405", KSPActionGroup.Light)]
	public void ToggleLightAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleLightAction(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001406")]
	public void LightOnAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001407")]
	public void LightOffAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_6001409")]
	public void ToggleLights()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6001408")]
	public void LightsOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6001409")]
	public void LightsOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPAWShown(UIPartActionWindow window, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPAWNotShown(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLightState(bool state, bool isCounterPart = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLightState(bool state, Color originalLightColor, bool isCounterPart = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLightColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyLightVector(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetAngleVector(string axisName, float angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModifyCurrentBlinkTime(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_6011169")]
	public void ToggleBlink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6011169")]
	public void BlinkLightOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6011168")]
	public void BlinkLightOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBlinkState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Blink(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessLightEditorScene(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetRotationLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPitchLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetBlinkRateLimits(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLimits(ref BaseAxisField axisField, ref UI_FloatRange uiField, string valueFieldName, float minValue, float maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEmission(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LightVarsModified(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetAnims()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFlareColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float ApplyIntensityAdjustments(float intensity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetCurrentColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<Color> PresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnColorChanged(Color color, string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnColorChanged(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
