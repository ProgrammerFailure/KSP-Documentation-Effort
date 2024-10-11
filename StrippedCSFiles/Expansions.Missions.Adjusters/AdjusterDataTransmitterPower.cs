using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Adjusters;

public class AdjusterDataTransmitterPower : AdjusterDataTransmitterBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100219")]
	public float transmitterPowerMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDataTransmitterPower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDataTransmitterPower(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PowerValueChanged(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePowerValueDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double ApplyPowerAdjustment(double power)
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
