using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasPixelPerfectHandler : MonoBehaviour
{
	public Canvas canvas;

	public float delay = 0.2f;

	public void OnDestroy()
	{
		if (IsInvoking())
		{
			CancelInvoke();
		}
	}

	public void TemporaryDisable()
	{
		if (IsInvoking())
		{
			CancelInvoke();
		}
		Disable();
		Invoke("Enable", delay);
	}

	public void Enable()
	{
		if ((bool)canvas && !canvas.pixelPerfect)
		{
			canvas.pixelPerfect = true;
		}
	}

	public void Disable()
	{
		if ((bool)canvas && canvas.pixelPerfect)
		{
			canvas.pixelPerfect = false;
		}
	}
}
