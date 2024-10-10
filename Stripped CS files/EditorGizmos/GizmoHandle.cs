using UnityEngine;

namespace EditorGizmos;

public abstract class GizmoHandle : MonoBehaviour, IMouseEvents
{
	[SerializeField]
	public Color normalColor;

	[SerializeField]
	public Color hoverColor;

	[SerializeField]
	public Color downColor;

	[SerializeField]
	public Color disabledColor;

	public bool hover;

	public bool drag;

	[SerializeField]
	public Renderer primaryRenderer;

	[SerializeField]
	public Renderer highlightRenderer;

	public bool Hover => hover;

	public bool Drag => drag;

	public GizmoHandle()
	{
	}

	public void OnMouseEnter()
	{
		if (!hover && CanHover())
		{
			hover = true;
			primaryRenderer.material.color = hoverColor;
			On_MouseEnter();
		}
	}

	public void OnMouseDown()
	{
		if (hover)
		{
			drag = true;
			primaryRenderer.material.color = downColor;
			On_MouseDown();
		}
	}

	public void OnMouseDrag()
	{
		if (drag)
		{
			On_MouseDrag();
		}
	}

	public void OnMouseUp()
	{
		if (!hover)
		{
			OnMouseOut();
		}
		else
		{
			primaryRenderer.material.color = hoverColor;
		}
		On_MouseUp();
		drag = false;
	}

	public void OnMouseExit()
	{
		if (!drag && CanHover())
		{
			OnMouseOut();
		}
		On_MouseExit();
		hover = false;
	}

	public void OnMouseOut()
	{
		primaryRenderer.material.color = normalColor;
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}

	public void SetLock(bool lockSt)
	{
		if (lockSt)
		{
			primaryRenderer.material.color = disabledColor;
			if (highlightRenderer != null)
			{
				highlightRenderer.enabled = false;
			}
		}
		else
		{
			primaryRenderer.material.color = (hover ? hoverColor : normalColor);
			if (highlightRenderer != null)
			{
				highlightRenderer.enabled = true;
			}
		}
	}

	public void BaseSetup()
	{
		primaryRenderer = GetComponent<Renderer>();
		primaryRenderer.material.color = normalColor;
	}

	public abstract bool CanHover();

	public abstract void On_MouseEnter();

	public abstract void On_MouseDown();

	public abstract void On_MouseDrag();

	public abstract void On_MouseUp();

	public abstract void On_MouseExit();
}
