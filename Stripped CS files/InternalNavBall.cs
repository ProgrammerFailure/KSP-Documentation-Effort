using FinePrint;
using UnityEngine;

public class InternalNavBall : InternalModule
{
	[KSPField]
	public string navBallName = "navBall";

	public Transform navBall;

	[KSPField]
	public Vector3 iconUp = Vector3.forward;

	[KSPField]
	public string progradeVectorName = "progradeVector";

	public Transform progradeVector;

	[KSPField]
	public string retrogradeVectorName = "retrogradeVector";

	public Transform retrogradeVector;

	[KSPField]
	public string progradeWaypointName = "progradeWaypoint";

	public Transform progradeWaypoint;

	[KSPField]
	public string retrogradeWaypointName = "retrogradeWaypoint";

	public Transform retrogradeWaypoint;

	[KSPField]
	public string retrogradeDeltaVName = "retrogradeDeltaV";

	public Transform retrogradeDeltaV;

	[KSPField]
	public string progradeDeltaVName = "progradeDeltaV";

	public Transform progradeDeltaV;

	[KSPField]
	public string normalVectorName = "NormalVector";

	public Transform normalVector;

	[KSPField]
	public string antiNormalVectorName = "antiNormalVector";

	public Transform antiNormalVector;

	[KSPField]
	public string radialInVectorName = "RadialInVector";

	public Transform radialInVector;

	[KSPField]
	public string radialOutVectorName = "RadialOutVector";

	public Transform radialOutVector;

	[KSPField]
	public string navWaypointVectorName = "NavWaypointVector";

	public Transform navWaypointVector;

	[KSPField]
	public string maneuverArrowName = "ManeuverArrow";

	public Transform maneuverArrowVector;

	[KSPField]
	public string anchorName = "Anchor";

	[KSPField]
	public float vectorVelocityThreshold = 0.1f;

	public Transform anchorVector;

	public Color navWaypointColor;

	public string navWaypointTexture;

	public PatchedConicSolver solver;

	public Vector3 direction;

	[KSPField]
	public Vector3 vesselOffset = new Vector3(90f, 0f, 0f);

	[KSPField]
	public Vector3 gaugeOffset = new Vector3(90f, 0f, 0f);

	public Quaternion vesselOffsetRotation;

	public Quaternion gaugeOffsetRotation;

	public bool listenerSet;

	public Vector3 wCoM;

	public Vector3 obtVel;

	public Vector3 cbPos;

	public Vector3 normal;

	public Vector3 radial;

	public override void OnAwake()
	{
		GameEvents.onVesselChange.Add(OnVesselSwitch);
		listenerSet = true;
		if (navBall == null)
		{
			navBall = internalProp.FindModelTransform(navBallName);
		}
		if (progradeVector == null && !string.IsNullOrEmpty(progradeVectorName))
		{
			progradeVector = internalProp.FindModelTransform(progradeVectorName);
		}
		if (retrogradeVector == null && !string.IsNullOrEmpty(retrogradeVectorName))
		{
			retrogradeVector = internalProp.FindModelTransform(retrogradeVectorName);
		}
		if (progradeWaypoint == null && !string.IsNullOrEmpty(progradeWaypointName))
		{
			progradeWaypoint = internalProp.FindModelTransform(progradeWaypointName);
		}
		if (retrogradeWaypoint == null && !string.IsNullOrEmpty(retrogradeWaypointName))
		{
			retrogradeWaypoint = internalProp.FindModelTransform(retrogradeWaypointName);
		}
		if (retrogradeDeltaV == null && !string.IsNullOrEmpty(retrogradeDeltaVName))
		{
			retrogradeDeltaV = internalProp.FindModelTransform(retrogradeDeltaVName);
		}
		if (progradeDeltaV == null && !string.IsNullOrEmpty(progradeDeltaVName))
		{
			progradeDeltaV = internalProp.FindModelTransform(progradeDeltaVName);
		}
		if (normalVector == null && !string.IsNullOrEmpty(normalVectorName))
		{
			normalVector = internalProp.FindModelTransform(normalVectorName);
		}
		if (antiNormalVector == null && !string.IsNullOrEmpty(antiNormalVectorName))
		{
			antiNormalVector = internalProp.FindModelTransform(antiNormalVectorName);
		}
		if (radialInVector == null && !string.IsNullOrEmpty(radialInVectorName))
		{
			radialInVector = internalProp.FindModelTransform(radialInVectorName);
		}
		if (radialOutVector == null && !string.IsNullOrEmpty(radialOutVectorName))
		{
			radialOutVector = internalProp.FindModelTransform(radialOutVectorName);
		}
		if (navWaypointVector == null && !string.IsNullOrEmpty(navWaypointVectorName))
		{
			navWaypointVector = internalProp.FindModelTransform(navWaypointVectorName);
			if (navWaypointVector != null)
			{
				navWaypointVector.gameObject.SetActive(value: false);
			}
		}
		if (maneuverArrowVector == null && !string.IsNullOrEmpty(maneuverArrowName))
		{
			maneuverArrowVector = internalProp.FindModelTransform(maneuverArrowName);
			if (maneuverArrowVector != null)
			{
				maneuverArrowVector.gameObject.SetActive(value: false);
			}
		}
		if (anchorVector == null && !string.IsNullOrEmpty(anchorName))
		{
			anchorVector = internalProp.FindModelTransform(anchorName);
		}
		vesselOffsetRotation = Quaternion.Euler(vesselOffset);
		gaugeOffsetRotation = Quaternion.Euler(gaugeOffset);
	}

