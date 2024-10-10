using UnityEngine;

namespace EdyCommonTools;

public class DebugUtility
{
	public static void DrawCrossMark(Vector3 pos, Transform trans, Color col, float length = 0.1f)
	{
		DrawCrossMark(pos, trans.forward, trans.right, trans.up, col, length);
	}

	public static void DrawCrossMark(Vector3 pos, Vector3 forward, Vector3 right, Vector3 up, Color col, float length = 0.1f)
	{
		length *= 0.5f;
		Vector3 vector = forward * length;
		Vector3 vector2 = up * length;
		Vector3 vector3 = right * length;
		Debug.DrawLine(pos - vector, pos + vector, col);
		Debug.DrawLine(pos - vector2, pos + vector2, col);
		Debug.DrawLine(pos - vector3, pos + vector3, col);
	}

	public static void CrossMarkGizmo(Vector3 pos, Transform trans, float length = 0.1f)
	{
		CrossMarkGizmo(pos, trans.forward, trans.right, trans.up, length);
	}

	public static void CrossMarkGizmo(Vector3 pos, Vector3 forward, Vector3 right, Vector3 up, float length = 0.1f)
	{
		length *= 0.5f;
		Vector3 vector = forward * length;
		Vector3 vector2 = up * length;
		Vector3 vector3 = right * length;
		Gizmos.DrawLine(pos - vector, pos + vector);
		Gizmos.DrawLine(pos - vector2, pos + vector2);
		Gizmos.DrawLine(pos - vector3, pos + vector3);
	}

	public static void SphereGizmo(Transform trans, Color col, float radius = 0.1f)
	{
		Gizmos.color = col;
		Gizmos.matrix = Matrix4x4.TRS(trans.position, trans.rotation, trans.lossyScale);
		Gizmos.DrawSphere(Vector3.zero, radius);
		Gizmos.DrawWireSphere(Vector3.zero, radius);
		Gizmos.matrix = Matrix4x4.identity;
	}
}
