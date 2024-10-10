using System.Collections.Generic;
using UnityEngine;

namespace CommNet.Network;

public class Path<_Net, _Data, _Link, _Path> : List<_Link>, IConfigNode where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public virtual _Link First
	{
		get
		{
			if (base.Count == 0)
			{
				return null;
			}
			return base[0];
		}
	}

	public virtual _Link Last
	{
		get
		{
			if (base.Count == 0)
			{
				return null;
			}
			return base[base.Count - 1];
		}
	}

	public virtual bool IsEmpty()
	{
		return base.Count == 0;
	}

	public virtual void CopyTo(_Path path, bool deepCopy = true)
	{
		path.Clear();
		if (deepCopy)
		{
			int i = 0;
			for (int count = base.Count; i < count; i++)
			{
				_Link val = base[i];
				_Link val2 = new _Link();
				val2.Set(val.a, val.b, val.cost);
				path.Add(val2);
			}
		}
		else
		{
			path.AddRange(this);
		}
	}

	public virtual void Load(ConfigNode node)
	{
	}

	public virtual void Save(ConfigNode node)
	{
	}

	public virtual void UpdateFromPath()
	{
	}

	public virtual void GetPoints(List<Vector3> points, bool discrete = true)
	{
		if (points == null)
		{
			return;
		}
		int count = base.Count;
		if (count == 0)
		{
			points.Clear();
		}
		else if (discrete)
		{
			if (count * 2 != points.Count)
			{
				points.Clear();
				for (int i = 0; i < count; i++)
				{
					points.Add(base[i].start.position);
					points.Add(base[i].end.position);
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			while (num < count)
			{
				points[num2] = base[num].start.position;
				points[++num2] = base[num].end.position;
				num++;
				num2++;
			}
		}
		else if (count + 1 != points.Count)
		{
			points.Clear();
			for (int j = 0; j < count; j++)
			{
				points.Add(base[j].start.position);
			}
			points.Add(base[count - 1].end.position);
		}
		else
		{
			for (int k = 0; k < count; k++)
			{
				points[k] = base[k].start.position;
			}
			points[count] = base[count - 1].end.position;
		}
	}

	public static bool operator ==(Path<_Net, _Data, _Link, _Path> a, Path<_Net, _Data, _Link, _Path> b)
	{
		if ((object)a == b)
		{
			return true;
		}
		if ((object)a != null && (object)b != null)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = a.Count; i < count; i++)
			{
				_Link val = a[i];
				int j = 0;
				for (int count2 = b.Count; j < count2; j++)
				{
					_Link val2 = b[j];
					if (val.start != val2.start && val.end != val2.end)
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}

	public static bool operator !=(Path<_Net, _Data, _Link, _Path> a, Path<_Net, _Data, _Link, _Path> b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		_Path val = obj as _Path;
		if (val == null)
		{
			return false;
		}
		return this == val;
	}

	public bool Equals(_Path p)
	{
		if ((object)p == null)
		{
			return false;
		}
		return this == p;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
