using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
	public AudioClip theme;

	public AudioClip ambienceLoop;

	public static bool themePlayedOnce;

	public static MenuMusic instance;

	public AudioSource _audioSource;

	public void Awake()
	{
		if ((bool)instance)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		this.GetComponentCached(ref _audioSource).volume = GameSettings.MUSIC_VOLUME;
		Object.DontDestroyOnLoad(base.gameObject);
		if (!themePlayedOnce)
		{
			PlayTheme();
			Invoke("PlayAmbienceLoop", theme.length);
			themePlayedOnce = true;
		}
		else
		{
			PlayAmbienceLoop();
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes level)
	{
		if (level != GameScenes.SETTINGS && level != GameScenes.MAINMENU)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (IsInvoking())
		{
			CancelInvoke();
		}
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void PlayTheme()
	{
		this.GetComponentCached(ref _audioSource).clip = theme;
		this.GetComponentCached(ref _audioSource).loop = false;
		this.GetComponentCached(ref _audioSource).Play();
	}

	public void PlayAmbienceLoop()
	{
		this.GetComponentCached(ref _audioSource).clip = ambienceLoop;
		this.GetComponentCached(ref _audioSource).loop = true;
		this.GetComponentCached(ref _audioSource).Play();
	}
}
