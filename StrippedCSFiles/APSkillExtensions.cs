using System;
using System.Runtime.CompilerServices;

public static class APSkillExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AvailableAtLevel(this VesselAutopilot.AutopilotMode mode, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("\n\nUse AvailableAtLevel(Vessel vessel) instead.\nAutopilot level now caters to Kerbals and SASModules with more depth")]
	public static bool AvailableAtLevel(this VesselAutopilot.AutopilotMode mode, int skillLvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetRequiredSkill(this VesselAutopilot.AutopilotMode mode)
	{
		throw null;
	}
}
