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

public class StageManager : MonoBehaviour
{
	public class DVUsageStats
	{
		public int stageHide;

		public int stageShow;

		public int allStagesShow;

		public int allStagesHide;

		public bool UsageFound
		{
			get
			{
				if (stageHide <= 0 && stageShow <= 0 && allStagesShow <= 0)
				{
					return allStagesHide > 0;
				}
				return true;
			}
		}

		public void Reset()
		{
			stageHide = 0;
			stageShow = 0;
			allStagesShow = 0;
			allStagesHide = 0;
		}
	}

	public Button resetButton;

	[SerializeField]
	public RectTransform resetButtonRT;

	public StageIcon stageIconPrefab;

	public StageGroup stageGroupPrefab;

	public ScrollRect scrollRect;

	public RectTransform layoutGroup;

	public RectTransform frozenGroup;

	public RectTransform heldGroup;

	public RectTransform malarkyGroup;

	public RectTransform iconCacheGroup;

	public AudioClip nextStageClip;

	public AudioClip cannotSeparateClip;

	public PointerEnterExitHandler hoverHandlerPlusMinus;

	[SerializeField]
	public VerticalLayoutGroup mainListAnchor;

	public LayoutElement deltaVTotalSection;

	public Button deltaVTotalButton;

	public TextMeshProUGUI deltaVTotalText;

	[SerializeField]
	public float plusMinusButtonWidth = 25f;

	[SerializeField]
	public float minimumPlusMinusButtonWidth = 75f;

	[NonSerialized]
	public Color stageIconNumberColor_lead = XKCDColors.KSPBadassGreen;

	[NonSerialized]
	public Color stageIconNumberColor_symmetry = XKCDColors.KSPNotSoGoodOrange;

	[SerializeField]
	public List<StageGroup> stages = new List<StageGroup>();

	[SerializeField]
	public int _currentStage;

	public List<StageIcon> iconsInHold = new List<StageIcon>();

	public List<StageIcon> selection = new List<StageIcon>();

	public List<StageIcon> iconCache = new List<StageIcon>();

	public bool canSeparate;

	public const int radioGroupExpanded = 0;

	public const int radioGroupCollapsed = 99999;

	public const float tweeningSpeed = 0.15f;

	public bool holdEditorSelectedIcons;

	public static bool EnableDeleteEmptyStages = true;

	public bool rebuildIndexes;

	public Vector3 pointerWorldPos_InsertionIndex;

	public Vector3 pointerWorldPos_UnderCursor;

	public Coroutine sortRoutine;

	public int coroutineFramesToWait;

	[SerializeField]
	public bool deltaVTotalEnabled;

	[SerializeField]
	public bool deltaVHeadingsEnabled;

	[SerializeField]
	public bool infoPanelsEnabled;

	public VesselDeltaV simulation;

	public static string cacheAutoLOC_180095;

	public DVUsageStats usageDV;

	public static StageManager Instance { get; set; }

	public List<StageGroup> Stages => stages;

	public static int LastStage
	{
		get
		{
			if (Instance.stages.Count - 1 < 0)
			{
				return 0;
			}
			return Instance.stages.Count - 1;
		}
	}

	public static int StageCount => Instance.stages.Count;

	public static int CurrentStage => Instance._currentStage;

	public static List<StageIcon> Selection
	{
		get
		{
			if (!(Instance != null))
			{
				return new List<StageIcon>();
			}
			return Instance.selection;
		}
	}

	public static bool CanSeparate => Instance.canSeparate;

	public bool Visible { get; set; }

	public bool DeltaVTotalEnabled => deltaVTotalEnabled;

	public bool DeltaVHeadingsEnabled => deltaVHeadingsEnabled;

	public bool InfoPanelsEnabled => infoPanelsEnabled;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("StageManager: Only one instance of this class may exist per scene. Destroying potential usurper.", base.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (!HighLogic.LoadedSceneIsFlight)
		{
			resetButton.onClick.AddListener(Reset);
			resetButtonRT.gameObject.SetActive(value: true);
		}
		else
		{
			resetButtonRT.gameObject.SetActive(value: false);
		}
		Instance = this;
		Visible = true;
		usageDV = new DVUsageStats();
		GameEvents.StageManager.SortIcons.Add(SortIcons);
		GameEvents.onVesselClearStaging.Add(OnVesselClearStaging);
		GameEvents.onVesselResumeStaging.Add(OnVesselResumeStaging);
		GameEvents.onEditorStarted.Add(OnEditorStarted);
		GameEvents.onEditorPodPicked.Add(OnEditorPodSelected);
		GameEvents.onEditorPartPicked.Add(OnEditorPartPicked);
		GameEvents.onEditorPartPlaced.Add(OnEditorPartPlaced);
		GameEvents.onEditorPodDeleted.Add(OnEditorPodDeleted);
		GameEvents.onEditorPartDeleted.Add(OnEditorPartDeleted);
		GameEvents.onEditorRestoreState.Add(OnEditorRestoreState);
		GameEvents.StageManager.OnPartUpdateStageability.Add(OnPartUpdateStageability);
		GameEvents.onVesselSwitching.Add(OnVesselSwitching);
		GameEvents.onEditorLoad.Add(OnEditorLoad);
		GameEvents.onDeltaVCalcsCompleted.Add(DeltaVCalcsCompleted);
		GameEvents.onDeltaVAppAtmosphereChanged.Add(DeltaVAppAtmosphereChanged);
		if (deltaVTotalButton != null && deltaVTotalSection != null)
		{
			deltaVTotalButton.onClick.AddListener(DeltaVButtonClicked);
			deltaVTotalSection.gameObject.SetActive(value: false);
		}
		GameEvents.StageManager.OnGUIStageSequenceModified.Add(OnGUIStageSequenceModified);
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
		MapView.OnEnterMapView = (Callback)Delegate.Combine(MapView.OnEnterMapView, new Callback(OnEnterMapView));
		MapView.OnExitMapView = (Callback)Delegate.Combine(MapView.OnExitMapView, new Callback(OnExitMapView));
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		for (int i = 0; i < iconCache.Count; i++)
		{
			if (iconCache[i] != null)
			{
				UnityEngine.Object.Destroy(iconCache[i].gameObject);
			}
		}
		GameEvents.StageManager.SortIcons.Remove(SortIcons);
		GameEvents.onVesselClearStaging.Remove(OnVesselClearStaging);
		GameEvents.onVesselResumeStaging.Remove(OnVesselResumeStaging);
		GameEvents.onEditorStarted.Remove(OnEditorStarted);
		GameEvents.onEditorPodPicked.Remove(OnEditorPodSelected);
		GameEvents.onEditorPartPicked.Remove(OnEditorPartPicked);
		GameEvents.onEditorPartPlaced.Remove(OnEditorPartPlaced);
		GameEvents.onEditorPodDeleted.Remove(OnEditorPodDeleted);
		GameEvents.onEditorPartDeleted.Remove(OnEditorPartDeleted);
		GameEvents.onEditorRestoreState.Remove(OnEditorRestoreState);
		GameEvents.StageManager.OnPartUpdateStageability.Remove(OnPartUpdateStageability);
		GameEvents.onVesselSwitching.Remove(OnVesselSwitching);
		GameEvents.onEditorLoad.Remove(OnEditorLoad);
		GameEvents.onDeltaVCalcsCompleted.Remove(DeltaVCalcsCompleted);
		GameEvents.onDeltaVAppAtmosphereChanged.Remove(DeltaVAppAtmosphereChanged);
		if (deltaVTotalButton != null && deltaVTotalSection != null)
		{
			deltaVTotalButton.onClick.RemoveListener(DeltaVButtonClicked);
		}
		GameEvents.StageManager.OnGUIStageSequenceModified.Remove(OnGUIStageSequenceModified);
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		MapView.OnEnterMapView = (Callback)Delegate.Remove(MapView.OnEnterMapView, new Callback(OnEnterMapView));
		MapView.OnExitMapView = (Callback)Delegate.Remove(MapView.OnExitMapView, new Callback(OnExitMapView));
	}

