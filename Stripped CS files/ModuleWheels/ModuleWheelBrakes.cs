using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelBrakes : ModuleWheelSubmodule
{
	[KSPField]
	public float maxBrakeTorque = 2000f;

	[KSPField]
	public float brakeResponse = 10f;

	[UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, stepIncrement = 5f, maxValue = 200f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_148080", guiUnits = "%")]
	public float brakeTweakable = 50f;

	[KSPField]
	public int statusLightModuleIndex = -1;

	[KSPField(isPersistant = true)]
	public float brakeInput = -1f;

	public ModuleStatusLight statusLight;

	public List<AdjusterWheelBrakesBase> adjusterCache = new List<AdjusterWheelBrakesBase>();

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (statusLightModuleIndex != -1)
		{
			ModuleStatusLight moduleStatusLight = base.part.Modules[statusLightModuleIndex] as ModuleStatusLight;
			if (moduleStatusLight != null)
			{
				statusLight = moduleStatusLight;
			}
			else
			{
				Debug.LogError("[ModuleWheelBrakes]: Module at statusLightModuleIndex " + statusLightModuleIndex + " in " + base.part.partName + " is not of type ModuleStatusLight!", base.gameObject);
			}
		}
		if (brakeInput < 0f)
		{
			brakeInput = 0f;
			if (HighLogic.LoadedScene == GameScenes.FLIGHT && base.vessel.ActionGroups[base.Actions["BrakeAction"].actionGroup])
			{
				brakeInput = 1f;
			}
		}
	}

	public override void OnWheelSetup()
	{
		wheel.maxBrakeTorque = maxBrakeTorque;
		wheel.brakeResponse = brakeResponse;
		if (statusLight != null)
		{
			statusLight.SetStatus(brakeInput > 0f && brakeTweakable > 0f);
		}
	}

	[KSPAction("#autoLOC_6001458", KSPActionGroup.Brakes)]
	public void BrakeAction(KSPActionParam kPar)
	{
		KSPActionType kSPActionType = kPar.type;
		if (kSPActionType == KSPActionType.Toggle)
		{
			kSPActionType = ((!(brakeInput <= 0f)) ? KSPActionType.Deactivate : KSPActionType.Activate);
		}
		switch (kSPActionType)
		{
		case KSPActionType.Deactivate:
			brakeInput = 0f;
			break;
		case KSPActionType.Activate:
			brakeInput = 1f;
			break;
		}
		if (statusLight != null)
		{
			statusLight.SetStatus(kPar.type == KSPActionType.Activate && brakeTweakable > 0f);
		}
	}

	public void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && baseSetup)
		{
			if (!base.part.packed && !wheelBase.InopSystems.HasType(WheelSubsystem.SystemTypes.Brakes))
			{
				float num = Mathf.Min(1f, Mathf.Lerp(1f, (float)base.vessel.mainBody.GeeASL, Mathf.Abs(wheel.currentState.angularVelocity)));
				wheel.brakeInput = ApplyTorqueAdjustments(brakeInput) * (brakeTweakable * 0.01f) * num;
				wheel.brakeResponse = ((wheel.brakeInput != 0f) ? brakeResponse : 100f);
			}
			else
			{
				wheel.brakeResponse = brakeResponse;
				wheel.brakeInput = 0f;
			}
		}
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_246599", maxBrakeTorque.ToString("0.0"));
	}

	public override void OnSubsystemsModified(WheelSubsystems s)
	{
		if (s != wheelBase.InopSystems || !(statusLight != null))
		{
			return;
		}
		if (s.HasType(WheelSubsystem.SystemTypes.Brakes))
		{
			if (statusLight.IsOn)
			{
				statusLight.SetStatus(status: false);
			}
		}
		else if (brakeInput > 0f)
		{
			statusLight.SetStatus(status: true);
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterWheelBrakesBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterWheelBrakesBase item = adjuster as AdjusterWheelBrakesBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyTorqueAdjustments(float torque)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			torque = adjusterCache[i].ApplyTorqueAdjustment(torque);
		}
		return torque;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004212");
	}
}
