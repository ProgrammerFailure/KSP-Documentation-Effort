using ns11;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class GenericAppFrame : MonoBehaviour
{
	public Image gfxHeader;

	public Image gfxFooter;

	public Image gfxBg;

	public PointerEnterExitHandler hoverController;

	public DragHandler dragHeader;

	public DragHandler dragFooter;

	public TextMeshProUGUI header;

	public UIList scrollList;

	public int minHeight = 176;

	public int maxHeight = 476;

	public ApplicationLauncherButton appLauncherButton;

	public RectTransform rectTransform;

	public ScrollRect scrollRect;

	public bool scaleHeightToContainList;

	public bool anchorToAppButton;

	public int width = 166;

	public int height = 176;

	public void Awake()
	{
		rectTransform = base.transform as RectTransform;
		if (scrollList != null)
		{
			scrollRect = scrollList.GetComponentInParent<ScrollRect>();
		}
	}

	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string displayName)
	{
		base.name = "AppFrame " + appName;
		this.appLauncherButton = appLauncherButton;
		if (header != null)
		{
			header.text = displayName;
		}
		ApplicationLauncher.Instance.AddOnRepositionCallback(Reposition);
		Reposition();
	}

	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string displayName, int width, int height)
	{
		this.width = width;
		this.height = height;
		rectTransform = base.transform as RectTransform;
		rectTransform.sizeDelta = new Vector2(width, height);
		Setup(appLauncherButton, appName, displayName);
	}

	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string displayName, int width, int height, bool scaleHeightToContainList)
	{
		this.scaleHeightToContainList = scaleHeightToContainList;
		Setup(appLauncherButton, appName, displayName, width, height);
		AutoScale();
	}

	public void AutoScale()
	{
		if (scaleHeightToContainList)
		{
			height = (int)scrollRect.content.sizeDelta.y + 27;
			rectTransform.sizeDelta = new Vector2(width, height);
		}
	}

	public void AddGlobalInputDelegate(UnityAction<PointerEventData> pointerEnter, UnityAction<PointerEventData> pointerExit)
	{
		hoverController.onPointerEnter.AddListener(pointerEnter);
		hoverController.onPointerExit.AddListener(pointerExit);
	}

	public void Reposition()
	{
		Debug.Log("[GenericAppFrame] Reposition " + Time.timeSinceLevelLoad + " " + Time.frameCount);
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			base.gameObject.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
			if (anchorToAppButton)
			{
				rectTransform.anchoredPosition = appLauncherButton.GetAnchorLocal();
			}
			else
			{
				rectTransform.anchoredPosition = appLauncherButton.GetAnchorTopRight();
			}
			if (!scaleHeightToContainList && dragFooter != null)
			{
				dragFooter.ClearEvents();
				dragFooter.AddEvents(OnBeginDragFooter, OnDragFooter, OnEndDragFooter);
			}
		}
		else
		{
			base.gameObject.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
			if (!scaleHeightToContainList && dragHeader != null)
			{
				dragHeader.ClearEvents();
				dragHeader.AddEvents(OnBeginDragHeader, OnDragHeader, OnEndDragHeader);
			}
			UpdateDraggingBoundsAtBottom(rectTransform.sizeDelta.y, appLauncherButton.GetAnchorUR().y + rectTransform.sizeDelta.y * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS);
		}
	}

	public void OnDestroy()
	{
		if ((bool)ApplicationLauncher.Instance)
		{
			ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
		}
	}

	public void OnBeginDragFooter(PointerEventData eventData)
	{
	}

	public void OnDragFooter(PointerEventData eventData)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - eventData.delta.y);
		UpdateDraggingBounds((int)rectTransform.sizeDelta.y);
	}

	public void OnEndDragFooter(PointerEventData eventData)
	{
		UpdateDraggingBounds((int)rectTransform.sizeDelta.y);
	}

	public void UpdateDraggingBounds(int height)
	{
		int num = height;
		if ((int)rectTransform.sizeDelta.y < minHeight)
		{
			num = minHeight;
		}
		if ((int)rectTransform.sizeDelta.y > maxHeight)
		{
			num = maxHeight;
		}
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num);
	}

	public void OnBeginDragHeader(PointerEventData eventData)
	{
	}

	public void OnDragHeader(PointerEventData eventData)
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + eventData.delta.y);
		UpdateDraggingBoundsAtBottom(rectTransform.sizeDelta.y, appLauncherButton.GetAnchorUR().y + rectTransform.sizeDelta.y * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS);
	}

	public void OnEndDragHeader(PointerEventData eventData)
	{
		UpdateDraggingBoundsAtBottom(rectTransform.sizeDelta.y, appLauncherButton.GetAnchorUR().y + rectTransform.sizeDelta.y * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS);
	}

	public void UpdateDraggingBoundsAtBottom(float height, float pos)
	{
		float y = height;
		float y2 = pos;
		if (!scaleHeightToContainList)
		{
			if (rectTransform.sizeDelta.y < (float)minHeight)
			{
				y = minHeight;
				y2 = (float)minHeight * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS + appLauncherButton.GetAnchorUR().y;
			}
			if (rectTransform.sizeDelta.y > (float)maxHeight)
			{
				y = maxHeight;
				y2 = (float)maxHeight * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS + appLauncherButton.GetAnchorUR().y;
			}
		}
		base.transform.position = new Vector3(appLauncherButton.GetAnchorUR().x, y2, appLauncherButton.GetAnchorUR().z);
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, y);
	}
}
