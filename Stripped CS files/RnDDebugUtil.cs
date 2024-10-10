using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ns11;
using ns4;
using UnityEngine;

public class RnDDebugUtil
{
	public enum Page
	{
		TechTree,
		Node,
		Tech,
		Science,
		Parts,
		Files,
		Settings
	}

	public class NodeMetrics
	{
		public RDNode rdNode;

		public RDTech rdTech;

		public List<PartMetrics> partMetrics;

		public float avgPartCostVsEntryCostRatio;

		public float avgPartEntryCostVsScienceCostRatio;

		public NodeMetrics(RDNode node)
		{
			rdNode = node;
			rdTech = node.tech;
			this.partMetrics = new List<PartMetrics>();
			avgPartCostVsEntryCostRatio = 0f;
			int count = rdTech.partsAssigned.Count;
			for (int i = 0; i < count; i++)
			{
				PartMetrics partMetrics = new PartMetrics(rdTech.partsAssigned[i], rdTech);
				avgPartCostVsEntryCostRatio += partMetrics.costVsEntryCostRatio;
				avgPartEntryCostVsScienceCostRatio += partMetrics.costVsNodeScienceCostRatio;
				this.partMetrics.Add(partMetrics);
			}
			avgPartCostVsEntryCostRatio /= this.partMetrics.Count;
			avgPartEntryCostVsScienceCostRatio /= this.partMetrics.Count;
		}
	}

	public class PartMetrics
	{
		public AvailablePart part;

		public RDTech assignedNode;

		public float costVsEntryCostRatio;

		public float costVsNodeScienceCostRatio;

		public PartMetrics(AvailablePart p, RDTech techNode)
		{
			part = p;
			assignedNode = techNode;
			costVsEntryCostRatio = p.cost / (float)p.entryCost;
			if (techNode.scienceCost == 0)
			{
				costVsNodeScienceCostRatio = 0f;
			}
			else
			{
				costVsNodeScienceCostRatio = p.entryCost / techNode.scienceCost;
			}
		}
	}

	public float wWidth;

	public RDTechTree techTree;

	public RDTech selectedTech;

	public RDNode selectedNode;

	public NodeMetrics selectedNodeMetrics;

	public Vector2 scrollPos;

	public List<AvailablePart> moddedParts;

	public List<RDNode> rdNodes;

	public RDController treeController;

	public string textEditor = string.Empty;

	public string treeCfgPath = "GameData/TechTree.cfg";

	public static bool showPartsInNodeTooltips;

	public Page page;

	public string[] pages;

	public float yThreshold = 30f;

	public Vector2 scrollPosition;

	public int selGridInt;

	public string outputText = string.Empty;

	public void Init(RDTechTree tecTree, float wWidth)
	{
		techTree = tecTree;
		this.wWidth = wWidth;
		RDNode.OnNodeSelected.Add(OnNodeSelected);
		RDNode.OnNodeUnselected.Add(OnNodeUnselected);
		RDController.OnRDTreeSpawn.Add(OnTreeSpawn);
		RDController.OnRDTreeDespawn.Add(OnTreeDespawn);
		pages = Enum.GetNames(typeof(Page));
		page = Page.TechTree;
		moddedParts = new List<AvailablePart>();
		LoadSettings();
	}

	public void Terminate()
	{
		RDNode.OnNodeSelected.Remove(OnNodeSelected);
		RDNode.OnNodeUnselected.Remove(OnNodeUnselected);
		RDController.OnRDTreeSpawn.Remove(OnTreeSpawn);
		RDController.OnRDTreeDespawn.Remove(OnTreeDespawn);
	}

	public void OnTreeSpawn(RDController treeController)
	{
		this.treeController = treeController;
		rdNodes = treeController.nodes;
	}

	public void OnTreeDespawn(RDController treeController)
	{
		if (this.treeController == treeController)
		{
			rdNodes.Clear();
			this.treeController = null;
		}
	}

