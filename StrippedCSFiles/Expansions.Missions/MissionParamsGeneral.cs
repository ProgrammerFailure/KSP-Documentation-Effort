using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
internal class MissionParamsGeneral : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_8005036", toolTip = "#autoLOC_8005037")]
	public bool enableFunding;

	[GameParameters.CustomFloatParameterUI("#autoLOC_8005038", stepCount = 100, maxValue = 1000000f, minValue = 0f, addTextField = true, toolTip = "#autoLOC_8005039")]
	public float startingFunds;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer;

	[GameParameters.CustomParameterUI("#autoLOC_8005040", toolTip = "#autoLOC_8005041")]
	public bool enableKerbalLevels;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005042", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005043")]
	public int kerbalLevelPilot;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005044", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005045")]
	public int kerbalLevelScientist;

	[GameParameters.CustomIntParameterUI("#autoLOC_8005046", maxValue = 5, minValue = 0, toolTip = "#autoLOC_8005047")]
	public int kerbalLevelEngineer;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer2;

	[GameParameters.CustomParameterUI("#autoLOC_8002105", toolTip = "#autoLOC_8002106")]
	public bool preventVesselRecovery;

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
	public MissionParamsGeneral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Interactible(MemberInfo member, GameParameters parameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetDifficultyPreset(GameParameters.Preset preset)
	{
		throw null;
	}
}
