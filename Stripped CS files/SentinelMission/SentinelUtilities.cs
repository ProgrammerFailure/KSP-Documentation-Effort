using System;
using System.Collections.Generic;
using System.Linq;
using FinePrint;
using FinePrint.Utilities;
using ns9;

namespace SentinelMission;

public static class SentinelUtilities
{
	public static string _SentinelPartName = "InfraredTelescope";

	public static string _SentinelPartDisplayName = "#autoLOC_6002282";

	public static string _SentinelModuleName = "SentinelModule";

	public static string _SentinelModuleDisplayName = "#autoLOC_6002283";

	public static double _SentinelViewAngle = 200.0;

	public static float _SpawnChance = 0.25f;

	public static int _WeightedStability = 3;

	public static double MinAsteroidEccentricity = 0.05;

	public static double MaxAsteroidEccentricity = 0.4;

	public static double MinAsteroidInclination = 5.0;

	public static double MaxAsteroidInclination = 40.0;

	public static int perSentinelObjectLimit = 10;

	public static string sentinelPartTitle = "";

	public static string SentinelPartName
	{
		get
		{
			return _SentinelPartName;
		}
		set
		{
			_SentinelPartName = value;
		}
	}

	public static string SentinelPartDisplayName
	{
		get
		{
			return _SentinelPartDisplayName;
		}
		set
		{
			_SentinelPartDisplayName = value;
		}
	}

	public static string SentinelModuleName
	{
		get
		{
			return _SentinelModuleName;
		}
		set
		{
			_SentinelModuleName = value;
		}
	}

	public static string SentinelModuleDisplayName
	{
		get
		{
			return _SentinelModuleDisplayName;
		}
		set
		{
			_SentinelModuleDisplayName = value;
		}
	}

	public static double SentinelViewAngle
	{
		get
		{
			return _SentinelViewAngle;
		}
		set
		{
			_SentinelViewAngle = value;
		}
	}

	public static float SpawnChance
	{
		get
		{
			return _SpawnChance;
		}
		set
		{
			_SpawnChance = value;
		}
	}

	public static int WeightedStability
	{
		get
		{
			return _WeightedStability;
		}
		set
		{
			_WeightedStability = value;
		}
	}

	public static string SentinelPartTitle
	{
		get
		{
			if (sentinelPartTitle != "")
			{
				return sentinelPartTitle;
			}
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(SentinelPartName);
			if (partInfoByName != null)
			{
				return partInfoByName.title;
			}
			return Localizer.Format("#autoLOC_6002284");
		}
	}

	public static bool SentinelCanScan(Vessel v, CelestialBody innerBody = null, CelestialBody outerBody = null)
	{
		double inclination;
		double eccentricity;
		if (v.loaded)
		{
			if (v.situation != Vessel.Situations.ORBITING || v.orbit.referenceBody != Planetarium.fetch.Sun)
			{
				return false;
			}
			inclination = v.orbit.inclination;
			eccentricity = v.orbit.eccentricity;
		}
		else
		{
			if (v.protoVessel.situation != Vessel.Situations.ORBITING || FlightGlobals.Bodies[v.protoVessel.orbitSnapShot.ReferenceBodyIndex] != Planetarium.fetch.Sun)
			{
				return false;
			}
			inclination = v.protoVessel.orbitSnapShot.inclination;
			eccentricity = v.protoVessel.orbitSnapShot.eccentricity;
		}
		if ((innerBody == null || outerBody == null) && !FindInnerAndOuterBodies(v, out innerBody, out outerBody))
		{
			return false;
		}
		if (Math.Abs(outerBody.orbit.eccentricity - eccentricity) > 0.20000000298023224)
		{
			return false;
		}
		if (Math.Abs(Math.Abs(outerBody.orbit.inclination) - Math.Abs(inclination)) > (double)ContractDefs.Sentinel.SignificantDeviation / 100.0 * 90.0)
		{
			return false;
		}
		if (Math.Abs(outerBody.orbit.inclination) % 180.0 >= 1.0)
		{
			double num = v.orbit.double_0;
			double num2 = outerBody.orbit.double_0;
			if (v.orbit.inclination < 0.0)
			{
				num = (num + 180.0) % 360.0;
			}
			if (outerBody.orbit.inclination < 0.0)
			{
				num2 = (num2 + 180.0) % 360.0;
			}
			float num3 = (float)Math.Abs(num - num2) % 360f;
			if (num3 > 180f)
			{
				num3 = 360f - num3;
			}
			if ((double)num3 > ContractDefs.Satellite.SignificantDeviation / 100.0 * 360.0)
			{
				return false;
			}
		}
		return true;
	}

