using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace CommNet.Network;

public class Link<_Net, _Data, _Link, _Path> where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public _Data a;

	public _Data b;

	public double cost;

	public int pathingID;

	public _Data start => a;

	public _Data end => b;

	public virtual void Set(_Data a, _Data b, double cost)
	{
		this.a = a;
		this.b = b;
		Update(cost);
		this.cost = cost;
	}

	public virtual void Update(double cost)
	{
		this.cost = cost;
	}

	public virtual void OnDestroy()
	{
	}

	public _Data OtherEnd(_Data toNode)
	{
		if (a == toNode)
		{
			return b;
		}
		if (b == toNode)
		{
			return a;
		}
		return null;
	}

	public bool Contains(_Data node)
	{
		if (a != node)
		{
			return b == node;
		}
		return true;
	}

	public virtual void GetPoints(List<Vector3> points)
	{
		if (points != null)
		{
			if (2 != points.Count)
			{
				points.Clear();
				points.Add(a.position);
				points.Add(b.position);
			}
			else
			{
				points[0] = a.position;
				points[1] = b.position;
			}
		}
	}

	public override string ToString()
	{
		return "Link: " + ((a != null) ? Localizer.Format(a.displayName) : "null") + " -to- " + ((b != null) ? Localizer.Format(b.displayName) : "null");
	}
}
