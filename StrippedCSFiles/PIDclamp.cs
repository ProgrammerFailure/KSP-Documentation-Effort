using System;
using System.Runtime.CompilerServices;

[Serializable]
public class PIDclamp : IPid
{
	public string name;

	public double kp;

	public double ki;

	public double kd;

	public double clamp;

	public float tuningScalar;

	public float kpScalar;

	public float kiScalar;

	public float kdScalar;

	public float clampScalar;

	private double integral;

	private double integralLast;

	private double derivative;

	private double errorLast;

	private double output;

	public bool ignoreIntegral;

	private bool clampIntegral;

	private double integralMax;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PIDclamp(string name, float kp, float ki, float kd, float clamp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampIntegral(float limit, bool isEnabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reinitialize(string name, float kp, float ki, float kd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reinitialize(float kp, float ki, float kd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetIntegral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double Update(double error, double dt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Update(float error, float dt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static implicit operator double(PIDclamp v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PIDclamp Parse(string s)
	{
		throw null;
	}
}
