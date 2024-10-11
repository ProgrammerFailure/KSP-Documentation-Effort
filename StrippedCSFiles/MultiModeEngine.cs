using System.Runtime.CompilerServices;

public class MultiModeEngine : PartModule, IEngineStatus
{
	[KSPField]
	public string primaryEngineID;

	[KSPField]
	public string secondaryEngineID;

	[KSPField]
	public string primaryEngineModeDisplayName;

	[KSPField]
	public string secondaryEngineModeDisplayName;

	[KSPField]
	public bool autoSwitchAvailable;

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001894")]
	public string mode;

	[KSPField]
	public bool carryOverThrottle;

	[KSPField]
	public string engineID;

	private ModuleEnginesFX primaryEngine;

	private ModuleEnginesFX secondaryEngine;

	private ModuleEnginesFX tempEngine;

	private bool loadFailure;

	private static EventData<MultiModeEngine> onAutoSwitchModes;

	private MultiModeEngine leadModule;

	[KSPField(isPersistant = true)]
	public bool runningPrimary;

	[KSPField(isPersistant = true)]
	public bool autoSwitch;

	public ModuleEnginesFX PrimaryEngine
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleEnginesFX SecondaryEngine
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isOperational
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float normalizedOutput
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float throttleSetting
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string engineName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MultiModeEngine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAutoSwitch(MultiModeEngine other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001300")]
	public void ModeAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001391")]
	public void DisableAutoSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001392")]
	public void EnableAutoSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001393")]
	public void ModeEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001381")]
	public void ShutdownAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001382")]
	public void ActivateAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001380")]
	public void OnAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPrimary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPrimary(bool fireEvents)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSecondary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSecondary(bool fireEvents)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}
