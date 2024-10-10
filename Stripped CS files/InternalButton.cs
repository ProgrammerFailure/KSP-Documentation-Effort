using System.Collections;
using UnityEngine;

public class InternalButton : MonoBehaviour
{
	public delegate void InternalButtonDelegate();

	public InternalButtonDelegate onDown;

	public InternalButtonDelegate onUp;

	public InternalButtonDelegate onTap;

	public InternalButtonDelegate onDoubleTap;

	public InternalButtonDelegate onDrag;

	public InternalButtonDelegate onOver;

	public InternalButtonDelegate onExit;

	public bool isMouseDown;

	public bool isTapStarted;

	public static InternalButton Create(GameObject obj)
	{
		InternalButton component = obj.gameObject.GetComponent<InternalButton>();
		if (component == null)
		{
			return obj.gameObject.AddComponent<InternalButton>();
		}
		return component;
	}

	public void OnDown(InternalButtonDelegate onDownDelegate)
	{
		onDown = onDownDelegate;
	}

	public void OnUp(InternalButtonDelegate onUpDelegate)
	{
		onUp = onUpDelegate;
	}

	public void OnTap(InternalButtonDelegate onTapDelegate)
	{
		onTap = onTapDelegate;
	}

	public void OnDoubleTap(InternalButtonDelegate onDoubleTapDelegate)
	{
		onDoubleTap = onDoubleTapDelegate;
	}

	public void OnDrag(InternalButtonDelegate onDragDelegate)
	{
		onDrag = onDragDelegate;
	}

	public void OnOver(InternalButtonDelegate onOverDelegate)
	{
		onOver = onOverDelegate;
	}

	public void OnExit(InternalButtonDelegate onExitDelegate)
	{
		onExit = onExitDelegate;
	}

	public void OnMouseDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (onDown != null)
			{
				onDown();
			}
			isMouseDown = true;
			if (!isTapStarted)
			{
				StartCoroutine(TapRoutine(0));
			}
		}
	}

	public void OnMouseUp()
	{
		if (Input.GetMouseButtonUp(0))
		{
			isMouseDown = false;
			if (onUp != null)
			{
				onUp();
			}
		}
	}

	public void OnMouseTap()
	{
		if (onTap != null)
		{
			onTap();
		}
	}

	public void OnMouseDoubleTap()
	{
		if (onDoubleTap != null)
		{
			onDoubleTap();
		}
	}

	public void OnMouseDrag()
	{
		if (isMouseDown && onDrag != null)
		{
			onDrag();
		}
	}

	public void OnMouseOver()
	{
		if (onOver != null)
		{
			onOver();
		}
	}

	public void OnMouseExit()
	{
		if (onExit != null)
		{
			onExit();
		}
	}

	public IEnumerator TapRoutine(int btn)
	{
		isTapStarted = true;
		float endTime2 = Time.realtimeSinceStartup + 0.2f;
		bool tapped = false;
		while (!(Time.realtimeSinceStartup >= endTime2))
		{
			if (Input.GetMouseButtonUp(btn))
			{
				OnMouseTap();
				tapped = true;
				break;
			}
			yield return null;
		}
		if (tapped)
		{
			endTime2 = Time.realtimeSinceStartup + 0.2f;
			while (!(Time.realtimeSinceStartup >= endTime2))
			{
				if (Input.GetMouseButtonDown(btn))
				{
					OnMouseDoubleTap();
					break;
				}
				yield return null;
			}
		}
		isTapStarted = false;
	}
}
