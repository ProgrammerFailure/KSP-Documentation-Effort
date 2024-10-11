using System.Runtime.CompilerServices;

public class ModuleHeatEffects : PartModuleFXSetter
{
	public double draperPoint;

	[KSPField]
	public double lerpMin;

	[KSPField]
	public double lerpOffset;

	[KSPField]
	public double lerpMax;

	[KSPField]
	public double lerpScalar;

	public double lerpDivRecip;

	[KSPField]
	public bool useSkinTemp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleHeatEffects()
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
	public void HeatEffectStartup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateHeatEffect()
	{
		throw null;
	}
}
