using System.Runtime.CompilerServices;
using UnityEngine;

public class GAPUtil_DynamicCylinder : MonoBehaviour
{
	private const int RADIAL_SEGMENTS = 30;

	private const int HEIGHT_SEGMENTS = 2;

	private MeshRenderer meshRenderer;

	private MeshFilter meshFilter;

	private Mesh mesh;

	private int numVertexColumns;

	private int numVertexRows;

	private float heightStep;

	private float angleStep;

	private GameObject pivot;

	private float botOffset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPUtil_DynamicCylinder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateMesh(Material cylinderMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateValues(float radius, double botHeight, double topHeight, double distanceToCenter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetPivotPosition()
	{
		throw null;
	}
}
