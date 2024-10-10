using UnityEngine;

public class ExplosionDestroyer : MonoBehaviour
{
	public float explosionDuration = 1f;

	public float startTime;

	public void Awake()
	{
		startTime = Time.time;
	}

	public void Update()
	{
		if ((Time.time - startTime) / explosionDuration >= 1f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
