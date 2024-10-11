using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

[Serializable]
public struct DisplayParameter
{
	public string module;

	public string parameter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
