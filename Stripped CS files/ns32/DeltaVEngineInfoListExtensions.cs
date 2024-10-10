using System.Collections.Generic;

namespace ns32;

public static class DeltaVEngineInfoListExtensions
{
	public static ScreenDeltaVEngineInfo Get(this List<ScreenDeltaVEngineInfo> list, DeltaVEngineInfo engineInfo)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].engineInfo == engineInfo)
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
