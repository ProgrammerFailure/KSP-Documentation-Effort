using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro), typeof(Collider))]
public class TextProButton3D : MonoBehaviour, IMouseEvents
{
	public Color normalColor;

	public Color hoverColor;

	public Color downColor;

	public Color disabledColor;

	public Callback onPressed = delegate
	{
	};

	public Callback onReleased = delegate
	{
	};

	public Callback onTap = delegate
	{
	};

	public bool lockButton;

	public bool isHover;

	public bool isBeingHovered;

	public TextMeshPro text;

	public BoxCollider boxCollider;

	public Color backupNormalColor;

	public bool tapping;

	public bool tapped;

	public TextMeshPro Text
	{
		get
		{
			if (text == null)
			{
				text = GetComponent<TextMeshPro>();
			}
			return text;
		}
	}

	public void Awake()
	{
		backupNormalColor = normalColor;
		boxCollider = GetComponent<BoxCollider>();
		SetEnable(boxCollider.enabled);
	}

	public void SetEnable(bool enable)
	{
		boxCollider.enabled = enable;
		if (enable)
		{
			normalColor = backupNormalColor;
			Text.color = normalColor;
		}
		else
		{
			normalColor = disabledColor;
			Text.color = normalColor;
		}
	}

	public void OnMouseEnter()
	{
		if (!lockButton)
		{
			Text.color = hoverColor;
			isBeingHovered = true;
			onPressed();
		}
	}

	public void Highlight()
	{
		if (!lockButton)
		{
			Text.color = hoverColor;
			isBeingHovered = true;
		}
	}

	public void OnMouseDown()
	{
		if (!lockButton)
		{
			Text.color = downColor;
			onReleased();
			if (!tapping)
			{
				StartCoroutine(OnMouseTap());
			}
		}
	}

	public void OnMouseUp()
	{
		if (!lockButton)
		{
			if (tapping)
			{
				tapped = true;
			}
			Text.color = hoverColor;
		}
	}

	public void OnMouseExit()
	{
		isBeingHovered = false;
		if (!lockButton)
		{
			Text.color = normalColor;
		}
	}

	public void UnHighlight()
	{
		isBeingHovered = false;
		if (!lockButton)
		{
			Text.color = normalColor;
		}
	}

	public IEnumerator OnMouseTap()
	{
		float delay = 0.5f;
		tapping = true;
		tapped = false;
		while (delay >= 0f && !tapped)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		if (tapped)
		{
			onTap();
		}
		tapping = false;
		tapped = false;
	}

	public void Lock()
	{
		lockButton = true;
		Text.color = downColor;
	}

	public void Unlock()
	{
		lockButton = false;
		Text.color = normalColor;
	}

	public void KeyStrokeLock()
	{
		Lock();
		isBeingHovered = false;
	}

	public bool IsButtonLocked()
	{
		return lockButton;
	}

	public void OnMouseDrag()
	{
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}
}
