using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Flow;

public class MEFlowParser : MonoBehaviour
{
	[SerializeField]
	public Transform flowObjectsParent;

	[SerializeField]
	public TMP_Text emptyObjectivesMessage;

	public static Object parserPrefab;

	public Object textPrefab;

	public bool showNonObjectives;

	public bool showEvents;

	public Mission mission;

	public Color[] GroupColors;

	public MEFlowUINode.ButtonAction buttonAction;

	public ToggleGroup toggleGroup;

	public Callback<MEFlowUINode> toggleCallback;

	public Callback<PointerEventData> buttonCallback;

	public void Awake()
	{
		if (MEFlowUINode.nodePrefab == null)
		{
			MEFlowUINode.nodePrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowUINode.prefab");
		}
		if (MEFlowUINode.nodeTogglePrefab == null)
		{
			MEFlowUINode.nodeTogglePrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowUINodeToggle.prefab");
		}
		if (MEFlowUIGroup_Or.groupPrefab == null)
		{
			MEFlowUIGroup_Or.groupPrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowUIGroup_Or.prefab");
		}
		if (MEFlowUIGroup_Then.groupPrefab == null)
		{
			MEFlowUIGroup_Then.groupPrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowUIGroup_Then.prefab");
		}
		textPrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowUIText.prefab");
	}

	public void Start()
	{
		byte[] hashBytes = null;
		string signature = null;
		if (mission != null)
		{
			hashBytes = mission.HashBytes;
			signature = mission.Signature;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			Object.Destroy(this);
		}
	}

	public void CreateMissionFlowUI_Toggle(Mission mission, Callback<MEFlowUINode> toggleCallback, ToggleGroup toggleGroup, bool showEvents = false, bool showNonObjectives = false, bool showStartNodes = false)
	{
		buttonAction = MEFlowUINode.ButtonAction.CallbackToggle;
		this.toggleCallback = toggleCallback;
		this.toggleGroup = toggleGroup;
		CreateMissionFlowUI(mission, showEvents, showNonObjectives, showStartNodes);
	}

	public void CreateMissionFlowUI_Button(Mission mission, MEFlowUINode.ButtonAction buttonAction = MEFlowUINode.ButtonAction.None, Callback<PointerEventData> buttonCallback = null, bool showEvents = false, bool showNonObjectives = false, bool showStartNodes = false)
	{
		this.buttonAction = buttonAction;
		this.buttonCallback = buttonCallback;
		CreateMissionFlowUI(mission, showEvents, showNonObjectives, showStartNodes);
	}

	public void CreateMissionFlowUI(Mission mission, bool showEvents, bool showNonObjectives, bool showStartNodes)
	{
		this.mission = mission;
		this.showEvents = showEvents;
		this.showNonObjectives = showNonObjectives;
		ClearChildren();
		if (!mission.isInitialized)
		{
			mission.InitMission();
		}
		ParseMission(mission);
		if (mission.flow != null && mission.flow.missionBlock != null)
		{
			if (showStartNodes)
			{
				CreateFlowStartNodes(mission.startNode, flowObjectsParent);
			}
			CreateFlowUIBlock(mission.flow.missionBlock, flowObjectsParent);
			UpdateFlowUIItems(mission.flow.missionBlock);
		}
		flowObjectsParent.gameObject.SetActive(flowObjectsParent.childCount != 0);
		if (emptyObjectivesMessage != null)
		{
			emptyObjectivesMessage.gameObject.SetActive(flowObjectsParent.childCount == 0);
		}
	}

	public void CreateFlowStartNodes(MENode startNode, Transform parent, int colorIndex = -1)
	{
		foreach (MENode dockedNode in startNode.dockedNodes)
		{
			if (!dockedNode.IsLaunchPadNode)
			{
				MEFlowUINode mEFlowUINode = ((buttonAction == MEFlowUINode.ButtonAction.CallbackToggle) ? MEFlowUINode.Create(dockedNode, toggleCallback, toggleGroup, this) : MEFlowUINode.Create(dockedNode, buttonAction, buttonCallback, this));
				mEFlowUINode.gameObject.transform.SetParent(parent);
				mEFlowUINode.gameObject.transform.localScale = Vector3.one;
			}
		}
	}

	public void ClearChildren()
	{
		int childCount = flowObjectsParent.childCount;
		while (childCount-- > 0)
		{
			Object.DestroyImmediate(flowObjectsParent.GetChild(childCount).gameObject);
		}
	}

