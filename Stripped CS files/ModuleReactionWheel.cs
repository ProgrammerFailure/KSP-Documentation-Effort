using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleReactionWheel : PartModule, IResourceConsumer, ITorqueProvider
{
	public enum WheelState
	{
		[Description("#autoLOC_218513")]
		Active,
		[Description("#autoLOC_6003062")]
		Disabled,
		[Description("#autoLOC_6003063")]
		Broken
	}

	[KSPField]
	public string actionGUIName = "#autoLOC_6001305";

	[KSPField]
	public float PitchTorque = 5f;

	[KSPField]
	public float YawTorque = 5f;

	[KSPField]
	public float RollTorque = 5f;

	[KSPField]
	public float torqueResponseSpeed = 30f;

	public WheelState wheelState;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001306")]
	[UI_Cycle(stateNames = new string[] { "#autoLOC_6001075", "#autoLOC_6001307", "#autoLOC_6001308" }, controlEnabled = true, scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All)]
	public int actuatorModeCycle;

	public bool operational = true;

	[KSPField(guiFormat = "0", isPersistant = true, guiActive = true, guiName = "#autoLOC_6001309", guiUnits = "%")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float authorityLimiter = 100f;

	public Vector3 inputVector = new Vector3(0f, 0f, 0f);

	public float inputSum;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001306")]
	public string stateString = Localizer.Format("#autoLOC_218513");

	public List<PartResourceDefinition> consumedResources;

	public Vector3 cmdInput;

	public Vector3 inputVectorTemp;

	public List<AdjusterReactionWheelBase> adjusterCache = new List<AdjusterReactionWheelBase>();

	public static string cacheAutoLOC_236147;

	public static string cacheAutoLOC_7001223;

	public static string cacheAutoLOC_218716;

	public static string cacheAutoLOC_6003052;

	public WheelState State
	{
		get
		{
			return wheelState;
		}
		set
		{
			wheelState = value;
			stateString = wheelState.displayDescription();
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public override void OnStart(StartState state)
	{
		base.Actions["Toggle"].guiName = actionGUIName;
		BaseField baseField = base.Fields["actuatorModeCycle"];
		bool guiActive = (base.Fields["actuatorModeCycle"].guiActiveEditor = moduleIsEnabled);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["authorityLimiter"];
		guiActive = (base.Fields["authorityLimiter"].guiActiveEditor = moduleIsEnabled);
		baseField2.guiActive = guiActive;
		BaseField baseField3 = base.Fields["stateString"];
		guiActive = (base.Fields["stateString"].guiActiveEditor = moduleIsEnabled);
		baseField3.guiActive = guiActive;
		base.Events["OnToggle"].guiActive = (base.Events["OnToggle"].guiActiveEditor = moduleIsEnabled);
		base.Actions["CycleAction"].active = moduleIsEnabled;
		base.Actions["Activate"].active = moduleIsEnabled;
		base.Actions["Deactivate"].active = moduleIsEnabled;
		base.Actions["Toggle"].active = moduleIsEnabled;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("WheelState"))
		{
			State = (WheelState)Enum.Parse(typeof(WheelState), node.GetValue("WheelState"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("WheelState", State.ToString());
	}

	public void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && base.part.started && moduleIsEnabled && !IsAdjusterBreakingReactionWheel())
		{
			switch (State)
			{
			case WheelState.Active:
				ActiveUpdate((VesselActuatorMode)actuatorModeCycle);
				break;
			case WheelState.Disabled:
			case WheelState.Broken:
				break;
			}
		}
	}

	public Vector3 GetAppliedTorque()
	{
		return inputVectorTemp;
	}

	public void ActiveUpdate(VesselActuatorMode mode)
	{
		if (!(base.vessel == null) && (base.vessel.Autopilot.Enabled || mode != VesselActuatorMode.const_1))
		{
			inputVectorTemp.x = base.vessel.ctrlState.pitch;
			inputVectorTemp.y = base.vessel.ctrlState.roll;
			inputVectorTemp.z = base.vessel.ctrlState.yaw;
			cmdInput = ((base.vessel == FlightGlobals.ActiveVessel) ? FlightInputHandler.state.GetPYR() : Vector3.zero);
			switch (mode)
			{
			case VesselActuatorMode.Pilot:
				inputVectorTemp = cmdInput;
				break;
			case VesselActuatorMode.const_1:
				inputVectorTemp -= cmdInput;
				break;
			}
			inputVectorTemp.x *= PitchTorque;
			inputVectorTemp.y *= RollTorque;
			inputVectorTemp.z *= YawTorque;
			inputVectorTemp *= authorityLimiter * 0.01f;
			Vector3 vector = ApplyTorqueAdjustments(inputVectorTemp);
			bool num = vector != inputVectorTemp;
			inputVectorTemp = vector;
			inputVector = Vector3.Lerp(inputVector, inputVectorTemp, torqueResponseSpeed * TimeWarp.fixedDeltaTime);
			inputSum = (Mathf.Abs(base.vessel.ctrlState.pitch) + Mathf.Abs(base.vessel.ctrlState.roll) + Mathf.Abs(base.vessel.ctrlState.yaw)) * (authorityLimiter * 0.01f);
			if (num && inputSum == 0f)
			{
				Vector3 vector2 = inputVectorTemp;
				vector2.x /= PitchTorque;
				vector2.y /= RollTorque;
				vector2.z /= YawTorque;
				inputSum = Mathf.Abs(vector2.x) + Mathf.Abs(vector2.y) + Mathf.Abs(vector2.z);
			}
			stateString = cacheAutoLOC_236147;
			if (inputSum > 0f)
			{
				operational = resHandler.UpdateModuleResourceInputs(ref stateString, inputSum, 0.9, returnOnFirstLack: false);
			}
			else
			{
				operational = false;
			}
			if (operational)
			{
				stateString = cacheAutoLOC_7001223;
				base.part.AddTorque(base.vessel.ReferenceTransform.rotation * -inputVector);
			}
		}
	}

	[KSPAction("#autoLOC_6001301")]
	public void CycleAction(KSPActionParam param)
	{
		actuatorModeCycle++;
		if (actuatorModeCycle > 2)
		{
			actuatorModeCycle = 0;
		}
	}

	[KSPAction("#autoLOC_6001302")]
	public void Activate(KSPActionParam param)
	{
		switch (State)
		{
		case WheelState.Disabled:
			State = WheelState.Active;
			break;
		case WheelState.Active:
		case WheelState.Broken:
			break;
		}
	}

	[KSPAction("#autoLOC_6001303")]
	public void Deactivate(KSPActionParam param)
	{
		switch (State)
		{
		case WheelState.Active:
			State = WheelState.Disabled;
			break;
		case WheelState.Disabled:
		case WheelState.Broken:
			break;
		}
	}

	[KSPAction("#autoLOC_6001304")]
	public void Toggle(KSPActionParam param)
	{
		OnToggle();
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001310")]
	public void OnToggle()
	{
		switch (State)
		{
		case WheelState.Active:
			State = WheelState.Disabled;
			break;
		case WheelState.Disabled:
			State = WheelState.Active;
			break;
		case WheelState.Broken:
			break;
		}
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_218709", PitchTorque.ToString("0.0###"));
		text += Localizer.Format("#autoLOC_218710", YawTorque.ToString("0.0###"));
		text += Localizer.Format("#autoLOC_218711", RollTorque.ToString("0.0###"));
		text += resHandler.PrintModuleResources();
		if (!moduleIsEnabled)
		{
			text += cacheAutoLOC_218716;
		}
		return text;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003052;
	}

	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		pos = (neg = Vector3.zero);
		if (moduleIsEnabled && wheelState == WheelState.Active && actuatorModeCycle != 2)
		{
			neg.x = (pos.x = PitchTorque);
			neg.y = (pos.y = RollTorque);
			neg.z = (pos.z = YawTorque);
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterReactionWheelBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterReactionWheelBase item = adjuster as AdjusterReactionWheelBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public bool IsAdjusterBreakingReactionWheel()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsReactionWheelBroken())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public Vector3 ApplyTorqueAdjustments(Vector3 torque)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			torque = adjusterCache[i].ApplyTorqueAdjustment(torque);
		}
		return torque;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_236147 = Localizer.Format("#autoLOC_236147");
		cacheAutoLOC_7001223 = Localizer.Format("#autoLOC_7001223");
		cacheAutoLOC_218716 = Localizer.Format("#autoLOC_218716");
		cacheAutoLOC_6003052 = Localizer.Format("#autoLoc_6003052");
	}
}
