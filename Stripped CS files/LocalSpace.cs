using UnityEngine;

public class LocalSpace : MonoBehaviour
{
	public static LocalSpace fetch;

	public static Transform Transform => fetch.transform;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
			return;
		}
		fetch = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}
}
