using ns11;
using UnityEngine;

namespace ns36;

public class GeeGauge : MonoBehaviour
{
	public RotationalGauge gauge;

	public void Reset()
	{
		gauge = GetComponent<RotationalGauge>();
	}

	public void LateUpdate()
	{
		if (FlightGlobals.ready && (object)gauge != null)
		{
			gauge.SetValue(FlightGlobals.ActiveVessel.geeForce);
		}
	}
}
