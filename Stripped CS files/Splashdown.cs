using UnityEngine;

public class Splashdown : MonoBehaviour
{
	public float splashBurstDuration = 2.1f;

	public bool SelfDestruct;

	public GameObject[] splashBurstElements;

	public void Awake()
	{
		GetComponent<AudioSource>().volume = GameSettings.SHIP_VOLUME;
	}

	public void Start()
	{
		Invoke("endSplashBurst", splashBurstDuration);
	}

	public void endSplashBurst()
	{
		if (SelfDestruct)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		int num = splashBurstElements.Length;
		while (num-- > 0)
		{
			Object.Destroy(splashBurstElements[num]);
		}
	}
}
