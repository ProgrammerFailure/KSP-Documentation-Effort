using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns11;
using ns12;
using ns9;
using UnityEngine;

public class ModuleEngines : PartModule, IEngineStatus, IModuleInfo, IResourceConsumer, IThrustProvider, IConstruction
{
	public List<Propellant> propellants;

	public Dictionary<Propellant, ProtoStageIconInfo> PropellantGauges;

	public float mixtureDensity;

	public double mixtureDensityRecip;

	public double ratioSum;

	public int displayNameLimit = 14;

	public bool isThrusting;

	[KSPField]
	public string engineID = "Engine";

	[KSPField]
	public bool throttleLocked;

	[KSPField]
	public bool exhaustDamage = true;

	[KSPField]
	public bool exhaustDamageLogEvent;

	[KSPField]
	public bool exhaustSplashbackDamage = true;

	[KSPField]
	public double exhaustDamageMultiplier = 165.0;

	[KSPField]
	public double exhaustDamageFalloffPower = 1.0;

	[KSPField]
	public double exhaustDamageSplashbackFallofPower = 2.5;

	[KSPField]
	public double exhaustDamageSplashbackMult;

	[KSPField]
	public double exhaustDamageSplashbackMaxMutliplier = 1.0;

	[KSPField]
	public double exhaustDamageDistanceOffset;

	[KSPField]
	public float exhaustDamageMaxRange = 10f;

	[KSPField]
	public double exhaustDamageMaxMutliplier = 100.0;

	public float minThrust;

	public float maxThrust = 215f;

	public float maxFuelFlow = 20f;

	public float minFuelFlow;

	[KSPField]
	public float ignitionThreshold = 0.1f;

	[KSPField]
	public bool clampPropReceived;

	[KSPField]
	public double clampPropReceivedMinLowerAmount = 0.999;

	[KSPField]
	public bool allowRestart = true;

	[KSPField]
	public bool allowShutdown = true;

	[KSPField]
	public bool shieldedCanActivate = true;

	[KSPField]
	public bool autoPositionFX;

	[KSPField]
	public string fxGroupPrefix = "thruster_";

	[KSPField]
	public Vector3 fxOffset = Vector3.zero;

	[KSPField]
	public float heatProduction = 370f;

	[KSPField]
	public FloatCurve atmosphereCurve;

	[KSPField]
	public bool atmChangeFlow;

	[KSPField]
	public FloatCurve atmCurve = new FloatCurve();

	[KSPField]
	public bool useAtmCurve;

	[KSPField]
	public FloatCurve velCurve = new FloatCurve();

	[KSPField]
	public bool useVelCurve;

	[KSPField]
	public float CLAMP = 1E-05f;

	[KSPField]
	public FloatCurve atmCurveIsp = new FloatCurve();

	[KSPField]
	public bool useAtmCurveIsp;

	[KSPField]
	public FloatCurve velCurveIsp = new FloatCurve();

	[KSPField]
	public bool useVelCurveIsp;

	[KSPField]
	public float machLimit = float.MaxValue;

	[KSPField]
	public float flameoutBar = 0.07f;

	[KSPField]
	public float machHeatMult = 1f;

	[KSPField]
	public float flowMultCap = float.MaxValue;

	[KSPField]
	public bool normalizeHeatForFlow = true;

	[KSPField]
	public float flowMultCapSharpness = 2f;

	[KSPField]
	public float multFlow = 1f;

	[KSPField]
	public float multIsp = 1f;

	[KSPField]
	public bool useThrustCurve;

	[KSPField]
	public FloatCurve thrustCurve = new FloatCurve();

	[KSPField]
	public bool useThrottleIspCurve;

	[KSPField]
	public FloatCurve throttleIspCurveAtmStrength = new FloatCurve();

	[KSPField]
	public FloatCurve throttleIspCurve = new FloatCurve();

