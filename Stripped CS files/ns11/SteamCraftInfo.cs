using Steamworks;

namespace ns11;

public class SteamCraftInfo
{
	public SteamUGCDetails_t itemDetails;

	public ulong totalSubscriptions;

	public ulong totalFavorites;

	public ulong totalFollowers;

	public string previewURL;

	public ConfigNode metaData;

	public string modsBriefing;

	public string vesselType;

	public float mass;

	public int partCount;

	public int stageCount;

	public float cost;

	public int crewCapacity;

	public string KSPversion;

	public int steamState;

	public bool subscribed;

	public bool canBeUsed;

	public string steamStateText;

	public SteamCraftInfo(SteamUGCDetails_t itemDetails)
	{
		this.itemDetails = itemDetails;
		metaData = new ConfigNode();
		totalSubscriptions = 0uL;
		totalFavorites = 0uL;
		totalFollowers = 0uL;
		previewURL = "";
		modsBriefing = "";
		vesselType = "";
		mass = 0f;
		cost = 0f;
		partCount = 0;
		stageCount = 0;
		crewCapacity = 0;
		steamState = 0;
		canBeUsed = false;
		steamStateText = "";
		if (SteamManager.Initialized)
		{
			steamState = SteamManager.Instance.GetItemState(itemDetails.m_nPublishedFileId, out steamStateText, out canBeUsed, out subscribed);
		}
		KSPversion = Versioning.version_major + "." + Versioning.version_minor + "." + Versioning.Revision;
	}

	public void ProcessMetaData(string metaData)
	{
		if (!string.IsNullOrEmpty(metaData))
		{
			ConfigNode configNode = ConfigNode.Parse(metaData);
			if (configNode.HasNode("CraftMetadata"))
			{
				this.metaData = configNode.GetNode("CraftMetadata");
				this.metaData.TryGetValue("modsBriefing", ref modsBriefing);
				this.metaData.TryGetValue("vesselType", ref vesselType);
				this.metaData.TryGetValue("mass", ref mass);
				this.metaData.TryGetValue("cost", ref cost);
				this.metaData.TryGetValue("partCount", ref partCount);
				this.metaData.TryGetValue("stageCount", ref stageCount);
				this.metaData.TryGetValue("crewCapacity", ref crewCapacity);
				this.metaData.TryGetValue("KSPversion", ref KSPversion);
			}
		}
	}

	public static string CreateMetaData(string modsBriefing, string vesselType, float mass, float cost, int partCount, int stageCount, int crewCapacity)
	{
		ConfigNode configNode = new ConfigNode("CraftMetadata");
		configNode.AddValue("modsBriefing", modsBriefing);
		configNode.AddValue("vesselType", vesselType);
		configNode.AddValue("mass", mass);
		configNode.AddValue("cost", cost);
		configNode.AddValue("partCount", partCount);
		configNode.AddValue("stageCount", stageCount);
		configNode.AddValue("crewCapacity", crewCapacity);
		configNode.AddValue("KSPversion", Versioning.version_major + "." + Versioning.version_minor + "." + Versioning.Revision);
		return configNode.ToString();
	}

	public void UpdateSteamState()
	{
		if (SteamManager.Initialized)
		{
			steamState = SteamManager.Instance.GetItemState(itemDetails.m_nPublishedFileId, out steamStateText, out canBeUsed, out subscribed);
		}
	}
}
