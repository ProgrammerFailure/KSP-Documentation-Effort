using System;
using System.Collections.Generic;
using Contracts;
using FinePrint.Utilities;
using ns1;
using ns23;
using ns9;
using UnityEngine;

namespace FinePrint;

public class Waypoint
{
	public int seed;

	public int index;

	public string id = "default";

	public string name = "Site";

	public string celestialName;

	public double latitude;

	public double longitude;

	public double altitude;

	public double height;

	public double radius;

	public bool isOccluded;

	public bool isExplored;

	public bool isClustered;

	public bool isOnSurface;

	public bool isNavigatable;

	public bool isCustom;

	public bool visible = true;

	public bool landLocked;

	public bool enableMarker = true;

	public bool enableTooltip = true;

	public bool blocksInput = true;

	public bool isMission;

	public Contract contractReference;

	public Vector3d worldPosition;

	public Vector3 orbitPosition;

	public Vector3d surfacePosition;

	public Vector3d cameraPosition;

	public CelestialBody focusBody;

	public CelestialBody cachedBody;

	public int iconSize = 16;

	public const float minimumAlpha = 0.6f;

	public const float maximumAlpha = 1f;

	public float alphaRatio = -1f;

	public float iconAlpha;

	public double fadeRange = double.MaxValue;

	public bool withinZoom;

	public bool mapOpen;

	public MapNode node;

	public MapContextMenu menu;

	public string nodeCaption1;

	public string nodeCaption2;

	public string nodeCaption3;

	public Guid navigationId = Guid.Empty;

	public CelestialBody celestialBody
	{
		get
		{
			if (cachedBody != null && cachedBody.GetName() == celestialName)
			{
				return cachedBody;
			}
			cachedBody = ((FlightGlobals.Bodies.Count > 0) ? FlightGlobals.Bodies[0] : null);
			int count = FlightGlobals.Bodies.Count;
			while (count-- > 0)
			{
				CelestialBody celestialBody = FlightGlobals.Bodies[count];
				if (celestialBody.GetName() == celestialName)
				{
					cachedBody = celestialBody;
				}
			}
			return cachedBody;
		}
	}

	public int uniqueSeed => (index + seed) * (index + seed + 1) / 2 + seed;

	public string FullName => name + (isClustered ? (" " + StringUtilities.IntegerToGreek(index)) : "");

	public Waypoint()
	{
		navigationId = Guid.NewGuid();
	}

	public Waypoint(Waypoint that)
	{
		seed = that.seed;
		index = that.index;
		id = that.id;
		name = that.name;
		celestialName = that.celestialName;
		latitude = that.latitude;
		longitude = that.longitude;
		altitude = that.altitude;
		height = that.height;
		worldPosition = that.worldPosition;
		orbitPosition = that.orbitPosition;
		isOccluded = that.isOccluded;
		isExplored = that.isExplored;
		isClustered = that.isClustered;
		isOnSurface = that.isOnSurface;
		isNavigatable = that.isNavigatable;
		visible = that.visible;
		landLocked = that.landLocked;
		contractReference = that.contractReference;
		enableMarker = that.enableMarker;
		enableTooltip = that.enableTooltip;
		iconSize = that.iconSize;
		alphaRatio = that.alphaRatio;
		iconAlpha = that.iconAlpha;
		fadeRange = that.fadeRange;
		navigationId = that.navigationId;
		if (navigationId == Guid.Empty)
		{
			navigationId = Guid.NewGuid();
		}
	}

	public void setName(bool uniqueSites = true, bool allowNamed = true)
	{
		if (celestialBody != null)
		{
			string text = StringUtilities.GenerateSiteName(uniqueSites ? uniqueSeed : seed, celestialBody, landLocked, allowNamed);
			name = Localizer.Format("#autoLOC_7001301", text);
		}
	}

