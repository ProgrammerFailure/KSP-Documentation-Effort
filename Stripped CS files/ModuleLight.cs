using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleLight : PartModule, IResourceConsumer, IScalarModule
{
	public List<Light> lights;

	public List<float> brightnessLevels;

	public Transform movementTransform;

	public Transform movementTransformPitch;

	public Transform movementTransformRotate;

	[KSPField]
	public string lightName = "toggleableLight";

	[KSPField]
	public string lightMeshRendererName;

	[KSPField]
	public string flareRendererName = "Flare";

	public Renderer flareRenderer;

	[KSPField(isPersistant = true)]
	public bool isOn;

	[KSPField(isPersistant = true)]
	public bool uiWriteLock;

	[KSPField]
	public bool useResources;

	[KSPField]
	public string resourceName = "ElectricCharge";

	[KSPField]
	public string animationName = "LightAnimation";

	[KSPField]
	public float resourceAmount = 0.01f;

	[KSPField]
	public bool useAnimationDim;

	[KSPField]
	public bool useAutoDim;

	[KSPField]
	public float lightBrightenSpeed = 0.3f;

	[KSPField]
	public float lightDimSpeed = 0.8f;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001401")]
	public string displayStatus = Localizer.Format("#autoLOC_219034");

	[KSPField]
	public string status = "Nominal";

	[KSPField(isPersistant = true)]
	public bool disableColorPicker = true;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001402")]
	public float lightR = 0.875f;

	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001403")]
	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	public float lightG = 0.875f;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 1f, minValue = 0f)]
	[KSPAxisField(incrementalSpeed = 0.5f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001404")]
	public float lightB = 0.875f;

	[UI_ColorPicker]
	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_8003454")]
	public string colorChanger;

	[KSPField(advancedTweakable = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6002640")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	public bool castLight = true;

	[KSPField(isPersistant = true)]
	public string movementTransformName = "movementTransformName";

	[KSPField]
	public string movementRotateTransformName = "movementRotateTransformName";

	[KSPField]
	public string movementPitchTransformName = "movementPitchTransformName";

	[KSPField]
	public bool canRotate;

	[KSPField]
	public string rotateAxisName = "Z";

	[KSPField]
	public bool canPitch;

	[KSPField]
	public string pitchAxisName = "Y";

	[KSPField]
	public float rotateMin;

	[KSPField]
	public float rotateMax = 360f;

	[KSPField]
	public float pitchMin;

	[KSPField]
	public float pitchMax = 360f;

	[KSPField]
	public bool canBlink;

	[KSPField(isPersistant = true)]
	public bool isBlinking;

	[KSPField]
	public float blinkMin = 0.2f;

	[KSPField]
	public float blinkMax = 2f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 360f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 360f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = 0f, guiName = "#autoLOC_6011165")]
	public float rotationAngle;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 45f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = -45f, guiName = "#autoLOC_6011166")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 45f, minValue = -45f, affectSymCounterparts = UI_Scene.All)]
	public float pitchAngle;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, guiActiveUnfocused = true, maxValue = 2f, incrementalSpeed = 30f, unfocusedRange = 10f, axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, ignoreClampWhenIncremental = true, minValue = 0.2f, guiName = "#autoLOC_6011167")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 2f, minValue = 0.2f, affectSymCounterparts = UI_Scene.All)]
	public float blinkRate = 1f;

	public double recieved;

	public float currentLight;

	public float targetLight;

	public float resourceFraction;

	public Animation anim;

	public bool isStarted;

	public Color lightColor;

	public Color lastColor;

	public Color defaultColor;

	public Vector3 rotationVector = Vector3.zero;

	public float currentBlinkTime;

	public float currentTime;

	public Color currentColor;

	public bool blinkState;

	public UI_FloatRange rotationAngleUIField;

	public BaseAxisField rotationAngleAxisField;

	public UI_FloatRange pitchAngleUIField;

	public BaseAxisField pitchAngleAxisField;

	public UI_FloatRange blinkRateUIField;

	public BaseAxisField blinkRateAxisField;

	public UIPartActionColorPicker colorPickerWindow;

	public MeshRenderer lightMeshRenderer;

	public List<LightStruct> lightsList;

	public Color tempOnLightColor = Color.white;

	public float lastBlinkRate;

	public List<PartResourceDefinition> consumedResources;

	[KSPField]
	public string moduleID = "lightModule";

	public EventData<float, float> OnMove = new EventData<float, float>("OnMove");

	public EventData<float> OnStopped = new EventData<float>("OnStop");

	public List<AdjusterLightBase> adjusterCache = new List<AdjusterLightBase>();

	public static string cacheAutoLOC_220477;

	public static string cacheAutoLOC_219034;

	public static string cacheAutoLOC_6003046;

	public string ScalarModuleID => moduleID;

	public float GetScalar
	{
		get
		{
			if (!isOn)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public bool CanMove
	{
		get
		{
			if (!useResources)
			{
				return true;
			}
			return resourceFraction > 0.5f;
		}
	}

	public EventData<float, float> OnMoving => OnMove;

	public EventData<float> OnStop => OnStopped;

	[KSPAction("#autoLOC_6001405", KSPActionGroup.Light)]
	public void ToggleLightAction(KSPActionParam param)
	{
		ToggleLightAction(param.type);
	}

	public void ToggleLightAction(KSPActionType action)
	{
		if ((action == KSPActionType.Activate && !uiWriteLock) || (action == KSPActionType.Toggle && !isOn && !uiWriteLock))
		{
			SetLightState(state: true);
		}
		else
		{
			SetLightState(state: false);
		}
	}

	[KSPAction("#autoLOC_6001406")]
	public void LightOnAction(KSPActionParam param)
	{
		if (!uiWriteLock)
		{
			SetLightState(state: true);
		}
	}

	[KSPAction("#autoLOC_6001407")]
	public void LightOffAction(KSPActionParam param)
	{
		SetLightState(state: false);
	}

	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_6001409")]
	public void ToggleLights()
	{
		if (isOn)
		{
			LightsOff();
		}
		else
		{
			LightsOn();
		}
	}

	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6001408")]
	public void LightsOff()
	{
		SetLightState(state: false);
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (base.part.symmetryCounterparts[count] != base.part)
			{
				base.part.symmetryCounterparts[count].Modules.GetModule<ModuleLight>().SetLightState(state: false, tempOnLightColor, isCounterPart: true);
			}
		}
	}

	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6001409")]
	public void LightsOn()
	{
		SetLightState(state: true);
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (base.part.symmetryCounterparts[count] != base.part)
			{
				base.part.symmetryCounterparts[count].Modules.GetModule<ModuleLight>().SetLightState(state: true, tempOnLightColor, isCounterPart: true);
			}
		}
	}

	public void OnPAWShown(UIPartActionWindow window, Part p)
	{
		if (p.persistentId == base.part.persistentId && !disableColorPicker)
		{
			colorPickerWindow = window.GetComponentInChildren<UIPartActionColorPicker>();
		}
	}

	public void OnPAWNotShown(Part p)
	{
		if (p.persistentId == base.part.persistentId && !disableColorPicker)
		{
			colorPickerWindow = null;
		}
	}

	public void SetLightState(bool state, bool isCounterPart = false)
	{
		SetLightState(state, default(Color), isCounterPart);
	}

	public void SetLightState(bool state, Color originalLightColor, bool isCounterPart = false)
	{
		if (isCounterPart)
		{
			Color color = new Color(lightR, lightG, lightB);
			if (color != Color.black && color != Color.clear)
			{
				tempOnLightColor = color;
			}
			else
			{
				tempOnLightColor = originalLightColor;
			}
		}
		if (state)
		{
			if (!isOn)
			{
				currentTime = 0f;
			}
			isOn = true;
			currentBlinkTime = blinkRate;
			status = "Nominal";
			displayStatus = cacheAutoLOC_219034;
			base.Events["ToggleLights"].guiName = Localizer.Format("#autoLOC_6001408");
			if (tempOnLightColor == Color.black)
			{
				tempOnLightColor = defaultColor;
			}
			OnColorChanged(tempOnLightColor);
			GameEvents.onLightsOn.Fire(base.part, this);
		}
		else
		{
			if (isOn)
			{
				currentTime = 0f;
			}
			if (lightColor != Color.black && lightColor != Color.clear)
			{
				tempOnLightColor = lightColor;
			}
			OnColorChanged(Color.black);
			isOn = false;
			status = "Off";
			displayStatus = cacheAutoLOC_220477;
			base.Events["ToggleLights"].guiName = Localizer.Format("#autoLOC_6001409");
			GameEvents.onLightsOff.Fire(base.part, this);
		}
		if (!useAnimationDim)
		{
			int i = 0;
			for (int count = lights.Count; i < count; i++)
			{
				Light light = lights[i];
				if (light != null)
				{
					light.enabled = state && castLight;
				}
			}
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			ProcessLightEditorScene(state);
		}
		base.Events["LightsOn"].active = !state;
		base.Events["LightsOff"].active = state;
	}

	public void UpdateLightColors()
	{
		if (lightColor != Color.black)
		{
			int i = 0;
			for (int count = lights.Count; i < count; i++)
			{
				lights[i].color = lightColor;
			}
		}
		useAutoDim = true;
	}

	public void ModifyLightVector(object field)
	{
		if (movementTransform != null)
		{
			movementTransform.localRotation = Quaternion.Euler(GetAngleVector(rotateAxisName, rotationAngle)) * Quaternion.Euler(GetAngleVector(pitchAxisName, pitchAngle));
		}
		if (movementTransformRotate != null)
		{
			movementTransformRotate.localRotation = Quaternion.Euler(GetAngleVector(rotateAxisName, rotationAngle));
		}
		if (movementTransformPitch != null)
		{
			movementTransformPitch.localRotation = Quaternion.Euler(GetAngleVector(pitchAxisName, pitchAngle));
		}
	}

	public Vector3 GetAngleVector(string axisName, float angle)
	{
		Vector3 result = Vector3.zero;
		if (axisName.Length < 1)
		{
			return result;
		}
		if (axisName.Length > 1 && axisName[1] == '-')
		{
			angle = 0f - angle;
		}
		switch (axisName.Substring(0, 1).ToUpper())
		{
		case "Z":
			result = new Vector3(0f, 0f, angle);
			break;
		case "Y":
			result = new Vector3(0f, angle, 0f);
			break;
		case "X":
			result = new Vector3(angle, 0f, 0f);
			break;
		}
		return result;
	}

	public void ModifyCurrentBlinkTime(object field)
	{
		currentBlinkTime += blinkRate - lastBlinkRate;
		lastBlinkRate = blinkRate;
	}

	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 10f, guiName = "#autoLOC_6011169")]
	public void ToggleBlink()
	{
		if (!isBlinking)
		{
			BlinkLightOn();
		}
		else
		{
			BlinkLightOff();
		}
	}

	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6011169")]
	public void BlinkLightOn()
	{
		SetBlinkState(state: true);
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (base.part.symmetryCounterparts[count] != base.part)
			{
				base.part.symmetryCounterparts[count].Modules.GetModule<ModuleLight>().SetBlinkState(state: true);
			}
		}
	}

	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = false, guiActive = false, unfocusedRange = 10f, guiName = "#autoLOC_6011168")]
	public void BlinkLightOff()
	{
		SetBlinkState(state: false);
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (base.part.symmetryCounterparts[count] != base.part)
			{
				base.part.symmetryCounterparts[count].Modules.GetModule<ModuleLight>().SetBlinkState(state: false);
			}
		}
	}

	public void SetBlinkState(bool state)
	{
		if (!canBlink)
		{
			base.Events["BlinkLightOn"].active = false;
			base.Events["BlinkLightOff"].active = false;
			base.Fields["blinkRate"].guiActive = false;
			base.Fields["blinkRate"].guiActiveEditor = false;
		}
		else if (state)
		{
			isBlinking = true;
			base.Events["BlinkLightOn"].active = false;
			base.Events["BlinkLightOff"].active = true;
			base.Events["ToggleBlink"].guiName = Localizer.Format("#autoLOC_6011168");
		}
		else
		{
			isBlinking = false;
			base.Events["BlinkLightOn"].active = true;
			base.Events["BlinkLightOff"].active = false;
			base.Events["ToggleBlink"].guiName = Localizer.Format("#autoLOC_6011169");
			SetLightState(isOn);
			blinkState = false;
		}
	}

	public void Blink(bool state)
	{
		if (!useAnimationDim)
		{
			int i = 0;
			for (int count = lights.Count; i < count; i++)
			{
				Light light = lights[i];
				if (light != null)
				{
					light.enabled = state && castLight;
				}
			}
		}
		else
		{
			if (state)
			{
				OnColorChanged(tempOnLightColor);
				if (lightMeshRenderer != null)
				{
					lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
				}
				int j = 0;
				for (int count2 = lights.Count; j < count2; j++)
				{
					lights[j].intensity = 1f;
					lights[j].enabled = castLight;
				}
			}
			else
			{
				tempOnLightColor = lightColor;
				OnColorChanged(Color.black);
				if (lightMeshRenderer != null)
				{
					lightMeshRenderer.material.SetColor("_EmissiveColor", Color.black);
				}
				int k = 0;
				for (int count3 = lights.Count; k < count3; k++)
				{
					lights[k].intensity = 0f;
					lights[k].enabled = false;
				}
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				ProcessLightEditorScene(state);
			}
		}
		blinkState = !state;
	}

	public void ProcessLightEditorScene(bool state)
	{
		if (!useAutoDim)
		{
			return;
		}
		if (state)
		{
			targetLight = 1f;
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.white);
			}
		}
		else
		{
			targetLight = 0f;
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", Color.black);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.black);
			}
		}
		int i = 0;
		for (int count = lights.Count; i < count; i++)
		{
			lights[i].intensity = 1f;
			lights[i].enabled = state && castLight;
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		if (useResources)
		{
			if (consumedResources == null)
			{
				consumedResources = new List<PartResourceDefinition>();
			}
			else
			{
				consumedResources.Clear();
			}
			int i = 0;
			for (int count = resHandler.inputResources.Count; i < count; i++)
			{
				consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
			}
		}
		else if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
	}

	public override void OnStart(StartState state)
	{
		lights = new List<Light>(base.part.FindModelComponents<Light>(lightName));
		if (!string.IsNullOrEmpty(lightMeshRendererName))
		{
			lightMeshRenderer = base.part.FindModelComponent<MeshRenderer>(lightMeshRendererName);
		}
		if (lightMeshRenderer == null)
		{
			Renderer[] partRenderers = base.part.GetPartRenderers();
			lightMeshRenderer = (MeshRenderer)partRenderers[0];
		}
		flareRenderer = base.part.FindModelComponent<MeshRenderer>(flareRendererName);
		if (flareRenderer == null)
		{
			Transform transform = base.part.FindModelTransformByLayer("TransparentFX");
			if (transform != null)
			{
				flareRenderer = transform.GetComponent<MeshRenderer>();
			}
		}
		if (lightMeshRenderer == null)
		{
			useAnimationDim = false;
		}
		brightnessLevels = new List<float>();
		defaultColor = new Color(0.875f, 0.875f, 0.875f);
		lightColor = new Color(lightR, lightG, lightB);
		tempOnLightColor = lightColor;
		movementTransform = base.part.FindModelTransform(movementTransformName);
		if (movementRotateTransformName == movementPitchTransformName)
		{
			movementTransform = base.part.FindModelTransform(movementRotateTransformName);
		}
		if (movementTransform == null)
		{
			movementTransformRotate = base.part.FindModelTransform(movementRotateTransformName);
			movementTransformPitch = base.part.FindModelTransform(movementPitchTransformName);
		}
		lastBlinkRate = blinkRate;
		int i = 0;
		for (int count = lights.Count; i < count; i++)
		{
			Light light = lights[i];
			brightnessLevels.Add(light.intensity);
			light.color = lightColor;
		}
		if (isOn)
		{
			SetLightState(state: true);
		}
		else
		{
			SetLightState(state: false);
			currentTime = lightBrightenSpeed;
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", currentColor);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", currentColor);
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			isStarted = true;
			if (base.vessel.situation == Vessel.Situations.PRELAUNCH && isOn)
			{
				base.vessel.ActionGroups[KSPActionGroup.Light] = true;
			}
		}
		base.Fields["lightR"].guiActive = false;
		base.Fields["lightR"].guiActiveEditor = false;
		base.Fields["lightG"].guiActive = false;
		base.Fields["lightG"].guiActiveEditor = false;
		base.Fields["lightB"].guiActive = false;
		base.Fields["lightB"].guiActiveEditor = false;
		if (disableColorPicker)
		{
			base.Fields["colorChanger"].guiActive = false;
			base.Fields["colorChanger"].guiActiveEditor = false;
		}
		else
		{
			base.Fields["colorChanger"].guiActive = true;
			base.Fields["colorChanger"].guiActiveEditor = true;
		}
		base.Fields["rotationAngle"].guiActive = canRotate;
		base.Fields["rotationAngle"].guiActiveEditor = canRotate;
		SetRotationLimits(null);
		if (rotationAngleAxisField != null)
		{
			rotationAngleAxisField.active = movementTransform != null || movementTransformRotate != null;
		}
		base.Fields["pitchAngle"].guiActive = canPitch;
		base.Fields["pitchAngle"].guiActiveEditor = canPitch;
		SetPitchLimits(null);
		if (pitchAngleAxisField != null)
		{
			pitchAngleAxisField.active = movementTransform != null || movementTransformPitch != null;
		}
		base.Fields["blinkRate"].guiActive = canBlink;
		base.Fields["blinkRate"].guiActiveEditor = canBlink;
		SetBlinkRateLimits(null);
		if (canBlink)
		{
			base.Events["ToggleBlink"].active = true;
		}
		else
		{
			base.Events["ToggleBlink"].active = false;
			isBlinking = false;
		}
		GameEvents.onPartActionUIShown.Add(OnPAWShown);
		GameEvents.onPartActionUIDismiss.Add(OnPAWNotShown);
		base.Fields["lightR"].OnValueModified += LightVarsModified;
		base.Fields["lightG"].OnValueModified += LightVarsModified;
		base.Fields["lightB"].OnValueModified += LightVarsModified;
		base.Fields["rotationAngle"].OnValueModified += ModifyLightVector;
		base.Fields["pitchAngle"].OnValueModified += ModifyLightVector;
		base.Fields["blinkRate"].OnValueModified += ModifyCurrentBlinkTime;
		base.Fields["rotateMin"].OnValueModified += SetRotationLimits;
		base.Fields["rotateMax"].OnValueModified += SetRotationLimits;
		base.Fields["pitchMin"].OnValueModified += SetPitchLimits;
		base.Fields["pitchMax"].OnValueModified += SetPitchLimits;
		base.Fields["blinkMin"].OnValueModified += SetBlinkRateLimits;
		base.Fields["blinkMax"].OnValueModified += SetBlinkRateLimits;
		base.Fields["castLight"].OnValueModified += SetEmission;
		ModifyLightVector(null);
		GetAnims();
		if (anim != null)
		{
			anim.Stop();
		}
		if (lightsList != null && lightsList.Count > 0)
		{
			for (int j = 0; j < lightsList.Count; j++)
			{
				lightsList[j].FindLightComponents(base.part, disableColorPicker);
			}
		}
	}

	public override void OnInventoryModeDisable()
	{
		lights = new List<Light>(base.part.FindModelComponents<Light>(lightName));
		int i = 0;
		for (int count = lights.Count; i < count; i++)
		{
			if (lights[i] != null)
			{
				lights[i].enabled = false;
			}
		}
		if (!string.IsNullOrEmpty(lightMeshRendererName))
		{
			lightMeshRenderer = base.part.FindModelComponent<MeshRenderer>(lightMeshRendererName);
		}
		if (lightMeshRenderer != null)
		{
			lightMeshRenderer.material.SetColor("_EmissiveColor", Color.black);
		}
		flareRenderer = base.part.FindModelComponent<MeshRenderer>(flareRendererName);
		if (flareRenderer == null)
		{
			Transform transform = base.part.FindModelTransformByLayer("TransparentFX");
			if (transform != null)
			{
				flareRenderer = transform.GetComponent<MeshRenderer>();
			}
		}
		if (flareRenderer != null)
		{
			flareRenderer.material.SetColor("_TintColor", Color.black);
		}
		isOn = false;
	}

	public void SetRotationLimits(object field)
	{
		SetLimits(ref rotationAngleAxisField, ref rotationAngleUIField, "rotationAngle", rotateMin, rotateMax);
		rotationAngle = Mathf.Clamp(rotationAngle, rotateMin, rotateMax);
	}

	public void SetPitchLimits(object field)
	{
		SetLimits(ref pitchAngleAxisField, ref pitchAngleUIField, "pitchAngle", pitchMin, pitchMax);
		pitchAngle = Mathf.Clamp(pitchAngle, pitchMin, pitchMax);
	}

	public void SetBlinkRateLimits(object field)
	{
		SetLimits(ref blinkRateAxisField, ref blinkRateUIField, "blinkRate", blinkMin, blinkMax);
		blinkRate = Mathf.Clamp(blinkRate, blinkMin, blinkMax);
	}

	public void SetLimits(ref BaseAxisField axisField, ref UI_FloatRange uiField, string valueFieldName, float minValue, float maxValue)
	{
		axisField = base.Fields[valueFieldName] as BaseAxisField;
		if (axisField != null)
		{
			axisField.minValue = minValue;
			axisField.maxValue = maxValue;
		}
		if (uiField == null && base.Fields.TryGetFieldUIControl<UI_FloatRange>(valueFieldName, out uiField))
		{
			uiField.minValue = minValue;
			uiField.maxValue = maxValue;
		}
	}

	public void SetEmission(object obj)
	{
		SetLightState(isOn);
	}

	public void LightVarsModified(object field)
	{
		lightColor.r = lightR;
		lightColor.g = lightG;
		lightColor.b = lightB;
		lastColor = lightColor;
		if (colorPickerWindow != null && colorPickerWindow.colorPicker != null && !disableColorPicker)
		{
			colorPickerWindow.colorPicker.SetColor(lightColor);
		}
		UpdateLightColors();
		if (lightColor != Color.black)
		{
			tempOnLightColor = lightColor;
		}
		lastColor = lightColor;
		if (!isOn)
		{
			return;
		}
		if (currentTime / lightBrightenSpeed > 1f)
		{
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.white);
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.white);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (resHandler.inputResources.Count == 0)
		{
			ModuleResource moduleResource = new ModuleResource();
			moduleResource.name = resourceName;
			moduleResource.title = KSPUtil.PrintModuleName(resourceName);
			moduleResource.id = resourceName.GetHashCode();
			moduleResource.rate = resourceAmount;
			resHandler.inputResources.Add(moduleResource);
		}
		if (lightsList != null)
		{
			return;
		}
		ConfigNode[] nodes = node.GetNodes("LIGHT");
		if (nodes.Length != 0)
		{
			lightsList = new List<LightStruct>();
			for (int i = 0; i < nodes.Length; i++)
			{
				LightStruct lightStruct = new LightStruct();
				lightStruct.Load(nodes[i]);
				lightsList.Add(lightStruct);
			}
		}
	}

	public void GetAnims()
	{
		List<Animation> list = new List<Animation>(base.part.FindModelComponents<Animation>());
		int num = 0;
		int count = list.Count;
		Animation animation;
		while (true)
		{
			if (num < count)
			{
				animation = list[num];
				if (animation[animationName] != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		anim = animation;
	}

	public override string GetInfo()
	{
		string text = "";
		if (useResources)
		{
			text += resHandler.PrintModuleResources();
		}
		return text;
	}

	public void SetFlareColor(Color c)
	{
		if (flareRenderer != null)
		{
			flareRenderer.material.SetColor("_TintColor", c);
		}
	}

	public void FixedUpdate()
	{
		if (isBlinking && isOn)
		{
			currentBlinkTime -= TimeWarp.fixedDeltaTime;
			if (currentBlinkTime <= 0f)
			{
				Blink(blinkState);
				currentBlinkTime = blinkRate;
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (isOn && lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
			}
		}
		else
		{
			if (!isStarted)
			{
				return;
			}
			float fixedDeltaTime = TimeWarp.fixedDeltaTime;
			currentTime += TimeWarp.fixedDeltaTime;
			if (isOn && (!isBlinking || !blinkState))
			{
				if (useResources)
				{
					resourceFraction = (float)resHandler.UpdateModuleResourceInputs(ref status, 1.0, 0.99, returnOnFirstLack: false, average: false, stringOps: true);
					if (resourceFraction >= 0.99f)
					{
						if (status != "Nominal")
						{
							status = "Nominal";
							displayStatus = cacheAutoLOC_219034;
						}
					}
					else
					{
						SetLightState(state: false);
					}
					resourceFraction = ApplyIntensityAdjustments(resourceFraction);
				}
				else
				{
					resourceFraction = 1f;
				}
				if (useAutoDim && !isBlinking)
				{
					int i = 0;
					for (int count = lights.Count; i < count; i++)
					{
						targetLight = 1f * resourceFraction;
						if (currentLight < targetLight)
						{
							currentLight = Mathf.Lerp(currentLight, targetLight, lightBrightenSpeed * fixedDeltaTime);
						}
						else
						{
							currentLight = Mathf.Lerp(currentLight, targetLight, lightDimSpeed * fixedDeltaTime);
						}
						lights[i].enabled = castLight;
						lights[i].intensity = currentLight;
					}
					if (currentTime / lightBrightenSpeed < 1f)
					{
						currentColor = Color.Lerp(Color.black, lightColor, currentTime / lightBrightenSpeed);
						if (lightMeshRenderer != null)
						{
							lightMeshRenderer.material.SetColor("_EmissiveColor", currentColor);
						}
						if (flareRenderer != null)
						{
							flareRenderer.material.SetColor("_TintColor", Color.Lerp(Color.black, Color.white, currentTime / lightBrightenSpeed));
						}
					}
				}
			}
			else if (useAutoDim)
			{
				int j = 0;
				for (int count2 = lights.Count; j < count2; j++)
				{
					targetLight = 0f;
					if (currentLight < targetLight)
					{
						currentLight = Mathf.Lerp(currentLight, targetLight, lightBrightenSpeed * fixedDeltaTime);
					}
					else
					{
						currentLight = Mathf.Lerp(currentLight, targetLight, lightDimSpeed * fixedDeltaTime);
					}
					lights[j].intensity = currentLight;
				}
				if (currentTime / lightBrightenSpeed < 1f)
				{
					currentColor = Color.Lerp(tempOnLightColor, Color.black, currentTime / lightBrightenSpeed);
					if (lightMeshRenderer != null)
					{
						lightMeshRenderer.material.SetColor("_EmissiveColor", currentColor);
					}
					if (flareRenderer != null)
					{
						flareRenderer.material.SetColor("_TintColor", Color.Lerp(Color.white, Color.black, currentTime / lightBrightenSpeed));
					}
				}
			}
			if (lightsList != null && lightsList.Count > 0)
			{
				for (int k = 0; k < lightsList.Count; k++)
				{
					lightsList[k].FixedUpdateLightsStatus(isOn, currentTime, fixedDeltaTime, resourceFraction, targetLight, currentLight, lightBrightenSpeed, lightDimSpeed, castLight, lightColor);
				}
			}
		}
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUIShown.Remove(OnPAWShown);
		GameEvents.onPartActionUIDismiss.Remove(OnPAWNotShown);
		base.Fields["lightR"].OnValueModified -= LightVarsModified;
		base.Fields["lightG"].OnValueModified -= LightVarsModified;
		base.Fields["lightB"].OnValueModified -= LightVarsModified;
		base.Fields["rotationAngle"].OnValueModified -= ModifyLightVector;
		base.Fields["pitchAngle"].OnValueModified -= ModifyLightVector;
		base.Fields["blinkRate"].OnValueModified -= ModifyCurrentBlinkTime;
		base.Fields["rotateMin"].OnValueModified -= SetRotationLimits;
		base.Fields["rotateMax"].OnValueModified -= SetRotationLimits;
		base.Fields["pitchMin"].OnValueModified -= SetPitchLimits;
		base.Fields["pitchMax"].OnValueModified -= SetPitchLimits;
		base.Fields["blinkMin"].OnValueModified -= SetBlinkRateLimits;
		base.Fields["blinkMax"].OnValueModified -= SetBlinkRateLimits;
	}

	public void SetScalar(float t)
	{
		if (t > 0.5f)
		{
			if (!isOn)
			{
				LightsOn();
			}
		}
		else if (t <= 0.5f && isOn)
		{
			LightsOff();
		}
	}

	public void SetUIRead(bool state)
	{
	}

	public void SetUIWrite(bool state)
	{
		uiWriteLock = !state;
		if (state)
		{
			base.Events["LightsOn"].active = !isOn;
			base.Events["LightsOff"].active = isOn;
		}
		else
		{
			base.Events["LightsOn"].active = false;
			base.Events["LightsOff"].active = false;
		}
	}

	public bool IsMoving()
	{
		return false;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterLightBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterLightBase item = adjuster as AdjusterLightBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyIntensityAdjustments(float intensity)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			intensity = adjusterCache[i].ApplyIntensityAdjustment(intensity);
		}
		return intensity;
	}

	public override Color GetCurrentColor()
	{
		return new Color(lightR, lightG, lightB);
	}

	public override List<Color> PresetColors()
	{
		return GameSettings.GetLightPresetColors();
	}

	public override void OnColorChanged(Color color, string id)
	{
		OnColorChanged(color);
	}

	public override void OnColorChanged(Color color)
	{
		if (color != Color.black)
		{
			lightR = color.r;
			lightG = color.g;
			lightB = color.b;
			lightColor = color;
		}
		if (colorPickerWindow != null && colorPickerWindow.colorPicker != null && !disableColorPicker)
		{
			colorPickerWindow.colorPicker.SetColor(lightColor);
		}
		UpdateLightColors();
		if (lightColor != Color.black)
		{
			tempOnLightColor = lightColor;
		}
		lastColor = lightColor;
		if (isOn)
		{
			if (currentTime / lightBrightenSpeed > 1f)
			{
				if (lightMeshRenderer != null)
				{
					lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
				}
				if (flareRenderer != null)
				{
					flareRenderer.material.SetColor("_TintColor", Color.white);
				}
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				if (lightMeshRenderer != null)
				{
					lightMeshRenderer.material.SetColor("_EmissiveColor", lightColor);
				}
				if (flareRenderer != null)
				{
					flareRenderer.material.SetColor("_TintColor", Color.white);
				}
			}
		}
		if (lightsList != null && lightsList.Count > 0)
		{
			for (int i = 0; i < lightsList.Count; i++)
			{
				lightsList[i].UpdateLightStatusOnEditor(isOn, lightColor);
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003046;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_220477 = Localizer.Format("#autoLOC_220477");
		cacheAutoLOC_219034 = Localizer.Format("#autoLOC_219034");
		cacheAutoLOC_6003046 = Localizer.Format("#autoLoc_6003046");
	}
}
