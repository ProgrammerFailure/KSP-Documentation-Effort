using System.Collections;
using UnityEngine;

public class DayNightGameObjectSwitch : MonoBehaviour
{
	public GameObject[] objects;

	public double longitude;

	public double latitude;

	public double altitude;

	public CelestialBody body;

	public bool OnDuringNight = true;

	public bool objectState;

	public bool setInitialState;

	public Coroutine coroutine;

	public bool setUpComplete;

	public void Start()
	{
		if (objects == null)
		{
			Object.Destroy(this);
		}
		if (objects.Length == 0)
		{
			Object.Destroy(this);
		}
		GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded);
	}

	public void OnDisable()
	{
		StopAllCoroutines();
		coroutine = null;
		setInitialState = false;
	}

	public void OnEnable()
	{
		setInitialState = false;
	}

	public void OnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(onLevelWasLoaded);
	}

	public void onLevelWasLoaded(GameScenes scene)
	{
		StopAllCoroutines();
		coroutine = null;
		setInitialState = false;
		switch (scene)
		{
		case GameScenes.SPACECENTER:
			if (setUpComplete)
			{
				break;
			}
			goto case GameScenes.FLIGHT;
		case GameScenes.FLIGHT:
			Setup();
			break;
		}
	}

	public void Setup()
	{
		body = objects[0].GetComponentInParent<CelestialBody>();
		if (!(body == null))
		{
			body.GetLatLonAlt(objects[0].transform.position, out latitude, out longitude, out altitude);
			if (Sun.Instance == null)
			{
				setObjects(state: true);
			}
			setUpComplete = true;
		}
	}

	public void Update()
	{
		if (HighLogic.fetch != null && (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.SPACECENTER))
		{
			if (!setUpComplete)
			{
				Setup();
			}
			if (setUpComplete && body != null && coroutine == null && base.isActiveAndEnabled)
			{
				coroutine = StartCoroutine(UpdateObjects());
			}
		}
	}

	public IEnumerator UpdateObjects()
	{
		while ((bool)this)
		{
			double localTimeAtPosition = Sun.Instance.GetLocalTimeAtPosition(latitude, longitude, body);
			if (!(localTimeAtPosition < 0.25) && localTimeAtPosition <= 0.699999988079071)
			{
				setObjects(!OnDuringNight);
			}
			else
			{
				setObjects(OnDuringNight);
			}
			yield return new WaitForSeconds(10f);
		}
	}

	public void setObjects(bool state)
	{
		if (objectState == state && setInitialState)
		{
			return;
		}
		for (int i = 0; i < objects.Length; i++)
		{
			if (objects[i].gameObject != null)
			{
				objects[i].gameObject.SetActive(state);
			}
		}
		objectState = state;
		setInitialState = true;
	}
}
