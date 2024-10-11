using System.Runtime.CompilerServices;
using UnityEngine;

public class GAPCelestialBodyCamera : MonoBehaviour
{
	public Camera cam;

	public Camera camMapFX;

	public Transform pqsTarget;

	public Material GAPCelestialBodyMaterial;

	public AnimationCurve curveZoomLocal;

	public AnimationCurve curveZoomScaled;

	public AnimationCurve curveDragLocal;

	public AnimationCurve curveDragScaled;

	private double rotationSpeed;

	private double smooth;

	private double distanceMin;

	private double distanceMax;

	private bool customBoundries;

	private double x;

	private double y;

	private double distanceCurrent;

	private double distancePercentage;

	private QuaternionD smoothedQ;

	private Vector3d smoothPos;

	private Transform focusTarget;

	private Vector3 focusPoint;

	private Vector3 newFocusPoint;

	private CelestialBody celestialBody;

	private CelestialBody sunCelestialBody;

	private bool isActive;

	private bool lockDragging;

	private bool pqsActive;

	private double minMultiplierLocal;

	private double maxMultiplierLocal;

	private double minMultiplierScaled;

	private double maxMultiplierScaled;

	private double heightOffset;

	private float zoomValue;

	private AnimationCurve curveDrag;

	private AnimationCurve curveZoom;

	public bool LockDragging
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

	public double ZoomValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double DistanceToSurface
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform FocusTarget
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
	public GAPCelestialBodyCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Enable(CelestialBody newCelestialBody, bool usePQS)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Disable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateMouse(float xAxis, float yAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTransform(double surfaceHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Camera GetLocalSpaceCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBoundries(double minMultiplier, double maxMultiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OverridePosition(double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPosition(double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLightCycle(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Quaternion GetLightRot(double UT, CelestialBody userCB, CelestialBody referenceCB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OverrideDistance(double percentage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}
}
