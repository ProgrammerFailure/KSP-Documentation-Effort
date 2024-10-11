using UnityEngine;

public interface ITargetable
{
	Transform GetTransform();

	Vector3 GetObtVelocity();

	Vector3 GetSrfVelocity();

	Vector3 GetFwdVector();

	Vessel GetVessel();

	string GetName();

	string GetDisplayName();

	Orbit GetOrbit();

	OrbitDriver GetOrbitDriver();

	VesselTargetModes GetTargetingMode();

	bool GetActiveTargetable();
}
