using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Pools;

public static class ListPool<T>
{
	private static readonly Pool<List<T>> _Instance;

	public static Pool<List<T>> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ListPool()
	{
		throw null;
	}
}
