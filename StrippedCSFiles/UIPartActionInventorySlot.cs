using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using KSP.UI.Screens.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionInventorySlot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private enum actionTemp
	{
		EmptyStack,
		RemovePart,
		ClearPart
	}

	public int slotIndex;

	public UIPartActionInventory inventoryPartActionUI;

	private AvailablePart cacheAvailablePart;

	private Image slotImage;

	private ModuleCargoPart cargoPartRef;

	private ModulePartVariants cargoPartVariant;

	internal ModuleInventoryPart moduleInventoryPart;

	private InventoryPartListTooltipController tooltipController;

	private PointerEventData mouseEventDataOnEnter;

	public bool HaveStackingSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CurrentStackedAmount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MaxStackedAmount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsStackable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionInventorySlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanStackInSlot(AvailablePart part, string partVariantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(UIPartActionInventory inventoryRef, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SlotClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessClickWithHeldPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessClickNoHeldPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EmptyStack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearEmptiedSlot(int prevStackAmount = 1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StorePartInEmptySlot(Part partToStore, string variantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool StackPartsInThisSlot(Part heldPart, string partVariantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StackPartsInSlot(int slot, string variantName = "", bool destroyHeldPart = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStackedPartsInSlot(int slot, int howMany, string variantName = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartFromStack(int slot, string variantName = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateStackUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part CreatePartFromThisSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private EditorInventoryOnlyIcon CreatePartIconFromThisSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReturnHeldPartToThisSlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCurrentSelectedSlot(bool isCurrent)
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
	private void OnEditorPartDeleted(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCargoModeToggled(bool opened)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
