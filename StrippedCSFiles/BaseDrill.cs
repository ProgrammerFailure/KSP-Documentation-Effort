using System.Runtime.CompilerServices;

public class BaseDrill : BaseConverter
{
	[KSPField]
	public float ImpactRange;

	[KSPField]
	public string ImpactTransform;

	[KSPField]
	public float Efficiency;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseDrill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
