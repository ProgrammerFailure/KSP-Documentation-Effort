using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

[Serializable]
public class WheelFrictionNode
{
	public float stiffness;

	public float extremumSlip;

	public float extremumValue;

	public float asymptoteSlip;

	public float asymptoteValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelFrictionNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelFrictionNode From(WheelFrictionCurve fCurve)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelFrictionNode Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelFrictionCurve GetCurve()
	{
		throw null;
	}
}
