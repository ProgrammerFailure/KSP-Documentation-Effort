using System.Collections.Generic;
using ns9;

namespace Expansions.Missions;

public static class MissionMappedVesselsExtension
{
	public static int IndexPartId(this List<MissionMappedVessel> list, uint id)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].partPersistentId == id)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public static int IndexMappedVesselId(this List<MissionMappedVessel> list, uint id)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].mappedVesselPersistentId == id)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public static int IndexCurrentVesselId(this List<MissionMappedVessel> list, uint id)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].currentVesselPersistentId == id)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public static int MappedVesselName(this List<MissionMappedVessel> list, string vesselName)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (Localizer.Format(list[num].partVesselName) == vesselName)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public static uint ConvertMappedId(this List<MissionMappedVessel> list, uint id)
	{
		uint result = id;
		int num = list.IndexPartId(id);
		if (num != -1)
		{
			result = list[num].currentVesselPersistentId;
		}
		else
		{
			int num2 = list.IndexMappedVesselId(id);
			if (num2 != -1)
			{
				result = list[num2].currentVesselPersistentId;
			}
		}
		return result;
	}

	public static bool Exists(this List<MissionMappedVessel> list, uint vslId, uint partId, string vesselName, string craftName)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if ((list[num].currentVesselPersistentId == vslId || list[num].originalVesselPersistentId == vslId) && list[num].partPersistentId == partId && list[num].partVesselName == vesselName && list[num].craftFileName == craftName)
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

	public static MissionMappedVessel Find(this List<MissionMappedVessel> list, uint vslId, uint partId, string vesselName, string craftName)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if ((list[num].currentVesselPersistentId == vslId || list[num].originalVesselPersistentId == vslId) && list[num].partPersistentId == partId && list[num].partVesselName == vesselName && list[num].craftFileName == craftName)
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
