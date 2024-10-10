using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;
using ns9;
using UnityEngine;

public class ModuleControlSurface : ModuleLiftingSurface, IMultipleDragCube, ITorqueProvider
{
	[KSPField]
	public new string transformName = "obj_ctrlSrf";

	[KSPField]
	public float ctrlSurfaceRange = 15f;

	[KSPField]
	public float ctrlSurfaceArea = 0.5f;

	[KSPField]
	public float actuatorSpeed = 25f;

	public float originalActuatorSpeed;

	[KSPField]
	public bool useExponentialSpeed;

	[KSPField]
	public float actuatorSpeedNormScale = 25f;

	[KSPField]
	public bool alwaysRecomputeLift;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool mirrorDeploy;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool usesMirrorDeploy;

	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	public bool ignorePitch;

	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001331")]
	public bool ignoreYaw;

	[UI_Toggle(disabledText = "#autoLOC_6001079", scene = UI_Scene.All, enabledText = "#autoLOC_6001078", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001332")]
	public bool ignoreRoll;

	[UI_Toggle(disabledText = "#autoLOC_6001081", scene = UI_Scene.All, enabledText = "#autoLOC_6001080", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001333")]
	public bool deploy;

	[UI_Toggle(disabledText = "#autoLOC_6001075", scene = UI_Scene.All, enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001334")]
	public bool deployInvert;

	[KSPField(unfocusedRange = 25f, guiActiveUnfocused = true, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001335")]
	[UI_Toggle(disabledText = "#autoLOC_6001075", enabledText = "#autoLOC_6001077", affectSymCounterparts = UI_Scene.None)]
	public bool partDeployInvert;

	[Obsolete("Do not use as per #24665")]
	public bool precessionControl;

	public Vector3 potentialBladeControlTorque;

	public Vector3 vesselBladeLiftReference;

	[KSPAxisField(unfocusedRange = 25f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, incrementalSpeed = 50f, guiFormat = "0", axisMode = KSPAxisMode.Incremental, minValue = -100f, guiActive = true, guiName = "#autoLOC_6013041", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = -100f, affectSymCounterparts = UI_Scene.All)]
	public float deployAngle = float.NaN;

	public Vector2 deployAngleLimits = new Vector2(float.NaN, float.NaN);

	[KSPAxisField(minValue = -150f, incrementalSpeed = 60f, guiActiveUnfocused = false, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 150f, guiActive = false, guiName = "#autoLOC_6001336")]
	public float authorityLimiter = 100f;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 150f, minValue = -150f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(guiActiveUnfocused = true, guiFormat = "0", isPersistant = false, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001336", guiUnits = "°")]
	public float authorityLimiterUI = 100f;

	public float maxAuthority = 150f;

	public Vector3 inputVector;

	public Vector3 rotatingControlInput;

	public Quaternion airflowIncidence;

	public Vector3 baseLiftForce = Vector3.zero;

	public float action;

	public float deflection;

	public float roll;

	public float deflectionDirection = 1f;

	public Rigidbody referenceRigidBody;

	public bool partActionWindowOpen;

	[SerializeField]
	public Quaternion neutral;

	[SerializeField]
	public Transform ctrlSurface;

	[SerializeField]
	public double bladeUpdateInterval = 1.0;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012000", guiUnits = "°")]
	public float angleOfAttack;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012001", guiUnits = "#autoLOC_6002488")]
	public float forwardLift;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012002", guiUnits = "#autoLOC_6002488")]
	public float verticalLift;

	[KSPField(advancedTweakable = true, guiFormat = "F1", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8012003", guiUnits = "#autoLOC_6002487")]
	public float airSpeed;

	public double lastBladeInfoUpdate;

	[SerializeField]
	public ModuleRoboticServoRotor parentRotor;

	public IRoboticServo parentRotorServo;

	public float bladePitch;

	public BaseField verticalLiftField;

	public BaseField forwardLiftField;

	public BaseField airSpeedField;

	public BaseField aoaField;

	public BaseField bladePitchField;

	public bool comCalculated;

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003384")]
	public string PitchCtrlState = "n/a";

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003385")]
	public string YawCtrlState = "n/a";

	[KSPField(groupName = "RotationControlState", groupDisplayName = "#autoLOC_6013043", guiActive = false, advancedTweakable = true, guiName = "#autoLOC_8003386")]
	public string RollCtrlState = "n/a";

	public new List<AdjusterControlSurfaceBase> adjusterCache = new List<AdjusterControlSurfaceBase>();

	public static string cacheAutoLOC_6003032;

	public static string cacheAutoLOC_8003402;

	public static string cacheAutoLOC_8003403;

	public static string cacheAutoLOC_8003404;

	public float currentDeployAngle { get; set; }

	public bool IsMultipleCubesActive => true;

	public override void OnAwake()
	{
		base.OnAwake();
		if (ctrlSurface != null)
		{
			ctrlSurface.localRotation = neutral;
		}
		ctrlSurface = null;
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onPartActionUIShown.Add(OnPartActionUIShown);
			GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
		}
		originalActuatorSpeed = actuatorSpeed;
		if (displaceVelocity)
		{
			verticalLiftField = base.Fields["verticalLift"];
			forwardLiftField = base.Fields["forwardLift"];
			airSpeedField = base.Fields["airSpeed"];
			aoaField = base.Fields["angleOfAttack"];
			bladePitchField = base.Fields["bladePitch"];
		}
	}

	public void OnDestroy()
	{
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Remove(onVariantApplied);
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onVesselReferenceTransformSwitch.Remove(onVesselReferenceTransformSwitch);
		}
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		if (p == base.part && (evt == ConstructionEventType.PartPicked || evt == ConstructionEventType.PartDetached))
		{
			deploy = false;
			ctrlSurface.localRotation = neutral;
		}
	}

	public void onVariantApplied(Part eventPart, PartVariant variant)
	{
		if (!(eventPart != base.part))
		{
			bool result = false;
			string extraInfoValue = variant.GetExtraInfoValue("reverseDeployDirection");
			if (!string.IsNullOrEmpty(extraInfoValue))
			{
				bool.TryParse(extraInfoValue, out result);
			}
			deflectionDirection = ((!result) ? 1 : (-1));
		}
	}

	public void OnPartActionUIShown(UIPartActionWindow paw, Part p)
	{
		if (p.persistentId == base.part.persistentId)
		{
			partActionWindowOpen = true;
		}
	}

	public void OnPartActionUIDismiss(Part p)
	{
		if (p.persistentId == base.part.persistentId)
		{
			partActionWindowOpen = false;
		}
	}

	public void ModifyAuthorityLimiter(object field)
	{
		authorityLimiterUI = ctrlSurfaceRange * authorityLimiter * 0.01f;
		if (UIPartActionController.Instance != null)
		{
			UIPartActionWindow item = UIPartActionController.Instance.GetItem(base.part);
			if (item != null)
			{
				item.displayDirty = true;
			}
		}
	}

	public void ModifyAuthorityLimiterUI(object field)
	{
		authorityLimiter = 100f * authorityLimiterUI / ctrlSurfaceRange;
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Add(onVariantApplied);
		}
		if (float.IsNaN(deployAngle))
		{
			deployAngle = ctrlSurfaceRange;
		}
		if (float.IsNaN(deployAngleLimits.x))
		{
			deployAngleLimits.x = (0f - ctrlSurfaceRange) * 1.5f;
		}
		if (float.IsNaN(deployAngleLimits.y))
		{
			deployAngleLimits.y = ctrlSurfaceRange * 1.5f;
		}
		if (HighLogic.LoadedSceneIsGame)
		{
			ctrlSurface = base.part.FindModelTransform(transformName);
			if (ctrlSurface != null)
			{
				neutral = ctrlSurface.localRotation;
			}
			base.Fields["authorityLimiter"].OnValueModified += ModifyAuthorityLimiter;
			base.Fields["authorityLimiterUI"].OnValueModified += ModifyAuthorityLimiterUI;
			if (base.Fields.TryGetFieldUIControl<UI_FloatRange>("authorityLimiterUI", out var control))
			{
				control.minValue = -1.5f * ctrlSurfaceRange;
				control.maxValue = 1.5f * ctrlSurfaceRange;
				control.stepIncrement = 0.015f * ctrlSurfaceRange;
			}
			ModifyAuthorityLimiter(null);
			base.Fields["partDeployInvert"].guiActive = false;
			base.Fields["partDeployInvert"].guiActiveEditor = false;
			BaseAxisField obj = base.Fields["deployAngle"] as BaseAxisField;
			obj.minValue = deployAngleLimits.x;
			obj.maxValue = deployAngleLimits.y;
			obj.incrementalSpeed = deployAngleLimits.y * 0.2f;
			if (base.Fields.TryGetFieldUIControl<UI_FloatRange>("deployAngle", out var control2))
			{
				control2.minValue = deployAngleLimits.x;
				control2.maxValue = deployAngleLimits.y;
				control2.stepIncrement = (deployAngleLimits.y - deployAngleLimits.x) * 0.01f;
			}
		}
		if (HighLogic.LoadedSceneIsFlight && displaceVelocity)
		{
			parentRotor = base.part.FindParentModuleImplementing<ModuleRoboticServoRotor>();
			if (parentRotor != null)
			{
				parentRotorServo = parentRotor;
			}
		}
		if (displaceVelocity)
		{
			if (verticalLiftField != null)
			{
				verticalLiftField.guiActive = true;
				verticalLiftField.guiUnits = Localizer.Format(verticalLiftField.guiUnits);
			}
			if (forwardLiftField != null)
			{
				forwardLiftField.guiActive = true;
				forwardLiftField.guiUnits = Localizer.Format(forwardLiftField.guiUnits);
			}
			if (airSpeedField != null)
			{
				airSpeedField.guiActive = true;
				airSpeedField.guiUnits = Localizer.Format(airSpeedField.guiUnits);
			}
			if (aoaField != null)
			{
				aoaField.guiActive = true;
			}
			base.Fields["PitchCtrlState"].guiActive = true;
			base.Fields["YawCtrlState"].guiActive = true;
			base.Fields["RollCtrlState"].guiActive = true;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onVesselReferenceTransformSwitch.Add(onVesselReferenceTransformSwitch);
		}
	}

	public void onVesselReferenceTransformSwitch(Transform from, Transform to)
	{
		Part rigidBodyPart = to.gameObject.GetComponentUpwards<Part>().RigidBodyPart;
		referenceRigidBody = ((rigidBodyPart != null) ? rigidBodyPart.Rigidbody : null);
	}

	public new void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		liftField.guiActive = PhysicsGlobals.AeroDataDisplay;
		dragField.guiActive = PhysicsGlobals.AeroDataDisplay && useInternalDragModel;
		if (base.part.Rigidbody != null && !base.part.ShieldedFromAirstream)
		{
			pointVelocity = base.part.Rigidbody.GetPointVelocity(displaceVelocity ? baseTransform.TransformPoint(velocityOffset) : baseTransform.position) + Krakensbane.GetFrameVelocityV3f();
			double submergedPortion = base.part.submergedPortion;
			double num = 1.0 - submergedPortion;
			float mach = (float)base.part.machNumber;
			double dynamicPressurekPa = base.part.dynamicPressurekPa;
			if (displaceVelocity && base.vessel != null)
			{
				mach = ((!(base.vessel.speedOfSound > 0.0)) ? pointVelocity.magnitude : (pointVelocity.magnitude / (float)base.vessel.speedOfSound));
				airSpeed = pointVelocity.magnitude;
				dynamicPressurekPa = base.part.atmDensity;
				Vector3 vector = pointVelocity;
				float sqrMagnitude = vector.sqrMagnitude;
				double num2 = 0.0005 * (double)sqrMagnitude;
				dynamicPressurekPa *= num2;
				Qlift = (dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedLiftScalar) * 1000.0;
				Qdrag = (dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedDragScalar) * 1000.0;
			}
			else
			{
				Qlift = (base.part.dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedLiftScalar) * 1000.0;
				Qdrag = (base.part.dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedDragScalar) * 1000.0;
			}
			SetupCoefficients(pointVelocity, out nVel, out liftVector, out liftDot, out absDot);
			baseLiftForce = GetLiftVector(liftVector, liftDot, absDot, Qlift, mach);
			liftForce = baseLiftForce * (1f - ctrlSurfaceArea);
			if (useInternalDragModel)
			{
				dragForce = GetDragVector(nVel, absDot, Qdrag, mach) * (1f - ctrlSurfaceArea);
			}
			float f = liftDot;
			float num3 = absDot;
			if (ctrlSurface != null)
			{
				actuatorSpeed = originalActuatorSpeed;
				actuatorSpeed = ApplyActuatorSpeedAdjustments(actuatorSpeed);
				CtrlSurfaceUpdate(pointVelocity);
				if (alwaysRecomputeLift || ctrlSurface.localRotation != neutral)
				{
					airflowIncidence = Quaternion.AngleAxis(deflection, baseTransform.rotation * Vector3.right);
					liftVector = airflowIncidence * liftVector;
					f = Vector3.Dot(nVel, liftVector);
					num3 = Mathf.Abs(f);
				}
			}
			bool num4 = liftScalar > 0f;
			bool flag = dragScalar > 0f;
			liftForce += GetLiftVector(liftVector, f, num3, Qlift, mach) * ctrlSurfaceArea;
			liftForce = ApplyLiftForceAdjustments(liftForce);
			base.part.AddForceAtPosition(liftForce, base.part.partTransform.TransformPoint(base.part.CoLOffset));
			if (useInternalDragModel)
			{
				dragForce += GetDragVector(nVel, num3, Qdrag, mach) * ctrlSurfaceArea;
				base.part.AddForceAtPosition(dragForce, base.part.partTransform.TransformPoint(base.part.CoPOffset));
			}
			if (num4)
			{
				liftScalar = liftForce.magnitude;
			}
			if (flag)
			{
				dragScalar = dragForce.magnitude;
			}
			float num5 = deflection / ctrlSurfaceRange;
			base.part.DragCubes.SetCubeWeight("neutral", maxAuthority * 0.01f - Mathf.Abs(num5));
			base.part.DragCubes.SetCubeWeight("fullDeflectionPos", Mathf.Clamp01(num5));
			base.part.DragCubes.SetCubeWeight("fullDeflectionNeg", Mathf.Clamp01(0f - num5));
			UpdateAeroDisplay(Color.yellow);
		}
		else
		{
			double qlift = 0.0;
			Qdrag = 0.0;
			Qlift = qlift;
			nVel = Vector3.zero;
		}
		if (displaceVelocity && partActionWindowOpen && Planetarium.GetUniversalTime() - lastBladeInfoUpdate > bladeUpdateInterval)
		{
			CalcAngleOfAttack();
			forwardLift = Vector3.Dot(liftForce, base.vessel.ReferenceTransform.up);
			verticalLift = Vector3.Dot(liftForce, base.vessel.upAxis);
			lastBladeInfoUpdate = Planetarium.GetUniversalTime();
		}
		if (displaceVelocity && !comCalculated && base.part.rb != null)
		{
			CalculateCoM();
		}
	}

	public void CalcAngleOfAttack()
	{
		angleOfAttack = Mathf.Abs(Vector3.Angle(nVel, Vector3.ProjectOnPlane(nVel, ctrlSurface.up)));
	}

	public Vector3 GetPotentialLift(bool positiveDeflection)
	{
		Vector3 vector = baseLiftForce * ctrlSurfaceArea;
		float num = (positiveDeflection ? 1 : (-1));
		Vector3 rhs = Quaternion.AngleAxis(currentDeployAngle + num * ctrlSurfaceRange * authorityLimiter * 0.01f, baseTransform.rotation * Vector3.right) * baseTransform.forward;
		float f = Vector3.Dot(nVel, rhs);
		float num2 = Mathf.Abs(f);
		return GetLiftVector(rhs, f, num2, Qlift, (float)base.part.machNumber) * ctrlSurfaceArea - vector;
	}

	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		pos = (neg = Vector3.zero);
		if (Qlift != 0.0 && (!ignorePitch || !ignoreYaw || !ignoreRoll))
		{
			Vector3 currentCoM = base.vessel.CurrentCoM;
			Vector3d vector3d = base.part.Rigidbody.worldCenterOfMass - currentCoM;
			Vector3 b = base.vessel.ReferenceTransform.InverseTransformDirection(vector3d);
			Vector3 potentialLift = GetPotentialLift(positiveDeflection: true);
			Vector3 potentialLift2 = GetPotentialLift(positiveDeflection: false);
			if (displaceVelocity)
			{
				float magnitude = vesselBladeLiftReference.magnitude;
				pos = Vector3.Dot(potentialLift, vesselBladeLiftReference) * potentialBladeControlTorque / magnitude;
				neg = Vector3.Dot(potentialLift2, vesselBladeLiftReference) * potentialBladeControlTorque / magnitude;
			}
			else
			{
				pos = Vector3.Scale(base.vessel.ReferenceTransform.InverseTransformDirection(Vector3.Cross(vector3d, potentialLift)), b);
				neg = Vector3.Scale(base.vessel.ReferenceTransform.InverseTransformDirection(Vector3.Cross(vector3d, potentialLift2)), b);
			}
			if (ignorePitch)
			{
				float x = 0f;
				neg.x = 0f;
				pos.x = x;
			}
			if (ignoreRoll)
			{
				float x = 0f;
				neg.y = 0f;
				pos.y = x;
			}
			else
			{
				pos.y = 0f - pos.y;
				neg.y = 0f - neg.y;
			}
			if (ignoreYaw)
			{
				float x = 0f;
				neg.z = 0f;
				pos.z = x;
			}
		}
	}

	public void LateUpdate()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			CtrlSurfaceEditorUpdate(EditorMarker_CoM.CraftCoM);
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_213511", deflectionLiftCoeff) + Localizer.Format("#autoLOC_213512", ctrlSurfaceArea * 100f) + Localizer.Format("#autoLOC_213513", ctrlSurfaceRange) + Localizer.Format("#autoLOC_213514", actuatorSpeed);
	}

	public float ActionSign(float action, float epsilon)
	{
		return (action > epsilon) ? 1 : ((action < epsilon) ? (-1) : 0);
	}

	public virtual void RotatingCtrlSurfaceUpdate(Vector3 vel)
	{
		Vector3 currentCoM = base.vessel.CurrentCoM;
		baseTransform.InverseTransformPoint(currentCoM);
		if (referenceRigidBody == null)
		{
			onVesselReferenceTransformSwitch(null, base.vessel.ReferenceTransform);
		}
		Vector3 lhs = Vector3.one;
		Vector3 lhs2 = Vector3.zero;
		Vector3 vector = (vesselBladeLiftReference = base.vessel.ReferenceTransform.InverseTransformVector(liftVector));
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		if (referenceRigidBody != null)
		{
			Rigidbody rigidbody = referenceRigidBody;
			Vector3 position = baseTransform.position;
			Vector3 vector2 = base.part.Rigidbody.angularVelocity - rigidbody.angularVelocity;
			Vector3 vector3 = Vector3.Cross(base.part.Rigidbody.GetPointVelocity(position) - rigidbody.GetPointVelocity(position), vector2) / Vector3.Dot(vector2, vector2);
			if ((double)Vector3.Dot(vector2, vector2) > 0.0001)
			{
				displayAxisArrow = true;
				Vector3 vector4 = position - vector3;
				rotationAxisDirection = vector2;
				rotationAxisPosition = baseTransform.InverseTransformPoint(vector4);
				lhs = base.vessel.ReferenceTransform.InverseTransformVector(baseTransform.TransformPoint(base.part.CoLOffset) - vector4);
				lhs2 = base.vessel.ReferenceTransform.InverseTransformVector(vector4 - currentCoM);
				vector2 = base.vessel.ReferenceTransform.InverseTransformVector(vector2);
				lhs = Quaternion.AngleAxis(vector2.magnitude * 180f * TimeWarp.fixedDeltaTime / (float)Math.PI, vector2) * lhs;
				vector2 = vector2.normalized;
				flag = Math.Abs(vector2.x) < 0.9962f;
				flag3 = Math.Abs(vector2.y) < 0.9962f;
				flag2 = Math.Abs(vector2.z) < 0.9962f;
			}
			else
			{
				PitchCtrlState = "n/a";
				RollCtrlState = "n/a";
				YawCtrlState = "n/a";
			}
			if (Vector3.Dot(vector2, vector) < 0f)
			{
				vector = -vector;
			}
		}
		Vector3 vector5 = Vector3.Cross(lhs, vector);
		Vector3 vector6 = Vector3.Cross(lhs2, vector);
		Vector3 zero = Vector3.zero;
		float magnitude = vector5.magnitude;
		vector5 = vector5.normalized;
		float magnitude2 = vector6.magnitude;
		vector6 = vector6.normalized;
		if (flag)
		{
			if (magnitude > Mathf.Abs(vector6.x) * magnitude2)
			{
				if (precessionControl)
				{
					zero.x = (0f - vector5.z - vector5.y) * deflectionDirection;
				}
				else
				{
					zero.x = vector5.x;
				}
				potentialBladeControlTorque.x = magnitude;
				PitchCtrlState = cacheAutoLOC_8003402;
			}
			else
			{
				zero.x = ActionSign(vector6.x, 0.01f);
				potentialBladeControlTorque.x = zero.x * magnitude2;
				PitchCtrlState = cacheAutoLOC_8003403;
			}
		}
		else
		{
			potentialBladeControlTorque.x = 0f;
			PitchCtrlState = cacheAutoLOC_8003404;
		}
		if (flag3)
		{
			if (magnitude > Mathf.Abs(vector6.y) * magnitude2)
			{
				if (precessionControl)
				{
					zero.y = (vector5.x - vector5.z) * deflectionDirection;
				}
				else
				{
					zero.y = vector5.y;
				}
				potentialBladeControlTorque.y = magnitude;
				RollCtrlState = cacheAutoLOC_8003402;
			}
			else
			{
				zero.y = ActionSign(vector6.y, 0.01f);
				potentialBladeControlTorque.y = (0f - zero.y) * magnitude2;
				RollCtrlState = cacheAutoLOC_8003403;
			}
		}
		else
		{
			potentialBladeControlTorque.y = 0f;
			RollCtrlState = cacheAutoLOC_8003404;
		}
		if (flag2)
		{
			if (magnitude > Mathf.Abs(vector6.z) * magnitude2)
			{
				if (precessionControl)
				{
					zero.z = (vector5.x - vector5.y) * deflectionDirection;
				}
				else
				{
					zero.z = vector5.z;
				}
				potentialBladeControlTorque.z = magnitude;
				YawCtrlState = cacheAutoLOC_8003402;
			}
			else
			{
				zero.z = ActionSign(vector6.z, 0.01f);
				potentialBladeControlTorque.z = zero.z * magnitude2;
				YawCtrlState = cacheAutoLOC_8003403;
			}
		}
		else
		{
			potentialBladeControlTorque.z = 0f;
			YawCtrlState = cacheAutoLOC_8003404;
		}
		FlightCtrlState ctrlState = base.part.vessel.ctrlState;
		inputVector = new Vector3(ignorePitch ? 0f : ctrlState.pitch, ignoreRoll ? 0f : ctrlState.roll, ignoreYaw ? 0f : ctrlState.yaw);
		float num = actuatorSpeed * TimeWarp.fixedDeltaTime / ctrlSurfaceRange;
		if (!useExponentialSpeed)
		{
			rotatingControlInput.x = Mathf.MoveTowards(rotatingControlInput.x, inputVector.x, num);
			rotatingControlInput.y = Mathf.MoveTowards(rotatingControlInput.y, inputVector.y, num);
			rotatingControlInput.z = Mathf.MoveTowards(rotatingControlInput.z, inputVector.z, num);
		}
		else
		{
			rotatingControlInput.x = Mathf.Lerp(rotatingControlInput.x, inputVector.x, num);
			rotatingControlInput.y = Mathf.Lerp(rotatingControlInput.y, inputVector.y, num);
			rotatingControlInput.z = Mathf.Lerp(rotatingControlInput.z, inputVector.z, num);
		}
		action = Vector3.Dot(rotatingControlInput, zero);
		deflection = ctrlSurfaceRange * action * authorityLimiter * 0.01f;
		if (deploy)
		{
			currentDeployAngle = (deployInvert ? (-1f) : 1f) * (partDeployInvert ? (-1f) : 1f) * deployAngle * deflectionDirection;
			deflection += currentDeployAngle;
		}
		deflection = Mathf.Clamp(deflection, -1.5f * ctrlSurfaceRange, 1.5f * ctrlSurfaceRange);
		ctrlSurface.localRotation = neutral * Quaternion.AngleAxis(0f - deflection, Vector3.right);
	}

	public virtual void FixedCtrlSurfaceUpdate(Vector3 vel)
	{
		Vector3 currentCoM = base.vessel.CurrentCoM;
		Vector3 vector = baseTransform.InverseTransformPoint(currentCoM);
		Vector3 rhs = new Vector3(vector.x, 0f, vector.z);
		inputVector = base.vessel.ReferenceTransform.rotation * new Vector3(ignorePitch ? 0f : base.part.vessel.ctrlState.pitch, 0f, ignoreYaw ? 0f : base.part.vessel.ctrlState.yaw);
		action = Vector3.Dot(inputVector, baseTransform.rotation * Vector3.right);
		if (ignoreRoll)
		{
			roll = 0f;
		}
		else
		{
			roll = Vector3.Dot(Vector3.right, rhs) * (1f - (Mathf.Abs(Vector3.Dot(rhs.normalized, Quaternion.Inverse(baseTransform.rotation) * base.vessel.ReferenceTransform.up)) * 0.5f + 0.5f)) * Mathf.Sign(Vector3.Dot(baseTransform.up, base.vessel.ReferenceTransform.up)) * base.part.vessel.ctrlState.roll * Mathf.Sign(ctrlSurfaceRange);
		}
		if (vector.y < 0f)
		{
			action = 0f - action;
		}
		action = deflectionDirection * Mathf.Clamp(action - roll, -1f, 1f);
		action = ctrlSurfaceRange * action * authorityLimiter * 0.01f;
		if (deploy)
		{
			float num = 0f;
			num = (usesMirrorDeploy ? ((deployInvert ? (-1f) : 1f) * (partDeployInvert ? (-1f) : 1f) * (mirrorDeploy ? (-1f) : 1f)) : ((deployInvert ? (-1f) : 1f) * Mathf.Sign((Quaternion.Inverse(base.vessel.ReferenceTransform.rotation) * (baseTransform.position - currentCoM)).x)));
			currentDeployAngle = (0f - num) * deployAngle;
			action += currentDeployAngle;
		}
		action = Mathf.Clamp(action, -1.5f * ctrlSurfaceRange, 1.5f * ctrlSurfaceRange);
		action *= deflectionDirection;
		if (!useExponentialSpeed)
		{
			deflection = Mathf.MoveTowards(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		else
		{
			deflection = Mathf.Lerp(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
	}

	public virtual void CtrlSurfaceUpdate(Vector3 vel)
	{
		if (displaceVelocity)
		{
			RotatingCtrlSurfaceUpdate(vel);
		}
		else
		{
			FixedCtrlSurfaceUpdate(vel);
		}
	}

	public virtual void CtrlSurfaceEditorUpdate(Vector3 CoM)
	{
		if (baseTransform == null)
		{
			return;
		}
		baseTransform.InverseTransformPoint(CoM);
		action = 0f;
		if (EditorLogic.fetch != null && EditorLogic.fetch.editorScreen == EditorScreen.Parts)
		{
			usesMirrorDeploy = true;
			mirrorDeploy = false;
			if (base.part.symMethod == SymmetryMethod.Mirror && base.part.symmetryCounterparts != null && base.part.symmetryCounterparts.Count > 0)
			{
				Part part = base.part.symmetryCounterparts[0];
				if (Mathf.Abs(base.part.transform.localRotation.w) < Mathf.Abs(part.transform.localRotation.w))
				{
					mirrorDeploy = true;
				}
				else if (Mathf.Abs(base.part.transform.localRotation.w) == Mathf.Abs(part.transform.localRotation.w) && base.part.transform.localRotation.x < part.transform.localRotation.x)
				{
					mirrorDeploy = true;
				}
			}
		}
		if (deploy)
		{
			action -= (deployInvert ? (-1f) : 1f) * (partDeployInvert ? (-1f) : 1f) * (mirrorDeploy ? (-1f) : 1f) * deployAngle;
		}
		action = Mathf.Clamp(action, -1.5f * ctrlSurfaceRange, 1.5f * ctrlSurfaceRange);
		action *= deflectionDirection;
		if (displaceVelocity)
		{
			deflection = action;
		}
		else if (!useExponentialSpeed)
		{
			deflection = Mathf.MoveTowards(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		else
		{
			deflection = Mathf.Lerp(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
	}

	[KSPAction("#autoLOC_6001337", KSPActionGroup.None)]
	public void ActionToggle(KSPActionParam act)
	{
		deploy = !deploy;
	}

	[KSPAction("#autoLOC_6001338", KSPActionGroup.None)]
	public void ActionExtend(KSPActionParam act)
	{
		deploy = true;
	}

	[KSPAction("#autoLOC_6001339", KSPActionGroup.None)]
	public void ActionRetract(KSPActionParam act)
	{
		deploy = false;
	}

	[KSPAction("#autoLOC_6006035", KSPActionGroup.None)]
	public void PitchActive(KSPActionParam act)
	{
		ignorePitch = false;
	}

	[KSPAction("#autoLOC_6006036", KSPActionGroup.None)]
	public void PitchInactive(KSPActionParam act)
	{
		ignorePitch = true;
	}

	[KSPAction("#autoLOC_6006037", KSPActionGroup.None)]
	public void TogglePitch(KSPActionParam act)
	{
		ignorePitch = !ignorePitch;
	}

	[KSPAction("#autoLOC_6006038", KSPActionGroup.None)]
	public void YawActive(KSPActionParam act)
	{
		ignoreYaw = false;
	}

	[KSPAction("#autoLOC_6006039", KSPActionGroup.None)]
	public void YawInactive(KSPActionParam act)
	{
		ignoreYaw = true;
	}

	[KSPAction("#autoLOC_6006040", KSPActionGroup.None)]
	public void ToggleYaw(KSPActionParam act)
	{
		ignoreYaw = !ignoreYaw;
	}

	[KSPAction("#autoLOC_6006041", KSPActionGroup.None)]
	public void RollActive(KSPActionParam act)
	{
		ignoreRoll = false;
	}

	[KSPAction("#autoLOC_6006042", KSPActionGroup.None)]
	public void RollInactive(KSPActionParam act)
	{
		ignoreRoll = true;
	}

	[KSPAction("#autoLOC_6006043", KSPActionGroup.None)]
	public void ToggleRoll(KSPActionParam act)
	{
		ignoreRoll = !ignoreRoll;
	}

	[KSPAction("#autoLOC_6006044", KSPActionGroup.None)]
	public void ActivateAllControls(KSPActionParam act)
	{
		ignorePitch = false;
		ignoreYaw = false;
		ignoreRoll = false;
	}

	[KSPAction("#autoLOC_6006045", KSPActionGroup.None)]
	public void DeactivateAllControls(KSPActionParam act)
	{
		ignorePitch = true;
		ignoreYaw = true;
		ignoreRoll = true;
	}

	public string[] GetDragCubeNames()
	{
		return new string[3] { "neutral", "fullDeflectionPos", "fullDeflectionNeg" };
	}

	public void AssumeDragCubePosition(string name)
	{
		switch (name)
		{
		case "fullDeflectionNeg":
			action = (0f - maxAuthority) * 0.01f;
			break;
		case "fullDeflectionPos":
			action = maxAuthority * 0.01f;
			break;
		default:
			action = 0f;
			break;
		}
		if (ctrlSurface == null)
		{
			baseTransform = base.transform;
			ctrlSurface = base.part.FindModelTransform(transformName);
			if (ctrlSurface != null)
			{
				neutral = ctrlSurface.localRotation;
			}
		}
		if (ctrlSurface != null)
		{
			ctrlSurface.localRotation = Quaternion.AngleAxis(ctrlSurfaceRange * action, Vector3.right) * neutral;
			return;
		}
		Debug.LogWarning("[ModuleControlSurface]: No ctrlSurface transform found! This is most likely wrong!", base.gameObject);
		Debug.Break();
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterControlSurfaceBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterControlSurfaceBase item = adjuster as AdjusterControlSurfaceBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyActuatorSpeedAdjustments(float actuatorSpeed)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			actuatorSpeed = adjusterCache[i].ApplyActuatorSpeedAdjustment(actuatorSpeed);
		}
		return actuatorSpeed;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003032;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_6003032 = Localizer.Format("#autoLoc_6003032");
		cacheAutoLOC_8003402 = Localizer.Format("#autoLOC_8003402");
		cacheAutoLOC_8003403 = Localizer.Format("#autoLOC_8003403");
		cacheAutoLOC_8003404 = Localizer.Format("#autoLOC_8003404");
	}

	public void CalculateCoM()
	{
		if (base.part.parent != null && base.part.parent.isRoboticRotor())
		{
			float mass = base.part.rb.mass;
			float mass2 = base.part.parent.servoRb.mass;
			float num = Vector3.Distance(base.part.parent.transform.position, base.part.transform.position);
			float x = mass * num / (mass2 + mass) / 2f;
			base.part.CoMOffset = new Vector3(x, 0f, 0f);
			base.part.rb.centerOfMass = Vector3.zero + base.part.CoMOffset;
			comCalculated = true;
		}
		else if (base.part.parent != null && !base.part.parent.isRoboticRotor())
		{
			float mass3 = base.part.rb.mass;
			float mass4 = base.part.parent.rb.mass;
			float num2 = Vector3.Distance(base.part.parent.transform.position, base.part.transform.position);
			float x2 = mass3 * num2 / (mass4 + mass3) * 2f;
			base.part.CoMOffset = new Vector3(x2, 0f, 0f);
			base.part.rb.centerOfMass = Vector3.zero + base.part.CoMOffset;
			comCalculated = true;
		}
	}
}
