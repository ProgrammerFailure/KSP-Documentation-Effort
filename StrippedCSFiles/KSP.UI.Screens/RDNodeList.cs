using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

[RequireComponent(typeof(UIList))]
public class RDNodeList : MonoBehaviour
{
	public RDController controller;

	public RDNodeListItem nodeListItemPrefab;

	private UIList scrollList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDNodeList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddNodes(RDNode.Parent[] arrows)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearList(bool destroyItems)
	{
		throw null;
	}
}
