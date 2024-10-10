using Contracts;
using Contracts.Agents;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class MissionControl : MonoBehaviour
{
	public enum DisplayMode
	{
		Available,
		Active,
		Archive
	}

	public enum ArchiveMode
	{
		All,
		Completed,
		Failed,
		Cancelled
	}

	public class MissionSelection
	{
		public bool isAvailable;

		public Contract contract;

		public UIListItem listItem;

		public MissionSelection(bool isAvailable, Contract contract, UIListItem listItem)
		{
			this.isAvailable = isAvailable;
			this.contract = contract;
			this.listItem = listItem;
		}
	}

	public RectTransform panelView;

	public MCListItem PrfbMissionListItem;

	public UIList scrollListContracts;

	public TextMeshProUGUI contractText;

	public ScrollRect contractTextRect;

	public RawImage logoRenderer;

	public TextMeshProUGUI textContractInfo;

	public TextMeshProUGUI textDateInfo;

	public TextMeshProUGUI textMCStats;

	public Button btnAccept;

	public Button btnDecline;

	public Button btnCancel;

	public Button btnAgentInfo;

	public Button btnAgentBack;

	public Toggle toggleDisplayModeAvailable;

	public Toggle toggleDisplayModeActive;

	public Toggle toggleDisplayModeArchive;

	public RectTransform toggleArchiveGroup;

	public Toggle toggleArchiveAll;

	public Toggle toggleArchiveCompleted;

	public Toggle toggleArchiveFailed;

	public Toggle toggleArchiveCancelled;

	public TextMeshProUGUI instructorText;

	public RawImage instructorRawImage;

	public KerbalInstructor instructorPrefab;

	public RenderTexture instructorTexture;

	public int instructorPortraitSize = 128;

	public KerbalInstructor instructor;

	public MCAvatarController avatarController;

	public float lastOnSelectSoundTrigger;

	public MissionSelection selectedMission;

	public bool isShowingAgentInformation;

	public int maxActiveContracts;

	public int activeContractCount;

	public DisplayMode displayMode;

	public ArchiveMode archiveMode;

	public static MissionControl Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		btnAccept.onClick.AddListener(OnClickAccept);
		btnDecline.onClick.AddListener(OnClickDecline);
		btnCancel.onClick.AddListener(OnClickCancel);
		btnAgentInfo.onClick.AddListener(OnClickAgentInfo);
		btnAgentBack.onClick.AddListener(OnClickAgentInfo);
		toggleDisplayModeAvailable.onValueChanged.AddListener(OnClickAvailable);
		toggleDisplayModeActive.onValueChanged.AddListener(OnClickActive);
		toggleDisplayModeArchive.onValueChanged.AddListener(OnClickArchive);
		toggleArchiveAll.onValueChanged.AddListener(OnClickArchiveAll);
		toggleArchiveCompleted.onValueChanged.AddListener(OnClickArchiveCompleted);
		toggleArchiveFailed.onValueChanged.AddListener(OnClickArchiveFailed);
		toggleArchiveCancelled.onValueChanged.AddListener(OnClickArchiveCancelled);
		lastOnSelectSoundTrigger = Time.realtimeSinceStartup;
	}

	public void Start()
	{
		panelView.gameObject.SetActive(value: false);
		maxActiveContracts = GameVariables.Instance.GetActiveContractsLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl));
		SetupInstructor();
		GameEvents.Contract.onContractsListChanged.Add(RefreshContracts);
		SetDisplayModeAvailable(force: true);
		UpdateInfoPanelContract(null);
		RefreshUIControls();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		GameEvents.Contract.onContractsListChanged.Remove(RefreshContracts);
		DestroyInstructor();
	}

	public void SetDisplayModeAvailable(bool force = false)
	{
		if (force || displayMode != 0)
		{
			displayMode = DisplayMode.Available;
			toggleArchiveGroup.gameObject.SetActive(value: false);
			RebuildContractList();
		}
	}

	public void SetDisplayModeActive(bool force = false)
	{
		if (force || displayMode != DisplayMode.Active)
		{
			displayMode = DisplayMode.Active;
			toggleArchiveGroup.gameObject.SetActive(value: false);
			RebuildContractList();
		}
	}

	public void SetDisplayModeArchive(bool force = false)
	{
		if (force || displayMode != DisplayMode.Archive)
		{
			displayMode = DisplayMode.Archive;
			toggleArchiveGroup.gameObject.SetActive(value: true);
			RebuildContractList();
		}
	}

	public void SetArchiveMode(ArchiveMode mode)
	{
		if (archiveMode != mode)
		{
			archiveMode = mode;
			RebuildContractList();
		}
	}

	public void OnClickAvailable(bool selected)
	{
		if (selected)
		{
			SetDisplayModeAvailable();
		}
	}

	public void OnClickActive(bool selected)
	{
		if (selected)
		{
			SetDisplayModeActive();
		}
	}

	public void OnClickArchive(bool selected)
	{
		if (selected)
		{
			SetDisplayModeArchive();
		}
	}

	public void OnClickArchiveAll(bool selected)
	{
		if (selected)
		{
			SetArchiveMode(ArchiveMode.All);
		}
	}

	public void OnClickArchiveCompleted(bool selected)
	{
		if (selected)
		{
			SetArchiveMode(ArchiveMode.Completed);
		}
	}

	public void OnClickArchiveFailed(bool selected)
	{
		if (selected)
		{
			SetArchiveMode(ArchiveMode.Failed);
		}
	}

	public void OnClickArchiveCancelled(bool selected)
	{
		if (selected)
		{
			SetArchiveMode(ArchiveMode.Cancelled);
		}
	}

	public void OnClickAccept()
	{
		selectedMission.contract.Accept();
		selectedMission = null;
		ClearInfoPanel();
		RebuildContractList();
		panelView.gameObject.SetActive(value: false);
		RefreshUIControls();
		UpdateInstructor(avatarController.animTrigger_accept, avatarController.animLoop_default);
	}

	public void OnClickDecline()
	{
		selectedMission.contract.Decline();
		selectedMission = null;
		ClearInfoPanel();
		RebuildContractList();
		panelView.gameObject.SetActive(value: false);
		RefreshUIControls();
		UpdateInstructor(avatarController.animTrigger_decline, avatarController.animLoop_default);
	}

	public void OnClickCancel()
	{
		selectedMission.contract.Cancel();
		selectedMission = null;
		ClearInfoPanel();
		RebuildContractList();
		panelView.gameObject.SetActive(value: false);
		RefreshUIControls();
		UpdateInstructor(avatarController.animTrigger_cancel, avatarController.animLoop_default);
	}

	public void OnClickAgentInfo()
	{
		if (selectedMission != null && selectedMission.contract != null && selectedMission.contract.Agent != null)
		{
			if (isShowingAgentInformation)
			{
				UpdateInfoPanelContract(selectedMission.contract);
			}
			else
			{
				UpdateInfoPanelAgent(selectedMission.contract.Agent);
			}
		}
	}

	public void OnSelectContract(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			panelView.gameObject.SetActive(value: true);
			selectedMission = button.GetComponent<UIListItem>().Data as MissionSelection;
			UpdateInfoPanelContract(selectedMission.contract);
			if (selectedMission.contract.Prestige == Contract.ContractPrestige.Exceptional)
			{
				UpdateInstructor(avatarController.animTrigger_selectHard, avatarController.animLoop_excited);
			}
			else if (selectedMission.contract.Prestige == Contract.ContractPrestige.Trivial)
			{
				UpdateInstructor(avatarController.animTrigger_selectEasy, avatarController.animLoop_default);
			}
			else
			{
				UpdateInstructor(avatarController.animTrigger_selectNormal, avatarController.animLoop_default);
			}
		}
	}

	public void OnDeselectContract(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			panelView.gameObject.SetActive(value: false);
			ClearInfoPanel();
			selectedMission = null;
		}
	}

	public void AddItem(Contract contract, bool isAvailable, string label = "")
	{
		MCListItem mCListItem = Object.Instantiate(PrfbMissionListItem);
		mCListItem.container.Data = new MissionSelection(isAvailable, contract, mCListItem.container);
		mCListItem.radioButton.onFalseBtn.AddListener(OnDeselectContract);
		mCListItem.radioButton.onTrueBtn.AddListener(OnSelectContract);
		if (label.Equals(""))
		{
			mCListItem.Setup(contract, "<color=#fefa87>" + contract.Title + "</color>");
		}
		else
		{
			mCListItem.Setup(contract, label);
		}
		scrollListContracts.AddItem(mCListItem.container);
		contract.SetViewed(Contract.Viewed.Seen);
	}

	public void UpdateInfoPanelContract(Contract contract)
	{
		isShowingAgentInformation = false;
		if (contract != null)
		{
			if (selectedMission.contract.ContractState == Contract.State.Offered)
			{
				btnAccept.gameObject.SetActive(value: true);
				btnDecline.gameObject.SetActive(value: true);
				btnCancel.gameObject.SetActive(value: false);
				btnAgentBack.gameObject.SetActive(value: false);
			}
			else if (selectedMission.contract.ContractState == Contract.State.Active)
			{
				btnAccept.gameObject.SetActive(value: false);
				btnDecline.gameObject.SetActive(value: false);
				btnCancel.gameObject.SetActive(value: true);
				btnAgentBack.gameObject.SetActive(value: false);
			}
			else
			{
				btnAccept.gameObject.SetActive(value: false);
				btnDecline.gameObject.SetActive(value: false);
				btnCancel.gameObject.SetActive(value: false);
				btnAgentBack.gameObject.SetActive(value: false);
			}
			string text = "";
			text += Localizer.Format("#autoLOC_467996");
			text += contract.Title;
			if (contract.Agent != null)
			{
				text += Localizer.Format("#autoLOC_468001");
				text += contract.Agent.Title;
				logoRenderer.texture = contract.Agent.Logo;
			}
			textContractInfo.text = text;
			contractText.text = contract.MissionControlTextRich();
			contractTextRect.verticalNormalizedPosition = 1f;
			if (contract.ContractState == Contract.State.Active)
			{
				if (contract.DateDeadline == 0.0)
				{
					textDateInfo.text = Localizer.Format("#autoLOC_468018");
				}
				else
				{
					string text2 = Localizer.Format("#autoLOC_468022");
					textDateInfo.text = "<b><color=#DB8310>" + text2 + "</color></b>\n" + ((!GameSettings.SHOW_DEADLINES_AS_DATES) ? KSPUtil.PrintDateDeltaCompact(Planetarium.fetch.time - contract.DateDeadline, includeTime: true, includeSeconds: true, useAbs: true) : KSPUtil.PrintDate(contract.DateDeadline, includeTime: true, includeSeconds: true));
				}
				btnCancel.interactable = selectedMission.contract.CanBeCancelled();
			}
			else if (contract.ContractState != Contract.State.Completed && contract.ContractState != Contract.State.Failed && contract.ContractState != Contract.State.Cancelled)
			{
				string text3 = Localizer.Format("#autoLOC_468036");
				if (contract.ContractState == Contract.State.Offered)
				{
					text3 = Localizer.Format("#autoLOC_468038");
				}
				if (contract.DateExpire == 0.0)
				{
					textDateInfo.text = Localizer.Format("#autoLOC_468042", text3);
				}
				else
				{
					textDateInfo.text = "<b><color=#DB8310>" + text3 + "</color></b>\n" + ((!GameSettings.SHOW_DEADLINES_AS_DATES) ? KSPUtil.PrintDateDeltaCompact(Planetarium.fetch.time - contract.DateExpire, includeTime: true, includeSeconds: true, useAbs: true) : KSPUtil.PrintDate(contract.DateExpire, includeTime: true, includeSeconds: true));
				}
				btnDecline.interactable = selectedMission.contract.CanBeDeclined();
			}
			else
			{
				textDateInfo.text = "";
			}
			contract.SetViewed(Contract.Viewed.Read);
		}
		else
		{
			btnAccept.gameObject.SetActive(value: false);
			btnDecline.gameObject.SetActive(value: false);
			btnCancel.gameObject.SetActive(value: false);
			btnAgentBack.gameObject.SetActive(value: false);
		}
	}

	public void UpdateInfoPanelAgent(Agent agent)
	{
		isShowingAgentInformation = true;
		if (agent != null)
		{
			btnAccept.gameObject.SetActive(value: false);
			btnDecline.gameObject.SetActive(value: false);
			btnCancel.gameObject.SetActive(value: false);
			btnAgentBack.gameObject.SetActive(value: true);
			string text = "";
			text += Localizer.Format("#autoLOC_468078");
			text += agent.Title;
			textContractInfo.text = text;
			logoRenderer.texture = agent.Logo;
			contractText.text = agent.DescriptionRich;
			contractTextRect.verticalNormalizedPosition = 1f;
		}
		else
		{
			btnAccept.gameObject.SetActive(value: false);
			btnDecline.gameObject.SetActive(value: false);
			btnCancel.gameObject.SetActive(value: false);
			btnAgentBack.gameObject.SetActive(value: false);
		}
	}

	public void ClearInfoPanel()
	{
		isShowingAgentInformation = false;
		btnAccept.gameObject.SetActive(value: false);
		btnDecline.gameObject.SetActive(value: false);
		btnCancel.gameObject.SetActive(value: false);
		btnAgentBack.gameObject.SetActive(value: false);
		logoRenderer.texture = null;
		textContractInfo.text = "";
		contractText.text = "Select a contract";
		btnAgentBack.gameObject.SetActive(value: false);
	}

	public void SetInstructorText(string text)
	{
		instructorText.text = Localizer.Format("#autoLOC_468120", text);
	}

	public void SetupInstructor()
	{
		instructor = Object.Instantiate(instructorPrefab);
		instructor.gameObject.transform.position = Vector3.zero;
		instructor.instructorCamera.targetTexture = instructorTexture;
		instructor.instructorCamera.ResetAspect();
		avatarController = instructor.gameObject.GetComponent<MCAvatarController>();
		instructorRawImage.texture = instructorTexture;
		avatarController.OnInstructorReady(delegate
		{
			avatarController.TriggerAnim(avatarController.animTrigger_accept, avatarController.animLoop_default);
		});
	}

	public void DestroyInstructor()
	{
		if (instructor != null)
		{
			Object.Destroy(instructor.gameObject);
		}
	}

	public void UpdateInstructor(MCAvatarController.AvatarState state, MCAvatarController.AvatarAnimation anim)
	{
		bool playSound;
		if (playSound = lastOnSelectSoundTrigger + 10f < Time.realtimeSinceStartup)
		{
			lastOnSelectSoundTrigger = Time.realtimeSinceStartup;
		}
		string text = avatarController.TriggerAnim(state, anim, playSound);
		SetInstructorText(text);
	}

	public void RefreshContracts()
	{
		RebuildContractList();
		RefreshUIControls();
	}

	public void RefreshUIControls()
	{
		activeContractCount = ContractSystem.Instance.GetActiveContractCount();
		if (activeContractCount < maxActiveContracts)
		{
			btnAccept.interactable = true;
			if (maxActiveContracts < int.MaxValue)
			{
				textMCStats.text = Localizer.Format("#autoLOC_468173", activeContractCount, maxActiveContracts);
			}
			else
			{
				textMCStats.text = Localizer.Format("#autoLOC_468177", activeContractCount);
			}
		}
		else
		{
			btnAccept.interactable = false;
			textMCStats.text = Localizer.Format("#autoLOC_468183", XKCDColors.HexFormat.Orange, activeContractCount, maxActiveContracts);
		}
	}

	public void RebuildContractList()
	{
		scrollListContracts.Clear(destroyElements: true);
		if (displayMode == DisplayMode.Available)
		{
			ContractSystem.Instance.Contracts.Sort((Contract a, Contract b) => RUIutils.SortAscDescPrimarySecondary(asc: true, a.Prestige.CompareTo(b.Prestige), a.Title.CompareTo(b.Title)));
			int i = 0;
			for (int count = ContractSystem.Instance.Contracts.Count; i < count; i++)
			{
				Contract contract = ContractSystem.Instance.Contracts[i];
				if (contract.ContractState == Contract.State.Offered)
				{
					AddItem(contract, isAvailable: true);
				}
			}
		}
		else if (displayMode == DisplayMode.Active)
		{
			ContractSystem.Instance.Contracts.Sort((Contract a, Contract b) => RUIutils.SortAscDescPrimarySecondary(asc: true, a.Prestige.CompareTo(b.Prestige), a.Title.CompareTo(b.Title)));
			for (int j = 0; j < ContractSystem.Instance.Contracts.Count; j++)
			{
				Contract contract2 = ContractSystem.Instance.Contracts[j];
				if (contract2.ContractState == Contract.State.Active)
				{
					AddItem(contract2, isAvailable: false);
				}
			}
		}
		else
		{
			if (displayMode != DisplayMode.Archive)
			{
				return;
			}
			ContractSystem.Instance.ContractsFinished.Sort((Contract a, Contract b) => RUIutils.SortAscDescPrimarySecondary(asc: true, a.DateFinished.CompareTo(b.DateFinished), a.Title.CompareTo(b.Title)));
			int k = 0;
			for (int count2 = ContractSystem.Instance.ContractsFinished.Count; k < count2; k++)
			{
				Contract contract3 = ContractSystem.Instance.ContractsFinished[k];
				if (contract3.ContractState == Contract.State.Completed && (archiveMode == ArchiveMode.All || archiveMode == ArchiveMode.Completed))
				{
					AddItem(contract3, isAvailable: false, "<color=#00ff00>" + Localizer.Format("#autoLOC_468222") + " </color> <color=#fefa87>" + contract3.Title + "</color>");
				}
				else if ((contract3.ContractState != Contract.State.Failed && contract3.ContractState != Contract.State.DeadlineExpired) || (archiveMode != 0 && archiveMode != ArchiveMode.Failed))
				{
					if (contract3.ContractState == Contract.State.Cancelled && (archiveMode == ArchiveMode.All || archiveMode == ArchiveMode.Cancelled))
					{
						AddItem(contract3, isAvailable: false, "<color=#aaaaaa>" + Localizer.Format("#autoLOC_468233") + "</color> <color=#fefa87>" + contract3.Title + "</color>");
					}
				}
				else if (contract3.ContractState == Contract.State.Failed)
				{
					AddItem(contract3, isAvailable: false, "<color=#ff0000>" + Localizer.Format("#autoLOC_468227") + "</color> <color=#fefa87>" + contract3.Title + "</color>");
				}
				else
				{
					AddItem(contract3, isAvailable: false, "<color=#ff0000>" + Localizer.Format("#autoLOC_468229") + "</color> <color=#fefa87>" + contract3.Title + "</color>");
				}
			}
		}
	}
}
