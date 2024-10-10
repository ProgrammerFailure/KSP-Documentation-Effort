using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LinkedMeshSO : ScriptableObject
{
	[Serializable]
	public class LinkedTri
	{
		public int aVert;

		public int aIndex;

		public int bVert;

		public int bIndex;

		public int cVert;

		public int cIndex;

		public static int size => 24;

		public LinkedTri(int aVert, int aIndex, int bVert, int bIndex, int cVert, int cIndex)
		{
			this.aVert = aVert;
			this.aIndex = aIndex;
			this.bVert = bVert;
			this.bIndex = bIndex;
			this.cVert = cVert;
			this.cIndex = cIndex;
		}
	}

	[Serializable]
	public class LinkedVert
	{
		public int[] meshVerts;

		public Vector3 origVert;

		public int size => 4 * meshVerts.Length + 12;

		public LinkedVert(int[] meshVerts, Vector3 origVert)
		{
			this.meshVerts = meshVerts;
			this.origVert = origVert;
		}

		public bool Contains(int vertIndex)
		{
			int[] array = meshVerts;
			int num = 0;
			while (true)
			{
				if (num < array.Length)
				{
					if (array[num] == vertIndex)
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

		public int IndexOf(int vertIndex)
		{
			int num = 0;
			int num2 = meshVerts.Length;
			while (true)
			{
				if (num < num2)
				{
					if (meshVerts[num] == vertIndex)
					{
						break;
					}
					num++;
					continue;
				}
				return -1;
			}
			return num;
		}
	}

	[SerializeField]
	public string meshName;

	[SerializeField]
	public LinkedVert[] linkedVerts;

	[SerializeField]
	public LinkedTri[] linkedTris;

	[SerializeField]
	public Vector2[] uvs;

	[SerializeField]
	public int vertCount;

	[SerializeField]
	public int linkedVertCount;

	[SerializeField]
	public int triCount;

	[SerializeField]
	public int triIndexCount;

	[SerializeField]
	public bool isBuilt;

	[SerializeField]
	public int size;

	[SerializeField]
	public bool isLocked;

	public static Vector3[] meshVerts;

	public static Vector2[] meshUV;

	public static Vector3[] meshNormals;

	public static int[] meshTris;

	public static Vector4[] tangents;

	public static Vector3[] tan1;

	public static Vector3[] tan2;

	public static Vector3[] cloneVerts;

	public static Vector2[] cloneUV;

	public string MeshName => meshName;

	public int VertCount => vertCount;

	public int LinkedVertCount => linkedVertCount;

	public int TriCount => triCount;

	public int TriIndexCount => triIndexCount;

	public bool IsBuilt => isBuilt;

	public int Size => size;

	public string SizeString
	{
		get
		{
			if (Size > 1048576)
			{
				return KSPUtil.LocalizeNumber(9.536743E-07f * (float)Size, "F2") + "MB";
			}
			if (Size > 1024)
			{
				return KSPUtil.LocalizeNumber(0.0009765625f * (float)Size, "F1") + "kB";
			}
			return Size + "B";
		}
	}

	public bool IsLocked => isLocked;

	public Vector3[] CloneVerts => cloneVerts;

	public Vector2[] CloneUV => cloneUV;

	public LinkedMeshSO()
	{
		isBuilt = false;
	}

	public void NormalizeMesh(Mesh baseMesh)
	{
		Vector3[] vertices = baseMesh.vertices;
		uvs = baseMesh.uv;
		vertCount = baseMesh.vertexCount;
		int[] triangles = baseMesh.triangles;
		triIndexCount = baseMesh.triangles.Length;
		triCount = triIndexCount / 3;
		List<int> list = new List<int>(16);
		List<LinkedVert> list2 = new List<LinkedVert>();
		float num = 0.001f;
		for (int i = 0; i < vertCount; i++)
		{
			bool flag = false;
			int j = 0;
			for (int count = list2.Count; j < count; j++)
			{
				if (list2[j].Contains(i))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			Vector3 vector = vertices[i];
			list.Clear();
			list.Add(i);
			if (i < vertCount - 1)
			{
				for (int k = i + 1; k < vertCount; k++)
				{
					if (Vector3.SqrMagnitude(vertices[k] - vector) < num)
					{
						list.Add(k);
					}
				}
			}
			list2.Add(new LinkedVert(list.ToArray(), vertices[i]));
		}
		linkedVerts = list2.ToArray();
		linkedVertCount = list2.Count;
		linkedTris = new LinkedTri[triCount];
		int l = 0;
		int num2 = 0;
		for (; l < triIndexCount; l += 3)
		{
			int vertIndex = triangles[l];
			int vertIndex2 = triangles[l + 1];
			int vertIndex3 = triangles[l + 2];
			int aVert = -1;
			int num3 = -1;
			int m = 0;
			for (int count2 = list2.Count; m < count2; m++)
			{
				num3 = list2[m].IndexOf(vertIndex);
				if (num3 != -1)
				{
					aVert = m;
					break;
				}
			}
			int bVert = -1;
			int num4 = -1;
			int n = 0;
			for (int count3 = list2.Count; n < count3; n++)
			{
				num4 = list2[n].IndexOf(vertIndex2);
				if (num4 != -1)
				{
					bVert = n;
					break;
				}
			}
			int cVert = -1;
			int num5 = -1;
			int num6 = 0;
			for (int count4 = list2.Count; num6 < count4; num6++)
			{
				num5 = list2[num6].IndexOf(vertIndex3);
				if (num5 != -1)
				{
					cVert = num6;
					break;
				}
			}
			linkedTris[num2] = new LinkedTri(aVert, num3, bVert, num4, cVert, num5);
			num2++;
		}
	}

	public void CalculateSize()
	{
		size = 0;
		size += triCount * LinkedTri.size;
		for (int i = 0; i < linkedVertCount; i++)
		{
			size += linkedVerts[i].size;
		}
		size += 8 + vertCount;
	}

	public Vector3[] ComputeNormals(Vector3[] verts)
	{
		Vector3[] array = new Vector3[triCount];
		for (int i = 0; i < triCount; i++)
		{
			LinkedTri linkedTri = linkedTris[i];
			Vector3 vector = verts[linkedVerts[linkedTri.bVert].meshVerts[linkedTri.bIndex]] - verts[linkedVerts[linkedTri.aVert].meshVerts[linkedTri.aIndex]];
			Vector3 vector2 = verts[linkedVerts[linkedTri.cVert].meshVerts[linkedTri.cIndex]] - verts[linkedVerts[linkedTri.aVert].meshVerts[linkedTri.aIndex]];
			array[i] = new Vector3(vector.y * vector2.z - vector.z * vector2.y, vector.z * vector2.x - vector.x * vector2.z, vector.x * vector2.y - vector.y * vector2.x);
			array[i].Normalize();
		}
		if (meshNormals == null || meshNormals.Length != vertCount)
		{
			meshNormals = new Vector3[vertCount];
		}
		for (int i = 0; i < vertCount; i++)
		{
			meshNormals[i] = Vector3.zero;
		}
		for (int i = 0; i < triCount; i++)
		{
			LinkedTri linkedTri = linkedTris[i];
			int num = linkedVerts[linkedTri.aVert].meshVerts[linkedTri.aIndex];
			Vector3 normalized = (meshNormals[num] + array[i]).normalized;
			meshNormals[num] = normalized;
			num = linkedVerts[linkedTri.bVert].meshVerts[linkedTri.bIndex];
			normalized = (meshNormals[num] + array[i]).normalized;
			meshNormals[num] = normalized;
			num = linkedVerts[linkedTri.cVert].meshVerts[linkedTri.cIndex];
			normalized = (meshNormals[num] + array[i]).normalized;
			meshNormals[num] = normalized;
		}
		return meshNormals;
	}

	public static Vector4[] CalculateTangets(Vector3[] verts, Vector3[] normals, Vector2[] uv, int[] tris)
	{
		int num = tris.Length;
		int num2 = verts.Length;
		if (tangents == null || tangents.Length != num2)
		{
			tangents = new Vector4[num2];
			tan1 = new Vector3[num2];
			tan2 = new Vector3[num2];
		}
		int num3 = 0;
		for (int i = 0; i < num; i += 3)
		{
			int num4 = tris[num3];
			int num5 = tris[num3 + 1];
			int num6 = tris[num3 + 2];
			Vector3 vector = verts[num4];
			Vector3 vector2 = verts[num5];
			Vector3 vector3 = verts[num6];
			Vector3 vector4 = uv[num4];
			Vector3 vector5 = uv[num5];
			Vector3 vector6 = uv[num6];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			tan1[num4] += vector7;
			tan1[num5] += vector7;
			tan1[num6] += vector7;
			tan2[num4] += vector8;
			tan2[num5] += vector8;
			tan2[num6] += vector8;
			num3 += 3;
		}
		for (int i = 0; i < num2; i++)
		{
			Vector3 normal = normals[i];
			Vector3 tangent = tan1[i];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			tangents[i].x = tangent.x;
			tangents[i].y = tangent.y;
			tangents[i].z = tangent.z;
			tangents[i].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), tan2[i]) < 0f) ? (-1f) : 1f);
		}
		return tangents;
	}

	public void CreateLinkedMesh(Mesh baseMesh, bool setLock)
	{
		if (baseMesh == null)
		{
			Debug.Log("LinkedMesh: Mesh is null, aborting.");
			return;
		}
		if (isLocked)
		{
			Debug.Log("LinkedMesh: Mesh is locked, cannot rebuild.");
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		meshName = baseMesh.name;
		NormalizeMesh(baseMesh);
		CalculateSize();
		isBuilt = true;
		isLocked = true;
		Debug.Log("MeshBuild: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3"));
	}

	public Mesh GetOriginalMesh(bool calcTangents)
	{
		if (meshVerts == null || meshVerts.Length != vertCount)
		{
			meshVerts = new Vector3[vertCount];
			meshUV = new Vector2[vertCount];
			meshNormals = new Vector3[vertCount];
		}
		if (meshTris == null || meshTris.Length != triIndexCount)
		{
			meshTris = new int[triIndexCount];
		}
		LinkedVert[] array = linkedVerts;
		foreach (LinkedVert linkedVert in array)
		{
			int[] array2 = linkedVert.meshVerts;
			foreach (int num in array2)
			{
				meshVerts[num] = linkedVert.origVert;
			}
		}
		int num2 = 0;
		LinkedTri[] array3 = linkedTris;
		foreach (LinkedTri linkedTri in array3)
		{
			meshTris[num2++] = linkedVerts[linkedTri.aVert].meshVerts[linkedTri.aIndex];
			meshTris[num2++] = linkedVerts[linkedTri.bVert].meshVerts[linkedTri.bIndex];
			meshTris[num2++] = linkedVerts[linkedTri.cVert].meshVerts[linkedTri.cIndex];
		}
		meshNormals = ComputeNormals(meshVerts);
		Mesh mesh = new Mesh();
		mesh.vertices = meshVerts;
		mesh.uv = uvs;
		mesh.triangles = meshTris;
		mesh.normals = meshNormals;
		if (calcTangents)
		{
			mesh.tangents = CalculateTangets(meshVerts, meshNormals, uvs, meshTris);
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	public void SetupClone()
	{
		if (cloneVerts == null || cloneVerts.Length != linkedVertCount)
		{
			cloneVerts = new Vector3[linkedVertCount];
			cloneUV = new Vector2[linkedVertCount];
		}
		for (int i = 0; i < linkedVertCount; i++)
		{
			cloneVerts[i] = linkedVerts[i].origVert;
			cloneUV[i] = uvs[linkedVerts[i].meshVerts[0]];
		}
	}

	public Mesh GetCloneMesh(bool calcTangents)
	{
		if (meshVerts == null || meshVerts.Length != vertCount)
		{
			meshVerts = new Vector3[vertCount];
			meshUV = new Vector2[vertCount];
			meshNormals = new Vector3[vertCount];
		}
		if (meshTris == null || meshTris.Length != triIndexCount)
		{
			meshTris = new int[triIndexCount];
		}
		for (int i = 0; i < linkedVertCount; i++)
		{
			int[] array = linkedVerts[i].meshVerts;
			foreach (int num in array)
			{
				meshVerts[num] = cloneVerts[i];
				meshUV[num] = cloneUV[i];
			}
		}
		int num2 = 0;
		LinkedTri[] array2 = linkedTris;
		foreach (LinkedTri linkedTri in array2)
		{
			meshTris[num2++] = linkedVerts[linkedTri.aVert].meshVerts[linkedTri.aIndex];
			meshTris[num2++] = linkedVerts[linkedTri.bVert].meshVerts[linkedTri.bIndex];
			meshTris[num2++] = linkedVerts[linkedTri.cVert].meshVerts[linkedTri.cIndex];
		}
		meshNormals = ComputeNormals(meshVerts);
		Mesh mesh = new Mesh();
		mesh.vertices = meshVerts;
		mesh.uv = meshUV;
		mesh.triangles = meshTris;
		mesh.normals = meshNormals;
		if (calcTangents)
		{
			mesh.tangents = CalculateTangets(meshVerts, meshNormals, meshUV, meshTris);
		}
		mesh.RecalculateBounds();
		return mesh;
	}
}
