using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UIListMover : MonoBehaviour
{
	[Serializable]
	public class ListAnchor
	{
		public string anchorName;

		public bool reversedOrder;

		public RectTransform anchor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ListAnchor()
		{
			throw null;
		}
	}

	public UIList list;

	public ListAnchor[] anchors;

	[NonSerialized]
	public ListAnchor CurrentAnchor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListMover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAnchor(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAnchor(string name)
	{
		throw null;
	}
}
