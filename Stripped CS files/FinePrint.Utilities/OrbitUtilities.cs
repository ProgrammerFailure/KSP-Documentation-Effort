using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinePrint.Utilities;

public static class OrbitUtilities
{
	public class OrbitGenerationInfo
	{
		public CelestialBody body;

		public OrbitType type;

		public double altitudeFactor;

		public double inclinationFactor;

		public double eccentricityOverride;

		public Orbit orbit;

		public KSPRandom generator;

		public OrbitGenerationInfo()
		{
			body = FlightGlobals.GetHomeBody();
			type = OrbitType.RANDOM;
			altitudeFactor = ContractDefs.Satellite.SignificantAltitudeDifficulty;
			inclinationFactor = ContractDefs.Satellite.SignificantInclinationDifficulty;
			eccentricityOverride = 0.0;
			orbit = new Orbit
			{
				referenceBody = body
			};
			generator = new KSPRandom();
		}

		public OrbitGenerationInfo(int seed, CelestialBody body, OrbitType type, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
		{
			this.body = body;
			this.type = type;
			altitudeFactor = altitudeDifficulty;
			inclinationFactor = inclinationDifficulty;
			this.eccentricityOverride = eccentricityOverride;
			orbit = new Orbit
			{
				referenceBody = body
			};
			generator = new KSPRandom(seed);
		}

		public OrbitGenerationInfo(int seed, ref Orbit orbit, OrbitType type, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
		{
			body = orbit.referenceBody;
			this.type = type;
			altitudeFactor = altitudeDifficulty;
			inclinationFactor = inclinationDifficulty;
			this.eccentricityOverride = eccentricityOverride;
			this.orbit = orbit;
			generator = new KSPRandom(seed);
		}
	}

	public static Vector3d PositionOfApoapsis(Orbit o)
	{
		return o.getPositionFromTrueAnomaly(Math.PI);
	}

	public static Vector3d PositionOfPeriapsis(Orbit o)
	{
		return o.getPositionFromTrueAnomaly(0.0);
	}

	public static double AngleOfAscendingNode(Orbit currentOrbit, Orbit targetOrbit)
	{
		Vector3d lhs = -currentOrbit.GetOrbitNormal().xzy;
		Vector3d rhs = -targetOrbit.GetOrbitNormal().xzy;
		Vector3d vector3d = Vector3d.Cross(lhs, rhs);
		Vector3d xzy = currentOrbit.getRelativePositionAtT(0.0).xzy;
		double num = Vector3d.Angle(xzy, vector3d);
		if (!(Math.Abs(Vector3d.Angle(vector3d, Vector3d.Cross(lhs, xzy))) < 90.0))
		{
			return 360.0 - num;
		}
		return num;
	}

	public static double AngleOfDescendingNode(Orbit currentOrbit, Orbit targetOrbit)
	{
		return (AngleOfAscendingNode(currentOrbit, targetOrbit) + 180.0) % 360.0;
	}

	public static double GetRelativeInclination(Orbit a, Orbit b)
	{
		return Math.Acos(Vector3.Dot(a.GetOrbitNormal().normalized, b.GetOrbitNormal().normalized)) * 57.295780181884766;
	}

	public static OrbitType IdentifyOrbit(Orbit o)
	{
		double significantDeviation = ContractDefs.Satellite.SignificantDeviation;
		double v = o.referenceBody.minOrbitalDistance * 1.05;
		double v2 = CelestialUtilities.SynchronousSMA(o.referenceBody);
		bool flag = Math.Abs(Math.Abs(o.inclination) - 116.6) <= significantDeviation / 100.0 * 90.0 || Math.Abs(Math.Abs(o.inclination) - 63.4) <= significantDeviation / 100.0 * 90.0;
		if (SystemUtilities.WithinDeviation(o.semiMajorAxis, v2, significantDeviation))
		{
			if (flag && SystemUtilities.WithinDeviation(o.PeR, v, significantDeviation))
			{
				return OrbitType.TUNDRA;
			}
			if (Math.Abs(o.inclination) <= significantDeviation && SystemUtilities.WithinDeviation(o.PeR, o.ApR, significantDeviation))
			{
				return OrbitType.STATIONARY;
			}
			return OrbitType.SYNCHRONOUS;
		}
		if (SystemUtilities.WithinDeviation(o.semiMajorAxis, CelestialUtilities.KolniyaSMA(o.referenceBody), significantDeviation) && flag && SystemUtilities.WithinDeviation(o.PeR, v, significantDeviation))
		{
			return OrbitType.KOLNIYA;
		}
		if (Math.Abs(o.inclination) % 180.0 <= significantDeviation)
		{
			return OrbitType.EQUATORIAL;
		}
		if (Math.Abs(o.inclination - 90.0) % 180.0 <= significantDeviation)
		{
			return OrbitType.POLAR;
		}
		return OrbitType.RANDOM;
	}

	public static Orbit GenerateOrbit(int seed, CelestialBody targetBody, OrbitType orbitType, double altitudeDifficulty, double inclinationDifficulty, double eccentricityOverride = 0.0)
	{
		if (targetBody == null)
		{
			Debug.LogWarning("Orbit generation cannot generate with null celestial bodies, defaulting to home body.");
			targetBody = FlightGlobals.GetHomeBody();
		}
		OrbitGenerationInfo info = new OrbitGenerationInfo(seed, targetBody, orbitType, altitudeDifficulty, inclinationDifficulty, eccentricityOverride);
		GenerateApsides(ref info);
		GenerateInclination(ref info);
		GenerateLongitudeOfAscendingNode(ref info);
		GenerateArgumentOfPeriapsis(ref info);
		GenerateEpoch(ref info);
		GenerateMeanAnomalyAtEpoch(ref info);
		GenerateFinalization(ref info);
		return info.orbit;
	}

	public static bool ValidateOrbit(int seed, ref Orbit orbit, OrbitType orbitType, double altitudeDifficulty, double inclinationDifficulty, string source = "")
	{
		OrbitGenerationInfo info = new OrbitGenerationInfo(seed, ref orbit, orbitType, altitudeDifficulty, inclinationDifficulty);
		List<string> list = new List<string>();
		bool flag = false;
		if (double.IsNaN(info.orbit.eccentricity) || double.IsInfinity(info.orbit.eccentricity))
		{
			GenerateApsides(ref info);
			list.Add("eccentricity");
			flag = true;
		}
		info.eccentricityOverride = info.orbit.eccentricity;
		if (double.IsNaN(info.orbit.semiMajorAxis) || double.IsInfinity(info.orbit.semiMajorAxis))
		{
			GenerateApsides(ref info);
			list.Add("semi-major axis");
			flag = true;
		}
		if (double.IsNaN(info.orbit.inclination) || double.IsInfinity(info.orbit.inclination))
		{
			GenerateInclination(ref info);
			list.Add("inclination");
			flag = true;
		}
		if (double.IsNaN(info.orbit.double_0) || double.IsInfinity(info.orbit.double_0))
		{
			GenerateLongitudeOfAscendingNode(ref info);
			list.Add("longitude of ascending node");
			flag = true;
		}
		if (double.IsNaN(info.orbit.argumentOfPeriapsis) || double.IsInfinity(info.orbit.argumentOfPeriapsis))
		{
			GenerateArgumentOfPeriapsis(ref info);
			list.Add("argument of periapsis");
			flag = true;
		}
		if (double.IsNaN(info.orbit.epoch) || double.IsInfinity(info.orbit.epoch))
		{
			GenerateEpoch(ref info);
			list.Add("epoch");
			flag = true;
		}
		if (double.IsNaN(info.orbit.meanAnomalyAtEpoch) || double.IsInfinity(info.orbit.meanAnomalyAtEpoch))
		{
			GenerateMeanAnomalyAtEpoch(ref info);
			list.Add("mean anomaly at epoch");
			flag = true;
		}
		if (flag)
		{
			GenerateFinalization(ref info);
			Debug.LogWarning("Generated orbit around " + info.orbit.referenceBody.bodyName + ((source != string.Empty) ? (" for " + source) : "") + " had the following invalid orbital parameters: " + StringUtilities.ThisThisAndThat(list) + ". They were adjusted to make it valid again.");
			orbit = info.orbit;
		}
		return !flag;
	}

	public static void GenerateApsides(ref OrbitGenerationInfo info)
	{
		if (info.orbit.referenceBody == null)
		{
			Debug.LogError("Cannot generate orbit apoapsis or periapsis with no reference body.");
			return;
		}
		double num = info.orbit.referenceBody.minOrbitalDistance * 1.05;
		double num2 = ((info.orbit.referenceBody == Planetarium.fetch.Sun) ? CelestialUtilities.GetSolarExtents() : Math.Max(num, info.orbit.referenceBody.sphereOfInfluence * info.altitudeFactor));
		switch (info.type)
		{
		case OrbitType.SYNCHRONOUS:
		case OrbitType.STATIONARY:
			info.orbit.semiMajorAxis = CelestialUtilities.SynchronousSMA(info.orbit.referenceBody);
			info.orbit.eccentricity = ((info.type == OrbitType.SYNCHRONOUS) ? info.eccentricityOverride : 0.0);
			break;
		case OrbitType.KOLNIYA:
		{
			info.orbit.semiMajorAxis = CelestialUtilities.KolniyaSMA(info.orbit.referenceBody);
			double num6 = num;
			double num5 = info.orbit.semiMajorAxis * 2.0 - num6;
			info.orbit.eccentricity = (num5 - num6) / (num5 + num6);
			break;
		}
		case OrbitType.TUNDRA:
		{
			info.orbit.semiMajorAxis = CelestialUtilities.SynchronousSMA(info.orbit.referenceBody);
			double num6 = num;
			double num5 = info.orbit.semiMajorAxis * 2.0 - num6;
			info.orbit.eccentricity = (num5 - num6) / (num5 + num6);
			break;
		}
		case OrbitType.POLAR:
		case OrbitType.EQUATORIAL:
		case OrbitType.RANDOM:
		{
			double num3 = num + (num2 - num) * info.generator.NextDouble();
			double num4 = num + (num2 - num) * info.generator.NextDouble();
			double val = Mathf.Lerp((float)num3, (float)num4, (float)(1.0 - info.altitudeFactor));
			double num5 = Math.Max(val, num4);
			double num6 = Math.Min(val, num4);
			info.orbit.semiMajorAxis = (num5 + num6) / 2.0;
			info.orbit.eccentricity = (num5 - num6) / (num5 + num6);
			break;
		}
		}
	}

	public static void GenerateInclination(ref OrbitGenerationInfo info)
	{
		double num = Math.Max(1.0, info.generator.NextDouble() * 90.0 * info.inclinationFactor);
		if (info.type != OrbitType.EQUATORIAL && info.type != OrbitType.STATIONARY)
		{
			if (info.type == OrbitType.POLAR)
			{
				num = 90.0;
			}
		}
		else
		{
			num = 0.0;
		}
		if (info.orbit.referenceBody != null && (info.orbit.referenceBody == Planetarium.fetch.Home || info.orbit.referenceBody == Planetarium.fetch.Sun))
		{
			switch (info.type)
			{
			case OrbitType.KOLNIYA:
			case OrbitType.TUNDRA:
				num = (((double)info.generator.Next(0, 100) < info.inclinationFactor * 50.0) ? 116.6 : 63.4);
				break;
			case OrbitType.SYNCHRONOUS:
			case OrbitType.POLAR:
			case OrbitType.EQUATORIAL:
			case OrbitType.RANDOM:
				if ((double)info.generator.Next(0, 100) < info.inclinationFactor * 50.0)
				{
					num = 180.0 - num;
				}
				break;
			}
		}
		else
		{
			switch (info.type)
			{
			case OrbitType.KOLNIYA:
			case OrbitType.TUNDRA:
				num = (SystemUtilities.CoinFlip(info.generator) ? 116.6 : 63.4);
				break;
			case OrbitType.SYNCHRONOUS:
			case OrbitType.POLAR:
			case OrbitType.EQUATORIAL:
			case OrbitType.RANDOM:
				if (SystemUtilities.CoinFlip(info.generator))
				{
					num = 180.0 - num;
				}
				break;
			}
		}
		info.orbit.inclination = num;
	}

	public static void GenerateLongitudeOfAscendingNode(ref OrbitGenerationInfo info)
	{
		info.orbit.double_0 = ((info.type == OrbitType.EQUATORIAL || info.type == OrbitType.STATIONARY) ? 0.0 : (info.generator.NextDouble() * 360.0));
	}

	public static void GenerateArgumentOfPeriapsis(ref OrbitGenerationInfo info)
	{
		switch (info.type)
		{
		case OrbitType.KOLNIYA:
		case OrbitType.TUNDRA:
			info.orbit.argumentOfPeriapsis = (SystemUtilities.CoinFlip(info.generator) ? 270 : 90);
			break;
		case OrbitType.SYNCHRONOUS:
		case OrbitType.STATIONARY:
		case OrbitType.POLAR:
		case OrbitType.EQUATORIAL:
		case OrbitType.RANDOM:
			info.orbit.argumentOfPeriapsis = info.generator.NextDouble() * 360.0;
			break;
		}
	}

	public static void GenerateEpoch(ref OrbitGenerationInfo info)
	{
		info.orbit.epoch = 0.999 + info.generator.NextDouble() * 0.0019999999999998908;
	}

	public static void GenerateMeanAnomalyAtEpoch(ref OrbitGenerationInfo info)
	{
		info.orbit.meanAnomalyAtEpoch = info.generator.NextDouble() * 2.0 * Math.PI;
	}

	public static void GenerateFinalization(ref OrbitGenerationInfo info)
	{
		if (info.type == OrbitType.EQUATORIAL || info.type == OrbitType.STATIONARY)
		{
			info.orbit.an = Vector3.zero;
		}
		info.orbit.Init();
		if (info.orbit.referenceBody != null)
		{
			info.orbit.UpdateFromStateVectors(info.orbit.getRelativePositionAtUT(0.0), info.orbit.getOrbitalVelocityAtUT(0.0), info.orbit.referenceBody, 0.0);
		}
		else
		{
			Debug.LogError("Cannot finalize procedural orbit with no reference body.");
		}
	}
}
