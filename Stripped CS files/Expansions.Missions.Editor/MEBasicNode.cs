using System;
using System.Collections.Generic;
using System.Text;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEBasicNode : IConfigNode
{
	public class ExtendedInfo
	{
		public string name = "";

		public string information = "";

		public List<ExtendedInfo> scoringInformation;
	}

	public string name = "Unknown";

	public string displayName = "#autoLOC_260145";

	public string description = "#autoLOC_260145";

	public string tooltipDescription = "#autoLOC_260145";

	public string categoryShortDescription = "Unknown";

	public bool isObjective;

	public bool isLogicNode;

	public Color headerColor;

	public string category = "#autoLOC_8100159";

	public string categoryDisplayName = "#autoLOC_260145";

	public string iconURL;

	public List<string> tests;

	public List<string> actions;

	public List<DisplayParameter> defaultSAPParameters;

	public List<DisplayParameter> defaultNodeBodyParameters;

	public List<ExtendedInfo> extInfoActionModules;

	public List<ExtendedInfo> extInfoTestModules;

	public MEBasicNode()
	{
		name = Localizer.Format("#autoLOC_8100159");
		description = Localizer.Format("#autoLOC_8200061");
		headerColor = Color.gray;
		tests = new List<string>();
		actions = new List<string>();
		defaultSAPParameters = new List<DisplayParameter>();
		defaultNodeBodyParameters = new List<DisplayParameter>();
		extInfoTestModules = new List<ExtendedInfo>();
		extInfoActionModules = new List<ExtendedInfo>();
	}

	public void Initialize()
	{
		MENode mENode = MENode.Spawn(null, this);
		MEGUINode mEGUINode = UnityEngine.Object.Instantiate(MissionEditorLogic.Instance.MENodePrefab);
		mEGUINode.SetNode(mENode);
		mEGUINode.SetupDockingData();
		categoryShortDescription = "<color=" + XKCDColors.HexFormat.LightCyan + ">" + Localizer.Format("#autoLOC_8004092", categoryDisplayName);
		if (!mEGUINode.Node.IsLogicNode && !mEGUINode.Node.IsLaunchPadNode && !mEGUINode.Node.IsVesselNode)
		{
			categoryShortDescription += Localizer.Format("#autoLOC_8004097");
		}
		else
		{
			if (mEGUINode.Node.IsLogicNode)
			{
				categoryShortDescription += Localizer.Format("#autoLOC_8004093");
			}
			if (mEGUINode.Node.IsVesselNode || mEGUINode.Node.IsLaunchPadNode)
			{
				categoryShortDescription += Localizer.Format("#autoLOC_8004095");
			}
		}
		categoryShortDescription += "</color>";
		List<TestGroup> testGroups = mENode.testGroups;
		for (int i = 0; i < testGroups.Count; i++)
		{
			for (int j = 0; j < testGroups[i].testModules.Count; j++)
			{
				extInfoTestModules.Add(BuildExtendedInfoListFromObject(testGroups[i].testModules[j], isObjective));
			}
		}
		List<IActionModule> actionModules = mENode.actionModules;
		for (int k = 0; k < actionModules.Count; k++)
		{
			extInfoActionModules.Add(BuildExtendedInfoListFromObject(actionModules[k], supportsGlobalScoringModules: false));
		}
		mENode.gameObject.DestroyGameObject();
		mEGUINode.gameObject.DestroyGameObject();
	}

	public ExtendedInfo BuildExtendedInfoListFromObject(IMENodeDisplay module, bool supportsGlobalScoringModules)
	{
		ExtendedInfo extendedInfo = new ExtendedInfo();
		extendedInfo.name = module.GetDisplayName();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(module.GetInfo()).Append("\n\n");
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(module);
		if (baseAPFieldList.Count > 0)
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_8004096"));
		}
		int i = 0;
		for (int count = baseAPFieldList.Count; i < count; i++)
		{
			stringBuilder.Append("-").Append(Localizer.Format(baseAPFieldList[i].guiName)).Append("\n");
		}
		extendedInfo.information = stringBuilder.ToString().Trim();
		extendedInfo.scoringInformation = new List<ExtendedInfo>();
		object[] customAttributes = module.GetType().GetCustomAttributes(inherit: false);
		for (int j = 0; j < customAttributes.Length; j++)
		{
			if (customAttributes[j] is MEScoreModule mEScoreModule && new List<Type>(mEScoreModule.AllowedSystems).Contains(typeof(ScoreModule_Accuracy)))
			{
				ExtendedInfo extendedInfo2 = new ExtendedInfo();
				extendedInfo2.name = "#autoLOC_8001028";
				extendedInfo2.information = "#autoLOC_8001029";
				extendedInfo.scoringInformation.Add(extendedInfo2);
			}
		}
		return extendedInfo;
	}

	public void Save(ConfigNode node)
	{
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("name", ref name);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("description", ref description);
		node.TryGetValue("tooltipDescription", ref tooltipDescription);
		node.TryGetValue("category", ref category);
		node.TryGetValue("categoryDisplayName", ref categoryDisplayName);
		node.TryGetValue("iconURL", ref iconURL);
		node.TryGetValue("isLogicNode", ref isLogicNode);
		if (!node.TryGetValue("isObjective", ref isObjective))
		{
			if (isLogicNode)
			{
				isObjective = false;
			}
			else
			{
				isObjective = true;
			}
		}
		string value = "";
		if (node.TryGetValue("headerColor", ref value))
		{
			ColorUtility.TryParseHtmlString(value, out headerColor);
		}
		else
		{
			headerColor = MissionEditorLogic.GetCategoryColor(category);
		}
		tests = new List<string>(node.GetValues("tests"));
		actions = new List<string>(node.GetValues("actions"));
		if (node.HasNode("SAP_PARAMETERS"))
		{
			ConfigNode node2 = new ConfigNode();
			if (node.TryGetNode("SAP_PARAMETERS", ref node2))
			{
				ConfigNode[] nodes = node2.GetNodes("MODULE_PARAMETER");
				for (int i = 0; i < nodes.Length; i++)
				{
					DisplayParameter item = default(DisplayParameter);
					item.Load(nodes[i]);
					defaultSAPParameters.AddUnique(item);
				}
			}
		}
		if (!node.HasNode("NODEBODY_PARAMETERS"))
		{
			return;
		}
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("NODEBODY_PARAMETERS", ref node3))
		{
			ConfigNode[] nodes2 = node3.GetNodes("MODULE_PARAMETER");
			for (int j = 0; j < nodes2.Length; j++)
			{
				DisplayParameter item2 = default(DisplayParameter);
				item2.Load(nodes2[j]);
				defaultNodeBodyParameters.AddUnique(item2);
			}
		}
	}
}
