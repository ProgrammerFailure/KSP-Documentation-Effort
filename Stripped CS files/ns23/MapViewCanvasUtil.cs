using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns23;

public class MapViewCanvasUtil : MonoBehaviour
{
	public static Transform nodeCanvasContainer;

	public static bool nodeCanvasContainerSetup;

	public static Camera canvasCamera;

	public static Canvas mapViewCanvas;

	public static RectTransform mapViewCanvasRect;

	public static Transform NodeContainer => GetNodeCanvasContainer();

	public static Camera CanvasCamera
	{
		get
		{
			if (canvasCamera == null)
			{
				canvasCamera = UIMasterController.Instance.uiCamera;
			}
			return canvasCamera;
		}
	}

	public static Canvas MapViewCanvas
	{
		get
		{
			if (mapViewCanvas == null)
			{
				mapViewCanvas = UIMasterController.Instance.mainCanvas;
			}
			return mapViewCanvas;
		}
	}

	public static RectTransform MapViewCanvasRect
	{
		get
		{
			if (mapViewCanvasRect == null)
			{
				mapViewCanvasRect = MapViewCanvas.GetComponent<RectTransform>();
			}
			return mapViewCanvasRect;
		}
	}

	public static Vector3 ScaledToUISpacePos(Vector3d scaledSpacePos, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		zPositive = PlanetariumCamera.fetch.transform.InverseTransformPoint(scaledSpacePos).z > 0f;
		Vector3 result = PlanetariumCamera.Camera.WorldToViewportPoint(scaledSpacePos);
		result.x = result.x * MapViewCanvasRect.sizeDelta.x - MapViewCanvasRect.sizeDelta.x * 0.5f;
		result.y = result.y * MapViewCanvasRect.sizeDelta.y - MapViewCanvasRect.sizeDelta.y * 0.5f;
		result *= UIMasterController.Instance.uiScale;
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

	public static Vector3 ScaledToUISpacePos(Vector3d scaledSpacePos, RectTransform uiSpace, Camera uiCamera, ref bool zPositive, float zFlattenEasing, float zFlattenMidPoint, float zUIstart, float zUIlength)
	{
		zPositive = uiCamera.transform.InverseTransformPoint(scaledSpacePos).z > 0f;
		Vector3 result = uiCamera.WorldToViewportPoint(scaledSpacePos);
		result.x = result.x * uiSpace.sizeDelta.x - uiSpace.sizeDelta.x * 0.5f;
		result.y = result.y * uiSpace.sizeDelta.y - uiSpace.sizeDelta.y * 0.5f;
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

	public static Transform GetNodeCanvasContainer()
	{
		Canvas canvas = null;
		if (nodeCanvasContainer == null)
		{
			GameObject obj = new GameObject("nodeCanvasContainer");
			canvas = obj.AddComponent<Canvas>();
			obj.AddComponent<GraphicRaycaster>();
			obj.layer = LayerMask.NameToLayer("UI");
			nodeCanvasContainer = obj.transform;
		}
		if (!nodeCanvasContainerSetup && UIMasterController.Instance != null)
		{
			nodeCanvasContainer.transform.NestToParent(UIMasterController.Instance.mainCanvas.transform, resetParent: true);
			nodeCanvasContainer.transform.SetAsFirstSibling();
			if (canvas != null)
			{
				canvas.overrideSorting = true;
				canvas.sortingOrder = -1;
			}
			nodeCanvasContainerSetup = true;
		}
		return nodeCanvasContainer;
	}

	public static Transform ResetNodeCanvasContainer()
	{
		OnUIScaleChange();
		return GetNodeCanvasContainer();
	}

	public static void ResetNodeCanvasContainerScale(float uiScale)
	{
		OnUIScaleChange();
		float num = 1f / uiScale;
		if (nodeCanvasContainer != null)
		{
			nodeCanvasContainer.localScale = Vector3.one * num;
		}
	}

	public static void OnUIScaleChange()
	{
		nodeCanvasContainerSetup = false;
	}
}
