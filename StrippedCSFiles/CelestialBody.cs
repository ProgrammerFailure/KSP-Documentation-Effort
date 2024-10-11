using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSPAchievements;
using UnityEngine;

[SelectionBase]
public class CelestialBody : MonoBehaviour, ITargetable, IDiscoverable
{
	public string bodyName;

	public string bodyDisplayName;

	public string bodyAdjectiveDisplayName;

	public string bodyDescription;

	public double GeeASL;

	public double Radius;

	public double Mass;

	public double Density;

	public double SurfaceArea;

	public double gravParameter;

	public double sphereOfInfluence;

	public double hillSphere;

	public double gMagnitudeAtCenter;

	public double atmDensityASL;

	public double atmPressureASL;

	public bool scaledEllipsoid;

	public Vector3d scaledElipRadMult;

	public double scaledRadiusHorizonMultiplier;

	public double navballSwitchRadiusMult;

	public double navballSwitchRadiusMultLow;

	[Obsolete("theName and use_The_InName are deprecated and will be removed in future updates, please use name or displayName instead.")]
	public bool use_The_InName;

	public bool isHomeWorld;

	public bool isCometPerturber;

	public bool ocean;

	public bool oceanUseFog;

	public double oceanFogPQSDepth;

	public float oceanFogPQSDepthRecip;

	public float oceanFogDensityStart;

	public float oceanFogDensityEnd;

	public float oceanFogDensityPQSMult;

	public float oceanFogDensityAltScalar;

	public float oceanFogDensityExponent;

	public Color oceanFogColorStart;

	public Color oceanFogColorEnd;

	public float oceanFogDawnFactor;

	public float oceanSkyColorMult;

	public float oceanSkyColorOpacityBase;

	public float oceanSkyColorOpacityAltMult;

	public double oceanDensity;

	public float oceanAFGBase;

	public float oceanAFGAltMult;

	public float oceanAFGMin;

	public float oceanSunBase;

	public float oceanSunAltMult;

	public float oceanSunMin;

	public bool oceanAFGLerp;

	public float oceanMinAlphaFogDistance;

	public float oceanMaxAlbedoFog;

	public float oceanMaxAlphaFog;

	public float oceanAlbedoDistanceScalar;

	public float oceanAlphaDistanceScalar;

	public double minOrbitalDistance;

	public bool atmosphere;

	public bool atmosphereContainsOxygen;

	public double atmosphereDepth;

	public double atmosphereTemperatureSeaLevel;

	public double atmospherePressureSeaLevel;

	public double atmosphereMolarMass;

	public double atmosphereAdiabaticIndex;

	public double atmosphereTemperatureLapseRate;

	public double atmosphereGasMassLapseRate;

	public bool atmosphereUseTemperatureCurve;

	public bool atmosphereTemperatureCurveIsNormalized;

	public FloatCurve atmosphereTemperatureCurve;

	public FloatCurve latitudeTemperatureBiasCurve;

	public FloatCurve latitudeTemperatureSunMultCurve;

	public FloatCurve axialTemperatureSunMultCurve;

	public FloatCurve axialTemperatureSunBiasCurve;

	public FloatCurve atmosphereTemperatureSunMultCurve;

	public double maxAxialDot;

	public FloatCurve eccentricityTemperatureBiasCurve;

	public double albedo;

	public double emissivity;

	public double coreTemperatureOffset;

	public double convectionMultiplier;

	public double shockTemperatureMultiplier;

	public double bodyTemperature;

	public bool atmosphereUsePressureCurve;

	public bool atmospherePressureCurveIsNormalized;

	public FloatCurve atmospherePressureCurve;

	public double radiusAtmoFactor;

	public bool hasSolidSurface;

	public bool isStar;

	public Vector3d transformRight;

	public Vector3d transformUp;

	private Vector3d _position;

	public QuaternionD rotation;

	public OrbitDriver orbitDriver;

	public PQS pqsController;

	public PQSSurfaceObject[] pqsSurfaceObjects;

	public GameObject scaledBody;

