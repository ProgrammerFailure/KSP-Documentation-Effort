using ns2;
using UnityEngine;

namespace ns36;

public class SASDisplay : MonoBehaviour
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
		if (!FlightGlobals.ready)
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (activeVessel.isEVA)
		{
			if (GameSettings.EVA_ROTATE_ON_MOVE)
			{
				SetNewState("On");
			}
			else
			{
				SetNewState("Off");
			}
		}
		else if (activeVessel.ActionGroups[KSPActionGroup.flag_6])
		{
			if (activeVessel.Autopilot.VesselSAS_0.dampingMode)
			{
				SetNewState("Override");
			}
			else
			{
				SetNewState("On");
			}
		}
		else
		{
			SetNewState("Off");
		}
	}
}
