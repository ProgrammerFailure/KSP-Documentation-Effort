using System;
using System.Collections;
using UnityEngine;

public class OrbitPhysicsManager : MonoBehaviour
{
	public static OrbitPhysicsManager fetch;

	public CelestialBody dominantBody;

	[Obsolete("Use Vessel.distancePackThreshold instead")]
	public float distantPartPackThreshold = 2250f;

	[Obsolete("Use Vessel.distanceUnpackThreshold instead")]
	public float distantPartUnpackThreshold = 2000f;

	[Obsolete("Use Vessel.distanceLandedPackThreshold instead")]
	public float distantLandedPartPackThreshold = 350f;

	[Obsolete("Use Vessel.distanceLandedUnpackThreshold instead")]
	public float distantLandedPartUnpackThreshold = 200f;

	public bool toggleDominantBodyRotation;

	public bool degub;

	public bool holdVesselUnpack;

	public int releaseUnpackIn;

	public bool started;

	public static CelestialBody DominantBody
	{
		get
		{
			if (!fetch)
			{
				return null;
			}
			return fetch.dominantBody;
		}
	}

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public IEnumerator Start()
	{
		while (!FlightGlobals.ready)
		{
			yield return new WaitForSeconds(0.5f);
		}
		dominantBody = FlightGlobals.currentMainBody;
		started = true;
	}

	public static void HoldVesselUnpack(int releaseAfter = 1)
	{
		if ((bool)fetch)
		{
			fetch.holdVesselUnpack = true;
			fetch.releaseUnpackIn = releaseAfter;
		}
	}

	public void FixedUpdate()
	{
		if (!FlightGlobals.ready || !started || FlightGlobals.overrideOrbit)
		{
			return;
		}
		if (!degub)
		{
			checkReferenceFrame();
		}
		if (holdVesselUnpack)
		{
			if (releaseUnpackIn > 0)
			{
				releaseUnpackIn--;
			}
			else
			{
				holdVesselUnpack = false;
			}
		}
	}

