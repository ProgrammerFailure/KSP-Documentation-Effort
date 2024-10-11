using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class SimpleLayout : MonoBehaviour, ApplauncherLayout
{
	public string layoutName;

	public LayoutElement topRightSpacer;

	public LayoutElement bottomLeftSpacer;

	public UIList listStock;

	public UIList listMod;

	public UIList listKB;

	public ScrollRect scrollRectMods;

	public Image modListDivider;

	public PointerClickAndHoldHandler modListBtnUp;

	public PointerClickAndHoldHandler modListBtnDown;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SimpleLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetGameObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RectTransform GetRectTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LayoutElement GetTopRightSpacer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LayoutElement GetBottomLeftSpacer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList GetStockList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList GetModList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList GetKnowledgeBaseList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScrollRect GetModScrollRect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Image GetModListDivider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PointerClickAndHoldHandler GetModListBtnUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PointerClickAndHoldHandler GetModListBtnDown()
	{
		throw null;
	}
}
