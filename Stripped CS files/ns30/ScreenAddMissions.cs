using System.Collections.Generic;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using UnityEngine;

namespace ns30;

public class ScreenAddMissions : MonoBehaviour
{
	public ScreenMissionExistingItem itemPrefab;

	public RectTransform listParent;

	public List<ScreenMissionExistingItem> items = new List<ScreenMissionExistingItem>();

	public bool dirty;

	public void Start()
	{
		GameEvents.onGameStateCreated.Add(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Add(OnGameStateLoaded);
		RefreshItems();
	}

	public void OnDestroy()
	{
		GameEvents.onGameStateCreated.Remove(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Remove(OnGameStateLoaded);
	}

	public void Update()
	{
		if (dirty)
		{
			RefreshItems();
			dirty = false;
		}
	}

	public void RefreshItems()
	{
		ClearItems();
		if (MissionSystem.Instance == null)
		{
			CreateError("Mission system is not available at this time!");
			return;
		}
		int i = 0;
		for (int count = MissionSystem.missions.Count; i < count; i++)
		{
			CreateItem(MissionSystem.missions[i]);
		}
	}

	public void ClearItems()
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			Object.Destroy(items[i].gameObject);
		}
		items.Clear();
	}

	public void CreateItem(Mission mission)
	{
		ScreenMissionExistingItem screenMissionExistingItem = Object.Instantiate(itemPrefab);
		screenMissionExistingItem.transform.SetParent(listParent, worldPositionStays: false);
		screenMissionExistingItem.Setup(mission);
		items.Add(screenMissionExistingItem);
	}

	public void CreateError(string errorText)
	{
		ScreenMissionExistingItem screenMissionExistingItem = Object.Instantiate(itemPrefab);
		screenMissionExistingItem.transform.SetParent(listParent, worldPositionStays: false);
		screenMissionExistingItem.SetupError(errorText);
		items.Add(screenMissionExistingItem);
	}

	public void OnGameStateCreated(Game game)
	{
		dirty = true;
	}

	public void OnGameStateLoaded(ConfigNode node)
	{
		dirty = true;
	}
}
