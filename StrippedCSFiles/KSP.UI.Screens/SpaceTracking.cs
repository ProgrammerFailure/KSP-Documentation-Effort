using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommNet;
using Expansions.Missions;
using Expansions.Missions.Flow;
using FinePrint;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class SpaceTracking : MonoBehaviour
{
	private class MissionObjective
	{
		public Waypoint waypoint;

		public MENode node;

		public MapNode mapNode;

		public MissionOrbitRenderer missionOrbitRenderer;

		public CelestialBody body;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MissionObjective()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompleteStartUp_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SpaceTracking _003C_003E4__this;

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
		public _003CCompleteStartUp_003Ed__39(int _003C_003E1__state)
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
	private sealed class _003CPostInitTgtFocus_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int frameDelay;

		public SpaceTracking _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CPostInitTgtFocus_003Ed__48(int _003C_003E1__state)
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
	private sealed class _003CsetRequestedVessel_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SpaceTracking _003C_003E4__this;

		public Vessel v;

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
		public _003CsetRequestedVessel_003Ed__51(int _003C_003E1__state)
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

	public static SpaceTracking Instance;

	public Button LeaveBtn;

	public Button FlyButton;

	public Button DeleteButton;

	public Button RecoverButton;

	public Button TrackButton;

	public TrackingStationWidget listItemPrefab;

	public Transform listContainer;

	public ToggleGroup listToggleGroup;

	private List<TrackingStationWidget> vesselWidgets;

	public Toggle tglTrackedObjects;

	public Toggle tglMissionObjectives;

	public ToggleGroup tabsToggleGroup;

	public Transform missionsListContainer;

	public ToggleGroup missionsListToggleGroup;

	[SerializeField]
	private GameObject TimeWarpWidget;

	[SerializeField]
	private GameObject TimeScrubberWidget;

	[SerializeField]
	private ScrollRect sideBarScrollRect;

	private Game st;

	private List<Vessel> trackedVessels;

	[SerializeField]
	private Dictionary<Vessel, MapObject> scaledTargets;

	private Vessel selectedVessel;

	private PlanetariumCamera mainCamera;

	private bool unownedTrackingUnlocked;

	private MissionRecoveryDialog summaryScreen;

	private Mission mission;

	private MEFlowParser flowParser;

	internal static string missionFilePath;

	private CommNetUI.DisplayMode commNetUIMode;

	private bool recoverButtonMissionAllowed;

	[SerializeField]
	private List<MissionObjective> missionsListObjectives;

	private static Guid tgtVesselId;

	private Coroutine requestCoroutine;

	private int lastSetVesselFrame;

	public Vessel SelectedVessel
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

	public PlanetariumCamera MainCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SpaceTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCompleteStartUp_003Ed__39))]
	private IEnumerator CompleteStartUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void buildVesselsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearUIList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConstructUIList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StartTrackingObject(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StopTrackingObject(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GoToAndFocusVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CPostInitTgtFocus_003Ed__48))]
	private IEnumerator PostInitTgtFocus(int frameDelay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RequestVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CsetRequestedVessel_003Ed__51))]
	private IEnumerator setRequestedVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVessel(Vessel v, bool keepFocus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FlyVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onCBIconClicked(CelestialBody data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselIconClick(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnOnClick_LeaveTrackingStation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnOnClick_FlySelectedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnOnClick_DeleteSelectedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnOnclick_RecoverSelectedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnOnclick_TrackSelectedVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnOnClick_TrackedObjects(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnOnClick_MissionObjectives(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setMissionObjectives(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setTrackedObjects(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StopTrackingVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRecoverConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDialogDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPlanetariumTargetChanged(MapObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSummaryDialogSpawn(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSummaryDialogDespawn(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselCreated(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselDestroyed(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> l)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMapViewFiltersModified(MapViewFiltering.VesselTypeFilter data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void lockUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void unlockUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MissionModeValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void createMissionObjectivesItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTestGroupChanged(TestGroup testGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMissionRequest(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionNodeChanged(Mission mission, GameEvents.FromToAction<MENode, MENode> nodeChanges)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateMissionObjectivesItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MissionsObjectiveCallback(MEFlowUINode uiNode)
	{
		throw null;
	}
}
