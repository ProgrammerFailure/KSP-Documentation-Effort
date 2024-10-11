using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Scenery.Scripts;
using KSP.UI.Screens.Mapview;
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
		private LaunchSite host;

		private Transform spawnPointTransform;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpawnPoint()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(LaunchSite host)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Transform GetSpawnPointTransform()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void GetSpawnPointLatLonAlt(out double latitude, out double longitude, out double altitude)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetSpawnPointLatLonAlt()
		{
			throw null;
		}
	}

	public string name;

	public string pqsName;

	public PQS launchsitePQS;

	public PQSCity pqsCity;

	public PQSCity2 pqsCity2;

	private bool isSetup;

	public string launchSiteName;

	public SpawnPoint[] spawnPoints;

	public Transform launchSiteTransform;

	public string launchSiteTransformURL;

	[SerializeField]
	internal string prefabPath;

	[SerializeField]
	internal string[] additionalprefabPaths;

	[SerializeField]
	internal string BundleName;

	public EditorFacility editorFacility;

	public MapNode.SiteType nodeType;

	public PositionMobileLaunchPad positionMobileLaunchPad;

	public bool requiresPOIVisit;

	public string poiName;

	private CelestialBody body;

	private Vector3d srfNVector;

	private bool greatCircleVarsSet;

	public bool isPQSCity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isPQSCity2
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsSetup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CelestialBody Body
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchSite(string name, string pqsName, string launchSiteName, SpawnPoint[] spawnPoints, string launchSiteTransformURL, EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Setup(UnityEngine.Object pqsCity, PQS[] PQSs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpawnPoint GetSpawnPoint(string spawnPointName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSpawnPointsLatLonAlt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetWorldPos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateNodeCaption(MapNode mn, MapNode.CaptionData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GreatCircleDistance(Vector3d fromSrfNrm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupGreatCircleVariables()
	{
		throw null;
	}
}
