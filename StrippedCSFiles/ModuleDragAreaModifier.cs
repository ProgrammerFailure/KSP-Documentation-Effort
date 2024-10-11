using System.Runtime.CompilerServices;

public class ModuleDragAreaModifier : PartModule
{
	[KSPField]
	public string dragCubeName;

	[KSPField]
	public float areaModifier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDragAreaModifier()
	{
		throw null;
	}
}
