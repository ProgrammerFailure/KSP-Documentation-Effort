using System.Runtime.CompilerServices;
using UnityEngine;

public class PQSMeshWrapper : MonoBehaviour
{
	public Mesh sphereMesh;

	public PQS targetPQS;

	public double outputRadius;

	public string outputName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMeshWrapper()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh CreateWrappedMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh CreateWrappedMesh2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4[] CreateTangents(Vector3[] verts, Vector2[] uvs, Vector3[] normals, int[] tris)
	{
		throw null;
	}
}
