using System;
using UnityEngine;

public class SpaceCenter : MonoBehaviour
{
	public static SpaceCenter Instance;

	public Transform trf;

	public double latitude;

	public double longitude;

	public double AreaRadius = 1500.0;

	public Vector3d srfNVector;

	public CelestialBody cb;

	public Collider spaceCenterAreaTrigger;

	public Transform SpaceCenterTransform
	{
		get
		{
			return trf;
		}
		set
		{
			trf = value;
		}
	}

	public double Latitude => latitude;

	public double Longitude => longitude;

	public Vector3d SrfNVector => srfNVector;

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		GameEvents.onLevelWasLoaded.Add(OnGameSceneLoaded);
	}

	public void Start()
	{
		trf = base.transform;
		cb = Part.GetComponentUpwards<CelestialBody>(base.gameObject);
		Vector2d latitudeAndLongitude = cb.GetLatitudeAndLongitude(base.transform.position);
		latitude = latitudeAndLongitude.x;
		longitude = latitudeAndLongitude.y;
		srfNVector = cb.GetRelSurfaceNVector(latitude, longitude);
	}

	public void OnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(OnGameSceneLoaded);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnGameSceneLoaded(GameScenes scn)
	{
		if (scn == GameScenes.SPACECENTER)
		{
			spaceCenterAreaTrigger.gameObject.SetActive(value: false);
		}
		else
		{
			spaceCenterAreaTrigger.gameObject.SetActive(value: true);
		}
	}

	public double GreatCircleDistance(Vector3d fromSrfNrm)
	{
		return cb.Radius * Math.Acos(Vector3d.Dot(srfNVector, fromSrfNrm));
	}
}
