using System.Collections.Generic;

namespace Expansions.Missions.Flow;

public static class MEFlowExtensions
{
	public static List<MENode> DistinctStartNodes(this List<MEPath> paths)
	{
		List<MENode> list = new List<MENode>();
		for (int i = 0; i < paths.Count; i++)
		{
			if (paths[i].Nodes.Count > 0 && list.IndexOf(paths[i].Nodes[0]) == -1)
			{
				list.Add(paths[i].Nodes[0]);
			}
		}
		return list;
	}

	public static int CountStartNodes(this List<MEPath> paths)
	{
		return paths.DistinctStartNodes().Count;
	}

	public static List<MEPath> GetPathsStarting(this List<MEPath> paths, MENode node)
	{
		List<MEPath> list = new List<MEPath>();
		for (int i = 0; i < paths.Count; i++)
		{
			if (paths[i].Nodes.Count > 0 && paths[i].Nodes[0] == node)
			{
				list.Add(paths[i]);
			}
		}
		return list;
	}
}
