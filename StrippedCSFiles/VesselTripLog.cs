using System.Runtime.CompilerServices;

public class VesselTripLog
{
	public FlightLog Log;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselTripLog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselTripLog FromVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselTripLog FromProtoVessel(ProtoVessel pv)
	{
		throw null;
	}
}
