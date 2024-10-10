using System;

namespace Expansions.Missions;

[Serializable]
public class MissionParamsFacilities : GameParameters.CustomParameterNode
{
	[GameParameters.CustomIntParameterUI("#autoLOC_8005015", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelAdmin = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005016", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelAC = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005017", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelLaunchpad = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005018", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelMC = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005019", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelRD = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005020", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelRunway = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005021", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelSPH = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005022", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelTS = 3;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005023", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelVAB = 3;

	public override string Title => "#autoLOC_8005012";

	public override string DisplaySection => "#autoLOC_8005013";

	public override string Section => "Mission";

	public override int SectionOrder => 1;

	public override GameParameters.GameMode GameMode => GameParameters.GameMode.MISSION;

	public override bool HasPresets => true;

	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		switch (preset)
		{
		case GameParameters.Preset.Easy:
			facilityLevelAdmin = 3;
			facilityLevelAC = 3;
			facilityLevelLaunchpad = 3;
			facilityLevelMC = 3;
			facilityLevelRD = 3;
			facilityLevelRunway = 3;
			facilityLevelSPH = 3;
			facilityLevelTS = 3;
			facilityLevelVAB = 3;
			break;
		case GameParameters.Preset.Normal:
			facilityLevelAdmin = 3;
			facilityLevelAC = 3;
			facilityLevelLaunchpad = 3;
			facilityLevelMC = 3;
			facilityLevelRD = 3;
			facilityLevelRunway = 3;
			facilityLevelSPH = 3;
			facilityLevelTS = 3;
			facilityLevelVAB = 3;
			break;
		case GameParameters.Preset.Moderate:
			facilityLevelAdmin = 2;
			facilityLevelAC = 2;
			facilityLevelLaunchpad = 2;
			facilityLevelMC = 2;
			facilityLevelRD = 2;
			facilityLevelRunway = 2;
			facilityLevelSPH = 2;
			facilityLevelTS = 2;
			facilityLevelVAB = 2;
			break;
		case GameParameters.Preset.Hard:
			facilityLevelAdmin = 1;
			facilityLevelAC = 1;
			facilityLevelLaunchpad = 1;
			facilityLevelMC = 1;
			facilityLevelRD = 1;
			facilityLevelRunway = 1;
			facilityLevelSPH = 1;
			facilityLevelTS = 1;
			facilityLevelVAB = 1;
			break;
		}
	}
}
