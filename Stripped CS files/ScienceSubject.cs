using System;
using ns9;

[Serializable]
public class ScienceSubject : IConfigNode
{
	public string id;

	public string title;

	public float dataScale;

	public float scientificValue;

	public bool applyScienceScale;

	public float subjectValue;

	public float scienceCap;

	public float science;

	public ScienceSubject(string id, string title, float dataScale, float subjectValue, float scienceCap)
		: this(id, title, dataScale, subjectValue, scienceCap, applyScienceScale: true)
	{
	}

	public ScienceSubject(string id, string title, float dataScale, float subjectValue, float scienceCap, bool applyScienceScale)
	{
		this.id = id;
		this.title = title;
		this.dataScale = dataScale;
		this.subjectValue = subjectValue;
		this.scienceCap = scienceCap;
		science = 0f;
		scientificValue = 1f;
		this.applyScienceScale = applyScienceScale;
	}

	public ScienceSubject(ScienceExperiment exp, ExperimentSituations sit, CelestialBody body, string biome = "", string displaybiome = "")
	{
		if (biome == null)
		{
			biome = "";
		}
		if (displaybiome == null)
		{
			displaybiome = "";
		}
		if (biome.Contains("#autoLOC"))
		{
			biome = Localizer.Format(biome);
		}
		if (displaybiome.Contains("#autoLOC"))
		{
			displaybiome = Localizer.Format(displaybiome);
		}
		id = exp.id + "@" + body.name + sit.ToString() + biome.Replace(" ", string.Empty);
		title = ScienceUtil.GenerateScienceSubjectTitle(exp, sit, body, biome, displaybiome);
		switch (sit)
		{
		case ExperimentSituations.FlyingHigh:
			subjectValue = body.scienceValues.FlyingHighDataValue;
			break;
		case ExperimentSituations.SrfLanded:
			subjectValue = body.scienceValues.LandedDataValue;
			break;
		case ExperimentSituations.SrfSplashed:
			subjectValue = body.scienceValues.SplashedDataValue;
			break;
		case ExperimentSituations.FlyingLow:
			subjectValue = body.scienceValues.FlyingLowDataValue;
			break;
		default:
			subjectValue = 1f;
			break;
		case ExperimentSituations.InSpaceHigh:
			subjectValue = body.scienceValues.InSpaceHighDataValue;
			break;
		case ExperimentSituations.InSpaceLow:
			subjectValue = body.scienceValues.InSpaceLowDataValue;
			break;
		}
		dataScale = exp.dataScale;
		scientificValue = 1f;
		scienceCap = exp.scienceCap * subjectValue;
		applyScienceScale = exp.applyScienceScale;
		science = 0f;
	}

	public ScienceSubject(ScienceExperiment exp, ExperimentSituations sit, string sourceUid, string sourceTitle, CelestialBody body, string biome = "", string displaybiome = "")
	{
		if (biome == null)
		{
			biome = "";
		}
		if (displaybiome == null)
		{
			displaybiome = "";
		}
		if (biome.Contains("#autoLOC"))
		{
			biome = Localizer.Format(biome);
		}
		if (displaybiome.Contains("#autoLOC"))
		{
			displaybiome = Localizer.Format(displaybiome);
		}
		else if (displaybiome != string.Empty)
		{
			displaybiome = ScienceUtil.GetBiomedisplayName(body, displaybiome);
		}
		id = exp.id + "@" + body.name + sit.ToString() + ((sourceUid != "") ? (biome.Replace(" ", string.Empty) + "_" + sourceUid) : "");
		title = ScienceUtil.GenerateScienceSubjectTitle(exp, sit, sourceUid, sourceTitle, body, biome, displaybiome);
		subjectValue = 1f;
		dataScale = exp.dataScale;
		scientificValue = 1f;
		scienceCap = exp.scienceCap * subjectValue;
		science = 0f;
		applyScienceScale = exp.applyScienceScale;
	}

	public ScienceSubject(ConfigNode node)
	{
		Load(node);
	}

	public bool IsFromBody(CelestialBody cb)
	{
		return id.Contains("@" + cb.name);
	}

	public bool IsFromSituation(ExperimentSituations situation)
	{
		return id.Contains(situation.ToString());
	}

	public bool HasPartialIDstring(string pId)
	{
		return id.Contains(pId);
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("id"))
		{
			id = node.GetValue("id");
		}
		title = ScienceUtil.GenerateLocalizedTitle(id);
		if (title == string.Empty)
		{
			node.TryGetValue("title", ref title);
		}
		node.TryGetValue("dsc", ref dataScale);
		node.TryGetValue("scv", ref scientificValue);
		node.TryGetValue("sbv", ref subjectValue);
		node.TryGetValue("sci", ref science);
		applyScienceScale = true;
		node.TryGetValue("asc", ref applyScienceScale);
		node.TryGetValue("cap", ref scienceCap);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", id);
		node.AddValue("title", title);
		node.AddValue("dsc", dataScale);
		node.AddValue("scv", scientificValue);
		node.AddValue("sbv", subjectValue);
		node.AddValue("sci", science);
		node.AddValue("asc", applyScienceScale);
		node.AddValue("cap", scienceCap);
	}
}
