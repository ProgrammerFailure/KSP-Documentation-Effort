using System;
using CameraFXModules;
using UnityEngine;

public class AerodynamicsFX : MonoBehaviour
{
	public FXCamera fxCamera;

	public FXDepthCamera fxDepthCamera;

	public Light fxLight;

	public AudioSource airspeedNoise;

	public AeroFXState Condensation;

	public AeroFXState ReentryHeat;

	public Vector3 velocity;

	public double densityFactor;

	public double airDensity;

	public double airSpeed;

	public float machNumber;

	public double airTemp;

	public double densityCutoffMultiplier = 667.0;

	public double heatFlux;

	public float FxScalar;

	public double p0 = 8000000.0;

	public float state;

	public float transitionFade;

	public int detailLevel;

	public Wobble machCameraFX;

	public Wobble reentryCameraFX;

	public WobbleFXParams cameraFXmach;

	public WobbleFXParams cameraFXreentry;

	public Camera _fxcamera_camera;

	public void Awake()
	{
		GameEvents.onGamePause.Add(OnPause);
		GameEvents.onGameUnpause.Add(OnUnpause);
		GameEvents.OnGameSettingsApplied.Add(OnSettingsUpdate);
		machCameraFX = new Wobble("MachFX", 0f, 30f, WobbleModes.All, Views.FlightExternal | Views.FlightInternal, GetInstanceID(), cameraFXmach.fxPars.rotFactor, cameraFXmach.fxPars.linFactor);
		reentryCameraFX = new Wobble("ReentryFX", 0f, 30f, WobbleModes.All, Views.FlightExternal | Views.FlightInternal, GetInstanceID(), cameraFXreentry.fxPars.rotFactor, cameraFXreentry.fxPars.linFactor);
	}

	public void OnDestroy()
	{
		if (CameraFX.Instance != null)
		{
			CameraFX.Instance.cameraFXCollection_0.RemoveFX(machCameraFX);
		}
		GameEvents.onGamePause.Remove(OnPause);
		GameEvents.onGameUnpause.Remove(OnUnpause);
		GameEvents.OnGameSettingsApplied.Remove(OnSettingsUpdate);
	}

	public void Start()
	{
		OnSettingsUpdate();
		if ((bool)fxCamera)
		{
			fxCamera.transform.parent = FlightCamera.fetch.mainCamera.transform;
			fxCamera.transform.localPosition = Vector3.zero;
			fxCamera.transform.localRotation = Quaternion.identity;
			if (CameraFX.Instance != null)
			{
				CameraFX.Instance.cameraFXCollection_0.AddFX(machCameraFX);
			}
		}
		if ((bool)fxDepthCamera)
		{
			fxDepthCamera.transform.parent = FlightCamera.fetch.mainCamera.transform;
			fxDepthCamera.transform.localPosition = Vector3.zero;
			fxDepthCamera.transform.localRotation = Quaternion.identity;
		}
	}

