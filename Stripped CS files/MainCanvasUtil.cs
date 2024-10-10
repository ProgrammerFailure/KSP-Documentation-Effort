using ns2;
using UnityEngine;

public static class MainCanvasUtil
{
	public static Canvas mainCanvas;

	public static RectTransform mainCanvasRect;

	public static RectTransform MainCanvasRect
	{
		get
		{
			if (mainCanvasRect == null)
			{
				mainCanvasRect = MainCanvas.GetComponent<RectTransform>();
			}
			return mainCanvasRect;
		}
	}

	public static Canvas MainCanvas
	{
		get
		{
			if (mainCanvas == null)
			{
				mainCanvas = UIMasterController.Instance.mainCanvas;
			}
			return mainCanvas;
		}
	}
}
