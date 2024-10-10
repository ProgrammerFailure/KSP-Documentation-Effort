using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
{
	public RectTransform panelRectTransform;

	public int edgeOffset = 60;

	public void Start()
	{
		panelRectTransform = base.transform as RectTransform;
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
	}

	public void OnGameSettingsApplied()
	{
		if (UIMasterController.AnyCornerOffScreen(panelRectTransform))
		{
			UIMasterController.DragTooltip(panelRectTransform, Vector2.zero, Vector3.one * edgeOffset);
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		panelRectTransform.SetAsLastSibling();
	}

	public void OnDrag(PointerEventData data)
	{
		if (!(panelRectTransform == null))
		{
			UIMasterController.DragTooltip(panelRectTransform, data.delta, Vector3.one * edgeOffset);
		}
	}
}
