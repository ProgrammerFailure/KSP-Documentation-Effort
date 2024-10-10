using UnityEngine;

public class MultiModeEngine : PartModule, IEngineStatus
{
	[KSPField]
	public string primaryEngineID = "Primary";

	[KSPField]
	public string secondaryEngineID = "Secondary";

	[KSPField]
	public string primaryEngineModeDisplayName = "";

	[KSPField]
	public string secondaryEngineModeDisplayName = "";

	[KSPField]
	public bool autoSwitchAvailable = true;

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001894")]
	public string mode = "Primary";

	[KSPField]
	public bool carryOverThrottle;

	[KSPField]
	public string engineID = "Multimode";

	public ModuleEnginesFX primaryEngine;

	public ModuleEnginesFX secondaryEngine;

	public ModuleEnginesFX tempEngine;

	public bool loadFailure;

	public static EventData<MultiModeEngine> onAutoSwitchModes;

	public MultiModeEngine leadModule;

	[KSPField(isPersistant = true)]
	public bool runningPrimary = true;

	[KSPField(isPersistant = true)]
	public bool autoSwitch = true;

	public ModuleEnginesFX PrimaryEngine => primaryEngine;

	public ModuleEnginesFX SecondaryEngine => secondaryEngine;

	public bool isOperational
	{
		get
		{
			if (runningPrimary)
			{
				return primaryEngine.isOperational;
			}
			return secondaryEngine.isOperational;
		}
	}

	public float normalizedOutput
	{
		get
		{
			if (runningPrimary)
			{
				return primaryEngine.normalizedOutput;
			}
			return secondaryEngine.normalizedOutput;
		}
	}

	public float throttleSetting
	{
		get
		{
			if (runningPrimary)
			{
				return primaryEngine.throttleSetting;
			}
			return secondaryEngine.throttleSetting;
		}
	}

	public string engineName => engineID;

	public void OnAutoSwitch(MultiModeEngine other)
	{
		if (!(other.vessel != base.vessel) && !(other.part.partInfo.name != base.part.partInfo.name))
		{
			if (leadModule == null)
			{
				leadModule = other;
			}
			if (!(other != leadModule) && autoSwitch && other.runningPrimary != runningPrimary)
			{
				ToggleMode();
			}
		}
	}

	public void onVesselModified(Vessel v)
	{
		if (v == base.vessel)
		{
			leadModule = null;
		}
	}

