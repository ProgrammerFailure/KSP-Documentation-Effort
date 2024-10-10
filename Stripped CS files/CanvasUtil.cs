using UnityEngine;

public static class CanvasUtil
{
	public static Vector2 ScreenToUISpacePos(Vector3 screenPos, RectTransform canvasRect, out bool zPositive)
	{
		zPositive = screenPos.z > 0f;
		Vector3 vector = new Vector3(screenPos.x / (float)Screen.width, screenPos.y / (float)Screen.height, 0f);
		vector.x = vector.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x * 0.5f;
		vector.y = vector.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y * 0.5f;
		return vector;
	}

	public static Vector2 AnchorOffset(Vector2 uiPos, RectTransform rt, Vector2 anchor)
	{
		Vector2 vector = new Vector2(rt.rect.width * anchor.x, rt.rect.height * anchor.y);
		return uiPos + vector;
	}

	public static bool RectContains(RectTransform rt, Vector3 screenPos)
	{
		screenPos.y = (float)Screen.height - screenPos.y;
		screenPos = (Vector2)screenPos - rt.anchoredPosition;
		return rt.rect.Contains(screenPos);
	}

	public static bool Contains(this RectTransform rt, Vector3 screenPos)
	{
		return RectContains(rt, screenPos);
	}
}
