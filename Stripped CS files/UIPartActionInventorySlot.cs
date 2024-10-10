using Highlighting;
using ns11;
using ns13;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionInventorySlot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	public enum actionTemp
	{
		EmptyStack,
		RemovePart,
		ClearPart
	}

	public int slotIndex;

	public UIPartActionInventory inventoryPartActionUI;

	public AvailablePart cacheAvailablePart;

	public Image slotImage;

	public ModuleCargoPart cargoPartRef;

	public ModulePartVariants cargoPartVariant;

	public ModuleInventoryPart moduleInventoryPart;

	public InventoryPartListTooltipController tooltipController;

	public PointerEventData mouseEventDataOnEnter;

	public bool HaveStackingSpace => moduleInventoryPart.HasStackingSpace(slotIndex);

	public int CurrentStackedAmount => moduleInventoryPart.GetStackAmountAtSlot(slotIndex);

	public int MaxStackedAmount => moduleInventoryPart.GetStackCapacityAtSlot(slotIndex);

	public bool IsStackable => moduleInventoryPart.IsStackable(slotIndex);

	public bool CanStackInSlot(AvailablePart part, string partVariantName)
	{
		return moduleInventoryPart.CanStackInSlot(part, partVariantName, slotIndex);
	}

	public void Setup(UIPartActionInventory inventoryRef, int index)
	{
		inventoryPartActionUI = inventoryRef;
		moduleInventoryPart = inventoryPartActionUI.inventoryPartModule;
		slotIndex = index;
		slotImage = GetComponent<Image>();
		tooltipController = GetComponent<InventoryPartListTooltipController>();
		GameEvents.onEditorPartDeleted.Add(OnEditorPartDeleted);
		GameEvents.OnEVACargoMode.Add(OnCargoModeToggled);
	}

	public void SlotClicked()
	{
		if ((EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.evaEditor != null && EVAConstructionModeController.Instance.evaEditor.EVAConstructionMode != 0) || UIPartActionController.Instance == null)
		{
			return;
		}
		if (UIPartActionController.Instance.InventoryAndCargoPartExist())
		{
			ProcessClickWithHeldPart();
		}
		else
		{
			if (tooltipController != null)
			{
				tooltipController.OnPointerExit(null);
			}
			ProcessClickNoHeldPart();
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			UIPartActionControllerInventory.Instance.CurrentInventory = moduleInventoryPart;
			tooltipController.OnPointerEnter(mouseEventDataOnEnter);
		}
	}

	public void ProcessClickWithHeldPart()
	{
		Part[] componentsInChildren = UIPartActionControllerInventory.Instance.CurrentCargoPart.GetComponentsInChildren<Part>();
		int num = componentsInChildren.Length;
		do
		{
			if (num-- > 0)
			{
				continue;
			}
			if (moduleInventoryPart.volumeCapacityReached || moduleInventoryPart.massCapacityReached || !(UIPartActionControllerInventory.Instance.CurrentCargoPart.GetComponent<ModuleCargoPart>().packedVolume >= 0f))
			{
				return;
			}
			cargoPartVariant = UIPartActionControllerInventory.Instance.CurrentCargoPart.GetComponent<ModulePartVariants>();
			string text = "";
			if (cargoPartVariant != null && cargoPartVariant.SelectedVariant != null)
			{
				text = cargoPartVariant.SelectedVariant.Name;
			}
			if (moduleInventoryPart.IsSlotEmpty(slotIndex))
			{
				StorePartInEmptySlot(UIPartActionControllerInventory.Instance.CurrentCargoPart, text);
				if (UIPartActionControllerInventory.heldPartIsStack)
				{
					SetStackedPartsInSlot(slotIndex, UIPartActionControllerInventory.stackSize, text);
					UIPartActionControllerInventory.heldPartIsStack = false;
					UIPartActionControllerInventory.stackSize = 0;
				}
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = null;
				UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied = null;
				inventoryPartActionUI.SetAllSlotsNotSelected();
				UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
				inventoryPartActionUI.DestroyHeldPart();
				return;
			}
			cacheAvailablePart = inventoryPartActionUI.slotPartIcon[slotIndex].partInfo;
			Part currentCargoPart = UIPartActionControllerInventory.Instance.CurrentCargoPart;
			int stackSize = UIPartActionControllerInventory.stackSize;
			string text2 = "";
			if (moduleInventoryPart.storedParts.ContainsKey(slotIndex))
			{
				text2 = moduleInventoryPart.storedParts[slotIndex].variantName;
			}
			UIPartActionInventory uIPartActionInventory = null;
			if ((bool)UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked)
			{
				uIPartActionInventory = UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.inventoryPartActionUI;
			}
			if (moduleInventoryPart.CanStackInSlot(currentCargoPart.partInfo, text, slotIndex))
			{
				StackPartsInThisSlot(UIPartActionControllerInventory.Instance.CurrentCargoPart, text);
				if (uIPartActionInventory != null)
				{
					uIPartActionInventory.SetAllSlotsNotSelected();
				}
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = null;
				UIPartActionControllerInventory.Instance.PlayPartDroppedSFX();
				return;
			}
			inventoryPartActionUI.DestroyHeldPart();
			Part part = CreatePartFromThisSlot(slotIndex);
			EditorInventoryOnlyIcon editorInventoryOnlyIcon = CreatePartIconFromThisSlot(slotIndex);
			int currentStackedAmount = CurrentStackedAmount;
			if (CurrentStackedAmount > 1)
			{
				EmptyStack();
				UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied = this;
			}
			else
			{
				inventoryPartActionUI.inventoryPartModule.ClearPartAtSlot(slotIndex);
				inventoryPartActionUI.slotPartIcon[slotIndex].SetEmptySlot();
			}
			inventoryPartActionUI.slotPartIcon[slotIndex].Create(null, currentCargoPart.partInfo, null, inventoryPartActionUI.iconSize, 1f, 1f, inventoryPartActionUI.StartPartPlacement, inventoryPartActionUI.AbleToPlaceParts, skipVariants: false, (currentCargoPart.variants != null) ? currentCargoPart.variants.SelectedVariant : null, useImageThumbnail: true, inInventory: true);
			inventoryPartActionUI.inventoryPartModule.StoreCargoPartAtSlot(currentCargoPart, slotIndex);
			SetStackedPartsInSlot(slotIndex, stackSize, text);
			if (text2 != "" && part.variants != null)
			{
				part.variants.SetVariant(text2);
			}
			UIPartActionControllerInventory.stackSize = currentStackedAmount;
			UIPartActionControllerInventory.heldPartIsStack = currentStackedAmount > 1;
			UIPartActionControllerInventory.Instance.CurrentCargoPart = part;
			if (editorInventoryOnlyIcon != null)
			{
				editorInventoryOnlyIcon.SetStackAmount(UIPartActionControllerInventory.stackSize);
				editorInventoryOnlyIcon.ToggleBackgroundVisibility(UIPartActionControllerInventory.stackSize > 1);
			}
			if (uIPartActionInventory != null && uIPartActionInventory != inventoryPartActionUI)
			{
				uIPartActionInventory.SetAllSlotsNotSelected();
			}
			else if ((bool)UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked)
			{
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
			}
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = null;
			UIPartActionControllerInventory.Instance.PlayPartDroppedSFX();
			inventoryPartActionUI.SetAllSlotsNotSelected();
			return;
		}
		while (componentsInChildren[num].persistentId == UIPartActionControllerInventory.Instance.CurrentCargoPart.persistentId);
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6005091"), 5f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void ProcessClickNoHeldPart()
	{
		if (moduleInventoryPart.IsSlotEmpty(slotIndex))
		{
			return;
		}
		UpdateCurrentSelectedSlot(isCurrent: true);
		Part currentCargoPart = CreatePartFromThisSlot(slotIndex);
		EditorInventoryOnlyIcon editorInventoryOnlyIcon = null;
		if (CurrentStackedAmount > 1)
		{
			if (GameSettings.MODIFIER_KEY.GetKey())
			{
				UIPartActionControllerInventory.stackSize = CurrentStackedAmount;
				UIPartActionControllerInventory.heldPartIsStack = true;
				UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied = this;
				editorInventoryOnlyIcon = CreatePartIconFromThisSlot(slotIndex);
				EmptyStack();
			}
			else
			{
				UIPartActionControllerInventory.stackSize = 1;
				UIPartActionControllerInventory.heldPartIsStack = false;
				editorInventoryOnlyIcon = CreatePartIconFromThisSlot(slotIndex);
				RemovePartFromStack(slotIndex);
			}
		}
		else
		{
			UIPartActionControllerInventory.stackSize = 1;
			UIPartActionControllerInventory.heldPartIsStack = false;
			editorInventoryOnlyIcon = CreatePartIconFromThisSlot(slotIndex);
			inventoryPartActionUI.inventoryPartModule.ClearPartAtSlot(slotIndex);
		}
		UIPartActionControllerInventory.Instance.CurrentCargoPart = currentCargoPart;
		if (editorInventoryOnlyIcon != null)
		{
			editorInventoryOnlyIcon.SetStackAmount(UIPartActionControllerInventory.stackSize);
		}
		InputLockManager.SetControlLock(ControlTypes.UI_DRAGGING, "CargoPartHeld");
	}

	public void EmptyStack()
	{
		if (UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied = this;
		}
		inventoryPartActionUI.inventoryPartModule.ClearPartAtSlot(slotIndex);
		ClearEmptiedSlot();
	}

	public void ClearEmptiedSlot(int prevStackAmount = 1)
	{
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied == this)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied = null;
			inventoryPartActionUI.slotPartIcon[slotIndex].SetEmptySlot();
			SetStackedPartsInSlot(slotIndex, 1);
		}
	}

	public void StorePartInEmptySlot(Part partToStore, string variantName)
	{
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked == null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = this;
		}
		else
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
		}
		cargoPartRef = partToStore.GetComponent<ModuleCargoPart>();
		moduleInventoryPart.StoreCargoPartAtSlot(partToStore, slotIndex);
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied != null)
		{
			if (UIPartActionControllerInventory.heldPartIsStack)
			{
				SetStackedPartsInSlot(slotIndex, UIPartActionControllerInventory.stackSize, variantName);
				UIPartActionControllerInventory.stackSize = 0;
				UIPartActionControllerInventory.heldPartIsStack = false;
			}
			else
			{
				_ = CurrentStackedAmount;
				SetStackedPartsInSlot(slotIndex, UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied.CurrentStackedAmount, variantName);
			}
		}
		int howMany = ((!GameSettings.MODIFIER_KEY.GetKey() || !HighLogic.LoadedSceneIsEditor) ? CurrentStackedAmount : cargoPartRef.stackableQuantity);
		SetStackedPartsInSlot(slotIndex, howMany, variantName);
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.inventoryPartActionUI.DestroyHeldPart();
		}
		inventoryPartActionUI.DestroyHeldPart();
		UIPartActionControllerInventory.Instance.PlayPartDroppedSFX();
		UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
	}

	public bool StackPartsInThisSlot(Part heldPart, string partVariantName)
	{
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked == null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = this;
		}
		else
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
		}
		cargoPartRef = heldPart.GetComponent<ModuleCargoPart>();
		if ((UIPartActionControllerInventory.heldPartIsStack && UIPartActionControllerInventory.stackSize > 1) || GameSettings.MODIFIER_KEY.GetKey())
		{
			int num = ((!GameSettings.MODIFIER_KEY.GetKey() || !HighLogic.LoadedSceneIsEditor) ? UIPartActionControllerInventory.stackSize : (UIPartActionControllerInventory.Instance.CurrentModuleCargoPart.stackableQuantity - CurrentStackedAmount));
			while (num > 0 && HaveStackingSpace)
			{
				StackPartsInSlot(slotIndex, partVariantName, destroyHeldPart: false);
				num--;
			}
			UIPartActionControllerInventory.stackSize = num;
			UIPartActionControllerInventory.heldPartIsStack = num > 1;
			if (num == 0)
			{
				inventoryPartActionUI.DestroyHeldPart();
				UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
			}
			else if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon != null)
			{
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.SetStackAmount(num);
			}
			return true;
		}
		if (HaveStackingSpace && heldPart.Resources.Count == 0)
		{
			StackPartsInSlot(slotIndex, partVariantName);
			inventoryPartActionUI.DestroyHeldPart();
			UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
			return true;
		}
		return false;
	}

	public void StackPartsInSlot(int slot, string variantName = "", bool destroyHeldPart = true)
	{
		if (GameSettings.MODIFIER_KEY.GetKey() && HighLogic.LoadedSceneIsEditor)
		{
			moduleInventoryPart.UpdateStackAmountAtSlot(slot, MaxStackedAmount, variantName);
		}
		else
		{
			moduleInventoryPart.UpdateStackAmountAtSlot(slot, CurrentStackedAmount + 1, variantName);
		}
		if (destroyHeldPart && UIPartActionControllerInventory.stackSize == 0)
		{
			inventoryPartActionUI.DestroyHeldPart();
		}
	}

	public void SetStackedPartsInSlot(int slot, int howMany, string variantName = "")
	{
		moduleInventoryPart.UpdateStackAmountAtSlot(slot, howMany, variantName);
	}

	public void RemovePartFromStack(int slot, string variantName = "")
	{
		moduleInventoryPart.UpdateStackAmountAtSlot(slot, CurrentStackedAmount - 1, variantName);
	}

	public void UpdateStackUI()
	{
		inventoryPartActionUI.slotPartIcon[slotIndex].UpdateStackUI(IsStackable, CurrentStackedAmount, MaxStackedAmount);
	}

	public Part CreatePartFromThisSlot(int slotIndex)
	{
		Part part = null;
		UIPartActionControllerInventory.Instance.PlayPartSelectedSFX();
		UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = this;
		UIPartActionControllerInventory.Instance.CurrentInventory = inventoryPartActionUI.inventoryPartModule;
		if (slotIndex > -1 && moduleInventoryPart.storedParts.ContainsKey(slotIndex))
		{
			ProtoPartSnapshot snapshot = inventoryPartActionUI.inventoryPartModule.storedParts[slotIndex].snapshot;
			part = UIPartActionControllerInventory.Instance.CreatePartFromInventory(snapshot);
		}
		if (part == null)
		{
			return null;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			Quaternion selectedPartOriginalRotation = Quaternion.identity;
			if (HighLogic.LoadedSceneIsFlight && FlightGlobals.fetch != null && FlightGlobals.fetch.activeVessel != null)
			{
				switch (FlightGlobals.fetch.activeVessel.situation)
				{
				case Vessel.Situations.LANDED:
				case Vessel.Situations.PRELAUNCH:
				case Vessel.Situations.FLYING:
				case Vessel.Situations.DOCKED:
				{
					CelestialBody mainBody = FlightGlobals.fetch.activeVessel.mainBody;
					Vector3 position = FlightGlobals.fetch.activeVessel.transform.position;
					selectedPartOriginalRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(mainBody.position + (Vector3d)mainBody.transform.up * mainBody.Radius - position, (position - mainBody.position).normalized).normalized, (position - mainBody.position).normalized);
					break;
				}
				}
			}
			EVAConstructionModeController.Instance.evaEditor.selectedPartOriginalRotation = selectedPartOriginalRotation;
		}
		if (part.GetComponent<ModuleCargoPart>() != null)
		{
			cargoPartRef = part.GetComponent<ModuleCargoPart>();
		}
		if (!string.IsNullOrEmpty(inventoryPartActionUI.inventoryPartModule.storedParts[slotIndex].variantName))
		{
			ModulePartVariants modulePartVariants = part.FindModuleImplementing<ModulePartVariants>();
			if (modulePartVariants != null)
			{
				modulePartVariants.SetVariant(inventoryPartActionUI.inventoryPartModule.storedParts[slotIndex].variantName);
			}
		}
		UIPartActionControllerInventory.Instance.SetPartAsIcon(inventoryPartActionUI.currentSelectedItemPartIcon, part);
		if (!part.IsUnderConstructionWeightLimit() && EVAConstructionModeController.Instance != null && !EVAConstructionModeController.Instance.Hover && !UIPartActionControllerInventory.Instance.IsCursorOverAnyPAWOrCargoPane)
		{
			part.SetHighlightColor(Highlighter.colorPartEditorDetached);
			part.SetHighlight(active: true, recursive: true);
		}
		part.isAttached = false;
		return part;
	}

	public EditorInventoryOnlyIcon CreatePartIconFromThisSlot(int slotIndex)
	{
		UIPartActionControllerInventory.Instance.PlayPartSelectedSFX();
		UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked = this;
		UIPartActionControllerInventory.Instance.CurrentInventory = inventoryPartActionUI.inventoryPartModule;
		if (slotIndex > -1 && moduleInventoryPart.storedParts.ContainsKey(slotIndex))
		{
			_ = inventoryPartActionUI.inventoryPartModule.storedParts[slotIndex].snapshot;
			EditorPartIcon editorPartIcon = inventoryPartActionUI.slotPartIcon[slotIndex];
			if (UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon == null)
			{
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon = Object.Instantiate(UIPartActionControllerInventory.Instance.InventoryOnlyIconPrefab, UIMasterController.Instance.actionCanvas.transform).GetComponent<EditorInventoryOnlyIcon>();
			}
			EditorInventoryOnlyIcon currentInventoryOnlyIcon = UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon;
			currentInventoryOnlyIcon.SetIconTexture(editorPartIcon.inventoryItemThumbnail.texture);
			currentInventoryOnlyIcon.SetStackAmount(UIPartActionControllerInventory.stackSize);
			if (HighLogic.LoadedSceneIsEditor || (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen))
			{
				currentInventoryOnlyIcon.ToggleBackgroundVisibility(UIPartActionControllerInventory.stackSize > 1);
			}
			return currentInventoryOnlyIcon;
		}
		return null;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		mouseEventDataOnEnter = eventData;
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			UIPartActionControllerInventory.Instance.DestroyTooltip();
			SlotClicked();
		}
	}

	public void ReturnHeldPartToThisSlot()
	{
		UIPartActionControllerInventory.Instance.PlayPartDroppedSFX();
		SlotClicked();
	}

	public void UpdateCurrentSelectedSlot(bool isCurrent)
	{
		if (UIPartActionController.Instance != null)
		{
			if (UIPartActionController.Instance.InventoryAndCargoPartExist() && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked != this)
			{
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
			}
			if (slotImage != null)
			{
				slotImage.sprite = (isCurrent ? UIPartActionControllerInventory.Instance.slot_partItem_current : UIPartActionControllerInventory.Instance.slot_partItem_normal);
			}
		}
		UpdateStackUI();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!(inventoryPartActionUI == null) && slotIndex <= moduleInventoryPart.InventorySlots - 1 && !(UIPartActionControllerInventory.Instance == null))
		{
			cacheAvailablePart = inventoryPartActionUI.slotPartIcon[slotIndex].partInfo;
			UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered = this;
			moduleInventoryPart.showPreview = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (UIPartActionControllerInventory.Instance.CurrentCargoPart != null)
		{
			moduleInventoryPart.RestoreLimitsDisplay();
		}
		UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered = null;
		moduleInventoryPart.showPreview = false;
	}

	public void OnEditorPartDeleted(Part part)
	{
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotEmptied.ClearEmptiedSlot();
		}
		if (UIPartActionControllerInventory.heldPartIsStack)
		{
			UIPartActionControllerInventory.heldPartIsStack = false;
			UIPartActionControllerInventory.stackSize = 0;
		}
	}

	public void OnCargoModeToggled(bool opened)
	{
		if (!opened && HighLogic.LoadedSceneIsFlight && UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked == this)
		{
			UIPartActionControllerInventory.Instance.ReturnHeldPart();
		}
	}

	public void OnDestroy()
	{
		if (HighLogic.LoadedSceneIsFlight && UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked == this)
		{
			if (UIPartActionControllerInventory.Instance.currentPawAutoOpening)
			{
				UIPartActionControllerInventory.Instance.currentPawAutoOpening = false;
				return;
			}
			UIPartActionControllerInventory.Instance.ReturnHeldPart();
			UIPartActionControllerInventory.Instance.DestroyTooltip();
		}
		GameEvents.onEditorPartDeleted.Remove(OnEditorPartDeleted);
		GameEvents.OnEVACargoMode.Remove(OnCargoModeToggled);
	}
}
