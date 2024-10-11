using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightDriver : MonoBehaviour
{
	public enum StartupBehaviours
	{
		NEW_FROM_FILE,
		RESUME_SAVED_FILE,
		RESUME_SAVED_CACHE,
		NEW_FROM_CRAFT_NODE
	}

	private enum TimeInMode
	{
		Flight,
		Map,
		External,
		IVA,
		Internal,
		EVA,
		EVAConstruction
	}

	[CompilerGenerated]
	private sealed class _003CPostInit_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FlightDriver _003C_003E4__this;

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
		public _003CPostInit_003Ed__34(int _003C_003E1__state)
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

	private bool canRun;

	public StartupBehaviours inEditorStartUpMode;

	public int targetVessel;

	public static string newShipToLoadPath;

	public static StartupBehaviours StartupBehaviour;

	private static bool resumeCacheUsed;

	public static string newShipFlagURL;

	public static VesselCrewManifest newShipManifest;

	public static string StateFileToLoad;

	public static Game FlightStateCache;

	public static int FocusVesselAfterLoad;

	public static string LaunchSiteName;

	public static bool flightStarted;

	public bool bypassPersistence;

	public bool DEBUG_PauseAfterStart;

	public static bool CanRevertToPostInit;

	public static bool CanRevertToPrelaunch;

	public static GameBackup PostInitState;

	public static GameBackup PreLaunchState;

	public static FlightDriver fetch;

	public bool bypassLoadingEnforce;

	public int framesBeforeInitialSave;

	private int framesAtStartup;

	private ShipConstruct newVessel;

	public string uiSkinName;

	private UISkinDef uiskin;

	private static bool pause;

	private double startTime;

	private double modeStartTime;

	private Dictionary<string, double> timeInMode;

	private TimeInMode currentMode;

	public static bool BypassPersistence
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool CanRevert
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool Pause
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightDriver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightDriver()
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
	private void setStartupNewVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CPostInit_003Ed__34))]
	private IEnumerator PostInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Focus Target Vessel")]
	public void FocusTargetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnApplicationFocus(bool focus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPause(bool pauseState, bool postScreenMessage = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StartWithNewLaunch(string fullFilePath, string missionFlagURL, string launchSiteName, VesselCrewManifest manifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RevertToLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RevertToPrelaunch(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ReturnToEditor(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StartAndFocusVessel(string stateFileToLoad, int vesselToFocusIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void StartAndFocusVessel(Game stateToLoad, int vesselToFocusIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Game RemoveSavedVessel(Game original, int vesselIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitAnalytics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FinalizeAnalytics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModeChange(TimeInMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraChange(CameraManager.CameraMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnConstructionModeChange(bool open)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewOnEva(GameEvents.FromToAction<Part, Part> fv)
	{
		throw null;
	}
}
