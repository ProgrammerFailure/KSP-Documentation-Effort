public class ModuleAsteroidAnalysis : ModuleSpaceObjectAnalysis
{
	public Part potatoPart;

	public override void OnStart(StartState state)
	{
		GameEvents.onVesselWasModified.Add(CheckForPotato);
		if (HighLogic.LoadedSceneIsFlight)
		{
			CheckForPotato(base.vessel);
		}
	}

	public void OnDestroy()
	{
		analyzers.Clear();
		GameEvents.onVesselWasModified.Remove(CheckForPotato);
	}

	public void CheckForPotato(Vessel v)
	{
		ModuleAsteroid moduleAsteroid = v.FindPartModuleImplementing<ModuleAsteroid>();
		if (moduleAsteroid != null)
		{
			FindAsteroidResources(moduleAsteroid.part);
		}
		else
		{
			ClearAsteroidResources();
		}
	}

	public void ClearAsteroidResources()
	{
		Part part = base.part;
		if (potatoPart != null)
		{
			part = potatoPart;
			potatoPart = null;
			ClearAsteroidResources();
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

	public void FindAsteroidResources(Part p)
	{
		if (potatoPart != null && potatoPart != p)
		{
			ClearAsteroidResources();
		}
		potatoPart = p;
		analyzers.Clear();
		int count = p.Modules.Count;
		for (int i = 0; i < count; i++)
		{
			PartModule partModule = p.Modules[i];
			if (partModule is ModuleAsteroidResource)
			{
				ModuleAsteroidResource moduleAsteroidResource = partModule as ModuleAsteroidResource;
				ModuleAnalysisResource moduleAnalysisResource = FindMatchingAnalyzer(base.part, moduleAsteroidResource.resourceName);
				if (moduleAnalysisResource != null)
				{
					moduleAnalysisResource.abundance = moduleAsteroidResource.abundance;
					moduleAnalysisResource.displayAbundance = moduleAsteroidResource.displayAbundance;
				}
			}
		}
	}
}
