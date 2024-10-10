using System;
using System.Collections.Generic;
using Expansions.Missions;
using Expansions.Missions.Actions;
using ns23;
using ns9;
using UnityEngine;
using Vectrosity;

public class OrbitRendererBase : MonoBehaviour
{
	public enum DrawMode
	{
		const_0,
		REDRAW_ONLY,
		REDRAW_AND_FOLLOW,
		REDRAW_AND_RECALCULATE
	}

	public enum DrawIcons
	{
		NONE,
		const_1,
		OBJ_PE_AP,
		const_3
	}

	public struct OrbitCastHit : IScreenCaster
	{
		public Vector3 orbitOrigin;

		public Vector3 hitPoint;

		public Vector3 orbitPoint;

		public Vector3 orbitScreenPoint;

		public double mouseTA;

		public double radiusAtTA;

		public double UTatTA;

		public OrbitRendererBase or;

		public OrbitDriver driver;

		public Vector3 GetUpdatedOrbitPoint()
		{
			if (driver.updateMode == OrbitDriver.UpdateMode.IDLE)
			{
				if ((bool)or.vessel)
				{
					return ScaledSpace.LocalToScaledSpace(or.vessel.GetWorldPos3D());
				}
				if ((bool)or.celestialBody)
				{
					return ScaledSpace.LocalToScaledSpace(or.celestialBody.position);
				}
				return ScaledSpace.LocalToScaledSpace(or.transform.position);
			}
			return ScaledSpace.LocalToScaledSpace(or.orbit.getPositionFromTrueAnomaly(mouseTA));
		}

		public Vector3 GetScreenSpacePoint()
		{
			return PlanetariumCamera.Camera.WorldToScreenPoint(GetUpdatedOrbitPoint());
		}
	}

	public OrbitDriver driver;

	public bool nodesAttached;

	public Vessel vessel;

	public CelestialBody celestialBody;

	public MENode meNode;

	public DiscoveryInfo discoveryInfo;

	public static double sampleResolution = 2.0;

	public Color orbitColor = Color.grey;

	public Color nodeColor = Color.grey;

	public float lowerCamVsSmaRatio = 0.03f;

	public float upperCamVsSmaRatio = 25f;

	public bool drawNodes = true;

	public bool isFocused;

	public bool autoTextureOffset = true;

	public float textureOffset;

	public DrawMode drawMode = DrawMode.REDRAW_AND_RECALCULATE;

	public DrawIcons drawIcons = DrawIcons.const_1;

	public double st;

	public double end;

	public double rng;

	public double itv;

	public Vector3d[] orbitPoints;

	public float CamVsSmaRatio;

	public float lineOpacity;

	public VectorLine orbitLine;

	public bool draw3dLines;

	public MapObject objectMO;

	public MapObject DescMO;

	public MapObject AscMO;

	public MapObject ApMO;

	public MapObject PeMO;

	public ActionCreateVessel cachedCreateVessel;

	public ActionCreateAsteroid cachedCreateAsteroid;

	public ActionCreateComet cachedCreateComet;

	public ActionCreateKerbal cachedCreateKerbal;

	public MapNode objectNode;

	public MapNode descNode;

	public MapNode ascNode;

	public MapNode apNode;

	public MapNode peNode;

	public Transform nodesParent;

	public int layerMask = 31;

	public EventData<Vessel> onVesselIconClicked = new EventData<Vessel>("OnVesselIconClicked");

	public EventData<CelestialBody> onCelestialBodyIconClicked = new EventData<CelestialBody>("OnCBIconClicked");

	public double eccOffset;

	public float twkOffset;

	public bool EccOffsetInvert;

	public static string cacheAutoLOC_196762;

	public static string cacheAutoLOC_196878;

	public static string cacheAutoLOC_196888;

	public static string cacheAutoLOC_196893;

	public static string cacheAutoLOC_7001411;

	public bool isActive
	{
		get
		{
			if (orbitLine != null)
			{
				return orbitLine.active;
			}
			return false;
		}
	}

	public bool mouseOver
	{
		get
		{
			if (objectNode != null)
			{
				return objectNode.Hover;
			}
			return false;
		}
	}

