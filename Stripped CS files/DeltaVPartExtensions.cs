using System.Collections.Generic;

public static class DeltaVPartExtensions
{
	public static bool ContainsPart(this List<DeltaVPartInfo> list, Part part)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].part.persistentId == part.persistentId)
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

	public static DeltaVPartInfo Get(this List<DeltaVPartInfo> list, Part part)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].part.persistentId == part.persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return list[num];
	}

	public static List<Part> PartsInStage(this List<DeltaVPartInfo> list, int stage)
	{
		List<Part> list2 = new List<Part>(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].decoupleStage != stage || (list[i].decoupleStage == stage && !list[i].decoupleBeforeBurn))
			{
				list2.Add(list[i].part);
			}
		}
		return list2;
	}
}