	public void OnNodeSelected(RDNode node)
	{
		if (selectedNode != node && node != null)
		{
			selectedTech = node.tech;
			selectedNode = node;
			selectedNodeMetrics = new NodeMetrics(node);
		}
	}

	public void OnNodeUnselected(RDNode node)
	{
		if (node == selectedNode && node != null)
		{
			selectedNodeMetrics = null;
			selectedNode = null;
			selectedTech = null;
		}
	}

	public void RefreshUI()
	{
		techTree.RefreshUI();
	}

	public void DrawWindow(int id)
	{
		Page page = (Page)GUILayout.Toolbar((int)this.page, pages);
		if (page != this.page)
		{
			switch (this.page)
			{
			}
			switch (page)
			{
			}
			this.page = page;
			scrollPos = Vector2.zero;
			GUIUtil.ClearEditableFieldFlags();
		}
		switch (this.page)
		{
		case Page.TechTree:
			DrawTreePage();
			break;
		case Page.Node:
			if (selectedNode != null)
			{
				DrawNodePage(selectedNode);
			}
			break;
		case Page.Tech:
			if (selectedTech != null)
			{
				DrawTechPage(selectedTech);
			}
			break;
		case Page.Science:
			DrawSciencePage();
			break;
		case Page.Parts:
			if (selectedTech != null)
			{
				DrawPartsPage(selectedNodeMetrics);
			}
			break;
		case Page.Files:
			DrawFilesPage(moddedParts);
			break;
		case Page.Settings:
			DrawSettingsPage();
			break;
		}
		GUILayout.FlexibleSpace();
		GUI.DragWindow();
	}

