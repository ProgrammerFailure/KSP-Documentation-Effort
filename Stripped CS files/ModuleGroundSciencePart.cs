using System.Collections;
using System.Collections.Generic;
using Expansions;
using Expansions.Serenity.DeployedScience.Runtime;
using Experience;
using Experience.Effects;
using ns11;
using ns9;
using UnityEngine;

public class ModuleGroundSciencePart : ModuleGroundPart
{
	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002231")]
	public string ConnectionState;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002232")]
	public string PowerState;

	[KSPField(guiActiveUnfocused = false, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8002353")]
	[SerializeField]
	public int powerUnitsProduced;

	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002235")]
	[SerializeField]
	public int actualPowerUnitsProduced;

	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002236")]
	[SerializeField]
	public int powerUnitsRequired;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public bool isSolarPanel;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public uint ControlUnitId;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public float DistanceToController;

	[KSPField(unfocusedRange = 20f, guiActiveUnfocused = true, isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002252")]
	[SerializeField]
	public bool moduleEnabled = true;

	[SerializeField]
	public DeployedScienceCluster scienceClusterData;

	[KSPField]
	public bool TrackSun;

	[KSPField]
	public string secondaryTransformName = string.Empty;

	[KSPField]
	public float raycastOffset = 0.25f;

	[KSPField]
	public string pivotName = "suncatcher";

	[KSPField]
	public float trackingSpeed = 0.25f;

	[KSPField]
	public string localRotationAxis = "Z";

	[KSPField]
	public Vector3 targetRotationAngle;

	[KSPField]
	public float packingRotationMultiplier;

	public new UIPartActionWindow partActionWindow;

	public RaycastHit hit;

	public bool trackingLOS;

	public string blockingObject;

	public CelestialBody trackingBody;

	public Transform secondaryTransform;

	public int planetLayerMask;

	public int defaultLayerMask;

	public Transform trackingTransformScaled;

	public Transform panelRotationTransform;

	public static string cacheAutoLOC_7003285;

	public static string cacheAutoLOC_8002238;

	public static string cacheAutoLOC_8002239;

	public static string cacheAutoLOC_8002240;

	public static string cacheAutoLOC_8002241;

	public static string cacheAutoLOC_8002242;

	public static string cacheAutoLOC_8002243;

	public static string cacheAutoLOC_8002244;

	public static string cacheAutoLOC_8002253;

	public static string cacheAutoLOC_6006034;

	public int PowerUnitsProduced
	{
		get
		{
			return powerUnitsProduced;
		}
		set
		{
			if (value != powerUnitsProduced)
			{
				powerUnitsProduced = value;
				if (!beingRetrieved)
				{
					GameEvents.onGroundSciencePartChanged.Fire(this);
				}
			}
		}
	}

	public int ActualPowerUnitsProduced
	{
		get
		{
			return actualPowerUnitsProduced;
		}
		set
		{
			if (value != actualPowerUnitsProduced)
			{
				actualPowerUnitsProduced = value;
				if (!beingRetrieved)
				{
					GameEvents.onGroundSciencePartChanged.Fire(this);
				}
			}
		}
	}

	public int PowerUnitsRequired
	{
		get
		{
			return powerUnitsRequired;
		}
		set
		{
			if (value != powerUnitsRequired)
			{
				powerUnitsRequired = value;
				if (!beingRetrieved)
				{
					GameEvents.onGroundSciencePartChanged.Fire(this);
				}
			}
		}
	}

	public bool IsSolarPanel
	{
		get
		{
			return isSolarPanel;
		}
		set
		{
			if (value != isSolarPanel)
			{
				isSolarPanel = value;
				if (!beingRetrieved)
				{
					GameEvents.onGroundSciencePartChanged.Fire(this);
				}
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return moduleEnabled;
		}
		set
		{
			if (value != moduleEnabled)
			{
				moduleEnabled = value;
				if (!beingRetrieved)
				{
					GameEvents.onGroundSciencePartEnabledStateChanged.Fire(this);
					UpdateModuleUI();
				}
			}
		}
	}

	public bool DeployedOnGround => deployedOnGround;

	public DeployedScienceCluster ScienceClusterData
	{
		get
		{
			if (scienceClusterData == null && (bool)DeployedScience.Instance && ExpansionsLoader.IsExpansionInstalled("Serenity"))
			{
				DeployedScience.Instance.DeployedScienceClusters.TryGetValue(ControlUnitId, out scienceClusterData);
			}
			return scienceClusterData;
		}
	}

	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 4f, guiName = "")]
	public void ToggleActive()
	{
		if (deployedOnGround)
		{
			Enabled = !Enabled;
			UpdateModuleUI();
		}
	}

	[KSPAction(guiName = "#autoLOC_8002237")]
	public void ToggleActiveAction()
	{
		ToggleActive();
	}

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
			Object.Destroy(this);
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		DistanceToController = float.MaxValue;
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		ActualPowerUnitsProduced = powerUnitsProduced;
		trackingBody = Planetarium.fetch.Sun;
		if ((bool)trackingBody.scaledBody)
		{
			trackingTransformScaled = trackingBody.scaledBody.transform;
		}
		if (!string.IsNullOrEmpty(secondaryTransformName))
		{
			secondaryTransform = base.part.FindModelTransform(secondaryTransformName);
			if (secondaryTransform == null)
			{
				Debug.LogError("Couldn't access secondaryTransform");
			}
		}
		planetLayerMask = 1 << LayerMask.NameToLayer("Scaled Scenery");
		defaultLayerMask = (1 << LayerMask.NameToLayer("PhysicalObjects")) | (1 << LayerMask.NameToLayer("TerrainColliders")) | (1 << LayerMask.NameToLayer("Local Scenery")) | LayerUtil.DefaultEquivalent;
		panelRotationTransform = base.part.FindModelTransform(pivotName);
		if (HighLogic.LoadedSceneIsFlight && (state & StartState.Landed) != 0 && base.vessel.vesselType != VesselType.DroppedPart)
		{
			GameEvents.onGroundScienceDeregisterCluster.Add(OnGroundScienceDeregisterCluster);
			GameEvents.onGroundScienceClusterUpdated.Add(OnGroundScienceClusterUpdated);
			GameEvents.onGroundScienceClusterPowerStateChanged.Add(OnGroundScienceClusterPowerStateChanged);
			if (base.part.FindModuleImplementing<ModuleGroundExpControl>() == null && (bool)DeployedScience.Instance && ControlUnitId == 0)
			{
				if (base.vessel != null)
				{
					base.vessel.vesselType = VesselType.DeployedScienceController;
					if (base.vessel.orbitRenderer != null)
					{
						base.vessel.orbitRenderer.RefreshMapObject();
					}
					if ((bool)KSCVesselMarkers.fetch)
					{
						KSCVesselMarkers.fetch.RefreshMarkers();
					}
					GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(base.vessel, base.vessel.vesselName, base.vessel.vesselName));
				}
				GameEvents.onGroundSciencePartDeployed.Fire(this);
			}
		}
		if (base.vessel != null && base.vessel.vesselType != VesselType.DroppedPart)
		{
			GameEvents.onPartActionUIShown.Add(OnPartActionUIOpened);
			GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
		}
		base.Fields["powerUnitsProduced"].guiActive = isSolarPanel;
	}

	public override void OnDestroy()
	{
		ActualPowerUnitsProduced = powerUnitsProduced;
		base.OnDestroy();
		GameEvents.onGroundScienceDeregisterCluster.Remove(OnGroundScienceDeregisterCluster);
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIOpened);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
		GameEvents.onGroundScienceClusterUpdated.Remove(OnGroundScienceClusterUpdated);
		GameEvents.onGroundScienceClusterPowerStateChanged.Remove(OnGroundScienceClusterPowerStateChanged);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (partActionWindow != null)
		{
			UpdateModuleUI();
		}
	}

