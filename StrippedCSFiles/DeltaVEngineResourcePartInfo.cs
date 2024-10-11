using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class DeltaVEngineResourcePartInfo
{
	public DeltaVPartInfo resourcePart;

	public List<int> resourceIdsUsed;

	public float fuelMassUsed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVEngineResourcePartInfo(DeltaVPartInfo partInfo)
	{
		throw null;
	}
}
