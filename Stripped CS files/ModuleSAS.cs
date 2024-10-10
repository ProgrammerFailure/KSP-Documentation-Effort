using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using Experience.Effects;
using ns9;
using UnityEngine;

public class ModuleSAS : PartModule
{
	[KSPField]
	public int SASServiceLevel;

	public int targetSASServiceLevel;

	[KSPField]
	public int CommandModuleIndex = -1;

	[KSPField]
	public bool RequireCrew;

	[KSPField]
	public bool standalone;

	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001073", scene = UI_Scene.All, enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001371")]
	public bool standaloneToggle = true;

	[SerializeField]
	public bool standaloneOperational;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001372")]
	public string standaloneStateText = "";

	public ModuleCommand cmdModule;

	public List<AdjusterSASBase> adjusterCache = new List<AdjusterSASBase>();

	public static string cacheAutoLOC_257237;

	public static string cacheAutoLOC_6001373;

	public static string cacheAutoLOC_6001374;

	public static string cacheAutoLOC_218888;

	public static string cacheAutoLOC_6003057;

	public override void OnStart(StartState state)
	{
		targetSASServiceLevel = SASServiceLevel;
		if (base.vessel != null)
		{
			base.part.PartValues.AutopilotSkill.Add(GetValue);
			base.part.PartValues.AutopilotSASSkill.Add(GetValue);
		}
		if (!standalone)
		{
			if (CommandModuleIndex != -1)
			{
				cmdModule = base.part.Modules[CommandModuleIndex] as ModuleCommand;
			}
			else
			{
				cmdModule = base.part.GetComponent<ModuleCommand>();
			}
			if (cmdModule == null)
			{
				Debug.LogWarning("[ModuleSAS]: No ModuleCommand found in " + base.part.name + ". SAS services won't be available.");
			}
			base.Fields["standaloneToggle"].guiActive = false;
			base.Fields["standaloneStateText"].guiActive = false;
			base.Fields["standaloneToggle"].guiActiveEditor = false;
			base.Fields["standaloneStateText"].guiActiveEditor = false;
		}
		else
		{
			BaseField baseField = base.Fields["standaloneToggle"];
			bool guiActive = (base.Fields["standaloneToggle"].guiActiveEditor = moduleIsEnabled);
			baseField.guiActive = guiActive;
			base.Fields["standaloneStateText"].guiActive = moduleIsEnabled;
		}
	}

	public void OnDestroy()
	{
		if (base.vessel != null)
		{
			base.part.PartValues.AutopilotSkill.Remove(GetValue);
			base.part.PartValues.AutopilotSASSkill.Remove(GetValue);
		}
	}

	public int GetValue()
	{
		if (cmdModule != null && moduleIsEnabled)
		{
			if (RequireCrew ? (cmdModule.ModuleState == ModuleCommand.ModuleControlState.Nominal) : (cmdModule.ModuleState != ModuleCommand.ModuleControlState.NotEnoughResources))
			{
				return SASServiceLevel;
			}
			return -1;
		}
		if (standalone)
		{
			if (!standaloneOperational)
			{
				return -1;
			}
			return SASServiceLevel;
		}
		return -1;
	}

	public void FixedUpdate()
	{
		if (moduleIsEnabled && HighLogic.LoadedSceneIsFlight && base.part.started)
		{
			SASServiceLevel = ApplySASServiceLevelAdjustments(targetSASServiceLevel);
		}
		if (!moduleIsEnabled || !standalone || !HighLogic.LoadedSceneIsFlight || !base.part.started)
		{
			return;
		}
		if (standaloneToggle)
		{
			if (base.vessel.Autopilot.Enabled)
			{
				standaloneStateText = cacheAutoLOC_257237;
				standaloneOperational = resHandler.UpdateModuleResourceInputs(ref standaloneStateText, 1.0, 0.9, returnOnFirstLack: true);
			}
			else
			{
				standaloneOperational = true;
				standaloneStateText = cacheAutoLOC_6001373;
			}
		}
		else
		{
			standaloneOperational = false;
			standaloneStateText = cacheAutoLOC_6001374;
		}
	}

	public override string GetInfo()
	{
		int sASServiceLevel = SASServiceLevel;
		if (sASServiceLevel == 0)
		{
			return AutopilotSkill.SkillsReadable[0];
		}
		string text = "";
		for (int i = 0; i < Mathf.Min(sASServiceLevel + 1, AutopilotSkill.SkillsReadable.Length); i++)
		{
			if (i != 0)
			{
				text += "\n";
			}
			text += AutopilotSkill.SkillsReadable[i];
		}
		if (standalone)
		{
			text += resHandler.PrintModuleResources();
		}
		if (!moduleIsEnabled)
		{
			text += cacheAutoLOC_218888;
		}
		return text;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterSASBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterSASBase item = adjuster as AdjusterSASBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public int ApplySASServiceLevelAdjustments(int serviceLevel)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			serviceLevel = adjusterCache[i].ApplySASServiceLevelAdjustment(serviceLevel);
		}
		return serviceLevel;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003057;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_257237 = Localizer.Format("#autoLOC_257237");
		cacheAutoLOC_6001373 = Localizer.Format("#autoLOC_6001373");
		cacheAutoLOC_6001374 = Localizer.Format("#autoLOC_6001374");
		cacheAutoLOC_218888 = Localizer.Format("#autoLOC_218888");
		cacheAutoLOC_6003057 = Localizer.Format("#autoLoc_6003057");
	}
}
