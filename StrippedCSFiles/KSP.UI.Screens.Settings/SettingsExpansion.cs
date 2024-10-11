using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings;

public static class SettingsExpansion
{
	public enum Expansion
	{
		Everything = -1,
		Nothing = 0,
		MakingHistory = 2
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsExpansion(Expansion expansion)
	{
		throw null;
	}
}
