using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.PartListCategories;

[Serializable]
public class BulkheadProfile : PartCategory
{
	public string profileTag;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BulkheadProfile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ExclusionCriteria(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string defaultTag(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string appendValue(string input, string value, char separator)
	{
		throw null;
	}
}
