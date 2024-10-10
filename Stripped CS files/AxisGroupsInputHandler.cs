using System;
using UnityEngine;

public class AxisGroupsInputHandler : MonoBehaviour
{
	public AxisGroupsModule VesselAxisGroups { get; set; }

	public void Awake()
	{
		FlightInputHandler.OnRawAxisInput = (FlightInputCallback)Delegate.Combine(FlightInputHandler.OnRawAxisInput, new FlightInputCallback(AxisInputHandler));
		GameEvents.onVesselChange.Add(onVesselChange);
	}

	public void OnDestroy()
	{
		FlightInputHandler.OnRawAxisInput = (FlightInputCallback)Delegate.Remove(FlightInputHandler.OnRawAxisInput, new FlightInputCallback(AxisInputHandler));
		GameEvents.onVesselChange.Remove(onVesselChange);
	}

	public void onVesselChange(Vessel vessel)
	{
		VesselAxisGroups = null;
		int count = vessel.vesselModules.Count;
		VesselModule vesselModule;
		do
		{
			if (count-- > 0)
			{
				vesselModule = vessel.vesselModules[count];
				continue;
			}
			return;
		}
		while (!(vesselModule is AxisGroupsModule));
		VesselAxisGroups = vesselModule as AxisGroupsModule;
	}

	public void AxisInputHandler(FlightCtrlState ctrlState)
	{
		if (VesselAxisGroups != null)
		{
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.Pitch, ctrlState.pitch);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.Yaw, ctrlState.yaw);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.Roll, ctrlState.roll);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.TranslateX, ctrlState.float_0);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.TranslateY, ctrlState.float_1);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.TranslateZ, ctrlState.float_2);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.MainThrottle, ctrlState.mainThrottle);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.WheelSteer, ctrlState.wheelSteer);
			VesselAxisGroups.UpdateAxisGroup(KSPAxisGroup.WheelThrottle, ctrlState.wheelThrottle);
			for (int i = 0; i < ctrlState.custom_axes.Length; i++)
			{
				int axisGroup = 512 << i;
				VesselAxisGroups.UpdateAxisGroup((KSPAxisGroup)axisGroup, ctrlState.custom_axes[i]);
			}
		}
	}
}
