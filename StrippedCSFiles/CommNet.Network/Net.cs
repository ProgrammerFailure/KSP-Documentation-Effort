using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet.Network;

public abstract class Net<_Net, _Data, _Link, _Path> where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public Action OnNetworkPreUpdate;

	public Action OnNetworkPostUpdate;

	protected List<_Data> nodes;

	protected List<_Link> links;

	protected List<Occluder> occluders;

	private int _pathingID;

	protected Queue<_Data> candidates;

	protected Dictionary<_Data, _Link>.KeyCollection.Enumerator nodeEnum;

	protected Dictionary<_Data, _Link>.Enumerator nodeLinkEnum;

	protected KeyValuePair<_Data, _Link> nodeLink;

	public _Data this[int i]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<_Link> Links
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int OccludersCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected int pathingID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Net()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(_Data conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual _Data Add(_Data conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool Remove(_Data conn)
	{
		throw null;
	}

	protected abstract bool SetNodeConnection(_Data connA, _Data connB);

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual _Link Connect(_Data a, _Data b, double distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Disconnect(_Data a, _Data b, bool removeFromA = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(Occluder conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Occluder Add(Occluder conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool Remove(Occluder conn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void IncrementPathingID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Rebuild()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PreUpdateNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateOccluders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateNetwork()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PostUpdateNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool FindPath(_Data start, _Path path, _Data end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual _Data FindClosestWhere(_Data start, _Path path, Func<_Data, _Data, bool> where)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateShortestPathTree(_Data start, _Data end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateShortestPath(_Data node, _Data neighbor, _Link link, double bestCost, _Data startNode, _Data endNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual _Data UpdateShortestWhere(_Data a, _Data b, _Link link, double bestDistance, _Data startNode, Func<_Data, _Data, bool> whereClause)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetLinkPoints(List<Vector3> discreteLines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
