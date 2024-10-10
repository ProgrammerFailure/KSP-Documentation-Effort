using System;

namespace Expansions.Missions.Editor;

[Serializable]
public struct DisplayParameter
{
	public string module;

	public string parameter;

	public void Load(ConfigNode node)
	{
		node.TryGetValue("module", ref module);
		node.TryGetValue("parameter", ref parameter);
	}
}
