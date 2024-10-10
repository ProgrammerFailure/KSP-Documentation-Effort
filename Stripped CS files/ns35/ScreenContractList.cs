using System.Collections.Generic;
using Contracts;
using ns9;
using UnityEngine;

namespace ns35;

public class ScreenContractList : MonoBehaviour
{
	public enum ScreenContractListMode
	{
		Active,
		Offered,
		Archived
	}

	public ScreenContractExistingItem itemPrefab;

	public RectTransform listParent;

	public List<ScreenContractExistingItem> items = new List<ScreenContractExistingItem>();

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
		if (ContractSystem.Instance == null)
		{
			CreateError(Localizer.Format("#autoLOC_7003271"));
			return;
		}
		List<Contract> list = ((mode == ScreenContractListMode.Archived) ? ContractSystem.Instance.ContractsFinished : ContractSystem.Instance.Contracts);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Contract contract = list[i];
			switch (mode)
			{
			case ScreenContractListMode.Active:
				if (contract.ContractState == Contract.State.Active)
				{
					CreateItem(contract);
				}
				break;
			case ScreenContractListMode.Offered:
				if (contract.ContractState == Contract.State.Offered)
				{
					CreateItem(contract);
				}
				break;
			case ScreenContractListMode.Archived:
				if (contract.IsFinished())
				{
					CreateItem(contract);
				}
				break;
			}
		}
		if (items.Count <= 0)
		{
			switch (mode)
			{
			default:
				CreateError(Localizer.Format("#autoLOC_7003282"));
				break;
			case ScreenContractListMode.Active:
				CreateError(Localizer.Format("#autoLOC_7003279"));
				break;
			case ScreenContractListMode.Offered:
				CreateError(Localizer.Format("#autoLOC_7003280"));
				break;
			case ScreenContractListMode.Archived:
				CreateError(Localizer.Format("#autoLOC_7003281"));
				break;
			}
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

	public void CreateItem(Contract contract)
	{
		ScreenContractExistingItem screenContractExistingItem = Object.Instantiate(itemPrefab);
		screenContractExistingItem.transform.SetParent(listParent, worldPositionStays: false);
		screenContractExistingItem.Setup(contract, this);
		items.Add(screenContractExistingItem);
	}

	public void CreateError(string errorText)
	{
		ScreenContractExistingItem screenContractExistingItem = Object.Instantiate(itemPrefab);
		screenContractExistingItem.transform.SetParent(listParent, worldPositionStays: false);
		screenContractExistingItem.SetupError(errorText);
		items.Add(screenContractExistingItem);
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
