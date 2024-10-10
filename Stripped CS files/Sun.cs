using System;
using UnityEngine;

public class Sun : MonoBehaviour
{
	public static Sun Instance;

	public Transform target;

	public CelestialBody sun;

	public LensFlare sunFlare;

	public double double_0;

	public Vector3d sunDirection;

	public Vector3d sunRotation;

	public AnimationCurve brightnessCurve;

	public float brightnessMultiplier = 1f;

	public bool useLocalSpaceSunLight;

	public Light scaledSunLight;

	public float localTime;

	public float fadeStart;

	public float fadeEnd = 0.1f;

	public CelestialBody mainBody;

	public double targetAltitude;

	public double horizonDistance;

	public double horizonAngle;

	public float horizonScalar;

	public float dayNightRatio;

	public float fadeEndAtAlt;

	public float fadeStartAtAlt;

	public float shadowBiasSpaceCentre = 1f;

	public float shadowBiasFlight = 0.125f;

	public float shadowNmlBiasZero = 0.4f;

	public float shadowNmlBiasMid = 1f;

	public float shadowNmlBiasFar = 2f;

	public float shadowNmlBiasMidDist = 2000f;

	public float shadowNmlBiasFarDist = 15000f;

	public int sunRotationPrecision = 12;

	public int sunRotationPrecisionMapView = 12;

	public int sunRotationPrecisionDefault = 12;

	public Light lgt;

	public float showCascadeZeroZoomed = 0.01f;

	public float showCascadeOneZoomed = 0.02f;

	public float shadowCascadeCamDist = 20f;

	public float showCascadeZeroMidRange = 0.02f;

	public float showCascadeOneMidRange = 0.05f;

	public float shadowCascadeCamDistMidRange = 500f;

	[SerializeField]
	public float camDistance;

	[SerializeField]
	public bool processSunBias = true;

	[SerializeField]
	public bool processShadowCascades = true;

	public Light sunLight
	{
		get
		{
			return lgt;
		}
		set
		{
		}
	}

	public virtual void Awake()
	{
		Instance = this;
		lgt = GetComponent<Light>();
	}

