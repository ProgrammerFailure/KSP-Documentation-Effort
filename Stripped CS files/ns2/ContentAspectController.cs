using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class ContentAspectController : MonoBehaviour, ILayoutController, ILayoutSelfController
{
	public enum AspectMode
	{
		None,
		WidthControlsHeight,
		HeightControlsWidth
	}

	public float desiredContentAspect = 1.3333f;

	public float borderTop;

	public float borderBottom;

	public float borderLeft;

	public float borderRight;

	public AspectMode aspectMode;

	public void Reset()
	{
		desiredContentAspect = 1.3333334f;
	}

	public void OnRectTransformDimensionsChange()
	{
	}

	public void SetLayoutHorizontal()
	{
		if (aspectMode == AspectMode.HeightControlsWidth)
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			float y = rectTransform.sizeDelta.y;
			float num = y - borderTop - borderBottom;
			float num2 = (desiredContentAspect * num + borderLeft + borderRight) / y;
			rectTransform.sizeDelta = new Vector2(num2 * rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
		}
	}

	public void SetLayoutVertical()
	{
		if (aspectMode == AspectMode.WidthControlsHeight)
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			float x = rectTransform.sizeDelta.x;
			float num = x - borderLeft - borderRight;
			float num2 = 1f / desiredContentAspect * num + borderTop + borderBottom;
			float num3 = x / num2;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 1f / num3 * rectTransform.sizeDelta.x);
		}
	}
}
