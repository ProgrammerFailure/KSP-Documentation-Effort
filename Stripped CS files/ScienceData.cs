public class ScienceData : IConfigNode
{
	public float labValue;

	public float dataAmount;

	public float scienceValueRatio;

	public float baseTransmitValue;

	public float transmitBonus;

	public string subjectID;

	public string title;

	public bool triggered;

	public uint container;

	public string extraResultString;

	public ScienceData(ConfigNode node)
	{
		Load(node);
	}

	public ScienceData(float amount, float xmitValue, float xmitBonus, string id, string dataName, bool triggered = false, uint container = 0u)
		: this(amount, 1f, xmitValue, xmitBonus, id, dataName, triggered, container)
	{
	}

	public ScienceData(float amount, float scienceValueRatio, float xmitValue, float xmitBonus, string id, string dataName, bool triggered = false, uint container = 0u, string extraResultString = "")
	{
		dataAmount = amount;
		subjectID = id;
		baseTransmitValue = xmitValue;
		transmitBonus = xmitBonus;
		title = dataName;
		this.triggered = triggered;
		this.container = container;
		this.scienceValueRatio = scienceValueRatio;
		this.extraResultString = extraResultString;
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("data", ref dataAmount);
		scienceValueRatio = 1f;
		node.TryGetValue("scienceValueRatio", ref scienceValueRatio);
		node.TryGetValue("subjectID", ref subjectID);
		node.TryGetValue("xmit", ref baseTransmitValue);
		node.TryGetValue("xmitBonus", ref transmitBonus);
		ScienceExperiment scienceExperiment = new ScienceExperiment();
		if (!subjectID.Contains("@"))
		{
			scienceExperiment = ResearchAndDevelopment.GetExperiment(subjectID);
		}
		if (scienceExperiment.id != null)
		{
			title = ResearchAndDevelopment.GetExperiment(subjectID).experimentTitle;
		}
		else
		{
			title = ScienceUtil.GenerateLocalizedTitle(subjectID, skipAsteroidsComets: true);
			if (title == string.Empty)
			{
				node.TryGetValue("title", ref title);
			}
		}
		node.TryGetValue("triggered", ref triggered);
		node.TryGetValue("container", ref container);
		node.TryGetValue("extraResultString", ref extraResultString);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("data", dataAmount);
		node.AddValue("scienceValueRatio", scienceValueRatio);
		node.AddValue("subjectID", subjectID);
		node.AddValue("xmit", baseTransmitValue);
		node.AddValue("xmitBonus", transmitBonus);
		node.AddValue("title", title);
		node.AddValue("triggered", triggered);
		node.AddValue("container", container);
		if (!string.IsNullOrEmpty(extraResultString))
		{
			node.AddValue("extraResultString", extraResultString);
		}
	}

	public static ScienceData CopyOf(ScienceData src)
	{
		if (src != null)
		{
			return new ScienceData(src.dataAmount, src.scienceValueRatio, src.baseTransmitValue, src.transmitBonus, src.subjectID, src.title, src.triggered, src.container, src.extraResultString);
		}
		return null;
	}
}
