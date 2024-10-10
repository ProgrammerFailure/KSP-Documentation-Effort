using System;
using System.Collections.Generic;

namespace ProceduralFairings;

[Serializable]
public class FairingPanelFlags
{
	public List<uint> attachedFlagsPartIds;

	public int panelIndex;

	public FairingPanelFlags()
	{
		attachedFlagsPartIds = new List<uint>();
		panelIndex = -1;
	}
}
