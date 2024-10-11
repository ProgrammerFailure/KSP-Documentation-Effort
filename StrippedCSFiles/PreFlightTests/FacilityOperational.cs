using System.Runtime.CompilerServices;

namespace PreFlightTests;

public class FacilityOperational : IPreFlightTest
{
	private string facilityName;

	private string facilityTitle;

	private bool operational;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FacilityOperational(string FacilityName, string FacilityTitle)
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
