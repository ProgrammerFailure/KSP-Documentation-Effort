using System;
using UnityEngine;

[Serializable]
public class CometOrbitType
{
	[SerializeField]
	public string name;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string description;

	[SerializeField]
	public float inclination1Min;

	[SerializeField]
	public float inclination1Max;

	[SerializeField]
	public float inclination2Min;

	[SerializeField]
	public float inclination2Max;

	[SerializeField]
	public float inclination1Chance = 0.8f;

	[SerializeField]
	public float peMin;

	[SerializeField]
	public float peMax;

	[SerializeField]
	public float apMin;

	[SerializeField]
	public float apMax;

	[SerializeField]
	public bool useEccentricity;

	[SerializeField]
	public float eccentricityMin = 1.01f;

	[SerializeField]
	public float eccentricityMax = 1.4f;

	[SerializeField]
	public UntrackedObjectClass minCometClass = UntrackedObjectClass.const_5;

	[SerializeField]
	public UntrackedObjectClass maxCometClass = UntrackedObjectClass.const_8;

	[SerializeField]
	public int chanceWeight = 5;

	public double trueAnom;

	public double eccAnom;

	public double meanAnomalyAtEpoch;

	public CometOrbitType()
	{
	}

	public CometOrbitType(string name, float inclination1Min, float inclination1Max, float inclination2Min, float inclination2Max, float inclination1Chance, float PEMin, float PEMax, float APMin, float APMax, bool UseEcc, float EccMin, float EccMax, UntrackedObjectClass minCometClass, UntrackedObjectClass maxCometClass)
		: this()
	{
		this.name = name;
		this.inclination1Min = inclination1Min;
		this.inclination1Max = inclination1Max;
		this.inclination2Min = inclination2Min;
		this.inclination2Max = inclination2Max;
		this.inclination1Chance = inclination1Chance;
		peMin = PEMin;
		peMax = PEMax;
		apMin = APMin;
		apMax = APMax;
		useEccentricity = UseEcc;
		eccentricityMin = EccMin;
		eccentricityMax = EccMax;
		this.minCometClass = minCometClass;
		this.maxCometClass = maxCometClass;
	}

	public Orbit CalculateHomeOrbit()
	{
		bool validOrbit;
		return CalculateOrbit(homeOrbit: true, null, double.MaxValue, out validOrbit);
	}

	public Orbit CalculateSentinelOrbit(Orbit sentinelOrbit, double minDiscoveryDistance, out bool validOrbit)
	{
		return CalculateOrbit(homeOrbit: false, sentinelOrbit, minDiscoveryDistance, out validOrbit);
	}

