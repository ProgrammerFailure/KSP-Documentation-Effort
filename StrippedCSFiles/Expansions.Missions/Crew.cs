using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[Serializable]
public class Crew : IConfigNode
{
	public uint partPersistentID;

	public uint vesselPersistentID;

	public List<string> crewNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Crew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string LocalizeCrewMember(int nameIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
