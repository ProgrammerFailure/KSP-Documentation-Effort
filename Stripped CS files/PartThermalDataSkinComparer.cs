using System.Collections.Generic;

public class PartThermalDataSkinComparer : IComparer<PartThermalData>
{
	public int Compare(PartThermalData a, PartThermalData b)
	{
		if (a.unifiedTemp < b.unifiedTemp)
		{
			return -1;
		}
		if (a.unifiedTemp == b.unifiedTemp)
		{
			return 0;
		}
		return 1;
	}
}
