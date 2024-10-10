using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIScrollList : DialogGUIBase
{
	public DialogGUILayoutBase layout;

	public bool hScrollBar;

	public bool vScrollBar;

	public ScrollRect scrollRect;

	public GameObject content;

	public Vector2 contentSize;

	public GameObject Content => content;

	public DialogGUIScrollList(Vector2 size, bool hScroll, bool vScroll, DialogGUILayoutBase layout)
	{
		this.layout = layout;
		base.size = size;
		contentSize = size;
		vScrollBar = vScroll;
		hScrollBar = hScroll;
	}

	public DialogGUIScrollList(Vector2 size, Vector2 contentSize, bool hScroll, bool vScroll, DialogGUILayoutBase layout)
	{
		this.layout = layout;
		base.size = size;
		this.contentSize = contentSize;
		vScrollBar = vScroll;
		hScrollBar = hScroll;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Resize()
	{
		base.Resize();
		if (scrollRect != null && scrollRect.vertical)
		{
			scrollRect.verticalNormalizedPosition = ((scrollRect.verticalScrollbar.direction == Scrollbar.Direction.BottomToTop) ? 1f : 0f);
		}
		if (scrollRect != null && scrollRect.horizontal)
		{
			scrollRect.horizontalNormalizedPosition = ((scrollRect.horizontalScrollbar.direction == Scrollbar.Direction.LeftToRight) ? 0f : 1f);
		}
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		GameObject gameObject = (uiItem = Object.Instantiate(UISkinManager.GetPrefab("UIScrollViewPrefab")));
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		scrollRect = uiItem.GetComponentInChildren<ScrollRect>();
		content = scrollRect.content.gameObject;
		scrollRect.scrollSensitivity = 10f;
		scrollRect.movementType = ScrollRect.MovementType.Elastic;
		if (!vScrollBar)
		{
			scrollRect.vertical = false;
			scrollRect.verticalScrollbar.gameObject.SetActive(value: false);
			scrollRect.verticalScrollbar = null;
		}
		else
		{
			scrollRect.verticalScrollbar.gameObject.SetActive(value: true);
			scrollRect.verticalScrollbar.GetComponent<Image>().sprite = skin.verticalScrollbar.normal.background;
			scrollRect.verticalScrollbar.GetComponent<Scrollbar>().targetGraphic.GetComponent<Image>().sprite = skin.verticalScrollbarThumb.normal.background;
			scrollRect.verticalNormalizedPosition = ((scrollRect.verticalScrollbar.direction == Scrollbar.Direction.BottomToTop) ? 1f : 0f);
			scrollRect.verticalScrollbar.handleRect.anchoredPosition = new Vector2(skin.verticalSlider.fixedWidth / scrollRect.verticalScrollbar.GetComponent<RectTransform>().sizeDelta.y, 0f - skin.verticalScrollbar.fixedWidth);
		}
		if (!hScrollBar)
		{
			scrollRect.horizontal = false;
			scrollRect.horizontalScrollbar.gameObject.SetActive(value: false);
			scrollRect.horizontalScrollbar = null;
		}
		else
		{
			scrollRect.horizontalScrollbar.gameObject.SetActive(value: true);
			scrollRect.horizontalScrollbar.GetComponent<Image>().sprite = skin.horizontalScrollbar.normal.background;
			scrollRect.horizontalScrollbar.GetComponent<Scrollbar>().targetGraphic.GetComponent<Image>().sprite = skin.horizontalScrollbarThumb.normal.background;
			scrollRect.horizontalNormalizedPosition = ((scrollRect.horizontalScrollbar.direction == Scrollbar.Direction.LeftToRight) ? 0f : 1f);
		}
		uiItem = scrollRect.content.gameObject;
		layout.useParent = true;
		layouts.Push(uiItem.transform);
		layout.Create(ref layouts, skin);
		layouts.Pop();
		uiItem = gameObject;
		base.Create(ref layouts, skin);
		children.Add(layout);
		if (scrollRect.vertical)
		{
			scrollRect.verticalNormalizedPosition = ((scrollRect.verticalScrollbar.direction == Scrollbar.Direction.BottomToTop) ? 1f : 0f);
		}
		if (scrollRect.horizontal)
		{
			scrollRect.horizontalNormalizedPosition = ((scrollRect.horizontalScrollbar.direction == Scrollbar.Direction.LeftToRight) ? 0f : 1f);
		}
		if (contentSize.y > size.y)
		{
			(scrollRect.content.transform as RectTransform).offsetMin = new Vector2((scrollRect.content.transform as RectTransform).offsetMin.x, size.y - contentSize.y);
		}
		if (contentSize.x > size.x)
		{
			(scrollRect.content.transform as RectTransform).offsetMin = new Vector2(size.x - contentSize.x, (scrollRect.content.transform as RectTransform).offsetMin.y);
		}
		return uiItem;
	}
}