	public AtmosphereFromGround afg;

	private MapObject mapObject;

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

	public bool clampInverseRotThreshold;

	public bool inverseRotation;

	public float inverseRotThresholdAltitude;

	public double angularV;

	public float[] timeWarpAltitudeLimits;

	public Color atmosphericAmbientColor;

	public List<CelestialBody> orbitingBodies;

	public Planetarium.CelestialFrame BodyFrame;

	public CelestialBodySubtree progressTree;

	public CelestialBodyType bodyType;

	public CelestialBodyScienceParams scienceValues;

	public CBAttributeMapSO BiomeMap;

	public MiniBiome[] MiniBiomes;

	public Transform bodyTransform;

	private Vector3d rPos;

	private double latValue;

	private double lonValue;

	protected const double bodyEmissiveScalarS0Front = 0.782048841;

	protected const double bodyEmissiveScalarS0Back = 0.093081228;

	protected const double bodyEmissiveScalarS1 = 0.87513007;

	protected const double bodyEmissiveScalarS0Top = 0.398806364;

	protected const double bodyEmissiveScalarS1Top = 0.797612728;

	private Texture2D resourceMap;

	private DiscoveryInfo dscInfo;

	private static string cacheAutoLOC_193189;

	private static string cacheAutoLOC_193194;

	private static string cacheAutoLOC_439019;

	private static string cacheAutoLOC_193198;

	public string displayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete("theName and use_The_InName are deprecated and will be removed in future updates, please use name or displayName instead.")]
	public string theName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public new string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int flightGlobalsIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public Vector3d position
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Orbit orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MapObject MapObject
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public CelestialBody referenceBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Texture2D ResourceMap
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DiscoveryInfo DiscoveryInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3d RotationAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupConstants()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetPressure(double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetPressureAtm(double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetTemperature(double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetDensity(double pressure, double temperature)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSpeedOfSound(double pressure, double density)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSolarPowerFactor(double density)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CBUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Bounds getBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetFrameVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetFrameVelAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRFrmVel(Vector3d worldPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRFrmVelOrbit(Orbit o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getTruePositionAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelSurfaceNVector(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetSurfaceNVector(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelSurfacePosition(double lat, double lon, double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelSurfacePosition(double lat, double lon, double alt, out Vector3d normal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelSurfacePosition(Vector3d worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelSurfaceDirection(Vector3d worldDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetWorldSurfacePosition(double lat, double lon, double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetLatitude(Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetLongitude(Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2d GetLatitudeAndLongitude(Vector3d pos, bool isRadial = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetImpactLatitudeAndLongitude(Vector3d position, Vector3d velocity, out double latitude, out double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetAltitude(Vector3d worldPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetLatLonAlt(Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetLatLonAltOrbital(Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetRandomLatitudeAndLongitude(ref double latitude, ref double longitude, bool waterAllowed = false, bool spaceCenterAllowed = false, KSPRandom generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TerrainAltitude(double latitude, double longitude, bool allowNegative = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CelestialBody GetBodyReferencing(CelestialBody body, CelestialBody sun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetAtmoThermalStats(bool getBodyFlux, CelestialBody sunBody, Vector3d sunVector, double sunDot, Vector3d upAxis, double altitude, out double atmosphereTemperatureOffset, out double bodyEmissiveFlux, out double bodyAlbedoFlux)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetSolarAtmosphericEffects(double sunDot, double density, out double solarAirMass, out double solarFluxMultiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetFullTemperature(double altitude, double atmoSunBasedTempOffset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetFullTemperature(Vector3d pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Debug Time Warp Limits")]
	public void debugTimeWarpLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Time Warp Limits")]
	public void resetTimeWarpLimits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasParent(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasChild(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetObtVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetSrfVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetFwdVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit GetOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitDriver GetOrbitDriver()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselTargetModes GetTargetingMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetActiveTargetable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetResourceMap(Texture2D map)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideSurfaceResource()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RevealAltitude()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealSituationString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string RevealType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float RevealMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void PreciseUpdateQuadPositions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupShadowCasting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
