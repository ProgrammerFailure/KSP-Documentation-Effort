using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EVAConstructionModeController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
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

	public enum PanelMode
	{
		Construction,
		Cargo
	}

	public static EVAConstructionModeController Instance;

	public EVAConstructionModeEditor evaEditor;

	public EVAConstructionToolsUI evaToolsUI;

	[SerializeField]
	private LayerMask markerCamCullingMask;

	private Camera markerCam;

	[SerializeField]
	private UIPanelTransition constructionModeTransition;

	[SerializeField]
	private UIPanelTransition navballTransition;

	private bool navballPreviouslyOpen;

	private bool isOpen;

	private KerbalEVA activeEVA;

	[SerializeField]
	private GameObject uiPartActionInventoryContainerPrefab;

	[SerializeField]
	private GameObject uiPartActionInventoryParent;

	private Dictionary<uint, ModuleInventoryPart> loadedModuleInventoryPart;

	private List<InventoryDisplayItem> displayedInventories;

	[SerializeField]
	private RectTransform partList;

	private Vector2 partListVector;

	[SerializeField]
	private RectTransform footerConstruction;

	[SerializeField]
	private RectTransform footerCargo;

	protected bool hover;

	private ApplicationLauncherButton applauncherConstruction;

	private ApplicationLauncherButton applauncherCargo;

	[SerializeField]
	private Button exitButton;

	[SerializeField]
	private TooltipController_Text exitTooltipText;

	[SerializeField]
	private RectTransform exitButtonOriginalPos;

	[SerializeField]
	public PanelMode panelMode;

	public TextMeshProUGUI AssistingKerbalsLabel;

	public TextMeshProUGUI AssistingKerbalsNumber;

	public TextMeshProUGUI MaxMassLimitLabel;

	public TextMeshProUGUI MaxMassLimitNumber;

	private double constructionGravity;

	private double lastConstructionGravity;

	private InventoryDisplayItem displayItem;

	private Vector3 maxVector;

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

	public static bool MovementRestricted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EVAConstructionModeController()
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
	private void SetAppLauncherButtonVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RegisterAppButtonConstruction(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RegisterAppButtonCargo(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OpenConstructionPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OpenCargoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClosePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanOpenConstructionPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanOpenCargoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType eventType, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveInvDisplayItem(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnActionGroupsOpened()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchForInventoryParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplayedInventories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInfoLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyHeldIcons(UIPartActionInventory callingInventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryGetDisplayedInventory(ModuleInventoryPart inventoryModule, out InventoryDisplayItem displayItem)
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
	private void SpawnMarkerCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoadedScene(GameScenes loadedScene)
	{
		throw null;
	}
}
