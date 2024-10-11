using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Actions;

public class ActionVessel : ActionModule
{
	[MEGUI_VesselSelect(order = 0, onControlCreated = "vesselSelectorCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	protected bool useActiveVessel;

	private Vessel _vessel;

	private uint oldRootPart;

	public Vessel vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionVessel()
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
	public override void OnVesselsDocking(uint oldId, uint newId)
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
