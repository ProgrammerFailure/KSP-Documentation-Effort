using System.Reflection;
using ns9;

namespace CommNet;

public class CommNetParams : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_117018", toolTip = "#autoLoc_6002200")]
	public bool requireSignalForControl;

	[GameParameters.CustomParameterUI("#autoLOC_117021", toolTip = "#autoLoc_6002201")]
	public bool plasmaBlackout;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117024", stepCount = 100, logBase = 10f, displayFormat = "F2", maxValue = 100f, minValue = 0.1f, toolTip = "#autoLOC_117025")]
	public float rangeModifier = 1f;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117028", stepCount = 100, logBase = 10f, displayFormat = "F2", maxValue = 100f, minValue = 0f, toolTip = "#autoLOC_117029")]
	public float DSNModifier = 1f;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117032", stepCount = 23, displayFormat = "F2", maxValue = 1.1f, minValue = 0f, toolTip = "#autoLOC_117033")]
	public float occlusionMultiplierVac = 0.9f;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117036", stepCount = 23, displayFormat = "F2", maxValue = 1.1f, minValue = 0f, toolTip = "#autoLOC_117037")]
	public float occlusionMultiplierAtm = 0.75f;

	[GameParameters.CustomParameterUI("#autoLOC_117040", toolTip = "#autoLoc_6002202")]
	public bool enableGroundStations = true;

	public override string Title => Localizer.Format("#autoLOC_117012");

	public override string DisplaySection => Localizer.Format("#autoLoc_6002170");

	public override string Section => "Advanced";

	public override int SectionOrder => 1;

	public override GameParameters.GameMode GameMode => GameParameters.GameMode.const_6;

	public override bool HasPresets => true;

	public override bool Interactible(MemberInfo member, GameParameters parameters)
	{
		return parameters.Difficulty.EnableCommNet;
	}

	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		switch (preset)
		{
		case GameParameters.Preset.Easy:
			requireSignalForControl = false;
			rangeModifier = 1.5f;
			occlusionMultiplierVac = 0f;
			occlusionMultiplierAtm = 0f;
			plasmaBlackout = false;
			break;
		case GameParameters.Preset.Moderate:
			rangeModifier = 0.8f;
			occlusionMultiplierVac = 1f;
			occlusionMultiplierAtm = 0.85f;
			plasmaBlackout = false;
			break;
		case GameParameters.Preset.Hard:
			rangeModifier = 0.65f;
			occlusionMultiplierVac = 1f;
			occlusionMultiplierAtm = 1f;
			plasmaBlackout = false;
			break;
		case GameParameters.Preset.Normal:
			break;
		}
	}
}
