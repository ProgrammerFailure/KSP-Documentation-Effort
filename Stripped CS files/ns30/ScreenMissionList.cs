using System.Collections.Generic;
using Contracts;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using UnityEngine;

namespace ns30;

public class ScreenMissionList : MonoBehaviour
{
	public enum ScreenContractListMode
	{
		Active,
		Offered,
		Archived
	}

	public ScreenMissionExistingItem itemPrefab;

	public RectTransform listParent;

	public List<ScreenMissionExistingItem> items = new List<ScreenMissionExistingItem>();

	public ScreenContractListMode mode;

	public bool dirty;

	public void Start()
	{
		GameEvents.onGameStateCreated.Add(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Add(OnGameStateLoaded);
		GameEvents.Contract.onOffered.Add(OnContractsModified);
		GameEvents.Contract.onAccepted.Add(OnContractsModified);
		GameEvents.Contract.onDeclined.Add(OnContractsModified);
		GameEvents.Contract.onCompleted.Add(OnContractsModified);
		GameEvents.Contract.onFailed.Add(OnContractsModified);
		GameEvents.Contract.onCancelled.Add(OnContractsModified);
		GameEvents.Contract.onFinished.Add(OnContractsModified);
		GameEvents.Contract.onContractsLoaded.Add(OnContractsModified);
		GameEvents.Contract.onContractsListChanged.Add(OnContractsModified);
		RefreshItems();
	}

	public void OnDestroy()
	{
		GameEvents.onGameStateCreated.Remove(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Remove(OnGameStateLoaded);
		GameEvents.Contract.onOffered.Remove(OnContractsModified);
		GameEvents.Contract.onAccepted.Remove(OnContractsModified);
		GameEvents.Contract.onDeclined.Remove(OnContractsModified);
		GameEvents.Contract.onCompleted.Remove(OnContractsModified);
		GameEvents.Contract.onFailed.Remove(OnContractsModified);
		GameEvents.Contract.onCancelled.Remove(OnContractsModified);
		GameEvents.Contract.onFinished.Remove(OnContractsModified);
		GameEvents.Contract.onContractsLoaded.Remove(OnContractsModified);
		GameEvents.Contract.onContractsListChanged.Remove(OnContractsModified);
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
		List<Mission> missions = MissionSystem.missions;
		int i = 0;
		for (int count = missions.Count; i < count; i++)
		{
			Mission mission = missions[i];
			CreateItem(mission);
		}
		if (items.Count <= 0)
		{
			CreateError("There are no missions!");
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

	public void OnContractsModified()
	{
		dirty = true;
	}

	public void OnContractsModified(Contract contract)
	{
		dirty = true;
	}

	public void SetDirty()
	{
		dirty = true;
	}
}