	public static MEFlowParser Create(Transform parent, Transform flowObjectsParent, TMP_Text emptyObjectivesMessage)
	{
		return Create(parent, flowObjectsParent, emptyObjectivesMessage, null);
	}

	public static MEFlowParser Create(Transform parent, Transform flowObjectsParent, TMP_Text emptyObjectivesMessage, Color[] groupColors)
	{
		if (parserPrefab == null)
		{
			parserPrefab = MissionsUtils.MEPrefab("Prefabs/MEFlowParser.prefab");
		}
		GameObject obj = (GameObject)Object.Instantiate(parserPrefab);
		obj.transform.SetParent(parent);
		obj.transform.localScale = Vector3.one;
		MEFlowParser component = obj.GetComponent<MEFlowParser>();
		component.flowObjectsParent = flowObjectsParent;
		component.emptyObjectivesMessage = emptyObjectivesMessage;
		if (groupColors != null)
		{
			component.GroupColors = groupColors;
		}
		else if (component.GroupColors == null || component.GroupColors.Length < 1)
		{
			component.GroupColors = new Color[5]
			{
				new Color(0f, 0.91796f, 0.1406f, 1f),
				new Color(0.15234f, 9f / 32f, 1f, 1f),
				new Color(1f, 0.15234f, 0.15234f, 1f),
				new Color(1f, 0.79687f, 0.15234f, 1f),
				new Color(0.64843f, 0.15234f, 1f, 1f)
			};
		}
		return component;
	}

