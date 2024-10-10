using System;
using Expansions.Missions.Scenery.Scripts;
using ns9;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/City2")]
public class PQSCity2 : PQSSurfaceObject
{
	[Serializable]
	public class LodObject
	{
		public float visibleRange;

		public GameObject[] objects;

		public bool isActive = true;

		public bool KeepActive;

		public float visibleRangeSqr { get; set; }

		public bool IsActive => isActive;

		public virtual void Setup()
		{
			SetRange();
			int num = objects.Length;
			while (num-- > 0)
			{
				objects[num].SetActive(value: false);
			}
			isActive = false;
		}

		public virtual void SetRange()
		{
			visibleRangeSqr = visibleRange * visibleRange;
		}

		public virtual void SetActive(bool active)
		{
			if (KeepActive)
			{
				active = true;
			}
			if (active != isActive)
			{
				int num = objects.Length;
				while (num-- > 0)
				{
					objects[num].SetActive(active);
				}
			}
			isActive = active;
		}
	}

	public Vector3d planetRelativePosition = Vector3d.zero;

	public LodObject[] objects;

	public string objectName = string.Empty;

	public string displayobjectName = string.Empty;

	public double lat;

	public double lon;

	public double alt;

	public Vector3 up = Vector3.up;

	public double rotation;

	public Transform PositioningPoint;

	public bool snapToSurface;

	public bool raycastSurface;

	public bool setOnWaterSurface;

	public double snapHeightOffset;

	public bool baseRotFacesNorthPole;

	public PSystemSetup.SpaceCenterFacility spaceCenterFacility;

	public LaunchSite launchSite;

	[SerializeField]
	public PositionMobileLaunchPad positionMobileLaunchPad;

	public CrashObjectName crashObjectName;

	public bool inPOIRange;

	public double poiRange;

	[SerializeField]
	public bool useIndividualPOIRange;

	[SerializeField]
	public float individualPOIRange = 100f;

	[SerializeField]
	public bool inVisibleRange;

	[SerializeField]
	public bool raycastCompleted;

	[SerializeField]
	public bool positioningCompleted;

	[SerializeField]
	public bool positionedWhenVisible;

	public bool OrientateOnStart = true;

	public bool OrientateOnPostSetup = true;

	public CelestialBody body;

	public override Vector3d PlanetRelativePosition => base.PlanetRelativePosition;

	public override string SurfaceObjectName => objectName;

	public override string DisplaySurfaceObjectName => Localizer.Format(displayobjectName);

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

	public bool IsActive
	{
		get
		{
			bool flag = false;
			for (int i = 0; i < objects.Length; i++)
			{
				flag = flag || objects[i].IsActive;
			}
			return flag;
		}
	}

	public bool InVisibleRange => inVisibleRange;

	public bool PositioningCompleted => positioningCompleted;

	public CelestialBody celestialBody => body;

	public void Awake()
	{
		if (PositioningPoint == null)
		{
			PositioningPoint = base.transform;
		}
		if (positionMobileLaunchPad == null)
		{
			positionMobileLaunchPad = GetComponent<PositionMobileLaunchPad>();
			if (positionMobileLaunchPad == null)
			{
				positionMobileLaunchPad = GetComponentInChildren<PositionMobileLaunchPad>();
			}
		}
	}

	public void SetBody()
	{
		body = base.gameObject.GetComponentInParent<CelestialBody>();
	}

