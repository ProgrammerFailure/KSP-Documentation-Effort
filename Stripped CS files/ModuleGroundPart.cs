using System.Collections;
using Expansions;
using Expansions.Serenity.DeployedScience.Runtime;
using ns9;
using UnityEngine;

public class ModuleGroundPart : ModuleCargoPart, IAnimatedModule
{
	public ModuleInventoryPart kerbalInventoryModule;

	public int firstEmptyInvSlot;

	[KSPField]
	public bool placementAllowXRotation = true;

	[KSPField]
	public bool placementAllowYRotation = true;

	[KSPField]
	public bool placementAllowZRotation = true;

	[KSPField]
	public string fxGroupDeploy;

	public ModuleGroundExpControl groundExpConModule;

	public ModuleAnimationGroup animationGroup;

	public UIPartActionWindow partActionWindow;

	public FixedJoint joint;

	public DictionaryValueList<uint, float> childMass;

	public bool beingRetrieved;

	[KSPField(isPersistant = true)]
	public bool beingDeployed;

	[KSPField(guiActiveEditor = false, guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002325")]
	public bool deployedOnGround;

	public static string cacheAutoLOC_6006111;

	public static string cacheAutoLOC_6006116;

	[KSPEvent(guiActiveEditor = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_8002230")]
	public virtual void RetrievePart()
	{
		if (!HasChildParts())
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6006111, 10f);
		}
		else
		{
			if (!CanBeStored())
			{
				return;
			}
			beingRetrieved = true;
			deployedOnGround = false;
			if (base.part.attachRules.allowStack || base.part.attachRules.allowSrfAttach)
			{
				base.part.mass = base.part.partInfo.partPrefab.mass;
				base.part.prefabMass = base.part.partInfo.partPrefab.prefabMass;
			}
			base.vessel.vesselType = VesselType.DeployedSciencePart;
			if (kerbalInventoryModule == null)
			{
				OnVesselChange(FlightGlobals.ActiveVessel);
			}
			if (kerbalInventoryModule != null)
			{
				kerbalInventoryModule.partBeingRetrieved = true;
				if (kerbalInventoryModule.StoreCargoPartAtSlot(base.part, -1) && ExpansionsLoader.IsExpansionInstalled("Serenity") && groundScienceModule != null)
				{
					GameEvents.onGroundSciencePartRemoved.Fire(groundScienceModule);
				}
				PlayRetractAnimation();
			}
		}
	}

	public bool CanBeStored()
	{
		bool result = true;
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.isEVA)
		{
			ModuleInventoryPart moduleInventoryPart = FlightGlobals.ActiveVessel.FindPartModuleImplementing<ModuleInventoryPart>();
			if (moduleInventoryPart != null)
			{
				if (!moduleInventoryPart.HasCapacity(base.part))
				{
					ScreenMessages.PostScreenMessage(cacheAutoLOC_6006116, 5f);
					result = false;
				}
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	public virtual bool HasChildParts()
	{
		bool flag = true;
		if (base.part.vessel != null && base.part.vessel.Parts != null && base.part.vessel.Parts.Count > 1)
		{
			flag = false;
		}
		if (flag && base.part.attachNodes != null)
		{
			for (int i = 0; i < base.part.attachNodes.Count; i++)
			{
				if (base.part.attachNodes[i].attachedPart != null)
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	public void RetrieveScienceData()
	{
		if (!(kerbalInventoryModule != null) || !ExpansionsLoader.IsExpansionInstalled("Serenity") || !(groundScienceModule != null))
		{
			return;
		}
		ModuleScienceContainer moduleScienceContainer = FlightGlobals.ActiveVessel.FindPartModuleImplementing<ModuleScienceContainer>();
		if (moduleScienceContainer != null && groundScienceModule.ScienceClusterData != null)
		{
			DeployedScienceExperiment experimentByPartID = groundScienceModule.ScienceClusterData.DeployedScienceParts.GetExperimentByPartID(base.part.persistentId);
			if (experimentByPartID != null && experimentByPartID.GatherScienceData(out var experimentData))
			{
				moduleScienceContainer.AddData(experimentData);
			}
		}
	}

	public override void OnPartPack()
	{
		if (base.part != null)
		{
			base.part.Rigidbody.constraints = RigidbodyConstraints.None;
		}
		if (joint != null)
		{
			Object.Destroy(joint);
		}
	}

	public override void OnPartUnpack()
	{
		if (animationGroup == null)
		{
			animationGroup = base.part.FindModuleImplementing<ModuleAnimationGroup>();
		}
		if (beingDeployed && animationGroup != null)
		{
			animationGroup.isEnabled = true;
		}
		if (beingAttached)
		{
			beingAttached = false;
		}
		else if (beingSettled || beingDeployed || deployedOnGround || (base.vessel.parts.Count == 1 && (base.vessel.situation == Vessel.Situations.LANDED || base.vessel.situation == Vessel.Situations.PRELAUNCH)))
		{
			StartCoroutine(MakePartKinematic());
		}
		OnVesselChange(null);
	}

	public override void OnStart(StartState state)
	{
		animationGroup = base.part.FindModuleImplementing<ModuleAnimationGroup>();
		groundScienceModule = base.part.FindModuleImplementing<ModuleGroundSciencePart>();
		groundExpConModule = base.part.FindModuleImplementing<ModuleGroundExpControl>();
		if (base.vessel != null && base.vessel.vesselType == VesselType.DroppedPart)
		{
			isEnabled = false;
			if (animationGroup != null)
			{
				animationGroup.isEnabled = false;
			}
			if (groundScienceModule != null)
			{
				groundScienceModule.Enabled = false;
			}
			if (groundExpConModule != null)
			{
				groundExpConModule.Enabled = false;
			}
		}
		UpdateModuleUI();
		GameEvents.onPartActionUIShown.Add(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.onSceneConfirmExit.Add(OnLeavingScene);
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onPartWillDie.Add(OnPartWillDie);
		}
		inConstructionMode = EVAConstructionModeController.Instance != null && (EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != EVAConstructionModeController.PanelMode.Construction);
	}

	public override void OnDestroy()
	{
		StopCoroutine("MakePartKinematic");
		StopCoroutine("MakePartSettle");
		base.OnDestroy();
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.OnAnimationGroupRetractComplete.Remove(OnRetractCompleted);
		GameEvents.onPartWillDie.Remove(OnPartWillDie);
		GameEvents.onSceneConfirmExit.Remove(OnLeavingScene);
		if (joint != null)
		{
			Object.Destroy(joint);
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (partActionWindow != null)
		{
			UpdateModuleUI();
		}
	}

	public virtual IEnumerator MakePartKinematic()
	{
		if (beingDeployed && kinematicDelay < 1f)
		{
			kinematicDelay = 3f;
		}
		if (base.part.vessel != null && base.part.vessel.rootPart.persistentId == base.part.persistentId && base.vessel.parts.Count > 1)
		{
			childMass = new DictionaryValueList<uint, float>();
			for (int i = 0; i < base.vessel.parts.Count; i++)
			{
				if (base.vessel.parts[i].persistentId != base.vessel.rootPart.persistentId)
				{
					childMass.Add(base.vessel.parts[i].persistentId, base.vessel.parts[i].prefabMass);
					base.vessel.parts[i].prefabMass = 0.01f;
					base.vessel.parts[i].mass = 0.01f;
					if (base.vessel.parts[i].rb != null)
					{
						base.vessel.parts[i].rb.mass = 0.01f;
					}
				}
			}
		}
		yield return StartCoroutine(MakePartSettle(deployedOnGround || (beingDeployed && kinematicDelay > 1f)));
		if (deployedOnGround || beingDeployed)
		{
			if (!base.part.GroundContact && kinematicDelay >= 1f)
			{
				Debug.LogFormat("[ModuleCargoPart]: Part {0} cannot deploy unless on the ground.", base.part.partInfo.title);
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8002326"), 3f, ScreenMessageStyle.UPPER_CENTER, persist: true);
				EnableModule();
				yield break;
			}
			Debug.LogFormat("[ModuleCargoPart]: Part {0} velocity {1} riveting to the ground.", base.part.partInfo.title, base.vessel.srf_velocity.magnitude);
			base.part.PermanentGroundContact = true;
			base.part.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			base.part.autoStrutMode = Part.AutoStrutMode.ForceGrandparent;
			PlayDeployAnimation();
			SendMessage("SetDeployedOnGroundFromCoRoutine");
		}
	}

	public void SetDeployedOnGroundFromCoRoutine()
	{
		deployedOnGround = true;
		beingDeployed = false;
		if (base.part.attachRules.allowStack || base.part.attachRules.allowSrfAttach)
		{
			base.part.prefabMass = 2f;
		}
		if (base.vessel.vesselType == VesselType.Unknown)
		{
			base.vessel.vesselType = VesselType.DeployedGroundPart;
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity") && groundScienceModule != null)
		{
			groundScienceModule.SetDeployedOnGround();
		}
		if (joint != null)
		{
			Object.Destroy(joint);
		}
		joint = base.gameObject.AddComponent<FixedJoint>();
		joint.breakForce = float.PositiveInfinity;
		joint.breakTorque = float.PositiveInfinity;
		joint.enablePreprocessing = false;
		if (childMass != null && childMass.Count > 0)
		{
			for (int i = 0; i < base.vessel.parts.Count; i++)
			{
				if (childMass.ContainsKey(base.vessel.parts[i].persistentId))
				{
					base.vessel.parts[i].prefabMass = childMass[base.vessel.parts[i].persistentId];
					base.vessel.parts[i].mass = 0.01f;
					if (base.vessel.parts[i].rb != null)
					{
						base.vessel.parts[i].rb.mass = 0.01f;
					}
				}
			}
			childMass.Clear();
		}
		UpdateModuleUI();
	}

	public void OnLeavingScene(GameScenes scn)
	{
		if (beingRetrieved)
		{
			base.vessel.state = Vessel.State.DEAD;
		}
	}

	public new void OnPartWillDie(Part p)
	{
		if (base.part != null && p.persistentId == base.part.persistentId && base.part.Rigidbody != null)
		{
			base.part.Rigidbody.constraints = RigidbodyConstraints.None;
			base.part.Rigidbody.ResetInertiaTensor();
		}
	}

	public override void PlayDeployAnimation()
	{
		if (animationGroup != null && animationGroup.isEnabled && !animationGroup.isDeployed)
		{
			animationGroup.DeployModule();
		}
		if (!string.IsNullOrEmpty(fxGroupDeploy) && base.part.findFxGroup(fxGroupDeploy) != null && beingDeployed)
		{
			base.part.findFxGroup(fxGroupDeploy).Burst();
		}
	}

	public void PlayRetractAnimation()
	{
		if (animationGroup != null && animationGroup.isEnabled && (animationGroup.isDeployed || base.vessel != null))
		{
			GameEvents.OnAnimationGroupRetractComplete.Add(OnRetractCompleted);
			animationGroup.RetractModule();
		}
		else
		{
			OnRetractCompleted(animationGroup);
		}
	}

	public void OnRetractCompleted(ModuleAnimationGroup aGroup)
	{
		if (!(aGroup == animationGroup))
		{
			return;
		}
		GameEvents.OnAnimationGroupRetractComplete.Remove(OnRetractCompleted);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			if (groundScienceModule != null)
			{
				GameEvents.onGroundSciencePartRemoved.Fire(groundScienceModule);
			}
			if (groundExpConModule != null)
			{
				GameEvents.onGroundScienceDeregisterCluster.Fire(base.part.persistentId);
			}
		}
		if (kerbalInventoryModule != null)
		{
			kerbalInventoryModule.partBeingRetrieved = false;
		}
		base.vessel.Die();
	}

	public void OnPartActionUIShown(UIPartActionWindow window, Part p)
	{
		if (p == base.part)
		{
			partActionWindow = window;
			UpdateModuleUI();
		}
	}

	public void OnPartActionUIDismiss(Part p)
	{
		if (p == base.part)
		{
			partActionWindow = null;
		}
	}

	public void OnVesselChange(Vessel vessel)
	{
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.isEVA)
		{
			kerbalInventoryModule = FlightGlobals.ActiveVessel.parts[0].FindModuleImplementing<ModuleInventoryPart>();
		}
	}

	public void UpdateModuleUI()
	{
		base.Events["RetrievePart"].active = false;
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.isEVA && kerbalInventoryModule != null)
		{
			firstEmptyInvSlot = kerbalInventoryModule.FirstEmptySlot();
			if (firstEmptyInvSlot > -1)
			{
				base.Events["RetrievePart"].active = true;
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6012033");
	}

	public override void EnableModule()
	{
		if (base.vessel != null && base.vessel.vesselType != VesselType.DroppedPart)
		{
			isEnabled = true;
		}
	}

	public virtual void DisableModule()
	{
		isEnabled = false;
	}

	public virtual bool ModuleIsActive()
	{
		if (groundScienceModule != null)
		{
			if (isEnabled)
			{
				return groundScienceModule.Enabled;
			}
			return false;
		}
		if (groundExpConModule != null)
		{
			if (isEnabled)
			{
				return groundExpConModule.Enabled;
			}
			return false;
		}
		return isEnabled;
	}

	public virtual bool IsSituationValid()
	{
		if (HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			return false;
		}
		return base.part.State != PartStates.PLACEMENT;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6006111 = Localizer.Format("#autoLOC_6006111");
		cacheAutoLOC_6006116 = Localizer.Format("#autoLOC_6006116");
	}
}
