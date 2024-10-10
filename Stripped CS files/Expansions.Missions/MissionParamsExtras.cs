using System;
using System.Reflection;

namespace Expansions.Missions;

[Serializable]
public class MissionParamsExtras : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_8005025", toolTip = "#autoLOC_8005030")]
	public bool facilityOpenAC;

	[GameParameters.CustomParameterUI("#autoLOC_8005026", toolTip = "#autoLOC_8005031")]
	public bool astronautHiresAreFree = true;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer = "";

	[GameParameters.CustomParameterUI("#autoLOC_8005027", toolTip = "#autoLOC_8005032")]
	public bool facilityOpenEditor;

	[GameParameters.CustomParameterUI("#autoLOC_8005028", toolTip = "#autoLOC_8005033")]
	public bool launchSitesOpen;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer2 = "";

	[GameParameters.CustomParameterUI("#autoLOC_8005029", toolTip = "#autoLOC_8005034")]
	public bool cheatsEnabled;

	public override string Title => "#autoLOC_8005024";

	public override string DisplaySection => "#autoLOC_8005013";

	public override string Section => "Mission";

	public override int SectionOrder => 2;

	public override GameParameters.GameMode GameMode => GameParameters.GameMode.MISSION;

	public override bool HasPresets => true;

	public override bool Interactible(MemberInfo member, GameParameters parameters)
	{
		if (member.Name == "astronautHiresAreFree")
		{
			return facilityOpenAC;
		}
		if (member.Name == "launchSitesOpen")
		{
			return facilityOpenEditor;
		}
		return base.Interactible(member, parameters);
	}

	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		switch (preset)
		{
		case GameParameters.Preset.Easy:
			facilityOpenAC = false;
			astronautHiresAreFree = true;
			facilityOpenEditor = false;
			launchSitesOpen = true;
			cheatsEnabled = true;
			break;
		case GameParameters.Preset.Normal:
			facilityOpenAC = false;
			astronautHiresAreFree = true;
			facilityOpenEditor = false;
			launchSitesOpen = true;
			cheatsEnabled = false;
			break;
		case GameParameters.Preset.Moderate:
			facilityOpenAC = true;
			astronautHiresAreFree = true;
			facilityOpenEditor = false;
			launchSitesOpen = true;
			cheatsEnabled = false;
			break;
		case GameParameters.Preset.Hard:
			facilityOpenAC = true;
			astronautHiresAreFree = false;
			facilityOpenEditor = false;
			launchSitesOpen = true;
			cheatsEnabled = false;
			break;
		}
	}
}