	public string objName { get; set; }

	public Vector3d[] OrbitPoints
	{
		get
		{
			return orbitPoints;
		}
		set
		{
			orbitPoints = value;
		}
	}

	public VectorLine OrbitLine => orbitLine;

	public Orbit orbit => driver.orbit;

	public virtual void Start()
	{
		if (!(ScaledSpace.Instance == null))
		{
			orbitColor = (nodeColor * 0.5f).smethod_0(nodeColor.a);
			lineOpacity = 1f;
			objName = base.name;
			discoveryInfo = new DiscoveryInfo(null, DiscoveryLevels.Owned, double.PositiveInfinity);
			orbitPoints = new Vector3d[360 / (int)sampleResolution];
			MakeLine(ref orbitLine);
			FindMapObject();
			CreateScaledSpaceNodes();
			AttachNodeUIs(nodesParent);
			if (vessel != null)
			{
				DrawOrbit(DrawMode.REDRAW_AND_RECALCULATE);
				return;
			}
			UpdateSpline();
			DrawOrbit(DrawMode.REDRAW_ONLY);
		}
	}

	public virtual void OnDestroy()
	{
		DetachNodeUIs();
		DestroyScaledSpaceNodes();
		VectorLine.Destroy(ref orbitLine);
	}

	public virtual void LateUpdate()
	{
		DrawOrbit((driver.updateMode != OrbitDriver.UpdateMode.IDLE) ? drawMode : DrawMode.const_0);
		DrawNodes();
	}

	public void SetColor(Color color)
	{
		nodeColor = color;
		orbitColor = (nodeColor * 0.5f).smethod_0(nodeColor.a);
	}

	public void MakeLine(ref VectorLine l)
	{
		if (l != null)
		{
			VectorLine.Destroy(ref l);
		}
		l = new VectorLine(base.name + " orbit", new List<Vector3>(GetSegmentCount(sampleResolution)), 5f, LineType.Continuous);
		l.texture = MapView.OrbitLinesMaterial.mainTexture;
		l.material = MapView.OrbitLinesMaterial;
		l.material.SetFloat("_FadeStrength", GameSettings.ORBIT_FADE_STRENGTH);
		l.material.SetFloat("_FadeSign", GameSettings.ORBIT_FADE_DIRECTION_INV ? (-1f) : 1f);
		l.continuousTexture = true;
		l.color = GetOrbitColour();
		l.rectTransform.gameObject.layer = layerMask;
		l.UpdateImmediate = true;
	}

	public virtual void UpdateSpline()
	{
		double semiMinorAxis = orbit.semiMinorAxis;
		if (orbit.eccentricity < 1.0)
		{
			int num = (int)Math.Floor(360.0 / sampleResolution);
			double num2 = sampleResolution * (Math.PI / 180.0);
			for (int i = 0; i < num; i++)
			{
				orbitPoints[i] = orbit.getPositionFromEccAnomalyWithSemiMinorAxis((double)i * num2, semiMinorAxis);
			}
		}
		else
		{
			st = 0.0 - Math.Acos(0.0 - 1.0 / orbit.eccentricity);
			end = Math.Acos(0.0 - 1.0 / orbit.eccentricity);
			rng = end - st;
			itv = rng / (double)(orbitPoints.Length - 1);
			int num3 = orbitPoints.Length;
			for (int i = 0; i < num3; i++)
			{
				orbitPoints[i] = orbit.getPositionFromEccAnomalyWithSemiMinorAxis(st + itv * (double)i, semiMinorAxis);
			}
		}
		ScaledSpace.LocalToScaledSpace(orbitPoints, orbitLine.points3);
		if (orbit.eccentricity < 1.0)
		{
			orbitLine.points3[orbitLine.points3.Count - 1] = orbitLine.points3[0];
			orbitLine.drawEnd = orbitLine.points3.Count - 1;
		}
		else
		{
			orbitLine.drawEnd = orbitLine.points3.Count - 2;
		}
	}

