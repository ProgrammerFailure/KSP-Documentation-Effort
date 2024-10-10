using System.Collections.Generic;
using UnityEngine;

public class ModuleStructuralNode : PartModule, IActivateOnDecouple
{
	public Transform structTransform;

	[KSPField]
	public string attachNodeNames = "bottom";

	[KSPField]
	public float nodeRadius = 0.4f;

	[KSPField]
	public string rootObject = "Fairing";

	[KSPField]
	public bool spawnManually;

	[KSPField(isPersistant = true)]
	public bool spawnState;

	[KSPField]
	public bool reverseVisibility;

	[KSPField(isPersistant = true)]
	public bool visibilityState = true;

	public override void OnStart(StartState state)
	{
		structTransform = base.part.FindModelTransform(rootObject);
		if (!(structTransform == null))
		{
			if (state == StartState.Editor)
			{
				structTransform.gameObject.SetActive(value: false);
			}
			else
			{
				CheckDisplay();
			}
		}
	}

	public void LateUpdate()
	{
		if (HighLogic.LoadedSceneIsEditor && !(structTransform == null))
		{
			CheckDisplay();
		}
	}

	public void CheckDisplayEditor(Part p)
	{
		if (!(structTransform == null))
		{
			CheckDisplay();
		}
	}

	public void SpawnStructure()
	{
		spawnState = true;
	}

	public void DespawnStructure()
	{
		spawnState = false;
	}

	public void SetNodeState(bool showNodes)
	{
		string[] array = attachNodeNames.Split(',');
		foreach (string nodeId in array)
		{
			AttachNode attachNode = base.part.FindAttachNode(nodeId);
			if (attachNode != null)
			{
				if (showNodes)
				{
					attachNode.nodeType = AttachNode.NodeType.Stack;
					attachNode.radius = nodeRadius;
				}
				else
				{
					attachNode.nodeType = AttachNode.NodeType.Dock;
					attachNode.radius = 0.001f;
				}
			}
		}
	}

	public void CheckDisplay()
	{
		string[] array = attachNodeNames.Split(',');
		List<AttachNode> list = new List<AttachNode>();
		string[] array2 = array;
		foreach (string nodeId in array2)
		{
			AttachNode attachNode = base.part.FindAttachNode(nodeId);
			if (attachNode != null)
			{
				list.Add(attachNode);
			}
		}
		bool flag = false;
		if (visibilityState)
		{
			if (spawnManually)
			{
				flag = spawnState;
			}
			else
			{
				foreach (AttachNode item in list)
				{
					if (item.attachedPart != null)
					{
						flag = true;
						break;
					}
				}
			}
			if (reverseVisibility)
			{
				flag = !flag;
			}
		}
		structTransform.gameObject.SetActive(flag);
	}

	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		CheckDisplay();
	}
}