	public static MEFlowBlock ParseMission(Mission mission)
	{
		if (!(mission == null) && !(mission.startNode == null))
		{
			mission.flow = new MissionFlow(mission);
			BuildPathsByDFS(mission.startNode, null);
			List<MENode> list = new List<MENode>();
			for (int i = 0; i < mission.nodes.ValuesList.Count; i++)
			{
				MENode mENode = mission.nodes.ValuesList[i];
				if (mENode.IsDockedToStartNode)
				{
					continue;
				}
				if (mENode.isEndNode)
				{
					list.Add(mENode);
					continue;
				}
				if (mENode.toNodes.Count < 1)
				{
					list.Add(mENode);
					continue;
				}
				bool flag = false;
				for (int j = 0; j < mENode.toNodes.Count; j++)
				{
					if (!mENode.toNodes[j].IsOrphanNode)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(mENode);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				BuildReversePathsByDFS(list[k], null);
			}
			MEFlowThenBlock mEFlowThenBlock = new MEFlowThenBlock(mission);
			List<MEPath> list2 = new List<MEPath>();
			for (int l = 0; l < mission.flow.NodePaths[mission.startNode].paths.Count; l++)
			{
				list2.Add(mission.flow.NodePaths[mission.startNode].paths[l].Clone());
			}
			if (list2.Count > 0)
			{
				mEFlowThenBlock = ParseBlock(list2);
				mission.flow.missionBlock = mEFlowThenBlock;
			}
			return mEFlowThenBlock;
		}
		Debug.Log("No Start Node defined in mission - skippping parse");
		return null;
	}

	public static List<MEPath> BuildPathsByDFS(MENode startNode, HashSet<MENode> visited)
	{
		MissionFlow flow = startNode.mission.flow;
		if (flow.NodePaths.ContainsKey(startNode))
		{
			return flow.NodePaths[startNode].paths;
		}
		if (visited == null)
		{
			visited = new HashSet<MENode>();
		}
		visited.Add(startNode);
		MENodePathInfo mENodePathInfo = new MENodePathInfo(startNode);
		for (int i = 0; i < startNode.toNodes.Count; i++)
		{
			MENode mENode = startNode.toNodes[i];
			if (mENode.IsOrphanNode)
			{
				continue;
			}
			List<MEPath> list = (flow.NodePaths.ContainsKey(mENode) ? flow.NodePaths[mENode].paths : ((!visited.Contains(mENode)) ? BuildPathsByDFS(mENode, visited) : new List<MEPath>()));
			if (list.Count < 1)
			{
				mENodePathInfo.AddPath(new MEPath(mENode));
				continue;
			}
			for (int j = 0; j < list.Count; j++)
			{
				MEPath mEPath = new MEPath(mENode);
				mEPath.AddNodes(list[j].Nodes);
				if (mEPath.Nodes.Count > 1 && mEPath.Nodes[0] == mEPath.Nodes[mEPath.Nodes.Count - 1])
				{
					mEPath.Nodes.RemoveAt(mEPath.Nodes.Count - 1);
				}
				mENodePathInfo.AddPath(mEPath);
			}
		}
		if (!startNode.isStartNode)
		{
			for (int k = 0; k < startNode.dockedNodes.Count; k++)
			{
				MENode mENode2 = startNode.dockedNodes[k];
				if (!mENode2.IsLogicNode || mENode2.IsOrphanNode)
				{
					continue;
				}
				List<MEPath> list2 = (flow.NodePaths.ContainsKey(mENode2) ? flow.NodePaths[mENode2].paths : ((!visited.Contains(mENode2)) ? BuildPathsByDFS(mENode2, visited) : new List<MEPath>()));
				if (list2.Count < 1)
				{
					mENodePathInfo.AddPath(new MEPath(mENode2));
					continue;
				}
				for (int l = 0; l < list2.Count; l++)
				{
					MEPath mEPath2 = new MEPath(mENode2);
					mEPath2.AddNodes(list2[l].Nodes);
					if (mEPath2.Nodes.Count > 1 && mEPath2.Nodes[0] == mEPath2.Nodes[mEPath2.Nodes.Count - 1])
					{
						mEPath2.Nodes.RemoveAt(mEPath2.Nodes.Count - 1);
					}
					mENodePathInfo.AddPath(mEPath2);
				}
			}
		}
		flow.NodePaths.Add(startNode, mENodePathInfo);
		return mENodePathInfo.paths;
	}

	public static List<MEPath> BuildReversePathsByDFS(MENode startNode, HashSet<MENode> visited)
	{
		MissionFlow flow = startNode.mission.flow;
		if (flow.NodeReversePaths.ContainsKey(startNode))
		{
			return flow.NodeReversePaths[startNode].paths;
		}
		if (visited == null)
		{
			visited = new HashSet<MENode>();
		}
		visited.Add(startNode);
		MENodePathInfo mENodePathInfo = new MENodePathInfo(startNode);
		if (startNode.IsOrphanNode)
		{
			return mENodePathInfo.paths;
		}
		for (int i = 0; i < startNode.fromNodes.Count; i++)
		{
			MENode mENode = startNode.fromNodes[i];
			List<MEPath> list = (flow.NodeReversePaths.ContainsKey(mENode) ? flow.NodeReversePaths[mENode].paths : ((!visited.Contains(mENode)) ? BuildReversePathsByDFS(mENode, visited) : new List<MEPath>()));
			if (list.Count < 1)
			{
				mENodePathInfo.AddPath(new MEPath(mENode));
				continue;
			}
			for (int j = 0; j < list.Count; j++)
			{
				MEPath mEPath = new MEPath(mENode);
				mEPath.AddNodes(list[j].Nodes);
				if (mEPath.Nodes.Count > 1 && mEPath.Nodes[0] == mEPath.Nodes[mEPath.Nodes.Count - 1])
				{
					mEPath.Nodes.RemoveAt(mEPath.Nodes.Count - 1);
				}
				mENodePathInfo.AddPath(mEPath);
			}
		}
		if (startNode.dockParentNode != null)
		{
			List<MEPath> list2 = (flow.NodeReversePaths.ContainsKey(startNode.dockParentNode) ? flow.NodeReversePaths[startNode.dockParentNode].paths : ((!visited.Contains(startNode.dockParentNode)) ? BuildReversePathsByDFS(startNode.dockParentNode, visited) : new List<MEPath>()));
			if (list2.Count < 1)
			{
				mENodePathInfo.AddPath(new MEPath(startNode.dockParentNode));
			}
			else
			{
				for (int k = 0; k < list2.Count; k++)
				{
					MEPath mEPath2 = new MEPath(startNode.dockParentNode);
					mEPath2.AddNodes(list2[k].Nodes);
					if (mEPath2.Nodes.Count > 1 && mEPath2.Nodes[0] == mEPath2.Nodes[mEPath2.Nodes.Count - 1])
					{
						mEPath2.Nodes.RemoveAt(mEPath2.Nodes.Count - 1);
					}
					mENodePathInfo.AddPath(mEPath2);
				}
			}
		}
		flow.NodeReversePaths.Add(startNode, mENodePathInfo);
		return mENodePathInfo.paths;
	}

	public static MEFlowThenBlock ParseBlock(List<MEPath> paths, MENode insertNode = null)
	{
		if (paths.Count >= 1 && !(paths[0].First == null))
		{
			MEFlowThenBlock mEFlowThenBlock = new MEFlowThenBlock(paths[0].First.mission);
			if (insertNode != null)
			{
				mEFlowThenBlock.AddBlock(insertNode);
			}
			bool flag = false;
			while (!flag)
			{
				if (paths.CountStartNodes() == 1)
				{
					while (paths.CountStartNodes() == 1)
					{
						if (paths[0].Nodes.Count < 1)
						{
							paths.RemoveAt(0);
							continue;
						}
						mEFlowThenBlock.AddBlock(paths[0].Nodes[0]);
						foreach (MEPath path in paths)
						{
							if (path.Nodes.Count > 0)
							{
								path.Nodes.RemoveAt(0);
							}
						}
					}
				}
				List<MEPath> list = new List<MEPath>();
				if (paths.CountStartNodes() > 1)
				{
					List<MEPathGroup> list2 = ExtractPathGroups(paths);
					if (paths[0].First != null)
					{
						MEFlowOrBlock mEFlowOrBlock = new MEFlowOrBlock(paths[0].First.mission);
						if (list2.Count > 1)
						{
							for (int i = 0; i < list2.Count; i++)
							{
								MEFlowBlock mEFlowBlock = ParseBlock(list2[i].Paths);
								if (mEFlowBlock != null)
								{
									mEFlowOrBlock.AddBlock(mEFlowBlock);
								}
							}
						}
						else
						{
							List<MEPath> paths2 = list2[0].Paths;
							for (int j = 0; j < paths2.Count; j++)
							{
								MEPath mEPath = paths2[j];
								if (!(mEPath.First != null))
								{
									continue;
								}
								MEPath mEPath2 = new MEPath(mEPath.Nodes[0].mission);
								int count = mEPath.Nodes.Count;
								while (count-- > 0)
								{
									int num = mEPath.Nodes.IndexOf(list2[0].joinNode);
									if (num < 0)
									{
										break;
									}
									if (num <= count)
									{
										mEPath2.Nodes.Insert(0, mEPath.Nodes[count]);
										mEPath.Nodes.RemoveAt(count);
									}
								}
								list.Add(mEPath2);
							}
							List<MENode> list3 = paths2.DistinctStartNodes();
							for (int k = 0; k < list3.Count; k++)
							{
								MEFlowBlock mEFlowBlock = ParseBlock(paths2.GetPathsStarting(list3[k]));
								if (mEFlowBlock != null)
								{
									mEFlowOrBlock.AddBlock(mEFlowBlock);
								}
							}
						}
						if (mEFlowOrBlock.Blocks.Count > 0)
						{
							mEFlowThenBlock.AddBlock(mEFlowOrBlock);
						}
					}
				}
				if (list != null && list.Count > 0)
				{
					paths = list;
				}
				else
				{
					flag = true;
				}
			}
			return mEFlowThenBlock;
		}
		return null;
	}

	public static List<MEPathGroup> ExtractPathGroups(List<MEPath> paths)
	{
		List<MEPathGroup> list = new List<MEPathGroup>();
		for (int i = 0; i < paths.Count; i++)
		{
			MEPath mEPath = paths[i];
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				MEPathGroup mEPathGroup = list[j];
				MEFlowConvergence c;
				if (mEPathGroup.First == mEPath)
				{
					flag = true;
				}
				else if (mEPathGroup.First.GetConvergence(mEPath, out c, mEPathGroup.minHops))
				{
					flag = true;
					mEPathGroup.Paths.Add(mEPath);
					if (mEPathGroup.joinNode == null)
					{
						mEPathGroup.joinNode = c.Node;
					}
					else if (mEPathGroup.Paths[0].Nodes.IndexOf(mEPathGroup.joinNode) < c.Hops)
					{
						mEPathGroup.joinNode = c.Node;
						mEPathGroup.minHops = c.Hops;
						mEPathGroup.Paths.RemoveRange(1, mEPathGroup.Paths.Count - 1);
						i = -1;
					}
					break;
				}
			}
			if (!flag)
			{
				list.Add(new MEPathGroup(mEPath));
			}
		}
		return list;
	}

