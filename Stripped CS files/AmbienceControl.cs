using UnityEngine;

public class AmbienceControl : MonoBehaviour
{
	public AudioSource editorAmbience;

	public AudioSource flightAmbienceHigh;

	public AudioSource flightAmbienceLow;

	public AudioSource airspeed;

	public AudioSource gForceNoise;

	public AnimationCurve flightLowAltitudeCurve;

	public AnimationCurve flightHighAltitudeCurve;

	public GameObject cameraRef;

	public float stPatm;

	public float camAlt;

	public float ambienceGain = 2.5f;

	public void Awake()
	{
		GameEvents.onGamePause.Add(flightAmbienceStop);
		GameEvents.onGameUnpause.Add(flightAmbienceGo);
		GameEvents.onLevelWasLoaded.Add(OnSceneLoaded);
		GameEvents.onGameSceneSwitchRequested.Add(OnLeavingScene);
		if (HighLogic.LoadedSceneIsEditor)
		{
			editorAmbienceGo();
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			flightAmbienceGo();
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(flightAmbienceStop);
		GameEvents.onGameUnpause.Remove(flightAmbienceGo);
		GameEvents.onLevelWasLoaded.Remove(OnSceneLoaded);
		GameEvents.onGameSceneSwitchRequested.Remove(OnLeavingScene);
	}

	public void OnSceneLoaded(GameScenes scn)
	{
		switch (scn)
		{
		case GameScenes.FLIGHT:
			flightAmbienceGo();
			break;
		case GameScenes.EDITOR:
			editorAmbienceGo();
			break;
		}
	}

	public void OnLeavingScene(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		switch (scn.from)
		{
		case GameScenes.FLIGHT:
			flightAmbienceStop();
			break;
		case GameScenes.EDITOR:
			editorAmbienceStop();
			break;
		}
	}

	public void editorAmbienceGo()
	{
		if (editorAmbience != null)
		{
			editorAmbience.loop = true;
			editorAmbience.Play();
		}
	}

	public void editorAmbienceStop()
	{
		if (editorAmbience != null)
		{
			editorAmbience.Stop();
		}
	}

	public void flightAmbienceGo()
	{
		if (flightAmbienceLow != null)
		{
			flightAmbienceLow.loop = true;
			flightAmbienceLow.Play();
			flightAmbienceLow.volume = 0f;
		}
		if (flightAmbienceHigh != null)
		{
			flightAmbienceHigh.loop = true;
			flightAmbienceHigh.Play();
			flightAmbienceHigh.volume = 0f;
		}
		if (airspeed != null)
		{
			airspeed.loop = true;
			airspeed.volume = 0f;
			airspeed.Play();
		}
		if (gForceNoise != null)
		{
			gForceNoise.loop = true;
			gForceNoise.volume = 0f;
			gForceNoise.Play();
		}
	}

	public void flightAmbienceStop()
	{
		if (flightAmbienceHigh != null)
		{
			flightAmbienceHigh.Stop();
		}
		if (flightAmbienceLow != null)
		{
			flightAmbienceLow.Stop();
		}
		if (airspeed != null)
		{
			airspeed.Stop();
		}
		if (gForceNoise != null)
		{
			gForceNoise.Stop();
		}
	}

	public void Update()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (FlightGlobals.currentMainBody.atmosphere)
		{
			camAlt = (float)((double)FlightGlobals.getAltitudeAtPos(cameraRef.transform.position, FlightGlobals.currentMainBody) / FlightGlobals.currentMainBody.atmosphereDepth);
			stPatm = (float)(FlightGlobals.getStaticPressure(cameraRef.transform.position, FlightGlobals.currentMainBody) / Planetarium.fetch.Home.atmospherePressureSeaLevel);
			if (airspeed != null)
			{
				if (!FlightGlobals.ActiveVessel.packed)
				{
					airspeed.pitch = 0.3f + Mathf.Min((float)FlightGlobals.ship_srfSpeed / 250f, 2f);
					airspeed.volume = Mathf.Max(0.1f, Mathf.Min((float)FlightGlobals.ship_srfSpeed / 150f, 3f)) * stPatm;
					airspeed.volume *= (MapView.MapIsEnabled ? 0f : GameSettings.AMBIENCE_VOLUME);
				}
				else
				{
					airspeed.volume = 0f;
				}
			}
			if (flightAmbienceLow != null)
			{
				flightAmbienceLow.volume = flightLowAltitudeCurve.Evaluate(camAlt) * stPatm * ambienceGain;
				flightAmbienceLow.volume *= (MapView.MapIsEnabled ? 0f : GameSettings.AMBIENCE_VOLUME);
			}
			if (flightAmbienceHigh != null)
			{
				flightAmbienceHigh.volume = flightHighAltitudeCurve.Evaluate(camAlt) * stPatm * ambienceGain;
				flightAmbienceHigh.volume *= (MapView.MapIsEnabled ? 0f : GameSettings.AMBIENCE_VOLUME);
			}
			if (gForceNoise != null)
			{
				if (!FlightGlobals.ActiveVessel.packed)
				{
					gForceNoise.volume = Mathf.Lerp(0f, 1f, Mathf.Pow(Mathf.Abs((float)FlightGlobals.ActiveVessel.geeForce - 1f) / 10f, 2f)) * stPatm * ambienceGain;
					gForceNoise.volume *= (MapView.MapIsEnabled ? 0f : GameSettings.AMBIENCE_VOLUME);
				}
				else
				{
					gForceNoise.volume = 0f;
				}
			}
		}
		else
		{
			if (airspeed != null)
			{
				airspeed.volume = 0f;
			}
			if (flightAmbienceLow != null)
			{
				flightAmbienceLow.volume = 0f;
			}
			if (flightAmbienceHigh != null)
			{
				flightAmbienceHigh.volume = 0f;
			}
			if (gForceNoise != null)
			{
				gForceNoise.volume = 0f;
			}
		}
	}
}
