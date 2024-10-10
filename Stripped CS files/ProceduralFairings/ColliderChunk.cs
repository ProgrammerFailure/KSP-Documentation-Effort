using System.Collections.Generic;
using UnityEngine;

namespace ProceduralFairings;

public class ColliderChunk
{
	public MeshCollider collider;

	public Mesh mesh;

	public List<int> tris;

	public MeshPoint[] pts;

	public int lodStep;

	public int nSides;

	public ColliderChunk(MeshArc bottom, MeshArc top, int nSidesTotal, int nArcs, int nColliders, int subdivIdx)
	{
		tris = new List<int>();
		mesh = new Mesh();
		int num = nSidesTotal / nArcs - 1;
		nSides = nSidesTotal / nArcs / nColliders;
		int num2 = nSides * subdivIdx;
		int num3 = Mathf.Min(num2 + nSides, num);
		if (num - (nSides - 1) < num3)
		{
			num3 = num;
		}
		nSides = num3 - num2 + 1;
		pts = new MeshPoint[nSides * 2 + 4];
		for (int i = 0; i < nSides; i++)
		{
			pts[i] = bottom.outer[num2 + i];
			pts[i].index = i;
			pts[nSides + i] = top.outer[num2 + i];
			pts[nSides + i].index = nSides + i;
		}
		int num4 = pts.Length - 4;
		pts[num4] = top.inner[num2];
		pts[num4].index = num4;
		num4 = pts.Length - 3;
		pts[num4] = top.inner[num3];
		pts[num4].index = num4;
		num4 = pts.Length - 2;
		pts[num4] = bottom.inner[num2];
		pts[num4].index = num4;
		num4 = pts.Length - 1;
		pts[num4] = bottom.inner[num3];
		pts[num4].index = num4;
	}

	public MeshCollider CreateColliderGO(GameObject parent, string name, int layer)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.NestToParent(parent.transform);
		gameObject.layer = layer;
		collider = gameObject.AddComponent<MeshCollider>();
		return collider;
	}

	public Mesh GenerateColliderMesh(bool triangulate, Vector3 offset)
	{
		if (triangulate)
		{
			TriangulateColliderMesh(pts, tris);
		}
		FairingPanel.AssignMesh(mesh, pts, tris, offset, triangulate);
		return mesh;
	}

	public void TriangulateColliderMesh(MeshPoint[] pts, List<int> tris)
	{
		tris.Clear();
		int index2;
		int index3;
		int index4;
		for (int i = 1; i < nSides; i++)
		{
			int index = pts[i - 1].index;
			index2 = pts[nSides + i - 1].index;
			index3 = pts[i].index;
			index4 = pts[nSides + i].index;
			FairingPanel.TriangulateQuad(index, index2, index3, index4, tris);
		}
		index2 = pts[pts.Length - 4].index;
		index4 = pts[pts.Length - 3].index;
		int index5 = pts[pts.Length - 2].index;
		index3 = pts[pts.Length - 1].index;
		FairingPanel.TriangulateQuad(index5, index2, index3, index4, tris);
		int index6 = pts[0].index;
		index2 = pts[nSides].index;
		index3 = pts[pts.Length - 2].index;
		index4 = pts[pts.Length - 4].index;
		FairingPanel.TriangulateQuad(index6, index2, index3, index4, tris);
		int index7 = pts[pts.Length - 1].index;
		index2 = pts[pts.Length - 3].index;
		index3 = pts[nSides - 1].index;
		index4 = pts[nSides * 2 - 1].index;
		FairingPanel.TriangulateQuad(index7, index2, index3, index4, tris);
	}
}
