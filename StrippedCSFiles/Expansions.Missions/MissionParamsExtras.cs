using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
internal class MissionParamsExtras : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_8005025", toolTip = "#autoLOC_8005030")]
	public bool facilityOpenAC;

	[GameParameters.CustomParameterUI("#autoLOC_8005026", toolTip = "#autoLOC_8005031")]
	public bool astronautHiresAreFree;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer;

	[GameParameters.CustomParameterUI("#autoLOC_8005027", toolTip = "#autoLOC_8005032")]
	public bool facilityOpenEditor;

	[GameParameters.CustomParameterUI("#autoLOC_8005028", toolTip = "#autoLOC_8005033")]
	public bool launchSitesOpen;

	[GameParameters.CustomStringParameterUI("")]
	public string spacer2;

	[GameParameters.CustomParameterUI("#autoLOC_8005029", toolTip = "#autoLOC_8005034")]
	public bool cheatsEnabled;

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
	public MissionParamsExtras()
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
