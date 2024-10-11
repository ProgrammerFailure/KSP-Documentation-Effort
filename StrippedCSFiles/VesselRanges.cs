using System;
using System.Runtime.CompilerServices;

[Serializable]
public class VesselRanges : IConfigNode
{
	[Serializable]
	public class Situation : IConfigNode
	{
		public float load;

		public float unload;

		public float pack;

		public float unpack;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Situation(float load, float unload, float pack, float unpack)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Situation(Situation copyFrom)
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
	}

	public Situation prelaunch;

	public Situation landed;

	public Situation splashed;

	public Situation flying;

	public Situation subOrbital;

	public Situation orbit;

	public Situation escaping;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRanges(VesselRanges copyFrom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Situation GetSituationRanges(Vessel.Situations situation)
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
}
