using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalCompass : InternalModule
{
	[KSPField]
	public string indicatorName;

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float smooth;

	public Transform hand;

	public Quaternion handInitial;

	public float current;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalCompass()
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