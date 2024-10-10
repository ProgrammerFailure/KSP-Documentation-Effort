using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Flow;

public class MEFlowBlock : IMEFlowBlock, IConfigNode
{
	public Mission mission;

	public List<IMEFlowBlock> Blocks { get; set; }

	public int CountObjectives
	{
		get
		{
			int num = 0;
			for (int i = 0; i < Blocks.Count; i++)
			{
				if (Blocks[i] is MENode)
				{
					if (((MENode)Blocks[i]).isObjective)
					{
						num++;
					}
				}
				else if ((Blocks[i] is MEFlowOrBlock || Blocks[i] is MEFlowThenBlock) && ((MEFlowBlock)Blocks[i]).HasObjectives)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool HasObjectives => CountObjectives > 0;

	public bool HasReachableObjectives
	{
		get
		{
			int num = 0;
			while (true)
			{
				if (num < Blocks.Count)
				{
					if (Blocks[num] is MENode)
					{
						if (((MENode)Blocks[num]).IsReachable)
						{
							return true;
						}
					}
					else if ((Blocks[num] is MEFlowOrBlock || Blocks[num] is MEFlowThenBlock) && ((MEFlowBlock)Blocks[num]).HasReachableObjectives)
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
	}

	public bool HasVisibleChildren
	{
		get
		{
			int num = 0;
			while (true)
			{
				if (num < Blocks.Count)
				{
					if (Blocks[num] is MENode)
					{
						if (!((MENode)Blocks[num]).IsHiddenByEvent)
						{
							return true;
						}
					}
					else if ((Blocks[num] is MEFlowOrBlock || Blocks[num] is MEFlowThenBlock) && ((MEFlowBlock)Blocks[num]).HasVisibleChildren)
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
	}

	public MEFlowBlock(Mission mission)
	{
		this.mission = mission;
		Blocks = new List<IMEFlowBlock>();
	}

	public void UpdateMissionFlowUI(MEFlowParser parser)
	{
		Debug.LogError("[MEFlowBlock]: This method should be hidden by new declaration on child types");
	}

	public void AddBlock(IMEFlowBlock newBlock)
	{
		Blocks.Add(newBlock);
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.nodes.Count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (configNode.name == "NODE")
			{
				Guid value = Guid.Empty;
				if (configNode.TryGetValue("id", ref value))
				{
					if (mission.nodes.ContainsKey(value))
					{
						Blocks.Add(mission.nodes[value]);
						continue;
					}
					Debug.LogErrorFormat("[MEFlowBlock] Node ID loaded that doesnt exist in the mission. The flow will be invalid. ID={0}", value.ToString());
				}
			}
			else if (configNode.name == "THEN")
			{
				MEFlowThenBlock mEFlowThenBlock = new MEFlowThenBlock(mission);
				mEFlowThenBlock.Load(configNode);
				Blocks.Add(mEFlowThenBlock);
			}
			else if (configNode.name == "OR")
			{
				MEFlowOrBlock mEFlowOrBlock = new MEFlowOrBlock(mission);
				mEFlowOrBlock.Load(configNode);
				Blocks.Add(mEFlowOrBlock);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < Blocks.Count; i++)
		{
			if (Blocks[i] is MENode)
			{
				node.AddNode("NODE").AddValue("id", ((MENode)Blocks[i]).id.ToString());
			}
			else if (Blocks[i] is MEFlowThenBlock)
			{
				ConfigNode node2 = node.AddNode("THEN");
				((MEFlowThenBlock)Blocks[i]).Save(node2);
			}
			else if (Blocks[i] is MEFlowOrBlock)
			{
				ConfigNode node3 = node.AddNode("OR");
				((MEFlowOrBlock)Blocks[i]).Save(node3);
			}
		}
	}
}
