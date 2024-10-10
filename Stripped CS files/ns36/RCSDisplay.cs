using ns2;
using UnityEngine;

namespace ns36;

public class RCSDisplay : MonoBehaviour
{
	public UIStateImage stateImage;

	public UIStateText stateText;

	public void Reset()
	{
		stateImage = GetComponent<UIStateImage>();
		stateText = GetComponent<UIStateText>();
	}

	public void Start()
	{
		LateUpdate();
	}

	public void SetNewState(string newState)
	{
		stateImage.SetState(newState);
		stateText.SetState(newState);
	}

	public void LateUpdate()
	{
		if (FlightGlobals.ready)
		{
			KerbalEVA kerbalEVA = (FlightGlobals.ActiveVessel.isEVA ? FlightGlobals.ActiveVessel.evaController : null);
			if (kerbalEVA == null)
			{
				SetNewState(FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_5] ? "On" : "Off");
			}
			else
			{
				SetNewState(kerbalEVA.JetpackDeployed ? "On" : "Off");
			}
		}
	}
}
