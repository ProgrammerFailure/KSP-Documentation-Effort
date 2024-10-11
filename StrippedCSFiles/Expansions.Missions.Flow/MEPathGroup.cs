using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Flow;

public class MEPathGroup
{
	public MENode joinNode;

	public int minHops;

	public List<MEPath> Paths
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public MEPath First
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEPathGroup(MEPath first)
	{
		throw null;
	}
}
