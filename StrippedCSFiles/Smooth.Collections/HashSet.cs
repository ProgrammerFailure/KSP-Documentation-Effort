using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public static class HashSet
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static HashSet<T> Create<T>()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static HashSet<T> Create<T>(IEnumerable<T> collection)
	{
		throw null;
	}
}
