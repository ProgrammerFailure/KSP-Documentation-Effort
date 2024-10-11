using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class MEBasicNodeListFilter<T>
{
	private Func<T, bool> filterCriteria;

	private string id;

	public Func<T, bool> FilterCriteria
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEBasicNodeListFilter(string filterID, Func<T, bool> criteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FilterList(List<T> nodes, Func<T, bool> filter)
	{
		throw null;
	}
}
