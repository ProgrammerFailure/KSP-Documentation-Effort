using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo : MonoBehaviour
{
	public Transform gizmoContainer;

	public CelestialBody celestialBody;

	public double longitude;

	public double latitude;

	public double altitude;

	public GAPCelestialBody gapRef;

	public double Latitude => latitude;

	public double Longitude => longitude;

	public double Altitude => altitude;

	public virtual void Initialize(GAPCelestialBody newGapRef)
	{
		gapRef = newGapRef;
		celestialBody = newGapRef.CelestialBody;
	}

	public virtual void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		celestialBody = newCelestialBody;
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnDestroy()
	{
	}

	public virtual bool IsActive()
	{
		return true;
	}

	public virtual double SetGizmoPosition(double newLatitude, double newLongitude, double newAltitude)
	{
		latitude = newLatitude;
		longitude = newLongitude;
		Vector3 vector = celestialBody.GetRelSurfacePosition(newLatitude, newLongitude, newAltitude);
		double num = celestialBody.Radius;
		if (celestialBody.pqsController != null)
		{
			num = celestialBody.pqsController.GetSurfaceHeight(vector);
		}
		double result = num - celestialBody.Radius;
		double num2 = celestialBody.TerrainAltitude(latitude, longitude, allowNegative: true);
		if (num2 < 0.0)
		{
			SetInSurfaceFromPointAtSeaLevel(vector, Math.Abs(num2));
			return result;
		}
		SetInSurfaceFromPoint(vector);
		return result;
	}

	public void SetInSurfaceFromPoint(Vector3d point)
	{
		QuaternionD quaternionD = Quaternion.LookRotation(point);
		Vector3d vector3d = quaternionD * Vector3d.forward;
		double num = celestialBody.Radius;
		if (celestialBody.pqsController != null)
		{
			num = celestialBody.pqsController.GetSurfaceHeight(vector3d);
		}
		vector3d *= num;
		base.transform.rotation = quaternionD;
		base.transform.position = vector3d;
	}

	public void SetInSurfaceFromPointAtSeaLevel(Vector3d point, double seaLevel)
	{
		QuaternionD quaternionD = Quaternion.LookRotation(point);
		Vector3d vector3d = quaternionD * Vector3d.forward;
		double num = celestialBody.Radius;
		if (celestialBody.pqsController != null)
		{
			num = celestialBody.pqsController.GetSurfaceHeight(vector3d);
		}
		num += seaLevel;
		vector3d *= num;
		base.transform.rotation = quaternionD;
		base.transform.position = vector3d;
	}
}
