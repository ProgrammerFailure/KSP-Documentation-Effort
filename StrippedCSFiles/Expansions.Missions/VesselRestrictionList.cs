using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class VesselRestrictionList : DynamicModuleList
{
	private static List<Type> vesselRestrictionTypes;

	public List<VesselRestriction> ActiveRestrictions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestrictionList(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselRestrictionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<Type> GetSupportedTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartAppEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearAppEvents()
	{
		throw null;
	}
}
