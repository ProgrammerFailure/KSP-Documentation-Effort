using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;

public class PartTestConstraint : IConfigNode
{
	public enum ConstraintType
	{
		SPEED,
		SPEEDENV,
		ALTITUDE,
		ALTITUDEENV,
		DENSITY,
		DYNAMICPRESSURE,
		OXYGEN,
		ATMOSPHERE,
		SITUATION,
		REPEATABILITY
	}

	public enum ConstraintTest
	{
		GT,
		LT,
		EQ,
		NEQ
	}

	public enum TestRepeatability
	{
		ALWAYS,
		BODYANDSITUATION,
		ONCEPERPART
	}

	public ConstraintType type;

	public ConstraintTest test;

	public string body;

	public string prestige;

	public Contract.ContractPrestige prestigeLevel;

	public string value;

	public double valueD;

	public bool valueB;

	public uint valueI;

	public TestRepeatability valueR;

	public uint situationMask;

	public bool valid;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartTestConstraint(ConfigNode node = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SituationAvailable(Vessel.Situations sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool BodyAvailable(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PrestigeAvailable(Contract.ContractPrestige pres)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test(double input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test(int input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test(bool input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test(uint sitmask)
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
	public static bool SituationAvailable(List<PartTestConstraint> constraints, Vessel.Situations sit)
	{
		throw null;
	}
}
