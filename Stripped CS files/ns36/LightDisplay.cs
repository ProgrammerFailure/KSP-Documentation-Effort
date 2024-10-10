using ns2;
using UnityEngine;

namespace ns36;

public class LightDisplay : MonoBehaviour
{
	public UIButtonToggle toggle;

	public void LateUpdate()
	{
		if (FlightGlobals.ready)
		{
			if (FlightGlobals.ActiveVessel.isEVA)
			{
				toggle.SetState(FlightGlobals.ActiveVessel.evaController.lampOn);
			}
			else
			{
				toggle.SetState(FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.Light]);
			}
		}
	}
}
