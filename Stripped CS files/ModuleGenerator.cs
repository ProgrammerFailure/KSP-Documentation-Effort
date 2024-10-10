using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;

public class ModuleGenerator : PartModule, IContractObjectiveModule
{
	[KSPField(isPersistant = true)]
	public bool generatorIsActive;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001422", guiUnits = "")]
	public float efficiency;

	[KSPField]
	public string efficiencyGUIName = "#autoLOC_6001422";

	[KSPField]
	public string activateGUIName = "#autoLOC_235502";

	[KSPField]
	public string shutdownGUIName = "#autoLOC_235505";

	[KSPField]
	public string toggleGUIName = "#autoLOC_235508";

	[KSPField]
	public bool isAlwaysActive;

	[KSPField]
	public bool isGroundFixture;

	[KSPField]
	public bool requiresAllInputs = true;

	[KSPField]
	public float resourceThreshold = 0.1f;

	[KSPField]
	public bool isThrottleControlled;

	[KSPField(isPersistant = true)]
	public float throttle;

	[KSPField(guiActive = true, guiName = "#autoLOC_235532")]
	public string displayStatus = string.Empty;

	[KSPField]
	public string status = string.Empty;

	[KSPField]
	public string statusGUIName = "#autoLOC_235532";

	public ModuleResource currentResource;

	public double inputRecieved;

	public bool lackingResources;

	public List<AdjusterGeneratorBase> adjusterCache = new List<AdjusterGeneratorBase>();

	public static string cacheAutoLOC_219034;

	public static string cacheAutoLOC_220477;

	[KSPAction("#autoLOC_235508")]
	public void ToggleAction(KSPActionParam param)
	{
		if (param.type != 0 && (param.type != KSPActionType.Toggle || generatorIsActive))
		{
			Shutdown();
		}
		else
		{
			Activate();
		}
	}

	[KSPAction("#autoLOC_235502")]
	public void ActivateAction(KSPActionParam param)
	{
		Activate();
	}

	[KSPAction("#autoLOC_235505")]
	public void ShutdownAction(KSPActionParam param)
	{
		Shutdown();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_235502")]
	public void Activate()
	{
		if (!isAlwaysActive)
		{
			generatorIsActive = true;
			base.Events["Shutdown"].active = true;
			base.Events["Activate"].active = false;
		}
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_235505")]
	public void Shutdown()
	{
		if (!isAlwaysActive)
		{
			generatorIsActive = false;
			base.Events["Shutdown"].active = false;
			base.Events["Activate"].active = true;
		}
	}

	public override void OnStart(StartState state)
	{
		base.Events["Activate"].active = moduleIsEnabled && !isAlwaysActive && !generatorIsActive;
		base.Events["Shutdown"].active = moduleIsEnabled && !isAlwaysActive && generatorIsActive;
		base.Actions["ToggleAction"].active = moduleIsEnabled && !isAlwaysActive;
		base.Actions["ActivateAction"].active = moduleIsEnabled && !isAlwaysActive;
		base.Actions["ShutdownAction"].active = moduleIsEnabled && !isAlwaysActive;
		base.Fields["efficiency"].guiActive = moduleIsEnabled;
		base.Fields["displayStatus"].guiActive = moduleIsEnabled;
		base.Fields["efficiency"].guiName = efficiencyGUIName;
		base.Fields["displayStatus"].guiName = Localizer.Format(statusGUIName);
		BaseEvent baseEvent = base.Events["Shutdown"];
		string text2 = (base.Actions["ShutdownAction"].guiName = shutdownGUIName);
		baseEvent.guiName = text2;
		BaseEvent baseEvent2 = base.Events["Activate"];
		text2 = (base.Actions["ActivateAction"].guiName = activateGUIName);
		baseEvent2.guiName = text2;
		base.Actions["ToggleAction"].guiName = toggleGUIName;
		if (state != StartState.Editor && isAlwaysActive)
		{
			generatorIsActive = moduleIsEnabled;
		}
	}

	public override string GetInfo()
	{
		string text = "";
		text = ((!isAlwaysActive) ? (text + Localizer.Format("#autoLOC_235617")) : (text + Localizer.Format("#autoLOC_235616")));
		text += resHandler.PrintModuleResources();
		if (!moduleIsEnabled)
		{
			text += Localizer.Format("#autoLOC_235622");
		}
		return text;
	}

	public void FixedUpdate()
	{
		if (generatorIsActive && moduleIsEnabled)
		{
			inputRecieved = 1.0;
			lackingResources = false;
			efficiency = 1f;
			if (status != "Nominal")
			{
				displayStatus = cacheAutoLOC_219034;
				status = "Nominal";
			}
			if (isThrottleControlled)
			{
				if (base.part.isControllable)
				{
					throttle = base.vessel.ctrlState.mainThrottle;
				}
			}
			else
			{
				throttle = 1f;
			}
			int count = resHandler.inputResources.Count;
			if (count > 0)
			{
				inputRecieved = resHandler.UpdateModuleResourceInputs(ref status, throttle, resourceThreshold, returnOnFirstLack: false, average: true, stringOps: true);
				if (status == "Nominal")
				{
					displayStatus = cacheAutoLOC_219034;
				}
				else
				{
					displayStatus = cacheAutoLOC_220477;
				}
				lackingResources = false;
				int index = count;
				while (index-- > 0)
				{
					currentResource = resHandler.inputResources[index];
					if (!currentResource.available)
					{
						lackingResources = true;
						break;
					}
				}
				if (requiresAllInputs && lackingResources)
				{
					efficiency = 0f;
				}
				else if (isThrottleControlled)
				{
					efficiency = (float)(inputRecieved * (double)throttle);
				}
				else
				{
					efficiency = (float)inputRecieved;
				}
			}
			else if (isThrottleControlled)
			{
				efficiency = throttle;
			}
			efficiency = ApplyEfficiencyAdjustments(efficiency);
			resHandler.UpdateModuleResourceOutputs(efficiency);
		}
		else
		{
			efficiency = 0f;
			if (status != "Off")
			{
				displayStatus = cacheAutoLOC_220477;
				status = "Off";
			}
		}
	}

	public string GetContractObjectiveType()
	{
		return "Generator";
	}

	public bool CheckContractObjectiveValidity()
	{
		return !isGroundFixture;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003042");
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterGeneratorBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterGeneratorBase item = adjuster as AdjusterGeneratorBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyEfficiencyAdjustments(float efficiency)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			efficiency = adjusterCache[i].ApplyEfficiencyAdjustment(efficiency);
		}
		return efficiency;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_219034 = Localizer.Format("#autoLOC_219034");
		cacheAutoLOC_220477 = Localizer.Format("#autoLOC_220477");
	}
}
