using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/City LOD Controller")]
public class PQSCity : PQSSurfaceObject
{
	[Serializable]
	public class LODRange
	{
		public float visibleRange;

		public GameObject[] renderers;

		public List<Renderer> rendererList;

		public GameObject[] objects;

		public bool isActive;

		public void Setup()
		{
			isActive = true;
			if (rendererList != null)
			{
				return;
			}
			rendererList = new List<Renderer>();
			GameObject[] array = renderers;
			foreach (GameObject gameObject in array)
			{
				if (!(gameObject == null))
				{
					Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
					rendererList.AddRange(componentsInChildren);
				}
			}
		}

		public void SetActive(bool active)
		{
			if (active != isActive)
			{
				int i = 0;
				for (int count = rendererList.Count; i < count; i++)
				{
					rendererList[i].enabled = active;
				}
				int j = 0;
				for (int num = objects.Length; j < num; j++)
				{
					GameObject gameObject = objects[j];
					if (!(gameObject == null))
					{
						gameObject.SetActive(active);
					}
				}
			}
			isActive = active;
		}
	}

	public Vector3d planetRelativePosition = Vector3d.zero;

	[HideInInspector]
	public bool debugOrientated;

	public LODRange[] lod;

	public float frameDelta;

	public bool randomizeOnSphere;

	public bool repositionToSphere;

	public bool repositionToSphereSurface;

	public bool repositionToSphereSurfaceAddHeight;

	public Vector3 repositionRadial;

	public double repositionRadiusOffset;

	public bool reorientToSphere;

	public Vector3 reorientInitialUp;

	public float reorientFinalAngle;

	public double lat;

	public double lon;

	public double alt;

	public PSystemSetup.SpaceCenterFacility spaceCenterFacility;

	public LaunchSite launchSite;

	public Collider[] colliders;

	public bool transformsActive = true;

	public bool isLoaded;

	public double loadRange;

	public bool inPOIRange;

	public double poiRange;

	[SerializeField]
	public bool useIndividualPOIRange;

	[SerializeField]
	public float individualPOIRange = 100f;

	public CelestialBody body;

	public Vector3 planetPosition;

	public Quaternion planetRotation;

	public override Vector3d PlanetRelativePosition => base.PlanetRelativePosition;

	public override string SurfaceObjectName => base.gameObject.name;

	public bool InPOIRange => inPOIRange;

	public double POIRange
	{
		get
		{
			if (!useIndividualPOIRange)
			{
				return poiRange;
			}
			return individualPOIRange;
		}
	}

	public CelestialBody celestialBody => body;

	public void Reset()
	{
		frameDelta = 1f;
		repositionToSphere = true;
		repositionRadial = Vector3.back;
		repositionRadiusOffset = 0.0;
		reorientToSphere = true;
		reorientInitialUp = Vector3.up;
		reorientFinalAngle = 0f;
	}

	public virtual void Start()
	{
		body = base.gameObject.GetComponentInParent<CelestialBody>();
		GameEvents.OnPQSCityStarting.Fire(this);
		Orientate(allowReposition: false);
	}

	public virtual void ResetCelestialBody()
	{
		body = base.gameObject.GetComponentInParent<CelestialBody>();
	}

	public override void OnSetup()
	{
		Array.Sort(lod, (LODRange a, LODRange b) => a.visibleRange.CompareTo(b.visibleRange));
		int num = lod.Length;
		while (num-- > 0)
		{
			lod[num].Setup();
			lod[num].SetActive(active: false);
		}
	}

