using System.Collections.Generic;
using UnityEngine;

public class PhysicMaterialLibrary : MonoBehaviour
{
	[SerializeField]
	public Dictionary<string, PhysicMaterialDefinition> physicMaterials;

	public string resourcePath;

	public string resourceExtension;

	public PhysicMaterialDefinition DefaultMaterial;

	public static PhysicMaterialLibrary Instance { get; set; }

	public void Reset()
	{
		resourcePath = "Resources/";
		resourceExtension = "cfg";
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("PhysicMaterialLibrary cannot exist on two gameobjects in scene");
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		physicMaterials = new Dictionary<string, PhysicMaterialDefinition>();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LoadDefinitions()
	{
		physicMaterials.Clear();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("PHYSICMATERIAL_DEFINITION");
		if (configNodes == null)
		{
			return;
		}
		int num = configNodes.Length;
		for (int i = 0; i < num; i++)
		{
			CreatePhysicMaterials(configNodes[i]);
			if (configNodes[i].HasValue("name"))
			{
				Debug.Log("Resource " + configNodes[i].GetValue("name") + " added to database");
			}
			else
			{
				Debug.Log("Resource " + configNodes[i].name + " added to database");
			}
		}
		if (physicMaterials.ContainsKey("Default"))
		{
			DefaultMaterial.material = physicMaterials["Default"].material;
		}
		else
		{
			CreateDefaultPhysicMaterial();
		}
	}

	public void CreatePhysicMaterials(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogWarning("Config has no name field");
			return;
		}
		string value = node.GetValue("name");
		if (physicMaterials.ContainsKey(value))
		{
			Debug.LogWarning("physicMaterialsDictionary already contains definition for '" + value + "'");
			return;
		}
		PhysicMaterialDefinition physicMaterialDefinition = new PhysicMaterialDefinition();
		physicMaterialDefinition.Load(node);
		PhysicMaterial physicMaterial = new PhysicMaterial(physicMaterialDefinition.name + "PhysicMaterial");
		physicMaterial.bounceCombine = physicMaterialDefinition.bounceCombine;
		physicMaterial.bounciness = physicMaterialDefinition.bounciness;
		physicMaterial.dynamicFriction = physicMaterialDefinition.dynamicFriction;
		physicMaterial.frictionCombine = physicMaterialDefinition.frictionCombine;
		physicMaterial.staticFriction = physicMaterialDefinition.staticFriction;
		physicMaterialDefinition.material = physicMaterial;
		PDebug.Log("PhysicMaterial: " + physicMaterialDefinition.name, PDebug.DebugLevel.ResourceNetwork);
		physicMaterials.Add(physicMaterialDefinition.name, physicMaterialDefinition);
	}

	public void CreateDefaultPhysicMaterial()
	{
		DefaultMaterial = new PhysicMaterialDefinition("Default", "#autoLOC_6011085", 0.4f, 0.4f, 0f, PhysicMaterialCombine.Average, PhysicMaterialCombine.Average);
		PhysicMaterial physicMaterial = new PhysicMaterial(string.Concat(DefaultMaterial, "PhysicMaterial"));
		physicMaterial.bounceCombine = DefaultMaterial.bounceCombine;
		physicMaterial.bounciness = DefaultMaterial.bounciness;
		physicMaterial.dynamicFriction = DefaultMaterial.dynamicFriction;
		physicMaterial.frictionCombine = DefaultMaterial.frictionCombine;
		physicMaterial.staticFriction = DefaultMaterial.staticFriction;
		DefaultMaterial.material = physicMaterial;
	}
}
