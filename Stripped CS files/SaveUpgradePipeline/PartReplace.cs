using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

public abstract class PartReplace : PartOffset
{
	public class attachNodeOffset
	{
		public string id;

		public Vector3 offset;
	}

	public string replacementPartName;

	public string referenceTransformName;

	public Vector3 childPositionOffset;

	public Vector3 attach0Offset;

	public List<attachNodeOffset> attachNodeOffsets;

	public char[] commaSplitArray = new char[1] { ',' };

	public PartReplace()
	{
	}

	public override void OnInit()
	{
		Setup(out partName, out replacementPartName, out referenceTransformName, out positionOffset, out rotationOffset, out childPositionOffset, out attach0Offset, out attachNodeOffsets);
		PartLoader.Instance.AddPartReplacement(partName, replacementPartName);
	}

	public abstract void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets);

	public override void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset)
	{
		Debug.LogError("Abstract Setup method called on PartReplace upgradeScript - shouldnt be called");
		throw new NotImplementedException();
	}

	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		return base.OnTest(node, loadContext, ref nodeName);
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		string partNodeNameValue = NodeUtil.GetPartNodeNameValue(node, loadContext);
		string text = (partNodeNameValue.Contains("_") ? partNodeNameValue.Substring(partNodeNameValue.LastIndexOf('_')) : string.Empty);
		string text2 = replacementPartName + text;
		NodeUtil.SetPartNodeNameValue(node, loadContext, text2);
		ConfigNode[] nodes = parentNode.GetNodes(nodeUrlCraft);
		ConfigNode configNode = null;
		switch (loadContext)
		{
		case LoadContext.flag_1:
		{
			int value = -1;
			if (node.TryGetValue("parent", ref value))
			{
				configNode = nodes[value];
			}
			if (configNode == node)
			{
				configNode = null;
			}
			break;
		}
		case LoadContext.Craft:
			foreach (ConfigNode configNode2 in nodes)
			{
				if (configNode == null)
				{
					List<string> valuesList = configNode2.GetValuesList("link");
					for (int j = 0; j < valuesList.Count; j++)
					{
						if (partNodeNameValue == valuesList[j])
						{
							configNode = configNode2;
							valuesList[j] = text2;
							break;
						}
					}
					if (configNode != null)
					{
						configNode2.RemoveValues("link");
						for (int k = 0; k < valuesList.Count; k++)
						{
							configNode2.AddValue("link", valuesList[k]);
						}
					}
				}
				List<string> valuesList2 = configNode2.GetValuesList("attN");
				bool flag = false;
				for (int l = 0; l < valuesList2.Count; l++)
				{
					if (valuesList2[l].Contains(partNodeNameValue))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					configNode2.RemoveValues("attN");
					for (int m = 0; m < valuesList2.Count; m++)
					{
						configNode2.AddValue("attN", valuesList2[m].Replace(partNodeNameValue, text2));
					}
				}
			}
			break;
		}
		Vector3 vector = positionOffset;
		Vector3 vector2 = childPositionOffset;
		position0 = KSPUtil.ParseVector3(NodeUtil.GetPartPosition(node, loadContext));
		rotation0 = KSPUtil.ParseQuaternion(NodeUtil.GetPartRotation(node, loadContext));
		if (configNode != null)
		{
			Vector3 rhs = KSPUtil.ParseVector3(NodeUtil.GetPartPosition(configNode, loadContext)) - position0;
			if (Vector3.Dot(rotation0 * Vector3.up, rhs) < 0f)
			{
				Vector3 vector3 = vector;
				vector = vector2;
				vector2 = vector3;
			}
		}
		if (loadContext == LoadContext.Craft || configNode != null)
		{
			Vector3 newPos = position0 + rotation0 * vector;
			NodeUtil.SetPartPosition(node, loadContext, newPos);
			Quaternion newRot = rotation0 * rotationOffset;
			NodeUtil.SetPartRotation(node, loadContext, newRot);
		}
		if (loadContext == LoadContext.flag_1 && node.HasValue("rTrf"))
		{
			node.SetValue("rTrf", referenceTransformName);
		}
		if (loadContext == LoadContext.Craft)
		{
			if (node.HasValue("attPos0"))
			{
				Vector3 vector4 = KSPUtil.ParseVector3(node.GetValue("attPos0"));
				vector4 += rotation0 * attach0Offset;
				node.SetValue("attPos0", KSPUtil.WriteVector(vector4));
			}
			List<string> valuesList3 = node.GetValuesList("attN");
			node.RemoveValues("attN");
			for (int n = 0; n < valuesList3.Count; n++)
			{
				string[] array = valuesList3[n].Split(commaSplitArray, 2);
				if (array.Length > 1 && array[1].Split('_').Length > 2)
				{
					string text3 = array[0];
					int num = valuesList3[n].LastIndexOf('_');
					Vector3 vector5 = KSPUtil.ParseVector3(valuesList3[n].Substring(num + 1), '|');
					for (int num2 = 0; num2 < attachNodeOffsets.Count; num2++)
					{
						if (text3 == attachNodeOffsets[num2].id)
						{
							vector5 += attachNodeOffsets[num2].offset;
						}
					}
					node.AddValue("attN", valuesList3[n].Substring(0, num + 1) + KSPUtil.WriteVector(vector5, "|"));
				}
				else
				{
					node.AddValue("attN", valuesList3[n]);
				}
			}
		}
		switch (loadContext)
		{
		case LoadContext.Craft:
			AdjustCraftChildPartsPos(node, configNode, loadContext, nodes, vector - vector2);
			break;
		case LoadContext.flag_1:
			if (configNode != null)
			{
				AdjustSFSChildPartsPos(node, loadContext, nodes, vector - vector2);
			}
			else
			{
				AdjustSFSRootChildPartsPos(node, loadContext, nodes, vector, vector2);
			}
			break;
		}
	}

	public void AdjustCraftChildPartsPos(ConfigNode node, ConfigNode parent, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offset)
	{
		List<string> valuesList = node.GetValuesList("link");
		if (parent == null)
		{
			List<string> valuesList2 = node.GetValuesList("attN");
			for (int i = 0; i < valuesList2.Count; i++)
			{
				if (!(valuesList2[i].Split(commaSplitArray, 2)[0] == "top"))
				{
					continue;
				}
				int count = valuesList.Count;
				while (count-- > 0)
				{
					if (valuesList2[i].IndexOf(valuesList[count]) > -1)
					{
						valuesList.RemoveAt(count);
						break;
					}
				}
				break;
			}
		}
		node.GetValuesList("link");
		for (int j = 0; j < valuesList.Count; j++)
		{
			foreach (ConfigNode configNode in partNodes)
			{
				if (NodeUtil.GetPartNodeNameValue(configNode, loadContext) == valuesList[j])
				{
					AdjustPartByOffset(configNode, loadContext, offset);
					AdjustCraftChildPartsPos(configNode, node, loadContext, partNodes, offset);
				}
			}
		}
	}

	public void AdjustSFSChildPartsPos(ConfigNode node, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offset)
	{
		int num = partNodes.IndexOf(node);
		for (int i = 0; i < partNodes.Length; i++)
		{
			if (num != i)
			{
				ConfigNode configNode = partNodes[i];
				if (configNode.GetValue("parent") == num.ToString())
				{
					AdjustPartByOffset(configNode, loadContext, offset);
					AdjustSFSChildPartsPos(configNode, loadContext, partNodes, offset);
				}
			}
		}
	}

	public void AdjustSFSRootChildPartsPos(ConfigNode node, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offsetTop, Vector3 offsetBottom)
	{
		int num = partNodes.IndexOf(node);
		for (int i = 0; i < partNodes.Length; i++)
		{
			if (num == i)
			{
				continue;
			}
			ConfigNode configNode = partNodes[i];
			if (configNode.GetValue("parent") == num.ToString())
			{
				if (KSPUtil.ParseVector3(NodeUtil.GetPartPosition(configNode, loadContext)).y > 0f)
				{
					AdjustPartByOffset(configNode, loadContext, -offsetTop);
					AdjustSFSChildPartsPos(configNode, loadContext, partNodes, -offsetTop);
				}
				else
				{
					AdjustPartByOffset(configNode, loadContext, -offsetBottom);
					AdjustSFSChildPartsPos(configNode, loadContext, partNodes, -offsetBottom);
				}
			}
		}
	}

	public void AdjustPartByOffset(ConfigNode part, LoadContext loadContext, Vector3 offset)
	{
		Vector3 newPos = KSPUtil.ParseVector3(NodeUtil.GetPartPosition(part, loadContext));
		newPos += rotation0 * offset;
		NodeUtil.SetPartPosition(part, loadContext, newPos);
	}
}