	[KSPAction("#autoLOC_6001300")]
	public void ModeAction(KSPActionParam param)
	{
		ModeEvent();
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001391")]
	public void DisableAutoSwitch()
	{
		autoSwitch = false;
		base.Events["DisableAutoSwitch"].active = false;
		base.Events["EnableAutoSwitch"].active = true;
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001392")]
	public void EnableAutoSwitch()
	{
		autoSwitch = true;
		base.Events["DisableAutoSwitch"].active = true;
		base.Events["EnableAutoSwitch"].active = false;
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001393")]
	public void ModeEvent()
	{
		DisableAutoSwitch();
		ToggleMode();
	}

	public void ToggleMode()
	{
		if (!runningPrimary)
		{
			SetPrimary(HighLogic.LoadedSceneIsFlight);
		}
		else
		{
			SetSecondary(HighLogic.LoadedSceneIsFlight);
		}
	}

	[KSPAction("#autoLOC_6001381")]
	public void ShutdownAction(KSPActionParam param)
	{
		if (runningPrimary)
		{
			primaryEngine.Shutdown();
		}
		else
		{
			secondaryEngine.Shutdown();
		}
	}

	[KSPAction("#autoLOC_6001382")]
	public void ActivateAction(KSPActionParam param)
	{
		if (runningPrimary)
		{
			primaryEngine.Activate();
		}
		else
		{
			secondaryEngine.Activate();
		}
	}

	[KSPAction("#autoLOC_6001380")]
	public void OnAction(KSPActionParam param)
	{
		if (runningPrimary)
		{
			if (primaryEngine.EngineIgnited)
			{
				primaryEngine.Shutdown();
			}
			else
			{
				primaryEngine.Activate();
			}
		}
		else if (secondaryEngine.EngineIgnited)
		{
			secondaryEngine.Shutdown();
		}
		else
		{
			secondaryEngine.Activate();
		}
	}

	public void SetPrimary()
	{
		SetPrimary(fireEvents: false);
	}

	public void SetPrimary(bool fireEvents)
	{
		if (loadFailure)
		{
			return;
		}
		if (fireEvents)
		{
			if (!primaryEngine.EngineIgnited && secondaryEngine.EngineIgnited)
			{
				primaryEngine.Activate();
			}
			if (carryOverThrottle)
			{
				primaryEngine.currentThrottle = secondaryEngine.currentThrottle;
			}
			if (secondaryEngine.EngineIgnited)
			{
				secondaryEngine.Shutdown();
			}
		}
		mode = (string.IsNullOrEmpty(primaryEngineModeDisplayName) ? primaryEngineID : primaryEngineModeDisplayName);
		primaryEngine.manuallyOverridden = false;
		primaryEngine.isEnabled = true;
		secondaryEngine.manuallyOverridden = true;
		secondaryEngine.isEnabled = false;
		runningPrimary = true;
		GameEvents.onMultiModeEngineSwitchActive.Fire(this);
	}

	public void SetSecondary()
	{
		SetSecondary(fireEvents: false);
	}

	public void SetSecondary(bool fireEvents)
	{
		if (loadFailure)
		{
			return;
		}
		if (fireEvents)
		{
			if (!secondaryEngine.EngineIgnited && primaryEngine.EngineIgnited)
			{
				secondaryEngine.Activate();
			}
			if (carryOverThrottle)
			{
				secondaryEngine.currentThrottle = primaryEngine.currentThrottle;
			}
			if (primaryEngine.EngineIgnited)
			{
				primaryEngine.Shutdown();
			}
		}
		mode = (string.IsNullOrEmpty(secondaryEngineModeDisplayName) ? secondaryEngineID : secondaryEngineModeDisplayName);
		primaryEngine.manuallyOverridden = true;
		primaryEngine.isEnabled = false;
		secondaryEngine.manuallyOverridden = false;
		secondaryEngine.isEnabled = true;
		runningPrimary = false;
		GameEvents.onMultiModeEngineSwitchActive.Fire(this);
	}

	public override void OnAwake()
	{
		if (onAutoSwitchModes == null)
		{
			onAutoSwitchModes = new EventData<MultiModeEngine>("onMultiModeEngineAutoSwitch");
		}
		onAutoSwitchModes.Add(OnAutoSwitch);
		GameEvents.onVesselWasModified.Add(onVesselModified);
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if (!autoSwitchAvailable)
		{
			autoSwitch = false;
			BaseEvent baseEvent = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent2 = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent3 = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent4 = base.Events["EnableAutoSwitch"];
			BaseEvent baseEvent5 = base.Events["EnableAutoSwitch"];
			base.Events["EnableAutoSwitch"].guiActiveEditor = false;
			baseEvent5.guiActive = false;
			baseEvent4.active = false;
			baseEvent3.guiActiveEditor = false;
			baseEvent2.guiActive = false;
			baseEvent.active = false;
		}
	}

	public void OnDestroy()
	{
		onAutoSwitchModes.Remove(OnAutoSwitch);
		GameEvents.onVesselWasModified.Remove(onVesselModified);
	}

	public override void OnStart(StartState state)
	{
		int i = 0;
		for (int count = base.part.Modules.Count; i < count; i++)
		{
			PartModule partModule = base.part.Modules[i];
			if (partModule is ModuleEnginesFX)
			{
				tempEngine = (ModuleEnginesFX)partModule;
				if (tempEngine.engineID == primaryEngineID)
				{
					primaryEngine = tempEngine;
				}
				else if (tempEngine.engineID == secondaryEngineID)
				{
					secondaryEngine = tempEngine;
				}
			}
		}
		tempEngine = null;
		if (primaryEngine != null && secondaryEngine != null)
		{
			for (int j = 0; j < secondaryEngine.Actions.Count; j++)
			{
				primaryEngine.Actions[j].active = false;
			}
			for (int k = 0; k < secondaryEngine.Actions.Count; k++)
			{
				primaryEngine.Actions[k].active = false;
			}
			if (runningPrimary)
			{
				SetPrimary();
			}
			else
			{
				SetSecondary();
			}
		}
		else
		{
			Debug.LogError("Failed to find primary and secondary engine, check engine IDs and module definitions");
			loadFailure = true;
		}
		if (!autoSwitchAvailable)
		{
			autoSwitch = false;
			BaseEvent baseEvent = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent2 = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent3 = base.Events["DisableAutoSwitch"];
			BaseEvent baseEvent4 = base.Events["EnableAutoSwitch"];
			BaseEvent baseEvent5 = base.Events["EnableAutoSwitch"];
			base.Events["EnableAutoSwitch"].guiActiveEditor = false;
			baseEvent5.guiActive = false;
			baseEvent4.active = false;
			baseEvent3.guiActiveEditor = false;
			baseEvent2.guiActive = false;
			baseEvent.active = false;
		}
		else if (autoSwitch)
		{
			base.Events["DisableAutoSwitch"].active = true;
			base.Events["EnableAutoSwitch"].active = false;
		}
		else
		{
			base.Events["DisableAutoSwitch"].active = false;
			base.Events["EnableAutoSwitch"].active = true;
		}
	}

	public override void OnUpdate()
	{
		if (loadFailure)
		{
			return;
		}
		if (leadModule != null)
		{
			if (!leadModule.autoSwitch)
			{
				leadModule = null;
			}
			else if (leadModule != this)
			{
				return;
			}
		}
		if (!autoSwitch)
		{
			return;
		}
		if (runningPrimary)
		{
			if (primaryEngine.flameout && secondaryEngine.CanStart())
			{
				SetSecondary(fireEvents: true);
				onAutoSwitchModes.Fire(this);
			}
		}
		else if (secondaryEngine.flameout && primaryEngine.CanStart())
		{
			SetPrimary(fireEvents: true);
			onAutoSwitchModes.Fire(this);
		}
	}
}
