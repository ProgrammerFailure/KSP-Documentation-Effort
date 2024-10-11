using System.Runtime.CompilerServices;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Ground Materials/Ground Marks Renderer")]
public class VPGroundMarksRenderer : MonoBehaviour
{
	public enum Mode
	{
		PressureAndSkid,
		PressureOnly
	}

	public Mode mode;

	[Range(0f, 1f)]
	public float pressureBoost;

	[Space(5f)]
	public int maxMarks;

	public float minDistance;

	public float groundOffset;

	public float textureOffsetY;

	[Range(0f, 1f)]
	public float fadeOutRange;

	public Material material;

	private int m_markCount;

	private int m_markArraySize;

	private MarkPoint[] m_markPoints;

	private BiasedRatio m_pressureRatioBias;

	private bool m_segmentsUpdated;

	private int m_segmentCount;

	private int m_segmentArraySize;

	private Mesh m_mesh;

	private Vector3[] m_vertices;

	private Vector3[] m_normals;

	private Vector4[] m_tangents;

	private Color[] m_colors;

	private Vector2[] m_uvs;

	private int[] m_triangles;

	private Vector2[] m_values;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPGroundMarksRenderer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int AddMark(Vector3 pos, Vector3 normal, float pressureRatio, float skidRatio, float width, int lastIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddSegment(MarkPoint first, MarkPoint second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
