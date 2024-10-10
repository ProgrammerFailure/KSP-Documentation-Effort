using System.Collections.Generic;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.MISSIONBUILDER,
	GameScenes.EDITOR
})]
public class KerbalInventoryScenario : ScenarioModule
{
	public Dictionary<string, ModuleInventoryPart> kerbalInventories;

	public static GameObject kerbalInventoryGameObject;

	public static KerbalInventoryScenario Instance { get; set; }

	public static GameObject KerbalInventoryGameObject
	{
		get
		{
			if (kerbalInventoryGameObject == null)
			{
				kerbalInventoryGameObject = new GameObject("KerbalInventoryGameObject");
				Object.DontDestroyOnLoad(kerbalInventoryGameObject);
			}
			return kerbalInventoryGameObject;
		}
	}

	public override void OnAwake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[KerbalInventoryScenario]: Instance already exists!", Instance.gameObject);
			Object.Destroy(Instance);
		}
		Instance = this;
		kerbalInventories = new Dictionary<string, ModuleInventoryPart>();
		GameEvents.onKerbalNameChanged.Add(OnKerbalNameChanged);
	}

	public void Update()
	{
		foreach (KeyValuePair<string, ModuleInventoryPart> kerbalInventory in kerbalInventories)
		{
			kerbalInventory.Value.Update();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		foreach (KeyValuePair<string, ModuleInventoryPart> kerbalInventory in kerbalInventories)
		{
			if (kerbalInventory.Value != null)
			{
				HighLogic.CurrentGame.CrewRoster[kerbalInventory.Key]?.SaveInventory(kerbalInventory.Value);
			}
		}
	}

	public void OnDestroy()
	{
		foreach (KeyValuePair<string, ModuleInventoryPart> kerbalInventory in kerbalInventories)
		{
			if (kerbalInventory.Value != null && kerbalInventory.Value.gameObject != null)
			{
				kerbalInventory.Value.gameObject.DestroyGameObject();
			}
		}
		kerbalInventories.Clear();
		for (int num = KerbalInventoryGameObject.transform.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = KerbalInventoryGameObject.transform.GetChild(num).gameObject;
			if (gameObject != null)
			{
				gameObject.DestroyGameObject();
			}
		}
		GameEvents.onKerbalNameChanged.Remove(OnKerbalNameChanged);
	}

	public void OnKerbalNameChanged(ProtoCrewMember crew, string prevName, string newName)
	{
		if (kerbalInventories.ContainsKey(prevName))
		{
			ModuleInventoryPart value = kerbalInventories[prevName];
			kerbalInventories.Remove(prevName);
			kerbalInventories.Add(newName, value);
		}
	}

	public void AddKerbalInventoryInstance(string crewName, ModuleInventoryPart inventory)
	{
		if (kerbalInventories.ContainsKey(crewName))
		{
			Debug.LogWarningFormat("[KerbalInventoryScenario]: Duplicate Inventory Instance, replaced old instance for {0}", crewName);
			kerbalInventories[crewName] = inventory;
		}
		else
		{
			kerbalInventories.Add(crewName, inventory);
			inventory.gameObject.transform.SetParent(KerbalInventoryGameObject.transform);
			inventory.gameObject.name = crewName;
		}
	}

	public bool ContainsCrew(string crewName)
	{
		return kerbalInventories.ContainsKey(crewName);
	}

	public void RemoveKerbalInventoryInstance(string crewName)
	{
		if (!kerbalInventories.ContainsKey(crewName))
		{
			return;
		}
		kerbalInventories.Remove(crewName);
		int childCount = KerbalInventoryGameObject.transform.childCount;
		do
		{
			if (childCount-- <= 0)
			{
				return;
			}
		}
		while (!(KerbalInventoryGameObject.transform.GetChild(childCount).name == crewName));
		Object.Destroy(KerbalInventoryGameObject.transform.GetChild(childCount).gameObject);
	}
}
