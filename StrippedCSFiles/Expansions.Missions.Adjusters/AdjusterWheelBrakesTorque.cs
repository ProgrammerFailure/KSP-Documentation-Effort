using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class AdjusterWheelBrakesTorque : AdjusterWheelBrakesBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100263")]
	public float brakeTorqueMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterWheelBrakesTorque()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterWheelBrakesTorque(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float ApplyTorqueAdjustment(float torque)
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
