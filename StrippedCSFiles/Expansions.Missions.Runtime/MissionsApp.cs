using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Flow;
using KSP.UI;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Runtime;

public class MissionsApp : UIApp
{
	public class MissionListItem
	{
		public UICascadingList.CascadingListItem listItem;

		public MEFlowParser parser;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionListItem()
		{
			throw null;
		}
	}

	private enum AppIconStates
	{
		Normal,
		Warning
	}

	[CompilerGenerated]
	private sealed class _003CUpdateMissionsDaemon_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MissionsApp _003C_003E4__this;

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
		public _003CUpdateMissionsDaemon_003Ed__54(int _003C_003E1__state)
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
	private sealed class _003CUpdateVesselsDaemon_003Ed__68 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MissionsApp _003C_003E4__this;

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
		public _003CUpdateVesselsDaemon_003Ed__68(int _003C_003E1__state)
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
	private sealed class _003CbuildStartedMissionVessels_003Ed__82 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Mission missionToTest;

		public MissionsApp _003C_003E4__this;

		public Callback<Mission> OnComplete;

		private int _003CvslI_003E5__2;

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
		public _003CbuildStartedMissionVessels_003Ed__82(int _003C_003E1__state)
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

	public static MissionsApp Instance;

	private DictionaryValueList<string, MissionListItem> missionList;

	private DictionaryValueList<uint, UICascadingList.CascadingListItem> vesselList;

	public float refreshTimeToWait;

	private Callback onCreateNextVessel;

	private bool refreshRequested;

	private bool refreshRequestedCurrentVessel;

	private VesselSituation requestedCurrentVessel;

	private List<Mission> refreshMissions;

	private bool refreshMissionPos;

	private MissionAppMode mode;

	private bool waitingForEditor;

	private bool startNewVessel;

	private bool lockAppOpen;

	private bool ShowEnterBuildMsg;

	[SerializeField]
	private GenericAppFrame appFramePrefab;

	private GenericAppFrame appFrame;

	[SerializeField]
	private GenericCascadingList cascadingListPrefab;

	private GenericCascadingList cascadingList;

	[SerializeField]
	private UIListItem BodyMissionVesselHeader_prefab;

	[SerializeField]
	private UIListItem_spacer BodyMissionVesselItem_prefab;

	[SerializeField]
	private UIListItem BodySettingsToggleItem_prefab;

	[SerializeField]
	private UIListItem BodySettingsButtonItem_prefab;

	private UIListItem TestModeActiveNode;

	private TextMeshProUGUI TestModeActiveNodeText;

	private MissionsAppVesselInfo currentVessel;

	private bool appInitializing;

	private bool updateMissionsDaemonRunning;

	private bool updateVesselsDaemonRunning;

	private uint lastVesselBuildId;

	public DictionaryValueList<string, MissionListItem> MissionList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DictionaryValueList<uint, UICascadingList.CascadingListItem> VesselList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MissionAppMode Mode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MissionsAppVesselInfo CurrentVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionsAppVesselInfo GetVesselListAppVesselInfo(uint persistentId)
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
	protected override void OnAppInitialized()
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
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionsLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveMission(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddMission(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMissionParameter(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMissionRequest(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshMissionRequested(Mission mission, bool changePositions = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMission(Mission mission, bool changePositions = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateMissionsDaemon_003Ed__54))]
	private IEnumerator UpdateMissionsDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTestGroupChanged(TestGroup testGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionNodeChanged(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorIcon(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onStateIcon(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void changeVessels(MissionsAppVesselInfo vesselSwitchToInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshVesselsToCreate(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshCurrentVesselToBuild(Mission mission, VesselSituation currentVesselToBuild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void editorSaveClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode findCraftFile(string craftFolder, string craftFile, out EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void changeVesselListState(MissionsAppVesselInfo vesselToSwitch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setEditorIconStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateVesselsDaemon_003Ed__68))]
	private IEnumerator UpdateVesselsDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void stopVesselsDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void startVesselDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool EditorVesselCompleted(string craftFile, VesselCrewManifest vesselManifest, Callback onCreateNextVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CheckforVesselstoBuild(Callback onCreateNextVessel, uint inlastVesselBuildId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StayOnEditor(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionLaunchSitesGenerated(Mission missionToTest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionLaunchSitesGeneratedContinue(Mission missionToTest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onBuildStartedMissionVesselsComplete(Mission missionToTest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSetupFlightSuccess(Mission missionToTest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSetupFlightFailed(Mission missionToTest, string errorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCreateVesselWarningOK(bool dontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FireCreateNextVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CbuildStartedMissionVessels_003Ed__82))]
	private IEnumerator buildStartedMissionVessels(Mission missionToTest, Callback<Mission> OnComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ExitEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetVesselsCountsLists(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UICascadingList.CascadingListItem CreateTestModeSettingsItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> CreateTestModeSettingsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckpointsSettingsChange(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckpointCreate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetColoredMissionHeader(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetColoredObjectiveHeader(string text, bool complete, bool alert = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateMissionsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MissionListItem CreateMissionItem(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> CreateObjectivesList(Mission mission, MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string setVesselHeaderColor(VesselSituation vessel, bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateVesselsList(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UICascadingList.CascadingListItem CreateItem(Mission mission, VesselSituation vessel, bool firstVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVesselItemReadyIcon(VesselSituation vesselSituation, MissionsAppVesselInfo vesselInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<UIListItem> CreateVesselDetails(MissionsAppVesselInfo vesselInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddVesselSubParameter(List<UIListItem> requirements, MissionsAppVesselInfo vesselInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonPlayAnim(float duration = 5f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonStopAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAppIconState(AppIconStates iconState)
	{
		throw null;
	}
}
