using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalAnimationAltimeter : InternalModule
{
	[KSPField]
	public string animationName;

	[KSPField]
	public float altitudeStart;

	[KSPField]
	public float altitudeEnd;

	private Animation[] animations;

	private float normTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalAnimationAltimeter()
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
