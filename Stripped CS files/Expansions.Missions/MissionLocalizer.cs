using System.Collections.Generic;
using System.IO;

namespace Expansions.Missions;

public class MissionLocalizer
{
	public static List<ConfigNode> LoadMissionLocalizationFiles()
	{
		List<ConfigNode> list = new List<ConfigNode>();
		if (Directory.Exists(MissionsUtils.GetMissionsFolder(MissionTypes.User)))
		{
			string[] directories = Directory.GetDirectories(MissionsUtils.GetMissionsFolder(MissionTypes.User));
			for (int i = 0; i < directories.Length; i++)
			{
				if (!Directory.Exists(directories[i] + "/Localization"))
				{
					continue;
				}
				string[] files = Directory.GetFiles(directories[i] + "/Localization/", "*.cfg");
				for (int j = 0; j < files.Length; j++)
				{
					ConfigNode[] nodes = ConfigNode.Load(files[j]).GetNodes("Localization");
					for (int k = 0; k < nodes.Length; k++)
					{
						list.Add(nodes[k]);
					}
				}
			}
		}
		return list;
	}
}
