using System;
using ns5;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEGUINodeIcon : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IComparable<MEGUINodeIcon>, IPointerClickHandler
{
	public static MEGUINodeIcon selectedMEGUINodeIcon;

	public static Vector2 mouseSelectedPosition;

	public RawImage nodeImage;

	public TextMeshProUGUI nodeText;

	public bool IsInteractable;

	public MissionEditorLogic meLogic;

	public MEGUINode draggedNode;

	[SerializeField]
	public MEBasicNode basicNode;

	[SerializeField]
	public string lowerCaseCategoryName;

	[SerializeField]
	public string lowerCaseDisplayName;

	[SerializeField]
	public string lowerCaseDescription;

	[SerializeField]
	public Toggle toggle;

	[SerializeField]
	public RectTransform rectTransform;

	[SerializeField]
	public RectTransform nodeImageRect;

	[SerializeField]
	public LayoutElement rectLayout;

	[SerializeField]
	public Image highlighter;

	public Image backgroundImage;

	[SerializeField]
	public Color backgroundMouseOverColor = Color.white;

	[SerializeField]
	public Color nodeImageMouseOverColor = Color.white;

	[SerializeField]
	public Color backgroundCanvasNodeColor = Color.white;

	[SerializeField]
	public Color nodeImageCanvasNodeColor = Color.white;

	[SerializeField]
	public Color backgroundCanvasNodeMouseOverColor = Color.white;

	[SerializeField]
	public Color nodeImageCanvasNodeMouseOverColor = Color.white;

	[SerializeField]
	public Color uninteractableColor = Color.white;

	[SerializeField]
	public MEGUINode canvasMEGUINode;

	public Color backgroundNormalColor;

	public Color nodeImageNormalColor;

	public bool withinCanvas;

	[SerializeField]
	public Color highLightColor = Color.white;

	[SerializeField]
	public Color searchHighLightColor = Color.white;

	public bool IsCanvasNode => canvasMEGUINode != null;

	public MEGUINode CanvasMEGUINode => canvasMEGUINode;

	public void Start()
	{
		meLogic = MissionEditorLogic.Instance;
		backgroundNormalColor = backgroundImage.color;
		nodeImageNormalColor = nodeImage.color;
	}

	public void SetUp(MEBasicNode newBasicNode, Icon icon)
	{
		SetUp(newBasicNode, icon, null);
	}

	public void SetUp(MEBasicNode newBasicNode, Icon icon, MEGUINode meguicanvasNode)
	{
		basicNode = newBasicNode;
		canvasMEGUINode = meguicanvasNode;
		if (IsCanvasNode)
		{
			nodeText.text = meguicanvasNode.title.text;
			base.name = base.name + " " + nodeText.text;
			if (rectTransform != null && nodeImageRect != null && rectLayout != null)
			{
				rectLayout.minHeight = 30f;
				rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, 30f);
				nodeImageRect.sizeDelta = new Vector2(20f, 20f);
			}
			if (backgroundImage != null)
			{
				backgroundImage.color = backgroundCanvasNodeColor;
				backgroundNormalColor = backgroundImage.color;
				nodeImage.color = nodeImageCanvasNodeColor;
				nodeImageNormalColor = nodeImage.color;
			}
		}
		else
		{
			nodeText.text = basicNode.displayName;
		}
		lowerCaseDisplayName = basicNode.displayName.ToLowerInvariant();
		lowerCaseCategoryName = basicNode.categoryDisplayName.ToLowerInvariant();
		lowerCaseDescription = basicNode.description.ToLowerInvariant();
		toggle.onValueChanged.AddListener(onToggle);
		if (icon != null && !string.IsNullOrEmpty(newBasicNode.iconURL))
		{
			nodeImage.texture = icon.iconNormal;
		}
	}

	public void onToggle(bool toggled)
	{
		SetHighlighterColor(toggled ? searchHighLightColor : highLightColor);
		ToggleHighlighter(toggled);
		if (IsCanvasNode)
		{
			if (toggled)
			{
				canvasMEGUINode.Select(deselectOtherNodes: true, bypassNodeSelection: true);
				meLogic.NodeCanvas.FocusNode(canvasMEGUINode);
			}
			else
			{
				canvasMEGUINode.Deselect();
			}
		}
		else
		{
			if (basicNode == null)
			{
				return;
			}
			for (int num = MissionEditorLogic.Instance.editorNodeList.Count - 1; num >= 0; num--)
			{
				MEGUINode mEGUINode = MissionEditorLogic.Instance.editorNodeList[num];
				if (mEGUINode.BasicNode.displayName == basicNode.displayName)
				{
					if (toggled)
					{
						mEGUINode.Select(deselectOtherNodes: false, bypassNodeSelection: true);
					}
					else
					{
						mEGUINode.Deselect();
					}
				}
			}
			if (toggled)
			{
				meLogic.NodeCanvas.FitCameraToSelectedNodes(clampMin: true, clampMax: false);
			}
		}
	}

	public void ResetCanvasSetting()
	{
		withinCanvas = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (IsInteractable)
		{
			if (IsCanvasNode)
			{
				backgroundImage.color = backgroundCanvasNodeMouseOverColor;
				nodeImage.color = nodeImageCanvasNodeMouseOverColor;
			}
			else
			{
				backgroundImage.color = backgroundMouseOverColor;
				nodeImage.color = nodeImageMouseOverColor;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (IsInteractable)
		{
			backgroundImage.color = backgroundNormalColor;
			nodeImage.color = nodeImageNormalColor;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if ((IsCanvasNode && canvasMEGUINode.Node.isStartNode) || !IsInteractable || !(draggedNode == null))
		{
			return;
		}
		if (IsCanvasNode)
		{
			MENode node = MENode.Clone(canvasMEGUINode.Node, meLogic.GetGridMousePosition());
			draggedNode = meLogic.CreateNodeAtPosition(meLogic.GetGridMousePosition(), node);
			if (draggedNode.dockedParentNode != null)
			{
				draggedNode.UndockNode();
			}
		}
		else
		{
			draggedNode = meLogic.CreateNodeAtMousePosition(basicNode);
		}
		meLogic.ClearConnectorGroupSelection();
		meLogic.ClearSelectedNodesList();
		meLogic.NodeSelectionChange(null);
		draggedNode.SetupDockingData();
		draggedNode.OnDragSetDockingIndicators();
		withinCanvas = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (IsInteractable && draggedNode != null)
		{
			draggedNode.CheckNodeDockingPosition(meLogic.GetGridMousePosition());
			if (withinCanvas)
			{
				MENodeCanvas.Instance.TryCanvasMovement(eventData, draggedNode);
				MENodeCanvas.Instance.ToggleScrollLock(newValue: true);
			}
			else
			{
				withinCanvas = MENodeCanvas.Instance.PointerInsideCanvasView(eventData);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (IsInteractable && draggedNode != null)
		{
			draggedNode.PlaceNodeAtCanvas(meLogic.GetGridMousePosition(), this);
			meLogic.ClearSelectedNodesList();
			if (MENodeCategorizer.Instance.ShowingSearchResults)
			{
				MENodeCategorizer.Instance.HighLightSelectedSearchResults();
			}
			else
			{
				draggedNode.Select(deselectOtherNodes: true, bypassNodeSelection: true);
			}
			draggedNode.CheckDockStatus();
			draggedNode.OnDragEndClearDockingIndicators();
			draggedNode = null;
			MENodeCanvas.Instance.StopCanvasMovement();
			MENodeCanvas.Instance.ToggleScrollLock(newValue: false);
		}
	}

	public void Update()
	{
		if (draggedNode != null)
		{
			draggedNode.transform.localPosition = MENodeCanvas.CheckSnap(meLogic.GetGridMousePosition());
			if (draggedNode.DockCandidate != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(draggedNode.DockCandidate.GetComponent<RectTransform>());
			}
		}
		else if (selectedMEGUINodeIcon != null && Input.GetMouseButtonDown(0))
		{
			selectedMEGUINodeIcon.OnClickCanvas();
		}
	}

	public int CompareTo(MEGUINodeIcon other)
	{
		return other.basicNode.displayName.CompareTo(basicNode.displayName);
	}

	public void ToggleHighlighter(bool state)
	{
		if (highlighter != null)
		{
			highlighter.gameObject.SetActive(state);
		}
	}

	public void SetHighlighterColor(Color newColor)
	{
		if (highlighter != null)
		{
			highlighter.color = newColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.dragging)
		{
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (selectedMEGUINodeIcon == this)
			{
				selectedMEGUINodeIcon = null;
				return;
			}
			selectedMEGUINodeIcon = this;
			mouseSelectedPosition = meLogic.GetGridMousePosition();
		}
		else
		{
			selectedMEGUINodeIcon = null;
		}
	}

	public void OnClickCanvas()
	{
		if ((IsCanvasNode && canvasMEGUINode.Node.isStartNode) || !IsInteractable || !(draggedNode == null))
		{
			return;
		}
		if (IsCanvasNode)
		{
			MENode node = MENode.Clone(canvasMEGUINode.Node, meLogic.GetGridMousePosition());
			draggedNode = meLogic.CreateNodeAtPosition(mouseSelectedPosition, node);
			if (draggedNode.dockedParentNode != null)
			{
				draggedNode.UndockNode();
			}
		}
		else
		{
			draggedNode = meLogic.CreateNodeAtMousePosition(basicNode);
		}
		meLogic.ClearConnectorGroupSelection();
		meLogic.ClearSelectedNodesList();
		meLogic.NodeSelectionChange(null);
		draggedNode.SetupDockingData();
		draggedNode.OnDragSetDockingIndicators();
		withinCanvas = false;
		draggedNode.PlaceNodeAtCanvas(meLogic.GetGridMousePosition(), this);
		meLogic.ClearSelectedNodesList();
		if (MENodeCategorizer.Instance.ShowingSearchResults)
		{
			MENodeCategorizer.Instance.HighLightSelectedSearchResults();
		}
		else
		{
			draggedNode.Select(deselectOtherNodes: true, bypassNodeSelection: true);
		}
		draggedNode.CheckDockStatus();
		draggedNode.OnDragEndClearDockingIndicators();
		draggedNode = null;
		MENodeCanvas.Instance.StopCanvasMovement();
		MENodeCanvas.Instance.ToggleScrollLock(newValue: false);
		selectedMEGUINodeIcon = null;
	}
}
