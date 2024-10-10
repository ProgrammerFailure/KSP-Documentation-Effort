using System;
using UnityEngine;

public class GClass3 : MonoBehaviour
{
	public enum QuadEdge
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

	public delegate void QuadDelegate(GClass3 quad);

	public int id;

	public int subdivision;

	public GClass4 sphereRoot;

	public GClass3 quadRoot;

	public GClass3 parent;

	public GClass3 north;

	public GClass3 south;

	public GClass3 east;

	public GClass3 west;

	public GClass3[] subNodes;

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

	public GClass4.QuadPlane plane;

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

	public double angularinterval;

	public double subdivideThresholdFactor;

	public double gcDist;

	public double gcd1;

	public double gcd2;

	public GClass4.EdgeState edgeState;

	public static GClass4.EdgeState newEdgeState;

	public Vector2 uvSW;

	public Vector2 uvDelta;

	public static Vector2 uvMidPoint;

	public static Vector2 uvMidS;

	public static Vector2 uvMidW;

	public static Vector2 uvDel;

	public QuadDelegate onUpdate;

	public QuadDelegate onDestroy;

	public QuadDelegate onVisible;

	public QuadDelegate onInvisible;

	public MeshRenderer meshRenderer;

	public MeshFilter meshFilter;

	public MeshCollider meshCollider;

	public int quadIndex;

	public Transform quadTransform;

	public Vector3d PrecisePosition;

	public int Corner;

	public GClass3 CreateParent;

	public bool outOfTime;

	public void Awake()
	{
		quadTransform = base.transform;
		subNodes = new GClass3[4];
		isBuilt = false;
		isVisible = false;
		edgeState = GClass4.EdgeState.Reset;
	}

	public void SetupQuad(GClass3 parentQuad, QuadChild parentChildPosition)
	{
		if (north == null || south == null || east == null || west == null)
		{
			Debug.LogError("SetupQuad: " + base.gameObject.name);
			Debug.Break();
			Debug.DebugBreak();
		}
		angularinterval = sphereRoot.circumference * Math.Atan(scalePlaneRelative / sphereRoot.radius);
		quadRotation = Quaternion.FromToRotation(Vector3.up, positionPlanetRelative);
		if (quadRoot != null)
		{
			quadScaleFactor = 1.0 / Math.Pow(2.0, subdivision - 1);
			quadArea = quadRoot.quadArea * quadScaleFactor;
			quadScale = Vector3d.one * quadScaleFactor;
			quadMatrix = Matrix4x4.TRS(positionPlanePosition, quadRoot.planeRotation, quadScale);
			if (sphereRoot.surfaceRelativeQuads)
			{
				quadTransform.localPosition = positionPlanet;
				quadTransform.localRotation = Quaternion.identity;
				if (subdivision == sphereRoot.maxLevel)
				{
					sphereRoot.AddPQToLocalSpaceStorage(this);
					quadTransform.parent = sphereRoot.LocalSpacePQStorage.transform;
				}
				else
				{
					quadTransform.parent = sphereRoot.transform;
				}
				PrecisePosition = quadTransform.position;
				quadTransform.localScale = Vector3.one;
			}
			else
			{
				quadTransform.localPosition = Vector3.zero;
				quadTransform.localRotation = Quaternion.identity;
				if (subdivision == sphereRoot.maxLevel)
				{
					sphereRoot.AddPQToLocalSpaceStorage(this);
					quadTransform.parent = sphereRoot.LocalSpacePQStorage.transform;
				}
				else
				{
					quadTransform.parent = sphereRoot.transform;
				}
				PrecisePosition = quadTransform.position;
			}
		}
		else
		{
			planeRotation = Quaternion.FromToRotation(Vector3.up, positionPlanePosition.normalized);
			quadScaleFactor = 2.0;
			quadArea = Math.Pow(sphereRoot.radius * 2.0, 2.0);
			quadScale = Vector3d.one * quadScaleFactor;
			quadMatrix = Matrix4x4.TRS(positionPlanePosition, planeRotation, quadScale);
			if (sphereRoot.surfaceRelativeQuads)
			{
				quadTransform.localPosition = positionPlanet;
				quadTransform.localRotation = Quaternion.identity;
				quadTransform.localScale = Vector3.one;
			}
			else
			{
				quadTransform.localPosition = Vector3.zero;
				quadTransform.localRotation = Quaternion.identity;
			}
		}
		if (parentQuad == null)
		{
			quadIndex = 0;
		}
		else if (subdivision == 1)
		{
			quadIndex = (int)(parentQuad.quadIndex + parentChildPosition);
		}
		else
		{
			quadIndex = parentQuad.quadIndex + ((int)parentChildPosition << 2 * (subdivision - 1));
		}
		UpdateTargetRelativity();
	}

