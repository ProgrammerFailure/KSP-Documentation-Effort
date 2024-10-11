using System.Runtime.CompilerServices;

public class ModuleOrbitalSurveyor : PartModule, IAnimatedModule
{
	[KSPField]
	public double minThreshold;

	[KSPField]
	public double maxThreshold;

	[KSPField]
	public bool useCustomThresholds;

	[KSPField]
	public int ScanTime;

	[KSPField]
	public int SciBonus;

	private BaseEvent perfSurv;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleOrbitalSurveyor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001486")]
	public virtual void PerformSurvey()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void rescaleThresholds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void sendDataToComms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CompleteSurvey(ScienceData data, Vessel origin, bool xmitAborted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckForScanData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
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
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
