using System;
using System.Collections;
using System.Collections.Generic;
using ns11;
using ns12;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using Upgradeables;

public class SpaceCenterBuilding : MonoBehaviour, ITooltipController
{
	public static float highlightRimFalloff = 3f;

	public static Color highlightColor = new Color(0.7f, 0.7f, 0.7f);

	public PSystemSetup.SpaceCenterFacility spaceCenterFacility;

	public string facilityName;

	public Transform buildingTransform;

	public MeshRenderer[] buildingRenderers;

	public UpgradeableFacility upgradeableFacility;

	public UpgradeableSlave upgradeableSlave;

	public string buildingInfoName;

	public string buildingDescription;

	public DestructibleBuilding[] destructibles;

	public GameObject additionalColliderPrefab;

	public GameObject additionalCollider;

	public PopupDialog facilityClosedDialog;

	public PopupDialog facilityLockedDialog;

	public static bool useColliderIgnoreMaterials = false;

	public string[] colliderIgnoreMaterialNames = new string[1] { "grass" };

	public bool operational;

	public float structuralDamage;

	public EventData<bool> OnInViewChange = new EventData<bool>("OnInViewChange");

	public bool hover;

	public bool highlighted;

	public bool clickable;

	public bool inView;

	public bool tapping;

	public bool tapped;

	public Mouse.Buttons mouseButtons;

	public MaterialPropertyBlock mpb;

	public EventData<bool> OnClick = new EventData<bool>("OnClick");

	public Tooltip_TitleAndText tooltipPrefab;

	public Transform BuildingTransform => buildingTransform;

	public UpgradeableFacility Facility => upgradeableFacility;

	public bool Operational => operational;

	public float StructuralDamage => structuralDamage;

	public bool InView => inView;

	public bool isTooltipAllowed => InputLockManager.IsUnlocked(ControlTypes.KSC_FACILITIES | ControlTypes.KSC_UI);

	public Tooltip TooltipPrefabType
	{
		get
		{
			return tooltipPrefab;
		}
		set
		{
			tooltipPrefab = value as Tooltip_TitleAndText;
		}
	}

	public Tooltip TooltipPrefabInstance { get; set; }

	public RectTransform TooltipPrefabInstanceTransform { get; set; }

	string ITooltipController.name
	{
		get
		{
			return base.name;
		}
		set
		{
			base.name = value;
		}
	}

