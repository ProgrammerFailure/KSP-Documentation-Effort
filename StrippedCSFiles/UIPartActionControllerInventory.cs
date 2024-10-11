using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class UIPartActionControllerInventory : MonoBehaviour
{
	public static UIPartActionControllerInventory Instance;

	public static bool heldPartIsStack;

	public static int stackSize;

	public static int amountToStore;

	[SerializeField]
	private UIPartActionController pawController;

	protected ModuleInventoryPart currentInventory;

	protected UIPartActionInventorySlot currentInventorySlotClicked;

	protected UIPartActionInventorySlot currentInventorySlotHovered;

	protected UIPartActionInventorySlot currentInventorySlotEmptied;

	[Obsolete("Not in use")]
	public bool currentPartSnapping;

	[Obsolete("Not in use")]
	public bool hoveredPartIsKerbal;

	public bool currentPartIsAttachable;

	protected Part currentCargoPart;

	protected Part exchangedCargoPart;

	private uint currentHoveredInventoryPart;

	public GameObject inventoryTooltipPrefab;

	protected UIPartActionWindow currentPawAutoOpened;

	internal bool currentPawAutoOpening;

	public bool IsCursorOverAnyPAWOrCargoPane;

	[SerializeField]
	private float cargoPartDepthOffset;

	[SerializeField]
	private float partScaleMultiplier;

	[SerializeField]
	private float partScale;

	public bool isHeldPartOutOfActionWindows;

	public bool referenceNextCreatedInventoryPAW;

	public Sprite slot_partItem_normal;

	public Sprite slot_partItem_current;

	[SerializeField]
	private GameObject flight_TooltipPrefab;

	private GameObject currentTooltip;

	private bool currentCargoExists;

	private Vector3 partOriginalScale;

	private List<string> availableCargoParts;

	[HideInInspector]
	public List<string> availableInventoryParts;

	private float currentDistanceFromPart;

	public bool editorPartPickedBlockSfx;

	public bool editorPartDroppedBlockSfx;

	public Shader noAmbientHue;

	public Shader noAmbientHueBumped;

	[SerializeField]
	private Light cargoPartsLight;

	private int cargoInventoriesOpen;

	private bool enableCargoLight;

	private Vector2 heldPartPosition;

	private Vector3 heldPartNewPosition;

	public float partIconDepthDistance;

	public float iconHeight;

	public bool isSetAsPart;

	public GameObject InventoryOnlyIconPrefab;

	[HideInInspector]
	public EditorInventoryOnlyIcon CurrentInventoryOnlyIcon;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip pickupPart;

	[SerializeField]
	private AudioClip droppedPart;

	public ModuleInventoryPart CurrentInventory
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

	public UIPartActionInventorySlot CurrentInventorySlotClicked
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

	public UIPartActionInventorySlot CurrentInventorySlotHovered
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

	public ModuleInventoryPart CurrentInventorySlotHoveredModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIPartActionInventorySlot CurrentInventorySlotEmptied
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

	public Part CurrentCargoPart
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

	public ModuleCargoPart CurrentModuleCargoPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Not in use")]
	public Part ExchangedCargoPart
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

	[Obsolete("Not in use")]
	public uint CurrentHoveredInventoryPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIPartActionWindow CurrentPawAutoOpened
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionControllerInventory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UIPartActionControllerInventory()
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
	public bool IsKerbalWithinRange(ModuleInventoryPart inventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCargoPartConstructionEvent(ConstructionEventType construction, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetInventoryCacheValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetInventoryCacheValues(bool destroyCurrentCargoPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnInventoryPartHover(uint partId, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateCursorOverPAWs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCurrentCargoPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MoveSelectedPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 dragOverPlane()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReturnHeldPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDestroy(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateFlightTooltip(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleCargoPartsLight(int counter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayPartSelectedSFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayPartDroppedSFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAudioVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraChange(CameraManager.CameraMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPurchased(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyHeldPartAsIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartAsIcon(Transform parent, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconAsPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartIconScale()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateSelectedPartVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Please use UIPartActionControllerInventory.Instance.CreatePartFromInventory(ProtoPartSnapshot protoPartSnapshot)")]
	public Part CreatePartFromInventory(AvailablePart partInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part CreatePartFromInventory(ProtoPartSnapshot protoPartSnapshot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfigureInventoryPart(Part invPart)
	{
		throw null;
	}
}
