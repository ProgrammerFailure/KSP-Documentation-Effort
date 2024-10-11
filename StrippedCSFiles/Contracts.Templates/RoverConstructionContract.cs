using System.Runtime.CompilerServices;
using FinePrint.Utilities;

namespace Contracts.Templates;

public class RoverConstructionContract : Contract, IUpdateWaypoints
{
	protected CelestialBody targetBody;

	protected double craftStartLatitude;

	protected double craftStartLongitude;

	protected double craftEndLatitude;

	protected double craftEndLongitude;

	protected uint roverVesselId;

	protected PreBuiltCraftDefinition roverCraftDefinition;

	protected bool waitingForVslSpawn;

	protected string[] VesselNameStrings;

	public CelestialBody TargetBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CraftStartLatitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CraftStartLongitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CraftEndLatitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double CraftEndLongitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint RoverVesselId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RoverConstructionContract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateWaypoint(float minRadius, float maxRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeDeclined()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetSynopsys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string MessageCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string CreateVesselName(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnContractPreBuiltVesselSpawned(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDestroy(Vessel vsl)
	{
		throw null;
	}
}
