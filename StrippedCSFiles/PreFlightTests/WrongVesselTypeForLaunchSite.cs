using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class WrongVesselTypeForLaunchSite : IPreFlightTest
{
	private EditorFacility correctType;

	private string craftFile;

	private string vesselType;

	private string launchSiteName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WrongVesselTypeForLaunchSite(EditorFacility CorrectType, string CraftFileName, string VesselTypeName, string LaunchSiteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWarningDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetProceedOption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetAbortOption()
	{
		throw null;
	}
}
