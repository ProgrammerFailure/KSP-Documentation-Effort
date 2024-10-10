using UnityEngine;

namespace ns16;

public static class RectUtil
{
	public enum ContainmentLevel
	{
		None,
		Partial,
		Full,
		Enclosing
	}

	public static Vector3[] rectCorners = new Vector3[4];

	public static ContainmentLevel GetRectContainment(RectTransform self, RectTransform ctr, Camera refCamera, bool testOppositeCase)
	{
		if (!(self == null) && !(ctr == null) && !(refCamera == null))
		{
			self.GetWorldCorners(rectCorners);
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(refCamera, rectCorners[0]);
			Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(refCamera, rectCorners[1]);
			Vector2 screenPoint3 = RectTransformUtility.WorldToScreenPoint(refCamera, rectCorners[2]);
			Vector2 screenPoint4 = RectTransformUtility.WorldToScreenPoint(refCamera, rectCorners[3]);
			bool flag = RectTransformUtility.RectangleContainsScreenPoint(ctr, screenPoint, refCamera);
			bool flag2 = RectTransformUtility.RectangleContainsScreenPoint(ctr, screenPoint2, refCamera);
			bool flag3 = RectTransformUtility.RectangleContainsScreenPoint(ctr, screenPoint3, refCamera);
			bool flag4 = RectTransformUtility.RectangleContainsScreenPoint(ctr, screenPoint4, refCamera);
			if (flag && flag2 && flag3 && flag4)
			{
				return ContainmentLevel.Full;
			}
			if (flag || flag2 || flag3 || flag4)
			{
				return ContainmentLevel.Partial;
			}
			if (testOppositeCase)
			{
				ContainmentLevel rectContainment = GetRectContainment(ctr, self, refCamera, testOppositeCase: false);
				if (rectContainment == ContainmentLevel.Full)
				{
					return ContainmentLevel.Enclosing;
				}
				return rectContainment;
			}
			return ContainmentLevel.None;
		}
		return ContainmentLevel.None;
	}

	public static Vector3 WorldToUISpacePos(Vector3 worldSpacePos, Camera refCamera, RectTransform canvasRect, ref bool zPositive)
	{
		return WorldToUISpacePos(worldSpacePos, refCamera, canvasRect, ref zPositive, 0f, 0f, 0f, 0f);
	}

	public static Vector3 WorldToUISpacePos(Vector3 worldSpacePos, Camera refCamera, RectTransform canvasRect, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		zPositive = refCamera.transform.InverseTransformPoint(worldSpacePos).z > 0f;
		Vector3 result = refCamera.WorldToViewportPoint(worldSpacePos);
		result.x = result.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x * 0.5f;
		result.y = result.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y * 0.5f;
		if (zFlattenEasing == 0f)
		{
			result.z = 0f;
		}
		else
		{
			result.z = (float)UtilMath.Flatten(result.z, zFlattenMidPoint, zFlattenEasing) * zUIlength + zUIstart;
		}
		return result;
	}
}
