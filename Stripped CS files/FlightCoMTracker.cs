using System.Collections;
using ns9;
using UnityEngine;

public class FlightCoMTracker : MonoBehaviour, ITargetable
{
	public PartModule host;

	public Transform trf;

	public bool activeTargetable;

	public static FlightCoMTracker Create(PartModule host, bool activeTargetable = false)
	{
		FlightCoMTracker flightCoMTracker = new GameObject(host.name + " Vessel CoM Marker").AddComponent<FlightCoMTracker>();
		flightCoMTracker.trf = flightCoMTracker.transform;
		flightCoMTracker.host = host;
		flightCoMTracker.activeTargetable = activeTargetable;
		return flightCoMTracker;
	}

	public void FixedUpdate()
	{
		base.transform.position = host.vessel.CurrentCoM;
	}

	public void MakeTarget()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (FlightGlobals.ready)
			{
				FlightGlobals.fetch.SetVesselTarget(this);
			}
			else
			{
				StartCoroutine(makeTgtWhenReady());
			}
		}
	}

	public IEnumerator makeTgtWhenReady()
	{
		while (!FlightGlobals.ready)
		{
			yield return null;
		}
		FlightGlobals.fetch.SetVesselTarget(this);
	}

	public Transform GetTransform()
	{
		return trf;
	}

	public Vector3 GetObtVelocity()
	{
		return host.vessel.obt_velocity;
	}

	public Vector3 GetSrfVelocity()
	{
		return host.vessel.srf_velocity;
	}

	public Vector3 GetFwdVector()
	{
		return trf.forward;
	}

	public Vessel GetVessel()
	{
		return host.vessel;
	}

	public string GetName()
	{
		return host.vessel.GetDisplayName() + " " + Localizer.Format("#autoLOC_6002484");
	}

	public string GetDisplayName()
	{
		return GetName();
	}

	public Orbit GetOrbit()
	{
		return host.vessel.orbit;
	}

	public OrbitDriver GetOrbitDriver()
	{
		return host.vessel.GetOrbitDriver();
	}

	public VesselTargetModes GetTargetingMode()
	{
		return VesselTargetModes.DirectionAndVelocity;
	}

	public bool GetActiveTargetable()
	{
		return activeTargetable;
	}
}
