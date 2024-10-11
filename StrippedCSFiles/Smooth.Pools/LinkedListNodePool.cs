using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public static class LinkedListNodePool<T>
{
	private static readonly PoolWithInitializer<LinkedListNode<T>, T> _Instance;

	public static PoolWithInitializer<LinkedListNode<T>, T> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LinkedListNodePool()
	{
		throw null;
	}
}
