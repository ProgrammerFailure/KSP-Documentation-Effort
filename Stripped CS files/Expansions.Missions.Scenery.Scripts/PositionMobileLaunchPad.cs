using System;
using UnityEngine;

namespace Expansions.Missions.Scenery.Scripts;

public class PositionMobileLaunchPad : MonoBehaviour
{
	[Serializable]
	public class ExtensionPoint
	{
		public Transform Pivot;

		public Transform Anchor;

		public Vector3 AnchorOriginalPosition = new Vector3(-1f, -1f, -1f);

		public Transform Stretch;

		public Vector3 StretchOriginalPosition = new Vector3(-1f, -1f, -1f);

		public float initialHeight = -1f;

		public float scaleFactor = 1f;

		public float height;

		public float baseHeight;

		public float anchorHeight;

		public float hitDistance;

		public float distanceMoved;

		public Vector3[] Points;

		public Material material;
	}

	public ExtensionPoint[] ExtensionPoints;

	public Transform[] BottomPoints;

	public float minimumModelClearance = 0.2f;

	public float waterModelClearance = 0.5f;

	public PQSCity2 City;

	public LaunchSite launchSite;

	public Transform RampStart;

	public Transform RampEnd;

	public Transform[] RampObjects;

	[SerializeField]
	public bool positioningComplete;

	public bool hideRampOverMax = true;

	public bool scaleMaterial;

	public Transform[] hideOnWaterSurface;

	public Transform[] showOnWaterSurface;

	[SerializeField]
	public bool waterSurfaceMode;

	public bool PositioningComplete => positioningComplete;

	public void Awake()
	{
		GameEvents.OnScenerySettingChanged.Add(OnScenerySettingChanged);
	}

	public void OnDestroy()
	{
		GameEvents.OnScenerySettingChanged.Remove(OnScenerySettingChanged);
	}

	[ContextMenu("Reset")]
	public void ResetPositioning()
	{
		OnScenerySettingChanged();
		CompleteOrientation(City.celestialBody, City.objectName, overrideVisible: true);
	}

	[ContextMenu("ScenerySettingChange/Reset Legs")]
	public void OnScenerySettingChanged()
	{
		if (!waterSurfaceMode)
		{
			ResetLegs();
		}
		positioningComplete = false;
	}

	public void CompleteOrientation(CelestialBody body, string cityName, bool overrideVisible = false)
	{
		if (City != null && City.objectName == cityName && !positioningComplete && City.sphere.isStarted && City.PositioningCompleted && (City.InVisibleRange || overrideVisible))
		{
			MoveModel();
			bool flag = false;
			if (waterSurfaceMode)
			{
				ToggleObjects(setforGroundMode: false);
			}
			else
			{
				ToggleObjects(setforGroundMode: true);
				CreatePoints();
				flag = ExtendLegs();
			}
			if (hideRampOverMax)
			{
				CheckGrounded(RampStart, RampEnd);
			}
			else
			{
				ToggleRampObjects(setActive: true);
			}
			if (flag || waterSurfaceMode)
			{
				positioningComplete = true;
				CheckForVessels();
			}
		}
	}

