using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class ContractsApp : UIApp
{
	public static ContractsApp Instance;

	public Dictionary<Guid, UICascadingList.CascadingListItem> contractList = new Dictionary<Guid, UICascadingList.CascadingListItem>();

	public string[] colors_levels = new string[4] { "<color=#ffffff>", "<color=#bdbdbd>", "<color=#7e7e7e>", "<color=#636363>" };

	public string[] colors_notes = new string[4] { "<color=#acfcff>", "<color=#7eb5bc>", "<color=#527c80>", "<color=#3f5f62>" };

	public float refreshTimeToWait = 0.5f;

	public bool refreshRequested;

	public List<Contract> refreshContracts;

	public bool refreshContractPos;

	public bool gamePaused;

	[SerializeField]
	public GenericAppFrame appFramePrefab;

	public GenericAppFrame appFrame;

	[SerializeField]
	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	[SerializeField]
	public UIListItem_spacer listItem_BodyContractState_prefab;

	[SerializeField]
	public UIListItem_spacer listItem_BodyContractNote_prefab;

	public bool updateDaemonRunning;

	public override void Awake()
	{
		if (HighLogic.fetch != null && HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (Instance != null)
		{
			Debug.LogWarning("ContractsApp already exist, destroying this instance");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
			base.Awake();
		}
	}

	public override bool OnAppAboutToStart()
	{
		return true;
	}

	public override void OnAppInitialized()
	{
		appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appFrame.Setup(base.appLauncherButton, base.name, Localizer.Format("#autoLOC_442409"), 266, 440);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		HideApp();
		cascadingList = UnityEngine.Object.Instantiate(cascadingListPrefab);
		cascadingList.Setup(appFrame.scrollList);
		refreshContracts = new List<Contract>();
		StartCoroutine(UpdateDaemon());
		GameEvents.Contract.onAccepted.Add(AddContract);
		GameEvents.Contract.onDeclined.Add(RemoveContract);
		GameEvents.Contract.onCancelled.Add(RemoveContract);
		GameEvents.Contract.onCompleted.Add(RefreshContractRequest);
		GameEvents.Contract.onFailed.Add(RefreshContractRequest);
		GameEvents.Contract.onParameterChange.Add(RefreshContractParameter);
		GameEvents.Contract.onContractsLoaded.Add(OnContractsLoaded);
		GameEvents.onGamePause.Add(onGamePause);
		GameEvents.onGameUnpause.Add(onGameUnPause);
		CreateContractsList();
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
		}
	}

	public override void HideApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.ALWAYS;
	}

	public override void OnAppDestroy()
	{
		if (refreshContracts != null)
		{
			refreshContracts.Clear();
		}
		if (HighLogic.fetch != null && HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
		{
			return;
		}
		GameEvents.Contract.onAccepted.Remove(AddContract);
		GameEvents.Contract.onDeclined.Remove(RemoveContract);
		GameEvents.Contract.onCancelled.Remove(RemoveContract);
		GameEvents.Contract.onCompleted.Remove(RefreshContractRequest);
		GameEvents.Contract.onFailed.Remove(RefreshContractRequest);
		GameEvents.Contract.onParameterChange.Remove(RefreshContractParameter);
		GameEvents.Contract.onContractsLoaded.Remove(OnContractsLoaded);
		GameEvents.onGamePause.Remove(onGamePause);
		GameEvents.onGameUnpause.Remove(onGameUnPause);
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(appFrame.Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return new Vector3(ApplicationLauncher.Instance.transform.position.x, defaultAnchorPos.y, defaultAnchorPos.z);
	}

	public void OnContractsLoaded()
	{
		CreateContractsList();
	}

	public void RemoveContract(Contract contract)
	{
		if (contractList.ContainsKey(contract.ContractGuid))
		{
			UICascadingList.CascadingListItem item = contractList[contract.ContractGuid];
			cascadingList.ruiList.RemoveCascadingItem(item);
			contractList.Remove(contract.ContractGuid);
		}
	}

	public void AddContract(Contract contract)
	{
		if (contractList.ContainsKey(contract.ContractGuid))
		{
			Debug.Log("[Contracts App]: Trying to add a contract which already exists in the list.");
		}
		else
		{
			contractList.Add(contract.ContractGuid, CreateItem(contract));
		}
	}

	public void RefreshContractParameter(Contract contract, ContractParameter parameter)
	{
		RefreshContractRequested(contract, changePositions: false);
	}

	public void RefreshContractRequest(Contract contract)
	{
		RefreshContractRequested(contract);
	}

	public void RefreshContractRequested(Contract contract, bool changePositions = true)
	{
		refreshRequested = true;
		refreshContracts.AddUnique(contract);
		refreshContractPos = changePositions;
	}

	public void RefreshContract(Contract contract, bool changePositions = true)
	{
		if (!contractList.ContainsKey(contract.ContractGuid))
		{
			Debug.Log("[Contracts App]: No contract found while attempting to refresh contracts list");
			return;
		}
		UICascadingList.CascadingListItem item = contractList[contract.ContractGuid];
		UIListItem uIListItem = null;
		Button button;
		if (contract.ContractState != Contract.State.Failed && contract.ContractState != Contract.State.DeadlineExpired)
		{
			if (contract.ContractState == Contract.State.Completed)
			{
				uIListItem = cascadingList.CreateHeader("<color=#00ff00>" + contract.Title + "</color>", out button, scaleBg: true);
				UpdateLauncherButtonPlayAnim();
			}
			else
			{
				uIListItem = cascadingList.CreateHeader("<color=#e6752a>" + contract.Title + "</color>", out button, scaleBg: true);
			}
		}
		else
		{
			uIListItem = cascadingList.CreateHeader("<color=#ff0000>" + contract.Title + "</color>", out button, scaleBg: true);
		}
		UICascadingList.CascadingListItem cascadingListItem = null;
		cascadingListItem = ((!changePositions) ? cascadingList.ruiList.UpdateCascadingItem(item, uIListItem, cascadingList.CreateFooter(), CreateParameterList(contract), button) : cascadingList.ruiList.UpdateCascadingItem(item, uIListItem, cascadingList.CreateFooter(), CreateParameterList(contract), button, UICascadingList.UpdateScrollAction.SCROLL_TO_UPDATED));
		if (cascadingListItem != null)
		{
			contractList[contract.ContractGuid] = cascadingListItem;
		}
	}

	public IEnumerator UpdateDaemon()
	{
		yield return null;
		yield return null;
		if (updateDaemonRunning)
		{
			yield break;
		}
		updateDaemonRunning = true;
		while ((bool)this)
		{
			if (ApplicationLauncher.Ready && appFrame.gameObject.activeSelf && !gamePaused && refreshRequested)
			{
				int i = 0;
				for (int count = refreshContracts.Count; i < count; i++)
				{
					RefreshContract(refreshContracts[i], refreshContractPos);
				}
				refreshContracts.Clear();
				refreshRequested = false;
			}
			yield return new WaitForSeconds(refreshTimeToWait);
		}
	}

	public void CreateContractsList()
	{
		appFrame.scrollList.Clear(destroyElements: true);
		contractList.Clear();
		if (!(ContractSystem.Instance != null))
		{
			return;
		}
		int i = 0;
		for (int count = ContractSystem.Instance.Contracts.Count; i < count; i++)
		{
			Contract contract = ContractSystem.Instance.Contracts[i];
			if (contract.ContractState == Contract.State.Active)
			{
				contractList.Add(contract.ContractGuid, CreateItem(contract));
			}
		}
	}

	public UICascadingList.CascadingListItem CreateItem(Contract contract)
	{
		Button button;
		UIListItem header = cascadingList.CreateHeader("<color=#e6752a>" + contract.Title + "</color>", out button, scaleBg: true);
		return cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), CreateParameterList(contract), button);
	}

	public List<UIListItem> CreateParameterList(Contract contract)
	{
		List<UIListItem> list = new List<UIListItem>();
		for (int i = 0; i < contract.ParameterCount; i++)
		{
			ContractParameter parameter = contract.GetParameter(i);
			AddSubParameter(list, parameter, 0);
		}
		return list;
	}

	public void AddSubParameter(List<UIListItem> tasks, ContractParameter p, int level)
	{
		if (level >= 4 || string.IsNullOrEmpty(p.Title))
		{
			return;
		}
		string text = "    ";
		UIListItem_spacer uIListItem_spacer = cascadingList.CreateBody_spacer(listItem_BodyContractState_prefab, text + colors_levels[level] + p.Title + "</color>", 6 * level);
		tasks.Add(uIListItem_spacer);
		uIListItem_spacer.GetComponentInChildren<UIStateImage>().SetState(p.State.ToString().ToLower());
		if (p.State != ParameterState.Complete && !string.IsNullOrEmpty(p.Notes))
		{
			tasks.Add(cascadingList.CreateBodyCollapsable_spacer(listItem_BodyContractNote_prefab, colors_notes[level] + Localizer.Format("#autoLOC_7003003") + "</color>", colors_notes[level] + Localizer.Format("#autoLOC_7003004", p.Notes) + "</color>", 6 * level));
		}
		if (p.State == ParameterState.Incomplete)
		{
			for (int i = 0; i < p.ParameterCount; i++)
			{
				AddSubParameter(tasks, p.GetParameter(i), level + 1);
			}
		}
	}

	public void UpdateLauncherButtonPlayAnim()
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready)
		{
			base.appLauncherButton.PlayAnim(ApplicationLauncherButton.AnimatedIconType.NOTIFICATION, 5f);
		}
	}

	public void UpdateLauncherButtonStopAnim()
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready)
		{
			base.appLauncherButton.StopAnim();
		}
	}

	public void onGamePause()
	{
		gamePaused = true;
	}

	public void onGameUnPause()
	{
		gamePaused = false;
		if (!(ContractSystem.Instance != null))
		{
			return;
		}
		Dictionary<Guid, UICascadingList.CascadingListItem>.Enumerator enumerator = contractList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Contract contractByGuid = ContractSystem.Instance.GetContractByGuid(enumerator.Current.Key);
			if (contractByGuid != null)
			{
				RefreshContractRequest(contractByGuid);
			}
		}
		enumerator.Dispose();
	}
}
