using System.Runtime.CompilerServices;

public class ModuleConductionMultiplier : PartModule
{
	[KSPField]
	public double modifiedConductionFactor;

	[KSPField]
	public double convectionFluxThreshold;

	private double originalConduction;

	private double currentConduction;

	private double threshRecip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleConductionMultiplier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}
