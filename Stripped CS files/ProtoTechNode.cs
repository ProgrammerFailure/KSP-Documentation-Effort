using System;
using System.Collections.Generic;
using UnityEngine;

public class ProtoTechNode
{
	public string techID;

	public RDTech.State state;

	public int scienceCost;

	public List<AvailablePart> partsPurchased;

	public ProtoTechNode()
	{
	}

	public ProtoTechNode(ConfigNode node)
	{
		if (node.HasValue("id"))
		{
			techID = node.GetValue("id");
		}
		if (node.HasValue("state"))
		{
			state = (RDTech.State)Enum.Parse(typeof(RDTech.State), node.GetValue("state"));
		}
		int num;
		if (node.HasValue("cost"))
		{
			scienceCost = int.Parse(node.GetValue("cost"));
		}
		else
		{
			Debug.LogWarning("[R&D Tech " + techID + "]: Tech Node did not contain cost data. Looking for node in TechTree.cfg. This message should not appear again for this node.");
			ConfigNode[] nodes = AssetBase.RnDTechTree.GetTreeConfigNode().GetNodes("RDNode");
			num = nodes.Length;
			ConfigNode configNode = null;
			ConfigNode configNode2 = null;
			for (int i = 0; i < num; i++)
			{
				configNode2 = nodes[i];
				if (configNode2.GetValue("id") == techID)
				{
					configNode = configNode2;
					break;
				}
			}
			if (configNode != null)
			{
				if (configNode.HasValue("cost"))
				{
					scienceCost = int.Parse(configNode.GetValue("cost"));
				}
				else
				{
					Debug.LogError("[R&D Tech " + techID + "]: Node exists in TechTree.cfg but no science cost defined! Something is very wrong here. Strongly recommend checking TechTree.cfg file integrity");
				}
			}
			else
			{
				Debug.LogWarning("[R&D Tech " + techID + "]: No such tech node defined in tech tree!");
				scienceCost = 0;
			}
		}
		partsPurchased = new List<AvailablePart>();
		string[] values = node.GetValues("part");
		num = values.Length;
		for (int j = 0; j < num; j++)
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(values[j]);
			if (partInfoByName != null)
			{
				partsPurchased.Add(partInfoByName);
			}
		}
	}

	public void UpdateFromTechNode(RDTech nodeRef)
	{
		techID = nodeRef.techID;
		state = nodeRef.state;
		partsPurchased = nodeRef.partsPurchased;
		scienceCost = nodeRef.scienceCost;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", techID);
		node.AddValue("state", state.ToString());
		node.AddValue("cost", scienceCost.ToString());
		int count = partsPurchased.Count;
		for (int i = 0; i < count; i++)
		{
			node.AddValue("part", partsPurchased[i].name);
		}
	}
}
