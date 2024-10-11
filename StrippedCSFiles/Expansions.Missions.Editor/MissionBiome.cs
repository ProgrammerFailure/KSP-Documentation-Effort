using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

public class MissionBiome
{
	[MEGUI_Dropdown(addDefaultOption = false, order = 1, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody body;

	[MEGUI_Dropdown(addDefaultOption = false, order = 2, SetDropDownItems = "SetBiomes", gapDisplay = true, guiName = "#autoLOC_8000136")]
	public string biomeName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionBiome(CelestialBody celestialBody, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetBiomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
	{
		throw null;
	}
}
