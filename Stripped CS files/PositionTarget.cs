using UnityEngine;

public class PositionTarget : ITargetable
{
	public GameObject g = new GameObject();

	public string name;

	public string displayName;

	public PositionTarget(string name)
	{
		this.name = name;
	}

	public PositionTarget(string name, string displayName)
	{
		this.name = name;
		this.displayName = displayName;
	}

	public void Update(CelestialBody body, double latitude, double longitude)
	{
		double altitude = body.TerrainAltitude(latitude, longitude);
		Update(body, latitude, longitude, altitude);
	}

	public void Update(CelestialBody body, double latitude, double longitude, double altitude)
	{
		Update(body.GetWorldSurfacePosition(latitude, longitude, altitude));
	}

	public void Update(Vector3d position)
	{
		g.transform.position = position;
	}

	public Vector3 GetFwdVector()
	{
		return Vector3d.up;
	}

	public string GetName()
	{
		return name;
	}

	public string GetDisplayName()
	{
		return displayName;
	}

	public Vector3 GetObtVelocity()
	{
		return Vector3.zero;
	}

	public Orbit GetOrbit()
	{
		return null;
	}

	public OrbitDriver GetOrbitDriver()
	{
		return null;
	}

	public Vector3 GetSrfVelocity()
	{
		return Vector3.zero;
	}

	public Transform GetTransform()
	{
		if (!(g != null))
		{
			return null;
		}
		return g.transform;
	}

	public Vessel GetVessel()
	{
		return null;
	}

	public VesselTargetModes GetTargetingMode()
	{
		return VesselTargetModes.Direction;
	}

	public bool GetActiveTargetable()
	{
		return false;
	}

	~PositionTarget()
	{
		Object.Destroy(g);
	}
}
