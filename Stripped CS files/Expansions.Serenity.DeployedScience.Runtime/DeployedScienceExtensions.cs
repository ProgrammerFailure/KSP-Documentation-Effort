using System.Collections.Generic;

namespace Expansions.Serenity.DeployedScience.Runtime;

public static class DeployedScienceExtensions
{
	public static DeployedSciencePart Get(this List<DeployedSciencePart> list, uint persistentId)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].PartId == persistentId)
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

	public static DeployedSciencePart GetExperiment(this List<DeployedSciencePart> list, string experimentId)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].Experiment != null && list[num].Experiment.ExperimentId == experimentId)
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

	public static List<string> GetExperiments(this List<DeployedSciencePart> list)
	{
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Experiment != null)
			{
				list2.AddUnique(list[i].Experiment.ExperimentId);
			}
		}
		return list2;
	}

	public static ModuleGroundSciencePart Get(this List<ModuleGroundSciencePart> list, uint persistentId)
	{
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].part != null && list[num].part.persistentId == persistentId)
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

	public static DeployedScienceExperiment GetExperimentByPartID(this List<DeployedSciencePart> list, uint persistentId)
	{
		new List<string>();
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num].Experiment != null && list[num].PartId == persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return list[num].Experiment;
	}
}
