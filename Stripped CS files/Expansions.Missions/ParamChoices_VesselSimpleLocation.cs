using System.ComponentModel;
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

	public string GetNodeBodyParameterString()
	{
		return locationChoice switch
		{
			Choices.orbit => orbit.GetNodeBodyParameterString(), 
			Choices.landed => landed.GetNodeBodyParameterString(), 
			_ => "", 
		};
	}

	public void Load(ConfigNode node)
	{
		node.TryGetEnum("locationChoice", ref locationChoice, Choices.orbit);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("LANDEDDATA", ref node2))
		{
			landed.Load(node2);
		}
		if (node.TryGetNode("ORBITDATA", ref node2))
		{
			orbit.Load(node2);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("locationChoice", locationChoice);
		if (landed != null)
		{
			landed.Save(node.AddNode("LANDEDDATA"));
		}
		if (orbit != null)
		{
			orbit.Save(node.AddNode("ORBITDATA"));
		}
	}
}
