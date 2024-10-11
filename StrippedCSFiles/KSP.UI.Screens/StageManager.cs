using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class StageManager : MonoBehaviour
{
	internal class DVUsageStats
	{
		public int stageHide;

		public int stageShow;

		public int allStagesShow;

		public int allStagesHide;

		internal bool UsageFound
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DVUsageStats()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSortIconsSequence_003Ed__101 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StageManager _003C_003E4__this;

		public int framesToWait;

		public Part p;

		public bool switchingVessels;

		public bool deleteManualStages;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CSortIconsSequence_003Ed__101(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CcooldownSeparation_003Ed__110 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float cooldownPeriod;

		public StageManager _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CcooldownSeparation_003Ed__110(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public Button resetButton;

	[SerializeField]
	private RectTransform resetButtonRT;

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
	private VerticalLayoutGroup mainListAnchor;

	public LayoutElement deltaVTotalSection;

	public Button deltaVTotalButton;

	public TextMeshProUGUI deltaVTotalText;

	[SerializeField]
	private float plusMinusButtonWidth;

	[SerializeField]
	private float minimumPlusMinusButtonWidth;

	[NonSerialized]
	public Color stageIconNumberColor_lead;

	[NonSerialized]
	public Color stageIconNumberColor_symmetry;

	[SerializeField]
	protected List<StageGroup> stages;

	[SerializeField]
	private int _currentStage;

	private List<StageIcon> iconsInHold;

	private List<StageIcon> selection;

	private List<StageIcon> iconCache;

	private bool canSeparate;

	public const int radioGroupExpanded = 0;

	public const int radioGroupCollapsed = 99999;

	public const float tweeningSpeed = 0.15f;

	private bool holdEditorSelectedIcons;

	public static bool EnableDeleteEmptyStages;

	private bool rebuildIndexes;

	private Vector3 pointerWorldPos_InsertionIndex;

	private Vector3 pointerWorldPos_UnderCursor;

	private Coroutine sortRoutine;

	private int coroutineFramesToWait;

	[SerializeField]
	private bool deltaVTotalEnabled;

	[SerializeField]
	private bool deltaVHeadingsEnabled;

	[SerializeField]
	private bool infoPanelsEnabled;

	private VesselDeltaV simulation;

	private static string cacheAutoLOC_180095;

	internal DVUsageStats usageDV;

	public static StageManager Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public List<StageGroup> Stages
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int LastStage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int StageCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int CurrentStage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<StageIcon> Selection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool CanSeparate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Visible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool DeltaVTotalEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool DeltaVHeadingsEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InfoPanelsEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static StageManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselClearStaging(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselResumeStaging(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartUpdateStageability(Part data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPodSelected(Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPodDeleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartPicked(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartPlaced(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartDeleted(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorRestoreState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSwitching(Vessel from, Vessel to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorLoad(ShipConstruct constr, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateHoverArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddNewStageGroupsIfNeeded(Part selectedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddHeldIconsToStages(Part selectedPart, bool switchingVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetParachuteStageIndex(int currentIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageGroupInsertionIndex(PointerEventData eventData, out int modifiedSiblingIndex, bool freshEventData = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private StageGroup GetNonDraggedStageGroupAboveGroup(StageGroup group, bool includeGroup = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageGroupUnderCursor(PointerEventData eventData, out StageGroup stageGroup, bool freshEventData = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HoldIcon(StageIcon stageIcon, bool removeIcon = true, bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnHoldIcon(StageIcon stageIcon, bool changeParents = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static StageIcon CreateIcon(ProtoStageIcon protoIcon, bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private StageIcon GetNewStageIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveIcon(StageIcon stageIcon, bool destroyIcon = true, bool removeSelection = true, bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DisableIcon(StageIcon iconToDisable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddStageAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InsertStageAt(StageGroup stageGroup, int index, int forcedSiblingIndex = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteStage(StageGroup stageToDelete, bool tweenOut = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteEmptyStages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeleteEmptyStages(bool deleteManualStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveStage(StageGroup stageGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetStageCount(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetStageCount(List<Part> ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Part> FindPartsWithModuleSeparatingBeforeOtherPartsWithModule<Before, After>(List<Part> parts) where Before : PartModule where After : PartModule
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetManualStageOffset(int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("SortIcons")]
	private void SortIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortIcons(bool instant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortIcons(bool instant, bool deleteManualStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortIcons(bool instant, Part p, bool switchingVessels, bool deleteManualStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSortIconsSequence_003Ed__101))]
	private IEnumerator SortIconsSequence(Part p, int framesToWait, bool switchingVessels, bool deleteManualStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortIconsSequence(Part p, bool switchingVessels, bool deleteManualStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Force manualStageOffsets")]
	private void ForceUpdateStageGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateStageGroups(bool seqOverride = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStageGroup(StageGroup group, int index, bool seqOverride = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BeginFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResumeFlight(int currStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ActivateNextStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ActivateStage(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CcooldownSeparation_003Ed__110))]
	private IEnumerator cooldownSeparation(float cooldownPeriod)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IncrementCurrentStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DecrementCurrentStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetCurrentStage(int cStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StepBackCurrentStage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVisible(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ToggleStageStack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ShowHideStageStack(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<StageIcon> AddPartsWithStageiconsToListRecursive(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPartsWithStageiconsToListRecursive(ref List<StageIcon> partsWithIcons, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GenerateStagingSequence(Part root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindStageIndices(Part part, int thisStage, int carriedInverseStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int RecalculateVesselStaging(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int FindLastInverseStage(Part part, int lastStg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetSeparationIndices()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetSeparationIndices(Part p, int sepIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void setSeparationIndices(Part part, int sepIndex, int carriedInverseStage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckNullrefsInStageIconList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeltaVAppAtmosphereChanged(DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeltaVCalcsCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDeltaVTotal_OnDeltaVCalcsCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDeltaVTotal_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeltaVButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleInfoPanels(bool showPanels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVTotal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableDeltaVTotal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVTotal(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVHeadings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableDeltaVHeadings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVHeadings(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableInfoPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableInfoPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableInfoPanels(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUIStageSequenceModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}
}
