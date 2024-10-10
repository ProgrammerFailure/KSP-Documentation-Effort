using ns11;
using UnityEngine;

namespace ns36;

public class VerticalSpeedGauge : MonoBehaviour
{
	public RotationalGauge gauge;

	public GClass9 sinkRateLED;

	public static double SinkRateOn = -15.0;

	public static double SinkRateOff = -14.0;

	public void Reset()
	{
		gauge = GetComponent<RotationalGauge>();
	}

	public void Start()
	{
		sinkRateLED.SetColor(GClass9.colorIndices.yellow);
		sinkRateLED.setOff();
	}

	public void LateUpdate()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (sinkRateLED.IsOn)
		{
			if (FlightGlobals.ActiveVessel.verticalSpeed > SinkRateOff)
			{
				sinkRateLED.setOff();
			}
		}
		else if (FlightGlobals.ActiveVessel.verticalSpeed < SinkRateOn)
		{
			sinkRateLED.SetOn();
		}
		gauge.SetValue(FlightGlobals.ActiveVessel.verticalSpeed);
	}
}
