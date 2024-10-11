using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Contracts;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugToolbar : MonoBehaviour
{
	private enum WindowToolbar
	{
		Cheats,
		Physics,
		Debug,
		Database,
		Contracts,
		Missions,
		Kerbals,
		Gameplay,
		Resources,
		RnD,
		Performance
	}

	private enum PhysicsToolbar
	{
		Aero,
		Drag,
		DragProfile,
		Thermal,
		Database
	}

	private enum DragProfileType
	{
		Tail,
		Surface,
		Tip,
		Multiplier
	}

	private enum DatabaseToolbar
	{
		Overview,
		Configs,
		Assemblies,
		Models,
		Textures,
		Audio
	}

	private enum ContractsToolbar
	{
		Active,
		Offered,
		Archive,
		Add,
		Tools
	}

	private enum MissionsToolbar
	{
		Active = 0,
		Completed = 1,
		Tools = 4
	}

	private enum KerbalToolbar
	{
		Roster,
		Details,
		Career,
		Flight
	}

	[CompilerGenerated]
	private sealed class _003CRecompileAssets_003Ed__100 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DebugToolbar _003C_003E4__this;

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
		public _003CRecompileAssets_003Ed__100(int _003C_003E1__state)
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
	private sealed class _003CwaitForGameLoad_003Ed__122 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int waitdelay;

		public DebugToolbar _003C_003E4__this;

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
		public _003CwaitForGameLoad_003Ed__122(int _003C_003E1__state)
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

	public static bool toolbarShown;

	private bool inputlockStack;

	private bool setShipOrbit;

	private bool drawFlightStats;

	private bool drawWhackAKerbal;

	private float realDt;

	private float lastRealTime;

	private float ActualTime;

	private Dictionary<CelestialBody, double> originalGeeForces;

	private bool gravityHack;

	private bool over;

	private bool closed;

	private string[] windowToolbarStrings;

	private WindowToolbar windowToolbarIndex;

	private Rect windowRect;

	private Rect setOrbitRect;

	private string ecc;

	private string inc;

	private string sma;

	private string lPe;

	private string MnA;

	private string LAN;

	private string ObT;

	private int selBodyIndex;

	private Game.Modes currentMode;

	private bool drawExtraCheats;

	private const float cheatDisplayTime = 1.5f;

	private bool drawAeroOptions;

	private bool drawCameraFX;

	private string[] windowPhysicsStrings;

	private PhysicsToolbar windowPhysicsIndex;

	private bool updateDragData;

	private static int dragCurvePointCount;

	private float dragMachDisplay;

	private Vector2[] dragMachPoints;

	private float[] dragCurveGrads;

	private bool dragUpdateCurves;

	private float[] dragProfileGrads;

	private DragProfileType dragProfileSelected;

	private string[] dragProfileTypeString;

	private Vector2[] dragProfilePoints;

	private Vector2 debugScrollView;

	private string debugInput;

	private string[] databaseToolbarStrings;

	private DatabaseToolbar databaseToolbarIndex;

	private Vector2 databaseOverviewScroll;

	private Vector2 databaseConfigScroll;

	private Vector2 databaseConfigPreviewScroll;

	private string[] databaseConfigPreview;

	private UrlDir.UrlConfig databaseConfigPreviewURL;

	private bool databaseConfigFilterPART;

	private bool databaseConfigFilterPROP;

	private bool databaseConfigFilterINTERNAL;

	private bool databaseConfigFilterRESOURCE;

	private Vector2 databaseAssemblyScroll;

	private Vector2 databaseModelsScroll;

	private Vector2 databaseTexturesScroll;

	private Vector2 databaseAudioScroll;

	private bool isRecompiling;

	private string[] contractsToolbarStrings;

	private ContractsToolbar contractsToolbarIndex;

	private Vector2 contractsOfferedScroll;

	private Vector2 contractsActiveScroll;

	private Vector2 contractsArchiveScroll;

	private string[] missionsToolbarStrings;

	private MissionsToolbar missionsToolbarIndex;

	private GameParameters gPars;

	private Vector2 gameplayTweaksScrollPos;

	private Vector2 resourceTweaksScrollPos;

	private string[] kerbalToolbarStrings;

	private KerbalToolbar kerbalToolbarIndex;

	private int kerbalIndex;

	private Vector2 kerbalRosterScroll;

	private Vector2 kerbalRosterFlightScroll;

	private Vector2 kerbalRosterCareerScroll;

	private RnDDebugUtil RDdebugUtil;

	private int RDwindowID;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DebugToolbar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DebugToolbar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawFlightStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Window(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowCheats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetBiomesVisible(bool isTrue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheatInScience(float science)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheatInFunds(double funds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheatInReputation(float rep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheatInExperience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowWhackAKerbalDialog(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drawMoveShipWindow(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowPhysics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowPhysicsDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowAero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowThermal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowDragProfile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowDragDisplay_Profiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool FloatCurveEditor(FloatCurve curve)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2[] GetSurfaceDragCurvePoints(float mach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2[] GetAnimationCurvePoints(AnimationCurve curve)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowDebug()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseOverview()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseOverviewConfig(string type, ref bool filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseConfigs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseConfigsSpam(string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseConfigPreview(UrlDir.UrlConfig cfg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseAssemblies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseModels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseTextures()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DatabaseAudio()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRecompileAssets_003Ed__100))]
	private IEnumerator RecompileAssets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowContracts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContractsOffered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContractsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContractsArchive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContractsAdd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ContractGenerate(Type cType, Contract.ContractPrestige difficulty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContractsTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MissionsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MissionsCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MissionsTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitForGameLoad_003Ed__122))]
	private IEnumerator waitForGameLoad(int waitdelay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowGameplayTweaks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowResourceTweaks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowKerbals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalRosterAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalRosterDetails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalRosterCareer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalRosterFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalDatabaseSelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WindowPerformance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRnDSpawn(RDTechTree tecTree)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRnDDespawn(RDTechTree tecTree)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawWindowRnD()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FloatField(ref float var, params GUILayoutOption[] options)
	{
		throw null;
	}
}
