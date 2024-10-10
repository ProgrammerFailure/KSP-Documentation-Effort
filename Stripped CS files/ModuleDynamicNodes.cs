using System.Collections.Generic;
using UnityEngine;

public class ModuleDynamicNodes : PartModule
{
	public List<DynamicNodeSet> SetList;

	public const float NODE_RADIUS_ENABLED = 0.4f;

	public const float NODE_RADIUS_DISABLED = 0.0001f;

	[UI_Cycle(controlEnabled = true, scene = UI_Scene.Editor, affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_8005422")]
	public int setIndex;

	[KSPField]
	public bool autostrut;

	[KSPField]
	public string MenuName = "Cluster Nodes";

	[KSPField(isPersistant = true)]
	public int NodeSetIdx;

	public void ChangeNodeSet(int newIndex)
	{
		ToggleNodeSet(NodeSetIdx, showNodes: false);
		NodeSetIdx = newIndex;
		ToggleNodeSet(NodeSetIdx, showNodes: true);
		DynamicNodeSet dynamicNodeSet = SetList[NodeSetIdx];
		base.part.stackSymmetry = dynamicNodeSet.Symmetry;
		MonoUtilities.RefreshPartContextWindow(base.part);
	}

	public void ToggleNodeSet(int idx, bool showNodes)
	{
		DynamicNodeSet dynamicNodeSet = SetList[idx];
		Transform transform = base.part.FindModelTransform(dynamicNodeSet.MeshTransform);
		if (transform != null)
		{
			transform.gameObject.SetActive(showNodes);
		}
		for (int i = 0; i < dynamicNodeSet.SetCount + 1; i++)
		{
			string nodeId = dynamicNodeSet.NodePrefix + i;
			AttachNode attachNode = base.part.FindAttachNode(nodeId);
			if (attachNode != null)
			{
				if (showNodes)
				{
					attachNode.nodeType = AttachNode.NodeType.Stack;
					attachNode.radius = 0.4f;
				}
				else
				{
					attachNode.nodeType = AttachNode.NodeType.Dock;
					attachNode.radius = 0.0001f;
				}
			}
		}
	}

	public override void OnStart(StartState state)
	{
		InitializeNodeSets();
		ChangeNodeSet(NodeSetIdx);
		GameEvents.onEditorShipModified.Add(OnShipModified);
	}

	public override void OnStartFinished(StartState state)
	{
		if (autostrut)
		{
			SetupAutoStrut();
		}
	}

	public void OnShipModified(ShipConstruct ship)
	{
		SetupAutoStrut();
	}

	public void SetupAutoStrut()
	{
		if (base.part.parent == null)
		{
			return;
		}
		int count = base.part.attachNodes.Count;
		for (int i = 0; i < count; i++)
		{
			AttachNode attachNode = base.part.attachNodes[i];
			if (attachNode.nodeType == AttachNode.NodeType.Stack && attachNode.attachedPart != null && attachNode.attachedPart != base.part && attachNode.attachedPart.persistentId != base.part.parent.persistentId)
			{
				base.part.isRobotic(out var servo);
				Part attachedPart = attachNode.attachedPart;
				if (servo == null)
				{
					attachedPart.autoStrutMode = Part.AutoStrutMode.ForceGrandparent;
					attachedPart.UpdateAutoStrut();
				}
			}
		}
	}

	public void InitializeNodeSets()
	{
		UI_Cycle uI_Cycle = base.Fields["setIndex"].uiControlEditor as UI_Cycle;
		int count = SetList.Count;
		uI_Cycle.stateNames = new string[count];
		for (int i = 0; i < count; i++)
		{
			uI_Cycle.stateNames[i] = SetList[i].DisplayText;
			if (i == NodeSetIdx)
			{
				ToggleNodeSet(i, showNodes: true);
			}
			else
			{
				ToggleNodeSet(i, showNodes: false);
			}
		}
		MonoUtilities.RefreshPartContextWindow(base.part);
	}

	public override void OnAwake()
	{
		if (SetList == null)
		{
			SetList = new List<DynamicNodeSet>();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("NODE_SET"))
		{
			SetList.Clear();
		}
		if (node.HasValue("NodeSetIdx"))
		{
			node.TryGetValue("NodeSetIdx", ref setIndex);
		}
		ConfigNode[] nodes = node.GetNodes("NODE_SET");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode node2 = nodes[i];
			DynamicNodeSet item = default(DynamicNodeSet);
			item.Load(node2);
			SetList.Add(item);
		}
	}

	public void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsEditor && setIndex != NodeSetIdx)
		{
			ChangeNodeSet(setIndex);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onEditorShipModified.Remove(OnShipModified);
	}
}
