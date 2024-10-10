using ns11;
using UnityEngine;

namespace ns36;

public class ThrottleGauge : MonoBehaviour
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
			KerbalEVA kerbalEVA = (FlightGlobals.ActiveVessel.isEVA ? FlightGlobals.ActiveVessel.evaController : null);
			if (kerbalEVA == null)
			{
				gauge.SetValue(FlightGlobals.ActiveVessel.ctrlState.mainThrottle);
			}
			else
			{
				gauge.SetValue(Mathf.Lerp(gauge.Value, kerbalEVA.JetpackIsThrusting ? 1 : 0, Time.deltaTime * 0.5f));
			}
		}
	}
}
