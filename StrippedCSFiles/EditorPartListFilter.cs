using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EditorPartListFilter<T>
{
	private Func<T, bool> filterCriteria;

	private string id;

	public string criteriaFailMessage;

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
	public EditorPartListFilter(string filterID, Func<T, bool> criteria, string failMessage = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FilterList(List<T> parts, Func<T, bool> filter)
	{
		throw null;
	}
}
