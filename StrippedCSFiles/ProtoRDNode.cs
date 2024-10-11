using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ProtoRDNode
{
	public List<ProtoRDNode> parents;

	public List<ProtoRDNode> children;

	public bool AnyParentToUnlock;

	public ProtoTechNode tech;

	public string iconRef;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoRDNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoRDNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadLinks(ConfigNode node, List<ProtoRDNode> rdNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoRDNode FindNodeByID(string techID, List<ProtoRDNode> nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
