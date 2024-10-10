using ns2;
using UnityEngine;

public static class DialogCanvasUtil
{
	public static Canvas dialogCanvas;

	public static RectTransform dialogCanvasRect;

	public static RectTransform DialogCanvasRect
	{
		get
		{
			if (dialogCanvasRect == null)
			{
				dialogCanvasRect = DialogCanvas.GetComponent<RectTransform>();
			}
			return dialogCanvasRect;
		}
	}

	public static Canvas DialogCanvas
	{
		get
		{
			if (dialogCanvas == null && UIMasterController.Instance != null)
			{
				dialogCanvas = UIMasterController.Instance.dialogCanvas;
			}
			return dialogCanvas;
		}
	}
}
