using System.Collections.Generic;

public class DeltaVEngineResourcePartInfo
{
	public DeltaVPartInfo resourcePart;

	public List<int> resourceIdsUsed;

	public float fuelMassUsed;

	public DeltaVEngineResourcePartInfo(DeltaVPartInfo partInfo)
	{
		resourcePart = partInfo;
		resourceIdsUsed = new List<int>();
		fuelMassUsed = 0f;
	}
}
