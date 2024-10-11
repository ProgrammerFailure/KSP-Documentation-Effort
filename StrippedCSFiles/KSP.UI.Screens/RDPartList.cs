using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[RequireComponent(typeof(UIList))]
public class RDPartList : MonoBehaviour
{
	public GameObject partListItem;

	public Dictionary<AvailablePart, Texture2D> partIcons;

	public RectTransform partTransformMask;

	private ScrollRect partListScrollRect;

	private UIList scrollList;

	private RDNode selected_node;

	private List<RDPartListItem> partListItems;

	public List<RDPartListItem> listItems
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDPartList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupParts(RDNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddUpgrades(List<PartUpgradeHandler.Upgrade> purchased, List<PartUpgradeHandler.Upgrade> available)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddUpgradeListItem(PartUpgradeHandler.Upgrade upgrade, bool purchased)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddParts(List<AvailablePart> purchased, List<AvailablePart> assigned)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPartListItem(AvailablePart part, bool purchased)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPart(RDPartListItem item, bool unlocked, string label, AvailablePart part, PartUpgradeHandler.Upgrade upgrade)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartListUpdate(Vector2 vector)
	{
		throw null;
	}
}
