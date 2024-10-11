using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalAxisIndicatorYaw : InternalModule
{
	[KSPField]
	public string indicatorName;

	[KSPField]
	public Vector3 min;

	[KSPField]
	public Vector3 max;

	[KSPField]
	public float smooth;

	public Transform hand;

	public Vector3 mid;

	public float current;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalAxisIndicatorYaw()
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
