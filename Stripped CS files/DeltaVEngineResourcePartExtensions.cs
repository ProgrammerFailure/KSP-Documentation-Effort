using System;
using System.Collections.Generic;

public static class DeltaVEngineResourcePartExtensions
{
	public static bool ContainsPartInfo(this List<DeltaVEngineResourcePartInfo> list, DeltaVPartInfo partInfo)
	{
		if (partInfo.part != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].resourcePart.part != null && list[i].resourcePart.part.persistentId == partInfo.part.persistentId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static DeltaVEngineResourcePartInfo Get(this List<DeltaVEngineResourcePartInfo> list, DeltaVPartInfo partInfo)
	{
		if (partInfo.part != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].resourcePart.part != null && list[i].resourcePart.part.persistentId == partInfo.part.persistentId)
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public static List<DeltaVPartInfo> PartInfoList(this List<DeltaVEngineResourcePartInfo> list)
	{
		List<DeltaVPartInfo> list2 = new List<DeltaVPartInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].resourcePart);
		}
		return list2;
	}

	public static int GetHighestPartSeparationIndex(this List<DeltaVEngineResourcePartInfo> list, int stage)
	{
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].resourcePart.decoupleStage < stage)
			{
				num = Math.Max(num, list[i].resourcePart.decoupleStage);
			}
		}
		return num;
	}

	public static List<DeltaVEngineResourcePartInfo> PartsMatchingSeparationIndex(this List<DeltaVEngineResourcePartInfo> inputParts, int index)
	{
		List<DeltaVEngineResourcePartInfo> list = new List<DeltaVEngineResourcePartInfo>();
		for (int i = 0; i < inputParts.Count; i++)
		{
			if (inputParts[i].resourcePart.decoupleStage == index)
			{
				list.Add(inputParts[i]);
			}
		}
		return list;
	}
}
