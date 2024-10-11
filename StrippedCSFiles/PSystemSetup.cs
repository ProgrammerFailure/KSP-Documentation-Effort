using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgradeables;

public class PSystemSetup : MonoBehaviour
{
	[Serializable]
	public class SpaceCenterFacility
	{
		[Serializable]
		public class SpawnPoint
		{
			public string name;

			public string spawnTransformURL;

			public double latitude;

			public double longitude;

			public double altitude;

			public bool latlonaltSet;

			[NonSerialized]
			private SpaceCenterFacility host;

			private Transform spawnPointTransform;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public SpawnPoint()
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void Setup(SpaceCenterFacility host)
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public Transform GetSpawnPointTransform()
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void GetSpawnPointLatLonAlt(out double latitude, out double longitude, out double altitude)
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void SetSpawnPointLatLonAlt()
			{
				throw null;
			}
		}

		public string name;

		public string pqsName;

		public string facilityTransformName;

		public string facilityName;

		public string facilityDisplayName;

		public PQS facilityPQS;

		public CelestialBody hostBody;

		public Transform facilityTransform;

		public EditorFacility editorFacility;

		public SpawnPoint[] spawnPoints;

		public UpgradeableObject upgradeableObject;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpaceCenterFacility()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(PQS[] PQSs)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetSpawnPointsLatLonAlt()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpawnPoint GetSpawnPoint(string spawnPointName)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetFacilityDamage()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetFacilityLevel()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsLaunchFacility()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSetupSystem_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PSystemSetup _003C_003E4__this;

		private Transform _003CsphereTarget_003E5__2;

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
		public _003CSetupSystem_003Ed__35(int _003C_003E1__state)
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
	private sealed class _003CSetupLaunchSites_003Ed__38 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PSystemSetup _003C_003E4__this;

		private List<PQS> _003CspherestoSetup_003E5__2;

		private int _003ClsI_003E5__3;

		private LaunchSite _003ClaunchSite_003E5__4;

		private bool _003CforcePQSPosition_003E5__5;

		private PQS _003Csphere_003E5__6;

		private GameObject _003CtempObject_003E5__7;

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
		public _003CSetupLaunchSites_003Ed__38(int _003C_003E1__state)
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

	public GameScenes nextScene;

	private bool loadTestScene;

	public string pqsToActivate;

	public string pqsTransformToCache;

	[SerializeField]
	private SpaceCenterFacility[] facilities;

	[SerializeField]
	private List<SpaceCenterFacility> spaceCenterFacilityLaunchSites;

	internal PQS[] pqsArray;

	[SerializeField]
	private LaunchSite[] stocklaunchsites;

	[SerializeField]
	private List<LaunchSite> launchsites;

	[SerializeField]
	internal GameObject mobileLaunchSitePrefab;

	private PQS pqs;

	private PQSMod_CelestialBodyTransform cb;

	private Transform scTransform;

	public static PSystemSetup Instance
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

	public List<SpaceCenterFacility> SpaceCenterFacilityLaunchSites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public SpaceCenterFacility[] SpaceCenterFacilities
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

	public LaunchSite[] StockLaunchSites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public LaunchSite[] NonStockLaunchSites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<LaunchSite> LaunchSites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform SCTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PSystemSetup()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUpgradeableObjLevelChange(UpgradeableObject upgradeObject, int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetupSystem_003Ed__35))]
	private IEnumerator SetupSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPSystemReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFacilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetupLaunchSites_003Ed__38))]
	private IEnumerator SetupLaunchSites()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnAdditionalPrefabs(bool setupSuccess, LaunchSite launchSite, GameObject PQSParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject FindPQS(string pqsName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsStockLaunchSite(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFacilityOrLaunchSite(string inputName, out bool isFacility, out string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFacility(string inputName, out string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsLaunchSite(string inputName, out string displayName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNonStockLaunchSites()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchSite GetLaunchSite(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBody GetLaunchSiteBody(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLaunchSiteBodyName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLaunchSiteDisplayName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddLaunchSite(LaunchSite launchsite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool AddLaunchSite(LaunchSite launchsite, bool overRide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveLaunchSite(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceCenterFacility GetSpaceCenterFacility(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadTestScene(GameScenes sceneBase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnSceneChange(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetMainMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSpaceCentre()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetMissionEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTrackingStation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDisabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ForceInitPQS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ForceInitPQS(PQS pqsIn, Transform target = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPQSInactive(bool keepActiveInFlight = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPQSActive(PQS pqs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPQSActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPQSDisabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetTransforms()
	{
		throw null;
	}
}