	public void OnDestroy()
	{
		if (listenerSet)
		{
			GameEvents.onVesselChange.Remove(OnVesselSwitch);
		}
	}

	public void OnVesselSwitch(Vessel v)
	{
		solver = v.patchedConicSolver;
	}

	public override void OnUpdate()
	{
		navBall.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation * gaugeOffsetRotation;
		Vector3 vector = FlightGlobals.speedDisplayMode switch
		{
			FlightGlobals.SpeedDisplayModes.Surface => FlightGlobals.ship_srfVelocity, 
			FlightGlobals.SpeedDisplayModes.Target => FlightGlobals.ship_tgtVelocity, 
			_ => FlightGlobals.ship_obtVelocity, 
		};
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		wCoM = activeVessel.CurrentCoM;
		obtVel = activeVessel.orbit.GetVel();
		cbPos = activeVessel.mainBody.position;
		radial = Vector3.ProjectOnPlane((wCoM - cbPos).normalized, obtVel).normalized;
		normal = Vector3.Cross(radial, obtVel.normalized);
		bool flag = vector.magnitude >= vectorVelocityThreshold;
		if (progradeVector != null)
		{
			progradeVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, vector) * gaugeOffsetRotation;
			progradeVector.transform.rotation = Quaternion.AngleAxis(-90f, progradeVector.transform.forward) * Quaternion.LookRotation(progradeVector.transform.forward, -base.transform.forward);
			progradeVector.gameObject.SetActive(flag);
		}
		if (retrogradeVector != null)
		{
			retrogradeVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, -vector) * gaugeOffsetRotation;
			retrogradeVector.transform.rotation = Quaternion.AngleAxis(-90f, retrogradeVector.transform.forward) * Quaternion.LookRotation(retrogradeVector.transform.forward, -base.transform.forward);
			retrogradeVector.gameObject.SetActive(flag);
		}
		flag = flag && FlightGlobals.speedDisplayMode == FlightGlobals.SpeedDisplayModes.Orbit;
		if (normalVector != null)
		{
			normalVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, -normal) * gaugeOffsetRotation;
			normalVector.transform.rotation = Quaternion.AngleAxis(-90f, normalVector.transform.forward) * Quaternion.LookRotation(normalVector.transform.forward, -base.transform.forward);
			normalVector.gameObject.SetActive(flag);
		}
		if (antiNormalVector != null)
		{
			antiNormalVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, normal) * gaugeOffsetRotation;
			antiNormalVector.transform.rotation = Quaternion.AngleAxis(-90f, antiNormalVector.transform.forward) * Quaternion.LookRotation(antiNormalVector.transform.forward, -base.transform.forward);
			antiNormalVector.gameObject.SetActive(flag);
		}
		if (radialInVector != null)
		{
			radialInVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, -radial) * gaugeOffsetRotation;
			radialInVector.transform.rotation = Quaternion.AngleAxis(-90f, radialInVector.transform.forward) * Quaternion.LookRotation(radialInVector.transform.forward, -base.transform.forward);
			radialInVector.gameObject.SetActive(flag);
		}
		if (radialOutVector != null)
		{
			radialOutVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, radial) * gaugeOffsetRotation;
			radialOutVector.transform.rotation = Quaternion.AngleAxis(-90f, radialOutVector.transform.forward) * Quaternion.LookRotation(radialOutVector.transform.forward, -base.transform.forward);
			radialOutVector.gameObject.SetActive(flag);
		}
		NavWaypoint fetch = NavWaypoint.fetch;
		if (navWaypointVector != null && fetch != null)
		{
			navWaypointVector.gameObject.SetActive(fetch.IsVisible());
			if (navWaypointTexture != fetch.TextureID)
			{
				navWaypointTexture = fetch.TextureID;
				Texture2D texture = ContractDefs.sprites[navWaypointTexture].texture;
				if (texture != null)
				{
					navWaypointVector.gameObject.GetComponent<Renderer>().material.mainTexture = texture;
				}
			}
			if (navWaypointColor != fetch.Color)
			{
				navWaypointColor = fetch.Color;
				navWaypointVector.gameObject.GetComponent<Renderer>().material.SetColor(PropertyIDs._Color, navWaypointColor);
			}
			navWaypointVector.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, NavWaypoint.fetch.NavigationVector) * gaugeOffsetRotation;
			navWaypointVector.transform.rotation = Quaternion.AngleAxis(-90f, navWaypointVector.transform.forward) * Quaternion.LookRotation(navWaypointVector.transform.forward, -base.transform.forward);
		}
		if (solver != null && solver.maneuverNodes.Count != 0)
		{
			direction = solver.maneuverNodes[0].GetBurnVector(FlightGlobals.ActiveVessel.orbit);
			direction.Normalize();
			if (progradeDeltaV != null)
			{
				progradeDeltaV.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, direction) * gaugeOffsetRotation;
				progradeDeltaV.transform.rotation = Quaternion.AngleAxis(-90f, progradeDeltaV.transform.forward) * Quaternion.LookRotation(progradeDeltaV.transform.forward, -base.transform.forward);
				progradeDeltaV.gameObject.SetActive(value: true);
				progradeWaypoint.gameObject.SetActive(value: false);
			}
			if (retrogradeDeltaV != null)
			{
				retrogradeDeltaV.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, -direction) * gaugeOffsetRotation;
				retrogradeDeltaV.transform.rotation = Quaternion.AngleAxis(-90f, retrogradeDeltaV.transform.forward) * Quaternion.LookRotation(retrogradeDeltaV.transform.forward, -base.transform.forward);
				retrogradeDeltaV.gameObject.SetActive(value: true);
				retrogradeWaypoint.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (progradeDeltaV != null)
			{
				progradeDeltaV.gameObject.SetActive(value: false);
				progradeWaypoint.gameObject.SetActive(value: true);
			}
			if (retrogradeDeltaV != null)
			{
				retrogradeDeltaV.gameObject.SetActive(value: false);
				retrogradeWaypoint.gameObject.SetActive(value: true);
			}
			if (maneuverArrowVector != null)
			{
				maneuverArrowVector.gameObject.SetActive(value: false);
			}
		}
		if (FlightGlobals.fetch.vesselTargetMode == VesselTargetModes.None)
		{
			progradeWaypoint.gameObject.SetActive(value: false);
			retrogradeWaypoint.gameObject.SetActive(value: false);
		}
		if (progradeWaypoint != null)
		{
			progradeWaypoint.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, FlightGlobals.fetch.vesselTargetDirection) * gaugeOffsetRotation;
			progradeWaypoint.transform.rotation = Quaternion.AngleAxis(-90f, progradeWaypoint.transform.forward) * Quaternion.LookRotation(progradeWaypoint.transform.forward, -base.transform.forward);
		}
		if (retrogradeWaypoint != null)
		{
			retrogradeWaypoint.transform.localRotation = vesselOffsetRotation * FlightGlobals.ship_orientation_offset * Quaternion.FromToRotation(iconUp, -FlightGlobals.fetch.vesselTargetDirection) * gaugeOffsetRotation;
			retrogradeWaypoint.transform.rotation = Quaternion.AngleAxis(-90f, retrogradeWaypoint.transform.forward) * Quaternion.LookRotation(retrogradeWaypoint.transform.forward, -base.transform.forward);
		}
	}
}