	public void LateUpdate()
	{
		if (!FlightGlobals.ready || !started || FlightGlobals.overrideOrbit)
		{
			return;
		}
		bool flag = TimeWarp.WarpMode == TimeWarp.Modes.HIGH && TimeWarp.CurrentRate > TimeWarp.MaxPhysicsRate;
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!FlightGlobals.warpDriveActive)
		{
			if (flag)
			{
				if (!activeVessel.packed)
				{
					activeVessel.GoOnRails();
				}
			}
			else if (activeVessel.packed && !holdVesselUnpack)
			{
				activeVessel.GoOffRails();
			}
		}
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			if (vessel == activeVessel || vessel == null || vessel.orbitDriver == null)
			{
				continue;
			}
			if (!FlightGlobals.warpDriveActive && flag && !vessel.packed)
			{
				vessel.GoOnRails();
			}
			float num = Vector3.Distance(vessel.transform.position, activeVessel.transform.position);
			if (num > vessel.vesselRanges.GetSituationRanges(vessel.BestSituation).pack)
			{
				if (!vessel.packed)
				{
					vessel.GoOnRails();
				}
			}
			else if (num < vessel.vesselRanges.GetSituationRanges(vessel.situation).unpack && vessel.packed && TimeWarp.CurrentRate <= TimeWarp.MaxPhysicsRate && !holdVesselUnpack)
			{
				vessel.GoOffRails();
			}
		}
	}

	public void setDominantBody(CelestialBody body)
	{
		dominantBody = body;
		Vector3[] array = new Vector3[FlightGlobals.physicalObjects.Count];
		MonoBehaviour.print("setting new dominant body: " + dominantBody.bodyName + "\nFlightGlobals.mainBody: " + FlightGlobals.currentMainBody.bodyName);
		int count = Planetarium.Orbits.Count;
		for (int i = 0; i < count; i++)
		{
			Planetarium.Orbits[i].reverse = false;
		}
		if ((bool)dominantBody.orbitDriver)
		{
			SetReverseOrbit(dominantBody.orbitDriver);
		}
		FlightLogger.IgnoreGeeForces(0.5f);
		int count2 = FlightGlobals.physicalObjects.Count;
		if (count2 > 0)
		{
			for (int j = 0; j < count2; j++)
			{
				if (!(FlightGlobals.physicalObjects[j] == null) && !(FlightGlobals.physicalObjects[j].rb == null))
				{
					array[j] = FlightGlobals.physicalObjects[j].rb.velocity - (FlightGlobals.ActiveVessel.orbit.GetVel() - Krakensbane.GetFrameVelocity());
				}
			}
		}
		Vector3d frameVelocity = Krakensbane.GetFrameVelocity();
		for (int k = 0; k < FlightGlobals.VesselsLoaded.Count; k++)
		{
			Vessel vessel = FlightGlobals.VesselsLoaded[k];
			if (vessel.state == Vessel.State.DEAD || vessel.packed)
			{
				continue;
			}
			vessel.IgnoreGForces(20);
			Vector3d vector3d = vessel.orbit.GetVel() - frameVelocity;
			for (int l = 0; l < vessel.parts.Count; l++)
			{
				Part part = vessel.parts[l];
				if (part.State != PartStates.DEAD && !part.packed && !(part.rb == null))
				{
					part.rb.velocity = vector3d;
					if (part.servoRb != null)
					{
						part.servoRb.velocity = vector3d;
					}
				}
			}
			Debug.Log("Vessel " + vessel.name + " velocity resumed. Reference body: " + vessel.orbit.referenceBody.name + " vel: " + vector3d.ToString(), vessel.gameObject);
		}
		if (count2 > 0)
		{
			for (int m = 0; m < count2; m++)
			{
				if (!(FlightGlobals.physicalObjects[m] == null) && !(FlightGlobals.physicalObjects[m].rb == null))
				{
					FlightGlobals.physicalObjects[m].rb.velocity = FlightGlobals.ActiveVessel.orbit.GetVel() - Krakensbane.GetFrameVelocity() + array[m];
				}
			}
		}
		GameEvents.onDominantBodyChange.Fire(new GameEvents.FromToAction<CelestialBody, CelestialBody>(dominantBody, body));
	}

	public static void SetDominantBody(CelestialBody body)
	{
		if ((bool)fetch)
		{
			fetch.setDominantBody(body);
		}
	}

	public void SetReverseOrbit(OrbitDriver orbit)
	{
		orbit.reverse = true;
		if (orbit.referenceBody.orbitDriver != null)
		{
			SetReverseOrbit(orbit.referenceBody.orbitDriver);
		}
	}

	public void setRotatingFrame(bool rotatingFrameState)
	{
		FlightLogger.IgnoreGeeForces(0.5f);
		dominantBody.inverseRotation = rotatingFrameState;
		Debug.Log("Reference Frame: " + (rotatingFrameState ? "Rotating" : "Inertial"));
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			if (vessel.state == Vessel.State.DEAD || vessel.packed)
			{
				continue;
			}
			Vector3d rFrmVel = dominantBody.getRFrmVel(vessel.transform.position);
			for (int j = 0; j < vessel.parts.Count; j++)
			{
				Part part = vessel.parts[j];
				if (part.State == PartStates.DEAD || part.packed)
				{
					continue;
				}
				Rigidbody rb = part.rb;
				if (rb == null)
				{
					continue;
				}
				if (rotatingFrameState)
				{
					rb.velocity = (Vector3d)rb.velocity - rFrmVel;
					if (part.servoRb != null)
					{
						part.servoRb.velocity = (Vector3d)part.servoRb.velocity - rFrmVel;
					}
				}
				else
				{
					rb.velocity = (Vector3d)rb.velocity + rFrmVel;
					if (part.servoRb != null)
					{
						part.servoRb.velocity = (Vector3d)part.servoRb.velocity + rFrmVel;
					}
				}
			}
			if (rotatingFrameState)
			{
				vessel.krakensbaneAcc -= rFrmVel;
			}
			else
			{
				vessel.krakensbaneAcc += rFrmVel;
			}
		}
		for (int k = 0; k < FlightGlobals.physicalObjects.Count; k++)
		{
			physicalObject physicalObject2 = FlightGlobals.physicalObjects[k];
			if (!(physicalObject2 == null) && !(physicalObject2.rb == null))
			{
				if (rotatingFrameState)
				{
					physicalObject2.rb.velocity = physicalObject2.rb.velocity - dominantBody.getRFrmVel(physicalObject2.transform.position);
				}
				else
				{
					physicalObject2.rb.velocity = physicalObject2.rb.velocity + dominantBody.getRFrmVel(physicalObject2.transform.position);
				}
			}
		}
		GameEvents.onRotatingFrameTransition.Fire(new GameEvents.HostTargetAction<CelestialBody, bool>(dominantBody, rotatingFrameState));
	}

	[ContextMenu("Toggle Rotating Frame (Force Debug)")]
	public void ToggleRotatingFrame()
	{
		if (!degub)
		{
			degub = true;
		}
		setRotatingFrame(!dominantBody.inverseRotation);
	}

	public static void SetRotatingFrame(bool rotating)
	{
		if ((bool)fetch)
		{
			fetch.setRotatingFrame(rotating);
		}
	}

	public void checkReferenceFrame()
	{
		if (dominantBody != FlightGlobals.ActiveVessel.orbit.referenceBody)
		{
			setDominantBody(FlightGlobals.ActiveVessel.orbit.referenceBody);
		}
		if (!fetch.dominantBody.rotates)
		{
			return;
		}
		double num = FlightGlobals.getAltitudeAtPos(FlightGlobals.ActiveVessel.transform.position, dominantBody);
		if (num < (double)dominantBody.inverseRotThresholdAltitude)
		{
			if (!dominantBody.inverseRotation)
			{
				setRotatingFrame(rotatingFrameState: true);
			}
		}
		else if (num > (double)dominantBody.inverseRotThresholdAltitude && dominantBody.inverseRotation)
		{
			setRotatingFrame(rotatingFrameState: false);
		}
	}

	public static void CheckReferenceFrame()
	{
		if ((bool)fetch)
		{
			fetch.checkReferenceFrame();
		}
	}
}
