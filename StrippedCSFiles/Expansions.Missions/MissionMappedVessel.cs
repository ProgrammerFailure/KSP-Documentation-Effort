using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
public class MissionMappedVessel
{
	public uint partPersistentId;

	public uint mappedVesselPersistentId;

	public uint currentVesselPersistentId;

	public uint originalVesselPersistentId;

	public string partVesselName;

	public string craftFileName;

	public string situationVesselName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionMappedVessel(uint partPersistentId, uint originalVesselId, uint currentVesselId, string partVslName, string craftFileName, string situationVslName = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<MissionMappedVessel> Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode Save(ConfigNode node, List<MissionMappedVessel> list)
	{
		throw null;
	}
}
