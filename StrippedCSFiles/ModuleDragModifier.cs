using System.Runtime.CompilerServices;

public class ModuleDragModifier : PartModule
{
	[KSPField]
	public string dragCubeName;

	[KSPField]
	public float dragModifier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDragModifier()
	{
		throw null;
	}
}
