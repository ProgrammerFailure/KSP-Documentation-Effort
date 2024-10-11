using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class AdjusterActiveRadiatorMaxEnergyTransfer : AdjusterActiveRadiatorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100213")]
	public float radiatorMaxEnergyTransferMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterActiveRadiatorMaxEnergyTransfer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterActiveRadiatorMaxEnergyTransfer(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double ApplyMaxEnergyTransferAdjustment(double maxEnergyTransfer)
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