	public override bool OnSphereStart()
	{
		if (!sphere.isAlive && transformsActive)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			transformsActive = false;
		}
		return false;
	}

	public override void OnPostSetup()
	{
		StartCoroutine(PostSetup());
	}

	public IEnumerator PostSetup()
	{
		if (randomizeOnSphere)
		{
			yield return null;
			Randomize();
		}
		Orientate();
		CheckLocals();
	}

	public override void OnSphereReset()
	{
		if (transformsActive)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			transformsActive = false;
		}
	}

	public override void OnSphereActive()
	{
		OnUpdateFinished();
	}

	public override void OnSphereInactive()
	{
		int num = lod.Length;
		while (num-- > 0)
		{
			lod[num].SetActive(active: false);
		}
	}

	public override void OnUpdateFinished()
	{
		if (sphere.target == null)
		{
			int num = lod.Length;
			while (num-- > 0)
			{
				lod[num].SetActive(active: false);
			}
			return;
		}
		if (!transformsActive)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: true);
			}
			transformsActive = true;
		}
		float num2 = Vector3.Distance(sphere.target.transform.position, base.transform.position);
		if (!isLoaded && (double)num2 < loadRange)
		{
			if (base.gameObject != null)
			{
				isLoaded = true;
				GameEvents.OnPQSCityLoaded.Fire(body, base.gameObject.name);
			}
		}
		else if (isLoaded && (double)num2 > loadRange && base.gameObject != null)
		{
			isLoaded = false;
			GameEvents.OnPQSCityUnloaded.Fire(body, base.gameObject.name);
		}
		if (!inPOIRange && (double)num2 < POIRange)
		{
			if (base.gameObject != null)
			{
				inPOIRange = true;
				GameEvents.OnPOIRangeEntered.Fire(body, base.gameObject.name);
			}
		}
		else if (inPOIRange && (double)num2 > POIRange && base.gameObject != null)
		{
			inPOIRange = false;
			GameEvents.OnPOIRangeExited.Fire(body, base.gameObject.name);
		}
		int num3 = lod.Length;
		while (num3-- > 0)
		{
			lod[num3].SetActive(num2 < lod[num3].visibleRange);
		}
	}

	public void Randomize()
	{
		repositionToSphereSurface = true;
		if (body == null)
		{
			Debug.LogWarningFormat("[PQSCity]: {0} is not parented to a valid CelestialBody.", base.name);
			return;
		}
		KSPRandom kSPRandom = new KSPRandom(GetPQSCitySeed(this));
		double latitude = 0.0;
		double longitude = 0.0;
		body.GetRandomLatitudeAndLongitude(ref latitude, ref longitude, waterAllowed: false, spaceCenterAllowed: false, kSPRandom);
		Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
		Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
		Vector3d surfaceNVector = LatLon.GetSurfaceNVector(cf, latitude, longitude);
		repositionRadial = surfaceNVector * sphere.radius;
		reorientFinalAngle = (float)(kSPRandom.NextDouble() * 360.0);
	}

	public void Orientate(bool allowReposition = true)
	{
		if (body == null)
		{
			ResetCelestialBody();
		}
		Vector3d vector3d = repositionRadial.normalized;
		if (repositionToSphere)
		{
			planetRelativePosition = vector3d * (sphere.radius + repositionRadiusOffset);
			base.transform.localPosition = planetRelativePosition;
		}
		if (repositionToSphereSurface && allowReposition)
		{
			double num = sphere.GetSurfaceHeight(vector3d, overrideQuadBuildCheck: true);
			if (repositionToSphereSurfaceAddHeight)
			{
				num += repositionRadiusOffset;
			}
			planetRelativePosition = vector3d * num;
			base.transform.localPosition = planetRelativePosition;
		}
		if (reorientToSphere)
		{
			base.transform.localRotation = Quaternion.FromToRotation(reorientInitialUp, base.transform.localPosition.normalized) * Quaternion.AngleAxis(reorientFinalAngle, Vector3.up);
		}
		if (body == null)
		{
			Debug.LogWarningFormat("[PQSCity]: {0} is not parented to a valid CelestialBody.", base.name);
		}
		else
		{
			body.GetLatLonAlt(base.transform.position, out lat, out lon, out alt);
			if (launchSite != null && launchSite.launchSiteTransform != null)
			{
				launchSite.SetSpawnPointsLatLonAlt();
			}
			if (spaceCenterFacility != null && spaceCenterFacility.facilityTransform != null)
			{
				spaceCenterFacility.SetSpawnPointsLatLonAlt();
			}
		}
		GameEvents.OnPQSCityOrientated.Fire(body, base.gameObject.name);
	}

	public void OrientateToOrigin()
	{
		planetPosition = sphere.transform.position;
		planetRotation = sphere.transform.rotation;
		GameObject gameObject = new GameObject();
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		Transform parent = sphere.transform.parent;
		sphere.transform.parent = gameObject.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.identity;
		sphere.transform.parent = parent;
		UnityEngine.Object.DestroyImmediate(gameObject);
	}

	public void OrientateToWorld()
	{
		sphere.transform.position = planetPosition;
		sphere.transform.rotation = planetRotation;
	}

	public void CheckLocals()
	{
		loadRange = GClass4.GameBindings.GetPQSCityLoadRange(this);
		poiRange = GClass4.GameBindings.GetPOIRange(this);
	}

	public static int GetPQSCitySeed(PQSCity city)
	{
		if (city != null && city.gameObject != null && city.body != null)
		{
			return GetPQSCitySeed(city.gameObject.name, city.body.bodyName);
		}
		return 0;
	}

	public static int GetPQSCitySeed(string name, string body)
	{
		int hashCode_Net = (name + body).GetHashCode_Net35();
		int num = ((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.Seed : 0);
		return (num + hashCode_Net) * (num + hashCode_Net + 1) / 2 + hashCode_Net;
	}

	public static int GetStableHashCode(string str, bool Bit32 = false)
	{
		int num = 5381;
		if (Bit32)
		{
			num = 352654597;
		}
		int num2 = num;
		for (int i = 0; i < str.Length && str[i] != 0; i += 2)
		{
			num = ((num << 5) + num) ^ str[i];
			if (i == str.Length - 1 || str[i + 1] == '\0')
			{
				break;
			}
			num2 = ((num2 << 5) + num2) ^ str[i + 1];
		}
		return num + num2 * 1566083941;
	}
}
