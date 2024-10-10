using UnityEngine;

public class GizmoDrawUtil
{
	public static void DrawCrosshairs(Vector3 position, float size, Color color)
	{
		Gizmos.color = color;
		Gizmos.DrawLine(position + Vector3.left * size, position + Vector3.right * size);
		Gizmos.DrawLine(position + Vector3.down * size, position + Vector3.up * size);
		Gizmos.DrawLine(position + Vector3.forward * size, position + Vector3.back * size);
	}

	public static void DrawReach(Bounds bounds, float radius, Color color)
	{
		Gizmos.color = color;
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.up, Vector3.forward, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.forward, Vector3.right, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.right, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.up, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.forward, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, bounds.extents.z), Vector3.right, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.up, Vector3.forward, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.forward, Vector3.down, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.right, Vector3.forward, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.up, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.forward, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, bounds.extents.z), Vector3.right, Vector3.forward, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.up, Vector3.right, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.forward, Vector3.right, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.left, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.down, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.forward, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, bounds.extents.y, 0f - bounds.extents.z), Vector3.left, Vector3.up, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.up, Vector3.right, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.forward, Vector3.down, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.right, Vector3.down, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.down, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.forward, Vector3.left, 0f, 90f, radius);
		DrawArc(bounds.center + new Vector3(0f - bounds.extents.x, 0f - bounds.extents.y, 0f - bounds.extents.z), Vector3.right, Vector3.down, 0f, 90f, radius);
		Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.up * radius * 2f);
		Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.forward * radius * 2f);
		Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.right * radius * 2f);
	}

	public static void DrawArc(Vector3 pivot, Vector3 axis, Vector3 v0, float fromAngle, float toAngle, float radius, int subdivisions = 12)
	{
		Vector3 vector = Vector3.zero;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i <= subdivisions; i++)
		{
			zero = vector;
			vector = pivot + Quaternion.AngleAxis(Mathf.Lerp(fromAngle, toAngle, Mathf.InverseLerp(0f, subdivisions, i)), axis) * v0 * radius;
			if (i > 0)
			{
				Gizmos.DrawLine(zero, vector);
			}
		}
	}
}
