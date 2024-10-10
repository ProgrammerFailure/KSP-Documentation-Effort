using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleResourceDrain : PartModule, IConstruction
{
	[Serializable]
	public class ResourceDrainStatus
	{
		public PartResourceDefinition resource;

		public bool isDraining;

		public bool showDrainFX;

		public string drainFXName;

		public int fxPriority;

		public float drainISP;
	}

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006030")]
	public string noResourceAvailable = string.Empty;

	[KSPField(isPersistant = true)]
	public string resourceName;

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900321")]
	[UI_Resources]
	public int resourcesDraining;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 20f, minValue = 1f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(isPersistant = true, incrementalSpeed = 1f, axisGroup = KSPAxisGroup.None, guiActiveUnfocused = true, maxValue = 20f, groupDisplayName = "#autoLOC_6006010", minValue = 1f, unfocusedRange = 5f, groupName = "DrainResources", guiActiveEditor = false, guiActive = true, guiName = "#autoLOC_6006008", guiUnits = "%")]
	public float drainRate = 10f;

	public float minDrainRate = 1f;

	public float maxDrainRate = 20f;

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006003")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6006018", enabledText = "#autoLOC_6006017", affectSymCounterparts = UI_Scene.All)]
	public bool isDraining;

	public static double epsilon = 1E-15;

	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001017", enabledText = "#autoLOC_6001220", affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6006015")]
	public bool flowMode;

	public bool isInactive;

	public UI_Resources UI_Resources;

	public double drainAmount;

	public double amtReceived;

	public int highestValue;

	public int highestIndex;

	public float perResourceExhaustVel;

	public float perResourceThrust;

	public List<PartResource> resourcesAvailable;

	public List<ResourceDrainStatus> resourceDrainStatus;

	public int drainedResources;

	public string loadedSavedResources = "";

	public List<FXGroup> drainFXGroups;

	public FXGroup highestPriorityFXGroup;

	public List<string> loadedResources;

	public static string cacheAutoLOC_6006002;

	public static string cacheAutoLOC_6006003;

	public static string cacheAutoLOC_6006004;

	public static string cacheAutoLOC_6006015;

	public static string cacheAutoLOC_6006016;

	public static string cacheAutoLOC_6006031;

	public bool IsDraining
	{
		get
		{
			return isDraining;
		}
		set
		{
			isDraining = value;
			CheckDrainingForEffects(null);
			UpdateResourceDrainRate(null);
		}
	}

	public bool IsInactive => isInactive;

	public override void OnAwake()
	{
		CacheLocalStrings();
		base.Actions["ToggleResourceDrainAction"].guiName = cacheAutoLOC_6006002;
		base.Actions["ToggleResourceDrainFlowAction"].guiName = cacheAutoLOC_6006016;
		base.Fields["flowMode"].guiName = cacheAutoLOC_6006015;
		resourcesAvailable = new List<PartResource>();
		resourceDrainStatus = new List<ResourceDrainStatus>();
		drainFXGroups = new List<FXGroup>();
	}

	public override void OnStart(StartState state)
	{
		GameEvents.onEditorPartPlaced.Add(PartPlaced);
		GameEvents.onPartResourceListChange.Add(OnPartResourceListChanged);
		GameEvents.onPartActionUIShown.Add(OnPartMenuOpen);
		base.Fields["drainRate"].OnValueModified += UpdateResourceDrainRate;
		base.Fields["isDraining"].OnValueModified += CheckDrainingForEffects;
		drainRate = Mathf.Clamp(drainRate, minDrainRate, maxDrainRate);
		FXGroup fXGroup = null;
		if (base.part.parent != null)
		{
			for (int i = 0; i < base.part.parent.Resources.Count; i++)
			{
				if (base.part.parent.Resources[i].info.partResourceDrainDefinition.isDrainable)
				{
					fXGroup = base.part.findFxGroup(base.part.parent.Resources[i].info.partResourceDrainDefinition.drainFXDefinition);
					if (fXGroup != null)
					{
						drainFXGroups.AddUnique(fXGroup);
					}
				}
			}
		}
		Initialize();
	}

	public override void OnStartFinished(StartState state)
	{
		if (IsDraining)
		{
			CheckDrainingForEffects(null);
			UpdateResourceDrainRate(null);
		}
	}

	public void FixedUpdate()
	{
		if (isDraining && snapshot != null)
		{
			DrainPart();
		}
	}

	public void OnDestroy()
	{
		GameEvents.onEditorPartPlaced.Remove(PartPlaced);
		GameEvents.onPartResourceListChange.Remove(OnPartResourceListChanged);
		GameEvents.onPartActionUIShown.Remove(OnPartMenuOpen);
		base.Fields["drainRate"].OnValueModified -= UpdateResourceDrainRate;
		base.Fields["isDraining"].OnValueModified -= CheckDrainingForEffects;
	}

	[KSPAction(activeEditor = false)]
	public void StartResourceDrainAction(KSPActionParam param)
	{
		TurnOnDrain();
	}

	[KSPAction(activeEditor = false)]
	public void StopResourceDrainAction(KSPActionParam param)
	{
		TurnOffDrain();
	}

	[KSPAction(activeEditor = false)]
	public void ToggleResourceDrainAction(KSPActionParam param)
	{
		ToggleDrain();
	}

	[KSPAction(activeEditor = false)]
	public void ToggleResourceDrainFlowAction(KSPActionParam param)
	{
		TogglePartFlowMode();
	}

	public void ToggleDrainPartEvent()
	{
		ToggleDrain();
	}

	public void TogglePartFlowMode()
	{
		flowMode = !flowMode;
	}

	public void OnPartResourceListChanged(Part hostPart)
	{
		if (hostPart.persistentId == base.part.parent.persistentId)
		{
			LoadResources();
		}
	}

	public void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		if (inpPart.persistentId == base.part.persistentId)
		{
			UpdateVFXToUse();
		}
	}

	public void Initialize()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			UI_Resources = base.Fields["resourcesDraining"].uiControlFlight as UI_Resources;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			UI_Resources = base.Fields["resourcesDraining"].uiControlEditor as UI_Resources;
		}
		LoadResources();
		base.Actions["StartResourceDrainAction"].guiName = cacheAutoLOC_6006003;
		base.Actions["StopResourceDrainAction"].guiName = cacheAutoLOC_6006004;
		if (base.Fields["drainRate"] is BaseAxisField baseAxisField)
		{
			baseAxisField.minValue = minDrainRate;
			baseAxisField.maxValue = maxDrainRate;
		}
		noResourceAvailable = cacheAutoLOC_6006031;
		base.Fields["noResourceAvailable"].guiActive = resourcesAvailable.Count <= 0;
		base.Fields["noResourceAvailable"].guiActiveEditor = resourcesAvailable.Count <= 0;
	}

	public void PartPlaced(Part p)
	{
		if (!(p == null) && (base.part.persistentId == p.persistentId || p.isSymmetryCounterPart(base.part)))
		{
			Initialize();
		}
	}

	public void LoadResources()
	{
		if (base.part.parent == null)
		{
			isInactive = true;
			base.enabled = false;
		}
		else
		{
			resourcesAvailable.Clear();
			LoadResourcesAvailable();
			if (resourcesAvailable.Count == 0)
			{
				isInactive = true;
				base.enabled = false;
			}
		}
		if (loadedResources != null && loadedResources.Count > 0)
		{
			UpdateResourcesToRelease(loadedResources);
		}
	}

	public void LoadResourcesAvailable()
	{
		if (base.part.parent != null)
		{
			for (int i = 0; i < base.part.parent.Resources.Count; i++)
			{
				if (base.part.parent.Resources[i].info.partResourceDrainDefinition.isDrainable)
				{
					resourcesAvailable.AddUnique(base.part.parent.Resources[i]);
				}
			}
		}
		for (int j = 0; j < resourcesAvailable.Count; j++)
		{
			ResourceDrainStatus resourceDrainStatus = new ResourceDrainStatus();
			resourceDrainStatus.resource = resourcesAvailable[j].info;
			resourceDrainStatus.isDraining = loadedResources != null && loadedResources.Contains(resourcesAvailable[j].resourceName);
			resourceDrainStatus.showDrainFX = resourcesAvailable[j].info.partResourceDrainDefinition.showDrainFX;
			resourceDrainStatus.drainFXName = resourcesAvailable[j].info.partResourceDrainDefinition.drainFXDefinition;
			resourceDrainStatus.fxPriority = resourcesAvailable[j].info.partResourceDrainDefinition.drainFXPriority;
			resourceDrainStatus.drainISP = resourcesAvailable[j].info.partResourceDrainDefinition.drainForceISP;
			this.resourceDrainStatus.AddUnique(resourceDrainStatus);
		}
	}

	public void UpdateVFXToUse()
	{
		if (highestPriorityFXGroup != null)
		{
			highestPriorityFXGroup.setActive(value: false);
		}
		if (resourceDrainStatus.Count > 0)
		{
			UpdatePriorityIndices();
			highestPriorityFXGroup = SelectFXGroupByName(resourceDrainStatus[highestIndex].drainFXName);
		}
		else
		{
			highestPriorityFXGroup = null;
		}
		if (highestPriorityFXGroup != null && IsDraining && !HighLogic.LoadedSceneIsEditor)
		{
			highestPriorityFXGroup.setActive(value: true);
		}
	}

	public FXGroup SelectFXGroupByName(string fxName)
	{
		FXGroup result = null;
		for (int i = 0; i < drainFXGroups.Count; i++)
		{
			if (drainFXGroups[i].name.Equals(fxName))
			{
				result = drainFXGroups[i];
				break;
			}
		}
		return result;
	}

	public void UpdatePriorityIndices()
	{
		highestValue = 0;
		for (int i = 0; i < resourceDrainStatus.Count; i++)
		{
			if (resourceDrainStatus[i].showDrainFX && resourceDrainStatus[i].isDraining && DrainResourceNotFinished(i) && resourceDrainStatus[i].fxPriority > highestValue)
			{
				highestIndex = i;
				highestValue = resourceDrainStatus[i].fxPriority;
			}
		}
	}

	public bool HasEnoughDrainingResources()
	{
		bool result = false;
		Part parent = base.part.parent;
		double amount = 0.0;
		double maxAmount = 0.0;
		double num = 0.0;
		if (parent != null)
		{
			for (int i = 0; i < resourceDrainStatus.Count; i++)
			{
				if (resourceDrainStatus[i].isDraining)
				{
					parent.GetConnectedResourceTotals(resourceDrainStatus[i].resource.id, flowMode ? resourceDrainStatus[i].resource.resourceFlowMode : ResourceFlowMode.NO_FLOW, simulate: false, out amount, out maxAmount);
					num = maxAmount / (double)(100f / drainRate) * (double)TimeWarp.deltaTime;
					if (!(amount < num))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	public bool DrainResourceNotFinished(int rdsIndex)
	{
		Part parent = base.part.parent;
		double amount = 0.0;
		double maxAmount = 0.0;
		if (parent != null)
		{
			parent.GetConnectedResourceTotals(resourceDrainStatus[rdsIndex].resource.id, flowMode ? resourceDrainStatus[rdsIndex].resource.resourceFlowMode : ResourceFlowMode.NO_FLOW, simulate: false, out amount, out maxAmount);
			if (amount > 0.0)
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateResourcesToRelease(List<string> resourceNames, bool exclude = false)
	{
		if (exclude)
		{
			for (int i = 0; i < resourceNames.Count; i++)
			{
				for (int j = 0; j < resourcesAvailable.Count; j++)
				{
					if (string.Equals(resourcesAvailable[j].resourceName, resourceNames[i]))
					{
						UpdateResourceDrainStatus(resourcesAvailable[j].info.id, status: false);
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < resourceNames.Count; k++)
			{
				if (PartContainsResource(resourceNames[k]))
				{
					UpdateResourceDrainStatus(GetPartResource(resourceNames[k]).info.id, status: true);
				}
			}
			for (int l = 0; l < resourceDrainStatus.Count; l++)
			{
				if (!resourceNames.Contains(resourceDrainStatus[l].resource.name))
				{
					resourceDrainStatus[l].isDraining = false;
				}
			}
		}
		if (ResourcesToDrain() <= 0)
		{
			IsDraining = false;
		}
	}

	public void UpdateResourceDrainRate(object field)
	{
		if (highestPriorityFXGroup != null)
		{
			highestPriorityFXGroup.SetPower(drainRate * 0.05f);
		}
		if (UI_Resources != null && UI_Resources.resourcesToDrain_PAW != null)
		{
			UI_Resources.resourcesToDrain_PAW.UpdateResourcesDrainRate();
		}
	}

	public void UpdateResourceDrainRateAndStatus(string resourceName, bool status)
	{
		if (UI_Resources != null && UI_Resources.resourcesToDrain_PAW != null)
		{
			UI_Resources.resourcesToDrain_PAW.UpdateResourcesDrainRateAndStatus(resourceName, status);
		}
	}

	public void ForceUpdateResourcePAW(string resourceName, bool status)
	{
		UpdateResourceDrainRateAndStatus(resourceName, status);
	}

	public virtual void DrainPart()
	{
		drainAmount = 0.0;
		amtReceived = 0.0;
		drainedResources = 0;
		bool flag = false;
		double num = 0.0;
		for (int i = 0; i < resourcesAvailable.Count; i++)
		{
			if (CheatOptions.InfinitePropellant || GetResourceDrainStatus(resourcesAvailable[i]))
			{
				drainAmount = resourcesAvailable[i].maxAmount / (double)(100f / drainRate) * (double)TimeWarp.deltaTime;
				if (CheatOptions.InfinitePropellant)
				{
					amtReceived = drainAmount;
				}
				else if (flowMode)
				{
					amtReceived = base.part.parent.RequestResource(resourcesAvailable[i].info.id, drainAmount, resourcesAvailable[i].info.resourceFlowMode);
				}
				else
				{
					amtReceived = base.part.parent.RequestResource(resourcesAvailable[i].info.id, drainAmount, ResourceFlowMode.NO_FLOW);
				}
				if (amtReceived <= epsilon)
				{
					drainedResources++;
					flag = true;
					continue;
				}
				num = amtReceived * (double)resourcesAvailable[i].info.density;
				perResourceExhaustVel = resourcesAvailable[i].info.partResourceDrainDefinition.drainForceISP * 9.80665f;
				perResourceThrust = perResourceExhaustVel * (float)num / Time.fixedDeltaTime;
				base.part.AddForce(base.part.transform.forward * perResourceThrust);
			}
		}
		if (drainedResources == ResourcesToDrain())
		{
			IsDraining = false;
		}
		if (flag)
		{
			UpdateVFXToUse();
		}
	}

	public bool PartContainsResource(string partResource)
	{
		if (base.part.parent == null)
		{
			return false;
		}
		return base.part.parent.Resources.Contains(partResource);
	}

	public void TurnOnDrain()
	{
		if (ResourcesToDrain() > 0 && HasEnoughDrainingResources())
		{
			IsDraining = true;
		}
	}

	public void CheckDrainingForEffects(object obj)
	{
		if (!HighLogic.LoadedSceneIsEditor && highestPriorityFXGroup != null)
		{
			if (IsDraining && ResourcesToDrain() > 0 && ResourcesToDrainHaveFX() && HasEnoughDrainingResources())
			{
				highestPriorityFXGroup.setActive(value: true);
				UpdateResourceDrainRate(null);
			}
			else
			{
				highestPriorityFXGroup.setActive(value: false);
			}
		}
	}

	public void TurnOffDrain()
	{
		IsDraining = false;
	}

	public void TurnOffDrain(List<string> resourcesToStopDraining)
	{
		UpdateResourcesToRelease(resourcesToStopDraining, exclude: true);
	}

	public void ToggleDrain()
	{
		IsDraining = !IsDraining;
		if (IsDraining)
		{
			TurnOnDrain();
		}
		else
		{
			TurnOffDrain();
		}
	}

	public bool IsResourceDraining(PartResource pR)
	{
		return GetResourceDrainStatus(pR);
	}

	public PartResource GetPartResource(string resourceName)
	{
		if (base.part.parent == null)
		{
			return null;
		}
		int num = 0;
		while (true)
		{
			if (num < base.part.parent.Resources.Count)
			{
				if (base.part.parent.Resources[num].info.partResourceDrainDefinition.isDrainable && string.Equals(base.part.parent.Resources[num].info.name, resourceName))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return base.part.parent.Resources[num];
	}

	public void TogglePartResource(PartResource resource, bool status)
	{
		if (resource != null)
		{
			UpdateResourceDrainStatus(resource.info.id, status);
		}
	}

	public void UpdateResourceDrainStatus(int resourceID, bool status)
	{
		for (int i = 0; i < resourceDrainStatus.Count; i++)
		{
			if (resourceDrainStatus[i].resource.id == resourceID)
			{
				resourceDrainStatus[i].isDraining = status;
			}
		}
		UpdateVFXToUse();
	}

	public int ResourcesToDrain()
	{
		resourcesDraining = 0;
		for (int i = 0; i < resourceDrainStatus.Count; i++)
		{
			if (CheatOptions.InfinitePropellant || resourceDrainStatus[i].isDraining)
			{
				resourcesDraining++;
			}
		}
		return resourcesDraining;
	}

	public bool ResourcesToDrainHaveFX()
	{
		int num = 0;
		while (true)
		{
			if (num < resourceDrainStatus.Count)
			{
				if ((CheatOptions.InfinitePropellant || resourceDrainStatus[num].isDraining) && resourceDrainStatus[num].showDrainFX)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool GetResourceDrainStatus(PartResource resource)
	{
		int num = 0;
		while (true)
		{
			if (num < resourceDrainStatus.Count)
			{
				if (resourceDrainStatus[num].resource.id == resource.info.id)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		if (!CheatOptions.InfinitePropellant)
		{
			return resourceDrainStatus[num].isDraining;
		}
		return true;
	}

	public virtual bool CanBeDetached()
	{
		return !isDraining;
	}

	public virtual bool CanBeOffset()
	{
		return !isDraining;
	}

	public virtual bool CanBeRotated()
	{
		return !isDraining;
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		if (resourcesAvailable.Count > 0)
		{
			string text = "";
			for (int i = 0; i < resourcesAvailable.Count; i++)
			{
				if (GetResourceDrainStatus(resourcesAvailable[i]))
				{
					text = ((!string.IsNullOrEmpty(text)) ? (text + "," + resourcesAvailable[i].resourceName) : (text + resourcesAvailable[i].resourceName));
				}
			}
			node.AddValue("resourcesToRelease", text);
		}
		else
		{
			node.AddValue("resourcesToRelease", loadedSavedResources);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		node.TryGetValue("resourcesToRelease", ref loadedSavedResources);
		if (string.IsNullOrEmpty(loadedSavedResources))
		{
			return;
		}
		loadedResources = new List<string>();
		string[] array = loadedSavedResources.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				loadedResources.Add(array[i]);
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6006002 = Localizer.Format("#autoLOC_6006002");
		cacheAutoLOC_6006003 = Localizer.Format("#autoLOC_6006003");
		cacheAutoLOC_6006004 = Localizer.Format("#autoLOC_6006004");
		cacheAutoLOC_6006015 = Localizer.Format("#autoLOC_6006015");
		cacheAutoLOC_6006016 = Localizer.Format("#autoLOC_6006016");
		cacheAutoLOC_6006031 = Localizer.Format("#autoLOC_6006031");
	}
}
