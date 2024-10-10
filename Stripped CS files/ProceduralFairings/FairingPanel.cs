using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
	}

	public GameObject go;

	public int panelIndex;

	public Mesh mesh;

	public Material mat;

	public GameObject ColliderContainer;

	public List<Transform> detachedPartsForJettison;

	public List<AttachedFlagParts> attachedFlagParts;

	public bool isCap;

	public bool isLast;

	[NonSerialized]
	public MeshFilter mf;

	[NonSerialized]
	public MeshRenderer mr;

	[NonSerialized]
	public FairingPanel topNeighbour;

	[NonSerialized]
	public FairingPanel bottomNeighbour;

	public float wallThickness = 0.1f;

	public float arcFrom;

	public float arcTo;

	public ModuleProceduralFairing host;

	public float tgtOpacity;

	public float opacity;

	public float explodedT;

	public float tgtExplodedT;

	public Vector3 pivotOffset;

	public Vector3 explodedPos;

	public float responsiveness = 12f;

	public int nSides;

	public int pCount;

	public MeshPoint[] pts;

	public List<int> tris;

	public Quaternion slope;

	public MeshArc[] Arcs;

	public FairingXSection bottom;

	public FairingXSection top;

	public FairingXSection persistentXSection;

	public MaterialPropertyBlock mpb;

	public FairingPanel(FairingXSection[] xSections, Material mat, float arcFrom, float arcTo, ModuleProceduralFairing host)
	{
		if (xSections.Length < 2)
		{
			Debug.LogError("[FairingPanel]: Cannot create panel with less than two cross sections!");
			return;
		}
		this.arcFrom = arcFrom;
		this.arcTo = arcTo;
		pivotOffset = Vector3.zero;
		this.host = host;
		nSides = (int)((float)host.nSides * ((arcTo - arcFrom) / 360f));
		go = null;
		mesh = null;
		this.mat = mat;
		opacity = 1f;
		tgtOpacity = 1f;
		explodedT = 0f;
		tgtOpacity = 0f;
		Arcs = new MeshArc[xSections.Length];
		pCount = 0;
		int i = 0;
		for (int num = xSections.Length; i < num; i++)
		{
			if (!xSections[i].isCap)
			{
				MeshArc meshArc = new MeshArc(xSections[i], null, null);
				LazyInitMeshArray(ref meshArc.inner, nSides);
				LazyInitMeshArray(ref meshArc.outer, nSides);
				pCount += nSides * 2;
				Arcs[i] = meshArc;
			}
			else
			{
				MeshArc meshArc2 = new MeshArc(xSections[i], null, null);
				LazyInitMeshArray(ref meshArc2.inner, 1);
				LazyInitMeshArray(ref meshArc2.outer, 1);
				pCount += 2;
				Arcs[i] = meshArc2;
			}
		}
		LazyInitMeshArray(ref pts, pCount);
		tris = new List<int>();
	}

	public bool ContainsSection(FairingXSection sec)
	{
		int num = Arcs.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (Arcs[num].xSection != sec);
		return true;
	}

	public static List<FairingPanel> SetupPanelArray(int nSegments, Material FairingMaterial, Material FairingConeMaterial, List<FairingXSection> xSections, ModuleProceduralFairing host, float capRadius)
	{
		List<FairingPanel> list = new List<FairingPanel>();
		float num = 360f / (float)nSegments;
		int num2 = 0;
		int i = 0;
		for (int num3 = xSections.Count - 1; i < num3; i++)
		{
			FairingXSection fairingXSection = xSections[i];
			FairingXSection fairingXSection2 = xSections[i + 1];
			FairingXSection fairingXSection3 = new FairingXSection(fairingXSection, fairingXSection2);
			FairingXSection fairingXSection4 = new FairingXSection(fairingXSection2, fairingXSection3);
			for (int j = 0; j < nSegments; j++)
			{
				float num4 = num * (float)j;
				float num5 = num * (float)j + num;
				FairingPanel fairingPanel = new FairingPanel(new FairingXSection[4] { fairingXSection, fairingXSection3, fairingXSection4, fairingXSection2 }, FairingMaterial, num4, num5, host);
				fairingPanel.bottom = fairingXSection;
				fairingPanel.top = fairingXSection2;
				if (i > 0)
				{
					list[num2 - nSegments].topNeighbour = fairingPanel;
					fairingPanel.bottomNeighbour = list[num2 - nSegments];
				}
				fairingPanel.panelIndex = list.Count;
				xSections[i].AddNewFairingPanel(fairingPanel.panelIndex);
				fairingPanel.FetchExistingFlagReferences(xSections, i);
				list.Add(fairingPanel);
				num2++;
			}
		}
		if (xSections[xSections.Count - 1].isLast && xSections[xSections.Count - 1].r == capRadius)
		{
			FairingXSection fairingXSection = xSections[xSections.Count - 1];
			FairingXSection fairingXSection2 = new FairingXSection();
			fairingXSection2.r = 0f;
			fairingXSection2.h = fairingXSection.h + fairingXSection.r * Mathf.Tan(FairingXSection.GetSlopeAngle(xSections[xSections.Count - 2], fairingXSection)) * host.noseTip;
			fairingXSection2.isCap = true;
			FairingXSection fairingXSection3 = new FairingXSection(fairingXSection, fairingXSection2);
			for (int k = 0; k < nSegments; k++)
			{
				FairingPanel fairingPanel = list[list.Count - nSegments];
				float num4 = num * (float)k;
				float num5 = num * (float)k + num;
				FairingPanel fairingPanel2 = (fairingPanel.topNeighbour = new FairingPanel(new FairingXSection[2] { fairingXSection, fairingXSection2 }, FairingConeMaterial, num4, num5, host));
				fairingPanel.isLast = true;
				fairingPanel2.bottomNeighbour = fairingPanel;
				fairingPanel2.bottom = fairingXSection;
				fairingPanel2.top = fairingXSection2;
				fairingPanel2.isCap = true;
				fairingPanel2.panelIndex = list.Count;
				fairingPanel2.persistentXSection = xSections[xSections.Count - 1];
				xSections[xSections.Count - 1].AddNewFairingPanel(fairingPanel2.panelIndex);
				fairingPanel2.FetchExistingFlagReferences(xSections, xSections.Count - 1);
				list.Add(fairingPanel2);
			}
		}
		return list;
	}

	public void FetchExistingFlagReferences(List<FairingXSection> xSections, int index)
	{
		if (xSections[index].fairingPanelFlags == null || xSections[index].fairingPanelFlags.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < xSections[index].fairingPanelFlags.Count; i++)
		{
			if (xSections[index].fairingPanelFlags[i].panelIndex == panelIndex)
			{
				if (attachedFlagParts == null)
				{
					attachedFlagParts = new List<AttachedFlagParts>();
				}
				for (int j = 0; j < xSections[index].fairingPanelFlags[i].attachedFlagsPartIds.Count; j++)
				{
					new AttachedFlagParts().flagVesselID = xSections[index].fairingPanelFlags[i].attachedFlagsPartIds[j];
				}
			}
		}
	}

	public void Spawn(Transform trf)
	{
		if (go == null)
		{
			go = new GameObject("FairingPanel");
			go.layer = 0;
		}
		mesh = new Mesh();
		mesh.MarkDynamic();
		mf = go.AddComponent<MeshFilter>();
		mr = go.AddComponent<MeshRenderer>();
		mr.sharedMaterial = new Material(mat);
		mr.reflectionProbeUsage = ReflectionProbeUsage.BlendProbesAndSkybox;
		go.transform.NestToParent(trf);
	}

	public void Despawn()
	{
		go.DestroyGameObject();
	}

	public void CleanUp()
	{
		pts = null;
		tris = null;
	}

	public void UpdateInterpolations()
	{
		float num = ((topNeighbour == null) ? 0f : (GetSlope() - topNeighbour.GetSlope()));
		float num2 = ((bottomNeighbour == null) ? 0f : (bottomNeighbour.GetSlope() - GetSlope()));
		Arcs[1].xSection.UpdateLerp(host.edgeSlide, num2 * host.edgeWarp);
		Arcs[2].xSection.UpdateLerp(host.edgeSlide, num * host.edgeWarp);
	}

	public float GetSlope()
	{
		return (Arcs[0].xSection.r - Arcs[Arcs.Length - 1].xSection.r) / (Arcs[0].xSection.h - Arcs[Arcs.Length - 1].xSection.h);
	}

	public void BuildMesh(bool triangulate, FairingPanel prevPanel)
	{
		if (go == null)
		{
			Debug.LogError("[FairingPanel]: Cannot build mesh without spawning the gameobject first. Did you forget to call Spawn()?");
			return;
		}
		if (Arcs.Length > 2)
		{
			UpdateInterpolations();
		}
		LazyInitMeshArray(ref pts, pCount);
		if (triangulate)
		{
			tris.Clear();
		}
		MeshArc meshArc = null;
		if (prevPanel != null && prevPanel.Arcs.Length != 0)
		{
			meshArc = prevPanel.Arcs[prevPanel.Arcs.Length - 1];
		}
		Vector3 forward = Vector3.forward;
		bool flag = false;
		int i = 0;
		for (int num = Arcs.Length; i < num; i++)
		{
			MeshArc meshArc2;
			if (i == 0)
			{
				meshArc2 = Arcs[i];
				LazyInitMeshArray(ref meshArc2.outer, nSides);
				LazyInitMeshArray(ref meshArc2.inner, nSides);
				SetArcPivot(meshArc2);
				continue;
			}
			MeshArc meshArc3 = Arcs[i];
			meshArc2 = Arcs[i - 1];
			if (!meshArc3.xSection.isCap)
			{
				LazyInitMeshArray(ref meshArc3.outer, nSides);
				LazyInitMeshArray(ref meshArc3.inner, nSides);
			}
			else
			{
				LazyInitMeshArray(ref meshArc3.outer, 1);
				LazyInitMeshArray(ref meshArc3.inner, 1);
			}
			SetArcPivot(meshArc3);
			GetPanelSlope(meshArc2, meshArc3, ref slope);
			flag = ((Mathf.Abs((Arcs[0].xSection.r - Arcs[Arcs.Length - 1].xSection.r) / (Arcs[0].xSection.h - Arcs[Arcs.Length - 1].xSection.h)) > host.aberrantNormalLimit) ? true : false);
			float h;
			if (i == 1)
			{
				h = meshArc2.xSection.h;
				if (meshArc3.xSection.isCap)
				{
					if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
					{
						meshArc2.hOffsetOuter = SetArcPoints(meshArc2.pivot, host.axis, host.nArcs, Vector3.forward, forward, slope, arcFrom, arcTo, meshArc2.xSection.r, 0f, 1f, 0f, 1f, meshArc2.xSection.color, ref meshArc2.outer, null, null);
						meshArc2.hOffsetInner = SetArcPoints(meshArc2.pivot, host.axis, host.nArcs, Vector3.forward, -forward, slope, arcFrom, arcTo, meshArc2.xSection.r - wallThickness, 0f, 1f, 0f, 1f, meshArc2.xSection.color, ref meshArc2.inner, null, null);
					}
				}
				else
				{
					meshArc2.hOffsetOuter = SetArcPoints(meshArc2.pivot, host.axis, host.nArcs, Vector3.forward, forward, slope, arcFrom, arcTo, meshArc2.xSection.r, h, 0.5f, 0f, 0.5f, meshArc2.xSection.color, ref meshArc2.outer, meshArc?.hOffsetOuter, meshArc?.outer);
					meshArc2.hOffsetInner = SetArcPoints(meshArc2.pivot, host.axis, host.nArcs, Vector3.forward, -forward, slope, arcFrom, arcTo, meshArc2.xSection.r - wallThickness, h, 0f, 0.5f, 0.5f, meshArc2.xSection.color, ref meshArc2.inner, meshArc?.hOffsetInner, meshArc?.inner);
				}
				SetPanelMeshPoints(ref pts, ref meshArc2.outer, nSides * 2 * (i - 1), flag);
				SetPanelMeshPoints(ref pts, ref meshArc2.inner, nSides * 2 * (i - 1) + nSides, flag);
			}
			h = meshArc3.xSection.h;
			if (!meshArc3.xSection.isCap)
			{
				meshArc3.hOffsetOuter = SetArcPoints(meshArc3.pivot, host.axis, host.nArcs, Vector3.forward, forward, slope, arcFrom, arcTo, meshArc3.xSection.r, h, 0.5f, 0f, 0.5f, meshArc3.xSection.color, ref meshArc3.outer, meshArc2.hOffsetOuter, meshArc2.outer);
				meshArc3.hOffsetInner = SetArcPoints(meshArc3.pivot, host.axis, host.nArcs, Vector3.forward, -forward, slope, arcFrom, arcTo, meshArc3.xSection.r - wallThickness, h, 0f, 0.5f, 0.5f, meshArc3.xSection.color, ref meshArc3.inner, meshArc2.hOffsetInner, meshArc2.inner);
				SetPanelMeshPoints(ref pts, ref meshArc3.outer, nSides * 2 * (i - 1) + nSides * 2, flag);
				SetPanelMeshPoints(ref pts, ref meshArc3.inner, nSides * 2 * (i - 1) + nSides * 3, flag);
				if (triangulate)
				{
					TriangulatePanelMesh(meshArc2.outer, meshArc3.outer, meshArc2.inner, meshArc3.inner, nSides, i == Arcs.Length - 1, i == 1, tris);
				}
			}
			else
			{
				SetNoseConePoints(meshArc3.pivot, host.axis, Vector3.up, meshArc3.xSection.r, 0.5f, ref meshArc3.outer);
				SetNoseConePoints(meshArc3.pivot, host.axis, Vector3.down, meshArc3.xSection.r, 0.5f, ref meshArc3.inner);
				SetPanelMeshPoints(ref pts, ref meshArc3.outer, nSides * 2 * i, aberrantNormals: false);
				SetPanelMeshPoints(ref pts, ref meshArc3.inner, nSides * 2 * i + meshArc3.outer.Length, aberrantNormals: false);
				if (triangulate)
				{
					TriangulateConePanel(meshArc2.outer, meshArc3.outer, meshArc2.inner, meshArc3.inner, capTop: false, i == 1, tris);
				}
			}
		}
		pivotOffset = GetBarycenter(pts);
		explodedPos = pivotOffset + (pivotOffset - GetMidPoint()).normalized * Mathf.Max(Arcs[0].xSection.r, Arcs[Arcs.Length - 1].xSection.r);
		AssignMesh(mesh, pts, tris, pivotOffset, triangulate);
		mf.sharedMesh = mesh;
		go.transform.localPosition = pivotOffset;
	}

	public Vector3 GetBarycenter(MeshPoint[] pts)
	{
		Vector3 zero = Vector3.zero;
		int num = pts.Length;
		while (num-- > 0)
		{
			zero += pts[num].vert;
		}
		return zero / pts.Length;
	}

	public static void AssignMesh(Mesh m, MeshPoint[] pts, List<int> tris, Vector3 pivotOffset, bool triangulate)
	{
		m.vertices = MeshPoint.GetVerts(pts, -pivotOffset);
		m.normals = MeshPoint.GetNormals(pts);
		m.uv = MeshPoint.GetUV1(pts);
		m.uv2 = MeshPoint.GetUV2(pts);
		m.colors = MeshPoint.GetColors(pts);
		if (triangulate)
		{
			m.triangles = tris.ToArray();
		}
		m.RecalculateBounds();
	}

	public void SetArcPivot(MeshArc arc)
	{
		arc.pivot = host.pivot + host.axis * arc.xSection.h;
	}

	public void GetPanelSlope(MeshArc bottom, MeshArc top, ref Quaternion slope)
	{
		if (top.xSection.r != bottom.xSection.r)
		{
			slope = Quaternion.FromToRotation(host.axis, top.pivot + Vector3.forward * top.xSection.r - (bottom.pivot + Vector3.forward * bottom.xSection.r));
		}
		else
		{
			slope = Quaternion.identity;
		}
	}

	public float GetPanelHeight()
	{
		return Arcs[Arcs.Length - 1].xSection.h - Arcs[0].xSection.h;
	}

	public float GetBaseHeight()
	{
		return Arcs[0].xSection.h;
	}

	public Vector3 GetMidPoint()
	{
		return host.pivot + host.axis * (GetBaseHeight() + GetPanelHeight() * 0.5f);
	}

	public static float[] SetArcPoints(Vector3 pivot, Vector3 axis, float nArcs, Vector3 v0, Vector3 n0, Quaternion slope, float fromAngle, float toAngle, float radius, float h, float arcOffset, float uOffset, float uTiling, Color vColor, ref MeshPoint[] arcPoints, float[] prevH, MeshPoint[] prevPoints)
	{
		float[] array = new float[arcPoints.Length];
		int num = arcPoints.Length;
		while (num-- > 0)
		{
			float num2 = Mathf.InverseLerp(0f, arcPoints.Length - 1, num);
			float angle = Mathf.Lerp(fromAngle, toAngle, num2);
			MeshPoint meshPoint = arcPoints[num];
			meshPoint.Clear();
			meshPoint.normal = Quaternion.AngleAxis(angle, axis) * (slope * n0);
			meshPoint.vert = pivot + Quaternion.AngleAxis(angle, axis) * v0 * radius;
			array[num] = ((prevPoints == null) ? h : (prevH[num] + Math.Max(h - prevH[num], Vector3.Distance(prevPoints[num].vert, meshPoint.vert))));
			meshPoint.uv1 = new Vector2(Mathf.Abs(arcOffset - num2 * uTiling) + uOffset, array[num] / 3f);
			meshPoint.color = vColor;
		}
		return array;
	}

	public static int SetPanelMeshPoints(ref MeshPoint[] pts, ref MeshPoint[] arc, int ptsOffset, bool aberrantNormals)
	{
		int num = 0;
		int i = 0;
		for (int num2 = arc.Length; i < num2; i++)
		{
			num = i + ptsOffset;
			pts[num] = arc[i];
			pts[num].index = num;
			if (aberrantNormals && num < pts.Length / 2)
			{
				pts[num].normal *= -1f;
			}
		}
		return num;
	}

	public static void TriangulatePanelMesh(MeshPoint[] bottomArcOuter, MeshPoint[] topArcOuter, MeshPoint[] bottomArcInner, MeshPoint[] topArcInner, int nSides, bool capTop, bool capBottom, List<int> tris)
	{
		for (int i = 1; i < nSides; i++)
		{
			int index = bottomArcOuter[i - 1].index;
			int index2 = bottomArcOuter[i].index;
			int index3 = topArcOuter[i - 1].index;
			int index4 = topArcOuter[i].index;
			TriangulateQuad(index2, index4, index, index3, tris);
		}
		for (int j = 1; j < nSides; j++)
		{
			int index = bottomArcInner[j].index;
			int index5 = bottomArcInner[j - 1].index;
			int index3 = topArcInner[j].index;
			int index4 = topArcInner[j - 1].index;
			TriangulateQuad(index5, index4, index, index3, tris);
		}
		if (capBottom)
		{
			for (int k = 1; k < nSides; k++)
			{
				int index6 = bottomArcInner[k].index;
				int index = bottomArcInner[k - 1].index;
				int index4 = bottomArcOuter[k].index;
				int index3 = bottomArcOuter[k - 1].index;
				TriangulateQuad(index6, index4, index, index3, tris);
			}
		}
		if (capTop)
		{
			for (int l = 1; l < nSides; l++)
			{
				int index7 = topArcOuter[l].index;
				int index = topArcOuter[l - 1].index;
				int index4 = topArcInner[l].index;
				int index3 = topArcInner[l - 1].index;
				TriangulateQuad(index7, index4, index, index3, tris);
			}
		}
		TriangulateQuad(bottomArcOuter[0].index, topArcOuter[0].index, bottomArcInner[0].index, topArcInner[0].index, tris);
		TriangulateQuad(bottomArcInner[nSides - 1].index, topArcInner[nSides - 1].index, bottomArcOuter[nSides - 1].index, topArcOuter[nSides - 1].index, tris);
	}

	public static void TriangulateQuad(int v00, int v01, int v0, int v1, List<int> tris)
	{
		tris.Add(v00);
		tris.Add(v01);
		tris.Add(v1);
		tris.Add(v1);
		tris.Add(v0);
		tris.Add(v00);
	}

	public static void SetNoseConePoints(Vector3 pivot, Vector3 axis, Vector3 n0, float radius, float uOffset, ref MeshPoint[] pts)
	{
		MeshPoint obj = pts[0];
		obj.Clear();
		obj.vert = pivot + axis * radius;
		obj.normal = n0;
		obj.uv1 = new Vector2(uOffset, 1f);
	}

	public static void TriangulateConePanel(MeshPoint[] bottomArcOuter, MeshPoint[] topArcOuter, MeshPoint[] bottomArcInner, MeshPoint[] topArcInner, bool capTop, bool capBottom, List<int> tris)
	{
		TriangulateTriangleMesh(bottomArcOuter, topArcOuter, bottomArcOuter.Length, topArcOuter.Length, tris, flip: true);
		TriangulateTriangleMesh(bottomArcInner, topArcInner, bottomArcInner.Length, topArcInner.Length, tris, flip: false);
		if (capBottom || capTop)
		{
			int num = bottomArcOuter.Length;
			if (capBottom)
			{
				for (int i = 1; i < num; i++)
				{
					int index = bottomArcInner[i].index;
					int index2 = bottomArcInner[i - 1].index;
					int index3 = bottomArcOuter[i].index;
					int index4 = bottomArcOuter[i - 1].index;
					TriangulateQuad(index, index3, index2, index4, tris);
				}
			}
			if (capTop)
			{
				for (int j = 1; j < num; j++)
				{
					int index5 = topArcOuter[j].index;
					int index2 = topArcOuter[j - 1].index;
					int index3 = topArcInner[j].index;
					int index4 = topArcInner[j - 1].index;
					TriangulateQuad(index5, index4, index2, index3, tris);
				}
			}
		}
		TriangulateQuad(bottomArcOuter[0].index, topArcOuter[0].index, bottomArcInner[0].index, topArcInner[0].index, tris);
		TriangulateQuad(bottomArcInner[bottomArcInner.Length - 1].index, topArcInner[topArcInner.Length - 1].index, bottomArcOuter[bottomArcOuter.Length - 1].index, topArcOuter[topArcOuter.Length - 1].index, tris);
	}

	public static void TriangulateTriangleMesh(MeshPoint[] arcOuter, MeshPoint[] arcInner, int nSidesOuter, int nSidesInner, List<int> tris, bool flip)
	{
		int num = nSidesOuter / nSidesInner;
		if ((float)nSidesOuter % (float)nSidesInner != 0f)
		{
			Debug.LogError("[Procedural Fairings Mesh Util]: Cannot triangulate between a " + nSidesOuter + "-sided arc and a " + nSidesInner + "-sided one. Vert Counts are not cleanly divisible (Mod = " + ((float)nSidesOuter % (float)nSidesInner).ToString("0.###") + ").");
			return;
		}
		int num2 = 0;
		for (int i = 1; i < nSidesOuter; i++)
		{
			int index = arcOuter[i - 1].index;
			int index2 = arcOuter[i].index;
			int index3 = arcInner[num2].index;
			TriangulateTri(index, index2, index3, tris, flip);
			if (i % num == 0)
			{
				num2++;
			}
		}
	}

	public static void TriangulateTri(int v00, int v0, int v1, List<int> tris, bool flip)
	{
		if (!flip)
		{
			tris.Add(v00);
			tris.Add(v1);
			tris.Add(v0);
		}
		else
		{
			tris.Add(v00);
			tris.Add(v0);
			tris.Add(v1);
		}
	}

	public static void LazyInitMeshArray(ref MeshPoint[] mArray, int length)
	{
		if (mArray == null)
		{
			mArray = new MeshPoint[length];
			int num = length;
			while (num-- > 0)
			{
				mArray[num] = new MeshPoint();
			}
		}
		else
		{
			int num2 = length;
			while (num2-- > 0)
			{
				mArray[num2].Clear();
			}
		}
	}

	public float GetArea()
	{
		float r = Arcs[0].xSection.r;
		float r2 = Arcs[Arcs.Length - 1].xSection.r;
		float panelHeight = GetPanelHeight();
		float num = Mathf.Sqrt(Mathf.Pow(r - r2, 2f) + panelHeight * panelHeight);
		float num2 = (float)Math.PI * (r + r2) * num;
		float num3 = (arcTo - arcFrom) / 360f;
		return num2 * num3;
	}

	public Bounds GetBounds()
	{
		return mesh.bounds;
	}

	public float GetCursorProximity(Vector3 cursorPosition, float range, Camera referenceCamera)
	{
		float num = Mathf.Tan(referenceCamera.fieldOfView * 0.5f * ((float)Math.PI / 180f)) * (go.transform.position - referenceCamera.transform.position).sqrMagnitude;
		float num2 = range * range / num;
		cursorPosition /= (float)Screen.height;
		Vector3 vector = referenceCamera.WorldToScreenPoint(go.transform.position) / Screen.height;
		Vector3 vector2 = cursorPosition - vector;
		float sqrMagnitude = Vector3.ProjectOnPlane(vector2, Vector3.forward).sqrMagnitude;
		Vector3 vector3 = referenceCamera.WorldToScreenPoint(host.transform.TransformPoint(pivotOffset)) / Screen.height;
		Vector3 vector4 = cursorPosition - vector3;
		float sqrMagnitude2 = Vector3.ProjectOnPlane(vector4, Vector3.forward).sqrMagnitude;
		Vector3 vector5 = referenceCamera.WorldToScreenPoint(host.transform.TransformPoint(GetMidPoint())) / Screen.height;
		Vector3 vector6 = cursorPosition - vector5;
		float sqrMagnitude3 = Vector3.ProjectOnPlane(vector6, Vector3.forward).sqrMagnitude;
		return Mathf.Clamp01(1f - Mathf.Min(sqrMagnitude, Mathf.Min(sqrMagnitude2, sqrMagnitude3)) / num2);
	}

	public void SetOpacity(float o)
	{
		opacity = o;
		if (mpb == null)
		{
			mpb = new MaterialPropertyBlock();
		}
		mpb.SetFloat(PropertyIDs._Opacity, o);
		mr.SetPropertyBlock(mpb);
		if (attachedFlagParts == null)
		{
			return;
		}
		for (int i = 0; i < attachedFlagParts.Count; i++)
		{
			for (int j = 0; j < attachedFlagParts[i].flagMeshRenderers.Count; j++)
			{
				attachedFlagParts[i].flagMeshRenderers[j].SetPropertyBlock(mpb);
			}
		}
	}

	public void SetExplodedView(float t)
	{
		explodedT = t;
		go.transform.localPosition = Vector3.Lerp(pivotOffset, explodedPos, t);
	}

	public void SetTgtOpacity(float o)
	{
		tgtOpacity = o;
		SetOpacity(Mathf.Lerp(opacity, tgtOpacity, Time.deltaTime * responsiveness));
	}

	public void SetTgtExplodedView(float t)
	{
		tgtExplodedT = t;
		SetExplodedView(Mathf.Lerp(explodedT, tgtExplodedT, Time.deltaTime * responsiveness));
	}

	public void SetCollapsedViewInstantly()
	{
		go.transform.localPosition = pivotOffset;
	}

	public void GenerateSectionColliders(Transform modelTransform, List<FairingXSection> xSections, int nSides)
	{
		int i = 0;
		for (int num = xSections.Count - 1; i < num; i++)
		{
			ColliderSection.Create(modelTransform, xSections[i], xSections[i + 1], nSides);
		}
	}

	public void GeneratePanelColliders(GameObject hostGo, int layer, bool isTrigger)
	{
		ColliderContainer = hostGo;
		int i = 1;
		for (int num = Arcs.Length; i < num; i++)
		{
			if (Arcs[i].xSection.isCap)
			{
				GenerateConvexMesh(hostGo, layer, isTrigger);
			}
			else
			{
				GenerateCompoundColliders(hostGo, Arcs[i - 1], Arcs[i], triangulate: true, layer, isTrigger);
			}
		}
	}

	public void GenerateCompoundColliders(GameObject hostGo, MeshArc bottomArc, MeshArc topArc, bool triangulate, int layer, bool isTrigger)
	{
		for (int i = 0; i < host.nCollidersPerArc; i++)
		{
			ColliderChunk colliderChunk = new ColliderChunk(bottomArc, topArc, host.nSides, Mathf.RoundToInt(host.nArcs), host.nCollidersPerArc, i);
			colliderChunk.CreateColliderGO(hostGo, "collider" + i, layer);
			colliderChunk.collider.sharedMesh = colliderChunk.GenerateColliderMesh(triangulate, hostGo.transform.parent.localPosition);
			colliderChunk.collider.convex = true;
			colliderChunk.collider.isTrigger = isTrigger;
		}
	}

	public void GenerateConvexMesh(GameObject hostGo, int layer, bool isTrigger)
	{
		MeshCollider meshCollider = new GameObject("colliderCap").AddComponent<MeshCollider>();
		meshCollider.gameObject.transform.NestToParent(hostGo.transform);
		meshCollider.gameObject.layer = layer;
		meshCollider.sharedMesh = mesh;
		meshCollider.convex = true;
		meshCollider.isTrigger = isTrigger;
	}

	public void AddFlag(Part flag)
	{
		if (flag == null)
		{
			return;
		}
		AttachedFlagParts attachedFlagParts = null;
		if (this.attachedFlagParts == null)
		{
			this.attachedFlagParts = new List<AttachedFlagParts>();
		}
		else
		{
			for (int i = 0; i < this.attachedFlagParts.Count; i++)
			{
				if (this.attachedFlagParts[i].flagVesselID == flag.craftID)
				{
					if (this.attachedFlagParts[i].flagPart != null)
					{
						return;
					}
					attachedFlagParts = this.attachedFlagParts[i];
					break;
				}
			}
		}
		uint num = 0u;
		FlagDecalBackground component = flag.GetComponent<FlagDecalBackground>();
		if (component != null)
		{
			if (component.placementID == 0)
			{
				num = flag.craftID;
				component.placementID = (int)flag.craftID;
			}
			else
			{
				num = (uint)component.placementID;
			}
		}
		if (attachedFlagParts == null)
		{
			attachedFlagParts = new AttachedFlagParts();
			attachedFlagParts.flagVesselID = num;
			this.attachedFlagParts.Add(attachedFlagParts);
		}
		attachedFlagParts.flagPart = flag;
		bool flag2 = false;
		if (bottom != null)
		{
			flag2 = bottom.AddAttachedFlag(panelIndex, num);
		}
		if (!flag2 && top != null)
		{
			flag2 = top.AddAttachedFlag(panelIndex, num);
		}
		if (!flag2 && persistentXSection != null)
		{
			flag2 = persistentXSection.AddAttachedFlag(panelIndex, num);
		}
		if (!flag2)
		{
			Debug.LogError($"Flag could not be added to fairing index = {panelIndex}");
		}
		if (HighLogic.fetch != null)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				flag.transform.SetParent(go.transform);
			}
			attachedFlagParts.flagPart = flag;
			attachedFlagParts.flagMeshRenderers = new List<MeshRenderer>();
			MeshRenderer[] componentsInChildren = flag.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			if (componentsInChildren != null)
			{
				attachedFlagParts.flagMeshRenderers.AddRange(componentsInChildren);
			}
		}
	}

	public void RemoveFlag(uint FlagID)
	{
		if (attachedFlagParts == null)
		{
			return;
		}
		for (int i = 0; i < attachedFlagParts.Count; i++)
		{
			if (attachedFlagParts[i].flagVesselID == FlagID)
			{
				attachedFlagParts.Remove(attachedFlagParts[i]);
				i--;
			}
		}
	}

	public bool ReadyFlagsForJettison()
	{
		if (attachedFlagParts != null && attachedFlagParts.Count != 0)
		{
			if (detachedPartsForJettison == null)
			{
				detachedPartsForJettison = new List<Transform>();
			}
			else
			{
				detachedPartsForJettison.Clear();
			}
			for (int i = 0; i < attachedFlagParts.Count; i++)
			{
				detachedPartsForJettison.Add(attachedFlagParts[i].flagPart.transform);
				attachedFlagParts[i].flagPart.disconnect();
				AttachNode attachNode = attachedFlagParts[i].flagPart.FindAttachNodeByPart(attachedFlagParts[i].flagPart.parent);
				if (attachNode != null)
				{
					attachNode.attachedPart = null;
				}
				attachNode = attachedFlagParts[i].flagPart.parent.FindAttachNodeByPart(attachedFlagParts[i].flagPart);
				if (attachedFlagParts[i].flagPart.parent != null && attachNode != null)
				{
					attachNode.attachedPart = null;
				}
				if (attachedFlagParts[i].flagPart.parent != null)
				{
					attachedFlagParts[i].flagPart.parent.removeChild(attachedFlagParts[i].flagPart);
				}
				attachedFlagParts[i].flagPart.deactivate();
				UnityEngine.Object.Destroy(attachedFlagParts[i].flagPart);
			}
			return ReparentFlagstoPanels();
		}
		return false;
	}

	public bool ReparentFlagstoPanels()
	{
		if (detachedPartsForJettison != null && detachedPartsForJettison.Count != 0)
		{
			for (int i = 0; i < detachedPartsForJettison.Count; i++)
			{
				detachedPartsForJettison[i].SetParent(go.transform);
			}
			return true;
		}
		return false;
	}
}
