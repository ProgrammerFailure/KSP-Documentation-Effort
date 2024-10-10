using System.Collections.Generic;
using ns11;
using ns13;
using ns2;
using ns9;
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

	public float partIconDepthDistance = 39f;

	public float iconSize = 35f;

	public List<UIPartActionInventorySlot> slotButton = new List<UIPartActionInventorySlot>();

	public List<EditorPartIcon> slotPartIcon = new List<EditorPartIcon>();

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
	public Color colorImageVolume;

	[SerializeField]
	public Color colorImageMass;

	[SerializeField]
	public Color colorImageError;

	public ModuleInventoryPart inventoryPartModule;

	public AvailablePart ap;

	public Vector2 currentSelectedItemPartIconMovePos;

	public bool parentWindowHovered;

	public bool AbleToPlaceParts
	{
		get
		{
			if (inventoryPartModule != null)
			{
				return inventoryPartModule.AbleToPlaceParts;
			}
			return false;
		}
	}

	public void Awake()
	{
		slotButton = new List<UIPartActionInventorySlot>();
		slotPartIcon = new List<EditorPartIcon>();
	}

	public void OnDestroy()
	{
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
		GameEvents.onModuleInventorySlotChanged.Remove(OnModuleInventorySlotChanged);
		GameEvents.onGUIAstronautComplexDespawn.Remove(ShowGridObject);
		GameEvents.onGUIAstronautComplexSpawn.Remove(HideGridObject);
		if (UIPartActionControllerInventory.Instance != null)
		{
			if (currentSelectedItemPartIcon.childCount > 0)
			{
				UIPartActionControllerInventory.Instance.SetIconAsPart();
			}
			UIPartActionControllerInventory.Instance.ToggleCargoPartsLight(-1);
		}
		InputLockManager.RemoveControlLock("CargoPartHeld");
	}

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		inventoryPartModule = partModule as ModuleInventoryPart;
		GameEvents.onGUIAstronautComplexDespawn.Add(ShowGridObject);
		GameEvents.onGUIAstronautComplexSpawn.Add(HideGridObject);
		if (inventoryPartModule != null)
		{
			if (inventoryPartModule.IsKerbalOnEVA)
			{
				GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
				GameEvents.onVesselChange.Add(OnVesselChange);
			}
			InitializeMassVolumeDisplay();
		}
		GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
		GameEvents.onModuleInventorySlotChanged.Add(OnModuleInventorySlotChanged);
		if (scene == UI_Scene.Flight || scene == UI_Scene.Editor)
		{
			fieldValue = GetFieldValue();
			if (field != null)
			{
				inventoryNameText.text = field.guiName;
			}
			if (gridControl == null)
			{
				gridControl = (UI_Grid)control;
				gridControl.pawInventory = this;
			}
			InitializeSlots();
		}
	}

	public void SetupConstruction(ModuleInventoryPart inventoryPart)
	{
		inventoryPartModule = inventoryPart;
		partModule = inventoryPart;
		inventoryPartModule.constructorModeInventory = this;
		inCargoPane = true;
		InitializeMassVolumeDisplay();
		GameEvents.onGUIAstronautComplexDespawn.Add(ShowGridObject);
		GameEvents.onGUIAstronautComplexSpawn.Add(HideGridObject);
		if (inventoryPartModule != null)
		{
			if (inventoryPartModule.IsKerbalOnEVA)
			{
				GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
				GameEvents.onVesselChange.Add(OnVesselChange);
			}
			GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
			GameEvents.onModuleInventorySlotChanged.Add(OnModuleInventorySlotChanged);
			fieldValue = inventoryPartModule.InventorySlots;
		}
		if (inventoryPartModule.kerbalMode && inventoryPartModule.kerbalReference != null)
		{
			inventoryNameText.text = inventoryPartModule.kerbalReference.displayName;
		}
		else
		{
			inventoryNameText.text = inventoryPartModule.part.partInfo.title;
		}
		InitializeSlots();
	}

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		if (data.host.id == inventoryPartModule.vessel.id)
		{
			UpdatePlacePartIcons(data.to == Vessel.Situations.LANDED);
		}
	}

	public void OnVesselChange(Vessel vsl)
	{
		UpdatePlacePartIcons(vsl.id == inventoryPartModule.vessel.id);
	}

	public void OnModuleInventoryChanged(ModuleInventoryPart changedModuleInventoryPart)
	{
		if (!(changedModuleInventoryPart != inventoryPartModule))
		{
			for (int i = 0; i < slotPartIcon.Count; i++)
			{
				UpdateSlot(i, updateStackUI: false);
			}
		}
	}

	public void OnModuleInventorySlotChanged(ModuleInventoryPart inventory, int slotChanged)
	{
		if (!(inventoryPartModule != inventory))
		{
			UpdateSlot(slotChanged, updateStackUI: true);
		}
	}

	public void UpdateSlot(int slotChanged, bool updateStackUI)
	{
		string text = ((slotPartIcon[slotChanged].partInfo != null) ? slotPartIcon[slotChanged].partInfo.name : string.Empty);
		if (inventoryPartModule.storedParts.TryGetValue(slotChanged, out var val))
		{
			if (text != val.partName)
			{
				DestroyItem(slotChanged);
				SpawnLoadedPart(val);
			}
		}
		else if (!string.IsNullOrEmpty(text))
		{
			DestroyItem(slotChanged);
		}
		if (updateStackUI)
		{
			slotButton[slotChanged].UpdateStackUI();
		}
	}

	public int GetFieldValue()
	{
		return field.GetValue<int>(field.host);
	}

	public void InitializeSlots()
	{
		if (gridControl == null)
		{
			gridLayout.constraintCount = 3;
		}
		else
		{
			gridLayout.constraintCount = gridControl.columnCount;
		}
		if (UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.ToggleCargoPartsLight(1);
		}
		for (int i = 0; i < fieldValue; i++)
		{
			GameObject gameObject = Object.Instantiate(slotPrefab, contentTransform);
			gameObject.transform.SetParent(contentTransform);
			gameObject.transform.localPosition = Vector3.zero;
			slotPartIcon.Add(gameObject.GetComponent<EditorPartIcon>());
			UIPartActionInventorySlot item = gameObject.AddComponent<UIPartActionInventorySlot>();
			Object.Destroy(gameObject.GetComponent<PartListTooltipController>());
			gameObject.AddComponent<InventoryPartListTooltipController>().tooltipPrefab = UIPartActionControllerInventory.Instance.inventoryTooltipPrefab.GetComponent<InventoryPartListTooltip>();
			slotButton.Add(item);
			if (inventoryPartModule.storedParts != null && inventoryPartModule.storedParts.ContainsKey(i))
			{
				SpawnLoadedPart(inventoryPartModule.storedParts[i]);
			}
			else
			{
				SpawnEmptySlot(i);
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				PartListTooltipController component = gameObject.GetComponent<PartListTooltipController>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	public void SpawnEmptySlot(int slotIndex)
	{
		if (slotIndex > -1 && slotIndex <= slotButton.Count - 1)
		{
			slotButton[slotIndex].Setup(this, slotIndex);
		}
		DestroyItem(slotIndex);
	}

	public void SpawnLoadedPart(StoredPart sp)
	{
		if (sp.slotIndex > -1 && sp.slotIndex <= slotButton.Count - 1)
		{
			slotButton[sp.slotIndex].Setup(this, sp.slotIndex);
			if (!string.IsNullOrEmpty(sp.partName))
			{
				sp.partName = KSPUtil.SanitizeInstanceName(sp.partName);
				ap = PartLoader.getPartInfoByName(sp.partName);
				PartVariant variant = null;
				if (sp.variantName != "")
				{
					variant = ap.GetVariant(sp.variantName);
				}
				slotPartIcon[sp.slotIndex].Create(null, ap, sp, iconSize, 1f, 60f, StartPartPlacement, AbleToPlaceParts, skipVariants: false, variant, useImageThumbnail: true, inInventory: true);
				if (sp.quantity > 1)
				{
					slotButton[sp.slotIndex].SetStackedPartsInSlot(sp.slotIndex, sp.quantity);
				}
			}
			else
			{
				slotPartIcon[sp.slotIndex].SetEmptySlot();
			}
		}
		else
		{
			Debug.LogWarning("[UIPartActionInventory]: Invalid slotIndex passed Index: " + sp.slotIndex);
		}
	}

	public void DestroyHeldPart()
	{
		DestroyHeldPart(fromConstructionController: false);
	}

	public void DestroyHeldPart(bool fromConstructionController)
	{
		if (inCargoPane && !fromConstructionController)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				EVAConstructionModeController.Instance.DestroyHeldIcons(this);
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				InventoryPanelController.Instance.DestroyHeldIcons(this);
			}
		}
		int childCount = currentSelectedItemPartIcon.childCount;
		while (childCount-- > 0)
		{
			Object.Destroy(currentSelectedItemPartIcon.GetChild(childCount).gameObject);
		}
		if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon != null)
		{
			Object.Destroy(UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.gameObject);
			UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon = null;
		}
		InputLockManager.RemoveControlLock("CargoPartHeld");
	}

	public void DestroyItem(int index)
	{
		if (slotPartIcon.Count >= index && slotPartIcon[index] != null)
		{
			slotPartIcon[index].SetEmptySlot();
		}
	}

	public void Update()
	{
		if (base.Window != null)
		{
			parentWindowHovered = base.Window.Hover;
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			parentWindowHovered = EVAConstructionModeController.Instance.Hover;
		}
		else
		{
			if (!HighLogic.LoadedSceneIsEditor)
			{
				return;
			}
			parentWindowHovered = InventoryPanelController.Instance.Hover;
		}
		if (parentWindowHovered)
		{
			if (!(UIPartActionController.Instance != null) || !UIPartActionController.Instance.InventoryAndCargoPartExist())
			{
				return;
			}
			if (UIPartActionControllerInventory.Instance.isSetAsPart)
			{
				Part currentCargoPart = UIPartActionControllerInventory.Instance.CurrentCargoPart;
				if (HighLogic.LoadedSceneIsEditor)
				{
					UIPartActionControllerInventory.Instance.editorPartPickedBlockSfx = true;
					UIPartActionControllerInventory.Instance.editorPartDroppedBlockSfx = true;
					EditorLogic.fetch.ReleasePartToIcon();
					UIPartActionControllerInventory.Instance.CurrentCargoPart = currentCargoPart;
				}
				if (window == null || !window.displayDirty)
				{
					UIPartActionControllerInventory.Instance.SetPartAsIcon(currentSelectedItemPartIcon.transform, currentCargoPart);
				}
				currentCargoPart.highlighter.ConstantOffImmediate();
			}
			if (!UIPartActionControllerInventory.Instance.isSetAsPart)
			{
				if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon != null)
				{
					RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMasterController.Instance.actionCanvas.transform as RectTransform, Input.mousePosition, UIMainCamera.Camera, out currentSelectedItemPartIconMovePos);
					UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.transform.localPosition = currentSelectedItemPartIconMovePos;
				}
				else
				{
					RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, UIMainCamera.Camera, out currentSelectedItemPartIconMovePos);
				}
				currentSelectedItemPartIcon.anchoredPosition = currentSelectedItemPartIconMovePos;
				UIPartActionControllerInventory.Instance.isHeldPartOutOfActionWindows = false;
			}
		}
		else if ((HighLogic.LoadedSceneIsEditor && currentSelectedItemPartIcon.childCount > 0) || (HighLogic.LoadedSceneIsEditor && UIPartActionControllerInventory.Instance != null && !UIPartActionControllerInventory.Instance.isSetAsPart) || (HighLogic.LoadedSceneIsFlight && UIPartActionControllerInventory.Instance != null && !UIPartActionControllerInventory.Instance.IsCursorOverAnyPAWOrCargoPane))
		{
			UIPartActionControllerInventory.Instance.SetIconAsPart();
			UIPartActionControllerInventory.Instance.isHeldPartOutOfActionWindows = true;
			if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon != null)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMasterController.Instance.actionCanvas.transform as RectTransform, Input.mousePosition, UIMainCamera.Camera, out currentSelectedItemPartIconMovePos);
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.transform.localPosition = currentSelectedItemPartIconMovePos;
			}
		}
	}

	public void SetAllSlotsNotSelected()
	{
		for (int i = 0; i < slotButton.Count; i++)
		{
			slotButton[i].UpdateCurrentSelectedSlot(isCurrent: false);
		}
	}

	public void StartPartPlacement(EditorPartIcon partIcon)
	{
		int num = -1;
		for (int i = 0; i < slotPartIcon.Count; i++)
		{
			if (slotPartIcon[i] == partIcon)
			{
				num = i;
				break;
			}
		}
		if (num > -1 && partModule != null)
		{
			ModuleInventoryPart moduleInventoryPart = partModule as ModuleInventoryPart;
			if (moduleInventoryPart != null)
			{
				UpdatePlacePartIcons(active: false);
				moduleInventoryPart.DeployInventoryItem(num);
			}
		}
	}

	public void ResetPlacePartIcons(bool active)
	{
		if (active)
		{
			if (inventoryPartModule != null && inventoryPartModule.vessel != null && inventoryPartModule.AbleToPlaceParts)
			{
				UpdatePlacePartIcons(active: true);
			}
			else
			{
				UpdatePlacePartIcons(active: false);
			}
		}
		else
		{
			UpdatePlacePartIcons(active: false);
		}
	}

	public void UpdatePlacePartIcons(bool active)
	{
		for (int i = 0; i < slotPartIcon.Count; i++)
		{
			if (slotPartIcon[i].btnPlacePart != null)
			{
				slotPartIcon[i].btnPlacePart.gameObject.SetActive(active && slotPartIcon[i].HasIconOrThumbnail && slotPartIcon[i].IsDeployablePart && (!EVAConstructionModeController.Instance || !EVAConstructionModeController.Instance.IsOpen));
			}
		}
	}

	public void ShowGridObject()
	{
		ToggleGridObject(toggle: true);
	}

	public void HideGridObject()
	{
		ToggleGridObject(toggle: false);
	}

	public void ToggleGridObject(bool toggle)
	{
		if (gridLayout != null && gridLayout.gameObject != null)
		{
			gridLayout.gameObject.SetActive(toggle);
		}
	}

	public void InitializeMassVolumeDisplay()
	{
		if (limitsFooter != null)
		{
			limitsFooter.SetActive(inventoryPartModule.HasMassLimit || inventoryPartModule.HasPackedVolumeLimit);
		}
		if (packedVolumeLimits != null)
		{
			packedVolumeLimits.SetActive(inventoryPartModule.HasPackedVolumeLimit);
		}
		if (massLimits != null)
		{
			massLimits.SetActive(inventoryPartModule.HasMassLimit);
		}
		if (inventoryPartModule != null)
		{
			UpdateVolumeLimits(inventoryPartModule.volumeCapacity, inventoryPartModule.packedVolumeLimit, inventoryPartModule.volumeCapacity > inventoryPartModule.packedVolumeLimit, constructionOnly: false);
			UpdateMassLimits(inventoryPartModule.massCapacity, inventoryPartModule.massLimit, inventoryPartModule.massCapacity > inventoryPartModule.massLimit);
		}
	}

	public void UpdateVolumeLimits(float currentPackedVolume, float maxPackedVolume, bool overLimit, bool constructionOnly)
	{
		if (packedVolumeSlider != null)
		{
			packedVolumeSlider.maxValue = maxPackedVolume;
			packedVolumeSlider.value = currentPackedVolume;
		}
		if (packedVolumeText != null)
		{
			if (constructionOnly)
			{
				packedVolumeText.text = Localizer.Format("#autoLOC_6002642");
			}
			else
			{
				packedVolumeText.text = Localizer.Format("#autoLOC_8003412", currentPackedVolume.ToString("0.0"), maxPackedVolume.ToString("0.0"));
			}
		}
		if (packedVolumeFillImage != null)
		{
			if (overLimit)
			{
				packedVolumeFillImage.color = colorImageError;
			}
			else
			{
				packedVolumeFillImage.color = colorImageVolume;
			}
		}
	}

	public void UpdateMassLimits(float currentMass, float maxMass, bool overLimit)
	{
		if (massSlider != null)
		{
			massSlider.maxValue = maxMass;
			massSlider.value = currentMass;
		}
		if (massText != null)
		{
			massText.text = Localizer.Format("#autoLOC_8003413", currentMass.ToString("0.000"), maxMass.ToString("0.000"));
		}
		if (massFillImage != null)
		{
			if (overLimit)
			{
				massFillImage.color = colorImageError;
			}
			else
			{
				massFillImage.color = colorImageMass;
			}
		}
	}
}
