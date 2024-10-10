using UnityEngine;

public class PartResourceLibrary : MonoBehaviour
{
	public static int ElectricityHashcode = "ElectricCharge".GetHashCode();

	[SerializeField]
	public PartResourceDefinitionList resourceDefinitions;

	public string resourcePath;

	public string resourceExtension;

	public static PartResourceLibrary Instance { get; set; }

	public void Reset()
	{
		resourcePath = "Resources/";
		resourceExtension = "cfg";
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("PartResourceLibrary cannot exist on two gameobjects in scene");
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		resourceDefinitions = new PartResourceDefinitionList();
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
		resourceDefinitions.Clear();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("RESOURCE_DEFINITION");
		int num = configNodes.Length;
		for (int i = 0; i < num; i++)
		{
			resourceDefinitions.Add(configNodes[i]);
			if (configNodes[i].HasValue("name"))
			{
				Debug.Log("Resource " + configNodes[i].GetValue("name") + " added to database");
			}
			else
			{
				Debug.Log("Resource " + configNodes[i].name + " added to database");
			}
		}
	}

	public PartResourceDefinition GetDefinition(string name)
	{
		return resourceDefinitions[name];
	}

	public PartResourceDefinition GetDefinition(int id)
	{
		return resourceDefinitions[id];
	}

	public static ResourceFlowMode GetDefaultFlowMode(string resourceName)
	{
		return GetDefaultFlowMode(resourceName.GetHashCode());
	}

	public static ResourceFlowMode GetDefaultFlowMode(int resourceID)
	{
		PartResourceDefinition definition = Instance.GetDefinition(resourceID);
		if (definition == null)
		{
			Debug.LogError("Resource System Error: Requested resource (id: " + resourceID + ") does not exist");
			return ResourceFlowMode.NULL;
		}
		return definition.resourceFlowMode;
	}
}
