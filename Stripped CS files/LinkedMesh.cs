using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LinkedMesh
{
	[Flags]
	public enum MeshOption
	{
		None = 0,
		CalculateNormals = 1,
		CalculateBounds = 2,
		CalculateTangents = 4,
		Debug = 8
	}

	[Serializable]
	public class LinkedTri
	{
		public int a;

		public int b;

		public int c;

		public int[] links;

		public Vector3 normal;

		public Vector3 edgeAB;

		public Vector3 edgeAC;

		public Vector3 midpoint;

		public bool isInSelection;

		public float selectionDelta;

		public LinkedVert aVert { get; set; }

		public LinkedVert bVert { get; set; }

		public LinkedVert cVert { get; set; }

		public LinkedTri[] linkTris { get; set; }

		public LinkedTri(int a, int b, int c, Vector3 midpoint)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.midpoint = midpoint;
			isInSelection = false;
		}

		public void SetVerts(LinkedVert aVert, LinkedVert bVert, LinkedVert cVert)
		{
			this.aVert = aVert;
			this.bVert = bVert;
			this.cVert = cVert;
		}

		public void ClearVerts()
		{
			aVert = null;
			bVert = null;
			cVert = null;
		}

		public void SetLinks(int[] links)
		{
			this.links = links;
		}

		public void SetLinkTris(LinkedTri[] links)
		{
			linkTris = links;
		}

		public bool IsVertInTri(int vert)
		{
			int[] meshVerts = aVert.meshVerts;
			int num = 0;
			while (true)
			{
				if (num < meshVerts.Length)
				{
					if (meshVerts[num] == vert)
					{
						break;
					}
					num++;
					continue;
				}
				meshVerts = bVert.meshVerts;
				num = 0;
				while (true)
				{
					if (num < meshVerts.Length)
					{
						if (meshVerts[num] == vert)
						{
							break;
						}
						num++;
						continue;
					}
					meshVerts = cVert.meshVerts;
					num = 0;
					while (true)
					{
						if (num < meshVerts.Length)
						{
							if (meshVerts[num] == vert)
							{
								break;
							}
							num++;
							continue;
						}
						return false;
					}
					return true;
				}
				return true;
			}
			return true;
		}

		public bool HasVertsInTri(LinkedTri tri)
		{
			int[] meshVerts = tri.aVert.meshVerts;
			int num = 0;
			while (true)
			{
				if (num < meshVerts.Length)
				{
					int vert = meshVerts[num];
					if (IsVertInTri(vert))
					{
						break;
					}
					num++;
					continue;
				}
				meshVerts = tri.bVert.meshVerts;
				num = 0;
				while (true)
				{
					if (num < meshVerts.Length)
					{
						int vert2 = meshVerts[num];
						if (IsVertInTri(vert2))
						{
							break;
						}
						num++;
						continue;
					}
					meshVerts = tri.cVert.meshVerts;
					num = 0;
					while (true)
					{
						if (num < meshVerts.Length)
						{
							int vert3 = meshVerts[num];
							if (IsVertInTri(vert3))
							{
								break;
							}
							num++;
							continue;
						}
						return false;
					}
					return true;
				}
				return true;
			}
			return true;
		}

		public bool IsNeighbourOf(LinkedTri tri)
		{
			LinkedTri[] array = linkTris;
			int num = 0;
			while (true)
			{
				if (num < array.Length)
				{
					if (array[num] == tri)
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
	}

	[Serializable]
	public class LinkedVert
	{
		public int meshVert;

		public int[] meshVerts;

		public int meshVertCount;

		public int triCount;

		public bool isInSelection;

		public float selectionDelta;

		public LinkedVert(int meshVert)
		{
			this.meshVert = meshVert;
		}
	}

	[HideInInspector]
	public Vector3[] verts;

	[HideInInspector]
	public int[] tris;

	[HideInInspector]
	public Vector2[] uv;

	[HideInInspector]
	public Vector3[] normals;

	[HideInInspector]
	public LinkedVert[] linkedVerts;

	[HideInInspector]
	public LinkedTri[] linkedTris;

	public int vertCount;

	[HideInInspector]
	public int triIndexCount;

	public int triCount;

	public Vector4[] tangents;

	public Vector3[] tan1;

	public Vector3[] tan2;

	public bool meshBuilt;

	public Mesh mesh;

	[HideInInspector]
	public List<LinkedTri> selection;

	[HideInInspector]
	public int selectionCount;

	[HideInInspector]
	public List<LinkedVert> selectionVerts;

	[HideInInspector]
	public int selectionVertsCount;

	public Vector3 selectionCenter;

	public float selectionRadiusSqr;

	public LinkedMesh()
	{
		meshBuilt = false;
	}

	public LinkedMesh(Mesh baseMesh)
	{
		ClearMesh();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		verts = baseMesh.vertices;
		vertCount = baseMesh.vertexCount;
		tris = baseMesh.triangles;
		triIndexCount = baseMesh.triangles.Length;
		triCount = triIndexCount / 3;
		uv = baseMesh.uv;
		normals = baseMesh.normals;
		selection = new List<LinkedTri>(256);
		selectionCount = 0;
		selectionVerts = new List<LinkedVert>(256);
		selectionVertsCount = 0;
		NormalizeMesh();
		CreateLinks();
		meshBuilt = true;
		Debug.Log("MeshBuild: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3"));
	}

	public void ClearMesh()
	{
		verts = null;
		tris = null;
		uv = null;
		normals = null;
		vertCount = 0;
		triIndexCount = 0;
		triCount = 0;
		linkedVerts = null;
		linkedTris = null;
	}

	public void CalculateTangets()
	{
		if (tangents == null)
		{
			tangents = new Vector4[vertCount];
			tan1 = new Vector3[vertCount];
			tan2 = new Vector3[vertCount];
		}
		int num = 0;
		for (int i = 0; i < triCount; i++)
		{
			int num2 = tris[num];
			int num3 = tris[num + 1];
			int num4 = tris[num + 2];
			Vector3 vector = verts[num2];
			Vector3 vector2 = verts[num3];
			Vector3 vector3 = verts[num4];
			Vector3 vector4 = uv[num2];
			Vector3 vector5 = uv[num3];
			Vector3 vector6 = uv[num4];
			float num5 = vector2.x - vector.x;
			float num6 = vector3.x - vector.x;
			float num7 = vector2.y - vector.y;
			float num8 = vector3.y - vector.y;
			float num9 = vector2.z - vector.z;
			float num10 = vector3.z - vector.z;
			float num11 = vector5.x - vector4.x;
			float num12 = vector6.x - vector4.x;
			float num13 = vector5.y - vector4.y;
			float num14 = vector6.y - vector4.y;
			float num15 = 1f / (num11 * num14 - num12 * num13);
			Vector3 vector7 = new Vector3((num14 * num5 - num13 * num6) * num15, (num14 * num7 - num13 * num8) * num15, (num14 * num9 - num13 * num10) * num15);
			Vector3 vector8 = new Vector3((num11 * num6 - num12 * num5) * num15, (num11 * num8 - num12 * num7) * num15, (num11 * num10 - num12 * num9) * num15);
			tan1[num2] += vector7;
			tan1[num3] += vector7;
			tan1[num4] += vector7;
			tan2[num2] += vector8;
			tan2[num3] += vector8;
			tan2[num4] += vector8;
			num += 3;
		}
		for (int j = 0; j < vertCount; j++)
		{
			Vector3 normal = normals[j];
			Vector3 tangent = tan1[j];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			tangents[j].x = tangent.x;
			tangents[j].y = tangent.y;
			tangents[j].z = tangent.z;
			tangents[j].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), tan2[j]) < 0f) ? (-1f) : 1f);
		}
	}

	public Mesh GetMesh()
	{
		return mesh;
	}

	public Mesh GetMesh(MeshOption meshOption)
	{
		bool flag = (meshOption & MeshOption.CalculateNormals) != 0;
		bool flag2 = (meshOption & MeshOption.CalculateBounds) != 0;
		bool flag3 = (meshOption & MeshOption.CalculateTangents) != 0;
		bool flag4 = (meshOption & MeshOption.Debug) != 0;
		int num = verts.Length;
		if (flag4)
		{
			string text = "";
			if (flag4)
			{
				for (int i = 0; i < num; i++)
				{
					text = text + "V " + i + ": " + verts[i].ToString() + "\n";
				}
			}
			if (flag4)
			{
				for (int j = 0; j < num; j++)
				{
					text = text + "UV " + j + ": " + uv[j].ToString() + "\n";
				}
			}
			for (int k = 0; k < triIndexCount; k += 3)
			{
				text = text + "Tri " + k + ": " + tris[k] + ", " + tris[k + 1] + ", " + tris[k + 2] + "\n";
			}
			if (!flag)
			{
				for (int l = 0; l < num; l++)
				{
					text = text + "N " + l + ": " + normals[l].ToString() + "\n";
				}
			}
			Debug.Log(text);
		}
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh.Clear();
		}
		mesh.vertices = verts;
		mesh.uv = uv;
		mesh.triangles = tris;
		if (flag3)
		{
			CalculateTangets();
			mesh.tangents = tangents;
		}
		if (!flag)
		{
			mesh.normals = normals;
		}
		else
		{
			mesh.RecalculateNormals();
		}
		if (flag2)
		{
			mesh.RecalculateBounds();
		}
		return mesh;
	}

	public void NormalizeMesh()
	{
		LinkedVert linkedVert = null;
		List<int> list = new List<int>(16);
		linkedVerts = new LinkedVert[vertCount];
		float num = 0.01f;
		for (int i = 0; i < vertCount - 1; i++)
		{
			if (linkedVerts[i] != null)
			{
				continue;
			}
			Vector3 vector = verts[i];
			list.Clear();
			int num2 = 0;
			list.Add(i);
			num2 = 1;
			for (int j = i + 1; j < vertCount; j++)
			{
				if (Vector3.SqrMagnitude(verts[j] - vector) < num)
				{
					list.Add(j);
					num2++;
				}
			}
			linkedVert = new LinkedVert(i);
			linkedVert.meshVertCount = num2;
			linkedVert.meshVerts = list.ToArray();
			for (int k = 0; k < num2; k++)
			{
				linkedVerts[linkedVert.meshVerts[k]] = linkedVert;
			}
		}
		linkedTris = new LinkedTri[triCount];
		int l = 0;
		int num3 = 0;
		for (; l < triIndexCount; l += 3)
		{
			int num4 = tris[l];
			int num5 = tris[l + 1];
			int num6 = tris[l + 2];
			linkedTris[num3] = new LinkedTri(num4, num5, num6, (verts[num4] + verts[num5] + verts[num6]) / 3f);
			linkedVerts[num4].triCount++;
			linkedVerts[num5].triCount++;
			linkedVerts[num6].triCount++;
			num3++;
		}
		LinkedTri[] array = linkedTris;
		foreach (LinkedTri linkedTri in array)
		{
			linkedTri.SetVerts(linkedVerts[linkedTri.a], linkedVerts[linkedTri.b], linkedVerts[linkedTri.c]);
		}
		List<int> list2 = new List<int>();
		for (int n = 0; n < triCount; n++)
		{
			LinkedTri linkedTri2 = linkedTris[n];
			for (int num7 = 0; num7 < triCount; num7++)
			{
				if (n != num7 && linkedTris[num7].HasVertsInTri(linkedTri2))
				{
					list2.Add(num7);
				}
			}
			linkedTri2.links = list2.ToArray();
			list2.Clear();
		}
		array = linkedTris;
		for (int m = 0; m < array.Length; m++)
		{
			array[m].ClearVerts();
		}
	}

	public void CreateLinks()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		LinkedTri[] array = linkedTris;
		foreach (LinkedTri linkedTri in array)
		{
			linkedTri.SetVerts(linkedVerts[linkedTri.a], linkedVerts[linkedTri.b], linkedVerts[linkedTri.c]);
		}
		List<LinkedTri> list = new List<LinkedTri>();
		array = linkedTris;
		foreach (LinkedTri linkedTri2 in array)
		{
			int[] links = linkedTri2.links;
			foreach (int num in links)
			{
				list.Add(linkedTris[num]);
			}
			linkedTri2.SetLinkTris(list.ToArray());
			list.Clear();
		}
		Debug.Log("Links Created: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3"));
	}

	public void ComputeNormals()
	{
		LinkedTri[] array = linkedTris;
		foreach (LinkedTri linkedTri in array)
		{
			linkedTri.edgeAB = verts[linkedTri.bVert.meshVert] - verts[linkedTri.aVert.meshVert];
			linkedTri.edgeAC = verts[linkedTri.cVert.meshVert] - verts[linkedTri.aVert.meshVert];
			linkedTri.normal.x = linkedTri.edgeAB.y * linkedTri.edgeAC.z - linkedTri.edgeAB.z * linkedTri.edgeAC.y;
			linkedTri.normal.y = linkedTri.edgeAB.z * linkedTri.edgeAC.x - linkedTri.edgeAB.x * linkedTri.edgeAC.z;
			linkedTri.normal.z = linkedTri.edgeAB.x * linkedTri.edgeAC.y - linkedTri.edgeAB.y * linkedTri.edgeAC.x;
			linkedTri.normal.Normalize();
		}
		for (int j = 0; j < vertCount; j++)
		{
			normals[j] = Vector3.zero;
		}
		array = linkedTris;
		foreach (LinkedTri linkedTri2 in array)
		{
			Vector3 normalized = (normals[linkedTri2.aVert.meshVert] + linkedTri2.normal).normalized;
			int[] meshVerts = linkedTri2.aVert.meshVerts;
			foreach (int num in meshVerts)
			{
				normals[num] = normalized;
			}
			normalized = (normals[linkedTri2.bVert.meshVert] + linkedTri2.normal).normalized;
			meshVerts = linkedTri2.bVert.meshVerts;
			foreach (int num2 in meshVerts)
			{
				normals[num2] = normalized;
			}
			normalized = (normals[linkedTri2.cVert.meshVert] + linkedTri2.normal).normalized;
			meshVerts = linkedTri2.cVert.meshVerts;
			foreach (int num3 in meshVerts)
			{
				normals[num3] = normalized;
			}
		}
	}

	public void ComputeSelectionNormals()
	{
		foreach (LinkedTri item in selection)
		{
			item.edgeAB = verts[item.bVert.meshVert] - verts[item.aVert.meshVert];
			item.edgeAC = verts[item.cVert.meshVert] - verts[item.aVert.meshVert];
			item.normal.x = item.edgeAB.y * item.edgeAC.z - item.edgeAB.z * item.edgeAC.y;
			item.normal.y = item.edgeAB.z * item.edgeAC.x - item.edgeAB.x * item.edgeAC.z;
			item.normal.z = item.edgeAB.x * item.edgeAC.y - item.edgeAB.y * item.edgeAC.x;
			item.normal.Normalize();
		}
		foreach (LinkedVert selectionVert in selectionVerts)
		{
			int[] meshVerts = selectionVert.meshVerts;
			foreach (int num in meshVerts)
			{
				normals[num] = Vector3.zero;
			}
		}
		foreach (LinkedTri item2 in selection)
		{
			Vector3 normalized = (normals[item2.aVert.meshVert] + item2.normal).normalized;
			int[] meshVerts = item2.aVert.meshVerts;
			foreach (int num2 in meshVerts)
			{
				normals[num2] = normalized;
			}
			normalized = (normals[item2.bVert.meshVert] + item2.normal).normalized;
			meshVerts = item2.bVert.meshVerts;
			foreach (int num3 in meshVerts)
			{
				normals[num3] = normalized;
			}
			normalized = (normals[item2.cVert.meshVert] + item2.normal).normalized;
			meshVerts = item2.cVert.meshVerts;
			foreach (int num4 in meshVerts)
			{
				normals[num4] = normalized;
			}
		}
	}

	public bool Raycast(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, out LinkedTri hitTri, out float hitDistance, out Vector3 hitNormal)
	{
		return RayIntersectMesh(rayPosition, rayDirection, rayDistance, out hitTri, out hitDistance, out hitNormal);
	}

	public bool RayIntersectMesh(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, out LinkedTri hitTri, out float hitDistance, out Vector3 hitNormal)
	{
		float pickDistance = rayDistance;
		float barycentricU = 0f;
		float barycentricV = 0f;
		hitDistance = -1f;
		hitTri = null;
		hitNormal = Vector3.up;
		LinkedTri[] array = linkedTris;
		foreach (LinkedTri linkedTri in array)
		{
			if (RayIntersectTriangle(rayPosition, rayDirection, pickDistance, linkedTri, ref pickDistance, ref barycentricU, ref barycentricV))
			{
				hitTri = linkedTri;
				hitDistance = pickDistance;
				hitNormal = linkedTri.normal;
			}
		}
		if (hitTri == null)
		{
			return false;
		}
		return true;
	}

	public bool RayIntersectTriangle(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, LinkedTri t, ref float pickDistance, ref float barycentricU, ref float barycentricV)
	{
		Vector3 rhs = Vector3.Cross(rayDirection, t.edgeAC);
		float num = Vector3.Dot(t.edgeAB, rhs);
		if (num < 0.0001f)
		{
			return false;
		}
		Vector3 lhs = rayPosition - verts[t.a];
		barycentricU = Vector3.Dot(lhs, rhs);
		if (!(barycentricU < 0f) && barycentricU <= num)
		{
			Vector3 rhs2 = Vector3.Cross(lhs, t.edgeAB);
			barycentricV = Vector3.Dot(rayDirection, rhs2);
			if (!(barycentricV < 0f) && barycentricU + barycentricV <= num)
			{
				pickDistance = Vector3.Dot(t.edgeAC, rhs2);
				if (pickDistance > rayDistance)
				{
					return false;
				}
				float num2 = 1f / num;
				pickDistance *= num2;
				barycentricU *= num2;
				barycentricV *= num2;
				return true;
			}
			return false;
		}
		return false;
	}

	public void SelectMesh(Vector3 point, int triIndex, float fallOffRadius)
	{
		ClearSelection();
		selectionCenter = point;
		selectionRadiusSqr = fallOffRadius * fallOffRadius;
		SelectByLinks(linkedTris[triIndex], selection);
	}

	public void SelectMesh(Vector3 point, LinkedTri tri, float fallOffRadius)
	{
		ClearSelection();
		selectionCenter = point;
		selectionRadiusSqr = fallOffRadius * fallOffRadius;
		SelectByLinks(tri, selection);
	}

	public void SelectByLinks(LinkedTri tri, List<LinkedTri> selection)
	{
		float sqrMagnitude = (tri.midpoint - selectionCenter).sqrMagnitude;
		if (!(sqrMagnitude < selectionRadiusSqr))
		{
			return;
		}
		selection.Add(tri);
		tri.isInSelection = true;
		tri.selectionDelta = sqrMagnitude / selectionRadiusSqr;
		selectionCount++;
		if (!tri.aVert.isInSelection)
		{
			tri.aVert.selectionDelta = 1f - (verts[tri.aVert.meshVert] - selectionCenter).sqrMagnitude / selectionRadiusSqr;
			if (tri.aVert.selectionDelta > 0f)
			{
				tri.aVert.isInSelection = true;
				selectionVerts.Add(tri.aVert);
				selectionVertsCount++;
			}
		}
		if (!tri.bVert.isInSelection)
		{
			tri.bVert.selectionDelta = 1f - (verts[tri.bVert.meshVert] - selectionCenter).sqrMagnitude / selectionRadiusSqr;
			if (tri.bVert.selectionDelta > 0f)
			{
				tri.bVert.isInSelection = true;
				selectionVerts.Add(tri.bVert);
				selectionVertsCount++;
			}
		}
		if (!tri.cVert.isInSelection)
		{
			tri.cVert.selectionDelta = 1f - (verts[tri.cVert.meshVert] - selectionCenter).sqrMagnitude / selectionRadiusSqr;
			if (tri.cVert.selectionDelta > 0f)
			{
				tri.cVert.isInSelection = true;
				selectionVerts.Add(tri.cVert);
				selectionVertsCount++;
			}
		}
		LinkedTri[] linkTris = tri.linkTris;
		foreach (LinkedTri linkedTri in linkTris)
		{
			if (!linkedTri.isInSelection)
			{
				SelectByLinks(linkedTri, selection);
			}
		}
	}

	public void UpdateSelection(bool selectionNormals)
	{
		foreach (LinkedTri item in selection)
		{
			item.midpoint = (verts[item.aVert.meshVert] + verts[item.aVert.meshVert] + verts[item.aVert.meshVert]) / 3f;
		}
		if (selectionNormals)
		{
			ComputeSelectionNormals();
		}
		else
		{
			ComputeNormals();
		}
	}

	public void ClearSelection()
	{
		foreach (LinkedTri item in selection)
		{
			item.isInSelection = false;
		}
		foreach (LinkedVert selectionVert in selectionVerts)
		{
			selectionVert.isInSelection = false;
		}
		selection.Clear();
		selectionCount = 0;
		selectionVerts.Clear();
		selectionVertsCount = 0;
	}
}
