using System;
using System.Collections.Generic;
using Contracts;
using ns9;
using UnityEngine;

namespace ns35;

public class ScreenAddContracts : MonoBehaviour
{
	public ScreenContractNewItem itemPrefab;

	public RectTransform listParent;

	public List<ScreenContractNewItem> items = new List<ScreenContractNewItem>();

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
		if (ContractSystem.ContractTypes == null)
		{
			CreateError(Localizer.Format("#autoLOC_7003271"));
			return;
		}
		int i = 0;
		for (int count = ContractSystem.ContractTypes.Count; i < count; i++)
		{
			CreateItem(ContractSystem.ContractTypes[i]);
		}
	}

	public void ClearItems()
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			UnityEngine.Object.Destroy(items[i].gameObject);
		}
		items.Clear();
	}

	public void CreateItem(Type type)
	{
		ScreenContractNewItem screenContractNewItem = UnityEngine.Object.Instantiate(itemPrefab);
		screenContractNewItem.transform.SetParent(listParent, worldPositionStays: false);
		screenContractNewItem.Setup(type);
		items.Add(screenContractNewItem);
	}

	public void CreateError(string errorText)
	{
		ScreenContractNewItem screenContractNewItem = UnityEngine.Object.Instantiate(itemPrefab);
		screenContractNewItem.transform.SetParent(listParent, worldPositionStays: false);
		screenContractNewItem.SetupError(errorText);
		items.Add(screenContractNewItem);
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
