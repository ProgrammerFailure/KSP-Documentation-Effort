using UnityEngine;

public class PQSMeshWrapper : MonoBehaviour
{
	public Mesh sphereMesh;

	public GClass4 targetPQS;

	public double outputRadius;

	public string outputName;

	public Mesh CreateWrappedMesh()
	{
		LinkedMesh linkedMesh = new LinkedMesh(sphereMesh);
		LinkedMesh.LinkedVert[] linkedVerts = linkedMesh.linkedVerts;
		int num = linkedVerts.Length;
		Vector3[] vertices = sphereMesh.vertices;
		double[] array = new double[num];
		Vector3d[] array2 = new Vector3d[num];
		double num2 = double.MaxValue;
		double num3 = double.MinValue;
		targetPQS.SetupExternalRender();
		for (int i = 0; i < num; i++)
		{
			Vector3d vector3d = vertices[linkedVerts[i].meshVert];
			vector3d.Normalize();
			double surfaceHeight = targetPQS.GetSurfaceHeight(vector3d);
			if (num2 > surfaceHeight)
			{
				num2 = surfaceHeight;
			}
			if (num3 < surfaceHeight)
			{
				num3 = surfaceHeight;
			}
			array2[i] = vector3d;
			array[i] = surfaceHeight;
		}
		targetPQS.CloseExternalRender();
		double num4 = outputRadius / num2;
		for (int j = 0; j < num; j++)
		{
			Vector3 vector = array2[j] * (array[j] * num4);
			for (int k = 0; k < linkedVerts[j].meshVertCount; k++)
			{
				linkedMesh.verts[linkedVerts[j].meshVerts[k]] = vector;
			}
		}
		linkedMesh.ComputeNormals();
		return linkedMesh.GetMesh(LinkedMesh.MeshOption.CalculateBounds | LinkedMesh.MeshOption.CalculateTangents);
	}

	public Mesh CreateWrappedMesh2()
	{
		Vector3[] vertices = sphereMesh.vertices;
		Vector2[] uv = sphereMesh.uv;
		Vector3[] normals = sphereMesh.normals;
		int[] triangles = sphereMesh.triangles;
		int num = vertices.Length;
		double[] array = new double[num];
		Vector3d[] array2 = new Vector3d[num];
		double num2 = double.MaxValue;
		double num3 = double.MinValue;
		targetPQS.SetupExternalRender();
		for (int i = 0; i < num; i++)
		{
			Vector3d vector3d = vertices[i];
			vector3d.Normalize();
			double surfaceHeight = targetPQS.GetSurfaceHeight(vector3d);
			if (num2 > surfaceHeight)
			{
				num2 = surfaceHeight;
			}
			if (num3 < surfaceHeight)
			{
				num3 = surfaceHeight;
			}
			array2[i] = vector3d;
			array[i] = surfaceHeight;
		}
		targetPQS.CloseExternalRender();
		double num4 = outputRadius / num2;
		for (int j = 0; j < num; j++)
		{
			double surfaceHeight = array[j] * num4;
			vertices[j] = array2[j] * surfaceHeight;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = sphereMesh.uv;
		mesh.triangles = sphereMesh.triangles;
		mesh.normals = sphereMesh.normals;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		normals = sphereMesh.normals;
		mesh.tangents = CreateTangents(vertices, uv, normals, triangles);
		return mesh;
	}

	public static Vector4[] CreateTangents(Vector3[] verts, Vector2[] uvs, Vector3[] normals, int[] tris)
	{
		int num = verts.Length;
		int num2 = tris.Length / 3;
		Vector4[] array = new Vector4[num];
		Vector3[] array2 = new Vector3[num];
		Vector3[] array3 = new Vector3[num];
		int num3 = 0;
		int num4 = 0;
		while (num3 < num2)
		{
			int num5 = tris[num4];
			int num6 = tris[num4 + 1];
			int num7 = tris[num4 + 2];
			Vector3 vector = verts[num5];
			Vector3 vector2 = verts[num6];
			Vector3 vector3 = verts[num7];
			Vector3 vector4 = uvs[num5];
			Vector3 vector5 = uvs[num6];
			Vector3 vector6 = uvs[num7];
			float num8 = vector2.x - vector.x;
			float num9 = vector3.x - vector.x;
			float num10 = vector2.y - vector.y;
			float num11 = vector3.y - vector.y;
			float num12 = vector2.z - vector.z;
			float num13 = vector3.z - vector.z;
			float num14 = vector5.x - vector4.x;
			float num15 = vector6.x - vector4.x;
			float num16 = vector5.y - vector4.y;
			float num17 = vector6.y - vector4.y;
			float num18 = 1f / (num14 * num17 - num15 * num16);
			Vector3 vector7 = new Vector3((num17 * num8 - num16 * num9) * num18, (num17 * num10 - num16 * num11) * num18, (num17 * num12 - num16 * num13) * num18);
			Vector3 vector8 = new Vector3((num14 * num9 - num15 * num8) * num18, (num14 * num11 - num15 * num10) * num18, (num14 * num13 - num15 * num12) * num18);
			array2[num5] += vector7;
			array2[num6] += vector7;
			array2[num7] += vector7;
			array3[num5] += vector8;
			array3[num6] += vector8;
			array3[num7] += vector8;
			num3++;
			num4 += 3;
		}
		for (num3 = 0; num3 < num; num3++)
		{
			Vector3 normal = normals[num3];
			Vector3 tangent = array2[num3];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			array[num3].x = tangent.x;
			array[num3].y = tangent.y;
			array[num3].z = tangent.z;
			array[num3].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), array3[num3]) < 0f) ? (-1f) : 1f);
		}
		return array;
	}
}
