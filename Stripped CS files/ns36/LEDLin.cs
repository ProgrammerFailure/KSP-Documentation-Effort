using ns11;
using UnityEngine;

namespace ns36;

public class LEDLin : MonoBehaviour
{
	public GClass9 led;

	public void Reset()
	{
		led = GetComponent<GClass9>();
	}

	public void LateUpdate()
	{
		if (FlightGlobals.ready && led != null)
		{
			led.SetOn(!InputBinding.linRotState);
		}
	}
}
