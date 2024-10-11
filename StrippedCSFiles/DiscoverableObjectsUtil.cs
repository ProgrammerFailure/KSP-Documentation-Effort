using System;
using System.Runtime.CompilerServices;

public static class DiscoverableObjectsUtil
{
	private static int specialNameChance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DiscoverableObjectsUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateAsteroidName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static char randomLetter(bool uppercase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string randomNumber(int digits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ProtoVessel SpawnAsteroid(string asteroidName, Orbit o, uint seed, UntrackedObjectClass objClass, double lifeTime, double lifeTimeMax)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ProtoVessel SpawnComet(string cometName, Orbit o, CometDefinition cometDef, uint seed, UntrackedObjectClass objClass, double lifeTime, double lifeTimeMax, bool optimizedCollider, float fragmentDynamicPressureModifier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateCometName(Random generator = null)
	{
		throw null;
	}
}
