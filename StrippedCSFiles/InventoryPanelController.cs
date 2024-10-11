using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanelController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private class InventoryDisplayItem
	{
		public ModuleInventoryPart inventoryModule;

		public GameObject uiObject;

		public UIPartActionInventory uiInventory;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InventoryDisplayItem()
		{
			throw null;
		}
	}

	public static InventoryPanelController Instance;

	private bool isOpen;

	protected bool hover;

	[Tooltip("The prefab we spawn for each inventory")]
	[SerializeField]
	private GameObject uiPartActionInventoryContainerPrefab;

	[Tooltip("The object that holds all the inventory GOs")]
	[SerializeField]
	private GameObject uiPartActionInventoryParent;

	private List<ModuleInventoryPart> moduleInventories;

	private List<InventoryDisplayItem> displayedInventories;

	private EditorPartListFilter<AvailablePart> cargoPartsFilter;

	private bool prevIsOpen;

	private List<ProtoCrewMember> crew;

	private InventoryDisplayItem displayItem;

	public bool IsOpen
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InventoryPanelController()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorScreenChange(EditorScreen screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorCrewModified(VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorShipModified(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInventoryLists(List<Part> parts, List<ProtoCrewMember> crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplayedInventories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetDisplayedInventory(ModuleInventoryPart inventoryModule, out InventoryDisplayItem displayItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyHeldIcons(UIPartActionInventory callingInventory)
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
}
