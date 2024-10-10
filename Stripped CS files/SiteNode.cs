using FinePrint;
using ns23;
using ns9;
using UnityEngine;

public class SiteNode : MonoBehaviour
{
	public ISiteNode siteObject;

	public Waypoint wayPoint;

	public MapNode.SiteType siteType;

	public bool showNode;

	public void LateUpdate()
	{
		wayPoint.node.NodeUpdate();
	}

	public void Awake()
	{
		GameEvents.OnMapEntered.Add(OnMapEntered);
		GameEvents.OnMapExited.Add(OnMapExited);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.OnMapEntered.Remove(OnMapEntered);
		GameEvents.OnMapExited.Remove(OnMapExited);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoaded);
		if (wayPoint != null)
		{
			WaypointManager.RemoveWaypoint(wayPoint);
		}
	}

	public static SiteNode Spawn(ISiteNode siteNodeObject)
	{
		SiteNode siteNode = new GameObject(siteNodeObject.GetName() + "_Node").AddComponent<SiteNode>();
		siteNode.siteObject = siteNodeObject;
		SetupWaypoint(siteNode);
		return siteNode;
	}

	public static void SetupWaypoint(SiteNode siteNode)
	{
		siteNode.wayPoint = new Waypoint();
		if (siteNode.siteObject is LaunchSite)
		{
			string text = siteNode.siteObject.GetName();
			LaunchSite launchSite = siteNode.siteObject as LaunchSite;
			siteNode.wayPoint.celestialName = launchSite.Body.name;
			siteNode.wayPoint.latitude = launchSite.spawnPoints[0].latitude;
			siteNode.wayPoint.longitude = launchSite.spawnPoints[0].longitude;
			siteNode.wayPoint.name = PSystemSetup.Instance.GetLaunchSiteDisplayName(text);
			siteNode.wayPoint.id = ((text == "Runway" || text == "Island_Airfield") ? "runway" : "launchsite");
		}
		else if (siteNode.siteObject is KSCSiteNode)
		{
			siteNode.wayPoint.celestialName = "Kerbin";
			double lat = 0.0;
			double lon = 0.0;
			double alt = 0.0;
			PSystemSetup.Instance.GetLaunchSiteBody("Runway").GetLatLonAlt(siteNode.siteObject.GetWorldPos().position, out lat, out lon, out alt);
			siteNode.wayPoint.latitude = lat;
			siteNode.wayPoint.longitude = lon;
			siteNode.wayPoint.altitude = alt;
			siteNode.wayPoint.name = Localizer.Format("#autoLOC_300900");
			siteNode.wayPoint.id = "ksc";
		}
		siteNode.wayPoint.index = 0;
		siteNode.wayPoint.seed = -1;
		siteNode.wayPoint.landLocked = true;
		siteNode.wayPoint.altitude = 0.0;
		siteNode.wayPoint.isOnSurface = true;
		siteNode.wayPoint.isNavigatable = true;
		siteNode.wayPoint.node = siteNode.SetupMapNode();
		siteNode.wayPoint.SetFadeRange();
		WaypointManager.AddWaypoint(siteNode.wayPoint);
	}

	public MapNode SetupMapNode()
	{
		siteType = MapNode.SiteType.LaunchSite;
		if (siteObject is LaunchSite)
		{
			siteType = ((LaunchSite)siteObject).nodeType;
		}
		else if (siteObject is KSCSiteNode)
		{
			siteType = MapNode.SiteType.const_2;
		}
		MapNode mapNode = MapNode.Create(siteObject.GetName(), Color.white, 24, hoverable: true, pinnable: false, blocksInput: false);
		mapNode.OnUpdateVisible += OnUpdateNodeIcon;
		mapNode.OnUpdatePosition += OnUpdateNodePosition;
		mapNode.OnUpdateCaption += OnUpdateNodeCaption;
		mapNode.OnUpdateType += OnUpdateType;
		return mapNode;
	}

	public void OnMapEntered()
	{
		showNode = true;
	}

	public void OnMapExited()
	{
		showNode = false;
	}

	public void OnSceneLoaded(GameScenes scn)
	{
		switch (scn)
		{
		}
		showNode = false;
	}

	public void OnUpdateType(MapNode mn, MapNode.TypeData tData)
	{
		tData.oType = MapObject.ObjectType.Site;
		tData.sType = siteType;
	}

	public void OnUpdateNodeIcon(MapNode n, MapNode.IconData data)
	{
		data.visible = showNode && (MapViewFiltering.vesselTypeFilter & MapViewFiltering.VesselTypeFilter.Site) != 0;
	}

	public Vector3d OnUpdateNodePosition(MapNode n)
	{
		return ScaledSpace.LocalToScaledSpace(siteObject.GetWorldPos().position);
	}

	public void OnUpdateNodeCaption(MapNode mn, MapNode.CaptionData data)
	{
		siteObject.UpdateNodeCaption(mn, data);
	}
}
