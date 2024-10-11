using System;
using System.Runtime.CompilerServices;

[Serializable]
public struct DynamicNodeSet
{
	public string DisplayText;

	public string MeshTransform;

	public string NodePrefix;

	public int SetCount;

	public int Symmetry;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