	public virtual void DrawOrbit(DrawMode mode)
	{
		if (OrbitLine == null)
		{
			return;
		}
		SplineOpacityUpdate();
		if (lineOpacity != 0f && mode != 0)
		{
			OrbitLine.SetColor(GetOrbitColour());
			switch (mode)
			{
			case DrawMode.REDRAW_ONLY:
				DrawSpline();
				break;
			case DrawMode.const_0:
				break;
			case DrawMode.REDRAW_AND_FOLLOW:
				DrawSpline();
				break;
			case DrawMode.REDRAW_AND_RECALCULATE:
				OrbitLine.rectTransform.position = Vector3.zero;
				UpdateSpline();
				DrawSpline();
				break;
			}
		}
	}

	public virtual Color GetNodeColour()
	{
		return nodeColor.smethod_0(lineOpacity);
	}

	public virtual Color GetOrbitColour()
	{
		return orbitColor.smethod_0(lineOpacity);
	}

	public void SplineOpacityUpdate()
	{
		if (vessel != null && vessel == FlightGlobals.ActiveVessel)
		{
			lineOpacity = 1f;
		}
		else if (!isFocused)
		{
			MapObject target = PlanetariumCamera.fetch.target;
			CelestialBody tgt = target.celestialBody ?? target.orbit.referenceBody;
			if (IsRenderableOrbit(orbit, tgt))
			{
				if (vessel != null && vessel.LandedOrSplashed)
				{
					CamVsSmaRatio = (float)getCamVsSmaRatio(orbit.referenceBody.Radius);
				}
				else
				{
					CamVsSmaRatio = (float)getCamVsSmaRatio(orbit.semiMajorAxis);
				}
			}
			else
			{
				CamVsSmaRatio = 0f;
			}
			if (CamVsSmaRatio > lowerCamVsSmaRatio && CamVsSmaRatio < upperCamVsSmaRatio && lineOpacity < 1f)
			{
				lineOpacity = Mathf.Min(1f, lineOpacity += 0.25f * Time.deltaTime);
			}
			else if (lineOpacity > 0f)
			{
				lineOpacity = Mathf.Max(0f, lineOpacity -= 0.25f * Time.deltaTime);
			}
		}
		else
		{
			lineOpacity = 1f;
		}
	}

	public bool IsRenderableOrbit(Orbit o, CelestialBody tgt)
	{
		if (!(o.referenceBody == Planetarium.fetch.Sun) && o != tgt.orbit)
		{
			return OrbitUtil.FindCommonAncestor(o, tgt) != Planetarium.fetch.Sun;
		}
		return true;
	}

	public double getCamVsSmaRatio(double double_0)
	{
		double num = (double)MapView.MapCamera.Distance / (Math.Abs(double_0) * (double)ScaledSpace.InverseScaleFactor);
		if (double.IsNaN(num))
		{
			num = 0.0;
		}
		return num;
	}

	public virtual void DrawSpline()
	{
		if (autoTextureOffset)
		{
			if (orbit.eccentricity < 1.0)
			{
				double num = 0.01745329238474369 - UtilMath.TwoPI;
				eccOffset = (orbit.eccentricAnomaly - num) % UtilMath.TwoPI / UtilMath.TwoPI;
				twkOffset = (float)eccOffset * MapView.GetEccOffset((float)eccOffset, (float)orbit.eccentricity, 4f);
				textureOffset = 1f - twkOffset;
			}
			else
			{
				textureOffset = 0f;
			}
		}
		orbitLine.ContinuousTextureOffset = textureOffset;
		if (MapView.Draw3DLines != draw3dLines)
		{
			draw3dLines = MapView.Draw3DLines;
			MakeLine(ref orbitLine);
			UpdateSpline();
		}
		if (draw3dLines)
		{
			orbitLine.Draw3D();
		}
		else
		{
			orbitLine.Draw();
		}
	}

	public int GetSegmentCount(double sampleResolution)
	{
		return 360 / (int)sampleResolution + 1;
	}

	public virtual void RefreshMapObject()
	{
		if (vessel != null && vessel.vesselType == VesselType.DeployedSciencePart)
		{
			if (objectNode != null)
			{
				objectNode.Terminate();
				objectNode = null;
			}
			objectMO = null;
		}
		FindMapObject();
		CreateScaledSpaceNodes();
		AttachNodeUIs(nodesParent);
	}

