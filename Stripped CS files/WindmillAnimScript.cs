using UnityEngine;

public class WindmillAnimScript : MonoBehaviour
{
	public float rotationSpeed = 0.001f;

	public Vector3 rotationAxis;

	public PQSCity2 ReferenceCity;

	public bool paused;

	public void Start()
	{
		rotationSpeed = Random.Range(0.5f, 1f);
		GameEvents.onGamePause.Add(onPause);
		GameEvents.onGameUnpause.Add(onUnPause);
	}

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(onPause);
		GameEvents.onGameUnpause.Remove(onUnPause);
	}

	public void onPause()
	{
		paused = true;
	}

	public void onUnPause()
	{
		paused = false;
	}

	public void Update()
	{
		if (ReferenceCity == null || (ReferenceCity != null && ReferenceCity.InVisibleRange))
		{
			if (FlightDriver.fetch != null)
			{
				paused = FlightDriver.Pause;
			}
			if (!paused)
			{
				base.transform.Rotate(rotationAxis, rotationSpeed);
			}
		}
	}
}
