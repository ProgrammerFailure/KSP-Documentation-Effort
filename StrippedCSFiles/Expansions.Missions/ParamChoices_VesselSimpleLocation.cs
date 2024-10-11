using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class ParamChoices_VesselSimpleLocation : IConfigNode
{
	public enum Choices
	{
		[Description("#autoLOC_8000201")]
		landed,
		[Description("#autoLOC_8000202")]
		orbit
	}

	[MEGUI_ParameterSwitchCompound_KeyField(gapDisplay = false, guiName = "#autoLOC_8000027")]
	public Choices locationChoice;

	[MEGUI_VesselGroundLocation(DisableRotationY = true, DisableRotationX = true, AllowWaterSurfacePlacement = true, gapDisplay = true, guiName = "#autoLOC_8000179")]
	public VesselGroundLocation landed;

	[MEGUI_CelestialBody_Orbit(gapDisplay = true, guiName = "#autoLOC_8000226")]
	public MissionOrbit orbit;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ParamChoices_VesselSimpleLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
