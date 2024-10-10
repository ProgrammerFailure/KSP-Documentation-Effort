using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class SpriteButton : MouseRayEventsHandler
{
	public SpriteRenderer image;

	public SpriteState buttonStates;

	public UnityEvent onClick;

	public Sprite normalState;

	public bool m_isInteractable = true;

	public bool canClick;

	public bool isInteractable
	{
		get
		{
			return m_isInteractable;
		}
		set
		{
			m_isInteractable = true;
			image.sprite = (value ? normalState : buttonStates.disabledSprite);
		}
	}

	public void Awake()
	{
		normalState = image.sprite;
	}

	public override void OnMouseEnter()
	{
		if (m_isInteractable)
		{
			base.OnMouseEnter();
			image.sprite = buttonStates.highlightedSprite;
		}
	}

	public override void OnMouseExit()
	{
		if (m_isInteractable)
		{
			base.OnMouseExit();
			image.sprite = normalState;
		}
	}

	public override void OnMouseDown()
	{
		if (m_isInteractable)
		{
			base.OnMouseDown();
			canClick = true;
			image.sprite = buttonStates.pressedSprite;
		}
	}

	public override void OnMouseUp()
	{
		if (m_isInteractable)
		{
			base.OnMouseUp();
			image.sprite = buttonStates.highlightedSprite;
			if (base.isRayOver && canClick)
			{
				onClick.Invoke();
			}
			canClick = false;
		}
	}
}