	[ContextMenu("Orientate")]
	public virtual void Orientate()
	{
		if (positioningCompleted && (!positioningCompleted || positionedWhenVisible))
		{
			return;
		}
		setVisibility();
		if (positioningCompleted && !inVisibleRange)
		{
			return;
		}
		bool flag = false;
		Transform target = sphere.target;
		Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
		Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
		Vector3d surfaceNVector = LatLon.GetSurfaceNVector(cf, lat, lon);
		Vector3d vector3d = surfaceNVector * (sphere.radius + alt);
		planetRelativePosition = vector3d;
		base.transform.localPosition = planetRelativePosition;
		if (setOnWaterSurface && OrientateOnWater(surfaceNVector, target))
		{
			return;
		}
		double num = 0.0;
		if (snapToSurface)
		{
			float num2 = Vector3.Distance(base.transform.position, Vector3.zero);
			if (!HighLogic.LoadedSceneIsFlight && num2 > 7000f)
			{
				flag = true;
				FloatingOrigin.SetOffset(sphere.PrecisePosition + base.transform.localPosition);
			}
			num = sphere.GetSurfaceHeight(surfaceNVector, overrideQuadBuildCheck: true);
			num += snapHeightOffset;
			if (raycastSurface && !raycastCompleted && num2 < 7000f)
			{
				num = OrientateRayCast(surfaceNVector, num);
				num += snapHeightOffset;
			}
			alt = num - sphere.radius;
			planetRelativePosition = surfaceNVector * num;
			base.transform.localPosition = planetRelativePosition;
		}
		if (baseRotFacesNorthPole)
		{
			QuaternionD quaternionD = Quaternion.LookRotation(celestialBody.GetRelSurfacePosition(lat, lon + celestialBody.directRotAngle, alt));
			QuaternionD quaternionD2 = Quaternion.Euler(0f, 0f, (float)rotation);
			QuaternionD quaternionD3 = Quaternion.Euler(-90f, -90f, -90f);
			base.transform.rotation = quaternionD * quaternionD2 * quaternionD3;
		}
		else
		{
			base.transform.localRotation = Quaternion.FromToRotation(up, surfaceNVector) * Quaternion.AngleAxis((float)rotation, Vector3.up);
		}
		if (body == null)
		{
			Debug.LogWarningFormat("[PQSCity]: {0} is not parented to a valid CelestialBody.", base.name);
		}
		else
		{
			body.GetLatLonAlt(base.transform.position, out lat, out lon, out alt);
			if (launchSite != null)
			{
				launchSite.SetSpawnPointsLatLonAlt();
			}
			if (spaceCenterFacility != null && spaceCenterFacility.facilityTransform != null)
			{
				spaceCenterFacility.SetSpawnPointsLatLonAlt();
			}
		}
		positioningCompleted = true;
		setVisibility();
		if (inVisibleRange)
		{
			positionedWhenVisible = true;
		}
		if (positionMobileLaunchPad != null && (!snapToSurface || !raycastSurface || (raycastSurface && raycastCompleted)) && positionedWhenVisible)
		{
			positionMobileLaunchPad.CompleteOrientation(body, objectName, overrideVisible: true);
		}
		if (flag && !HighLogic.LoadedSceneIsFlight)
		{
			FloatingOrigin.SetOffset(FloatingOrigin.ReverseOffset);
		}
	}

	public bool OrientateOnWater(Vector3d surfPosNorm, Transform sphereTarget)
	{
		bool flag = false;
		float num = Vector3.Distance(base.transform.position, Vector3.zero);
		if (!HighLogic.LoadedSceneIsFlight && num > 7000f)
		{
			flag = true;
			FloatingOrigin.SetOffset(sphere.PrecisePosition + base.transform.localPosition);
		}
		if (sphere.GetSurfaceHeight(surfPosNorm, overrideQuadBuildCheck: true) - sphere.radius < 0.0)
		{
			float num2 = 0f;
			if (positionMobileLaunchPad != null)
			{
				num2 = positionMobileLaunchPad.waterModelClearance;
			}
			Vector3 vector = celestialBody.GetWorldSurfacePosition(lat, lon, 0.0);
			Vector3d normalized = (vector - sphere.PrecisePosition).normalized;
			base.transform.position = vector;
			if (num2 != 0f)
			{
				Transform obj = base.transform;
				obj.position += normalized * num2;
			}
			if (baseRotFacesNorthPole)
			{
				QuaternionD quaternionD = Quaternion.LookRotation(celestialBody.GetRelSurfacePosition(lat, lon + celestialBody.directRotAngle, alt));
				QuaternionD quaternionD2 = Quaternion.Euler(0f, 0f, (float)rotation);
				QuaternionD quaternionD3 = Quaternion.Euler(-90f, -90f, -90f);
				base.transform.rotation = quaternionD * quaternionD2 * quaternionD3;
			}
			else
			{
				base.transform.localRotation = Quaternion.FromToRotation(up, surfPosNorm) * Quaternion.AngleAxis((float)rotation, Vector3.up);
			}
			positioningCompleted = true;
			raycastSurface = false;
			if (positionMobileLaunchPad != null)
			{
				positionMobileLaunchPad.CompleteOrientation(body, objectName, overrideVisible: true);
			}
			body.GetLatLonAlt(base.transform.position, out lat, out lon, out alt);
			if (launchSite != null)
			{
				launchSite.SetSpawnPointsLatLonAlt();
			}
			if (spaceCenterFacility != null && spaceCenterFacility.facilityTransform != null)
			{
				spaceCenterFacility.SetSpawnPointsLatLonAlt();
			}
			setVisibility();
			if (inVisibleRange)
			{
				positionedWhenVisible = true;
			}
			if (flag && !HighLogic.LoadedSceneIsFlight)
			{
				FloatingOrigin.SetOffset(FloatingOrigin.ReverseOffset);
			}
			GameEvents.OnPQSCityOrientated.Fire(body, objectName);
			return true;
		}
		setOnWaterSurface = false;
		if (flag && !HighLogic.LoadedSceneIsFlight)
		{
			FloatingOrigin.SetOffset(FloatingOrigin.ReverseOffset);
		}
		return false;
	}

