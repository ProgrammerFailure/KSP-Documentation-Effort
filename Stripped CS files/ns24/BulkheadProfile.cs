using System;

namespace ns24;

[Serializable]
public class BulkheadProfile : PartCategory
{
	public string profileTag;

	public override bool ExclusionCriteria(AvailablePart aP)
	{
		if (aP.partPrefab.srfAttachNode == null && aP.partPrefab.attachNodes.Count == 0)
		{
			return false;
		}
		if (string.IsNullOrEmpty(aP.bulkheadProfiles))
		{
			return false;
		}
		string[] array = aP.bulkheadProfiles.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
		int num = 0;
		while (true)
		{
			if (num < array.Length)
			{
				if (string.IsNullOrEmpty(array[num]))
				{
					array[num] = defaultTag(aP);
				}
				if (!(profileTag == "srf"))
				{
					if (array[num].Equals(profileTag))
					{
						break;
					}
					num++;
					continue;
				}
				return array[num] == "srf";
			}
			return false;
		}
		return true;
	}

	public string defaultTag(AvailablePart aP)
	{
		string text = "";
		int count = aP.partPrefab.attachNodes.Count;
		for (int i = 0; i < count; i++)
		{
			text = appendValue(text, "size" + aP.partPrefab.attachNodes[i].size, ',');
		}
		if (aP.partPrefab.srfAttachNode != null)
		{
			text = appendValue(text, "srf", ',');
		}
		return text;
	}

	public string appendValue(string input, string value, char separator)
	{
		if (string.IsNullOrEmpty(input))
		{
			return value;
		}
		if (!input.Contains(value))
		{
			return input + separator + " " + value;
		}
		return input;
	}
}
