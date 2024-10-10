using UnityEngine;

public class InternalRadarAltitude : InternalModule
{
	[KSPField]
	public string indicatorName = "indicator";

	[KSPField]
	public InternalDialIncrement increments = new InternalDialIncrement();

	[KSPField]
	public Vector3 axis = new Vector3(0f, 0f, 1f);

	[KSPField]
	public float smooth = 10f;

	public Transform hand;

	public Quaternion handInitial;

	public float current;

	public float altitude;

	public float reportedAlt;

	public override void OnAwake()
	{
		if (hand == null)
		{
			hand = internalProp.FindModelTransform(indicatorName);
			handInitial = hand.transform.localRotation;
		}
	}

	public override void OnUpdate()
	{
		reportedAlt = (float)base.vessel.radarAltitude;
		if (reportedAlt < 0f && base.vessel.heightFromTerrain > 0f)
		{
			reportedAlt = base.vessel.heightFromTerrain;
		}
		altitude = Mathf.Lerp(altitude, reportedAlt, smooth * Time.deltaTime);
		current = increments.CalculateAngle(altitude);
		hand.transform.localRotation = handInitial * Quaternion.AngleAxis(current, axis);
	}
}
