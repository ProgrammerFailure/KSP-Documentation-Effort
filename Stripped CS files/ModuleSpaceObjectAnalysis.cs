using System.Collections.Generic;

public class ModuleSpaceObjectAnalysis : PartModule
{
	public Dictionary<string, ModuleAnalysisResource> analyzers = new Dictionary<string, ModuleAnalysisResource>();

	public ModuleAnalysisResource FindMatchingAnalyzer(Part p, string rName)
	{
		if (analyzers.TryGetValue(rName, out var value))
		{
			return value;
		}
		int count = p.Modules.Count;
		while (true)
		{
			if (count-- > 0)
			{
				PartModule partModule = p.Modules[count];
				if (partModule is ModuleAnalysisResource)
				{
					value = partModule as ModuleAnalysisResource;
					if (value.resourceName == rName)
					{
						break;
					}
				}
				continue;
			}
			analyzers.Add(rName, null);
			return null;
		}
		analyzers.Add(rName, value);
		return value;
	}
}
