using System;
using Expansions.Missions.Scenery.Scripts;
using ns23;
using ns9;
using UnityEngine;

[Serializable]
public class LaunchSite : ISiteNode
{
	[Serializable]
	public class SpawnPoint
	{
		public string name;

		public string spawnTransformURL;

		public double latitude;

		public double longitude;

		public double altitude;

		public bool latlonaltSet;

		[NonSerialized]
		public LaunchSite host;

		public Transform spawnPointTransform;

		public void Setup(LaunchSite host)
		{
			this.host = host;
		}

		public Transform GetSpawnPointTransform()
		{
			if (spawnPointTransform == null)
			{
				spawnPointTransform = host.launchSiteTransform.Find(spawnTransformURL);
			}
			if (spawnPointTransform == null)
			{
				Debug.Log("[LaunchSite::SpawnPoint]: Cannot find a transform named '" + spawnTransformURL + "' on '" + name + "'");
				return null;
			}
			return spawnPointTransform;
		}

		public void GetSpawnPointLatLonAlt(out double latitude, out double longitude, out double altitude)
		{
			latitude = this.latitude;
			longitude = this.longitude;
			altitude = this.altitude;
		}

		public void SetSpawnPointLatLonAlt()
		{
			if (spawnPointTransform == null)
			{
				spawnPointTransform = host.launchSiteTransform.Find(spawnTransformURL);
			}
			if (spawnPointTransform == null)
			{
				Debug.Log("[LaunchSite::SpawnPoint]: Cannot find a transform named '" + spawnTransformURL + "' on '" + name + "'");
				latlonaltSet = false;
			}
			else if (host.Body != null)
			{
				host.Body.GetLatLonAlt(spawnPointTransform.position, out latitude, out longitude, out altitude);
				latlonaltSet = true;
			}
			else
			{
				Debug.Log("[LaunchSite::SpawnPoint]: (" + name + ") Unable to set Latitude, Longitude, Altitude");
				latlonaltSet = false;
			}
		}
	}

	public string name;

	public string pqsName;

	public GClass4 launchsitePQS;

	public PQSCity pqsCity;

	public PQSCity2 pqsCity2;

	public bool isSetup;

	public string launchSiteName;

	public SpawnPoint[] spawnPoints;

	public Transform launchSiteTransform;

	public string launchSiteTransformURL;

	[SerializeField]
	public string prefabPath;

	[SerializeField]
	public string[] additionalprefabPaths;

	[SerializeField]
	public string BundleName;

	public EditorFacility editorFacility;

	public MapNode.SiteType nodeType;

	public PositionMobileLaunchPad positionMobileLaunchPad;

	public bool requiresPOIVisit;

	public string poiName = "";

	public CelestialBody body;

	public Vector3d srfNVector;

	public bool greatCircleVarsSet;

	public bool isPQSCity => pqsCity != null;

	public bool isPQSCity2 => pqsCity2 != null;

	public bool IsSetup => isSetup;

	public CelestialBody Body
	{
		get
		{
			if (body == null)
			{
				if (isPQSCity)
				{
					body = pqsCity.celestialBody;
				}
				if (isPQSCity2)
				{
					body = pqsCity2.celestialBody;
				}
			}
			return body;
		}
	}

	public LaunchSite(string name, string pqsName, string launchSiteName, SpawnPoint[] spawnPoints, string launchSiteTransformURL, EditorFacility editorFacility)
	{
		this.name = name;
		this.pqsName = pqsName;
		this.launchSiteName = launchSiteName;
		this.spawnPoints = spawnPoints;
		this.launchSiteTransformURL = launchSiteTransformURL;
		this.editorFacility = editorFacility;
	}

	public bool Setup(UnityEngine.Object pqsCity, GClass4[] PQSs)
	{
		if (pqsCity == null)
		{
			Debug.Log("[LaunchSite]: pqsCity cannot be null");
			return false;
		}
		this.pqsCity = pqsCity as PQSCity;
		if (this.pqsCity == null)
		{
			pqsCity2 = pqsCity as PQSCity2;
		}
		if (this.pqsCity == null && pqsCity2 == null)
		{
			Debug.LogError("[LaunchSite]: unable to determine if Object is PQSCity or PQSCity2");
			return false;
		}
		for (int i = 0; i < PQSs.Length; i++)
		{
			if (PQSs[i].gameObject.name == pqsName)
			{
				launchsitePQS = PQSs[i];
				launchSiteTransform = PQSs[i].transform.Find(launchSiteTransformURL);
				if (launchSiteTransform == null)
				{
					launchSiteTransform = PQSs[i].transform.Find(launchSiteTransformURL + "(Clone)");
				}
				break;
			}
		}
		if (launchsitePQS == null)
		{
			Debug.LogError("[LaunchSite]: unable to find LaunchSite PQS : " + pqsName);
			return false;
		}
		if (launchSiteTransform == null)
		{
			Debug.LogError("[LaunchSite]: unable to find LaunchSite Transform :" + launchSiteTransformURL);
			return false;
		}
		for (int j = 0; j < spawnPoints.Length; j++)
		{
			spawnPoints[j].Setup(this);
		}
		if (isPQSCity2 && pqsCity2 != null)
		{
			positionMobileLaunchPad = pqsCity2.transform.GetComponentInChildren<PositionMobileLaunchPad>();
		}
		isSetup = true;
		return true;
	}

	public SpawnPoint GetSpawnPoint(string spawnPointName)
	{
		int num = 0;
		while (true)
		{
			if (num < spawnPoints.Length)
			{
				if (spawnPoints[num].name == spawnPointName)
				{
					break;
				}
				num++;
				continue;
			}
			Debug.Log("[LaunchSite]: Cannot find spawnpoint named '" + spawnPointName + "'");
			return null;
		}
		return spawnPoints[num];
	}

	public void SetSpawnPointsLatLonAlt()
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i].SetSpawnPointLatLonAlt();
		}
	}

	public string GetName()
	{
		return name;
	}

	public Transform GetWorldPos()
	{
		if (launchSiteTransform == null)
		{
			launchSiteTransform = Body.pqsController.transform.Find(launchSiteTransformURL);
			if (launchSiteTransform == null)
			{
				launchSiteTransform = Body.pqsController.transform.Find(launchSiteTransformURL + "(Clone)");
			}
		}
		return launchSiteTransform;
	}

	public void UpdateNodeCaption(MapNode mn, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format(launchSiteName);
	}

	public double GreatCircleDistance(Vector3d fromSrfNrm)
	{
		if (!greatCircleVarsSet)
		{
			setupGreatCircleVariables();
			greatCircleVarsSet = true;
		}
		if (body == null)
		{
			return double.MinValue;
		}
		return body.Radius * Math.Acos(Vector3d.Dot(srfNVector, fromSrfNrm));
	}

	public void setupGreatCircleVariables()
	{
		if (pqsCity2 != null)
		{
			body = pqsCity2.celestialBody;
			srfNVector = body.GetRelSurfaceNVector(pqsCity2.lat, pqsCity2.lon);
		}
		else if (pqsCity != null)
		{
			body = pqsCity.celestialBody;
			body.GetLatLonAlt(pqsCity.transform.position, out var lat, out var lon, out var _);
			srfNVector = body.GetRelSurfaceNVector(lat, lon);
		}
	}
}
