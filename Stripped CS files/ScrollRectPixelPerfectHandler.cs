using UnityEngine;
using UnityEngine.UI;

public class ScrollRectPixelPerfectHandler : MonoBehaviour
{
	public ScrollRect scrollRect;

	public Scrollbar horizontal;

	public Scrollbar vertical;

	public CanvasPixelPerfectHandler pixelPerfectHandler;

	public void Start()
	{
		pixelPerfectHandler = base.gameObject.GetComponentUpwards<CanvasPixelPerfectHandler>();
		if (!(pixelPerfectHandler == null) && !(scrollRect == null))
		{
			horizontal = scrollRect.horizontalScrollbar;
			vertical = scrollRect.verticalScrollbar;
			if (horizontal != null)
			{
				horizontal.onValueChanged.AddListener(DisablePixelPerfect);
			}
			if (vertical != null)
			{
				vertical.onValueChanged.AddListener(DisablePixelPerfect);
			}
		}
		else
		{
			Object.Destroy(this);
		}
	}

	public void Update()
	{
		if (scrollRect != null && scrollRect.velocity.sqrMagnitude > 0f)
		{
			DisablePixelPerfect();
		}
	}

	public void DisablePixelPerfect(float delta = 0f)
	{
		if ((bool)pixelPerfectHandler)
		{
			pixelPerfectHandler.TemporaryDisable();
		}
	}
}
