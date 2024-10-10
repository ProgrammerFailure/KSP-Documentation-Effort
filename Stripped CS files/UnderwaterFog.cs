using System.Collections;
using UnityEngine;

public class UnderwaterFog : MonoBehaviour
{
	public bool oldFog;

	public FogMode oldFogMode;

	public float oldFogDensity;

	public Color oldFogColor;

	public float oldSunBrightnessMult;

	public float oldSunFlareBrightnessMult;

	public float alt;

	public Vector4 camVectorPos;

	public Vector4 camVectorDir;

	public float lastAlt;

	public bool isScaled;

	public bool foundAfg;

	public bool fxOn = true;

	public bool isPQS;

	public int ShaderGlobalCamVectorPos;

	public int ShaderGlobalCamVectorDir;

	public int ShaderGlobal_UnderwaterFogColor;

	public int ShaderGlobal_UnderwaterMinAlphaFogDistance;

	public int ShaderGlobal_UnderwaterMaxAlbedoFog;

	public int ShaderGlobal_UnderwaterMaxAlphaFog;

	public int ShaderGlobal_UnderwaterAlbedoDistanceScalar;

	public int ShaderGlobal_UnderwaterAlphaDistanceScalar;

	public CelestialBody curBody;

	public void Awake()
	{
		ShaderGlobalCamVectorPos = Shader.PropertyToID("_LocalCameraPos");
		ShaderGlobalCamVectorDir = Shader.PropertyToID("_LocalCameraDir");
		ShaderGlobal_UnderwaterFogColor = Shader.PropertyToID("_UnderwaterFogColor");
		ShaderGlobal_UnderwaterMinAlphaFogDistance = Shader.PropertyToID("_UnderwaterMinAlphaFogDistance");
		ShaderGlobal_UnderwaterMaxAlbedoFog = Shader.PropertyToID("_UnderwaterMaxAlbedoFog");
		ShaderGlobal_UnderwaterMaxAlphaFog = Shader.PropertyToID("_UnderwaterMaxAlphaFog");
		ShaderGlobal_UnderwaterAlbedoDistanceScalar = Shader.PropertyToID("_UnderwaterAlbedoDistanceScalar");
		ShaderGlobal_UnderwaterAlphaDistanceScalar = Shader.PropertyToID("_UnderwaterAlphaDistanceScalar");
		if (!isPQS)
		{
			Shader.SetGlobalVector(ShaderGlobalCamVectorPos, camVectorPos);
			Shader.SetGlobalVector(ShaderGlobalCamVectorDir, camVectorDir);
			GameEvents.onVesselSOIChanged.Add(VesselSOIChange);
			GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
			SetDefaultCB();
		}
	}

	public IEnumerator Start()
	{
		if (!isPQS)
		{
			yield return null;
			SetDefaultCB();
		}
	}

	public void OnDestroy()
	{
		if (!isPQS)
		{
			GameEvents.onVesselSOIChanged.Remove(VesselSOIChange);
			GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		}
	}

	public void OnSceneChange(GameScenes scene)
	{
		UpdateCBProperties(Planetarium.fetch.Home);
		UpdateShaderPropertiesDefault();
	}

