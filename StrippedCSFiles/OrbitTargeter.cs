using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using Vectrosity;

[RequireComponent(typeof(PatchedConicRenderer))]
public class OrbitTargeter : MonoBehaviour
{
	private enum MenuDrawMode
	{
		OFF,
		PATCH,
		TARGET
	}

	public class Marker
	{
		public MapObject mo;

		public MapNode mn;

		public MapObject.ObjectType otype;

		public MapNode.ApproachNodeType atype;

		public Color color;

		public int pixelSize;

		public bool visible;

		public bool lineOk;

		public VectorLine line;

		public Vector3d pos;

		public OrbitTargeter orbitTargeter;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Marker(OrbitTargeter orbitTargeter, string name, Color color, int pixelSize)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Marker(OrbitTargeter orbitTargeter, string name, Orbit patch, MapObject.ObjectType otype, Color color, int pixelSize)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateVisible(MapNode mn, MapNode.IconData iData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3d OnUpdatePosition(MapNode mn)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateType(MapNode mn, MapNode.TypeData tData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Terminate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void NodeUpdate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DestroyLine()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DrawLine(bool draw3d)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DottedLineUpdate(ref VectorLine line, Vector3d N, Vector3d N2, string name, Color color)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(bool visible, bool lineOk)
		{
			throw null;
		}
	}

	public class AnDnMarker : Marker
	{
		public bool ascending;

		public Vector3d pos2;

		public double rInc;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AnDnMarker(OrbitTargeter orbitTargeter, bool ascending, Orbit patch)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(Vector3d pos, Vector3d pos2, double iInc, bool visible, bool lineOk)
		{
			throw null;
		}
	}

	public class ISectMarker : Marker
	{
		public int num;

		public bool target;

		public Vector3d pos2;

		public double separation;

		public double relSpeed;

		public double angle;

		public double UT;

		private static Color[] isectColors;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ISectMarker(OrbitTargeter orbitTargeter, int num, bool target, Orbit patch)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static ISectMarker()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(Vector3d pos, double separation, double relSpeed, double angle, double UT, bool visible)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(Vector3d pos, Vector3d pos2, bool visible, bool lineOk)
		{
			throw null;
		}
	}

	public class ClApprMarker : Marker
	{
		private bool target;

		public double separation;

		public double dT;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClApprMarker(OrbitTargeter orbitTargeter, bool target, Orbit patch)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(Vector3d pos, double separation, double dT, bool visible)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(Vector3d pos, bool visible)
		{
			throw null;
		}
	}

	public class CursorMarker : Marker
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public CursorMarker(OrbitTargeter orbitTargeter)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnUpdateCaption(MapNode mn, MapNode.CaptionData cData)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update()
		{
			throw null;
		}
	}

	private const float nodeEpsilon = 0.001f;

	private PatchedConicRenderer pcr;

	private PatchedConicRenderer tgtPCR;

	private PatchedConics.PatchCastHit screenCastHit;

	private PatchedConics.PatchCastHit menuCastHit;

	private OrbitRendererBase.OrbitCastHit orbitCastHit;

	public bool AllowPlaceManeuverNode;

	public static bool HasManeuverNode;

	private bool orbitHover;

	private bool menuHover;

	private MenuDrawMode menuDrawMode;

	private MapContextMenu ContextMenu;

	private OrbitDriver target;

	private PatchRendering pr;

	private Vessel host;

	private Orbit refPatch;

	private Orbit tgtRefPatch;

	private bool flightPlanningUnlocked;

	private int maxPatchesAheadForAutoWarp;

	private PatchedConicRenderer _pcr;

	private CursorMarker cursorMarker;

	private Vector3 menuScreenPos;

	private Rect menuRect;

	private List<Marker> markers;

	private bool draw3dLines;

	private const int dottedLineSegments = 128;

	private bool encountersTarget;

	private const int maxIterations = 20;

	private int iterations;

	private AnDnMarker anMarker;

	private AnDnMarker dnMarker;

	private ISectMarker isect1Marker;

	private ISectMarker isect1TgtMarker;

	private ISectMarker isect2Marker;

	private ISectMarker isect2TgtMarker;

	private ClApprMarker clApprMarker;

	private ClApprMarker clApprTgtMarker;

	private static string cacheAutoLOC_7001351;

	private static string cacheAutoLOC_7001352;

	public bool OrbitHover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitTargeter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OrbitTargeter()
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
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetETAString(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CursorInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetMenuMode(MenuDrawMode m, OrbitDriver tgt = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onContextMenuDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onViewInTrackingStationButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTrackingStationProceed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTrackingStationDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTargetCb(CelestialBody refBody, CelestialBody tgtCb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PatchRendering FindPatch(List<PatchRendering> patchList, int startIndex, int endIndex, CelestialBody refCb, CelestialBody tgtCb = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Orbit ReferencePatchSelect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Orbit getTargetReferencePatch(Orbit refPatch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private OrbitDriver TargetCastNodes(out OrbitRendererBase.OrbitCastHit orbitHit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private OrbitDriver TargetCastSplines(out OrbitRendererBase.OrbitCastHit orbitHit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTarget(OrbitDriver tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateISectMarkers(ref ISectMarker isectMarker, ref ISectMarker isectTgtMarker, int num, double EVpUT, double EVp, double EVs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateNodesAndVectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Marker CleanupMarker(Marker marker, bool force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void NodeCleanup(bool willDestroy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool DropInvalidTargets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanDrawAnyLines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanDrawAnyNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2d GetSeparations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearMenus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
