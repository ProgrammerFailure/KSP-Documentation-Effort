using System.Runtime.CompilerServices;

public class ModuleResourceAutoShiftState : PartModule
{
	[KSPField(isPersistant = true)]
	public string affectedResourceName;

	[KSPField(isPersistant = true)]
	public string affectedModuleName;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "-")]
	[UI_Label]
	public string moduleAutoShiftName;

	private UI_Label moduleAutoShiftNameField;

	[UI_Toggle(disabledText = "#autoLOC_6001071", scene = UI_Scene.All, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005057")]
	public bool resourceShutOffStartUpUsePercent;

	[KSPField(isPersistant = true)]
	public bool resourceShutOffHandler;

	[KSPField(isPersistant = true)]
	public bool resourceStartUpHandler;

	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Shutoff amount")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5000f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float resourceShutOffAmount;

	private UI_FloatRange resourceShutOffAmountField;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Shutoff percent")]
	public float resourceShutOffPercent;

	private UI_FloatRange resourceShutOffPercentField;

	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto-startup amount")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 1f, maxValue = 5000f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float resourceStartUpAmount;

	private UI_FloatRange resourceStartUpAmountField;

	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "AutoStartStop", groupDisplayName = "#autoLOC_6005053", isPersistant = true, groupStartCollapsed = true, guiActive = false, guiActiveEditor = true, guiName = "Auto Startup percent")]
	public float resourceStartUpPercent;

	private UI_FloatRange resourceStartUpPercentField;

	public PartResource partResource;

	public ModuleResource resource;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResourceAutoShiftState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyStartUpPercent(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyShutOffAmount(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyStartUpAmount(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyShutOffPercent(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyAmountPercent(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleWarningVisibility(bool state)
	{
		throw null;
	}
}
