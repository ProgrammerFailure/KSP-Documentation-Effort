using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartVariant : IConfigNode
{
	[SerializeField]
	public string name;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string primaryColor;

	[SerializeField]
	public string secondaryColor;

	[SerializeField]
	public string sizeGroup;

	[SerializeField]
	public List<Material> materials;

	[SerializeField]
	public List<AttachNode> attachNodes;

	[SerializeField]
	public AttachNode srfAttachNode;

	[SerializeField]
	public Transform modelTransform;

	[SerializeField]
	public List<PartGameObjectInfo> infoGameObjects;

	[SerializeField]
	public List<PartRawInfo> extraInfo;

	[SerializeField]
	public float mass;

	[SerializeField]
	public float cost;

	[SerializeField]
	public List<string> disabledAnimations;

	[SerializeField]
	public List<string> disabledEvents;

	[SerializeField]
	public AttachRules attachRules;

	public string themeName;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return displayName;
		}
		set
		{
			displayName = value;
		}
	}

	public string PrimaryColor
	{
		get
		{
			return primaryColor;
		}
		set
		{
			primaryColor = value;
		}
	}

	public string SecondaryColor
	{
		get
		{
			return secondaryColor;
		}
		set
		{
			secondaryColor = value;
		}
	}

	public string SizeGroup
	{
		get
		{
			return sizeGroup;
		}
		set
		{
			sizeGroup = value;
		}
	}

	public List<Material> Materials => materials;

	public List<PartGameObjectInfo> InfoGameObjects => infoGameObjects;

	public List<AttachNode> AttachNodes => attachNodes;

	public AttachNode SrfAttachNode => srfAttachNode;

	public float Mass
	{
		get
		{
			return mass;
		}
		set
		{
			mass = value;
		}
	}

	public float Cost
	{
		get
		{
			return cost;
		}
		set
		{
			cost = value;
		}
	}

	public List<string> DisabledAnimations
	{
		get
		{
			return disabledAnimations;
		}
		set
		{
			disabledAnimations = value;
		}
	}

	public List<string> DisabledEvents
	{
		get
		{
			return disabledEvents;
		}
		set
		{
			disabledEvents = value;
		}
	}

	public AttachRules AttachRules
	{
		get
		{
			return attachRules;
		}
		set
		{
			attachRules = value;
		}
	}

	public PartVariant(string name, string displayName, List<AttachNode> attachNodes)
		: this(name, displayName, attachNodes, null)
	{
	}

	public PartVariant(string name, string displayName, List<AttachNode> attachNodes, AttachNode srfAttachNode)
	{
		this.name = name;
		this.displayName = displayName;
		materials = new List<Material>();
		infoGameObjects = new List<PartGameObjectInfo>();
		extraInfo = new List<PartRawInfo>();
		this.attachNodes = new List<AttachNode>();
		this.srfAttachNode = srfAttachNode;
		mass = 0f;
		cost = 0f;
		if (attachNodes != null)
		{
			for (int i = 0; i < attachNodes.Count; i++)
			{
				this.attachNodes.Add(AttachNode.Clone(attachNodes[i]));
			}
		}
		disabledAnimations = new List<string>();
		disabledEvents = new List<string>();
	}

	public PartVariant(PartVariant baseVariant)
	{
		name = baseVariant.name;
		displayName = baseVariant.displayName;
		primaryColor = baseVariant.primaryColor;
		secondaryColor = baseVariant.secondaryColor;
		sizeGroup = baseVariant.sizeGroup;
		modelTransform = baseVariant.modelTransform;
		infoGameObjects = new List<PartGameObjectInfo>();
		extraInfo = new List<PartRawInfo>();
		materials = new List<Material>();
		attachNodes = new List<AttachNode>();
		mass = baseVariant.mass;
		cost = baseVariant.cost;
		srfAttachNode = baseVariant.srfAttachNode;
		for (int i = 0; i < baseVariant.materials.Count; i++)
		{
			Material item = new Material(baseVariant.materials[i])
			{
				name = baseVariant.materials[i].name
			};
			materials.Add(item);
		}
		if (baseVariant.attachNodes != null)
		{
			for (int j = 0; j < baseVariant.attachNodes.Count; j++)
			{
				attachNodes.Add(AttachNode.Clone(baseVariant.attachNodes[j]));
			}
		}
		disabledAnimations = new List<string>();
		if (baseVariant.disabledAnimations != null)
		{
			for (int k = 0; k < baseVariant.disabledAnimations.Count; k++)
			{
				disabledAnimations.Add(baseVariant.disabledAnimations[k]);
			}
		}
		disabledEvents = new List<string>();
		if (baseVariant.disabledEvents != null)
		{
			for (int l = 0; l < baseVariant.disabledEvents.Count; l++)
			{
				disabledEvents.Add(baseVariant.disabledEvents[l]);
			}
		}
		themeName = baseVariant.themeName;
		attachRules = baseVariant.attachRules;
	}

	public void Load(ConfigNode node)
	{
		name = node.GetValue("name");
		displayName = node.GetValue("displayName");
		if (string.IsNullOrEmpty(displayName))
		{
			displayName = name;
		}
		primaryColor = node.GetValue("primaryColor");
		secondaryColor = node.GetValue("secondaryColor");
		sizeGroup = node.GetValue("sizeGroup");
		float result = 0f;
		float.TryParse(node.GetValue("mass"), out result);
		mass = result;
		float result2 = 0f;
		float.TryParse(node.GetValue("cost"), out result2);
		cost = result2;
		ConfigNode node2 = node.GetNode("GAMEOBJECTS");
		if (node2 != null)
		{
			infoGameObjects.Clear();
			foreach (ConfigNode.Value value4 in node2.values)
			{
				if (bool.TryParse(value4.value, out var result3))
				{
					PartGameObjectInfo item = new PartGameObjectInfo(value4.name, result3);
					infoGameObjects.Add(item);
				}
			}
		}
		ConfigNode node3 = node.GetNode("EXTRA_INFO");
		if (node3 != null)
		{
			extraInfo.Clear();
			foreach (ConfigNode.Value value5 in node3.values)
			{
				PartRawInfo item2 = new PartRawInfo(value5.name, value5.value);
				extraInfo.Add(item2);
			}
		}
		ConfigNode node4 = node.GetNode("NODES");
		if (node4 != null)
		{
			foreach (ConfigNode.Value value6 in node4.values)
			{
				AttachNode attachNode = ParseNode(value6);
				if (attachNode == null)
				{
					continue;
				}
				if (!(attachNode.id == "attach") && !(attachNode.id == "srfAttach"))
				{
					int attachedNodeIndex = GetAttachedNodeIndex(attachNode.id);
					if (attachedNodeIndex != -1)
					{
						attachNodes[attachedNodeIndex] = attachNode;
					}
				}
				else
				{
					srfAttachNode = attachNode;
				}
			}
		}
		ConfigNode[] nodes = node.GetNodes("TEXTURE");
		for (int i = 0; i < nodes.Length; i++)
		{
			PartMaterialInfo partMaterialInfo = new PartMaterialInfo();
			partMaterialInfo.Load(nodes[i]);
			UpdateMaterial(partMaterialInfo);
		}
		if (node.HasValue("disabledAnimations"))
		{
			string[] array = node.GetValue("disabledAnimations").Split(',');
			for (int j = 0; j < array.Length; j++)
			{
				disabledAnimations.Add(array[j]);
			}
		}
		if (node.HasValue("disabledEvents"))
		{
			string[] array2 = node.GetValue("disabledEvents").Split(',');
			for (int k = 0; k < array2.Length; k++)
			{
				disabledEvents.Add(array2[k]);
			}
		}
		node.TryGetValue("themeName", ref themeName);
		string value3 = "";
		if (node.TryGetValue("attachRules", ref value3))
		{
			attachRules = AttachRules.Parse(value3);
		}
	}

	public int GetAttachedNodeIndex(string id)
	{
		int num = 0;
		while (true)
		{
			if (num < attachNodes.Count)
			{
				if (attachNodes[num].id == id)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public void Save(ConfigNode node)
	{
	}

	public void UpdateMaterial(PartMaterialInfo partMaterialInfo)
	{
		List<Material> list = new List<Material>();
		if (!string.IsNullOrEmpty(partMaterialInfo.MaterialName))
		{
			for (int i = 0; i < materials.Count; i++)
			{
				if (materials[i].name.StartsWith(partMaterialInfo.MaterialName))
				{
					list.Add(materials[i]);
				}
			}
		}
		else
		{
			list = materials;
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (partMaterialInfo.CustomShader != null)
			{
				list[j].shader = partMaterialInfo.CustomShader;
			}
			partMaterialInfo.ApplyCommands(list[j]);
		}
	}

	public void UpdateMaterialFromExtraInfo(Material material)
	{
		string text = "";
		if (material.shader.name.Contains("(Mapped)"))
		{
			material.SetTexture("_SpecMap", null);
		}
		for (int i = 0; i < extraInfo.Count; i++)
		{
			text = extraInfo[i].Value;
			if (!string.IsNullOrEmpty(text))
			{
				float result = 0f;
				if (float.TryParse(text, out result))
				{
					material.SetFloat(extraInfo[i].Name, result);
				}
				else if (text.Contains(material.name.Replace(" (Instance)", "")) || text.Contains(material.name.Replace(" (Instance)", "").Replace("Flight", "")))
				{
					material.SetTexture(extraInfo[i].Name, GameDatabase.Instance.GetTexture(text, asNormalMap: false));
				}
			}
		}
		if (material.shader.name.Contains("(Mapped)") && material.GetTexture("_SpecMap") == null)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				material.shader = Shader.Find("KSP/Bumped Specular Opaque (Cutoff)");
			}
			else
			{
				material.shader = Shader.Find("KSP/Bumped Specular (Cutoff)");
			}
		}
	}

	public bool MaterialExists(string materialName)
	{
		bool result = false;
		for (int i = 0; i < materials.Count; i++)
		{
			if (materialName == materials[i].name)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool TryCopyMaterial(Material baseMaterial)
	{
		if (!MaterialExists(baseMaterial.name))
		{
			Material item = new Material(baseMaterial)
			{
				name = baseMaterial.name
			};
			materials.Add(item);
		}
		return false;
	}

	public void UpdateModel(Transform partRoot)
	{
		if (infoGameObjects == null || infoGameObjects.Count <= 0 || !(partRoot != null))
		{
			return;
		}
		for (int i = 0; i < infoGameObjects.Count; i++)
		{
			Transform transform = FindChild(partRoot, infoGameObjects[i].Name);
			if (transform != null)
			{
				transform.gameObject.SetActive(infoGameObjects[i].Status);
			}
		}
	}

	public Transform FindChild(Transform parent, string childName)
	{
		Transform result = null;
		Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == childName)
			{
				result = componentsInChildren[i];
				break;
			}
		}
		return result;
	}

	public string GetExtraInfoValue(string name)
	{
		string result = string.Empty;
		for (int i = 0; i < extraInfo.Count; i++)
		{
			if (extraInfo[i].Name == name)
			{
				result = extraInfo[i].Value;
			}
		}
		return result;
	}

	public AttachNode ParseNode(ConfigNode.Value nodeValue)
	{
		AttachNode attachNode = null;
		if (nodeValue.name.StartsWith("node"))
		{
			string[] array = nodeValue.value.Split(',');
			string[] array2 = nodeValue.name.Split('_');
			if (array.Length < 6)
			{
				PDebug.Warning("ERROR: Bad node definition at " + nodeValue.name);
				return null;
			}
			attachNode = new AttachNode();
			attachNode.position = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
			attachNode.orientation = new Vector3(float.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5]));
			attachNode.originalPosition = attachNode.position;
			attachNode.originalOrientation = attachNode.orientation;
			if (array.Length >= 7)
			{
				attachNode.size = int.Parse(array[6]);
			}
			else
			{
				attachNode.size = 1;
			}
			if (array.Length >= 8)
			{
				attachNode.attachMethod = (AttachNodeMethod)int.Parse(array[7]);
			}
			else
			{
				if (array2[1] == "stack")
				{
					attachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
				}
				if (array2[1] == "dock")
				{
					attachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
				}
				if (array2[1] == "attach")
				{
					attachNode.attachMethod = AttachNodeMethod.HINGE_JOINT;
				}
			}
			if (array.Length >= 9)
			{
				attachNode.ResourceXFeed = int.Parse(array[8]) > 0;
			}
			if (array.Length >= 10)
			{
				attachNode.rigid = int.Parse(array[9]) > 0;
			}
			PDebug.Log("Added " + nodeValue.name + " at " + nodeValue.value, PDebug.DebugLevel.PartLoader);
			if (array2[1] == "stack")
			{
				if (array2.Length > 2)
				{
					attachNode.id = array2[2];
				}
				attachNode.nodeType = AttachNode.NodeType.Stack;
			}
			if (array2[1] == "dock")
			{
				if (array2.Length > 2)
				{
					attachNode.id = array2[2];
				}
				attachNode.nodeType = AttachNode.NodeType.Dock;
			}
			if (array2[1] == "attach")
			{
				attachNode.id = "srfAttach";
				attachNode.nodeType = AttachNode.NodeType.Surface;
			}
		}
		return attachNode;
	}
}
