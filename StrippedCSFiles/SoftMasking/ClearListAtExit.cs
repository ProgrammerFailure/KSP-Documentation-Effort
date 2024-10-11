using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SoftMasking;

internal struct ClearListAtExit<T> : IDisposable
{
	private List<T> _list;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ClearListAtExit(List<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