	public void FindMapObject()
	{
		if (vessel != null && vessel.vesselType != VesselType.DeployedSciencePart)
		{
			objectMO = vessel.mapObject;
		}
		else if (celestialBody != null)
		{
			objectMO = celestialBody.MapObject;
		}
		else
		{
			if (!(meNode != null))
			{
				return;
			}
			objectMO = meNode.mapObject;
			if (!meNode.IsVesselNode)
			{
				return;
			}
			for (int i = 0; i < meNode.actionModules.Count; i++)
			{
				if (meNode.actionModules[i] is ActionCreateVessel)
				{
					cachedCreateVessel = meNode.actionModules[i] as ActionCreateVessel;
				}
				if (meNode.actionModules[i] is ActionCreateAsteroid)
				{
					cachedCreateAsteroid = meNode.actionModules[i] as ActionCreateAsteroid;
				}
				if (meNode.actionModules[i] is ActionCreateComet)
				{
					cachedCreateComet = meNode.actionModules[i] as ActionCreateComet;
				}
				if (meNode.actionModules[i] is ActionCreateKerbal)
				{
					cachedCreateKerbal = meNode.actionModules[i] as ActionCreateKerbal;
				}
			}
		}
	}

	public void CreateScaledSpaceNodes()
	{
		if (DescMO == null)
		{
			DescMO = MapObject.Create(base.name + "'s Descending Node", Localizer.Format("#autoLOC_196698", base.name), driver.orbit, MapObject.ObjectType.DescendingNode);
			DescMO.vesselRef = vessel;
		}
		if (AscMO == null)
		{
			AscMO = MapObject.Create(base.name + "'s Ascending Node", Localizer.Format("#autoLOC_196699", base.name), driver.orbit, MapObject.ObjectType.AscendingNode);
			AscMO.vesselRef = vessel;
		}
		if (ApMO == null)
		{
			ApMO = MapObject.Create(base.name + "'s Apoapsis", Localizer.Format("#autoLOC_196700", base.name), driver.orbit, MapObject.ObjectType.Apoapsis);
			ApMO.vesselRef = vessel;
		}
		if (PeMO == null)
		{
			PeMO = MapObject.Create(base.name + "'s Periapsis", Localizer.Format("#autoLOC_196701", base.name), driver.orbit, MapObject.ObjectType.Periapsis);
			PeMO.vesselRef = vessel;
		}
	}

	public virtual void AttachNodeUIs(Transform parent)
	{
		Color nodeColour = GetNodeColour();
		if (parent == null)
		{
			parent = MapViewCanvasUtil.NodeContainer;
		}
		bool pinnable = HighLogic.LoadedScene != GameScenes.TRACKSTATION;
		if (objectMO != null && objectNode == null)
		{
			objectNode = MapNode.Create(objectMO, nodeColour, 24, hoverable: true, pinnable, blocksInput: true, parent);
			objectNode.OnUpdateVisible += objectNode_OnUpdateIcon;
			objectNode.OnUpdatePosition += objectNode_OnUpdatePosition;
			objectNode.OnUpdateType += objectNode_OnUpdateType;
			objectNode.OnUpdateCaption += objectNode_OnUpdateCaption;
			objectNode.OnClick += objectNode_OnClick;
		}
		if (descNode == null)
		{
			descNode = MapNode.Create(DescMO, nodeColour, 32, hoverable: true, pinnable, blocksInput: false, parent);
			descNode.OnUpdateVisible += ANDNNodes_OnUpdateIcon;
			descNode.OnUpdatePosition += descNode_OnUpdatePosition;
			descNode.OnUpdateCaption += descNode_OnUpdateCaption;
		}
		if (ascNode == null)
		{
			ascNode = MapNode.Create(AscMO, nodeColour, 32, hoverable: true, pinnable, blocksInput: false, parent);
			ascNode.OnUpdateVisible += ANDNNodes_OnUpdateIcon;
			ascNode.OnUpdatePosition += ascNode_OnUpdatePosition;
			ascNode.OnUpdateCaption += ascNode_OnUpdateCaption;
		}
		if (apNode == null)
		{
			apNode = MapNode.Create(ApMO, nodeColour, 32, hoverable: true, pinnable, blocksInput: true, parent);
			apNode.OnUpdateVisible += ApNode_OnUpdateIcon;
			apNode.OnUpdatePosition += apNode_OnUpdatePosition;
			apNode.OnUpdateCaption += apNode_OnUpdateCaption;
		}
		if (peNode == null)
		{
			peNode = MapNode.Create(PeMO, nodeColour, 32, hoverable: true, pinnable, blocksInput: true, parent);
			peNode.OnUpdateVisible += PeNode_OnUpdateIcon;
			peNode.OnUpdatePosition += peNode_OnUpdatePosition;
			peNode.OnUpdateCaption += peNode_OnUpdateCaption;
		}
		nodesAttached = true;
	}