	public void FastUpdateSubQuadsPosition(Vector3d movement)
	{
		PrecisePosition += movement;
		quadTransform.position = PrecisePosition;
	}

	public void PreciseUpdateSubQuadsPosition()
	{
		quadTransform.parent = sphereRoot.transform;
		int num = Corner % 2;
		int num2 = Corner / 2;
		positionParentRelative = quadRoot.planeRotation * new Vector3d(((double)num - 0.5) * CreateParent.scalePlaneRelative, 0.0, ((double)num2 - 0.5) * CreateParent.scalePlaneRelative);
		positionPlanePosition = CreateParent.positionPlanePosition + positionParentRelative;
		positionPlanetRelative = positionPlanePosition.normalized;
		positionPlanet = positionPlanetRelative * sphereRoot.GetSurfaceHeight(positionPlanetRelative);
		quadTransform.localPosition = positionPlanet;
		quadTransform.parent = sphereRoot.LocalSpacePQStorage.transform;
		PrecisePosition = quadTransform.position;
	}

	public void UpdateTargetRelativity()
	{
		gcd1 = Math.Acos(Vector3d.Dot(positionPlanetRelative, sphereRoot.relativeTargetPositionNormalized)) * sphereRoot.radius * 1.3;
		gcDist = gcd1 + Math.Abs(sphereRoot.targetHeight) - angularinterval;
	}

	public void UpdateVisibility()
	{
		if (!isSubdivided && gcd1 < sphereRoot.visibleRadius)
		{
			SetVisible();
			newEdgeState = GetEdgeState();
			if (newEdgeState != edgeState)
			{
				mesh.triangles = GClass4.cacheIndices[(int)newEdgeState];
				edgeState = newEdgeState;
				QueueForNormalUpdate();
			}
		}
		else
		{
			SetInvisible();
		}
	}

	public void UpdateSubdivision()
	{
		UpdateTargetRelativity();
		outOfTime = Time.realtimeSinceStartup > sphereRoot.maxFrameEnd;
		isPendingCollapse = false;
		if (isSubdivided)
		{
			meshRenderer.enabled = false;
			bool flag = gcDist > sphereRoot.collapseThresholds[subdivision] * subdivideThresholdFactor;
			if (subdivision <= sphereRoot.maxLevel && (!flag || outOfTime))
			{
				if (flag)
				{
					isPendingCollapse = true;
				}
				for (int i = 0; i < 4; i++)
				{
					if ((bool)subNodes[i])
					{
						subNodes[i].UpdateSubdivision();
					}
				}
			}
			else if (!Collapse())
			{
				for (int j = 0; j < 4; j++)
				{
					if ((bool)subNodes[j])
					{
						subNodes[j].UpdateSubdivision();
					}
				}
			}
		}
		else if (subdivision >= sphereRoot.minLevel && (!(gcDist < sphereRoot.subdivisionThresholds[subdivision] * subdivideThresholdFactor) || subdivision >= sphereRoot.maxLevelAtCurrentTgtSpeed || outOfTime))
		{
			UpdateVisibility();
		}
		else
		{
			Subdivide();
		}
		if (onUpdate != null)
		{
			onUpdate(this);
		}
	}

	public void UpdateSubdivisionInit()
	{
		UpdateTargetRelativity();
		if (isSubdivided)
		{
			if (subdivision > sphereRoot.minLevel && (subdivision > sphereRoot.maxLevel || gcDist > sphereRoot.collapseThresholds[subdivision] * subdivideThresholdFactor))
			{
				Collapse();
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if ((bool)subNodes[i])
				{
					subNodes[i].UpdateSubdivisionInit();
				}
			}
		}
		else if (gcDist < sphereRoot.subdivisionThresholds[subdivision] * subdivideThresholdFactor && subdivision < sphereRoot.maxLevel)
		{
			Subdivide();
		}
		else if (onUpdate != null)
		{
			onUpdate(this);
		}
	}

