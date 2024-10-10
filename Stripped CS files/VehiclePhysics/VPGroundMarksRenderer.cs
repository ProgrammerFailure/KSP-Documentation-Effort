using EdyCommonTools;
using UnityEngine;
using UnityEngine.Rendering;

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
	public int maxMarks = 1024;

	public float minDistance = 0.1f;

	public float groundOffset = 0.02f;

	public float textureOffsetY = 0.05f;

	[Range(0f, 1f)]
	public float fadeOutRange = 0.5f;

	public Material material;

	public int m_markCount;

	public int m_markArraySize;

	public MarkPoint[] m_markPoints;

	public BiasedRatio m_pressureRatioBias = new BiasedRatio();

	public bool m_segmentsUpdated;

	public int m_segmentCount;

	public int m_segmentArraySize;

	public Mesh m_mesh;

	public Vector3[] m_vertices;

	public Vector3[] m_normals;

	public Vector4[] m_tangents;

	public Color[] m_colors;

	public Vector2[] m_uvs;

	public int[] m_triangles;

	public Vector2[] m_values;

	public void OnEnable()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (base.gameObject.GetComponent<MeshRenderer>() == null)
		{
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.material = material;
		}
		if (maxMarks < 10)
		{
			maxMarks = 10;
		}
		m_markPoints = new MarkPoint[maxMarks * 2];
		int i = 0;
		for (int num = m_markPoints.Length; i < num; i++)
		{
			m_markPoints[i] = new MarkPoint();
		}
		m_markCount = 0;
		m_markArraySize = m_markPoints.Length;
		m_vertices = new Vector3[maxMarks * 4];
		m_normals = new Vector3[maxMarks * 4];
		m_tangents = new Vector4[maxMarks * 4];
		m_colors = new Color[maxMarks * 4];
		m_uvs = new Vector2[maxMarks * 4];
		m_triangles = new int[maxMarks * 6];
		m_values = new Vector2[maxMarks];
		m_segmentCount = 0;
		m_segmentArraySize = maxMarks;
		m_segmentsUpdated = false;
		for (int j = 0; j < m_segmentArraySize; j++)
		{
			m_uvs[j * 4] = new Vector2(0f, textureOffsetY);
			m_uvs[j * 4 + 1] = new Vector2(1f, textureOffsetY);
			m_uvs[j * 4 + 2] = new Vector2(0f, 1f - textureOffsetY);
			m_uvs[j * 4 + 3] = new Vector2(1f, 1f - textureOffsetY);
			m_triangles[j * 6] = j * 4;
			m_triangles[j * 6 + 2] = j * 4 + 1;
			m_triangles[j * 6 + 1] = j * 4 + 2;
			m_triangles[j * 6 + 3] = j * 4 + 2;
			m_triangles[j * 6 + 5] = j * 4 + 1;
			m_triangles[j * 6 + 4] = j * 4 + 3;
		}
		m_mesh = new Mesh();
		m_mesh.name = "VPGroundMarksRenderer Mesh";
		m_mesh.MarkDynamic();
		m_mesh.vertices = m_vertices;
		m_mesh.normals = m_normals;
		m_mesh.tangents = m_tangents;
		m_mesh.colors = m_colors;
		m_mesh.triangles = m_triangles;
		m_mesh.uv = m_uvs;
		m_mesh.RecalculateBounds();
		meshFilter.mesh = m_mesh;
	}

	public void OnValidate()
	{
		if (m_uvs != null)
		{
			for (int i = 0; i < m_uvs.Length / 4; i++)
			{
				m_uvs[i * 4] = new Vector2(0f, textureOffsetY);
				m_uvs[i * 4 + 1] = new Vector2(1f, textureOffsetY);
				m_uvs[i * 4 + 2] = new Vector2(0f, 1f - textureOffsetY);
				m_uvs[i * 4 + 3] = new Vector2(1f, 1f - textureOffsetY);
			}
		}
		m_segmentsUpdated = true;
	}

	public int AddMark(Vector3 pos, Vector3 normal, float pressureRatio, float skidRatio, float width, int lastIndex)
	{
		if (base.isActiveAndEnabled && m_markArraySize != 0)
		{
			pressureRatio = m_pressureRatioBias.BiasedLerp(pressureRatio, 0.5f + pressureBoost * 0.5f);
			float num = 0f;
			switch (mode)
			{
			case Mode.PressureOnly:
				num = Mathf.Clamp01(pressureRatio);
				break;
			case Mode.PressureAndSkid:
				num = Mathf.Clamp01(pressureRatio * skidRatio);
				break;
			}
			if (num <= 0f)
			{
				return -1;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			Vector3 vector = pos + normal * groundOffset;
			if (lastIndex >= 0 && Vector3.Distance(vector, m_markPoints[lastIndex % m_markArraySize].pos) < minDistance)
			{
				return lastIndex;
			}
			MarkPoint markPoint = m_markPoints[m_markCount % m_markArraySize];
			markPoint.pos = vector;
			markPoint.normal = normal;
			markPoint.intensity = num;
			markPoint.lastIndex = lastIndex;
			if (lastIndex >= 0 && lastIndex > m_markCount - m_markArraySize)
			{
				MarkPoint markPoint2 = m_markPoints[lastIndex % m_markArraySize];
				Vector3 normalized = Vector3.Cross(markPoint.pos - markPoint2.pos, normal).normalized;
				Vector3 vector2 = 0.5f * width * normalized;
				markPoint.posl = markPoint.pos + vector2;
				markPoint.posr = markPoint.pos - vector2;
				markPoint.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
				if (markPoint2.lastIndex < 0)
				{
					markPoint2.tangent = markPoint.tangent;
					markPoint2.posl = markPoint.pos + vector2;
					markPoint2.posr = markPoint.pos - vector2;
				}
				AddSegment(markPoint2, markPoint);
			}
			m_markCount++;
			return m_markCount - 1;
		}
		return -1;
	}

	public void Clear()
	{
		if (base.isActiveAndEnabled)
		{
			m_markCount = 0;
			m_segmentCount = 0;
			int i = 0;
			for (int num = m_vertices.Length; i < num; i++)
			{
				m_vertices[i] = Vector3.zero;
			}
			m_mesh.vertices = m_vertices;
			m_segmentsUpdated = true;
		}
	}

	public void AddSegment(MarkPoint first, MarkPoint second)
	{
		int num = m_segmentCount % m_segmentArraySize * 4;
		m_vertices[num] = first.posl;
		m_vertices[num + 1] = first.posr;
		m_vertices[num + 2] = second.posl;
		m_vertices[num + 3] = second.posr;
		m_normals[num] = first.normal;
		m_normals[num + 1] = first.normal;
		m_normals[num + 2] = second.normal;
		m_normals[num + 3] = second.normal;
		m_tangents[num] = first.tangent;
		m_tangents[num + 1] = first.tangent;
		m_tangents[num + 2] = second.tangent;
		m_tangents[num + 3] = second.tangent;
		m_colors[num].a = first.intensity;
		m_colors[num + 1].a = first.intensity;
		m_colors[num + 2].a = second.intensity;
		m_colors[num + 3].a = second.intensity;
		m_values[num / 4] = new Vector2(first.intensity, second.intensity);
		if (m_segmentCount == 0)
		{
			Vector3 vector = m_vertices[0];
			int i = 4;
			for (int num2 = m_vertices.Length; i < num2; i++)
			{
				m_vertices[i] = vector;
			}
		}
		m_segmentCount++;
		m_segmentsUpdated = true;
	}

	public void LateUpdate()
	{
		if (!m_segmentsUpdated)
		{
			return;
		}
		m_segmentsUpdated = false;
		int num = (int)((float)m_segmentArraySize * fadeOutRange);
		if (num > 0)
		{
			int num2 = m_segmentCount - m_segmentArraySize;
			int num3 = 0;
			if (num2 < 0)
			{
				num3 = -num2;
				num2 = 0;
			}
			float num4 = 1f / (float)num;
			for (int i = num3; i < num; i++)
			{
				int num5 = num2 % m_segmentArraySize;
				int num6 = num5 * 4;
				float num7 = (float)i * num4;
				float a = m_values[num5].x * num7;
				float a2 = m_values[num5].y * num7 + num4;
				m_colors[num6].a = a;
				m_colors[num6 + 1].a = a;
				m_colors[num6 + 2].a = a2;
				m_colors[num6 + 3].a = a2;
				num2++;
			}
		}
		m_mesh.MarkDynamic();
		m_mesh.vertices = m_vertices;
		m_mesh.normals = m_normals;
		m_mesh.tangents = m_tangents;
		m_mesh.colors = m_colors;
		m_mesh.RecalculateBounds();
	}
}
