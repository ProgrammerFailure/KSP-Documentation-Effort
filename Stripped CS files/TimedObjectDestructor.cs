using UnityEngine;

public class TimedObjectDestructor : MonoBehaviour
{
	public float timeOut = 1f;

	public bool detachChildren;

	public void Awake()
	{
		Invoke("DestroyNow", timeOut);
	}

	public void DestroyNow()
	{
		if (detachChildren)
		{
			base.transform.DetachChildren();
		}
		base.gameObject.DestroyGameObject();
	}
}
