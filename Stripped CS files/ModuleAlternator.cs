using System.Collections.Generic;
using Expansions;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;
using ns9;

public class ModuleAlternator : PartModule
{
	public IEngineStatus engine;

	public ModuleRoboticServoRotor rotor;

	[KSPField]
	public bool roboticRotorMode;

	[KSPField]
	public double singleTickThreshold = 0.0001;

	[KSPField]
	public bool preferMultiMode;

	[KSPField]
	public int engineIndex;

	[KSPField]
	public string engineName = "";

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001419", guiUnits = "#autoLOC_7001414")]
	public float outputRate;

	[KSPField]
	public string outputName = "#autoLOC_6001419";

	[KSPField]
	public string outputUnits = "#autoLOC_7001414";

	[KSPField]
	public string outputFormat = "F2";

	public BaseField fld;

	public List<AdjusterAlternatorBase> adjusterCache = new List<AdjusterAlternatorBase>();

	public override void OnAwake()
	{
		fld = base.Fields["outputRate"];
		resHandler.moduleResourceBasedPrimaryIsInput = false;
	}

	public override void OnLoad(ConfigNode node)
	{
		fld.guiName = outputName;
		fld.guiUnits = Localizer.Format(outputUnits);
		fld.guiFormat = outputFormat;
	}

	public override void OnStart(StartState state)
	{
		fld.guiName = outputName;
		fld.guiUnits = Localizer.Format(outputUnits);
		fld.guiFormat = outputFormat;
		fld.guiActive = moduleIsEnabled;
		if (roboticRotorMode)
		{
			if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
			{
				roboticRotorMode = false;
				return;
			}
			rotor = base.part.FindModuleImplementing<ModuleRoboticServoRotor>();
			if (rotor == null)
			{
				roboticRotorMode = false;
			}
			else
			{
				if (rotor.resHandler == null)
				{
					return;
				}
				for (int i = 0; i < rotor.resHandler.inputResources.Count; i++)
				{
					if (rotor.resHandler.inputResources[i].resourceDef != null && rotor.resHandler.inputResources[i].resourceDef.name == "ElectricCharge")
					{
						roboticRotorMode = false;
					}
				}
			}
		}
		else
		{
			engine = base.part.Modules.FindEngineNearby(engineName, engineIndex, preferMultiMode);
		}
	}

	public override string GetInfo()
	{
		string text = "";
		if (resHandler.outputResources.Count > 0)
		{
			text += PartModuleUtil.PrintResourceRequirements(Localizer.Format("#autoLOC_234166"), resHandler.outputResources.ToArray());
		}
		if (!moduleIsEnabled)
		{
			text += Localizer.Format("#autoLOC_234170");
		}
		return text;
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !moduleIsEnabled)
		{
			return;
		}
		float num = 0f;
		if (roboticRotorMode)
		{
			if (rotor != null)
			{
				num = ApplyOutputAdjustments(rotor.normalizedOutput);
			}
		}
		else if (engine != null)
		{
			num = ApplyOutputAdjustments(engine.normalizedOutput);
		}
		if (num > 0f)
		{
			outputRate = (float)resHandler.UpdateModuleResourceOutputs(num, singleTickThreshold);
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterAlternatorBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterAlternatorBase item = adjuster as AdjusterAlternatorBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyOutputAdjustments(float output)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			output = adjusterCache[i].ApplyOutputAdjustment(output);
		}
		return output;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003026");
	}
}
