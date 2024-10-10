using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommNet.Network;

public abstract class Net<_Net, _Data, _Link, _Path> where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public Action OnNetworkPreUpdate;

	public Action OnNetworkPostUpdate;

	public List<_Data> nodes = new List<_Data>();

	public List<_Link> links = new List<_Link>();

	public List<Occluder> occluders = new List<Occluder>();

	public int _pathingID = int.MinValue;

	public Queue<_Data> candidates = new Queue<_Data>();

	public Dictionary<_Data, _Link>.KeyCollection.Enumerator nodeEnum;

	public Dictionary<_Data, _Link>.Enumerator nodeLinkEnum;

	public KeyValuePair<_Data, _Link> nodeLink;

	public _Data this[int i] => nodes[i];

	public int Count => nodes.Count;

	public List<_Link> Links => links;

	public int OccludersCount => nodes.Count;

	public int pathingID => _pathingID;

	public Net()
	{
	}

	public bool Contains(_Data conn)
	{
		return nodes.Contains(conn);
	}

	public virtual _Data Add(_Data conn)
	{
		if (conn == null)
		{
			return null;
		}
		if (!Contains(conn))
		{
			conn.SetNet((_Net)this);
			nodes.Add(conn);
		}
		if (conn.occluder != null)
		{
			Add(conn.occluder);
		}
		conn.Clear();
		return conn;
	}

	public virtual bool Remove(_Data conn)
	{
		if (conn == null)
		{
			return false;
		}
		if (conn.occluder != null)
		{
			Remove(conn.occluder);
		}
		nodeLinkEnum = conn.GetEnumerator();
		while (nodeLinkEnum.MoveNext())
		{
			_Link value = nodeLinkEnum.Current.Value;
			Disconnect(conn, value.OtherEnd(conn), removeFromA: false);
		}
		conn.Clear();
		return nodes.Remove(conn);
	}

	public abstract bool SetNodeConnection(_Data connA, _Data connB);

	public virtual _Link Connect(_Data a, _Data b, double distance)
	{
		if (a.TryGetValue(b, out var value))
		{
			value.Update(distance);
		}
		else
		{
			value = new _Link();
			value.Set(a, b, distance);
			a.Add(b, value);
			b.Add(a, value);
			links.Add(value);
		}
		return value;
	}

	public virtual void Disconnect(_Data a, _Data b, bool removeFromA = true)
	{
		if (a != null && b != null && (a.TryGetValue(b, out var value) || b.TryGetValue(a, out value)))
		{
			value.OnDestroy();
			b.Remove(a);
			if (removeFromA)
			{
				a.Remove(b);
			}
			links.Remove(value);
		}
	}

	public bool Contains(Occluder conn)
	{
		return occluders.Contains(conn);
	}

	public virtual Occluder Add(Occluder conn)
	{
		if (!Contains(conn))
		{
			occluders.Add(conn);
		}
		return conn;
	}

	public virtual bool Remove(Occluder conn)
	{
		return occluders.Remove(conn);
	}

	public void IncrementPathingID()
	{
		if (_pathingID == int.MaxValue)
		{
			_pathingID = int.MinValue;
		}
		_pathingID++;
	}

	public virtual void Rebuild()
	{
		if (OnNetworkPreUpdate != null)
		{
			OnNetworkPreUpdate();
		}
		PreUpdateNodes();
		UpdateOccluders();
		UpdateNetwork();
		PostUpdateNodes();
		if (OnNetworkPostUpdate != null)
		{
			OnNetworkPostUpdate();
		}
	}

	public virtual void PreUpdateNodes()
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			nodes[i].NetworkPreUpdate();
		}
	}

	public virtual void UpdateOccluders()
	{
		int i = 0;
		for (int count = occluders.Count; i < count; i++)
		{
			occluders[i].Update();
		}
	}

	public virtual void UpdateNetwork()
	{
		int count = nodes.Count;
		int i = 0;
		for (int num = count - 1; i < num; i++)
		{
			_Data connA = nodes[i];
			for (int j = i + 1; j < count; j++)
			{
				_Data connB = this[j];
				SetNodeConnection(connA, connB);
			}
		}
	}

	public virtual void PostUpdateNodes()
	{
		int i = 0;
		for (int count = nodes.Count; i < count; i++)
		{
			nodes[i].NetworkPostUpdate();
		}
	}

	public virtual bool FindPath(_Data start, _Path path, _Data end)
	{
		if (path != null)
		{
			path.Clear();
		}
		if (end == null)
		{
			return false;
		}
		if (start == end)
		{
			return false;
		}
		CreateShortestPathTree(start, end);
		_Data val = end;
		path.Add(new _Link
		{
			a = end,
			cost = 0.0
		});
		if (val.bestLinkNode != null)
		{
			while (val != start)
			{
				val = val.bestLinkNode;
				_Link val2 = new _Link();
				val2.Set(val, null, val.bestCost);
				path.Add(val2);
			}
		}
		if (path.Count == 0)
		{
			return false;
		}
		if (!path[0].Contains(start))
		{
			path.Clear();
			return false;
		}
		path.UpdateFromPath();
		return true;
	}

	public virtual _Data FindClosestWhere(_Data start, _Path path, Func<_Data, _Data, bool> where)
	{
		if (path != null)
		{
			path.Clear();
		}
		int count = nodes.Count;
		_Data val;
		while (count-- > 0)
		{
			val = this[count];
			val.bestCost = double.MaxValue;
			val.bestLinkNode = null;
			val.isInCandidateList = false;
		}
		_Data val2 = null;
		double num = double.MaxValue;
		candidates.Enqueue(start);
		start.bestCost = 0.0;
		start.isInCandidateList = true;
		while (candidates.Count > 0)
		{
			val = candidates.Dequeue();
			val.isInCandidateList = false;
			val.pathingID = pathingID;
			nodeLinkEnum = val.GetEnumerator();
			while (nodeLinkEnum.MoveNext())
			{
				nodeLink = nodeLinkEnum.Current;
				_Data val3 = UpdateShortestWhere(val, nodeLink.Key, nodeLink.Value, val.bestCost + nodeLink.Value.cost, start, where);
				if (val3 != null && val3.bestCost < num)
				{
					val2 = val3;
					num = val3.bestCost;
				}
			}
		}
		if (val2 == null)
		{
			return null;
		}
		val = val2;
		if (path != null)
		{
			path.Clear();
			_Link val4 = new _Link();
			val4.Set(val, null, 0.0);
			path.Add(val4);
			if (val.bestLinkNode != null)
			{
				while (val != start)
				{
					val = val.bestLinkNode;
					val4 = new _Link();
					val4.Set(val, null, 0.0);
					path.Add(val4);
				}
			}
			if (!path[0].Contains(start))
			{
				path.Clear();
				return null;
			}
			path.UpdateFromPath();
			return val2;
		}
		return val2;
	}

	public virtual void CreateShortestPathTree(_Data start, _Data end)
	{
		IncrementPathingID();
		int count = nodes.Count;
		while (count-- > 0)
		{
			_Data val = this[count];
			val.bestCost = double.MaxValue;
			val.bestLinkNode = null;
			val.isInCandidateList = false;
		}
		candidates.Enqueue(start);
		start.bestCost = 0.0;
		start.isInCandidateList = true;
		while (candidates.Count > 0)
		{
			_Data val = candidates.Dequeue();
			val.isInCandidateList = false;
			val.pathingID = pathingID;
			nodeLinkEnum = val.GetEnumerator();
			while (nodeLinkEnum.MoveNext())
			{
				nodeLink = nodeLinkEnum.Current;
				UpdateShortestPath(val, nodeLink.Key, nodeLink.Value, val.bestCost, start, end);
			}
		}
	}

	public virtual void UpdateShortestPath(_Data node, _Data neighbor, _Link link, double bestCost, _Data startNode, _Data endNode)
	{
		if (bestCost < neighbor.bestCost)
		{
			neighbor.bestCost = bestCost;
			neighbor.bestLinkNode = node;
			neighbor.bestLink = link;
			if (!neighbor.isInCandidateList && neighbor.pathingID != pathingID)
			{
				candidates.Enqueue(neighbor);
				neighbor.isInCandidateList = true;
				neighbor.pathingID = pathingID;
			}
		}
	}

	public virtual _Data UpdateShortestWhere(_Data a, _Data b, _Link link, double bestDistance, _Data startNode, Func<_Data, _Data, bool> whereClause)
	{
		if (bestDistance < b.bestCost)
		{
			b.bestCost = bestDistance;
			b.bestLinkNode = a;
			b.bestLink = link;
			if (!b.isInCandidateList && b.pathingID != pathingID)
			{
				candidates.Enqueue(b);
				b.isInCandidateList = true;
				b.pathingID = pathingID;
			}
			if (whereClause(startNode, b))
			{
				return b;
			}
			return null;
		}
		return null;
	}

	public virtual void GetLinkPoints(List<Vector3> discreteLines)
	{
		if (discreteLines == null)
		{
			return;
		}
		int count = links.Count;
		if (count == 0)
		{
			discreteLines.Clear();
			return;
		}
		if (count * 2 != discreteLines.Count)
		{
			discreteLines.Clear();
			for (int i = 0; i < count; i++)
			{
				discreteLines.Add(links[i].start.position);
				discreteLines.Add(links[i].end.position);
			}
			return;
		}
		int num = 0;
		int num2 = 0;
		while (num < count)
		{
			discreteLines[num2] = links[num].start.position;
			discreteLines[++num2] = links[num].end.position;
			num++;
			num2++;
		}
	}

	public override string ToString()
	{
		return "Net: " + nodes.Count + " nodes. " + links.Count + " links. " + occluders.Count + " occluders.";
	}
}
