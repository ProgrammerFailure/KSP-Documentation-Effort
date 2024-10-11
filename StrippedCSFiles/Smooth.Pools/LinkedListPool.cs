using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public static class LinkedListPool<T>
{
	private static readonly Pool<LinkedList<T>> _Instance;

	public static Pool<LinkedList<T>> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LinkedListPool()
	{
		throw null;
	}
}
