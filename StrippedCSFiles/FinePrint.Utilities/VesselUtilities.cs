using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint.Utilities;

public class VesselUtilities
{
	private static int highestPodCapacity;

	public static int HighestPodCapacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ActiveVesselFallback(ref Vessel v, bool logging = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasPartName(string partName, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasModuleName(string moduleName, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double VesselResourceAmount(string resourceName, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasAnyParts(List<string> partList, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasAnyModules(List<string> moduleList, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasAnyPartsOrModules(List<string> partList, List<string> moduleList, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int VesselPartAndModuleCount(List<string> partList, List<string> moduleList, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<uint> GetPartIDList(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vessel FindVesselWithPartIDs(List<uint> partIDs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselType ClassifyVesselType(Vessel v = null, bool useVesselType = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Dictionary<Vessel, VesselType> ClassifyAllVesselsAt(CelestialBody body, bool useVesselType = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Vessel> SpecificVesselClassAt(CelestialBody body, VesselType vesselType, bool requireOwned = false, bool excludeActive = false, bool useVesselType = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Vessel> SpecificVesselClassAt(Vessel.Situations situation, CelestialBody body, VesselType vesselType, bool requireOwned = false, bool excludeActive = false, bool useVesselType = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselHasWheelsOnGround(Vessel v = null, params WheelType[] validWheelTypes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FlightBand GetFlightBand(double threshold, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int ActualCrewCapacity(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselLaunchedAfterID(uint launchID, Vessel v, params string[] ignore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselIsOwned(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DiscoverVessel(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint VesselID(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ProtoCrewMember> VesselCrewWithTrait(string trait, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int VesselCrewWithTraitCount(string trait, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int CrewTraitMissionAvailability(string trait, CelestialBody targetBody = null, Vessel excludeVessel = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit GenerateAdjustedVesselOrbit(double minimumDeviation, double modificationLength, int numberOfModifications, System.Random generator = null, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VesselAtOrbit(Orbit o, double deviationWindow, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double VesselAtOrbitAccuracy(Orbit o, double deviationWindow, Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 EstimatePartSize(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindFirstPartOrModuleName(List<string> partNames, List<string> moduleNames)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPartName(Part p)
	{
		throw null;
	}
}
