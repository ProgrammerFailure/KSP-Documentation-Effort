using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

public class VesselRestrictionOptions : Attribute
{
	public bool listedInSAP;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestrictionOptions()
	{
		throw null;
	}
}
