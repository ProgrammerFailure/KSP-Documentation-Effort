using System.Collections.Generic;

namespace ns11;

public static class RDEnvironmentAdapter
{
	public static ScienceExperiment GetExperiment(string key)
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetExperiment(key);
		}
		return RDTestSceneLoader.GetExperiment(key);
	}

	public static List<string> GetExperimentIDs()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetExperimentIDs();
		}
		return RDTestSceneLoader.GetExperimentIDs();
	}

	public static List<string> GetSituationTagsDescriptions()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetSituationTagsDescriptions();
		}
		return RDTestSceneLoader.GetSituationTagsDescriptions();
	}

	public static List<string> GetSituationTags()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetSituationTags();
		}
		return RDTestSceneLoader.GetSituationTags();
	}

	public static List<ScienceSubject> GetSubjects()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetSubjects();
		}
		return RDTestSceneLoader.Instance.subjects;
	}

	public static List<string> GetBiomeTagsLocalized(CelestialBody cb, bool includeMiniBiomes)
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetBiomeTagsLocalized(cb, includeMiniBiomes);
		}
		return RDTestSceneLoader.GetBiomeTagsLocalized(cb, includeMiniBiomes);
	}

	public static List<string> GetBiomeTags(CelestialBody cb, bool includeMiniBiomes)
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetBiomeTags(cb, includeMiniBiomes);
		}
		return RDTestSceneLoader.GetBiomeTags(cb, includeMiniBiomes);
	}

	public static string GetResults(string subjectID)
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			return ResearchAndDevelopment.GetResults(subjectID);
		}
		return RDTestSceneLoader.GetResults(subjectID);
	}
}
