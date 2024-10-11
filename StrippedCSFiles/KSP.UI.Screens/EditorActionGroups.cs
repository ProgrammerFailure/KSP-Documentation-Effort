using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Serenity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorActionGroups : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private enum ColumnDirection
	{
		Left,
		Right
	}

	[CompilerGenerated]
	private sealed class _003CToggleMenuNavigationInputLock_003Ed__130 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorActionGroups _003C_003E4__this;

		public bool state;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CToggleMenuNavigationInputLock_003Ed__130(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public int currentSelectedIndex;

	public bool interfaceActive;

	public UISelectableGridLayoutGroup actionGroupList;

	public UISelectableGridLayoutGroup groupActionsList;

	public UISelectableGridLayoutGroup partActionList;

	public TextMeshProUGUI GroupActionsHeading;

	public TextMeshProUGUI actionGroupColumnHeaderPrefab;

	private TextMeshProUGUI axisGroupHeader;

	private TextMeshProUGUI controllersHeader;

	public EditorActionOverrideGroup actionOverrideGroupHeaderPrefab;

	public EditorActionOverrideGroup separatorGroupHeaderPrefab;

	public EditorActionOverrideToggle actionOverrideTogglePrefab;

	public EditorActionGroup actionGroupPrefab;

	public EditorActionController actionControllerPrefab;

	public EditorActionPartItem groupPartTitlePrefab;

	public GameObject groupPartActionPrefab;

	public EditorActionPartItem partActionTitlePrefab;

	public EditorActionPartItem partActionTextPrefab;

	public EditorActionPartItem partActionPrefab;

	public EditorActionPartReset partActionResetPrefab;

	public EditorActionControllerHeader actionControllerHeaderPrefab;

	public EditorActionControllerOpenButton actionControllerOpenButtonPrefab;

	public Toggle additionalActionsToggle;

	public bool isMouseOver;

	private bool lockGroupOverride;

	private int[] overrideGroupIndices;

	private bool[] overrideGroupOpen;

	private int selectedGroupOverride;

	private uint selectedControllerId;

	private ModuleRoboticController selectedController;

	private int selectedGroup;

	private EditorActionGroupType selectedGroupType;

	private MenuNavigation menuNavigation;

	private List<Selectable> mainSelectables;

	private List<Selectable> actiongroupSelectables;

	private List<Selectable> groupActionsSelectables;

	private List<Selectable> selectionSelectables;

	private Button flightExitButton;

	private Selectable currentSelectable;

	private GameObject cachedCurrentSelected;

	private bool currentColumnFound;

	private int lastSelectedItemColumn;

	private int lastSelectedItemIndex;

	private List<EditorActionPartSelector> selectedParts;

	private List<ModuleRoboticController> cacheControllers;

	public static EditorActionGroups Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	private bool[] overrideDefault
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private KSPActionGroup[] overrideActionControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private KSPAxisGroup[] overrideAxisControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private bool OverrideDefault
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public KSPActionGroup SelectedGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public KSPAxisGroup SelectedAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorActionGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasSelectedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSelection(bool reconstruct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SelectionContains(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddToSelection(EditorActionPartSelector s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Part> GetSelectedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetPart(EditorActionPartSelector selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetPart(Part part, bool resetSym)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructActionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructLists(bool full)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<bool> CreateGroups_Action()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<bool> CreateGroups_Axis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<bool> CreateGroups_Controller(List<ModuleRoboticController> controllers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructGroupList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ModuleRoboticController> GetShipControllers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAxisGroupsHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupControllersHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructGroupActionList(int overrideGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructGroupAxisList(int overrideGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructPartActionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructPartActionList(bool combinedList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructPartAxisList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructPartAxisList(bool combinedList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToGroup(BaseAction action, KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToGroup(EditorActionPartItem item, KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToGroup(BaseAxisField axisField, KSPAxisGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToGroup(EditorActionPartItem item, KSPAxisGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToController(EditorActionPartItem item, ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddActionToGroup(EditorActionPartItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveFromGroup(BaseAction action, KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveFromGroup(EditorActionPartItem item, KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveFromGroup(BaseAxisField axisField, KSPAxisGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveFromGroup(EditorActionPartItem item, KSPAxisGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveFromController(EditorActionPartItem item, ModuleRoboticController controller, bool transferToSymPartner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveActionFromGroup(EditorActionPartItem item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseGroup(int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGroupOverride(int groupOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGroupOverrideState(int groupOverride, bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetControlActionOverrideState(int groupOverride, bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetControlAxisOverrideState(int groupOverride, bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateInterface(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivateInterface(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateInFlightInterface(Vessel ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DectivateInFlightInterface(Vessel ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorUndoRedo(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleAdditionalActions(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnControllerAxesOrActionsChanged(ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SelectController(ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RebuildLists(bool fullRebuild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RebuildLists(bool fullRebuild, bool keepSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeMenuNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateMenuNavigationSelectables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetExplicitNavigation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetColumnLastItemExplicitNavigation(List<Selectable> selList, Selectable lastSelectable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NavigateColumn(ColumnDirection dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectNextColumnItem(List<Selectable> targetList, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetCurrentSelectableColumn(Selectable item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetCurrentSelectableIndex(Selectable item, List<Selectable> selectablecolumn = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CacheCurrentSelectable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UnityAction<string> VesselNaming(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetMenuNavEnabled(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CToggleMenuNavigationInputLock_003Ed__130))]
	private IEnumerator ToggleMenuNavigationInputLock(bool state)
	{
		throw null;
	}
}