	[KSPField(guiFormat = "P3", isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001830")]
	public float thrustCurveDisplay = 1f;

	[KSPField(guiFormat = "P3", isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001831")]
	public float thrustCurveRatio = 1f;

	[KSPField]
	public bool useEngineResponseTime;

	[KSPField]
	public float engineAccelerationSpeed = 0.2f;

	[KSPField]
	public float engineDecelerationSpeed = 0.1f;

	[KSPField]
	public bool throttleUseAlternate;

	public float throttleMin;

	[KSPField]
	public float throttleResponseRate = -1f;

	[KSPField]
	public float throttleIgniteLevelMult = 1f;

	[KSPField]
	public float throttleStartupMult = 1f;

	[KSPField]
	public float throttleStartedMult = 1f;

	[KSPField]
	public bool throttleInstantShutdown = true;

	[KSPField]
	public float throttleShutdownMult = 100f;

	[KSPField]
	public bool throttleInstant;

	[KSPField]
	public double throttlingBaseRate = 10.0;

	[KSPField]
	public double throttlingBaseClamp = 1.1;

	[KSPField]
	public double throttlingBaseDivisor = 0.2;

	[KSPField]
	public string thrustVectorTransformName = "thrustTransform";

	public List<Transform> thrustTransforms;

	public List<float> thrustTransformMultipliers;

	[KSPField(guiFormat = "F5", guiActive = true, guiName = "#autoLOC_6001375", guiUnits = "#autoLOC_7001409")]
	public float fuelFlowGui;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001376", guiUnits = "%")]
	public float propellantReqMet;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001377", guiUnits = "#autoLOC_7001408")]
	public float finalThrust;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001378", guiUnits = "#autoLOC_7001400")]
	public float realIsp;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status = "Nominal";

	[KSPField(guiActive = false, guiName = "#autoLOC_6001379")]
	public string statusL2 = " ";

	[UI_Toggle(disabledText = "#autoLOC_6013010", scene = UI_Scene.All, enabledText = "#autoLOC_6005051", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900770")]
	public bool independentThrottle;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 20f, isPersistant = true, maxValue = 100f, minValue = 0f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_900770")]
	public float independentThrottlePercentage;

	public UI_FloatRange independentThrottlePercentField;

	[KSPField]
	public bool disableUnderwater;

	[KSPField]
	public bool nonThrustMotor;

	public float resultingThrust;

	public float flowMultiplier = 1f;

	public float requestedMassFlow;

	public float requestedThrottle;

	public static int damageLayerMask;

	public BaseField statusL2Field;

	public RaycastHit hit;

	public EngineType engineType;

	public float g = 9.80665f;

	public BaseEvent shutdownEvent;

	public BaseEvent activateEvent;

	public BaseField propellantReqMetField;

	[KSPField(isPersistant = true)]
	public bool staged;

	[KSPField(isPersistant = true)]
	public bool flameout;

	[KSPField(isPersistant = true)]
	public bool EngineIgnited;

	[KSPField(isPersistant = true)]
	public bool engineShutdown;

	[KSPField(isPersistant = true)]
	public float currentThrottle;

	[UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f)]
	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage = 100f;

	[KSPField(isPersistant = true)]
	public bool manuallyOverridden;

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool includeinDVCalcs;

	public double massFlow;

	public bool listenersSet;

	public FXGroup flameoutGroup;

	public FXGroup runningGroup;

	public FXGroup powerGroup;

	public FXGroup engageGroup;

	public FXGroup disengageGroup;

	public List<FXGroup> runningGroups;

	public List<FXGroup> flameoutGroups;

	public List<FXGroup> powerGroups;

	public AudioSource powerSfx;

	public List<PartResourceDefinition> consumedResources;

	public List<AdjusterEnginesBase> adjusterCache = new List<AdjusterEnginesBase>();

	public static string cacheAutoLOC_220473;

	public static string cacheAutoLOC_220477;

	public static string cacheAutoLOC_219016;

	public static string cacheAutoLOC_219034;

	public static string cacheAutoLOC_219636;

	public static string cacheAutoLOC_220370;

	public static string cacheAutoLOC_220377;

	public static string cacheAutoLOC_220657;

	public static string cacheAutoLOC_6001000;

	public static string cacheAutoLOC_6001001;

	public static string cacheAutoLOC_220748;

	public static string cacheAutoLOC_220761;

	public static string cacheAutoLOC_220762;

	public float normalizedThrustOutput => finalThrust / maxThrust;

	public bool getFlameoutState => flameout;

	public bool getIgnitionState => EngineIgnited;

	public bool isOperational
	{
		get
		{
			if (!flameout)
			{
				return EngineIgnited;
			}
			return false;
		}
	}

	public float normalizedOutput => finalThrust / maxThrust;

	public float throttleSetting => currentThrottle;

	public string engineName => engineID;

	public void SetupPropellant()
	{
		mixtureDensity = 0f;
		mixtureDensityRecip = 1.0;
		float num = 0f;
		ratioSum = 0.0;
		int count = propellants.Count;
		for (int i = 0; i < count; i++)
		{
			Propellant propellant = propellants[i];
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(propellant.name.GetHashCode());
			if (definition == null)
			{
				Debug.Log(propellant.name + " not found in resource database. Propellant Setup has failed.");
				base.part.partInfo.amountAvailable = 0;
				continue;
			}
			if (!propellant.ignoreForIsp)
			{
				num += definition.density * propellant.ratio;
				ratioSum += propellant.ratio;
			}
			propellant.GetFlowMode();
		}
		mixtureDensity = num;
		if ((double)mixtureDensity > 0.0)
		{
			mixtureDensityRecip = 1.0 / (double)mixtureDensity;
		}
	}

	public void UpdatePropellantStatus(bool doGauge = true)
	{
		if (propellants == null)
		{
			return;
		}
		int count = propellants.Count;
		for (int i = 0; i < count; i++)
		{
			Propellant propellant = propellants[i];
			propellant.UpdateConnectedResources(base.part);
			if (propellant.drawStackGauge && doGauge)
			{
				UpdatePropellantGauge(propellant);
			}
		}
	}

	public void UpdatePropellantGauge(Propellant p)
	{
		if (!PropellantGauges.ContainsKey(p))
		{
			ProtoStageIconInfo protoStageIconInfo = base.part.stackIcon.DisplayInfo();
			if (protoStageIconInfo == null)
			{
				return;
			}
			protoStageIconInfo.SetLength(2f);
			TooltipController_Text component = protoStageIconInfo.infoBoxRef.gameObject.GetComponent<TooltipController_Text>();
			protoStageIconInfo.SetMsgBgColor(XKCDColors.DarkLime.smethod_0(0.6f));
			protoStageIconInfo.SetMsgTextColor(XKCDColors.ElectricLime.smethod_0(0.6f));
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(p.id);
			if (definition != null)
			{
				if (definition.displayName.LocalizeRemoveGender().Length > displayNameLimit && definition.displayName != "Electric Charge")
				{
					protoStageIconInfo.SetMessage(definition.abbreviation);
					if (component != null)
					{
						component.SetText(definition.displayName.LocalizeRemoveGender());
						component.enabled = true;
					}
				}
				else
				{
					protoStageIconInfo.SetMessage(p.displayName.LocalizeRemoveGender());
					if (component != null)
					{
						component.SetText(string.Empty);
						component.enabled = false;
					}
				}
			}
			else
			{
				protoStageIconInfo.SetMessage(p.displayName);
				if (component != null)
				{
					component.SetText(string.Empty);
					component.enabled = false;
				}
			}
			protoStageIconInfo.SetProgressBarBgColor(XKCDColors.DarkLime.smethod_0(0.6f));
			protoStageIconInfo.SetProgressBarColor(XKCDColors.Yellow.smethod_0(0.6f));
			PropellantGauges.Add(p, protoStageIconInfo);
		}
		else if (p.totalResourceCapacity != 0.0)
		{
			PropellantGauges[p].SetValue((float)UtilMath.Clamp01(p.totalResourceAvailable / p.totalResourceCapacity));
		}
		else
		{
			PropellantGauges[p].SetValue(0f);
		}
	}

	public virtual void Flameout(string message, bool statusOnly = false, bool showFX = true)
	{
		statusL2Field.guiActive = true;
		statusL2 = message;
		if (!statusOnly)
		{
			if (!flameout && showFX)
			{
				PlayFlameoutFX(flamingOut: true);
			}
			flameout = true;
			if (!allowRestart)
			{
				Shutdown();
			}
			status = cacheAutoLOC_219016;
		}
		currentThrottle = 0f;
	}

	public virtual void UnFlameout(bool showFX = true)
	{
		if (flameout && showFX)
		{
			PlayFlameoutFX(flamingOut: false);
		}
		flameout = false;
		status = cacheAutoLOC_219034;
		statusL2Field.guiActive = false;
		ActivateRunningFX();
	}

	public virtual bool CheckDeprived(double requiredPropellant, out string propName)
	{
		bool result = false;
		propName = "";
		int count = propellants.Count;
		for (int i = 0; i < count; i++)
		{
			Propellant propellant = propellants[i];
			propellant.currentRequirement = requiredPropellant * (double)propellant.ratio;
			if (propellant.totalResourceAvailable >= propellant.currentRequirement * (double)ignitionThreshold)
			{
				propellant.isDeprived = false;
				continue;
			}
			propellant.isDeprived = true;
			result = true;
			propName = propellant.displayName;
		}
		return result;
	}

	public virtual double RequestPropellant(double mass)
	{
		double num = (clampPropReceived ? 1.0 : 0.0);
		double num2 = mass * mixtureDensityRecip;
		double num3 = 0.0;
		if (num2 == 0.0)
		{
			return 0.0;
		}
		if (CheckDeprived(num2, out var propName))
		{
			Flameout(Localizer.Format("#autoLOC_219085", propName));
		}
		else
		{
			UnFlameout();
			int count = propellants.Count;
			for (int i = 0; i < count; i++)
			{
				Propellant propellant = propellants[i];
				if (clampPropReceived)
				{
					propellant.currentAmount = UtilMath.Clamp(base.part.RequestResource(propellant.id, Math.Min(propellant.totalResourceAvailable, propellant.currentRequirement * num), propellant.GetFlowMode()), 0.0, propellant.currentRequirement);
					double num4 = propellant.currentAmount / propellant.currentRequirement;
					if (num4 < num && num4 < clampPropReceivedMinLowerAmount)
					{
						num = num4;
					}
				}
				else
				{
					propellant.currentAmount = UtilMath.Clamp(base.part.RequestResource(propellant.id, Math.Min(propellant.totalResourceAvailable, propellant.currentRequirement), propellant.GetFlowMode()), 0.0, propellant.currentRequirement);
					num3 += propellant.currentAmount / propellant.currentRequirement;
				}
			}
			if (!clampPropReceived)
			{
				num = num3 / (double)count;
				num = UtilMath.Clamp(num, 0.0, 1.0);
			}
		}
		if (flameout)
		{
			return 0.0;
		}
		return num;
	}

	public override void OnLoad(ConfigNode node)
	{
		status = Localizer.Format("#autoLOC_219034");
		if (node.HasNode("PROPELLANT"))
		{
			propellants.Clear();
		}
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			string text = configNode.name;
			if (text == "PROPELLANT")
			{
				if (!configNode.HasValue("name"))
				{
					Debug.Log("Propellant must have value 'name'");
					continue;
				}
				Propellant propellant = new Propellant();
				propellant.Load(configNode);
				propellants.Add(propellant);
			}
		}
		SetupPropellant();
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int j = 0;
		for (int count2 = propellants.Count; j < count2; j++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(propellants[j].name));
		}
		if (node.HasValue("maxThrust"))
		{
			maxThrust = float.Parse(node.GetValue("maxThrust"));
			maxFuelFlow = maxThrust / (atmosphereCurve.Evaluate(0f) * g);
		}
		if (node.HasValue("minThrust"))
		{
			minThrust = float.Parse(node.GetValue("minThrust"));
			minFuelFlow = minThrust / (atmosphereCurve.Evaluate(0f) * g);
		}
		throttleMin = minFuelFlow / maxFuelFlow;
		if (throttleUseAlternate)
		{
			if (!throttleInstant)
			{
				if (throttleResponseRate <= 0f)
				{
					throttleResponseRate = (float)(throttlingBaseRate / Math.Log(Math.Max(throttlingBaseClamp, Math.Sqrt(throttlingBaseDivisor * (double)base.part.mass * (double)maxThrust * (double)maxThrust))));
				}
			}
			else
			{
				throttleResponseRate = 1000000f;
			}
		}
		if (node.HasValue("EngineType"))
		{
			engineType = (EngineType)Enum.Parse(typeof(EngineType), node.GetValue("EngineType"));
		}
		thrustTransforms = new List<Transform>(base.part.FindModelTransforms(thrustVectorTransformName));
		if (node.HasNode("transformMultipliers"))
		{
			thrustTransformMultipliers.Clear();
			ConfigNode node2 = node.GetNode("transformMultipliers");
			int num = node2.values.Count;
			float num2 = 0f;
			if (num == thrustTransforms.Count - 1)
			{
				num++;
			}
			if (num == thrustTransforms.Count)
			{
				for (int k = 0; k < num - 1; k++)
				{
					if (node2.HasValue("trf" + k) && float.TryParse(node2.GetValue("trf" + k), out var result))
					{
						thrustTransformMultipliers.Add(result);
						num2 += result;
					}
				}
				if (thrustTransformMultipliers.Count + 1 == num && num2 <= 1f)
				{
					thrustTransformMultipliers.Add(1f - num2);
				}
			}
		}
		if (thrustTransformMultipliers.Count != thrustTransforms.Count)
		{
			float item = 1f / (float)thrustTransforms.Count;
			thrustTransformMultipliers.Clear();
			int count3 = thrustTransforms.Count;
			while (count3-- > 0)
			{
				thrustTransformMultipliers.Add(item);
			}
		}
	}

	public void OnDestroy()
	{
		if (listenersSet)
		{
			GameEvents.onGamePause.Remove(OnPause);
			GameEvents.onGameUnpause.Remove(OnUnpause);
		}
		if (base.part.stackIcon != null)
		{
			ProtoStageIcon stackIcon = base.part.stackIcon;
			stackIcon.onStageIconDestroy = (Callback)Delegate.Remove(stackIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		}
		base.Fields["independentThrottle"].OnValueModified -= throttleModeChanged;
	}

	public void OnPause()
	{
		if ((bool)powerSfx)
		{
			powerSfx.volume = 0f;
		}
		if (runningGroup != null)
		{
			DeactivateRunningFX();
		}
	}

	public void OnUnpause()
	{
		if ((bool)powerSfx)
		{
			powerSfx.volume = GameSettings.SHIP_VOLUME;
		}
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001382")]
	public virtual void Activate()
	{
		if (!allowRestart && engineShutdown)
		{
			return;
		}
		if (base.part.ShieldedFromAirstream && !shieldedCanActivate)
		{
			ScreenMessages.PostScreenMessage("<color=orange>[" + base.part.partInfo.title + "]: " + cacheAutoLOC_219636 + "</color>", 6f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		if (!EngineIgnited)
		{
			PlayEngageFX();
		}
		EngineIgnited = true;
		if (allowShutdown)
		{
			shutdownEvent.active = true;
		}
		else
		{
			shutdownEvent.active = false;
		}
		activateEvent.active = false;
		propellantReqMetField.guiActive = true;
		if (!staged)
		{
			int count = base.part.Modules.Count;
			while (count-- > 0)
			{
				PartModule partModule = base.part.Modules[count];
				if (partModule is ModuleGimbal)
				{
					ModuleGimbal moduleGimbal = partModule as ModuleGimbal;
					if (!moduleGimbal.gimbalActive)
					{
						moduleGimbal.gimbalActive = true;
						moduleGimbal.OnStart(StartState.Flying);
					}
				}
			}
		}
		FXReset();
		GameEvents.onEngineActiveChange.Fire(this);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001381")]
	public virtual void Shutdown()
	{
		if (!allowShutdown)
		{
			return;
		}
		if (!allowRestart)
		{
			engineShutdown = true;
			shutdownEvent.active = false;
			activateEvent.active = false;
		}
		else
		{
			shutdownEvent.active = false;
			activateEvent.active = true;
		}
		propellantReqMetField.guiActive = false;
		EngineIgnited = false;
		PlayShutdownFX();
		int count = propellants.Count;
		for (int i = 0; i < count; i++)
		{
			Propellant key = propellants[i];
			if (PropellantGauges.ContainsKey(key))
			{
				base.part.stackIcon.RemoveInfo(PropellantGauges[key]);
			}
		}
		PropellantGauges.Clear();
		GameEvents.onEngineActiveChange.Fire(this);
	}

	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "")]
	public virtual void ToggleIncludeinDV()
	{
		includeinDVCalcs = !includeinDVCalcs;
		for (int i = 0; i < base.part.symmetryCounterparts.Count; i++)
		{
			ModuleEngines component = base.part.symmetryCounterparts[i].GetComponent<ModuleEngines>();
			if (component != null)
			{
				component.includeinDVCalcs = includeinDVCalcs;
				component.UpdateIncludeDVUI();
			}
		}
		UpdateIncludeDVUI();
		GameEvents.onChangeEngineDVIncludeState.Fire(this);
	}

	[KSPAction("#autoLOC_6001380", activeEditor = false)]
	public virtual void OnAction(KSPActionParam param)
	{
		if (!EngineIgnited)
		{
			Activate();
		}
		else
		{
			Shutdown();
		}
	}

	[KSPAction("#autoLOC_6001381", activeEditor = false)]
	public virtual void ShutdownAction(KSPActionParam param)
	{
		Shutdown();
	}

	[KSPAction("#autoLOC_6001382", activeEditor = false)]
	public virtual void ActivateAction(KSPActionParam param)
	{
		Activate();
	}

	[KSPAction("#autoLOC_6005052")]
	public virtual void ToggleThrottle(KSPActionParam param)
	{
		independentThrottle = !independentThrottle;
		throttleModeChanged(this);
	}

	public void thrustPercentChanged(BaseField field, object obj)
	{
		GameEvents.onEngineThrustPercentageChanged.Fire(this);
	}

	public void throttleModeChanged(object obj)
	{
		if (independentThrottlePercentField != null)
		{
			if (independentThrottle)
			{
				independentThrottlePercentField.SetSceneVisibility(UI_Scene.All, independentThrottle);
			}
			else
			{
				independentThrottlePercentField.SetSceneVisibility(UI_Scene.None, independentThrottle);
			}
		}
	}

	public void BurstFlameoutGroups()
	{
		int count = flameoutGroups.Count;
		for (int i = 0; i < count; i++)
		{
			flameoutGroups[i].Burst();
		}
	}

	public void SetRunningGroupsActive(bool active)
	{
		int count = runningGroups.Count;
		for (int i = 0; i < count; i++)
		{
			runningGroups[i].setActive(active);
		}
	}

	public void SetPowerGroupsActive(bool active)
	{
		int count = powerGroups.Count;
		for (int i = 0; i < count; i++)
		{
			powerGroups[i].setActive(active);
		}
	}

	public void HijackFX(FXGroup group, string groupName)
	{
		if (group != null)
		{
			FXGroup fXGroup = new FXGroup(fxGroupPrefix + groupName);
			int i = 0;
			for (int count = group.lights.Count; i < count; i++)
			{
				Light original = group.lights[i];
				fXGroup.lights.Add(UnityEngine.Object.Instantiate(original));
			}
			int j = 0;
			for (int count2 = group.fxEmittersNewSystem.Count; j < count2; j++)
			{
				ParticleSystem original2 = group.fxEmittersNewSystem[j];
				fXGroup.fxEmittersNewSystem.Add(UnityEngine.Object.Instantiate(original2));
			}
			fXGroup.sfx = group.sfx;
			fXGroup.audio = group.audio;
			base.part.fxGroups.Add(fXGroup);
			group = base.part.findFxGroup(fxGroupPrefix + groupName);
			group.setActive(value: false);
		}
	}

	public void SetupFXGroups()
	{
		flameoutGroup = base.part.findFxGroup("flameout");
		engageGroup = base.part.findFxGroup("engage");
		disengageGroup = base.part.findFxGroup("disengage");
		runningGroup = base.part.findFxGroup("running");
		powerGroup = base.part.findFxGroup("power");
		if (flameoutGroup != null)
		{
			HijackFX(flameoutGroup, "flameout");
		}
		if (engageGroup != null)
		{
			HijackFX(engageGroup, "engage");
		}
		if (disengageGroup != null)
		{
			HijackFX(disengageGroup, "disengage");
		}
		if (runningGroup != null)
		{
			HijackFX(runningGroup, "running");
		}
		if (powerGroup != null)
		{
			HijackFX(powerGroup, "power");
		}
		flameoutGroups = new List<FXGroup>();
		powerGroups = new List<FXGroup>();
		runningGroups = new List<FXGroup>();
		int num = 0;
		int i = 0;
		for (int count = thrustTransforms.Count; i < count; i++)
		{
			Transform thruster = thrustTransforms[i];
			if (flameoutGroup != null)
			{
				FXGroup fXGroup = new FXGroup(fxGroupPrefix + "Flameout" + num);
				int j = 0;
				for (int count2 = flameoutGroup.lights.Count; j < count2; j++)
				{
					Light original = flameoutGroup.lights[j];
					fXGroup.lights.Add(UnityEngine.Object.Instantiate(original));
				}
				int k = 0;
				for (int count3 = flameoutGroup.fxEmittersNewSystem.Count; k < count3; k++)
				{
					ParticleSystem original2 = flameoutGroup.fxEmittersNewSystem[k];
					fXGroup.fxEmittersNewSystem.Add(UnityEngine.Object.Instantiate(original2));
				}
				AutoPlaceFXGroup(fXGroup, thruster);
				flameoutGroups.Add(fXGroup);
				base.part.fxGroups.Add(fXGroup);
			}
			if (runningGroup != null)
			{
				FXGroup fXGroup2 = new FXGroup(fxGroupPrefix + "Running" + num);
				int l = 0;
				for (int count4 = runningGroup.lights.Count; l < count4; l++)
				{
					Light original3 = runningGroup.lights[l];
					fXGroup2.lights.Add(UnityEngine.Object.Instantiate(original3));
				}
				int m = 0;
				for (int count5 = runningGroup.fxEmittersNewSystem.Count; m < count5; m++)
				{
					ParticleSystem original4 = runningGroup.fxEmittersNewSystem[m];
					fXGroup2.fxEmittersNewSystem.Add(UnityEngine.Object.Instantiate(original4));
				}
				AutoPlaceFXGroup(fXGroup2, thruster);
				runningGroups.Add(fXGroup2);
				base.part.fxGroups.Add(fXGroup2);
			}
			if (powerGroup != null)
			{
				FXGroup fXGroup3 = new FXGroup(fxGroupPrefix + "Power" + num);
				int n = 0;
				for (int count6 = powerGroup.lights.Count; n < count6; n++)
				{
					Light original5 = powerGroup.lights[n];
					fXGroup3.lights.Add(UnityEngine.Object.Instantiate(original5));
				}
				int num2 = 0;
				for (int count7 = powerGroup.fxEmittersNewSystem.Count; num2 < count7; num2++)
				{
					ParticleSystem original6 = powerGroup.fxEmittersNewSystem[num2];
					fXGroup3.fxEmittersNewSystem.Add(UnityEngine.Object.Instantiate(original6));
				}
				AutoPlaceFXGroup(fXGroup3, thruster);
				powerGroups.Add(fXGroup3);
				base.part.fxGroups.Add(fXGroup3);
			}
			num++;
		}
		if (flameoutGroup != null)
		{
			int num3 = 0;
			for (int count8 = flameoutGroup.lights.Count; num3 < count8; num3++)
			{
				UnityEngine.Object.Destroy(flameoutGroup.lights[num3].gameObject);
			}
			int num4 = 0;
			for (int count9 = flameoutGroup.fxEmittersNewSystem.Count; num4 < count9; num4++)
			{
				UnityEngine.Object.Destroy(flameoutGroup.fxEmittersNewSystem[num4].gameObject);
			}
			flameoutGroup.lights.Clear();
			flameoutGroup.fxEmittersNewSystem.Clear();
		}
		if (powerGroup != null)
		{
			int num5 = 0;
			for (int count10 = powerGroup.lights.Count; num5 < count10; num5++)
			{
				UnityEngine.Object.Destroy(flameoutGroup.lights[num5].gameObject);
			}
			int num6 = 0;
			for (int count11 = powerGroup.fxEmittersNewSystem.Count; num6 < count11; num6++)
			{
				UnityEngine.Object.Destroy(powerGroup.fxEmittersNewSystem[num6].gameObject);
			}
			powerGroup.lights.Clear();
			powerGroup.fxEmittersNewSystem.Clear();
			SetPowerGroupsActive(active: false);
		}
		if (runningGroup != null)
		{
			int num7 = 0;
			for (int count12 = runningGroup.lights.Count; num7 < count12; num7++)
			{
				UnityEngine.Object.Destroy(runningGroup.lights[num7].gameObject);
			}
			int num8 = 0;
			for (int count13 = runningGroup.fxEmittersNewSystem.Count; num8 < count13; num8++)
			{
				UnityEngine.Object.Destroy(runningGroup.fxEmittersNewSystem[num8].gameObject);
			}
			runningGroup.lights.Clear();
			runningGroup.fxEmittersNewSystem.Clear();
			SetRunningGroupsActive(active: false);
		}
	}

	public void AutoPlaceFXGroup(FXGroup group, Transform thruster)
	{
		if (group == null || !(thruster != null))
		{
			return;
		}
		if (flameoutGroup != null)
		{
			int i = 0;
			for (int count = group.lights.Count; i < count; i++)
			{
				Light light = group.lights[i];
				light.transform.parent = thruster;
				Vector3 position = light.transform.position;
				if (!light.gameObject.name.Contains("Keep Pos"))
				{
					light.transform.localPosition = fxOffset;
				}
				else
				{
					light.transform.localPosition = position;
				}
				light.transform.localRotation = Quaternion.identity;
			}
		}
		int j = 0;
		for (int count2 = group.fxEmittersNewSystem.Count; j < count2; j++)
		{
			ParticleSystem particleSystem = group.fxEmittersNewSystem[j];
			Vector3 position2 = particleSystem.transform.position;
			particleSystem.transform.parent = thruster;
			if (!particleSystem.gameObject.name.Contains("Keep Pos"))
			{
				particleSystem.transform.localPosition = fxOffset;
			}
			else
			{
				particleSystem.transform.localPosition = position2;
			}
			particleSystem.transform.localRotation = Quaternion.identity;
			particleSystem.transform.Rotate(-90f, 0f, 0f);
		}
	}

	public virtual void InitializeFX()
	{
		SetupFXGroups();
		if (powerGroup != null)
		{
			powerSfx = base.gameObject.AddComponent<AudioSource>();
			powerSfx.playOnAwake = false;
			powerSfx.loop = true;
			powerSfx.rolloffMode = AudioRolloffMode.Logarithmic;
			powerSfx.dopplerLevel = 0f;
			powerSfx.volume = GameSettings.SHIP_VOLUME;
			powerSfx.spatialBlend = 1f;
			powerGroup.begin(powerSfx);
		}
	}

	public virtual void DeactivateLoopingFX()
	{
		DeactivateRunningFX();
		DeactivatePowerFX();
	}

	public virtual void DeactivateRunningFX()
	{
		if (runningGroup != null && runningGroup.Active)
		{
			runningGroup.setActiveWithAudioFade(value: false, 0.1f);
			SetRunningGroupsActive(active: false);
		}
	}

	public virtual void DeactivatePowerFX()
	{
		if (powerGroup != null && powerGroup.Active)
		{
			powerGroup.setActive(value: false);
			SetPowerGroupsActive(active: false);
		}
	}

	public void ActivateRunningFX()
	{
		if (runningGroup != null && !runningGroup.Active)
		{
			runningGroup.setActiveWithAudioFade(value: true, 0.1f);
			SetRunningGroupsActive(active: true);
		}
	}

	public void ActivatePowerFX()
	{
		if (powerGroup != null && !powerGroup.Active)
		{
			powerGroup.setActive(value: true);
			SetPowerGroupsActive(active: true);
		}
	}

	public virtual void PlayEngageFX()
	{
		DeactivateRunningFX();
		if (engageGroup != null)
		{
			engageGroup.Burst();
		}
		DeactivateRunningFX();
	}

	public virtual void PlayShutdownFX()
	{
		if (!flameout)
		{
			if (runningGroup != null && runningGroup.Active)
			{
				runningGroup.setActive(value: false);
				SetRunningGroupsActive(active: false);
			}
			if (disengageGroup != null)
			{
				disengageGroup.Burst();
			}
		}
		if (powerSfx != null && powerSfx.isPlaying)
		{
			powerSfx.Stop();
		}
	}

	public virtual void PlayFlameoutFX(bool flamingOut)
	{
		if (flameoutGroup != null)
		{
			flameoutGroup.Burst();
			BurstFlameoutGroups();
		}
		if (flamingOut)
		{
			DeactivateRunningFX();
		}
		else
		{
			ActivateRunningFX();
		}
	}

	public virtual void FXReset()
	{
		if (powerGroup != null)
		{
			powerGroup.setActive(value: false);
			SetPowerGroupsActive(active: false);
		}
		if (runningGroup != null)
		{
			runningGroup.setActive(value: false);
			SetRunningGroupsActive(active: false);
		}
	}

	public virtual void FXUpdate()
	{
		if (powerGroup != null)
		{
			if (EngineIgnited)
			{
				if (currentThrottle > 1E-05f)
				{
					if (!flameout)
					{
						ActivatePowerFX();
					}
					else
					{
						DeactivatePowerFX();
					}
				}
				else
				{
					DeactivatePowerFX();
				}
				powerGroup.Power = Mathf.Lerp(0.45f, 1.2f, currentThrottle);
				int count = powerGroups.Count;
				for (int i = 0; i < count; i++)
				{
					powerGroups[i].Power = Mathf.Lerp(0.45f, 1.2f, currentThrottle);
				}
			}
			else
			{
				DeactivatePowerFX();
			}
		}
		if (runningGroup == null)
		{
			return;
		}
		if (finalThrust != 0f && EngineIgnited && !flameout)
		{
			ActivateRunningFX();
			float power = Mathf.Max(Mathf.Min(finalThrust / (maxThrust * 0.5f), 1.75f), 0.5f);
			runningGroup.Power = power;
			int count2 = runningGroups.Count;
			for (int j = 0; j < count2; j++)
			{
				runningGroups[j].Power = power;
			}
		}
		else
		{
			DeactivateRunningFX();
		}
	}

	public virtual void EngineExhaustDamage()
	{
		if (!exhaustDamage)
		{
			return;
		}
		bool flag = false;
		string text = "None";
		int count = thrustTransforms.Count;
		for (int i = 0; i < count; i++)
		{
			Transform transform = thrustTransforms[i];
			if (!Physics.Raycast(transform.position, transform.forward, out hit, exhaustDamageMaxRange, LayerUtil.DefaultEquivalent | damageLayerMask))
			{
				continue;
			}
			Transform transform2 = hit.collider.transform;
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(transform2.gameObject);
			if (partUpwardsCached != null && partUpwardsCached != base.part && !transform2.GetComponentInChildren<physicalObject>())
			{
				double num = (double)(finalThrust * thrustTransformMultipliers[i]) * exhaustDamageMultiplier;
				double num2 = 10.0;
				num2 = (double)hit.distance + exhaustDamageDistanceOffset;
				if (num2 < 0.0)
				{
					num2 = 0.001;
				}
				double num3 = 1.0 / Math.Pow(num2, exhaustDamageFalloffPower);
				if (num3 > exhaustDamageMaxMutliplier)
				{
					num3 = exhaustDamageMaxMutliplier;
				}
				double num4 = 1.0 / Math.Pow(num2, exhaustDamageSplashbackFallofPower) * exhaustDamageSplashbackMult;
				if (num4 > exhaustDamageSplashbackMaxMutliplier)
				{
					num4 = exhaustDamageSplashbackMaxMutliplier;
				}
				partUpwardsCached.AddSkinThermalFlux(num * num3);
				base.part.AddSkinThermalFlux(num * num4);
				partUpwardsCached.AddForceAtPosition(transform.forward * finalThrust * thrustTransformMultipliers[i], hit.point);
				flag = true;
				text = partUpwardsCached.partInfo.title;
			}
		}
		if (flag && exhaustDamageLogEvent)
		{
			GameEvents.onSplashDamage.Fire(new EventReport(FlightEvents.SPLASHDAMAGE, base.part, text, base.part.partInfo.title));
		}
	}

	public virtual void UpdateThrottle()
	{
		if (throttleLocked)
		{
			requestedThrottle = (EngineIgnited ? (thrustPercentage * 0.01f) : 0f);
		}
		else if (independentThrottle)
		{
			requestedThrottle = (EngineIgnited ? (independentThrottlePercentage * 0.01f * (thrustPercentage * 0.01f)) : 0f);
		}
		else if (FlightGlobals.ready)
		{
			if (EngineIgnited)
			{
				if (base.part.isControllable)
				{
					requestedThrottle = base.vessel.ctrlState.mainThrottle * (thrustPercentage * 0.01f);
				}
			}
			else
			{
				requestedThrottle = 0f;
			}
		}
		if (throttleUseAlternate)
		{
			if (requestedThrottle > 0f)
			{
				requestedThrottle = Mathf.Lerp(throttleMin, 1f, requestedThrottle);
			}
			else if (throttleInstantShutdown)
			{
				currentThrottle = 0f;
				return;
			}
			if (throttleInstant)
			{
				currentThrottle = requestedThrottle;
				return;
			}
			float num = 0.01f * throttleIgniteLevelMult;
			float fixedDeltaTime = TimeWarp.fixedDeltaTime;
			float num2 = requestedThrottle - currentThrottle;
			if (num2 == 0f)
			{
				return;
			}
			float num3 = throttleResponseRate * fixedDeltaTime;
			float num4 = 1f;
			if (num2 < 0f)
			{
				num4 = -1f;
				num2 = 0f - num2;
				if (requestedThrottle <= num)
				{
					num3 *= throttleShutdownMult;
				}
			}
			if (currentThrottle > num)
			{
				float num5 = 1f - num2;
				num3 *= (1f - num5 * num5) * 5f * throttleStartedMult;
			}
			else
			{
				num3 *= 0.0005f + 4.05f * currentThrottle * throttleStartupMult;
			}
			if (num2 > num3 && num2 > 0.005f)
			{
				currentThrottle += num3 * num4;
			}
			else
			{
				currentThrottle = requestedThrottle;
			}
			return;
		}
		if (throttleMin != 0f)
		{
			requestedThrottle = Mathf.Lerp(throttleMin, 1f, requestedThrottle);
		}
		if (useEngineResponseTime)
		{
			if (Mathf.Abs(currentThrottle - requestedThrottle) <= 0.0001f)
			{
				currentThrottle = requestedThrottle;
			}
			else if (currentThrottle < requestedThrottle)
			{
				currentThrottle = Mathf.Lerp(currentThrottle, requestedThrottle, engineAccelerationSpeed * TimeWarp.fixedDeltaTime);
			}
			else
			{
				currentThrottle = Mathf.Lerp(currentThrottle, requestedThrottle, engineDecelerationSpeed * TimeWarp.fixedDeltaTime);
			}
		}
		else
		{
			currentThrottle = requestedThrottle;
		}
	}

	public double RequiredPropellantMass(float throttleAmount)
	{
		float num = (float)base.part.staticPressureAtm;
		realIsp = atmosphereCurve.Evaluate(num);
		if (useThrottleIspCurve)
		{
			realIsp *= GetThrottlingMult(num, throttleAmount);
		}
		requestedMassFlow = maxFuelFlow * throttleAmount;
		flowMultiplier = ModifyFlow();
		requestedMassFlow *= flowMultiplier * multFlow;
		if (useAtmCurveIsp)
		{
			realIsp *= atmCurveIsp.Evaluate((float)(base.part.atmDensity * 0.8163265147242933));
		}
		if (useVelCurveIsp)
		{
			realIsp *= velCurveIsp.Evaluate((float)base.part.machNumber);
		}
		float num2 = realIsp * g * multIsp;
		resultingThrust = requestedMassFlow * num2;
		return requestedMassFlow * TimeWarp.fixedDeltaTime;
	}

	public float CalculateThrust()
	{
		massFlow = 0.0;
		double num = 0.0;
		float num2 = base.vessel.VesselValues.FuelUsage.value;
		if (num2 == 0f)
		{
			num2 = 1f;
		}
		massFlow = RequiredPropellantMass(currentThrottle) * (double)num2;
		if (flowMultiplier < flameoutBar)
		{
			Flameout(cacheAutoLOC_220370);
			num = 0.0;
		}
		else if (disableUnderwater && base.vessel != null && base.vessel.mainBody.ocean && CheckTransformsUnderwater())
		{
			Flameout(cacheAutoLOC_220377);
			num = 0.0;
		}
		else if (CheatOptions.InfinitePropellant)
		{
			num = 1.0;
			if (currentThrottle > 0f)
			{
				UnFlameout();
			}
		}
		else
		{
			num = RequestPropellant(massFlow);
		}
		propellantReqMet = (float)(num * 100.0);
		float result = (float)((double)resultingThrust * num);
		fuelFlowGui = (float)(massFlow * num * mixtureDensityRecip * ratioSum) * (1f / TimeWarp.fixedDeltaTime);
		return result;
	}

	public bool CheckTransformsUnderwater()
	{
		int count = thrustTransforms.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (FlightGlobals.getAltitudeAtPos((Vector3d)thrustTransforms[count].position, base.vessel.mainBody) >= 0.0);
		return true;
	}

	public bool TimeWarping()
	{
		if ((TimeWarp.CurrentRate > 1f && TimeWarp.WarpMode == TimeWarp.Modes.HIGH) || base.part.packed)
		{
			DeactivateLoopingFX();
			return true;
		}
		return false;
	}

	public void ThrustUpdate()
	{
		isThrusting = false;
		if (EngineIgnited)
		{
			finalThrust = CalculateThrust() * base.vessel.VesselValues.EnginePower.value;
			if (finalThrust > 0f)
			{
				isThrusting = true;
				if (base.part.Rigidbody != null)
				{
					int i = 0;
					for (int count = thrustTransforms.Count; i < count; i++)
					{
						Transform transform = thrustTransforms[i];
						base.part.AddForceAtPosition(-transform.forward * finalThrust * thrustTransformMultipliers[i], transform.position);
					}
				}
				EngineExhaustDamage();
			}
			double num = 1.0;
			if (base.part.machNumber > (double)machLimit)
			{
				num += (base.part.machNumber - (double)machLimit) * base.part.atmDensity * (double)machHeatMult;
			}
			double kilowatts = num * (double)heatProduction * (normalizeHeatForFlow ? 1.0 : (1.0 / (double)flowMultiplier)) * (double)(finalThrust / maxThrust) * (double)base.vessel.VesselValues.HeatProduction.value * PhysicsGlobals.InternalHeatProductionFactor * base.part.thermalMass;
			base.part.AddThermalFlux(kilowatts);
		}
		else
		{
			shutdownEvent.active = false;
			activateEvent.active = true;
			propellantReqMetField.guiActive = false;
			fuelFlowGui = 0f;
			realIsp = 0f;
			finalThrust = 0f;
			if (base.part.ShieldedFromAirstream)
			{
				status = cacheAutoLOC_220473;
			}
			else
			{
				status = cacheAutoLOC_220477;
			}
		}
	}

	public void FixedUpdate()
	{
		if (TimeWarping())
		{
			finalThrust = 0f;
			return;
		}
		UpdateThrottle();
		currentThrottle = ApplyThrottleAdjustments(currentThrottle);
		if (EngineIgnited)
		{
			UpdatePropellantStatus();
		}
		ThrustUpdate();
		FXUpdate();
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		statusL2Field = base.Fields["statusL2"];
		damageLayerMask = 1 << LayerMask.NameToLayer("Parts");
		if (propellants == null)
		{
			propellants = new List<Propellant>();
		}
		if (atmosphereCurve == null)
		{
			atmosphereCurve = new FloatCurve();
		}
		if (thrustTransformMultipliers == null)
		{
			thrustTransformMultipliers = new List<float>();
		}
		PropellantGauges = new Dictionary<Propellant, ProtoStageIconInfo>();
		if (base.part.stackIcon != null)
		{
			ProtoStageIcon stackIcon = base.part.stackIcon;
			stackIcon.onStageIconDestroy = (Callback)Delegate.Combine(stackIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		}
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = propellants.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(propellants[i].name));
		}
	}

	public void OnStageIconDestroy()
	{
		PropellantGauges = new Dictionary<Propellant, ProtoStageIconInfo>();
	}

	public override void OnStart(StartState state)
	{
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "LIQUID_ENGINE";
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			InitializeFX();
		}
		shutdownEvent = base.Events["Shutdown"];
		activateEvent = base.Events["Activate"];
		propellantReqMetField = base.Fields["propellantReqMet"];
		shutdownEvent.active = false;
		activateEvent.active = false;
		UI_Control uiControlFlight = base.Fields["thrustPercentage"].uiControlFlight;
		uiControlFlight.onFieldChanged = (Callback<BaseField, object>)Delegate.Combine(uiControlFlight.onFieldChanged, new Callback<BaseField, object>(thrustPercentChanged));
		UI_Control uiControlEditor = base.Fields["thrustPercentage"].uiControlEditor;
		uiControlEditor.onFieldChanged = (Callback<BaseField, object>)Delegate.Combine(uiControlEditor.onFieldChanged, new Callback<BaseField, object>(thrustPercentChanged));
		base.Fields["independentThrottle"].OnValueModified += throttleModeChanged;
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("independentThrottlePercentage", out independentThrottlePercentField);
		if (!independentThrottle && independentThrottlePercentField != null)
		{
			independentThrottlePercentField.SetSceneVisibility(UI_Scene.None, state: false);
		}
		UpdateIncludeDVUI();
		if (state != StartState.PreLaunch)
		{
			if (EngineIgnited)
			{
				if (allowShutdown)
				{
					shutdownEvent.active = true;
				}
				else
				{
					shutdownEvent.active = false;
				}
				activateEvent.active = false;
				propellantReqMetField.guiActive = true;
			}
			else
			{
				shutdownEvent.active = false;
				if (!allowRestart && engineShutdown)
				{
					activateEvent.active = false;
				}
				else
				{
					activateEvent.active = true;
				}
				propellantReqMetField.guiActive = false;
			}
		}
		if (throttleLocked)
		{
			base.Fields["thrustPercentage"].guiActive = false;
			base.Fields["independentThrottle"].guiActive = false;
			base.Fields["independentThrottlePercentage"].guiActive = false;
		}
		base.Fields["thrustCurveDisplay"].guiActive = useThrustCurve;
		SetListener();
		thrustTransforms = new List<Transform>(base.part.FindModelTransforms(thrustVectorTransformName));
	}

	public virtual void SetListener()
	{
		GameEvents.onGamePause.Add(OnPause);
		GameEvents.onGameUnpause.Add(OnUnpause);
		listenersSet = true;
	}

	public override void OnActive()
	{
		if (!EngineIgnited && !manuallyOverridden && stagingEnabled && !staged)
		{
			if (!IsEngineDead())
			{
				Activate();
			}
			staged = EngineIgnited;
		}
	}

	public override void OnInventoryModeDisable()
	{
		flameoutGroup = base.part.findFxGroup("flameout");
		engageGroup = base.part.findFxGroup("engage");
		disengageGroup = base.part.findFxGroup("disengage");
		runningGroup = base.part.findFxGroup("running");
		powerGroup = base.part.findFxGroup("power");
		if (flameoutGroup != null)
		{
			ParticlesStop(flameoutGroup.fxEmittersNewSystem);
		}
		if (engageGroup != null)
		{
			ParticlesStop(engageGroup.fxEmittersNewSystem);
		}
		if (disengageGroup != null)
		{
			ParticlesStop(disengageGroup.fxEmittersNewSystem);
		}
		if (runningGroup != null)
		{
			ParticlesStop(runningGroup.fxEmittersNewSystem);
		}
		if (powerGroup != null)
		{
			ParticlesStop(powerGroup.fxEmittersNewSystem);
		}
		if (runningGroups != null)
		{
			for (int i = 0; i < runningGroups.Count; i++)
			{
				if (runningGroups[i] != null)
				{
					ParticlesStop(runningGroups[i].fxEmittersNewSystem);
				}
			}
		}
		if (flameoutGroups != null)
		{
			for (int j = 0; j < flameoutGroups.Count; j++)
			{
				if (flameoutGroups[j] != null)
				{
					ParticlesStop(flameoutGroups[j].fxEmittersNewSystem);
				}
			}
		}
		if (powerGroups == null)
		{
			return;
		}
		for (int k = 0; k < powerGroups.Count; k++)
		{
			if (powerGroups[k] != null)
			{
				ParticlesStop(powerGroups[k].fxEmittersNewSystem);
			}
		}
	}

	public void ParticlesStop(List<ParticleSystem> particles)
	{
		if (particles == null)
		{
			return;
		}
		for (int i = 0; i < particles.Count; i++)
		{
			if (particles[i] != null)
			{
				particles[i].Stop();
			}
		}
	}

	public void UpdateIncludeDVUI()
	{
		if (nonThrustMotor)
		{
			base.Events["ToggleIncludeinDV"].active = true;
			base.Events["ToggleIncludeinDV"].guiActive = true;
			base.Events["ToggleIncludeinDV"].guiActiveEditor = true;
			base.Events["ToggleIncludeinDV"].guiName = Localizer.Format("#autoLOC_8002216", Convert.ToInt32(includeinDVCalcs));
		}
		else
		{
			base.Events["ToggleIncludeinDV"].active = false;
			base.Events["ToggleIncludeinDV"].guiActive = false;
			base.Events["ToggleIncludeinDV"].guiActiveEditor = false;
		}
	}

	public string GetModuleTitle()
	{
		return "Engine";
	}

	public virtual float MaxThrustOutputAtm(bool runningActive = false, bool useThrustLimiter = true, float atmPressure = 1f, double atmTemp = 310.0, double atmDensity = 1.225000023841858)
	{
		float throttle = (useThrustLimiter ? (thrustPercentage * 0.01f) : 1f);
		throttle = ApplyThrottleAdjustments(throttle);
		float num = GetEngineThrust(atmosphereCurve.Evaluate(atmPressure), throttle);
		if (useThrottleIspCurve)
		{
			num *= GetThrottlingMult(atmPressure, throttle);
		}
		float num2 = 1f;
		if (atmChangeFlow)
		{
			num2 = (float)(atmDensity * 0.8163265147242933);
			if (useAtmCurve)
			{
				num2 = atmCurve.Evaluate(num2);
			}
		}
		if (useVelCurve)
		{
			num2 *= velCurve.Evaluate((float)base.part.machNumber);
		}
		float num3 = 1f;
		float num4 = 1f;
		if (useThrustCurve)
		{
			int count = propellants.Count;
			while (count-- > 0)
			{
				Propellant propellant = propellants[count];
				if (!propellant.ignoreForThrustCurve && (runningActive || !(propellant.name == "IntakeAir")))
				{
					float num5 = (float)(propellant.totalResourceAvailable / propellant.totalResourceCapacity);
					if (num5 < num4)
					{
						num4 = num5;
					}
				}
			}
			num3 = thrustCurve.Evaluate(num4);
			num2 *= num3;
		}
		if (num2 > flowMultCap)
		{
			float num6 = num2 - flowMultCap;
			num2 = flowMultCap + num6 / (flowMultCapSharpness + num6 / flowMultCap);
		}
		if (num2 < CLAMP)
		{
			num2 = CLAMP;
		}
		return num * num2;
	}

	public virtual float MaxThrustOutputVac(bool useThrustLimiter = true)
	{
		int num = 0;
		while (true)
		{
			if (num < propellants.Count)
			{
				if (propellants[num].name == "IntakeAir")
				{
					break;
				}
				num++;
				continue;
			}
			float throttle = (useThrustLimiter ? (thrustPercentage * 0.01f) : 1f);
			throttle = ApplyThrottleAdjustments(throttle);
			float num2 = GetEngineThrust(atmosphereCurve.Evaluate(0f), throttle);
			if (useThrottleIspCurve)
			{
				num2 *= GetThrottlingMult(0f, 1f);
			}
			return num2;
		}
		return 0f;
	}

	public double MassFlow()
	{
		return massFlow;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_220657;
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public virtual string GetInfoThrust(bool mainInfoWindow)
	{
		string text = "";
		float num = GetEngineThrust(atmosphereCurve.Evaluate(1f), 1f);
		float num2 = GetEngineThrust(atmosphereCurve.Evaluate(0f), 1f);
		if (useThrottleIspCurve)
		{
			num *= GetThrottlingMult(1f, 1f);
			num2 *= GetThrottlingMult(0f, 1f);
		}
		if (atmChangeFlow)
		{
			if (useVelCurve)
			{
				if (mainInfoWindow)
				{
					velCurve.FindMinMaxValue(out var _, out var max, out var _, out var tMax);
					if (max > flowMultCap)
					{
						float num3 = max - flowMultCap;
						max = flowMultCap + num3 / (flowMultCapSharpness + num3 / flowMultCap);
					}
					text += Localizer.Format("#autoLOC_220691", (num * max).ToString("0.0##"), tMax.ToString("0.#"));
				}
				text += Localizer.Format("#autoLOC_220695", num.ToString("0.0##"));
			}
			else
			{
				text += Localizer.Format("#autoLOC_220700", num.ToString("0.0##"));
			}
		}
		else
		{
			string text2 = "";
			if (throttleLocked)
			{
				text2 = cacheAutoLOC_6001000;
			}
			if (num != num2)
			{
				text += Localizer.Format("#autoLOC_6001002", text2, num.ToString("0.0##"));
				text += Localizer.Format("#autoLOC_6001003", text2, num2.ToString("0.0##"));
			}
			else
			{
				text += Localizer.Format("#autoLOC_6001004", text2, num2.ToString("0.0##"));
			}
			if (mainInfoWindow && !throttleLocked)
			{
				text2 = cacheAutoLOC_6001001;
				float num4 = GetEngineThrust(atmosphereCurve.Evaluate(1f), 0f);
				float num5 = GetEngineThrust(atmosphereCurve.Evaluate(0f), 0f);
				if (useThrottleIspCurve)
				{
					num4 *= GetThrottlingMult(1f, 0f);
					num5 *= GetThrottlingMult(0f, 0f);
				}
				if (num4 != num5)
				{
					text += Localizer.Format("#autoLOC_6001002", text2, num4.ToString("0.0##"));
					text += Localizer.Format("#autoLOC_6001003", text2, num5.ToString("0.0##"));
				}
				else
				{
					text += Localizer.Format("#autoLOC_6001004", text2, num5.ToString("0.0##"));
				}
			}
		}
		return text;
	}

	public override string GetInfo()
	{
		string infoThrust = GetInfoThrust(mainInfoWindow: true);
		infoThrust += Localizer.Format("#autoLOC_220745", atmosphereCurve.Evaluate(1f).ToString("0.###"), atmosphereCurve.Evaluate(0f).ToString("0.###"));
		infoThrust += cacheAutoLOC_220748;
		int i = 0;
		for (int count = propellants.Count; i < count; i++)
		{
			Propellant propellant = propellants[i];
			string text = KSPUtil.PrintModuleName(propellant.displayName);
			infoThrust += Localizer.Format("#autoLOC_220756", text, getMaxFuelFlow(propellant).ToString("0.0##"));
			infoThrust += propellant.GetFlowModeDescription();
		}
		infoThrust += Localizer.Format("#autoLOC_220759", (ignitionThreshold * 100f).ToString("0.#"));
		if (!allowShutdown)
		{
			infoThrust += cacheAutoLOC_220761;
		}
		if (!allowRestart)
		{
			infoThrust += cacheAutoLOC_220762;
		}
		return infoThrust;
	}

	public string GetPrimaryField()
	{
		return GetInfoThrust(mainInfoWindow: false);
	}

	public virtual float getMaxFuelFlow(Propellant p)
	{
		return getFuelFlow(p, maxFuelFlow);
	}

	public virtual float getFuelFlow(Propellant p, float fuelFlow)
	{
		fuelFlow = Mathf.Clamp(fuelFlow, minFuelFlow, maxFuelFlow);
		return (float)((double)fuelFlow * mixtureDensityRecip * (double)p.ratio);
	}

	public float getExhaustVelocity(float isp)
	{
		return isp * g;
	}

	public float GetEngineThrust(float isp, float throttle)
	{
		return Mathf.Lerp(minFuelFlow, maxFuelFlow, throttle) * getExhaustVelocity(isp);
	}

	public float GetThrottlingMult(float atm, float throttle)
	{
		return Mathf.Lerp(1f, throttleIspCurve.Evaluate(throttle), throttleIspCurveAtmStrength.Evaluate(atm));
	}

	public virtual float ModifyFlow()
	{
		float num = 1f;
		if (atmChangeFlow)
		{
			num = (float)(base.part.atmDensity * 0.8163265147242933);
			if (useAtmCurve)
			{
				num = atmCurve.Evaluate(num);
			}
		}
		if (useVelCurve)
		{
			num *= velCurve.Evaluate((float)base.part.machNumber);
		}
		if (useThrustCurve)
		{
			float num2 = 1f;
			thrustCurveRatio = 1f;
			thrustCurveDisplay = num2;
			int count = propellants.Count;
			while (count-- > 0)
			{
				Propellant propellant = propellants[count];
				if (!propellant.ignoreForThrustCurve)
				{
					float num3 = (float)(propellant.totalResourceAvailable / propellant.totalResourceCapacity);
					if (num3 < thrustCurveRatio)
					{
						thrustCurveRatio = num3;
					}
				}
			}
			thrustCurveDisplay = thrustCurve.Evaluate(thrustCurveRatio);
			num *= thrustCurveDisplay;
		}
		if (num > flowMultCap)
		{
			float num4 = num - flowMultCap;
			num = flowMultCap + num4 / (flowMultCapSharpness + num4 / flowMultCap);
		}
		if (num < CLAMP)
		{
			num = CLAMP;
		}
		return num;
	}

	public virtual void OnCenterOfThrustQuery(CenterOfThrustQuery qry)
	{
		if (!manuallyOverridden && !(this == null))
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			int count = thrustTransforms.Count;
			for (int i = 0; i < count; i++)
			{
				Transform transform = thrustTransforms[i];
				zero += transform.transform.position * thrustTransformMultipliers[i];
				zero2 += transform.transform.forward * thrustTransformMultipliers[i];
			}
			qry.pos = zero;
			qry.dir = zero2;
			qry.thrust = maxThrust * (thrustPercentage * 0.01f);
		}
	}

	public float GetMaxThrust()
	{
		return maxThrust;
	}

	public float GetCurrentThrust()
	{
		return finalThrust;
	}

	public EngineType GetEngineType()
	{
		return engineType;
	}

	public override bool IsStageable()
	{
		return true;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterEnginesBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterEnginesBase item = adjuster as AdjusterEnginesBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyThrottleAdjustments(float throttle)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			throttle = adjusterCache[i].ApplyThrottleAdjustment(throttle);
		}
		return throttle;
	}

	public bool IsEngineDead()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsEngineDead())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public virtual bool CanBeDetached()
	{
		return !isThrusting;
	}

	public virtual bool CanBeOffset()
	{
		return !isThrusting;
	}

	public virtual bool CanBeRotated()
	{
		return !isThrusting;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_220473 = Localizer.Format("#autoLOC_220473");
		cacheAutoLOC_220477 = Localizer.Format("#autoLOC_220477");
		cacheAutoLOC_219016 = Localizer.Format("#autoLOC_219016");
		cacheAutoLOC_219034 = Localizer.Format("#autoLOC_219034");
		cacheAutoLOC_219636 = Localizer.Format("#autoLOC_219636");
		cacheAutoLOC_220370 = Localizer.Format("#autoLOC_220370");
		cacheAutoLOC_220377 = Localizer.Format("#autoLOC_220377");
		cacheAutoLOC_220657 = Localizer.Format("#autoLOC_220657");
		cacheAutoLOC_6001000 = Localizer.Format("#autoLOC_6001000");
		cacheAutoLOC_6001001 = Localizer.Format("#autoLOC_6001001");
		cacheAutoLOC_220748 = Localizer.Format("#autoLOC_220748");
		cacheAutoLOC_220761 = Localizer.Format("#autoLOC_220761");
		cacheAutoLOC_220762 = Localizer.Format("#autoLOC_220762");
	}
}
