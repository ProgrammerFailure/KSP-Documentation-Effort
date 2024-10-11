using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Flow;

public class MENodePathInfo : IConfigNode
{
	public MENode node;

	public List<MEPath> paths;

	private Mission mission;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodePathInfo(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodePathInfo(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPath(MEPath path)
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
