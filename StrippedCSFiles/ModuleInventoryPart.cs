using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleInventoryPart : PartModule, IPartMassModifier, IPartCostModifier
{
	[CompilerGenerated]
	private sealed class _003CCreatePartObject_003Ed__94 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleInventoryPart _003C_003E4__this;

		public StoredPart storedPart;

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
		public _003CCreatePartObject_003Ed__94(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CUpdateEngineerReport_003Ed__110 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CUpdateEngineerReport_003Ed__110(int _003C_003E1__state)
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

	[KSPField]
	public string Inventory;

	[UI_Grid(columnCount = 3)]
	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8320000")]
	public int InventorySlots;

	[KSPField]
	public float placementRotateSpeed;

	[KSPField]
	public float placementOpacity;

	[KSPField]
	public float placementGroundOffset;

	[KSPField]
	public float placementGroundOffsetCap;

	private string flagDecalsTagName;

	[KSPField]
	[Obsolete("This field has been replaced by the use of GameSettings.EVA_INVENTORY_RANGE")]
	public float allowedKerbalEvaDistance;

	public float volumeCapacity;

	[KSPField]
	public float packedVolumeLimit;

	private float volumeOccupied;

	public bool volumeCapacityReached;

	public float massCapacity;

	[KSPField]
	public float massLimit;

	private float massOccupied;

	public bool massCapacityReached;

	private RaycastHit positionFromTerrainHit;

	private UI_Grid grid;

	public UIPartActionInventory constructorModeInventory;

	[SerializeField]
	private List<string> inventoryPartsByName;

	[SerializeField]
	private List<string> defaultInventoryParts;

	[SerializeField]
	public DictionaryValueList<int, StoredPart> storedParts;

	public bool showPreview;

	private bool hasInitialized;

	private List<string> availableCargoParts;

	private List<string> availableCargoInventoryParts;

	[SerializeField]
	private float spawnDistance;

	[SerializeField]
	private int maxInventorySlots;

	[Obsolete("Placeholders no longer required")]
	public int transferToSlot;

	[Obsolete("Placeholders no longer required")]
	public int transferFromSlot;

	private bool isGridShowing;

	public ProtoCrewMember kerbalReference;

	[SerializeField]
	private int placementPendingIndex;

	[SerializeField]
	private Part selectedPart;

	[SerializeField]
	private ModuleGroundPart selectedPartModuleGround;

	[SerializeField]
	private Bounds selectedPartBounds;

	[SerializeField]
	private bool placementAllowXRotation;

	[SerializeField]
	private bool placementAllowYRotation;

	[SerializeField]
	private bool placementAllowZRotation;

	private bool placementCoolDown;

	private double placementCooldownTimer;

	private AudioSource placementNotAllowedsFX;

	public FXGroup placementNotAllowedGroup;

	private bool placementPositionOk;

	private bool placementInsideCap;

	private bool placementonTerrain;

	private bool partFullyCreated;

	public bool partBeingRetrieved;

	private Vector3 placementStartingPosition;

	private List<string> listedItemsToPut;

	private const string baseShader = "KSP/Bumped Specular";

	private const string flagShaderName = "KSP/Scenery/Decal/Multiply";

	[SerializeField]
	private bool inConstructionMode;

	[SerializeField]
	private bool containerHighlight;

	[SerializeField]
	private float distanceToVessel;

	public bool HasPackedVolumeLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasMassLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("This is the list of part names in the slots, but you should use storedParts from now on")]
	public List<string> InventoryPartsList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InventoryIsEmpty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InventoryIsFull
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int InventoryItemCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool AbleToPlaceParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsKerbalOnEVA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool kerbalMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part SelectedPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PlacementAllowXRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PlacementAllowYRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PlacementAllowZRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleInventoryPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselEditorPartHighlighter(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PartDroppedOnInventory(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Placeholders no longer required")]
	public void SlotClickedFirst(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Placeholders no longer required")]
	public void SlotClickedSetParts(AvailablePart ap, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Placeholders no longer required")]
	public void ResetInventoryCacheVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStackAmountAtSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStackCapacityAtSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsStackable(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanStackInSlot(AvailablePart part, string partVariantName, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UpdateStackAmountAtSlot(int slotIndex, int howMany, string variantName = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetVariantNameForSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasStackingSpace(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsSlotEmpty(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeployInventoryItem(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool InventoryItemCanBeDeployed(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCreatePartObject_003Ed__94))]
	private IEnumerator CreatePartObject(StoredPart storedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPositionFromTerrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeletePartObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetLoadedPartsHighlight(Part loadedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("This method uses the old part names, you should use the new method instead now")]
	public bool StoreCargoPartAtSlot(string partName, bool updatePAW = true, int index = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool StoreCargoPartAtSlot(string partName, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool StoreCargoPartAtSlot(Part sPart, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool StoreCargoPartAtSlot(ProtoPartSnapshot pPart, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefillEVAPropellantOnStoring(ProtoPartSnapshot ppSS)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RefillEVAPropellantOnBoarding(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefillEVAPropellant(ProtoPartSnapshot ppSS, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ClearPartAtSlot(int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool ClearPartAtSlot(int slotIndex, bool bypassEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckPartStorage(string partName, int index = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RequestUpdateEngineerReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateEngineerReport_003Ed__110))]
	private IEnumerator UpdateEngineerReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModuleInventoryChanged(ModuleInventoryPart moduleInventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPurchased(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSoundFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveDefault(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCapacityValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeMassVolumeDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMassVolumeDisplay(bool updateCapacities, bool constructionOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PreviewLimits(Part newPart, int amountToStore, AvailablePart slotPart, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RestoreLimitsDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetPartMass(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCapacity(Part newPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void KerbalStateChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleInventoryVisibility(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartActionUIOpened(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeInventory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInventoryDefaults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetInventoryPartsByName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Please use UIPartActionControllerInventory.Instance.CreatePartFromInventory")]
	public Part CreatePartForInventoryUse(AvailablePart partInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CancelPartPlacementMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int FirstEmptySlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int FirstFullSlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int TotalEmptySlots()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCopy(PartModule fromModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsPart(string partName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeployGroundPart(StoredPart storedPart, Vector3 partPosition, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode GetProtoVesselNode(StoredPart storedPart, Vector3 partPosition, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode CreatePartNode(uint id, StoredPart storedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int TotalAmountOfPartStored(string requestedPartName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveNPartsFromInventory(string requestedPartName, int n, bool playSound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IgnoreKerbalInventoryLimits()
	{
		throw null;
	}
}
