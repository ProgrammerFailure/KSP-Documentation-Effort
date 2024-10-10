using System;
using System.Reflection;

namespace Expansions.Missions;

[Serializable]
public class MissionParamsGeneral : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_8005036", toolTip = "#autoLOC_8005037")]
	public bool enableFunding;

	[GameParameters.CustomFloatParameterUI("#autoLOC_8005038", stepCount = 100, maxValue = 1000000f, minValue = 0f, addTextField = true, toolTip = "#autoLOC_8005039")]
	public float startingFunds = 100000f;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer = "";

	[GameParameters.CustomParameterUI("#autoLOC_8005040", toolTip = "#autoLOC_8005041")]
	public bool enableKerbalLevels;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005042", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005043")]
	public int kerbalLevelPilot = 4;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005044", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005045")]
	public int kerbalLevelScientist = 4;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005046", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005047")]
	public int kerbalLevelEngineer = 4;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer2 = "";

	[GameParameters.CustomParameterUI("#autoLOC_8002105", toolTip = "#autoLOC_8002106")]
	public bool preventVesselRecovery;

	public override string Title => "#autoLOC_8005035";

	public override string DisplaySection => "#autoLOC_8005013";

	public override string Section => "Mission";

	public override int SectionOrder => 0;

	public override GameParameters.GameMode GameMode => GameParameters.GameMode.MISSION;

	public override bool HasPresets => true;

	public override bool Interactible(MemberInfo member, GameParameters parameters)
	{
		if (!(member.Name == "kerbalLevelPilot") && !(member.Name == "kerbalLevelScientist") && !(member.Name == "kerbalLevelEngineer"))
		{
			if (member.Name == "startingFunds")
			{
				return enableFunding;
			}
			return base.Interactible(member, parameters);
		}
		return enableKerbalLevels;
	}

	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		switch (preset)
		{
		case GameParameters.Preset.Easy:
			enableFunding = false;
			startingFunds = 250000f;
			enableKerbalLevels = false;
			kerbalLevelPilot = 5;
			kerbalLevelScientist = 5;
			kerbalLevelEngineer = 5;
			preventVesselRecovery = false;
			break;
		case GameParameters.Preset.Normal:
			enableFunding = false;
			startingFunds = 100000f;
			enableKerbalLevels = false;
			kerbalLevelPilot = 4;
			kerbalLevelScientist = 4;
			kerbalLevelEngineer = 4;
			preventVesselRecovery = false;
			break;
		case GameParameters.Preset.Moderate:
			enableFunding = true;
			startingFunds = 25000f;
			enableKerbalLevels = true;
			kerbalLevelPilot = 2;
			kerbalLevelScientist = 2;
			kerbalLevelEngineer = 2;
			preventVesselRecovery = false;
			break;
		case GameParameters.Preset.Hard:
			enableFunding = true;
			startingFunds = 10000f;
			enableKerbalLevels = true;
			kerbalLevelPilot = 1;
			kerbalLevelScientist = 1;
			kerbalLevelEngineer = 1;
			preventVesselRecovery = true;
			break;
		}
	}
}
