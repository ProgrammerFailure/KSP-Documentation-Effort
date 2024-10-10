using System;
using System.Collections.Generic;
using ModuleWheels;
using ns9;
using UnityEngine;
using VehiclePhysics;

public class ModuleWheelBase : PartModule, IModuleInfo, IContractObjectiveModule
{
	public enum DriftCorrectionState
	{
		Idle,
		Acquire,
		Fix
	}

	[SerializeField]
	public KSPWheelController wheel;

	[SerializeField]
	public GameObject wheelHost;

	[KSPField(isPersistant = true)]
	public WheelType wheelType;

	[KSPField(isPersistant = true)]
	public bool isGrounded;

	[KSPField]
	public bool FitWheelColliderToMesh;

	[KSPField]
	public float radius;

	[KSPField]
	public Vector3 center;

	[KSPField]
	public float mass;

	[KSPField]
	public float frictionSharpness;

	[KSPField]
	public float wheelDamping = 0.05f;

	[KSPField]
	public float wheelMaxSpeed = 1000f;

	[KSPField]
	public string clipObject = string.Empty;

	public GameObject clipGameObject;

	[KSPField]
	public float adherentStart = 0.5f;

	[KSPField]
	public float frictionAdherent = 0.25f;

	[KSPField]
	public float peakStart = 4f;

	[KSPField]
	public float frictionPeak = 1.45f;

	[KSPField]
	public float limitStart = 7f;

	[KSPField]
	public float frictionLimit = 1.1f;

	[KSPField]
	public bool autoFrictionAvailable = true;

