using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet.Network;

public class Link<_Net, _Data, _Link, _Path> where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public _Data a;

	public _Data b;

	public double cost;

	public int pathingID;

	public _Data start
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public _Data end
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Link()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Set(_Data a, _Data b, double cost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update(double cost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public _Data OtherEnd(_Data toNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(_Data node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetPoints(List<Vector3> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
