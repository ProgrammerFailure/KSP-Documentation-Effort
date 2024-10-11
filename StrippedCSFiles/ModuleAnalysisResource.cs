using System.Runtime.CompilerServices;

public class ModuleAnalysisResource : PartModule
{
	[KSPField(isPersistant = true)]
	public float abundance;

	[KSPField(isPersistant = true)]
	public string resourceName;

	[KSPField(isPersistant = true)]
	public float displayAbundance;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public string status;

	protected BaseField statusFld;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAnalysisResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupAnalysis()
	{
		throw null;
	}
}
