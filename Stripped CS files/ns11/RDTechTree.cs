using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns11;

public class RDTechTree : MonoBehaviour
{
	public RDController controller;

	public static EventData<RDTechTree> OnTechTreeSpawn = new EventData<RDTechTree>("OnTechTreeSpawn");

	public static EventData<RDTechTree> OnTechTreeDespawn = new EventData<RDTechTree>("OnTechTreeDespawn");

	public static string backupTechTreeUrl = "GameData/Squad/Resources/TechTree.cfg";

	public static List<ProtoRDNode> rdNodes;

	public static ProtoTechNode[] TechTreeTechs;

	public static ProtoRDNode[] TechTreeNodes;

	public static ConfigNode treeNode;

	public void Awake()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			StartCoroutine(CallbackUtil.DelayedCallback(1, SpawnTechTreeNodes));
		}
	}

	public ProtoTechNode[] GetTreeTechs()
	{
		PreLoad();
		return TechTreeTechs;
	}

	public ProtoRDNode[] GetTreeNodes()
	{
		PreLoad();
		return TechTreeNodes;
	}

	public ConfigNode GetTreeConfigNode()
	{
		PreLoad();
		return treeNode;
	}

	public GameObject GetRDScreenPrefab()
	{
		return base.gameObject;
	}

	public void PreLoad()
	{
		if (rdNodes == null)
		{
			ReLoad();
		}
	}

	public void ReLoad()
	{
		ReLoad(HighLogic.CurrentGame.Parameters.Career.TechTreeUrl, loadFromDatabase: true);
	}

	public static void ReLoad(string filePath, bool loadFromDatabase)
	{
		if (rdNodes == null)
		{
			rdNodes = new List<ProtoRDNode>();
		}
		else
		{
			rdNodes.Clear();
		}
		if (string.IsNullOrEmpty(filePath))
		{
			filePath = HighLogic.CurrentGame.Parameters.Career.TechTreeUrl;
		}
		LoadTechTree(filePath, rdNodes, loadFromDatabase: true);
		TechTreeTechs = new ProtoTechNode[rdNodes.Count];
		TechTreeNodes = new ProtoRDNode[rdNodes.Count];
		int count = rdNodes.Count;
		while (count-- > 0)
		{
			TechTreeTechs[count] = rdNodes[count].tech;
			TechTreeNodes[count] = rdNodes[count];
		}
	}

	public void SpawnTechTreeNodes()
	{
		if (controller.nodes == null)
		{
			controller.nodes = new List<RDNode>();
		}
		else if (controller.nodes.Count > 0)
		{
			WipeTechTree(controller.nodes);
			LoadTechTree(HighLogic.CurrentGame.Parameters.Career.TechTreeUrl, rdNodes, loadFromDatabase: true);
			TechTreeTechs = new ProtoTechNode[rdNodes.Count];
			TechTreeNodes = new ProtoRDNode[rdNodes.Count];
			int count = rdNodes.Count;
			while (count-- > 0)
			{
				TechTreeTechs[count] = rdNodes[count].tech;
				TechTreeNodes[count] = rdNodes[count];
			}
		}
		LoadTechTree(HighLogic.CurrentGame.Parameters.Career.TechTreeUrl, controller.nodes, loadFromDatabase: true);
		OnTechTreeSpawn.Fire(this);
	}

	public void RefreshUI()
	{
		if (!(controller == null))
		{
			int count = controller.nodes.Count;
			for (int i = 0; i < count; i++)
			{
				controller.nodes[i].UpdateGraphics();
			}
			if (controller.node_selected != null)
			{
				controller.partList.SetupParts(controller.node_selected);
				controller.UpdatePanel();
				controller.node_selected.Warmup(controller.node_selected.tech);
				controller.node_selected.controller.ShowNodePanel(controller.node_selected);
			}
			if (RDTechTreeSearchBar.Instance != null)
			{
				RDTechTreeSearchBar.Instance.SelectPartIcons();
			}
		}
	}

	public void SaveTechTree(List<RDNode> nodes, string filePath)
	{
		string fileFullName = KSPUtil.ApplicationRootPath + filePath;
		ConfigNode configNode = new ConfigNode();
		ConfigNode configNode2 = configNode.AddNode("TechTree");
		int count = nodes.Count;
		for (int i = 0; i < count; i++)
		{
			RDNode rDNode = nodes[i];
			ConfigNode node = configNode2.AddNode("RDNode");
			rDNode.tech.Save(node);
			rDNode.Save(node);
		}
		configNode.Save(fileFullName);
	}

	public void WipeTechTree(List<RDNode> nodes)
	{
		int count = nodes.Count;
		while (count-- > 0)
		{
			UnityEngine.Object.Destroy(nodes[count].gameObject);
		}
		nodes.Clear();
		OnTechTreeDespawn.Fire(this);
	}

	public void LoadTechTree(string filePath, List<RDNode> rdNodes)
	{
		LoadTechTree(filePath, rdNodes, loadFromDatabase: false);
	}

	public void LoadTechTree(string filePath, List<RDNode> rdNodes, bool loadFromDatabase)
	{
		bool flag = false;
		if (loadFromDatabase)
		{
			treeNode = new ConfigNode();
			flag = GetTechNodesFromDB(out treeNode);
		}
		if (!loadFromDatabase || !flag)
		{
			ConfigNode configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + filePath);
			if (configNode == null || !configNode.HasNode("TechTree"))
			{
				Debug.LogError("[Tech Tree]: file does not exist or has bad nodes. Loading from backup path " + backupTechTreeUrl);
				configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + backupTechTreeUrl);
				if (configNode == null || !configNode.HasNode("TechTree"))
				{
					Debug.Log("[Tech Tree]: backup still not found or lacks nodes!");
					return;
				}
			}
			if (configNode.HasNode("TechTree"))
			{
				treeNode = configNode.GetNode("TechTree");
			}
		}
		ConfigNode[] nodes = treeNode.GetNodes("RDNode");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode node = nodes[i];
			RDTech rDTech = new GameObject("newNode").AddComponent<RDTech>();
			rDTech.gameObject.layer = 5;
			rDTech.Load(node);
			rDTech.Warmup();
			RDNode rDNode = rDTech.gameObject.AddComponent<RDNode>();
			rDNode.controller = controller;
			rDNode.Load(node);
			rdNodes.Add(rDNode);
		}
		for (int j = 0; j < num; j++)
		{
			rdNodes[j].LoadLinks(nodes[j], rdNodes);
		}
	}

	public void LoadTechTree(string filePath, List<ProtoRDNode> rdNodes)
	{
		LoadTechTree(filePath, rdNodes, loadFromDatabase: false);
	}

	public static void LoadTechTree(string filePath, List<ProtoRDNode> rdNodes, bool loadFromDatabase)
	{
		bool flag = false;
		if (loadFromDatabase)
		{
			treeNode = new ConfigNode();
			flag = GetTechNodesFromDB(out treeNode);
		}
		if (!loadFromDatabase || !flag)
		{
			ConfigNode configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + filePath);
			if (configNode == null || !configNode.HasNode("TechTree"))
			{
				Debug.LogError("[Tech Tree]: file does not exist or has bad nodes. Loading from backup path " + backupTechTreeUrl);
				configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + backupTechTreeUrl);
				if (configNode == null || !configNode.HasNode("TechTree"))
				{
					Debug.Log("[Tech Tree]: backup still not found or lacks nodes!");
					return;
				}
			}
			if (configNode.HasNode("TechTree"))
			{
				treeNode = configNode.GetNode("TechTree");
			}
		}
		ConfigNode[] nodes = treeNode.GetNodes("RDNode");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ProtoRDNode item = new ProtoRDNode(nodes[i]);
			rdNodes.Add(item);
		}
		for (int j = 0; j < num; j++)
		{
			rdNodes[j].LoadLinks(nodes[j], rdNodes);
		}
	}

	public static void LoadTechTitles(string filePath, Dictionary<string, string> dict)
	{
		LoadTechTitles(filePath, dict, loadFromDatabase: false);
	}

	public static void LoadTechTitles(string filePath, Dictionary<string, string> dict, bool loadFromDatabase)
	{
		bool flag = false;
		if (loadFromDatabase)
		{
			treeNode = new ConfigNode();
			flag = GetTechNodesFromDB(out treeNode);
		}
		if (!loadFromDatabase || !flag)
		{
			ConfigNode configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + filePath);
			if (configNode == null || !configNode.HasNode("TechTree"))
			{
				Debug.LogError("[Tech Tree]: file does not exist or has bad nodes. Loading from backup path " + backupTechTreeUrl);
				configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + backupTechTreeUrl);
				if (configNode == null || !configNode.HasNode("TechTree"))
				{
					Debug.Log("[Tech Tree]: backup still not found or lacks nodes!");
					return;
				}
			}
			if (configNode.HasNode("TechTree"))
			{
				treeNode = configNode.GetNode("TechTree");
			}
		}
		ConfigNode[] nodes = treeNode.GetNodes("RDNode");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode configNode2 = nodes[i];
			if (configNode2.HasValue("id") && configNode2.HasValue("title"))
			{
				dict[configNode2.GetValue("id")] = configNode2.GetValue("title");
			}
		}
	}

	public static bool GetTechNodesFromDB(out ConfigNode treeNode)
	{
		bool result = false;
		treeNode = new ConfigNode();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("TechTree");
		if (configNodes.Length != 0)
		{
			result = true;
			treeNode = new ConfigNode();
			int num = configNodes.Length;
			while (num-- > 0)
			{
				configNodes[num].CopyTo(treeNode);
			}
		}
		return result;
	}

	public List<ProtoTechNode> GetCheapestUnavailableNodes(int tolerance)
	{
		PreLoad();
		int num = int.MaxValue;
		int num2 = TechTreeTechs.Length;
		int num3 = num2;
		while (num3-- > 0)
		{
			ProtoTechNode protoTechNode = TechTreeTechs[num3];
			if (protoTechNode != null && ResearchAndDevelopment.GetTechnologyState(protoTechNode.techID) == RDTech.State.Unavailable && num > protoTechNode.scienceCost)
			{
				num = protoTechNode.scienceCost;
			}
		}
		List<ProtoTechNode> list = new List<ProtoTechNode>();
		for (int i = 0; i < num2; i++)
		{
			ProtoTechNode protoTechNode = TechTreeTechs[i];
			if (protoTechNode.scienceCost - tolerance <= num)
			{
				list.Add(protoTechNode);
			}
		}
		return list;
	}

	public List<ProtoTechNode> GetNextUnavailableNodes()
	{
		PreLoad();
		List<ProtoTechNode> list = new List<ProtoTechNode>();
		ProtoRDNode node = null;
		int num = TechTreeNodes.Length;
		for (int i = 0; i < num; i++)
		{
			ProtoRDNode protoRDNode = TechTreeNodes[i];
			if (protoRDNode.parents.Count <= 0)
			{
				node = protoRDNode;
				break;
			}
		}
		recurseForNextTechs(node, list);
		return list;
	}

	public void recurseForNextTechs(ProtoRDNode node, List<ProtoTechNode> techs)
	{
		if (ResearchAndDevelopment.GetTechnologyState(node.tech.techID) == RDTech.State.Available)
		{
			for (int i = 0; i < node.children.Count; i++)
			{
				ProtoRDNode node2 = node.children[i];
				recurseForNextTechs(node2, techs);
			}
		}
		else
		{
			techs.Add(node.tech);
		}
	}

	public ProtoTechNode FindTech(string requiredTechId)
	{
		PreLoad();
		ProtoTechNode protoTechNode = Array.Find(TechTreeTechs, (ProtoTechNode t) => t.techID == requiredTechId);
		if (protoTechNode != null)
		{
			return protoTechNode;
		}
		if (!string.IsNullOrEmpty(requiredTechId))
		{
			Debug.LogWarning("[RDTechTree]: No tech node found called " + requiredTechId);
		}
		return null;
	}
}
