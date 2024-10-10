using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaAspectController : MonoBehaviour, ILayoutController, ILayoutSelfController
{
	public enum AspectMode
	{
		None,
		WidthControlsHeight,
		HeightControlsWidth,
		BestFitMin,
		BestFitMax
	}

	public float desiredContentAspect = 1.3333f;

	public float borderTop = 20f;

	public float borderLeft = 200f;

	public float borderRight = 20f;

	public float borderBottom;

	public AspectMode aspectMode = AspectMode.BestFitMax;

	public float screenBorder = 40f;

	public Canvas screenCanvas;

	public RectTransform rt;

	public RectTransform canvasRT;

	public void Reset()
	{
		desiredContentAspect = 1.3333334f;
	}

	public void Start()
	{
		SetLayout();
	}

	public void OnRectTransformDimensionsChange()
	{
	}

	public void SetLayoutHorizontal()
	{
		SetLayout();
	}

	public void SetLayoutVertical()
	{
		SetLayout();
	}

	public void SetLayout()
	{
		if (screenCanvas == null)
		{
			Debug.LogWarning("AspectController: Must set screen canvas");
			return;
		}
		rt = (RectTransform)base.transform;
		canvasRT = screenCanvas.transform as RectTransform;
		switch (aspectMode)
		{
		case AspectMode.WidthControlsHeight:
			rt.sizeDelta = new Vector2(rt.sizeDelta.x, GetHeightFromWidth());
			break;
		case AspectMode.HeightControlsWidth:
			rt.sizeDelta = new Vector2(GetWidthFromHeight(), rt.sizeDelta.y);
			break;
		case AspectMode.BestFitMin:
		case AspectMode.BestFitMax:
			SetLayoutBestFit();
			break;
		}
		SendMessage("OnAspectControllerLayoutChange", SendMessageOptions.DontRequireReceiver);
	}

	public void SetLayoutBestFit()
	{
		float num = canvasRT.sizeDelta.x - screenBorder;
		float num2 = canvasRT.sizeDelta.y - screenBorder;
		float widthFromHeight = GetWidthFromHeight(num2);
		float heightFromWidth = GetHeightFromWidth(num);
		if (heightFromWidth > num2)
		{
			rt.sizeDelta = new Vector2(widthFromHeight, num2);
		}
		else
		{
			rt.sizeDelta = new Vector2(num, heightFromWidth);
		}
	}

	public float GetWidthFromHeight(float height = -1f)
	{
		if (height <= 0f)
		{
			height = rt.sizeDelta.y - screenBorder;
		}
		float num = height - borderTop - borderBottom;
		return desiredContentAspect * num + borderLeft + borderRight;
	}

	public float GetHeightFromWidth(float width = -1f)
	{
		if (width <= 0f)
		{
			width = rt.sizeDelta.x - screenBorder;
		}
		float num = width - borderLeft - borderRight;
		return 1f / desiredContentAspect * num + borderTop + borderBottom;
	}
}
