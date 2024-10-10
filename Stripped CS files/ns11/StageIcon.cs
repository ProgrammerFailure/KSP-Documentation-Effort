using System;
using System.Collections.Generic;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class StageIcon : MonoBehaviour
{
	[SerializeField]
	public RawImage iconImage;

	[SerializeField]
	public Image backgroundImage;

	[SerializeField]
	public Image borderImage;

	[SerializeField]
	public Texture2D defaultIconMap;

	[SerializeField]
	public int defaultIconSize = 64;

	[SerializeField]
	public RectTransform listAnchorVertical;

	[SerializeField]
	public UIDragPanel dragHandler;

	[SerializeField]
	public UIRadioButton radioButton;

	[SerializeField]
	public TextMeshProUGUI textStar;

	[SerializeField]
	public TextMeshProUGUI textSymmetry;

	[SerializeField]
	public CanvasGroup canvasGroup;

	[SerializeField]
	public PointerEnterExitHandler hoverHandler;

	[SerializeField]
	public RectTransform listAnchorInfoBoxes;

	[SerializeField]
	public StageIconInfoBox stageIconInfoBoxPrefab;

	public StageGroup stage;

	public ProtoStageIcon protoIcon;

	public int SiblingIndex = -1;

	public DefaultIcons iconType = DefaultIcons.MYSTERY_PART;

	public bool frozen;

	public bool grouped;

	public bool expanded;

	public List<StageIcon> groupedIcons = new List<StageIcon>();

	public StageIcon groupLead;

	public bool infoDisplay;

	public RectTransform rectTransform;

	public bool mouseHover;

	public StageGroup oldStageGroup;

	public static int maxInfoBoxes = 3;

	public StageIconInfoBox[] infoBoxes;

	public bool underDrag;

	public bool showBorder;

	public bool selected;

	public bool blinkBorder;

	public float blinkInterval = 1f;

	public int foundInverseStageIndex = -1;

	public int lastInversetageIndex = -1;

	public int foundInStageIndex = -1;

	public int lastInStageIndex = -1;

	public ResizingLayoutElement lastSpawnedResizingElement;

	public bool beginDrag = true;

	public StageGroup droppedOnGroup;

	public CanvasGroup CanvasGroup
	{
		get
		{
			return canvasGroup;
		}
		set
		{
			canvasGroup = value;
		}
	}

	public StageGroup Stage
	{
		get
		{
			return stage;
		}
		set
		{
			stage = value;
		}
	}

	public ProtoStageIcon ProtoIcon => protoIcon;

	public Part Part => protoIcon.Part;

	public bool isMainIcon
	{
		get
		{
			if (!(groupLead == this))
			{
				if (groupLead == null)
				{
					return Stage != null;
				}
				return false;
			}
			return true;
		}
	}

	public bool isSymmetryCounterPart
	{
		get
		{
			if (!(protoIcon.Part != null))
			{
				return false;
			}
			return protoIcon.Part.symmetryCounterparts.Count > 0;
		}
	}

	public int InverseStage
	{
		get
		{
			if (!(protoIcon.Part != null))
			{
				return 0;
			}
			return protoIcon.Part.inverseStage;
		}
	}

	public int InStageIndex
	{
		get
		{
			if (!(protoIcon.Part != null))
			{
				return -1;
			}
			return protoIcon.Part.inStageIndex;
		}
	}

	public int DefaultSequenceIndex
	{
		get
		{
			if (!(protoIcon.Part != null))
			{
				return 0;
			}
			return protoIcon.Part.defaultInverseStage;
		}
	}

	public int OriginalStageIndex
	{
		get
		{
			if (!(protoIcon.Part != null))
			{
				return 0;
			}
			return protoIcon.Part.originalStage;
		}
	}

	public PartStates partState
	{
		get
		{
			if (!protoIcon.Part)
			{
				return PartStates.IDLE;
			}
			return protoIcon.Part.State;
		}
	}

	public Type partModule
	{
		get
		{
			if (!protoIcon.Part)
			{
				return null;
			}
			return protoIcon.Part.GetType();
		}
	}

	public StackIconGrouping IconGroupingRule
	{
		get
		{
			if (!protoIcon.Part)
			{
				return StackIconGrouping.NONE;
			}
			return protoIcon.Part.stackIconGrouping;
		}
	}

	public string partType
	{
		get
		{
			if (!protoIcon.Part)
			{
				return "";
			}
			return protoIcon.Part.partInfo.name;
		}
	}

	public List<Part> Counterparts
	{
		get
		{
			if (!protoIcon.Part)
			{
				return null;
			}
			return protoIcon.Part.symmetryCounterparts;
		}
	}

	public bool isDisplayingInfo
	{
		get
		{
			int num = 0;
			int num2 = infoBoxes.Length;
			while (true)
			{
				if (num < num2)
				{
					if (infoBoxes[num] != null && infoBoxes[num].expanded)
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
	}

	public bool isDisplayingInfoInGroup
	{
		get
		{
			if (groupLead != this)
			{
				return false;
			}
			if (isDisplayingInfo)
			{
				return true;
			}
			int num = 0;
			int count = groupedIcons.Count;
			while (true)
			{
				if (num < count)
				{
					if (groupedIcons[num].isDisplayingInfo)
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
	}

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		dragHandler.beforeBeginDrag.AddListener(OnBeginDrag);
		dragHandler.onDrag.AddListener(OnDrag);
		dragHandler.beforeEndDrag.AddListener(OnEndDrag);
		dragHandler.manualDragOffset = new Vector3(7f, 0f, 0f);
		hoverHandler.onPointerEnter.AddListener(MouseInput_pointerEnter);
		hoverHandler.onPointerExit.AddListener(MouseInput_pointerExit);
		radioButton.onTrueBtn.AddListener(OnTrue);
		radioButton.onFalseBtn.AddListener(OnFalse);
		radioButton.onClick.AddListener(OnClick);
		textStar.gameObject.SetActive(value: false);
		textSymmetry.gameObject.SetActive(value: false);
		infoBoxes = new StageIconInfoBox[maxInfoBoxes];
	}

	public void Start()
	{
		SetBackgroundColor(Color.white * 0.9f);
		SetBorderColor(Color.white);
	}

	public void Setup(ProtoStageIcon protoIcon)
	{
		this.protoIcon = protoIcon;
	}

	public void OnClick(PointerEventData data, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		if (data.button == PointerEventData.InputButton.Right && callType == UIRadioButton.CallType.USER && Part != null)
		{
			UIPartActionController.Instance.SpawnPartActionWindow(Part);
		}
	}

	public void MouseInput_pointerEnter(PointerEventData eventData)
	{
		mouseHover = true;
		HighlightPart(highlightState: true);
		if (groupLead == this && !expanded)
		{
			int i = 0;
			for (int count = groupedIcons.Count; i < count; i++)
			{
				groupedIcons[i].HighlightPart(highlightState: true);
			}
		}
	}

	public void MouseInput_pointerExit(PointerEventData eventData)
	{
		mouseHover = false;
		HighlightPart(highlightState: false);
		if (groupLead == this)
		{
			int i = 0;
			for (int count = groupedIcons.Count; i < count; i++)
			{
				groupedIcons[i].HighlightPart(highlightState: false);
			}
		}
	}

	public void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		if (button.RadioGroup == 0)
		{
			if (StageManager.Selection.Count > 0 && groupLead != null && StageManager.Selection[0].groupLead != groupLead)
			{
				if (groupLead.isDisplayingInfoInGroup)
				{
					List<StageIcon> list = new List<StageIcon>(StageManager.Selection);
					int i = 0;
					for (int count = list.Count; i < count; i++)
					{
						list[i].radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null, popButtonsInGroup: false);
					}
					StageManager.Selection.Clear();
				}
				else
				{
					StageManager.Selection[0].groupLead.CollapseGroup();
				}
			}
		}
		else
		{
			if (StageManager.Selection.Count > 0 && StageManager.Selection[0].groupLead != null && StageManager.Selection[0].groupLead != groupLead)
			{
				if (StageManager.Selection[0].groupLead.isDisplayingInfoInGroup)
				{
					int j = 0;
					for (int count2 = StageManager.Selection.Count; j < count2; j++)
					{
						StageManager.Selection[j].radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
					}
					StageManager.Selection.Clear();
				}
				else
				{
					StageManager.Selection[0].groupLead.CollapseGroup();
				}
			}
			if (this == groupLead && callType == UIRadioButton.CallType.USER && !groupedIcons.Exists((StageIcon a) => a.radioButton.Value) && !groupLead.isDisplayingInfoInGroup)
			{
				ExpandGroup();
				return;
			}
		}
		StageManager.Selection.Add(this);
	}

	public void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		if (groupLead != null && !StageManager.Selection.Exists((StageIcon a) => a.radioButton.Value) && !groupLead.isDisplayingInfoInGroup)
		{
			groupLead.CollapseGroup();
		}
		StageManager.Selection.Remove(this);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		beginDrag = true;
		if (groupLead != null)
		{
			oldStageGroup = groupLead.Stage;
		}
		else
		{
			oldStageGroup = GetComponentInParent<StageGroup>();
		}
		oldStageGroup.SetSiblingIndexes();
		if (!StageManager.Selection.Contains(this))
		{
			if (isSymmetryCounterPart && groupLead != null)
			{
				if (groupLead.groupedIcons.Exists((StageIcon a) => a.radioButton.Value))
				{
					radioButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, eventData, popButtonsInGroup: false);
				}
				else
				{
					radioButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.USER, eventData);
				}
			}
			else
			{
				radioButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.USER, eventData);
			}
		}
		if (StageManager.Selection.Count > 0 && StageManager.Selection[0].isSymmetryCounterPart)
		{
			int i = 0;
			for (int count = StageManager.Selection.Count; i < count; i++)
			{
				StageIcon stageIcon = StageManager.Selection[i];
				if (stageIcon != this)
				{
					if (StageManager.Instance.Visible)
					{
						TweeningController.Instance.SpawnResizingLayoutElement(oldStageGroup.IconsUIList, stageIcon.InStageIndex, stageIcon.transform.GetSiblingIndex(), stageIcon.rectTransform.sizeDelta.y, 0f, 0.15f, destroyOnCompletion: true, addElementOnTopOfList: true);
					}
					stageIcon.transform.SetParent(listAnchorVertical, worldPositionStays: false);
				}
				else if (StageManager.Instance.Visible)
				{
					TweeningController.Instance.SpawnResizingLayoutElement(oldStageGroup.IconsUIList, stageIcon.InStageIndex, stageIcon.transform.GetSiblingIndex(), stageIcon.rectTransform.sizeDelta.y, 0f, 0.15f, destroyOnCompletion: true, addElementOnTopOfList: true);
				}
			}
		}
		else if (StageManager.Instance.Visible)
		{
			TweeningController.Instance.SpawnResizingLayoutElement(Stage.IconsUIList, InStageIndex, base.transform.GetSiblingIndex(), rectTransform.sizeDelta.y, 0f, 0.15f, destroyOnCompletion: true, addElementOnTopOfList: true);
		}
		foundInverseStageIndex = -1;
		lastInversetageIndex = -1;
		foundInStageIndex = -1;
		lastInStageIndex = -1;
	}

	public void OnDrag(PointerEventData eventData)
	{
		lastInversetageIndex = foundInverseStageIndex;
		lastInStageIndex = foundInStageIndex;
		droppedOnGroup = null;
		foundInverseStageIndex = StageManager.Instance.GetStageGroupUnderCursor(eventData, out droppedOnGroup, beginDrag);
		beginDrag = false;
		if (lastInversetageIndex != foundInverseStageIndex)
		{
			foundInStageIndex = -1;
			lastInStageIndex = -1;
			if (lastSpawnedResizingElement != null)
			{
				lastSpawnedResizingElement.ReverseResizing(destroyOnCompletion: true, delegate
				{
				});
			}
		}
		if (!(droppedOnGroup != null))
		{
			return;
		}
		int modifiedSiblingIndex = -1;
		foundInStageIndex = droppedOnGroup.GetStageGroupIndex(eventData, out var _, out modifiedSiblingIndex);
		if (lastInStageIndex != foundInStageIndex)
		{
			if (lastSpawnedResizingElement != null)
			{
				lastSpawnedResizingElement.ReverseResizing(destroyOnCompletion: true, delegate
				{
				});
			}
			if (StageManager.Instance.Visible)
			{
				lastSpawnedResizingElement = TweeningController.Instance.SpawnResizingLayoutElement(droppedOnGroup.IconsUIList, foundInStageIndex, modifiedSiblingIndex, 0f, rectTransform.sizeDelta.y, 0.15f, destroyOnCompletion: false, addElementOnTopOfList: true);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (lastSpawnedResizingElement != null)
		{
			lastSpawnedResizingElement.ReverseResizing(destroyOnCompletion: true, delegate
			{
			});
		}
		if (droppedOnGroup == null)
		{
			if (StageManager.Selection.Count > 0 && StageManager.Selection[0].isSymmetryCounterPart && StageManager.Selection[0].groupLead != null)
			{
				StageManager.Selection[0].groupLead.ResetStageIconSymmetryGroup();
			}
			else
			{
				Stage.RemoveIcon(this, changeParents: false);
				oldStageGroup.AddIconAt(this, InStageIndex, SiblingIndex);
			}
		}
		else
		{
			int modifiedSiblingIndex = -1;
			foundInStageIndex = droppedOnGroup.GetStageGroupIndex(eventData, out var _, out modifiedSiblingIndex);
			int num = foundInStageIndex;
			if (InStageIndex < foundInStageIndex && droppedOnGroup == oldStageGroup)
			{
				num--;
			}
			if (StageManager.Selection.Count > 0 && StageManager.Selection[0].isSymmetryCounterPart)
			{
				if (droppedOnGroup == oldStageGroup)
				{
					StageIcon stageIcon2 = ((StageManager.Selection[0].groupLead != null) ? StageManager.Selection[0].groupLead : StageManager.Selection[0]);
					stageIcon2.Stage.RemoveIcon(stageIcon2);
					droppedOnGroup.AddIconAt(stageIcon2, num, modifiedSiblingIndex);
					stageIcon2.SetInStageIndexOFGroupedIcons();
					stageIcon2.SetSymmetryMarkers();
				}
				else
				{
					List<StageIcon> list = new List<StageIcon>(StageManager.Selection);
					List<StageIcon> list2 = new List<StageIcon>();
					if (list[0].groupLead != null)
					{
						int count = list[0].groupLead.groupedIcons.Count;
						while (count-- > 0)
						{
							if (!list.Contains(list[0].groupLead.groupedIcons[count]))
							{
								list2.Add(list[0].groupLead.groupedIcons[count]);
							}
						}
						if (!list[0].groupLead.radioButton.Value)
						{
							list2.Insert(0, list[0].groupLead);
						}
					}
					StageIcon stageIcon3 = null;
					StageIcon stageIcon4 = null;
					if (list.Count == 1)
					{
						stageIcon3 = this;
						stageIcon4 = list2.Find((StageIcon a) => a.groupLead == a);
					}
					else
					{
						stageIcon3 = list.Find((StageIcon a) => a.groupLead == a);
						stageIcon4 = list2.Find((StageIcon a) => a.groupLead == a);
					}
					droppedOnGroup.MoveIconsInto(oldStageGroup, stageIcon3, list, stageIcon4, list2, num, modifiedSiblingIndex);
				}
			}
			else
			{
				Stage.RemoveIcon(this, changeParents: false);
				droppedOnGroup.AddIconAt(this, num, modifiedSiblingIndex);
				if (droppedOnGroup != oldStageGroup)
				{
					SetManualStageOffset(Stage.inverseStageIndex);
				}
			}
			oldStageGroup.UpdateInStageIndexes();
			droppedOnGroup.UpdateInStageIndexes();
		}
		if (oldStageGroup.Icons.Count == 0)
		{
			StageManager.Instance.SortIcons(instant: false);
		}
		else
		{
			StageManager.SetSeparationIndices();
		}
		oldStageGroup = null;
		base.transform.localScale = Vector3.one;
		GameEvents.StageManager.OnGUIStageSequenceModified.Fire();
	}

	public void SetIcon(Texture2D texture, int x, int y)
	{
		int num = texture.width / defaultIconSize;
		iconImage.uvRect = new Rect((float)(x % num) / (float)num, 1f - (float)(y % num + 1) / (float)num, 1f / (float)num, 1f / (float)num);
		iconImage.texture = texture;
	}

	public void SetIcon(DefaultIcons icon)
	{
		int num = defaultIconMap.width / defaultIconSize;
		SetIcon(defaultIconMap, (int)(icon - 1) % num, (int)(icon - 1) / num);
	}

	public void SetIcon(string customIconFilePath, int x, int y)
	{
		Texture2D texture = GameDatabase.Instance.GetTexture(customIconFilePath, asNormalMap: false);
		if (texture != null)
		{
			SetIcon(texture, x, y);
		}
		else
		{
			SetIcon(DefaultIcons.MYSTERY_PART);
		}
	}

	public void SetIconColor(Color color)
	{
		iconImage.color = color;
	}

	public void SetBackgroundColor(Color color)
	{
		backgroundImage.color = color;
	}

	public void ShowBorder(bool show)
	{
		if (groupLead != null && groupLead != this && !groupLead.expanded)
		{
			groupLead.borderImage.gameObject.SetActive(show);
		}
		else
		{
			borderImage.gameObject.SetActive(show);
		}
	}

	public void SetBorderColor(Color color)
	{
		borderImage.color = color;
	}

	public void Freeze()
	{
		frozen = true;
		StageManager.Instance.UnHoldIcon(this);
		base.transform.SetParent(StageManager.Instance.frozenGroup.transform, worldPositionStays: false);
		CanvasGroup.blocksRaycasts = false;
		base.gameObject.SetActive(value: true);
		if (groupLead != null)
		{
			groupLead.HoldGroupedIcons();
		}
		if (Stage != null)
		{
			Stage.RemoveIcon(this, changeParents: false);
		}
		radioButton.SetGroup(99999, pop: false);
		radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
		StageManager.Selection.Remove(this);
		ShowBorder(show: false);
	}

	public void Unfreeze()
	{
		frozen = false;
		StageManager.Instance.HoldIcon(this);
		CanvasGroup.blocksRaycasts = false;
	}

	public void BlinkBorder(float interval)
	{
		blinkInterval = interval;
		if (interval != blinkInterval)
		{
			if (blinkInterval != 0f)
			{
				CancelInvoke("Blink");
			}
			blinkInterval = interval;
			if (blinkInterval != 0f)
			{
				InvokeRepeating("Blink", 0f, interval);
			}
		}
	}

	public void Blink()
	{
		blinkBorder = !blinkBorder;
	}

	public void Highlight(bool highlightState, bool highlightReferencedPart)
	{
		if (!underDrag)
		{
			if (!mouseHover)
			{
				ShowBorder(highlightState);
			}
			if (highlightReferencedPart)
			{
				HighlightPart(highlightState);
			}
		}
	}

	public void HighlightPart(bool highlightState)
	{
		if (Part != null)
		{
			Part.SetHighlight(highlightState, recursive: false);
		}
	}

	public void SetInverseSequenceIndex(int inverseIndex, int inStageIndex, bool seqOverride = true)
	{
		if (protoIcon.Part == null)
		{
			return;
		}
		protoIcon.Part.inverseStage = inverseIndex;
		if (seqOverride)
		{
			protoIcon.Part.manualStageOffset = inverseIndex;
			if (protoIcon.Part.HasModuleImplementing<ModuleDecouple>() || protoIcon.Part.HasModuleImplementing<ModuleAnchoredDecoupler>() || protoIcon.Part.isEngine())
			{
				protoIcon.Part.separationIndex = inverseIndex;
				StageManager.SetSeparationIndices();
			}
		}
		if (groupLead != null && groupLead == this)
		{
			int i = 0;
			for (int count = groupedIcons.Count; i < count; i++)
			{
				groupedIcons[i].protoIcon.Part.inverseStage = inverseIndex;
				groupedIcons[i].textStar.gameObject.SetActive(value: false);
				if (seqOverride)
				{
					groupedIcons[i].protoIcon.Part.manualStageOffset = inverseIndex;
				}
			}
		}
		if ((bool)Stage)
		{
			SetInStageIndex(inStageIndex);
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			textStar.gameObject.SetActive(Part.manualStageOffset != -1 && Part.inverseStage != Part.defaultInverseStage);
		}
	}

	public void SetManualStageOffset(int inverseIndex)
	{
		if (protoIcon.Part == null)
		{
			return;
		}
		protoIcon.Part.manualStageOffset = inverseIndex;
		if (groupLead != null && groupLead == this)
		{
			int i = 0;
			for (int count = groupedIcons.Count; i < count; i++)
			{
				groupedIcons[i].protoIcon.Part.manualStageOffset = inverseIndex;
				groupedIcons[i].textStar.gameObject.SetActive(value: false);
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			textStar.gameObject.SetActive(Part.manualStageOffset != -1 && Part.inverseStage != Part.defaultInverseStage);
		}
	}

	public void SetInStageIndex(int index)
	{
		protoIcon.Part.inStageIndex = index;
	}

	public void SetInStageIndexOFGroupedIcons(bool setSiblingIndex = true)
	{
		StageGroup stageGroup = ((groupLead != null) ? groupLead.Stage : Stage);
		if (!(stageGroup != null))
		{
			return;
		}
		int siblingIndex = base.transform.GetSiblingIndex();
		int count = groupedIcons.Count;
		while (count-- > 0)
		{
			if (setSiblingIndex)
			{
				groupedIcons[count].transform.SetParent(stageGroup.IconsUIList, worldPositionStays: false);
				groupedIcons[count].transform.SetSiblingIndex(siblingIndex + 1);
			}
			groupedIcons[count].SetInStageIndex(InStageIndex);
		}
	}

	public void SortGroupedIcons()
	{
		StageGroup stageGroup = (groupLead ? groupLead.Stage : Stage);
		if (stageGroup != null)
		{
			int siblingIndex = base.transform.GetSiblingIndex();
			groupedIcons.Sort((StageIcon a, StageIcon b) => a.Part.parent.children.IndexOf(a.Part).CompareTo(b.Part.parent.children.IndexOf(b.Part)));
			int count = groupedIcons.Count;
			while (count-- > 0)
			{
				groupedIcons[count].transform.SetParent(stageGroup.IconsUIList, worldPositionStays: false);
				groupedIcons[count].transform.SetSiblingIndex(siblingIndex + 1);
				groupedIcons[count].SetInStageIndex(InStageIndex);
			}
		}
	}

	public void ResetStageIconSymmetryGroup()
	{
		if (!(groupLead != this))
		{
			base.transform.SetParent(Stage.IconsUIList, worldPositionStays: false);
			base.transform.SetSiblingIndex(SiblingIndex);
			int i = 0;
			for (int count = groupedIcons.Count; i < count; i++)
			{
				groupedIcons[i].transform.SetParent(Stage.IconsUIList, worldPositionStays: false);
				groupedIcons[i].transform.SetSiblingIndex(SiblingIndex + 1 + i);
			}
		}
	}

	public void Reset()
	{
		if (!(Part == null))
		{
			if (groupLead != null)
			{
				groupLead.ResetStageGroup();
			}
			else if (groupLead == null && Stage != null)
			{
				ResetStageGroup();
			}
		}
	}

	public void ResetStageGroup()
	{
		if (!(groupLead == this) && (!(groupLead == null) || !(Stage != null)))
		{
			return;
		}
		radioButton.SetGroup(99999, pop: false);
		radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
		StageManager.Selection.Remove(this);
		int inverseStage = Part.inverseStage;
		Part.inverseStage = Part.defaultInverseStage;
		if (inverseStage != Part.inverseStage)
		{
			GameEvents.onPartPriorityChanged.Fire(Part);
		}
		Part.manualStageOffset = Part.defaultInverseStage;
		textStar.gameObject.SetActive(value: false);
		if (!frozen)
		{
			if (Stage != null)
			{
				Stage.RemoveIcon(this);
			}
			StageManager.Instance.HoldIcon(this, removeIcon: false);
		}
		int i = 0;
		for (int num = groupedIcons.Count; i < num; i++)
		{
			if (groupedIcons[i] == null)
			{
				groupedIcons.RemoveAt(i--);
				num--;
				continue;
			}
			groupedIcons[i].gameObject.SetActive(value: true);
			groupedIcons[i].radioButton.SetGroup(99999, pop: false);
			groupedIcons[i].radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
			StageManager.Selection.Remove(groupedIcons[i]);
			if (!groupedIcons[i].frozen)
			{
				StageManager.Instance.HoldIcon(groupedIcons[i], removeIcon: false);
			}
			groupedIcons[i].Part.inverseStage = groupedIcons[i].Part.defaultInverseStage;
			groupedIcons[i].Part.manualStageOffset = groupedIcons[i].Part.defaultInverseStage;
			groupedIcons[i].textStar.gameObject.SetActive(value: false);
			groupedIcons[i].groupLead = null;
			groupedIcons[i].Stage = null;
			groupedIcons[i].grouped = false;
			groupedIcons[i].expanded = false;
		}
		groupedIcons.Clear();
		groupLead = null;
		Stage = null;
		grouped = false;
		expanded = false;
	}

	public bool ResetAvailable()
	{
		if (Part == null)
		{
			return false;
		}
		if (groupLead != null)
		{
			return groupLead.ResetStageGroupAvailable();
		}
		if (groupLead == null && Stage != null)
		{
			return ResetStageGroupAvailable();
		}
		return false;
	}

	public bool ResetStageGroupAvailable()
	{
		if (!(groupLead == this) && (!(groupLead == null) || !(Stage != null)))
		{
			return false;
		}
		bool result = false;
		if (Part.manualStageOffset != -1 && Part.inverseStage != Part.defaultInverseStage)
		{
			result = true;
		}
		int i = 0;
		for (int count = groupedIcons.Count; i < count; i++)
		{
			if (groupedIcons[i].Part.manualStageOffset != -1 && groupedIcons[i].Part.inverseStage != groupedIcons[i].Part.defaultInverseStage)
			{
				result = true;
			}
		}
		return result;
	}

	public void AddToGroup(StageIcon icon, bool setParent = true)
	{
		if (!groupedIcons.Contains(icon))
		{
			groupedIcons.Add(icon);
			icon.grouped = true;
			icon.groupLead = this;
			grouped = true;
			groupLead = this;
			textSymmetry.gameObject.SetActive(value: true);
			textSymmetry.text = (groupedIcons.Count + 1).ToString();
			textSymmetry.color = StageManager.Instance.stageIconNumberColor_lead;
			int inverseStage = icon.protoIcon.Part.inverseStage;
			icon.protoIcon.Part.inverseStage = Stage.inverseStageIndex;
			if (inverseStage != stage.inverseStageIndex)
			{
				GameEvents.onPartPriorityChanged.Fire(icon.protoIcon.Part);
			}
			icon.SetInStageIndex(InStageIndex);
			icon.protoIcon.Part.manualStageOffset = Stage.inverseStageIndex;
			icon.CanvasGroup.blocksRaycasts = true;
			if (setParent)
			{
				icon.transform.SetParent(Stage.IconsUIList, worldPositionStays: false);
			}
		}
	}

	public void RemoveFromGroup(StageIcon icon, bool resetGroupLeadIfempty = true)
	{
		if (!(groupLead == icon))
		{
			if (groupedIcons.Contains(icon))
			{
				groupedIcons.Remove(icon);
			}
			if (groupedIcons.Count == 0 && resetGroupLeadIfempty)
			{
				grouped = false;
				groupLead = null;
			}
			icon.grouped = false;
			icon.groupLead = null;
			textSymmetry.gameObject.SetActive(value: true);
			textSymmetry.text = (groupedIcons.Count + 1).ToString();
			textSymmetry.color = StageManager.Instance.stageIconNumberColor_symmetry;
		}
	}

	public bool RemoveFromGroupAndReshuffle(out StageGroup foundInGroup)
	{
		bool result = false;
		StageIcon stageIcon = null;
		StageIcon stageIcon2 = null;
		foundInGroup = null;
		if (groupLead == this && groupedIcons.Count > 0)
		{
			foundInGroup = Stage;
			stageIcon = groupedIcons[0];
			stageIcon.groupLead = null;
			foundInGroup.RemoveIcon(this);
			foundInGroup.AddIconAt(stageIcon, InStageIndex, SiblingIndex, setParent: false);
			List<StageIcon> list = new List<StageIcon>(groupedIcons);
			int i = 1;
			for (int count = list.Count; i < count; i++)
			{
				stageIcon2 = list[i];
				RemoveFromGroup(stageIcon2);
				stageIcon.AddToGroup(stageIcon2);
			}
			result = true;
		}
		else if (groupLead != null)
		{
			foundInGroup = groupLead.Stage;
			stageIcon = groupLead;
			groupLead.RemoveFromGroup(this);
			result = true;
		}
		if (stageIcon != null)
		{
			stageIcon.SetInStageIndexOFGroupedIcons(setSiblingIndex: false);
			stageIcon.SetSymmetryMarkers();
		}
		if (foundInGroup == null)
		{
			for (int j = 0; j < StageManager.Instance.Stages.Count; j++)
			{
				if (StageManager.Instance.Stages[j].Icons.Contains(this))
				{
					foundInGroup = StageManager.Instance.Stages[j];
					foundInGroup.RemoveIcon(this, changeParents: false);
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public void HoldGroupedIcons(bool alertStagingSequencer = true)
	{
		if (groupLead != this)
		{
			return;
		}
		int i = 0;
		for (int num = groupedIcons.Count; i < num; i++)
		{
			if (groupedIcons[i] == null)
			{
				groupedIcons.RemoveAt(i--);
				num--;
				continue;
			}
			groupedIcons[i].gameObject.SetActive(value: true);
			groupedIcons[i].showBorder = groupedIcons[i].selected;
			groupedIcons[i].grouped = false;
			groupedIcons[i].groupLead = null;
			groupedIcons[i].expanded = false;
			groupedIcons[i].radioButton.SetGroup(99999, pop: false);
			groupedIcons[i].radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
			StageManager.Selection.Remove(groupedIcons[i]);
			if (!groupedIcons[i].frozen)
			{
				StageManager.Instance.HoldIcon(groupedIcons[i], removeIcon: true, alertStagingSequencer);
			}
		}
		groupedIcons.Clear();
		grouped = false;
		groupLead = null;
		expanded = false;
		radioButton.SetGroup(99999, pop: false);
		radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
		StageManager.Selection.Remove(this);
		if (!frozen)
		{
			if (Stage != null)
			{
				Stage.RemoveIcon(this);
			}
			StageManager.Instance.HoldIcon(this, removeIcon: true, alertStagingSequencer);
		}
	}

	public void ExpandGroup()
	{
		if (groupLead != this)
		{
			return;
		}
		radioButton.SetGroup(0, pop: false);
		StageManager.Selection.Add(this);
		int i = 0;
		for (int num = groupedIcons.Count; i < num; i++)
		{
			if (groupedIcons[i] == null)
			{
				groupedIcons.RemoveAt(i--);
				num--;
				continue;
			}
			if (groupedIcons[i].IsInvoking("DisableAfterMove"))
			{
				groupedIcons[i].CancelInvoke();
			}
			groupedIcons[i].gameObject.SetActive(value: true);
			groupedIcons[i].showBorder = groupedIcons[i].selected;
			groupedIcons[i].expanded = true;
			groupedIcons[i].textSymmetry.gameObject.SetActive(value: true);
			groupedIcons[i].textSymmetry.text = (groupedIcons.Count - i).ToString();
			groupedIcons[i].textSymmetry.color = StageManager.Instance.stageIconNumberColor_symmetry;
			groupedIcons[i].radioButton.SetGroup(0, pop: false);
			groupedIcons[i].radioButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
			StageManager.Selection.Add(groupedIcons[i]);
		}
		expanded = true;
	}

	public void ExpandGroupInUIOnly()
	{
		radioButton.SetGroup(0, pop: false);
		expanded = true;
		int i = 0;
		for (int count = groupedIcons.Count; i < count; i++)
		{
			groupedIcons[i].gameObject.SetActive(value: true);
			groupedIcons[i].expanded = true;
			groupedIcons[i].textSymmetry.gameObject.SetActive(value: true);
			groupedIcons[i].textSymmetry.text = (groupedIcons.Count - i).ToString();
			groupedIcons[i].textSymmetry.color = StageManager.Instance.stageIconNumberColor_symmetry;
			groupedIcons[i].radioButton.SetGroup(0, pop: false);
		}
	}

	public void CollapseGroup()
	{
		if (groupLead != this)
		{
			return;
		}
		expanded = false;
		radioButton.SetGroup(99999, pop: false);
		radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
		StageManager.Selection.Remove(this);
		int i = 0;
		for (int num = groupedIcons.Count; i < num; i++)
		{
			if (groupedIcons[i] == null)
			{
				groupedIcons.RemoveAt(i--);
				num--;
				continue;
			}
			groupedIcons[i].gameObject.SetActive(value: false);
			groupedIcons[i].expanded = false;
			groupedIcons[i].radioButton.SetGroup(99999, pop: false);
			groupedIcons[i].radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
			StageManager.Selection.Remove(groupedIcons[i]);
		}
	}

	public void DisableGroup(StageIcon iconToDisable)
	{
		if (iconToDisable.groupLead == iconToDisable)
		{
			StageGroup stageGroup = iconToDisable.Stage;
			int num = iconToDisable.InStageIndex;
			if (num < 0 || num > stageGroup.Icons.Count - 1)
			{
				num = Mathf.Clamp(num, 0, stageGroup.Icons.Count - 1);
			}
			stageGroup.RemoveIcon(iconToDisable, changeParents: false);
			for (int i = 0; i < iconToDisable.groupedIcons.Count; i++)
			{
				if (iconToDisable.groupedIcons[i] == null)
				{
					Debug.LogError("[StageIcon] DisableGroup: Null icon in grouped icons.");
					iconToDisable.groupedIcons.RemoveAt(i--);
				}
				else if (i == 0)
				{
					stageGroup.AddIconAt(iconToDisable.groupedIcons[0], num, num);
					iconToDisable.groupedIcons[0].grouped = false;
					iconToDisable.groupedIcons[0].groupLead = null;
				}
				else
				{
					iconToDisable.groupedIcons[0].AddToGroup(iconToDisable.groupedIcons[i], setParent: false);
				}
			}
		}
		else
		{
			iconToDisable.groupLead.RemoveFromGroup(iconToDisable);
		}
		iconToDisable.radioButton.SetGroup(99999, pop: false);
		iconToDisable.radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null, popButtonsInGroup: false);
		iconToDisable.gameObject.SetActive(value: false);
		iconToDisable.grouped = false;
		iconToDisable.groupLead = null;
		StageManager.Selection.Remove(iconToDisable);
	}

	public void ConsolidateMembers()
	{
		if (groupLead != this)
		{
			groupLead = null;
			grouped = false;
			return;
		}
		int i = 0;
		for (int count = groupedIcons.Count; i < count; i++)
		{
			if (groupedIcons[i].groupLead != this)
			{
				groupedIcons.Remove(groupedIcons[i]);
				count = groupedIcons.Count;
			}
		}
		if (groupedIcons.Count == 0)
		{
			groupLead = null;
			grouped = false;
		}
	}

	public void SetSymmetryMarkers()
	{
		if (groupLead != this)
		{
			if (isSymmetryCounterPart)
			{
				textSymmetry.gameObject.SetActive(value: true);
				textSymmetry.text = 1.ToString();
				textSymmetry.color = StageManager.Instance.stageIconNumberColor_lead;
			}
			return;
		}
		if (groupLead == null)
		{
			textSymmetry.gameObject.SetActive(value: false);
			return;
		}
		textSymmetry.gameObject.SetActive(value: true);
		textSymmetry.text = (groupedIcons.Count + 1).ToString();
		textSymmetry.color = StageManager.Instance.stageIconNumberColor_lead;
		int i = 0;
		for (int count = groupedIcons.Count; i < count; i++)
		{
			groupedIcons[i].textSymmetry.gameObject.SetActive(value: true);
			groupedIcons[i].textSymmetry.text = (groupedIcons.Count - i).ToString();
			groupedIcons[i].textSymmetry.color = StageManager.Instance.stageIconNumberColor_symmetry;
		}
	}

	public void SetSymmetryText(bool active, string number = "")
	{
		if (active)
		{
			textSymmetry.gameObject.SetActive(value: true);
			textSymmetry.text = number;
		}
		else
		{
			textSymmetry.gameObject.SetActive(value: false);
		}
	}

	public StageIconInfoBox DisplayInfo()
	{
		int num = -1;
		for (int i = 0; i < maxInfoBoxes; i++)
		{
			if (!(infoBoxes[i] == null))
			{
				if (!infoBoxes[i].expanded)
				{
					num = i;
					break;
				}
				continue;
			}
			StageIconInfoBox stageIconInfoBox = UnityEngine.Object.Instantiate(stageIconInfoBoxPrefab);
			stageIconInfoBox.transform.SetParent(listAnchorInfoBoxes, worldPositionStays: false);
			infoBoxes[i] = stageIconInfoBox;
			num = i;
			break;
		}
		if (num == -1)
		{
			return null;
		}
		infoBoxes[num].Expand();
		if (groupLead != null)
		{
			groupLead.ExpandGroupInUIOnly();
		}
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -0.5f);
		return infoBoxes[num];
	}

	public void RemoveInfo(StageIconInfoBox iBox)
	{
		if (iBox == null)
		{
			return;
		}
		int num = Array.IndexOf(infoBoxes, iBox);
		if (num != -1 && num < maxInfoBoxes)
		{
			for (int i = num + 1; i < maxInfoBoxes; i++)
			{
				infoBoxes[i - 1] = infoBoxes[i];
				infoBoxes[i] = iBox;
			}
			iBox.Collapse();
		}
	}

	public void ClearInfoBoxes()
	{
		int i = 0;
		for (int num = infoBoxes.Length; i < num; i++)
		{
			if (infoBoxes[i] != null)
			{
				infoBoxes[i].Collapse();
			}
		}
	}

	public StageIconInfoBox ForceDisplayInfo()
	{
		bool flag = false;
		for (int i = 0; i < maxInfoBoxes; i++)
		{
			if (!(infoBoxes[i] != null) || !infoBoxes[i].expanded)
			{
				flag = true;
				break;
			}
		}
		if (!flag && infoBoxes[maxInfoBoxes - 1] != null)
		{
			infoBoxes[maxInfoBoxes - 1].Collapse();
		}
		return DisplayInfo();
	}
}
