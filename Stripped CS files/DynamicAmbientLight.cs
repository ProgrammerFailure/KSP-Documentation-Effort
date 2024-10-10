using System.Collections.Generic;
using UnityEngine;

public class DynamicAmbientLight : MonoBehaviour
{
	public class AmbientLightColor
	{
		public Color ambientLight;

		public Color ambientEquatorColor;

		public Color ambientGroundColor;

		public Color ambientSkyColor;
	}

	public Color vacuumAmbientColor;

	public Color defaultAmbientColor;

	public bool disableDynamicAmbient;

	public float normalizedAtmosphericPressure;

	public double pressureAtSeaLevel;

	public double currentPressure;

	public CelestialBody currentBody;

	public AmbientLightColor originalLightColor;

	public Dictionary<GameScenes, AmbientLightColor> staticAmbientColors;

	public float boostFactor;

	public static DynamicAmbientLight Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("DynamicAmbientLight: Cannot have two instances in same scene");
			Object.Destroy(this);
			return;
		}
		Instance = this;
		originalLightColor = new AmbientLightColor();
		originalLightColor.ambientLight = RenderSettings.ambientLight;
		originalLightColor.ambientEquatorColor = RenderSettings.ambientEquatorColor;
		originalLightColor.ambientGroundColor = RenderSettings.ambientGroundColor;
		originalLightColor.ambientSkyColor = RenderSettings.ambientSkyColor;
		staticAmbientColors = new Dictionary<GameScenes, AmbientLightColor>();
		GameEvents.onLevelWasLoaded.Add(GrabDefaultAmbient);
	}

	public void GrabDefaultAmbient(GameScenes g)
	{
		if (!staticAmbientColors.ContainsKey(g))
		{
			AmbientLightColor ambientLightColor = new AmbientLightColor();
			ambientLightColor.ambientLight = RenderSettings.ambientLight;
			ambientLightColor.ambientEquatorColor = RenderSettings.ambientEquatorColor;
			ambientLightColor.ambientGroundColor = RenderSettings.ambientGroundColor;
			ambientLightColor.ambientSkyColor = RenderSettings.ambientSkyColor;
			staticAmbientColors.Add(g, ambientLightColor);
		}
	}

	public void Update()
	{
		boostFactor = GameSettings.AMBIENTLIGHT_BOOSTFACTOR;
		if (MapView.MapIsEnabled)
		{
			boostFactor = GameSettings.AMBIENTLIGHT_BOOSTFACTOR_MAPONLY;
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			boostFactor = GameSettings.AMBIENTLIGHT_BOOSTFACTOR_EDITONLY;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			Shader.SetGlobalFloat("_SpecularAmbientBoostDiffuse", 0f);
			Shader.SetGlobalFloat("_SpecularAmbientBoostEmissive", boostFactor);
		}
		else
		{
			Shader.SetGlobalFloat("_SpecularAmbientBoostDiffuse", boostFactor);
			Shader.SetGlobalFloat("_SpecularAmbientBoostEmissive", 0f);
		}
		if (disableDynamicAmbient)
		{
			if (staticAmbientColors.ContainsKey(HighLogic.LoadedScene))
			{
				if (boostFactor >= 0f)
				{
					RenderSettings.ambientLight = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientLight, Color.white, boostFactor);
					RenderSettings.ambientEquatorColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientEquatorColor, Color.white, boostFactor);
					RenderSettings.ambientGroundColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientGroundColor, Color.white, boostFactor);
					RenderSettings.ambientSkyColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientSkyColor, Color.white, boostFactor);
				}
				else
				{
					RenderSettings.ambientLight = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientLight, Color.black, Mathf.Abs(boostFactor));
					RenderSettings.ambientEquatorColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientEquatorColor, Color.black, Mathf.Abs(boostFactor));
					RenderSettings.ambientGroundColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientGroundColor, Color.black, Mathf.Abs(boostFactor));
					RenderSettings.ambientSkyColor = Color.Lerp(staticAmbientColors[HighLogic.LoadedScene].ambientSkyColor, Color.black, Mathf.Abs(boostFactor));
				}
			}
		}
		else
		{
			if (!FlightGlobals.ready)
			{
				return;
			}
			Color a;
			if (MapView.MapIsEnabled)
			{
				a = vacuumAmbientColor;
			}
			else
			{
				currentBody = FlightGlobals.currentMainBody;
				currentPressure = FlightGlobals.ship_stP;
				if (currentPressure != 0.0)
				{
					pressureAtSeaLevel = currentBody.GetPressure(0.0);
					normalizedAtmosphericPressure = (float)(currentPressure / pressureAtSeaLevel);
					normalizedAtmosphericPressure = Mathf.Clamp01(normalizedAtmosphericPressure);
				}
				else
				{
					normalizedAtmosphericPressure = 0f;
				}
				a = Color.Lerp(vacuumAmbientColor, currentBody.atmosphericAmbientColor, normalizedAtmosphericPressure);
			}
			if (boostFactor >= 0f)
			{
				RenderSettings.ambientLight = Color.Lerp(a, Color.white, boostFactor);
			}
			else
			{
				RenderSettings.ambientLight = Color.Lerp(a, Color.black, Mathf.Abs(boostFactor));
			}
		}
	}

	public void OnDestroy()
	{
		RenderSettings.ambientLight = originalLightColor.ambientLight;
		RenderSettings.ambientEquatorColor = originalLightColor.ambientEquatorColor;
		RenderSettings.ambientGroundColor = originalLightColor.ambientGroundColor;
		RenderSettings.ambientSkyColor = originalLightColor.ambientSkyColor;
		GameEvents.onLevelWasLoaded.Remove(GrabDefaultAmbient);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
