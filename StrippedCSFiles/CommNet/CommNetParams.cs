using System.Reflection;
using System.Runtime.CompilerServices;

namespace CommNet;

public class CommNetParams : GameParameters.CustomParameterNode
{
	[GameParameters.CustomParameterUI("#autoLOC_117018", toolTip = "#autoLoc_6002200")]
	public bool requireSignalForControl;

	[GameParameters.CustomParameterUI("#autoLOC_117021", toolTip = "#autoLoc_6002201")]
	public bool plasmaBlackout;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117024", stepCount = 100, logBase = 10f, displayFormat = "F2", maxValue = 100f, minValue = 0.1f, toolTip = "#autoLOC_117025")]
	public float rangeModifier;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117028", stepCount = 100, logBase = 10f, displayFormat = "F2", maxValue = 100f, minValue = 0f, toolTip = "#autoLOC_117029")]
	public float DSNModifier;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117032", stepCount = 23, displayFormat = "F2", maxValue = 1.1f, minValue = 0f, toolTip = "#autoLOC_117033")]
	public float occlusionMultiplierVac;

	[GameParameters.CustomFloatParameterUI("#autoLOC_117036", stepCount = 23, displayFormat = "F2", maxValue = 1.1f, minValue = 0f, toolTip = "#autoLOC_117037")]
	public float occlusionMultiplierAtm;

	[GameParameters.CustomParameterUI("#autoLOC_117040", toolTip = "#autoLoc_6002202")]
	public bool enableGroundStations;

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
	public CommNetParams()
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
