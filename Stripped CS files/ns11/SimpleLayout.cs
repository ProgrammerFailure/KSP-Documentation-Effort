using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

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

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public RectTransform GetRectTransform()
	{
		return base.transform as RectTransform;
	}

	public LayoutElement GetTopRightSpacer()
	{
		return topRightSpacer;
	}

	public LayoutElement GetBottomLeftSpacer()
	{
		return bottomLeftSpacer;
	}

	public UIList GetStockList()
	{
		return listStock;
	}

	public UIList GetModList()
	{
		return listMod;
	}

	public UIList GetKnowledgeBaseList()
	{
		return listKB;
	}

	public ScrollRect GetModScrollRect()
	{
		return scrollRectMods;
	}

	public Image GetModListDivider()
	{
		return modListDivider;
	}

	public PointerClickAndHoldHandler GetModListBtnUp()
	{
		return modListBtnUp;
	}

	public PointerClickAndHoldHandler GetModListBtnDown()
	{
		return modListBtnDown;
	}
}
