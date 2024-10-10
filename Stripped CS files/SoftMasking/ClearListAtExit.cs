using System;
using System.Collections.Generic;

namespace SoftMasking;

public struct ClearListAtExit<T> : IDisposable
{
	public List<T> _list;

	public ClearListAtExit(List<T> list)
	{
		_list = list;
	}

	public void Dispose()
	{
		_list.Clear();
	}
}
