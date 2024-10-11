using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
	public AudioClip theme;

	public AudioClip ambienceLoop;

	private static bool themePlayedOnce;

	private static MenuMusic instance;

	private AudioSource _audioSource;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MenuMusic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PlayTheme()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PlayAmbienceLoop()
	{
		throw null;
	}
}
