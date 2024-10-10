using System;
using System.Collections.Generic;
using Expansions.Missions.Actions;
using FinePrint;
using ns23;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Base
{
	public enum AdditionalEntity
	{
		Kerbal,
		Asteroid,
		Vessel,
		Flag,
		LaunchSite,
		Objective
	}

	public GAPCelestialBody gapRef;

	public AdditionalEntity[] entityTypes;

	public DictionaryValueList<AdditionalEntity, List<GAPSurfaceIcon>> additionalIcons;

	public DictionaryValueList<AdditionalEntity, List<GAPOrbitRenderer>> additionalOrbits;

	public static Dictionary<AdditionalEntity, bool> displayEntity;

	public List<GAPCelestialBody_SurfaceGizmo_Icon> additionalSurfaceGizmos;

	public virtual void Init(GAPCelestialBody gapRef)
	{
		this.gapRef = gapRef;
		entityTypes = (AdditionalEntity[])Enum.GetValues(typeof(AdditionalEntity));
		gapRef.Selector.sliderZoom.gameObject.SetActive(value: true);
		gapRef.Selector.containerFooter.gameObject.SetActive(value: true);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterKebals", "filterKerbalsIcon", "#autoLOC_8007211").button.onClick.AddListener(OnFilterButton_Kerbals);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterVessels", "filterVesselsIcon", "#autoLOC_8007212").button.onClick.AddListener(OnFilterButton_Vessels);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterAsteroids", "filterAsteroidsIcon", "#autoLOC_8007213").button.onClick.AddListener(OnFilterButton_Asteroids);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterComets", "filterCometsIcon", "#autoLOC_6006048").button.onClick.AddListener(OnFilterButton_Comets);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterFlags", "filterFlagsIcon", "#autoLOC_8007214").button.onClick.AddListener(OnFilterButton_Flags);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterLaunchSites", "filterLaunchSitesIcon", "#autoLOC_8006095").button.onClick.AddListener(OnFilterButton_LaunchSites);
		gapRef.Selector.AddSidebarGAPButton("buttonFilterObjectives", "filterObjectivesIcon", "#autoLOC_8006096").button.onClick.AddListener(OnFilterButton_Objectives);
		if (displayEntity == null)
		{
			displayEntity = new Dictionary<AdditionalEntity, bool>();
			for (int i = 0; i < entityTypes.Length; i++)
			{
				displayEntity.Add(entityTypes[i], value: true);
			}
		}
	}

	public virtual void Update()
	{
	}

	public virtual void LateUpdate()
	{
		UpdateAdditionalInfo();
	}

	public virtual void End()
	{
	}

	public virtual void LoadPlanet(CelestialBody newCelestialBody)
	{
		LoadAdditionalInfo(newCelestialBody);
		newCelestialBody.transform.rotation = QuaternionD.identity;
	}

	public virtual void UnloadPlanet()
	{
		gapRef.CelestialBody.transform.rotation = gapRef.StoredCBRotation;
		ClearAdditionalInfo();
	}

	public virtual void OnClick(RaycastHit? hit)
	{
	}

	public virtual void OnClickUp(RaycastHit? hit)
	{
	}

	public virtual void OnMouseOver(Vector2 cameraPoint)
	{
	}

	public virtual void OnDrag(PointerEventData.InputButton arg0, Vector2 arg1)
	{
	}

	public virtual void OnDragEnd(RaycastHit? hit)
	{
	}

	public void LoadAdditionalInfo(CelestialBody body)
	{
		DictionaryValueList<Guid, MENode> nodes = MissionEditorLogic.Instance.EditorMission.nodes;
		additionalIcons = new DictionaryValueList<AdditionalEntity, List<GAPSurfaceIcon>>();
		additionalOrbits = new DictionaryValueList<AdditionalEntity, List<GAPOrbitRenderer>>();
		for (int i = 0; i < entityTypes.Length; i++)
		{
			additionalIcons.Add(entityTypes[i], new List<GAPSurfaceIcon>());
			additionalOrbits.Add(entityTypes[i], new List<GAPOrbitRenderer>());
		}
		additionalSurfaceGizmos = new List<GAPCelestialBody_SurfaceGizmo_Icon>();
		for (int j = 0; j < nodes.Count; j++)
		{
			MENode mENode = nodes.At(j);
			if (MissionEditorLogic.Instance.CurrentSelectedNode.Node == mENode)
			{
				continue;
			}
			if (mENode.IsVesselNode)
			{
				for (int k = 0; k < mENode.actionModules.Count; k++)
				{
					if (mENode.actionModules[k] is ActionCreateFlag)
					{
						ActionCreateFlag actionCreateFlag = mENode.actionModules[k] as ActionCreateFlag;
						if (actionCreateFlag.location != null && actionCreateFlag.location.targetBody == body)
						{
							VesselGroundLocation location = actionCreateFlag.location;
							MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
							typeData.vType = VesselType.Flag;
							CreateSurfaceIcon(AdditionalEntity.Flag, actionCreateFlag.siteName, typeData, location.latitude, location.longitude, location.altitude);
						}
					}
					else if (mENode.actionModules[k] is ActionCreateVessel)
					{
						ActionCreateVessel actionCreateVessel = mENode.actionModules[k] as ActionCreateVessel;
						switch (actionCreateVessel.vesselSituation.location.situation)
						{
						case MissionSituation.VesselStartSituations.ORBITING:
							if (actionCreateVessel.vesselSituation.location.orbitSnapShot != null && actionCreateVessel.vesselSituation.location.orbitSnapShot.Body == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.Ship;
								CreateSimpleOrbit(AdditionalEntity.Vessel, actionCreateVessel.vesselSituation.vesselName, actionCreateVessel.vesselSituation.location.orbitSnapShot.Orbit, typeData);
							}
							break;
						case MissionSituation.VesselStartSituations.LANDED:
							if (actionCreateVessel.vesselSituation.location.vesselGroundLocation != null && actionCreateVessel.vesselSituation.location.vesselGroundLocation.targetBody == body)
							{
								VesselGroundLocation vesselGroundLocation = actionCreateVessel.vesselSituation.location.vesselGroundLocation;
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.Ship;
								CreateSurfaceIcon(AdditionalEntity.Vessel, actionCreateVessel.vesselSituation.vesselName, typeData, vesselGroundLocation.latitude, vesselGroundLocation.longitude, vesselGroundLocation.altitude);
							}
							break;
						}
					}
					else if (mENode.actionModules[k] is ActionCreateAsteroid)
					{
						ActionCreateAsteroid actionCreateAsteroid = mENode.actionModules[k] as ActionCreateAsteroid;
						switch (actionCreateAsteroid.location.locationChoice)
						{
						case ParamChoices_VesselSimpleLocation.Choices.orbit:
							if (actionCreateAsteroid.location.orbit != null && actionCreateAsteroid.location.orbit.Orbit.referenceBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.SpaceObject;
								CreateSimpleOrbit(AdditionalEntity.Asteroid, actionCreateAsteroid.asteroid.name, actionCreateAsteroid.location.orbit.Orbit, typeData);
							}
							break;
						case ParamChoices_VesselSimpleLocation.Choices.landed:
							if (actionCreateAsteroid.location.landed.targetBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								VesselGroundLocation landed = actionCreateAsteroid.location.landed;
								typeData.vType = VesselType.SpaceObject;
								CreateSurfaceIcon(AdditionalEntity.Asteroid, actionCreateAsteroid.asteroid.name, typeData, landed.latitude, landed.longitude, landed.altitude);
							}
							break;
						}
					}
					else if (mENode.actionModules[k] is ActionCreateComet)
					{
						ActionCreateComet actionCreateComet = mENode.actionModules[k] as ActionCreateComet;
						switch (actionCreateComet.location.locationChoice)
						{
						case ParamChoices_VesselSimpleLocation.Choices.orbit:
							if (actionCreateComet.location.orbit != null && actionCreateComet.location.orbit.Orbit.referenceBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.SpaceObject;
								CreateSimpleOrbit(AdditionalEntity.Asteroid, actionCreateComet.comet.name, actionCreateComet.location.orbit.Orbit, typeData);
							}
							break;
						case ParamChoices_VesselSimpleLocation.Choices.landed:
							if (actionCreateComet.location.landed.targetBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								VesselGroundLocation landed2 = actionCreateComet.location.landed;
								typeData.vType = VesselType.SpaceObject;
								CreateSurfaceIcon(AdditionalEntity.Asteroid, actionCreateComet.comet.name, typeData, landed2.latitude, landed2.longitude, landed2.altitude);
							}
							break;
						}
					}
					else
					{
						if (!(mENode.actionModules[k] is ActionCreateKerbal))
						{
							continue;
						}
						ActionCreateKerbal actionCreateKerbal = mENode.actionModules[k] as ActionCreateKerbal;
						string text = "";
						text = ((actionCreateKerbal.missionKerbal.Kerbal != null) ? actionCreateKerbal.missionKerbal.Kerbal.name : ((actionCreateKerbal.missionKerbal.TypeToShow != ProtoCrewMember.KerbalType.Tourist) ? Localizer.Format("#autoLOC_8000152") : Localizer.Format("#autoLOC_8002021")));
						switch (actionCreateKerbal.location.locationChoice)
						{
						case ParamChoices_VesselSimpleLocation.Choices.orbit:
							if (actionCreateKerbal.location.orbit != null && actionCreateKerbal.location.orbit.Orbit.referenceBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.const_11;
								CreateSimpleOrbit(AdditionalEntity.Kerbal, text, actionCreateKerbal.location.orbit.Orbit, typeData);
							}
							break;
						case ParamChoices_VesselSimpleLocation.Choices.landed:
							if (actionCreateKerbal.location.landed != null && actionCreateKerbal.location.landed.targetBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								VesselGroundLocation landed3 = actionCreateKerbal.location.landed;
								typeData.vType = VesselType.const_11;
								CreateSurfaceIcon(AdditionalEntity.Kerbal, text, typeData, landed3.latitude, landed3.longitude, landed3.altitude);
							}
							break;
						}
					}
				}
			}
			if (mENode.IsLaunchPadNode)
			{
				for (int l = 0; l < mENode.actionModules.Count; l++)
				{
					if (mENode.actionModules[l] is ActionCreateLaunchSite)
					{
						ActionCreateLaunchSite actionCreateLaunchSite = mENode.actionModules[l] as ActionCreateLaunchSite;
						if (actionCreateLaunchSite.launchSiteSituation.launchSiteGroundLocation != null && actionCreateLaunchSite.launchSiteSituation.launchSiteGroundLocation.targetBody == body)
						{
							MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Site);
							VesselGroundLocation launchSiteGroundLocation = actionCreateLaunchSite.launchSiteSituation.launchSiteGroundLocation;
							typeData.sType = MapNode.SiteType.LaunchSite;
							CreateSurfaceIcon(AdditionalEntity.LaunchSite, actionCreateLaunchSite.launchSiteSituation.launchSiteName, typeData, launchSiteGroundLocation.latitude, launchSiteGroundLocation.longitude, launchSiteGroundLocation.altitude);
						}
					}
				}
			}
			else
			{
				if (!mENode.HasTestModules)
				{
					continue;
				}
				for (int m = 0; m < mENode.testGroups.Count; m++)
				{
					for (int n = 0; n < mENode.testGroups[m].testModules.Count; n++)
					{
						TestModule testModule = mENode.testGroups[m].testModules[n] as TestModule;
						if (!(testModule != null))
						{
							continue;
						}
						if (testModule.hasWaypoint)
						{
							Waypoint nodeWaypoint = (testModule as INodeWaypoint).GetNodeWaypoint();
							if (nodeWaypoint != null && nodeWaypoint.celestialBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Vessel);
								typeData.vType = VesselType.Flag;
								CreateSurfaceIcon(AdditionalEntity.Objective, mENode.Title, typeData, nodeWaypoint.latitude, nodeWaypoint.longitude, nodeWaypoint.altitude);
								if (nodeWaypoint.radius > 0.0)
								{
									CreateSurfaceAreaGizmo(nodeWaypoint.latitude, nodeWaypoint.longitude, nodeWaypoint.radius);
								}
							}
						}
						if (testModule.hasOrbit)
						{
							Orbit nodeOrbit = (testModule as INodeOrbit).GetNodeOrbit();
							if (nodeOrbit != null && nodeOrbit.referenceBody == body)
							{
								MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Generic);
								CreateSimpleOrbit(AdditionalEntity.Objective, mENode.Title, nodeOrbit, typeData);
							}
						}
					}
				}
			}
		}
		List<LaunchSite> launchSites = PSystemSetup.Instance.LaunchSites;
		double lat;
		double lon;
		double alt;
		for (int num = 0; num < launchSites.Count; num++)
		{
			MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Site);
			typeData.sType = MapNode.SiteType.LaunchSite;
			if (launchSites[num].launchSiteTransform != null && launchSites[num].launchsitePQS == body.pqsController)
			{
				if (launchSites[num].isPQSCity2)
				{
					lat = launchSites[num].pqsCity2.lat;
					lon = launchSites[num].pqsCity2.lon;
					alt = launchSites[num].pqsCity2.alt;
				}
				else
				{
					body.GetLatLonAlt(launchSites[num].launchSiteTransform.position, out lat, out lon, out alt);
				}
				CreateSurfaceIcon(AdditionalEntity.LaunchSite, Localizer.Format(launchSites[num].launchSiteName), typeData, lat, lon, alt);
			}
		}
		if (body == FlightGlobals.GetHomeBody())
		{
			KSCSiteNode kSCSiteNode = new KSCSiteNode();
			body.GetLatLonAlt(kSCSiteNode.GetWorldPos().position, out lat, out lon, out alt);
			MapNode.TypeData typeData = new MapNode.TypeData(MapObject.ObjectType.Site);
			typeData.sType = MapNode.SiteType.const_2;
			CreateSurfaceIcon(AdditionalEntity.LaunchSite, kSCSiteNode.GetDisplayName(), typeData, lat, lon, alt);
		}
		for (int num2 = 0; num2 < entityTypes.Length; num2++)
		{
			SetFilter(entityTypes[num2], displayEntity[entityTypes[num2]]);
		}
		gapRef.Selector.GetSidebarGapButton("buttonFilterKebals").SetState(displayEntity[AdditionalEntity.Kerbal]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterVessels").SetState(displayEntity[AdditionalEntity.Vessel]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterAsteroids").SetState(displayEntity[AdditionalEntity.Asteroid]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterFlags").SetState(displayEntity[AdditionalEntity.Flag]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterLaunchSites").SetState(displayEntity[AdditionalEntity.LaunchSite]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterObjectives").SetState(displayEntity[AdditionalEntity.Objective]);
		gapRef.Selector.GetSidebarGapButton("buttonFilterKebals").SetCount(additionalIcons[AdditionalEntity.Kerbal].Count + additionalOrbits[AdditionalEntity.Kerbal].Count);
		gapRef.Selector.GetSidebarGapButton("buttonFilterVessels").SetCount(additionalIcons[AdditionalEntity.Vessel].Count + additionalOrbits[AdditionalEntity.Vessel].Count);
		gapRef.Selector.GetSidebarGapButton("buttonFilterAsteroids").SetCount(additionalIcons[AdditionalEntity.Asteroid].Count + additionalOrbits[AdditionalEntity.Asteroid].Count);
		gapRef.Selector.GetSidebarGapButton("buttonFilterFlags").SetCount(additionalIcons[AdditionalEntity.Flag].Count + additionalOrbits[AdditionalEntity.Flag].Count);
		gapRef.Selector.GetSidebarGapButton("buttonFilterLaunchSites").SetCount(additionalIcons[AdditionalEntity.LaunchSite].Count + additionalOrbits[AdditionalEntity.LaunchSite].Count);
		gapRef.Selector.GetSidebarGapButton("buttonFilterObjectives").SetCount(additionalIcons[AdditionalEntity.Objective].Count + additionalOrbits[AdditionalEntity.Objective].Count);
		gapRef.Selector.GetComponent<RectTransform>().SetAsLastSibling();
	}

	public GAPSurfaceIcon CreateSurfaceIcon(AdditionalEntity entityType, string name, MapNode.TypeData typeData, double latitude, double longitude, double altitude)
	{
		GAPSurfaceIcon gAPSurfaceIcon = new GAPSurfaceIcon(name, ref gapRef, gapRef.transform, typeData, latitude, longitude, altitude);
		additionalIcons[entityType].Add(gAPSurfaceIcon);
		return gAPSurfaceIcon;
	}

	public GAPOrbitRenderer CreateSimpleOrbit(AdditionalEntity entityType, string name, Orbit orbit, MapNode.TypeData typeData)
	{
		GAPOrbitRenderer gAPOrbitRenderer = GAPOrbitRenderer.Create(gapRef.gameObject, ScaledCamera.Instance.cam, orbit, isInteractive: false);
		gAPOrbitRenderer.GAPRef = gapRef;
		gAPOrbitRenderer.ObjectName = name;
		gAPOrbitRenderer.TypeData = typeData;
		additionalOrbits[entityType].Add(gAPOrbitRenderer);
		return gAPOrbitRenderer;
	}

	public void CreateSurfaceAreaGizmo(double latitude, double longitude, double radius)
	{
		GAPCelestialBody_SurfaceGizmo_Icon component = UnityEngine.Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/GAP_CelestialBody_SurfaceGizmo_Icon.prefab")).GetComponent<GAPCelestialBody_SurfaceGizmo_Icon>();
		component.Initialize(gapRef);
		component.Radius = radius;
		component.SetGizmoPosition(latitude, longitude, 0.0);
		additionalSurfaceGizmos.Add(component);
	}

	public void ClearAdditionalInfo()
	{
		if (additionalIcons != null && additionalOrbits != null)
		{
			for (int i = 0; i < entityTypes.Length; i++)
			{
				AdditionalEntity key = entityTypes[i];
				if (additionalIcons.ContainsKey(key))
				{
					for (int j = 0; j < additionalIcons[key].Count; j++)
					{
						additionalIcons[key][j].Destroy();
					}
					additionalIcons[key].Clear();
				}
				if (additionalOrbits.ContainsKey(key))
				{
					for (int k = 0; k < additionalOrbits[key].Count; k++)
					{
						additionalOrbits[key][k].Destroy();
					}
					additionalOrbits[key].Clear();
				}
			}
		}
		if (additionalSurfaceGizmos != null)
		{
			for (int l = 0; l < additionalSurfaceGizmos.Count; l++)
			{
				UnityEngine.Object.Destroy(additionalSurfaceGizmos[l].gameObject);
			}
			additionalSurfaceGizmos.Clear();
		}
	}

	public void UpdateAdditionalInfo()
	{
		for (int i = 0; i < additionalIcons.Count; i++)
		{
			if (displayEntity[additionalIcons.KeyAt(i)])
			{
				List<GAPSurfaceIcon> list = additionalIcons.At(i);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].OnUpdate();
				}
			}
		}
	}

	public void OnFilterButton(AdditionalEntity entityType)
	{
		displayEntity[entityType] = !displayEntity[entityType];
		SetFilter(entityType, displayEntity[entityType]);
	}

	public void OnFilterButton_Kerbals()
	{
		OnFilterButton(AdditionalEntity.Kerbal);
	}

	public void OnFilterButton_Asteroids()
	{
		OnFilterButton(AdditionalEntity.Asteroid);
	}

	public void OnFilterButton_Comets()
	{
		OnFilterButton(AdditionalEntity.Asteroid);
	}

	public void OnFilterButton_Vessels()
	{
		OnFilterButton(AdditionalEntity.Vessel);
	}

	public void OnFilterButton_Flags()
	{
		OnFilterButton(AdditionalEntity.Flag);
	}

	public void OnFilterButton_LaunchSites()
	{
		OnFilterButton(AdditionalEntity.LaunchSite);
	}

	public void OnFilterButton_Objectives()
	{
		OnFilterButton(AdditionalEntity.Objective);
		if (additionalSurfaceGizmos != null)
		{
			for (int i = 0; i < additionalSurfaceGizmos.Count; i++)
			{
				additionalSurfaceGizmos[i].gameObject.SetActive(displayEntity[AdditionalEntity.Objective]);
			}
		}
	}

	public void SetFilter(AdditionalEntity entityType, bool displayIcons)
	{
		if (additionalIcons != null && additionalOrbits != null)
		{
			for (int i = 0; i < additionalIcons[entityType].Count; i++)
			{
				additionalIcons[entityType][i].Display(displayIcons);
			}
			for (int j = 0; j < additionalOrbits[entityType].Count; j++)
			{
				additionalOrbits[entityType][j].Display(displayIcons);
			}
		}
	}
}
