using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalAnimationAirSpeed : InternalModule
{
	[KSPField]
	public string animationName;

	[KSPField]
	public float airSpeedStart;

	[KSPField]
	public float airSpeedEnd;

	[KSPField]
	public bool atmospheric;

	private Animation[] animations;

	private float normTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalAnimationAirSpeed()
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
