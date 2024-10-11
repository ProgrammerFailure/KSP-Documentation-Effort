using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
internal class MissionParamsFacilities : GameParameters.CustomParameterNode
{
	[GameParameters.CustomIntParameterUI("#autoLOC_8005015", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelAdmin;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005016", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelAC;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005017", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelLaunchpad;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005018", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelMC;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005019", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelRD;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005020", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelRunway;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005021", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelSPH;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005022", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelTS;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005023", maxValue = 3, minValue = 1, toolTip = "#autoLOC_8005014")]
	public int facilityLevelVAB;

	public override string Title
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string DisplaySection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string Section
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override int SectionOrder
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override GameParameters.GameMode GameMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override bool HasPresets
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionParamsFacilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		throw null;
	}
}
