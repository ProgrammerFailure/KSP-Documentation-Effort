using UnityEngine;

public class MouseEventsHandlerBase : MonoBehaviour
{
	public IMouseEvents handle;

	public virtual void Start()
	{
		handle = FindHandleUpwards(base.transform.parent.gameObject);
	}

	public static IMouseEvents FindHandleUpwards(GameObject go)
	{
		MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();
		int num = components.Length;
		int num2 = 0;
		MonoBehaviour monoBehaviour;
		while (true)
		{
			if (num2 < num)
			{
				monoBehaviour = components[num2];
				if (monoBehaviour is IMouseEvents)
				{
					break;
				}
				num2++;
				continue;
			}
			if (go.transform.parent != null)
			{
				return FindHandleUpwards(go.transform.parent.gameObject);
			}
			return null;
		}
		return monoBehaviour as IMouseEvents;
	}

	public virtual void OnMouseEnter()
	{
		if (handle != null)
		{
			handle.OnMouseEnter();
		}
	}

	public virtual void OnMouseDown()
	{
		if (handle != null)
		{
			handle.OnMouseDown();
		}
	}

	public virtual void OnMouseDrag()
	{
		if (handle != null)
		{
			handle.OnMouseDrag();
		}
	}

	public virtual void OnMouseUp()
	{
		if (handle != null)
		{
			handle.OnMouseUp();
		}
	}

	public virtual void OnMouseExit()
	{
		if (handle != null)
		{
			handle.OnMouseExit();
		}
	}
}