	public virtual void Start()
	{
		if (useLocalSpaceSunLight)
		{
			scaledSunLight = new GameObject("Scaledspace SunLight").AddComponent<Light>();
			scaledSunLight.type = LightType.Directional;
			scaledSunLight.intensity = lgt.intensity;
			scaledSunLight.color = lgt.color;
			scaledSunLight.transform.parent = base.transform;
			scaledSunLight.transform.localPosition = Vector3.zero;
			scaledSunLight.transform.localRotation = Quaternion.identity;
			scaledSunLight.cullingMask = 1024;
			lgt.cullingMask ^= 0x400;
			lgt.enabled = true;
			GameEvents.onGameSceneLoadRequested.Add(GameSceneLoadRequested);
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(GameSceneLoadRequested);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LateUpdate()
	{
		sunDirection = ((Vector3d)target.position - ScaledSpace.LocalToScaledSpace(sun.position)).normalized;
		if (MapView.MapIsEnabled)
		{
			sunRotationPrecision = sunRotationPrecisionMapView;
		}
		else
		{
			sunRotationPrecision = sunRotationPrecisionDefault;
		}
		sunRotation = sunDirection;
		sunRotation.x = Math.Round(sunRotation.x, sunRotationPrecision);
		sunRotation.y = Math.Round(sunRotation.y, sunRotationPrecision);
		sunRotation.z = Math.Round(sunRotation.z, sunRotationPrecision);
		base.transform.forward = sunRotation;
		sunFlare.brightness = brightnessMultiplier * brightnessCurve.Evaluate((float)(1.0 / (Vector3d.Distance(target.position, ScaledSpace.LocalToScaledSpace(sun.position)) / (double_0 * (double)ScaledSpace.InverseScaleFactor))));
		if (useLocalSpaceSunLight)
		{
			Vector3d position = ScaledSpace.ScaledToLocalSpace(target.position);
			mainBody = FlightGlobals.currentMainBody;
			if (mainBody != null && mainBody != sun)
			{
				targetAltitude = FlightGlobals.getAltitudeAtPos(position, mainBody);
				if (targetAltitude < 0.0)
				{
					targetAltitude = 0.0;
				}
				horizonAngle = Math.Acos(mainBody.Radius / (mainBody.Radius + targetAltitude));
				horizonScalar = 0f - Mathf.Sin((float)horizonAngle);
				dayNightRatio = 1f - Mathf.Abs(horizonScalar);
				fadeStartAtAlt = horizonScalar + fadeStart * dayNightRatio;
				fadeEndAtAlt = horizonScalar - fadeEnd * dayNightRatio;
				localTime = Vector3.Dot(-FlightGlobals.getUpAxis(position), base.transform.forward);
				lgt.intensity = Mathf.Lerp(0f, scaledSunLight.intensity, Mathf.InverseLerp(fadeEndAtAlt, fadeStartAtAlt, localTime));
			}
			else
			{
				localTime = 1f;
				lgt.intensity = scaledSunLight.intensity;
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (GameSettings.GraphicsVersion == GameSettings.GraphicsType.D3D11)
			{
				camDistance = FlightCamera.fetch.Distance;
				if (processSunBias)
				{
					if (camDistance <= shadowNmlBiasMidDist)
					{
						float t = Mathf.InverseLerp(0f, shadowNmlBiasMidDist, camDistance);
						float shadowNormalBias = Mathf.Lerp(shadowNmlBiasZero, shadowNmlBiasMid, t);
						lgt.shadowNormalBias = shadowNormalBias;
					}
					else if (camDistance <= shadowNmlBiasFarDist)
					{
						float t2 = Mathf.InverseLerp(shadowNmlBiasMidDist, shadowNmlBiasFarDist, camDistance);
						float shadowNormalBias2 = Mathf.Lerp(shadowNmlBiasMid, shadowNmlBiasFar, t2);
						lgt.shadowNormalBias = shadowNormalBias2;
					}
					else
					{
						lgt.shadowNormalBias = shadowNmlBiasFar;
					}
				}
				if (!processShadowCascades)
				{
					return;
				}
				if (camDistance < shadowCascadeCamDistMidRange)
				{
					bool flag = camDistance < shadowCascadeCamDist;
					switch (QualitySettings.GetQualityLevel())
					{
					case 2:
						QualitySettings.shadowCascades = 2;
						QualitySettings.shadowCascade2Split = (flag ? showCascadeZeroZoomed : showCascadeZeroMidRange);
						break;
					case 3:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = (flag ? new Vector3(showCascadeZeroZoomed, showCascadeOneZoomed, DynamicShadowSettings.Instance.Flight.cascadeGood.z) : new Vector3(showCascadeZeroMidRange, showCascadeOneMidRange, DynamicShadowSettings.Instance.Flight.cascadeGood.z));
						break;
					default:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = (flag ? new Vector3(showCascadeZeroZoomed, showCascadeOneZoomed, DynamicShadowSettings.Instance.Flight.cascadeBeautiful.z) : new Vector3(showCascadeZeroMidRange, showCascadeOneMidRange, DynamicShadowSettings.Instance.Flight.cascadeBeautiful.z));
						break;
					case 5:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = (flag ? new Vector3(showCascadeZeroZoomed, showCascadeOneZoomed, DynamicShadowSettings.Instance.Flight.cascadeFantastic.z) : new Vector3(showCascadeZeroMidRange, showCascadeOneMidRange, DynamicShadowSettings.Instance.Flight.cascadeFantastic.z));
						break;
					case 0:
					case 1:
						break;
					}
				}
				else
				{
					switch (QualitySettings.GetQualityLevel())
					{
					case 2:
						QualitySettings.shadowCascades = 2;
						QualitySettings.shadowCascade2Split = DynamicShadowSettings.Instance.Flight.cascadeSimple;
						break;
					case 3:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = DynamicShadowSettings.Instance.Flight.cascadeGood;
						break;
					default:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = DynamicShadowSettings.Instance.Flight.cascadeBeautiful;
						break;
					case 5:
						QualitySettings.shadowCascades = 4;
						QualitySettings.shadowCascade4Split = DynamicShadowSettings.Instance.Flight.cascadeFantastic;
						break;
					case 0:
					case 1:
						break;
					}
				}
			}
			else
			{
				lgt.shadowNormalBias = shadowNmlBiasZero;
			}
		}
		else
		{
			lgt.shadowNormalBias = 0.9f;
		}
	}

	public void SunlightEnabled(bool state)
	{
		lgt.enabled = state;
		if (useLocalSpaceSunLight)
		{
			scaledSunLight.enabled = state;
		}
		sunFlare.enabled = false;
	}

	public virtual double GetLocalTimeAtPosition(Vector3d wPos, CelestialBody cb)
	{
		Vector3d upAxis = FlightGlobals.getUpAxis(cb, wPos);
		Vector3d vector3d = Vector3d.Exclude(cb.angularVelocity, upAxis);
		Vector3d fromThat = Planetarium.fetch.Sun.position - cb.position;
		Vector3d vector3d2 = Vector3d.Exclude(cb.angularVelocity, fromThat);
		double num = (double)((Vector3d.Dot(Vector3d.Cross(vector3d2, vector3d), cb.angularVelocity) >= 0.0) ? 1 : (-1)) * UtilMath.AngleBetween(vector3d, vector3d2);
		num = num / (Math.PI * 2.0) + 0.5;
		if (num > Math.PI * 2.0)
		{
			num -= Math.PI * 2.0;
		}
		return num;
	}

	public virtual double GetLocalTimeAtPosition(double latitude, double longitude, CelestialBody body)
	{
		if (body == null)
		{
			return 0.0;
		}
		return GetLocalTimeAtPosition(body.GetWorldSurfacePosition(latitude, longitude, body.Radius), body);
	}

	public virtual double GetLocalTimeAtPosition(double latitude, double longitude, double altitude, CelestialBody body)
	{
		if (body == null)
		{
			return 0.0;
		}
		return GetLocalTimeAtPosition(body.GetWorldSurfacePosition(latitude, longitude, altitude), body);
	}

	public void GameSceneLoadRequested(GameScenes scene)
	{
		if (scene == GameScenes.SPACECENTER)
		{
			lgt.shadowBias = shadowBiasSpaceCentre;
		}
		else
		{
			lgt.shadowBias = shadowBiasFlight;
		}
	}
}
