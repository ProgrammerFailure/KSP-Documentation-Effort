using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Flow;

namespace Expansions.Missions;

public class MissionFlow : IConfigNode
{
	internal Dictionary<MENode, MENodePathInfo> NodePaths;

	internal Dictionary<MENode, MENodePathInfo> NodeReversePaths;

	internal MEFlowBlock missionBlock;

	private Mission mission;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionFlow(Mission mission)
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
