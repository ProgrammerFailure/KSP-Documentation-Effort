using UnityEngine;

public class InternalAtmosphereDepth : InternalModule
{
	[KSPField]
	public string indicatorName = "indicator";

	[KSPField]
	public Vector3 min = new Vector3(0f, 0f, 0f);

	[KSPField]
	public float minValue = 1f;

	[KSPField]
	public Vector3 max = new Vector3(10f, 0f, 0f);

	[KSPField]
	public float maxValue = 100000f;

	[KSPField]
	public float log = 2.71828f;

	[KSPField]
	public float smooth = 10f;

	public Transform hand;

	public float logMin;

	public float logMax;

	public float atmosphereDepth;

	public float tgtValue;

	public Vector3 tgtPos;

	public CelestialBody body;

	public double densityRecip = 1.0;

	public override void OnAwake()
	{
		if (hand == null)
		{
			hand = internalProp.FindModelTransform(indicatorName);
		}
		logMin = Mathf.Log(minValue, log);
		logMax = Mathf.Log(maxValue, log);
	}

	public override void OnUpdate()
	{
		if (body != FlightGlobals.currentMainBody)
		{
			body = FlightGlobals.currentMainBody;
			if (body.atmosphere)
			{
				densityRecip = 1.0 / body.GetDensity(body.GetPressure(0.0), body.GetTemperature(0.0));
			}
			else
			{
				densityRecip = 1.0;
			}
		}
		if (body.atmosphere)
		{
			atmosphereDepth = (float)(FlightGlobals.ActiveVessel.atmDensity * densityRecip);
			if (atmosphereDepth > 1f)
			{
				atmosphereDepth = 1f;
			}
			tgtValue = Mathf.Pow(atmosphereDepth, 0.25f);
			tgtPos = Vector3.Lerp(min, max, tgtValue);
		}
		else
		{
			tgtValue = 0f;
			tgtPos = min;
		}
		hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, tgtPos, Time.deltaTime * smooth);
	}
}