	public static bool FindInnerAndOuterBodies(double double_0, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		Dictionary<double, CelestialBody> dictionary = new Dictionary<double, CelestialBody>();
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			if (celestialBody == Planetarium.fetch.Sun)
			{
				dictionary.Add(0.0, celestialBody);
			}
			else if (celestialBody.referenceBody == Planetarium.fetch.Sun)
			{
				dictionary.Add(celestialBody.orbit.semiMajorAxis, celestialBody);
			}
		}
		List<double> list = dictionary.Keys.ToList();
		list.Sort();
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (list[num] <= double_0 && !(list[num + 1] <= double_0))
				{
					break;
				}
				num++;
				continue;
			}
			innerBody = dictionary[list[0]];
			outerBody = dictionary[list[list.Count - 1]];
			return false;
		}
		innerBody = dictionary[list[num]];
		outerBody = dictionary[list[num + 1]];
		return true;
	}

	public static bool FindInnerAndOuterBodies(Orbit o, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		double semiMajorAxis = o.semiMajorAxis;
		if (o.referenceBody != null && o.referenceBody != Planetarium.fetch.Sun)
		{
			CelestialBody referenceBody = o.referenceBody;
			while (referenceBody != Planetarium.fetch.Sun && referenceBody.referenceBody != null && referenceBody.referenceBody != Planetarium.fetch.Sun)
			{
				referenceBody = referenceBody.orbit.referenceBody;
			}
			semiMajorAxis = referenceBody.orbit.semiMajorAxis;
		}
		return FindInnerAndOuterBodies(semiMajorAxis, out innerBody, out outerBody);
	}

	public static bool FindInnerAndOuterBodies(Vessel v, out CelestialBody innerBody, out CelestialBody outerBody)
	{
		double semiMajorAxis;
		CelestialBody celestialBody;
		if (v.loaded)
		{
			semiMajorAxis = v.orbit.semiMajorAxis;
			celestialBody = v.orbit.referenceBody;
		}
		else
		{
			semiMajorAxis = v.protoVessel.orbitSnapShot.semiMajorAxis;
			celestialBody = FlightGlobals.Bodies[v.protoVessel.orbitSnapShot.ReferenceBodyIndex];
		}
		if (celestialBody != Planetarium.fetch.Sun)
		{
			CelestialBody celestialBody2 = celestialBody;
			while (celestialBody2 != Planetarium.fetch.Sun && celestialBody2.referenceBody != null && celestialBody2.referenceBody != Planetarium.fetch.Sun)
			{
				celestialBody2 = celestialBody2.orbit.referenceBody;
			}
			semiMajorAxis = celestialBody2.orbit.semiMajorAxis;
		}
		return FindInnerAndOuterBodies(semiMajorAxis, out innerBody, out outerBody);
	}

	public static double AdjustedSentinelViewAngle(Orbit innerOrbit, Orbit outerOrbit)
	{
		double num = innerOrbit.semiMajorAxis / 2.0;
		double num2 = outerOrbit.semiMajorAxis / 2.0;
		double num3 = Math.PI / 180.0 * ((360.0 - SentinelViewAngle) / 2.0);
		double num4 = Math.Asin(num / num2 * Math.Sin(num3));
		double num5 = Math.PI - num4;
		double val = (Math.PI - num3 - num4) * (180.0 / Math.PI);
		double val2 = (Math.PI - num3 - num5) * (180.0 / Math.PI);
		double num6 = Math.Max(val, val2) * 2.0;
		if (num6 > SentinelViewAngle)
		{
			num6 = SentinelViewAngle;
		}
		if (num6 < SentinelViewAngle / 100.0)
		{
			num6 = SentinelViewAngle / 100.0;
		}
		return num6;
	}

	public static double GetEscapeVelocity(CelestialBody body, double altitude)
	{
		if (body != Planetarium.fetch.Sun)
		{
			if (altitude <= body.sphereOfInfluence)
			{
				return Math.Sqrt(2.0 * body.gravParameter / altitude - body.gravParameter / (body.Radius + body.sphereOfInfluence));
			}
			return 0.0;
		}
		return Math.Sqrt(2.0 * body.gravParameter / altitude);
	}

	public static double GetMinimumOrbitalSpeed(CelestialBody body)
	{
		double num = ((body == Planetarium.fetch.Sun) ? CelestialUtilities.GetSolarExtents() : body.sphereOfInfluence);
		return Math.Sqrt(body.gravParameter / num);
	}

	public static double GetProgradeBurnAllowance(Orbit o)
	{
		return GetEscapeVelocity(o.referenceBody, o.altitude + o.referenceBody.Radius) - o.GetRelativeVel().magnitude;
	}

	public static double GetRetrogradeBurnAllowance(Orbit o)
	{
		return o.GetRelativeVel().magnitude - GetMinimumOrbitalSpeed(o.referenceBody);
	}

	public static UntrackedObjectClass GetVesselClass(Vessel v)
	{
		UntrackedObjectClass result = UntrackedObjectClass.const_2;
		if (!v.loaded)
		{
			if (v.protoVessel.discoveryInfo.HasValue("size"))
			{
				result = (UntrackedObjectClass)int.Parse(v.protoVessel.discoveryInfo.GetValue("size"));
			}
		}
		else
		{
			result = v.DiscoveryInfo.objectSize;
		}
		return result;
	}

	public static SentinelScanType RandomScanType(Random generator = null)
	{
		return (SentinelScanType)(((generator != null) ? (generator as KSPRandom) : new KSPRandom())?.Next(0, Enum.GetNames(typeof(SentinelScanType)).Length) ?? generator.Next(0, Enum.GetNames(typeof(SentinelScanType)).Length));
	}

	public static UntrackedObjectClass WeightedAsteroidClass(Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		int num = 0;
		int num2 = Enum.GetNames(typeof(UntrackedObjectClass)).Length - 1;
		if (ScenarioDiscoverableObjects.Instance != null)
		{
			num = (int)ScenarioDiscoverableObjects.Instance.minAsteroidClass;
			num2 = (int)ScenarioDiscoverableObjects.Instance.maxAsteroidClass;
		}
		int num3 = num2 - num;
		while (num3 > 0 && (kSPRandom?.Next(100) ?? generator.Next(100)) > 50)
		{
			num3--;
		}
		return (UntrackedObjectClass)(num3 + num);
	}

	public static double WeightedRandom(Random generator)
	{
		return Math.Pow(((generator != null) ? (generator as KSPRandom) : new KSPRandom())?.NextDouble() ?? generator.NextDouble(), WeightedStability);
	}

	public static double WeightedRandom(Random generator, double min, double max)
	{
		return min + Math.Pow(((generator != null) ? (generator as KSPRandom) : new KSPRandom())?.NextDouble() ?? generator.NextDouble(), WeightedStability) * (max - min);
	}

	public static double WeightedRandom(Random generator, double max)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		return WeightedRandom(generator, 0.0, max);
	}

	public static double RandomRange(Random generator = null, double min = double.MinValue, double max = double.MaxValue)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		double val = min;
		double val2 = max;
		min = Math.Min(val, val2);
		max = Math.Max(val, val2);
		return min + (kSPRandom?.NextDouble() ?? generator.NextDouble()) * (max - min);
	}

	public static float CalculateReadDuration(string s)
	{
		float num = 200f;
		float num2 = 5f;
		return (float)s.Length / num2 / num * 60f;
	}

	public static bool IsOnSolarOrbit(CelestialBody body)
	{
		if (!(body == Planetarium.fetch.Sun) && !(body.referenceBody == null) && !(body.referenceBody != Planetarium.fetch.Sun))
		{
			return true;
		}
		return false;
	}
}
