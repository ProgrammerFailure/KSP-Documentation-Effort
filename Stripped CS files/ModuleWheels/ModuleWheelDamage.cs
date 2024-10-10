using System;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelDamage : ModuleWheelSubmodule, IConstruction
{
	[KSPField]
	public float stressTolerance = 10f;

	[KSPField]
	public float impactTolerance = 40f;

	[KSPField]
	public float deflectionMagnitude = 1f;

	[KSPField]
	public float slipMagnitude = 1f;

	[KSPField]
	public float deflectionSharpness = 1f;

	[KSPField]
	public float slipSharpness = 2f;

	[KSPField]
	public string damagedTransformName = "";

	[KSPField]
	public string undamagedTransformName = "";

	[KSPField]
	public bool isRepairable = true;

	[KSPField]
	public float explodeMultiplier = 5f;

	public float currentDeflection;

	public float lastDeflection;

	public float currentSlip;

	public float lastSlip;

	public float currentDownForce;

	public float lastDownForce;

	public const float repairImmunityMin = 30f;

	public const float repairImmunityMax = 90f;

	public float repairImmunityTimeTotal = 30f;

	public float repairImmunityTimeLeft;

	public float startupTime = 8f;

	[SerializeField]
	public int repairKitsNecessary = 1;

	public bool initialized;

	[KSPField(isPersistant = true)]
	public bool isDamaged;

	public float totalStress;

	public float stressTime;

	public Transform dmgTransform;

	public Transform undmgTransform;

	[KSPField(guiFormat = "0.0", guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001459")]
	[UI_ProgressBar(scene = UI_Scene.Flight, maxValue = 100f, minValue = 0f)]
	public float stressPercent;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6003063")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string brokenStatusWarning = "";

	public UI_Label brokenStatusWarningField;

	public float stressVariability;

	public BaseEvent eventRepairExternal;

	public WheelSubsystem subsystemDamage;

	public BaseField stressField;

	public static string cacheAutoLOC_6005093;

	[KSPField]
	public float impactDamageVelocity;

	[KSPField]
	public string impactDamageColliderName = "";

	public Collider impactDamageCollider;

	public bool IsImpactDamageEnable
	{
		get
		{
			if (impactDamageVelocity > 0f)
			{
				if (GameSettings.WHEEL_DAMAGE_IMPACTCOLLIDER_ENABLED && impactDamageCollider != null)
				{
					return true;
				}
				return GameSettings.WHEEL_DAMAGE_WHEELCOLLIDER_ENABLED;
			}
			return false;
		}
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (wheelBase != null && wheelBase.wheelType == WheelType.const_2)
		{
			stressField = base.Fields["stressPercent"];
			if (stressField != null)
			{
				stressField.guiName = Localizer.Format("#autoLOC_6002671");
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onPartCoupleComplete.Add(onPartCouple);
			GameEvents.onPartDeCoupleComplete.Add(onPartDecouple);
			GameEvents.onVesselWasModified.Add(OnVesselModified);
			GameEvents.onVesselChange.Add(onVesselFocusChange);
		}
		repairKitsNecessary = Math.Min(Math.Max((int)(base.part.mass / GameSettings.PART_REPAIR_MASS_PER_KIT), 1), GameSettings.PART_REPAIR_MAX_KIT_AMOUNT);
		eventRepairExternal = base.Events["EventRepairExternal"];
		eventRepairExternal.active = isDamaged && isRepairable;
		if (HighLogic.LoadedSceneIsFlight)
		{
			brokenStatusWarning = cacheAutoLOC_6005093;
			if (base.Fields["brokenStatusWarning"] != null)
			{
				base.Fields.TryGetFieldUIControl<UI_Label>("brokenStatusWarning", out brokenStatusWarningField);
				if (brokenStatusWarningField != null)
				{
					brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, isDamaged && isRepairable);
				}
			}
			if (FlightGlobals.ActiveVessel.isEVA)
			{
				eventRepairExternal.guiName = Localizer.Format("#autoLOC_6005092", repairKitsNecessary.ToString());
			}
			else
			{
				eventRepairExternal.guiName = cacheAutoLOC_6005093;
			}
		}
		GetDamageTransforms(logError: true);
		if (!string.IsNullOrEmpty(impactDamageColliderName) && undmgTransform != null)
		{
			Transform transform = Part.FindHeirarchyTransform(undmgTransform, impactDamageColliderName);
			if (transform != null)
			{
				transform.GetComponentCached(ref impactDamageCollider);
			}
			else
			{
				Debug.LogError(("[ModuleWheelDamage]: No break collision collider found with id " + impactDamageColliderName + " for " + base.part.partName) ?? "", base.gameObject);
			}
		}
		subsystemDamage = new WheelSubsystem("Wheel Damage", (WheelSubsystem.SystemTypes)33, this);
		isDamaged = !isDamaged;
		SetDamaged(!isDamaged);
	}

	public void GetDamageTransforms(bool logError)
	{
		if (!string.IsNullOrEmpty(damagedTransformName))
		{
			Transform transform = base.part.FindModelTransform(damagedTransformName);
			if (transform != null)
			{
				dmgTransform = transform;
			}
			else if (logError)
			{
				Debug.LogError(("[ModuleWheelDamage]: No damaged transform object found with id " + damagedTransformName + " for " + base.part.partName) ?? "", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(undamagedTransformName))
		{
			Transform transform2 = base.part.FindModelTransform(undamagedTransformName);
			if (transform2 != null)
			{
				undmgTransform = transform2;
			}
			else if (logError)
			{
				Debug.LogError(("[ModuleWheelDamage]: No undamaged transform object found with id " + undamagedTransformName + " for " + base.part.partName) ?? "", base.gameObject);
			}
		}
	}

	public override void OnInventoryModeEnable()
	{
		if (dmgTransform == null || undmgTransform == null)
		{
			GetDamageTransforms(logError: false);
		}
		eventRepairExternal = base.Events["EventRepairExternal"];
		isDamaged = !isDamaged;
		SetDamaged(!isDamaged);
	}

	public override void OnInventoryModeDisable()
	{
		if (dmgTransform == null || undmgTransform == null)
		{
			GetDamageTransforms(logError: false);
		}
		eventRepairExternal = base.Events["EventRepairExternal"];
		isDamaged = !isDamaged;
		SetDamaged(!isDamaged);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onPartCoupleComplete.Remove(onPartCouple);
		GameEvents.onPartDeCoupleComplete.Remove(onPartDecouple);
		GameEvents.onVesselWasModified.Remove(OnVesselModified);
		GameEvents.onVesselChange.Remove(onVesselFocusChange);
	}

	public void onPartCouple(GameEvents.FromToAction<Part, Part> partAction)
	{
		if ((base.vessel != null && base.vessel.Landed && partAction.from.vessel.persistentId == base.part.vessel.persistentId) || partAction.to.vessel.persistentId == base.part.vessel.persistentId)
		{
			ResetImmunity();
		}
	}

	public void onPartDecouple(Part part)
	{
		if (base.vessel != null && base.vessel.Landed && part.vessel.persistentId == base.part.vessel.persistentId)
		{
			ResetImmunity();
		}
	}

	public void OnVesselModified(Vessel vsl)
	{
		if (base.vessel != null && base.vessel.Landed && base.part.vessel.persistentId == vsl.persistentId)
		{
			ResetImmunity();
		}
	}

	public void ResetImmunity()
	{
		startupTime = 8f;
		repairImmunityTimeTotal = 30f;
	}

	public override void OnWheelSetup()
	{
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			isDamaged = !isDamaged;
			SetDamaged(!isDamaged);
		}));
	}

	public void Initialize()
	{
		currentDeflection = deflectionMagnitude * wheel.LegacyWheelLoad;
		currentSlip = slipMagnitude * wheel.LegacyWheelLoad * wheelBase.slipDisplacement.magnitude;
		currentDownForce = wheel.currentState.downforce;
		lastDeflection = currentDeflection;
		lastSlip = currentSlip;
		lastDownForce = currentDownForce;
		initialized = true;
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !baseSetup || (!(base.part.vessel == null) && base.part.vessel.situation == Vessel.Situations.PRELAUNCH))
		{
			return;
		}
		if (!base.part.packed && !isDamaged)
		{
			if (!initialized)
			{
				Initialize();
			}
			lastDeflection = currentDeflection;
			lastSlip = currentSlip;
			lastDownForce = currentDownForce;
			if (repairImmunityTimeLeft > 0f)
			{
				repairImmunityTimeLeft = Mathf.Max(0f, repairImmunityTimeLeft - Time.fixedDeltaTime);
			}
			float num = 1f - repairImmunityTimeLeft / repairImmunityTimeTotal;
			currentDeflection = deflectionMagnitude * wheel.LegacyWheelLoad;
			currentSlip = slipMagnitude * wheel.LegacyWheelLoad * wheelBase.slipDisplacement.magnitude;
			currentDeflection = Mathf.Lerp(lastDeflection, currentDeflection, deflectionSharpness * TimeWarp.fixedDeltaTime);
			currentSlip = Mathf.Lerp(lastSlip, currentSlip, slipSharpness * TimeWarp.fixedDeltaTime);
			currentDownForce = wheel.currentState.downforce;
			if (startupTime > 0f)
			{
				startupTime -= Time.deltaTime;
				return;
			}
			float num2 = currentDeflection * num * GameSettings.WHEEL_WEIGHT_STRESS_MULTIPLIER;
			float num3 = currentSlip * num * GameSettings.WHEEL_SLIP_STRESS_MULTIPLIER;
			totalStress = Mathf.Clamp(num2 + num3, 0f, stressTolerance);
			stressPercent = totalStress / stressTolerance * 100f;
			float num4 = stressTolerance + stressVariability;
			if (totalStress >= num4)
			{
				stressTime += TimeWarp.fixedDeltaTime;
				float num5 = 1f - Mathf.Clamp01((totalStress - num4) / (num4 / 2f));
				float num6 = 0.25f + 0.25f * num5;
				if (stressTime > num6)
				{
					SetDamaged(damaged: true);
				}
			}
			else
			{
				stressTime = 0f;
			}
			float num7 = (currentDownForce - lastDownForce) * wheel.gravity.Magnitude / (float)PhysicsGlobals.GravitationalAcceleration;
			if (!CheatOptions.NoCrashDamage)
			{
				if (num7 >= impactTolerance * explodeMultiplier)
				{
					base.part.explode();
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246817", base.part.partInfo.title), 6f, ScreenMessageStyle.UPPER_LEFT);
					FlightLogger.fetch.LogEvent(Localizer.Format("<<1>> overstressed and was destroyed.", base.part.partInfo.title));
				}
				else if (num7 >= impactTolerance)
				{
					SetDamaged(damaged: true);
				}
			}
		}
		else
		{
			currentDeflection = 0f;
			currentSlip = 0f;
			currentDownForce = 0f;
			stressTime = 0f;
			initialized = false;
			startupTime = 8f;
			repairImmunityTimeTotal = 30f;
			repairImmunityTimeLeft = 0f;
			totalStress = 0f;
			stressPercent = 0f;
		}
	}

	public void SetDamaged(bool damaged)
	{
		if (damaged && !isDamaged)
		{
			if (dmgTransform != null)
			{
				dmgTransform.gameObject.SetActive(value: true);
			}
			if (undmgTransform != null)
			{
				undmgTransform.gameObject.SetActive(value: false);
			}
			if (baseSetup)
			{
				wheelBase.InopSystems.AddSubsystem(subsystemDamage);
			}
			eventRepairExternal.active = damaged && isRepairable;
			eventRepairExternal.guiActive = damaged && isRepairable;
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, state: true);
			}
			GameEvents.onPartRepaired.Fire(base.part);
		}
		if (!damaged && isDamaged)
		{
			repairImmunityTimeTotal = UnityEngine.Random.Range(30f, 90f);
			repairImmunityTimeLeft = repairImmunityTimeTotal;
			if (dmgTransform != null)
			{
				dmgTransform.gameObject.SetActive(value: false);
			}
			if (undmgTransform != null)
			{
				undmgTransform.gameObject.SetActive(value: true);
			}
			if (baseSetup)
			{
				wheelBase.InopSystems.RemoveSubsystem(subsystemDamage);
			}
			eventRepairExternal.active = damaged && isRepairable;
			eventRepairExternal.guiActive = damaged && isRepairable;
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, state: false);
			}
			GameEvents.onWheelRepaired.Fire(base.part);
		}
		isDamaged = damaged;
		stressVariability = (0f - UnityEngine.Random.value) * stressTolerance * 0.02f;
	}

	public void onVesselFocusChange(Vessel v)
	{
		if (v.isEVA)
		{
			eventRepairExternal.guiName = Localizer.Format("#autoLOC_6005092", repairKitsNecessary.ToString());
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, state: false);
			}
		}
		else
		{
			eventRepairExternal.guiName = cacheAutoLOC_6005093;
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, isDamaged && isRepairable);
			}
		}
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_246889", stressTolerance.ToString("0.0"));
	}

	[ContextMenu("Damage Wheel")]
	public void DamageWheel()
	{
		SetDamaged(damaged: true);
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_6001882")]
	public void EventRepairExternal()
	{
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode) && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246904", 1.ToString()));
		}
		else
		{
			if (!FlightGlobals.ActiveVessel.isEVA || !(FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference != null))
			{
				return;
			}
			if (FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value > 0)
			{
				if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaRepairKit") >= repairKitsNecessary)
				{
					FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaRepairKit", repairKitsNecessary, playSound: true);
					SetDamaged(damaged: false);
				}
				else
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", repairKitsNecessary.ToString(), repairKitsNecessary.ToString()));
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006098", KerbalRoster.GetLocalizedExperienceTraitName("Engineer")));
			}
		}
	}

	public virtual bool CanBeDetached()
	{
		return !isDamaged;
	}

	public virtual bool CanBeOffset()
	{
		return !isDamaged;
	}

	public virtual bool CanBeRotated()
	{
		return !isDamaged;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6005093 = Localizer.Format("#autoLOC_6005093");
	}

	public void HandleCollision(Collision c)
	{
		if (!IsImpactDamageEnable)
		{
			return;
		}
		int num = 0;
		int num2 = c.contacts.Length;
		ContactPoint contactPoint;
		while (true)
		{
			if (num < num2)
			{
				contactPoint = c.contacts[num];
				if (impactDamageCollider == contactPoint.thisCollider || impactDamageCollider == contactPoint.otherCollider)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		HandleCollisionVelocity(c.gameObject, c.relativeVelocity, contactPoint.normal);
	}

	public void HandleCollisionVelocity(GameObject otherObject, Vector3 velocity, Vector3 normal)
	{
		if (!IsImpactDamageEnable)
		{
			return;
		}
		Vector3 vector = Vector3.Project(velocity, normal);
		if (vector.magnitude < impactDamageVelocity)
		{
			return;
		}
		SetDamaged(damaged: true);
		GClass3 component = otherObject.GetComponent<GClass3>();
		if (component != null)
		{
			Debug.Log($"Hit {component.name} at {vector.magnitude:0.0} m.s and damaged wheel.");
			return;
		}
		Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(otherObject);
		if (partUpwardsCached != null)
		{
			Debug.Log($"Hit {partUpwardsCached.partInfo.title} on {partUpwardsCached.vessel.vesselName} at {vector.magnitude:0.0} m.s and damaged wheel.");
			return;
		}
		CrashObjectName componentUpwards = Part.GetComponentUpwards<CrashObjectName>(otherObject);
		if (componentUpwards != null)
		{
			Debug.Log($"Hit {Localizer.Format(componentUpwards.displayName)} at {vector.magnitude:0.0} m.s and damaged wheel.");
		}
	}
}
