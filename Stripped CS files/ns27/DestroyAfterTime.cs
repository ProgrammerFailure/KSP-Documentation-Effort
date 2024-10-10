using UnityEngine;

namespace ns27;

public class DestroyAfterTime : MonoBehaviour
{
	public float time = 1f;

	public float startTime;

	public void Start()
	{
		startTime = Time.realtimeSinceStartup;
	}

	public void Update()
	{
		if (Time.realtimeSinceStartup > startTime + time && base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