	public void CheckForVessels()
	{
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.flightState == null)
		{
			return;
		}
		for (int i = 0; i < HighLogic.CurrentGame.flightState.protoVessels.Count; i++)
		{
			ProtoVessel protoVessel = HighLogic.CurrentGame.flightState.protoVessels[i];
			if (!protoVessel.landed || !(protoVessel.landedAt == City.objectName))
			{
				continue;
			}
			Debug.LogFormat("[PositionMobileLaunchPad]: {0} Vessel: {1} set to Reposition itself as LaunchPad position has changed.", City.objectName, protoVessel.GetDisplayName());
			if (launchSite == null || launchSite.spawnPoints.Length < 1)
			{
				continue;
			}
			if (protoVessel.situation == Vessel.Situations.PRELAUNCH)
			{
				protoVessel.position = launchSite.spawnPoints[0].GetSpawnPointTransform().position;
				protoVessel.latitude = launchSite.spawnPoints[0].latitude;
				protoVessel.longitude = launchSite.spawnPoints[0].longitude;
				protoVessel.altitude = launchSite.spawnPoints[0].altitude;
			}
			protoVessel.PQSminLevel = 0;
			protoVessel.PQSmaxLevel = 0;
			protoVessel.skipGroundPositioning = false;
			if (protoVessel.vesselRef != null)
			{
				protoVessel.vesselRef.PQSminLevel = 0;
				protoVessel.vesselRef.PQSmaxLevel = 0;
				protoVessel.vesselRef.skipGroundPositioning = false;
				if (protoVessel.vesselRef.situation == Vessel.Situations.PRELAUNCH)
				{
					protoVessel.vesselRef.latitude = protoVessel.latitude;
					protoVessel.vesselRef.longitude = protoVessel.longitude;
					protoVessel.vesselRef.altitude = protoVessel.altitude;
					protoVessel.vesselRef.SetPosition(launchSite.spawnPoints[0].GetSpawnPointTransform().position);
				}
			}
		}
	}

	public void ToggleObjects(bool setforGroundMode)
	{
		for (int i = 0; i < hideOnWaterSurface.Length; i++)
		{
			hideOnWaterSurface[i].gameObject.SetActive(setforGroundMode);
		}
		for (int j = 0; j < showOnWaterSurface.Length; j++)
		{
			showOnWaterSurface[j].gameObject.SetActive(!setforGroundMode);
		}
	}

	public void MoveModel()
	{
		if (City.setOnWaterSurface)
		{
			Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
			Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
			Vector3d relSurfaceNVector = City.celestialBody.GetRelSurfaceNVector(City.lat, City.lon);
			if (City.celestialBody.pqsController.GetSurfaceHeight(relSurfaceNVector, overrideQuadBuildCheck: true) - City.celestialBody.Radius < 0.0)
			{
				waterSurfaceMode = true;
				return;
			}
		}
		bool flag = false;
		for (int i = 0; i < BottomPoints.Length; i++)
		{
			Transform transform = BottomPoints[i];
			Vector3 origin = transform.TransformPoint(Vector3.zero) + transform.up * 5000f;
			Vector3 position = City.transform.position;
			City.transform.position += City.transform.up * 7000f;
			if (Physics.Raycast(origin, -transform.up, out var hitInfo, 7000f, 32768))
			{
				float num = 5000f - hitInfo.distance;
				City.transform.position = position;
				if (num > 0f)
				{
					flag = true;
					City.transform.position += City.transform.up * (num + minimumModelClearance);
					Debug.Log(string.Format("[PositionMobileLaunchPad] Moving City {0} {1} {2} so legs are above ground.", City.objectName, (num > 0f) ? "Up" : "Down", num));
				}
				else if (!flag && Mathf.Abs(num) > minimumModelClearance)
				{
					City.transform.position += City.transform.up * (num + minimumModelClearance);
					Debug.Log(string.Format("[PositionMobileLaunchPad] Moving City {0} {1} {2} so legs are above ground.", City.objectName, (num > 0f) ? "Up" : "Down", num));
				}
			}
			else
			{
				City.transform.position = position;
			}
		}
	}

	public void CheckGrounded(Transform objectStart, Transform objectEnd)
	{
		ToggleRampObjects(setActive: true);
		if (!(objectStart != null) || !(objectEnd != null))
		{
			return;
		}
		objectStart.gameObject.SetActive(value: true);
		float num = Vector3.Distance(objectStart.position, objectEnd.position);
		if (Physics.Raycast(objectStart.position, objectStart.forward, out var hitInfo, 1000f, 32768))
		{
			if (hitInfo.distance > num)
			{
				ToggleRampObjects(setActive: false);
			}
		}
		else
		{
			ToggleRampObjects(setActive: false);
		}
	}

	public void ToggleRampObjects(bool setActive)
	{
		for (int i = 0; i < RampObjects.Length; i++)
		{
			RampObjects[i].gameObject.SetActive(setActive);
		}
	}

	public Collider FindCollider(Transform xform)
	{
		Collider collider = xform.GetComponent<Collider>();
		if (!collider)
		{
			foreach (Transform item in xform)
			{
				collider = FindCollider(item);
				if ((bool)collider)
				{
					return collider;
				}
			}
		}
		return collider;
	}

	public Vector3 MakePoint(Transform xform, Vector3 c, float x, float y, float z)
	{
		return new Vector3(c.x + x, c.y + y, c.z + z) - xform.position;
	}

	public void CreatePoints()
	{
		for (int i = 0; i < ExtensionPoints.Length; i++)
		{
			ExtensionPoint extensionPoint = ExtensionPoints[i];
			if ((bool)extensionPoint.Anchor)
			{
				Collider collider = FindCollider(extensionPoint.Anchor);
				if ((bool)collider)
				{
					Vector3 min = collider.bounds.min;
					Vector3 vector = collider.bounds.max - min;
					extensionPoint.anchorHeight = vector.y;
					extensionPoint.Points = new Vector3[4]
					{
						MakePoint(extensionPoint.Anchor, min, 0f, vector.y, 0f),
						MakePoint(extensionPoint.Anchor, min, vector.x, vector.y, 0f),
						MakePoint(extensionPoint.Anchor, min, 0f, vector.y, vector.z),
						MakePoint(extensionPoint.Anchor, min, vector.x, vector.y, vector.z)
					};
					collider.enabled = false;
				}
			}
		}
	}

	public bool ExtendLegs()
	{
		int num = 0;
		while (true)
		{
			if (num < ExtensionPoints.Length)
			{
				ExtensionPoint extensionPoint = ExtensionPoints[num];
				if (extensionPoint.Points == null)
				{
					break;
				}
				float num2 = -1f;
				for (int i = 0; i < extensionPoint.Points.Length; i++)
				{
					if (Physics.Raycast(extensionPoint.Anchor.TransformPoint(extensionPoint.Points[i]), -extensionPoint.Anchor.up, out var hitInfo, 10000f, 32768))
					{
						num2 = ((!(num2 < 0f)) ? Math.Min(num2, hitInfo.distance) : hitInfo.distance);
					}
				}
				if (num2 < 0f)
				{
					Debug.Log($"[PositionMobileLaunchPad] {City.objectName} Didn't hit the ground.");
				}
				extensionPoint.baseHeight = Vector3.Distance(extensionPoint.Anchor.position, extensionPoint.Stretch.position);
				extensionPoint.hitDistance = num2;
				extensionPoint.height = extensionPoint.baseHeight + num2;
				num++;
				continue;
			}
			for (int j = 0; j < ExtensionPoints.Length; j++)
			{
				ExtensionPoint extensionPoint2 = ExtensionPoints[j];
				if (!extensionPoint2.Anchor || !extensionPoint2.Pivot || !extensionPoint2.Stretch)
				{
					continue;
				}
				Collider collider = FindCollider(extensionPoint2.Anchor);
				if ((bool)collider)
				{
					collider.enabled = true;
				}
				if (extensionPoint2.initialHeight != -1f)
				{
					continue;
				}
				extensionPoint2.initialHeight = extensionPoint2.baseHeight;
				extensionPoint2.scaleFactor = Mathf.Max((extensionPoint2.height - extensionPoint2.baseHeight) / extensionPoint2.initialHeight, 1f);
				if (extensionPoint2.AnchorOriginalPosition == new Vector3(-1f, -1f, -1f))
				{
					extensionPoint2.AnchorOriginalPosition = extensionPoint2.Anchor.localPosition;
				}
				if (extensionPoint2.StretchOriginalPosition == new Vector3(-1f, -1f, -1f))
				{
					extensionPoint2.StretchOriginalPosition = extensionPoint2.Stretch.localPosition;
				}
				float num3 = Vector3.Distance(BottomPoints[j].position, extensionPoint2.Anchor.position) / 2f;
				extensionPoint2.distanceMoved = extensionPoint2.hitDistance - num3;
				extensionPoint2.Stretch.localScale = new Vector3(1f, extensionPoint2.scaleFactor, 1f);
				if (scaleMaterial)
				{
					extensionPoint2.material = extensionPoint2.Stretch.GetComponentInChildren<MeshRenderer>().material;
					if (extensionPoint2.material != null)
					{
						extensionPoint2.material.mainTextureScale = new Vector2(1f, extensionPoint2.scaleFactor);
					}
				}
				extensionPoint2.Anchor.position += -extensionPoint2.Anchor.up * extensionPoint2.distanceMoved;
				Debug.LogFormat("[PositionMobileLaunchPad]: {0} {1} InitialHeight:{2} ScaleFactor:{3} DistanceMoved:{4}", City.objectName, extensionPoint2.Pivot.gameObject.name, extensionPoint2.initialHeight, extensionPoint2.scaleFactor, extensionPoint2.distanceMoved);
			}
			return true;
		}
		return false;
	}

	public void ResetLegs()
	{
		for (int i = 0; i < ExtensionPoints.Length; i++)
		{
			ExtensionPoint extensionPoint = ExtensionPoints[i];
			if (!extensionPoint.Anchor || !extensionPoint.Pivot || !extensionPoint.Stretch)
			{
				continue;
			}
			if (scaleMaterial)
			{
				extensionPoint.material = extensionPoint.Stretch.GetComponentInChildren<MeshRenderer>().material;
				if (extensionPoint.material != null)
				{
					extensionPoint.material.mainTextureScale = new Vector2(1f, 1f);
				}
			}
			extensionPoint.Stretch.localScale = new Vector3(1f, 1f, 1f);
			if (extensionPoint.StretchOriginalPosition != new Vector3(-1f, -1f, -1f))
			{
				extensionPoint.Stretch.localPosition = extensionPoint.StretchOriginalPosition;
			}
			if (extensionPoint.AnchorOriginalPosition != new Vector3(-1f, -1f, -1f))
			{
				extensionPoint.Anchor.localPosition = extensionPoint.AnchorOriginalPosition;
			}
			extensionPoint.initialHeight = -1f;
		}
	}
}
