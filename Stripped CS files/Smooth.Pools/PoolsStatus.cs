using System;
using System.Collections.Generic;

namespace Smooth.Pools;

public class PoolsStatus
{
	public static readonly Dictionary<Type, PoolsStatus> poolsInfo = new Dictionary<Type, PoolsStatus>();

	public int maxSize;

	public int allocated;
}
