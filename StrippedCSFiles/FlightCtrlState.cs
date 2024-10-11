using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightCtrlState : IConfigNode
{
	public float mainThrottle;

	public float roll;

	public float yaw;

	public float pitch;

	public float rollTrim;

	public float yawTrim;

	public float pitchTrim;

	public float wheelSteer;

	public float wheelSteerTrim;

	public float wheelThrottle;

	public float wheelThrottleTrim;

	public float X;

	public float Y;

	public float Z;

	public bool killRot;

	public bool gearUp;

	public bool gearDown;

	public bool headlight;

	public float[] custom_axes;

	public bool isIdle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isNeutral
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightCtrlState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightCtrlState(float[] custom_axes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyFrom(FlightCtrlState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MaskedCopyFrom(FlightCtrlState st, KSPAxisGroup mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Neutralize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NeutralizeStick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetTrim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NeutralizeAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPYR()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetXYZ()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetWheels()
	{
		throw null;
	}
}
