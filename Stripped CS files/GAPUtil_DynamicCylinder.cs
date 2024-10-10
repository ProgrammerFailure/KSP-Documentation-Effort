using System;
using UnityEngine;

public class GAPUtil_DynamicCylinder : MonoBehaviour
{
	public const int RADIAL_SEGMENTS = 30;

	public const int HEIGHT_SEGMENTS = 2;

	public MeshRenderer meshRenderer;

	public MeshFilter meshFilter;

	public Mesh mesh;

	public int numVertexColumns;

	public int numVertexRows;

	public float heightStep;

	public float angleStep;

	public GameObject pivot;

	public float botOffset;

	public void CreateMesh(Material cylinderMaterial)
	{
		pivot = new GameObject();
		pivot.name = "Dynamic Cylinder Pivot";
		pivot.transform.parent = base.transform;
		pivot.transform.localRotation = Quaternion.identity;
		mesh = new Mesh();
		meshFilter = pivot.AddComponent<MeshFilter>();
		meshRenderer = pivot.AddComponent<MeshRenderer>();
		meshRenderer.material = cylinderMaterial;
		numVertexColumns = 31;
		numVertexRows = 2;
		int num = numVertexColumns * numVertexRows;
		int num2 = num;
		int num3 = 120;
		int num4 = 28;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num2];
		int[] array3 = new int[528];
		heightStep = 1f;
		angleStep = (float)Math.PI / 15f;
		float num5 = 1f;
		float num6 = 1f / 30f;
		float num7 = 0.5f;
		for (int i = 0; i < numVertexRows; i++)
		{
			for (int j = 0; j < numVertexColumns; j++)
			{
				float f = (float)j * angleStep;
				if (j == numVertexColumns - 1)
				{
					f = 0f;
				}
				array[i * numVertexColumns + j] = new Vector3(num5 * Mathf.Cos(f), (float)i * heightStep, num5 * Mathf.Sin(f));
				array2[i * numVertexColumns + j] = new Vector2((float)j * num6, (float)i * num7);
				if (i > 0 && j < numVertexColumns - 1)
				{
					int num8 = num4 * 3 + (i - 1) * 30 * 6 + j * 6;
					array3[num8] = i * numVertexColumns + j;
					array3[num8 + 1] = i * numVertexColumns + j + 1;
					array3[num8 + 2] = (i - 1) * numVertexColumns + j;
					array3[num8 + 3] = (i - 1) * numVertexColumns + j;
					array3[num8 + 4] = i * numVertexColumns + j + 1;
					array3[num8 + 5] = (i - 1) * numVertexColumns + j + 1;
				}
			}
		}
		bool flag = true;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = num - numVertexColumns;
		for (int k = 0; k < num4; k++)
		{
			int num13 = k * 3;
			int num14 = (num4 + num3) * 3 + k * 3;
			if (k == 0)
			{
				num11 = 0;
				num9 = 1;
				num10 = numVertexColumns - 2;
				flag = true;
			}
			else if (flag)
			{
				num11 = num10;
				num10--;
			}
			else
			{
				num11 = num9;
				num9++;
			}
			flag = !flag;
			array3[num13] = num10;
			array3[num13 + 1] = num11;
			array3[num13 + 2] = num9;
			array3[num14] = num12 + num9;
			array3[num14 + 1] = num12 + num11;
			array3[num14 + 2] = num12 + num10;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
	}

	public void UpdateValues(float radius, double botHeight, double topHeight, double distanceToCenter)
	{
		Vector3[] vertices = mesh.vertices;
		botOffset = radius / (float)distanceToCenter * 0.15f;
		float num = (float)((botHeight + 1.0) * (double)radius);
		float num2 = (float)((topHeight + 1.0) * (double)radius);
		float[] array = new float[2] { num, num2 };
		botHeight -= (double)botOffset;
		heightStep = Mathf.Abs((float)topHeight - (float)botHeight) * (float)distanceToCenter;
		for (int i = 0; i < numVertexRows; i++)
		{
			for (int j = 0; j < numVertexColumns; j++)
			{
				float f = (float)j * angleStep;
				if (j == numVertexColumns - 1)
				{
					f = 0f;
				}
				vertices[i * numVertexColumns + j] = new Vector3(array[i] * Mathf.Cos(f), (float)i * heightStep, array[i] * Mathf.Sin(f));
			}
		}
		mesh.vertices = vertices;
		pivot.transform.localPosition = new Vector3(0f, (float)(botHeight * distanceToCenter), 0f);
	}

	public Vector3 GetPivotPosition()
	{
		return pivot.transform.position;
	}
}