	public void Build()
	{
		if (isBuilt || !isActive || !sphereRoot.quadAllowBuild || isCached)
		{
			return;
		}
		if (isSubdivided)
		{
			for (int i = 0; i < 4; i++)
			{
				subNodes[i].Build();
			}
			return;
		}
		isBuilt = sphereRoot.BuildQuad(this);
		if (isBuilt)
		{
			QueueForNormalUpdate();
		}
	}

	public bool Subdivide()
	{
		if (north.subdivision < subdivision)
		{
			return false;
		}
		if (east.subdivision < subdivision)
		{
			return false;
		}
		if (south.subdivision < subdivision)
		{
			return false;
		}
		if (west.subdivision < subdivision)
		{
			return false;
		}
		if (isSubdivided)
		{
			return true;
		}
		if (!isActive)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			if (subNodes[i] != null)
			{
				subNodes[i].isActive = true;
				Debug.Log(subNodes[i].gameObject.name);
				Debug.Break();
				continue;
			}
			GClass3 gClass = sphereRoot.AssignQuad(subdivision + 1);
			int num = i % 2;
			int num2 = i / 2;
			gClass.scalePlaneRelative = scalePlaneRelative * 0.5;
			gClass.scalePlanetRelative = sphereRoot.radius * gClass.scalePlaneRelative;
			if (quadRoot == null)
			{
				gClass.quadRoot = this;
			}
			else
			{
				gClass.quadRoot = quadRoot;
			}
			gClass.CreateParent = this;
			gClass.positionParentRelative = gClass.quadRoot.planeRotation * new Vector3d(((double)num - 0.5) * scalePlaneRelative, 0.0, ((double)num2 - 0.5) * scalePlaneRelative);
			gClass.positionPlanePosition = positionPlanePosition + gClass.positionParentRelative;
			gClass.positionPlanetRelative = gClass.positionPlanePosition.normalized;
			gClass.positionPlanet = gClass.positionPlanetRelative * sphereRoot.GetSurfaceHeight(gClass.positionPlanetRelative);
			gClass.plane = plane;
			gClass.sphereRoot = sphereRoot;
			gClass.subdivision = subdivision + 1;
			gClass.parent = this;
			gClass.Corner = i;
			gClass.name = base.gameObject.name + i;
			gClass.gameObject.layer = base.gameObject.layer;
			sphereRoot.QuadCreated(gClass);
			subNodes[i] = gClass;
		}
		subNodes[0].north = subNodes[2];
		subNodes[0].east = subNodes[1];
		subNodes[1].north = subNodes[3];
		subNodes[1].west = subNodes[0];
		subNodes[2].south = subNodes[0];
		subNodes[2].east = subNodes[3];
		subNodes[3].south = subNodes[1];
		subNodes[3].west = subNodes[2];
		GClass3 left;
		GClass3 right;
		if (north.subdivision == subdivision && north.isSubdivided)
		{
			north.GetEdgeQuads(this, out left, out right);
			subNodes[2].north = left;
			subNodes[3].north = right;
			left.SetNeighbour(this, subNodes[2]);
			right.SetNeighbour(this, subNodes[3]);
		}
		else
		{
			subNodes[2].north = north;
			subNodes[3].north = north;
		}
		if (south.subdivision == subdivision && south.isSubdivided)
		{
			south.GetEdgeQuads(this, out left, out right);
			subNodes[1].south = left;
			subNodes[0].south = right;
			left.SetNeighbour(this, subNodes[1]);
			right.SetNeighbour(this, subNodes[0]);
		}
		else
		{
			subNodes[1].south = south;
			subNodes[0].south = south;
		}
		if (east.subdivision == subdivision && east.isSubdivided)
		{
			east.GetEdgeQuads(this, out left, out right);
			subNodes[3].east = left;
			subNodes[1].east = right;
			left.SetNeighbour(this, subNodes[3]);
			right.SetNeighbour(this, subNodes[1]);
		}
		else
		{
			subNodes[3].east = east;
			subNodes[1].east = east;
		}
		if (west.subdivision == subdivision && west.isSubdivided)
		{
			west.GetEdgeQuads(this, out left, out right);
			subNodes[0].west = left;
			subNodes[2].west = right;
			left.SetNeighbour(this, subNodes[0]);
			right.SetNeighbour(this, subNodes[2]);
		}
		else
		{
			subNodes[0].west = west;
			subNodes[2].west = west;
		}
		if (sphereRoot.reqUVQuad)
		{
			uvDel = uvDelta * 0.5f;
			uvMidPoint.x = uvSW.x + uvDel.x;
			uvMidPoint.y = uvSW.y + uvDel.y;
			uvMidS.x = uvMidPoint.x;
			uvMidS.y = uvSW.y;
			uvMidW.x = uvSW.x;
			uvMidW.y = uvMidPoint.y;
			subNodes[0].uvSW = uvMidPoint;
			subNodes[0].uvDelta = uvDel;
			subNodes[1].uvSW = uvMidW;
			subNodes[1].uvDelta = uvDel;
			subNodes[2].uvSW = uvMidS;
			subNodes[2].uvDelta = uvDel;
			subNodes[3].uvSW = uvSW;
			subNodes[3].uvDelta = uvDel;
		}
		isSubdivided = true;
		SetInvisible();
		for (int i = 0; i < 4; i++)
		{
			if (subNodes[i].north == null || subNodes[i].south == null || subNodes[i].east == null || subNodes[i].west == null)
			{
				Debug.Log("Subdivide: " + base.gameObject.name + " " + i);
				Debug.Break();
			}
			if (!subNodes[i].isCached)
			{
				subNodes[i].SetupQuad(this, (QuadChild)i);
			}
			if (sphereRoot.quadAllowBuild)
			{
				subNodes[i].UpdateVisibility();
			}
		}
		north.QueueForNormalUpdate();
		south.QueueForNormalUpdate();
		east.QueueForNormalUpdate();
		west.QueueForNormalUpdate();
		GClass3 rightmostCornerPQ = GetRightmostCornerPQ(north);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(west);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(south);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(east);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		return true;
	}

	public bool Collapse()
	{
		if (sphereRoot.normalUpdateList.Contains(this))
		{
			UnqueueForNormalUpdate();
		}
		if (!isSubdivided)
		{
			return false;
		}
		if (north.subdivision > subdivision)
		{
			return false;
		}
		if (south.subdivision > subdivision)
		{
			return false;
		}
		if (east.subdivision > subdivision)
		{
			return false;
		}
		if (west.subdivision > subdivision)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			if (subdivision + 1 == sphereRoot.maxLevel)
			{
				sphereRoot.RemovePQFromLocalSpaceStorage(subNodes[i]);
			}
			if (subNodes[i].isCached)
			{
				subNodes[i].SetInvisible();
				subNodes[i].isActive = false;
				if (sphereRoot.normalUpdateList.Contains(subNodes[i]))
				{
					subNodes[i].UnqueueForNormalUpdate();
				}
			}
			else
			{
				subNodes[i].ClearAndDestroy();
				subNodes[i] = null;
			}
		}
		if (north == null)
		{
			north = parent.north;
		}
		if (south == null)
		{
			south = parent.south;
		}
		if (east == null)
		{
			east = parent.east;
		}
		if (west == null)
		{
			west = parent.west;
		}
		isSubdivided = false;
		UpdateVisibility();
		north.QueueForNormalUpdate();
		south.QueueForNormalUpdate();
		east.QueueForNormalUpdate();
		west.QueueForNormalUpdate();
		GClass3 rightmostCornerPQ = GetRightmostCornerPQ(north);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(west);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(south);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		rightmostCornerPQ = GetRightmostCornerPQ(east);
		if (rightmostCornerPQ != null)
		{
			rightmostCornerPQ.QueueForCornerNormalUpdate();
		}
		return true;
	}

	public void ClearAndDestroy()
	{
		if (isSubdivided)
		{
			for (int i = 0; i < 4; i++)
			{
				if (subdivision + 1 == sphereRoot.maxLevel)
				{
					sphereRoot.RemovePQFromLocalSpaceStorage(subNodes[i]);
				}
				if (subNodes[i] != null)
				{
					subNodes[i].ClearAndDestroy();
					subNodes[i] = null;
				}
			}
		}
		if (north != null && north.subdivision == subdivision)
		{
			north.SetNeighbour(this, parent);
		}
		if (east != null && east.subdivision == subdivision)
		{
			east.SetNeighbour(this, parent);
		}
		if (south != null && south.subdivision == subdivision)
		{
			south.SetNeighbour(this, parent);
		}
		if (west != null && west.subdivision == subdivision)
		{
			west.SetNeighbour(this, parent);
		}
		if (sphereRoot.normalUpdateList.Contains(this))
		{
			UnqueueForNormalUpdate();
		}
		if (onDestroy != null)
		{
			onDestroy(this);
		}
		sphereRoot.DestroyQuad(this);
	}

	public void Destroy()
	{
		if (isSubdivided)
		{
			for (int i = 0; i < 4; i++)
			{
				if (subdivision + 1 == sphereRoot.maxLevel)
				{
					sphereRoot.RemovePQFromLocalSpaceStorage(subNodes[i]);
				}
				if (subNodes[i] != null)
				{
					subNodes[i].Destroy();
					subNodes[i] = null;
				}
			}
		}
		if (sphereRoot.normalUpdateList.Contains(this))
		{
			UnqueueForNormalUpdate();
		}
		if (onDestroy != null)
		{
			onDestroy(this);
		}
		sphereRoot.DestroyQuad(this);
	}

	public void SetCache()
	{
		isCached = true;
		if (isSubdivided)
		{
			for (int i = 0; i < 4; i++)
			{
				subNodes[i].SetCache();
			}
		}
	}

	public void ClearCache()
	{
		isCached = false;
		if (isSubdivided)
		{
			for (int i = 0; i < 4; i++)
			{
				subNodes[i].ClearCache();
			}
		}
	}

	public void SetVisible()
	{
		if (!isVisible)
		{
			isVisible = true;
			if (!isBuilt)
			{
				Build();
			}
			if (isForcedInvisible)
			{
				meshRenderer.enabled = false;
			}
			else
			{
				meshRenderer.enabled = true;
			}
			if (onVisible != null)
			{
				onVisible(this);
			}
		}
	}

	public void SetInvisible()
	{
		if (isVisible)
		{
			isVisible = false;
			meshRenderer.enabled = false;
			if (onInvisible != null)
			{
				onInvisible(this);
			}
		}
	}

	public void SetMasterVisible()
	{
		if (isVisible && !isForcedInvisible)
		{
			meshRenderer.enabled = true;
		}
		if (isSubdivided)
		{
			for (int num = 3; num >= 0; num--)
			{
				subNodes[num].SetMasterVisible();
			}
		}
	}

	public void SetMasterInvisible()
	{
		if (isVisible)
		{
			meshRenderer.enabled = false;
		}
		if (isSubdivided)
		{
			for (int num = 3; num >= 0; num--)
			{
				subNodes[num].SetMasterInvisible();
			}
		}
	}

	[ContextMenu("QueueForNormalUpdate")]
	public void QueueForNormalUpdate()
	{
		if (sphereRoot != null && sphereRoot.reqCustomNormals && !isQueuedForNormalUpdate && sphereRoot.quadAllowBuild)
		{
			sphereRoot.normalUpdateList.Add(this);
			isQueuedForNormalUpdate = true;
		}
	}

	public void QueueForCornerNormalUpdate()
	{
		if (sphereRoot != null && sphereRoot.reqCustomNormals && !isQueuedForNormalUpdate && sphereRoot.quadAllowBuild)
		{
			sphereRoot.normalUpdateList.Add(this);
			isQueuedForNormalUpdate = true;
			isQueuedOnlyForCornerNormalUpdate = true;
		}
	}

	public void UnqueueForNormalUpdate()
	{
		if (sphereRoot != null && isQueuedForNormalUpdate)
		{
			sphereRoot.normalUpdateList.Remove(this);
			isQueuedForNormalUpdate = false;
			isQueuedOnlyForCornerNormalUpdate = false;
		}
	}

	public GClass4.EdgeState GetEdgeState()
	{
		GClass4.EdgeState edgeState = GClass4.EdgeState.NoLerps;
		if (north.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.NorthLerp;
		}
		if (south.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.SouthLerp;
		}
		if (east.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.EastLerp;
		}
		if (west.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.WestLerp;
		}
		return edgeState;
	}

	public void SetEdgeState()
	{
		edgeState = GClass4.EdgeState.NoLerps;
		if (north.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.NorthLerp;
		}
		if (south.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.SouthLerp;
		}
		if (east.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.EastLerp;
		}
		if (west.subdivision < subdivision)
		{
			edgeState |= GClass4.EdgeState.WestLerp;
		}
	}

	public void GetEdgeQuads(GClass3 caller, out GClass3 left, out GClass3 right)
	{
		left = null;
		right = null;
		if (north == caller)
		{
			left = subNodes[3];
			right = subNodes[2];
		}
		else if (south == caller)
		{
			left = subNodes[0];
			right = subNodes[1];
		}
		else if (east == caller)
		{
			left = subNodes[1];
			right = subNodes[3];
		}
		else if (west == caller)
		{
			left = subNodes[2];
			right = subNodes[0];
		}
		else if (north == caller.parent)
		{
			left = subNodes[3];
			right = subNodes[2];
		}
		else if (south == caller.parent)
		{
			left = subNodes[0];
			right = subNodes[1];
		}
		else if (east == caller.parent)
		{
			left = subNodes[1];
			right = subNodes[3];
		}
		else if (west == caller.parent)
		{
			left = subNodes[2];
			right = subNodes[0];
		}
	}

	public GClass4.QuadEdge GetEdge(GClass3 caller)
	{
		if (caller == north)
		{
			return GClass4.QuadEdge.North;
		}
		if (caller == south)
		{
			return GClass4.QuadEdge.South;
		}
		if (caller == east)
		{
			return GClass4.QuadEdge.East;
		}
		if (caller == west)
		{
			return GClass4.QuadEdge.West;
		}
		return GClass4.QuadEdge.Null;
	}

	public GClass3 GetRightmostCornerPQ(GClass3 nextQuad)
	{
		GClass3 gClass = this;
		if (nextQuad.subdivision < subdivision)
		{
			gClass = gClass.parent;
		}
		GClass4.QuadEdge edge = nextQuad.GetEdge(gClass);
		if (edge == GClass4.QuadEdge.Null)
		{
			if (!gClass.isPendingCollapse && !gClass.parent.isPendingCollapse && !nextQuad.isPendingCollapse && !nextQuad.parent.isPendingCollapse)
			{
				Debug.LogFormat(string.Concat("[PQ] Edge in GetRightmostCornerPQ is null! Caller: ", gClass, " nextQuad: ", nextQuad));
			}
			return null;
		}
		GClass4.QuadEdge edgeRotatedCounterclockwise = GClass4.GetEdgeRotatedCounterclockwise(edge);
		GClass3 gClass2 = nextQuad.GetSidePQ(edgeRotatedCounterclockwise);
		if (!gClass2.isBuilt)
		{
			gClass2.Build();
		}
		if (gClass2.isSubdivided)
		{
			GClass3 right = null;
			gClass2.GetEdgeQuads(nextQuad, out var _, out right);
			gClass2 = right;
		}
		if (!gClass2.isBuilt)
		{
			gClass2.Build();
		}
		return gClass2;
	}

	public GClass3 GetSidePQ(GClass4.QuadEdge edge)
	{
		return edge switch
		{
			GClass4.QuadEdge.North => north, 
			GClass4.QuadEdge.East => east, 
			GClass4.QuadEdge.South => south, 
			_ => west, 
		};
	}

	public void SetNeighbour(GClass3 oldNeighbour, GClass3 newNeighbour)
	{
		if (newNeighbour == this)
		{
			return;
		}
		if (oldNeighbour == north)
		{
			north = newNeighbour;
		}
		else if (oldNeighbour == south)
		{
			south = newNeighbour;
		}
		else if (oldNeighbour == east)
		{
			east = newNeighbour;
		}
		else if (oldNeighbour == west)
		{
			west = newNeighbour;
		}
		else if (parent != null)
		{
			if (parent.north == oldNeighbour)
			{
				north = newNeighbour;
			}
			else if (parent.south == oldNeighbour)
			{
				south = newNeighbour;
			}
			else if (parent.east == oldNeighbour)
			{
				east = newNeighbour;
			}
			else if (parent.west == oldNeighbour)
			{
				west = newNeighbour;
			}
		}
	}
}