	public void OnEnable()
	{
		sortRoutine = null;
	}

	public void OnEnterMapView()
	{
	}

	public void OnExitMapView()
	{
	}

	public void OnVesselClearStaging(Vessel vessel)
	{
		SortIcons(instant: false);
	}

	public void OnVesselResumeStaging(Vessel vessel)
	{
		if (!(vessel != FlightGlobals.ActiveVessel))
		{
			ResumeFlight(vessel.currentStage);
		}
	}

	public void OnPartUpdateStageability(Part data)
	{
		rebuildIndexes = true;
		data.inverseStage = Mathf.Clamp(data.inverseStage, 0, int.MaxValue);
		SortIcons(instant: true);
	}

	public void OnEditorStarted()
	{
		SortIcons(instant: false, deleteManualStages: true);
		DeltaVCalcsCompleted();
	}

	public void OnEditorPodSelected(Part rootPart)
	{
		SetSeparationIndices(rootPart, -1);
		SortIcons(instant: false);
	}

	public void OnEditorPodDeleted()
	{
		SortIcons(instant: false);
	}

	public void OnEditorPartPicked(Part part)
	{
		List<StageIcon> list = AddPartsWithStageiconsToListRecursive(part);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			HoldIcon(list[i]);
		}
	}

	public void OnEditorPartPlaced(Part part)
	{
		SortIcons(instant: true, part, switchingVessels: false, deleteManualStages: false);
	}

	public void OnEditorPartDeleted(Part part)
	{
		SortIcons(instant: false);
	}

	public void OnEditorRestoreState()
	{
		holdEditorSelectedIcons = true;
		SortIcons(instant: false);
	}

	public void OnVesselSwitching(Vessel from, Vessel to)
	{
		if (from != null)
		{
			from.currentStage = _currentStage;
		}
	}

	public void OnEditorLoad(ShipConstruct constr, CraftBrowserDialog.LoadType loadType)
	{
		rebuildIndexes = true;
		DeltaVCalcsCompleted();
	}

	public void UpdateHoverArea()
	{
		float a = 0f;
		if (Stages == null || Stages.Count < 1)
		{
			a = 50f;
		}
		for (int i = 0; i < Stages.Count; i++)
		{
			a = Mathf.Max(a, Stages[i].RectTransform.sizeDelta.x);
		}
		a = Mathf.Max(a, minimumPlusMinusButtonWidth);
		(hoverHandlerPlusMinus.gameObject.transform as RectTransform).sizeDelta = new Vector2(a + plusMinusButtonWidth, (hoverHandlerPlusMinus.gameObject.transform as RectTransform).sizeDelta.y);
	}

	public void AddNewStageGroupsIfNeeded(Part selectedPart)
	{
		if (selectedPart != null && ((selectedPart.partInfo.preferredStage == PreferredInitialStage.LAST && stages.Count > 0) || (selectedPart.partInfo.preferredStage == PreferredInitialStage.PARACHUTESTAGE && stages.Count > 0)))
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < stages.Count; i++)
		{
			StageGroup stageGroup = stages[i];
			for (int j = 0; j < stageGroup.Icons.Count; j++)
			{
				num = Mathf.Max(num, stageGroup.Icons[j].InverseStage);
			}
		}
		int k = 0;
		for (int count = iconsInHold.Count; k < count; k++)
		{
			num = Mathf.Max(num, iconsInHold[k].InverseStage);
		}
		int num2 = 0;
		if (num > 0)
		{
			num2 = num;
		}
		if (num2 < stages.Count)
		{
			return;
		}
		int num3 = num2 - stages.Count + 1;
		if (num3 > 0)
		{
			for (int l = 0; l < num3; l++)
			{
				StageGroup stageGroup2 = UnityEngine.Object.Instantiate(stageGroupPrefab);
				stageGroup2.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
				stageGroup2.defaultStage = stages.Count;
				stageGroup2.inverseStageIndex = stages.Count;
				stages.Add(stageGroup2);
			}
		}
	}

	public void AddHeldIconsToStages(Part selectedPart, bool switchingVessels)
	{
		if (iconsInHold.Count > 0)
		{
			List<List<StageIcon>> list = new List<List<StageIcon>>();
			List<StageIcon> list2 = new List<StageIcon>();
			int num;
			for (num = iconsInHold.Count - 1; num >= 0; num = iconsInHold.Count - 1)
			{
				StageIcon stageIcon = iconsInHold[0];
				stageIcon.Part.inverseStage = Mathf.Clamp(stageIcon.Part.inverseStage, 0, int.MaxValue);
				List<StageIcon> list3 = new List<StageIcon>();
				int num2 = 0;
				for (int num3 = num; num3 >= 0; num3--)
				{
					StageIcon stageIcon2 = iconsInHold[num3];
					if (stageIcon2 != stageIcon && stageIcon2.InverseStage == stageIcon.InverseStage && stageIcon2.ProtoIcon.Part.isSymmetryCounterPart(stageIcon.ProtoIcon.Part))
					{
						list3.Add(stageIcon2);
						num2++;
					}
				}
				if (num2 > 0)
				{
					list3.Add(stageIcon);
					list.Add(list3);
					for (int num4 = num; num4 >= 0; num4--)
					{
						if (list3.Contains(iconsInHold[num4]))
						{
							iconsInHold.RemoveAt(num4);
						}
					}
				}
				else
				{
					list2.Add(stageIcon);
					iconsInHold.Remove(stageIcon);
				}
			}
			num = list.Count;
			List<StageIcon> iteratedIcons;
			for (int i = 0; i < num; i++)
			{
				iteratedIcons = list[i];
				StageIcon stageIcon3 = ((iteratedIcons[0].groupLead != null) ? iteratedIcons[0].groupLead : ((selectedPart != null && iteratedIcons.Contains(selectedPart.stackIcon.StageIcon)) ? selectedPart.stackIcon.StageIcon : ((!(iteratedIcons[0].Part.parent != null)) ? iteratedIcons[0] : iteratedIcons[0].Part.parent.children.Find((Part a) => iteratedIcons.Contains(a.stackIcon.StageIcon)).stackIcon.StageIcon)));
				if (HighLogic.LoadedSceneIsFlight)
				{
					stages[Mathf.Min(stageIcon3.InverseStage, _currentStage)].AddIcon(stageIcon3);
				}
				else if (selectedPart != null)
				{
					if (!selectedPart.editorFirstTimePlaced)
					{
						switch (selectedPart.partInfo.preferredStage)
						{
						default:
							stages[stageIcon3.InverseStage].AddIcon(stageIcon3);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.FIRST:
							stages[0].AddIcon(stageIcon3);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.LAST:
							stages[stages.Count - 1].AddIcon(stageIcon3);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.PARACHUTESTAGE:
						{
							int parachuteStageIndex = GetParachuteStageIndex(stageIcon3.InverseStage);
							stages[parachuteStageIndex].AddIcon(stageIcon3);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						}
						case PreferredInitialStage.DEFAULT:
							stages[stageIcon3.InverseStage].AddIcon(stageIcon3);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						}
					}
					else
					{
						stages[stageIcon3.InverseStage].AddIcon(stageIcon3);
					}
				}
				else
				{
					stages[stageIcon3.InverseStage].AddIcon(stageIcon3);
				}
				int j = 0;
				for (int count = iteratedIcons.Count; j < count; j++)
				{
					if (iteratedIcons[j] != stageIcon3)
					{
						stageIcon3.AddToGroup(iteratedIcons[j]);
					}
				}
				stageIcon3.CollapseGroup();
			}
			num = list2.Count;
			for (int k = 0; k < num; k++)
			{
				StageIcon stageIcon4 = null;
				if (list2[k].InverseStage < stages.Count)
				{
					stageIcon4 = stages[list2[k].InverseStage].FindSymmetryGroupleader(list2[k]);
				}
				if (stageIcon4 != null)
				{
					stageIcon4.AddToGroup(list2[k]);
					stageIcon4.CollapseGroup();
				}
				else if (HighLogic.LoadedSceneIsFlight)
				{
					stages[Mathf.Min(list2[k].InverseStage, _currentStage)].AddIcon(list2[k]);
				}
				else if (selectedPart != null)
				{
					if (!selectedPart.editorFirstTimePlaced)
					{
						switch (selectedPart.partInfo.preferredStage)
						{
						default:
							stages[list2[k].InverseStage].AddIcon(list2[k]);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.FIRST:
							stages[0].AddIcon(list2[k]);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.LAST:
							stages[stages.Count - 1].AddIcon(list2[k]);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						case PreferredInitialStage.PARACHUTESTAGE:
						{
							int parachuteStageIndex2 = GetParachuteStageIndex(list2[k].InverseStage);
							stages[parachuteStageIndex2].AddIcon(list2[k]);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						}
						case PreferredInitialStage.DEFAULT:
							stages[list2[k].InverseStage].AddIcon(list2[k]);
							if (!selectedPart.editorFirstTimePlaced)
							{
								selectedPart.editorFirstTimePlaced = true;
							}
							break;
						}
					}
					else if (list2[k].InverseStage >= stages.Count)
					{
						stages[stages.Count - 1].AddIcon(list2[k]);
					}
					else
					{
						stages[list2[k].InverseStage].AddIcon(list2[k]);
					}
				}
				else if (list2[k].InverseStage >= stages.Count)
				{
					stages[stages.Count - 1].AddIcon(list2[k]);
				}
				else
				{
					stages[list2[k].InverseStage].AddIcon(list2[k]);
				}
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			int l = 0;
			for (int num5 = stages.Count; l < num5; l++)
			{
				if (stages[l].Icons.Count == 0 && (stages[l].defaultStage != -1 || stages[l].inverseStageIndex >= _currentStage))
				{
					DeleteStage(stages[l--], Visible && !switchingVessels);
					num5--;
				}
			}
		}
		int count2 = stages.Count;
		while (count2-- > 0)
		{
			StageGroup stageGroup = stages[count2];
			if (stageGroup.gameObject.activeInHierarchy)
			{
				UpdateStageGroup(stageGroup, count2, seqOverride: false);
			}
		}
		iconsInHold.Clear();
	}

	public int GetParachuteStageIndex(int currentIndex)
	{
		if (currentIndex == 0)
		{
			if (stages[currentIndex].Icons.Count <= 0)
			{
				return currentIndex;
			}
			ModuleParachute moduleParachute = null;
			ModuleDecouple moduleDecouple = null;
			for (int i = 0; i < stages[currentIndex].Icons.Count; i++)
			{
				moduleParachute = stages[currentIndex].Icons[i].Part.FindModuleImplementing<ModuleParachute>();
				if (moduleParachute == null)
				{
					moduleDecouple = stages[currentIndex].Icons[i].Part.FindModuleImplementing<ModuleDecouple>();
				}
				if (moduleParachute != null || moduleDecouple != null)
				{
					return currentIndex;
				}
			}
			if (moduleParachute == null)
			{
				StageGroup stageGroup = UnityEngine.Object.Instantiate(stageGroupPrefab);
				stageGroup.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
				stageGroup.defaultStage = stages.Count;
				stageGroup.inverseStageIndex = stages.Count;
				stages.Add(stageGroup);
				for (int num = stages.Count - 2; num >= 0; num--)
				{
					stages[num].MoveAllIconsInto(stages[num + 1]);
				}
				return currentIndex;
			}
		}
		else
		{
			if (stages.Count > 0 && currentIndex >= stages.Count)
			{
				currentIndex = stages.Count - 1;
			}
			if (stages[currentIndex].Icons.Count > 0)
			{
				ModuleDecouple moduleDecouple2 = null;
				for (int j = 0; j < stages[currentIndex].Icons.Count; j++)
				{
					moduleDecouple2 = stages[currentIndex].Icons[j].Part.FindModuleImplementing<ModuleDecouple>();
					if (moduleDecouple2 != null)
					{
						return currentIndex;
					}
				}
				if (moduleDecouple2 == null && currentIndex > 0)
				{
					currentIndex--;
					GetParachuteStageIndex(currentIndex);
				}
			}
		}
		return currentIndex;
	}

	public int GetStageGroupInsertionIndex(PointerEventData eventData, out int modifiedSiblingIndex, bool freshEventData = true)
	{
		modifiedSiblingIndex = -1;
		if (freshEventData)
		{
			RectTransformUtility.ScreenPointToWorldPointInRectangle(layoutGroup, eventData.position, UIMasterController.Instance.uiCamera, out pointerWorldPos_InsertionIndex);
		}
		else
		{
			pointerWorldPos_InsertionIndex += (Vector3)eventData.delta;
		}
		if (pointerWorldPos_InsertionIndex.y > layoutGroup.transform.position.y + layoutGroup.sizeDelta.y * UIMasterController.Instance.uiScale)
		{
			modifiedSiblingIndex = 0;
			return 0;
		}
		if (pointerWorldPos_InsertionIndex.y < layoutGroup.transform.position.y)
		{
			modifiedSiblingIndex = layoutGroup.transform.childCount;
			return stages.Count;
		}
		Transform transform = null;
		int num = -1;
		int count = stages.Count;
		while (count-- > 0)
		{
			if (!stages[count].IsBeingDragged)
			{
				Transform transform2 = stages[count].transform;
				if (!(transform2.position.y < pointerWorldPos_InsertionIndex.y))
				{
					break;
				}
				transform = transform2;
				num = count;
			}
		}
		if (transform != null)
		{
			for (int i = num; i < stages.Count; i++)
			{
				StageGroup stageGroup = stages[i];
				if (stageGroup != null)
				{
					StageGroup nonDraggedStageGroupAboveGroup = GetNonDraggedStageGroupAboveGroup(stageGroup);
					if (nonDraggedStageGroupAboveGroup != null && nonDraggedStageGroupAboveGroup.transform.position.y - nonDraggedStageGroupAboveGroup.RectTransform.sizeDelta.y * UIMasterController.Instance.uiScale / 2f < pointerWorldPos_InsertionIndex.y)
					{
						modifiedSiblingIndex = nonDraggedStageGroupAboveGroup.transform.GetSiblingIndex();
						return nonDraggedStageGroupAboveGroup.inverseStageIndex;
					}
					modifiedSiblingIndex = stageGroup.transform.GetSiblingIndex();
					return stageGroup.inverseStageIndex;
				}
			}
		}
		StageGroup nonDraggedStageGroupAboveGroup2 = GetNonDraggedStageGroupAboveGroup(stages[stages.Count - 1], includeGroup: true);
		if (nonDraggedStageGroupAboveGroup2 != null && nonDraggedStageGroupAboveGroup2.transform.position.y - nonDraggedStageGroupAboveGroup2.RectTransform.sizeDelta.y * UIMasterController.Instance.uiScale / 2f < pointerWorldPos_InsertionIndex.y)
		{
			modifiedSiblingIndex = nonDraggedStageGroupAboveGroup2.transform.GetSiblingIndex();
			return nonDraggedStageGroupAboveGroup2.inverseStageIndex;
		}
		modifiedSiblingIndex = layoutGroup.transform.childCount;
		return stages.Count;
	}

	public StageGroup GetNonDraggedStageGroupAboveGroup(StageGroup group, bool includeGroup = false)
	{
		int num = 0;
		num = ((!includeGroup) ? (group.inverseStageIndex - 1) : group.inverseStageIndex);
		while (true)
		{
			if (num >= 0)
			{
				if (!stages[num].IsBeingDragged)
				{
					break;
				}
				num--;
				continue;
			}
			return null;
		}
		return stages[num];
	}

	public int GetStageGroupUnderCursor(PointerEventData eventData, out StageGroup stageGroup, bool freshEventData = true)
	{
		stageGroup = null;
		if (freshEventData)
		{
			RectTransformUtility.ScreenPointToWorldPointInRectangle(layoutGroup, eventData.position, UIMasterController.Instance.uiCamera, out pointerWorldPos_UnderCursor);
		}
		else
		{
			pointerWorldPos_UnderCursor += (Vector3)eventData.delta;
		}
		if (pointerWorldPos_UnderCursor.y > layoutGroup.transform.position.y + layoutGroup.sizeDelta.y * UIMasterController.Instance.uiScale)
		{
			return -1;
		}
		if (pointerWorldPos_UnderCursor.y < layoutGroup.transform.position.y)
		{
			return -2;
		}
		int count = stages.Count;
		do
		{
			if (count-- <= 0)
			{
				if (stages.Count > 0)
				{
					stageGroup = stages[stages.Count - 1];
					return stages[stages.Count - 1].inverseStageIndex;
				}
				return -3;
			}
		}
		while (!(stages[count].transform.position.y >= pointerWorldPos_UnderCursor.y) || stages[count].IsBeingDragged);
		stageGroup = stages[count];
		return stages[count].inverseStageIndex;
	}

	public void HoldIcon(StageIcon stageIcon, bool removeIcon = true, bool alertStagingSequencer = true)
	{
		if (!iconsInHold.Contains(stageIcon))
		{
			if (removeIcon)
			{
				RemoveIcon(stageIcon, destroyIcon: false, removeSelection: false, alertStagingSequencer);
			}
			iconsInHold.Add(stageIcon);
			stageIcon.transform.SetParent(heldGroup.transform, worldPositionStays: false);
		}
	}

	public void UnHoldIcon(StageIcon stageIcon, bool changeParents = false)
	{
		if (iconsInHold.Remove(stageIcon) && changeParents)
		{
			stageIcon.transform.SetParent(malarkyGroup.transform, worldPositionStays: false);
		}
	}

	public static StageIcon CreateIcon(ProtoStageIcon protoIcon, bool alertStagingSequencer = true)
	{
		if (Instance == null)
		{
			return null;
		}
		StageIcon newStageIcon = Instance.GetNewStageIcon();
		newStageIcon.Setup(protoIcon);
		Instance.HoldIcon(newStageIcon, removeIcon: true, alertStagingSequencer);
		return newStageIcon;
	}

	public StageIcon GetNewStageIcon()
	{
		int num = iconCache.Count - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (!(iconCache[num] == null))
				{
					break;
				}
				iconCache.RemoveAt(num);
				num--;
				continue;
			}
			return UnityEngine.Object.Instantiate(Instance.stageIconPrefab);
		}
		StageIcon stageIcon = iconCache[num];
		stageIcon.gameObject.SetActive(value: true);
		stageIcon.transform.localScale = Vector3.one;
		iconCache.RemoveAt(num);
		stageIcon.Stage = null;
		stageIcon.groupLead = null;
		return stageIcon;
	}

	public static void RemoveIcon(StageIcon stageIcon, bool destroyIcon = true, bool removeSelection = true, bool alertStagingSequencer = true)
	{
		if (Instance == null || stageIcon == null)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			StageGroup foundInGroup = null;
			stageIcon.RemoveFromGroupAndReshuffle(out foundInGroup);
			if (foundInGroup != null)
			{
				foundInGroup.UpdateInStageIndexes();
			}
		}
		else
		{
			if (stageIcon.groupLead != null)
			{
				stageIcon.groupLead.HoldGroupedIcons(alertStagingSequencer);
			}
			for (int i = 0; i < Instance.stages.Count; i++)
			{
				if (Instance.stages[i].Icons.Contains(stageIcon))
				{
					Instance.stages[i].RemoveIcon(stageIcon);
				}
			}
		}
		if (Instance.iconsInHold.Contains(stageIcon))
		{
			Instance.iconsInHold.Remove(stageIcon);
		}
		if (removeSelection)
		{
			Selection.Remove(stageIcon);
		}
		if (destroyIcon)
		{
			stageIcon.gameObject.SetActive(value: false);
			Instance.iconCache.Add(stageIcon);
			stageIcon.transform.SetParent(Instance.iconCacheGroup);
			stageIcon.groupLead = null;
			stageIcon.groupedIcons.Clear();
		}
		if (HighLogic.LoadedSceneIsFlight && alertStagingSequencer)
		{
			Instance.SortIcons(instant: false);
		}
	}

	public static void DisableIcon(StageIcon iconToDisable)
	{
		if (!(iconToDisable == null))
		{
			if (iconToDisable.groupLead != null)
			{
				iconToDisable.groupLead.DisableGroup(iconToDisable);
			}
			else if (iconToDisable.Stage != null)
			{
				iconToDisable.Stage.RemoveIcon(iconToDisable, changeParents: false);
				iconToDisable.gameObject.SetActive(value: false);
				Selection.Remove(iconToDisable);
			}
		}
	}

	public void AddStageAt(int index)
	{
		StageGroup stageGroup = UnityEngine.Object.Instantiate(stageGroupPrefab);
		stageGroup.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
		stageGroup.transform.SetSiblingIndex(index);
		stages.Insert(index, stageGroup);
		UpdateStageGroups();
	}

	public void InsertStageAt(StageGroup stageGroup, int index, int forcedSiblingIndex = -1)
	{
		if (!stages.Contains(stageGroup))
		{
			stageGroup.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
			stageGroup.transform.localPosition = new Vector3(stageGroup.transform.localPosition.x, stageGroup.transform.localPosition.y, 0f);
			int num = -1;
			num = ((index < stages.Count) ? stages[index].transform.GetSiblingIndex() : ((forcedSiblingIndex != -1) ? layoutGroup.transform.childCount : 0));
			stageGroup.transform.SetSiblingIndex(num);
			stages.Insert(index, stageGroup);
			UpdateStageGroups();
		}
	}

	public void DeleteStage(StageGroup stageToDelete, bool tweenOut = false)
	{
		stages.Remove(stageToDelete);
		Vector3 position = stageToDelete.transform.position;
		Vector2 endPos = Vector3.zero;
		int inverseStageIndex = stageToDelete.inverseStageIndex;
		if (tweenOut && Visible)
		{
			TweeningController.Instance.SpawnResizingLayoutElement(layoutGroup, stageToDelete.inverseStageIndex, stageToDelete.transform.GetSiblingIndex(), stageToDelete.RectTransform.sizeDelta.y, 0f, 0.3f, destroyOnCompletion: true, addElementOnTopOfList: false);
			if (HighLogic.LoadedScene == GameScenes.FLIGHT)
			{
				endPos = stageToDelete.transform.position + new Vector3(-250f, 0f, 0f);
			}
			else if (HighLogic.LoadedScene == GameScenes.EDITOR)
			{
				endPos = stageToDelete.transform.position + new Vector3(250f, 0f, 0f);
			}
		}
		UnityEngine.Object.Destroy(stageToDelete.gameObject);
		UpdateStageGroups();
		if (tweenOut && Visible)
		{
			StageGroup newStage = UnityEngine.Object.Instantiate(stageGroupPrefab);
			newStage.transform.position = position;
			newStage.SetUiStageIndex(inverseStageIndex);
			newStage.transform.localScale = new Vector3(GameSettings.UI_SCALE_STAGINGSTACK, GameSettings.UI_SCALE_STAGINGSTACK, 1f);
			TweeningController.Instance.Tween(newStage.RectTransform, newStage.transform.position, endPos, 0.3f, TweeningController.TweeningFunction.EASEINBACK, delegate
			{
				UnityEngine.Object.Destroy(newStage.gameObject);
			});
		}
	}

	public void DeleteEmptyStages()
	{
		DeleteEmptyStages(deleteManualStages: false);
	}

	public void DeleteEmptyStages(bool deleteManualStages)
	{
		bool flag = false;
		int count = stages.Count;
		while (count-- > 0)
		{
			StageGroup stageGroup = stages[count];
			if (stageGroup.Icons.Count == 0 && (stageGroup.defaultStage != -1 || deleteManualStages))
			{
				stages.Remove(stageGroup);
				DecrementCurrentStage();
				UnityEngine.Object.Destroy(stageGroup.gameObject);
				flag = true;
			}
		}
		if (flag)
		{
			UpdateStageGroups(seqOverride: false);
			SetSeparationIndices(EditorLogic.RootPart, -1);
		}
	}

	public void RemoveStage(StageGroup stageGroup)
	{
		stages.Remove(stageGroup);
		UpdateStageGroups();
	}

	public static int GetStageCount(List<Part> parts)
	{
		int num = 0;
		int count = parts.Count;
		while (count-- > 0)
		{
			num = Mathf.Max(num, parts[count].inverseStage);
		}
		return num;
	}

	public static void SetStageCount(List<Part> ship)
	{
		int num = 0;
		int count = ship.Count;
		while (count-- > 0)
		{
			int inverseStage = ship[count].inverseStage;
			if (inverseStage > num)
			{
				num = inverseStage;
			}
		}
	}

	public static List<Part> FindPartsWithModuleSeparatingBeforeOtherPartsWithModule<Before, After>(List<Part> parts) where Before : PartModule where After : PartModule
	{
		List<Part> list = new List<Part>();
		List<Part> list2 = new List<Part>(parts);
		list2.Sort((Part a, Part b) => a.separationIndex.CompareTo(b.separationIndex));
		Part part = null;
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		List<Part> list3 = new List<Part>();
		int i = 0;
		for (int count = list2.Count; i < count; i++)
		{
			part = list2[i];
			if (num != part.separationIndex)
			{
				if (flag)
				{
					break;
				}
				if (flag2)
				{
					list.AddRange(list3);
					list3.Clear();
				}
				num = part.separationIndex;
				flag2 = false;
			}
			if (part.FindModuleImplementing<Before>() != null)
			{
				flag = true;
			}
			if (!flag && part.FindModuleImplementing<After>() != null)
			{
				flag2 = true;
				list3.Add(part);
			}
		}
		return list;
	}

	public void SetManualStageOffset(int startIndex)
	{
		if (startIndex >= 0 && startIndex < stages.Count)
		{
			for (int i = startIndex; i < stages.Count; i++)
			{
				stages[i].SetManualStageOffset();
			}
		}
	}

	[ContextMenu("SortIcons")]
	public void SortIcons()
	{
		SortIcons(instant: true, null, switchingVessels: false, deleteManualStages: false);
	}

	public void SortIcons(bool instant)
	{
		SortIcons(instant, null, switchingVessels: false, deleteManualStages: false);
	}

	public void SortIcons(bool instant, bool deleteManualStages)
	{
		SortIcons(instant, null, switchingVessels: false, deleteManualStages);
	}

	public void SortIcons(bool instant, Part p, bool switchingVessels, bool deleteManualStages)
	{
		if (instant)
		{
			if (sortRoutine == null)
			{
				SortIconsSequence(p, switchingVessels, deleteManualStages);
			}
		}
		else if (sortRoutine == null)
		{
			sortRoutine = StartCoroutine(SortIconsSequence(p, 2, switchingVessels, deleteManualStages));
		}
		else
		{
			coroutineFramesToWait = 2;
		}
	}

	public IEnumerator SortIconsSequence(Part p, int framesToWait, bool switchingVessels, bool deleteManualStages)
	{
		for (coroutineFramesToWait = framesToWait; coroutineFramesToWait >= 0; coroutineFramesToWait--)
		{
			yield return null;
		}
		SortIconsSequence(p, switchingVessels, deleteManualStages);
		sortRoutine = null;
	}

	public void SortIconsSequence(Part p, bool switchingVessels, bool deleteManualStages)
	{
		AddNewStageGroupsIfNeeded(p);
		AddHeldIconsToStages(p, switchingVessels);
		SetCurrentStage((_currentStage < stages.Count) ? (stages.Count - 1) : stages.Count);
		if (holdEditorSelectedIcons && EditorLogic.SelectedPart != null)
		{
			List<StageIcon> list = AddPartsWithStageiconsToListRecursive(EditorLogic.SelectedPart);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				HoldIcon(list[i]);
			}
		}
		holdEditorSelectedIcons = false;
		if (EnableDeleteEmptyStages)
		{
			DeleteEmptyStages(deleteManualStages);
		}
		if (rebuildIndexes)
		{
			SetSeparationIndices();
			GameEvents.StageManager.OnGUIStageSequenceModified.Fire();
			rebuildIndexes = false;
		}
	}

	[ContextMenu("Force manualStageOffsets")]
	public void ForceUpdateStageGroups()
	{
		UpdateStageGroups();
	}

	public void UpdateStageGroups(bool seqOverride = true)
	{
		int i = 0;
		for (int count = stages.Count; i < count; i++)
		{
			UpdateStageGroup(stages[i], i, seqOverride);
		}
	}

	public void UpdateStageGroup(StageGroup group, int index, bool seqOverride = true)
	{
		group.SetInverseStageIndex(index);
		group.SetPartIndices(seqOverride);
		group.SetUiStageIndex(index);
	}

	public static void BeginFlight()
	{
		Instance.rebuildIndexes = true;
		Instance.SortIcons(instant: false);
		int num = 0;
		int count = FlightGlobals.ActiveVessel.parts.Count;
		while (count-- > 0)
		{
			int inverseStage = FlightGlobals.ActiveVessel.parts[count].inverseStage;
			if (inverseStage > num)
			{
				num = inverseStage;
			}
		}
		Instance.SetCurrentStage(num + 1);
		Instance.canSeparate = true;
	}

	public static void ResumeFlight(int currStage)
	{
		Instance.SetCurrentStage(currStage);
		Instance.canSeparate = true;
		Instance.rebuildIndexes = true;
		Instance.SortIcons(instant: false, null, switchingVessels: true, deleteManualStages: false);
	}

	public static void ActivateNextStage()
	{
		ActivateStage(Instance._currentStage - 1);
		if (ResourceDisplay.Instance != null)
		{
			ResourceDisplay.Instance.isDirty = true;
		}
	}

	public static void ActivateStage(int stage)
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		stage = Mathf.Min(Mathf.Max(0, stage), Instance.stages.Count - 1);
		bool flag;
		if (!(flag = !CanSeparate || Instance._currentStage == 0))
		{
			int count = Instance.stages.Count;
			while (count-- > 0)
			{
				if (Instance.stages[count].IsBeingDragged)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			AudioSource component = Instance.GetComponent<AudioSource>();
			if (component != null)
			{
				component.PlayOneShot(Instance.cannotSeparateClip);
			}
			return;
		}
		AudioSource component2 = Instance.GetComponent<AudioSource>();
		if (component2 != null)
		{
			component2.PlayOneShot(Instance.nextStageClip);
		}
		GameEvents.onStageActivate.Fire(stage);
		activeVessel.updateResourcesOnEvent = false;
		List<Part> list = new List<Part>();
		int count2 = activeVessel.parts.Count;
		while (count2-- > 0)
		{
			Part part = activeVessel.parts[count2];
			if (part.inverseStage == stage)
			{
				list.Add(part);
			}
		}
		int i = 0;
		for (int count3 = list.Count; i < count3; i++)
		{
			list[i].force_activate();
		}
		if (!FlightLogger.LiftOff)
		{
			GameEvents.onLaunch.Fire(new EventReport(FlightEvents.LAUNCH, null));
		}
		if (stage != Instance.stages.Count - 1)
		{
			Instance.stages[stage].MoveAllIconsInto(Instance.stages[Instance.stages.Count - 1]);
		}
		Instance.SetCurrentStage(stage);
		Instance.canSeparate = false;
		Instance.StartCoroutine(Instance.cooldownSeparation(PhysicsGlobals.StagingCooldownTimer));
		Instance.SortIcons(instant: true);
		activeVessel.updateResourcesOnEvent = true;
		activeVessel.UpdateResourceSetsIfDirty();
	}

	public IEnumerator cooldownSeparation(float cooldownPeriod)
	{
		yield return new WaitForSeconds(cooldownPeriod);
		canSeparate = true;
	}

	public void IncrementCurrentStage()
	{
		_currentStage++;
		if (HighLogic.LoadedSceneIsFlight)
		{
			FlightGlobals.ActiveVessel.currentStage = _currentStage;
		}
	}

	public void DecrementCurrentStage()
	{
		if (_currentStage > 0)
		{
			_currentStage--;
			if (HighLogic.LoadedSceneIsFlight)
			{
				FlightGlobals.ActiveVessel.currentStage = _currentStage;
			}
		}
	}

	public void SetCurrentStage(int cStage)
	{
		if (_currentStage >= 0)
		{
			_currentStage = cStage;
			if (HighLogic.LoadedSceneIsFlight)
			{
				FlightGlobals.ActiveVessel.currentStage = _currentStage;
			}
		}
	}

	public static void StepBackCurrentStage()
	{
		Instance.SetCurrentStage(Instance.stages.Count);
	}

	public void SetVisible(bool state)
	{
		Visible = state;
	}

	public static void ToggleStageStack()
	{
		ShowHideStageStack(!Instance.Visible);
	}

	public static void ShowHideStageStack(bool state)
	{
		Instance.mainListAnchor.gameObject.SetActive(state);
		Instance.SetVisible(state);
		if (!Instance.Visible)
		{
			InputLockManager.SetControlLock(ControlTypes.STAGING, "lockStage");
		}
		else
		{
			InputLockManager.RemoveControlLock("lockStage");
		}
	}

	public List<StageIcon> AddPartsWithStageiconsToListRecursive(Part part)
	{
		List<StageIcon> partsWithIcons = new List<StageIcon>();
		AddPartsWithStageiconsToListRecursive(ref partsWithIcons, part);
		return partsWithIcons;
	}

	public void AddPartsWithStageiconsToListRecursive(ref List<StageIcon> partsWithIcons, Part part)
	{
		if (part.stackIcon.StageIcon != null)
		{
			partsWithIcons.Add(part.stackIcon.StageIcon);
			for (int i = 0; i < part.children.Count; i++)
			{
				AddPartsWithStageiconsToListRecursive(ref partsWithIcons, part.children[i]);
			}
		}
		else
		{
			for (int j = 0; j < part.children.Count; j++)
			{
				AddPartsWithStageiconsToListRecursive(ref partsWithIcons, part.children[j]);
			}
		}
	}

	public static void GenerateStagingSequence(Part root)
	{
		FindStageIndices(root, -1, -1);
		SetSeparationIndices(root, -1);
	}

	public static void FindStageIndices(Part part, int thisStage, int carriedInverseStage)
	{
		int num;
		int num2;
		if (!part.hasStagingIcon)
		{
			num = ((part.stackIcon.StageIcon != null) ? 1 : 0);
			if (num == 0)
			{
				num2 = 0;
				goto IL_0026;
			}
		}
		else
		{
			num = 1;
		}
		num2 = part.stageOffset;
		goto IL_0026;
		IL_00a6:
		if (part.stageAfter)
		{
			thisStage++;
		}
		goto IL_00b5;
		IL_0026:
		int num3 = num2;
		thisStage = Mathf.Max(0, thisStage + num3);
		if (num != 0 && part.stageBefore)
		{
			thisStage++;
		}
		part.defaultInverseStage = thisStage;
		int inverseStage = part.inverseStage;
		if (num != 0)
		{
			part.inverseStage = ((part.manualStageOffset != -1) ? part.manualStageOffset : thisStage);
			if (part.inverseStageCarryover)
			{
				carriedInverseStage = part.inverseStage;
			}
		}
		else
		{
			part.inverseStage = carriedInverseStage;
		}
		if (inverseStage != part.inverseStage)
		{
			GameEvents.onPartPriorityChanged.Fire(part);
			if (num != 0)
			{
				goto IL_00a6;
			}
		}
		else if (num != 0)
		{
			goto IL_00a6;
		}
		goto IL_00b5;
		IL_00b5:
		int num4 = ((num != 0) ? part.childStageOffset : 0);
		thisStage = Mathf.Max(0, thisStage + num4);
		int i = 0;
		for (int count = part.children.Count; i < count; i++)
		{
			FindStageIndices(part.children[i], thisStage, carriedInverseStage);
		}
	}

	public static int RecalculateVesselStaging(Vessel v)
	{
		int result = FindLastInverseStage(v.rootPart, 0);
		SetSeparationIndices(v.rootPart, -1);
		return result;
	}

	public static int FindLastInverseStage(Part part, int lastStg)
	{
		lastStg = Mathf.Max(part.inverseStage, lastStg);
		for (int i = 0; i < part.children.Count; i++)
		{
			lastStg = FindLastInverseStage(part.children[i], lastStg);
		}
		return lastStg;
	}

	public static void SetSeparationIndices()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			SetSeparationIndices(FlightGlobals.ActiveVessel.rootPart, -1);
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			SetSeparationIndices(EditorLogic.RootPart, -1);
		}
	}

	public static void SetSeparationIndices(Part p, int sepIndex)
	{
		setSeparationIndices(p, -1, -1);
		GameEvents.StageManager.OnStagingSeparationIndices.Fire();
	}

	public static void setSeparationIndices(Part part, int sepIndex, int carriedInverseStage)
	{
		if (part == null)
		{
			return;
		}
		List<IStageSeparator> list = part.FindModulesImplementing<IStageSeparator>();
		IStageSeparatorChild stageSeparatorChild = part.FindModuleImplementing<IStageSeparatorChild>();
		bool flag = true;
		int num = sepIndex;
		List<Part> decoupledChildParts = new List<Part>();
		int count = list.Count;
		while (count-- > 0)
		{
			num = list[count].GetStageIndex(sepIndex);
		}
		if (stageSeparatorChild != null)
		{
			flag = stageSeparatorChild.PartDetaches(out decoupledChildParts);
		}
		if (flag)
		{
			sepIndex = num;
			part.separationIndex = sepIndex;
		}
		else
		{
			part.separationIndex = sepIndex;
			sepIndex = num;
		}
		if ((part.hasStagingIcon || part.stackIcon.StageIcon != null) && part.stagingOn)
		{
			if (part.inverseStageCarryover)
			{
				carriedInverseStage = part.inverseStage;
			}
		}
		else
		{
			part.inverseStage = carriedInverseStage;
		}
		int i = 0;
		for (int count2 = part.children.Count; i < count2; i++)
		{
			if (!flag && decoupledChildParts.Count > 0)
			{
				bool flag2 = false;
				for (int j = 0; j < decoupledChildParts.Count; j++)
				{
					if (part.children[i].persistentId == decoupledChildParts[j].persistentId)
					{
						flag2 = true;
						setSeparationIndices(part.children[i], sepIndex, carriedInverseStage);
					}
				}
				if (!flag2)
				{
					setSeparationIndices(part.children[i], part.separationIndex, carriedInverseStage);
				}
			}
			else
			{
				setSeparationIndices(part.children[i], sepIndex, carriedInverseStage);
			}
		}
	}

	public void Reset()
	{
		bool flag = false;
		int count = stages.Count;
		while (count-- > 0)
		{
			if (stages[count].ResetAvailable())
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			EditorLogic.fetch.SetBackup();
			int count2 = stages.Count;
			while (count2-- > 0)
			{
				stages[count2].Reset();
				UnityEngine.Object.Destroy(stages[count2].gameObject);
			}
			stages.Clear();
			SetCurrentStage(0);
			SortIcons(instant: false);
			SetSeparationIndices(EditorLogic.RootPart, -1);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_480791"), 2f, ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_480795"), 2f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void Update()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.STAGING | ControlTypes.flag_53))
		{
			if (GameSettings.SCROLL_ICONS_UP.GetKey())
			{
				scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(scrollRect, vertical: true));
			}
			if (GameSettings.SCROLL_ICONS_DOWN.GetKey())
			{
				scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(scrollRect, vertical: true));
			}
		}
		if (DeltaVTotalEnabled)
		{
			SetDeltaVTotal_OnUpdate();
		}
	}

	public bool CheckNullrefsInStageIconList()
	{
		bool result = false;
		int i = 0;
		for (int count = stages.Count; i < count; i++)
		{
			for (int j = 0; j < stages[i].Icons.Count; j++)
			{
				if (stages[i].Icons[j] == null)
				{
					Debug.LogError("StageGroup has null icon ->", stages[i].gameObject);
					result = true;
				}
			}
		}
		return result;
	}

	public void DeltaVAppAtmosphereChanged(DeltaVSituationOptions situation)
	{
		DeltaVCalcsCompleted();
	}

	public void DeltaVCalcsCompleted()
	{
		if (DeltaVTotalEnabled)
		{
			SetDeltaVTotal_OnDeltaVCalcsCompleted();
		}
	}

	public virtual void SetDeltaVTotal_OnDeltaVCalcsCompleted()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			simulation = EditorLogic.fetch.ship.vesselDeltaV;
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			simulation = FlightGlobals.ActiveVessel.VesselDeltaV;
		}
		else
		{
			simulation = null;
		}
		if (simulation == null)
		{
			deltaVTotalSection.gameObject.SetActive(value: false);
			return;
		}
		double situationTotalDeltaV = simulation.GetSituationTotalDeltaV(DeltaVGlobals.DeltaVAppValues.situation);
		deltaVTotalText.text = $"{situationTotalDeltaV:0}" + cacheAutoLOC_180095;
		deltaVTotalSection.gameObject.SetActive(value: true);
		deltaVTotalButton.interactable = true;
	}

	public virtual void SetDeltaVTotal_OnUpdate()
	{
		if (!HighLogic.LoadedSceneIsEditor && simulation != null)
		{
			double situationTotalDeltaV = simulation.GetSituationTotalDeltaV(DeltaVGlobals.DeltaVAppValues.situation);
			deltaVTotalText.text = $"{situationTotalDeltaV:0}" + cacheAutoLOC_180095;
		}
	}

	public void DeltaVButtonClicked()
	{
		bool flag = true;
		for (int i = 0; i < Stages.Count; i++)
		{
			if (Stages[i].infoPanelShown)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			usageDV.allStagesShow++;
		}
		else
		{
			usageDV.allStagesHide++;
		}
		ToggleInfoPanels(flag);
	}

	public void ToggleInfoPanels(bool showPanels)
	{
		for (int i = 0; i < Stages.Count; i++)
		{
			Stages[i].ToggleInfoPanel(showPanels);
		}
	}

	public void EnableDeltaVTotal()
	{
		EnableDeltaVTotal(enable: true);
	}

	public void DisableDeltaVTotal()
	{
		EnableDeltaVTotal(enable: false);
	}

	public void EnableDeltaVTotal(bool enable)
	{
		deltaVTotalEnabled = enable;
		deltaVTotalSection.gameObject.SetActive(enable);
		deltaVTotalButton.interactable = enable;
	}

	public void EnableDeltaVHeadings()
	{
		EnableDeltaVHeadings(enable: true);
	}

	public void DisableDeltaVHeadings()
	{
		EnableDeltaVHeadings(enable: false);
	}

	public void EnableDeltaVHeadings(bool enable)
	{
		deltaVHeadingsEnabled = enable;
		for (int i = 0; i < Stages.Count; i++)
		{
			Stages[i].EnableDeltaVHeading(enable);
		}
	}

	public void EnableInfoPanels()
	{
		EnableInfoPanels(enable: true);
	}

	public void DisableInfoPanels()
	{
		EnableInfoPanels(enable: false);
	}

	public void EnableInfoPanels(bool enable)
	{
		infoPanelsEnabled = enable;
		for (int i = 0; i < Stages.Count; i++)
		{
			Stages[i].EnableInfoPanel(enable);
		}
	}

	public void OnGUIStageSequenceModified()
	{
		deltaVTotalSection.gameObject.SetActive(Stages.Count > 1);
		UpdateHoverArea();
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_180095 = Localizer.Format("#autoLOC_180095");
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if ((scn.from == GameScenes.EDITOR || scn.from == GameScenes.FLIGHT) && usageDV.UsageFound)
		{
			AnalyticsUtil.DeltaVStageUsage(HighLogic.CurrentGame, scn.from, usageDV);
			usageDV.Reset();
		}
	}
}
