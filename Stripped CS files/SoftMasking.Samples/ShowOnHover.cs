using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples;

[RequireComponent(typeof(RectTransform))]
public class ShowOnHover : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public CanvasGroup targetGroup;

	public bool _forcedVisible;

	public bool _isPointerOver;

	public bool forcedVisible
	{
		get
		{
			return _forcedVisible;
		}
		set
		{
			if (_forcedVisible != value)
			{
				_forcedVisible = value;
				UpdateVisibility();
			}
		}
	}

	public override void Start()
	{
		base.Start();
		UpdateVisibility();
	}

	public void UpdateVisibility()
	{
		SetVisible(ShouldBeVisible());
	}

	public bool ShouldBeVisible()
	{
		if (!_forcedVisible)
		{
			return _isPointerOver;
		}
		return true;
	}

	public void SetVisible(bool visible)
	{
		if ((bool)targetGroup)
		{
			targetGroup.alpha = (visible ? 1f : 0f);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_isPointerOver = true;
		UpdateVisibility();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isPointerOver = false;
		UpdateVisibility();
	}
}
