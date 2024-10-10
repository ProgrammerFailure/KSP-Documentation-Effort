using System;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelSuspension : ModuleWheelSubmodule
{
	[KSPField]
	public string suspensionTransformName = "";

	[KSPField]
	public float suspensionOffset;

	[KSPField]
	public float suspensionDistance;

	[SerializeField]
	public float spring;

	[SerializeField]
	public float damper;

	[KSPField]
	public float targetPosition = 1f;

	[KSPField]
	public float springRatio = 50f;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 3f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001469")]
	public float springTweakable = 1f;

	[KSPField]
	public float damperRatio = 1f;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 2f, minValue = 0.05f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001470")]
	public float damperTweakable = 1f;

	[KSPField(isPersistant = true)]
	public bool autoSpringDamper = true;

	[SerializeField]
	public bool autoSpringDamperSet;

	[KSPField]
	public float boostRatio;

	[KSPField]
	public float maximumLoad;

	[KSPField]
	public bool suppressModuleInfo;

	[KSPField(isPersistant = true)]
	public Vector3 suspensionPos = -Vector3.one;

	[KSPField(isPersistant = true)]
	public Vector3 suspensionDefaultPos = -Vector3.one;

	[KSPField]
	public bool useDistributedMass;

	[SerializeField]
	public float damperFudge;

	[SerializeField]
	public float boost;

	[SerializeField]
	public float vesselMass;

	public Transform suspensionTransform;

	public float suspCompression;

	public float suspMass;

	public float sprungMassGravity;

	[KSPField(isPersistant = true)]
	public float autoBoost;

	[KSPField]
	public bool useAutoBoost = true;

	[KSPField]
	public string suspensionColliderName = string.Empty;

	public Collider suspensionCollider;

	[KSPField]
	public bool adjustForHighGee = true;

	[KSPField]
	public float highGeeThreshold = 1.5f;

	[KSPField]
	public float highGeeSpringTweakable = 3f;

	[KSPField]
	public float highGeeDamperTweakable = 1.2f;

	[SerializeField]
	public bool highGeeOverride;

	[SerializeField]
	public bool prevUseAutoBoost;

	[SerializeField]
	public float prevSpringTweakable;

	[SerializeField]
	public float springClampMax = 5000f;

	[SerializeField]
	public float damperClampMax = 5000f;

	[SerializeField]
	public float damperLerpBase = 1f;

	[SerializeField]
	public float oscillationDamper = 3f;

	[SerializeField]
	public float boostClamp = 0.5f;

	[SerializeField]
	public float autoBoostClamp = 1f;

	public BaseEvent evtAutoSpringDamperToggle;

	public BaseField fldSpringTweakable;

	public BaseField fldDamperTweakable;

	[SerializeField]
	public bool debugSuspension;

	public bool HighGeeOverride => highGeeOverride;

	[KSPEvent(advancedTweakable = true, active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoSpringDamperToggle()
	{
		autoSpringDamper = !autoSpringDamper;
		autoSpringDamperSet = false;
		ActionSpringDamperUIUpdate();
		ATsymPartUpdate();
	}

	public void ActionSpringDamperUIUpdate()
	{
		evtAutoSpringDamperToggle.guiActive = true;
		evtAutoSpringDamperToggle.guiActiveEditor = true;
		evtAutoSpringDamperToggle.guiName = Localizer.Format("#autoLOC_8002214", Convert.ToInt32(autoSpringDamper));
		fldSpringTweakable.guiActive = !autoSpringDamper || !GameSettings.WHEEL_AUTO_SPRINGDAMPER;
		fldSpringTweakable.guiActiveEditor = !autoSpringDamper || !GameSettings.WHEEL_AUTO_SPRINGDAMPER;
		fldDamperTweakable.guiActive = !autoSpringDamper || !GameSettings.WHEEL_AUTO_SPRINGDAMPER;
		fldDamperTweakable.guiActiveEditor = !autoSpringDamper || !GameSettings.WHEEL_AUTO_SPRINGDAMPER;
	}

	public void ATsymPartUpdate()
	{
		int i = 0;
		for (int count = base.part.symmetryCounterparts.Count; i < count; i++)
		{
			ModuleWheelSuspension moduleWheelSuspension = base.part.symmetryCounterparts[i].FindModuleImplementing<ModuleWheelSuspension>();
			if (moduleWheelSuspension != null)
			{
				moduleWheelSuspension.autoSpringDamper = autoSpringDamper;
				moduleWheelSuspension.autoSpringDamperSet = false;
				moduleWheelSuspension.ActionSpringDamperUIUpdate();
			}
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		autoSpringDamper = GameSettings.WHEEL_AUTO_SPRINGDAMPER || maximumLoad <= 0f;
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		evtAutoSpringDamperToggle = base.Events["EvtAutoSpringDamperToggle"];
		fldDamperTweakable = base.Fields["damperTweakable"];
		fldSpringTweakable = base.Fields["springTweakable"];
		ActionSpringDamperUIUpdate();
		if (!string.IsNullOrEmpty(suspensionTransformName))
		{
			suspensionTransform = base.part.FindModelTransform(suspensionTransformName);
			if (suspensionTransform == null)
			{
				Debug.LogError("[ModuleWheelSuspension]: No transform called " + suspensionTransformName + " found in " + base.part.partName + " hierarchy", base.gameObject);
			}
			else
			{
				if (suspensionDefaultPos == -Vector3.one)
				{
					suspensionDefaultPos = suspensionTransform.localPosition;
				}
				if (suspensionPos != -Vector3.one)
				{
					suspensionTransform.localPosition = suspensionPos;
				}
			}
		}
		if (!string.IsNullOrEmpty(suspensionColliderName))
		{
			Transform transform = base.part.FindModelTransform(suspensionColliderName);
			if (transform != null)
			{
				suspensionCollider = transform.GetComponent<Collider>();
				suspensionCollider.enabled = false;
			}
		}
		GameEvents.onVesselPartCountChanged.Add(onVesselPartCountChanged);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onVesselPartCountChanged.Remove(onVesselPartCountChanged);
	}

	public override void OnInventoryModeDisable()
	{
		GameEvents.onVesselPartCountChanged.Remove(onVesselPartCountChanged);
		if (!string.IsNullOrEmpty(suspensionTransformName))
		{
			suspensionTransform = base.part.FindModelTransform(suspensionTransformName);
		}
		if (suspensionTransform != null)
		{
			if (suspensionDefaultPos == -Vector3.one)
			{
				suspensionDefaultPos = suspensionTransform.localPosition;
			}
			suspensionTransform.localPosition = suspensionDefaultPos;
		}
		suspensionPos = -Vector3.one;
		if (wheelBase == null)
		{
			SetWheelBase();
		}
		wheelBase.wheelColliderHost = base.part.FindModelTransform(wheelBase.wheelColliderTransformName);
		if (wheelBase.wheelColliderHost != null && suspensionDefaultPos != -Vector3.one)
		{
			wheelBase.wheelColliderHost.localPosition = suspensionDefaultPos;
		}
	}

	public override void OnInventoryModeEnable()
	{
		suspensionPos = -Vector3.one;
		if (!string.IsNullOrEmpty(suspensionTransformName))
		{
			suspensionTransform = base.part.FindModelTransform(suspensionTransformName);
		}
		if (suspensionTransform != null)
		{
			if (suspensionDefaultPos == -Vector3.one)
			{
				suspensionDefaultPos = suspensionTransform.localPosition;
			}
			suspensionTransform.localPosition = suspensionDefaultPos;
		}
		if (wheelBase == null)
		{
			SetWheelBase();
		}
		wheelBase.wheelColliderHost = base.part.FindModelTransform(wheelBase.wheelColliderTransformName);
		if (wheelBase.wheelColliderHost != null && suspensionDefaultPos != -Vector3.one)
		{
			wheelBase.wheelColliderHost.localPosition = suspensionDefaultPos;
		}
	}

	public void onVesselPartCountChanged(Vessel changedVessel)
	{
		if (baseSetup && changedVessel.id == base.vessel.id)
		{
			wheel.wheelCollider.springRate = 0f;
		}
	}

	public override void OnWheelSetup()
	{
		if (suspensionTransform != null)
		{
			wheel.wheelCollider.suspensionTransform = suspensionTransform;
		}
		wheel.wheelCollider.updateSuspension = !wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Suspension);
		wheel.wheelCollider.suspensionDistance = suspensionDistance * base.part.rescaleFactor;
		wheel.wheelCollider.suspensionAnchor = targetPosition;
		wheel.wheelCollider.suspensionOffset = suspensionOffset * base.part.rescaleFactor;
		wheel.wheelCollider.springRate = spring;
		wheel.wheelCollider.damperRate = damper;
		ResetSuspensionCollider(3f);
	}

	[ContextMenu("Toggle Tweakables")]
	public void ToggleTweakables()
	{
		base.Fields["springTweakable"].guiName = Localizer.Format("#autoLOC_6001469");
		base.Fields["damperTweakable"].guiName = Localizer.Format("#autoLOC_6001470");
		base.Fields["springTweakable"].guiActive = !base.Fields["springTweakable"].guiActive;
		base.Fields["springTweakable"].guiActiveEditor = !base.Fields["springTweakable"].guiActiveEditor;
		base.Fields["damperTweakable"].guiActive = !base.Fields["damperTweakable"].guiActive;
		base.Fields["damperTweakable"].guiActiveEditor = !base.Fields["damperTweakable"].guiActiveEditor;
	}

	public void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && baseSetup && !base.part.packed && wheel != null && wheel.IsGrounded)
		{
			vesselMass = (float)base.vessel.totalMass;
			SuspensionSpringUpdate(vesselMass);
		}
	}

	public void LateUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (base.part.State == PartStates.CARGO)
			{
				return;
			}
			if (baseSetup && !base.part.packed)
			{
				if (suspensionTransform != null)
				{
					suspensionPos = suspensionTransform.localPosition;
				}
			}
			else if (suspensionTransform != null && suspensionPos != -Vector3.one)
			{
				suspensionTransform.localPosition = suspensionPos;
			}
		}
		else if (suspensionDefaultPos == -Vector3.one && suspensionTransform != null)
		{
			suspensionDefaultPos = suspensionTransform.localPosition;
		}
	}

	public float BoostCurve(float b, float power)
	{
		return Mathf.Round(Mathf.Clamp(1f / Mathf.Abs(1f - 2f / Mathf.Pow(Mathf.Clamp(b, 0f, 2f), Mathf.Clamp(power, 0.01f, autoBoostClamp))), 0.01f, 10000000f) * 10f) / 10f;
	}

	public void SuspensionSpringUpdate(float sprungMass)
	{
		suspCompression = wheel.currentState.suspensionCompression;
		suspMass = sprungMass;
		float num = sprungMass * suspCompression;
		springClampMax = Mathf.Lerp(5000f, 10000f, Mathf.InverseLerp(50f, 200f, sprungMass));
		damperClampMax = Mathf.Lerp(5000f, 10000f, Mathf.InverseLerp(50f, 200f, sprungMass));
		UpdateAutoBoost(suspCompression);
		boost = 1f + Mathf.Clamp(suspCompression * BoostCurve((float)base.vessel.mainBody.GeeASL * (float)base.vessel.gravityMultiplier, 1f), -1f, boostClamp);
		if (!useDistributedMass)
		{
			sprungMassGravity = num;
		}
		if (autoSpringDamper && (!autoSpringDamperSet || (maximumLoad > 0f && num > maximumLoad)))
		{
			if (base.vessel.mainBody.GeeASL < 1.0)
			{
				damperTweakable = 1.1f;
			}
			else
			{
				float t = Mathf.InverseLerp(damperLerpBase, (float)PSystemManager.Instance.MaximumSurfaceGeeASL, (float)base.vessel.mainBody.GeeASL);
				damperTweakable = (float)Math.Round(Mathf.Lerp(1f, 2f, t), 2);
			}
			if (spring > 0f && (!autoSpringDamperSet || (maximumLoad > 0f && num > maximumLoad)))
			{
				float value = suspensionDistance * spring / (float)PhysicsGlobals.GravitationalAcceleration;
				float t2 = Mathf.InverseLerp(0f, maximumLoad * 2f, value);
				springTweakable = (float)Math.Round(Mathf.Lerp(1f, 3f, t2), 2);
				autoSpringDamperSet = true;
			}
		}
		damperFudge = Mathf.Lerp(1f, 15f, Mathf.InverseLerp(10f, 100f, num));
		spring = Mathf.Clamp(springTweakable * springRatio * sprungMassGravity * BoostCurve(boost, boostRatio + autoBoost), 0.01f, springClampMax);
		damper = Mathf.Sqrt(Mathf.Clamp(spring * damperRatio * damperFudge, 0.01f, damperClampMax)) * damperTweakable;
		if (adjustForHighGee && base.vessel.mainBody.GeeASL > (double)highGeeThreshold)
		{
			spring *= highGeeSpringTweakable;
			damper *= highGeeDamperTweakable;
			highGeeOverride = true;
		}
		else
		{
			highGeeOverride = false;
		}
		damper = Mathf.Max(Mathf.Round(damper), 0.1f);
		spring = Mathf.Max(Mathf.Round(spring), 0.1f);
		if (spring - wheel.wheelCollider.springRate > oscillationDamper)
		{
			wheel.wheelCollider.springRate = spring;
		}
		else if (wheel.wheelCollider.springRate > spring + oscillationDamper)
		{
			wheel.wheelCollider.springRate = spring;
		}
		if (Mathf.Abs(damper - wheel.wheelCollider.damperRate) > 1.05f)
		{
			wheel.wheelCollider.damperRate = damper;
		}
		float num2 = wheel.wheelCollider.springRate * wheel.wheelState[0].contactDepth;
		if (num2 != 0f)
		{
			_ = num2 / ((float)base.vessel.mainBody.GeeASL * (float)base.vessel.gravityMultiplier);
		}
	}

	public void UpdateAutoBoost(float st)
	{
		if (!useAutoBoost)
		{
			autoBoost = 0f;
		}
		else if (st > 0.8f)
		{
			autoBoost = Mathf.Clamp(autoBoost + 0.2f * Time.fixedDeltaTime, 0.85f, 2f);
		}
		else if (st < 0.4f)
		{
			autoBoost = Mathf.Max(0f, autoBoost - 0.2f * Time.fixedDeltaTime);
		}
	}

	public override string OnGatherInfo()
	{
		if (suppressModuleInfo)
		{
			return null;
		}
		return Localizer.Format("#autoLOC_248720", suspensionDistance.ToString("0.0#"));
	}

	public override void OnSubsystemsModified(WheelSubsystems s)
	{
		if (s == wheelBase.InopSystems)
		{
			wheel.wheelCollider.updateSuspension = !s.HasType(WheelSubsystem.SystemTypes.Suspension);
		}
	}

	public void ResetSuspensionCollider(float delay)
	{
		if (suspensionCollider != null)
		{
			StartCoroutine(CallbackUtil.DelayedCallback(delay, delegate
			{
				suspensionCollider.enabled = true;
				base.part.ScheduleSetCollisionIgnores();
			}));
		}
	}
}
