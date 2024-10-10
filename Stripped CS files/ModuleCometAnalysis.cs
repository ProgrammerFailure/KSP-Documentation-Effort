public class ModuleCometAnalysis : ModuleSpaceObjectAnalysis
{
	public Part cometPart;

	public override void OnStart(StartState state)
	{
		GameEvents.onVesselWasModified.Add(CheckForComet);
		if (HighLogic.LoadedSceneIsFlight)
		{
			CheckForComet(base.vessel);
		}
	}

	public void OnDestroy()
	{
		analyzers.Clear();
		GameEvents.onVesselWasModified.Remove(CheckForComet);
	}

	public void CheckForComet(Vessel v)
	{
		ModuleComet moduleComet = v.FindPartModuleImplementing<ModuleComet>();
		if (moduleComet != null)
		{
			FindCometResources(moduleComet.part);
		}
		else
		{
			ClearCometResources();
		}
	}

	public void ClearCometResources()
	{
		Part part = base.part;
		if (cometPart != null)
		{
			part = cometPart;
			cometPart = null;
			ClearCometResources();
		}
		int count = part.Modules.Count;
		while (count-- > 0)
		{
			PartModule partModule = part.Modules[count];
			if (partModule is ModuleAnalysisResource)
			{
				ModuleAnalysisResource obj = partModule as ModuleAnalysisResource;
				obj.displayAbundance = 0f;
				obj.abundance = 0f;
			}
		}
		analyzers.Clear();
	}

	public void FindCometResources(Part p)
	{
		if (cometPart != null && cometPart != p)
		{
			ClearCometResources();
		}
		cometPart = p;
		analyzers.Clear();
		int count = p.Modules.Count;
		for (int i = 0; i < count; i++)
		{
			PartModule partModule = p.Modules[i];
			if (partModule is ModuleCometResource)
			{
				ModuleCometResource moduleCometResource = partModule as ModuleCometResource;
				ModuleAnalysisResource moduleAnalysisResource = FindMatchingAnalyzer(base.part, moduleCometResource.resourceName);
				if (moduleAnalysisResource != null)
				{
					moduleAnalysisResource.abundance = moduleCometResource.abundance;
					moduleAnalysisResource.displayAbundance = moduleCometResource.displayAbundance;
				}
			}
		}
	}
}
