using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleSpaceObjectAnalysis : PartModule
{
	protected Dictionary<string, ModuleAnalysisResource> analyzers;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSpaceObjectAnalysis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ModuleAnalysisResource FindMatchingAnalyzer(Part p, string rName)
	{
		throw null;
	}
}
