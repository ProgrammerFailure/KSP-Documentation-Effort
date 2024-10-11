using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalNavBall : InternalModule
{
	[KSPField]
	public string navBallName;

	public Transform navBall;

	[KSPField]
	public Vector3 iconUp;

	[KSPField]
	public string progradeVectorName;

	public Transform progradeVector;

	[KSPField]
	public string retrogradeVectorName;

	public Transform retrogradeVector;

	[KSPField]
	public string progradeWaypointName;

	public Transform progradeWaypoint;

	[KSPField]
	public string retrogradeWaypointName;

	public Transform retrogradeWaypoint;

	[KSPField]
	public string retrogradeDeltaVName;

	public Transform retrogradeDeltaV;

	[KSPField]
	public string progradeDeltaVName;

	public Transform progradeDeltaV;

	[KSPField]
	public string normalVectorName;

	public Transform normalVector;

	[KSPField]
	public string antiNormalVectorName;

	public Transform antiNormalVector;

	[KSPField]
	public string radialInVectorName;

	public Transform radialInVector;

	[KSPField]
	public string radialOutVectorName;

	public Transform radialOutVector;

	[KSPField]
	public string navWaypointVectorName;

	public Transform navWaypointVector;

	[KSPField]
	public string maneuverArrowName;

	public Transform maneuverArrowVector;

	[KSPField]
	public string anchorName;

	[KSPField]
	public float vectorVelocityThreshold;

	public Transform anchorVector;

	private Color navWaypointColor;

	private string navWaypointTexture;

	private PatchedConicSolver solver;

	private Vector3 direction;

	[KSPField]
	public Vector3 vesselOffset;

	[KSPField]
	public Vector3 gaugeOffset;

	public Quaternion vesselOffsetRotation;

	public Quaternion gaugeOffsetRotation;

	private bool listenerSet;

	private Vector3 wCoM;

	private Vector3 obtVel;

	private Vector3 cbPos;

	private Vector3 normal;

	private Vector3 radial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalNavBall()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSwitch(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}