	public Orbit CalculateOrbit(bool homeOrbit, Orbit sentinelOrbit, double minDiscoveryDistance, out bool validOrbit)
	{
		validOrbit = true;
		Orbit orbit = new Orbit();
		CelestialBody sun = Planetarium.fetch.Sun;
		double num5;
		double num6;
		if (!useEccentricity)
		{
			double num = UnityEngine.Random.Range(peMin, peMax);
			double num2 = UnityEngine.Random.Range(apMin, apMax);
			double num3 = num * FlightGlobals.GetCometPerturber().orbit.semiMajorAxis;
			double num4 = num2 * FlightGlobals.GetCometPerturber().orbit.semiMajorAxis;
			num5 = (num3 + num4) / 2.0;
			num6 = 1.0 - num3 / num5;
		}
		else
		{
			double num3 = (double)UnityEngine.Random.Range(peMin, peMax) * FlightGlobals.GetCometPerturber().orbit.semiMajorAxis;
			num6 = UnityEngine.Random.Range(eccentricityMin, eccentricityMax);
			double num4 = num3 / (1.0 - num6);
			num5 = (num3 + num4) / 2.0;
		}
		double num7 = ((!((float)UnityEngine.Random.Range(0, 1) < inclination1Chance)) ? ((double)UnityEngine.Random.Range(inclination2Min, inclination2Max)) : ((double)UnityEngine.Random.Range(inclination1Min, inclination1Max)));
		double lan = UnityEngine.Random.Range(0f, 360f);
		double argPe = 0.0;
		double universalTime = Planetarium.GetUniversalTime();
		orbit = new Orbit(num7, num6, num5, lan, argPe, 0.0, universalTime, sun);
		if (homeOrbit)
		{
			double val = CometManager.GenerateHomeSpawnDistance();
			val = Math.Min(val, minDiscoveryDistance);
			trueAnom = orbit.TrueAnomalyAtRadiusSimple(val);
			eccAnom = orbit.GetEccentricAnomaly(trueAnom);
			meanAnomalyAtEpoch = 0.0 - orbit.GetMeanAnomaly(eccAnom);
			if (double.IsNaN(meanAnomalyAtEpoch))
			{
				Debug.Log("[CometOrbitType] Unable to solve HomeSpawnDistance using default value");
				meanAnomalyAtEpoch = -Math.PI / 2.0;
			}
			orbit.SetOrbit(num7, num6, num5, lan, argPe, meanAnomalyAtEpoch, universalTime, sun);
		}
		else
		{
			double val2 = CometManager.GenerateSentinelSpawnDistance(sentinelOrbit.radius);
			val2 = Math.Max(val2, minDiscoveryDistance);
			if (val2 > orbit.ApR && orbit.eccentricity < 1.0)
			{
				validOrbit = false;
			}
			else
			{
				if (val2 < orbit.PeR)
				{
					Debug.Log("[CometOrbitType] Spawn distance inside pe of Comet Orbit - picking spot between 45 and 90 degs on inbound");
					eccAnom = UnityEngine.Random.Range((float)Math.PI / 4f, (float)Math.PI / 2f);
					trueAnom = orbit.GetTrueAnomaly(eccAnom);
				}
				else
				{
					trueAnom = orbit.TrueAnomalyAtRadiusSimple(val2);
					eccAnom = orbit.GetEccentricAnomaly(trueAnom);
				}
				meanAnomalyAtEpoch = 0.0 - orbit.GetMeanAnomaly(eccAnom);
				if (num7 > 90.0 || num7 < -90.0)
				{
					trueAnom *= -1.0;
				}
				if (double.IsNaN(meanAnomalyAtEpoch))
				{
					Debug.Log("[CometOrbitType] Unable to solve SentinelSpawnDistance using default value - setting to half way point");
					meanAnomalyAtEpoch = 0.0 - orbit.GetMeanAnomaly(-Math.PI / 2.0);
				}
				double num8 = (sentinelOrbit.double_0 + sentinelOrbit.argumentOfPeriapsis) * (Math.PI / 180.0) + sentinelOrbit.trueAnomaly + trueAnom;
				float num9 = (float)(sentinelOrbit.radius / val2 * Math.Sin((double)(180f - CometManager.SentinelDiscoveryArc) * Math.PI / 180.0));
				num8 += (double)UnityEngine.Random.Range(0f - num9, num9);
				num8 *= 180.0 / Math.PI;
				orbit.SetOrbit(num7, num6, num5, num8, argPe, meanAnomalyAtEpoch, universalTime, sun);
			}
		}
		return orbit;
	}

	public UntrackedObjectClass GetRandomObjClass()
	{
		return (UntrackedObjectClass)UnityEngine.Random.Range((int)minCometClass, (int)(maxCometClass + 1));
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("name", ref name);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("description", ref description);
		node.TryGetValue("inclination1Min", ref inclination1Min);
		node.TryGetValue("inclination1Max", ref inclination1Max);
		node.TryGetValue("inclination2Min", ref inclination2Min);
		node.TryGetValue("inclination2Max", ref inclination2Max);
		node.TryGetValue("inclination1Chance", ref inclination1Chance);
		node.TryGetValue("peMin", ref peMin);
		node.TryGetValue("peMax", ref peMax);
		node.TryGetValue("useEccentricity", ref useEccentricity);
		node.TryGetValue("eccentricityMin", ref eccentricityMin);
		node.TryGetValue("eccentricityMax", ref eccentricityMax);
		node.TryGetValue("apMin", ref apMin);
		node.TryGetValue("apMax", ref apMax);
		node.TryGetValue("chanceWeight", ref chanceWeight);
		node.TryGetEnum("minCometClass", ref minCometClass, UntrackedObjectClass.const_5);
		node.TryGetEnum("maxCometClass", ref maxCometClass, UntrackedObjectClass.const_8);
	}
}
