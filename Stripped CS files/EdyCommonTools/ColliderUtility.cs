using System.Collections.Generic;
using UnityEngine;

namespace EdyCommonTools;

public static class ColliderUtility
{
	public class LayerCollisionMatrix
	{
		public Dictionary<int, int> m_masksByLayer;

		public LayerCollisionMatrix()
		{
			Refresh();
		}

		public void Refresh()
		{
			m_masksByLayer = new Dictionary<int, int>();
			for (int i = 0; i < 32; i++)
			{
				int num = 0;
				for (int j = 0; j < 32; j++)
				{
					if (!Physics.GetIgnoreLayerCollision(i, j))
					{
						num |= 1 << j;
					}
				}
				m_masksByLayer.Add(i, num);
			}
		}

		public int GetLayerCollisionMask(int layer)
		{
			return m_masksByLayer[layer];
		}
	}

	public static Collider[] GetSolidColliders(Transform transform, bool includeInactive)
	{
		Collider[] componentsInChildren = transform.GetComponentsInChildren<Collider>(includeInactive);
		List<Collider> list = new List<Collider>();
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			if (!collider.isTrigger && !(collider is WheelCollider) && (includeInactive || collider.enabled))
			{
				list.Add(collider);
			}
		}
		return list.ToArray();
	}

	public static void ComputeCentroid(MeshCollider meshCollider, out Vector3 centroid, out float volume)
	{
		centroid = Vector3.zero;
		volume = 0f;
		ComputeCentroidAdditive(meshCollider, ref centroid, ref volume);
		centroid /= 4f * volume;
		volume /= 6f;
	}

	public static void ComputeCentroid(MeshCollider[] meshColliders, out Vector3 centroid, out float volume)
	{
		centroid = Vector3.zero;
		volume = 0f;
		for (int i = 0; i < meshColliders.Length; i++)
		{
			ComputeCentroidAdditive(meshColliders[i], ref centroid, ref volume);
		}
		centroid /= 4f * volume;
		volume /= 6f;
	}

	public static void ComputeCentroidAdditive(MeshCollider meshCollider, ref Vector3 centroid, ref float volume)
	{
		Vector3[] vertices = meshCollider.sharedMesh.vertices;
		int[] triangles = meshCollider.sharedMesh.triangles;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = meshCollider.transform.TransformPoint(vertices[i]);
		}
		float num = Mathf.Sign(meshCollider.transform.localToWorldMatrix.determinant);
		int num2 = triangles.Length / 3;
		for (int j = 0; j < num2; j++)
		{
			Vector3 vector = vertices[triangles[j * 3]];
			Vector3 vector2 = vertices[triangles[j * 3 + 1]];
			Vector3 vector3 = vertices[triangles[j * 3 + 2]];
			float num3 = vector.x * (vector2.y * vector3.z - vector3.y * vector2.z) + vector.y * (vector2.z * vector3.x - vector3.z * vector2.x) + vector.z * (vector2.x * vector3.y - vector3.x * vector2.y);
			num3 *= num;
			centroid += num3 * (vector + vector2 + vector3);
			volume += num3;
		}
	}

	public static void DrawColliderGizmos(Collider[] colliders, Color color, bool includeInactiveInHierarchy, bool includeNonConvex)
	{
		Gizmos.color = color;
		foreach (Collider collider in colliders)
		{
			if (collider == null || (!includeInactiveInHierarchy && !collider.gameObject.activeInHierarchy))
			{
				continue;
			}
			Transform transform = collider.gameObject.transform;
			if (collider is MeshCollider)
			{
				MeshCollider meshCollider = collider as MeshCollider;
				if (includeNonConvex || meshCollider.convex)
				{
					Mesh sharedMesh = meshCollider.sharedMesh;
					Gizmos.DrawMesh(sharedMesh, transform.position, transform.rotation, transform.lossyScale);
					Gizmos.DrawWireMesh(sharedMesh, transform.position, transform.rotation, transform.lossyScale);
				}
			}
			else if (collider is BoxCollider)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
				Gizmos.DrawCube(boxCollider.center, boxCollider.size);
				Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
			}
			else if (collider is SphereCollider)
			{
				SphereCollider sphereCollider = collider as SphereCollider;
				Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
				Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
				Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
			}
		}
	}
}
