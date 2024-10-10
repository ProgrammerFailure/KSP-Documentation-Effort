using System.Collections.Generic;
using Expansions.Missions.Flow;

namespace Expansions.Missions;

public class MissionFlow : IConfigNode
{
	public Dictionary<MENode, MENodePathInfo> NodePaths;

	public Dictionary<MENode, MENodePathInfo> NodeReversePaths;

	public MEFlowBlock missionBlock;

	public Mission mission;

	public MissionFlow(Mission mission)
	{
		this.mission = mission;
		NodePaths = new Dictionary<MENode, MENodePathInfo>();
		NodeReversePaths = new Dictionary<MENode, MENodePathInfo>();
		missionBlock = new MEFlowBlock(mission);
	}

	public void Load(ConfigNode node)
	{
		NodePaths = new Dictionary<MENode, MENodePathInfo>();
		NodeReversePaths = new Dictionary<MENode, MENodePathInfo>();
		missionBlock = new MEFlowThenBlock(mission);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("BLOCKS", ref node2))
		{
			missionBlock.Load(node2);
		}
	}

	public void Save(ConfigNode node)
	{
		ConfigNode node2 = node.AddNode("BLOCKS");
		missionBlock.Save(node2);
	}
}
