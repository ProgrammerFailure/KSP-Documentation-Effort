using System.Collections.Generic;

public class PartThermalDataIntComparer : IComparer<PartThermalData>
{
	public int Compare(PartThermalData a, PartThermalData b)
	{
		if (a.part.temperature < b.part.temperature)
		{
			return -1;
		}
		if (a.part.temperature == b.part.temperature)
		{
			return 0;
		}
		return 1;
	}
}
