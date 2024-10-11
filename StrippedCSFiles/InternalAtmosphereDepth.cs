using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalAtmosphereDepth : InternalModule
{
	[KSPField]
	public string indicatorName;

	[KSPField]
	public Vector3 min;

	[KSPField]
	public float minValue;

	[KSPField]
	public Vector3 max;

	[KSPField]
	public float maxValue;

	[KSPField]
	public float log;

	[KSPField]
	public float smooth;

	public Transform hand;

	public float logMin;

	public float logMax;

	private float atmosphereDepth;

	private float tgtValue;

	private Vector3 tgtPos;

	private CelestialBody body;

	private double densityRecip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalAtmosphereDepth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}
