using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet.Network;

public class Path<_Net, _Data, _Link, _Path> : List<_Link>, IConfigNode where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public virtual _Link First
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual _Link Last
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Path()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsEmpty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CopyTo(_Path path, bool deepCopy = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateFromPath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetPoints(List<Vector3> points, bool discrete = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(Path<_Net, _Data, _Link, _Path> a, Path<_Net, _Data, _Link, _Path> b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(Path<_Net, _Data, _Link, _Path> a, Path<_Net, _Data, _Link, _Path> b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(_Path p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}
}
