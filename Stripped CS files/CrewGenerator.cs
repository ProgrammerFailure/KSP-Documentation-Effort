using System;
using Expansions.Missions;
using Expansions.Missions.Editor;
using ns9;

public class CrewGenerator
{
	public static string[] kerbalNamesMale;

	public static string[] kerbalNamesFemale;

	public static int specialNameChance = 20;

	public static string crewLastName;

	public static string[] specialNamesMale;

	public static string[] specialNamesFemale;

	public static string[] KerbalNamesMale => kerbalNamesMale;

	public static string[] KerbalNamesFemale => kerbalNamesFemale;

	public static string[] SpecialNamesMale => specialNamesMale;

	public static string[] SpecialNamesFemale => specialNamesFemale;

	public static void Initialize()
	{
		LoadNameArray(ref kerbalNamesMale, "KerbalNames", ProtoCrewMember.Gender.Male);
		LoadNameArray(ref kerbalNamesFemale, "KerbalNames", ProtoCrewMember.Gender.Female);
		LoadNameArray(ref specialNamesMale, "KerbalSpecialNames", ProtoCrewMember.Gender.Male);
		LoadNameArray(ref specialNamesFemale, "KerbalSpecialNames", ProtoCrewMember.Gender.Female);
		crewLastName = Localizer.Format("#autoLoc_6003018");
	}

	public static ProtoCrewMember RandomCrewMemberPrototype(ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew)
	{
		KSPRandom kSPRandom = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
		ProtoCrewMember.Gender gender = ((kSPRandom.Next(2) > 0) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male);
		ProtoCrewMember protoCrewMember = new ProtoCrewMember(type, GetRandomName(gender, kSPRandom));
		protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
		kSPRandom = new KSPRandom(protoCrewMember.name.GetHashCode_Net35());
		protoCrewMember.courage = (float)kSPRandom.NextDouble();
		protoCrewMember.stupidity = (float)kSPRandom.NextDouble();
		protoCrewMember.isBadass = kSPRandom.Next(10) == 0;
		protoCrewMember.hasToured = false;
		protoCrewMember.gender = gender;
		KerbalRoster.SetExperienceTrait(protoCrewMember);
		if (MissionEditorLogic.Instance != null)
		{
			int maxValue = 6;
			if (MissionEditorLogic.Instance.EditorMission.situation.gameParameters.CustomParams<MissionParamsGeneral>().enableKerbalLevels)
			{
				switch (protoCrewMember.trait)
				{
				case "Scientist":
					maxValue = MissionEditorLogic.Instance.EditorMission.situation.gameParameters.CustomParams<MissionParamsGeneral>().kerbalLevelScientist;
					break;
				case "Engineer":
					maxValue = MissionEditorLogic.Instance.EditorMission.situation.gameParameters.CustomParams<MissionParamsGeneral>().kerbalLevelEngineer;
					break;
				case "Pilot":
					maxValue = MissionEditorLogic.Instance.EditorMission.situation.gameParameters.CustomParams<MissionParamsGeneral>().kerbalLevelPilot;
					break;
				}
			}
			protoCrewMember.experienceLevel = kSPRandom.Next(maxValue);
			KerbalRoster.SetExperienceLevel(protoCrewMember, protoCrewMember.experienceLevel, MissionEditorLogic.Instance.EditorMission.situation.gameParameters, Game.Modes.MISSION);
		}
		return protoCrewMember;
	}

	public static string GetRandomName(ProtoCrewMember.Gender g, Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		string[] array;
		string[] array2;
		if (g != 0 && g == ProtoCrewMember.Gender.Female)
		{
			array = specialNamesFemale;
			array2 = kerbalNamesFemale;
		}
		else
		{
			array = specialNamesMale;
			array2 = kerbalNamesMale;
		}
		string empty = string.Empty;
		string fullName;
		bool flag;
		do
		{
			empty = (((kSPRandom?.Next(specialNameChance) ?? generator.Next(specialNameChance)) != 0) ? array2[kSPRandom?.Next(array2.Length) ?? generator.Next(array2.Length)] : array[kSPRandom?.Next(array.Length) ?? generator.Next(array.Length)]);
			fullName = GetFullName(empty, crewLastName);
			if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.CrewRoster == null)
			{
				break;
			}
			KerbalRoster crewRoster = HighLogic.CurrentGame.CrewRoster;
			flag = false;
			for (int i = 0; i < crewRoster.Count; i++)
			{
				if (crewRoster[i].name.Contains(empty))
				{
					flag = true;
				}
			}
		}
		while (flag);
		return fullName;
	}

	public static string GetFullName(string name, string lastName)
	{
		string languageIdFromFile = Localizer.GetLanguageIdFromFile();
		if (languageIdFromFile == "ja")
		{
			return name + crewLastName;
		}
		return name + " " + crewLastName;
	}

	public static bool LoadNameArray(ref string[] nameArray, string filePrefix, ProtoCrewMember.Gender gender)
	{
		ConfigNode configNode = ConfigNode.LoadFromTextAssetResource("TextAssets/" + GetFileName(filePrefix, gender));
		nameArray = configNode.GetNode(filePrefix + gender).GetValues("Element");
		configNode.ClearData();
		return true;
	}

	public static string GetFileName(string filePrefix, ProtoCrewMember.Gender gender)
	{
		string languageIdFromFile = Localizer.GetLanguageIdFromFile();
		return string.Concat(string.Concat("CrewGenerator/" + languageIdFromFile + "/", filePrefix), gender.ToString());
	}

	public static string GetLastName()
	{
		string value = "Kerman";
		Localizer.TryGetStringByTag("#autoLoc_6003018", out value);
		return value;
	}

	public static string RemoveLastName(string fullKerbalName)
	{
		fullKerbalName = fullKerbalName.Replace(" " + GetLastName(), string.Empty);
		return fullKerbalName;
	}
}
