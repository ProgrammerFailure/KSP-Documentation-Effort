using System;

[Serializable]
public struct DynamicNodeSet
{
	public string DisplayText;

	public string MeshTransform;

	public string NodePrefix;

	public int SetCount;

	public int Symmetry;

	public void Load(ConfigNode node)
	{
		DisplayText = "";
		node.TryGetValue("DisplayText", ref DisplayText);
		MeshTransform = "";
		node.TryGetValue("MeshTransform", ref MeshTransform);
		NodePrefix = "";
		node.TryGetValue("NodePrefix", ref NodePrefix);
		SetCount = 0;
		node.TryGetValue("SetCount", ref SetCount);
		Symmetry = 0;
		node.TryGetValue("Symmetry", ref Symmetry);
	}
}
