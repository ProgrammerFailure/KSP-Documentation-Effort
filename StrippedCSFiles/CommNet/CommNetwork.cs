using System;
using System.Runtime.CompilerServices;
using CommNet.Network;

namespace CommNet;

public class CommNetwork : Net<CommNetwork, CommNode, CommLink, CommPath>
{
	protected bool isDirty;

	public bool IsDirty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNetwork()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Rebuild()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool FindHome(CommNode from, CommPath path = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool FindClosestControlSource(CommNode from, CommPath path = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool FindPath(CommNode start, CommPath path, CommNode end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override CommNode FindClosestWhere(CommNode start, CommPath path, Func<CommNode, CommNode, bool> where)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override CommNode Add(CommNode conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Remove(CommNode conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool TryConnect(CommNode a, CommNode b, double distance, bool aCanRelay, bool bCanRelay, bool bothRelay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool SetNodeConnection(CommNode a, CommNode b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool TestOcclusion(Vector3d aPos, Occluder a, Vector3d bPos, Occluder b, double distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool IsControlSource(CommNode start, CommNode a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool IsHome(CommNode start, CommNode a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateShortestPathTree(CommNode start, CommNode end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateShortestPath(CommNode a, CommNode b, CommLink link, double bestCost, CommNode startNode, CommNode endNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override CommNode UpdateShortestWhere(CommNode a, CommNode b, CommLink link, double bestCost, CommNode startNode, Func<CommNode, CommNode, bool> whereClause)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string CreateDebug()
	{
		throw null;
	}
}
