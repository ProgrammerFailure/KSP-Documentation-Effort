using System.Runtime.CompilerServices;
using UnityEngine;

public class OrbitDriver : MonoBehaviour
{
	public enum UpdateMode
	{
		TRACK_Phys,
		UPDATE,
		IDLE
	}

	public delegate void CelestialBodyDelegate(CelestialBody body);

	public Vector3d pos;

	public Vector3d vel;

	private bool isHyperbolic;

	public Orbit orbit;

	public bool drawOrbit;

	public bool reverse;

	public bool frameShift;

	public bool QueuedUpdate;

	public bool QueueOnce;

	public UpdateMode updateMode;

	public OrbitRendererBase Renderer;

	private bool ready;

	public Vessel vessel;

	public CelestialBody celestialBody;

	public Color orbitColor;

	public float lowerCamVsSmaRatio;

	public float upperCamVsSmaRatio;

	public Transform driverTransform;

	public CelestialBodyDelegate OnReferenceBodyChange;

	private double fdtLast;

	public double lastTrackUT;

	public UpdateMode lastMode;

	private double updateUT;

	public bool Ready
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CelestialBody referenceBody
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

	public ITargetable Targetable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitDriver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateOrbit(bool offset = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOrbitMode(UpdateMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckDominantBody(Vector3d refPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TrackRigidbody(CelestialBody refBody, double fdtOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void updateFromParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void updateFromParameters(bool setPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecalculateOrbit(CelestialBody newReferenceBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnRailsSOITransition(Orbit ownOrbit, CelestialBody to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void unlockFrameSwitch()
	{
		throw null;
	}
}
