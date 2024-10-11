using System.Runtime.CompilerServices;

public class ModuleOrbitalScanner : PartModule, IAnimatedModule
{
	[KSPField]
	public bool CheckForLock;

	protected BaseEvent switchR;

	protected BaseEvent toggleO;

	protected BaseEvent setCol;

	protected bool _ShowSurveyMenu;

	protected bool _ShowOverlayMenu;

	private static string cacheAutoLOC_6001054;

	private static string cacheAutoLOC_6002358;

	private static string cacheAutoLOC_7003276;

	private static string cacheAutoLOC_7003277;

	private static string cacheAutoLOC_7003278;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleOrbitalScanner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001479")]
	public virtual void SwitchResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001480")]
	public virtual void ToggleOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void GenerateOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void HideOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001481")]
	public virtual void SetColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetOverlayColor(MapDisplayTypes dispType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001483")]
	public virtual void IncreaseCutoff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001484")]
	public virtual void DecreaseCutoff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001485")]
	public virtual void ActivateScanner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ToggleSurvey(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ToggleMenu(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ShowResourceName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EnableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ModuleIsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsSituationValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnResourceMapLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
