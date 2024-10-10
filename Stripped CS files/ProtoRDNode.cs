using System.Collections.Generic;

public class ProtoRDNode
{
	public List<ProtoRDNode> parents;

	public List<ProtoRDNode> children;

	public bool AnyParentToUnlock;

	public ProtoTechNode tech;

	public string iconRef;

	public ProtoRDNode()
	{
		parents = new List<ProtoRDNode>();
		children = new List<ProtoRDNode>();
	}

	public ProtoRDNode(ConfigNode node)
	{
		parents = new List<ProtoRDNode>();
		children = new List<ProtoRDNode>();
		if (node.HasValue("icon"))
		{
			iconRef = node.GetValue("icon");
		}
		if (node.HasValue("anyToUnlock"))
		{
			AnyParentToUnlock = bool.Parse(node.GetValue("anyToUnlock"));
		}
		string techID = "";
		if (node.HasValue("id"))
		{
			techID = node.GetValue("id");
		}
		if (ResearchAndDevelopment.Instance != null)
		{
			tech = ResearchAndDevelopment.Instance.GetTechState(techID);
		}
		if (tech == null)
		{
			tech = new ProtoTechNode(node);
		}
	}

	public void LoadLinks(ConfigNode node, List<ProtoRDNode> rdNodes)
	{
		ConfigNode[] nodes = node.GetNodes("Parent");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode configNode = nodes[i];
			if (configNode.HasValue("parentID"))
			{
				string value = configNode.GetValue("parentID");
				ProtoRDNode protoRDNode = FindNodeByID(value, rdNodes);
				if (protoRDNode != null)
				{
					parents.Add(protoRDNode);
					protoRDNode.children.Add(this);
				}
			}
		}
	}

	public ProtoRDNode FindNodeByID(string techID, List<ProtoRDNode> nodes)
	{
		int count = nodes.Count;
		int num = 0;
		ProtoRDNode protoRDNode;
		while (true)
		{
			if (num < count)
			{
				protoRDNode = nodes[num];
				if (protoRDNode.tech != null && protoRDNode.tech.techID == techID)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return protoRDNode;
	}

	public void Save(ConfigNode node)
	{
	}
}
