using System;
using System.Collections.Generic;
using ModuleWheels;
using UnityEngine;

public class SuspensionLoadBalancer : VesselModule
{
	public List<ModuleWheelSuspension> suspensionModules;

	public bool isReady;

	public float multiplier = 1f;

	public float minMass = 0.5f;

	public override void OnAwake()
	{
		if ((bool)vessel)
		{
			vessel.suspensionLoadBalancer = this;
		}
	}

	public override void OnStart()
	{
		UpdateSuspensionModules();
		GameEvents.onVesselPartCountChanged.Add(VesselChanged);
		isReady = true;
	}

	public void FixedUpdate()
	{
		if (!isReady || suspensionModules.Count == 0)
		{
			return;
		}
		double num = vessel.GetTotalMass();
		if (vessel.Landed)
		{
			num = Math.Round(num * vessel.gravityMultiplier * vessel.mainBody.GeeASL * 10.0) / 10.0;
		}
		int num2 = 0;
		for (int i = 0; i < suspensionModules.Count; i++)
		{
			if (suspensionModules[i].wheel != null && suspensionModules[i].wheel.IsGrounded)
			{
				num2++;
			}
		}
		for (int j = 0; j < suspensionModules.Count; j++)
		{
			float num3 = 0f;
			if (suspensionModules[j].wheel != null)
			{
				num3 = Mathf.Round(100f / (((float)num2 > 0f) ? ((float)num2) : 1f)) / 100f;
				suspensionModules[j].sprungMassGravity = Mathf.Max((float)(num * (double)num3 * (double)multiplier), minMass);
			}
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVesselPartCountChanged.Remove(VesselChanged);
	}

	public void VesselChanged(Vessel vsl)
	{
		if (vsl.persistentId == vessel.persistentId)
		{
			UpdateSuspensionModules();
		}
	}

	public void UpdateSuspensionModules()
	{
		suspensionModules = new List<ModuleWheelSuspension>();
		for (int i = 0; i < vessel.parts.Count; i++)
		{
			ModuleWheelSuspension moduleWheelSuspension = vessel.parts[i].FindModuleImplementing<ModuleWheelSuspension>();
			if (moduleWheelSuspension != null)
			{
				suspensionModules.Add(moduleWheelSuspension);
			}
		}
	}
}
