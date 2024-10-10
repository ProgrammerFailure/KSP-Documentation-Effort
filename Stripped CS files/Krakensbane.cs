using UnityEngine;

public class Krakensbane : MonoBehaviour
{
	public static Krakensbane fetch;

	public double MaxV = 5.0;

	public double MaxVSqr = double.MaxValue;

	public double extraAltOffsetForVel = 100.0;

	public double altThreshold = 200.0;

	public double altThresholdAlone = 50.0;

	public Vector3d FrameVel;

	public Vector3d totalVel;

	public Vector3d RBVel;

	public Vector3d excessV;

	public Vector3d lastCorrection;

	public int loadedVesselsCount;

	public static double SqrThreshold => fetch.MaxVSqr;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
			return;
		}
		fetch = this;
		MaxVSqr = MaxV * MaxV;
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Start()
	{
		FrameVel = Vector3d.zero;
	}

	public void FixedUpdate()
	{
		if (!FlightGlobals.ready || FlightGlobals.ActiveVessel == null || !FlightGlobals.ActiveVessel.GetComponent<Rigidbody>())
		{
			return;
		}
		lastCorrection.Zero();
		bool safeForFloatingOrigin;
		bool flag = SafeToEngage(out safeForFloatingOrigin);
		FloatingOrigin.SetSafeToEngage(safeForFloatingOrigin);
		if (!FlightGlobals.ActiveVessel.packed)
		{
			if (flag && FlightGlobals.ActiveVessel.velocityD.sqrMagnitude > MaxVSqr)
			{
				AddExcess(FlightGlobals.ActiveVessel.rb_velocityD);
			}
			else if (!FrameVel.IsZero())
			{
				Zero();
			}
		}
		else
		{
			FrameVel.Zero();
			excessV.Zero();
		}
		if (!FrameVel.IsZero())
		{
			FloatingOrigin.SetOutOfFrameOffset(FrameVel * Time.fixedDeltaTime);
		}
	}

	public bool SafeToEngage(out bool safeForFloatingOrigin)
	{
		safeForFloatingOrigin = false;
		loadedVesselsCount = FlightGlobals.VesselsLoaded.Count;
		if (FlightGlobals.ActiveVessel.state == Vessel.State.DEAD)
		{
			return false;
		}
		safeForFloatingOrigin = true;
		int index = loadedVesselsCount;
		Vessel vessel;
		do
		{
			if (index-- > 0)
			{
				vessel = FlightGlobals.VesselsLoaded[index];
				continue;
			}
			if (FlightGlobals.ActiveVessel.LandedOrSplashed)
			{
				return false;
			}
			double num = FlightGlobals.ActiveVessel.radarAltitude - extraAltOffsetForVel;
			if (!(Vector3d.Dot(FlightGlobals.ActiveVessel.velocityD, -FlightGlobals.ActiveVessel.upAxis) * (double)Time.fixedDeltaTime > num) && num >= ((loadedVesselsCount > 1) ? altThreshold : altThresholdAlone))
			{
				return true;
			}
			return false;
		}
		while (!(vessel != FlightGlobals.ActiveVessel) || (!vessel.LandedOrSplashed && vessel.radarAltitude >= altThreshold));
		safeForFloatingOrigin = false;
		return false;
	}

	public void AddExcess(Vector3d vel)
	{
		excessV = vel;
		lastCorrection = -excessV;
		if (FrameVel.IsZero())
		{
			GameEvents.onKrakensbaneEngage.Fire(-excessV);
		}
		FrameVel += excessV;
		int index = loadedVesselsCount;
		while (index-- > 0)
		{
			Vessel vessel = FlightGlobals.VesselsLoaded[index];
			if (!vessel.packed && vessel.state != Vessel.State.DEAD)
			{
				vessel.ChangeWorldVelocity(-excessV);
			}
		}
		int count = FlightGlobals.physicalObjects.Count;
		for (int i = 0; i < count; i++)
		{
			physicalObject physicalObject2 = FlightGlobals.physicalObjects[i];
			if (!(physicalObject2 == null) && !(physicalObject2.rb == null))
			{
				physicalObject2.rb.AddForce(-excessV, ForceMode.VelocityChange);
			}
		}
	}

	public void Zero()
	{
		lastCorrection = FrameVel;
		GameEvents.onKrakensbaneDisengage.Fire(FrameVel);
		int count = FlightGlobals.VesselsLoaded.Count;
		for (int i = 0; i < count; i++)
		{
			Vessel vessel = FlightGlobals.VesselsLoaded[i];
			if (!vessel.packed && vessel.state != Vessel.State.DEAD && (!vessel.LandedOrSplashed || !(vessel != FlightGlobals.ActiveVessel)))
			{
				vessel.ChangeWorldVelocity(FrameVel);
			}
		}
		int count2 = FlightGlobals.physicalObjects.Count;
		for (int j = 0; j < count2; j++)
		{
			physicalObject physicalObject2 = FlightGlobals.physicalObjects[j];
			if (!(physicalObject2 == null) && !(physicalObject2.rb == null))
			{
				physicalObject2.rb.AddForce(FrameVel, ForceMode.VelocityChange);
			}
		}
		FrameVel.Zero();
	}

	public static Vector3d GetFrameVelocity()
	{
		return fetch.FrameVel;
	}

	public static Vector3 GetFrameVelocityV3f()
	{
		return fetch.FrameVel;
	}

	public static Vector3d GetLastCorrection()
	{
		return fetch.lastCorrection;
	}

	public static void ResetVelocityFrame(bool resetObjects)
	{
		if (resetObjects)
		{
			fetch.Zero();
		}
		else
		{
			fetch.FrameVel.Zero();
		}
	}

	public static void AddFrameVelocity(Vector3d vel)
	{
		if (fetch.SafeToEngage(out var _))
		{
			fetch.AddExcess(vel);
		}
	}
}
