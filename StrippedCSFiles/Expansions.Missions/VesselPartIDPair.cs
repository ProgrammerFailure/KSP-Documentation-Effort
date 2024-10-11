using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

[Serializable]
public class VesselPartIDPair : IConfigNode
{
	[MEGUI_VesselSelect(addDefaultOption = false, gapDisplay = true, guiName = "#autoLOC_8000001")]
	public uint VesselID;

	public uint partID;

	[MEGUI_Label(simpleLabel = false, guiName = "#autoLOC_8000012")]
	public string partName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselPartIDPair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ValidatePartAgainstCraft(MENode node, MissionEditorValidator validator)
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
}
