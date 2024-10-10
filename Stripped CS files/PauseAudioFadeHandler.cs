using System.Collections;
using UnityEngine;

public class PauseAudioFadeHandler : MonoBehaviour
{
	public AudioSource Source;

	public AudioClip pausedClip;

	public float pausedVolume;

	public bool pausedLoop;

	public float pausedTime;

	public Coroutine fadeRoutine;

	[HideInInspector]
	public bool IsPaused { get; set; }

	[HideInInspector]
	public bool IsFading { get; set; }

	public void PauseWithFade(float fadeDuration, AudioClip fadeClip, float fadeClipVolume, float fadeClipTime, bool fadeClipLoop)
	{
		if (fadeRoutine != null)
		{
			StopCoroutine(fadeRoutine);
			IsFading = false;
		}
		IsPaused = true;
		fadeRoutine = StartCoroutine(PauseWithFadeRoutine(Source, fadeDuration, fadeClip, fadeClipVolume, fadeClipTime, fadeClipLoop));
	}

	public IEnumerator PauseWithFadeRoutine(AudioSource fadeSource, float fadeDuration, AudioClip fadeClip, float fadeClipVolume, float fadeClipTime, bool fadeClipLoop)
	{
		IsFading = true;
		pausedClip = fadeSource.clip;
		pausedVolume = fadeSource.volume;
		pausedLoop = fadeSource.loop;
		float time2 = 0f;
		while (time2 < fadeDuration)
		{
			fadeSource.volume = pausedVolume * Mathf.Clamp01(1f - time2 / fadeDuration);
			time2 += Time.unscaledDeltaTime;
			yield return null;
		}
		fadeSource.volume = 0f;
		pausedTime = fadeSource.time;
		fadeSource.clip = fadeClip;
		fadeSource.time = fadeClipTime;
		fadeSource.loop = fadeClipLoop;
		fadeSource.Play();
		time2 = 0f;
		while (time2 < fadeDuration)
		{
			fadeSource.volume = fadeClipVolume * Mathf.Clamp01(time2 / fadeDuration);
			time2 += Time.unscaledDeltaTime;
			yield return null;
		}
		fadeSource.volume = fadeClipVolume;
		fadeRoutine = null;
		IsFading = false;
	}

	public void UnpauseWithFade()
	{
		if (fadeRoutine != null)
		{
			StopCoroutine(fadeRoutine);
			IsFading = false;
		}
		IsPaused = false;
		if (pausedClip != null)
		{
			fadeRoutine = StartCoroutine(PauseWithFadeRoutine(Source, 0.5f, pausedClip, pausedVolume, pausedTime, pausedLoop));
		}
	}
}
