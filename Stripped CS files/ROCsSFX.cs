using UnityEngine;

[EffectDefinition("ROCSAUDIO")]
public class ROCsSFX : EffectBehaviour
{
	[KSPField]
	public string idleEffectName = "idle";

	[KSPField]
	public string burstEffectName = "burst";

	[Persistent]
	public bool loop;

	public AudioClip clipAudio;

	public AudioSource audioSource;

	public float soundMaxDistance = 500f;

	public bool isPaused;

	public bool onPauseWasPlaying;

	public float sfxPower;

	public bool fadeoutPlayback;

	public bool fadeInPlayback;

	public string clipPath;

	public float playDelay;

	public float fadeOutSpeed = 0.25f;

	public float fadeInSpeed = 0.25f;

	public void OnEnable()
	{
		GameEvents.onGamePause.Add(OnGamePause);
		GameEvents.onGameUnpause.Add(OnGameUnpause);
	}

	public void OnDisable()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Update()
	{
		if (!(audioSource == null) && !isPaused)
		{
			if (fadeoutPlayback)
			{
				FadeOut();
			}
			if (fadeInPlayback)
			{
				FadeIn();
			}
		}
	}

	public void Start()
	{
		clipAudio = GameDatabase.Instance.GetAudioClip(clipPath);
		if (clipAudio == null)
		{
			Debug.Log("Cannot load AudioClip '" + clipPath + "' to AudioFX.");
		}
		if (effectName == idleEffectName)
		{
			PlayIdleSFX();
		}
	}

	public void Play(float power)
	{
		if (!(clipAudio == null))
		{
			if (audioSource == null)
			{
				CreateSource();
			}
			if (!audioSource.isPlaying)
			{
				audioSource.PlayDelayed(playDelay);
			}
		}
	}

	public void PlayIdleSFX()
	{
		Play(sfxPower);
	}

	public float PlayBurstSFX()
	{
		Play(sfxPower);
		if (clipAudio == null)
		{
			return 0f;
		}
		return clipAudio.length;
	}

	public void OnGamePause()
	{
		onPauseWasPlaying = false;
		if (audioSource != null)
		{
			onPauseWasPlaying = audioSource.isPlaying;
			audioSource.Pause();
		}
		isPaused = true;
	}

	public void OnGameUnpause()
	{
		if (audioSource != null && onPauseWasPlaying)
		{
			audioSource.Play();
		}
		isPaused = false;
	}

	public void FadeOutPlayback()
	{
		fadeoutPlayback = true;
	}

	public void FadeOut()
	{
		if (audioSource.volume > 0f)
		{
			audioSource.volume -= Time.deltaTime * fadeOutSpeed;
		}
		else
		{
			fadeoutPlayback = false;
		}
	}

	public void FadeIn()
	{
		if (audioSource.volume < sfxPower)
		{
			audioSource.volume += Time.deltaTime * fadeInSpeed;
		}
		else
		{
			fadeInPlayback = false;
		}
	}

	public void CreateSource()
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = clipAudio;
		audioSource.volume = sfxPower;
		audioSource.loop = loop;
		audioSource.dopplerLevel = 0f;
		audioSource.maxDistance = soundMaxDistance;
		audioSource.volume = sfxPower;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.spatialBlend = 1f;
		this.audioSource = audioSource;
	}

	public void SetSFXVolume(float value)
	{
		sfxPower = value;
	}

	public void SetClipsPath(string idleClipPath, string burstClipPath)
	{
		if (effectName == idleEffectName)
		{
			clipPath = idleClipPath;
		}
		else if (effectName == burstEffectName)
		{
			clipPath = burstClipPath;
		}
		else
		{
			Debug.Log("effectName: " + effectName + ", has to match with the idle or burst state.");
		}
	}

	public void FadeInPlayback()
	{
		fadeInPlayback = true;
	}
}
