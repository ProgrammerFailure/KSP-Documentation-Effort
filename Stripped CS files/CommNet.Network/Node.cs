using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace CommNet.Network;

public class Node<_Net, _Data, _Link, _Path> : Dictionary<_Data, _Link> where _Net : Net<_Net, _Data, _Link, _Path> where _Data : Node<_Net, _Data, _Link, _Path> where _Link : Link<_Net, _Data, _Link, _Path>, new() where _Path : Path<_Net, _Data, _Link, _Path>
{
	public bool isInCandidateList;

	public int pathingID;

	public double bestCost;

	public _Link bestLink;

	public _Data bestLinkNode;

	public static ValueCollection.Enumerator linkEnumerator;

	public virtual _Net Net { get; set; }

	public virtual string name
	{
		get
		{
			return string.Empty;
		}
		set
		{
		}
	}

	public virtual string displayName
	{
		get
		{
			return string.Empty;
		}
		set
		{
		}
	}

	public virtual Vector3d position
	{
		get
		{
			return Vector3.zero;
		}
		set
		{
		}
	}

	public virtual Occluder occluder { get; set; }

	public virtual void SetNet(_Net network)
	{
		Net = network;
	}

	public virtual void NetworkPreUpdate()
	{
	}

	public virtual void NetworkPostUpdate()
	{
	}

	public void SetOccluder(Occluder occluder)
	{
		if (occluder == null)
		{
			if (Net != null)
			{
				Net.Remove(this.occluder);
			}
			this.occluder = null;
			return;
		}
		if (Net != null)
		{
			if (this.occluder != null)
			{
				Net.Remove(this.occluder);
			}
			Net.Add(this.occluder);
		}
		this.occluder = occluder;
	}

	public override string ToString()
	{
		return "Node: " + Localizer.Format(displayName);
	}

	public virtual void GetLinkPoints(List<Vector3> discreteLines)
	{
		if (discreteLines == null)
		{
			return;
		}
		int num = base.Count;
		if (num == 0)
		{
			discreteLines.Clear();
			return;
		}
		linkEnumerator = base.Values.GetEnumerator();
		if (num * 2 != discreteLines.Count)
		{
			discreteLines.Clear();
			while (linkEnumerator.MoveNext())
			{
				_Link current = linkEnumerator.Current;
				discreteLines.Add(current.start.position);
				discreteLines.Add(current.end.position);
			}
			return;
		}
		int num2 = 0;
		while (linkEnumerator.MoveNext())
		{
			_Link current = linkEnumerator.Current;
			discreteLines[num2] = current.start.position;
			discreteLines[++num2] = current.end.position;
			num2++;
		}
	}
}
