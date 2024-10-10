using System.Collections.Generic;

namespace ns32;

public static class DeltaVStageInfoListExtensions
{
	public static ScreenDeltaVStageInfo Get(this List<ScreenDeltaVStageInfo> list, DeltaVStageInfo stageInfo)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].stageInfo == stageInfo)
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
