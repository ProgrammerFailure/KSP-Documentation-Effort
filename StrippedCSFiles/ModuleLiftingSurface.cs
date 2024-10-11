using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleLiftingSurface : PartModule, ILiftProvider
{
	public enum TransformDir
	{
		X,
		Y,
		Z
	}

	[KSPField]
	public TransformDir transformDir;

	[KSPField]
	public float transformSign;

	[KSPField]
	public string transformName;

	[KSPField]
	public float deflectionLiftCoeff;

	[KSPField]
	public bool omnidirectional;

	[KSPField]
	public bool nodeEnabled;

	[KSPField]
	public string attachNodeName;

	[KSPField]
	public bool disableBodyLift;

	[KSPField]
	public bool perpendicularOnly;

	[KSPField]
	public bool displaceVelocity;

	[KSPField]
	public Vector3 velocityOffset;

	[KSPField]
	public string liftingSurfaceCurve;

	[KSPField]
	public FloatCurve liftCurve;

	[KSPField]
	public FloatCurve liftMachCurve;

	[KSPField]
	public bool useInternalDragModel;

	[KSPField]
	public FloatCurve dragCurve;

	[KSPField]
	public FloatCurve dragMachCurve;

	public ArrowPointer liftArrow;

	public ArrowPointer dragArrow;

	public ArrowPointer velocityArrow;

	public ArrowPointer axisArrow;

	public bool displayAxisArrow;

	public Vector3 liftForce;

	public Vector3 dragForce;

	public Vector3 rotationAxisDirection;

	public Vector3 rotationAxisPosition;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001708", guiUnits = "#autoLOC_7001408")]
	public float liftScalar;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001711", guiUnits = "#autoLOC_7001408")]
	public float dragScalar;

	public float liftDot;

	protected Vector3 nVel;

	protected Vector3 liftVector;

	protected Vector3 pointVelocity;

	protected float absDot;

	protected double Qlift;

	protected double Qdrag;

	[SerializeField]
	protected Transform baseTransform;

	protected BaseField liftField;

	protected BaseField dragField;

	protected AttachNode attachNode;

	private List<AdjusterLiftingSurfaceBase> adjusterCache;

	private static string cacheAutoLOC_6003045;

	public bool DisableBodyLift
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsLifting
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleLiftingSurface()
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
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupCoefficients(Vector3 pointVelocity, out Vector3 nVel, out Vector3 liftVector, out float liftDot, out float absDot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetLiftVector(Vector3 liftVector, float liftDot, float absDot, double Q, float mach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetDragVector(Vector3 nVel, float absDot, double Q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetDragVector(Vector3 nVel, float absDot, double Q, float mach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateAeroDisplay(Color LineColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyLiftAndDragArrows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCenterOfLiftQuery(CenterOfLiftQuery qry)
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
	protected Vector3 ApplyLiftForceAdjustments(Vector3 liftForce)
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
