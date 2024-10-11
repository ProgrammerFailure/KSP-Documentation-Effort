using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class StageIcon : MonoBehaviour
{
	[SerializeField]
	protected RawImage iconImage;

	[SerializeField]
	protected Image backgroundImage;

	[SerializeField]
	protected Image borderImage;

	[SerializeField]
	protected Texture2D defaultIconMap;

	[SerializeField]
	protected int defaultIconSize;

	[SerializeField]
	private RectTransform listAnchorVertical;

	[SerializeField]
	protected UIDragPanel dragHandler;

	[SerializeField]
	public UIRadioButton radioButton;

	[SerializeField]
	protected TextMeshProUGUI textStar;

	[SerializeField]
	protected TextMeshProUGUI textSymmetry;

	[SerializeField]
	protected CanvasGroup canvasGroup;

	[SerializeField]
	protected PointerEnterExitHandler hoverHandler;

	[SerializeField]
	private RectTransform listAnchorInfoBoxes;

	[SerializeField]
	protected StageIconInfoBox stageIconInfoBoxPrefab;

	protected StageGroup stage;

	protected ProtoStageIcon protoIcon;

	public int SiblingIndex;

	public DefaultIcons iconType;

	public bool frozen;

	public bool grouped;

	public bool expanded;

	public List<StageIcon> groupedIcons;

	public StageIcon groupLead;

	public bool infoDisplay;

	private RectTransform rectTransform;

	private bool mouseHover;

	private StageGroup oldStageGroup;

	public static int maxInfoBoxes;

	private StageIconInfoBox[] infoBoxes;

	public bool underDrag;

	public bool showBorder;

	public bool selected;

	public bool blinkBorder;

	public float blinkInterval;

	private int foundInverseStageIndex;

	private int lastInversetageIndex;

	private int foundInStageIndex;

	private int lastInStageIndex;

	private ResizingLayoutElement lastSpawnedResizingElement;

	private bool beginDrag;

	private StageGroup droppedOnGroup;

	public CanvasGroup CanvasGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	public StageGroup Stage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public ProtoStageIcon ProtoIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isMainIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isSymmetryCounterPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int InverseStage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int InStageIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int DefaultSequenceIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int OriginalStageIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public PartStates partState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Type partModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public StackIconGrouping IconGroupingRule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string partType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<Part> Counterparts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isDisplayingInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isDisplayingInfoInGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static StageIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ProtoStageIcon protoIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClick(PointerEventData data, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(Texture2D texture, int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(DefaultIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(string customIconFilePath, int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBackgroundColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowBorder(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBorderColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Freeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unfreeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BlinkBorder(float interval)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Blink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Highlight(bool highlightState, bool highlightReferencedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightPart(bool highlightState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInverseSequenceIndex(int inverseIndex, int inStageIndex, bool seqOverride = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetManualStageOffset(int inverseIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInStageIndex(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInStageIndexOFGroupedIcons(bool setSiblingIndex = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortGroupedIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetStageIconSymmetryGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetStageGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ResetAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ResetStageGroupAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddToGroup(StageIcon icon, bool setParent = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveFromGroup(StageIcon icon, bool resetGroupLeadIfempty = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveFromGroupAndReshuffle(out StageGroup foundInGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HoldGroupedIcons(bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ExpandGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ExpandGroupInUIOnly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CollapseGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableGroup(StageIcon iconToDisable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConsolidateMembers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSymmetryMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSymmetryText(bool active, string number = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIconInfoBox DisplayInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveInfo(StageIconInfoBox iBox)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearInfoBoxes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIconInfoBox ForceDisplayInfo()
	{
		throw null;
	}
}
