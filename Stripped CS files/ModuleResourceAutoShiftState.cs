using ns9;
using UnityEngine;

public class ModuleResourceAutoShiftState : PartModule
{
	[KSPField(isPersistant = true)]
	public string affectedResourceName = string.Empty;

	[KSPField(isPersistant = true)]
	public string affectedModuleName = string.Empty;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "-")]
	[UI_Label]
	public string moduleAutoShiftName = Localizer.Format("#autoLOC_6005054", "#autoLOC_6001479");

	public UI_Label moduleAutoShiftNameField;

	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005057")]
	public bool resourceShutOffStartUpUsePercent;

	[KSPField(isPersistant = true)]
	public bool resourceShutOffHandler;

	[KSPField(isPersistant = true)]
	public bool resourceStartUpHandler;

	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Shutoff amount")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5000f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float resourceShutOffAmount;

	public UI_FloatRange resourceShutOffAmountField;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Shutoff percent")]
	public float resourceShutOffPercent;

	public UI_FloatRange resourceShutOffPercentField;

	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto-startup amount")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5000f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float resourceStartUpAmount;

	public UI_FloatRange resourceStartUpAmountField;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Startup percent")]
	public float resourceStartUpPercent;

	public UI_FloatRange resourceStartUpPercentField;

	public PartResource partResource;

	public ModuleResource resource;

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (string.IsNullOrEmpty(affectedResourceName))
		{
			Object.Destroy(this);
			return;
		}
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("resourceShutOffAmount", out resourceShutOffAmountField);
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("resourceShutOffPercent", out resourceShutOffPercentField);
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("resourceStartUpAmount", out resourceStartUpAmountField);
		base.Fields.TryGetFieldUIControl<UI_FloatRange>("resourceStartUpPercent", out resourceStartUpPercentField);
		base.Fields.TryGetFieldUIControl<UI_Label>("moduleAutoShiftName", out moduleAutoShiftNameField);
		base.Fields["resourceShutOffStartUpUsePercent"].OnValueModified += ModifyAmountPercent;
		base.Fields["resourceShutOffAmount"].OnValueModified += ModifyShutOffAmount;
		base.Fields["resourceShutOffPercent"].OnValueModified += ModifyShutOffPercent;
		base.Fields["resourceStartUpAmount"].OnValueModified += ModifyStartUpAmount;
		base.Fields["resourceStartUpPercent"].OnValueModified += ModifyStartUpPercent;
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(affectedResourceName);
		if (HighLogic.LoadedSceneIsFlight)
		{
			for (int i = 0; i < base.vessel.Parts.Count; i++)
			{
				Part part = base.vessel.Parts[i];
				for (int j = 0; j < part.Resources.Count; j++)
				{
					if (part.Resources[j].info.name.Equals(affectedResourceName))
					{
						partResource = part.Resources[j];
						resource = base.part.Modules.GetModule(affectedModuleName).resHandler.inputResources.Find((ModuleResource r) => r.name.Equals(affectedResourceName));
						break;
					}
				}
				if (!string.IsNullOrEmpty(partResource.resourceName))
				{
					break;
				}
			}
			if (resource != null && !string.IsNullOrEmpty(partResource.resourceName))
			{
				resource.autoStateShifter = this;
				if (resourceShutOffHandler)
				{
					resource.shutOffHandler = true;
					resource.shutOffStartUpUsePercent = resourceShutOffStartUpUsePercent;
					resource.shutOffAmount = resourceShutOffAmount;
					resource.shutOffPercent = resourceShutOffPercent;
					if (resourceShutOffAmountField != null && resourceShutOffPercentField != null)
					{
						resourceShutOffAmountField.maxValue = (float)partResource.maxAmount;
						base.Fields["resourceShutOffAmount"].guiName = Localizer.Format("#autoLOC_6005055", definition.displayName, definition.abbreviation);
						base.Fields["resourceShutOffPercent"].guiName = Localizer.Format("#autoLOC_6005055", definition.displayName, "%");
					}
					moduleAutoShiftName = Localizer.Format("#autoLOC_6005054", partResource.info.displayName);
				}
				if (resourceStartUpHandler)
				{
					resource.startUpHandler = true;
					resource.shutOffStartUpUsePercent = resourceShutOffStartUpUsePercent;
					resource.startUpAmount = resourceStartUpAmount;
					resource.startUpPercent = resourceStartUpPercent;
					if (resourceStartUpAmountField != null && resourceStartUpPercentField != null)
					{
						resourceStartUpAmountField.maxValue = (float)partResource.maxAmount;
						base.Fields["resourceStartUpAmount"].guiName = Localizer.Format("#autoLOC_6005056", definition.displayName, definition.abbreviation);
						base.Fields["resourceStartUpPercent"].guiName = Localizer.Format("#autoLOC_6005056", definition.displayName, "%");
					}
					moduleAutoShiftName = Localizer.Format("#autoLOC_6005054", partResource.info.displayName);
				}
				ModifyAmountPercent(null);
				moduleAutoShiftNameField.SetSceneVisibility(UI_Scene.None, state: false);
			}
			else
			{
				resourceShutOffAmountField.SetSceneVisibility(UI_Scene.None, state: false);
				resourceShutOffPercentField.SetSceneVisibility(UI_Scene.None, state: false);
				resourceStartUpAmountField.SetSceneVisibility(UI_Scene.None, state: false);
				resourceStartUpPercentField.SetSceneVisibility(UI_Scene.None, state: false);
				moduleAutoShiftNameField.SetSceneVisibility(UI_Scene.None, state: false);
			}
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			if (resourceShutOffAmountField != null && resourceShutOffPercentField != null)
			{
				base.Fields["resourceShutOffAmount"].guiName = Localizer.Format("#autoLOC_6005055", definition.displayName, definition.abbreviation);
				base.Fields["resourceShutOffPercent"].guiName = Localizer.Format("#autoLOC_6005055", definition.displayName, "%");
			}
			if (resourceStartUpAmountField != null && resourceStartUpPercentField != null)
			{
				base.Fields["resourceStartUpAmount"].guiName = Localizer.Format("#autoLOC_6005056", definition.displayName, definition.abbreviation);
				base.Fields["resourceStartUpPercent"].guiName = Localizer.Format("#autoLOC_6005056", definition.displayName, "%");
			}
			ModifyAmountPercent(null);
			moduleAutoShiftNameField.SetSceneVisibility(UI_Scene.None, state: false);
		}
	}

	public void OnDestroy()
	{
		base.Fields["resourceShutOffAmount"].OnValueModified -= ModifyShutOffAmount;
		base.Fields["resourceShutOffPercent"].OnValueModified -= ModifyShutOffPercent;
		base.Fields["resourceStartUpAmount"].OnValueModified -= ModifyStartUpAmount;
		base.Fields["resourceStartUpPercent"].OnValueModified -= ModifyStartUpPercent;
		base.Fields["resourceShutOffStartUpUsePercent"].OnValueModified -= ModifyAmountPercent;
	}

	public void ModifyStartUpPercent(object field)
	{
		if (resourceStartUpPercent <= resourceShutOffPercent)
		{
			resourceStartUpPercent = resourceShutOffPercent + 1f;
		}
		if (resource != null)
		{
			resource.startUpPercent = resourceStartUpPercent;
		}
	}

	public void ModifyShutOffAmount(object field)
	{
		if (resourceShutOffAmount >= resourceStartUpAmount)
		{
			resourceShutOffAmount = resourceStartUpAmount - 1f;
		}
		if (resource != null)
		{
			resource.shutOffAmount = resourceShutOffAmount;
		}
	}

	public void ModifyStartUpAmount(object field)
	{
		if (resourceStartUpAmount <= resourceShutOffAmount)
		{
			resourceStartUpAmount = resourceShutOffAmount + 1f;
		}
		if (resource != null)
		{
			resource.startUpAmount = resourceStartUpAmount;
		}
	}

	public void ModifyShutOffPercent(object field)
	{
		if (resourceShutOffPercent >= resourceStartUpPercent)
		{
			resourceShutOffPercent = resourceStartUpPercent - 1f;
		}
		if (resource != null)
		{
			resource.shutOffPercent = resourceShutOffPercent;
		}
	}

	public void ModifyAmountPercent(object field)
	{
		if (resource != null)
		{
			resource.shutOffStartUpUsePercent = resourceShutOffStartUpUsePercent;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			resourceShutOffAmountField.SetSceneVisibility(UI_Scene.Flight, !resourceShutOffStartUpUsePercent && resourceShutOffHandler);
			resourceShutOffPercentField.SetSceneVisibility(UI_Scene.Flight, resourceShutOffStartUpUsePercent && resourceShutOffHandler);
			resourceStartUpAmountField.SetSceneVisibility(UI_Scene.Flight, !resourceShutOffStartUpUsePercent && resourceStartUpHandler);
			resourceStartUpPercentField.SetSceneVisibility(UI_Scene.Flight, resourceShutOffStartUpUsePercent && resourceStartUpHandler);
		}
		else
		{
			resourceShutOffAmountField.SetSceneVisibility(UI_Scene.Editor, !resourceShutOffStartUpUsePercent && resourceShutOffHandler);
			resourceShutOffPercentField.SetSceneVisibility(UI_Scene.Editor, resourceShutOffStartUpUsePercent && resourceShutOffHandler);
			resourceStartUpAmountField.SetSceneVisibility(UI_Scene.Editor, !resourceShutOffStartUpUsePercent && resourceStartUpHandler);
			resourceStartUpPercentField.SetSceneVisibility(UI_Scene.Editor, resourceShutOffStartUpUsePercent && resourceStartUpHandler);
		}
	}

	public void ToggleWarningVisibility(bool state)
	{
		if (moduleAutoShiftNameField != null)
		{
			moduleAutoShiftNameField.SetSceneVisibility(UI_Scene.Flight, state);
		}
	}
}
