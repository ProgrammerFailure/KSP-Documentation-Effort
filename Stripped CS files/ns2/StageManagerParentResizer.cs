using System;
using UnityEngine;

namespace ns2;

public class StageManagerParentResizer : MonoBehaviour
{
	[SerializeField]
	public UIHoverSlidePanel timeWarpSlidePanel;

	[SerializeField]
	public RectTransform resizingPanel;

	public RectTransform slidePanelRT;

	public void Awake()
	{
		if (timeWarpSlidePanel != null)
		{
			slidePanelRT = timeWarpSlidePanel.transform as RectTransform;
			UIHoverSlidePanel uIHoverSlidePanel = timeWarpSlidePanel;
			uIHoverSlidePanel.OnUpdatePosition = (Callback<Vector2>)Delegate.Combine(uIHoverSlidePanel.OnUpdatePosition, new Callback<Vector2>(Resize));
		}
	}

	public void OnDestroy()
	{
		if (timeWarpSlidePanel != null)
		{
			UIHoverSlidePanel uIHoverSlidePanel = timeWarpSlidePanel;
			uIHoverSlidePanel.OnUpdatePosition = (Callback<Vector2>)Delegate.Remove(uIHoverSlidePanel.OnUpdatePosition, new Callback<Vector2>(Resize));
		}
	}

	public void Resize(Vector2 vector)
	{
		resizingPanel.sizeDelta = new Vector2(resizingPanel.sizeDelta.x, vector.y - resizingPanel.anchoredPosition.y - slidePanelRT.sizeDelta.y);
	}
}
