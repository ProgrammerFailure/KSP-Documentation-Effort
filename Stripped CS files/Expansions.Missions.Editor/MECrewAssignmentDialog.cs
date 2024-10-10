using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Actions;
using ns16;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MECrewAssignmentDialog : BaseCrewAssignmentDialog
{
	public delegate void OkCallback(KerbalRoster crewRoster, VesselCrewManifest newCrew);

	public enum TabEnum
	{
		Applicants,
		CreateEdit
	}

	public enum CreateMode
	{
		[Description("#autoLOC_900341")]
		Create,
		[Description("#autoLOC_8006030")]
		Edit
	}

	public enum ManagementMode
	{
		Vessel,
		Global
	}

	public class VesselSituationManifest
	{
		public VesselSituation vesselSituation;

		public VesselCrewManifest vesselCrewManifest;
	}

	public GameObject availableCrewTab;

	public CrewCreationDialog crewCreationDialog;

	public UIList scrollListTemp;

	[SerializeField]
	public UIListItem widgetTitle;

	public int defaultRosterSize = 20;

	public ToggleGroup rosterToggleGroup;

	public OkCallback OnDialogOk;

	public Callback OnDismiss;

	[SerializeField]
	public Button btnCancel;

	[SerializeField]
	public Button btnOk;

	[SerializeField]
	public Toggle tabApplicants;

	[SerializeField]
	public Toggle tabCreateEdit;

	[SerializeField]
	public GameObject panelApplicants;

	[SerializeField]
	public GameObject panelCreateEdit;

	[SerializeField]
	public UISkinDefSO uiSkin;

	public UISkinDef skin;

	public PopupDialog window;

	public CrewListItem selectedEntry;

	public KerbalRoster _currentCrewRoster;

	public List<ProtoCrewMember> temporalKerbals;

	public TabEnum currentTab;

	public CreateMode currentCreateMode;

	public ManagementMode currentManagementMode;

	public List<VesselSituationManifest> vesselSituationManifests;

	public List<ActionCreateKerbal> createKerbalActions;

	public MECrewAssignmentDialog Spawn(MissionCraft vessel, List<Crew> vesselCrew, OkCallback onDialogOk, Callback onDismiss)
	{
		MECrewAssignmentDialog component = UnityEngine.Object.Instantiate(this).GetComponent<MECrewAssignmentDialog>();
		component.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		component.OnDialogOk = onDialogOk;
		component.OnDismiss = onDismiss;
		component.CurrentCrewRoster = MissionEditorLogic.Instance.EditorMission.situation.crewRoster;
		component.listManifest = VesselCrewManifest.FromConfigNode(vessel.CraftNode);
		component.vesselSituationManifests = new List<VesselSituationManifest>();
		component.listManifest.AddCrewMembers(ref vesselCrew, component.CurrentCrewRoster);
		component.currentManagementMode = ManagementMode.Vessel;
		return component;
	}

	public MECrewAssignmentDialog Spawn(OkCallback onDialogOk, Callback onDismiss)
	{
		MECrewAssignmentDialog component = UnityEngine.Object.Instantiate(this).GetComponent<MECrewAssignmentDialog>();
		component.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		component.OnDialogOk = onDialogOk;
		component.OnDismiss = onDismiss;
		component.CurrentCrewRoster = MissionEditorLogic.Instance.EditorMission.situation.crewRoster;
		component.vesselSituationManifests = new List<VesselSituationManifest>();
		List<VesselSituation> allVesselSituations = MissionEditorLogic.Instance.EditorMission.GetAllVesselSituations();
		component.createKerbalActions = MissionEditorLogic.Instance.EditorMission.GetAllActionModules<ActionCreateKerbal>();
		int i = 0;
		for (int count = allVesselSituations.Count; i < count; i++)
		{
			VesselSituationManifest vesselSituationManifest = new VesselSituationManifest();
			vesselSituationManifest.vesselSituation = allVesselSituations[i];
			MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(vesselSituationManifest.vesselSituation.persistentId);
			if (craftBySituationsVesselID != null)
			{
				vesselSituationManifest.vesselCrewManifest = VesselCrewManifest.FromConfigNode(craftBySituationsVesselID.CraftNode);
				vesselSituationManifest.vesselCrewManifest.AddCrewMembers(ref vesselSituationManifest.vesselSituation.vesselCrew, component.CurrentCrewRoster);
			}
			component.vesselSituationManifests.Add(vesselSituationManifest);
		}
		component.currentManagementMode = ManagementMode.Global;
		return component;
	}

	public void Start()
	{
		skin = uiSkin.SkinDef;
		temporalKerbals = new List<ProtoCrewMember>();
		crewCreationDialog.Setup(OnCrewMemberCreate, OnCrewMemberCreateDialogCancelled, temporalKerbals, _currentCrewRoster);
		btnCancel.onClick.AddListener(OnButtonCancel);
		btnOk.onClick.AddListener(OnButtonOk);
		scrollListTemp.onDrop.AddListener(DropOnTempList);
		tabApplicants.onValueChanged.AddListener(OnTabApplicantsPress);
		tabCreateEdit.onValueChanged.AddListener(OnTabCreateEditPress);
		if (currentManagementMode == ManagementMode.Vessel)
		{
			RefreshCrewLists(listManifest, setAsDefault: false, updateUI: true);
		}
		else if (currentManagementMode == ManagementMode.Global)
		{
			RefreshCrewLists(vesselSituationManifests, setAsDefault: false, updateUI: true);
		}
		CreateTempKerbalList();
		scrollListCrew.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1f;
		scrollListAvail.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1f;
		scrollListTemp.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1f;
	}

	public override void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
		_currentCrewRoster = newRoster;
	}

	public override KerbalRoster GetCurrentCrewRoster()
	{
		return _currentCrewRoster;
	}

	public void OnButtonCancel()
	{
		Dismiss();
	}

	public void OnButtonOk()
	{
		if (currentManagementMode == ManagementMode.Vessel)
		{
			if (OnDialogOk != null)
			{
				OnDialogOk(MissionEditorLogic.Instance.EditorMission.situation.crewRoster, GetManifest());
			}
		}
		else if (currentManagementMode == ManagementMode.Global)
		{
			SaveGlobalChanges();
			if (OnDialogOk != null)
			{
				OnDialogOk(base.CurrentCrewRoster, null);
			}
		}
		Dismiss();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Dismiss()
	{
		if (OnDismiss != null)
		{
			OnDismiss();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void RemoveCrewFromRoster(CrewListItem entry)
	{
		base.CurrentCrewRoster.Remove(entry.GetCrewRef());
		RefreshCrewLists(listManifest, setAsDefault: false, updateUI: true);
	}

	public void CreateTempKerbalList()
	{
		scrollListTemp.Clear(destroyElements: true);
		for (int i = temporalKerbals.Count; i < defaultRosterSize; i++)
		{
			temporalKerbals.Add(CrewGenerator.RandomCrewMemberPrototype(ProtoCrewMember.KerbalType.Applicant));
		}
		int j = 0;
		for (int count = temporalKerbals.Count; j < count; j++)
		{
			AddAvailItem(temporalKerbals[j], out var item, scrollListTemp, CrewListItem.ButtonTypes.X2);
			if (item != null)
			{
				item.SetKerbal(temporalKerbals[j], CrewListItem.KerbalTypes.RECRUIT);
			}
		}
	}

	public void OnCrewSelectionChanged(CrewListItem entry)
	{
		if (entry != null)
		{
			ChangeKerbalEditorMode(CreateMode.Edit, entry);
		}
		else
		{
			ChangeKerbalEditorMode(CreateMode.Create, entry);
		}
	}

	public void OnCrewEntrySelected(bool status)
	{
		if (status)
		{
			CrewListItem componentInParent = new List<Toggle>(rosterToggleGroup.ActiveToggles())[0].GetComponentInParent<CrewListItem>();
			OnCrewSelectionChanged(componentInParent);
			selectedEntry = componentInParent;
		}
		else if (new List<Toggle>(rosterToggleGroup.ActiveToggles()).Count == 0)
		{
			OnCrewSelectionChanged(null);
			selectedEntry = null;
		}
	}

	public void CleanSelection()
	{
		rosterToggleGroup.SetAllTogglesOff();
		OnCrewSelectionChanged(null);
	}

	public void DropOnTempList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		if (!(fromList == scrollListAvail))
		{
			return;
		}
		CrewListItem component = insertItem.GetComponent<CrewListItem>();
		ProtoCrewMember crewRef = component.GetCrewRef();
		if (!crewRef.isHero)
		{
			scrollListAvail.RemoveItem(insertItem);
			scrollListTemp.InsertItem(insertItem, insertIndex, forceZ: true, worldPositionStays: true);
			if (crewRef.type != ProtoCrewMember.KerbalType.Tourist)
			{
				crewRef.type = ProtoCrewMember.KerbalType.Applicant;
			}
			crewRef.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			component.SetButton(CrewListItem.ButtonTypes.const_0);
			component.SetKerbal(crewRef, (crewRef.type == ProtoCrewMember.KerbalType.Tourist) ? CrewListItem.KerbalTypes.TOURIST : CrewListItem.KerbalTypes.RECRUIT);
			temporalKerbals.Add(component.GetCrewRef());
			base.CurrentCrewRoster.Remove(component.GetCrewRef());
		}
	}

	public void ChangeKerbalEditorMode(CreateMode mode, CrewListItem entry)
	{
		if (currentCreateMode != mode)
		{
			currentCreateMode = mode;
			tabCreateEdit.GetComponentInChildren<TextMeshProUGUI>().text = mode.displayDescription();
			if (currentTab == TabEnum.CreateEdit)
			{
				if (currentCreateMode == CreateMode.Create)
				{
					crewCreationDialog.Show();
				}
				else
				{
					crewCreationDialog.Show(entry.GetCrewRef());
				}
			}
		}
		else if (currentTab == TabEnum.CreateEdit && currentCreateMode == CreateMode.Edit)
		{
			crewCreationDialog.Show(entry.GetCrewRef());
		}
	}

	public void RefreshCrewLists(List<VesselSituationManifest> vesselSituationManifests, bool setAsDefault, bool updateUI, Func<PartCrewManifest, bool> displayFilter = null)
	{
		if (updateUI)
		{
			CreateCrewList(vesselSituationManifests, displayFilter);
			CreateAvailList(vesselSituationManifests);
			listIsValid = true;
		}
		else
		{
			listIsValid = false;
		}
	}

	public void CreateCrewList(List<VesselSituationManifest> vesselSituationManifests, Func<PartCrewManifest, bool> displayFilter = null)
	{
		scrollListCrew.Clear(destroyElements: true);
		List<ProtoCrewMember> list = new List<ProtoCrewMember>();
		int i = 0;
		for (int count = vesselSituationManifests.Count; i < count; i++)
		{
			VesselCrewManifest vesselCrewManifest = vesselSituationManifests[i].vesselCrewManifest;
			if (vesselCrewManifest == null)
			{
				continue;
			}
			AddVesselTitle(vesselSituationManifests[i].vesselSituation.vesselName, XKCDColors.KSPMETitle, vesselSituationManifests[i].vesselSituation);
			int j = 0;
			for (int count2 = vesselCrewManifest.PartManifests.Count; j < count2; j++)
			{
				PartCrewManifest partCrewManifest = vesselCrewManifest.PartManifests[j];
				if (partCrewManifest.partCrew.Length == 0)
				{
					continue;
				}
				if (displayFilter != null && !displayFilter(partCrewManifest))
				{
					AddCrewListBorder("[DETACHED]" + partCrewManifest.PartInfo.title, XKCDColors.KSPNotSoGoodOrange);
				}
				else
				{
					AddCrewListBorder(partCrewManifest.PartInfo.title, XKCDColors.KSPBadassGreen);
				}
				int num = partCrewManifest.partCrew.Length;
				for (int k = 0; k < num; k++)
				{
					ProtoCrewMember protoCrewMember = base.CurrentCrewRoster[partCrewManifest.partCrew[k]];
					if (protoCrewMember == null)
					{
						AddCrewItemEmpty(partCrewManifest.PartID);
						continue;
					}
					AddCrewItem(protoCrewMember, partCrewManifest.PartID);
					list.Add(protoCrewMember);
				}
			}
			AddSpacer();
		}
		if (createKerbalActions != null)
		{
			int l = 0;
			for (int count3 = createKerbalActions.Count; l < count3; l++)
			{
				ActionCreateKerbal actionCreateKerbal = createKerbalActions[l];
				AddVesselTitle(actionCreateKerbal.node.Title, XKCDColors.KSPMETitle, actionCreateKerbal);
				AddCrewListBorder(actionCreateKerbal.location.GetNodeBodyParameterString(), XKCDColors.KSPBadassGreen, expandHeight: true);
				if (actionCreateKerbal.missionKerbal.Kerbal == null)
				{
					AddCrewItemEmpty(0u);
				}
				else
				{
					AddCrewItem(actionCreateKerbal.missionKerbal.Kerbal, 0u);
					list.Add(actionCreateKerbal.missionKerbal.Kerbal);
				}
				AddSpacer();
			}
		}
		IEnumerator<ProtoCrewMember> enumerator = base.CurrentCrewRoster.Kerbals(ProtoCrewMember.RosterStatus.Assigned).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!list.Contains(enumerator.Current))
			{
				ProtoCrewMember current = enumerator.Current;
				if (current != null)
				{
					current.rosterStatus = ProtoCrewMember.RosterStatus.Available;
				}
			}
		}
		enumerator.Dispose();
	}

	public void CreateAvailList(List<VesselSituationManifest> vesselSituationManifests)
	{
		scrollListAvail.Clear(destroyElements: true);
		IEnumerator<ProtoCrewMember> enumerator = base.CurrentCrewRoster.Kerbals(ProtoCrewMember.KerbalType.Crew, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool flag = false;
			int i = 0;
			for (int count = vesselSituationManifests.Count; i < count; i++)
			{
				if (vesselSituationManifests[i].vesselCrewManifest != null && vesselSituationManifests[i].vesselCrewManifest.Contains(enumerator.Current))
				{
					flag = true;
					break;
				}
			}
			int j = 0;
			for (int count2 = createKerbalActions.Count; j < count2; j++)
			{
				if (createKerbalActions[j].missionKerbal.Kerbal != null && createKerbalActions[j].missionKerbal.Kerbal.Equals(enumerator.Current))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				AddAvailItem(enumerator.Current);
			}
		}
		enumerator.Dispose();
		enumerator = base.CurrentCrewRoster.Kerbals(ProtoCrewMember.KerbalType.Tourist, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool flag2 = false;
			int k = 0;
			for (int count3 = vesselSituationManifests.Count; k < count3; k++)
			{
				if (vesselSituationManifests[k].vesselCrewManifest != null && vesselSituationManifests[k].vesselCrewManifest.Contains(enumerator.Current))
				{
					flag2 = true;
					break;
				}
			}
			int l = 0;
			for (int count4 = createKerbalActions.Count; l < count4; l++)
			{
				if (createKerbalActions[l].missionKerbal.Kerbal != null && createKerbalActions[l].missionKerbal.Kerbal.Equals(enumerator.Current))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				AddAvailItem(enumerator.Current);
			}
		}
		enumerator.Dispose();
	}

	public void AddVesselTitle(string text, Color textColor, object referenceObject)
	{
		UIListItem uIListItem = UnityEngine.Object.Instantiate(widgetTitle);
		uIListItem.GetComponent<UIStatePanel>().SetText("partname", text, textColor);
		uIListItem.GetComponent<UIStatePanel>().SetTextOverflowMode("partname", TextOverflowModes.Ellipsis);
		uIListItem.GetComponent<Button>().onClick.AddListener(delegate
		{
			OnGlobalTitleClick(referenceObject);
		});
		scrollListCrew.AddItem(uIListItem);
	}

	public void AddSpacer()
	{
		UIListItem component = new GameObject("ListItem_Spacer", typeof(UIListItem), typeof(LayoutElement)).GetComponent<UIListItem>();
		component.GetComponent<LayoutElement>().minHeight = 5f;
		scrollListCrew.AddItem(component);
	}

	public void SaveGlobalChanges()
	{
		int i = 0;
		for (int count = vesselSituationManifests.Count; i < count; i++)
		{
			if (vesselSituationManifests[i].vesselCrewManifest != null)
			{
				vesselSituationManifests[i].vesselCrewManifest = VesselCrewManifest.CloneOf(vesselSituationManifests[i].vesselCrewManifest, blank: true);
			}
		}
		int j = 0;
		for (int count2 = createKerbalActions.Count; j < count2; j++)
		{
			createKerbalActions[j].missionKerbal.Kerbal = null;
		}
		int num = 0;
		int num2 = 0;
		int k = 0;
		for (int count3 = scrollListCrew.Count; k < count3; k++)
		{
			CrewListItem component = scrollListCrew.GetUilistItemAt(k).GetComponent<CrewListItem>();
			if (component == null)
			{
				num = 0;
				continue;
			}
			if (component != null && component.GetCrewRef() != null)
			{
				bool flag = false;
				PartCrewManifest partCrewManifest = null;
				int l = 0;
				for (int count4 = vesselSituationManifests.Count; l < count4; l++)
				{
					if (vesselSituationManifests[l].vesselCrewManifest != null)
					{
						partCrewManifest = vesselSituationManifests[l].vesselCrewManifest.GetPartCrewManifest(component.pUid);
						if (partCrewManifest != null)
						{
							flag = true;
							partCrewManifest.AddCrewToSeat(component.GetCrewRef(), num);
							break;
						}
					}
				}
				if (!flag)
				{
					if (num2 == 0)
					{
						num2 = k;
					}
					createKerbalActions[(k - num2) / 4].missionKerbal.Kerbal = component.GetCrewRef();
				}
			}
			num++;
		}
		int m = 0;
		for (int count5 = vesselSituationManifests.Count; m < count5; m++)
		{
			if (vesselSituationManifests[m].vesselCrewManifest != null)
			{
				vesselSituationManifests[m].vesselSituation.SetVesselCrew(base.CurrentCrewRoster, vesselSituationManifests[m].vesselCrewManifest);
			}
		}
	}

	public void OnGlobalTitleClick(object referenceObject)
	{
		if (referenceObject is VesselSituation vesselSituation)
		{
			Guid nodeGuidByVesselID = MissionEditorLogic.Instance.EditorMission.GetNodeGuidByVesselID(vesselSituation.persistentId);
			MissionEditorLogic.Instance.EditorMission.nodes[nodeGuidByVesselID].guiNode.Select(deselectOtherNodes: true);
		}
		ActionCreateKerbal actionCreateKerbal = referenceObject as ActionCreateKerbal;
		if (actionCreateKerbal != null)
		{
			actionCreateKerbal.node.guiNode.Select(deselectOtherNodes: true);
		}
	}

	public override void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		base.DropOnCrewList(fromList, insertItem, insertIndex);
		CleanSelection();
	}

	public override void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		if (fromList == scrollListTemp)
		{
			CrewListItem insertItemCrew = insertItem.GetComponent<CrewListItem>();
			scrollListTemp.RemoveItem(insertItem);
			ProtoCrewMember crewRef = insertItemCrew.GetCrewRef();
			if (crewRef.type != ProtoCrewMember.KerbalType.Tourist)
			{
				crewRef.type = ProtoCrewMember.KerbalType.Crew;
			}
			crewRef.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			scrollListAvail.InsertItem(insertItem, insertIndex, forceZ: true, worldPositionStays: true);
			insertItemCrew.SetKerbalAsApplicableType(crewRef);
			insertItemCrew.SetButton(CrewListItem.ButtonTypes.const_1);
			temporalKerbals.Remove(insertItemCrew.GetCrewRef());
			base.CurrentCrewRoster.AddCrewMember(insertItemCrew.GetCrewRef());
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				Toggle componentInChildren = insertItemCrew.GetComponentInChildren<Toggle>(includeInactive: true);
				componentInChildren.group = rosterToggleGroup;
				componentInChildren.onValueChanged.AddListener(OnCrewEntrySelected);
			}));
		}
		else
		{
			base.DropOnAvailList(fromList, insertItem, insertIndex);
		}
		CleanSelection();
	}

	public override CrewListItem AddItem(uint pUid, UIList list, ProtoCrewMember crew)
	{
		CrewListItem crewListItem = base.AddItem(pUid, list, crew);
		Toggle componentInChildren = crewListItem.GetComponentInChildren<Toggle>();
		componentInChildren.group = rosterToggleGroup;
		componentInChildren.onValueChanged.AddListener(OnCrewEntrySelected);
		return crewListItem;
	}

	public override void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		base.MoveCrewToEmptySeat(fromlist, tolist, itemToMove, index);
		CleanSelection();
	}

	public override void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		CrewListItem component = itemToMove.GetComponent<CrewListItem>();
		_currentCrewRoster[component.kerbalName.text].rosterStatus = ProtoCrewMember.RosterStatus.Available;
		base.MoveCrewToAvail(fromlist, tolist, itemToMove);
		CleanSelection();
	}

	public override void ListItemButtonClick(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		if (scrollListTemp.Contains(clickItem.GetComponent<UIListItem>()))
		{
			temporalKerbals.Remove(clickItem.GetCrewRef());
			CreateTempKerbalList();
		}
		else
		{
			base.ListItemButtonClick(type, clickItem);
		}
	}

	public override void Refresh()
	{
		if (currentManagementMode == ManagementMode.Vessel)
		{
			RefreshCrewLists(listManifest, setAsDefault: false, updateUI: true);
		}
		else if (currentManagementMode == ManagementMode.Global)
		{
			RefreshCrewLists(vesselSituationManifests, setAsDefault: false, updateUI: true);
		}
	}

	public void OnTabApplicantsPress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.Applicants);
		}
	}

	public void OnTabCreateEditPress(bool state)
	{
		if (state)
		{
			SetTab(TabEnum.CreateEdit);
		}
	}

	public void SetTab(TabEnum newTab)
	{
		if (currentTab != newTab)
		{
			SetTabVisible(currentTab, state: false);
			currentTab = newTab;
			SetTabVisible(currentTab, state: true);
		}
	}

	public void SetTabVisible(TabEnum tab, bool state)
	{
		switch (currentTab)
		{
		case TabEnum.CreateEdit:
			panelCreateEdit.gameObject.SetActive(state);
			if (state)
			{
				if (currentCreateMode == CreateMode.Create)
				{
					crewCreationDialog.Show();
				}
				else
				{
					crewCreationDialog.Show(selectedEntry.GetCrewRef());
				}
			}
			break;
		case TabEnum.Applicants:
			panelApplicants.gameObject.SetActive(state);
			break;
		}
	}

	public void OnCrewMemberCreateDialogCancelled()
	{
	}

	public void OnCrewMemberCreate(ProtoCrewMember kerbal)
	{
		if (currentCreateMode == CreateMode.Create)
		{
			base.CurrentCrewRoster.AddCrewMember(kerbal);
			if (currentManagementMode == ManagementMode.Vessel)
			{
				RefreshCrewLists(listManifest, setAsDefault: false, updateUI: true);
			}
			else if (currentManagementMode == ManagementMode.Global)
			{
				RefreshCrewLists(vesselSituationManifests, setAsDefault: false, updateUI: true);
			}
		}
		else
		{
			RefreshCrewItem(selectedEntry, kerbal);
		}
	}

	public void RefreshCrewItem(CrewListItem cic, ProtoCrewMember crew)
	{
		cic.SetName(crew.name);
		cic.SetStats(crew);
		cic.SetXP(crew);
		cic.SetCrewRef(crew);
		cic.SetKerbalAsApplicableType(crew);
		cic.SetTooltip(crew);
	}

	public void PromptDeleteCrewMemberConfirm()
	{
		if (window != null)
		{
			window.Dismiss();
		}
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout();
		dialogGUIHorizontalLayout.AddChild(new DialogGUIFlexibleSpace());
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_129950") + "</color>", delegate
		{
			OnCrewMemberDeleteConfirm();
		}, 120f, 30f, true));
		dialogGUIHorizontalLayout.AddChild(new DialogGUIButton(Localizer.Format("#autoLOC_464291"), Show, 120f, 30f, true));
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout();
		dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8003084"), expandW: true));
		dialogGUIVerticalLayout.AddChild(dialogGUIHorizontalLayout);
		window = PopupDialog.SpawnPopupDialog(new MultiOptionDialog("Delete Crewmember", "", Localizer.Format("#autoLOC_8003083"), skin, 350f, dialogGUIVerticalLayout), persistAcrossScenes: false, skin);
		window.OnDismiss = OnButtonCancel;
	}

	public void OnCrewMemberDeleteConfirm()
	{
		RemoveCrewFromRoster(selectedEntry);
		OnCrewSelectionChanged(null);
	}
}
