using System;
using UnityEngine;

public class OrbitDriver : MonoBehaviour
{
	public enum UpdateMode
	{
		TRACK_Phys,
		UPDATE,
		IDLE
	}

	public delegate void CelestialBodyDelegate(CelestialBody body);

	public Vector3d pos;

	public Vector3d vel;

	public bool isHyperbolic;

	public Orbit orbit = new Orbit();

	public bool drawOrbit;

	public bool reverse;

	public bool frameShift;

	public bool QueuedUpdate;

	public bool QueueOnce;

	public UpdateMode updateMode;

	public OrbitRendererBase Renderer;

	public bool ready;

	public Vessel vessel;

	public CelestialBody celestialBody;

	public Color orbitColor = Color.grey;

	public float lowerCamVsSmaRatio = 0.03f;

	public float upperCamVsSmaRatio = 25f;

	public Transform driverTransform;

	public CelestialBodyDelegate OnReferenceBodyChange;

	public double fdtLast = 0.02;

	public double lastTrackUT;

	public UpdateMode lastMode;

	public double updateUT;

	public bool Ready => ready;

	public CelestialBody referenceBody
	{
		get
		{
			return orbit.referenceBody;
		}
		set
		{
			orbit.referenceBody = value;
		}
	}

	public ITargetable Targetable
	{
		get
		{
			if (vessel != null)
			{
				return vessel;
			}
			if (celestialBody != null)
			{
				return celestialBody;
			}
			return null;
		}
	}

	public void Awake()
	{
		driverTransform = base.transform;
		vessel = GetComponent<Vessel>();
		celestialBody = GetComponent<CelestialBody>();
	}

	public void Start()
	{
		switch (updateMode)
		{
		case UpdateMode.UPDATE:
			orbit.Init();
			updateFromParameters();
			break;
		case UpdateMode.TRACK_Phys:
		case UpdateMode.IDLE:
			if (!referenceBody)
			{
				referenceBody = FlightGlobals.getMainBody(driverTransform.position);
			}
			TrackRigidbody(referenceBody, 0.0 - fdtLast);
			break;
		}
		ready = true;
		Planetarium.Orbits.Add(this);
		if (OnReferenceBodyChange != null)
		{
			OnReferenceBodyChange(referenceBody);
		}
	}

	public void OnDestroy()
	{
		if (Planetarium.Orbits != null)
		{
			Planetarium.Orbits.Remove(this);
			if ((bool)Renderer)
			{
				UnityEngine.Object.Destroy(Renderer);
			}
		}
	}

	public void FixedUpdate()
	{
		if (!QueuedUpdate)
		{
			if (QueueOnce)
			{
				QueueOnce = false;
			}
			else
			{
				UpdateOrbit();
			}
		}
	}

	public void UpdateOrbit(bool offset = true)
	{
		if (!ready)
		{
			return;
		}
		lastMode = updateMode;
		switch (updateMode)
		{
		case UpdateMode.UPDATE:
			updateFromParameters();
			if (vessel != null)
			{
				CheckDominantBody(referenceBody.position + pos);
			}
			break;
		case UpdateMode.TRACK_Phys:
		case UpdateMode.IDLE:
			if (!(vessel == null) && !(vessel.rootPart == null) && !(vessel.rootPart.rb == null))
			{
				if (!offset)
				{
					fdtLast = -0.0;
				}
				if (!CheckDominantBody(vessel.CoMD))
				{
					TrackRigidbody(referenceBody, 0.0 - fdtLast);
				}
			}
			break;
		}
		fdtLast = TimeWarp.fixedDeltaTime;
		if (isHyperbolic && orbit.eccentricity < 1.0)
		{
			isHyperbolic = false;
			if (vessel != null)
			{
				GameEvents.onVesselOrbitClosed.Fire(vessel);
			}
		}
		if (!isHyperbolic && orbit.eccentricity > 1.0)
		{
			isHyperbolic = true;
			if (vessel != null)
			{
				GameEvents.onVesselOrbitEscaped.Fire(vessel);
			}
		}
		if (drawOrbit)
		{
			orbit.DrawOrbit();
		}
	}

	public void SetOrbitMode(UpdateMode mode)
	{
		updateMode = mode;
		fdtLast = TimeWarp.fixedDeltaTime;
	}

	public bool CheckDominantBody(Vector3d refPos)
	{
		CelestialBody mainBody = FlightGlobals.getMainBody(refPos);
		if (referenceBody != mainBody && !FlightGlobals.overrideOrbit)
		{
			RecalculateOrbit(mainBody);
			return true;
		}
		return false;
	}

	public void TrackRigidbody(CelestialBody refBody, double fdtOffset)
	{
		updateUT = Planetarium.GetUniversalTime();
		if (vessel != null)
		{
			pos = (vessel.CoMD - referenceBody.position).xzy;
		}
		if (vessel != null && vessel.rootPart != null && vessel.rootPart.rb != null && !vessel.rootPart.rb.isKinematic)
		{
			updateUT += fdtOffset;
			vel = vessel.velocityD.xzy + orbit.GetRotFrameVelAtPos(referenceBody, pos);
		}
		else if (updateMode == UpdateMode.IDLE)
		{
			vel = orbit.GetRotFrameVel(referenceBody);
		}
		if (refBody != referenceBody)
		{
			if (vessel != null)
			{
				pos = (vessel.CoMD - refBody.position).xzy;
			}
			vel += referenceBody.GetFrameVel() - refBody.GetFrameVel();
		}
		lastTrackUT = updateUT;
		orbit.UpdateFromStateVectors(pos, vel, refBody, updateUT);
		pos.Swizzle();
		vel.Swizzle();
	}

