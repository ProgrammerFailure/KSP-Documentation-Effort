using System.Runtime.CompilerServices;

public class OrbitSnapshot
{
	public double semiMajorAxis;

	public double eccentricity;

	public double inclination;

	public double argOfPeriapsis;

	public double LAN;

	public double meanAnomalyAtEpoch;

	public double epoch;

	public int ReferenceBodyIndex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitSnapshot(Orbit orbitRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitSnapshot(CelestialBody bodyRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitSnapshot(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit Load()
	{
		throw null;
	}
}
