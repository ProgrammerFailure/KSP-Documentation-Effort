using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	public float delay = 1f;

	public void Start()
	{
		Invoke("Kill", delay);
	}

	public void Kill()
	{
		Object.Destroy(base.gameObject);
	}
}