	public virtual void objectNode_OnClick(MapNode mn, Mouse.Buttons btns)
	{
		if (vessel != null)
		{
			if (HighLogic.LoadedSceneIsFlight && Mouse.Left.GetDoubleClick() && InputLockManager.IsUnlocked(ControlTypes.VESSEL_SWITCHING) && InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
			{
				if (HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar)
				{
					if (FlightGlobals.SetActiveVessel(vessel))
					{
						FlightInputHandler.SetNeutralControls();
					}
				}
				else
				{
					ScreenMessages.PostScreenMessage(cacheAutoLOC_196762, 5f, ScreenMessageStyle.UPPER_CENTER);
				}
			}
			onVesselIconClicked.Fire(vessel);
		}
		if (celestialBody != null)
		{
			onCelestialBodyIconClicked.Fire(celestialBody);
		}
	}

	public void DetachNodeUIs()
	{
		if (objectNode != null)
		{
			objectNode.Terminate();
		}
		if (descNode != null)
		{
			descNode.Terminate();
		}
		if (ascNode != null)
		{
			ascNode.Terminate();
		}
		if (apNode != null)
		{
			apNode.Terminate();
		}
		if (peNode != null)
		{
			peNode.Terminate();
		}
		nodesAttached = false;
	}

	public void DestroyScaledSpaceNodes()
	{
		if (DescMO != null)
		{
			DescMO.Terminate();
		}
		if (AscMO != null)
		{
			AscMO.Terminate();
		}
		if (ApMO != null)
		{
			ApMO.Terminate();
		}
		if (PeMO != null)
		{
			PeMO.Terminate();
		}
	}

	public void DrawNodes()
	{
		if (nodesAttached)
		{
			if (objectNode != null)
			{
				objectNode.NodeUpdate();
			}
			if (ascNode != null)
			{
				ascNode.NodeUpdate();
			}
			if (descNode != null)
			{
				descNode.NodeUpdate();
			}
			if (apNode != null)
			{
				apNode.NodeUpdate();
			}
			if (peNode != null)
			{
				peNode.NodeUpdate();
			}
		}
	}

	public virtual void objectNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		data.color = nodeColor.smethod_0(lineOpacity);
		data.visible = CanDrawAnyIcons() && GetCurrentDrawMode() >= DrawIcons.const_1;
	}

