using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PQ : MonoBehaviour
{
	protected enum QuadEdge
	{
		North = 0,
		South = 1,
		East = 2,
		West = 3,
		Null = -1
	}

	public enum QuadChild
	{
		SouthWest = 0,
		SouthEast = 1,
		NorthWest = 2,
		NorthEast = 3,
		Null = -1
	}

	[Flags]
	public enum EdgeState
	{
		Reset = -1,
		NoLerps = 0,
		NorthLerp = 1,
		SouthLerp = 2,
		EastLerp = 4,
		WestLerp = 8
	}

	public delegate void QuadDelegate(PQ quad);

	public int id;

	public int subdivision;

	public PQS sphereRoot;

	public PQ quadRoot;

	public PQ parent;

	public PQ north;

	public PQ south;

	public PQ east;

	public PQ west;

	public PQ[] subNodes;

	public bool isActive;

	public bool isSubdivided;

	public bool isVisible;

	public bool isForcedInvisible;

	public bool isBuilt;

	public bool isQueuedForNormalUpdate;

	public bool isQueuedOnlyForCornerNormalUpdate;

	public bool isCached;

	public bool isPendingCollapse;

	[HideInInspector]
	public Mesh mesh;

	[HideInInspector]
	public Vector3[] verts;

	[HideInInspector]
	public Vector3[] vertNormals;

	[HideInInspector]
	public Vector3[][] edgeNormals;

	public Matrix4x4 quadMatrix;

	public QuaternionD planeRotation;

	public PQS.QuadPlane plane;

	public Vector3d positionPlanetRelative;

	public Vector3d positionPlanet;

	public Vector3d positionPlanePosition;

	public Vector3d positionParentRelative;

	public double scalePlanetRelative;

	public double scalePlaneRelative;

	public QuaternionD quadRotation;

	public double quadScaleFactor;

	public Vector3d quadScale;

	public double quadArea;

	public double meshVertMin;

	public double meshVertMax;

	private double angularinterval;

	public double subdivideThresholdFactor;

	public double gcDist;

	public double gcd1;

	public double gcd2;

	public PQS.EdgeState edgeState;

	private static PQS.EdgeState newEdgeState;

	public Vector2 uvSW;

	public Vector2 uvDelta;

	private static Vector2 uvMidPoint;

	private static Vector2 uvMidS;

	private static Vector2 uvMidW;

	private static Vector2 uvDel;

	public QuadDelegate onUpdate;

	public QuadDelegate onDestroy;

	public QuadDelegate onVisible;

	public QuadDelegate onInvisible;

	public MeshRenderer meshRenderer;

	public MeshFilter meshFilter;

	public MeshCollider meshCollider;

	public int quadIndex;

	public Transform quadTransform;

	private Vector3d PrecisePosition;

	private int Corner;

	public PQ CreateParent;

	private bool outOfTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQ()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupQuad(PQ parentQuad, QuadChild parentChildPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void FastUpdateSubQuadsPosition(Vector3d movement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void PreciseUpdateSubQuadsPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTargetRelativity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSubdivision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSubdivisionInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Build()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Subdivide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearAndDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInvisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMasterVisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMasterInvisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("QueueForNormalUpdate")]
	public void QueueForNormalUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void QueueForCornerNormalUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnqueueForNormalUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQS.EdgeState GetEdgeState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEdgeState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetEdgeQuads(PQ caller, out PQ left, out PQ right)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQS.QuadEdge GetEdge(PQ caller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQ GetRightmostCornerPQ(PQ nextQuad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQ GetSidePQ(PQS.QuadEdge edge)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNeighbour(PQ oldNeighbour, PQ newNeighbour)
	{
		throw null;
	}
}
