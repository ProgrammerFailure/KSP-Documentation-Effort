using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns11;
using ns9;
using UnityEngine;

public class ModuleRCS : PartModule, IResourceConsumer, ITorqueProvider, IConstruction
{
	public List<Transform> thrusterTransforms;

	public List<FXGroup> thrusterFX;

	public float[] thrustForces;

	[KSPField]
	public string thrusterTransformName = "RCSthruster";

	[KSPField]
	public bool useZaxis;

	[KSPField]
	public float thrusterPower = 1f;

	[KSPField]
	public string resourceName = "MonoPropellant";

	[KSPField]
	public string fxPrefabName = "fx_gasJet_white";

	[KSPField]
	public string fxPrefix = "rcsGroup";

	[KSPField]
	public Vector3 fxOffset = Vector3.zero;

	[KSPField]
	public Vector3 fxOffsetRot = Vector3.zero;

	[KSPField]
	public bool isJustForShow;

	[KSPField]
	public bool requiresFuel = true;

	[KSPField]
	public bool shieldedCanThrust;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001362")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.Editor)]
	public bool rcsEnabled = true;

	[UI_FloatRange(stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage = 100f;

	[KSPField]
	public float fullThrustMin = 0.2f;

	[KSPField]
	public bool useLever = true;

	[KSPField]
	public float precisionFactor = 0.1f;

	[KSPField]
	public bool useThrustCurve;

	[KSPField]
	public FloatCurve thrustCurve = new FloatCurve();

	[KSPField(guiFormat = "P3", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001830")]
	public float thrustCurveDisplay = 1f;

	[KSPField(guiFormat = "P3", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001831")]
	public float thrustCurveRatio = 1f;

	[KSPField]
	public bool showToggles = true;

	[KSPField(isPersistant = true)]
	public bool currentShowToggles;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001331")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableYaw = true;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enablePitch = true;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001332")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableRoll = true;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001364")]
	public bool enableX = true;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001365")]
	public bool enableY = true;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001366")]
	public bool enableZ = true;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001367")]
	public bool useThrottle;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001368")]
	public bool fullThrust;

	[KSPField]
	public float EPSILON = 0.05f;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001369", guiUnits = "#autoLOC_7001400")]
	public float realISP;

	[KSPField]
	public string resourceFlowMode = "STAGE_PRIORITY_FLOW";

	public double double_0 = 9.80665;

	[KSPField]
	public FloatCurve atmosphereCurve;

	public float leverDistance;

	public Vector3 inputRot = Vector3.zero;

	public Vector3 inputLin = Vector3.zero;

	public Vector3 rot;

	public int tC;

	public bool usePrecision = true;

	public bool rcs_active;

	public bool flameout;

	public double exhaustVel;

	public double maxFuelFlow;

	public double flowMult = 1.0;

	public double ispMult = 1.0;

	public float currentThrustForce;

	public float totalThrustForce;

	public float thrustForceRecip;

	public Transform currentThruster;

	public Vector3 predictedCOM;

	public Vector3 direction;

	public bool isOperating;

	public List<PartResourceDefinition> consumedResources;

	public Propellant p;

	public List<Propellant> propellants;

	public Dictionary<Propellant, ProtoStageIconInfo> PropellantGauges;

	public double mixtureDensity;

	public double mixtureDensityRecip;

	public float ignitionThreshold = 0.1f;

	public List<AdjusterRCSBase> adjusterCache = new List<AdjusterRCSBase>();

	public static string cacheAutoLOC_217939;

	public static string cacheAutoLOC_217950;

	public static string cacheAutoLOC_7003240;

	public static string cacheAutoLOC_7003241;

	public static string cacheAutoLOC_218287;

	public static string cacheAutoLOC_218288;

	public static string cacheAutoLOC_6003050;

	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001384")]
	public void ToggleToggles()
	{
		currentShowToggles = !currentShowToggles;
		UpdateToggles();
	}

	[KSPAction("#autoLOC_6001370")]
	public void ToggleAction(KSPActionParam param)
	{
		rcsEnabled = !rcsEnabled;
	}

	public void Update()
	{
		if (!(base.part.vessel != null))
		{
			return;
		}
		float num = EPSILON * EPSILON;
		float num2 = base.vessel.ctrlState.float_2;
		if (num2 * num2 < num)
		{
			if (useThrottle)
			{
				num2 -= base.vessel.ctrlState.mainThrottle;
				num2 = Mathf.Clamp(num2, -1f, 1f);
			}
			else
			{
				num2 = 0f;
			}
		}
		inputRot = new Vector3(enablePitch ? base.vessel.ctrlState.pitch : 0f, enableRoll ? base.vessel.ctrlState.roll : 0f, enableYaw ? base.vessel.ctrlState.yaw : 0f);
		inputLin = new Vector3(enableX ? base.vessel.ctrlState.float_0 : 0f, enableZ ? num2 : 0f, enableY ? base.vessel.ctrlState.float_1 : 0f);
		if (inputRot.x * inputRot.x < num)
		{
			inputRot.x = 0f;
		}
		if (inputRot.y * inputRot.y < num)
		{
			inputRot.y = 0f;
		}
		if (inputRot.z * inputRot.z < num)
		{
			inputRot.z = 0f;
		}
		if (inputLin.x * inputLin.x < num)
		{
			inputLin.x = 0f;
		}
		if (inputLin.z * inputLin.z < num)
		{
			inputLin.z = 0f;
		}
		Quaternion rotation = base.vessel.ReferenceTransform.rotation;
		inputRot = rotation * inputRot;
		inputLin = rotation * inputLin;
		inputRot = ApplyInputRotationAdjustments(inputRot);
		inputLin = ApplyInputLinearAdjustments(inputLin);
		if (FlightInputHandler.fetch != null)
		{
			usePrecision = FlightInputHandler.fetch.precisionMode;
		}
	}

	public float GetLeverDistanceOriginal(Vector3 leverPivotPosition)
	{
		return Vector3.Distance(base.transform.position, leverPivotPosition);
	}

	public float GetLeverDistance(Transform trf, Vector3 axis, Vector3 leverPivotPosition)
	{
		direction = leverPivotPosition - trf.position;
		float magnitude = direction.magnitude;
		float num = Vector3.Dot(direction / magnitude, axis);
		return Mathf.Sqrt(1f - num * num) * magnitude;
	}

	public void FixedUpdate()
	{
		isOperating = false;
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (TimeWarp.CurrentRate > 1f && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			DeactivatePowerFX();
			return;
		}
		tC = thrusterTransforms.Count;
		if (thrustForces.Length != tC)
		{
			thrustForces = new float[tC];
		}
		bool success = false;
		int num = tC;
		while (num-- > 0)
		{
			thrustForces[num] = 0f;
		}
		totalThrustForce = 0f;
		realISP = atmosphereCurve.Evaluate((float)base.part.staticPressureAtm);
		exhaustVel = (double)realISP * double_0 * ispMult;
		thrustForceRecip = 1f / thrusterPower;
		if (moduleIsEnabled && base.vessel != null && rcsEnabled && !IsAdjusterBreakingRCS() && (!base.part.ShieldedFromAirstream || shieldedCanThrust))
		{
			bool flag;
			if ((flag = base.vessel.ActionGroups[KSPActionGroup.flag_5]) != rcs_active)
			{
				rcs_active = flag;
			}
			if (rcs_active && (inputRot != Vector3.zero || inputLin != Vector3.zero))
			{
				predictedCOM = base.vessel.CurrentCoM;
				for (int i = 0; i < tC; i++)
				{
					currentThruster = thrusterTransforms[i];
					if (currentThruster.position == Vector3.zero || !currentThruster.gameObject.activeInHierarchy)
					{
						continue;
					}
					Vector3 vector = ((!useZaxis) ? currentThruster.up : currentThruster.forward);
					rot = Vector3.Cross(inputRot, Vector3.ProjectOnPlane(currentThruster.position - predictedCOM, inputRot).normalized);
					currentThrustForce = Mathf.Max(Vector3.Dot(vector, rot), 0f);
					currentThrustForce += Mathf.Max(Vector3.Dot(vector, inputLin), 0f);
					if (currentThrustForce > 1f)
					{
						currentThrustForce = 1f;
					}
					if (fullThrust && currentThrustForce >= fullThrustMin)
					{
						currentThrustForce = 1f;
					}
					if (usePrecision)
					{
						if (useLever)
						{
							leverDistance = GetLeverDistance(currentThruster, -vector, predictedCOM);
							if (leverDistance > 1f)
							{
								currentThrustForce /= leverDistance;
							}
						}
						else
						{
							currentThrustForce *= precisionFactor;
						}
					}
					UpdatePropellantStatus();
					currentThrustForce = CalculateThrust(currentThrustForce, out success);
					thrustForces[i] = currentThrustForce;
					bool flag2 = currentThrustForce > 0f && success;
					isOperating |= flag2;
					UpdatePowerFX(flag2, i, Mathf.Clamp(currentThrustForce * thrustForceRecip, 0.1f, 1f));
					if (flag2 && !isJustForShow)
					{
						totalThrustForce += currentThrustForce;
						base.part.AddForceAtPosition(-vector * currentThrustForce, currentThruster.transform.position);
					}
				}
			}
			else
			{
				DeactivateFX();
			}
		}
		else
		{
			DeactivateFX();
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		thrusterTransforms = new List<Transform>();
		thrusterFX = new List<FXGroup>();
		if (atmosphereCurve == null)
		{
			atmosphereCurve = new FloatCurve();
		}
		if (propellants == null)
		{
			propellants = new List<Propellant>();
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
		PropellantGauges = new Dictionary<Propellant, ProtoStageIconInfo>();
		if (base.part.stackIcon != null)
		{
			ProtoStageIcon stackIcon = base.part.stackIcon;
			stackIcon.onStageIconDestroy = (Callback)Delegate.Combine(stackIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		}
		if (base.part.partInfo == null || base.part.partInfo.partPrefab == null)
		{
			stagingEnabled = false;
		}
	}

	public void OnStageIconDestroy()
	{
		PropellantGauges = new Dictionary<Propellant, ProtoStageIconInfo>();
	}

	public override void OnLoad(ConfigNode node)
	{
		if (!node.HasNode("PROPELLANT") && propellants.Count == 0)
		{
			ConfigNode configNode = new ConfigNode("PROPELLANT");
			configNode.AddValue("name", resourceName);
			configNode.AddValue("ratio", "1.0");
			configNode.AddValue("resourceFlowMode", resourceFlowMode);
			node.AddNode(configNode);
		}
		SetupPropellant(node);
		maxFuelFlow = getMaxFuelFlow(atmosphereCurve.Evaluate(0f));
	}

	public override void OnStart(StartState state)
	{
		base.part.stackIcon.SetIcon(DefaultIcons.RCS_MODULE);
		base.part.stackIconGrouping = StackIconGrouping.SAME_TYPE;
		FindThrusters();
		SetupFX();
		UpdateToggles();
		base.Fields["thrustCurveDisplay"].guiActive = useThrustCurve && moduleIsEnabled;
		BaseField baseField = base.Fields["rcsEnabled"];
		bool guiActive = (base.Fields["rcsEnabled"].guiActiveEditor = moduleIsEnabled);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["thrustPercentage"];
		guiActive = (base.Fields["thrustPercentage"].guiActiveEditor = moduleIsEnabled);
		baseField2.guiActive = guiActive;
		base.Actions["ToggleAction"].active = moduleIsEnabled;
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_217936", thrusterPower.ToString("0.0###"));
		text += Localizer.Format("#autoLOC_217937", atmosphereCurve.Evaluate(1f).ToString("0.###"), atmosphereCurve.Evaluate(0f).ToString("0.###"));
		text += cacheAutoLOC_217939;
		int i = 0;
		for (int count = propellants.Count; i < count; i++)
		{
			Propellant propellant = propellants[i];
			text += Localizer.Format("#autoLOC_217946", propellant.displayName, getMaxFuelFlow(propellant, atmosphereCurve.Evaluate(0f)).ToString("0.0###"));
		}
		if (!moduleIsEnabled)
		{
			text += cacheAutoLOC_217950;
		}
		return text;
	}

	public override void OnActive()
	{
		if (stagingEnabled && moduleIsEnabled)
		{
			rcsEnabled = true;
		}
	}

	public void OnDestroy()
	{
		if (base.part.stackIcon != null)
		{
			ProtoStageIcon stackIcon = base.part.stackIcon;
			stackIcon.onStageIconDestroy = (Callback)Delegate.Remove(stackIcon.onStageIconDestroy, new Callback(OnStageIconDestroy));
		}
	}

	public void SetResource(string resource)
	{
		resourceName = resource;
		OnLoad(new ConfigNode());
	}

	public void FindThrusters()
	{
		thrusterTransforms = new List<Transform>(base.part.FindModelTransforms(thrusterTransformName));
		if (thrusterTransforms == null)
		{
			Debug.Log("RCS module unable to find any transforms in part named " + thrusterTransformName);
			return;
		}
		int count = thrusterTransforms.Count;
		if (thrustForces.Length != count)
		{
			thrustForces = new float[count];
		}
		if (count == 0)
		{
			Debug.Log("RCS module unable to find any transforms in part named " + thrusterTransformName);
		}
	}

	public double getMaxFuelFlow(float Isp)
	{
		exhaustVel = (double)Isp * double_0;
		if (mixtureDensity > 0.0)
		{
			return (double)thrusterPower / exhaustVel;
		}
		return 0.0;
	}

	public double getMaxFuelFlow(Propellant p, float Isp)
	{
		exhaustVel = (double)Isp * double_0;
		double num = getMaxFuelFlow(Isp);
		if (num > 0.0)
		{
			if (PartResourceLibrary.Instance.GetDefinition(p.id) != null)
			{
				return num * mixtureDensityRecip * (double)p.ratio;
			}
			return double.NaN;
		}
		return 0.0;
	}

	public void UpdateToggles()
	{
		bool flag = showToggles && currentShowToggles && moduleIsEnabled;
		BaseField baseField = base.Fields["enableYaw"];
		bool guiActive = (base.Fields["enableYaw"].guiActiveEditor = flag);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["enablePitch"];
		guiActive = (base.Fields["enablePitch"].guiActiveEditor = flag);
		baseField2.guiActive = guiActive;
		BaseField baseField3 = base.Fields["enableRoll"];
		guiActive = (base.Fields["enableRoll"].guiActiveEditor = flag);
		baseField3.guiActive = guiActive;
		BaseField baseField4 = base.Fields["enableX"];
		guiActive = (base.Fields["enableX"].guiActiveEditor = flag);
		baseField4.guiActive = guiActive;
		BaseField baseField5 = base.Fields["enableY"];
		guiActive = (base.Fields["enableY"].guiActiveEditor = flag);
		baseField5.guiActive = guiActive;
		BaseField baseField6 = base.Fields["enableZ"];
		guiActive = (base.Fields["enableZ"].guiActiveEditor = flag);
		baseField6.guiActive = guiActive;
		BaseField baseField7 = base.Fields["useThrottle"];
		guiActive = (base.Fields["useThrottle"].guiActiveEditor = flag);
		baseField7.guiActive = guiActive;
		BaseField baseField8 = base.Fields["fullThrust"];
		guiActive = (base.Fields["fullThrust"].guiActiveEditor = flag);
		baseField8.guiActive = guiActive;
		base.Events["ToggleToggles"].guiActive = (base.Events["ToggleToggles"].guiActiveEditor = showToggles && moduleIsEnabled);
		base.Events["ToggleToggles"].guiName = (currentShowToggles ? cacheAutoLOC_7003240 : cacheAutoLOC_7003241);
	}

	public float CalculateThrust(float totalForce, out bool success)
	{
		if (!isJustForShow && requiresFuel)
		{
			if (totalForce > EPSILON)
			{
				double num = flowMult;
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
					num *= (double)thrustCurveDisplay;
				}
				double num4 = num * maxFuelFlow * (double)totalForce * ((double)thrustPercentage * 0.01);
				double num5 = 1.0;
				if (!CheatOptions.InfinitePropellant)
				{
					num5 = RequestPropellant(num4 * (double)TimeWarp.fixedDeltaTime);
				}
				float result = (float)(num4 * num5 * exhaustVel);
				success = num5 > 0.0;
				flameout = !success;
				return result;
			}
			success = false;
			return 0f;
		}
		success = true;
		flameout = false;
		return 1f;
	}

	public void SetupPropellant(ConfigNode node)
	{
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
		mixtureDensity = 0.0;
		mixtureDensityRecip = 1.0;
		double num = 0.0;
		int j = 0;
		for (int count2 = propellants.Count; j < count2; j++)
		{
			Propellant propellant2 = propellants[j];
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(propellant2.name.GetHashCode());
			if (definition == null)
			{
				Debug.LogError(propellant2.name + " not found in resource database. Propellant Setup has failed.");
				base.part.partInfo.amountAvailable = 0;
				continue;
			}
			if (!propellant2.ignoreForIsp)
			{
				num += (double)(definition.density * propellant2.ratio);
			}
			propellant2.GetFlowMode();
		}
		mixtureDensity = num;
		if (mixtureDensity > 0.0)
		{
			mixtureDensityRecip = 1.0 / mixtureDensity;
		}
	}

	public void UpdatePropellantStatus()
	{
		if (propellants != null)
		{
			int count = propellants.Count;
			while (count-- > 0)
			{
				p = propellants[count];
				p.UpdateConnectedResources(base.part);
			}
		}
	}

	public void UpdatePropellantGauge(Propellant p)
	{
		if (!PropellantGauges.ContainsKey(p))
		{
			ProtoStageIconInfo protoStageIconInfo = base.part.stackIcon.DisplayInfo();
			if (protoStageIconInfo != null)
			{
				protoStageIconInfo.SetLength(2f);
				protoStageIconInfo.SetMsgBgColor(XKCDColors.DarkLime.smethod_0(0.6f));
				protoStageIconInfo.SetMsgTextColor(XKCDColors.ElectricLime.smethod_0(0.6f));
				protoStageIconInfo.SetMessage(p.displayName);
				protoStageIconInfo.SetProgressBarBgColor(XKCDColors.DarkLime.smethod_0(0.6f));
				protoStageIconInfo.SetProgressBarColor(XKCDColors.Yellow.smethod_0(0.6f));
				PropellantGauges.Add(p, protoStageIconInfo);
			}
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

	public double RequestPropellant(double mass)
	{
		if (mixtureDensity <= 0.0)
		{
			return 0.0;
		}
		double result = 0.0;
		double num = mass * mixtureDensityRecip;
		double num2 = 0.0;
		bool flag = false;
		int count = propellants.Count;
		int index = count;
		while (index-- > 0)
		{
			p = propellants[index];
			p.currentRequirement = num * (double)p.ratio;
			if (p.totalResourceAvailable >= p.currentRequirement)
			{
				p.isDeprived = false;
				continue;
			}
			if (p.totalResourceAvailable >= p.currentRequirement * (double)ignitionThreshold)
			{
				p.isDeprived = false;
				continue;
			}
			p.isDeprived = true;
			flag = true;
		}
		if (!flag)
		{
			int index2 = count;
			while (index2-- > 0)
			{
				p = propellants[index2];
				p.currentAmount = UtilMath.Clamp(base.part.RequestResource(p.id, Math.Min(p.totalResourceAvailable, p.currentRequirement), p.GetFlowMode()), 0.0, p.currentRequirement);
				num2 += p.currentAmount / p.currentRequirement;
			}
			result = num2 / (double)count;
			result = UtilMath.Clamp(result, 0.0, 1.0);
		}
		return result;
	}

	public override bool IsStageable()
	{
		if (!stagingEnabled && !stagingToggleEnabledEditor)
		{
			return stagingToggleEnabledFlight;
		}
		return true;
	}

	public override string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return cacheAutoLOC_218287;
	}

	public override string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return cacheAutoLOC_218288;
	}

	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		pos = (neg = Vector3.zero);
		if (!moduleIsEnabled || !rcsEnabled || !rcs_active || IsAdjusterBreakingRCS() || isJustForShow || flameout || (!enablePitch && !enableYaw && !enableRoll) || (base.part.ShieldedFromAirstream && !shieldedCanThrust))
		{
			return;
		}
		Vector3 lhs = new Vector3(enablePitch ? 1f : 0f, enableRoll ? 1f : 0f, enableYaw ? 1f : 0f);
		predictedCOM = base.vessel.CurrentCoM;
		tC = thrusterTransforms.Count;
		float num = thrusterPower * (thrustPercentage * 0.01f);
		int index = tC;
		while (index-- > 0)
		{
			currentThruster = thrusterTransforms[index];
			Vector3 vector = currentThruster.position - predictedCOM;
			Vector3 vector2 = (useZaxis ? (-currentThruster.forward) : (-currentThruster.up));
			float num2 = num;
			if (FlightInputHandler.fetch.precisionMode)
			{
				if (useLever)
				{
					float num3 = GetLeverDistance(currentThruster, vector2, predictedCOM);
					if (num3 > 1f)
					{
						num2 /= num3;
					}
				}
				else
				{
					num2 *= precisionFactor;
				}
			}
			rot = Vector3.Cross(lhs, Vector3.ProjectOnPlane(vector, inputRot).normalized);
			currentThrustForce = Mathf.Min(1f, Mathf.Max(Vector3.Dot(vector2, rot), 0f));
			Vector3 vector3 = Vector3.Cross(vector, vector2 * num2 * currentThrustForce);
			currentThrustForce = Mathf.Min(1f, Mathf.Max(Vector3.Dot(vector2, -rot), 0f));
			Vector3 vector4 = -Vector3.Cross(vector, vector2 * num2 * currentThrustForce);
			if (vector3.x > 0f)
			{
				pos.x = vector3.x;
			}
			if (vector3.y > 0f)
			{
				pos.y = vector3.y;
			}
			if (vector3.z > 0f)
			{
				pos.z = vector3.z;
			}
			if (vector4.x > 0f)
			{
				neg.x = vector4.x;
			}
			if (vector4.y > 0f)
			{
				neg.y = vector4.y;
			}
			if (vector4.z > 0f)
			{
				neg.z = vector4.z;
			}
		}
	}

	public virtual void SetupFX()
	{
		int num = 0;
		int i = 0;
		for (int count = thrusterTransforms.Count; i < count; i++)
		{
			Transform parent = thrusterTransforms[i];
			FXGroup item = new FXGroup(fxPrefix + num);
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Effects/" + fxPrefabName)) as GameObject;
			if (!(gameObject == null))
			{
				gameObject.transform.parent = parent;
				gameObject.transform.localRotation = Quaternion.Euler(fxOffsetRot);
				gameObject.transform.localPosition = Vector3.zero + fxOffset;
				base.part.fxGroups.Add(item);
				base.part.findFxGroup(fxPrefix + num).fxEmittersNewSystem.Add(gameObject.GetComponent<ParticleSystem>());
				thrusterFX.Add(item);
				num++;
			}
		}
	}

	public virtual void UpdatePowerFX(bool running, int idx, float power)
	{
		FXGroup fXGroup = thrusterFX[idx];
		if (running)
		{
			fXGroup.Power = power;
			fXGroup.setActive(value: true);
		}
		else
		{
			fXGroup.setActive(value: false);
		}
	}

	public virtual void DeactivateFX()
	{
		DeactivatePowerFX();
		int count = thrusterTransforms.Count;
		while (count-- > 0)
		{
			thrustForces[count] = 0f;
		}
	}

	public virtual void DeactivatePowerFX()
	{
		int count = thrusterFX.Count;
		while (count-- > 0)
		{
			FXGroup fXGroup = thrusterFX[count];
			fXGroup.setActive(value: false);
			fXGroup.Power = 0f;
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterRCSBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterRCSBase item = adjuster as AdjusterRCSBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public Vector3 ApplyInputRotationAdjustments(Vector3 inputRotation)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			inputRotation = adjusterCache[i].ApplyInputRotationAdjustment(inputRotation);
		}
		return inputRotation;
	}

	public Vector3 ApplyInputLinearAdjustments(Vector3 inputLinear)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			inputLinear = adjusterCache[i].ApplyInputLinearAdjustment(inputLinear);
		}
		return inputLinear;
	}

	public bool IsAdjusterBreakingRCS()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsRCSBroken())
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

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003050;
	}

	public virtual bool CanBeDetached()
	{
		return !isOperating;
	}

	public virtual bool CanBeOffset()
	{
		return !isOperating;
	}

	public virtual bool CanBeRotated()
	{
		return !isOperating;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_217939 = Localizer.Format("#autoLOC_217939");
		cacheAutoLOC_217950 = Localizer.Format("#autoLOC_217950");
		cacheAutoLOC_7003240 = Localizer.Format("#autoLOC_7003240");
		cacheAutoLOC_7003241 = Localizer.Format("#autoLOC_7003241");
		cacheAutoLOC_218287 = Localizer.Format("#autoLOC_218287");
		cacheAutoLOC_218288 = Localizer.Format("#autoLOC_218288");
		cacheAutoLOC_6003050 = Localizer.Format("#autoLoc_6003050");
	}
}
