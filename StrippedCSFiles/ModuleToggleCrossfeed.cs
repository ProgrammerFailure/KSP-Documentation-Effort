using System.Runtime.CompilerServices;

public class ModuleToggleCrossfeed : PartModule, IToggleCrossfeed
{
	[KSPField(isPersistant = true)]
	public bool crossfeedStatus;

	[KSPField]
	public bool eventPropagatesInEditor;

	[KSPField]
	public bool eventPropagatesInFlight;

	[KSPField]
	public bool toggleEditor;

	[KSPField]
	public bool toggleFlight;

	[KSPField]
	public string enableText;

	[KSPField]
	public string disableText;

	[KSPField]
	public string toggleText;

	[KSPField]
	public string techRequired;

	private BaseEvent toggleE;

	private BaseAction toggleA;

	private BaseAction enableA;

	private BaseAction disableA;

	public bool defaultCrossfeedStatus;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleToggleCrossfeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236030")]
	public void ToggleEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236032")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleAction(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236028")]
	public void EnableAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_236030")]
	public void DisableAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
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
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCrossfeed(bool fireEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedToggleableEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedToggleableFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedRequiresTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string CrossfeedTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CrossfeedHasTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
