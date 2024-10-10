using System;
using System.Collections;
using System.Collections.Generic;
using Experience.Effects;
using Highlighting;
using ns9;
using UnityEngine;

public class ModuleInventoryPart : PartModule, IPartMassModifier, IPartCostModifier
{
	[KSPField]
	public string Inventory;

	[UI_Grid(columnCount = 3)]
	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 5f, guiName = "#autoLOC_8320000")]
	public int InventorySlots = 9;

	[KSPField]
	public float placementRotateSpeed = 80f;

	[KSPField]
	public float placementOpacity = 0.4f;

	[KSPField]
	public float placementGroundOffset = 0.1f;

	[KSPField]
	public float placementGroundOffsetCap = 0.5f;

	public string flagDecalsTagName = "FlagDecal";

	[KSPField]
	[Obsolete("This field has been replaced by the use of GameSettings.EVA_INVENTORY_RANGE")]
	public float allowedKerbalEvaDistance = 5f;

	public float volumeCapacity;

	[KSPField]
	public float packedVolumeLimit;

	public float volumeOccupied;

	public bool volumeCapacityReached;

	public float massCapacity;

	[KSPField]
	public float massLimit;

	public float massOccupied;

	public bool massCapacityReached;

	public RaycastHit positionFromTerrainHit;

	public UI_Grid grid;

	public UIPartActionInventory constructorModeInventory;

	[SerializeField]
	public List<string> inventoryPartsByName;

	[SerializeField]
	public List<string> defaultInventoryParts;

	[SerializeField]
	public DictionaryValueList<int, StoredPart> storedParts;

	public bool showPreview;

	public bool hasInitialized;

	public List<string> availableCargoParts = new List<string>();

	public List<string> availableCargoInventoryParts = new List<string>();

	[SerializeField]
	public float spawnDistance = 1f;

	[SerializeField]
	public int maxInventorySlots = 30;

	[Obsolete("Placeholders no longer required")]
	public int transferToSlot = -1;

	[Obsolete("Placeholders no longer required")]
	public int transferFromSlot = -1;

	public bool isGridShowing;

	public ProtoCrewMember kerbalReference;

	[SerializeField]
	public int placementPendingIndex = -1;

	[SerializeField]
	public Part selectedPart;

	[SerializeField]
	public ModuleGroundPart selectedPartModuleGround;

	[SerializeField]
	public Bounds selectedPartBounds;

	[SerializeField]
	public bool placementAllowXRotation;

	[SerializeField]
	public bool placementAllowYRotation;

	[SerializeField]
	public bool placementAllowZRotation;

	public bool placementCoolDown;

	public double placementCooldownTimer;

	public AudioSource placementNotAllowedsFX;

	public FXGroup placementNotAllowedGroup;

	public bool placementPositionOk;

	public bool placementInsideCap;

	public bool placementonTerrain;

	public bool partFullyCreated;

	public bool partBeingRetrieved;

	public Vector3 placementStartingPosition;

	public List<string> listedItemsToPut = new List<string>();

	public const string baseShader = "KSP/Bumped Specular";

	public const string flagShaderName = "KSP/Scenery/Decal/Multiply";

	[SerializeField]
	public bool inConstructionMode;

	[SerializeField]
	public bool containerHighlight;

	[SerializeField]
	public float distanceToVessel;

	public bool HasPackedVolumeLimit => packedVolumeLimit > 0f;

	public bool HasMassLimit => massLimit > 0f;

	[Obsolete("This is the list of part names in the slots, but you should use storedParts from now on")]
	public List<string> InventoryPartsList
	{
		get
		{
			List<string> list = new List<string>();
			for (int i = 0; i < InventorySlots; i++)
			{
				if (storedParts.TryGetValue(i, out var val))
				{
					list.Add(val.partName);
				}
				else
				{
					list.Add("");
				}
			}
			return list;
		}
	}

	public bool InventoryIsEmpty
	{
		get
		{
			if (storedParts != null)
			{
				return storedParts.Count == 0;
			}
			return true;
		}
	}

	public bool InventoryIsFull
	{
		get
		{
			if (storedParts != null)
			{
				return storedParts.Count >= InventorySlots;
			}
			return false;
		}
	}

	public int InventoryItemCount
	{
		get
		{
			if (storedParts != null)
			{
				return storedParts.Count;
			}
			return 0;
		}
	}

	public bool AbleToPlaceParts
	{
		get
		{
			if (base.part != null && base.part.vessel != null && base.part.vessel.isEVA && base.part.vessel.Landed && base.part.vessel.isActiveVessel)
			{
				return selectedPart == null;
			}
			return false;
		}
	}

	public bool IsKerbalOnEVA
	{
		get
		{
			if (base.part != null && base.part.vessel != null)
			{
				return base.part.vessel.isEVA;
			}
			return false;
		}
	}

	public bool kerbalMode
	{
		get
		{
			if (kerbalReference != null)
			{
				return kerbalReference.name != "";
			}
			return false;
		}
	}

	public Part SelectedPart => selectedPart;

	public bool PlacementAllowXRotation => placementAllowXRotation;

	public bool PlacementAllowYRotation => placementAllowYRotation;

	public bool PlacementAllowZRotation => placementAllowZRotation;

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		switch (evt)
		{
		case ConstructionEventType.PartDropped:
			PartDroppedOnInventory(evt, p);
			break;
		case ConstructionEventType.PartAttached:
			PartDroppedOnInventory(evt, p);
			break;
		case ConstructionEventType.PartDeleted:
			PartDroppedOnInventory(evt, p);
			break;
		case ConstructionEventType.PartDetached:
		case ConstructionEventType.PartCopied:
			if (p != null && p.isCargoPart() && UIPartActionControllerInventory.Instance != null)
			{
				UIPartActionControllerInventory.Instance.CurrentCargoPart = p;
			}
			break;
		case ConstructionEventType.PartPicked:
		case ConstructionEventType.PartDragging:
			break;
		}
	}

	public void VesselEditorPartHighlighter(Part p)
	{
		bool flag;
		if (p == null)
		{
			base.part.highlightColor = Highlighter.colorPartHighlightDefault;
			flag = false;
		}
		else
		{
			if (!availableCargoParts.Contains(p.name))
			{
				return;
			}
			if (UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.CurrentCargoPart != null && !UIPartActionControllerInventory.Instance.CurrentCargoPart.attachRules.srfAttach && !UIPartActionControllerInventory.Instance.CurrentCargoPart.attachRules.allowStack)
			{
				base.part.highlightColor = Highlighter.colorPartInventoryContainer;
				flag = true;
			}
			else
			{
				flag = false;
			}
		}
		base.part.Highlight(flag);
		base.part.HighlightActive = flag;
	}

	public void PartDroppedOnInventory(ConstructionEventType evt, Part p)
	{
		if (availableCargoInventoryParts.Contains(p.name) || !availableCargoParts.Contains(p.name))
		{
			return;
		}
		if (base.part.HighlightActive)
		{
			base.part.highlightColor = Part.defaultHighlightPart;
			base.part.Highlight(active: false);
		}
		ModuleCargoPart moduleCargoPart = p.FindModuleImplementing<ModuleCargoPart>();
		if (moduleCargoPart == null || evt != ConstructionEventType.PartDropped || !base.part.MouseOver || (!IgnoreKerbalInventoryLimits() && HasPackedVolumeLimit && (moduleCargoPart.packedVolume < 0f || moduleCargoPart.packedVolume + volumeOccupied > packedVolumeLimit)) || (!IgnoreKerbalInventoryLimits() && HasMassLimit && massOccupied + GetPartMass(p) > massLimit))
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (moduleCargoPart.stackableQuantity > 1)
		{
			string partVariantName = ((p.variants == null) ? "" : p.variants.SelectedVariant.Name);
			for (int i = 0; i < storedParts.Keys.Count; i++)
			{
				int slotIndex = storedParts.KeysList[i];
				if (CanStackInSlot(p.partInfo, partVariantName, slotIndex))
				{
					int stackAmountAtSlot = GetStackAmountAtSlot(slotIndex);
					UpdateStackAmountAtSlot(slotIndex, stackAmountAtSlot + 1);
					flag = true;
					break;
				}
			}
		}
		if (!flag && TotalEmptySlots() > 0)
		{
			int num = FirstEmptySlot();
			if (StoreCargoPartAtSlot(p, num))
			{
				flag2 = true;
				if (grid != null && grid.pawInventory != null && grid.pawInventory.gameObject.activeInHierarchy)
				{
					grid.pawInventory.slotPartIcon[num].Create(null, p.partInfo, null, grid.pawInventory.iconSize, 1f, 1f, grid.pawInventory.StartPartPlacement, AbleToPlaceParts, skipVariants: false, null, useImageThumbnail: true, inInventory: true);
				}
			}
		}
		if ((!HighLogic.LoadedSceneIsEditor || !EditorLogic.fetch.lastEventName.ToLowerInvariant().Contains("root")) && (flag || flag2))
		{
			UnityEngine.Object.Destroy(p.gameObject);
		}
	}

	[Obsolete("Placeholders no longer required")]
	public void SlotClickedFirst(int index)
	{
		transferFromSlot = index;
	}

	[Obsolete("Placeholders no longer required")]
	public void SlotClickedSetParts(AvailablePart ap, int index)
	{
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.InventoryAndCargoPartExist())
		{
			transferFromSlot = UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.slotIndex;
			transferToSlot = index;
		}
	}

	[Obsolete("Placeholders no longer required")]
	public void ResetInventoryCacheVars()
	{
		if (grid != null)
		{
			grid.updateSlotItems = true;
		}
		transferFromSlot = -1;
		transferToSlot = -1;
	}

	public int GetStackAmountAtSlot(int slotIndex)
	{
		if (!storedParts.ContainsKey(slotIndex))
		{
			return 0;
		}
		return storedParts[slotIndex].quantity;
	}

	public int GetStackCapacityAtSlot(int slotIndex)
	{
		if (!storedParts.ContainsKey(slotIndex))
		{
			return 0;
		}
		return storedParts[slotIndex].stackCapacity;
	}

	public bool IsStackable(int slotIndex)
	{
		if (!storedParts.ContainsKey(slotIndex))
		{
			return false;
		}
		return storedParts[slotIndex].stackCapacity > 1;
	}

	public bool CanStackInSlot(AvailablePart part, string partVariantName, int slotIndex)
	{
		if (storedParts.TryGetValue(slotIndex, out var val) && val.partName.Equals(part.name))
		{
			if (partVariantName == "")
			{
				return HasStackingSpace(slotIndex);
			}
			if (partVariantName == val.variantName)
			{
				return HasStackingSpace(slotIndex);
			}
		}
		return false;
	}

	public bool UpdateStackAmountAtSlot(int slotIndex, int howMany, string variantName = "")
	{
		if (!storedParts.TryGetValue(slotIndex, out var val))
		{
			Debug.LogWarning($"[ModuleInventoryPart]: Cannot update quantity in slot {slotIndex}. There is no part stored there");
			return false;
		}
		howMany = Math.Max(1, Math.Min(howMany, val.stackCapacity));
		if (!string.IsNullOrEmpty(variantName) && val.variantName != variantName)
		{
			Debug.LogWarning($"[ModuleInventoryPart]: Cannot update quantity in slot {slotIndex}. There the variant being asked for is not the same");
			return false;
		}
		bool num = howMany != val.quantity;
		val.quantity = howMany;
		if (num)
		{
			GameEvents.onModuleInventorySlotChanged.Fire(this, slotIndex);
			GameEvents.onModuleInventoryChanged.Fire(this);
		}
		return true;
	}

	public string GetVariantNameForSlot(int slotIndex)
	{
		if (!storedParts.ContainsKey(slotIndex))
		{
			return "";
		}
		return storedParts[slotIndex].variantName;
	}

	public bool HasStackingSpace(int slotIndex)
	{
		if (!storedParts.ContainsKey(slotIndex))
		{
			return false;
		}
		return storedParts[slotIndex].quantity < storedParts[slotIndex].stackCapacity;
	}

	public bool IsSlotEmpty(int slotIndex)
	{
		return !storedParts.ContainsKey(slotIndex);
	}

	public virtual void DeployInventoryItem(int index)
	{
		if (AbleToPlaceParts && InventoryItemCanBeDeployed(index) && !partBeingRetrieved)
		{
			UIPartActionControllerInventory.Instance.DestroyTooltip();
			placementPendingIndex = index;
			StartCoroutine(CreatePartObject(storedParts[index]));
		}
		else
		{
			grid.pawInventory.ResetPlacePartIcons(active: true);
		}
	}

	public virtual bool InventoryItemCanBeDeployed(int index)
	{
		if (storedParts.TryGetValue(index, out var val) && selectedPart == null && !string.IsNullOrEmpty(val.partName))
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(val.partName);
			if (partInfoByName != null && partInfoByName.partPrefab.isGroundDeployable())
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerator CreatePartObject(StoredPart storedPart)
	{
		partFullyCreated = false;
		if (storedPart == null)
		{
			yield break;
		}
		if (base.vessel.isEVA && base.vessel.evaController != null)
		{
			if (!base.vessel.evaController.SetPartPlacementMode(mode: true, this))
			{
				yield break;
			}
			if (base.vessel.evaController.JetpackDeployed)
			{
				base.vessel.evaController.ToggleJetpack();
			}
		}
		Part part = storedPart.snapshot.CreatePart();
		part.ResumeState = PartStates.PLACEMENT;
		part.State = PartStates.PLACEMENT;
		part.name = storedPart.snapshot.partInfo.name;
		part.persistentId = FlightGlobals.CheckPartpersistentId(part.persistentId, part, removeOldId: false, addNewId: true);
		if (!string.IsNullOrEmpty(storedPart.variantName))
		{
			part.variants.SetVariant(storedPart.variantName);
		}
		selectedPart = part;
		selectedPartModuleGround = selectedPart.FindModuleImplementing<ModuleGroundPart>();
		if (selectedPartModuleGround != null)
		{
			placementAllowXRotation = selectedPartModuleGround.placementAllowXRotation;
			placementAllowYRotation = selectedPartModuleGround.placementAllowYRotation;
			placementAllowZRotation = selectedPartModuleGround.placementAllowZRotation;
		}
		else
		{
			placementAllowXRotation = true;
			placementAllowYRotation = true;
			placementAllowZRotation = true;
		}
		selectedPart.transform.rotation = (base.part ? base.part.transform.rotation : selectedPart.initRotation);
		Collider[] componentsInChildren = selectedPart.transform.Find("model").GetComponentsInChildren<Collider>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].isTrigger = true;
		}
		selectedPart.gameObject.SetActive(value: true);
		selectedPartBounds = selectedPart.GetPartRendererBound();
		MeshRenderer[] componentsInChildren2 = selectedPart.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] componentsInChildren3 = selectedPart.GetComponentsInChildren<SkinnedMeshRenderer>();
		Bounds bounds = default(Bounds);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			bounds.Encapsulate(componentsInChildren2[j].bounds);
		}
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			bounds.Encapsulate(componentsInChildren3[k].sharedMesh.bounds);
		}
		UnityEngine.Object.Destroy(selectedPart.rb);
		selectedPart.physicalSignificance = Part.PhysicalSignificance.NONE;
		selectedPart.partInfo = storedPart.snapshot.partInfo;
		spawnDistance = Mathf.Max(selectedPartBounds.extents.x, selectedPartBounds.extents.y, selectedPartBounds.extents.z) + 0.2f;
		selectedPart.transform.position = base.vessel.vesselTransform.position + base.vessel.vesselTransform.forward * spawnDistance;
		SetPositionFromTerrain();
		selectedPart.attRotation = Quaternion.identity;
		selectedPart.attRotation0 = selectedPart.transform.localRotation;
		selectedPart.attPos0 = selectedPart.transform.localPosition;
		selectedPart.transform.SetParent(base.vessel.transform);
		yield return null;
		selectedPart.highlighter.ReinitMaterials();
		yield return null;
		placementPositionOk = true;
		selectedPart.SetHighlightColor(Highlighter.colorPartHighlightDefault);
		selectedPart.SetHighlightType(Part.HighlightType.AlwaysOn);
		selectedPart.SetHighlight(active: true, recursive: true);
		for (int l = 0; l < FlightGlobals.PersistentLoadedPartIds.ValuesList.Count; l++)
		{
			if (FlightGlobals.PersistentLoadedPartIds.ValuesList[l].persistentId != selectedPart.persistentId)
			{
				FlightGlobals.PersistentLoadedPartIds.ValuesList[l].SetHighlightType(Part.HighlightType.Disabled);
			}
		}
		for (int m = 0; m < selectedPart.Modules.Count; m++)
		{
			selectedPart.Modules[m].enabled = false;
		}
		UIPartActionWindow item = UIPartActionController.Instance.GetItem(base.part, includeSymmetryCounterparts: false);
		if (item != null)
		{
			item.isValid = false;
		}
		partFullyCreated = true;
	}

	public void SetPositionFromTerrain()
	{
		float num = 50f;
		placementStartingPosition = base.vessel.vesselTransform.position + base.vessel.vesselTransform.forward * spawnDistance;
		Vector3 vector = placementStartingPosition;
		Vector3d upAxis = FlightGlobals.getUpAxis(base.vessel.mainBody, placementStartingPosition);
		float maxDistance = (float)(vector - base.vessel.mainBody.position).magnitude;
		vector += upAxis * 50.0;
		selectedPart.transform.position = upAxis * 100.0 + placementStartingPosition;
		if (Physics.Raycast(vector, -upAxis, out positionFromTerrainHit, maxDistance, 0x8000 | LayerUtil.DefaultEquivalent))
		{
			if (positionFromTerrainHit.collider.gameObject.layer == 15 && !positionFromTerrainHit.collider.gameObject.CompareTag("ROC"))
			{
				placementonTerrain = true;
			}
			else
			{
				placementonTerrain = false;
			}
			if (base.vessel != null && base.vessel.Splashed)
			{
				placementonTerrain = false;
			}
			Debug.DrawLine(vector, vector + -upAxis * positionFromTerrainHit.distance, Color.green, 2f);
			float num2 = positionFromTerrainHit.distance - num;
			float centerPointOffset = 0f;
			float boundsPoints = selectedPart.GetBoundsPoints(positionFromTerrainHit.normal, out centerPointOffset);
			boundsPoints += placementGroundOffset;
			float num3 = Mathf.Cos(Mathf.Abs((float)Vector3d.Angle(positionFromTerrainHit.normal, upAxis)) * ((float)Math.PI / 180f)) * boundsPoints;
			if (Mathf.Abs(boundsPoints - num3) > 0.1f)
			{
				boundsPoints = num3;
			}
			if (Mathf.Abs(num2) > placementGroundOffsetCap)
			{
				placementInsideCap = false;
				num2 = Mathf.Clamp(num2, 0f - placementGroundOffsetCap + centerPointOffset, placementGroundOffsetCap + centerPointOffset);
			}
			else
			{
				placementInsideCap = true;
			}
			if (num2 > boundsPoints)
			{
				boundsPoints = num2 - boundsPoints;
				selectedPart.transform.position = placementStartingPosition + upAxis * (0f - boundsPoints);
			}
			else
			{
				boundsPoints -= num2;
				selectedPart.transform.position = placementStartingPosition + upAxis * boundsPoints;
			}
		}
		else
		{
			selectedPart.transform.position = placementStartingPosition;
			placementonTerrain = false;
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (selectedPart != null)
		{
			Vector3 center = selectedPartBounds.center;
			float magnitude = selectedPartBounds.extents.magnitude;
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(selectedPart.transform.position + center, 0.02f);
			Gizmos.DrawWireSphere(selectedPart.transform.position + center, magnitude);
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(selectedPart.transform.position + selectedPartBounds.center, selectedPartBounds.size);
		}
	}

	public void DeletePartObject()
	{
		if (selectedPart != null)
		{
			selectedPart.OnDelete();
			UnityEngine.Object.Destroy(selectedPart.gameObject);
			selectedPart = null;
		}
		for (int i = 0; i < FlightGlobals.PersistentLoadedPartIds.ValuesList.Count; i++)
		{
			Part part = FlightGlobals.PersistentLoadedPartIds.ValuesList[i];
			if (!(part != null))
			{
				continue;
			}
			if (part.partInfo.category == PartCategories.Cargo)
			{
				if (part.State != PartStates.PLACEMENT)
				{
					ResetLoadedPartsHighlight(part);
				}
			}
			else
			{
				ResetLoadedPartsHighlight(part);
			}
		}
	}

	public void ResetLoadedPartsHighlight(Part loadedPart)
	{
		loadedPart.SetHighlightColor(Highlighter.colorPartHighlightDefault);
		loadedPart.SetHighlightType(Part.HighlightType.OnMouseOver);
	}

	[Obsolete("This method uses the old part names, you should use the new method instead now")]
	public bool StoreCargoPartAtSlot(string partName, bool updatePAW = true, int index = -1)
	{
		if (!CheckPartStorage(partName, index))
		{
			Debug.LogWarningFormat("[ModuleInventoryPart]: Unable to store {0} into {1}-{2} as there are no empty slots available.", partName, base.part.partInfo.title, base.part.persistentId);
			return false;
		}
		if (index == -1)
		{
			index = FirstEmptySlot();
		}
		return StoreCargoPartAtSlot(partName, index);
	}

	public bool StoreCargoPartAtSlot(string partName, int slotIndex)
	{
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(partName);
		if (partInfoByName != null)
		{
			return StoreCargoPartAtSlot(partInfoByName.partPrefab, slotIndex);
		}
		Debug.LogError($"[ModuleInventoryPart]: Unable to store part at index {slotIndex}. Unable find part with name={partName}");
		return false;
	}

	public bool StoreCargoPartAtSlot(Part sPart, int slotIndex)
	{
		ProtoPartSnapshot protoPartSnapshot = null;
		if (sPart != null && sPart.Modules != null)
		{
			for (int i = 0; i < sPart.Modules.Count; i++)
			{
				if (sPart.Modules[i] != null)
				{
					sPart.Modules[i].OnStoredInInventory(this);
				}
			}
		}
		if (base.vessel != null)
		{
			protoPartSnapshot = new ProtoPartSnapshot(sPart, base.vessel.protoVessel);
		}
		else if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsMissionBuilder || HighLogic.LoadedScene == GameScenes.MAINMENU || kerbalMode)
		{
			protoPartSnapshot = new ProtoPartSnapshot(sPart, null);
		}
		if (protoPartSnapshot == null)
		{
			Debug.LogError($"[ModuleInventoryPart]: Unable to create a snapshot part at index {slotIndex}. Unable part name={sPart.name}");
			return false;
		}
		return StoreCargoPartAtSlot(protoPartSnapshot, slotIndex);
	}

	public bool StoreCargoPartAtSlot(ProtoPartSnapshot pPart, int slotIndex)
	{
		bool flag = false;
		if (slotIndex == -1)
		{
			slotIndex = FirstEmptySlot();
		}
		if (slotIndex == -1)
		{
			Debug.LogError("[ModuleInventoryPart]: Unable to store part - there are no free slots. Unable find part with name=" + pPart.partName);
			return false;
		}
		if (storedParts.ContainsKey(slotIndex))
		{
			ClearPartAtSlot(slotIndex, bypassEvent: true);
			flag = true;
		}
		if (pPart != null && pPart.partInfo != null)
		{
			RefillEVAPropellantOnStoring(pPart);
			StoredPart val = new StoredPart(pPart.partName, slotIndex)
			{
				slotIndex = slotIndex,
				snapshot = pPart,
				variantName = pPart.moduleVariantName,
				quantity = 1,
				stackCapacity = pPart.moduleCargoStackableQuantity
			};
			storedParts.Add(slotIndex, val);
			flag = true;
			ResetInventoryPartsByName();
		}
		if (flag)
		{
			UpdateModuleUI();
			GameEvents.onModuleInventorySlotChanged.Fire(this, slotIndex);
			GameEvents.onModuleInventoryChanged.Fire(this);
		}
		return true;
	}

	public void RefillEVAPropellantOnStoring(ProtoPartSnapshot ppSS)
	{
		RefillEVAPropellant(ppSS, base.part);
	}

	public void RefillEVAPropellantOnBoarding(Part p)
	{
		for (int i = 0; i < storedParts.Count; i++)
		{
			StoredPart storedPart = storedParts.ValuesList[i];
			if (storedPart != null && storedPart.snapshot != null)
			{
				RefillEVAPropellant(storedPart.snapshot, p);
			}
		}
	}

	public void RefillEVAPropellant(ProtoPartSnapshot ppSS, Part p)
	{
		if (p.isKerbalEVA() || p.isKerbalSeat())
		{
			return;
		}
		ProtoPartModuleSnapshot protoPartModuleSnapshot = ppSS.FindModule("ModuleCargoPart");
		if (protoPartModuleSnapshot == null)
		{
			return;
		}
		bool value = false;
		if (protoPartModuleSnapshot.moduleValues.TryGetValue("reinitResourcesOnStoreInVessel", ref value) && value)
		{
			for (int i = 0; i < ppSS.resources.Count; i++)
			{
				ppSS.resources[i].amount = ppSS.resources[i].maxAmount;
				ppSS.resources[i].UpdateConfigNodeAmounts();
			}
		}
	}

	public bool ClearPartAtSlot(int slotIndex)
	{
		return ClearPartAtSlot(slotIndex, bypassEvent: false);
	}

	public bool ClearPartAtSlot(int slotIndex, bool bypassEvent)
	{
		bool flag = false;
		if (storedParts.ContainsKey(slotIndex))
		{
			storedParts.Remove(slotIndex);
			flag = true;
			ResetInventoryPartsByName();
		}
		if (flag && !bypassEvent)
		{
			UpdateModuleUI();
			GameEvents.onModuleInventorySlotChanged.Fire(this, slotIndex);
			GameEvents.onModuleInventoryChanged.Fire(this);
		}
		return true;
	}

	public bool CheckPartStorage(string partName, int index = -1)
	{
		return !InventoryIsFull;
	}

	public void RequestUpdateEngineerReport()
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(UpdateEngineerReport());
		}
		else if (EditorLogic.fetch != null && EditorLogic.fetch.ship != null)
		{
			EditorLogic.fetch.StartCoroutine(UpdateEngineerReport());
		}
	}

	public IEnumerator UpdateEngineerReport()
	{
		yield return null;
		if (EditorLogic.fetch != null && EditorLogic.fetch.ship != null)
		{
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		}
	}

	public void OnModuleInventoryChanged(ModuleInventoryPart moduleInventory)
	{
		if (!(moduleInventory != this))
		{
			if (kerbalMode)
			{
				kerbalReference.SaveInventory(this);
			}
			RequestUpdateEngineerReport();
			UpdateCapacityValues();
		}
	}

	public void OnPartPurchased(AvailablePart part)
	{
		availableCargoParts = PartLoader.Instance.GetAvailableCargoPartNames();
		availableCargoInventoryParts = PartLoader.Instance.GetAvailableCargoInventoryPartNames();
	}

	public override void OnStart(StartState state)
	{
		allowedKerbalEvaDistance = GameSettings.EVA_INVENTORY_RANGE;
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		IgnoreKerbalInventoryLimits();
		InitializeInventory();
		if (HighLogic.LoadedSceneIsFlight)
		{
			grid = base.Fields["InventorySlots"].uiControlFlight as UI_Grid;
			base.Fields["InventorySlots"].guiActive = true;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			grid = base.Fields["InventorySlots"].uiControlEditor as UI_Grid;
			GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
			GameEvents.OnPartPurchased.Add(OnPartPurchased);
			if (!base.part.isKerbalEVA())
			{
				GameEvents.OnInventoryPartOnMouseChanged.Add(VesselEditorPartHighlighter);
			}
		}
		KerbalEVA component = GetComponent<KerbalEVA>();
		if (component != null && base.part != null && base.part.protoModuleCrew.Count > 0)
		{
			hasInitialized = false;
			if (storedParts == null)
			{
				storedParts = new DictionaryValueList<int, StoredPart>();
			}
			else
			{
				storedParts.Clear();
			}
			Load(base.part.protoModuleCrew[0].InventoryNode);
		}
		ResetInventoryPartsByName();
		grid.inventoryItems = inventoryPartsByName;
		grid.inventoryPart = this;
		if (HighLogic.LoadedSceneIsFlight && base.part != null && base.part.HasModuleImplementing<KerbalEVA>())
		{
			Transform backpackTransform = component.BackpackTransform;
			Transform storageTransform = component.StorageTransform;
			Transform storageSlimTransform = component.StorageSlimTransform;
			Transform backpackStTransform = component.BackpackStTransform;
			if (backpackTransform != null)
			{
				backpackTransform.SetShader("KSP/Bumped Specular");
			}
			if (storageTransform != null)
			{
				storageTransform.SetShader("KSP/Bumped Specular");
			}
			if (storageSlimTransform != null)
			{
				storageSlimTransform.SetShader("KSP/Bumped Specular");
			}
			if (backpackStTransform != null)
			{
				backpackStTransform.SetShader("KSP/Bumped Specular");
			}
			Shader shader = Shader.Find("KSP/Scenery/Decal/Multiply");
			Transform[] array = base.part.FindModelTransformsWithTag(flagDecalsTagName);
			for (int i = 0; i < array.Length; i++)
			{
				MeshRenderer component2 = array[i].GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					component2.material.shader = shader;
				}
			}
		}
		isGridShowing = HighLogic.LoadedSceneIsEditor || KerbalEVA.alwaysShowInventory || (base.vessel != null && (!base.vessel.isEVA || (base.vessel.isEVA && PartLoader.Instance.CargoPartsLoaded)));
		ToggleInventoryVisibility(isGridShowing);
		GameEvents.onPartActionUICreate.Add(onPartActionUIOpened);
		SetupSoundFX();
		UpdateCapacityValues();
		UpdateMassVolumeDisplay(updateCapacities: true, constructionOnly: false);
		if (!IgnoreKerbalInventoryLimits() && HasPackedVolumeLimit && volumeCapacity > packedVolumeLimit)
		{
			volumeCapacityReached = true;
		}
		else
		{
			volumeCapacityReached = false;
		}
		massCapacity = massOccupied;
		if (!IgnoreKerbalInventoryLimits() && HasMassLimit && massCapacity > massLimit)
		{
			massCapacityReached = true;
		}
		else
		{
			massCapacityReached = false;
		}
		GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
	}

	public void SetupSoundFX()
	{
		placementNotAllowedGroup = base.part.findFxGroup("placementNotAllowed");
		if (placementNotAllowedGroup != null)
		{
			placementNotAllowedGroup.setActive(value: false);
			placementNotAllowedsFX = base.gameObject.GetComponent<AudioSource>();
			if (placementNotAllowedsFX == null)
			{
				placementNotAllowedsFX = base.gameObject.AddComponent<AudioSource>();
			}
			placementNotAllowedsFX.playOnAwake = false;
			placementNotAllowedsFX.loop = false;
			placementNotAllowedsFX.rolloffMode = AudioRolloffMode.Linear;
			placementNotAllowedsFX.dopplerLevel = 0f;
			placementNotAllowedsFX.volume = GameSettings.SHIP_VOLUME;
			placementNotAllowedsFX.spatialBlend = 1f;
			placementNotAllowedGroup.begin(placementNotAllowedsFX);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUICreate.Remove(onPartActionUIOpened);
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
		GameEvents.OnPartPurchased.Remove(OnPartPurchased);
		GameEvents.OnInventoryPartOnMouseChanged.Remove(VesselEditorPartHighlighter);
	}

	public override void OnLoad(ConfigNode node)
	{
		InitializeInventory();
		ConfigNode node2 = null;
		if (node.TryGetNode("STOREDPARTS", ref node2))
		{
			ConfigNode[] array = null;
			array = node2.GetNodes("STOREDPART");
			if (array.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					StoredPart storedPart = new StoredPart(array[i]);
					if (base.vessel != null)
					{
						storedPart.snapshot.pVesselRef = base.vessel.protoVessel;
					}
					storedParts.Add(storedPart.slotIndex, storedPart);
				}
			}
			else
			{
				array = node2.GetNodes("PART");
				ConfigNode node3 = null;
				bool flag = node.TryGetNode("STACKAMOUNTS", ref node3);
				int num = 0;
				for (int j = 0; j < array.Length; j++)
				{
					ConfigNode node4 = array[j];
					num = FirstEmptySlot();
					if (num >= 0)
					{
						ProtoPartSnapshot protoPartSnapshot = new ProtoPartSnapshot(node4, null, null);
						StoredPart storedPart2 = new StoredPart(protoPartSnapshot.partName, num);
						storedPart2.snapshot = protoPartSnapshot;
						if (base.vessel != null)
						{
							storedPart2.snapshot.pVesselRef = base.vessel.protoVessel;
						}
						if (flag && j < node3.nodes.Count)
						{
							node3.nodes[j].TryGetValue("amount", ref storedPart2.quantity);
						}
						else
						{
							storedPart2.quantity = 1;
						}
						storedParts.Add(storedPart2.slotIndex, storedPart2);
					}
				}
			}
		}
		if (node.TryGetValue("inventory", ref Inventory) && storedParts.Count < 1)
		{
			string[] array2 = Inventory.Split(',');
			int num2 = 0;
			for (int k = 0; k < array2.Length; k++)
			{
				num2 = FirstEmptySlot();
				if (num2 == -1)
				{
					break;
				}
				if (!string.IsNullOrEmpty(array2[k]) && availableCargoParts.Contains(array2[k]))
				{
					AvailablePart partInfoByName = PartLoader.getPartInfoByName(array2[k]);
					if (partInfoByName != null)
					{
						StoreCargoPartAtSlot(partInfoByName.partPrefab, num2);
					}
				}
			}
		}
		_ = storedParts.Count;
		ConfigNode node5 = null;
		if (node.TryGetNode("DEFAULTPARTS", ref node5))
		{
			string[] values = node5.GetValues("name");
			defaultInventoryParts = new List<string>();
			for (int l = 0; l < values.Length; l++)
			{
				if (!string.IsNullOrEmpty(values[l]) && l <= InventorySlots - 1)
				{
					defaultInventoryParts.Add(values[l]);
				}
			}
		}
		GameEvents.onModuleInventoryChanged.Fire(this);
	}

	public override void OnSave(ConfigNode node)
	{
		if (storedParts != null)
		{
			ConfigNode configNode = new ConfigNode();
			configNode.name = "STOREDPARTS";
			for (int i = 0; i < storedParts.Count; i++)
			{
				if (storedParts.ValuesList[i] != null)
				{
					ConfigNode node2 = new ConfigNode("STOREDPART");
					storedParts.ValuesList[i].Save(node2);
					configNode.AddNode(node2);
				}
				else
				{
					Debug.LogError("[ModuleInventoryPart]: A stored part was null when saving. Skipping this part in " + base.part.partName);
				}
			}
			node.AddNode(configNode);
		}
		Inventory = "";
		bool flag = false;
		ResetInventoryPartsByName();
		if (inventoryPartsByName != null)
		{
			for (int j = 0; j < inventoryPartsByName.Count; j++)
			{
				if (!string.IsNullOrEmpty(inventoryPartsByName[j]))
				{
					if (flag)
					{
						Inventory += ",";
					}
					Inventory += inventoryPartsByName[j];
					flag = true;
				}
			}
		}
		if (!string.IsNullOrEmpty(Inventory))
		{
			node.AddValue("inventory", Inventory);
		}
	}

	public void SaveDefault(ConfigNode node)
	{
		SetInventoryDefaults();
		OnSave(node);
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsEditor && base.part.MouseOver && UIPartActionControllerInventory.Instance.CurrentCargoPart != null)
		{
			UIPartActionControllerInventory.Instance.OnInventoryPartHover(base.part.persistentId, base.part);
		}
		if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance != null && (!EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != 0) && base.part.MouseOver && UIPartActionControllerInventory.Instance.CurrentCargoPart != null)
		{
			UIPartActionControllerInventory.Instance.OnInventoryPartHover(base.part.persistentId, base.part);
		}
		if (!(UIPartActionControllerInventory.Instance.CurrentCargoPart != null))
		{
			return;
		}
		if ((GameSettings.MODIFIER_KEY.GetKeyDown() || GameSettings.MODIFIER_KEY.GetKeyUp()) && !showPreview)
		{
			volumeCapacity = volumeOccupied;
			massCapacity = massOccupied;
			showPreview = true;
		}
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered != null && showPreview && UIPartActionControllerInventory.Instance.CurrentInventorySlotHoveredModule == this)
		{
			volumeCapacity = volumeOccupied;
			massCapacity = massOccupied;
			UIPartActionControllerInventory.amountToStore = (GameSettings.MODIFIER_KEY.GetKey() ? UIPartActionControllerInventory.Instance.CurrentModuleCargoPart.stackableQuantity : ((UIPartActionControllerInventory.stackSize == 0) ? 1 : UIPartActionControllerInventory.stackSize));
			int slotIndex = UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.slotIndex;
			storedParts.TryGetValue(slotIndex, out var val);
			if (val != null)
			{
				PreviewLimits(UIPartActionControllerInventory.Instance.CurrentCargoPart, UIPartActionControllerInventory.amountToStore, val.snapshot.partInfo, slotIndex);
			}
			else
			{
				PreviewLimits(UIPartActionControllerInventory.Instance.CurrentCargoPart, UIPartActionControllerInventory.amountToStore, null, slotIndex);
			}
		}
	}

	public override void OnUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		if (UIPartActionControllerInventory.Instance != null)
		{
			bool flag = false;
			bool flag2 = false;
			if (UIPartActionControllerInventory.Instance.CurrentCargoPart != null)
			{
				distanceToVessel = (FlightGlobals.ActiveVessel.vesselTransform.position - base.part.transform.position).magnitude;
				if (containerHighlight)
				{
					if (distanceToVessel > GameSettings.EVA_INVENTORY_RANGE)
					{
						flag = true;
						flag2 = false;
					}
				}
				else if (distanceToVessel <= GameSettings.EVA_INVENTORY_RANGE)
				{
					flag = true;
					flag2 = true;
					if (HighLogic.LoadedSceneIsFlight)
					{
						if (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction)
						{
							if (!UIPartActionControllerInventory.Instance.CurrentCargoPart.attachRules.srfAttach && !UIPartActionControllerInventory.Instance.CurrentCargoPart.attachRules.allowStack)
							{
								flag2 = true;
							}
							else
							{
								flag = false;
							}
						}
						if (EVAConstructionModeController.Instance == null || EVAConstructionModeController.Instance != null)
						{
							if (EVAConstructionModeController.Instance.IsOpen)
							{
								if (EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Cargo)
								{
									flag2 = true;
								}
							}
							else
							{
								flag2 = true;
							}
						}
					}
				}
			}
			else if (containerHighlight)
			{
				flag = true;
				flag2 = false;
			}
			if (flag)
			{
				if (flag2)
				{
					base.part.highlightColor = Highlighter.colorPartInventoryContainer;
					base.part.Highlight(active: true);
					base.part.HighlightActive = true;
				}
				else
				{
					base.part.highlightColor = Part.defaultHighlightPart;
					base.part.Highlight(active: false);
					base.part.HighlightActive = false;
				}
				containerHighlight = flag2;
			}
		}
		if (selectedPart != null)
		{
			if (!Input.GetKeyDown(KeyCode.Escape) && (!base.vessel.isEVA || !base.vessel.evaController.JetpackDeployed))
			{
				if (GameSettings.EVA_Jump.GetKeyDown() && base.vessel.isActiveVessel)
				{
					if (selectedPart.currentCollisions.Count == 0 && partFullyCreated && placementInsideCap && placementonTerrain)
					{
						Vector3 position = selectedPart.transform.position;
						Quaternion rotation = Quaternion.Inverse(base.vessel.mainBody.bodyTransform.rotation) * selectedPart.transform.rotation;
						DeletePartObject();
						DeployGroundPart(storedParts[placementPendingIndex], position, rotation);
						GameEvents.onDeployGroundPart.Fire(storedParts[placementPendingIndex].partName);
						ClearPartAtSlot(placementPendingIndex);
						if (grid.pawInventory != null && grid.pawInventory.gameObject.activeInHierarchy)
						{
							grid.pawInventory.DestroyItem(placementPendingIndex);
						}
						placementPendingIndex = -1;
						placementCoolDown = true;
						placementCooldownTimer = Planetarium.GetUniversalTime();
						partFullyCreated = false;
					}
					else if (placementNotAllowedsFX != null && !placementNotAllowedsFX.isPlaying)
					{
						placementNotAllowedsFX.Play();
					}
					return;
				}
				if (GameSettings.RCS_TOGGLE.GetKeyDown())
				{
					selectedPart.transform.rotation = (base.part ? base.part.transform.rotation : selectedPart.initRotation);
				}
				if (placementAllowXRotation)
				{
					if (GameSettings.TRANSLATE_UP.GetKey())
					{
						selectedPart.transform.Rotate(Vector3.right * placementRotateSpeed * Time.deltaTime);
					}
					if (GameSettings.TRANSLATE_DOWN.GetKey())
					{
						selectedPart.transform.Rotate(-Vector3.right * placementRotateSpeed * Time.deltaTime);
					}
				}
				if (placementAllowYRotation)
				{
					if (GameSettings.TRANSLATE_LEFT.GetKey())
					{
						selectedPart.transform.Rotate(Vector3.up * placementRotateSpeed * Time.deltaTime);
					}
					if (GameSettings.TRANSLATE_RIGHT.GetKey())
					{
						selectedPart.transform.Rotate(-Vector3.up * placementRotateSpeed * Time.deltaTime);
					}
				}
				if (placementAllowZRotation)
				{
					if (GameSettings.TRANSLATE_FWD.GetKey())
					{
						selectedPart.transform.Rotate(Vector3.forward * placementRotateSpeed * Time.deltaTime);
					}
					if (GameSettings.TRANSLATE_BACK.GetKey())
					{
						selectedPart.transform.Rotate(-Vector3.forward * placementRotateSpeed * Time.deltaTime);
					}
				}
				SetPositionFromTerrain();
				if (selectedPart.currentCollisions.Count == 0 && placementInsideCap && placementonTerrain)
				{
					if (!placementPositionOk)
					{
						selectedPart.SetHighlightColor(Highlighter.colorPartHighlightDefault);
						selectedPart.SetHighlight(active: true, recursive: true);
						placementPositionOk = true;
					}
				}
				else if (placementPositionOk)
				{
					selectedPart.SetHighlightColor(Highlighter.colorPartEditorDetached);
					selectedPart.SetHighlight(active: true, recursive: true);
					placementPositionOk = false;
				}
			}
			else
			{
				CancelPartPlacementMode();
			}
		}
		else if (placementCoolDown && Planetarium.GetUniversalTime() - placementCooldownTimer > 1.0)
		{
			placementCoolDown = false;
			if (base.vessel.isEVA && base.vessel.evaController != null)
			{
				base.vessel.evaController.SetPartPlacementMode(mode: false, null);
			}
			if (constructorModeInventory != null)
			{
				constructorModeInventory.ResetPlacePartIcons(active: true);
			}
			else
			{
				grid.pawInventory.ResetPlacePartIcons(active: true);
			}
		}
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8002218"), InventorySlots) + "\n";
		if (HasPackedVolumeLimit || HasPackedVolumeLimit)
		{
			text += "\n\n";
		}
		if (HasPackedVolumeLimit)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003415"), Localizer.Format("<<1>><<2>>", packedVolumeLimit.ToString("0.0"), "L")) + "\n";
		}
		if (HasMassLimit)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003416"), Localizer.Format("<<1>><<2>>", massLimit.ToString("0.000"), "")) + "\n";
		}
		return text;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8002219");
	}

	public void UpdateCapacityValues()
	{
		volumeOccupied = 0f;
		massOccupied = 0f;
		for (int i = 0; i < storedParts.Count; i++)
		{
			StoredPart storedPart = storedParts.ValuesList[i];
			if (storedPart != null && storedPart.snapshot != null && storedPart.snapshot.partPrefab != null)
			{
				ModuleCargoPart moduleCargoPart = storedPart.snapshot.partPrefab.FindModuleImplementing<ModuleCargoPart>();
				if (moduleCargoPart != null)
				{
					volumeOccupied += moduleCargoPart.packedVolume * (float)storedPart.quantity;
				}
				massOccupied += GetPartMass(storedPart.snapshot.partPrefab) * (float)storedPart.quantity;
			}
		}
		UpdateMassVolumeDisplay(updateCapacities: true, constructionOnly: false);
	}

	public void InitializeMassVolumeDisplay()
	{
		if (grid != null)
		{
			grid.pawInventory.InitializeMassVolumeDisplay();
		}
		if (constructorModeInventory != null)
		{
			constructorModeInventory.InitializeMassVolumeDisplay();
		}
	}

	public void UpdateMassVolumeDisplay(bool updateCapacities, bool constructionOnly)
	{
		if (updateCapacities)
		{
			volumeCapacity = volumeOccupied;
			massCapacity = massOccupied;
		}
		if (!IgnoreKerbalInventoryLimits() && HasPackedVolumeLimit && volumeCapacity > packedVolumeLimit)
		{
			volumeCapacityReached = true;
		}
		else
		{
			volumeCapacityReached = false;
		}
		if (!IgnoreKerbalInventoryLimits() && HasMassLimit && massCapacity > massLimit)
		{
			massCapacityReached = true;
		}
		else
		{
			massCapacityReached = false;
		}
		if (HasPackedVolumeLimit)
		{
			if (grid != null && grid.pawInventory != null)
			{
				grid.pawInventory.UpdateVolumeLimits(volumeCapacity, packedVolumeLimit, volumeCapacityReached, constructionOnly);
			}
			if (constructorModeInventory != null)
			{
				constructorModeInventory.UpdateVolumeLimits(volumeCapacity, packedVolumeLimit, volumeCapacityReached, constructionOnly);
			}
		}
		if (HasMassLimit)
		{
			if (grid != null && grid.pawInventory != null)
			{
				grid.pawInventory.UpdateMassLimits(massCapacity, massLimit, massCapacityReached);
			}
			if (constructorModeInventory != null)
			{
				constructorModeInventory.UpdateMassLimits(massCapacity, massLimit, massCapacityReached);
			}
		}
	}

	public void PreviewLimits(Part newPart, int amountToStore, AvailablePart slotPart, int slotIndex)
	{
		ModuleCargoPart moduleCargoPart = newPart.FindModuleImplementing<ModuleCargoPart>();
		if (slotPart == null)
		{
			if (moduleCargoPart.packedVolume < 0f)
			{
				volumeCapacity = packedVolumeLimit * 2f;
			}
			else
			{
				volumeCapacity += moduleCargoPart.packedVolume * (float)amountToStore;
			}
			massCapacity += GetPartMass(newPart) * (float)amountToStore;
		}
		else
		{
			ModuleCargoPart moduleCargoPart2 = slotPart.partPrefab.FindModuleImplementing<ModuleCargoPart>();
			ModulePartVariants component = newPart.GetComponent<ModulePartVariants>();
			string partVariantName = ((component != null) ? component.SelectedVariant.Name : "");
			int stackAmountAtSlot = GetStackAmountAtSlot(slotIndex);
			if (!CanStackInSlot(newPart.partInfo, partVariantName, slotIndex))
			{
				if (moduleCargoPart.packedVolume < 0f)
				{
					volumeCapacity = packedVolumeLimit * 2f;
				}
				else
				{
					volumeCapacity = volumeCapacity + moduleCargoPart.packedVolume * (float)amountToStore - moduleCargoPart2.packedVolume * (float)stackAmountAtSlot;
				}
				massCapacity = massCapacity + GetPartMass(newPart) * (float)amountToStore - GetPartMass(slotPart.partPrefab) * (float)stackAmountAtSlot;
			}
			else
			{
				float num = Math.Min(amountToStore + stackAmountAtSlot, moduleCargoPart.stackableQuantity);
				float num2;
				float num3;
				if (moduleCargoPart.packedVolume < 0f)
				{
					num2 = packedVolumeLimit * 2f;
					num3 = packedVolumeLimit * 2f;
				}
				else
				{
					num2 = moduleCargoPart.packedVolume * (float)stackAmountAtSlot;
					num3 = moduleCargoPart.packedVolume * num;
				}
				volumeCapacity = volumeCapacity + num3 - num2;
				float num4 = GetPartMass(newPart) * (float)stackAmountAtSlot;
				float num5 = GetPartMass(newPart) * num;
				massCapacity = massCapacity + num5 - num4;
			}
		}
		showPreview = false;
		UpdateMassVolumeDisplay(updateCapacities: false, moduleCargoPart.packedVolume < 0f);
	}

	public void RestoreLimitsDisplay()
	{
		UpdateMassVolumeDisplay(updateCapacities: true, constructionOnly: false);
	}

	public float GetPartMass(Part part)
	{
		return part.GetResourceMass() + part.mass;
	}

	public bool HasCapacity(Part newPart)
	{
		bool flag = true;
		ModuleCargoPart moduleCargoPart = newPart.FindModuleImplementing<ModuleCargoPart>();
		if (moduleCargoPart.packedVolume < 0f || moduleCargoPart.packedVolume + volumeCapacity > packedVolumeLimit)
		{
			flag = false;
		}
		if (flag && GetPartMass(base.part) + massCapacity > massLimit)
		{
			flag = false;
		}
		return flag;
	}

	public void KerbalStateChanged()
	{
		if (grid != null && grid.pawInventory != null)
		{
			grid.pawInventory.ResetPlacePartIcons(active: true);
		}
	}

	public void ToggleInventoryVisibility(bool show)
	{
		base.Fields["InventorySlots"].guiActive = show;
		base.Fields["InventorySlots"].guiActiveEditor = show;
	}

	public void onPartActionUIOpened(Part p)
	{
		if (base.part != null && p == base.part)
		{
			UpdateModuleUI();
		}
	}

	public void InitializeInventory()
	{
		availableCargoParts = PartLoader.Instance.GetAvailableCargoPartNames();
		availableCargoInventoryParts = PartLoader.Instance.GetAvailableCargoInventoryPartNames();
		if (!hasInitialized)
		{
			if (InventorySlots > maxInventorySlots)
			{
				InventorySlots = maxInventorySlots;
			}
			if (storedParts == null)
			{
				storedParts = new DictionaryValueList<int, StoredPart>();
			}
			else
			{
				storedParts.Clear();
			}
			hasInitialized = true;
		}
	}

	public void SetInventoryDefaults()
	{
		if (defaultInventoryParts == null || defaultInventoryParts.Count < 1)
		{
			return;
		}
		hasInitialized = false;
		InitializeInventory();
		for (int i = 0; i < defaultInventoryParts.Count && i < InventorySlots; i++)
		{
			if (availableCargoParts.Contains(defaultInventoryParts[i]))
			{
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(defaultInventoryParts[i]);
				if (partInfoByName != null && ResearchAndDevelopment.PartModelPurchased(partInfoByName))
				{
					StoreCargoPartAtSlot(partInfoByName.partPrefab, FirstEmptySlot());
				}
			}
		}
		ResetInventoryPartsByName();
	}

	public void ResetInventoryPartsByName()
	{
		inventoryPartsByName = new List<string>();
		for (int i = 0; i < InventorySlots; i++)
		{
			if (storedParts != null && storedParts.ContainsKey(i))
			{
				inventoryPartsByName.Add(storedParts[i].partName);
			}
			else
			{
				inventoryPartsByName.Add("");
			}
		}
	}

	[Obsolete("Please use UIPartActionControllerInventory.Instance.CreatePartFromInventory")]
	public Part CreatePartForInventoryUse(AvailablePart partInfo)
	{
		if ((bool)UIPartActionControllerInventory.Instance)
		{
			return UIPartActionControllerInventory.Instance.CreatePartFromInventory(partInfo);
		}
		return null;
	}

	public void UpdateModuleUI()
	{
		if (hasInitialized && grid != null)
		{
			grid.updateSlotItems = true;
		}
	}

	public void CancelPartPlacementMode()
	{
		if (selectedPart != null)
		{
			placementCoolDown = true;
			placementCooldownTimer = Planetarium.GetUniversalTime();
			DeletePartObject();
		}
	}

	public int FirstEmptySlot()
	{
		int num = 0;
		while (true)
		{
			if (num < InventorySlots)
			{
				if (!storedParts.ContainsKey(num))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public int FirstFullSlot()
	{
		int num = 0;
		while (true)
		{
			if (num < InventorySlots)
			{
				if (storedParts.ContainsKey(num))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public int TotalEmptySlots()
	{
		return InventorySlots - storedParts.Count;
	}

	public override void OnCopy(PartModule fromModule)
	{
		ModuleInventoryPart moduleInventoryPart = fromModule as ModuleInventoryPart;
		storedParts = new DictionaryValueList<int, StoredPart>();
		for (int i = 0; i < moduleInventoryPart.storedParts.ValuesList.Count; i++)
		{
			storedParts.Add(moduleInventoryPart.storedParts.ValuesList[i].slotIndex, moduleInventoryPart.storedParts.ValuesList[i].Copy());
		}
	}

	public bool ContainsPart(string partName)
	{
		if (storedParts != null && storedParts.ValuesList != null)
		{
			for (int i = 0; i < storedParts.ValuesList.Count; i++)
			{
				if (storedParts.ValuesList[i].partName.Equals(partName))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void DeployGroundPart(StoredPart storedPart, Vector3 partPosition, Quaternion rotation)
	{
		ConfigNode protoVesselNode = GetProtoVesselNode(storedPart, partPosition, rotation);
		HighLogic.CurrentGame.AddVessel(protoVesselNode);
	}

	public ConfigNode GetProtoVesselNode(StoredPart storedPart, Vector3 partPosition, Quaternion rotation)
	{
		uint uniqueFlightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		Orbit orbit = Orbit.CreateRandomOrbitAround(base.vessel.mainBody);
		string vesselName = storedPart.partName;
		VesselType vesselType = VesselType.Unknown;
		if (storedPart.snapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = storedPart.snapshot.FindModule("ModuleGroundExpControl");
			ProtoPartModuleSnapshot protoPartModuleSnapshot2 = storedPart.snapshot.FindModule("ModuleGroundSciencePart");
			ProtoPartModuleSnapshot protoPartModuleSnapshot3 = storedPart.snapshot.FindModule("ModuleGroundExperiment");
			vesselName = storedPart.snapshot.partInfo.title;
			if (protoPartModuleSnapshot != null || protoPartModuleSnapshot2 != null || protoPartModuleSnapshot3 != null)
			{
				vesselType = VesselType.DeployedScienceController;
			}
		}
		new ConfigNode();
		ConfigNode configNode = ProtoVessel.CreateVesselNode(vesselName, vesselType, orbit, 0, new ConfigNode[1] { CreatePartNode(uniqueFlightID, storedPart) });
		configNode.SetValue("landedAt", base.vessel.mainBody.name);
		double alt = base.vessel.altitude;
		double lat = base.vessel.latitude;
		double lon = base.vessel.longitude;
		base.vessel.mainBody.GetLatLonAlt(partPosition, out lat, out lon, out alt);
		configNode.SetValue("lat", lat);
		configNode.SetValue("lon", lon);
		configNode.SetValue("sit", Vessel.Situations.LANDED.ToString());
		configNode.SetValue("alt", alt);
		configNode.SetValue("landed", newValue: true);
		configNode.SetValue("splashed", newValue: false);
		configNode.SetValue("skipGroundPositioning", newValue: false);
		configNode.SetValue("vesselSpawning", newValue: true);
		configNode.AddValue("prst", value: true);
		_ = (Vector3)base.vessel.mainBody.GetRelSurfacePosition(lat, lon, alt);
		configNode.AddValue("rot", rotation);
		configNode.SetValue("skipGroundPositioning", newValue: false);
		configNode.SetValue("PQSMin", 0, createIfNotFound: true);
		configNode.SetValue("PQSMax", 0, createIfNotFound: true);
		return configNode;
	}

	public ConfigNode CreatePartNode(uint id, StoredPart storedPart)
	{
		ConfigNode configNode = new ConfigNode("PART");
		storedPart.snapshot.Save(configNode);
		configNode.SetValue("flag", base.part.flagURL, createIfNotFound: true);
		configNode.SetValue("uid", id, createIfNotFound: true);
		configNode.SetValue("mid", id, createIfNotFound: true);
		configNode.SetValue("state", 0);
		ConfigNode[] nodes = configNode.GetNodes("MODULE");
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = "";
			ConfigNode configNode2 = nodes[i];
			if (!configNode2.TryGetValue("name", ref value))
			{
				continue;
			}
			int value2 = 0;
			float num = 0f;
			if (configNode2.TryGetValue("powerUnitsProduced", ref value2) && value2 > 0 && base.vessel.isEVA && base.vessel.parts[0].protoModuleCrew[0].HasEffect<DeployedSciencePowerSkill>())
			{
				DeployedSciencePowerSkill effect = base.vessel.parts[0].protoModuleCrew[0].GetEffect<DeployedSciencePowerSkill>();
				if (effect != null)
				{
					num = effect.GetValue();
				}
				configNode2.SetValue("powerUnitsProduced", (float)value2 + num);
			}
			float num2 = 0f;
			if (value == "ModuleGroundExperiment" && base.vessel.isEVA && base.vessel.parts[0].protoModuleCrew[0].HasEffect<DeployedScienceExpSkill>())
			{
				DeployedScienceExpSkill effect2 = base.vessel.parts[0].protoModuleCrew[0].GetEffect<DeployedScienceExpSkill>();
				if (effect2 != null)
				{
					num2 = effect2.GetValue() * 100f;
					configNode2.SetValue("ScienceModifierRate", num2, createIfNotFound: true);
				}
			}
			if (configNode2.HasValue("beingDeployed"))
			{
				configNode2.SetValue("beingDeployed", newValue: true);
			}
		}
		return configNode;
	}

	public int TotalAmountOfPartStored(string requestedPartName)
	{
		int num = -1;
		int count = storedParts.Count;
		while (count-- > 0)
		{
			StoredPart storedPart = storedParts.ValuesList[count];
			if (storedPart != null && storedPart.partName.Equals(requestedPartName))
			{
				if (num == -1)
				{
					num = 0;
				}
				num += storedPart.quantity;
			}
		}
		return num;
	}

	public bool RemoveNPartsFromInventory(string requestedPartName, int n, bool playSound = false)
	{
		int num = n;
		EffectList effectList = null;
		if (TotalAmountOfPartStored(requestedPartName) < n)
		{
			return false;
		}
		int count = storedParts.Count;
		StoredPart storedPart;
		while (true)
		{
			if (count-- > 0)
			{
				storedPart = storedParts.ValuesList[count];
				if (storedPart != null && storedPart.partName.Equals(requestedPartName))
				{
					if (playSound)
					{
						effectList = storedPart.snapshot.partPrefab.Effects;
					}
					if (storedPart.quantity > num)
					{
						storedPart.quantity -= num;
						effectList?.PlayRandomEffect();
						GameEvents.onModuleInventorySlotChanged.Fire(this, storedPart.slotIndex);
						return true;
					}
					if (storedPart.quantity == num)
					{
						break;
					}
					num -= storedPart.quantity;
					ClearPartAtSlot(storedPart.slotIndex);
					GameEvents.onModuleInventorySlotChanged.Fire(this, storedPart.slotIndex);
				}
				continue;
			}
			Debug.LogWarning("[ModuleInventoryPart]: We removed some of part " + requestedPartName + "- but we ran out of them");
			return false;
		}
		ClearPartAtSlot(storedPart.slotIndex);
		effectList?.PlayRandomEffect();
		GameEvents.onModuleInventorySlotChanged.Fire(this, storedPart.slotIndex);
		return true;
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		float num = 0f;
		if (storedParts == null)
		{
			return num;
		}
		if (storedParts.Count > 0)
		{
			for (int i = 0; i < storedParts.Count; i++)
			{
				if (storedParts.ValuesList[i] == null || storedParts.ValuesList[i].snapshot == null)
				{
					continue;
				}
				StoredPart storedPart = storedParts.ValuesList[i];
				num += storedPart.snapshot.mass * (float)Math.Max(1, storedPart.quantity);
				float num2 = 0f;
				int count = storedPart.snapshot.resources.Count;
				while (count-- > 0)
				{
					ProtoPartResourceSnapshot protoPartResourceSnapshot = storedPart.snapshot.resources[count];
					if (protoPartResourceSnapshot != null && protoPartResourceSnapshot.definition != null)
					{
						num2 += protoPartResourceSnapshot.definition.density * (float)protoPartResourceSnapshot.amount;
					}
				}
				num += num2 * (float)Math.Max(1, storedPart.quantity);
			}
		}
		return num;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		float num = 0f;
		float dryCost = 0f;
		float fuelCost = 0f;
		if (InventoryItemCount > 0)
		{
			for (int i = 0; i < storedParts.Count; i++)
			{
				if (storedParts.ValuesList[i] != null)
				{
					StoredPart storedPart = storedParts.ValuesList[i];
					if (storedPart.snapshot != null)
					{
						ShipConstruction.GetPartCosts(storedPart.snapshot, storedPart.snapshot.partInfo, out dryCost, out fuelCost);
						num += (fuelCost + dryCost) * (float)Math.Max(1, storedPart.quantity);
					}
				}
			}
		}
		return num;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public bool IgnoreKerbalInventoryLimits()
	{
		if (CheatOptions.IgnoreKerbalInventoryLimits)
		{
			return base.part.isKerbalEVA();
		}
		return false;
	}
}
