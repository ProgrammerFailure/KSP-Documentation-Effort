using System;
using UnityEngine;

public class EditorMarker_CoL : EditorMarker
{
	public Vector3 referenceVelocity = Vector3.up;

	public float referencePitch = 1f;

	public float referenceSpeed = 70f;

	public double refAlt;

	public double refStP;

	public double refDens;

	public Ray CoL;

	public double refTemp;

	public static CenterOfLiftQuery lQry = new CenterOfLiftQuery();

	public void Awake()
	{
		if (rootPart == null && ((HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch) || (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)))
		{
			rootPart = (HighLogic.LoadedSceneIsEditor ? EditorLogic.RootPart : FlightGlobals.ActiveVessel.rootPart);
		}
	}

	public override Vector3 UpdatePosition()
	{
		if ((HighLogic.LoadedSceneIsEditor && EditorLogic.fetch == null) || (HighLogic.LoadedSceneIsFlight && (rootPart == null || rootPart.vessel == null)))
		{
			return Vector3.zero;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			referenceVelocity = EditorLogic.VesselRotation * (Quaternion.AngleAxis(referencePitch, EditorLogic.RootPart.transform.right) * Vector3.up);
			referenceVelocity *= referenceSpeed;
			refAlt = 100.0;
			refStP = FlightGlobals.getStaticPressure(refAlt, Planetarium.fetch.Home);
			refTemp = FlightGlobals.getExternalTemperature(refAlt, Planetarium.fetch.Home);
			refDens = FlightGlobals.getAtmDensity(refStP, refTemp, Planetarium.fetch.Home);
		}
		else
		{
			referenceVelocity = new Vector3((float)Math.Round(rootPart.vessel.angularVelocity.x, 2), (float)Math.Round(rootPart.vessel.angularVelocity.y, 2), (float)Math.Round(rootPart.vessel.angularVelocity.z, 2));
			refAlt = Math.Round(rootPart.vessel.altitude, 2);
			refStP = Math.Round(rootPart.vessel.staticPressurekPa, 2);
			refTemp = Math.Round(rootPart.vessel.externalTemperature, 2);
			refDens = Math.Round(rootPart.vessel.atmDensity, 2);
		}
		bool noLift = false;
		CoL = FindCoL(rootPart, referenceVelocity, refAlt, refStP, refDens, out noLift);
		if ((bool)posMarkerObject)
		{
			if (HighLogic.LoadedSceneIsFlight && noLift)
			{
				posMarkerObject.transform.position = ((rootPart.vessel != null) ? rootPart.vessel.CoM : rootPart.transform.position);
				CoL.origin = posMarkerObject.transform.position;
			}
			else
			{
				posMarkerObject.transform.position = CoL.origin;
			}
		}
		if (CoL.direction == Vector3.zero && dirMarkerObject.activeInHierarchy)
		{
			dirMarkerObject.SetActive(value: false);
		}
		if (CoL.direction != Vector3.zero && !dirMarkerObject.activeInHierarchy)
		{
			dirMarkerObject.SetActive(value: true);
		}
		if ((bool)dirMarkerObject && CoL.direction != Vector3.zero)
		{
			dirMarkerObject.transform.forward = CoL.direction;
		}
		return CoL.origin;
	}

	public static Ray FindCoL(Part rootPart, Vector3 refVel, double refAlt, double refStp, double refDens)
	{
		bool noLift = false;
		return FindCoL(rootPart, refVel, refAlt, refStp, refDens, out noLift);
	}

	public static Ray FindCoL(Part rootPart, Vector3 refVel, double refAlt, double refStp, double refDens, out bool noLift)
	{
		Vector3 CoL = Vector3.zero;
		Vector3 DoL = Vector3.zero;
		float t = 0f;
		noLift = true;
		if (rootPart != null)
		{
			recurseParts(rootPart, refVel, ref CoL, ref DoL, ref t, refAlt, refStp, refDens);
		}
		if (HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.SelectedPart && !EditorLogic.fetch.ship.Contains(EditorLogic.SelectedPart) && (bool)EditorLogic.SelectedPart.potentialParent)
		{
			recurseParts(EditorLogic.SelectedPart, refVel, ref CoL, ref DoL, ref t, refAlt, refStp, refDens);
			for (int i = 0; i < EditorLogic.SelectedPart.symmetryCounterparts.Count; i++)
			{
				recurseParts(EditorLogic.SelectedPart.symmetryCounterparts[i], refVel, ref CoL, ref DoL, ref t, refAlt, refStp, refDens);
			}
		}
		if (t != 0f)
		{
			float num = 1f / t;
			CoL *= num;
			DoL *= num;
			noLift = false;
			return new Ray(CoL, DoL);
		}
		return new Ray(Vector3.zero, Vector3.zero);
	}

	public static Ray FindCoL(Vector3 refVel, double refAlt, double refStp, double refDens)
	{
		Part part = null;
		if ((HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch) || (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null))
		{
			part = (HighLogic.LoadedSceneIsEditor ? EditorLogic.RootPart : FlightGlobals.ActiveVessel.rootPart);
		}
		return FindCoL(part, refVel, refAlt, refStp, refDens);
	}

	public static void recurseParts(Part part, Vector3 refVel, ref Vector3 CoL, ref Vector3 DoL, ref float t, double refAlt, double refStp, double refDens)
	{
		int count = part.Modules.Count;
		while (count-- > 0)
		{
			if (part.Modules[count] is ILiftProvider liftProvider)
			{
				lQry.Reset();
				lQry.refVector = refVel;
				lQry.refAltitude = refAlt;
				lQry.refStaticPressure = refStp;
				lQry.refAirDensity = refDens;
				liftProvider.OnCenterOfLiftQuery(lQry);
				CoL += lQry.pos * lQry.lift;
				DoL += lQry.dir * lQry.lift;
				t += lQry.lift;
			}
		}
		for (int i = 0; i < part.children.Count; i++)
		{
			recurseParts(part.children[i], refVel, ref CoL, ref DoL, ref t, refAlt, refStp, refDens);
		}
	}
}
