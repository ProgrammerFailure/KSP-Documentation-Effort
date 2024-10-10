using UnityEngine;

public class DebugDrawUtil
{
	public static void DrawCrosshairs(Vector3 position, float size, Color color, float duration)
	{
		Debug.DrawLine(position + Vector3.left * size, position + Vector3.right * size, color, duration);
		Debug.DrawLine(position + Vector3.down * size, position + Vector3.up * size, color, duration);
		Debug.DrawLine(position + Vector3.forward * size, position + Vector3.back * size, color, duration);
	}

	public static void DrawDirection(Vector3 position, Vector3 direction, float crosshairsSize, float directionSize, Color color, float duration)
	{
		DrawCrosshairs(position, crosshairsSize, color, duration);
		Debug.DrawLine(position, position + direction.normalized * directionSize, color, duration);
	}
}
