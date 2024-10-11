using System.Runtime.CompilerServices;
using UnityEngine;

public class SunFlare : MonoBehaviour
{
	public static SunFlare Instance;

	public Transform target;

	public CelestialBody sun;

	public LensFlare sunFlare;

	public double AU;

	public Vector3d sunDirection;

	public AnimationCurve brightnessCurve;

	public float brightnessMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SunFlare()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SunlightEnabled(bool state)
	{
		throw null;
	}
}
