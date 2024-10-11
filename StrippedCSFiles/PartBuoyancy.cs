using System.Runtime.CompilerServices;
using UnityEngine;

public class PartBuoyancy : MonoBehaviour
{
	public Vector3 centerOfBuoyancy;

	public Vector3 centerOfDisplacement;

	public Vector3d maxCoords;

	public Vector3d minCoords;

	public float maxDimension;

	public Bounds bounds;

	public Vector3[] boundCoords;

	public Vector3 boundsCenter;

	public double[] depths;

	public bool early;

	public bool allSplashed;

	public bool slow;

	public bool wasSplashed;

	public bool dead;

	public static bool overrideCubeOnChutesIfUnspecified;

	public double waterLevel;

	public CelestialBody body;

	public double depth;

	public double maxDepth;

	public double minDepth;

	public double dragScalar;

	public double splashedCounter;

	public double liftScalar;

	public double drag;

	public double submergedPortion;

	public Vector3 effectiveForce;

	public double buoyantGeeForce;

	public DragCube overrideCube;

	public double localVelocityMag;

	public Vector3 lastBuoyantForce;

	public Vector3 lastForcePosition;

	public bool physWarp;

	public double xPortion;

	public double yPortion;

	public double zPortion;

	public double xzPortion;

	private Part part;

	public bool splashed;

	public double displacement;

	private bool canForceUpdate;

	private Vector3 pLast;

	private Vector3 V;

	private Vector3 downAxis;

	private float dH;

	private bool isKerbal;

	private Rigidbody _rigidbody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartBuoyancy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartBuoyancy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindOverrideCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplacement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReportSplashDown()
	{
		throw null;
	}
}
