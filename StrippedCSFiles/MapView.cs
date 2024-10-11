using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using KSP.UI.Util;
using UnityEngine;

public class MapView : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__79 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MapView _003C_003E4__this;

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
		public _003CStart_003Ed__79(int _003C_003E1__state)
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

	public bool ConstantMode;

	public Camera[] uiCameras;

	public MonoBehaviour[] scriptsToDisable;

	public float transitionDuration;

	public Material orbitLinesMaterial;

	public static Material OrbitLinesMaterialStatic;

	public Material orbitIconsMaterial;

	public Material dottedLineMaterial;

	public static Material DottedLineMaterialStatic;

	public GUISkin orbitIconsTextSkin;

	public UISkinDefSO orbitIconsTextSkinDef;

	public Texture2D orbitIconsMap;

	public GameObject maneuverNodePrefab;

	public float max3DlineDrawDist;

	public Color[] patchColors;

	public Color[] targetPatchColors;

	public MapNode uiNodePrefab;

	private static bool _mapIsEnabled;

	public static bool ReportMapAsDisabled;

	public static int displayedOrbits;

	public static Callback OnEnterMapView;

	public static Callback OnExitMapView;

	private Camera vectorCam;

	[SerializeField]
	private AnimationCurve splineEccentricOffset;

	protected bool updateMap;

	private float camDistance;

	private float camHdg;

	private float camPitch;

	private int camFocusTarget;

	private FlightCamera mainCamera;

	private Transform spaceCameraHome;

	protected bool draw3Dlines;

	private bool highOrbitCountMode;

	public MapObject scaledVessel;

	protected Camera mapFxCameraNear;

	protected Camera mapFxCameraFar;

	protected bool started;

	public List<SiteNode> siteNodes;

	[SerializeField]
	private KSCSiteNode kscSiteNode;

	public static MapView fetch
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

	public static Material OrbitLinesMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Material DottedLinesMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Material OrbitIconsMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static GUISkin OrbitIconsTextSkin
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static UISkinDef OrbitIconsTextSkinDef
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Texture2D OrbitIconsMap
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static PlanetariumCamera MapCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static GameObject ManeuverNodePrefab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool Draw3DLines
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool HighOrbitCountMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Color[] PatchColors
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Color[] TargetPatchColors
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("Use NavBallToggle.Instance.ManeuverModeActive instead")]
	public static bool ManeuverModeActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static MapNode UINodePrefab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool MapIsEnabled
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

	public Camera VectorCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AnimationCurve SplineEccentricOffset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected internal virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__79))]
	public IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Camera CreateVectorCanvasCam()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Camera createMapFXCamera(int depthOffset, float nearPlane, float farPlane)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateMap(bool forceUpdate = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateOrbitRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateLaunchSiteNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddLaunchSiteNode(LaunchSite launchSite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateKSCNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void EnterMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ExitMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void enterMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void endEnterMapTransition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void exitMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void endExitMapTransition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void enableMapFXCameras(bool enabledState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetEccOffset(float eccOffset, float ecc, float eccOffsetPower)
	{
		throw null;
	}
}
