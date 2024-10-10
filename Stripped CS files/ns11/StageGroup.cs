using System;
using System.Collections;
using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

[Serializable]
public class StageGroup : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	public RectTransform iconsUiList;

	[SerializeField]
	public TextMeshProUGUI uiStageIndex;

	[SerializeField]
	public Button addButton;

	[SerializeField]
	public Button deleteButton;

	[SerializeField]
	public UIDragPanel dragHandler;

	[SerializeField]
	public LayoutElement InfoPanelLayout;

	[SerializeField]
	public float InfoPanelWidth = 100f;

	[SerializeField]
	public float InfoPanelSlidePeriod = 0.3f;

	[SerializeField]
	public LayoutElement FooterLayout;

	[SerializeField]
	public LayoutElement DeltaVHeadingObject;

	[SerializeField]
	public TextMeshProUGUI DeltaVHeadingText;

	[SerializeField]
	public Color DeltaVHeadingColor;

	public Image DeltaVHeadingImage;

	[SerializeField]
	public StageGroupInfoItem infoItemPrefab;

	[SerializeField]
	public bool deltaVHeadingEnabled = true;

	[SerializeField]
	public bool infoPanelEnabled = true;

	[SerializeField]
	public bool infoPanelStockDisplayEnabled = true;

	[SerializeField]
	public List<StageIcon> icons = new List<StageIcon>();

	public int defaultStage = -1;

	public int inverseStageIndex;

	public bool manualOverride;

	public RectTransform rectTransform;

	public int insertAtInverseStageIndex = -1;

	public int insertAtSiblingIndex = -1;

	public int lastInversetageIndex = -1;

	public ResizingLayoutElement lastSpawnedResizingElement;

	public bool beginDrag = true;

	public bool infoPanelShown;

	public bool infoPanelSliding;

	public VesselDeltaV simulation;

	public DeltaVAppValues dvAppValues;

	public DeltaVStageInfo stage;

	public List<StageGroupInfoItem> stageGroupInfos;

	public int stageGroupInfosCount;

	public static string cacheAutoLOC_180095;

	public RectTransform IconsUIList => iconsUiList;

	public bool DeltaVHeadingEnabled => deltaVHeadingEnabled;

	public bool InfoPanelEnabled => infoPanelEnabled;

	public bool InfoPanelStockDisplayEnabled => infoPanelStockDisplayEnabled;

	public List<StageIcon> Icons => icons;

	public RectTransform RectTransform => rectTransform;

	public bool IsBeingDragged { get; set; }

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		addButton.onClick.AddListener(AddStageAfter);
		deleteButton.onClick.AddListener(Delete);
		dragHandler.onBeginDrag.AddListener(OnBeginDrag);
		dragHandler.onDrag.AddListener(OnDrag);
		dragHandler.beforeEndDrag.AddListener(OnEndDrag);
		bool active = RectTransformUtility.RectangleContainsScreenPoint(StageManager.Instance.hoverHandlerPlusMinus.transform as RectTransform, Input.mousePosition, UIMasterController.Instance.uiCamera);
		addButton.gameObject.SetActive(active);
		deleteButton.gameObject.SetActive(active);
		StageManager.Instance.hoverHandlerPlusMinus.onPointerEnter.AddListener(Hover);
		StageManager.Instance.hoverHandlerPlusMinus.onPointerExit.AddListener(HoverOut);
		if (HighLogic.LoadedSceneIsEditor)
		{
			InfoPanelWidth = GameSettings.STAGE_GROUP_INFO_WIDTH_EDITOR;
		}
		else
		{
			InfoPanelWidth = GameSettings.STAGE_GROUP_INFO_WIDTH_FLIGHT;
		}
		GameEvents.onDeltaVCalcsCompleted.Add(DeltaVCalcsCompleted);
		GameEvents.onDeltaVAppAtmosphereChanged.Add(DeltaVAppAtmosphereChanged);
		GameEvents.onDeltaVAppInfoItemsChanged.Add(DeltaVCalcsCompleted);
		GameEvents.onEditorStarted.Add(EditorStarted);
		InfoPanelLayout.preferredWidth = 0f;
		InfoPanelLayout.gameObject.SetActive(value: false);
		infoPanelShown = false;
		infoPanelSliding = false;
		DeltaVHeadingImage = DeltaVHeadingObject.GetComponent<Image>();
		if (DeltaVHeadingImage != null)
		{
			DeltaVHeadingImage.color = DeltaVHeadingColor;
		}
		GameEvents.onGameSceneSwitchRequested.Add(OnLeavingScene);
	}

	public void OnDestroy()
	{
		GameEvents.onDeltaVCalcsCompleted.Remove(DeltaVCalcsCompleted);
		GameEvents.onEditorStarted.Remove(EditorStarted);
		GameEvents.onGameSceneSwitchRequested.Remove(OnLeavingScene);
		if (!(StageManager.Instance == null))
		{
			StageManager.Instance.hoverHandlerPlusMinus.onPointerEnter.RemoveListener(Hover);
			StageManager.Instance.hoverHandlerPlusMinus.onPointerExit.RemoveListener(HoverOut);
		}
	}

	public void OnLeavingScene(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		GameEvents.onEditorStarted.Remove(EditorStarted);
	}

	public void Hover(PointerEventData eventData)
	{
		addButton.gameObject.SetActive(value: true);
		deleteButton.gameObject.SetActive(value: true);
	}

	public void HoverOut(PointerEventData eventData)
	{
		addButton.gameObject.SetActive(value: false);
		deleteButton.gameObject.SetActive(value: false);
	}

	public void SetUiStageIndex(int index)
	{
		uiStageIndex.text = index.ToString();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		beginDrag = true;
		IsBeingDragged = true;
		insertAtInverseStageIndex = -1;
		if (StageManager.Instance.Visible)
		{
			TweeningController.Instance.SpawnResizingLayoutElement(StageManager.Instance.layoutGroup, inverseStageIndex, inverseStageIndex, rectTransform.sizeDelta.y, 0f, 0.15f, destroyOnCompletion: true, addElementOnTopOfList: false);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		lastInversetageIndex = insertAtInverseStageIndex;
		insertAtSiblingIndex = -1;
		insertAtInverseStageIndex = StageManager.Instance.GetStageGroupInsertionIndex(eventData, out insertAtSiblingIndex, beginDrag);
		beginDrag = false;
		if (lastInversetageIndex != insertAtInverseStageIndex)
		{
			if (lastSpawnedResizingElement != null)
			{
				lastSpawnedResizingElement.ReverseResizing(destroyOnCompletion: true, delegate
				{
				});
			}
			if (StageManager.Instance.Visible)
			{
				lastSpawnedResizingElement = TweeningController.Instance.SpawnResizingLayoutElement(StageManager.Instance.layoutGroup, insertAtInverseStageIndex, insertAtSiblingIndex, 0f, 40f, 0.15f, destroyOnCompletion: false, addElementOnTopOfList: true);
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
		int insertAt = insertAtInverseStageIndex;
		if (inverseStageIndex < insertAtInverseStageIndex)
		{
			insertAt--;
		}
		StageGroup stageToUpdateA = null;
		StageGroup stageToUpdateB = null;
		if (insertAt != inverseStageIndex)
		{
			stageToUpdateA = this;
			if (insertAt < StageManager.StageCount)
			{
				stageToUpdateB = StageManager.Instance.Stages[insertAt];
			}
		}
		bool flag = false;
		if (StageManager.Instance.Visible)
		{
			flag = TweeningController.Instance.TweenIntoList(GetComponent<RectTransform>(), StageManager.Instance.layoutGroup, insertAt, insertAtSiblingIndex, 0.15f, delegate
			{
				OnEndDragCompleted(stageToUpdateA, stageToUpdateB, insertAt, insertAtSiblingIndex);
			});
		}
		if (!flag)
		{
			OnEndDragCompleted(stageToUpdateA, stageToUpdateB, insertAt, insertAtSiblingIndex);
		}
	}

	public void OnEndDragCompleted(StageGroup stageToUpdateA, StageGroup stageToUpdateB, int insertAt, int modifiedSiblingIndex)
	{
		StageManager.Instance.RemoveStage(this);
		StageManager.Instance.InsertStageAt(this, insertAt, modifiedSiblingIndex);
		base.transform.localScale = Vector3.one;
		if (stageToUpdateA != null)
		{
			stageToUpdateA.SetManualStageOffset();
		}
		if (stageToUpdateB != null)
		{
			stageToUpdateB.SetManualStageOffset();
		}
		StageManager.SetSeparationIndices();
		GameEvents.StageManager.OnGUIStageSequenceModified.Fire();
		IsBeingDragged = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!InfoPanelEnabled)
		{
			return;
		}
		ToggleInfoPanel();
		if (StageManager.Instance != null)
		{
			if (infoPanelShown)
			{
				StageManager.Instance.usageDV.stageHide++;
			}
			else
			{
				StageManager.Instance.usageDV.stageShow++;
			}
		}
	}

	public void ToggleInfoPanel()
	{
		ToggleInfoPanel(!infoPanelShown);
	}

	public void ToggleInfoPanel(bool showPanel)
	{
		if (!(InfoPanelLayout == null) && infoPanelShown != showPanel)
		{
			if (!InfoPanelLayout.gameObject.activeSelf)
			{
				InfoPanelLayout.preferredWidth = 0f;
				InfoPanelLayout.gameObject.SetActive(value: true);
			}
			if (infoPanelSliding)
			{
				iTween.Stop(base.gameObject);
			}
			infoPanelShown = showPanel;
			float preferredWidth = InfoPanelLayout.preferredWidth;
			float num = (infoPanelShown ? InfoPanelWidth : 0f);
			infoPanelSliding = true;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", preferredWidth, "to", num, "easetype", iTween.EaseType.easeOutSine, "time", InfoPanelSlidePeriod, "onupdate", "OnInfoPanelSlideUpdate", "oncomplete", "OnInfoPanelSlideComplete", "ignoretimescale", true));
		}
	}

	public void OnInfoPanelSlideUpdate(float width)
	{
		InfoPanelLayout.preferredWidth = width;
	}

	public void OnInfoPanelSlideComplete()
	{
		infoPanelSliding = false;
		if (InfoPanelLayout.preferredWidth == 0f)
		{
			InfoPanelLayout.gameObject.SetActive(value: false);
		}
		if (StageManager.Instance != null)
		{
			StartCoroutine(UpdateStageManagerHover());
		}
	}

	public IEnumerator UpdateStageManagerHover()
	{
		yield return null;
		StageManager.Instance.UpdateHoverArea();
	}

	public void EnableDeltaVHeading()
	{
		EnableDeltaVHeading(enable: true);
	}

	public void DisableDeltaVHeading()
	{
		EnableDeltaVHeading(enable: false);
	}

	public void EnableDeltaVHeading(bool enable)
	{
		deltaVHeadingEnabled = enable;
		if (stage == null)
		{
			DeltaVHeadingObject.gameObject.SetActive(value: false);
			return;
		}
		float situationDeltaV = stage.GetSituationDeltaV(DeltaVGlobals.DeltaVAppValues.situation);
		DeltaVHeadingObject.gameObject.SetActive(enable && situationDeltaV > 0f);
	}

	public void EnableInfoPanel()
	{
		EnableInfoPanel(enable: true);
	}

	public void DisableInfoPanel()
	{
		EnableInfoPanel(enable: false);
	}

	public void EnableInfoPanel(bool enable)
	{
		infoPanelEnabled = enable;
		if (infoPanelShown)
		{
			ToggleInfoPanel(showPanel: false);
		}
	}

	public void EnableStockInfoPanelDisplays()
	{
		EnableStockInfoPanelDisplays(enable: true);
	}

	public void DisableStockInfoPanelDisplays()
	{
		EnableStockInfoPanelDisplays(enable: false);
	}

	public void EnableStockInfoPanelDisplays(bool enable)
	{
		infoPanelStockDisplayEnabled = enable;
	}

	public void EditorStarted()
	{
		ToggleInfoPanel(showPanel: false);
	}

	public void DeltaVAppAtmosphereChanged(DeltaVSituationOptions situation)
	{
		DeltaVCalcsCompleted();
	}

	public void DeltaVCalcsCompleted()
	{
		if (DeltaVHeadingObject == null)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			simulation = EditorLogic.fetch.ship.vesselDeltaV;
		}
		else
		{
			if (!HighLogic.LoadedSceneIsFlight)
			{
				simulation = null;
				return;
			}
			simulation = FlightGlobals.ActiveVessel.VesselDeltaV;
		}
		dvAppValues = DeltaVGlobals.DeltaVAppValues;
		stage = simulation.GetStage(Convert.ToInt32(uiStageIndex.text));
		if (stage == null || DeltaVGlobals.DeltaVAppValues == null)
		{
			return;
		}
		if (DeltaVHeadingEnabled)
		{
			SetDeltaVHeading_OnDeltaVCalcsCompleted();
		}
		if (!InfoPanelEnabled || !InfoPanelStockDisplayEnabled)
		{
			return;
		}
		if (stageGroupInfos == null)
		{
			stageGroupInfos = new List<StageGroupInfoItem>();
		}
		int num = 0;
		for (int i = 0; i < dvAppValues.infoLines.Count; i++)
		{
			DeltaVAppValues.InfoLine infoLine = dvAppValues.infoLines[i];
			if (infoLine.Enabled)
			{
				if (num >= stageGroupInfos.Count)
				{
					stageGroupInfos.Add(SpawnInfoObject(infoLine, stage));
				}
				else
				{
					UpdateInfoObject(stageGroupInfos[num], infoLine, stage);
				}
				num++;
			}
		}
		if (stageGroupInfos.Count > num)
		{
			int count = stageGroupInfos.Count;
			while (count-- > num)
			{
				stageGroupInfos[count].gameObject.DestroyGameObject();
				stageGroupInfos.RemoveAt(count);
			}
		}
		stageGroupInfosCount = stageGroupInfos.Count;
	}

	public void Update()
	{
		if (DeltaVHeadingEnabled)
		{
			SetDeltaVHeading_OnUpdate();
		}
		if (!infoPanelShown || !InfoPanelStockDisplayEnabled)
		{
			return;
		}
		if (stage == null && simulation != null)
		{
			stage = simulation.GetStage(Convert.ToInt32(uiStageIndex.text));
		}
		if (stage != null)
		{
			for (int i = 0; i < stageGroupInfosCount; i++)
			{
				stageGroupInfos[i].OnUpdate(stage, dvAppValues.situation);
			}
		}
	}

	public virtual void SetDeltaVHeading_OnDeltaVCalcsCompleted()
	{
		float situationDeltaV = stage.GetSituationDeltaV(DeltaVGlobals.DeltaVAppValues.situation);
		DeltaVHeadingObject.gameObject.SetActive(situationDeltaV > 0f);
		if (situationDeltaV > 0f)
		{
			DeltaVHeadingText.text = situationDeltaV.ToString("0");
			if (DeltaVHeadingText.text.Length < 5)
			{
				DeltaVHeadingText.text += cacheAutoLOC_180095;
			}
		}
	}

	public virtual void SetDeltaVHeading_OnUpdate()
	{
		if (!HighLogic.LoadedSceneIsEditor && stage != null)
		{
			DeltaVHeadingText.text = stage.GetSituationDeltaV(DeltaVGlobals.DeltaVAppValues.situation).ToString("0");
			if (DeltaVHeadingText.text.Length < 5)
			{
				DeltaVHeadingText.text += cacheAutoLOC_180095;
			}
		}
	}

	public void UpdateInfoObject(StageGroupInfoItem info, DeltaVAppValues.InfoLine dvInfo, DeltaVStageInfo stage)
	{
		info.Setup(dvInfo, InfoPanelWidth);
		info.UpdateValue(stage, dvAppValues.situation);
	}

	public StageGroupInfoItem SpawnInfoObject(DeltaVAppValues.InfoLine dvInfo, DeltaVStageInfo stage)
	{
		StageGroupInfoItem stageGroupInfoItem = UnityEngine.Object.Instantiate(infoItemPrefab);
		stageGroupInfoItem.gameObject.transform.SetParent(InfoPanelLayout.gameObject.transform);
		stageGroupInfoItem.transform.SetLocalPositionZ();
		stageGroupInfoItem.transform.localScale = Vector3.one;
		stageGroupInfoItem.Setup(dvInfo, InfoPanelWidth);
		stageGroupInfoItem.UpdateValue(stage, dvAppValues.situation);
		return stageGroupInfoItem;
	}

	public void AddIcons(StageIcon[] iconsToAdd)
	{
		int i = 0;
		for (int num = iconsToAdd.Length; i < num; i++)
		{
			AddIcon(iconsToAdd[i]);
		}
	}

	public void AddIcon(StageIcon icon, bool setParent = true)
	{
		if (!icons.Contains(icon))
		{
			icons.Add(icon);
			icon.Stage = this;
			if (setParent)
			{
				icon.transform.SetParent(IconsUIList, worldPositionStays: false);
				icon.transform.localScale = Vector3.one;
			}
			icon.CanvasGroup.blocksRaycasts = true;
			icon.SetInverseSequenceIndex(inverseStageIndex, icons.Count - 1);
			icon.SetInStageIndex(icons.Count - 1);
			icon.SetSymmetryText(active: false);
			icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, -0.5f);
		}
	}

	public void AddIconAt(StageIcon icon, int index, int forcedSiblingIndex, bool setParent = true)
	{
		if (!icons.Contains(icon))
		{
			icons.Insert(index, icon);
			icon.gameObject.SetActive(value: true);
			icon.Stage = this;
			if (setParent)
			{
				icon.transform.SetParent(IconsUIList, worldPositionStays: false);
				if (forcedSiblingIndex != -1)
				{
					icon.transform.SetSiblingIndex(forcedSiblingIndex);
				}
				else
				{
					icon.transform.SetSiblingIndex(index);
				}
				icon.transform.localScale = Vector3.one;
			}
			icon.CanvasGroup.blocksRaycasts = true;
			icon.SetInverseSequenceIndex(inverseStageIndex, index);
			icon.SetInStageIndex(index);
			icon.SetSymmetryText(active: false);
			icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, -0.5f);
		}
		else
		{
			Debug.LogWarning("[StageGroup] AddIconAt(" + index + ") failed (already in group) ->", icon.gameObject);
		}
	}

	public void RemoveIcon(StageIcon icon, bool changeParents = true)
	{
		if (icons.Contains(icon))
		{
			icons.Remove(icon);
			icon.Stage = null;
			if (changeParents)
			{
				icon.transform.SetParent(StageManager.Instance.malarkyGroup, worldPositionStays: false);
			}
		}
		else
		{
			Debug.LogWarning("[StageGroup] RemoveIcon() failed (not in group) ->", icon.gameObject);
		}
	}

	public void Delete()
	{
		if (icons.Count == 0)
		{
			StageManager.Instance.DecrementCurrentStage();
			int num = inverseStageIndex;
			StageManager.Instance.DeleteStage(this, StageManager.Instance.Visible);
			StageManager.Instance.SetManualStageOffset(num);
			StageManager.SetSeparationIndices();
			GameEvents.StageManager.OnGUIStageRemoved.Fire(num);
		}
	}

	public void AddStageAfter()
	{
		StageManager.Instance.IncrementCurrentStage();
		int data = inverseStageIndex + 1;
		StageManager.Instance.AddStageAt(inverseStageIndex + 1);
		StageManager.Instance.SetManualStageOffset(inverseStageIndex + 1);
		StageManager.SetSeparationIndices();
		GameEvents.StageManager.OnGUIStageAdded.Fire(data);
	}

	public void UpdateInStageIndexes()
	{
		int i = 0;
		for (int count = icons.Count; i < count; i++)
		{
			icons[i].SetInStageIndex(i);
			icons[i].SetInStageIndexOFGroupedIcons();
		}
	}

	public void SetInverseStageIndex(int index)
	{
		inverseStageIndex = index;
	}

	public void SetPartIndices(bool seqOverride = true)
	{
		int i = 0;
		for (int count = icons.Count; i < count; i++)
		{
			icons[i].SetInverseSequenceIndex(inverseStageIndex, i, seqOverride);
		}
	}

	public void SetManualStageOffset()
	{
		int i = 0;
		for (int count = icons.Count; i < count; i++)
		{
			icons[i].SetManualStageOffset(inverseStageIndex);
		}
	}

	public void SetSiblingIndexes()
	{
		int num = 0;
		int i = 0;
		for (int count = icons.Count; i < count; i++)
		{
			StageIcon stageIcon = icons[i];
			stageIcon.SiblingIndex = num++;
			if (stageIcon.groupLead != null && stageIcon.groupLead == stageIcon)
			{
				int j = 0;
				for (int count2 = stageIcon.groupedIcons.Count; j < count2; j++)
				{
					stageIcon.groupedIcons[j].SiblingIndex = num++;
				}
			}
		}
	}

	public void Reset()
	{
		List<StageIcon> list = new List<StageIcon>(icons);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list[i].Reset();
		}
		icons.Clear();
	}

	public bool ResetAvailable()
	{
		int num = 0;
		int count = icons.Count;
		while (true)
		{
			if (num < count)
			{
				if (icons[num].ResetAvailable())
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

	public int GetStageGroupIndex(PointerEventData eventData, out StageIcon stageIcon, out int modifiedSiblingIndex)
	{
		stageIcon = null;
		modifiedSiblingIndex = -1;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(IconsUIList, eventData.position, UIMasterController.Instance.uiCamera, out var worldPoint);
		if (worldPoint.y > IconsUIList.transform.position.y)
		{
			modifiedSiblingIndex = 0;
			return 0;
		}
		if (worldPoint.y < IconsUIList.transform.position.y - IconsUIList.sizeDelta.y)
		{
			modifiedSiblingIndex = IconsUIList.transform.childCount;
			return icons.Count;
		}
		Transform transform = null;
		int num = -1;
		int childCount = IconsUIList.childCount;
		while (childCount-- > 0)
		{
			Transform child = IconsUIList.transform.GetChild(childCount);
			if (child.gameObject.activeSelf)
			{
				if (!(child.position.y < worldPoint.y))
				{
					break;
				}
				transform = child;
				num = childCount;
			}
		}
		if (transform != null)
		{
			for (int i = num; i < IconsUIList.childCount; i++)
			{
				stageIcon = IconsUIList.GetChild(i).GetComponent<StageIcon>();
				if (!(stageIcon != null))
				{
					continue;
				}
				if (stageIcon.groupLead != null)
				{
					if (stageIcon.groupLead.transform.parent != IconsUIList)
					{
						int j = 0;
						for (int count = stageIcon.groupLead.groupedIcons.Count; j < count; j++)
						{
							if (stageIcon.groupLead.groupedIcons[j].transform.parent == IconsUIList)
							{
								modifiedSiblingIndex = stageIcon.groupLead.groupedIcons[j].transform.GetSiblingIndex();
								break;
							}
						}
					}
					else
					{
						modifiedSiblingIndex = stageIcon.groupLead.transform.GetSiblingIndex();
					}
					return stageIcon.groupLead.InStageIndex;
				}
				modifiedSiblingIndex = stageIcon.transform.GetSiblingIndex();
				return stageIcon.InStageIndex;
			}
		}
		modifiedSiblingIndex = IconsUIList.transform.childCount;
		return icons.Count;
	}

	public StageIcon FindSymmetryGroupleader(StageIcon icon)
	{
		int num = 0;
		int count = icons.Count;
		while (true)
		{
			if (num < count)
			{
				if (icons[num].IconGroupingRule == icon.IconGroupingRule)
				{
					StackIconGrouping iconGroupingRule = icons[num].IconGroupingRule;
					if (iconGroupingRule == StackIconGrouping.SYM_COUNTERPARTS && icons[num].ProtoIcon.Part.isSymmetryCounterPart(icon.ProtoIcon.Part))
					{
						break;
					}
				}
				num++;
				continue;
			}
			return null;
		}
		return icons[num];
	}

	public void MoveAllIconsInto(StageGroup moveInto)
	{
		int count = icons.Count;
		while (count-- > 0)
		{
			StageIcon stageIcon = icons[count];
			if (stageIcon.groupLead != null && stageIcon.groupLead == stageIcon)
			{
				List<StageIcon> list = new List<StageIcon>(stageIcon.groupedIcons);
				list.Insert(0, stageIcon);
				moveInto.MoveIconsInto(stageIcon.groupLead.Stage, stageIcon, list, null, null, 0, 0);
			}
			else if (stageIcon.isSymmetryCounterPart)
			{
				moveInto.MoveIconsInto(stageIcon.Stage, stageIcon, new List<StageIcon>(new StageIcon[1] { stageIcon }), null, null, 0, 0);
			}
			else
			{
				RemoveIcon(stageIcon);
				moveInto.AddIconAt(stageIcon, 0, 0);
			}
		}
	}

	public void MoveIconsInto(StageGroup oldStageGroup, StageIcon selectedLeader, List<StageIcon> selectedIcons, StageIcon unSelectedLeader, List<StageIcon> unSelectedIcons, int insertAt, int modifiedSiblingIndex)
	{
		StageIcon stageIcon = ((selectedLeader != null) ? selectedLeader : selectedIcons[0].Part.parent.children.Find((Part a) => selectedIcons.Contains(a.stackIcon.StageIcon)).stackIcon.StageIcon);
		StageIcon stageIcon2 = FindSymmetryGroupleader(stageIcon);
		int inStageIndex = stageIcon.InStageIndex;
		if (selectedLeader == null)
		{
			if (stageIcon.groupLead != null)
			{
				stageIcon.groupLead.RemoveFromGroup(stageIcon);
			}
		}
		else if (selectedLeader.Stage != null)
		{
			selectedLeader.Stage.RemoveIcon(selectedLeader, changeParents: false);
		}
		else
		{
			selectedLeader.groupLead.RemoveFromGroup(selectedLeader);
		}
		if (stageIcon2 == null)
		{
			AddIconAt(stageIcon, insertAt, modifiedSiblingIndex);
			int i = 0;
			for (int count = selectedIcons.Count; i < count; i++)
			{
				StageIcon stageIcon3 = selectedIcons[i];
				if (!(stageIcon3 == stageIcon))
				{
					if (stageIcon3.groupLead != null)
					{
						stageIcon3.groupLead.RemoveFromGroup(stageIcon3);
					}
					stageIcon.AddToGroup(stageIcon3);
				}
			}
		}
		else if (stageIcon2.Part.parent.children.IndexOf(stageIcon2.Part) < stageIcon.Part.parent.children.IndexOf(stageIcon.Part))
		{
			List<StageIcon> list = new List<StageIcon>();
			list.Add(stageIcon);
			int j = 0;
			for (int count2 = selectedIcons.Count; j < count2; j++)
			{
				if (selectedIcons[j] != stageIcon)
				{
					list.Add(selectedIcons[j]);
				}
			}
			stageIcon.groupedIcons.Clear();
			int k = 0;
			for (int count3 = list.Count; k < count3; k++)
			{
				stageIcon2.AddToGroup(list[k]);
			}
		}
		else
		{
			AddIconAt(stageIcon, insertAt, modifiedSiblingIndex);
			int l = 0;
			for (int count4 = selectedIcons.Count; l < count4; l++)
			{
				StageIcon stageIcon4 = selectedIcons[l];
				if (!(stageIcon4 == stageIcon))
				{
					stageIcon4.groupLead.RemoveFromGroup(stageIcon4);
					stageIcon.AddToGroup(stageIcon4);
				}
			}
			List<StageIcon> list2 = new List<StageIcon>();
			list2.Add(stageIcon2);
			list2.AddRange(stageIcon2.groupedIcons);
			stageIcon2.groupedIcons.Clear();
			stageIcon2.Stage.RemoveIcon(stageIcon2, changeParents: false);
			int m = 0;
			for (int count5 = list2.Count; m < count5; m++)
			{
				stageIcon.AddToGroup(list2[m]);
			}
			stageIcon2 = stageIcon;
		}
		if (unSelectedLeader != null)
		{
			unSelectedLeader.ConsolidateMembers();
			unSelectedLeader.ResetStageIconSymmetryGroup();
			unSelectedLeader.SetSymmetryMarkers();
			if (!unSelectedLeader.isDisplayingInfoInGroup)
			{
				unSelectedLeader.radioButton.SetGroup(99999, pop: false);
				unSelectedLeader.CollapseGroup();
			}
		}
		else if (unSelectedIcons != null && unSelectedIcons.Count > 0)
		{
			StageIcon stageIcon5 = unSelectedIcons[0].Part.parent.children.Find((Part a) => unSelectedIcons.Contains(a.stackIcon.StageIcon)).stackIcon.StageIcon;
			oldStageGroup.AddIconAt(stageIcon5, inStageIndex, stageIcon5.transform.GetSiblingIndex(), setParent: false);
			int n = 0;
			for (int count6 = unSelectedIcons.Count; n < count6; n++)
			{
				StageIcon stageIcon6 = unSelectedIcons[n];
				stageIcon6.groupLead.RemoveFromGroup(stageIcon6, resetGroupLeadIfempty: false);
				if (!(stageIcon6 == stageIcon5))
				{
					stageIcon5.AddToGroup(stageIcon6, setParent: false);
				}
			}
			stageIcon5.ConsolidateMembers();
			stageIcon5.SetSymmetryMarkers();
			if (!stageIcon5.isDisplayingInfoInGroup)
			{
				stageIcon5.radioButton.SetGroup(99999, pop: false);
				stageIcon5.CollapseGroup();
			}
			stageIcon5.SetInStageIndexOFGroupedIcons();
		}
		StageIcon stageIcon7 = ((stageIcon2 != null) ? stageIcon2 : stageIcon);
		stageIcon7.ConsolidateMembers();
		if (stageIcon7.groupLead == null)
		{
			stageIcon7.radioButton.SetGroup(99999, pop: false);
		}
		else
		{
			stageIcon7.ExpandGroupInUIOnly();
		}
		stageIcon7.SortGroupedIcons();
		stageIcon7.SetSymmetryMarkers();
		if (this != oldStageGroup)
		{
			stageIcon7.SetManualStageOffset(stageIcon7.Stage.inverseStageIndex);
		}
	}

	public void FixGroupUIState()
	{
		HashSet<StageIcon> hashSet = new HashSet<StageIcon>();
		for (int i = 0; i < IconsUIList.transform.childCount; i++)
		{
			hashSet.Add(IconsUIList.transform.GetChild(i).GetComponent<StageIcon>());
		}
		int j = 0;
		for (int num = icons.Count; j < num; j++)
		{
			if (!hashSet.Contains(icons[j]) || (icons[j].groupLead != null && icons[j].groupLead != icons[j]))
			{
				icons[j].Stage = null;
				icons.RemoveAt(j--);
				num--;
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_180095 = Localizer.Format("#autoLOC_180095");
	}
}
