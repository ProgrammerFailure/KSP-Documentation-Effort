using System;
using System.Collections;
using System.Collections.Generic;
using EditorGizmos;
using Expansions;
using Expansions.Serenity;
using FinePrint.Utilities;
using Highlighting;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EVAConstructionModeEditor : EditorLogicBase
{
	public class SelectedPartCollider
	{
		public Collider collider;

		public bool isTrigger;

		public SelectedPartCollider(Collider col)
		{
			collider = col;
			isTrigger = col.isTrigger;
		}
	}

	public ConstructionMode evaConstructionMode;

	public Part lastAttachedPart;

	public Part rootPart;

	[SerializeField]
	public AudioSource audioSource;

	public AudioClip attachClip;

	public AudioClip deletePartClip;

	public AudioClip partGrabClip;

	public AudioClip partReleaseClip;

	public AudioClip cannotPlaceClip;

	public AudioClip tweakGrabClip;

	public AudioClip tweakReleaseClip;

	public AudioClip reRootClip;

	public Part backedUpPart;

	public AttachNode backedUpAttachment;

	public AttachNode backedUpParentAttachment;

	[SerializeField]
	public UIOnClick angleSnapButton;

	[SerializeField]
	public UIStateImage angleSnapSprite;

	[SerializeField]
	public List<Vessel> attachVessels;

	public Quaternion gizmoAttRotate;

	public Quaternion gizmoAttRotate0;

	public GizmoRotate gizmoRotate;

	public GizmoOffset gizmoOffset;

	[SerializeField]
	public Button coordSpaceBtn;

	[SerializeField]
	public TextMeshProUGUI coordSpaceText;

	public Space symmetryCoordSpace = Space.Self;

	public bool skipPartAttach;

	public Ray ray;

	public RaycastHit hit;

	public Collider FairingHitCollider;

	[SerializeField]
	public Attachment attachment;

	public Attachment[] cPartAttachments;

	public bool allowSrfAttachment = true;

	public bool allowNodeAttachment = true;

	public bool attachSuccesful;

	public bool isPlacementValid;

	public bool isPlacementOnGround;

	public Quaternion selectedPartOriginalRotation;

	public float placementGroundOffset = 0.1f;

	public Quaternion vesselRotation = Quaternion.identity;

	public float dragSharpness = 0.3f;

	public float srfAttachAngleSnap = 15f;

	public float srfAttachAngleSnapFine = 5f;

	public List<SelectedPartCollider> selectedPartColliders = new List<SelectedPartCollider>();

	public int assistingKerbals;

	public double combinedConstructionWeightLimit;

	[SerializeField]
	public bool attachmentPossible;

	public Vector3 offsetGap;

	public float threshold;

	public AttachNode childToParent;

	public AttachNode parentToChild;

	public Vector3 diff;

	public ConstructionMode EVAConstructionMode => evaConstructionMode;

	public Part SelectedPart => selectedPart;

	public Part LastAttachedPart => lastAttachedPart;

	public Part RootPart => rootPart;

	public bool IsPlacementValid => isPlacementValid;

	public int AssistingKerbals => assistingKerbals;

	public double CombinedConstructionWeightLimit => combinedConstructionWeightLimit;

	public void Awake()
	{
		GameEvents.OnEVAConstructionModeChanged.Add(OnConstructionModeChanged);
		GameEvents.OnInventoryPartOnMouseChanged.Add(OnCurrentMousePartChanged);
		GameEvents.OnEVAConstructionMode.Add(OnConstructionMode);
		GameEvents.onVesselUnloaded.Add(OnVesselUnloaded);
		GameEvents.onGameAboutToQuicksave.Add(OnGameAboutToQuicksave);
		GameEvents.OnMapEntered.Add(OnMapViewEntered);
		coordSpaceBtn.onClick.AddListener(ChangeCoordSpace);
		modeMsg = new ScreenMessage("", 5f, ScreenMessageStyle.LOWER_CENTER);
		interactMsg = new ScreenMessage("", 5f, ScreenMessageStyle.LOWER_CENTER);
		angleSnapButton.onClick.Add(SnapButton);
		checkInputLocks = false;
	}

	public void Start()
	{
		if (audioSource == null)
		{
			audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		audioSource.volume = GameSettings.UI_VOLUME;
		attachVessels = new List<Vessel>();
	}

	public void Update()
	{
		if (!EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != 0)
		{
			return;
		}
		UpdateAssistingKerbals();
		SnapInputUpdate();
		if (selectedPart != null)
		{
			selectedPart.OnConstructionModeUpdate();
			for (int i = 0; i < selectedPart.Modules.Count; i++)
			{
				selectedPart.Modules[i].OnConstructionModeUpdate();
			}
		}
		switch (EVAConstructionMode)
		{
		case ConstructionMode.Place:
			InputCheckDetachCompundPart();
			UpdatePartPlacementPosition();
			if (!InputLockManager.IsLocked(ControlTypes.EDITOR_SOFT_LOCK))
			{
				AttachInput();
				DetachInput();
				DropPartInput();
				PickupPartInput();
			}
			if (selectedPart != null)
			{
				ProcessAttachNodes();
				partRotationInputUpdate();
			}
			else
			{
				RemoveAttachNodes();
			}
			attachSuccesful = false;
			break;
		case ConstructionMode.Move:
			RemoveAttachNodes();
			SelectPartInput();
			CoordToggleInput();
			break;
		case ConstructionMode.Rotate:
			RemoveAttachNodes();
			SelectPartInput();
			CoordToggleInput();
			break;
		}
	}

	public void FixedUpdate()
	{
		if (EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction && selectedPart != null)
		{
			selectedPart.OnConstructionModeFixedUpdate();
			for (int i = 0; i < selectedPart.Modules.Count; i++)
			{
				selectedPart.Modules[i].OnConstructionModeFixedUpdate();
			}
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnEVAConstructionModeChanged.Remove(OnConstructionModeChanged);
		GameEvents.OnInventoryPartOnMouseChanged.Remove(OnCurrentMousePartChanged);
		GameEvents.OnEVAConstructionMode.Remove(OnConstructionMode);
		coordSpaceBtn.onClick.RemoveListener(ChangeCoordSpace);
		GameEvents.onVesselUnloaded.Remove(OnVesselUnloaded);
		GameEvents.onGameAboutToQuicksave.Remove(OnGameAboutToQuicksave);
		GameEvents.OnMapEntered.Remove(OnMapViewEntered);
		RemoveAttachNodes();
	}

	public void OnCurrentMousePartChanged(Part p)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (selectedPart != null)
		{
			isCurrentPartFlag = selectedPart.FindModuleImplementing<FlagDecalBackground>() != null;
			ToggleCollidersTrigger(toggle: false);
			handleAttachNodeIcons(selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
		}
		else
		{
			isCurrentPartFlag = false;
		}
		bool currentPartIsAttachable = false;
		if (p != null && p.HasModuleImplementing<ModuleCargoPart>())
		{
			currentPartIsAttachable = true;
			InterruptKerbalWelding();
			selectedPart = p;
			switch (evaConstructionMode)
			{
			case ConstructionMode.Place:
				FetchColliders();
				ToggleCollidersTrigger(toggle: true);
				ToggleOffsetGizmo(activate: false);
				ToggleRotateGizmo(activate: false);
				ToggleCoordSpaceButton(toggle: false);
				break;
			case ConstructionMode.Move:
				ToggleRotateGizmo(activate: false);
				ToggleOffsetGizmo(selectedPart.PartCanBeOffset());
				ToggleCoordSpaceButton(toggle: true);
				break;
			case ConstructionMode.Rotate:
				ToggleRotateGizmo(selectedPart.PartCanBeRotated());
				ToggleOffsetGizmo(activate: false);
				ToggleCoordSpaceButton(toggle: true);
				break;
			}
		}
		else
		{
			DestroyHeldPart();
		}
		if (UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.currentPartIsAttachable = currentPartIsAttachable;
		}
	}

	public void OnConstructionModeChanged(ConstructionMode mode)
	{
		if (mode == evaConstructionMode)
		{
			return;
		}
		switch (mode)
		{
		case ConstructionMode.Place:
			if (selectedPart != null && (evaConstructionMode == ConstructionMode.Move || evaConstructionMode == ConstructionMode.Rotate))
			{
				if (selectedPart.previousPhysicalSignificance != selectedPart.physicalSignificance)
				{
					StartCoroutine(ResetToPhysicalPart(selectedPart));
				}
				ToggleCollidersTrigger(toggle: false);
				selectedPart = null;
				rootPart = null;
				if (UIPartActionControllerInventory.Instance != null)
				{
					if (UIPartActionControllerInventory.Instance.CurrentInventory != null)
					{
						UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
					}
					UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
				}
			}
			ToggleRotateGizmo(activate: false);
			ToggleOffsetGizmo(activate: false);
			ToggleCoordSpaceButton(toggle: false);
			break;
		case ConstructionMode.Move:
			if (selectedPart != null && selectedPart.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				selectedPart.DemoteToPhysicslessPart();
			}
			ToggleRotateGizmo(activate: false);
			ToggleOffsetGizmo(selectedPart != null && selectedPart.PartCanBeOffset());
			ToggleCoordSpaceButton(toggle: true);
			break;
		case ConstructionMode.Rotate:
			if (selectedPart != null && selectedPart.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				selectedPart.DemoteToPhysicslessPart();
			}
			ToggleRotateGizmo(selectedPart != null && selectedPart.PartCanBeRotated());
			ToggleOffsetGizmo(activate: false);
			ToggleCoordSpaceButton(toggle: true);
			break;
		}
		evaConstructionMode = mode;
	}

	public void OnGameAboutToQuicksave()
	{
		PutPartBackInInventory();
	}

	public void OnMapViewEntered()
	{
		PutPartBackInInventory();
	}

	public IEnumerator ResetToPhysicalPart(Part p)
	{
		p.PromoteToPhysicalPart();
		yield return null;
		p.rb.isKinematic = p.packed;
		p.ModulesBeforePartAttachJoint();
		if (p.physicalSignificance == Part.PhysicalSignificance.FULL)
		{
			p.CreateAttachJoint(p.attachMode);
		}
		p.CycleAutoStrut();
	}

	public void UpdatePartPlacementPosition()
	{
		isPlacementValid = false;
		isPlacementOnGround = false;
		if ((!(UIPartActionControllerInventory.Instance != null) || !UIPartActionControllerInventory.Instance.IsCursorOverAnyPAWOrCargoPane) && !UIPartActionControllerInventory.heldPartIsStack && !EVAConstructionModeController.Instance.Hover && !(FlightGlobals.ActiveVessel == null) && FlightGlobals.ActiveVessel.isEVA && !(selectedPart == null))
		{
			_ = selectedPart.transform.position;
			this.ray = Camera.main.ScreenPointToRay(Input.mousePosition - srfAttachCursorOffset);
			if (Physics.Raycast(this.ray, out hit, 10000f, 32768))
			{
				Vector3 point = hit.point;
				Vector3d upAxis = FlightGlobals.getUpAxis(FlightGlobals.ActiveVessel.mainBody, point);
				float centerPointOffset = 0f;
				float boundsPoints = selectedPart.GetBoundsPoints(hit.normal, out centerPointOffset);
				boundsPoints += placementGroundOffset;
				point += upAxis * boundsPoints;
				if (Vector3.Distance(point, FlightGlobals.ActiveVessel.GetWorldPos3D()) < GameSettings.EVA_CONSTRUCTION_RANGE)
				{
					selectedPart.transform.position = point;
					isPlacementValid = true;
					isPlacementOnGround = true;
				}
			}
			if (!isPlacementValid)
			{
				Vector3 position = FlightGlobals.ActiveVessel.ReferenceTransform.position;
				Plane plane = new Plane(FlightCamera.fetch.transform.forward, position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float enter = 0f;
				plane.Raycast(ray, out enter);
				Vector3 point2 = ray.GetPoint(enter);
				if (Vector3.Distance(point2, FlightGlobals.ActiveVessel.ReferenceTransform.position) < GameSettings.EVA_CONSTRUCTION_RANGE)
				{
					selectedPart.transform.position = point2;
					isPlacementValid = true;
				}
				else
				{
					isPlacementValid = false;
				}
			}
			vesselRotation = selectedPartOriginalRotation;
			selectedPart.transform.rotation = vesselRotation * selectedPart.attRotation;
			attachment = CheckAttach(selectedPart);
			attachmentPossible = attachment.possible && Vector3.Distance(attachment.position, FlightGlobals.ActiveVessel.ReferenceTransform.position) < GameSettings.EVA_CONSTRUCTION_RANGE;
			if (attachmentPossible)
			{
				selectedPart.attRotation0 = attachment.rotation;
				selectedPart.attPos0 = attachment.position;
				selectedPart.transform.SetParent(null, worldPositionStays: true);
				selectedPart.transform.rotation = attachment.rotation * selectedPart.attRotation;
				selectedPart.transform.position = attachment.position;
			}
			else
			{
				selectedPart.transform.rotation = selectedPartOriginalRotation * selectedPart.attRotation;
			}
			bool flag = Mouse.HoveredPart != null;
			isPlacementValid = !flag || (flag && (selectedPart.attachRules.allowSrfAttach || selectedPart.attachRules.allowStack));
			int count = selectedPart.cargoColliders.Count;
			while (count-- > 0)
			{
				if (selectedPart.cargoColliders[count] == null)
				{
					selectedPart.cargoColliders.RemoveAt(count);
					continue;
				}
				Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(selectedPart.cargoColliders[count].gameObject);
				if (!(partUpwardsCached != null))
				{
					isPlacementValid = false;
					attachmentPossible = false;
					continue;
				}
				if (attachment.potentialParent == null)
				{
					isPlacementValid = false;
					attachmentPossible = false;
				}
				else if (partUpwardsCached.persistentId != attachment.potentialParent.persistentId && partUpwardsCached.persistentId != selectedPart.persistentId)
				{
					isPlacementValid = false;
					attachmentPossible = false;
				}
				break;
			}
			HighLightPossibleAttach(attachmentPossible || isPlacementValid);
			selectedPart.potentialParent = attachment.potentialParent;
			if (Mouse.IsMoving)
			{
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDragging, selectedPart);
			}
		}
		else
		{
			attachment = null;
			attachmentPossible = false;
			if (selectedPart == null)
			{
				HighLightPossibleAttach(highlight: false);
			}
		}
	}

	public void HighLightPossibleAttach(bool highlight)
	{
		if (!(selectedPart == null))
		{
			Color highlightColor = (highlight ? Highlighter.colorPartEditorAttached : Highlighter.colorPartEditorDetached);
			if (highlight && IsPlacementValid && !attachmentPossible)
			{
				highlightColor = Highlighter.colorConstructionPartDropAsNewVessel;
			}
			selectedPart.SetHighlightColor(highlightColor);
			Part.HighlightType highlightType = ((!highlight) ? Part.HighlightType.OnMouseOver : Part.HighlightType.AlwaysOn);
			selectedPart.SetHighlightType(highlightType);
			selectedPart.SetHighlight(active: true, recursive: true);
			selectedPart.highlighter.UpdateHighlighting(isDepthAvailable: true);
		}
	}

	public void PickupPartInput()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0) && selectedPart == null && Mouse.HoveredPart != null && Vector3.Distance(Mouse.HoveredPart.transform.position, FlightGlobals.ActiveVessel.GetWorldPos3D()) < GameSettings.EVA_CONSTRUCTION_RANGE && UIPartActionControllerInventory.Instance != null && !UIPartActionControllerInventory.Instance.IsCursorOverAnyPAWOrCargoPane && !EVAConstructionModeController.Instance.Hover && !EventSystem.current.IsPointerOverGameObject())
		{
			PickupPart();
		}
	}

	public void PickupPart()
	{
		if (attachSuccesful)
		{
			return;
		}
		Part hoveredPart = Mouse.HoveredPart;
		if (hoveredPart.vessel != null && (hoveredPart.vessel.vesselType == VesselType.DroppedPart || hoveredPart.vessel.vesselType == VesselType.Debris))
		{
			if (!CanPartBeEdited(hoveredPart, weightOnlyCheck: false))
			{
				return;
			}
			if (hoveredPart.protoPartSnapshot != null)
			{
				hoveredPart.protoPartSnapshot = new ProtoPartSnapshot(hoveredPart, hoveredPart.vessel.protoVessel);
				UIPartActionControllerInventory.Instance.CurrentCargoPart = UIPartActionControllerInventory.Instance.CreatePartFromInventory(hoveredPart.protoPartSnapshot);
				if (hoveredPart.vessel != null)
				{
					BackupPart(hoveredPart, null, droppedPart: true);
					hoveredPart.vessel.Die();
				}
				UIPartActionControllerInventory.Instance.isSetAsPart = true;
			}
			if (selectedPart != null)
			{
				selectedPart.attRotation = Quaternion.identity;
				selectedPart.attRotation0 = hoveredPart.transform.localRotation;
				selectedPart.attPos0 = hoveredPart.transform.localPosition;
				selectedPartOriginalRotation = hoveredPart.transform.rotation;
				selectedPart.gameObject.SetLayerRecursive(0, filterTranslucent: true, 2097152);
				if (selectedPart.rb == null)
				{
					selectedPart.rb = selectedPart.gameObject.AddComponent<Rigidbody>();
				}
				selectedPart.rb.isKinematic = true;
				selectedPart.rb.useGravity = false;
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(selectedPart.partInfo.name);
				if (partInfoByName != null)
				{
					selectedPart.mass = partInfoByName.partPrefab.mass;
				}
				FetchColliders();
				ToggleCollidersTrigger(toggle: true);
				PlayAudioClip(partGrabClip);
				CreateSelectedPartIcon();
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartPicked, hoveredPart);
			}
		}
		FlightGlobals.ActiveVessel.evaController.InterruptWeld();
	}

	public void RemoveAttachNodes()
	{
		for (int i = 0; i < attachVessels.Count; i++)
		{
			handleChildNodeIcons(attachVessels[i].rootPart);
			handleAttachNodeIcons(attachVessels[i].rootPart, stackNodes: false, srfNodes: false, dockNodes: false);
		}
		if (selectedPart != null)
		{
			handleAttachNodeIcons(selectedPart, stackNodes: false, srfNodes: false, dockNodes: false);
		}
	}

	public void ProcessAttachNodes()
	{
		Vector3 b = FlightGlobals.ActiveVessel.GetWorldPos3D();
		UpdateVesselsInConstructionRange();
		for (int i = 0; i < attachVessels.Count; i++)
		{
			if (!(attachVessels[i] != null))
			{
				continue;
			}
			Vessel vessel = attachVessels[i];
			int j = 0;
			for (int count = vessel.parts.Count; j < count; j++)
			{
				float num = Vector3.Distance(vessel.parts[j].transform.position, b);
				if (GameSettings.EVA_CONSTRUCTION_RANGE > num)
				{
					handleAttachNodeIcons(vessel.parts[j], selectedPart.attachRules.stack, selectedPart.attachRules.srfAttach, selectedPart.attachRules.allowDock);
				}
				else
				{
					handleAttachNodeIcons(vessel.parts[j], stackNodes: false, srfNodes: false, dockNodes: false);
				}
			}
		}
		handleAttachNodeIcons(selectedPart, selectedPart.attachRules.stack, selectedPart.attachRules.srfAttach, selectedPart.attachRules.allowDock);
		handleChildNodeIcons(selectedPart);
	}

	public void UpdateVesselsInConstructionRange()
	{
		Vector3 b = FlightGlobals.ActiveVessel.GetWorldPos3D();
		for (int i = 0; i < FlightGlobals.VesselsLoaded.Count; i++)
		{
			float num = Vector3.Distance(FlightGlobals.VesselsLoaded[i].transform.position, b) - FlightGlobals.VesselsLoaded[i].vesselSize.magnitude;
			if (!attachVessels.Contains(FlightGlobals.VesselsLoaded[i]))
			{
				if (GameSettings.EVA_CONSTRUCTION_RANGE > num)
				{
					attachVessels.Add(FlightGlobals.VesselsLoaded[i]);
				}
			}
			else if (GameSettings.EVA_CONSTRUCTION_RANGE <= num)
			{
				handleChildNodeIcons(FlightGlobals.VesselsLoaded[i].rootPart);
				handleAttachNodeIcons(FlightGlobals.VesselsLoaded[i].rootPart, stackNodes: false, srfNodes: false, dockNodes: false);
				attachVessels.Remove(FlightGlobals.VesselsLoaded[i]);
			}
		}
		int count = attachVessels.Count;
		while (count-- > 0)
		{
			if (attachVessels[count] != null && !attachVessels[count].loaded)
			{
				handleChildNodeIcons(FlightGlobals.VesselsLoaded[count].rootPart);
				handleAttachNodeIcons(FlightGlobals.VesselsLoaded[count].rootPart, stackNodes: false, srfNodes: false, dockNodes: false);
				attachVessels.RemoveAt(count);
			}
			else if (attachVessels[count] == null)
			{
				attachVessels.RemoveAt(count);
			}
		}
	}

	public void OnVesselUnloaded(Vessel vessel)
	{
		handleChildNodeIcons(vessel.rootPart);
		handleAttachNodeIcons(vessel.rootPart, stackNodes: false, srfNodes: false, dockNodes: false);
	}

	public void AttachInput()
	{
		if (!Input.GetMouseButtonUp(0) || !(selectedPart != null) || attachment == null || !attachmentPossible || EventSystem.current.IsPointerOverGameObject() || !CanPartBeEdited(selectedPart, weightOnlyCheck: true))
		{
			return;
		}
		Part data = AttachPart(selectedPart, attachment);
		ToggleCollidersTrigger(toggle: false);
		attachSuccesful = true;
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartAttached, selectedPart);
		GameEvents.OnEVAConstructionModePartAttached.Fire(attachment.potentialParent.vessel, data);
		lastAttachedPart = data;
		selectedPart = null;
		if (UIPartActionControllerInventory.Instance != null)
		{
			if (UIPartActionControllerInventory.Instance.CurrentInventory != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked != null)
			{
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
			}
			UIPartActionControllerInventory.Instance.CurrentCargoPart = null;
			UIPartActionControllerInventory.Instance.ResetInventoryCacheValues(destroyCurrentCargoPart: false);
		}
		DestroyBackupPart();
	}

	public override void partRotationResetUpdate()
	{
		if (GameSettings.Editor_resetRotation.GetKeyDown() && selectedPart.transform == selectedPart.GetReferenceTransform())
		{
			srfAttachCursorOffset = Vector3.zero;
			selectedPart.attRotation = Quaternion.identity;
			selectedPart.transform.localRotation = selectedPart.attRotation0;
			partTweaked = true;
		}
	}

	public Part AttachPart(Part part, Attachment attach)
	{
		if (!(part == null) && attach != null && !(attach.potentialParent == null))
		{
			_ = part.transform.localPosition;
			_ = part.transform.localRotation;
			part.FindModuleImplementing<ModuleCargoPart>().beingAttached = true;
			SetModuleCargoPartValue(part, "beingAttached", true);
			Part part2 = part.protoPartSnapshot.CreatePart();
			part2.isAttached = true;
			part2.partInfo = part.partInfo;
			int value = 0;
			if (part.protoPartSnapshot.partInfo.partConfig.TryGetValue("PhysicsSignificance", ref value))
			{
				part2.physicalSignificance = (Part.PhysicalSignificance)value;
			}
			part2.transform.rotation = part.transform.rotation;
			part2.transform.position = part.transform.position;
			UnityEngine.Object.Destroy(part.gameObject);
			if (UIPartActionControllerInventory.Instance != null)
			{
				UIPartActionControllerInventory.Instance.DestroyHeldPartAsIcon();
			}
			if (attach.callerPartNode != null)
			{
				if (attach.callerPartNode.owner.persistentId == part.persistentId)
				{
					AttachNode attachNode = part2.FindAttachNode(attach.callerPartNode.id);
					if (attachNode != null)
					{
						attachNode.attachedPart = attach.potentialParent;
					}
					else if (attach.callerPartNode.id == part2.srfAttachNode.id)
					{
						part2.srfAttachNode.attachedPart = attach.potentialParent;
						part2.srfAttachNode.srfAttachMeshName = attach.callerPartNode.srfAttachMeshName;
					}
				}
				else
				{
					attach.callerPartNode.attachedPart = attach.potentialParent;
				}
			}
			if (attach.otherPartNode != null)
			{
				attach.otherPartNode.attachedPart = part2;
			}
			part2.attPos0 = part.transform.localPosition;
			part2.attRotation0 = part.transform.localRotation;
			part2.OnAttachFlight(attach.potentialParent);
			AttachModes mode = attach.mode;
			if (mode == AttachModes.SRF_ATTACH)
			{
				part2.attachMode = AttachModes.SRF_ATTACH;
				part2.srfAttachNode.attachedPart = attach.potentialParent;
			}
			part2.SetHighlightColor(Highlighter.colorPartHighlightDefault);
			part2.SetHighlightType(Part.HighlightType.AlwaysOn);
			part2.SetHighlight(active: true, recursive: true);
			PlayAudioClip(attachClip);
			FlightGlobals.ActiveVessel.evaController.Weld(part2);
			if (part2.isCompund)
			{
				selectedCompoundPart = part2 as CompoundPart;
			}
			return part2;
		}
		return null;
	}

	public void SetModuleCargoPartValue(Part part, string name, object value)
	{
		ModuleCargoPart moduleCargoPart = part.FindModuleImplementing<ModuleCargoPart>();
		if (moduleCargoPart != null)
		{
			part.protoPartSnapshot.FindModule(moduleCargoPart.moduleName)?.moduleValues.SetValue(name, value.ToString(), createIfNotFound: true);
		}
	}

	public void DetachInput()
	{
		if (!(selectedPart != null))
		{
			Part hoveredPart = Mouse.HoveredPart;
			if (!attachSuccesful && Input.GetMouseButtonUp(0) && hoveredPart != null && hoveredPart.HasModuleImplementing<ModuleCargoPart>() && Vector3.Distance(hoveredPart.transform.position, FlightGlobals.ActiveVessel.GetWorldPos3D()) < GameSettings.EVA_CONSTRUCTION_RANGE && hoveredPart.vessel != null && hoveredPart.vessel.rootPart.persistentId != hoveredPart.persistentId && UIPartActionControllerInventory.Instance != null && !UIPartActionControllerInventory.Instance.IsCursorOverAnyPAWOrCargoPane && !EVAConstructionModeController.Instance.Hover && !EventSystem.current.IsPointerOverGameObject() && CanPartBeEdited(hoveredPart, weightOnlyCheck: false))
			{
				Vessel vessel = hoveredPart.vessel;
				DetachPart(hoveredPart);
				PlayAudioClip(partGrabClip);
				GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDetached, hoveredPart);
				GameEvents.OnEVAConstructionModePartDetached.Fire(vessel, hoveredPart);
			}
		}
	}

	public void DetachPart(Part part)
	{
		Part parent = part.parent;
		Vector3 localPosition = part.transform.localPosition;
		selectedPartOriginalRotation = part.transform.rotation;
		BackupPart(part, parent);
		part.OnDetachFlight();
		part.attRotation = Quaternion.identity;
		part.attRotation0 = part.transform.localRotation;
		part.attPos0 = localPosition;
		part.RefreshHighlighter();
		if (UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.CurrentCargoPart = part;
			UIPartActionControllerInventory.Instance.isSetAsPart = true;
		}
		isCurrentPartFlag = selectedPart != null && selectedPart.FindModuleImplementing<FlagDecalBackground>() != null;
		CreateSelectedPartIcon();
		FetchColliders();
		ToggleCollidersTrigger(toggle: true);
	}

	public void CopyActionGroups(Part sPart, Part cPart)
	{
		if (sPart == null || cPart == null || sPart == cPart)
		{
			return;
		}
		int count = cPart.Modules.Count;
		while (count-- > 0)
		{
			if (sPart.Modules[count].Actions.Count == cPart.Modules[count].Actions.Count)
			{
				for (int i = 0; i < cPart.Modules[count].Actions.Count; i++)
				{
					cPart.Modules[count].Actions[i].actionGroup = sPart.Modules[count].Actions[i].actionGroup;
				}
			}
		}
		int count2 = sPart.children.Count;
		while (count2-- > 0)
		{
			CopyActionGroups(sPart.children[count2], cPart.children[count2]);
		}
	}

	public void OnPartDropped()
	{
		if (selectedPart != null && !selectedPart.isAttached && UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.ReturnHeldPart();
		}
	}

	public void BackupPart(Part part, Part partParent, bool droppedPart = false)
	{
		if (part == null || (partParent == null && !droppedPart))
		{
			return;
		}
		if (backedUpPart != null)
		{
			DestroyBackupPart();
		}
		backedUpPart = UnityEngine.Object.Instantiate(part);
		backedUpPart.gameObject.SetActive(value: false);
		if (partParent != null)
		{
			backedUpPart.parent = partParent;
		}
		ConfigurableJoint[] components = backedUpPart.gameObject.GetComponents<ConfigurableJoint>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] != null)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
		ModuleRoboticServoPiston moduleRoboticServoPiston = part.FindModuleImplementing<ModuleRoboticServoPiston>();
		if (moduleRoboticServoPiston != null)
		{
			UnityEngine.Object.Destroy(moduleRoboticServoPiston.DebugServoJoint);
		}
		if (!droppedPart)
		{
			backedUpAttachment = AttachNode.Clone(part.FindAttachNodeByPart(partParent));
			backedUpParentAttachment = partParent.FindAttachNodeByPart(part);
		}
		backedUpPart.attachMode = part.attachMode;
		backedUpPart.name = backedUpPart.name.Replace("(Clone)", "(Backup)");
		backedUpPart.protoPartSnapshot = ((part.protoPartSnapshot != null) ? part.protoPartSnapshot : new ProtoPartSnapshot(part, part.vessel.protoVessel));
		if (!droppedPart)
		{
			backedUpPart.transform.SetParent(partParent.transform);
		}
		backedUpPart.protoPartSnapshot = new ProtoPartSnapshot(part, part.vessel.protoVessel);
		backedUpPart.transform.position = part.transform.position;
		backedUpPart.transform.rotation = part.transform.rotation;
	}

	public void RestoreLastAttachedPart()
	{
		if (!(backedUpPart != null))
		{
			return;
		}
		if (backedUpPart.parent == null)
		{
			backedUpPart.name = backedUpPart.name.Replace("(Backup)", "");
			if (selectedPart == null)
			{
				selectedPart = backedUpPart;
			}
			RestoreLastDroppedPart();
			return;
		}
		backedUpPart.name = backedUpPart.name.Replace("(Backup)", "");
		backedUpPart.gameObject.SetActive(value: true);
		backedUpPart.isAttached = true;
		int value = 0;
		if (backedUpPart.protoPartSnapshot.partInfo.partConfig.TryGetValue("PhysicsSignificance", ref value))
		{
			backedUpPart.physicalSignificance = (Part.PhysicalSignificance)value;
		}
		backedUpPart.FindAttachNode(backedUpAttachment.id);
		if (backedUpParentAttachment != null)
		{
			backedUpParentAttachment.attachedPart = backedUpPart;
		}
		backedUpPart.OnAttachFlight(backedUpPart.parent);
		backedUpPart.CreateAttachJoint(backedUpPart.attachMode);
		CollisionManager.SetCollidersOnVessel(backedUpPart.vessel, ignore: true, backedUpPart.GetPartColliders());
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartAttached, backedUpPart);
		GameEvents.OnEVAConstructionModePartAttached.Fire(backedUpPart.vessel, backedUpPart);
		lastAttachedPart = backedUpPart;
		backedUpPart.SetHighlightColor(Highlighter.colorPartHighlightDefault);
		backedUpPart.SetHighlightType(Part.HighlightType.OnMouseOver);
		backedUpPart.SetHighlight(active: false, recursive: true);
		PlayAudioClip(attachClip);
		backedUpPart = null;
	}

	public void RestoreLastDroppedPart()
	{
		if (backedUpPart != null)
		{
			Quaternion rotation = Quaternion.Inverse(FlightGlobals.ActiveVessel.mainBody.bodyTransform.rotation) * backedUpPart.transform.rotation;
			DropAttachablePart(backedUpPart.name, backedUpPart.transform.position, rotation);
			DestroyBackupPart();
		}
	}

	public Attachment CheckAttach(Part selPart)
	{
		Attachment attachment = new Attachment();
		attachment.caller = selPart;
		attachment.position = selPart.transform.position;
		attachment.rotation = vesselRotation;
		attachment.potentialParent = null;
		bool flag = false;
		bool flag2 = false;
		if (selPart.attachRules.stack && allowNodeAttachment)
		{
			Part part = null;
			for (int i = 0; i < attachVessels.Count; i++)
			{
				Vessel vessel = attachVessels[i];
				int j = 0;
				for (int count = vessel.parts.Count; j < count; j++)
				{
					part = vessel.parts[j];
					if (part == selPart || PartInHierarchy(selPart, part) || (!part.attachRules.allowStack && !part.attachRules.allowDock))
					{
						continue;
					}
					AttachNode attachNode = null;
					AttachNode attachNode2 = null;
					AttachNode attachNode3 = null;
					int k = 0;
					for (int count2 = selPart.attachNodes.Count; k < count2; k++)
					{
						attachNode3 = selPart.attachNodes[k];
						if (attachNode3.attachedPart != null || attachNode3.icon == null)
						{
							continue;
						}
						Vector3 normalized = selPart.transform.TransformDirection(attachNode3.orientation).normalized;
						AttachNode attachNode4 = null;
						int l = 0;
						for (int count3 = part.attachNodes.Count; l < count3; l++)
						{
							attachNode4 = part.attachNodes[l];
							if (((attachNode4.icon == null || attachNode4.attachedPart != null) && !CheatOptions.AllowPartClipping) || attachNode3.nodeType != attachNode4.nodeType || (!attachNode4.icon.GetComponent<Renderer>().bounds.IntersectRay(FlightCamera.fetch.mainCamera.ScreenPointToRay(FlightCamera.fetch.mainCamera.WorldToScreenPoint(selPart.transform.TransformPoint(attachNode3.position)))) && !attachNode4.icon.GetComponent<Renderer>().bounds.Contains(selPart.transform.TransformPoint(attachNode3.position))))
							{
								continue;
							}
							Vector3 normalized2 = part.transform.TransformDirection(attachNode4.orientation).normalized;
							float num = Vector3.Dot(normalized, normalized2);
							if (!CheatOptions.NonStrictAttachmentOrientation)
							{
								if (num < -0.7f)
								{
									attachNode2 = attachNode4;
									attachNode = attachNode3;
									attachment.potentialParent = part;
									stackRotation = Quaternion.FromToRotation(normalized, -normalized2);
									attachment.rotation = stackRotation * vesselRotation;
									attachment.position = part.transform.TransformPoint(attachNode2.position + attachNode2.offset) - attachment.rotation * selPart.attRotation * attachNode.position;
									break;
								}
							}
							else if (!(Mathf.Abs(num) <= 0.7f))
							{
								attachNode2 = attachNode4;
								attachNode = attachNode3;
								attachment.potentialParent = part;
								stackRotation = Quaternion.FromToRotation(normalized * Mathf.Sign(num), normalized2);
								attachment.rotation = stackRotation * vesselRotation;
								attachment.position = part.transform.TransformPoint(attachNode2.position + attachNode2.offset) - attachment.rotation * selPart.attRotation * attachNode.position;
								break;
							}
						}
					}
					if (attachNode != null && attachNode2 != null)
					{
						flag = true;
						attachment.callerPartNode = attachNode;
						attachment.otherPartNode = attachNode2;
						selPart.attachMode = AttachModes.STACK;
					}
				}
			}
		}
		if ((selPart.attachRules.srfAttach && allowSrfAttachment && !GameSettings.MODIFIER_KEY.GetKey()) || (GameSettings.MODIFIER_KEY.GetKey() && isCurrentPartFlag))
		{
			this.ray = FlightCamera.fetch.mainCamera.ScreenPointToRay(Input.mousePosition - srfAttachCursorOffset);
			bool flag3 = false;
			Part part2 = null;
			RaycastHit[] array = Physics.RaycastAll(this.ray, 10000f, LayerUtil.DefaultEquivalent);
			if (array.Length == 1)
			{
				part2 = FlightGlobals.GetPartUpwardsCached(array[0].collider.gameObject);
				if (part2 != null && part2.State != PartStates.CARGO)
				{
					flag3 = true;
					hit = array[0];
				}
			}
			else if (array.Length > 1)
			{
				Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
				for (int m = 0; m < array.Length; m++)
				{
					part2 = FlightGlobals.GetPartUpwardsCached(array[m].collider.gameObject);
					if (part2 != null && part2.State != PartStates.CARGO)
					{
						flag3 = true;
						hit = array[m];
						break;
					}
				}
			}
			if (flag3 && part2 != null)
			{
				if (isCurrentPartFlag)
				{
					FairingHitCollider = hit.collider;
				}
				else
				{
					FairingHitCollider = null;
					ModuleProceduralFairing moduleProceduralFairing = null;
					if (part2 != null)
					{
						moduleProceduralFairing = part2.FindModuleImplementing<ModuleProceduralFairing>();
						if (moduleProceduralFairing != null)
						{
							for (int n = 0; n < moduleProceduralFairing.Panels.Count; n++)
							{
								if (hit.collider.transform.FindParent(moduleProceduralFairing.Panels[n].ColliderContainer.name, findActiveParent: false) != null)
								{
									attachment.possible = false;
									return attachment;
								}
							}
						}
					}
				}
				if (part2 != null)
				{
					if (hit.collider.CompareTag("NoAttach"))
					{
						part2 = null;
					}
					if ((bool)part2 && part2.attachRules.allowSrfAttach)
					{
						attachment.callerPartNode = selPart.srfAttachNode;
						attachment.otherPartNode = null;
						selPart.attachMode = AttachModes.SRF_ATTACH;
						selPart.srfAttachNode.srfAttachMeshName = hit.collider.name;
						attachment.potentialParent = part2;
						flag2 = true;
						attachment.rotation = Quaternion.LookRotation(hit.normal, part2.transform.rotation * Vector3.up) * Quaternion.LookRotation(selPart.srfAttachNode.orientation, Vector3.up);
						attachment.position = hit.point - attachment.rotation * selPart.attRotation * selPart.srfAttachNode.position;
						if (GameSettings.VAB_USE_ANGLE_SNAP)
						{
							Vector3 vector = hit.point - part2.transform.position;
							Ray ray = new Ray(part2.transform.position, part2.transform.up);
							Vector3 vector2 = Vector3.ProjectOnPlane(vector, ray.direction);
							Vector3 forward = part2.transform.forward;
							float num2 = Vector3.Dot(vector, ray.direction);
							float num3 = KSPUtil.BearingDegrees(forward, vector2.normalized, ray.direction);
							float num4 = (GameSettings.Editor_fineTweak.GetKey() ? srfAttachAngleSnapFine : srfAttachAngleSnap);
							float angle = ((!(num3 / num4 - Mathf.Floor(num3 / num4) > 0.5f)) ? ((float)Mathf.FloorToInt(num3 / num4) * num4) : ((float)Mathf.CeilToInt(num3 / num4) * num4));
							Vector3 vector3 = Quaternion.AngleAxis(angle, ray.direction) * forward;
							Vector3 vector4 = ray.origin + ray.direction * num2;
							vector.Normalize();
							vector4 += vector3 * FindPartSurface(part2, ray.origin, ray.direction, vector, hit, out var srfNormal, 2097152);
							if (Mathf.Abs(Vector3.Dot(srfNormal, ray.direction)) < 1f)
							{
								srfNormal = Quaternion.FromToRotation(Vector3.ProjectOnPlane(srfNormal, ray.direction).normalized, vector3) * srfNormal;
							}
							attachment.rotation = Quaternion.LookRotation(srfNormal, part2.transform.rotation * Vector3.up) * Quaternion.LookRotation(selPart.srfAttachNode.orientation, Vector3.up);
							attachment.position = vector4 - attachment.rotation * selPart.attRotation * selPart.srfAttachNode.position;
						}
					}
				}
			}
		}
		attachment.mode = selPart.attachMode;
		attachment.possible = (flag2 || flag) && !attachment.collision;
		return attachment;
	}

	public void ForceDrop()
	{
		if (UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.CurrentCargoPart != null && !UIPartActionControllerInventory.Instance.isSetAsPart)
		{
			if (backedUpPart != null)
			{
				RestoreLastDroppedPart();
			}
		}
		else if (evaConstructionMode == ConstructionMode.Place && selectedPart != null && CanPartBeEdited(selectedPart, weightOnlyCheck: true))
		{
			DropAttachablePart(selectedPart.name, selectedPart.transform.position, Quaternion.Inverse(FlightGlobals.ActiveVessel.mainBody.bodyTransform.rotation) * selectedPart.transform.rotation);
		}
	}

	public void DropPartInput()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0) && isPlacementValid && selectedPart != null && FlightGlobals.ActiveVessel != null && !EventSystem.current.IsPointerOverGameObject() && CanPartBeEdited(selectedPart, weightOnlyCheck: true))
		{
			DropAttachablePart(selectedPart.name, selectedPart.transform.position, Quaternion.Inverse(FlightGlobals.ActiveVessel.mainBody.bodyTransform.rotation) * selectedPart.transform.rotation);
		}
	}

	public void DropAttachablePart(string partName, Vector3 partPosition, Quaternion rotation)
	{
		ConfigNode protoVesselNode = GetProtoVesselNode(partName, partPosition, rotation, FlightGlobals.ActiveVessel, selectedPart);
		ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
		for (int i = 0; i < FlightGlobals.VesselsUnloaded.Count; i++)
		{
			if (protoVessel.persistentId == FlightGlobals.VesselsUnloaded[i].persistentId)
			{
				FlightGlobals.VesselsUnloaded[i].SetPhysicsHoldExpiryOverride();
				break;
			}
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && selectedPart.FindModuleImplementing<ModuleGroundSciencePart>() != null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6002644", selectedPart.partInfo.title), modeMsg);
		}
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartDropped, selectedPart);
		DestroyHeldPart();
		selectedPartColliders.Clear();
		PlayAudioClip(attachClip);
		DestroyBackupPart();
		if (UIPartActionControllerInventory.Instance != null)
		{
			if (UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked != null)
			{
				UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked.UpdateCurrentSelectedSlot(isCurrent: false);
			}
			UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
		}
	}

	public ConfigNode GetProtoVesselNode(string partName, Vector3 partPosition, Quaternion rotation, Vessel vessel, Part part)
	{
		uint uniqueFlightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		Orbit orbit = null;
		Vector3d vector3d = Vector3.zero;
		Vessel.Situations situation = vessel.situation;
		switch (situation)
		{
		case Vessel.Situations.LANDED:
		case Vessel.Situations.PRELAUNCH:
			orbit = Orbit.CreateRandomOrbitAround(vessel.mainBody);
			break;
		case Vessel.Situations.FLYING:
		case Vessel.Situations.SUB_ORBITAL:
		case Vessel.Situations.ORBITING:
		case Vessel.Situations.ESCAPING:
			orbit = vessel.GetCurrentOrbit();
			vector3d = vessel.orbit.pos + new Vector3d(partPosition.x, partPosition.z, partPosition.y);
			orbit.UpdateFromStateVectors(vector3d, vessel.orbit.vel, vessel.orbit.referenceBody, Planetarium.GetUniversalTime());
			orbit.Init();
			orbit.UpdateFromUT(Planetarium.GetUniversalTime());
			break;
		}
		AvailablePart partInfo = part.partInfo;
		string vesselName = partName;
		if (partInfo != null)
		{
			vesselName = partInfo.partPrefab.partInfo.title;
		}
		ConfigNode configNode = new ConfigNode();
		configNode = ProtoVessel.CreateVesselNode(vesselName, VesselType.DroppedPart, orbit, 0, new ConfigNode[1] { CreatePartNode(uniqueFlightID, part, partInfo) });
		double alt = vessel.altitude;
		double lat = vessel.latitude;
		double lon = vessel.longitude;
		configNode.SetValue("skipGroundPositioning", newValue: false);
		switch (situation)
		{
		case Vessel.Situations.LANDED:
		case Vessel.Situations.PRELAUNCH:
			vessel.mainBody.GetLatLonAlt(partPosition, out lat, out lon, out alt);
			if ((situation == Vessel.Situations.LANDED || situation == Vessel.Situations.PRELAUNCH) && isPlacementValid && !isPlacementOnGround)
			{
				configNode.SetValue("sit", Vessel.Situations.SUB_ORBITAL.ToString());
				configNode.SetValue("landed", newValue: true);
				configNode.SetValue("skipGroundPositioning", newValue: true);
				configNode.SetValue("skipGroundPositioningForDroppedPart", newValue: true, createIfNotFound: true);
			}
			else
			{
				configNode.SetValue("sit", Vessel.Situations.LANDED.ToString());
				configNode.SetValue("landed", newValue: true);
				configNode.SetValue("skipGroundPositioning", newValue: false);
			}
			break;
		case Vessel.Situations.FLYING:
		case Vessel.Situations.SUB_ORBITAL:
		case Vessel.Situations.ORBITING:
		case Vessel.Situations.ESCAPING:
			vessel.mainBody.GetLatLonAltOrbital(vector3d, out lat, out lon, out alt);
			configNode.SetValue("sit", Vessel.Situations.ORBITING.ToString());
			configNode.SetValue("landed", newValue: false);
			configNode.SetValue("skipGroundPositioning", newValue: true);
			break;
		}
		configNode.SetValue("lat", lat);
		configNode.SetValue("lon", lon);
		configNode.SetValue("alt", alt);
		configNode.SetValue("splashed", newValue: false);
		configNode.SetValue("vesselSpawning", newValue: true);
		configNode.AddValue("prst", value: true);
		configNode.AddValue("rot", rotation);
		configNode.SetValue("PQSMin", 0, createIfNotFound: true);
		configNode.SetValue("PQSMax", 0, createIfNotFound: true);
		return configNode;
	}

	public ConfigNode CreatePartNode(uint id, Part part, AvailablePart availablePart)
	{
		ConfigNode configNode = ProtoVessel.CreatePartNode(part, part.partInfo.name, id, (ProtoCrewMember[])null);
		configNode.SetValue("flag", part.flagURL, createIfNotFound: true);
		for (int i = 0; i < part.Modules.Count; i++)
		{
			ModuleCargoPart moduleCargoPart = part.Modules[i] as ModuleCargoPart;
			if (moduleCargoPart != null)
			{
				moduleCargoPart.beingSettled = FlightGlobals.ActiveVessel != null && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH);
				moduleCargoPart.Save(configNode.AddNode("MODULE"));
			}
			else
			{
				part.Modules[i].Save(configNode.AddNode("MODULE"));
			}
		}
		return configNode;
	}

	public void UpdateAssistingKerbals()
	{
		double num = combinedConstructionWeightLimit;
		if (GameSettings.EVA_CONSTRUCTION_COMBINE_ENABLED && !(FlightGlobals.ActiveVessel == null))
		{
			int num2 = 0;
			Vector3 position = FlightGlobals.ActiveVessel.transform.position;
			List<Vessel> vesselsLoaded = FlightGlobals.VesselsLoaded;
			for (int i = 0; i < vesselsLoaded.Count; i++)
			{
				Vessel vessel = vesselsLoaded[i];
				if (vessel.isEVA && vessel != FlightGlobals.ActiveVessel && vessel.evaController != null && Vector3.Distance(vessel.transform.position, position) <= GameSettings.EVA_CONSTRUCTION_COMBINE_RANGE && !vessel.evaController.IsSeated())
				{
					if (GameSettings.EVA_CONSTRUCTION_COMBINE_NONENGINEERS)
					{
						num2++;
					}
					else if (VesselUtilities.VesselCrewWithTraitCount("Engineer", vessel) > 0)
					{
						num2++;
					}
				}
			}
			assistingKerbals = num2;
			combinedConstructionWeightLimit = PhysicsGlobals.ConstructionWeightLimit + (double)num2 * PhysicsGlobals.ConstructionWeightLimitPerKerbalCombine;
			if (num != combinedConstructionWeightLimit)
			{
				GameEvents.OnCombinedConstructionWeightLimitChanged.Fire();
				if (num > combinedConstructionWeightLimit && SelectedPart != null && !SelectedPart.IsUnderConstructionWeightLimit())
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8012062", SelectedPart.partInfo.title), 5f, ScreenMessageStyle.UPPER_CENTER);
					DropAttachablePart(selectedPart.name, selectedPart.transform.position, Quaternion.Inverse(FlightGlobals.ActiveVessel.mainBody.bodyTransform.rotation) * selectedPart.transform.rotation);
				}
			}
		}
		else
		{
			assistingKerbals = 0;
			combinedConstructionWeightLimit = 0.0;
			if (num != combinedConstructionWeightLimit)
			{
				GameEvents.OnCombinedConstructionWeightLimitChanged.Fire();
			}
		}
	}

	public void ToggleRotateGizmo(bool activate)
	{
		if (activate)
		{
			if (!(selectedPart == null) && selectedPart.PartCanBeRotated())
			{
				if (gizmoRotate != null)
				{
					gizmoRotate.Detach();
				}
				if (!(selectedPart == null))
				{
					gizmoRotate = GizmoRotate.Attach(selectedPart.transform, selectedPart.transform.position, selectedPart.initRotation, OnRotateGizmoUpdate, OnRotateGizmoUpdated, Camera.main);
					gizmoAttRotate = selectedPart.attRotation;
					gizmoAttRotate0 = selectedPart.attRotation0;
					PlayAudioClip(tweakGrabClip);
					FetchColliders();
					ToggleCollidersTrigger(toggle: true);
				}
			}
			else
			{
				CannotInteract();
			}
		}
		else if (gizmoRotate != null)
		{
			gizmoRotate.Detach();
		}
	}

	public void OnRotateGizmoUpdate(Quaternion dRot)
	{
		Transform transform = selectedPart.transform;
		switch (gizmoRotate.CoordSpace)
		{
		case Space.Self:
			transform.rotation = gizmoRotate.transform.rotation * selectedPart.initRotation;
			break;
		case Space.World:
			transform.rotation = dRot * gizmoRotate.HostRot0;
			break;
		}
		selectedPart.attRotation0 = selectedPart.transform.localRotation;
		selectedPart.attRotation = gizmoAttRotate * Quaternion.Inverse(gizmoAttRotate0) * selectedPart.transform.localRotation;
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotating, selectedPart);
	}

	public void OnRotateGizmoUpdated(Quaternion dRot)
	{
		if (selectedPart != null)
		{
			if (selectedPart.vessel != null)
			{
				selectedPart.UpdateOrgPosAndRot(selectedPart.vessel.rootPart);
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, selectedPart);
		}
	}

	public Vector3 GetPivotOffset(Vector3 pivot0, Vector3 pivot)
	{
		return pivot0 - pivot;
	}

	public void ToggleOffsetGizmo(bool activate)
	{
		if (activate)
		{
			if (!(selectedPart == null) && selectedPart.PartCanBeOffset())
			{
				if (gizmoOffset != null)
				{
					gizmoOffset.Detach();
				}
				if (!(selectedPart == null))
				{
					gizmoOffset = GizmoOffset.Attach(selectedPart.transform, selectedPart.initRotation, OnOffsetGizmoUpdate, OnOffsetGizmoUpdated, Camera.main);
					PlayAudioClip(tweakGrabClip);
					FetchColliders();
					ToggleCollidersTrigger(toggle: true);
				}
			}
			else
			{
				CannotInteract();
			}
		}
		else if (gizmoOffset != null)
		{
			gizmoOffset.Detach();
		}
	}

	public void OnOffsetGizmoUpdate(Vector3 dPos)
	{
		Transform transform = selectedPart.transform;
		transform.position = gizmoOffset.transform.position;
		if (rootPart != null && selectedPart != rootPart)
		{
			threshold = (GameSettings.Editor_fineTweak.GetKey() ? GameSettings.VAB_FINE_OFFSET_THRESHOLD : 0.1f);
			if (selectedPart.surfaceAttachGO != null)
			{
				selectedPart.surfaceAttachGO.SetActive(value: true);
			}
			Part referenceParent = selectedPart.GetReferenceParent();
			if (EditorGeometryUtil.TestPartBoundsSeparate(selectedPart, referenceParent, threshold, out offsetGap))
			{
				if (selectedPart.attachMode == AttachModes.STACK)
				{
					childToParent = selectedPart.FindAttachNodeByPart(selectedPart.parent);
					parentToChild = selectedPart.parent.FindAttachNodeByPart(selectedPart);
					diff = parentToChild.position - (transform.localPosition + selectedPart.attRotation * childToParent.position);
					if (diff.sqrMagnitude > threshold * (float)(parentToChild.size + 1))
					{
						gizmoOffset.transform.position -= offsetGap;
						transform.position = gizmoOffset.transform.position;
					}
				}
				else
				{
					transform.position = gizmoOffset.transform.position;
				}
			}
			if (selectedPart.surfaceAttachGO != null)
			{
				selectedPart.surfaceAttachGO.SetActive(value: false);
			}
		}
		selectedPart.attPos = transform.localPosition - selectedPart.attPos0;
		GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffsetting, selectedPart);
	}

	public void OnOffsetGizmoUpdated(Vector3 dPos)
	{
		if (selectedPart != null)
		{
			if (selectedPart.vessel != null)
			{
				selectedPart.UpdateOrgPosAndRot(selectedPart.vessel.rootPart);
			}
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartOffset, selectedPart);
		}
	}

	public void CoordToggleInput()
	{
		if (GameSettings.Editor_coordSystem.GetKeyUp())
		{
			ChangeCoordSpace();
		}
	}

	public void ToggleCoordSpaceButton(bool toggle)
	{
		if (!(coordSpaceBtn == null))
		{
			if (toggle)
			{
				toggle = selectedPart != null;
			}
			coordSpaceBtn.gameObject.SetActive(toggle);
		}
	}

	public void ChangeCoordSpace()
	{
		if (gizmoOffset != null)
		{
			switch (gizmoOffset.CoordSpace)
			{
			case Space.Self:
				gizmoOffset.SetCoordSystem(Space.World);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001221, modeMsg);
				break;
			case Space.World:
				gizmoOffset.SetCoordSystem(Space.Self);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001222, modeMsg);
				break;
			}
		}
		else if (gizmoRotate != null)
		{
			switch (gizmoRotate.CoordSpace)
			{
			case Space.Self:
				gizmoRotate.SetCoordSystem(Space.World);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001217;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001223, modeMsg);
				break;
			case Space.World:
				gizmoRotate.SetCoordSystem(Space.Self);
				coordSpaceText.text = EditorLogicBase.cacheAutoLOC_6001218;
				ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6001224, modeMsg);
				break;
			}
		}
	}

	public void SnapInputUpdate()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_SYM_SNAP_UI) && !DeltaVApp.AnyTextFieldHasFocus() && GameSettings.Editor_toggleAngleSnap.GetKeyDown())
		{
			SnapButton();
		}
	}

	public void SnapButton(PointerEventData evtData = null)
	{
		Debug.Log("SnapMode");
		GameSettings.VAB_USE_ANGLE_SNAP = !GameSettings.VAB_USE_ANGLE_SNAP;
		angleSnapSprite.SetState(GameSettings.VAB_USE_ANGLE_SNAP ? 1 : 0);
		GameEvents.onEditorSnapModeChange.Fire(GameSettings.VAB_USE_ANGLE_SNAP);
	}

	public void PutPartBackInInventory()
	{
		if (selectedPart != null && UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.ReturnHeldPart();
		}
	}

	public void SelectPartInput()
	{
		if (!Input.GetMouseButtonUp(0) || !(Mouse.HoveredPart != null) || (!(selectedPart == null) && selectedPart.persistentId == Mouse.HoveredPart.persistentId) || !Mouse.HoveredPart.HasModuleImplementing<ModuleCargoPart>() || !CanPartBeEdited(Mouse.HoveredPart, weightOnlyCheck: false))
		{
			return;
		}
		if (selectedPart != null)
		{
			if (selectedPart.previousPhysicalSignificance != selectedPart.physicalSignificance)
			{
				StartCoroutine(ResetToPhysicalPart(selectedPart));
			}
			if (selectedPart.persistentId != Mouse.HoveredPart.persistentId)
			{
				ToggleCollidersTrigger(toggle: false);
			}
		}
		selectedPart = Mouse.HoveredPart;
		selectedPart.PartTweakerSelected = Mouse.partPointer != null;
		GetRootPart();
		OnCurrentMousePartChanged(selectedPart);
		BreakSymmetry(selectedPart);
		if (evaConstructionMode == ConstructionMode.Move || evaConstructionMode == ConstructionMode.Rotate)
		{
			selectedPart.DemoteToPhysicslessPart();
		}
	}

	public void GetRootPart()
	{
		if (!(selectedPart == null) && !(selectedPart.vessel == null) && !(selectedPart.vessel.rootPart == null))
		{
			rootPart = selectedPart.vessel.rootPart;
		}
	}

	public void FetchColliders()
	{
		selectedPartColliders.Clear();
		if (SelectedPart != null)
		{
			Collider[] componentsInChildren = SelectedPart.GetComponentsInChildren<Collider>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SelectedPartCollider item = new SelectedPartCollider(componentsInChildren[i]);
				selectedPartColliders.Add(item);
			}
		}
	}

	public void ToggleCollidersTrigger(bool toggle)
	{
		if (selectedPartColliders == null || selectedPartColliders.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < selectedPartColliders.Count; i++)
		{
			if (selectedPartColliders[i] != null && selectedPartColliders[i].collider != null)
			{
				selectedPartColliders[i].collider.isTrigger = toggle | selectedPartColliders[i].isTrigger;
			}
		}
	}

	public void OnConstructionMode(bool opened)
	{
		if (!opened)
		{
			if (selectedPart != null && backedUpPart != null && UIPartActionControllerInventory.Instance != null && UIPartActionControllerInventory.Instance.CurrentInventorySlotClicked == null)
			{
				DestroyHeldPart();
				UIPartActionControllerInventory.Instance.ResetInventoryCacheValues();
				RestoreLastAttachedPart();
			}
			if (selectedPart != null && selectedPart.previousPhysicalSignificance != selectedPart.physicalSignificance)
			{
				StartCoroutine(ResetToPhysicalPart(selectedPart));
			}
			ToggleOffsetGizmo(activate: false);
			ToggleRotateGizmo(activate: false);
			ToggleCollidersTrigger(toggle: false);
			selectedPartColliders.Clear();
			OnPartDropped();
			RemoveAttachNodes();
		}
		else
		{
			PutPartBackInInventory();
			ToggleCoordSpaceButton(toggle: false);
		}
	}

	public void DestroyHeldPart()
	{
		if (selectedPart != null)
		{
			UnityEngine.Object.Destroy(selectedPart.gameObject);
			if (selectedPart != null)
			{
				UnityEngine.Object.Destroy(selectedPart);
			}
			selectedPart = null;
		}
		if (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen)
		{
			GameEvents.OnCollisionIgnoreUpdate.Fire();
		}
	}

	public void BreakSymmetry(Part part)
	{
		if (part != null)
		{
			part.RemoveFromSymmetry();
			if (part.vessel != null)
			{
				GameEvents.onVesselWasModified.Fire(part.vessel);
			}
		}
	}

	public void DestroyBackupPart()
	{
		if (backedUpPart != null)
		{
			UnityEngine.Object.Destroy(backedUpPart.gameObject);
		}
	}

	public bool CanPartBeEdited(Part part, bool weightOnlyCheck)
	{
		if (!part.IsUnderConstructionWeightLimit())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8012062", part.partInfo.title), 5f, ScreenMessageStyle.UPPER_CENTER);
			return false;
		}
		if (weightOnlyCheck)
		{
			return true;
		}
		ModuleCargoPart component = part.GetComponent<ModuleCargoPart>();
		if (part.children.Count <= 0 && !component.HasLinkedCompoundParts())
		{
			if (!part.PartCanBeDetached())
			{
				CannotInteract();
				return false;
			}
			if (part.protoModuleCrew.Count > 0)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6011175", part.partInfo.title), 5f, ScreenMessageStyle.UPPER_CENTER);
				return false;
			}
			return true;
		}
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006096", part.partInfo.title), 5f, ScreenMessageStyle.UPPER_CENTER);
		return false;
	}

	public void CannotInteract()
	{
		ScreenMessages.PostScreenMessage(EditorLogicBase.cacheAutoLOC_6006095, interactMsg);
	}

	public void InterruptKerbalWelding()
	{
		if (FlightGlobals.fetch != null && FlightGlobals.fetch.activeVessel != null && FlightGlobals.fetch.activeVessel.isEVA)
		{
			FlightGlobals.fetch.activeVessel.evaController.InterruptWeld();
		}
	}

	public void InputCheckDetachCompundPart()
	{
		if (GameSettings.PAUSE.GetKeyDown(ignoreInputLock: true) && selectedCompoundPart != null && selectedCompoundPart.attachState == CompoundPart.AttachState.Attaching)
		{
			DetachPart(selectedCompoundPart);
		}
	}

	public void PlayAudioClip(AudioClip clip)
	{
		if (!(clip == null) && !(audioSource == null))
		{
			audioSource.PlayOneShot(clip);
		}
	}
}