	public void OnSettingsUpdate()
	{
		detailLevel = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) ? GameSettings.AERO_FX_QUALITY : 0);
		densityCutoffMultiplier = Math.Ceiling(1.0 / PhysicsGlobals.AeroFXDensityFadeStart);
	}

	public void Update()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (FlightGlobals.ready && activeVessel.state != Vessel.State.DEAD && activeVessel.atmDensity > 0.0 && !activeVessel.packed)
		{
			velocity = activeVessel.srf_velocity;
			if (velocity == Vector3.zero)
			{
				velocity = activeVessel.transform.up;
			}
			airTemp = activeVessel.externalTemperature;
			densityFactor = (airDensity = activeVessel.atmDensity);
			if (densityFactor < PhysicsGlobals.AeroFXDensityFadeStart)
			{
				densityFactor = UtilMath.Lerp(0.0, densityFactor, densityFactor * densityCutoffMultiplier);
			}
			densityFactor = Math.Pow(densityFactor, PhysicsGlobals.AeroFXDensityExponent1) * PhysicsGlobals.AeroFXDensityScalar1 + Math.Pow(densityFactor, PhysicsGlobals.AeroFXDensityExponent2) * PhysicsGlobals.AeroFXDensityScalar2;
			airSpeed = activeVessel.speed;
			machNumber = (float)activeVessel.mach;
			state = Mathf.InverseLerp(PhysicsGlobals.AeroFXStartThermalFX, PhysicsGlobals.AeroFXFullThermalFX, machNumber);
		}
		else
		{
			double num = 0.0;
			densityFactor = 0.0;
			airSpeed = num;
			float num2 = 0f;
			state = 0f;
			machNumber = num2;
		}
	}

	public void LateUpdate()
	{
		if (densityFactor * airSpeed > 0.0)
		{
			velocity.Normalize();
			heatFlux = 0.5 * densityFactor * Math.Pow(airSpeed, PhysicsGlobals.AeroFXVelocityExponent);
			FxScalar = (float)Math.Min(1.0, (heatFlux - p0) / (5.0 * p0));
			state = Mathf.Clamp01(state);
			if (state < 1f)
			{
				float num = (float)airDensity;
				if (num < PhysicsGlobals.AeroFXMachFXFadeStart)
				{
					float a = 0f;
					if (num > PhysicsGlobals.AeroFXMachFXFadeEnd)
					{
						a = (PhysicsGlobals.AeroFXMachFXFadeEnd - num) / (PhysicsGlobals.AeroFXMachFXFadeEnd - PhysicsGlobals.AeroFXMachFXFadeStart);
					}
					FxScalar *= Mathf.Lerp(a, 1f, state);
				}
			}
		}
		else
		{
			FxScalar = 0f;
		}
		if ((bool)fxCamera)
		{
			if (FxScalar > 0f && !MapView.MapIsEnabled && detailLevel > 0 && !PhysicsGlobals.ThermalColorsDebug)
			{
				if (!fxCamera.GetComponentCached(ref _fxcamera_camera).enabled)
				{
					fxCamera.GetComponentCached(ref _fxcamera_camera).enabled = true;
				}
				if (!fxDepthCamera.depthCamera.enabled)
				{
					fxDepthCamera.depthCamera.enabled = true;
				}
				fxCamera.shaderLOD = Mathf.Min(fxCamera.ReplacementShaders.Length - detailLevel, fxCamera.ReplacementShaders.Length - 1);
				fxCamera.effectDirection = -velocity;
				fxCamera.color = Color.Lerp(Condensation.color.GetLerp(FxScalar), ReentryHeat.color.GetLerp(FxScalar), state);
				fxCamera.color.a = 1f;
				fxCamera.length = Mathf.Lerp(Condensation.length.GetLerp(FxScalar), ReentryHeat.length.GetLerp(FxScalar), state);
				fxCamera.edgeFade = Mathf.Lerp(Condensation.edgeFade.GetLerp(FxScalar), ReentryHeat.edgeFade.GetLerp(FxScalar), state);
				fxCamera.falloff1 = Mathf.Lerp(Condensation.falloff1.GetLerp(FxScalar), ReentryHeat.falloff1.GetLerp(FxScalar), state);
				fxCamera.falloff2 = Mathf.Lerp(Condensation.falloff2.GetLerp(FxScalar), ReentryHeat.falloff2.GetLerp(FxScalar), state);
				fxCamera.falloff3 = Mathf.Lerp(Condensation.falloff3.GetLerp(FxScalar), ReentryHeat.falloff3.GetLerp(FxScalar), state);
				fxCamera.intensity = Mathf.Lerp(Condensation.intensity.GetLerp(FxScalar), ReentryHeat.intensity.GetLerp(FxScalar), state);
				fxCamera.wobble = Mathf.Lerp(Condensation.wobble.GetLerp(FxScalar), ReentryHeat.wobble.GetLerp(FxScalar), state);
			}
			else
			{
				if (fxCamera.GetComponentCached(ref _fxcamera_camera).enabled)
				{
					fxCamera.GetComponentCached(ref _fxcamera_camera).enabled = false;
				}
				if (fxDepthCamera.depthCamera.enabled)
				{
					fxDepthCamera.depthCamera.enabled = false;
				}
			}
		}
		if ((bool)fxLight)
		{
			if (FxScalar > 0f && !MapView.MapIsEnabled)
			{
				if (!fxLight.enabled)
				{
					fxLight.enabled = true;
				}
				switch (detailLevel)
				{
				default:
					fxLight.shadows = LightShadows.Soft;
					break;
				case 0:
				case 1:
					fxLight.shadows = LightShadows.Hard;
					break;
				}
				fxLight.transform.forward = -velocity;
				fxLight.color = Color.Lerp(Condensation.color.GetLerp(FxScalar), ReentryHeat.color.GetLerp(FxScalar), state);
				fxLight.intensity = Mathf.Lerp(Condensation.lightPower.GetLerp(FxScalar), ReentryHeat.lightPower.GetLerp(FxScalar), state);
			}
			else if (fxLight.enabled)
			{
				fxLight.enabled = false;
			}
		}
		if ((bool)airspeedNoise)
		{
			if (FxScalar > 0f && !MapView.MapIsEnabled)
			{
				if (!airspeedNoise.enabled)
				{
					airspeedNoise.enabled = true;
				}
				airspeedNoise.volume = Mathf.Lerp(Condensation.airspeedNoiseVolume.GetLerp(FxScalar), ReentryHeat.airspeedNoiseVolume.GetLerp(FxScalar), state);
				airspeedNoise.volume *= GameSettings.AMBIENCE_VOLUME;
				airspeedNoise.pitch = Mathf.Lerp(Condensation.airspeedNoisePitch.GetLerp(FxScalar), ReentryHeat.airspeedNoisePitch.GetLerp(FxScalar), state);
			}
			else if (airspeedNoise.enabled)
			{
				airspeedNoise.enabled = false;
			}
		}
		if (machCameraFX != null)
		{
			if (FxScalar > 0f && !MapView.MapIsEnabled && FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
			{
				machCameraFX.amplitude = Mathf.Lerp(cameraFXmach.amplitude.min, cameraFXmach.amplitude.max, Mathf.Pow(FxScalar * state, cameraFXreentry.slope));
				machCameraFX.frequency = Mathf.Lerp(cameraFXmach.frequency.min, cameraFXmach.frequency.max, Mathf.Pow(FxScalar * state, cameraFXmach.slope));
			}
			else
			{
				machCameraFX.amplitude = 0f;
			}
		}
		if (reentryCameraFX != null)
		{
			if (FxScalar > 0f && !MapView.MapIsEnabled && FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
			{
				reentryCameraFX.amplitude = Mathf.Lerp(cameraFXreentry.amplitude.min, cameraFXreentry.amplitude.max, Mathf.Pow(FxScalar * (1f - state), cameraFXreentry.slope));
				reentryCameraFX.frequency = Mathf.Lerp(cameraFXreentry.frequency.min, cameraFXreentry.frequency.max, Mathf.Pow(FxScalar * (1f - state), cameraFXreentry.slope));
			}
			else
			{
				reentryCameraFX.amplitude = 0f;
			}
		}
	}

	public void OnPause()
	{
		if ((bool)airspeedNoise && airspeedNoise.enabled)
		{
			airspeedNoise.Stop();
		}
	}

	public void OnUnpause()
	{
		if ((bool)airspeedNoise && airspeedNoise.enabled)
		{
			airspeedNoise.Play();
		}
	}
}
