using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public interface ApplauncherLayout
{
	GameObject GetGameObject();

	RectTransform GetRectTransform();

	LayoutElement GetTopRightSpacer();

	LayoutElement GetBottomLeftSpacer();

	UIList GetStockList();

	UIList GetModList();

	UIList GetKnowledgeBaseList();

	ScrollRect GetModScrollRect();

	Image GetModListDivider();

	PointerClickAndHoldHandler GetModListBtnUp();

	PointerClickAndHoldHandler GetModListBtnDown();
}
