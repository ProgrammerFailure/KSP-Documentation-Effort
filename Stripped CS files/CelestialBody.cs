using System;
using System.Collections.Generic;
using FinePrint.Utilities;
using KSPAchievements;
using ns9;
using UnityEngine;
using UnityEngine.Rendering;

[SelectionBase]
public class CelestialBody : MonoBehaviour, ITargetable, IDiscoverable
{
	public string bodyName = "Unnamed";

	public string bodyDisplayName = "Unnamed";

	public string bodyAdjectiveDisplayName = "Unnamed";

	public string bodyDescription = "A mysterious uncharted celestial body.";

	public double GeeASL;

	public double Radius;

	public double Mass;

	public double Density;

	public double SurfaceArea;

	public double gravParameter;

	public double sphereOfInfluence = double.PositiveInfinity;

	public double hillSphere;

	public double gMagnitudeAtCenter;

	public double atmDensityASL;

	public double atmPressureASL;

	public bool scaledEllipsoid = true;

	public Vector3d scaledElipRadMult = Vector3d.one;

	public double scaledRadiusHorizonMultiplier = 1.0;

	public double navballSwitchRadiusMult = 0.06;

	public double navballSwitchRadiusMultLow = 0.055;

	[Obsolete("theName and use_The_InName are deprecated and will be removed in future updates, please use name or displayName instead.")]
	public bool use_The_InName;

	public bool isHomeWorld;

	public bool isCometPerturber;

	public bool ocean;

	public bool oceanUseFog = true;

	public double oceanFogPQSDepth = 1000.0;

	public float oceanFogPQSDepthRecip = 0.001f;

	public float oceanFogDensityStart = 0.015f;

	public float oceanFogDensityEnd = 0.13f;

	public float oceanFogDensityPQSMult = 0.02f;

	public float oceanFogDensityAltScalar = -0.0008f;

	public float oceanFogDensityExponent = 1f;

	public Color oceanFogColorStart = new Color(0f, 0.3372549f, 0.4862745f, 1f);

	public Color oceanFogColorEnd = new Color(0f, 0.084f, 0.122f, 1f);

	public float oceanFogDawnFactor = 10f;

	public float oceanSkyColorMult = 1.2f;

	public float oceanSkyColorOpacityBase = 0.2f;

	public float oceanSkyColorOpacityAltMult = 2f;

	public double oceanDensity = 1.0;

	public float oceanAFGBase = 0.6f;

	public float oceanAFGAltMult = 0.05f;

	public float oceanAFGMin = 0.05f;

	public float oceanSunBase = 0.5f;

	public float oceanSunAltMult = 0.01f;

	public float oceanSunMin = 0.05f;

	public bool oceanAFGLerp;

	public float oceanMinAlphaFogDistance = 200f;

	public float oceanMaxAlbedoFog = 0.95f;

	public float oceanMaxAlphaFog = 0.9f;

	public float oceanAlbedoDistanceScalar = 0.01f;

	public float oceanAlphaDistanceScalar = 0.01f;

	public double minOrbitalDistance = 100000.0;

	public bool atmosphere;

	public bool atmosphereContainsOxygen;

	public double atmosphereDepth;

	public double atmosphereTemperatureSeaLevel = 288.0;

	public double atmospherePressureSeaLevel = 101.325;

	public double atmosphereMolarMass = 0.0289644;

	public double atmosphereAdiabaticIndex = 1.4;

	public double atmosphereTemperatureLapseRate;

	public double atmosphereGasMassLapseRate;

	public bool atmosphereUseTemperatureCurve;

	public bool atmosphereTemperatureCurveIsNormalized;

	public FloatCurve atmosphereTemperatureCurve = new FloatCurve();

	public FloatCurve latitudeTemperatureBiasCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public FloatCurve latitudeTemperatureSunMultCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public FloatCurve axialTemperatureSunMultCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public FloatCurve axialTemperatureSunBiasCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public FloatCurve atmosphereTemperatureSunMultCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public double maxAxialDot;

	public FloatCurve eccentricityTemperatureBiasCurve = new FloatCurve(new Keyframe[1]
	{
		new Keyframe(0f, 0f)
	});

	public double albedo = 0.35;

	public double emissivity = 0.65;

	public double coreTemperatureOffset;

	public double convectionMultiplier = 1.0;

	public double shockTemperatureMultiplier = 1.0;

	public double bodyTemperature;

	public bool atmosphereUsePressureCurve;

	public bool atmospherePressureCurveIsNormalized;

	public FloatCurve atmospherePressureCurve = new FloatCurve();

	public double radiusAtmoFactor = 1.0;

	public bool hasSolidSurface;

	public bool isStar;

	public Vector3d transformRight;

	public Vector3d transformUp;

	public Vector3d _position;

	public QuaternionD rotation;

	public OrbitDriver orbitDriver;

	public GClass4 pqsController;

	public PQSSurfaceObject[] pqsSurfaceObjects;

	public GameObject scaledBody;

	public AtmosphereFromGround afg;

	public MapObject mapObject;

	public bool rotates;

