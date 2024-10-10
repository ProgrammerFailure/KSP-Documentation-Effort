using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FinePrint.Utilities;

public static class CelestialUtilities
{
	public static CelestialBody RandomBody(List<CelestialBody> bodies)
	{
		KSPRandom kSPRandom = new KSPRandom();
		return bodies[kSPRandom.Next(bodies.Count)];
	}

	public static CelestialBody HighestBody(List<CelestialBody> bodies)
	{
		float num = float.MinValue;
		int count = bodies.Count;
		while (count-- > 0)
		{
			float num2 = PlanetScienceRanking(bodies[count]);
			if (num2 > num)
			{
				num = num2;
			}
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count2 = bodies.Count;
		while (count2-- > 0)
		{
			if (PlanetScienceRanking(bodies[count2]) == num)
			{
				list.Add(bodies[count2]);
			}
		}
		if (list.Count > 0)
		{
			return RandomBody(list);
		}
		return null;
	}

	public static CelestialBody LowestBody(List<CelestialBody> bodies)
	{
		float num = float.MaxValue;
		int count = bodies.Count;
		while (count-- > 0)
		{
			float num2 = PlanetScienceRanking(bodies[count]);
			if (num2 < num)
			{
				num = num2;
			}
		}
		List<CelestialBody> list = new List<CelestialBody>();
		int count2 = bodies.Count;
		while (count2-- > 0)
		{
			if (PlanetScienceRanking(bodies[count2]) == num)
			{
				list.Add(bodies[count2]);
			}
		}
		if (list.Count > 0)
		{
			return RandomBody(list);
		}
		return null;
	}

	public static List<CelestialBody> GetNeighbors(CelestialBody body, Func<CelestialBody, bool> where = null)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		if (body == null)
		{
			return list;
		}
		if (body.referenceBody != null && (where == null || where(body.referenceBody)))
		{
			list.Add(body.referenceBody);
		}
		if (body.orbitingBodies == null)
		{
			return list;
		}
		int count = body.orbitingBodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = body.orbitingBodies[count];
			if (!(celestialBody == null) && (where == null || where(celestialBody)))
			{
				list.Add(celestialBody);
			}
		}
		return list;
	}

	public static List<CelestialBody> ChildrenOf(CelestialBody parentBody)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		AddChildren(parentBody, list);
		return list;
	}

	public static void AddChildren(CelestialBody parent, List<CelestialBody> list)
	{
		int count = parent.orbitingBodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = parent.orbitingBodies[count];
			list.Add(celestialBody);
			AddChildren(celestialBody, list);
		}
	}

	public static double GetHighestPeak(CelestialBody body)
	{
		if (!(body == null) && !(body.pqsController == null))
		{
			PQSMod[] componentsInChildren = body.pqsController.transform.GetComponentsInChildren<PQSMod>(includeInactive: true);
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				double num = 0.0;
				int num2 = componentsInChildren.Length;
				while (num2-- > 0)
				{
					FieldInfo[] fields = componentsInChildren[num2].GetType().GetFields();
					int num3 = fields.Length;
					while (num3-- > 0)
					{
						switch (fields[num3].Name)
						{
						case "deformity":
						case "heightMapDeformity":
						case "heightMapOffset":
						{
							object value = fields[num3].GetValue(componentsInChildren[num2]);
							if (value is double)
							{
								num += (double)value;
							}
							break;
						}
						}
					}
				}
				return num;
			}
			return 0.0;
		}
		return 0.0;
	}

	public static double GetNextTimeWarp(CelestialBody body, double altitude)
	{
		if (body == null)
		{
			return 0.0;
		}
		if (body.timeWarpAltitudeLimits.Length == 0)
		{
			return 0.0;
		}
		double num = double.MaxValue;
		double num2 = double.MinValue;
		int num3 = body.timeWarpAltitudeLimits.Length;
		while (num3-- > 0)
		{
			double num4 = body.timeWarpAltitudeLimits[num3];
			if (num4 > num2)
			{
				num2 = num4;
			}
			if (num4 > altitude && num4 < num)
			{
				num = num4;
			}
		}
		if (num > num2)
		{
			num = num2;
		}
		return num;
	}

	public static double GetMinimumOrbitalDistance(CelestialBody body, float margin)
	{
		if (body == Planetarium.fetch.Sun)
		{
			CelestialBody celestialBody = FlightGlobals.GetHomeBody();
			double num = 0.0;
			if (celestialBody != null)
			{
				while (celestialBody.referenceBody != FlightGlobals.Bodies[0] && celestialBody.referenceBody != null)
				{
					celestialBody = celestialBody.referenceBody;
				}
				num = Math.Pow(celestialBody.orbit.semiMajorAxis, 2.0) * 4.0 * Math.PI * PhysicsGlobals.SolarLuminosityAtHome;
			}
			double num2 = ContractDefs.SolarOrbitHeatTolerance;
			num2 *= num2;
			num2 *= num2;
			double spaceTemperature = PhysicsGlobals.SpaceTemperature;
			spaceTemperature *= spaceTemperature;
			spaceTemperature *= spaceTemperature;
			return (double)margin * Math.Sqrt(0.5 * num / PhysicsGlobals.StefanBoltzmanConstant / (num2 - spaceTemperature) / (Math.PI * 4.0));
		}
		double num3 = body.Radius;
		double radius = body.Radius;
		if (body.atmosphere)
		{
			num3 += (double)margin * body.atmosphereDepth;
		}
		double highestPeak = GetHighestPeak(body);
		radius += (double)margin * GetNextTimeWarp(body, highestPeak);
		return Math.Max(num3, radius);
	}

	public static double GetAltitudeForDensity(CelestialBody body, double density)
	{
		if (body.atmosphere && body.GetDensity(body.GetPressure(0.0), body.GetTemperature(0.0)) >= density)
		{
			double num = 0.0;
			double num2 = body.atmosphereDepth;
			double num3 = 0.0;
			int num4 = 99;
			while (true)
			{
				if (num4 >= 0)
				{
					num3 = num + (num2 - num) * 0.5;
					double density2 = body.GetDensity(body.GetPressure(num3), body.GetTemperature(num3));
					if (density2 == density)
					{
						break;
					}
					if (density2 > density)
					{
						num = num3;
					}
					else
					{
						num2 = num3;
					}
					num4--;
					continue;
				}
				return num3;
			}
			return num3;
		}
		return 0.0;
	}

	public static Vessel.Situations ApplicableSituation(int seed, CelestialBody body, bool splashAllowed)
	{
		KSPRandom kSPRandom = new KSPRandom(seed);
		List<Vessel.Situations> list = new List<Vessel.Situations>();
		list.Add(Vessel.Situations.ORBITING);
		if (body.ocean && splashAllowed)
		{
			list.Add(Vessel.Situations.SPLASHED);
		}
		if (!IsGasGiant(body))
		{
			list.Add(Vessel.Situations.LANDED);
		}
		return list[kSPRandom.Next(0, list.Count)];
	}

	public static bool IsGasGiant(CelestialBody body)
	{
		if (body == null)
		{
			return false;
		}
		return !body.hasSolidSurface;
	}

	public static bool IsFlyablePlanet(CelestialBody body)
	{
		if (!(body == null) && !(body == Planetarium.fetch.Sun))
		{
			return body.atmosphere;
		}
		return false;
	}

	public static float PlanetScienceRanking(CelestialBody body)
	{
		float num = 0f;
		int count = FlightGlobals.Bodies.Count;
		while (count-- > 0)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[count];
			if (celestialBody.scienceValues.RecoveryValue > num)
			{
				num = celestialBody.scienceValues.RecoveryValue;
			}
		}
		return body.scienceValues.RecoveryValue / num;
	}

	public static Vector3 LLAtoECEF(double lat, double lon, double alt, double radius)
	{
		lat = (lat - 90.0) * (Math.PI / 180.0);
		lon *= Math.PI / 180.0;
		double num = (radius + alt) * -1.0 * Math.Sin(lat) * Math.Cos(lon);
		double num2 = (radius + alt) * Math.Cos(lat);
		double num3 = (radius + alt) * -1.0 * Math.Sin(lat) * Math.Sin(lon);
		return new Vector3((float)num, (float)num2, (float)num3);
	}

	public static double SynchronousSMA(CelestialBody body)
	{
		if ((object)body == null)
		{
			return 0.0;
		}
		return Math.Pow(body.gravParameter * Math.Pow(body.rotationPeriod / (Math.PI * 2.0), 2.0), 1.0 / 3.0);
	}

	public static bool CanBodyBeSynchronous(CelestialBody body, double eccentricity)
	{
		if ((object)body == null)
		{
			return false;
		}
		double num = body.minOrbitalDistance * 1.05;
		double num2 = SynchronousSMA(body);
		if (!((1.0 + eccentricity) * num2 > body.sphereOfInfluence) && num2 >= num)
		{
			return true;
		}
		return false;
	}

	public static double KolniyaSMA(CelestialBody body)
	{
		if ((object)body == null)
		{
			return 0.0;
		}
		double num = body.rotationPeriod / 2.0;
		return Math.Pow(body.gravParameter * Math.Pow(num / (Math.PI * 2.0), 2.0), 1.0 / 3.0);
	}

	public static bool CanBodyBeKolniya(CelestialBody body)
	{
		if ((object)body == null)
		{
			return false;
		}
		double num = body.minOrbitalDistance * 1.05;
		double num2 = KolniyaSMA(body);
		double num3 = num;
		if (!(num2 * 2.0 - num3 > body.sphereOfInfluence) && num2 >= num)
		{
			return true;
		}
		return false;
	}

	public static bool CanBodyBeTundra(CelestialBody body)
	{
		if ((object)body == null)
		{
			return false;
		}
		double num = body.minOrbitalDistance * 1.05;
		double num2 = SynchronousSMA(body);
		double num3 = num;
		if (!(num2 * 2.0 - num3 > body.sphereOfInfluence) && num2 >= num)
		{
			return true;
		}
		return false;
	}

	public static double TerrainAltitude(CelestialBody body, double latitude, double longitude, bool underwater = false)
	{
		double num = 0.0;
		if (body.pqsController != null)
		{
			Vector3d radialVector = QuaternionD.AngleAxis(longitude, Vector3d.down) * QuaternionD.AngleAxis(latitude, Vector3d.forward) * Vector3d.right;
			num = body.pqsController.GetSurfaceHeight(radialVector) - body.pqsController.radius;
			if (body.ocean && !underwater && num < 0.0)
			{
				num = 0.0;
			}
		}
		return num;
	}

	public static CelestialBody MapFocusBody(CelestialBody fallback = null)
	{
		if (PlanetariumCamera.fetch == null)
		{
			return fallback;
		}
		MapObject target = PlanetariumCamera.fetch.target;
		if (target == null)
		{
			return fallback;
		}
		switch (target.type)
		{
		default:
			if (target.orbit == null)
			{
				return fallback;
			}
			return target.orbit.referenceBody;
		case MapObject.ObjectType.CelestialBody:
			return target.celestialBody;
		case MapObject.ObjectType.Vessel:
			return target.vessel.mainBody;
		case MapObject.ObjectType.ManeuverNode:
			return target.maneuverNode.patch.referenceBody;
		}
	}

	public static double GetSolarExtents()
	{
		double num = 0.0;
		for (int num2 = Planetarium.fetch.Sun.orbitingBodies.Count - 1; num2 >= 0; num2--)
		{
			CelestialBody celestialBody = Planetarium.fetch.Sun.orbitingBodies[num2];
			if (celestialBody.orbit != null && celestialBody.orbit.ApR > num)
			{
				num = celestialBody.orbit.ApR;
			}
		}
		return num;
	}

	public static double GreatCircleDistance(CelestialBody body, double latitude1, double longitude1, double latitude2, double longitude2)
	{
		return body.Radius * Math.Acos(Vector3d.Dot(body.GetRelSurfaceNVector(latitude1, longitude1), body.GetRelSurfaceNVector(latitude2, longitude2)));
	}

	public static double GreatCircleDistance(CelestialBody body, Vector3d position1, Vector3d position2)
	{
		return body.Radius * Math.Acos(Vector3d.Dot((position1 - body.position).normalized, (position2 - body.position).normalized));
	}

	public static CelestialBody GetHostPlanet(CelestialBody body)
	{
		if (body == null)
		{
			return Planetarium.fetch.Sun;
		}
		while (body.referenceBody != null && body.referenceBody != Planetarium.fetch.Sun)
		{
			body = body.referenceBody;
		}
		return body;
	}

	public static List<CelestialBody> GetPlanetarySystem(CelestialBody body)
	{
		body = GetHostPlanet(body);
		return ChildrenOf(body);
	}
}