	public void FixedUpdate()
	{
		if (!TrackSun)
		{
			return;
		}
		if (deployedOnGround && isEnabled && base.enabled && secondaryTransform != null && trackingBody != null && TimeWarp.CurrentRate == 1f)
		{
			Vector3 normalized = (trackingBody.transform.position - base.part.partTransform.position).normalized;
			trackingLOS = CalculateTrackingLOS(normalized, ref blockingObject);
			if (trackingLOS)
			{
				if (isSolarPanel && actualPowerUnitsProduced != powerUnitsProduced)
				{
					ActualPowerUnitsProduced = powerUnitsProduced;
				}
				if (panelRotationTransform != null)
				{
					Vector3 vector = panelRotationTransform.InverseTransformPoint(trackingBody.transform.position);
					float num = Mathf.Atan2(vector.y, vector.z) * 57.29578f;
					Quaternion b = Quaternion.identity;
					switch (localRotationAxis)
					{
					case "X":
						b = panelRotationTransform.localRotation * Quaternion.Euler(num, 0f, 0f);
						break;
					case "Y":
						b = panelRotationTransform.localRotation * Quaternion.Euler(0f, num, 0f);
						break;
					case "Z":
						b = panelRotationTransform.localRotation * Quaternion.Euler(0f, 0f, num);
						break;
					}
					panelRotationTransform.localRotation = Quaternion.Lerp(panelRotationTransform.localRotation, b, TimeWarp.deltaTime * trackingSpeed);
				}
			}
			else if (isSolarPanel && actualPowerUnitsProduced > 0)
			{
				ActualPowerUnitsProduced = 0;
			}
		}
		else if (!deployedOnGround || !isEnabled || !base.enabled)
		{
			ActualPowerUnitsProduced = 0;
		}
	}

