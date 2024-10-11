using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProceduralFairings;

[Serializable]
public class FairingPanelFlags
{
	public List<uint> attachedFlagsPartIds;

	public int panelIndex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingPanelFlags()
	{
		throw null;
	}
}
