using System.Collections.Generic;
using ns11;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanelController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public class InventoryDisplayItem
	{
		public ModuleInventoryPart inventoryModule;

		public GameObject uiObject;

		public UIPartActionInventory uiInventory;
	}

	public static InventoryPanelController Instance;

	public bool isOpen;

	public bool hover;

	[Tooltip("The prefab we spawn for each inventory")]
	[SerializeField]
	public GameObject uiPartActionInventoryContainerPrefab;

	[Tooltip("The object that holds all the inventory GOs")]
	[SerializeField]
	public GameObject uiPartActionInventoryParent;

	public List<ModuleInventoryPart> moduleInventories;

	public List<InventoryDisplayItem> displayedInventories;

	public EditorPartListFilter<AvailablePart> cargoPartsFilter;

	public bool prevIsOpen;

	public List<ProtoCrewMember> crew;

	public InventoryDisplayItem displayItem;

	public bool IsOpen => isOpen;

	public bool Hover => hover;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		moduleInventories = new List<ModuleInventoryPart>();
		displayedInventories = new List<InventoryDisplayItem>();
		cargoPartsFilter = new EditorPartListFilter<AvailablePart>("cargoPartsFilter", (AvailablePart aPart) => PartLoader.Instance.GetAvailableAndPurchaseableCargoParts().Contains(aPart), "Not a Cargo Part");
		prevIsOpen = isOpen;
	}

	public void Start()
	{
		GameEvents.onEditorShipModified.Add(OnEditorShipModified);
		GameEvents.onEditorShipCrewModified.Add(OnEditorCrewModified);
		GameEvents.onEditorScreenChange.Add(OnEditorScreenChange);
	}

	public void OnDestroy()
	{
		GameEvents.onEditorShipModified.Remove(OnEditorShipModified);
		GameEvents.onEditorShipCrewModified.Remove(OnEditorCrewModified);
		GameEvents.onEditorScreenChange.Remove(OnEditorScreenChange);
	}

	public void Update()
	{
		if (isOpen)
		{
			UpdateDisplayedInventories();
		}
	}

	public void Reset()
	{
		OnEditorScreenChange(EditorLogic.fetch.editorScreen);
	}

	public void OnEditorScreenChange(EditorScreen screen)
	{
		isOpen = screen == EditorScreen.Cargo;
		if (isOpen == prevIsOpen)
		{
			return;
		}
		prevIsOpen = IsOpen;
		if (!(EditorPartList.Instance != null))
		{
			return;
		}
		if (isOpen)
		{
			EditorPartList.Instance.ExcludeFilters.AddFilter(cargoPartsFilter);
		}
		else
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(cargoPartsFilter);
			moduleInventories.Clear();
			int count = displayedInventories.Count;
			while (count-- > 0)
			{
				if (displayedInventories[count].uiObject != null)
				{
					Object.Destroy(displayedInventories[count].uiObject);
				}
			}
			displayedInventories.Clear();
		}
		EditorPartList.Instance.Refresh();
	}

	public void OnEditorCrewModified(VesselCrewManifest manifest)
	{
		if (manifest != null)
		{
			crew = manifest.GetAllCrew(includeNulls: false);
		}
		if (EditorLogic.fetch != null && EditorLogic.fetch.ship != null)
		{
			UpdateInventoryLists(EditorLogic.fetch.ship.parts, crew);
		}
	}

	public void OnEditorShipModified(ShipConstruct ship)
	{
		if (ShipConstruction.ShipManifest != null)
		{
			crew = ShipConstruction.ShipManifest.GetAllCrew(includeNulls: false);
		}
		if (ship != null)
		{
			UpdateInventoryLists(ship.parts, crew);
		}
	}

	public void UpdateInventoryLists(List<Part> parts, List<ProtoCrewMember> crew)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			ModuleInventoryPart moduleInventoryPart = parts[i].FindModuleImplementing<ModuleInventoryPart>();
			if (moduleInventoryPart != null && !moduleInventories.Contains(moduleInventoryPart))
			{
				moduleInventories.Add(moduleInventoryPart);
			}
		}
		if (ShipConstruction.ShipManifest != null)
		{
			crew = ShipConstruction.ShipManifest.GetAllCrew(includeNulls: false);
			for (int j = 0; j < crew.Count; j++)
			{
				ModuleInventoryPart kerbalInventoryModule = crew[j].KerbalInventoryModule;
				if (kerbalInventoryModule != null && !moduleInventories.Contains(kerbalInventoryModule))
				{
					moduleInventories.Add(kerbalInventoryModule);
				}
			}
		}
		int count = moduleInventories.Count;
		while (count-- > 0)
		{
			bool flag = false;
			for (int k = 0; k < parts.Count; k++)
			{
				ModuleInventoryPart moduleInventoryPart2 = parts[k].FindModuleImplementing<ModuleInventoryPart>();
				if (moduleInventoryPart2 != null && moduleInventories[count] == moduleInventoryPart2)
				{
					flag = true;
					break;
				}
			}
			if (crew != null)
			{
				for (int l = 0; l < crew.Count; l++)
				{
					ModuleInventoryPart kerbalInventoryModule2 = crew[l].KerbalInventoryModule;
					if (kerbalInventoryModule2 != null && moduleInventories[count] == kerbalInventoryModule2)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				moduleInventories.RemoveAt(count);
			}
		}
		for (int m = 0; m < displayedInventories.Count; m++)
		{
			if (displayedInventories[m].uiInventory != null)
			{
				displayedInventories[m].uiInventory.SetAllSlotsNotSelected();
			}
		}
	}

	public void UpdateDisplayedInventories()
	{
		bool flag = false;
		for (int i = 0; i < moduleInventories.Count; i++)
		{
			ModuleInventoryPart inventoryModule = moduleInventories[i];
			if (TryGetDisplayedInventory(inventoryModule, out displayItem))
			{
				continue;
			}
			displayItem = new InventoryDisplayItem();
			displayItem.inventoryModule = inventoryModule;
			displayItem.uiObject = Object.Instantiate(uiPartActionInventoryContainerPrefab);
			if (displayItem.uiObject != null)
			{
				displayItem.uiObject.transform.SetParent(uiPartActionInventoryParent.transform);
				displayItem.uiObject.transform.localPosition = Vector3.zero;
				displayItem.uiObject.transform.localScale = Vector3.one;
				displayItem.uiInventory = displayItem.uiObject.GetComponentInChildren<UIPartActionInventory>(includeInactive: true);
				if (displayItem.uiInventory != null)
				{
					displayItem.uiInventory.SetupConstruction(moduleInventories[i]);
				}
			}
			displayedInventories.Add(displayItem);
			flag = true;
		}
		int count = displayedInventories.Count;
		while (count-- > 0)
		{
			if (!moduleInventories.Contains(displayedInventories[count].inventoryModule))
			{
				Object.Destroy(displayedInventories[count].uiObject);
				displayedInventories.RemoveAt(count);
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		int num = 0;
		for (int j = 0; j < displayedInventories.Count; j++)
		{
			if (displayedInventories[j].inventoryModule.kerbalMode)
			{
				displayedInventories[j].uiObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
		for (int k = 0; k < displayedInventories.Count; k++)
		{
			if (!displayedInventories[k].inventoryModule.kerbalMode)
			{
				displayedInventories[k].uiObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	public bool TryGetDisplayedInventory(ModuleInventoryPart inventoryModule, out InventoryDisplayItem displayItem)
	{
		int num = 0;
		while (true)
		{
			if (num < displayedInventories.Count)
			{
				if (displayedInventories[num].inventoryModule == inventoryModule)
				{
					break;
				}
				num++;
				continue;
			}
			displayItem = null;
			return false;
		}
		displayItem = displayedInventories[num];
		return true;
	}

	public void DestroyHeldIcons(UIPartActionInventory callingInventory)
	{
		for (int i = 0; i < displayedInventories.Count; i++)
		{
			if (displayedInventories[i].uiInventory != callingInventory)
			{
				displayedInventories[i].uiInventory.DestroyHeldPart(fromConstructionController: true);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hover = false;
	}
}
