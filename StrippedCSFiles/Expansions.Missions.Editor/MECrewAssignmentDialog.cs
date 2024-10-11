using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Actions;
using KSP.UI;
using KSP.UI.Util;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselSituationManifest()
		{
			throw null;
		}
	}

	public GameObject availableCrewTab;

	public CrewCreationDialog crewCreationDialog;

	public UIList scrollListTemp;

	[SerializeField]
	protected UIListItem widgetTitle;

	public int defaultRosterSize;

	public ToggleGroup rosterToggleGroup;

	public OkCallback OnDialogOk;

	public Callback OnDismiss;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private Button btnOk;

	[SerializeField]
	private Toggle tabApplicants;

	[SerializeField]
	private Toggle tabCreateEdit;

	[SerializeField]
	private GameObject panelApplicants;

	[SerializeField]
	private GameObject panelCreateEdit;

	[SerializeField]
	protected UISkinDefSO uiSkin;

	protected UISkinDef skin;

	private PopupDialog window;

	protected CrewListItem selectedEntry;

	protected KerbalRoster _currentCrewRoster;

	private List<ProtoCrewMember> temporalKerbals;

	private TabEnum currentTab;

	private CreateMode currentCreateMode;

	private ManagementMode currentManagementMode;

	private List<VesselSituationManifest> vesselSituationManifests;

	private List<ActionCreateKerbal> createKerbalActions;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MECrewAssignmentDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MECrewAssignmentDialog Spawn(MissionCraft vessel, List<Crew> vesselCrew, OkCallback onDialogOk, Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MECrewAssignmentDialog Spawn(OkCallback onDialogOk, Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override KerbalRoster GetCurrentCrewRoster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonOk()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RemoveCrewFromRoster(CrewListItem entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateTempKerbalList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewSelectionChanged(CrewListItem entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewEntrySelected(bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CleanSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DropOnTempList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ChangeKerbalEditorMode(CreateMode mode, CrewListItem entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshCrewLists(List<VesselSituationManifest> vesselSituationManifests, bool setAsDefault, bool updateUI, Func<PartCrewManifest, bool> displayFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateCrewList(List<VesselSituationManifest> vesselSituationManifests, Func<PartCrewManifest, bool> displayFilter = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAvailList(List<VesselSituationManifest> vesselSituationManifests)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddVesselTitle(string text, Color textColor, object referenceObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSpacer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveGlobalChanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGlobalTitleClick(object referenceObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override CrewListItem AddItem(uint pUid, UIList list, ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ListItemButtonClick(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabApplicantsPress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTabCreateEditPress(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTab(TabEnum newTab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTabVisible(TabEnum tab, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewMemberCreateDialogCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewMemberCreate(ProtoCrewMember kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RefreshCrewItem(CrewListItem cic, ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PromptDeleteCrewMemberConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewMemberDeleteConfirm()
	{
		throw null;
	}
}
