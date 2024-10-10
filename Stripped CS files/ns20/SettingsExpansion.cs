using Expansions;

namespace ns20;

public static class SettingsExpansion
{
	public enum Expansion
	{
		Everything = -1,
		Nothing = 0,
		MakingHistory = 2
	}

	public static bool IsExpansion(Expansion expansion)
	{
		switch (expansion)
		{
		case Expansion.Everything:
			return true;
		case Expansion.Nothing:
			return ExpansionsLoader.GetInstalledExpansions().Count < 1;
		default:
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && (expansion & Expansion.MakingHistory) != 0)
			{
				return true;
			}
			return false;
		}
	}
}
