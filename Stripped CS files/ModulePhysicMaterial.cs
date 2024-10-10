using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModulePhysicMaterial : PartModule
{
	[KSPField(isPersistant = true)]
	public PhysicMaterialCombine bounceCombine;

	[KSPField(isPersistant = true)]
	public float bounciness;

	[KSPField(isPersistant = true)]
	public float dynamicFriction = 0.4f;

	[KSPField(isPersistant = true)]
	public float staticFriction = 0.4f;

	[KSPField(isPersistant = true)]
	public PhysicMaterialCombine frictionCombine;

	[KSPField(isPersistant = true)]
	public string activePhysicMaterialName;

	public string activePhysicMaterialDisplayName;

	public Collider[] partColliders;

	public Renderer[] partRenderers;

	public PhysicMaterial physicMaterial;

	[SerializeField]
	public List<string> physicMaterialNames;

	public int cpIndex;

	[SerializeField]
	public List<PhysicMaterialColor> moduleMaterialColorList;

	public Dictionary<string, PhysicMaterialColor> physicMaterialColors;

	public override void OnAwake()
	{
		physicMaterialColors = new Dictionary<string, PhysicMaterialColor>();
		SetPhysicMaterialColors();
	}

	public override void OnStart(StartState state)
	{
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Add(OnVariantApplied);
		}
		if (physicMaterialNames.Count <= 0)
		{
			if (physicMaterial == null)
			{
				physicMaterial = new PhysicMaterial(base.gameObject.name + "PhysicMaterial");
				physicMaterial.bounceCombine = bounceCombine;
				physicMaterial.bounciness = bounciness;
				physicMaterial.dynamicFriction = dynamicFriction;
				physicMaterial.frictionCombine = frictionCombine;
				physicMaterial.staticFriction = staticFriction;
			}
			base.part.collider.material = physicMaterial;
			UpdatePhysicMaterialEvent();
		}
		else
		{
			ApplyListMaterial(activePhysicMaterialName);
		}
	}

	[KSPEvent(guiActiveEditor = true, guiName = "#autoLOC_6011081")]
	public void ChangePhysicMaterial()
	{
		cpIndex = physicMaterialNames.IndexOf(activePhysicMaterialName);
		cpIndex++;
		if (cpIndex >= physicMaterialNames.Count)
		{
			cpIndex = 0;
		}
		string materialName = physicMaterialNames[cpIndex];
		ApplyListMaterial(materialName);
		if (base.part.symmetryCounterparts.Count <= 0)
		{
			return;
		}
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			ModulePhysicMaterial component = base.part.symmetryCounterparts[count].GetComponent<ModulePhysicMaterial>();
			if (component != null)
			{
				component.ApplyListMaterial(materialName);
			}
		}
	}

	public void OnDestroy()
	{
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Remove(OnVariantApplied);
		}
	}

	public void OnVariantApplied(Part appliedPart, PartVariant partVariant)
	{
		if (base.part == appliedPart)
		{
			ApplyListMaterial(activePhysicMaterialName);
		}
	}

	public void ApplyListMaterial(string materialName)
	{
		if (physicMaterialNames == null)
		{
			Debug.LogErrorFormat("[ModulePhysicMaterial]: There are no physic materials for {0}", base.part.partInfo.title);
		}
		else if (!physicMaterialNames.Contains(materialName))
		{
			Debug.LogErrorFormat("[ModulePhysicMaterial]: The name {0} is not in this parts list of physicMaterials", materialName);
			SetPhysicMaterial("Default", useDefault: true);
		}
		else
		{
			SetPhysicMaterial(PhysicMaterialLibrary.Instance.physicMaterials[materialName]);
		}
	}

	public void SetPhysicMaterial(string materialName, bool useDefault)
	{
		if (physicMaterialNames == null)
		{
			Debug.LogErrorFormat("[ModulePhysicMaterial]: There are no physic materials for {0}", base.part.partInfo.title);
		}
		else if (!PhysicMaterialLibrary.Instance.physicMaterials.ContainsKey(materialName))
		{
			if (useDefault)
			{
				SetPhysicMaterial(PhysicMaterialLibrary.Instance.DefaultMaterial);
				return;
			}
			Debug.LogErrorFormat("[ModulePhysicMaterial]: The name {0} is not in the physic material library and useDefault is false", materialName);
		}
	}

	public void SetPhysicMaterial(PhysicMaterialDefinition materialDefinition)
	{
		if (materialDefinition == null)
		{
			Debug.LogErrorFormat("[ModulePhysicMaterial]: Empty materialDefinition passed to SetPhysicMaterial");
			return;
		}
		if (HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			partColliders = base.part.GetPartColliders();
			for (int i = 0; i < partColliders.Length; i++)
			{
				partColliders[i].material = materialDefinition.material;
			}
		}
		partRenderers = base.part.GetPartRenderers();
		if (physicMaterialColors.Count == 0)
		{
			SetPhysicMaterialColors();
		}
		if (physicMaterialColors.Count != 0)
		{
			for (int j = 0; j < partRenderers.Length; j++)
			{
				partRenderers[j].material.color = physicMaterialColors[materialDefinition.name].color;
			}
		}
		activePhysicMaterialName = materialDefinition.name;
		activePhysicMaterialDisplayName = materialDefinition.displayName;
		UpdatePhysicMaterialEvent();
		GameEvents.OnPhysicMaterialChanged.Fire(base.part, PhysicMaterialLibrary.Instance.physicMaterials[materialDefinition.name].material);
	}

	public void UpdatePhysicMaterialEvent()
	{
		bool flag = physicMaterialNames != null && physicMaterialNames.Count > 1 && HighLogic.LoadedScene != GameScenes.FLIGHT;
		base.Events["ChangePhysicMaterial"].guiActiveEditor = flag;
		if (flag)
		{
			base.Events["ChangePhysicMaterial"].guiName = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_6011081"), activePhysicMaterialDisplayName);
		}
	}

	public void SetPhysicMaterialColors()
	{
		if (physicMaterialNames == null)
		{
			physicMaterialNames = new List<string>();
		}
		if (moduleMaterialColorList != null)
		{
			for (int i = 0; i < moduleMaterialColorList.Count; i++)
			{
				physicMaterialColors.Add(moduleMaterialColorList[i].materialName, moduleMaterialColorList[i]);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		string[] values = node.GetValues("PhysicMaterialName");
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] != null)
			{
				physicMaterialNames.Add(values[i]);
			}
		}
		if (string.IsNullOrEmpty(activePhysicMaterialName) && physicMaterialNames.Count > 0)
		{
			activePhysicMaterialName = physicMaterialNames[0];
		}
		if (moduleMaterialColorList != null)
		{
			return;
		}
		moduleMaterialColorList = new List<PhysicMaterialColor>();
		ConfigNode node2 = new ConfigNode();
		if (!node.TryGetNode("PHYSICMATERIALCOLORS", ref node2))
		{
			return;
		}
		for (int j = 0; j < node2.nodes.Count; j++)
		{
			if (node2.nodes[j].name == "MATERIALCOLOR")
			{
				PhysicMaterialColor physicMaterialColor = new PhysicMaterialColor();
				physicMaterialColor.Load(node2.nodes[j]);
				moduleMaterialColorList.Add(physicMaterialColor);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		if (moduleMaterialColorList.Count > 0)
		{
			ConfigNode configNode = new ConfigNode("PHYSICMATERIALCOLORS");
			for (int i = 0; i < moduleMaterialColorList.Count; i++)
			{
				ConfigNode configNode2 = new ConfigNode("MATERIALCOLOR");
				configNode2.AddValue("name", physicMaterialNames[i]);
				configNode2.AddValue("color", moduleMaterialColorList[i].color);
				configNode.AddNode(configNode2);
			}
			node.AddNode(configNode);
		}
	}
}