	public IEnumerator Start()
	{
		spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(facilityName);
		if (spaceCenterFacility == null)
		{
			Debug.LogError(GetType().Name + ": Cannot find a facility of name '" + facilityName + "'");
			yield break;
		}
		buildingTransform = spaceCenterFacility.facilityTransform;
		base.transform.NestToParent(buildingTransform);
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneChange);
		yield return PSystemSetup.Instance.StartCoroutine(SetupFacility());
		SetupRenderers();
		SetupColliders();
		SetupDestructibles();
		buildingRenderers = buildingTransform.GetComponentsInChildren<MeshRenderer>();
		OnStart();
	}

	public void OnDestroy()
	{
		HighLightBuilding(mouseOverIcon: false);
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneChange);
		InputLockManager.RemoveControlLock(buildingInfoName + "_ContextMenu");
		OnOnDestroy();
	}

	public void OnGameSceneChange(GameScenes scene)
	{
		UnregisterDestructibles();
		UnregisterUpgradeables();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetupRenderers()
	{
		buildingRenderers = buildingTransform.GetComponentsInChildren<MeshRenderer>();
	}

	public void SetupColliders()
	{
		if (buildingTransform == null)
		{
			return;
		}
		Collider[] componentsInChildren = buildingTransform.GetComponentsInChildren<Collider>(includeInactive: true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			Collider collider = componentsInChildren[i];
			if (collider.transform != base.transform && !ObjectHasTerrainMaterials(collider.gameObject))
			{
				SpaceCenterBuildingCollider spaceCenterBuildingCollider = collider.gameObject.GetComponent<SpaceCenterBuildingCollider>();
				if (spaceCenterBuildingCollider == null)
				{
					spaceCenterBuildingCollider = collider.gameObject.AddComponent<SpaceCenterBuildingCollider>();
				}
				spaceCenterBuildingCollider.Setup(this, ownGameObject: false);
			}
		}
		if (additionalColliderPrefab != null)
		{
			if (additionalCollider == null)
			{
				additionalCollider = UnityEngine.Object.Instantiate(additionalColliderPrefab);
			}
			additionalCollider.transform.SetParent(buildingTransform.transform, worldPositionStays: false);
			SpaceCenterBuildingCollider spaceCenterBuildingCollider2 = additionalCollider.GetComponent<SpaceCenterBuildingCollider>();
			if (spaceCenterBuildingCollider2 == null)
			{
				spaceCenterBuildingCollider2 = additionalCollider.AddComponent<SpaceCenterBuildingCollider>();
			}
			spaceCenterBuildingCollider2.Setup(this, ownGameObject: true);
		}
	}

	public bool ObjectHasTerrainMaterials(GameObject obj)
	{
		if (!useColliderIgnoreMaterials)
		{
			return false;
		}
		Renderer component = obj.GetComponent<Renderer>();
		if (component == null)
		{
			return false;
		}
		Material[] materials = component.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			string text = materials[i].name.ToLower();
			for (int j = 0; j < colliderIgnoreMaterialNames.Length; j++)
			{
				if (text.Contains(colliderIgnoreMaterialNames[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool RendererHasTerrainMaterials(Renderer r)
	{
		if (!useColliderIgnoreMaterials)
		{
			return false;
		}
		if (r == null)
		{
			return false;
		}
		Material[] sharedMaterials = r.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			string text = sharedMaterials[i].name.ToLower();
			for (int j = 0; j < colliderIgnoreMaterialNames.Length; j++)
			{
				if (text.Contains(colliderIgnoreMaterialNames[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetInView()
	{
		if (!(buildingTransform == null))
		{
			Vector3 vector = FlightCamera.fetch.mainCamera.WorldToViewportPoint(buildingTransform.position);
			bool flag = vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f;
			if (inView != flag)
			{
				inView = flag;
				OnInViewChange.Fire(flag);
			}
		}
	}

	public void LateUpdate()
	{
		if (!isTooltipAllowed)
		{
			Despawntooltip();
		}
		SetInView();
		if (InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES) || EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		if (hover)
		{
			if (!highlighted)
			{
				HighLightBuilding(mouseOverIcon: true);
				highlighted = true;
				clickable = true;
			}
			if (clickable)
			{
				if (Mouse.Left.GetButtonDown() || Mouse.Right.GetButtonDown())
				{
					OnMouseDown();
				}
				if (Mouse.Left.GetButtonUp() || Mouse.Right.GetButtonUp())
				{
					OnMouseUp();
				}
			}
		}
		else if (highlighted)
		{
			HighLightBuilding(mouseOverIcon: false);
			highlighted = false;
			clickable = false;
		}
	}

	public void ColliderHover(bool hover)
	{
		this.hover = hover;
	}

	public void OnMouseDown()
	{
		if (!InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES) && clickable && !tapping)
		{
			StartCoroutine(OnMouseTap());
		}
	}

	public void OnMouseUp()
	{
		if (!InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES) && tapping)
		{
			tapped = true;
		}
	}

	public IEnumerator OnMouseTap()
	{
		float delay = 0.5f;
		tapping = true;
		tapped = false;
		mouseButtons = Mouse.GetAllMouseButtons();
		while (delay >= 0f && !tapped)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		if (tapped)
		{
			if (Mouse.CheckButtons(mouseButtons, Mouse.Buttons.Left))
			{
				OnLeftClick();
			}
			else if (Mouse.CheckButtons(mouseButtons, Mouse.Buttons.Right))
			{
				OnRightClick();
			}
		}
		tapping = false;
		tapped = false;
	}

	public void HighLightBuilding(bool mouseOverIcon)
	{
		if (mpb == null)
		{
			mpb = new MaterialPropertyBlock();
			mpb.SetFloat(PropertyIDs._RimFalloff, highlightRimFalloff);
		}
		if (mouseOverIcon)
		{
			mpb.SetColor(PropertyIDs._RimColor, highlightColor);
			int num = buildingRenderers.Length;
			while (num-- > 0)
			{
				if (buildingRenderers[num] != null)
				{
					buildingRenderers[num].SetPropertyBlock(mpb);
				}
			}
			UISpaceCenter.Instance.SetBuildingText(buildingInfoName);
			SpawnTooltip();
			return;
		}
		mpb.SetColor(PropertyIDs._RimColor, Color.black);
		int num2 = buildingRenderers.Length;
		while (num2-- > 0)
		{
			if (buildingRenderers[num2] != null)
			{
				buildingRenderers[num2].SetPropertyBlock(mpb);
			}
		}
		if (UISpaceCenter.Instance != null)
		{
			UISpaceCenter.Instance.SetBuildingText("");
		}
		Despawntooltip();
	}

	public void SetHighlighted(bool newValue)
	{
		highlighted = newValue;
	}

	public void OnLeftClick()
	{
		if (GetStructureDamage() < 70f)
		{
			EnterBuilding();
		}
		else
		{
			Debug.Log(buildingInfoName + " is not operational");
			OnContextMenuSpawn();
		}
		OnClick.Fire(data: true);
	}

	public void OnRightClick()
	{
		OnContextMenuSpawn();
		OnClick.Fire(data: false);
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnOnDestroy()
	{
	}

	public virtual void OnClicked()
	{
	}

	public virtual AnchoredDialog OnContextMenuSpawn()
	{
		Despawntooltip();
		InputLockManager.SetControlLock(ControlTypes.KSC_FACILITIES, buildingInfoName + "_ContextMenu");
		return KSCFacilityContextMenu.Create(this, OnContextMenuDismissed);
	}

	public void OnContextMenuDismissed(KSCFacilityContextMenu.DismissAction dma)
	{
		switch (dma)
		{
		case KSCFacilityContextMenu.DismissAction.Enter:
			EnterBuilding();
			break;
		case KSCFacilityContextMenu.DismissAction.Repair:
			if (!WarnOfObstructingVessels(includeGrounds: false, onlyDestroyed: true))
			{
				RepairFacility(Funding.Instance != null);
			}
			break;
		case KSCFacilityContextMenu.DismissAction.Upgrade:
			if (!WarnOfObstructingVessels(includeGrounds: true, onlyDestroyed: false))
			{
				UpgradeFacility(Funding.Instance != null);
			}
			break;
		case KSCFacilityContextMenu.DismissAction.Demolish:
			if (!WarnOfObstructingVessels(includeGrounds: true, onlyDestroyed: false))
			{
				InputLockManager.SetControlLock(ControlTypes.KSC_FACILITIES | ControlTypes.KSC_UI, buildingInfoName + "_ContextMenu_confirmation");
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ContextMenuConfirmation", Localizer.Format("#autoLOC_6002255"), Localizer.Format("#autoLOC_465206"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_6002250"), delegate
				{
					DemolishFacility();
					InputLockManager.RemoveControlLock(buildingInfoName + "_ContextMenu_confirmation");
				}), new DialogGUIButton(Localizer.Format("#autoLOC_483230"), delegate
				{
					InputLockManager.RemoveControlLock(buildingInfoName + "_ContextMenu_confirmation");
				})), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = delegate
				{
					InputLockManager.RemoveControlLock(buildingInfoName + "_ContextMenu_confirmation");
				};
			}
			break;
		case KSCFacilityContextMenu.DismissAction.Downgrade:
			if (!WarnOfObstructingVessels(includeGrounds: true, onlyDestroyed: false))
			{
				DowngradeFacility(Funding.Instance != null);
			}
			break;
		}
		InputLockManager.RemoveControlLock(buildingInfoName + "_ContextMenu");
	}

	public void EnterBuilding()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			OnClicked();
		}
		else
		{
			Debug.Log(buildingInfoName + " is up-and-running");
		}
	}

	public virtual bool IsOpen()
	{
		return true;
	}

	public List<ProtoVessel> FindVesselsAtFacility(FlightState st, IEnumerable<DestructibleBuilding> structuresToCheck)
	{
		List<ProtoVessel> list = new List<ProtoVessel>();
		foreach (DestructibleBuilding item in structuresToCheck)
		{
			foreach (ProtoVessel item2 in item.FindVesselsOverStructure(st))
			{
				list.AddUnique(item2);
			}
		}
		return list;
	}

	public List<ProtoVessel> FindVesselsAtGrounds(FlightState st, Transform facilityRoot)
	{
		List<string> list = new List<string>();
		KSPUtil.FindTagsInChildren(list, facilityRoot);
		List<ProtoVessel> list2 = new List<ProtoVessel>();
		int count = list.Count;
		while (count-- > 0)
		{
			list2.AddUniqueRange(ShipConstruction.FindVesselsLandedAt(st, list[count]));
		}
		return list2;
	}

	public bool WarnOfObstructingVessels(bool includeGrounds, bool onlyDestroyed)
	{
		List<ProtoVessel> list = new List<ProtoVessel>();
		List<string> list2 = new List<string>();
		int num = destructibles.Length;
		while (num-- > 0)
		{
			DestructibleBuilding destructibleBuilding = destructibles[num];
			if (destructibleBuilding != null && (!onlyDestroyed || (destructibleBuilding.IsDestroyed && destructibleBuilding.tag != null)))
			{
				list2.AddUnique(destructibleBuilding.tag);
			}
		}
		if (includeGrounds)
		{
			KSPUtil.FindTagsInChildren(list2, base.transform.parent);
		}
		int count = HighLogic.CurrentGame.flightState.protoVessels.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoVessel protoVessel = HighLogic.CurrentGame.flightState.protoVessels[i];
			int count2 = list2.Count;
			while (count2-- > 0)
			{
				if (protoVessel.landedAt.Contains(list2[count2]))
				{
					list.Add(protoVessel);
					break;
				}
			}
		}
		if (list.Count > 0)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("VesselObstruction", Localizer.Format("#autoLOC_6002252", buildingInfoName) + KSPUtil.PrintCollection(list, "\n", (ProtoVessel p) => p.vesselName) + Localizer.Format("#autoLOC_6002253"), Localizer.Format(buildingInfoName) + " " + Localizer.Format("#autoLOC_6002254"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_417274"), delegate
			{
			})), persistAcrossScenes: false, HighLogic.UISkin);
			return true;
		}
		return false;
	}

	public void SetupDestructibles()
	{
		destructibles = buildingTransform.GetComponentsInChildren<DestructibleBuilding>(includeInactive: true);
		structuralDamage = GetStructureDamage();
		operational = IsFacilityOperational(structuralDamage);
		GameEvents.OnKSCStructureCollapsing.Add(OnStructureCollapsing);
		GameEvents.OnKSCStructureCollapsed.Add(OnStructureCollapsed);
		GameEvents.OnKSCStructureRepairing.Add(OnStructureRepairing);
		GameEvents.OnKSCStructureRepaired.Add(OnStructureRepaired);
	}

	public void OnStructureCollapsing(DestructibleBuilding dBld)
	{
		if (!(ScenarioDestructibles.Instance != null) && HighLogic.LoadedSceneIsGame)
		{
			return;
		}
		int num = destructibles.Length;
		do
		{
			if (num-- <= 0)
			{
				return;
			}
		}
		while (!(destructibles[num] == dBld));
		structuralDamage = GetStructureDamage();
		operational = IsFacilityOperational(structuralDamage);
	}

	public void OnStructureCollapsed(DestructibleBuilding dBld)
	{
		SetupRenderers();
		SetupColliders();
	}

	public void OnStructureRepairing(DestructibleBuilding dBld)
	{
		int num = destructibles.Length;
		do
		{
			if (num-- <= 0)
			{
				return;
			}
		}
		while (!(destructibles[num] == dBld));
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, facilityName + "_" + dBld.id + "_Repairs");
	}

	public void OnStructureRepaired(DestructibleBuilding dBld)
	{
		bool flag = false;
		int num = destructibles.Length;
		while (num-- > 0)
		{
			if (destructibles[num] == dBld)
			{
				flag = true;
				break;
			}
		}
		if ((ScenarioDestructibles.Instance != null || !HighLogic.LoadedSceneIsGame) && flag)
		{
			structuralDamage = GetStructureDamage();
			operational = IsFacilityOperational(structuralDamage);
		}
		if (flag)
		{
			InputLockManager.RemoveControlLock(facilityName + "_" + dBld.id + "_Repairs");
		}
	}

	public float GetStructureDamage()
	{
		float num = 0f;
		int num2 = destructibles.Length;
		while (num2-- > 0)
		{
			if (!destructibles[num2].IsIntact)
			{
				num += destructibles[num2].FacilityDamageFraction;
			}
		}
		return num;
	}

	public bool IsFacilityOperational(float damage)
	{
		return damage < 100f;
	}

	public static string GetStructureDamageLevel(float lvl)
	{
		if (lvl <= 0f)
		{
			return Localizer.Format("#autoLOC_7003217");
		}
		if (lvl <= 20f)
		{
			return Localizer.Format("#autoLOC_7003218");
		}
		if (lvl <= 50f)
		{
			return Localizer.Format("#autoLOC_7003219");
		}
		if (lvl <= 80f)
		{
			return Localizer.Format("#autoLOC_7003220");
		}
		return Localizer.Format("#autoLOC_7003221");
	}

	public float GetRepairsCost()
	{
		float num = 0f;
		int num2 = destructibles.Length;
		while (num2-- > 0)
		{
			if (destructibles[num2].IsDestroyed)
			{
				num += destructibles[num2].RepairCost;
			}
		}
		return num * HighLogic.CurrentGame.Parameters.Career.FundsLossMultiplier;
	}

	public float GetCollapseReputationHit()
	{
		float num = 0f;
		int num2 = destructibles.Length;
		while (num2-- > 0)
		{
			if (!destructibles[num2].IsIntact)
			{
				num += destructibles[num2].CollapseReputationHit;
			}
		}
		return num * HighLogic.CurrentGame.Parameters.Career.RepLossMultiplier;
	}

	public void RepairFacility(bool deduceFunds)
	{
		if (deduceFunds)
		{
			if (!CurrencyModifierQuery.RunQuery(TransactionReasons.StructureRepair, 0f - GetRepairsCost(), 0f, 0f).CanAfford())
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003210"), 4f, ScreenMessageStyle.LOWER_CENTER);
				return;
			}
			Funding.Instance.AddFunds(0f - Math.Abs(GetRepairsCost()), TransactionReasons.StructureRepair);
		}
		RepairStructures();
	}

	public void DemolishFacility()
	{
		if (destructibles.Length == 0)
		{
			ScreenMessages.PostScreenMessage("<color=orange><b>" + Localizer.Format("#autoLOC_7003213") + "</b></color>", 4f, ScreenMessageStyle.LOWER_CENTER);
		}
		else
		{
			DestructibleBuilding[] array = destructibles;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Demolish();
			}
		}
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
	}

	public void RepairStructures()
	{
		float num = 0f;
		if (destructibles.Length == 0)
		{
			ScreenMessages.PostScreenMessage("<color=orange><b>" + Localizer.Format("#autoLOC_7003214") + "</b></color>", 4f, ScreenMessageStyle.LOWER_CENTER);
		}
		else
		{
			DestructibleBuilding[] array = destructibles;
			foreach (DestructibleBuilding destructibleBuilding in array)
			{
				destructibleBuilding.Repair();
				num = Mathf.Max(num, destructibleBuilding.GetRepairSequenceDuration());
			}
		}
		StartCoroutine(CallbackUtil.DelayedCallback(num + 0.1f, delegate
		{
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		}));
	}

	public void ResetStructures()
	{
		DestructibleBuilding[] array = destructibles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
	}

	public void UnregisterDestructibles()
	{
		GameEvents.OnKSCStructureCollapsing.Remove(OnStructureCollapsing);
		GameEvents.OnKSCStructureCollapsed.Remove(OnStructureCollapsed);
		GameEvents.OnKSCStructureRepairing.Remove(OnStructureRepairing);
		GameEvents.OnKSCStructureRepaired.Remove(OnStructureRepaired);
		DestructibleBuilding[] array = destructibles;
		foreach (DestructibleBuilding destructibleBuilding in array)
		{
			InputLockManager.RemoveControlLock(facilityName + "_" + destructibleBuilding.id + "_Repairs");
		}
	}

	public IEnumerator SetupFacility()
	{
		CallbackUtil.HoldUntil(() => buildingTransform != null);
		upgradeableFacility = buildingTransform.GetComponent<UpgradeableFacility>();
		upgradeableSlave = spaceCenterFacility.facilityTransform.GetComponentInChildren<UpgradeableSlave>();
		GameEvents.OnKSCFacilityUpgrading.Add(OnFacilityUpgrading);
		GameEvents.OnKSCFacilityUpgraded.Add(OnFacilityUpgraded);
		yield return null;
		yield return null;
	}

	public void OnFacilityUpgrading(UpgradeableFacility facility, int level)
	{
		if (facility == upgradeableFacility)
		{
			UnregisterDestructibles();
		}
	}

	public void OnFacilityUpgraded(UpgradeableFacility facility, int level)
	{
		if (facility == upgradeableFacility)
		{
			if (buildingTransform != null)
			{
				upgradeableFacility = buildingTransform.GetComponent<UpgradeableFacility>();
			}
			else
			{
				buildingTransform = spaceCenterFacility.facilityTransform;
			}
			SetupRenderers();
			SetupColliders();
			SetupDestructibles();
		}
		else
		{
			if (!(upgradeableSlave != null))
			{
				return;
			}
			int num = upgradeableSlave.NeighbourIDs.Length;
			do
			{
				if (num-- <= 0)
				{
					return;
				}
			}
			while (!(upgradeableSlave.NeighbourIDs[num] == facility.id));
			if (buildingTransform != null)
			{
				upgradeableFacility = buildingTransform.GetComponent<UpgradeableFacility>();
			}
			else
			{
				buildingTransform = spaceCenterFacility.facilityTransform;
			}
			SetupRenderers();
			SetupColliders();
			SetupDestructibles();
		}
	}

	public void UnregisterUpgradeables()
	{
		GameEvents.OnKSCFacilityUpgraded.Remove(OnFacilityUpgraded);
		GameEvents.OnKSCFacilityUpgrading.Remove(OnFacilityUpgrading);
	}

	public void UpgradeFacility(bool deduceFunds)
	{
		if (Facility.FacilityLevel == Facility.MaxLevel)
		{
			ScreenMessages.PostScreenMessage("<color=" + XKCDColors.HexFormat.KSPBadassGreen + "><b>" + Localizer.Format("#autoLOC_7003215") + "</b></color>", 4f, ScreenMessageStyle.LOWER_CENTER);
			return;
		}
		if (deduceFunds)
		{
			if (!CurrencyModifierQuery.RunQuery(TransactionReasons.StructureConstruction, 0f - Facility.GetUpgradeCost(), 0f, 0f).CanAfford())
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003211"), 4f, ScreenMessageStyle.LOWER_CENTER);
				return;
			}
			Funding.Instance.AddFunds(0f - Math.Abs(Facility.GetUpgradeCost()), TransactionReasons.StructureConstruction);
		}
		ResetStructures();
		Facility.SetLevel(Facility.FacilityLevel + 1);
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
	}

	public void DowngradeFacility(bool deduceFunds)
	{
		if (Facility.FacilityLevel == 0)
		{
			ScreenMessages.PostScreenMessage("<color=" + XKCDColors.HexFormat.KSPNotSoGoodOrange + "><b>" + Localizer.Format("#autoLOC_7003216") + "</b></color>", 4f, ScreenMessageStyle.LOWER_CENTER);
			return;
		}
		if (deduceFunds)
		{
			if (!CurrencyModifierQuery.RunQuery(TransactionReasons.StructureConstruction, 0f - Facility.GetDowngradeCost(), 0f, 0f).CanAfford())
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003212"), 4f, ScreenMessageStyle.LOWER_CENTER);
				return;
			}
			Funding.Instance.AddFunds(0f - Math.Abs(Facility.GetDowngradeCost()), TransactionReasons.StructureConstruction);
		}
		ResetStructures();
		Facility.SetLevel(Facility.FacilityLevel - 1);
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
	}

	public bool OnTooltipAboutToSpawn()
	{
		return true;
	}

	public bool OnTooltipAboutToDespawn()
	{
		return true;
	}

	public void OnTooltipSpawned(Tooltip instance)
	{
		Tooltip_TitleAndText obj = instance as Tooltip_TitleAndText;
		obj.title.text = buildingInfoName;
		obj.label.text = buildingDescription;
	}

	public void OnTooltipDespawned(Tooltip instance)
	{
	}

	public bool OnTooltipUpdate(Tooltip instance)
	{
		return true;
	}

	public void SpawnTooltip()
	{
		if (isTooltipAllowed && UIMasterController.Instance != null)
		{
			UIMasterController.Instance.SpawnTooltip(this);
		}
	}

	public void Despawntooltip()
	{
		if (UIMasterController.Instance != null)
		{
			UIMasterController.Instance.DespawnTooltip(this);
		}
	}
}
