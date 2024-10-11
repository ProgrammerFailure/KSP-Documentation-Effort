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

[Serializable]
public class StageGroup : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _003CUpdateStageManagerHover_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CUpdateStageManagerHover_003Ed__60(int _003C_003E1__state)
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

	[SerializeField]
	protected RectTransform iconsUiList;

	[SerializeField]
	private TextMeshProUGUI uiStageIndex;

	[SerializeField]
	protected Button addButton;

	[SerializeField]
	protected Button deleteButton;

	[SerializeField]
	protected UIDragPanel dragHandler;

	[SerializeField]
	protected LayoutElement InfoPanelLayout;

	[SerializeField]
	private float InfoPanelWidth;

	[SerializeField]
	private float InfoPanelSlidePeriod;

	[SerializeField]
	protected LayoutElement FooterLayout;

	[SerializeField]
	protected LayoutElement DeltaVHeadingObject;

	[SerializeField]
	protected TextMeshProUGUI DeltaVHeadingText;

	[SerializeField]
	protected Color DeltaVHeadingColor;

	private Image DeltaVHeadingImage;

	[SerializeField]
	protected StageGroupInfoItem infoItemPrefab;

	[SerializeField]
	private bool deltaVHeadingEnabled;

	[SerializeField]
	private bool infoPanelEnabled;

	[SerializeField]
	private bool infoPanelStockDisplayEnabled;

	[SerializeField]
	private List<StageIcon> icons;

	public int defaultStage;

	public int inverseStageIndex;

	public bool manualOverride;

	private RectTransform rectTransform;

	private int insertAtInverseStageIndex;

	private int insertAtSiblingIndex;

	private int lastInversetageIndex;

	private ResizingLayoutElement lastSpawnedResizingElement;

	private bool beginDrag;

	internal bool infoPanelShown;

	internal bool infoPanelSliding;

	private VesselDeltaV simulation;

	private DeltaVAppValues dvAppValues;

	private DeltaVStageInfo stage;

	private List<StageGroupInfoItem> stageGroupInfos;

	private int stageGroupInfosCount;

	private static string cacheAutoLOC_180095;

	public RectTransform IconsUIList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool DeltaVHeadingEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InfoPanelEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InfoPanelStockDisplayEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<StageIcon> Icons
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public RectTransform RectTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsBeingDragged
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageGroup()
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
	private void OnLeavingScene(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Hover(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HoverOut(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUiStageIndex(int index)
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
	private void OnEndDragCompleted(StageGroup stageToUpdateA, StageGroup stageToUpdateB, int insertAt, int modifiedSiblingIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleInfoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleInfoPanel(bool showPanel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInfoPanelSlideUpdate(float width)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInfoPanelSlideComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateStageManagerHover_003Ed__60))]
	private IEnumerator UpdateStageManagerHover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVHeading()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableDeltaVHeading()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeltaVHeading(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableInfoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableInfoPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableInfoPanel(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableStockInfoPanelDisplays()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableStockInfoPanelDisplays()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableStockInfoPanelDisplays(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditorStarted()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDeltaVHeading_OnDeltaVCalcsCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDeltaVHeading_OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInfoObject(StageGroupInfoItem info, DeltaVAppValues.InfoLine dvInfo, DeltaVStageInfo stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private StageGroupInfoItem SpawnInfoObject(DeltaVAppValues.InfoLine dvInfo, DeltaVStageInfo stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddIcons(StageIcon[] iconsToAdd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddIcon(StageIcon icon, bool setParent = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddIconAt(StageIcon icon, int index, int forcedSiblingIndex, bool setParent = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveIcon(StageIcon icon, bool changeParents = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Delete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddStageAfter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInStageIndexes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInverseStageIndex(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartIndices(bool seqOverride = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetManualStageOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSiblingIndexes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ResetAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageGroupIndex(PointerEventData eventData, out StageIcon stageIcon, out int modifiedSiblingIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIcon FindSymmetryGroupleader(StageIcon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MoveAllIconsInto(StageGroup moveInto)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MoveIconsInto(StageGroup oldStageGroup, StageIcon selectedLeader, List<StageIcon> selectedIcons, StageIcon unSelectedLeader, List<StageIcon> unSelectedIcons, int insertAt, int modifiedSiblingIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixGroupUIState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
