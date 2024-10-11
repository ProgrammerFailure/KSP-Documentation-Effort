using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
public class MissionCraft
{
	public string craftFile;

	public string craftFolder;

	public string facility;

	public bool mappedItem;

	public string mappedVesselName;

	public uint craftPersistentId;

	public uint rootPartPersistentId;

	private ConfigNode _configNodeCache;

	public ConfigNode CraftNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string VesselName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint persistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionCraft(string craftFolder, string craftFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPartNameInVessel(uint persistentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint GetPartPersistentId(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetPartName(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void checkPersistentIds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}
}
