using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KerbNet;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Dialogs;

public class KerbNetDialog : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CAutoRefreshRoutine_003Ed__123 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float duration;

		public KerbNetDialog _003C_003E4__this;

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
		public _003CAutoRefreshRoutine_003Ed__123(int _003C_003E1__state)
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

	public Vessel DisplayVessel;

	private bool display;

	public const string LockID = "KerbNetDialog";

	private bool _inputLocked;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private UIStateButton modeButton;

	[SerializeField]
	private CanvasRenderer modeButtonRenderer;

	[SerializeField]
	private TextMeshProUGUI modeButtonText;

	[SerializeField]
	private UIStateButton visibilityButton;

	[SerializeField]
	private UIStateButton autoRefreshButton;

	[SerializeField]
	private Sprite modeErrorButtonSprite;

	[SerializeField]
	private GameObject mapTexture;

	[SerializeField]
	private WaypointTarget waypointTarget;

	[SerializeField]
	private TextMeshProUGUI errorText;

	[SerializeField]
	private GameObject[] disableOnError;

	[SerializeField]
	private GameObject[] enableOnError;

	[SerializeField]
	private CanvasRenderer gridRenderer;

	[SerializeField]
	private CanvasRenderer waypointRenderer;

	[SerializeField]
	private TextMeshProUGUI bodyText;

	[SerializeField]
	private TextMeshProUGUI infoLabel;

	[SerializeField]
	private TextMeshProUGUI infoText;

	[SerializeField]
	private TextMeshProUGUI targetText;

	[SerializeField]
	private TextMeshProUGUI centerText;

	[SerializeField]
	private Slider fovSlider;

	[SerializeField]
	private TextMeshProUGUI fovText;

	[SerializeField]
	private TMP_InputField waypointField;

	[SerializeField]
	private Button customButton;

	[SerializeField]
	private TextMeshProUGUI customButtonText;

	[SerializeField]
	private TooltipController_Text customButtonTooltip;

	[SerializeField]
	private Button refreshButton;

	[SerializeField]
	private Button waypointButton;

	public IAccessKerbNet KerbNetAccessor;

	[HideInInspector]
	public static List<KerbNetMode> resourceDisplayModes;

	[HideInInspector]
	public static Dictionary<string, KerbNetMode> knownDisplayModes;

	[HideInInspector]
	public List<KerbNetMode> activeDisplayModes;

	[HideInInspector]
	public KerbNetMode currentDisplayMode;

	[HideInInspector]
	public double centerLatitude;

	[HideInInspector]
	public double centerLongitude;

	[HideInInspector]
	public double waypointLatitude;

	[HideInInspector]
	public double waypointLongitude;

	public Vector3d localVesselPos;

	public Vector3d cameraX;

	public Vector3d cameraY;

	public Vector3d cameraZ;

	public float fovMin;

	public float fovMax;

	public float fovCurrent;

	public double fovScale;

	private const int mapSize = 257;

	private const int halfMapSize = 128;

	public float visibilitySpeed;

	private float gridAlphaTarget;

	private float waypointAlphaTarget;

	public float AnomalyChance;

	private bool beenSetup;

	public bool delayedErrorRefresh;

	private Coroutine autoRefreshRoutine;

	private static Texture2D mapTexture2D;

	private static Color32[] mapResetArray;

	private byte[,] questionArray;

	private static string cacheAutoLOC_438370;

	private static string cacheAutoLOC_438382;

	private static string cacheAutoLOC_438390;

	private static string cacheAutoLOC_438413;

	private static string cacheAutoLOC_438415;

	private static string cacheAutoLOC_438467;

	private static string cacheAutoLOC_6001959;

	private static string cacheAutoLOC_258912;

	public static KerbNetDialog Instance
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

	public static bool isDisplaying
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InputLocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	[HideInInspector]
	public bool RecoveryRefreshQueued
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

	[HideInInspector]
	public bool HasError
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

	[HideInInspector]
	public string ErrorState
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

	public int Seed
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

	public KSPRandom Generator
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
	public KerbNetDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D GetTexture()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KerbNetDialog Display(IAccessKerbNet accessor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FullRefresh(bool refreshMap, bool refreshErrors = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshDataLabels(Vector2 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupModeButton(List<KerbNetMode> modes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshCustomButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFoVBounds(float min, float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFoVSlider(bool center)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFoVText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDestroyed(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselStatusChanged(Vessel vessel, bool data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModeChanged(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVisibilityChanged(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFoVSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWaypointFieldEndEdit(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRefreshClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWaypointClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupDisplayModes(List<string> askedDisplayModesNames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupAllResourceDisplayModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KerbNetMode GetModeByName(string modeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateAssemblyModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateResourceModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateKnownModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddKnownMode(KerbNetMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateDisplayMode(KerbNetMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivateDisplayMode(KerbNetMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAutoRefreshChanged(UIStateButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartAutoRefresh(float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StopAutoRefresh(string notification = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CAutoRefreshRoutine_003Ed__123))]
	private IEnumerator AutoRefreshRoutine(float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCameraVectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetScanLatitudeAndLongitude(int x, int y, out double latitude, out double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ScanSpaceLocation(Vector3d pos, out Vector2 cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D CreateMapTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawContourLines(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawAnomaliesOnMap(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasActiveConnection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasVesselControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasValidDisplayMode(out string error)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasValidAccessor(out string error)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshErrorState(IAccessKerbNet accessor = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetActivesForError(bool error)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ChangeMapPosition(Vector2 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetMapPosition(bool showDragTip = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RandomizeWaypointField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ShouldShowAnomaly(PQSSurfaceObject anomaly, float chance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float NormalizedDistanceFromCenter(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
