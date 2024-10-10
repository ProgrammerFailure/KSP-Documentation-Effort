using UnityEngine;

namespace ProceduralFairings;

public class ColliderSection
{
	public static Mesh CreateSection(FairingXSection bottom, FairingXSection top, int nSides)
	{
		Vector3[] array = new Vector3[nSides * 2 + 2];
		int[] array2 = new int[2 * (nSides * 6)];
		int num = nSides * 2;
		int num2 = num + 1;
		Vector3 vector = new Vector3(0f, 0f, bottom.r);
		Vector3 vector2 = new Vector3(0f, bottom.h, 0f);
		Vector3 vector3 = new Vector3(0f, 0f, top.r);
		Vector3 vector4 = new Vector3(0f, top.h, 0f);
		float num3 = 360f / (float)(nSides - 1);
		int num4 = 0;
		int num5 = nSides;
		while (num4 < nSides)
		{
			Quaternion quaternion = Quaternion.AngleAxis((float)num4 * num3, Vector3.up);
			array[num4] = quaternion * vector + vector2;
			array[num5] = quaternion * vector3 + vector4;
			num4++;
			num5++;
		}
		array[num] = vector2;
		array[num2] = vector4;
		int i = 0;
		int num6 = 0;
		int num7 = 0;
		for (; i < nSides; i++)
		{
			num6 = ((i != nSides - 1) ? (i + 1) : 0);
			array2[num7++] = i;
			array2[num7++] = num6;
			array2[num7++] = i + nSides;
			array2[num7++] = num6;
			array2[num7++] = num6 + nSides;
			array2[num7++] = i + nSides;
			array2[num7++] = i;
			array2[num7++] = num;
			array2[num7++] = num6;
			array2[num7++] = i + nSides;
			array2[num7++] = num6 + nSides;
			array2[num7++] = num2;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Mesh CreateCapSection(FairingXSection bottom, FairingXSection top, int nSides)
	{
		Vector3[] array = new Vector3[nSides + 1 + 1];
		int[] array2 = new int[2 * (nSides * 3)];
		int num = nSides + 1;
		Vector3 vector = new Vector3(0f, 0f, bottom.r);
		Vector3 vector2 = new Vector3(0f, bottom.h, 0f);
		Vector3 vector3 = new Vector3(0f, top.h, 0f);
		float num2 = 360f / (float)nSides;
		for (int i = 0; i < nSides; i++)
		{
			Quaternion quaternion = Quaternion.AngleAxis((float)i * num2, Vector3.up);
			array[i] = quaternion * vector + vector2;
		}
		array[nSides] = vector3;
		array[num] = vector2;
		int j = 0;
		int num3 = 0;
		int num4 = 0;
		for (; j < nSides; j++)
		{
			num3 = ((j != nSides - 1) ? (j + 1) : 0);
			array2[num4++] = j;
			array2[num4++] = num3;
			array2[num4++] = nSides;
			array2[num4++] = j;
			array2[num4++] = num;
			array2[num4++] = num3;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public static MeshCollider Create(Transform modelTransform, FairingXSection bottom, FairingXSection top, int nSides)
	{
		GameObject gameObject = new GameObject("SectionCollider");
		gameObject.transform.NestToParent(modelTransform);
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		if (top.isCap)
		{
			meshCollider.sharedMesh = CreateCapSection(bottom, top, nSides);
		}
		else
		{
			meshCollider.sharedMesh = CreateSection(bottom, top, nSides);
		}
		return meshCollider;
	}
}
