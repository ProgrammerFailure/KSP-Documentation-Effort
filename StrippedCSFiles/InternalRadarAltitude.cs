using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalRadarAltitude : InternalModule
{
	[KSPField]
	public string indicatorName;

	[KSPField]
	public InternalDialIncrement increments;

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float smooth;

	public Transform hand;

	public Quaternion handInitial;

	public float current;

	public float altitude;

	public float reportedAlt;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalRadarAltitude()
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
