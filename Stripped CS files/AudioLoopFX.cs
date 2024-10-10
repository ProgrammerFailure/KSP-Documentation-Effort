using UnityEngine;

[EffectDefinition("AUDIO_LOOP")]
public class AudioLoopFX : EffectBehaviour
{
	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip = "";

	public FXCurve volume = new FXCurve("volume", 1f);

	public FXCurve pitch = new FXCurve("pitch", 1f);

	public float volumeSet;

	public AudioClip clipAudio;

	public AudioSource src;

	public bool isPaused;

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Update()
	{
		if (!isPaused && !(src == null))
		{
			AudioFX.SetSourceVolume(src, volumeSet, channel);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
		volume.Load("volume", node);
		pitch.Load("pitch", node);
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode.CreateConfigFromObject(this, node);
		volume.Save(node);
		pitch.Save(node);
	}

	public override void OnInitialize()
	{
		clipAudio = GameDatabase.Instance.GetAudioClip(clip);
		if (clipAudio == null)
		{
			Debug.Log("Cannot assign AudioClip '" + clip + "' to AudioFX");
		}
		if (HighLogic.LoadedScene != 0)
		{
			GameEvents.onGamePause.Add(OnGamePause);
			GameEvents.onGameUnpause.Add(OnGameUnpause);
		}
	}

	public override void OnEvent(float power)
	{
		Play(power);
	}

	public override void OnEvent()
	{
		Play(1f);
	}

	public void Play(float power)
	{
		if (!(clipAudio == null))
		{
			if (src == null)
			{
				src = CreateSource();
			}
			volumeSet = volume.Value(power);
			AudioFX.SetSourceVolume(src, volumeSet, channel);
			src.pitch = pitch.Value(power);
			src.spatialBlend = 1f;
			if (!src.isPlaying && !isPaused)
			{
				src.Play();
			}
		}
	}

	public void Stop()
	{
		if (!(src == null))
		{
			Object.Destroy(src);
			src = null;
		}
	}

	public void OnGamePause()
	{
		isPaused = true;
		if (src != null)
		{
			src.Pause();
		}
	}

	public void OnGameUnpause()
	{
		isPaused = false;
		if (src != null && volumeSet > 0f)
		{
			src.Play();
		}
	}

	public AudioSource CreateSource()
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = clipAudio;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.loop = true;
		return audioSource;
	}
}
