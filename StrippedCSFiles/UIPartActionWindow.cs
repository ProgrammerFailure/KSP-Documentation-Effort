using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionWindow : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
	public enum DisplayType
	{
		Selected,
		ResourceOnly,
		ResourceSelected
	}

	private struct ListItemCache
	{
		public UIPartActionFieldItem listFieldItem;

		public UIPartActionButton listButton;

		public UIPartActionResource listResource;

		public UIPartActionResourceEditor listResourceEditor;

		public UIPartActionResourceTransfer listResourceTransfer;

		public UIPartActionResourcePriority listResourcePriority;

		public UIPartActionAeroDisplay listAeroDisplay;

		public UIPartActionThermalDisplay listThermalDisplay;

		public UIPartActionRoboticJointDisplay listRoboticJointDisplay;

		public UIPartActionFuelFlowOverlay listFuelFlowOverlay;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ListItemCache(UIPartActionItem item)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ClearReferences()
		{
			throw null;
		}
	}

	public LayerMask partLayerMask;

	public float zOffset;

	public RectTransform titleBar;

	public TextMeshProUGUI titleText;

	public VerticalLayoutGroup layoutGroup;

	public Toggle toggleNumeric;

	public Toggle togglePinned;

	public Material lineMaterial;

	public Color lineColor;

	public float lineCornerRadius;

	public float lineWidth;

	private bool _dragging;

	[SerializeField]
	private float screenEdgeOffset;

	private Vector2 screenEdgeOffsetVector;

	[SerializeField]
	private RectTransform windowTransform;

	private Vector2 windowDimensions;

	[SerializeField]
	private RectTransform titleBarTransform;

	[SerializeField]
	private RectTransform itemsContentTransform;

	private Vector2 itemsContentDimensions;

	private Vector2 contentAnchoredDimensions;

	[SerializeField]
	private ScrollRect itemsScrollRect;

	[SerializeField]
	private Scrollbar itemsScrollBar;

	[SerializeField]
	private float windowPixelMargin;

	private float windowHeight;

	private float maxWindowHeight;

	private float scrollBarWidth;

	private int Seed;

	private KSPRandom Generator;

	protected Part _part;

	[SerializeField]
	private bool _isValid;

	[SerializeField]
	private bool _displayDirty;

	[SerializeField]
	protected DisplayType displayType;

	private List<ListItemCache> listItemCache;

	private List<int> listItemPositions;

	protected List<UIPartActionItem> listItems;

	protected UI_Scene scene;

	protected RectTransform rectTransform;

	protected float previousUiScale;

	public bool PreviousResourceOnly;

	public bool usingNumericValue;

	public DictionaryValueList<string, UIPartActionGroup> parameterGroups;

	private Vector3 rootPartScreenPosition;

	private Vector3 onSidePosition;

	public List<UIPartActionColorPicker> colorPickers;

	protected int controlIndex;

	protected UIWorldPointer pointer;

	protected bool pinned;

	protected bool hover;

	protected bool numericSliders;

	public bool dragging
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool displayDirty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public DisplayType Display
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float targetDistanceFromCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public List<UIPartActionItem> ListItems
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIWorldPointer Pointer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Pinned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool NumericSliders
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Setup(Part part, DisplayType type, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCacheVariables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateWindowHeight(float newHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CreatePartList(bool clearFirst)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateField(BaseField fld, Part p, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFieldItem TrySetFieldControl(BaseField field, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveFieldControl(BaseField field, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFieldControl(BaseField field, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ProtoCrewMember> GetPartCrew(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddCrewInventory(List<ProtoCrewMember> crewMembers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddCrewInventory(ProtoCrewMember crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveCrewInventory(Part crewPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKerbalEVA(GameEvents.FromToAction<Part, Part> fv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorShipCrewModified(VesselCrewManifest vcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateEvent(BaseEvent evt, Part p, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionButton TrySetEventControl(BaseEvent evt, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveEventControl(BaseEvent evt, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEventControl(BaseEvent evt, Part part, PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddParameterGroup(UIPartActionItem pawFieldItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddGroup(Transform t, BasePAWGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddGroup(Transform t, string groupName, bool startCollapsed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveGroup(string groupName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupResourceControls(PartResource r, bool clearFirst, UI_Scene scene, ref int controlIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateResource(PartResource rsrc, Part p, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResource TrySetResourceControlFlight(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceEditor TrySetResourceControlEditor(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceTransfer TrySetResourceTransferControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResourceControlFlight(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResourceControlEditor(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResourceTransferControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddResourceTransferControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddResourceFlightControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddResourceEditorControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsResourceControl(PartResource r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsResourceTransferControl(PartResource resource, out UIPartActionResourceTransfer control)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshResourceTransferTargets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateAeroDisplay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionAeroDisplay TrySetAeroControl(Part r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddAeroControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveAeroControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateThermalDisplay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionThermalDisplay TrySetThermalControl(Part r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddThermalControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveThermalControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateRoboticJointDisplay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionRoboticJointDisplay TrySetRoboticJointControl(Part r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddRoboticJointControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveRoboticJointControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateResourcePriorityDisplay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourcePriority TrySetResourcePriorityControl(Part r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddResourcePriorityControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveResourcePriorityControl(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanActivateFuelFlowOverlay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionFuelFlowOverlay TrySetFuelFlowOverlay(Part r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFuelFlowOverlay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveFuelFlowOverlay(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasKerbalInventory(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RemoveItemAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddItem(UIPartActionItem item, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecyclePartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseAllColorPickers(UIPartActionColorPicker caller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResizeWindowDimensions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateColorPickers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PointerUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TrackPartPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RepositionWindow(Transform partTransform, Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPin(bool pinned)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnNumericSwap(bool numeric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNumericSwapped(bool numeric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool NumericInputModeAndFocusedItem()
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
	public bool GetPartHover()
	{
		throw null;
	}
}