	public void CreateFlowUIBlock(IMEFlowBlock start, Transform parent, int colorIndex = -1)
	{
		if (start is MEFlowOrBlock)
		{
			MEFlowOrBlock mEFlowOrBlock = start as MEFlowOrBlock;
			MEFlowUIGroup_Or mEFlowUIGroup_Or = MEFlowUIGroup_Or.Create(mEFlowOrBlock, this);
			mEFlowUIGroup_Or.gameObject.transform.SetParent(parent);
			mEFlowUIGroup_Or.gameObject.transform.localScale = Vector3.one;
			colorIndex++;
			if (colorIndex > GroupColors.Length - 1)
			{
				colorIndex = 0;
			}
			for (int i = 0; i < mEFlowOrBlock.Blocks.Count; i++)
			{
				if (i > 0 && (showEvents || (mEFlowOrBlock.Blocks[i - 1].HasVisibleChildren && mEFlowOrBlock.Blocks[i].HasVisibleChildren)) && (showNonObjectives || (mEFlowOrBlock.Blocks[i - 1].HasObjectives && mEFlowOrBlock.Blocks[i].HasObjectives)))
				{
					GameObject obj = (GameObject)Object.Instantiate(textPrefab);
					obj.transform.SetParent(mEFlowUIGroup_Or.ChildHolder);
					obj.transform.localPosition = Vector3.zero;
					obj.transform.localScale = Vector3.one;
					obj.GetComponent<MEFlowUIText>().text.text = "#autoLOC_8005402";
				}
				CreateFlowUIBlock(mEFlowOrBlock.Blocks[i], mEFlowUIGroup_Or.ChildHolder, colorIndex);
			}
		}
		else if (start is MEFlowThenBlock)
		{
			MEFlowThenBlock mEFlowThenBlock = start as MEFlowThenBlock;
			if (parent != flowObjectsParent)
			{
				MEFlowUIGroup_Then mEFlowUIGroup_Then = MEFlowUIGroup_Then.Create(mEFlowThenBlock, this);
				mEFlowUIGroup_Then.gameObject.transform.SetParent(parent);
				mEFlowUIGroup_Then.gameObject.transform.localScale = Vector3.one;
				mEFlowUIGroup_Then.SetBracketColor(GroupColors[colorIndex]);
				if (!mEFlowThenBlock.HasReachableObjectives)
				{
					mEFlowUIGroup_Then.SetUnreachable();
				}
				for (int j = 0; j < mEFlowThenBlock.Blocks.Count; j++)
				{
					CreateFlowUIBlock(mEFlowThenBlock.Blocks[j], mEFlowUIGroup_Then.ChildHolder, colorIndex);
				}
			}
			else
			{
				for (int k = 0; k < mEFlowThenBlock.Blocks.Count; k++)
				{
					CreateFlowUIBlock(mEFlowThenBlock.Blocks[k], parent, colorIndex);
				}
			}
		}
		else if (start is MENode)
		{
			MENode node = start as MENode;
			MEFlowUINode mEFlowUINode = ((buttonAction == MEFlowUINode.ButtonAction.CallbackToggle) ? MEFlowUINode.Create(node, toggleCallback, toggleGroup, this) : MEFlowUINode.Create(node, buttonAction, buttonCallback, this));
			mEFlowUINode.gameObject.transform.SetParent(parent);
			mEFlowUINode.gameObject.transform.localScale = Vector3.one;
		}
	}

	public void UpdateFlowUIItems()
	{
		UpdateFlowUIItems(mission.flow.missionBlock);
	}

	public void UpdateFlowUIItems(IMEFlowBlock start)
	{
		if (start is MEFlowOrBlock)
		{
			MEFlowOrBlock mEFlowOrBlock = start as MEFlowOrBlock;
			mEFlowOrBlock.UpdateMissionFlowUI(this);
			for (int i = 0; i < mEFlowOrBlock.Blocks.Count; i++)
			{
				UpdateFlowUIItems(mEFlowOrBlock.Blocks[i]);
			}
		}
		else if (start is MEFlowThenBlock)
		{
			MEFlowThenBlock mEFlowThenBlock = start as MEFlowThenBlock;
			mEFlowThenBlock.UpdateMissionFlowUI(this);
			for (int j = 0; j < mEFlowThenBlock.Blocks.Count; j++)
			{
				UpdateFlowUIItems(mEFlowThenBlock.Blocks[j]);
			}
		}
		else if (start is MENode)
		{
			start.UpdateMissionFlowUI(this);
		}
	}
}