	public double rotationPeriod;

	public double rotPeriodRecip;

	public double solarDayLength;

	public bool solarRotationPeriod;

	public double initialRotation;

	public double rotationAngle;

	public double directRotAngle;

	public Vector3d angularVelocity;

	public Vector3d zUpAngularVelocity;

	public bool tidallyLocked;

	public bool clampInverseRotThreshold = true;

	public bool inverseRotation;

	public float inverseRotThresholdAltitude = 15000f;

	public double angularV;

	public float[] timeWarpAltitudeLimits;

	public Color atmosphericAmbientColor;

	public List<CelestialBody> orbitingBodies = new List<CelestialBody>();

	public Planetarium.CelestialFrame BodyFrame;

	public CelestialBodySubtree progressTree;

	public CelestialBodyType bodyType;

	public CelestialBodyScienceParams scienceValues;

	public CBAttributeMapSO BiomeMap;

	public MiniBiome[] MiniBiomes;

	public Transform bodyTransform;

	public Vector3d rPos;

	public double latValue;

	public double lonValue;

	public const double bodyEmissiveScalarS0Front = 0.782048841;

	public const double bodyEmissiveScalarS0Back = 0.093081228;

	public const double bodyEmissiveScalarS1 = 0.87513007;

	public const double bodyEmissiveScalarS0Top = 0.398806364;

	public const double bodyEmissiveScalarS1Top = 0.797612728;

	public Texture2D resourceMap;

	public DiscoveryInfo dscInfo;

	public static string cacheAutoLOC_193189;

	public static string cacheAutoLOC_193194;

	public static string cacheAutoLOC_439019;

	public static string cacheAutoLOC_193198;

	public string displayName => bodyDisplayName;

	[Obsolete("theName and use_The_InName are deprecated and will be removed in future updates, please use name or displayName instead.")]
	public string theName => (use_The_InName ? "the " : "") + bodyName;

	public new string name => bodyName;

	public int flightGlobalsIndex { get; set; }

