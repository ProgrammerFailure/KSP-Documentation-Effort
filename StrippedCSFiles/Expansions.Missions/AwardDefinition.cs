using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class AwardDefinition : IConfigNode
{
	public string id;

	public string name;

	public string displayName;

	public string description;

	public Sprite icon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardDefinition()
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
