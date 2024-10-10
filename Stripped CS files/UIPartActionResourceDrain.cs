using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;
using UnityEngine.UI;

[UI_Resources]
public class UIPartActionResourceDrain : UIPartActionFieldItem
{
	public UI_Resources resourcesControl;

	public VerticalLayoutGroup verticalLayout;

	public GameObject resourceTogglePrefab;

	public int fieldValue;

	public ModuleResourceDrain moduleResourceDrain;

	public List<UIPartActionResourceToggle> uiPartActionResourcetoggle;

	public static string cacheAutoLOC_6006009;

	public void Awake()
	{
		CacheLocalStrings();
	}

	public void OnDestroy()
	{
		if (uiPartActionResourcetoggle != null)
		{
			for (int i = 0; i < uiPartActionResourcetoggle.Count; i++)
			{
				UIPartActionResourceToggle uIPartActionResourceToggle = uiPartActionResourcetoggle[i];
				uIPartActionResourceToggle.sendPartStatus = (UIPartActionResourceToggle.PartStatus)Delegate.Remove(uIPartActionResourceToggle.sendPartStatus, new UIPartActionResourceToggle.PartStatus(UpdatePartResourceStatus));
			}
		}
	}

	public int GetFieldValue()
	{
		return field.GetValue<int>(field.host);
	}

	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		base.Setup(window, part, partModule, scene, control, field);
		moduleResourceDrain = partModule as ModuleResourceDrain;
		fieldValue = GetFieldValue();
		if (resourcesControl == null)
		{
			resourcesControl = (UI_Resources)control;
			resourcesControl.resourcesToDrain_PAW = this;
		}
		uiPartActionResourcetoggle = new List<UIPartActionResourceToggle>();
		InitializeResources();
	}

	public void InitializeResources()
	{
		if (moduleResourceDrain == null)
		{
			return;
		}
		for (int i = 0; i < moduleResourceDrain.resourcesAvailable.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(resourceTogglePrefab, verticalLayout.transform);
			uiPartActionResourcetoggle.Add(gameObject.GetComponent<UIPartActionResourceToggle>());
			if (uiPartActionResourcetoggle[uiPartActionResourcetoggle.Count - 1] != null)
			{
				uiPartActionResourcetoggle[uiPartActionResourcetoggle.Count - 1].InitializeItem("", moduleResourceDrain.resourcesAvailable[i], moduleResourceDrain.IsResourceDraining(moduleResourceDrain.resourcesAvailable[i]));
				UIPartActionResourceToggle uIPartActionResourceToggle = uiPartActionResourcetoggle[uiPartActionResourcetoggle.Count - 1];
				uIPartActionResourceToggle.sendPartStatus = (UIPartActionResourceToggle.PartStatus)Delegate.Combine(uIPartActionResourceToggle.sendPartStatus, new UIPartActionResourceToggle.PartStatus(UpdatePartResourceStatus));
			}
		}
		UpdateResourcesDrainRate();
		InitializeResourceFieldsDrainStatus();
	}

	public void UpdatePartResourceStatus(PartResource p, bool active)
	{
		moduleResourceDrain.TogglePartResource(p, active);
		for (int i = 0; i < part.symmetryCounterparts.Count; i++)
		{
			if (part.symmetryCounterparts[i].persistentId != part.persistentId)
			{
				ModuleResourceDrain component = part.symmetryCounterparts[i].GetComponent<ModuleResourceDrain>();
				component.TogglePartResource(p, active);
				component.ForceUpdateResourcePAW(p.resourceName, active);
			}
		}
	}

	public void UpdateResourcesDrainRate()
	{
		for (int i = 0; i < uiPartActionResourcetoggle.Count; i++)
		{
			uiPartActionResourcetoggle[i].UpdateItemName(Localizer.Format(cacheAutoLOC_6006009, uiPartActionResourcetoggle[i].partResource.info.displayName, (uiPartActionResourcetoggle[i].partResource.maxAmount * 0.009999999776482582 * (double)moduleResourceDrain.drainRate).ToString("F2")));
		}
	}

	public void UpdateResourcesDrainRateAndStatus(string resourceName, bool status)
	{
		int num = 0;
		while (true)
		{
			if (num < uiPartActionResourcetoggle.Count)
			{
				if (uiPartActionResourcetoggle[num].partResource.resourceName.Equals(resourceName))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		uiPartActionResourcetoggle[num].UpdateItemNameAndStatus(Localizer.Format(cacheAutoLOC_6006009, uiPartActionResourcetoggle[num].partResource.info.displayName, (uiPartActionResourcetoggle[num].partResource.maxAmount * 0.009999999776482582 * (double)moduleResourceDrain.drainRate).ToString("F2")), status);
	}

	public void InitializeResourceFieldsDrainStatus()
	{
		for (int i = 0; i < uiPartActionResourcetoggle.Count; i++)
		{
			uiPartActionResourcetoggle[i].toggle.isOn = moduleResourceDrain.IsResourceDraining(uiPartActionResourcetoggle[i].partResource);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6006009 = Localizer.Format("#autoLOC_6006009");
	}
}
