using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class AdjusterGeneratorEfficiency : AdjusterGeneratorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100231")]
	public float generatorOutputMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterGeneratorEfficiency()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterGeneratorEfficiency(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float ApplyEfficiencyAdjustment(float efficiency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
