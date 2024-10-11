using System.Runtime.CompilerServices;

public class ModuleSpaceObjectResource : PartModule
{
	[KSPField(isPersistant = true)]
	public float abundance;

	[KSPField(isPersistant = true)]
	public float displayAbundance;

	[KSPField]
	public int highRange;

	[KSPField]
	public int lowRange;

	[KSPField]
	public int presenceChance;

	[KSPField]
	public string resourceName;

	[KSPField]
	public string FlowMode;

	public ResourceFlowMode _flowMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSpaceObjectResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}
}
