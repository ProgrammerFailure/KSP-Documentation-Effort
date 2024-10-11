using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProceduralFairings;

[Serializable]
public class FairingPanel
{
	[Serializable]
	public class AttachedFlagParts
	{
		public uint flagVesselID;

		public Part flagPart;

		public List<MeshRenderer> flagMeshRenderers;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AttachedFlagParts()
		{
			throw null;
		}
	}

	public GameObject go;

	public int panelIndex;

	public Mesh mesh;

	public Material mat;

	public GameObject ColliderContainer;

	private List<Transform> detachedPartsForJettison;

	public List<AttachedFlagParts> attachedFlagParts;

	public bool isCap;

	public bool isLast;

	[NonSerialized]
	private MeshFilter mf;

	[NonSerialized]
	private MeshRenderer mr;

	[NonSerialized]
	private FairingPanel topNeighbour;

	[NonSerialized]
	private FairingPanel bottomNeighbour;

	private float wallThickness;

	private float arcFrom;

	private float arcTo;

	private ModuleProceduralFairing host;

	private float tgtOpacity;

	private float opacity;

	private float explodedT;

	private float tgtExplodedT;

	private Vector3 pivotOffset;

	private Vector3 explodedPos;

	private float responsiveness;

	private int nSides;

	private int pCount;

	private MeshPoint[] pts;

	private List<int> tris;

	private Quaternion slope;

	private MeshArc[] Arcs;

	public FairingXSection bottom;

	public FairingXSection top;

	public FairingXSection persistentXSection;

	private MaterialPropertyBlock mpb;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FairingPanel(FairingXSection[] xSections, Material mat, float arcFrom, float arcTo, ModuleProceduralFairing host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsSection(FairingXSection sec)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<FairingPanel> SetupPanelArray(int nSegments, Material FairingMaterial, Material FairingConeMaterial, List<FairingXSection> xSections, ModuleProceduralFairing host, float capRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FetchExistingFlagReferences(List<FairingXSection> xSections, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Spawn(Transform trf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Despawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInterpolations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSlope()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildMesh(bool triangulate, FairingPanel prevPanel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetBarycenter(MeshPoint[] pts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void AssignMesh(Mesh m, MeshPoint[] pts, List<int> tris, Vector3 pivotOffset, bool triangulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetArcPivot(MeshArc arc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetPanelSlope(MeshArc bottom, MeshArc top, ref Quaternion slope)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetPanelHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetBaseHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetMidPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static float[] SetArcPoints(Vector3 pivot, Vector3 axis, float nArcs, Vector3 v0, Vector3 n0, Quaternion slope, float fromAngle, float toAngle, float radius, float h, float arcOffset, float uOffset, float uTiling, Color vColor, ref MeshPoint[] arcPoints, float[] prevH, MeshPoint[] prevPoints)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int SetPanelMeshPoints(ref MeshPoint[] pts, ref MeshPoint[] arc, int ptsOffset, bool aberrantNormals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TriangulatePanelMesh(MeshPoint[] bottomArcOuter, MeshPoint[] topArcOuter, MeshPoint[] bottomArcInner, MeshPoint[] topArcInner, int nSides, bool capTop, bool capBottom, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TriangulateQuad(int v00, int v01, int v0, int v1, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void SetNoseConePoints(Vector3 pivot, Vector3 axis, Vector3 n0, float radius, float uOffset, ref MeshPoint[] pts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TriangulateConePanel(MeshPoint[] bottomArcOuter, MeshPoint[] topArcOuter, MeshPoint[] bottomArcInner, MeshPoint[] topArcInner, bool capTop, bool capBottom, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TriangulateTriangleMesh(MeshPoint[] arcOuter, MeshPoint[] arcInner, int nSidesOuter, int nSidesInner, List<int> tris, bool flip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TriangulateTri(int v00, int v0, int v1, List<int> tris, bool flip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LazyInitMeshArray(ref MeshPoint[] mArray, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Bounds GetBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCursorProximity(Vector3 cursorPosition, float range, Camera referenceCamera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOpacity(float o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetExplodedView(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTgtOpacity(float o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTgtExplodedView(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCollapsedViewInstantly()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateSectionColliders(Transform modelTransform, List<FairingXSection> xSections, int nSides)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GeneratePanelColliders(GameObject hostGo, int layer, bool isTrigger)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GenerateCompoundColliders(GameObject hostGo, MeshArc bottomArc, MeshArc topArc, bool triangulate, int layer, bool isTrigger)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GenerateConvexMesh(GameObject hostGo, int layer, bool isTrigger)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFlag(Part flag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveFlag(uint FlagID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadyFlagsForJettison()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReparentFlagstoPanels()
	{
		throw null;
	}
}
