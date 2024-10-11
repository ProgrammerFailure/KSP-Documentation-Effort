using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalLeverThrottle : InternalModule
{
	[KSPField]
	public string leverName;

	[KSPField]
	public float angleMin;

	[KSPField]
	public float angleMax;

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float speed;

	public Collider leverObject;

	public Quaternion leverInitial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalLeverThrottle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Lever_OnDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}
