using ns9;
using Steamworks;

namespace Expansions.Missions;

public class SteamMissionFileInfo
{
	public SteamUGCDetails_t itemDetails;

	public ulong totalSubscriptions;

	public ulong totalFavorites;

	public ulong totalFollowers;

	public string previewURL;

	public ConfigNode metaData;

	public MissionDifficulty missionDifficulty;

	public string modsBriefing;

	public string packName;

	public string packDisplayName;

	public SteamMissionFileInfo(SteamUGCDetails_t itemDetails)
	{
		this.itemDetails = itemDetails;
		metaData = new ConfigNode();
		missionDifficulty = MissionDifficulty.Beginner;
		totalSubscriptions = 0uL;
		totalFavorites = 0uL;
		totalFollowers = 0uL;
		previewURL = "";
		modsBriefing = "";
		packName = "";
		packDisplayName = "";
	}

	public void ProcessMetaData(string metaData)
	{
		if (!string.IsNullOrEmpty(metaData))
		{
			ConfigNode configNode = ConfigNode.Parse(metaData);
			if (configNode.HasNode("MissionMetadata"))
			{
				this.metaData = configNode.GetNode("MissionMetadata");
				bool value = false;
				this.metaData.TryGetValue("hardIcon", ref value);
				this.metaData.TryGetEnum("difficulty", ref missionDifficulty, value ? MissionDifficulty.Advanced : MissionDifficulty.Beginner);
				this.metaData.TryGetValue("modsBriefing", ref modsBriefing);
				this.metaData.TryGetValue("packName", ref packName);
				packDisplayName = packName;
				this.metaData.TryGetValue("packDisplayName", ref packDisplayName);
			}
		}
	}

	public static string CreateMetaData(Mission mission)
	{
		ConfigNode configNode = new ConfigNode("MissionMetadata");
		configNode.AddValue("packName", mission.packName);
		for (int i = 0; i < MissionsUtils.MissionPacks.Count; i++)
		{
			if (MissionsUtils.MissionPacks.ValuesList[i].name == mission.packName)
			{
				configNode.AddValue("packDisplayName", Localizer.Format(MissionsUtils.MissionPacks.ValuesList[i].displayName));
			}
		}
		configNode.AddValue("difficulty", mission.difficulty);
		configNode.AddValue("modsBriefing", mission.modsBriefing);
		configNode.AddValue("hardIcon", mission.hardIcon);
		return configNode.ToString();
	}

	public void SetMissionFileInfo(ref MissionFileInfo missionFileInfo)
	{
		missionFileInfo.difficulty = missionDifficulty;
		missionFileInfo.modsBriefing = modsBriefing;
		missionFileInfo.packName = packName;
	}
}