	public virtual void objectNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		string empty = string.Empty;
		string captionLine = string.Empty;
		string text = string.Empty;
		if (!HaveStateVectorKnowledge())
		{
			empty = Localizer.Format("#autoLOC_196868", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - discoveryInfo.lastObservedTime, 3, explicitPositive: false));
			captionLine = discoveryInfo.size.Caption + DiscoveryInfo.GetSizeClassSizes(discoveryInfo.objectSize);
		}
		else if ((bool)vessel)
		{
			switch (vessel.situation)
			{
			case Vessel.Situations.LANDED:
				if (vessel.vesselType == VesselType.Flag)
				{
					empty = KSPUtil.PrintCoordinates(vessel.latitude, ResourceUtilities.clampLon(vessel.longitude), singleLine: false);
					captionLine = Localizer.Format("#autoLOC_196884", KSPUtil.PrintDate(vessel.launchTime, includeTime: true));
				}
				else
				{
					empty = ((!(vessel.landedAt == string.Empty)) ? ((!(vessel.displaylandedAt == string.Empty)) ? Localizer.Format("#autoLOC_6100012", Localizer.Format(vessel.displaylandedAt)) : Localizer.Format("#autoLOC_6100012", ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(Vessel.GetLandedAtString(vessel.landedAt), formatted: true))) : (cacheAutoLOC_196888 + KSPUtil.PrintCoordinates(vessel.latitude, ResourceUtilities.clampLon(vessel.longitude), singleLine: false)));
				}
				break;
			case Vessel.Situations.SPLASHED:
				empty = cacheAutoLOC_196893;
				captionLine = KSPUtil.PrintCoordinates(vessel.latitude, ResourceUtilities.clampLon(vessel.longitude), singleLine: false);
				break;
			default:
				empty = Localizer.Format("#autoLOC_196899", orbit.vel.magnitude.ToString("0.0"));
				captionLine = Localizer.Format("#autoLOC_196900", orbit.altitude.ToString("N0"));
				if (FlightGlobals.ActiveVessel != null && FlightGlobals.fetch.VesselTarget != null && FlightGlobals.fetch.VesselTarget.GetVessel() == vessel)
				{
					text += Localizer.Format("#autoLOC_196906", (base.transform.position - FlightGlobals.ActiveVessel.transform.position).magnitude.ToString("N1"));
				}
				break;
			case Vessel.Situations.PRELAUNCH:
				empty = cacheAutoLOC_196878;
				break;
			}
		}
		else if ((bool)celestialBody)
		{
			empty = Localizer.Format("#autoLOC_196914", orbit.vel.magnitude.ToString("0.0"));
			captionLine = Localizer.Format("#autoLOC_196915", orbit.altitude.ToString("N0"));
		}
		else if (meNode != null)
		{
			empty = meNode.Title;
			if (meNode.IsVesselNode)
			{
				if (cachedCreateVessel != null)
				{
					empty = empty + " " + cachedCreateVessel.vesselSituation.vesselName;
					switch (cachedCreateVessel.vesselSituation.location.situation)
					{
					case MissionSituation.VesselStartSituations.ORBITING:
						captionLine = Localizer.Format("#autoLOC_8002005", cachedCreateVessel.vesselSituation.location.orbitSnapShot.Body.displayName);
						break;
					case MissionSituation.VesselStartSituations.PRELAUNCH:
						captionLine = Localizer.Format("#autoLOC_8002003", PSystemSetup.Instance.GetLaunchSiteDisplayName(cachedCreateVessel.vesselSituation.location.launchSite));
						break;
					case MissionSituation.VesselStartSituations.LANDED:
						captionLine = Localizer.Format("#autoLOC_8002004", cachedCreateVessel.vesselSituation.location.vesselGroundLocation.targetBody.displayName);
						break;
					}
				}
				else if (cachedCreateAsteroid != null)
				{
					empty = empty + " " + cachedCreateAsteroid.asteroid.name;
					switch (cachedCreateAsteroid.location.locationChoice)
					{
					case ParamChoices_VesselSimpleLocation.Choices.orbit:
						captionLine = Localizer.Format("#autoLOC_8002005", cachedCreateAsteroid.location.orbit.Body.displayName);
						break;
					case ParamChoices_VesselSimpleLocation.Choices.landed:
						captionLine = Localizer.Format("#autoLOC_8002004", cachedCreateAsteroid.location.landed.targetBody.displayName);
						break;
					}
				}
				else if (cachedCreateComet != null)
				{
					empty = empty + " " + cachedCreateComet.comet.name;
					switch (cachedCreateComet.location.locationChoice)
					{
					case ParamChoices_VesselSimpleLocation.Choices.orbit:
						captionLine = Localizer.Format("#autoLOC_8002005", cachedCreateComet.location.orbit.Body.displayName);
						break;
					case ParamChoices_VesselSimpleLocation.Choices.landed:
						captionLine = Localizer.Format("#autoLOC_8002004", cachedCreateComet.location.landed.targetBody.displayName);
						break;
					}
				}
				else if (cachedCreateKerbal != null)
				{
					empty = empty + " " + cachedCreateKerbal.missionKerbal.Kerbal.name;
					switch (cachedCreateKerbal.location.locationChoice)
					{
					case ParamChoices_VesselSimpleLocation.Choices.orbit:
						captionLine = Localizer.Format("#autoLOC_8002005", cachedCreateKerbal.location.orbit.Body.displayName);
						break;
					case ParamChoices_VesselSimpleLocation.Choices.landed:
						captionLine = Localizer.Format("#autoLOC_8002004", cachedCreateKerbal.location.landed.targetBody.displayName);
						break;
					}
				}
			}
		}
		else
		{
			empty = Localizer.Format("#autoLOC_196919", orbit.vel.magnitude.ToString("0.0"));
			captionLine = Localizer.Format("#autoLOC_196920", orbit.altitude.ToString("N0"));
		}
		data.Header = Localizer.Format("#autoLOC_7001301", discoveryInfo.displayName.Value);
		data.captionLine1 = empty;
		data.captionLine2 = captionLine;
		data.captionLine3 = text;
	}

	public virtual Vector3d objectNode_OnUpdatePosition(MapNode n)
	{
		if ((bool)vessel)
		{
			return ScaledSpace.LocalToScaledSpace(vessel.GetWorldPos3D());
		}
		if ((bool)celestialBody)
		{
			return ScaledSpace.LocalToScaledSpace(celestialBody.position);
		}
		if (meNode != null)
		{
			return ScaledSpace.LocalToScaledSpace(meNode.GetNodeLocationInWorld());
		}
		return ScaledSpace.LocalToScaledSpace(base.transform.position);
	}

	public virtual void objectNode_OnUpdateType(MapNode n, MapNode.TypeData data)
	{
		if (vessel != null)
		{
			data.oType = MapObject.ObjectType.Vessel;
			if (vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.Appearance))
			{
				data.vType = vessel.vesselType;
			}
			else
			{
				data.vType = VesselType.Unknown;
			}
		}
		else if (celestialBody != null)
		{
			data.oType = MapObject.ObjectType.CelestialBody;
		}
		else if (meNode != null)
		{
			data.oType = MapObject.ObjectType.MENode;
		}
		else
		{
			data.oType = MapObject.ObjectType.Generic;
		}
	}

	public virtual Vector3d ascNode_OnUpdatePosition(MapNode n)
	{
		return ScaledSpace.LocalToScaledSpace(orbit.getPositionFromTrueAnomaly((360.0 - orbit.argumentOfPeriapsis) * 0.01745329238474369));
	}

	public virtual Vector3d descNode_OnUpdatePosition(MapNode n)
	{
		return ScaledSpace.LocalToScaledSpace(orbit.getPositionFromTrueAnomaly((360.0 - orbit.argumentOfPeriapsis + 180.0) * 0.01745329238474369));
	}

	public virtual void ascNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_6002405", orbit.double_0.ToString("0"));
	}

	public virtual void descNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_6002406", ((orbit.double_0 + 180.0) % 360.0).ToString("0"));
	}

	public virtual void ANDNNodes_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		data.color = GetNodeColour();
		data.visible = CanDrawAnyIcons() && HaveStateVectorKnowledge() && GetCurrentDrawMode() == DrawIcons.const_3;
	}

	public virtual Vector3d apNode_OnUpdatePosition(MapNode n)
	{
		return ScaledSpace.LocalToScaledSpace(orbit.getPositionAtT(orbit.period * 0.5));
	}

	public virtual void apNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_6002409", orbit.ApA.ToString("N0"), cacheAutoLOC_7001411);
	}

	public virtual void ApNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		data.color = GetNodeColour();
		data.visible = CanDrawAnyIcons() && HaveStateVectorKnowledge() && GetCurrentDrawMode() >= DrawIcons.OBJ_PE_AP && orbit.eccentricity < 1.0;
	}

	public virtual Vector3d peNode_OnUpdatePosition(MapNode n)
	{
		if (orbit.eccentricity < 1.0)
		{
			return ScaledSpace.LocalToScaledSpace(orbit.getPositionAtT(0.0));
		}
		return ScaledSpace.LocalToScaledSpace(driver.referenceBody.position + (Vector3)(orbit.eccVec.xzy.normalized * (0.0 - orbit.semiMajorAxis) * (orbit.eccentricity - 1.0)));
	}

	public virtual void PeNode_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		data.color = GetNodeColour();
		data.visible = CanDrawAnyIcons() && HaveStateVectorKnowledge() && GetCurrentDrawMode() >= DrawIcons.OBJ_PE_AP && orbit.PeR > driver.referenceBody.Radius;
	}

	public virtual void peNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_6002408", orbit.PeA.ToString("N0"), cacheAutoLOC_7001411);
	}

	public virtual bool CanDrawAnyIcons()
	{
		if (drawNodes && drawIcons != 0)
		{
			if (!MapViewFiltering.CheckAgainstFilter(vessel))
			{
				return false;
			}
			if (discoveryInfo.Level == DiscoveryLevels.None)
			{
				return false;
			}
			if (lineOpacity == 0f)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public virtual DrawIcons GetCurrentDrawMode()
	{
		if ((bool)vessel && vessel.LandedOrSplashed)
		{
			return DrawIcons.const_1;
		}
		return drawIcons;
	}

	public virtual bool HaveStateVectorKnowledge()
	{
		return discoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors);
	}

	public virtual bool OrbitCast(Vector3 screenPos, out OrbitCastHit hitInfo, float orbitPixelWidth = 10f)
	{
		return OrbitCast(screenPos, PlanetariumCamera.Camera, out hitInfo, orbitPixelWidth);
	}

	public virtual bool OrbitCast(Vector3 screenPos, Camera cam, out OrbitCastHit hitInfo, float orbitPixelWidth = 10f)
	{
		hitInfo = default(OrbitCastHit);
		hitInfo.or = this;
		hitInfo.driver = driver;
		if (orbitLine.active && lineOpacity >= 0.1f)
		{
			hitInfo.orbitOrigin = ScaledSpace.LocalToScaledSpace(orbit.referenceBody.position);
			Plane plane = new Plane(orbit.GetOrbitNormal().xzy, hitInfo.orbitOrigin);
			Debug.DrawRay(hitInfo.orbitOrigin, plane.normal * 1000f, Color.cyan);
			Ray ray = cam.ScreenPointToRay(screenPos);
			plane.Raycast(ray, out var enter);
			hitInfo.hitPoint = ray.origin + ray.direction * enter - hitInfo.orbitOrigin;
			Vector3 vector = QuaternionD.Inverse(QuaternionD.LookRotation(-orbit.GetEccVector().xzy, orbit.GetOrbitNormal().xzy)) * hitInfo.hitPoint;
			hitInfo.mouseTA = Mathf.Atan2(vector.x, 0f - vector.z);
			hitInfo.radiusAtTA = orbit.RadiusAtTrueAnomaly(hitInfo.mouseTA) * (double)ScaledSpace.InverseScaleFactor;
			hitInfo.orbitPoint = hitInfo.hitPoint.normalized * (float)hitInfo.radiusAtTA + hitInfo.orbitOrigin;
			hitInfo.UTatTA = orbit.GetUTforTrueAnomaly(hitInfo.mouseTA, 0.0);
			hitInfo.orbitScreenPoint = cam.WorldToScreenPoint(hitInfo.orbitPoint);
			hitInfo.orbitScreenPoint = new Vector3(hitInfo.orbitScreenPoint.x, hitInfo.orbitScreenPoint.y, 0f);
			if (Vector3.Distance(hitInfo.orbitScreenPoint, screenPos) < orbitPixelWidth)
			{
				Debug.DrawLine(hitInfo.orbitOrigin, hitInfo.orbitPoint, Color.green);
				return true;
			}
			Debug.DrawRay(hitInfo.orbitOrigin, hitInfo.hitPoint, Color.yellow);
			return false;
		}
		return false;
	}

	public bool GetMouseOverNode(Vector3 worldPos, float iconSize)
	{
		return mouseOver;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_196762 = Localizer.Format("#autoLOC_196762");
		cacheAutoLOC_196878 = Localizer.Format("#autoLOC_196878");
		cacheAutoLOC_196888 = Localizer.Format("#autoLOC_196888") + "\n";
		cacheAutoLOC_196893 = Localizer.Format("#autoLOC_196893");
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
	}
}
