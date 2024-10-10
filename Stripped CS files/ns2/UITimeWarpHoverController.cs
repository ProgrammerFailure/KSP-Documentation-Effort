using UnityEngine;

namespace ns2;

public class UITimeWarpHoverController : UIHoverSlidePanel
{
	public int currentRateIndex;

	public Vector2 fixedMaskStart;

	public Vector2 fixedMaskOffset;

	public Transform fixedMask;

	public void Update()
	{
		if (currentRateIndex == TimeWarp.CurrentRateIndex || coroutine != null)
		{
			return;
		}
		currentRateIndex = TimeWarp.CurrentRateIndex;
		if (TimeWarp.CurrentRateIndex > 0)
		{
			locked = true;
			coroutine = StartCoroutine(MoveToState(0f, newState: true));
			return;
		}
		locked = false;
		if (!pointOver)
		{
			coroutine = StartCoroutine(MoveToState(0f, newState: false));
		}
	}

	public void LateUpdate()
	{
		if (fixedMask != null)
		{
			fixedMaskOffset = panel.anchoredPosition;
			fixedMask.localPosition = fixedMaskStart - fixedMaskOffset;
			for (int i = 0; i < fixedMask.childCount; i++)
			{
				fixedMask.GetChild(i).localPosition = fixedMaskOffset;
			}
		}
	}
}
