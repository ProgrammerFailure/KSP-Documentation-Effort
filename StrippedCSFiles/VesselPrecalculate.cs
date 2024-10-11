using System.Runtime.CompilerServices;
using UnityEngine;

public class VesselPrecalculate : MonoBehaviour
{
	protected Vessel vessel;

	public bool physStatsNotDoneInUpdate;

	public bool ranFixedThisFrame;

	public bool packedInFixed;

	public double fDeltaTime;

	private double fDeltaTimeRecip;

	protected double lastUT;

	public Vector3d worldSurfacePos;

	public QuaternionD worldSurfaceRot;

	public Vector3d gAccel;

	public Vector3d gAccelTrue;

	public Vector3d coriolis;

	public Vector3d centrifugal;

	public Vector3d integrationAccel;

	public Vector3d postIntegrationVelocityCorrection;

	public Vector3d preIntegrationVelocityOffset;

	public static bool allowDriftCompensation;

	public static bool driftAlwaysPassThreshChecks;

	public static bool disableRunInUpdate;

	protected bool easing;

	protected bool easingLockOn;

	public double easingFrameIncrease;

	protected bool correctDriftThisFrame;

	public bool wasCorrectingDrift;

	protected Vector3d railsPosNext;

	protected Vector3d railsVelNext;

	protected Vector3d cpos;

	protected Vector3d cvel;

	protected Vector3d curRotFrameVel;

	protected Vector3d rotFrameNext;

	protected int framesUntilCorrect;

	internal bool easeDocking;

	internal bool firstStatsRunComplete;

	public OrbitDriver.UpdateMode lastMode;

	public bool railsSetPosRot;

	public bool updateOrbit;

	public bool calculateGravity;

	private static string cacheAutoLOC_6003101;

	public Vessel Vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual bool isEasingGravity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselPrecalculate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselPrecalculate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RunFirst()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void MainPhysics(bool doKillChecks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ApplyVelocityCorrection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GoOnRails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GoOffRails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void StartEasing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void StopEasing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CalculateGravity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetLandedPosRot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CalculatePhysicsStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onDockingComplete(GameEvents.FromToAction<Part, Part> FromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