	public void updateFromParameters()
	{
		updateFromParameters(setPosition: true);
	}

	public void updateFromParameters(bool setPosition)
	{
		updateUT = Planetarium.GetUniversalTime();
		orbit.UpdateFromUT(updateUT);
		pos = orbit.pos;
		vel = orbit.vel;
		pos.Swizzle();
		vel.Swizzle();
		if (double.IsNaN(pos.x))
		{
			MonoBehaviour.print("ObT : " + orbit.ObT + "\nM : " + orbit.meanAnomaly + "\nE : " + orbit.eccentricAnomaly + "\nV : " + orbit.trueAnomaly + "\nRadius: " + orbit.radius + "\nvel: " + vel.ToString() + "\nAN: " + orbit.an.ToString() + "\nperiod: " + orbit.period + "\n");
			if ((bool)vessel)
			{
				Debug.LogWarning("[OrbitDriver Warning!]: " + vessel.GetDisplayName() + " had a NaN Orbit and was removed.");
				vessel.Unload();
				UnityEngine.Object.Destroy(vessel.gameObject);
				return;
			}
		}
		if (!setPosition)
		{
			return;
		}
		if (!reverse)
		{
			if ((bool)vessel)
			{
				Vector3d vector3d = (QuaternionD)driverTransform.rotation * (Vector3d)vessel.localCoM;
				vessel.SetPosition(referenceBody.position + pos - vector3d);
			}
			else if ((bool)celestialBody)
			{
				celestialBody.position = referenceBody.position + pos;
			}
			else
			{
				driverTransform.position = referenceBody.position + pos;
			}
		}
		else
		{
			referenceBody.position = (celestialBody ? celestialBody.position : ((Vector3d)driverTransform.position)) - pos;
		}
	}

	public void RecalculateOrbit(CelestialBody newReferenceBody)
	{
		if (frameShift)
		{
			return;
		}
		frameShift = true;
		CelestialBody celestialBody = referenceBody;
		if (updateMode == UpdateMode.UPDATE && Time.timeScale > 0f)
		{
			OnRailsSOITransition(orbit, newReferenceBody);
		}
		else
		{
			Debug.Log(string.Concat("recalculating orbit for ", base.name, ": ", referenceBody.bodyName, " ( Update mode ", updateMode, " )\nrPos: ", pos.ToString(), "   rVel: ", vel.ToString(), " |", vel.magnitude, "|"));
			TrackRigidbody(newReferenceBody, 0.0 - fdtLast);
			orbit.GetOrbitalStateVectorsAtUT(lastTrackUT, out var vector3d, out var vector3d2);
			if (newReferenceBody != null && orbit != null && orbit.referenceBody != null && vessel != null && celestialBody != null)
			{
				Debug.Log("recalculated orbit for " + base.name + ": " + newReferenceBody.bodyName + " ( UT: " + Planetarium.GetUniversalTime() + " )\nrPos: " + pos.ToString() + "   rVel: " + orbit.GetFrameVel().ToString() + " |" + vel.magnitude + "|\nDelta: " + (vector3d + orbit.referenceBody.position.xzy - vessel.CoMD.xzy).ToString() + " / " + (vector3d2 - (vessel.rb_velocityD + Krakensbane.GetFrameVelocity()).xzy - ((celestialBody.orbit != null) ? celestialBody.orbit.GetFrameVel() : Vector3d.zero)).ToString());
			}
		}
		if (OnReferenceBodyChange != null)
		{
			OnReferenceBodyChange(newReferenceBody);
		}
		if (vessel != null)
		{
			GameEvents.onVesselSOIChanged.Fire(new GameEvents.HostedFromToAction<Vessel, CelestialBody>(vessel, celestialBody, newReferenceBody));
			ModuleTripLogger.ForceVesselLog(vessel, FlightLog.EntryType.Flyby, newReferenceBody);
		}
		Invoke("unlockFrameSwitch", 1f);
	}

	public void OnRailsSOITransition(Orbit ownOrbit, CelestialBody to)
	{
		CelestialBody celestialBody = referenceBody;
		double universalTime = Planetarium.GetUniversalTime();
		double num = universalTime - (double)TimeWarp.fixedDeltaTime;
		double num2 = universalTime - num;
		double epsilon = num2 * 1E-07;
		double v = universalTime;
		double SOIsqr = 0.0;
		int num3 = 0;
		if (orbit.referenceBody.HasChild(to))
		{
			SOIsqr = to.sphereOfInfluence * to.sphereOfInfluence;
			num3 = UtilMath.BSPSolver(ref v, num2, (double t) => Math.Abs((ownOrbit.getPositionAtUT(t) - to.getPositionAtUT(t)).sqrMagnitude - SOIsqr), num, universalTime, epsilon, 64);
		}
		else if (to.HasChild(orbit.referenceBody))
		{
			SOIsqr = orbit.referenceBody.sphereOfInfluence * orbit.referenceBody.sphereOfInfluence;
			num3 = UtilMath.BSPSolver(ref v, num2, (double t) => Math.Abs(ownOrbit.getRelativePositionAtUT(t).sqrMagnitude - SOIsqr), num, universalTime, epsilon, 64);
		}
		ownOrbit.UpdateFromOrbitAtUT(ownOrbit, v, to);
		Debug.Log("[OrbitDriver]: On-Rails SOI Transition from " + celestialBody.name + " to " + to.name + ".\n Transition UT Range: " + num.ToString("0.###") + " - " + universalTime.ToString("0.###") + ".\n Transition UT: " + v.ToString("0.###") + ". Iterations: " + num3 + ".", base.gameObject);
	}

	public void unlockFrameSwitch()
	{
		frameShift = false;
	}
}
