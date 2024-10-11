using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
public class MissionPack : IConfigNode
{
	[Persistent]
	public string name;

	[Persistent]
	public string displayName;

	[Persistent]
	public string description;

	[Persistent]
	public int order;

	[Persistent]
	public string color;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int CompareOrder(MissionPack a, MissionPack b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int CompareDisplayName(MissionPack a, MissionPack b)
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
