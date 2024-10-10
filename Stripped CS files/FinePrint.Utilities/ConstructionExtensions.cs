using System.Collections.Generic;

namespace FinePrint.Utilities;

public static class ConstructionExtensions
{
	public static bool ContainsPart(this List<ConstructionPart> list, string partName)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].partName == partName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
