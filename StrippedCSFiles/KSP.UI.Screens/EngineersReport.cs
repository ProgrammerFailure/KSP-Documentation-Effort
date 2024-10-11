using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PreFlightTests;
using RUI.Algorithms;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

public class EngineersReport : UIApp
{
	private class TestWrapper
	{
		private static int idCounter;

		private int id;

		private List<Part> selectors;

		public IDesignConcern test
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

		public UIListItem container
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

		public UIStateImage containerStateImage
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
		public TestWrapper(IDesignConcern test)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static TestWrapper()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void UpdateContainer()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string GetFormattedTestTitle()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string GetFormattedTestDescription()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateStateImage()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool RunTest()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected void MouseInput_PointerEnter(PointerEventData eventData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected void MouseInput_PointerExit(PointerEventData eventData)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CRunTests_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EngineersReport _003C_003E4__this;

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
		public _003CRunTests_003Ed__49(int _003C_003E1__state)
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
	private sealed class _003CUpdateCraftStatsRoutine_003Ed__73 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EngineersReport _003C_003E4__this;

		public ShipConstruct ship;

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
		public _003CUpdateCraftStatsRoutine_003Ed__73(int _003C_003E1__state)
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

	public static EngineersReport Instance;

	public static bool Ready;

	[SerializeField]
	private GameObject appFramePrefab;

	private GenericAppFrame appFrame;

	[SerializeField]
	private GameObject cascadingListPrefab;

	private GenericCascadingList cascadingListInfo;

	private GenericCascadingList cascadingListCheck;

	[SerializeField]
	private UIListItem listItem_SeveritySelector_prefab;

	private UIListItem listItem_SeveritySelector;

	[SerializeField]
	private UIListItem listItem_BodySeverity_prefab;

	private UIListItem listItem_partCountZero;

	private UIListItem listItem_allTestsPassed;

	private TextMeshProUGUI partCountLH;

	private TextMeshProUGUI partCountRH;

	private TextMeshProUGUI partMassLH;

	private TextMeshProUGUI partMassRH;

	private TextMeshProUGUI sizeLH;

	private TextMeshProUGUI sizeRH;

	private float partCount;

	private float partLimit;

	private float totalMass;

	private float massLimit;

	private Vector3 craftSize;

	private Vector3 maxSize;

	private SpaceCenterFacility editorFacility;

	private SpaceCenterFacility launchFacility;

	private bool allGood;

	private UICascadingList.CascadingListItem designConcernCascadingItem;

	private RUIHoverController scheduler;

	private List<TestWrapper> tests;

	private UIListItem designConcernHeader;

	private Coroutine updateRoutine;

	private Coroutine testRoutine;

	public static SCCFlowGraphUCFinder sccFlowGraphUCFinder;

	private List<TestWrapper> tempBodyList;

	private UIRadioButton btnNotice;

	private UIRadioButton btnWarning;

	private UIRadioButton btnCritical;

	private bool severityNotice;

	private bool severityWarning;

	private bool severityCritical;

	private static string cacheAutoLOC_442833;

	private static string cacheAutoLOC_443059;

	private static string cacheAutoLOC_443064;

	private static string cacheAutoLOC_443343;

	private static string cacheAutoLOC_443417;

	private static string cacheAutoLOC_443418;

	private static string cacheAutoLOC_443419;

	private static string cacheAutoLOC_443420;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_442811;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EngineersReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EngineersReport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAppAboutToStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStageAddedDeleted(int inverseStageIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrewDialogChange(VesselCrewManifest vcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCraftModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCraftModified_delayed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCraftModified(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewModified(VesselCrewManifest vcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddTest(IDesignConcern test)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveTest(IDesignConcern test)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRunTests_003Ed__49))]
	private IEnumerator RunTests()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDesignConcern()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> UpdateDesignConcernBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateStockDesignConcern()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ShowSeverity(TestWrapper testWrapper)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIListItem CreateSeveritySelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Notice_OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Notice_OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Warning_OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Warning_OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Critical_OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Critical_OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAndReposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UICascadingList.CascadingListItem CreateCraftStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> CreateCraftStatsbody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateCraftStatsRoutine_003Ed__73))]
	private IEnumerator UpdateCraftStatsRoutine(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCratStats(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShouldTest(IDesignConcern test)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
