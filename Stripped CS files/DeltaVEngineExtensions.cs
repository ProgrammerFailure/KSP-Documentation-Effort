using System.Collections.Generic;

public static class DeltaVEngineExtensions
{
	public static bool Contains(this List<DeltaVEngineInfo> list, Part part)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].engine.part == part)
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

	public static DeltaVEngineInfo Get(this List<DeltaVEngineInfo> list, Part part)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].engine.part == part)
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
}
