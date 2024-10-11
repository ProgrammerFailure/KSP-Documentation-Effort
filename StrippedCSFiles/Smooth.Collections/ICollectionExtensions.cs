using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public static class ICollectionExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IC AddAll<IC, T>(this IC collection, params T[] values) where IC : ICollection<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IC AddAll<IC, T>(this IC collection, IList<T> values) where IC : ICollection<T>
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IC AddAll<IC, T>(this IC collection, IEnumerable<T> values) where IC : ICollection<T>
	{
		throw null;
	}
}
