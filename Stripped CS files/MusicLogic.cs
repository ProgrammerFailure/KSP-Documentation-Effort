using System;
using System.Collections;
using System.Collections.Generic;
using ns11;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLogic : MonoBehaviour
{
	public enum CONSTRUCTION_BUILDING
	{
		const_0,
		const_1
	}

	public enum AdditionalThemes
	{
		AstronautComplex,
		ResearchAndDevelopment,
		MissionControl,
		Administration
	}

	public static MusicLogic fetch;

	public AudioClip menuTheme;

	public AudioClip menuAmbience;

	public AudioClip credits;

	public AudioClip trackingAmbience;

	public AudioClip spaceCenterAmbienceDay;

	public AudioClip spaceCenterAmbienceNight;

	public AudioClip VABAmbience;

	public AudioClip SPHAmbience;

	public AudioClip astroComplexAmbience;

	public AudioClip researchComplexAmbience;

	public AudioClip missionControlAmbience;

	public AudioClip adminFacilityAmbience;

	public List<AudioClip> constructionPlaylist;

	public List<AudioClip> spacePlaylist;

	public List<AudioClip> missionBuilderPlayList;

	public AudioSource audio1;

	public AudioSource audio2;

	public PauseAudioFadeHandler fadeHandler1;

	public PauseAudioFadeHandler fadeHandler2;

	public static float setVolume1 = 1f;

	public static float setVolume2 = 1f;

	public FloatCurve dayVolumeCurve;

	public FloatCurve nightVolumeCurve;

	public bool menuThemePlayedOnce;

	public bool backFromSettings;

	public bool constructionThemeFirstTime;

	public bool playList;

	public bool audio1PlayingBeforePause;

	public bool audio2PlayingBeforePause;

	public bool gamePaused;

	public GameScenes currentScene = GameScenes.MAINMENU;

	public double flightMusicSpaceAltitude = 70000.0;

	public static void SetVolume(float volume1, float volume2)
	{
		if ((bool)fetch)
		{
			setVolume1 = volume1;
			setVolume2 = volume2;
			fetch.audio1.volume = volume1;
			fetch.audio2.volume = volume2;
		}
	}

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		fetch = this;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneChange);
		GameEvents.OnGameSettingsApplied.Add(GetSettings);
		SceneManager.sceneLoaded += OnSceneLoaded;
		GameEvents.onGamePause.Add(onGamePause);
		GameEvents.onGameUnpause.Add(onGameUnpause);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneChange);
		GameEvents.OnGameSettingsApplied.Remove(GetSettings);
		GameEvents.onGamePause.Remove(onGamePause);
		GameEvents.onGameUnpause.Remove(onGameUnpause);
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void onGamePause()
	{
		if (UISpaceCenter.Instance != null && (UISpaceCenter.Instance.SpawnedAC || UISpaceCenter.Instance.SpawnedMC || UISpaceCenter.Instance.SpawnedRD || UISpaceCenter.Instance.SpawnedADM))
		{
			audio1PlayingBeforePause = false;
			audio2PlayingBeforePause = false;
			return;
		}
		if (audio1.isPlaying)
		{
			audio1PlayingBeforePause = true;
		}
		else
		{
			audio1PlayingBeforePause = false;
		}
		if (audio2.isPlaying)
		{
			audio2PlayingBeforePause = true;
		}
		else
		{
			audio2PlayingBeforePause = false;
		}
		audio1.Pause();
		audio2.Pause();
		SetVolume(0f, 0f);
		gamePaused = true;
	}

	public void onGameUnpause()
	{
		if (audio1PlayingBeforePause)
		{
			audio1.Play();
		}
		if (audio2PlayingBeforePause)
		{
			audio2.Play();
		}
		GetSettings();
		gamePaused = false;
	}

	public void GetSettings()
	{
		switch (currentScene)
		{
		case GameScenes.EDITOR:
			SetVolume(GameSettings.MUSIC_VOLUME, GameSettings.AMBIENCE_VOLUME);
			break;
		default:
			SetVolume(GameSettings.MUSIC_VOLUME, GameSettings.MUSIC_VOLUME);
			break;
		case GameScenes.SPACECENTER:
			if (!(UISpaceCenter.Instance != null) || (!UISpaceCenter.Instance.SpawnedAC && !UISpaceCenter.Instance.SpawnedMC && !UISpaceCenter.Instance.SpawnedRD && !UISpaceCenter.Instance.SpawnedADM))
			{
				SetVolume(GameSettings.AMBIENCE_VOLUME, GameSettings.AMBIENCE_VOLUME);
			}
			break;
		}
	}

	public void OnGameSceneChange(GameScenes scene)
	{
		gamePaused = false;
		currentScene = scene;
		GetSettings();
		switch (scene)
		{
		default:
			StopCoroutine("PlayList");
			audio1.Stop();
			audio2.Stop();
			break;
		case GameScenes.MAINMENU:
			if (!backFromSettings)
			{
				ResetConstValues();
				StopCoroutine("PlayList");
				audio1.Stop();
				audio2.Stop();
			}
			break;
		case GameScenes.SETTINGS:
			break;
		}
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex)));
	}

	public IEnumerator OnLevelLoaded(GameScenes level)
	{
		if (level >= (GameScenes)10 && level <= (GameScenes)17)
		{
			yield break;
		}
		gamePaused = false;
		if (level != GameScenes.SETTINGS && level != GameScenes.MAINMENU)
		{
			audio1.Stop();
			audio2.Stop();
		}
		yield return null;
		yield return null;
		switch (level)
		{
		default:
			ResetSpaceCenterValues();
			ResetFlightValues();
			break;
		case GameScenes.MISSIONBUILDER:
			MissionBuilderMusic();
			break;
		case GameScenes.MAINMENU:
			if (!backFromSettings)
			{
				MenuMusic();
			}
			else
			{
				backFromSettings = false;
			}
			break;
		case GameScenes.SETTINGS:
			backFromSettings = true;
			if (!audio1.isPlaying)
			{
				audio1.Play();
			}
			break;
		case GameScenes.CREDITS:
			CreditsMusic();
			break;
		case GameScenes.SPACECENTER:
			ResetConstValues();
			ResetFlightValues();
			SpaceCenterMusic();
			break;
		case GameScenes.EDITOR:
			ResetSpaceCenterValues();
			ResetFlightValues();
			ConstructionMusic(EditorDriver.editorFacility);
			break;
		case GameScenes.FLIGHT:
			ResetConstValues();
			ResetSpaceCenterValues();
			FlightSpaceMusic();
			break;
		case GameScenes.TRACKSTATION:
			ResetSpaceCenterValues();
			ResetFlightValues();
			TrackingStMusic();
			break;
		}
	}

	public void PlayTheme(AudioSource audioS, AudioClip clip)
	{
		audioS.clip = clip;
		audioS.loop = false;
		audioS.time = 0f;
		audioS.Play();
	}

	public void PlayAmbienceLoop(AudioSource audioS, AudioClip clip, bool randomize = false)
	{
		audioS.clip = clip;
		audioS.loop = true;
		if (randomize)
		{
			audioS.time = Convert.ToSingle(new KSPRandom().NextDouble()) * clip.length;
		}
		else
		{
			audioS.time = 0f;
		}
		audioS.Play();
	}

	public IEnumerator WaitandPlayLoop(AudioSource audioS, AudioClip clip, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
		{
			audioS.clip = clip;
			audioS.loop = true;
			audioS.Play();
		}
	}

	public IEnumerator PlayList(List<AudioClip> list)
	{
		if (list.Count <= 0)
		{
			yield break;
		}
		do
		{
			playList = HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.MISSIONBUILDER;
			if (!audio1.isPlaying && playList)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				PlayTheme(audio1, list[index]);
			}
			yield return null;
		}
		while (playList);
	}

	public void Crossfade(AudioSource a1, AudioSource a2, float duration)
	{
		float time = Time.time;
		float num = time + duration;
		while (Time.time < num)
		{
			float num2 = (Time.time - time) / duration;
			a1.volume = 1f - num2;
			a2.volume = num2;
		}
	}

	public void MenuMusic()
	{
		if (!menuThemePlayedOnce)
		{
			PlayTheme(audio1, menuTheme);
			StartCoroutine(WaitandPlayLoop(audio1, menuAmbience, menuTheme.length));
			menuThemePlayedOnce = true;
		}
		else
		{
			PlayAmbienceLoop(audio1, menuAmbience);
		}
	}

	public void CreditsMusic()
	{
		PlayTheme(audio1, credits);
	}

	public void TrackingStMusic()
	{
		PlayAmbienceLoop(audio1, trackingAmbience);
	}

	public void SpaceCenterMusic()
	{
		PlayAmbienceLoop(audio1, spaceCenterAmbienceDay, randomize: true);
		PlayAmbienceLoop(audio2, spaceCenterAmbienceNight, randomize: true);
		StartCoroutine(PlaySpaceCenter());
	}

	public IEnumerator PlaySpaceCenter()
	{
		while (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			if (gamePaused)
			{
				yield return null;
			}
			if ((bool)TimeWarp.fetch && TimeWarp.fetch.Mode == TimeWarp.Modes.HIGH && TimeWarp.fetch.current_rate_index == TimeWarp.fetch.warpRates.Length - 1)
			{
				audio1.volume = Mathf.MoveTowards(audio1.volume, 0f, Time.unscaledDeltaTime * 2f);
				audio2.volume = Mathf.MoveTowards(audio2.volume, 0f, Time.unscaledDeltaTime * 2f);
				yield return null;
				continue;
			}
			double num = ((!Sun.Instance || !SpaceCenter.Instance) ? 0.5 : Sun.Instance.GetLocalTimeAtPosition(SpaceCenter.Instance.Latitude, SpaceCenter.Instance.Longitude, SpaceCenter.Instance.cb));
			if (audio1.isPlaying && !fadeHandler1.IsPaused && !fadeHandler1.IsFading)
			{
				audio1.volume = setVolume1 * dayVolumeCurve.Evaluate((float)num);
			}
			if (audio2.isPlaying && !fadeHandler2.IsPaused && !fadeHandler2.IsFading)
			{
				audio2.volume = setVolume2 * nightVolumeCurve.Evaluate((float)num);
			}
			yield return null;
		}
	}

	public void ConstructionMusic(EditorFacility building)
	{
		if (!constructionThemeFirstTime)
		{
			audio1.Stop();
			PlayEditorAmbience(building);
			StartCoroutine(PlayList(constructionPlaylist));
			constructionThemeFirstTime = true;
		}
	}

	public void PlayEditorAmbience(EditorFacility building)
	{
		switch (building)
		{
		case EditorFacility.const_1:
			PlayAmbienceLoop(audio2, VABAmbience, randomize: true);
			break;
		case EditorFacility.const_2:
			PlayAmbienceLoop(audio2, SPHAmbience, randomize: true);
			break;
		}
	}

	public void MissionBuilderMusic()
	{
		audio1.Stop();
		StartCoroutine(PlayList(missionBuilderPlayList));
	}

	public void FlightSpaceMusic()
	{
		audio1.Stop();
		audio2.Stop();
		StartCoroutine(PlayFlight());
	}

	public IEnumerator PlayFlight()
	{
		yield return new WaitForSeconds(2f);
		while (HighLogic.LoadedSceneIsFlight)
		{
			while (gamePaused)
			{
				yield return null;
			}
			if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
			{
				if (FlightGlobals.ActiveVessel.orbitDriver.referenceBody.isHomeWorld)
				{
					if (FlightGlobals.ActiveVessel.altitude > flightMusicSpaceAltitude && spacePlaylist.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, spacePlaylist.Count);
						if (!audio1.isPlaying)
						{
							PlayTheme(audio1, spacePlaylist[index]);
						}
					}
					else if (audio1.isPlaying)
					{
						audio1.Stop();
					}
				}
				else if (!audio1.isPlaying)
				{
					int index = UnityEngine.Random.Range(0, spacePlaylist.Count);
					if (!audio1.isPlaying)
					{
						PlayTheme(audio1, spacePlaylist[index]);
					}
				}
			}
			else
			{
				audio1.Stop();
			}
			yield return null;
		}
	}

	public void ResetConstValues()
	{
		audio2.Stop();
		constructionThemeFirstTime = false;
		playList = false;
		StopCoroutine("PlayList");
	}

	public void ResetSpaceCenterValues()
	{
		StopCoroutine("PlaySpaceCenter");
	}

	public void ResetFlightValues()
	{
		StopCoroutine("PlayFlight");
	}

	public void PauseWithCrossfade(AdditionalThemes newTheme)
	{
		switch (newTheme)
		{
		case AdditionalThemes.AstronautComplex:
			fadeHandler1.PauseWithFade(0.5f, astroComplexAmbience, GameSettings.MUSIC_VOLUME, 0f, fadeClipLoop: true);
			fadeHandler2.PauseWithFade(0.5f, null, setVolume2, 0f, fadeClipLoop: true);
			break;
		case AdditionalThemes.ResearchAndDevelopment:
			fadeHandler1.PauseWithFade(0.5f, researchComplexAmbience, GameSettings.MUSIC_VOLUME, 0f, fadeClipLoop: true);
			fadeHandler2.PauseWithFade(0.5f, null, setVolume2, 0f, fadeClipLoop: true);
			break;
		case AdditionalThemes.MissionControl:
			fadeHandler1.PauseWithFade(0.5f, missionControlAmbience, GameSettings.MUSIC_VOLUME, 0f, fadeClipLoop: true);
			fadeHandler2.PauseWithFade(0.5f, null, setVolume2, 0f, fadeClipLoop: true);
			break;
		case AdditionalThemes.Administration:
			fadeHandler1.PauseWithFade(0.5f, adminFacilityAmbience, GameSettings.MUSIC_VOLUME, 0f, fadeClipLoop: true);
			fadeHandler2.PauseWithFade(0.5f, null, setVolume2, 0f, fadeClipLoop: true);
			break;
		}
	}

	public void UnpauseWithCrossfade()
	{
		fadeHandler1.UnpauseWithFade();
		fadeHandler2.UnpauseWithFade();
	}

	public void HandleEditorSwitch()
	{
		audio2.Stop();
		PlayEditorAmbience(EditorDriver.editorFacility);
	}
}