	public void DrawTreePage()
	{
		if (treeController == null)
		{
			GUILayout.Label("R&D Tree Not Spawned. Please go to the Tech Tree screen before using this tool.");
			return;
		}
		GUILayout.Label("R&D Tree Spawned. " + rdNodes.Count + " Nodes Registered");
		int count = rdNodes.Count;
		if (GUILayout.Button("Unlock All Nodes"))
		{
			for (int i = 0; i < count; i++)
			{
				rdNodes[i].tech.UnlockTech(updateGameState: false);
			}
			RefreshUI();
		}
		if (GUILayout.Button("Research All Parts"))
		{
			for (int j = 0; j < count; j++)
			{
				rdNodes[j].tech.AutoPurchaseAllParts();
			}
			RefreshUI();
		}
		if (GUILayout.Button("Rebuild Tree (Save Changes and Reload)"))
		{
			RebuildTree();
		}
		if (GUILayout.Button("Reload Parts & Rebuild Tree"))
		{
			ReloadParts();
			RebuildTree();
		}
		GUILayout.BeginHorizontal();
		if (rdNodes.Count > 0)
		{
			if (GUILayout.Button("Save Tree"))
			{
				techTree.SaveTechTree(GetSortedRDNodes(), treeCfgPath);
			}
			if (GUILayout.Button("Wipe Tree"))
			{
				techTree.WipeTechTree(rdNodes);
			}
		}
		else if (GUILayout.Button("Load Tree"))
		{
			techTree.LoadTechTree(treeCfgPath, rdNodes, loadFromDatabase: true);
			RDTechTree.OnTechTreeSpawn.Fire(techTree);
		}
		GUI.enabled = HasTextEditor();
		if (GUILayout.Button("Open Tree Cfg"))
		{
			Process.Start(textEditor, KSPUtil.ApplicationRootPath + treeCfgPath);
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();
	}

	public void ReloadParts()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			ReloadPartAssignments();
		}
		else
		{
			RDTestSceneLoader.Instance.LoadPartDefs();
		}
	}

	public void RebuildTree()
	{
		techTree.SaveTechTree(GetSortedRDNodes(), treeCfgPath);
		techTree.WipeTechTree(rdNodes);
		techTree.LoadTechTree(treeCfgPath, rdNodes, loadFromDatabase: true);
		RDTechTree.OnTechTreeSpawn.Fire(techTree);
		int count = rdNodes.Count;
		for (int i = 0; i < count; i++)
		{
			rdNodes[i].tech.UnlockTech(updateGameState: false);
		}
		RefreshUI();
	}

	public void ReloadPartAssignments()
	{
		FileInfo[] files = new DirectoryInfo(KSPUtil.ApplicationRootPath + "/GameData").GetFiles("*.cfg", SearchOption.AllDirectories);
		UnityEngine.Debug.Log(files.Length + " files found");
		int num = files.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode configNode = ConfigNode.Load(files[i].FullName);
			if (!configNode.HasNode("PART"))
			{
				continue;
			}
			ConfigNode node = configNode.GetNode("PART");
			string value = node.GetValue("name");
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(value);
			if (partInfoByName != null)
			{
				if (node.HasValue("TechRequired"))
				{
					partInfoByName.TechRequired = node.GetValue("TechRequired");
				}
				if (node.HasValue("entryCost"))
				{
					partInfoByName.SetEntryCost(int.Parse(node.GetValue("entryCost")));
				}
				if (node.HasValue("cost"))
				{
					partInfoByName.cost = float.Parse(node.GetValue("cost"));
					ShipConstruction.SanitizePartCosts(partInfoByName, node);
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("No part callled " + value + " exists in PartLoader's list.");
			}
		}
	}

	public List<RDNode> GetSortedRDNodes()
	{
		List<RDNode> list = new List<RDNode>(rdNodes);
		list.Sort(CompareRDNodes);
		return list;
	}

	public static int CompareRDNodes(RDNode a, RDNode b)
	{
		return a.tech.scienceCost.CompareTo(b.tech.scienceCost);
	}

	public void DrawNodePage(RDNode n)
	{
		GUILayout.Label("Selected Node: " + n.tech.title);
		GUIUtil.EditableStringField("Node Name", n.gameObject.name, delegate(string s)
		{
			n.gameObject.name = s;
			RefreshUI();
		});
		n.AnyParentToUnlock = GUIUtil.EditableBoolField("Any Parent To Unlock", n.AnyParentToUnlock, delegate
		{
			RefreshUI();
		});
		List<string> list = treeController.iconLoader.icons.ConvertAll((Icon a) => a.GetName());
		List<string> list2 = new List<string>();
		int count = rdNodes.Count;
		for (int i = 0; i < count; i++)
		{
			RDNode rDNode = rdNodes[i];
			if (rDNode.iconRef != RDController.Instance.iconLoader.FallbackIcon.GetName())
			{
				list2.Add(rDNode.iconRef);
			}
		}
		List<string> list3 = new List<string>();
		count = list.Count;
		for (int j = 0; j < count; j++)
		{
			string item = list[j];
			if (!list2.Contains(item))
			{
				list3.Add(item);
			}
		}
		if (list3.Count > 0)
		{
			List<Texture> list4 = new List<Texture>();
			count = treeController.iconLoader.icons.Count;
			for (int k = 0; k < count; k++)
			{
				Icon icon = treeController.iconLoader.icons[k];
				if (!list2.Contains(icon.GetName()))
				{
					list4.Add(icon.texture);
				}
			}
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, false, GUILayout.Height(95f));
			selGridInt = GUILayout.SelectionGrid(selGridInt, list4.ToArray(), list3.Count);
			selGridInt = Mathf.Clamp(selGridInt, 0, list3.Count - 1);
			GUILayout.EndScrollView();
			string text = "Unassigned nodes: ";
			string text2 = "Hidden nodes: ";
			string text3 = " | ";
			int num = 0;
			int num2 = 0;
			int count2 = rdNodes.Count;
			while (count2-- > 0)
			{
				RDNode rDNode2 = rdNodes[count2];
				if (rDNode2.iconRef == RDController.Instance.iconLoader.FallbackIcon.GetName())
				{
					text = text + text3 + rDNode2.name;
					num++;
				}
				if (rDNode2.state == RDNode.State.HIDDEN)
				{
					text2 = text2 + text3 + rDNode2.name;
					num2++;
				}
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label("Icon: " + list3[selGridInt] + " | generic: " + num + " | hidden: " + num2);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("test"))
			{
				UnityEngine.Debug.Log(text);
				UnityEngine.Debug.Log(text2);
			}
			if (GUILayout.Button("Update"))
			{
				n.SetIconState(RDController.Instance.iconLoader.GetIcon(list3[selGridInt]));
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.Label("IconRefs in total: " + list.Count + "  Nodes: " + rdNodes.Count);
		}
	}

	public void DrawTechPage(RDTech t)
	{
		GUILayout.Label("Selected Node: " + t.title);
		GUILayout.Space(15f);
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		t.techID = GUIUtil.EditableStringField("Node ID", t.techID, delegate
		{
			RefreshUI();
		});
		t.title = GUIUtil.EditableStringField("Title", t.title, delegate
		{
			RefreshUI();
		});
		t.scienceCost = GUIUtil.EditableIntField("ScienceCost", t.scienceCost, delegate
		{
			RefreshUI();
		});
		t.description = GUIUtil.EditableTextArea("Description", t.description, delegate
		{
			RefreshUI();
		}, GUILayout.Width(wWidth), GUILayout.ExpandHeight(expand: true));
		GUILayout.EndScrollView();
	}

	public void DrawSciencePage()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			if (GUILayout.Button("Check for Unassigned Parts"))
			{
				outputText = ResearchAndDevelopment.CheckForMissingParts();
			}
			if (GUILayout.Button("Part Assignment Summary"))
			{
				outputText = ResearchAndDevelopment.PartAssignmentSummary();
			}
			if (GUILayout.Button("Add up all the Science in the Universe"))
			{
				outputText = ResearchAndDevelopment.CountUniversalScience();
			}
			if (outputText != string.Empty)
			{
				GUI.enabled = false;
				GUILayout.TextArea(outputText, GUILayout.Width(wWidth), GUILayout.ExpandHeight(expand: true));
				GUI.enabled = true;
			}
		}
		else
		{
			GUILayout.Label("No R&D Module Present. This screen is only available if the R&D Module is loaded.");
		}
	}

	public void DrawPartsPage(NodeMetrics n)
	{
		GUILayout.Label("Selected Node: " + n.rdTech.title);
		GUILayout.Space(15f);
		GUILayout.Label(("Avg C / EC: " + n.avgPartCostVsEntryCostRatio.ToString("0.##") + " -- Avg EC / Sci: " + n.avgPartEntryCostVsScienceCostRatio.ToString("0.##")) ?? "");
		GUILayout.Label("Parts Assigned:");
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		int count = n.partMetrics.Count;
		for (int i = 0; i < count; i++)
		{
			PartMetrics partMetrics = n.partMetrics[i];
			GUILayout.Label(">> " + partMetrics.part.title + ": ");
			GUILayout.BeginHorizontal();
			GUILayout.Label(("C / EC: " + partMetrics.costVsEntryCostRatio.ToString("0.##") + " -- EC / Sci: " + partMetrics.costVsNodeScienceCostRatio.ToString("0.##")) ?? "");
			GUI.enabled = HasTextEditor();
			if (GUILayout.Button(GUI.enabled ? "Open .cfg file" : "No .cfg editor set"))
			{
				UnityEngine.Debug.Log("Starting Process " + textEditor + " \"" + partMetrics.part.configFileFullName + "\"");
				Process.Start(textEditor, "\"" + partMetrics.part.configFileFullName + "\"");
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void DrawFilesPage(List<AvailablePart> moddedParts)
	{
		GUILayout.Label("Modified Part Configs:");
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		int count = moddedParts.Count;
		while (count-- > 0)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(moddedParts[count].title);
			GUI.enabled = HasTextEditor();
			if (GUILayout.Button(GUI.enabled ? "Open" : "No .cfg editor set"))
			{
				UnityEngine.Debug.Log("Starting Process " + textEditor + " \"" + moddedParts[count].configFileFullName + "\"");
				Process.Start(textEditor, "\"" + moddedParts[count].configFileFullName + "\"");
			}
			GUI.enabled = true;
			if (GUILayout.Button("Save") && Event.current.type == EventType.MouseUp)
			{
				SavePart(moddedParts[count]);
				moddedParts.RemoveAt(count);
			}
			if (GUILayout.Button("Discard") && Event.current.type == EventType.MouseUp)
			{
				DiscardPart(moddedParts[count]);
				moddedParts.RemoveAt(count);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUILayout.FlexibleSpace();
		GUI.enabled = moddedParts.Count > 0;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save All") && Event.current.type == EventType.MouseUp)
		{
			int count2 = moddedParts.Count;
			while (count2-- > 0)
			{
				SavePart(moddedParts[count2]);
			}
			moddedParts.Clear();
		}
		if (GUILayout.Button("Discard All") && Event.current.type == EventType.MouseUp)
		{
			int count3 = moddedParts.Count;
			while (count3-- > 0)
			{
				DiscardPart(moddedParts[count3]);
			}
			moddedParts.Clear();
		}
		GUILayout.EndHorizontal();
		GUI.enabled = true;
	}

	public void SavePart(AvailablePart aP)
	{
		aP.partConfig.SetValue("name", aP.name, createIfNotFound: true);
		aP.partConfig.SetValue("title", aP.title, createIfNotFound: true);
		aP.partConfig.SetValue("description", (!string.IsNullOrEmpty(aP.description)) ? aP.description.Replace("\n", "\\n") : aP.description, createIfNotFound: true);
		aP.partConfig.SetValue("manufacturer", aP.manufacturer, createIfNotFound: true);
		aP.partConfig.SetValue("category", aP.category.ToString(), createIfNotFound: true);
		if (!string.IsNullOrEmpty(aP.TechRequired))
		{
			aP.partConfig.SetValue("TechRequired", aP.TechRequired, createIfNotFound: true);
		}
		aP.partConfig.SetValue("entryCost", aP.entryCost.ToString(), createIfNotFound: true);
		aP.partConfig.SetValue("cost", aP.cost.ToString(), createIfNotFound: true);
		aP.partConfig.Save(aP.configFileFullName);
	}

	public void DiscardPart(AvailablePart aP)
	{
		ConfigNode configNode = ConfigNode.Load(aP.configFileFullName);
		if (configNode.HasNode("PART"))
		{
			ConfigNode node = configNode.GetNode("PART");
			aP.name = node.GetValue("name");
			aP.title = node.GetValue("title");
			aP.description = node.GetValue("description");
			if (!string.IsNullOrEmpty(aP.description))
			{
				aP.description = aP.description.Replace("\\n", "\n");
			}
			aP.manufacturer = node.GetValue("manufacturer");
			aP.partConfig = node;
			aP.category = (PartCategories)Enum.Parse(typeof(PartCategories), node.GetValue("category"));
			if (node.HasValue("TechRequired"))
			{
				aP.TechRequired = node.GetValue("TechRequired");
			}
			if (node.HasValue("entryCost"))
			{
				aP.SetEntryCost(int.Parse(node.GetValue("entryCost")));
			}
			if (node.HasValue("cost"))
			{
				aP.cost = float.Parse(node.GetValue("cost"));
				ShipConstruction.SanitizePartCosts(aP, node);
			}
			aP.moduleInfo = string.Empty;
			ConfigNode[] nodes = node.GetNodes("MODULE");
			int num = nodes.Length;
			for (int i = 0; i < num; i++)
			{
				ConfigNode configNode2 = nodes[i];
				aP.moduleInfo = aP.moduleInfo + "<b>" + configNode2.GetValue("name") + "</b>\nWe also assume module infos take up\non average about 4 lines\ntodisplay their data.\nIt can get pretty long.\n";
				AvailablePart.ModuleInfo moduleInfo = new AvailablePart.ModuleInfo();
				moduleInfo.moduleName = KSPUtil.PrintModuleName(configNode2.GetValue("name"));
				moduleInfo.info = "Some <b>test</b> values\nGet <i>Displayed Here</i>\nin <color=orange>multiple lines</color>\nand rich-text formatted";
				aP.moduleInfos.Add(moduleInfo);
			}
			aP.resourceInfo = string.Empty;
			ConfigNode[] nodes2 = node.GetNodes("RESOURCE");
			num = nodes2.Length;
			for (int j = 0; j < num; j++)
			{
				ConfigNode configNode3 = nodes2[j];
				PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(configNode3.GetValue("name"));
				aP.resourceInfo = aP.resourceInfo + "<b>" + definition.name + "</b>\nResource Amounts,\nResource Density\nIt all adds up to one massive tooltip.\n";
				float num2 = float.Parse(configNode3.GetValue("amount"));
				float num3 = float.Parse(configNode3.GetValue("maxAmount"));
				float num4 = definition.unitCost * num2;
				AvailablePart.ResourceInfo resourceInfo = new AvailablePart.ResourceInfo();
				resourceInfo.resourceName = configNode3.GetValue("name");
				resourceInfo.displayName = (configNode3.HasValue("title") ? configNode3.GetValue("title") : KSPUtil.PrintModuleName(configNode3.GetValue("name")));
				resourceInfo.info = "Amount: " + num2.ToString("F1") + "/" + num3.ToString("F1") + "\nCost: \\F " + num4.ToString("0.00");
				aP.resourceInfos.Add(resourceInfo);
			}
		}
	}

	public void DrawSettingsPage()
	{
		GUILayout.Label("Utility Settings:");
		textEditor = GUILayout.TextField(textEditor);
		treeCfgPath = GUILayout.TextField(treeCfgPath);
		showPartsInNodeTooltips = GUILayout.Toggle(showPartsInNodeTooltips, "Show Parts in Node Tooltips");
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save Settings"))
		{
			SaveSettings();
			RefreshUI();
		}
		if (GUILayout.Button("Restore Settings"))
		{
			LoadSettings();
			RefreshUI();
		}
		GUILayout.EndHorizontal();
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetString("RnDDebugUtil.TextEditor", textEditor);
		PlayerPrefs.SetString("RnDDebugUtil.treeCfgPath", treeCfgPath);
		if (HighLogic.LoadedSceneIsGame)
		{
			HighLogic.CurrentGame.Parameters.Career.TechTreeUrl = treeCfgPath;
		}
		PlayerPrefs.SetString("RnDDebugUtil.showPartsInNodeTooltips", showPartsInNodeTooltips.ToString());
	}

	public void LoadSettings()
	{
		textEditor = PlayerPrefs.GetString("RnDDebugUtil.TextEditor", "None");
		treeCfgPath = PlayerPrefs.GetString("RnDDebugUtil.treeCfgPath", "GameData/Squad/Resources/TechTree.cfg");
		if (HighLogic.LoadedSceneIsGame)
		{
			treeCfgPath = HighLogic.CurrentGame.Parameters.Career.TechTreeUrl;
		}
		showPartsInNodeTooltips = bool.Parse(PlayerPrefs.GetString("RnDDebugUtil.showPartsInNodeTooltips", "False"));
	}

	public bool HasTextEditor()
	{
		if (!string.IsNullOrEmpty(textEditor))
		{
			return !(textEditor == "None");
		}
		return false;
	}
}