	public bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		bool result = true;
		Ray ray = new Ray(ScaledSpace.LocalToScaledSpace(secondaryTransform.position), ScaledSpace.LocalToScaledSpace(trackingBody.transform.position).normalized);
		float maxDistance = float.MaxValue;
		if (Physics.Raycast(ray, out hit, maxDistance, planetLayerMask) && hit.transform != trackingTransformScaled)
		{
			blocker = hit.transform.gameObject.name;
			result = false;
		}
		else
		{
			ray = new Ray(secondaryTransform.position + trackingDirection * raycastOffset, trackingBody.transform.position.normalized);
			Debug.DrawRay(ray.origin, ray.direction, Color.green);
			if (Physics.Raycast(ray, out hit, maxDistance, defaultLayerMask))
			{
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
				{
					if ((bool)hit.transform.gameObject && (bool)hit.transform.gameObject.GetComponent<GClass3>())
					{
						blocker = "Terrain";
					}
					else
					{
						Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hit.transform.gameObject);
						if (partUpwardsCached != null)
						{
							blocker = partUpwardsCached.partInfo.title;
						}
						else if (hit.transform.gameObject.tag.Contains("KSC"))
						{
							blocker = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(hit.transform.gameObject.tag, formatted: true);
						}
						else
						{
							blocker = hit.transform.gameObject.name;
						}
					}
				}
				result = false;
			}
		}
		return result;
	}

	public void SetDeployedOnGround()
	{
		if (!beingRetrieved)
		{
			GameEvents.onGroundSciencePartEnabledStateChanged.Fire(this);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if ((bool)DeployedScience.Instance && DeployedScience.IsActive && ExpansionsLoader.IsExpansionInstalled("Serenity") && ScienceClusterData != null)
		{
			DeployedSciencePart deployedSciencePart = scienceClusterData.DeployedScienceParts.Get(base.part.persistentId);
			if (deployedSciencePart != null)
			{
				powerUnitsProduced = deployedSciencePart.PowerUnitsProduced;
				powerUnitsRequired = deployedSciencePart.PowerUnitsRequired;
				isSolarPanel = deployedSciencePart.IsSolarPanel;
				moduleEnabled = deployedSciencePart.Enabled;
			}
		}
	}

	public override void RetrievePart()
	{
		if (!CanBeStored())
		{
			return;
		}
		beingRetrieved = true;
		deployedOnGround = false;
		ControlUnitId = 0u;
		DistanceToController = float.MaxValue;
		AvailablePart availablePart = ((base.part != null) ? base.part.partInfo : null);
		if (availablePart != null)
		{
			ModuleGroundSciencePart moduleGroundSciencePart = availablePart.partPrefab.FindModuleImplementing<ModuleGroundSciencePart>();
			if (moduleGroundSciencePart != null)
			{
				powerUnitsProduced = moduleGroundSciencePart.powerUnitsProduced;
			}
		}
		if (TrackSun)
		{
			StartCoroutine(SunTrackerToOrigin());
			return;
		}
		RetrieveScienceData();
		base.RetrievePart();
	}

	public IEnumerator SunTrackerToOrigin()
	{
		Quaternion rotationAngle = Quaternion.Euler(targetRotationAngle);
		while (panelRotationTransform.localRotation != rotationAngle)
		{
			float maxDegreesDelta = trackingSpeed * packingRotationMultiplier + TimeWarp.deltaTime;
			panelRotationTransform.localRotation = Quaternion.RotateTowards(panelRotationTransform.localRotation, rotationAngle, maxDegreesDelta);
			yield return null;
		}
		RetrieveScienceData();
		base.RetrievePart();
	}

	public void OnGroundScienceClusterUpdated(ModuleGroundExpControl controlUnit, DeployedScienceCluster cluster)
	{
		if (ControlUnitId == controlUnit.ControlUnitId)
		{
			UpdateModuleUI();
		}
	}

	public void OnGroundScienceClusterPowerStateChanged(DeployedScienceCluster scienceCluster)
	{
		if (ControlUnitId == scienceCluster.ControlModulePartId)
		{
			UpdateModuleUI();
		}
	}

	public void OnGroundScienceDeregisterCluster(uint controlUnitId)
	{
		if (controlUnitId != ControlUnitId)
		{
			return;
		}
		ControlUnitId = 0u;
		DistanceToController = float.MaxValue;
		if (base.vessel != null && base.part.persistentId != controlUnitId)
		{
			base.vessel.vesselType = VesselType.DeployedScienceController;
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(base.vessel, base.vessel.vesselName, base.vessel.vesselName));
			if ((bool)KSCVesselMarkers.fetch)
			{
				KSCVesselMarkers.fetch.RefreshMarkers();
			}
			if (base.vessel.orbitRenderer != null)
			{
				base.vessel.orbitRenderer.RefreshMapObject();
			}
		}
		if (!beingRetrieved)
		{
			GameEvents.onGroundSciencePartDeployed.Fire(this);
		}
	}

	public void OnPartActionUIOpened(UIPartActionWindow window, Part p)
	{
		if (p == base.part)
		{
			partActionWindow = window;
			UpdateModuleUI();
		}
	}

	public new void OnPartActionUIDismiss(Part p)
	{
		if (p == base.part)
		{
			partActionWindow = null;
		}
	}

	public new virtual void UpdateModuleUI()
	{
		PowerState = cacheAutoLOC_7003285;
		base.Events["ToggleActive"].guiActive = deployedOnGround;
		if (Enabled)
		{
			base.Events["ToggleActive"].guiName = cacheAutoLOC_8002238;
		}
		else
		{
			base.Events["ToggleActive"].guiName = cacheAutoLOC_8002239;
		}
		if (ScienceClusterData != null)
		{
			ConnectionState = cacheAutoLOC_8002240;
			if (Enabled && DeployedOnGround)
			{
				if (scienceClusterData.IsPowered)
				{
					PowerState = cacheAutoLOC_8002241;
				}
				else if (scienceClusterData.ControllerPartEnabled)
				{
					PowerState = cacheAutoLOC_8002242;
				}
				else
				{
					PowerState = cacheAutoLOC_8002253;
				}
			}
			else
			{
				PowerState = cacheAutoLOC_8002243;
			}
		}
		else
		{
			ConnectionState = cacheAutoLOC_8002244;
		}
	}

	public override string GetInfo()
	{
		string text = base.GetInfo();
		if (packedVolume > 0f || stackableQuantity > 1)
		{
			text += "\n";
		}
		text = text + Localizer.Format("<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002222") + "</color>") + "\n";
		if (powerUnitsRequired > 0)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8002236"), powerUnitsRequired) + "\n";
		}
		if (powerUnitsProduced > 0)
		{
			text += "\n";
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8002235"), "") + "\n";
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_6006033"), powerUnitsProduced) + "\n";
			ExperienceTraitConfig traitConfig = null;
			List<string> traitsNamesWithEffect = GameDatabase.Instance.ExperienceConfigs.GetTraitsNamesWithEffect("DeployedSciencePowerSkill");
			string empty = string.Empty;
			ConfigNode configNode = null;
			DeployedSciencePowerSkill deployedSciencePowerSkill = new DeployedSciencePowerSkill(new ExperienceTrait());
			for (int i = 0; i < traitsNamesWithEffect.Count; i++)
			{
				KerbalRoster.TryGetExperienceTraitConfig(traitsNamesWithEffect[i], out traitConfig);
				if (traitConfig == null || traitConfig.Effects == null)
				{
					continue;
				}
				for (int j = 0; j < traitConfig.Effects.Count; j++)
				{
					if (!(traitConfig.Effects[j].Name == "DeployedSciencePowerSkill"))
					{
						continue;
					}
					configNode = traitConfig.Effects[j].Config;
					deployedSciencePowerSkill.LoadFromConfig(configNode);
					if (deployedSciencePowerSkill != null && deployedSciencePowerSkill.LevelModifiers != null)
					{
						for (int k = 1; k < deployedSciencePowerSkill.LevelModifiers.Length; k++)
						{
							empty = Localizer.Format(traitConfig.Title);
							text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format(cacheAutoLOC_6006034, empty, k), (float)powerUnitsProduced + deployedSciencePowerSkill.GetTraitBonus(k)) + "\n";
						}
					}
				}
			}
		}
		return text + "\n";
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8002223");
	}

	public override string GetTooltip()
	{
		if (string.IsNullOrEmpty(inventoryTooltip))
		{
			return GetInfo();
		}
		return Localizer.Format(inventoryTooltip, PowerUnitsRequired);
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_7003285 = Localizer.Format("#autoLOC_7003285");
		cacheAutoLOC_8002238 = Localizer.Format("#autoLOC_8002238");
		cacheAutoLOC_8002239 = Localizer.Format("#autoLOC_8002239");
		cacheAutoLOC_8002240 = Localizer.Format("#autoLOC_8002240");
		cacheAutoLOC_8002241 = Localizer.Format("#autoLOC_8002241");
		cacheAutoLOC_8002242 = Localizer.Format("#autoLOC_8002242");
		cacheAutoLOC_8002243 = Localizer.Format("#autoLOC_8002243");
		cacheAutoLOC_8002244 = Localizer.Format("#autoLOC_8002244");
		cacheAutoLOC_8002253 = Localizer.Format("#autoLOC_8002253");
		cacheAutoLOC_6006034 = Localizer.Format("#autoLOC_6006034");
	}
}