	[KSPField(isPersistant = true)]
	public bool autoFriction = true;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActive = false, guiName = "#autoLOC_6001457")]
	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 0.01f, maxValue = 10f, minValue = 0.01f, affectSymCounterparts = UI_Scene.All)]
	public float frictionMultiplier = 1f;

	[KSPField]
	public float geeBias = 1.6f;

	public bool suspensionEnabled = true;

	[KSPField]
	public float groundHeightOffset;

	[KSPField]
	public int inactiveSubsteps = -1;

	[KSPField]
	public int activeSubsteps = -1;

	[KSPField]
	public float tireForceSharpness = 10f;

	[KSPField]
	public float suspensionForceSharpness = 10f;

	[KSPField]
	public bool ApplyForcesToParent;

	[KSPField]
	public string wheelColliderTransformName = "wheelCollider";

	[KSPField]
	public string wheelTransformName = "";

	[KSPField]
	public string TooltipTitle = "Wheel";

	[KSPField]
	public string TooltipPrimaryField;

	[KSPField]
	public float springSlerpRate = 0.02f;

	[KSPField]
	public float minimumDownforce = 0.5f;

	[KSPField]
	public bool useNewFrictionModel;

	public Transform wheelColliderHost;

	public Transform wheelTransform;

	public List<ModuleWheelSubmodule> subModules;

	public bool setup;

	public BaseEvent evtAutoFrictionToggle;

	public BaseField fldFrictionMultiplier;

	[SerializeField]
	public bool driftCorrection;

	[NonSerialized]
	public Collider gCollider;

	[NonSerialized]
	public Collider gColliderPrev;

	[NonSerialized]
	public Vessel vSrfContact;

	[NonSerialized]
	public Part tgtParent;

	[NonSerialized]
	public string vLandedAt;

	public Vector2 slipDisplacement;

	public WheelSubsystems inopSystems;

	public WheelSubsystem inopOnRails;

	public WheelSubsystem inopSuspension;

	public ModuleWheelBrakes brakesSubmodule;

	public ModuleWheelLock wheelLockSubmodule;

	public ModuleWheelDamage wheelDamageSubmodule;

	[SerializeField]
	public bool rbBrakeConstraints;

	public float schloompaTime = 2f;

	public ModuleWheelDamage moduleWheelDamage;

	public float acquireMaxSpeed = 0.1f;

	public Vector3 fixFwd;

	public Vector3 error;

	public Vector3 errorLast;

	[SerializeField]
	public float kd = 100f;

	[SerializeField]
	public float ki = 0.1f;

	public DriftCorrectionState driftCorrectionState = DriftCorrectionState.Acquire;

	public KSPWheelController Wheel => wheel;

	public Vector3 WheelOrgPosR { get; set; }

	public Quaternion WheelOrgRotR { get; set; }

	public WheelSubsystems InopSystems => inopSystems;

	public override void OnAwake()
	{
		subModules = new List<ModuleWheelSubmodule>();
		inopSystems = new WheelSubsystems();
	}

	public override void OnStart(StartState state)
	{
		suspensionEnabled = true;
		if (!string.IsNullOrEmpty(wheelTransformName))
		{
			wheelTransform = base.part.FindModelTransform(wheelTransformName);
			if (wheelTransform == null)
			{
				Debug.LogError("[ModuleWheelBase]: No transform called " + wheelTransformName + " found in " + base.part.partName + " hierarchy", base.gameObject);
				return;
			}
		}
		base.part.autoStrutMode = Part.AutoStrutMode.ForceHeaviest;
		wheelColliderHost = base.part.FindModelTransform(wheelColliderTransformName);
		if (wheelColliderHost == null)
		{
			Debug.LogError("[ModuleWheelBase]: No transform called " + wheelColliderTransformName + " found in " + base.part.partName + " hierarchy", base.gameObject);
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			StartCoroutine(CallbackUtil.WaitUntil(() => base.part.started && !base.part.packed, wheelSetup));
		}
		GameEvents.onPartPack.Add(onPartPack);
		GameEvents.onPartUnpack.Add(onPartUnpack);
		GameEvents.onVesselSOIChanged.Add(SOIChange);
		GameEvents.onVesselWasModified.Add(OnVesselModified);
		GameEvents.onDockingComplete.Add(onDockingComplete);
		GameEvents.onVesselsUndocking.Add(onVesselUndocking);
		inopOnRails = new WheelSubsystem("Part is on-rails", WheelSubsystem.SystemTypes.WheelCollider, this);
		inopSuspension = new WheelSubsystem("Suspension is disabled", WheelSubsystem.SystemTypes.Suspension, this);
		evtAutoFrictionToggle = base.Events["EvtAutoFrictionToggle"];
		fldFrictionMultiplier = base.Fields["frictionMultiplier"];
		ActionUIUpdate();
		if (!string.IsNullOrEmpty(clipObject))
		{
			Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				Transform transform = componentsInChildren[i];
				if (transform.gameObject.name == clipObject)
				{
					clipGameObject = transform.gameObject;
					break;
				}
			}
		}
		if (activeSubsteps < 0)
		{
			activeSubsteps = GameSettings.WHEEL_SUBSTEPS_ACTIVE;
		}
		if (inactiveSubsteps < 0)
		{
			inactiveSubsteps = GameSettings.WHEEL_SUBSTEPS_INACTIVE;
		}
		wheelDamageSubmodule = GetComponent<ModuleWheelDamage>();
		brakesSubmodule = GetComponent<ModuleWheelBrakes>();
		wheelLockSubmodule = GetComponent<ModuleWheelLock>();
		if (wheelLockSubmodule != null && wheelType == WheelType.const_2 && base.vessel != null && base.vessel.mainBody.GeeASL > 1.100000023841858)
		{
			ki = 1000f;
		}
		if (wheelType == WheelType.const_2)
		{
			minimumDownforce = 10f;
		}
	}

	public void wheelSetup()
	{
		base.part.UpdateAutoStrut();
		wheelHost = base.part.Rigidbody.gameObject;
		Vector3 velocity = base.part.Rigidbody.velocity;
		WheelCollider component = wheelColliderHost.GetComponent<WheelCollider>();
		if (component != null)
		{
			component.enabled = true;
		}
		wheel = KSPWheelController.Create(base.part.Rigidbody, wheelHost, wheelColliderHost.gameObject);
		wheel.gravity.Value = base.vessel.gravityForPos;
		wheel.minimumdownForce = minimumDownforce;
		if (wheel.wheelCollider != null)
		{
			wheel.wheelCollider.springSlerpRate = springSlerpRate;
		}
		Transform transform = base.part.partTransform.Find("model");
		if (transform != null)
		{
			wheel.wcTransform.SetParent(transform.parent);
			wheel.wcTransform.localScale *= 1f / base.part.rescaleFactor;
		}
		int num = ~LayerUtil.DefaultEquivalent;
		num = ~(num | 0x20000) | 0x8000;
		if (wheel.wheelCollider != null)
		{
			wheel.wheelCollider.layerMask = num;
			wheel.wheelCollider.mass = mass;
			CheckSubsteps();
			wheel.wheelCollider.suspensionDistance = 0.02f;
			wheel.wheelCollider.suspensionAnchor = 0.5f;
			wheel.wheelCollider.springRate = 10000f;
			wheel.wheelCollider.damperRate = 1f;
			wheel.wheelCollider.groundPenetration = 0.02f;
		}
		wheel.tireForceSmoothing = true;
		wheel.tireForceSharpness = tireForceSharpness;
		wheel.suspensionForceSmoothing = true;
		wheel.suspensionForceSharpness = suspensionForceSharpness;
		if (frictionSharpness > 0f)
		{
			wheel.tireSideDeflection = true;
			wheel.tireSideDeflectionRate = frictionSharpness;
		}
		wheel.wheelDamping = 1f - wheelDamping;
		wheel.maxRpm = WheelUtil.SpeedToRpm(wheelMaxSpeed, radius);
		wheel.OnShouldIgnoreForces = IgnoreForcesOnSameVesselContact;
		if (wheel.wheelCollider != null)
		{
			if (FitWheelColliderToMesh && wheelTransform != null)
			{
				wheel.wheelCollider.AdjustToWheelMesh();
			}
			else
			{
				wheel.wheelCollider.radius = radius * base.part.rescaleFactor;
				wheel.wheelCollider.center = center * base.part.rescaleFactor;
			}
		}
		if (wheelTransform != null)
		{
			WheelOrgPosR = base.part.partTransform.InverseTransformPoint(wheelTransform.position);
			WheelOrgRotR = Quaternion.Inverse(base.part.partTransform.rotation) * wheelTransform.rotation;
			if (wheel.wheelCollider != null)
			{
				wheel.wheelCollider.wheelTransform = wheelTransform;
			}
		}
		else
		{
			WheelOrgPosR = base.part.partTransform.InverseTransformPoint(wheelColliderHost.position);
			WheelOrgRotR = Quaternion.Inverse(base.part.partTransform.rotation) * wheelColliderHost.rotation;
		}
		inopSystems.OnModified = OnSubsystemsModified;
		OnWheelSetup(wheel);
		if (HighLogic.LoadedSceneIsFlight && base.part.vessel.mainBody != null)
		{
			ApplyGeeBias((float)base.part.vessel.mainBody.GeeASL);
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			ApplyGeeBias(1f);
		}
		wheel.tireFriction.model = TireFriction.Model.Lineal;
		setup = true;
		InopUpdate(force: true);
		if (!wheel.enabled)
		{
			wheel.Initialize();
		}
		wheel.rb.velocity = velocity;
		if (ApplyForcesToParent && base.part.parent != null)
		{
			wheel.tgtRb = base.part.parent.Rigidbody;
			tgtParent = base.part.parent;
		}
	}

	public float ApplyGeeBias(float gee)
	{
		if (autoFriction)
		{
			if (useNewFrictionModel)
			{
				frictionMultiplier = Mathf.Clamp(gee * geeBias + 0.4f, 0.4f, 10f);
			}
			else
			{
				minimumDownforce = 10f;
				if (geeBias == 1.6f)
				{
					geeBias = 1f;
				}
				frictionMultiplier = Mathf.Clamp(1f / gee * geeBias + 1f, 0.5f, 10f);
			}
		}
		if (Wheel != null && wheel.tireFriction != null)
		{
			Wheel.tireFriction.settings.adherent.x = adherentStart;
			Wheel.tireFriction.settings.peak.x = peakStart;
			Wheel.tireFriction.settings.limit.x = limitStart;
			UpdateFriction();
		}
		return frictionMultiplier;
	}

	public void UpdateFriction()
	{
		Wheel.tireFriction.settings.adherent.y = frictionAdherent * frictionMultiplier;
		Wheel.tireFriction.settings.peak.y = frictionPeak * frictionMultiplier;
		Wheel.tireFriction.settings.limit.y = frictionLimit * frictionMultiplier;
	}

	public void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(OnVesselModified);
		GameEvents.onPartPack.Remove(onPartPack);
		GameEvents.onPartUnpack.Remove(onPartUnpack);
		GameEvents.onVesselSOIChanged.Remove(SOIChange);
		GameEvents.onDockingComplete.Remove(onDockingComplete);
		GameEvents.onVesselsUndocking.Remove(onVesselUndocking);
	}

	public void FixedUpdate()
	{
		if (!setup || !HighLogic.LoadedSceneIsFlight || base.part.packed || base.part.State == PartStates.DEAD)
		{
			return;
		}
		CheckSuspensionToggle();
		CheckSubsteps();
		slipDisplacement.x = wheel.currentState.localWheelVelocity.x;
		slipDisplacement.y = wheel.currentState.angularVelocity * radius - wheel.currentState.localWheelVelocity.y;
		slipDisplacement *= TimeWarp.deltaTime;
		if (wheelDamageSubmodule != null && wheel.IsGrounded && !isGrounded)
		{
			wheelDamageSubmodule.ResetImmunity();
			if (GameSettings.WHEEL_DAMAGE_WHEELCOLLIDER_ENABLED)
			{
				Vector3 velocity = base.part.Rigidbody.velocity;
				Collider collider = wheel.currentState.hit.collider;
				if (collider.attachedRigidbody != null)
				{
					velocity -= wheel.currentState.hit.collider.attachedRigidbody.velocity;
				}
				wheelDamageSubmodule.HandleCollisionVelocity(collider.gameObject, velocity, wheel.currentState.hit.normal);
			}
		}
		isGrounded = wheel.IsGrounded;
		if (!autoFriction)
		{
			UpdateFriction();
		}
		wheel.gravity.Value = base.vessel.gravityForPos;
		LandingDetectionUpdate();
		wheel.maxRpm = WheelUtil.SpeedToRpm(wheelMaxSpeed, radius);
		wheel.wheelDamping = 1f - wheelDamping;
		if (driftCorrection)
		{
			updateDriftFix();
		}
		if (wheelType != WheelType.const_2)
		{
			LSchloomphaVPPProcessing();
		}
	}

	public void CheckSubsteps()
	{
		if (wheel != null && base.vessel != null)
		{
			wheel.integrationSteps = ((base.vessel == FlightGlobals.ActiveVessel) ? activeSubsteps : inactiveSubsteps);
		}
	}

	public void LandingDetectionUpdate()
	{
		if (!LandedDetectionNeedsUpdate(wheel.currentState.hit.collider, gColliderPrev, vSrfContact, wheel.IsGrounded))
		{
			return;
		}
		gColliderPrev = gCollider;
		gCollider = wheel.currentState.hit.collider;
		base.part.GroundContact = false;
		if (base.vessel.GroundContacts.Contains(base.part))
		{
			base.vessel.GroundContacts.Remove(base.part);
		}
		vSrfContact = null;
		base.vessel.crashObjectName = null;
		if (wheel.IsGrounded)
		{
			if (gCollider == null)
			{
				throw new InvalidOperationException("[ModuleWheelBase]: wheel says it's grounded but hit.collider is null! this can't possibly be right.");
			}
			if (gCollider.gameObject.layer == 15)
			{
				if (gCollider.gameObject.tag != string.Empty)
				{
					vLandedAt = gCollider.gameObject.tag;
					if (!gCollider.gameObject.CompareTag("Untagged"))
					{
						if (ROCManager.Instance != null && gCollider.gameObject.CompareTag("ROC"))
						{
							GameObject terrainObj = null;
							vLandedAt = ROCManager.Instance.GetTerrainTag(gCollider.gameObject, out terrainObj);
						}
						base.vessel.SetLandedAt(vLandedAt, gCollider.gameObject);
					}
					else
					{
						vLandedAt = string.Empty;
						base.vessel.SetLandedAt(vLandedAt);
					}
				}
				else
				{
					vLandedAt = string.Empty;
					base.vessel.SetLandedAt(vLandedAt);
				}
				base.part.GroundContact = true;
				if (!base.vessel.GroundContacts.Contains(base.part))
				{
					base.vessel.GroundContacts.Add(base.part);
				}
				CrashObjectName componentUpwards = Part.GetComponentUpwards<CrashObjectName>(gCollider.gameObject);
				if (componentUpwards != null)
				{
					base.vessel.crashObjectName = componentUpwards;
				}
			}
			else if (gCollider.gameObject.layer == 0)
			{
				Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(gCollider.gameObject);
				if (partUpwardsCached != null && !base.vessel.parts.Contains(partUpwardsCached) && partUpwardsCached.vessel.LandedOrSplashed)
				{
					base.part.GroundContact = true;
					if (!base.vessel.GroundContacts.Contains(base.part))
					{
						base.vessel.GroundContacts.Add(base.part);
					}
					vSrfContact = partUpwardsCached.vessel;
					vLandedAt = vSrfContact.landedAt;
					base.vessel.SetLandedAt(vLandedAt, null, partUpwardsCached.vessel.displaylandedAt);
				}
			}
		}
		else
		{
			base.part.ResetCollisions();
		}
		base.vessel.checkLanded();
	}

	public bool LandedDetectionNeedsUpdate(Collider hitCollider, Collider hitColliderPrev, Vessel vContact, bool isGrounded)
	{
		if (isGrounded && (bool)base.vessel && !base.vessel.Landed)
		{
			return true;
		}
		if (hitColliderPrev != hitCollider)
		{
			return true;
		}
		if (hitCollider != null && !isGrounded)
		{
			return true;
		}
		if (hitCollider == null && isGrounded)
		{
			return true;
		}
		if (vContact != null && !vContact.Landed)
		{
			return true;
		}
		return false;
	}

	public bool IgnoreForcesOnSameVesselContact(VehicleBase.WheelState st)
	{
		Collider collider = st.hit.collider;
		if (collider == null)
		{
			return false;
		}
		Rigidbody attachedRigidbody = collider.attachedRigidbody;
		if (attachedRigidbody == null)
		{
			return false;
		}
		Part component = attachedRigidbody.GetComponent<Part>();
		if (component == null)
		{
			return false;
		}
		if (component.vessel == base.vessel)
		{
			return true;
		}
		return false;
	}

	public void LSchloomphaVPPProcessing()
	{
		if (!(Time.timeSinceLevelLoad > 3f) || !base.vessel.loaded || !base.vessel.Landed || !isGrounded || !(wheel != null) || (!(brakesSubmodule != null) && !(wheelLockSubmodule != null)))
		{
			return;
		}
		if ((brakesSubmodule != null && brakesSubmodule.brakeInput >= 1f) || (wheelLockSubmodule != null && wheel.brakeInput >= 1f))
		{
			if (((wheelType == WheelType.const_2) ? (base.vessel.srfSpeed < 0.10000000149011612) : (base.vessel.srfSpeed < 9.999999747378752E-06)) && !rbBrakeConstraints)
			{
				toggleRbConstraints(freeze: true);
				StartCoroutine(CallbackUtil.DelayedCallback(schloompaTime, toggleRbConstraints, arg: false));
				rbBrakeConstraints = true;
			}
		}
		else
		{
			rbBrakeConstraints = false;
		}
	}

	public void EnableWheelCollider()
	{
		wheel.wheelCollider.gameObject.SetActive(value: true);
		WheelCollider wheelCollider = wheel.wheelCollider.ResetWheelCollider();
		CollisionManager.IgnoreCollidersOnVessel(base.vessel, wheelCollider);
		wheel.wheelCollider.enabled = true;
	}

	public void DisableWheelCollider(bool immediate = false)
	{
		wheel.wheelCollider.enabled = false;
		WheelCollider wheelCollider = wheel.wheelCollider.GetWheelCollider();
		if (wheelCollider != null)
		{
			if (immediate)
			{
				UnityEngine.Object.DestroyImmediate(wheelCollider);
			}
			else
			{
				UnityEngine.Object.Destroy(wheelCollider);
			}
		}
		wheel.wheelCollider.gameObject.SetActive(value: false);
	}

	public void OnVesselModified(Vessel v)
	{
		if (!(v == null) && v == base.vessel && ApplyForcesToParent)
		{
			if (base.part.parent == null)
			{
				wheel.tgtRb = base.part.Rigidbody;
				tgtParent = null;
			}
			else if (tgtParent == null || tgtParent.vessel != base.vessel || tgtParent != base.part.parent)
			{
				wheel.tgtRb = base.part.parent.Rigidbody;
				tgtParent = base.part.parent;
			}
		}
	}

	public override void ResetWheelGroundCheck()
	{
		gColliderPrev = null;
	}

	public void OnPutToGround(PartHeightQuery qry)
	{
		if (groundHeightOffset != 0f)
		{
			ModuleWheelDeployment moduleWheelDeployment = base.part.FindModuleImplementing<ModuleWheelDeployment>();
			bool flag = false;
			if (moduleWheelDeployment != null && moduleWheelDeployment.stateString == "Retracted")
			{
				qry.lowestOnParts[base.part] = Mathf.Min(qry.lowestOnParts[base.part], base.part.partTransform.TransformPoint(WheelOrgPosR).y - radius);
				flag = true;
			}
			if (!flag)
			{
				Debug.Log(("Putting to ground, manually-defined ground offset: " + groundHeightOffset) ?? "");
				qry.lowestOnParts[base.part] = Mathf.Min(qry.lowestOnParts[base.part], base.part.partTransform.position.y - groundHeightOffset);
			}
		}
		else
		{
			qry.lowestOnParts[base.part] = Mathf.Min(qry.lowestOnParts[base.part], base.part.partTransform.TransformPoint(WheelOrgPosR).y - radius);
		}
		qry.lowestPoint = Mathf.Min(qry.lowestPoint, qry.lowestOnParts[base.part]);
	}

	public void RegisterSubmodule(ModuleWheelSubmodule m)
	{
		subModules.AddUnique(m);
		if (setup)
		{
			m.OnWheelInit(wheel);
		}
	}

	public void UnregisterSubmodule(ModuleWheelSubmodule m)
	{
		subModules.Remove(m);
	}

	public void OnWheelSetup(KSPWheelController w)
	{
		int count = subModules.Count;
		while (count-- > 0)
		{
			subModules[count].OnWheelInit(w);
		}
	}

	public void onPartPack(Part p)
	{
		if (p == base.part)
		{
			InopSystems.AddSubsystem(inopOnRails);
		}
	}

	public void onPartUnpack(Part p)
	{
		if (p == base.part)
		{
			InopSystems.RemoveSubsystem(inopOnRails);
		}
	}

	public void onDockingComplete(GameEvents.FromToAction<Part, Part> FromTo)
	{
		if (FromTo.from.vessel == base.vessel || FromTo.to.vessel == base.vessel)
		{
			holdWheelDamage();
			CollisionManager.IgnoreCollidersOnVessel(base.vessel, wheel.wheelCollider.GetWheelCollider());
			suspensionEnabled = false;
			CheckSuspensionToggle();
			toggleRbConstraints(freeze: false);
			InopWheelCollider(inop: true, force: true);
			InopWheelCollider(inop: false, force: true);
			wheel.wheelCollider.springRate = 0f;
			suspensionEnabled = true;
			CheckSuspensionToggle();
			if (brakesSubmodule != null)
			{
				float brakeInput = brakesSubmodule.brakeInput;
				brakesSubmodule.brakeInput = 1f;
				StartCoroutine(CallbackUtil.DelayedCallback(1f, resetBrakeInput, brakeInput));
			}
			rbBrakeConstraints = false;
			if (wheelType != WheelType.const_2)
			{
				LSchloomphaVPPProcessing();
			}
		}
	}

	public void resetBrakeInput(float prevInput)
	{
		brakesSubmodule.brakeInput = prevInput;
	}

	public void onVesselUndocking(Vessel fromVessel, Vessel toVessel)
	{
		if ((fromVessel == base.vessel || toVessel == base.vessel) && !fromVessel.isEVA && !toVessel.isEVA)
		{
			holdWheelDamage();
			CollisionManager.IgnoreCollidersOnVessel(base.vessel, wheel.wheelCollider.GetWheelCollider());
			suspensionEnabled = false;
			CheckSuspensionToggle();
			toggleRbConstraints(freeze: false);
			InopWheelCollider(inop: true, force: true);
			InopWheelCollider(inop: false, force: true);
			wheel.wheelCollider.springRate = 0f;
			suspensionEnabled = true;
			CheckSuspensionToggle();
			rbBrakeConstraints = false;
		}
	}

	public void toggleRbConstraints(bool freeze)
	{
		if (freeze)
		{
			InputLockManager.SetControlLock(ControlTypes.GROUP_BRAKES | ControlTypes.WHEEL_STEER | ControlTypes.WHEEL_THROTTLE, "WheelRBConstraints");
			holdWheelDamage(schloompaTime * 1.5f);
			CollisionManager.IgnoreCollidersOnVessel(base.vessel, wheel.wheelCollider.GetWheelCollider());
			base.part.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		else
		{
			base.part.Rigidbody.constraints = RigidbodyConstraints.None;
			InputLockManager.RemoveControlLock("WheelRBConstraints");
		}
	}

	public void holdWheelDamage(float seconds = 3f)
	{
		if (moduleWheelDamage == null)
		{
			moduleWheelDamage = GetComponent<ModuleWheelDamage>();
		}
		if (moduleWheelDamage != null)
		{
			moduleWheelDamage.startupTime = seconds;
		}
	}

	public void SOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> FromTo)
	{
		if (FromTo.from == base.vessel)
		{
			ApplyGeeBias((float)FromTo.to.GeeASL);
		}
	}

	public override void OnInventoryModeDisable()
	{
		if (wheel != null)
		{
			onPartPack(base.part);
			DisableWheelCollider(immediate: true);
			VPWheelCollider componentInChildren = wheel.GetComponentInChildren<VPWheelCollider>(includeInactive: true);
			if (componentInChildren != null)
			{
				UnityEngine.Object.Destroy(componentInChildren);
			}
			UnityEngine.Object.Destroy(wheel);
		}
		if (base.part.Rigidbody != null)
		{
			base.part.Rigidbody.isKinematic = true;
		}
	}

	public override void OnInventoryModeEnable()
	{
		if (base.part.Rigidbody != null)
		{
			base.part.Rigidbody.isKinematic = false;
		}
		onPartUnpack(base.part);
	}

	public override void DemoteToPhysicslessPart()
	{
		OnInventoryModeDisable();
	}

	public override void PromoteToPhysicalPart()
	{
		if (base.part.rb != null)
		{
			base.part.rb.isKinematic = false;
		}
		else
		{
			base.part.rb = base.gameObject.AddComponent<Rigidbody>();
			base.part.rb.isKinematic = false;
			base.part.rb.useGravity = false;
		}
		if (wheelColliderHost == null)
		{
			wheelColliderHost = base.part.FindModelTransform(wheelColliderTransformName);
		}
		if (wheel == null)
		{
			wheelSetup();
			wheelColliderHost.transform.SetParent(base.part.partTransform.Find("model"));
			wheel.wheelCollider.gameObject.SetActive(value: true);
		}
		if (wheel != null && wheel.wheelCollider == null)
		{
			wheel.wheelCollider = wheelColliderHost.GetComponent<VPWheelCollider>() ?? wheelColliderHost.gameObject.AddComponent<VPWheelCollider>();
			wheel.wheelCollider.ResetWheelCollider();
		}
		onPartUnpack(base.part);
	}

	public override string GetInfo()
	{
		string text = "";
		int count = subModules.Count;
		while (count-- > 0)
		{
			string text2 = subModules[count].OnGatherInfo();
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + text2 + "\n";
			}
		}
		return text;
	}

	public string GetModuleTitle()
	{
		return "Wheel";
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return TooltipPrimaryField;
	}

	public override string GetModuleDisplayName()
	{
		return TooltipTitle;
	}

	public void OnSubsystemsModified(WheelSubsystems sub)
	{
		if (sub == inopSystems)
		{
			InopUpdate();
		}
	}

	public void InopUpdate(bool force = false)
	{
		InopWheelCollider(inopSystems.HasType(WheelSubsystem.SystemTypes.WheelCollider), force);
		InopWheelTransform(inopSystems.HasType(WheelSubsystem.SystemTypes.Tire));
	}

	public void InopWheelCollider(bool inop, bool force)
	{
		if (wheel == null)
		{
			return;
		}
		if (inop && (force || wheel.enabled) && setup)
		{
			DisableWheelCollider(force);
			wheel.enabled = false;
			wheel.IsGrounded = false;
			if (clipGameObject != null)
			{
				clipGameObject.layer = 26;
			}
		}
		if (!inop && (force || !wheel.enabled) && setup)
		{
			Vector3 velocity = base.part.Rigidbody.velocity;
			EnableWheelCollider();
			wheel.enabled = true;
			wheel.rb.velocity = velocity;
			if (clipGameObject != null)
			{
				clipGameObject.layer = 30;
				clipGameObject.tag = "Wheel_Piston_Collider";
			}
		}
	}

	public void InopWheelTransform(bool disable)
	{
		wheel.wheelCollider.updateWheel = !disable;
	}

	public virtual void EnableSuspension(KSPActionParam param)
	{
		suspensionEnabled = true;
	}

	public virtual void DisableSuspension(KSPActionParam param)
	{
		suspensionEnabled = false;
	}

	public void CheckSuspensionToggle()
	{
		if (!suspensionEnabled && !InopSystems.Contains(inopSuspension))
		{
			InopSystems.AddSubsystem(inopSuspension);
		}
		else if (suspensionEnabled && InopSystems.Contains(inopSuspension))
		{
			InopSystems.RemoveSubsystem(inopSuspension);
		}
	}

	public void updateDriftFix()
	{
		switch (driftCorrectionState)
		{
		case DriftCorrectionState.Idle:
			if (wheel.IsGrounded)
			{
				driftCorrectionState = DriftCorrectionState.Acquire;
			}
			break;
		case DriftCorrectionState.Acquire:
			if (wheel.IsGrounded)
			{
				if (wheel.currentState.localWheelVelocity.sqrMagnitude < Mathf.Pow(acquireMaxSpeed, 2f))
				{
					fixFwd = getFixFwd();
					error = Vector3.zero;
					driftCorrectionState = DriftCorrectionState.Fix;
				}
			}
			else
			{
				driftCorrectionState = DriftCorrectionState.Idle;
			}
			break;
		case DriftCorrectionState.Fix:
			if (wheel.IsGrounded)
			{
				if (wheel.currentState.localWheelVelocity.sqrMagnitude < Mathf.Pow(acquireMaxSpeed, 2f) && wheel.IsGrounded)
				{
					getFixTorque(fixFwd, getFixFwd());
					Vector3 vector = error * ki;
					Vector3 vector2 = (error - errorLast) * kd;
					wheel.RbTgt.AddTorque(vector + vector2, ForceMode.Acceleration);
				}
				else
				{
					driftCorrectionState = DriftCorrectionState.Acquire;
				}
			}
			else
			{
				driftCorrectionState = DriftCorrectionState.Idle;
			}
			break;
		}
	}

	public Vector3 getFixFwd()
	{
		if (wheelType != WheelType.const_2)
		{
			return wheel.cachedTransform.forward;
		}
		return wheel.cachedTransform.right;
	}

	public void getFixTorque(Vector3 fixOrt, Vector3 refOrt)
	{
		Vector3 vector = Vector3.Cross(refOrt, fixOrt) * (float)base.vessel.mainBody.GeeASL;
		errorLast = error;
		error += vector / Time.deltaTime;
	}

	[KSPEvent(active = true, guiActive = true, guiActiveEditor = true, guiName = "")]
	public void EvtAutoFrictionToggle()
	{
		autoFriction = !autoFriction;
		ActionUIUpdate();
		ATsymPartUpdate();
	}

	[KSPAction("#autoLOC_6001457")]
	public void ActAutoFrictionToggle(KSPActionParam act)
	{
		autoFriction = act.type != 0 || (act.type == KSPActionType.Toggle && !autoFriction);
		ActionUIUpdate();
	}

	public void ActionUIUpdate()
	{
		if (autoFrictionAvailable)
		{
			evtAutoFrictionToggle.guiName = Localizer.Format("#autoLOC_7001003", Convert.ToInt32(autoFriction));
			fldFrictionMultiplier.guiActive = !autoFriction;
			fldFrictionMultiplier.guiActiveEditor = !autoFriction;
			if (HighLogic.LoadedSceneIsFlight && base.part.vessel.mainBody != null)
			{
				ApplyGeeBias((float)base.part.vessel.mainBody.GeeASL);
			}
			else if (HighLogic.LoadedSceneIsFlight)
			{
				ApplyGeeBias(1f);
			}
			return;
		}
		evtAutoFrictionToggle.guiActive = false;
		evtAutoFrictionToggle.guiActiveEditor = false;
		fldFrictionMultiplier.guiActive = false;
		fldFrictionMultiplier.guiActiveEditor = false;
		autoFriction = false;
		frictionMultiplier = 1f;
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (base.part.vessel.mainBody != null)
			{
				ApplyGeeBias((float)base.part.vessel.mainBody.GeeASL);
			}
			else
			{
				ApplyGeeBias(1f);
			}
		}
	}

	public void ATsymPartUpdate()
	{
		int i = 0;
		for (int count = base.part.symmetryCounterparts.Count; i < count; i++)
		{
			ModuleWheelBase obj = base.part.symmetryCounterparts[i].Modules[base.part.Modules.IndexOf(this)] as ModuleWheelBase;
			obj.autoFriction = autoFriction;
			obj.ActionUIUpdate();
		}
	}

	public string GetContractObjectiveType()
	{
		return "Wheel";
	}

	public bool CheckContractObjectiveValidity()
	{
		return wheelType == WheelType.MOTORIZED;
	}
}
