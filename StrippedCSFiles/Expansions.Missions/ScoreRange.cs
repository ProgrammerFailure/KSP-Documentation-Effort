using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class ScoreRange : IConfigNode
{
	public double minRange;

	public double maxRange;

	public float score;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool isValueInRange(double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
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
