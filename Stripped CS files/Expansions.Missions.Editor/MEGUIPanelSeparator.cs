using Cursors;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MEGUIPanelSeparator : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
	public MEGUIPanel Panel;

	public string CursorID;

	public TextureCursor DefaultCursor;

	public TextureCursor LeftClickCursor;

	public TextureCursor RightClickCursor;

	public Vector2 PanelOffset;

	public static bool isDragging;

	public void Start()
	{
		isDragging = false;
		if (!string.IsNullOrEmpty(CursorID) && !CursorController.Instance.Contains(CursorID))
		{
			CursorController.Instance.AddCursor(CursorID, DefaultCursor, LeftClickCursor, RightClickCursor);
		}
		PanelOffset *= UIMasterController.Instance.uiScale;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (Panel.State == MEGUIPanel.PanelState.Normal)
		{
			if (Panel.Anchor == MEGUIPanel.PanelAnchor.Bottom)
			{
				Panel.CurrentHeight = Mathf.Clamp(eventData.position.y + PanelOffset.y, Panel.minHeight, (Panel.maxHeight <= 0f) ? ((float)Screen.height) : Panel.maxHeight) / UIMasterController.Instance.uiScale;
			}
			else if (Panel.Anchor == MEGUIPanel.PanelAnchor.Top)
			{
				Panel.CurrentHeight = Mathf.Clamp((float)Screen.height - (eventData.position.y + PanelOffset.y), Panel.minHeight, (Panel.maxHeight <= 0f) ? ((float)Screen.height) : Panel.maxHeight) / UIMasterController.Instance.uiScale;
			}
			else if (Panel.Anchor == MEGUIPanel.PanelAnchor.Left)
			{
				Panel.CurrentWidth = Mathf.Clamp(eventData.position.x, Panel.minWidth, (Panel.maxWidth <= 0f) ? ((float)Screen.width) : Panel.maxWidth) / UIMasterController.Instance.uiScale;
			}
			else if (Panel.Anchor == MEGUIPanel.PanelAnchor.Right)
			{
				Panel.CurrentWidth = Mathf.Clamp((float)Screen.width - eventData.position.x, Panel.minWidth, (Panel.maxWidth <= 0f) ? ((float)Screen.width) : Panel.maxWidth) / UIMasterController.Instance.uiScale;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!isDragging)
		{
			CursorController.Instance.ChangeCursor(CursorID);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isDragging)
		{
			CursorController.Instance.ChangeCursor("default");
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
		CursorController.Instance.ChangeCursor(CursorID);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
		CursorController.Instance.ChangeCursor("default");
		MissionEditorLogic.Instance.actionPane.SavePreferedGapSize();
	}
}