	public void SetFadeRange(double referenceAltitude = -1.0)
	{
		if (!(celestialBody == null))
		{
			if (referenceAltitude < 0.0)
			{
				fadeRange = (isOnSurface ? (celestialBody.Radius * (double)ScaledSpace.InverseScaleFactor * 20.0) : (celestialBody.sphereOfInfluence * (double)ScaledSpace.InverseScaleFactor * 10.0));
			}
			else
			{
				fadeRange = referenceAltitude * (double)ScaledSpace.InverseScaleFactor * 20.0;
			}
		}
	}

	public void RandomizeNear(double centerLatitude, double centerLongitude, double minimumDistance, double maximumDistance, bool waterAllowed = true, System.Random generator = null)
	{
		if (celestialBody == null)
		{
			return;
		}
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom(uniqueSeed));
		if (!celestialBody.hasSolidSurface)
		{
			waterAllowed = true;
		}
		if (celestialBody.ocean && !waterAllowed)
		{
			landLocked = true;
		}
		int num = 10000;
		while (num >= 0)
		{
			latitude = Math.PI / 2.0 - centerLatitude * (Math.PI / 180.0);
			longitude = centerLongitude * (Math.PI / 180.0);
			double num2 = minimumDistance + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (maximumDistance - minimumDistance);
			double num3 = Math.PI / 2.0 - num2 / celestialBody.Radius;
			double num4 = Math.PI * 2.0 * (kSPRandom?.NextDouble() ?? generator.NextDouble());
			Vector3d vector3d = new Vector3d(Math.Cos(num3) * Math.Sin(num4), Math.Cos(num3) * Math.Cos(num4), Math.Sin(num3));
			Vector3d vector3d2 = new Vector3d(vector3d.x, vector3d.y * Math.Cos(latitude) + vector3d.z * Math.Sin(latitude), (0.0 - vector3d.y) * Math.Sin(latitude) + vector3d.z * Math.Cos(latitude));
			Vector3d vector3d3 = new Vector3d(vector3d2.x * Math.Cos(longitude) + vector3d2.y * Math.Sin(longitude), (0.0 - vector3d2.x) * Math.Sin(longitude) + vector3d2.y * Math.Cos(longitude), vector3d2.z);
			latitude = Math.Asin(vector3d3.z) * (180.0 / Math.PI);
			longitude = Math.Atan2(vector3d3.x, vector3d3.y) * (180.0 / Math.PI);
			if (!(!celestialBody.ocean || waterAllowed) && !(CelestialUtilities.TerrainAltitude(celestialBody, latitude, longitude, underwater: true) >= 0.0))
			{
				maximumDistance *= 1.05;
				num--;
				continue;
			}
			break;
		}
	}

	public void RandomizeNear(double centerLatitude, double centerLongitude, double maximumDistance, bool waterAllowed = true, System.Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom(uniqueSeed));
		RandomizeNear(centerLatitude, centerLongitude, 0.0, maximumDistance, waterAllowed, (kSPRandom != null) ? kSPRandom : generator);
	}

	public void RandomizeAwayFrom(double originLatitude, double originLongitude, double minimumDistance, double maximumDistance, int samples = 3, bool waterAllowed = true, System.Random generator = null)
	{
		if (celestialBody == null)
		{
			return;
		}
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom(uniqueSeed));
		double centerLatitude = latitude;
		double centerLongitude = longitude;
		List<KeyValuePair<double, double>> list = new List<KeyValuePair<double, double>>();
		for (int num = samples; num > 0; num--)
		{
			RandomizeNear(centerLatitude, centerLongitude, minimumDistance, maximumDistance, waterAllowed, (kSPRandom != null) ? kSPRandom : generator);
			list.Add(new KeyValuePair<double, double>(latitude, longitude));
		}
		KeyValuePair<double, double> keyValuePair = new KeyValuePair<double, double>(originLatitude, originLongitude);
		double num2 = 0.0;
		int count = list.Count;
		while (count-- > 0)
		{
			KeyValuePair<double, double> keyValuePair2 = list[count];
			if (CelestialUtilities.GreatCircleDistance(celestialBody, keyValuePair2.Key, keyValuePair2.Value, originLatitude, originLongitude) > num2)
			{
				keyValuePair = keyValuePair2;
				num2 = CelestialUtilities.GreatCircleDistance(celestialBody, keyValuePair.Key, keyValuePair.Value, originLatitude, originLongitude);
			}
		}
		latitude = keyValuePair.Key;
		longitude = keyValuePair.Value;
	}

	public void RandomizeAwayFrom(double originLatitude, double originLongitude, double maximumDistance, int samples = 3, bool waterAllowed = true, System.Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom(uniqueSeed));
		RandomizeAwayFrom(originLatitude, originLongitude, 0.0, maximumDistance, samples, waterAllowed, (kSPRandom != null) ? kSPRandom : generator);
	}

	public void SetupMapNode()
	{
		Sprite sprite = ContractDefs.sprites[id];
		Color color = SystemUtilities.RandomColor(seed, 1f, 1f, 1f);
		int width = sprite.texture.width;
		node = MapNode.Create(name, color, width, hoverable: true, pinnable: false, blocksInput);
		node.OnUpdateVisible += OnUpdateNodeIcon;
		node.OnUpdatePosition += OnUpdateNodePosition;
		node.OnUpdateCaption += OnUpdateNodeCaption;
		node.SetIcon(sprite, WaypointManager.WaypointMaterial);
		if (enableMarker)
		{
			node.SetBackground(ContractDefs.sprites["marker"], WaypointManager.WaypointMaterial);
		}
	}

	public void CleanupMapNode()
	{
		if (node != null)
		{
			node.Terminate();
		}
	}

	public void UpdateWaypoint(bool mapOpen, bool clicked, CelestialBody focusBody, Vector3d cameraPosition)
	{
		this.mapOpen = mapOpen;
		this.focusBody = focusBody;
		this.cameraPosition = cameraPosition;
		if (mapOpen)
		{
			CachePositions();
			CheckOcclusion();
			CheckZoom();
			CheckAlpha();
		}
		if (clicked)
		{
			ProcessClick();
		}
		if (node != null)
		{
			node.NodeUpdate();
		}
	}

	public void CachePositions()
	{
		if (isOnSurface)
		{
			if (celestialBody != null)
			{
				height = CelestialUtilities.TerrainAltitude(celestialBody, latitude, longitude);
				surfacePosition = celestialBody.GetWorldSurfacePosition(latitude, longitude, height + altitude);
				Vector3d vector3d = ScaledSpace.LocalToScaledSpace(surfacePosition);
				worldPosition = new Vector3((float)vector3d.x, (float)vector3d.y, (float)vector3d.z);
			}
			else
			{
				height = 0.0;
				surfacePosition = Vector3d.zero;
				worldPosition = Vector3.zero;
			}
		}
		else
		{
			worldPosition = ScaledSpace.LocalToScaledSpace(orbitPosition);
		}
	}

	public void CheckOcclusion()
	{
		if (isOnSurface && Vector3d.Angle(cameraPosition - surfacePosition, celestialBody.position - surfacePosition) <= 90.0)
		{
			isOccluded = true;
		}
		else
		{
			isOccluded = false;
		}
	}

	public void CheckZoom()
	{
		double num = ((MapView.MapCamera == null || focusBody == null) ? double.MaxValue : (Vector3d.Distance(cameraPosition, focusBody.position) * (double)ScaledSpace.InverseScaleFactor));
		withinZoom = num <= fadeRange;
	}

	public void CheckAlpha()
	{
		if (alphaRatio == -1f)
		{
			alphaRatio = ((!isOccluded && withinZoom) ? 1 : 0);
		}
		alphaRatio = ((isOccluded || !withinZoom) ? Mathf.Clamp(alphaRatio - Time.deltaTime * 4f, 0f, 1f) : Mathf.Clamp(alphaRatio + Time.deltaTime * 4f, 0f, 1f));
		float b = (withinZoom ? (0.6f + 0.39999998f * alphaRatio) : (1f * alphaRatio));
		iconAlpha = Mathf.Lerp(iconAlpha, b, Time.deltaTime * 8f);
		iconAlpha = Mathf.Clamp(iconAlpha, 0f, 1f);
	}

	public void OnUpdateNodeIcon(MapNode n, MapNode.IconData data)
	{
		data.visible = true;
		if (mapOpen && visible && iconAlpha > 0f)
		{
			if (!(focusBody == null) && !(focusBody.GetName() != celestialName))
			{
				if (MapView.MapCamera.transform.InverseTransformPoint(worldPosition).z < 0f)
				{
					data.visible = false;
					return;
				}
				data.pixelSize = 16;
				data.color = SystemUtilities.RandomColor(seed, iconAlpha, 1f, 1f);
				data.bgColor = XKCDColors.White.smethod_0(alphaRatio);
				data.offset = new Vector3(0f, enableMarker ? 21f : 0f, 0f);
				data.bgSize = new Vector2(48f, 48f);
				data.bgOffset = new Vector3(0f, 12.5f, 0f);
			}
			else
			{
				data.visible = false;
			}
		}
		else
		{
			data.visible = false;
		}
	}

	public Vector3d OnUpdateNodePosition(MapNode n)
	{
		return worldPosition;
	}

	public void OnUpdateNodeCaption(MapNode n, MapNode.CaptionData data)
	{
		if (mapOpen && enableTooltip && withinZoom && !isOccluded)
		{
			string text = "<color=";
			if (contractReference == null)
			{
				text = ((!isCustom) ? (text + XKCDColors.HexFormat.KSPNeutralUIGrey + ">") : (text + XKCDColors.HexFormat.LightCyan + ">"));
			}
			else
			{
				text = text + ((contractReference.ContractState != Contract.State.Active) ? XKCDColors.HexFormat.Amber : XKCDColors.HexFormat.Chartreuse) + ">";
				data.captionLine1 = text + Localizer.Format("#autoLOC_7003010", contractReference.Agent.Title) + "</color>";
			}
			data.Header = text + (isOnSurface ? Localizer.Format("#autoLOC_7003011", FullName) : FullName) + "</color>";
			if (!string.IsNullOrEmpty(nodeCaption1))
			{
				data.captionLine1 = text + nodeCaption1 + "</color>";
			}
			if (!string.IsNullOrEmpty(nodeCaption2))
			{
				data.captionLine2 = text + nodeCaption2 + "</color>";
			}
			if (!string.IsNullOrEmpty(nodeCaption3))
			{
				data.captionLine3 = text + nodeCaption3 + "</color>";
			}
		}
		else
		{
			data.Header = null;
			data.captionLine1 = null;
			data.captionLine2 = null;
			data.captionLine3 = null;
		}
	}

	public void ProcessClick()
	{
		if (MapView.MapIsEnabled && withinZoom && !isOccluded && node != null && node.Hover)
		{
			if (menu == null)
			{
				List<MapContextMenuOption> list = new List<MapContextMenuOption>();
				if (HighLogic.LoadedSceneIsFlight && isNavigatable)
				{
					list.Add(new WaypointNavigation(this));
				}
				if (isCustom)
				{
					list.Add(new DeleteCustomWaypoint(this));
				}
				if (list.Count > 0)
				{
					float num = 28f + 36f * (float)list.Count;
					menu = MapContextMenu.Create(FullName, new Rect(0.5f, 0.5f, 300f, num), new WaypointCastHit(this), OnMenuDismissed, list.ToArray());
				}
			}
			else
			{
				menu.Dismiss();
			}
		}
		else if (menu != null && !menu.Hover)
		{
			menu.Dismiss();
		}
	}

	public void OnMenuDismissed()
	{
		menu = null;
	}
}
