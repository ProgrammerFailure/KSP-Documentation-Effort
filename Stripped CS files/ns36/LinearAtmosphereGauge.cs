using ns11;
using UnityEngine;

namespace ns36;

public class LinearAtmosphereGauge : MonoBehaviour
{
	public ns11.LinearGauge gauge;

	public CelestialBody body;

	public double densityRecip = 1.0;

	public void Reset()
	{
		gauge = GetComponent<ns11.LinearGauge>();
	}

	public void LateUpdate()
	{
		if (gauge == null || !FlightGlobals.ready)
		{
			return;
		}
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
			double num = FlightGlobals.ActiveVessel.atmDensity * densityRecip;
			if (num > 1.0)
			{
				num = 1.0;
			}
			gauge.SetValue(num);
		}
		else
		{
			gauge.SetValue(0f);
		}
	}
}
