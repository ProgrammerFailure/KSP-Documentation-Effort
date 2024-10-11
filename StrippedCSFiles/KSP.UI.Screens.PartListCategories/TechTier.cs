using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.PartListCategories;

[Serializable]
public class TechTier : PartCategory
{
	public float minScienceCost;

	public float maxScienceCost;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TechTier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ExclusionCriteria(AvailablePart aP)
	{
		throw null;
	}
}
