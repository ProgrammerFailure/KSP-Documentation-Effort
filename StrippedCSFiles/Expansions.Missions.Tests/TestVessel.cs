using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

public class TestVessel : TestModule
{
	[MEGUI_VesselSelect(order = 0, onControlCreated = "vesselSelectorCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	protected bool useActiveVessel;

	protected Vessel vessel;

	private List<uint> missingVessels;

	private uint oldRootPart;

	private string bodyScore;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselDocking(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void vesselSelectorCreated(MEGUIParameter parameter)
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