	public double OrientateRayCast(Vector3d surfPosNorm, double surfaceHeight)
	{
		double num = 0.0;
		planetRelativePosition = surfPosNorm * surfaceHeight;
		base.transform.localPosition = planetRelativePosition;
		Vector3 position = base.transform.position;
		Vector3d normalized = (PositioningPoint.position - sphere.PrecisePosition).normalized;
		float num2 = 500f;
		Vector3 vector = PositioningPoint.position + normalized * 500.0;
		base.transform.Translate(normalized * 5000.0, Space.World);
		float maxDistance = (float)(vector - sphere.PrecisePosition).magnitude - 500f;
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(vector, -normalized, out hitInfo, maxDistance, 32768))
		{
			num = (double)hitInfo.distance - (double)num2;
			raycastCompleted = true;
		}
		base.transform.position = position;
		if (num != 0.0)
		{
			Debug.Log("[PQSCity2]: Height Correction: " + SurfaceObjectName + " " + num);
		}
		return surfaceHeight + num;
	}

	public virtual void SetInactive()
	{
		int num = objects.Length;
		while (num-- > 0)
		{
			objects[num].SetActive(active: false);
		}
	}

	[ContextMenu("Reset")]
	public virtual void Reset()
	{
		up = Vector3.up;
		positioningCompleted = false;
		raycastCompleted = false;
		positionedWhenVisible = false;
		inVisibleRange = false;
	}

	public virtual void Start()
	{
		body = base.gameObject.GetComponentInParent<CelestialBody>();
		GameEvents.OnScenerySettingChanged.Add(OnScenerySettingChanged);
		if (OrientateOnStart)
		{
			Orientate();
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.OnScenerySettingChanged.Remove(OnScenerySettingChanged);
	}

	public virtual void OnScenerySettingChanged()
	{
		Reset();
		positionedWhenVisible = false;
	}

	public override void OnSetup()
	{
		int num = objects.Length;
		while (num-- > 0)
		{
			objects[num].Setup();
		}
		SetInactive();
		if (raycastSurface && positioningCompleted && !positionedWhenVisible)
		{
			Reset();
			if (positionMobileLaunchPad != null)
			{
				positionMobileLaunchPad.OnScenerySettingChanged();
			}
		}
	}

	public override bool OnSphereStart()
	{
		if (!sphere.isAlive)
		{
			SetInactive();
		}
		return false;
	}

	public override void OnPostSetup()
	{
		if (OrientateOnPostSetup)
		{
			Orientate();
		}
		poiRange = GClass4.GameBindings.GetPOIRange(this);
	}

	public override void OnSphereReset()
	{
		SetInactive();
	}

	public override void OnSphereActive()
	{
		OnUpdateFinished();
	}

	public override void OnSphereInactive()
	{
		SetInactive();
	}

	public override void OnUpdateFinished()
	{
		if (sphere.target == null)
		{
			SetInactive();
			return;
		}
		setVisibility();
		if (inVisibleRange && (!positioningCompleted || !positionedWhenVisible || (raycastSurface && !raycastCompleted)))
		{
			Reset();
			if (positionMobileLaunchPad != null)
			{
				positionMobileLaunchPad.OnScenerySettingChanged();
			}
			Orientate();
		}
		float num = Vector3.Distance(sphere.target.transform.position, base.transform.position);
		if (!inPOIRange && (double)num < POIRange)
		{
			if (base.gameObject != null)
			{
				inPOIRange = true;
				GameEvents.OnPOIRangeEntered.Fire(body, base.gameObject.name);
			}
		}
		else if (inPOIRange && (double)num > POIRange && base.gameObject != null)
		{
			inPOIRange = false;
			GameEvents.OnPOIRangeExited.Fire(body, base.gameObject.name);
		}
	}

	public void setVisibility(bool showMsg = false)
	{
		if (!(sphere.target != null))
		{
			return;
		}
		float num = Vector3.SqrMagnitude(sphere.target.transform.position - base.transform.position);
		int num2 = objects.Length;
		while (num2-- > 0)
		{
			if (objects[num2].visibleRangeSqr == 0f)
			{
				objects[num2].SetRange();
			}
			objects[num2].SetActive(num < objects[num2].visibleRangeSqr);
			inVisibleRange = inVisibleRange || num < objects[num2].visibleRangeSqr;
			if (showMsg)
			{
				Debug.LogFormat("PQSCity2: {0} Range to target: {1} Object {2} Visible Range: {3:N}", objectName, num, (objects[num2].objects.Length >= 1) ? objects[num2].objects[0].name : "Unknown", objects[num2].visibleRange);
			}
		}
	}
}
