using System.Collections.Generic;
using ns9;
using UnityEngine;

public class VesselSwitching : MonoBehaviour
{
	public static VesselSwitching fetch;

	public int nextVessel;

	public List<Vessel> nextVesselCandidates = new List<Vessel>();

	public Vessel v;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Update()
	{
		if (MapView.MapIsEnabled || InputLockManager.IsLocked(ControlTypes.VESSEL_SWITCHING) || FlightDriver.Pause)
		{
			return;
		}
		nextVessel = -1;
		int num = 0;
		bool flag = false;
		if (GameSettings.FOCUS_PREV_VESSEL.GetKeyDown() || GameSettings.FOCUS_NEXT_VESSEL.GetKeyDown())
		{
			if (HighLogic.CurrentGame == null || !HighLogic.LoadedSceneIsFlight)
			{
				return;
			}
			if (HighLogic.CurrentGame != null && !HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_137562"), 5f, ScreenMessageStyle.UPPER_CENTER);
				return;
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				CameraManager.Instance.SetCameraFlight();
			}
			nextVesselCandidates.Clear();
			nextVessel = FlightGlobals.Vessels.IndexOf(FlightGlobals.ActiveVessel);
			int num2 = ((!GameSettings.FOCUS_PREV_VESSEL.GetKeyDown()) ? 1 : (-1));
			int count = FlightGlobals.Vessels.Count;
			do
			{
				nextVessel += num2;
				if (nextVessel >= 0)
				{
					if (nextVessel >= count)
					{
						nextVessel -= count;
					}
				}
				else
				{
					nextVessel += count;
				}
				v = FlightGlobals.Vessels[nextVessel];
				if (v.loaded && v != FlightGlobals.ActiveVessel && v.vesselType != VesselType.DeployedSciencePart)
				{
					if (v.vesselType <= VesselType.SpaceObject && !GameSettings.MODIFIER_KEY.GetKey())
					{
						flag = true;
						continue;
					}
					nextVesselCandidates.Add(v);
					num++;
				}
			}
			while (v != FlightGlobals.ActiveVessel);
		}
		if (nextVessel == -1)
		{
			return;
		}
		if (nextVesselCandidates.Count == 0)
		{
			if (flag)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003367"), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_137597"), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			return;
		}
		ClearToSaveStatus clearToSaveStatus = FlightGlobals.ClearToSave();
		int num3 = 0;
		while (true)
		{
			if (num3 < num)
			{
				Vessel vessel = nextVesselCandidates[num3];
				if (vessel.packed && clearToSaveStatus != 0)
				{
					num3++;
					continue;
				}
				FlightGlobals.ForceSetActiveVessel(vessel);
				FlightInputHandler.ResumeVesselCtrlState(vessel);
				break;
			}
			if (FlightGlobals.SetActiveVessel(nextVesselCandidates[0]))
			{
				Debug.LogError("[Vessel Switching]: Should not have been possible to get here");
			}
			break;
		}
	}
}
