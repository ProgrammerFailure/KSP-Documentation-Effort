using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Grid]
public class UIPartActionInventory : UIPartActionFieldItem
{
	public UI_Grid gridControl;

	public TextMeshProUGUI inventoryNameText;

	public RectTransform contentTransform;

	public GridLayoutGroup gridLayout;

	public GameObject slotPrefab;

	public int fieldValue;

	public float partIconDepthDistance;

	public float iconSize;

	public List<UIPartActionInventorySlot> slotButton;

	public List<EditorPartIcon> slotPartIcon;

	public RectTransform currentSelectedItemPartIcon;

	public GameObject limitsFooter;

	public GameObject packedVolumeLimits;

	public Slider packedVolumeSlider;

	public TextMeshProUGUI packedVolumeText;

	public Image packedVolumeFillImage;

	public bool inCargoPane;

	public GameObject massLimits;

	public Slider massSlider;

	public TextMeshProUGUI massText;

	public Image massFillImage;

	[SerializeField]
	private Color colorImageVolume;

	[SerializeField]
	private Color colorImageMass;

	[SerializeField]
	private Color colorImageError;

	public ModuleInventoryPart inventoryPartModule;

	private AvailablePart ap;

	private Vector2 currentSelectedItemPartIconMovePos;

	internal bool parentWindowHovered;

	public bool AbleToPlaceParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionInventory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupConstruction(ModuleInventoryPart inventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModuleInventoryChanged(ModuleInventoryPart changedModuleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModuleInventorySlotChanged(ModuleInventoryPart inventory, int slotChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSlot(int slotChanged, bool updateStackUI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeSlots()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnEmptySlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnLoadedPart(StoredPart sp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyHeldPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyHeldPart(bool fromConstructionController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyItem(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAllSlotsNotSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartPartPlacement(EditorPartIcon partIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetPlacePartIcons(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePlacePartIcons(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowGridObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HideGridObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleGridObject(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void InitializeMassVolumeDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateVolumeLimits(float currentPackedVolume, float maxPackedVolume, bool overLimit, bool constructionOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateMassLimits(float currentMass, float maxMass, bool overLimit)
	{
		throw null;
	}
}
