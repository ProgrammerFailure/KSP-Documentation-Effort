using System;
using System.Runtime.CompilerServices;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEGUINodeIcon : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IComparable<MEGUINodeIcon>, IPointerClickHandler
{
	private static MEGUINodeIcon selectedMEGUINodeIcon;

	private static Vector2 mouseSelectedPosition;

	public RawImage nodeImage;

	public TextMeshProUGUI nodeText;

	public bool IsInteractable;

	private MissionEditorLogic meLogic;

	private MEGUINode draggedNode;

	[SerializeField]
	public MEBasicNode basicNode;

	[SerializeField]
	internal string lowerCaseCategoryName;

	[SerializeField]
	internal string lowerCaseDisplayName;

	[SerializeField]
	internal string lowerCaseDescription;

	[SerializeField]
	internal Toggle toggle;

	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private RectTransform nodeImageRect;

	[SerializeField]
	private LayoutElement rectLayout;

	[SerializeField]
	private Image highlighter;

	public Image backgroundImage;

	[SerializeField]
	private Color backgroundMouseOverColor;

	[SerializeField]
	private Color nodeImageMouseOverColor;

	[SerializeField]
	private Color backgroundCanvasNodeColor;

	[SerializeField]
	private Color nodeImageCanvasNodeColor;

	[SerializeField]
	private Color backgroundCanvasNodeMouseOverColor;

	[SerializeField]
	private Color nodeImageCanvasNodeMouseOverColor;

	[SerializeField]
	private Color uninteractableColor;

	[SerializeField]
	internal MEGUINode canvasMEGUINode;

	private Color backgroundNormalColor;

	private Color nodeImageNormalColor;

	private bool withinCanvas;

	[SerializeField]
	private Color highLightColor;

	[SerializeField]
	private Color searchHighLightColor;

	public bool IsCanvasNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MEGUINode CanvasMEGUINode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINodeIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MEGUINodeIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUp(MEBasicNode newBasicNode, Icon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUp(MEBasicNode newBasicNode, Icon icon, MEGUINode meguicanvasNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onToggle(bool toggled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCanvasSetting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(MEGUINodeIcon other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHighlighter(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlighterColor(Color newColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickCanvas()
	{
		throw null;
	}
}
