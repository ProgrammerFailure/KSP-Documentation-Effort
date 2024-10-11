using System.Runtime.CompilerServices;
using Expansions.Missions;
using Expansions.Missions.Actions;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using Vectrosity;

public class OrbitRendererBase : MonoBehaviour
{
	public enum DrawMode
	{
		OFF,
		REDRAW_ONLY,
		REDRAW_AND_FOLLOW,
		REDRAW_AND_RECALCULATE
	}

	public enum DrawIcons
	{
		NONE,
		OBJ,
		OBJ_PE_AP,
		ALL
	}

	public struct OrbitCastHit : IScreenCaster
	{
		public Vector3 orbitOrigin;

		public Vector3 hitPoint;

		public Vector3 orbitPoint;

		public Vector3 orbitScreenPoint;

		public double mouseTA;

		public double radiusAtTA;

		public double UTatTA;

		public OrbitRendererBase or;

		public OrbitDriver driver;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetUpdatedOrbitPoint()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetScreenSpacePoint()
		{
			throw null;
		}
	}

	public OrbitDriver driver;

	private bool nodesAttached;

	public Vessel vessel;

	public CelestialBody celestialBody;

	public MENode meNode;

	public DiscoveryInfo discoveryInfo;

	protected static double sampleResolution;

	public Color orbitColor;

	public Color nodeColor;

	public float lowerCamVsSmaRatio;

	public float upperCamVsSmaRatio;

	public bool drawNodes;

	public bool isFocused;

	public bool autoTextureOffset;

	public float textureOffset;

	public DrawMode drawMode;

	public DrawIcons drawIcons;

	private double st;

	private double end;

	private double rng;

	private double itv;

	private Vector3d[] orbitPoints;

	private float CamVsSmaRatio;

	protected float lineOpacity;

	protected VectorLine orbitLine;

	protected bool draw3dLines;

	private MapObject objectMO;

	private MapObject DescMO;

	private MapObject AscMO;

	private MapObject ApMO;

	private MapObject PeMO;

	private ActionCreateVessel cachedCreateVessel;

	private ActionCreateAsteroid cachedCreateAsteroid;

	private ActionCreateComet cachedCreateComet;

	private ActionCreateKerbal cachedCreateKerbal;

	protected internal MapNode objectNode;

	protected internal MapNode descNode;

	protected internal MapNode ascNode;

	protected internal MapNode apNode;

	protected internal MapNode peNode;

	protected Transform nodesParent;

	protected int layerMask;

	public EventData<Vessel> onVesselIconClicked;

	public EventData<CelestialBody> onCelestialBodyIconClicked;

	protected double eccOffset;

	protected float twkOffset;

	public bool EccOffsetInvert;

	private static string cacheAutoLOC_196762;

	private static string cacheAutoLOC_196878;

	private static string cacheAutoLOC_196888;

	private static string cacheAutoLOC_196893;

	private static string cacheAutoLOC_7001411;

	public bool isActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool mouseOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string objName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Vector3d[] OrbitPoints
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected set
		{
			throw null;
		}
	}

	public VectorLine OrbitLine
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected Orbit orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitRendererBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OrbitRendererBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected internal void MakeLine(ref VectorLine l)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateSpline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DrawOrbit(DrawMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Color GetNodeColour()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Color GetOrbitColour()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SplineOpacityUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsRenderableOrbit(Orbit o, CelestialBody tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getCamVsSmaRatio(double SMA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DrawSpline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected internal int GetSegmentCount(double sampleResolution)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RefreshMapObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindMapObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateScaledSpaceNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AttachNodeUIs(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void objectNode_OnClick(MapNode mn, Mouse.Buttons btns)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DetachNodeUIs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyScaledSpaceNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void objectNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void objectNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3d objectNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void objectNode_OnUpdateType(MapNode n, MapNode.TypeData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3d ascNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3d descNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ascNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void descNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ANDNNodes_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3d apNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void apNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ApNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3d peNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PeNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void peNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CanDrawAnyIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual DrawIcons GetCurrentDrawMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool HaveStateVectorKnowledge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool OrbitCast(Vector3 screenPos, out OrbitCastHit hitInfo, float orbitPixelWidth = 10f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool OrbitCast(Vector3 screenPos, Camera cam, out OrbitCastHit hitInfo, float orbitPixelWidth = 10f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetMouseOverNode(Vector3 worldPos, float iconSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