	public void VesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromTo)
	{
		if (!isPQS)
		{
			UpdateCBProperties(fromTo.to);
		}
	}

	public void SetDefaultCB()
	{
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.currentMainBody != null)
		{
			UpdateCBProperties(FlightGlobals.currentMainBody);
		}
		else if (Planetarium.fetch != null && Planetarium.fetch.Home != null)
		{
			UpdateCBProperties(Planetarium.fetch.Home);
		}
		UpdateShaderPropertiesDefault();
	}

	public void UpdateShaderPropertiesDefault()
	{
		camVectorPos = Vector4.one;
		ref Vector4 reference = ref camVectorPos;
		float w = float.PositiveInfinity;
		alt = float.PositiveInfinity;
		reference.w = w;
		camVectorDir = Vector4.one;
		camVectorDir.w = 0f;
		lastAlt = 0f;
		UpdateShaderProperties();
	}

	public void UpdateShaderProperties()
	{
		Shader.SetGlobalVector(ShaderGlobalCamVectorPos, camVectorPos);
		Shader.SetGlobalVector(ShaderGlobalCamVectorDir, camVectorDir);
	}

	public void UpdateCBProperties(CelestialBody body)
	{
		curBody = body;
		if (!(body == null))
		{
			Shader.SetGlobalColor(ShaderGlobal_UnderwaterFogColor, body.oceanFogColorStart);
			Shader.SetGlobalFloat(ShaderGlobal_UnderwaterMinAlphaFogDistance, body.oceanMinAlphaFogDistance);
			Shader.SetGlobalFloat(ShaderGlobal_UnderwaterMaxAlbedoFog, body.oceanMaxAlbedoFog);
			Shader.SetGlobalFloat(ShaderGlobal_UnderwaterMaxAlphaFog, body.oceanMaxAlphaFog);
			Shader.SetGlobalFloat(ShaderGlobal_UnderwaterAlbedoDistanceScalar, body.oceanAlbedoDistanceScalar);
			Shader.SetGlobalFloat(ShaderGlobal_UnderwaterAlphaDistanceScalar, body.oceanAlphaDistanceScalar);
		}
	}

	public void LateUpdate()
	{
		if (!isPQS)
		{
			FlightCamera flightCamera = null;
			if (fxOn && HighLogic.LoadedSceneIsFlight && !MapView.MapIsEnabled && (flightCamera = FlightCamera.fetch) != null && flightCamera.cameraAlt < 0f && FlightGlobals.ready && FlightGlobals.currentMainBody.ocean)
			{
				camVectorPos = flightCamera.transform.position;
				camVectorPos.w = (alt = flightCamera.cameraAlt);
				camVectorDir = flightCamera.upAxis;
				camVectorDir.w = 1f;
			}
			else
			{
				camVectorPos = Vector4.one;
				ref Vector4 reference = ref camVectorPos;
				float w = 0f;
				alt = 0f;
				reference.w = w;
				camVectorDir = Vector4.one;
				camVectorDir.w = 0f;
			}
			if (alt != lastAlt)
			{
				UpdateShaderProperties();
				lastAlt = alt;
			}
			if (fxOn && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && FlightGlobals.currentMainBody != curBody)
			{
				UpdateCBProperties(FlightGlobals.currentMainBody);
			}
		}
	}

	public void OnPreRender()
	{
		if (!fxOn || !HighLogic.LoadedSceneIsFlight || MapView.MapIsEnabled)
		{
			return;
		}
		if (Sun.Instance != null)
		{
			oldSunBrightnessMult = Sun.Instance.brightnessMultiplier;
		}
		if (SunFlare.Instance != null)
		{
			oldSunFlareBrightnessMult = SunFlare.Instance.brightnessMultiplier;
		}
		CelestialBody currentMainBody = FlightGlobals.currentMainBody;
		if (!(currentMainBody != null) || !currentMainBody.ocean)
		{
			return;
		}
		FlightCamera fetch = FlightCamera.fetch;
		if (!(fetch != null) || !fetch.isActiveAndEnabled)
		{
			return;
		}
		alt = fetch.cameraAlt;
		bool flag = alt < 0f;
		AtmosphereFromGround afg = currentMainBody.afg;
		if (afg != null)
		{
			foundAfg = true;
			if (flag)
			{
				float num = currentMainBody.oceanAFGBase + alt * currentMainBody.oceanAFGAltMult;
				if (num < currentMainBody.oceanAFGMin)
				{
					num = currentMainBody.oceanAFGMin;
				}
				afg.SetKrESunLerp(num);
			}
			else
			{
				afg.SetKrESunLerp(1f);
			}
		}
		foundAfg = false;
		if (flag)
		{
			float num2 = currentMainBody.oceanSunBase + alt * currentMainBody.oceanSunAltMult;
			if (num2 < currentMainBody.oceanSunMin)
			{
				num2 = currentMainBody.oceanSunMin;
			}
			if (Sun.Instance != null)
			{
				Sun.Instance.brightnessMultiplier = num2;
			}
			if (SunFlare.Instance != null)
			{
				SunFlare.Instance.brightnessMultiplier = num2;
			}
		}
	}

	public void OnPostRender()
	{
		if (!fxOn || !HighLogic.LoadedSceneIsFlight || MapView.MapIsEnabled)
		{
			return;
		}
		if (Sun.Instance != null)
		{
			Sun.Instance.brightnessMultiplier = oldSunBrightnessMult;
		}
		if (SunFlare.Instance != null)
		{
			SunFlare.Instance.brightnessMultiplier = oldSunFlareBrightnessMult;
		}
		if (FlightGlobals.currentMainBody != null)
		{
			AtmosphereFromGround afg = FlightGlobals.currentMainBody.afg;
			if (afg != null)
			{
				afg.SetKrESunLerp(1f);
			}
		}
	}
}
