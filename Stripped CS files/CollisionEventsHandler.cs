using UnityEngine;

public class CollisionEventsHandler : MonoBehaviour
{
	public ICollisionEvents handle;

	public void Start()
	{
		if (handle == null)
		{
			handle = FindHandleUpwards(base.transform.parent.gameObject);
		}
	}

	public void SetHandle(ICollisionEvents h)
	{
		handle = h;
	}

	public static ICollisionEvents FindHandleUpwards(GameObject go)
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
				if (monoBehaviour is ICollisionEvents)
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
		return monoBehaviour as ICollisionEvents;
	}

	public void OnCollisionEnter(Collision c)
	{
		if (handle != null)
		{
			handle.OnCollisionEnter(c);
		}
	}

	public void OnCollisionStay(Collision c)
	{
		if (handle != null)
		{
			handle.OnCollisionStay(c);
		}
	}

	public void OnCollisionExit(Collision c)
	{
		if (handle != null)
		{
			handle.OnCollisionExit(c);
		}
	}
}
