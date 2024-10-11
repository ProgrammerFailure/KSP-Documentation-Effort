using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public class FailureDataTransmitterBroken : AdjusterDataTransmitterBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDataTransmitterBroken()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureDataTransmitterBroken(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsDataTransmitterBroken()
	{
		throw null;
	}
}
