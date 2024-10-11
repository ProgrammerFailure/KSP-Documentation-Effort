using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public class PoolsStatus
{
	public static readonly Dictionary<Type, PoolsStatus> poolsInfo;

	public int maxSize;

	public int allocated;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PoolsStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PoolsStatus()
	{
		throw null;
	}
}