	public Vector3d position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
			bodyTransform.position = value;
			if (pqsController != null)
			{
				pqsController.PrecisePosition = value;
			}
		}
	}

	public Orbit orbit
	{
		get
		{
			if (!(orbitDriver != null))
			{
				return null;
			}
			return orbitDriver.orbit;
		}
	}

	public MapObject MapObject
	{
		get
		{
			return mapObject;
		}
		set
		{
			mapObject = value;
		}
	}

	public CelestialBody referenceBody
	{
		get
		{
			if (!orbitDriver)
			{
				return this;
			}
			return orbit.referenceBody;
		}
	}

	public Texture2D ResourceMap => resourceMap;

	public DiscoveryInfo DiscoveryInfo => dscInfo;

	public Vector3d RotationAxis => Vector3d.up;

	public void SetupConstants()
	{
		if (atmosphere)
		{
			atmosphereTemperatureLapseRate = atmosphereTemperatureSeaLevel / atmosphereDepth;
			atmosphereGasMassLapseRate = GeeASL * PhysicsGlobals.GravitationalAcceleration * atmosphereMolarMass / (PhysicsGlobals.IdealGasConstant * atmosphereTemperatureLapseRate);
			radiusAtmoFactor = Radius / atmosphereDepth * (0.0 - Math.Log(1E-06));
			atmDensityASL = GetDensity(GetPressure(0.0), GetTemperature(0.0));
			atmPressureASL = GetPressure(0.0) * 0.009869232667160128;
			if (clampInverseRotThreshold && (double)inverseRotThresholdAltitude < atmosphereDepth)
			{
				inverseRotThresholdAltitude = (float)atmosphereDepth;
			}
		}
		double num = Radius * Radius;
		Density = Mass / (4.1887902047863905 * num * Radius);
		SurfaceArea = Math.PI * 4.0 * num;
		if (orbitDriver != null)
		{
			maxAxialDot = Math.Sin(UtilMath.DegreesToRadians(orbit.inclination));
		}
		minOrbitalDistance = CelestialUtilities.GetMinimumOrbitalDistance(this, 1f);
	}

	public double GetPressure(double altitude)
	{
		if (atmosphere && altitude < atmosphereDepth)
		{
			if (atmosphereUsePressureCurve)
			{
				if (atmospherePressureCurveIsNormalized)
				{
					return Mathf.Lerp(0f, (float)atmospherePressureSeaLevel, atmospherePressureCurve.Evaluate((float)(altitude / atmosphereDepth)));
				}
				return atmospherePressureCurve.Evaluate((float)altitude);
			}
			return atmospherePressureSeaLevel * Math.Pow(1.0 - atmosphereTemperatureLapseRate * altitude / atmosphereTemperatureSeaLevel, atmosphereGasMassLapseRate);
		}
		return 0.0;
	}

	public double GetPressureAtm(double altitude)
	{
		return GetPressure(altitude) * 0.009869232667160128;
	}

	public double GetTemperature(double altitude)
	{
		if (altitude >= atmosphereDepth)
		{
			return 0.0;
		}
		if (atmosphereUseTemperatureCurve)
		{
			if (atmosphereTemperatureCurveIsNormalized)
			{
				return UtilMath.Lerp(PhysicsGlobals.SpaceTemperature, atmosphereTemperatureSeaLevel, atmosphereTemperatureCurve.Evaluate((float)(altitude / atmosphereDepth)));
			}
			return atmosphereTemperatureCurve.Evaluate((float)altitude);
		}
		return atmosphereTemperatureSeaLevel - atmosphereTemperatureLapseRate * altitude;
	}

	public double GetDensity(double pressure, double temperature)
	{
		if (!(pressure <= 0.0) && temperature > 0.0)
		{
			return pressure * 1000.0 * atmosphereMolarMass / (PhysicsGlobals.IdealGasConstant * temperature);
		}
		return 0.0;
	}

	public double GetSpeedOfSound(double pressure, double density)
	{
		if (!(pressure <= 0.0) && density > 0.0)
		{
			return Math.Sqrt(atmosphereAdiabaticIndex * (pressure * 1000.0 / density));
		}
		return 0.0;
	}

	public double GetSolarPowerFactor(double density)
	{
		double num = 1.225;
		if (Planetarium.fetch != null && Planetarium.fetch.Home.atmosphereDepth > 0.0)
		{
			num = Planetarium.fetch.Home.atmDensityASL;
		}
		double num2 = (1.0 - PhysicsGlobals.SolarInsolationAtHome) * num;
		return num2 / (num2 + density * PhysicsGlobals.SolarInsolationAtHome);
	}

	public void Awake()
	{
		bodyTransform = base.transform;
		_position = bodyTransform.position;
		orbitDriver = GetComponent<OrbitDriver>();
		pqsController = GetComponentInChildren<GClass4>();
		gMagnitudeAtCenter = GeeASL * PhysicsGlobals.GravitationalAcceleration * Math.Pow(Radius, 2.0);
		Mass = Radius * Radius * (GeeASL * PhysicsGlobals.GravitationalAcceleration) / 6.67408E-11;
		gravParameter = Mass * 6.67408E-11;
		if ((bool)orbitDriver)
		{
			orbitDriver.referenceBody.orbitingBodies.Add(this);
		}
		bodyDisplayName = Localizer.Format(bodyDisplayName);
		bodyAdjectiveDisplayName = Localizer.Format(bodyAdjectiveDisplayName);
		dscInfo = new DiscoveryInfo(this);
		if (BiomeMap != null)
		{
			BiomeMap.FormatLocalizationTags();
		}
	}

	public void Start()
	{
		if ((bool)orbitDriver)
		{
			sphereOfInfluence = orbit.semiMajorAxis * Math.Pow(Mass / orbit.referenceBody.Mass, 0.4);
			hillSphere = orbit.semiMajorAxis * (1.0 - orbit.eccentricity) * Math.Pow(Mass / orbit.referenceBody.Mass, 1.0 / 3.0);
			orbitDriver.QueuedUpdate = true;
			if (solarRotationPeriod)
			{
				double num = rotationPeriod;
				double num2 = Math.PI * 2.0 * Math.Sqrt(Math.Pow(Math.Abs(orbit.semiMajorAxis), 3.0) / orbit.referenceBody.gravParameter);
				double time = num * num2 / (num2 + num);
				Debug.Log("[CelestialBody]: " + bodyName + "'s solar day length is " + KSPUtil.PrintTime(num, 3, explicitPositive: false, logEnglish: true) + " long. sidereal day length is " + KSPUtil.PrintTime(time, 3, explicitPositive: false, logEnglish: true) + " long", base.gameObject);
				rotationPeriod = time;
			}
		}
		else
		{
			sphereOfInfluence = double.PositiveInfinity;
			hillSphere = double.PositiveInfinity;
		}
		SetupConstants();
		if ((bool)TimeWarp.fetch && timeWarpAltitudeLimits.Length != TimeWarp.fetch.altitudeLimits.Length)
		{
			Debug.LogWarning("Warp Rate limits for " + bodyName + " didn't match warp rate count. Limits reset to default");
			resetTimeWarpLimits();
		}
		oceanFogPQSDepthRecip = (float)(1.0 / oceanFogPQSDepth);
		if (scaledBody != null)
		{
			afg = scaledBody.GetComponentInChildren<AtmosphereFromGround>();
			if (afg != null && ocean)
			{
				afg.useKrESunLerp = oceanAFGLerp;
				afg.oceanDepthRecip = oceanFogPQSDepthRecip;
				afg.underwaterOpacityAltBase = oceanSkyColorOpacityBase;
				afg.underwaterOpacityAltMult = oceanSkyColorOpacityAltMult;
				afg.underwaterColorStart = oceanFogColorStart * oceanSkyColorMult;
				afg.underwaterColorEnd = oceanFogColorEnd * oceanSkyColorMult;
				afg.dawnFactor = oceanFogDawnFactor;
			}
		}
		else
		{
			afg = null;
		}
		if (pqsController != null)
		{
			PQSMod_AltitudeUV[] componentsInChildren = pqsController.GetComponentsInChildren<PQSMod_AltitudeUV>();
			int num3 = componentsInChildren.Length;
			for (int i = 0; i < num3; i++)
			{
				componentsInChildren[i].oceanDepth = oceanFogPQSDepth;
			}
			pqsSurfaceObjects = pqsController.GetComponentsInChildren<PQSSurfaceObject>(includeInactive: true);
		}
	}

	public void CBUpdate()
	{
		transformRight = base.transform.right;
		transformUp = base.transform.up;
		gMagnitudeAtCenter = GeeASL * PhysicsGlobals.GravitationalAcceleration * Radius * Radius;
		Mass = Radius * Radius * (GeeASL * PhysicsGlobals.GravitationalAcceleration) / 6.67408E-11;
		gravParameter = Mass * 6.67408E-11;
		if (rotates && rotationPeriod != 0.0 && (!tidallyLocked || (orbit != null && orbit.period != 0.0)))
		{
			if (tidallyLocked)
			{
				rotationPeriod = orbit.period;
			}
			rotPeriodRecip = 1.0 / rotationPeriod;
			angularVelocity = Vector3d.down * (Math.PI * 2.0 * rotPeriodRecip);
			zUpAngularVelocity = Vector3d.back * (Math.PI * 2.0 * rotPeriodRecip);
			rotationAngle = (initialRotation + 360.0 * rotPeriodRecip * Planetarium.GetUniversalTime()) % 360.0;
			angularV = angularVelocity.magnitude;
			if (!inverseRotation)
			{
				directRotAngle = (rotationAngle - Planetarium.InverseRotAngle) % 360.0;
				Planetarium.CelestialFrame.PlanetaryFrame(0.0, 90.0, directRotAngle, ref BodyFrame);
				rotation = BodyFrame.Rotation.swizzle;
				bodyTransform.rotation = rotation;
			}
			else
			{
				Planetarium.InverseRotAngle = (rotationAngle - directRotAngle) % 360.0;
				Planetarium.CelestialFrame.PlanetaryFrame(0.0, 90.0, Planetarium.InverseRotAngle, ref Planetarium.Zup);
				Planetarium.Rotation = QuaternionD.Inverse(Planetarium.Zup.Rotation).swizzle;
			}
		}
		if ((bool)orbitDriver)
		{
			orbitDriver.UpdateOrbit();
		}
		CelestialBody celestialBody = this;
		CelestialBody celestialBody2 = null;
		celestialBody2 = ((!(Planetarium.fetch != null)) ? FlightGlobals.Bodies[0] : Planetarium.fetch.Sun);
		while (celestialBody.referenceBody != celestialBody2 && celestialBody.referenceBody != null)
		{
			celestialBody = celestialBody.referenceBody;
		}
		if (celestialBody.orbit != null)
		{
			double num = celestialBody.orbit.period - rotationPeriod;
			if (num == 0.0)
			{
				solarDayLength = double.MaxValue;
			}
			else
			{
				solarDayLength = celestialBody.orbit.period * rotationPeriod / num;
			}
		}
		else
		{
			solarDayLength = 1.0;
		}
	}

	public Bounds getBounds()
	{
		return new Bounds(position, Vector3d.one * Radius * 2.0);
	}

	public void OnDrawGizmos()
	{
		if (sphereOfInfluence != 0.0)
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(ScaledSpace.LocalToScaledSpace(position), (float)sphereOfInfluence * ScaledSpace.InverseScaleFactor);
		}
	}

	public Vector3d GetFrameVel()
	{
		if ((bool)orbitDriver && orbitDriver.enabled)
		{
			return orbit.vel + ((orbit.referenceBody != null) ? orbit.referenceBody.GetFrameVel() : Vector3d.zero);
		}
		return Vector3d.zero;
	}

	public Vector3d GetFrameVelAtUT(double double_0)
	{
		if ((bool)orbitDriver && orbitDriver.enabled)
		{
			return orbit.getOrbitalVelocityAtUT(double_0) + ((orbit.referenceBody != null) ? orbit.referenceBody.GetFrameVelAtUT(double_0) : Vector3d.zero);
		}
		return Vector3d.zero;
	}

	public Vector3d getRFrmVel(Vector3d worldPos)
	{
		return Vector3d.Cross(angularVelocity, worldPos - position);
	}

	public Vector3d getRFrmVelOrbit(Orbit o)
	{
		return Vector3d.Cross(angularVelocity, o.pos.xzy);
	}

	public Vector3d getTruePositionAtUT(double double_0)
	{
		if ((bool)orbitDriver && orbitDriver.enabled)
		{
			return orbit.getRelativePositionAtUT(double_0).xzy + orbit.referenceBody.getTruePositionAtUT(double_0);
		}
		return position;
	}

	public Vector3d getPositionAtUT(double double_0)
	{
		if ((bool)orbitDriver && orbitDriver.enabled)
		{
			return orbit.getRelativePositionAtUT(double_0).xzy + orbit.referenceBody.position;
		}
		return position;
	}

	public Vector3d GetRelSurfaceNVector(double lat, double lon)
	{
		lat *= Math.PI / 180.0;
		lon *= Math.PI / 180.0;
		return Planetarium.SphericalVector(lat, lon).xzy;
	}

	public Vector3d GetSurfaceNVector(double lat, double lon)
	{
		lat *= Math.PI / 180.0;
		lon *= Math.PI / 180.0;
		Vector3d r = Planetarium.SphericalVector(lat, lon);
		return BodyFrame.LocalToWorld(r).xzy;
	}

	public Vector3d GetRelSurfacePosition(double lat, double lon, double alt)
	{
		return GetRelSurfaceNVector(lat, lon) * (Radius + alt);
	}

	public Vector3d GetRelSurfacePosition(double lat, double lon, double alt, out Vector3d normal)
	{
		normal = GetRelSurfaceNVector(lat, lon);
		return normal * (Radius + alt);
	}

	public Vector3d GetRelSurfacePosition(Vector3d worldPosition)
	{
		return BodyFrame.WorldToLocal((worldPosition - position).xzy).xzy;
	}

	public Vector3d GetRelSurfaceDirection(Vector3d worldDirection)
	{
		return BodyFrame.WorldToLocal(worldDirection.xzy).xzy;
	}

	public Vector3d GetWorldSurfacePosition(double lat, double lon, double alt)
	{
		return BodyFrame.LocalToWorld(GetRelSurfacePosition(lat, lon, alt).xzy).xzy + position;
	}

	public double GetLatitude(Vector3d pos, bool isRadial = false)
	{
		rPos = (isRadial ? pos.normalized : (pos - position).normalized);
		rPos = BodyFrame.WorldToLocal(rPos.xzy);
		latValue = Math.Asin(rPos.z) * 57.295780181884766;
		if (double.IsNaN(latValue))
		{
			latValue = 0.0;
		}
		return latValue;
	}

	public double GetLongitude(Vector3d pos, bool isRadial = false)
	{
		rPos = (isRadial ? pos.normalized : (pos - position).normalized);
		rPos = BodyFrame.WorldToLocal(rPos.xzy);
		lonValue = Math.Atan2(rPos.y, rPos.x) * 57.295780181884766;
		if (double.IsNaN(lonValue))
		{
			lonValue = 0.0;
		}
		return lonValue;
	}

	public Vector2d GetLatitudeAndLongitude(Vector3d pos, bool isRadial = false)
	{
		rPos = (isRadial ? pos.normalized : (pos - position).normalized);
		rPos = BodyFrame.WorldToLocal(rPos.xzy);
		latValue = Math.Asin(rPos.z) * 57.295780181884766;
		lonValue = Math.Atan2(rPos.y, rPos.x) * 57.295780181884766;
		if (double.IsNaN(latValue))
		{
			latValue = 0.0;
		}
		if (double.IsNaN(lonValue))
		{
			lonValue = 0.0;
		}
		return new Vector2d(latValue, lonValue);
	}

	public bool GetImpactLatitudeAndLongitude(Vector3d position, Vector3d velocity, out double latitude, out double longitude)
	{
		double num = Vector3d.Dot(position, velocity);
		double num2 = Vector3d.Dot(velocity, velocity);
		double num3 = Vector3d.Dot(position, position);
		double num4 = num / num2;
		double num5 = num4 * num4 - (num3 - Radius * Radius) / num2;
		if (num5 >= 0.0)
		{
			double num6 = 0.0 - num4 - Math.Sqrt(num5);
			Vector3d vector3d = position + velocity * num6;
			latitude = Math.Asin(vector3d.y / vector3d.magnitude) * (180.0 / Math.PI);
			longitude = Math.Atan2(vector3d.z, vector3d.x) * (180.0 / Math.PI);
			return true;
		}
		latitude = 0.0;
		longitude = 0.0;
		return false;
	}

	public double GetAltitude(Vector3d worldPos)
	{
		return (worldPos - position).magnitude - Radius;
	}

	public void GetLatLonAlt(Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		rPos = BodyFrame.WorldToLocal((worldPos - position).xzy);
		double magnitude = rPos.magnitude;
		alt = magnitude - Radius;
		rPos /= magnitude;
		lat = Math.Asin(rPos.z) * (180.0 / Math.PI);
		lon = Math.Atan2(rPos.y, rPos.x) * (180.0 / Math.PI);
		if (double.IsNaN(lat))
		{
			lat = 0.0;
		}
		if (double.IsNaN(lon))
		{
			lon = 0.0;
		}
	}

	public void GetLatLonAltOrbital(Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		rPos = BodyFrame.WorldToLocal(worldPos);
		double magnitude = rPos.magnitude;
		alt = magnitude - Radius;
		rPos /= magnitude;
		lat = Math.Asin(rPos.z) * (180.0 / Math.PI);
		lon = Math.Atan2(rPos.y, rPos.x) * (180.0 / Math.PI);
		if (double.IsNaN(lat))
		{
			lat = 0.0;
		}
		if (double.IsNaN(lon))
		{
			lon = 0.0;
		}
	}

	public void GetRandomLatitudeAndLongitude(ref double latitude, ref double longitude, bool waterAllowed = false, bool spaceCenterAllowed = false, KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
		}
		if (!hasSolidSurface)
		{
			waterAllowed = true;
		}
		bool flag = SpaceCenter.Instance != null && SpaceCenter.Instance.cb == this;
		for (int num = 1000; num >= 0; num--)
		{
			double num2 = generator.NextDouble();
			num2 = 1.0 - num2 * 2.0;
			latitude = Math.Asin(num2) * 57.295780181884766;
			longitude = generator.NextDouble() * 360.0 - 180.0;
			if (!ocean || waterAllowed || !(TerrainAltitude(latitude, longitude, allowNegative: true) < 0.0))
			{
				bool flag2 = false;
				if (flag && !spaceCenterAllowed && SpaceCenter.Instance.GreatCircleDistance(GetRelSurfaceNVector(latitude, longitude)) <= 4000.0)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					break;
				}
			}
		}
	}

	public double TerrainAltitude(double latitude, double longitude, bool allowNegative = false)
	{
		if (pqsController == null)
		{
			return 0.0;
		}
		Vector3d relSurfaceNVector = GetRelSurfaceNVector(latitude, longitude);
		double num = pqsController.GetSurfaceHeight(relSurfaceNVector) - pqsController.radius;
		if (!allowNegative && num < 0.0)
		{
			num = 0.0;
		}
		return num;
	}

	public static CelestialBody GetBodyReferencing(CelestialBody body, CelestialBody sun)
	{
		while (body != null && body.orbit != null && body.orbit.referenceBody != null && body.orbit.referenceBody != sun)
		{
			body = body.orbit.referenceBody;
		}
		return body;
	}

	public virtual void GetAtmoThermalStats(bool getBodyFlux, CelestialBody sunBody, Vector3d sunVector, double sunDot, Vector3d upAxis, double altitude, out double atmosphereTemperatureOffset, out double bodyEmissiveFlux, out double bodyAlbedoFlux)
	{
		atmosphereTemperatureOffset = 0.0;
		bodyEmissiveFlux = 0.0;
		bodyAlbedoFlux = 0.0;
		if (!(sunBody != this))
		{
			return;
		}
		Vector3d vector3d = sunBody.scaledBody.transform.position;
		Vector3 up = bodyTransform.up;
		double num = Vector3.Dot(sunVector, up);
		double num2 = Vector3.Dot(up, upAxis);
		double num3 = Math.Acos(num2);
		if (double.IsNaN(num3))
		{
			num3 = ((num2 < 0.0) ? Math.PI : 0.0);
		}
		double num4 = Math.Acos(num);
		if (double.IsNaN(num4))
		{
			num4 = ((num < 0.0) ? Math.PI : 0.0);
		}
		double num5 = (1.0 + Math.Cos(num4 - num3)) * 0.5;
		double num6 = (1.0 + Math.Cos(num4 + num3)) * 0.5;
		if (num2 < 0.0)
		{
			num = 0.0 - num;
		}
		double num7 = num3;
		double num8 = num4;
		if (num3 > Math.PI / 2.0)
		{
			num7 = Math.PI - num7;
			num8 = Math.PI - num8;
		}
		double sqrMagnitude = ((vector3d - scaledBody.transform.position) * ScaledSpace.ScaleFactor).sqrMagnitude;
		double num9 = PhysicsGlobals.SolarLuminosity / (Math.PI * 4.0 * sqrMagnitude);
		double num10 = (1.0 + (double)Vector3.Dot(sunVector, Quaternion.AngleAxis(45f * Mathf.Sign((float)rotationPeriod), up) * upAxis)) * 0.5;
		double num11 = num5 - num6;
		double num12;
		if (num11 > 0.001)
		{
			num12 = (num10 - num6) / num11;
			if (double.IsNaN(num12))
			{
				num12 = ((!(num10 > 0.5)) ? 0.0 : 1.0);
			}
		}
		else
		{
			num12 = num6 + num11 * 0.5;
		}
		if (atmosphere)
		{
			float num13 = (float)(Math.PI / 2.0 - num7);
			num13 *= 57.29578f;
			CelestialBody bodyReferencing = GetBodyReferencing(this, sunBody);
			float time = ((float)bodyReferencing.orbit.trueAnomaly * 57.29578f + 360f) % 360f;
			atmosphereTemperatureOffset = (double)latitudeTemperatureBiasCurve.Evaluate(num13) + (double)latitudeTemperatureSunMultCurve.Evaluate(num13) * num12 + (double)(axialTemperatureSunBiasCurve.Evaluate(time) * axialTemperatureSunMultCurve.Evaluate(num13)) + ((bodyReferencing.orbit.eccentricity == 0.0) ? 0.0 : ((double)eccentricityTemperatureBiasCurve.Evaluate((float)((bodyReferencing.orbit.radius - bodyReferencing.orbit.PeR) / (bodyReferencing.orbit.ApR - bodyReferencing.orbit.PeR)))));
		}
		else
		{
			atmosphereTemperatureOffset = 0.0;
		}
		if (getBodyFlux)
		{
			double num14 = 0.0;
			double num15 = 0.0;
			double num16 = 0.0;
			if (atmosphere)
			{
				double temperature = GetTemperature(0.0);
				bodyTemperature = temperature + atmosphereTemperatureOffset;
				num15 = temperature + (double)(latitudeTemperatureBiasCurve.Evaluate(90f) + axialTemperatureSunMultCurve.Evaluate(0f - (float)orbit.inclination));
				num16 = temperature + (double)(latitudeTemperatureBiasCurve.Evaluate(0f) + latitudeTemperatureSunMultCurve.Evaluate(0f) + axialTemperatureSunMultCurve.Evaluate((float)orbit.inclination));
				num14 = 1.0 - Math.Sqrt(num16) * 0.0016;
				num14 = UtilMath.Clamp01(num14);
			}
			else
			{
				double spaceTemperature = PhysicsGlobals.SpaceTemperature;
				spaceTemperature *= spaceTemperature;
				spaceTemperature *= spaceTemperature;
				double num17 = 1.0 / (PhysicsGlobals.StefanBoltzmanConstant * emissivity);
				double num18 = num9 * (1.0 - albedo) * num17;
				double num19 = Math.Sqrt(Math.Sqrt(0.25 * num18 + spaceTemperature));
				double num20 = Math.Sqrt(Math.Sqrt(num18 + spaceTemperature)) - num19;
				num16 = num19 + Math.Sqrt(num20) * 2.0;
				num15 = num19 - Math.Pow(num20, 1.1) * 1.22;
				double t = 2.0 / Math.Sqrt(Math.Sqrt(solarDayLength));
				num16 = UtilMath.Lerp(num16, num19, t);
				num15 = UtilMath.Lerp(num15, num19, t);
				double d = Math.Max(0.0, num5 * 2.0 - 1.0);
				d = Math.Sqrt(d);
				num14 = 1.0 - Math.Sqrt(num16) * 0.0016;
				num14 = UtilMath.Clamp01(num14);
				double num21 = (num16 - num15) * d;
				double num22 = num15 + num21;
				double num23 = num15 + num21 * num14;
				bodyTemperature = Math.Max(PhysicsGlobals.SpaceTemperature, num23 + (num22 - num23) * num12 + coreTemperatureOffset);
			}
			double b = UtilMath.LerpUnclamped(UtilMath.LerpUnclamped(UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.782048841, 0.87513007, num14)), UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.093081228, 0.87513007, num14)), num12), UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.398806364, 0.797612728, num14)), num5);
			double num24 = UtilMath.Lerp(bodyTemperature, b, 0.2 + altitude / Radius * 0.5);
			num24 *= num24;
			num24 *= num24;
			double num25 = Radius * Radius / ((Radius + altitude) * (Radius + altitude));
			if (num25 > 1.0)
			{
				num25 = 1.0;
			}
			bodyEmissiveFlux = PhysicsGlobals.StefanBoltzmanConstant * emissivity * num24 * num25;
			bodyAlbedoFlux = num9 * 0.5 * (sunDot + 1.0) * albedo * num25;
		}
	}

	public virtual void GetSolarAtmosphericEffects(double sunDot, double density, out double solarAirMass, out double solarFluxMultiplier)
	{
		double num = radiusAtmoFactor * sunDot;
		if (num < 0.0)
		{
			solarAirMass = Math.Sqrt(2.0 * radiusAtmoFactor + 1.0);
		}
		else
		{
			solarAirMass = Math.Sqrt(num * num + 2.0 * radiusAtmoFactor + 1.0) - num;
		}
		solarFluxMultiplier = GetSolarPowerFactor(density * solarAirMass);
	}

	public virtual double GetFullTemperature(double altitude, double atmoSunBasedTempOffset)
	{
		return GetTemperature(altitude) + (double)atmosphereTemperatureSunMultCurve.Evaluate((float)altitude) * atmoSunBasedTempOffset;
	}

	public virtual double GetFullTemperature(Vector3d pos)
	{
		double altitudeAtPos = FlightGlobals.getAltitudeAtPos(pos, this);
		Vector3d vector3d = Planetarium.fetch.Sun.scaledBody.transform.position - ScaledSpace.LocalToScaledSpace(pos);
		Vector3d upAxis = FlightGlobals.getUpAxis(this, pos);
		double sunDot = Vector3d.Dot(vector3d, upAxis);
		GetAtmoThermalStats(getBodyFlux: false, Planetarium.fetch.Sun, vector3d, sunDot, upAxis, altitudeAtPos, out var atmosphereTemperatureOffset, out var _, out var _);
		return atmosphereTemperatureOffset;
	}

	[ContextMenu("Debug Time Warp Limits")]
	public void debugTimeWarpLimits()
	{
		TimeWarp fetch = TimeWarp.fetch;
		string text = "Limits for " + Localizer.Format("#autoLOC_7001301", displayName) + ":\n";
		for (int i = 0; i < fetch.altitudeLimits.Length; i++)
		{
			text = text + fetch.warpRates[i] + "x after above " + fetch.GetAltitudeLimit(i, this).ToString("0.0") + "m\n";
		}
		MonoBehaviour.print(text);
	}

	[ContextMenu("Reset Time Warp Limits")]
	public void resetTimeWarpLimits()
	{
		TimeWarp fetch = TimeWarp.fetch;
		timeWarpAltitudeLimits = new float[fetch.altitudeLimits.Length];
		for (int i = 0; i < fetch.altitudeLimits.Length; i++)
		{
			timeWarpAltitudeLimits[i] = fetch.altitudeLimits[i] * (float)Radius * GameSettings.ORBIT_WARP_ALTMODE_LIMIT_MODIFIER;
		}
		debugTimeWarpLimits();
	}

	public bool HasParent(CelestialBody body)
	{
		if (referenceBody == body)
		{
			return true;
		}
		if (referenceBody == this)
		{
			return false;
		}
		return referenceBody.HasParent(body);
	}

	public bool HasChild(CelestialBody body)
	{
		int num = 0;
		while (true)
		{
			if (num < orbitingBodies.Count)
			{
				CelestialBody celestialBody = orbitingBodies[num];
				if (!(celestialBody == body))
				{
					if (celestialBody.HasChild(body))
					{
						break;
					}
					num++;
					continue;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public Transform GetTransform()
	{
		return bodyTransform;
	}

	public Vector3 GetObtVelocity()
	{
		if (PhysicsGlobals.CelestialBodyTargetingMode != VesselTargetModes.Direction && PhysicsGlobals.CelestialBodyTargetingMode != 0 && orbit != null)
		{
			return orbit.GetRelativeVel();
		}
		return Vector3d.zero;
	}

	public Vector3 GetSrfVelocity()
	{
		if (PhysicsGlobals.CelestialBodyTargetingMode != VesselTargetModes.Direction && PhysicsGlobals.CelestialBodyTargetingMode != 0 && orbit != null)
		{
			return orbit.GetVel() - FlightGlobals.currentMainBody.getRFrmVel(bodyTransform.position);
		}
		return Vector3d.zero;
	}

	public Vector3 GetFwdVector()
	{
		return bodyTransform.forward;
	}

	public Vessel GetVessel()
	{
		return null;
	}

	public string GetName()
	{
		return bodyName;
	}

	public string GetDisplayName()
	{
		return displayName;
	}

	public Orbit GetOrbit()
	{
		return orbit;
	}

	public OrbitDriver GetOrbitDriver()
	{
		return orbitDriver;
	}

	public VesselTargetModes GetTargetingMode()
	{
		return PhysicsGlobals.CelestialBodyTargetingMode;
	}

	public bool GetActiveTargetable()
	{
		return false;
	}

	public void SetResourceMap(Texture2D map)
	{
		resourceMap = map;
	}

	public void HideSurfaceResource()
	{
		if (resourceMap != null)
		{
			UnityEngine.Object.DestroyImmediate(resourceMap);
		}
	}

	public string RevealName()
	{
		return name;
	}

	public string RevealDisplayName()
	{
		return displayName;
	}

	public double RevealSpeed()
	{
		return orbit.GetRelativeVel().magnitude;
	}

	public double RevealAltitude()
	{
		return orbit.altitude;
	}

	public string RevealSituationString()
	{
		if (!(orbit.eccentricity < 1.0))
		{
			return Localizer.Format("#autoLoc_6002176", referenceBody.displayName);
		}
		return Localizer.Format("#autoLOC_193182", referenceBody.displayName);
	}

	public string RevealType()
	{
		if (double.IsInfinity(sphereOfInfluence))
		{
			return cacheAutoLOC_193189;
		}
		if (double.IsInfinity(referenceBody.sphereOfInfluence))
		{
			return cacheAutoLOC_193194;
		}
		return cacheAutoLOC_193198;
	}

	public float RevealMass()
	{
		return (float)Mass;
	}

	public void PreciseUpdateQuadPositions()
	{
		if ((bool)pqsController)
		{
			pqsController.PreciseUpdateQuadsPosition();
		}
	}

	public override bool Equals(object other)
	{
		CelestialBody celestialBody = other as CelestialBody;
		if (celestialBody == null)
		{
			return false;
		}
		return bodyName.Equals(celestialBody.bodyName);
	}

	public override int GetHashCode()
	{
		return bodyName.GetHashCode_Net35();
	}

	public void SetupShadowCasting()
	{
		if (!isStar)
		{
			MeshRenderer component = scaledBody.transform.GetComponent<MeshRenderer>();
			if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
			{
				component.shadowCastingMode = ShadowCastingMode.On;
				component.receiveShadows = true;
			}
			else
			{
				component.shadowCastingMode = ShadowCastingMode.Off;
				component.receiveShadows = false;
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_193189 = Localizer.Format("#autoLOC_193189");
		cacheAutoLOC_193194 = Localizer.Format("#autoLOC_193194");
		cacheAutoLOC_193198 = Localizer.Format("#autoLOC_193198");
	}
}
