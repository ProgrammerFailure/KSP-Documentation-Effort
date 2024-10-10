using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleLiftingSurface : PartModule, ILiftProvider
{
	public enum TransformDir
	{
		const_0,
		const_1,
		const_2
	}

	[KSPField]
	public TransformDir transformDir = TransformDir.const_2;

	[KSPField]
	public float transformSign = 1f;

	[KSPField]
	public string transformName = string.Empty;

	[KSPField]
	public float deflectionLiftCoeff = 1.5f;

	[KSPField]
	public bool omnidirectional = true;

	[KSPField]
	public bool nodeEnabled;

	[KSPField]
	public string attachNodeName = string.Empty;

	[KSPField]
	public bool disableBodyLift = true;

	[KSPField]
	public bool perpendicularOnly;

	[KSPField]
	public bool displaceVelocity;

	[KSPField]
	public Vector3 velocityOffset = Vector3.zero;

	[KSPField]
	public string liftingSurfaceCurve = "Default";

	[KSPField]
	public FloatCurve liftCurve = new FloatCurve();

	[KSPField]
	public FloatCurve liftMachCurve = new FloatCurve();

	[KSPField]
	public bool useInternalDragModel = true;

	[KSPField]
	public FloatCurve dragCurve = new FloatCurve();

	[KSPField]
	public FloatCurve dragMachCurve = new FloatCurve();

	public ArrowPointer liftArrow;

	public ArrowPointer dragArrow;

	public ArrowPointer velocityArrow;

	public ArrowPointer axisArrow;

	public bool displayAxisArrow;

	public Vector3 liftForce = Vector3.zero;

	public Vector3 dragForce = Vector3.zero;

	public Vector3 rotationAxisDirection;

	public Vector3 rotationAxisPosition;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001708", guiUnits = "#autoLOC_7001408")]
	public float liftScalar;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001711", guiUnits = "#autoLOC_7001408")]
	public float dragScalar;

	public float liftDot;

	public Vector3 nVel;

	public Vector3 liftVector;

	public Vector3 pointVelocity;

	public float absDot;

	public double Qlift;

	public double Qdrag;

	[SerializeField]
	public Transform baseTransform;

	public BaseField liftField;

	public BaseField dragField;

	public AttachNode attachNode;

	public List<AdjusterLiftingSurfaceBase> adjusterCache = new List<AdjusterLiftingSurfaceBase>();

	public static string cacheAutoLOC_6003045;

	public bool DisableBodyLift => disableBodyLift;

	public bool IsLifting => liftScalar != 0f;

	public override void OnAwake()
	{
		baseTransform = base.transform;
		liftField = base.Fields["liftScalar"];
		dragField = base.Fields["dragScalar"];
	}

	public override void OnStart(StartState state)
	{
		liftScalar = 0f;
		liftField.guiUnits = Localizer.Format(liftField.guiUnits);
		dragField.guiUnits = Localizer.Format(dragField.guiUnits);
		if (!string.IsNullOrEmpty(transformName))
		{
			Transform transform = base.part.FindModelTransform(transformName);
			if (transform != null)
			{
				baseTransform = transform;
			}
			else
			{
				baseTransform = base.transform;
			}
		}
		else
		{
			baseTransform = base.transform;
		}
		if (nodeEnabled && !string.IsNullOrEmpty(attachNodeName))
		{
			attachNode = base.part.FindAttachNode(attachNodeName);
			if (attachNode == null)
			{
				nodeEnabled = false;
			}
		}
		if (this.liftingSurfaceCurve != "Custom")
		{
			PhysicsGlobals.LiftingSurfaceCurve liftingSurfaceCurve = PhysicsGlobals.GetLiftingSurfaceCurve(this.liftingSurfaceCurve);
			if (liftingSurfaceCurve != null)
			{
				liftCurve = liftingSurfaceCurve.liftCurve;
				liftMachCurve = liftingSurfaceCurve.liftMachCurve;
				dragCurve = liftingSurfaceCurve.dragCurve;
				dragMachCurve = liftingSurfaceCurve.dragMachCurve;
			}
		}
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !(base.part.Rigidbody != null))
		{
			return;
		}
		liftField.guiActive = PhysicsGlobals.AeroDataDisplay;
		dragField.guiActive = PhysicsGlobals.AeroDataDisplay && useInternalDragModel;
		if (!base.part.ShieldedFromAirstream)
		{
			pointVelocity = base.part.Rigidbody.GetPointVelocity(displaceVelocity ? baseTransform.TransformPoint(velocityOffset) : baseTransform.position) + Krakensbane.GetFrameVelocityV3f();
			double submergedPortion = base.part.submergedPortion;
			double num = 1.0 - submergedPortion;
			Qlift = (base.part.dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedLiftScalar) * 1000.0;
			Qdrag = (base.part.dynamicPressurekPa * num + base.part.submergedDynamicPressurekPa * submergedPortion * base.part.submergedDragScalar) * 1000.0;
			SetupCoefficients(pointVelocity, out nVel, out liftVector, out liftDot, out var num2);
			liftForce = GetLiftVector(liftVector, liftDot, num2, Qlift, (float)base.part.machNumber);
			liftForce = ApplyLiftForceAdjustments(liftForce);
			base.part.AddForceAtPosition(liftForce, base.part.partTransform.TransformPoint(base.part.CoLOffset));
			if (useInternalDragModel)
			{
				dragForce = GetDragVector(nVel, num2, Qdrag);
				base.part.AddForceAtPosition(dragForce, base.part.partTransform.TransformPoint(base.part.CoPOffset));
			}
			UpdateAeroDisplay(Color.blue);
		}
		else
		{
			double qlift = 0.0;
			Qdrag = 0.0;
			Qlift = qlift;
			nVel = Vector3.zero;
		}
	}

	public void SetupCoefficients(Vector3 pointVelocity, out Vector3 nVel, out Vector3 liftVector, out float liftDot, out float absDot)
	{
		nVel = pointVelocity.normalized;
		if (transformDir == TransformDir.const_2)
		{
			liftVector = baseTransform.forward;
		}
		else if (transformDir == TransformDir.const_1)
		{
			liftVector = baseTransform.up;
		}
		else
		{
			liftVector = baseTransform.right;
		}
		liftVector *= transformSign;
		liftDot = Vector3.Dot(nVel, liftVector);
		if (omnidirectional)
		{
			absDot = Mathf.Abs(liftDot);
		}
		else
		{
			absDot = Mathf.Clamp01(liftDot);
		}
	}

	public Vector3 GetLiftVector(Vector3 liftVector, float liftDot, float absDot, double double_0, float mach)
	{
		if (nodeEnabled && attachNode.attachedPart != null)
		{
			liftScalar = 0f;
			return Vector3.zero;
		}
		liftScalar = Mathf.Sign(liftDot) * liftCurve.Evaluate(absDot) * liftMachCurve.Evaluate(mach);
		liftScalar *= deflectionLiftCoeff;
		if (liftScalar != 0f && !float.IsNaN(liftScalar))
		{
			liftScalar = (float)(double_0 * (double)(PhysicsGlobals.LiftMultiplier * liftScalar));
			if (perpendicularOnly)
			{
				Vector3 vector = -liftVector * liftScalar;
				vector = Vector3.ProjectOnPlane(vector, -nVel);
				liftScalar = vector.magnitude;
				return vector;
			}
			return -liftVector * liftScalar;
		}
		liftScalar = 0f;
		return Vector3.zero;
	}

	public Vector3 GetDragVector(Vector3 nVel, float absDot, double double_0)
	{
		return GetDragVector(nVel, absDot, double_0, (float)base.part.machNumber);
	}

	public Vector3 GetDragVector(Vector3 nVel, float absDot, double double_0, float mach)
	{
		if (nodeEnabled && attachNode.attachedPart != null)
		{
			dragScalar = 0f;
			return Vector3.zero;
		}
		dragScalar = dragCurve.Evaluate(absDot) * dragMachCurve.Evaluate(mach);
		dragScalar *= deflectionLiftCoeff;
		if (dragScalar != 0f && !float.IsNaN(dragScalar))
		{
			dragScalar = (float)(double_0 * (double)(dragScalar * PhysicsGlobals.LiftDragMultiplier));
			return -nVel * dragScalar;
		}
		dragScalar = 0f;
		return Vector3.zero;
	}

	public void UpdateAeroDisplay(Color LineColor)
	{
		if (PhysicsGlobals.AeroForceDisplay)
		{
			float length = Mathf.Abs(liftScalar) * PhysicsGlobals.AeroForceDisplayScale;
			if ((object)liftArrow == null)
			{
				liftArrow = ArrowPointer.Create(baseTransform, base.part.CoLOffset, liftForce, length, LineColor, worldSpace: true);
			}
			else
			{
				liftArrow.Direction = liftForce;
				liftArrow.Length = length;
			}
			if (useInternalDragModel)
			{
				float length2 = Mathf.Abs(dragScalar) * PhysicsGlobals.AeroForceDisplayScale;
				if ((object)dragArrow == null)
				{
					dragArrow = ArrowPointer.Create(baseTransform, base.part.CoPOffset, dragForce, length2, Color.red, worldSpace: true);
				}
				else
				{
					dragArrow.Direction = dragForce;
					dragArrow.Length = length2;
				}
			}
			if (displaceVelocity)
			{
				Mathf.Abs(pointVelocity.magnitude);
				_ = PhysicsGlobals.AeroForceDisplayScale;
				if ((object)velocityArrow == null)
				{
					velocityArrow = ArrowPointer.Create(baseTransform, velocityOffset, liftForce, length, Color.magenta, worldSpace: true);
				}
				else
				{
					velocityArrow.Direction = liftForce;
					velocityArrow.Length = length;
				}
			}
			if (displayAxisArrow)
			{
				float length3 = Mathf.Log(rotationAxisDirection.magnitude + 1f);
				if ((object)axisArrow == null)
				{
					axisArrow = ArrowPointer.Create(baseTransform, rotationAxisPosition, rotationAxisDirection, length3, Color.green, worldSpace: true);
					return;
				}
				axisArrow.Direction = rotationAxisDirection;
				axisArrow.Offset = rotationAxisPosition;
				axisArrow.Length = length3;
			}
		}
		else
		{
			DestroyLiftAndDragArrows();
		}
	}

	public void DestroyLiftAndDragArrows()
	{
		if ((object)liftArrow != null)
		{
			Object.Destroy(liftArrow.gameObject);
			liftArrow = null;
		}
		if ((object)dragArrow != null)
		{
			Object.Destroy(dragArrow.gameObject);
			dragArrow = null;
		}
		if ((object)velocityArrow != null)
		{
			Object.Destroy(velocityArrow.gameObject);
			velocityArrow = null;
		}
		if ((object)axisArrow != null)
		{
			Object.Destroy(axisArrow.gameObject);
			axisArrow = null;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_213511", deflectionLiftCoeff);
	}

	public void OnCenterOfLiftQuery(CenterOfLiftQuery qry)
	{
		if (!base.part.ShieldedFromAirstream)
		{
			double num = ((!HighLogic.LoadedSceneIsFlight || !(FlightGlobals.ActiveVessel != null) || !(FlightGlobals.ActiveVessel.mainBody != null)) ? FlightGlobals.GetHomeBody().GetSpeedOfSound(qry.refStaticPressure, qry.refAirDensity) : FlightGlobals.ActiveVessel.mainBody.GetSpeedOfSound(qry.refStaticPressure, qry.refAirDensity));
			float mach = (float)((double)qry.refVector.magnitude / num);
			double double_ = 0.5 * qry.refAirDensity * (double)qry.refVector.sqrMagnitude;
			SetupCoefficients(qry.refVector, out var _, out var vector2, out liftDot, out var num2);
			qry.pos = baseTransform.TransformPoint(base.part.CoLOffset);
			if (num2 < 0.001f)
			{
				qry.dir = Vector3.zero;
				qry.lift = 1E-05f;
			}
			else
			{
				qry.dir = GetLiftVector(vector2, liftDot, num2, double_, mach);
				qry.lift = qry.dir.magnitude;
				qry.dir.Normalize();
			}
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterLiftingSurfaceBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterLiftingSurfaceBase item = adjuster as AdjusterLiftingSurfaceBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public Vector3 ApplyLiftForceAdjustments(Vector3 liftForce)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			liftForce = adjusterCache[i].ApplyLiftForceAdjustment(liftForce);
		}
		return liftForce;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003045;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003045 = Localizer.Format("#autoLoc_6003045");
	}
}
