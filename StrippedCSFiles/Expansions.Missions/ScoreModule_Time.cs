using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class ScoreModule_Time : ScoreModule
{
	[MEGUI_ScoreRangeList(ContentType = MEGUI_ScoreRangeList.RangeContentType.Time, minLabel = "", maxLabel = "", valueLabel = "", guiName = "#autoLOC_8004155")]
	public List<ScoreRange> scoreRanges;

	private KSPUtil.DefaultDateTimeFormatter dateFormatter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule_Time()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule_Time(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float AwardScore(float currentScore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ScoreDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<string> GetDefaultPinnedParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}
}