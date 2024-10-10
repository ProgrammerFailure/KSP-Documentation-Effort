using System.Collections.Generic;
using System.IO;
using UnityEngine;

[EffectDefinition("AUDIO")]
public class AudioFX : EffectBehaviour
{
	public enum AudioChannel
	{
		Ship,
		Voice,
		Ambient,
		Music,
		const_4
	}

	[Persistent]
	public AudioChannel channel;

	[Persistent]
	public bool loop;

	[Persistent]
	public int polyphony = 1;

	[Persistent]
	public string clip = "";

	[Persistent]
	public float maxVolumeDistance = 500f;

	public FXCurve volume = new FXCurve("volume", 1f);

	public FXCurve pitch = new FXCurve("pitch", 1f);

	public float volumeSet;

	public AudioClip clipAudio;

	public List<AudioSource> sources = new List<AudioSource>();

	public bool isPaused;

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Update()
	{
		if (sources.Count == 0 || isPaused)
		{
			return;
		}
		int count = sources.Count;
		while (count-- > 0)
		{
			AudioSource audioSource = sources[count];
			if (audioSource == null)
			{
				sources.RemoveAt(count);
			}
			else if (!audioSource.isPlaying)
			{
				Object.Destroy(audioSource);
				sources.RemoveAt(count);
			}
			else
			{
				SetSourceVolume(audioSource, volumeSet, channel);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
		volume.Load("volume", node);
		pitch.Load("pitch", node);
		node.TryGetValue("maxVolumeDistance", ref maxVolumeDistance);
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
		if (clipAudio == null)
		{
			if (power <= 0f)
			{
				return;
			}
			string url = Path.ChangeExtension(clip, null);
			clipAudio = GameDatabase.Instance.GetAudioClip(url);
			if (clipAudio == null)
			{
				return;
			}
		}
		AudioSource audioSource = null;
		if (!HighLogic.LoadedSceneIsFlight || !(hostPart.vessel != null) || !hostPart.vessel.isEVA)
		{
			audioSource = ((sources.Count >= polyphony) ? sources[0] : CreateSource());
		}
		else
		{
			audioSource = hostPart.GetComponent<AudioSource>();
			audioSource.clip = clipAudio;
		}
		volumeSet = volume.Value(power);
		SetSourceVolume(audioSource, volumeSet, channel);
		audioSource.pitch = pitch.Value(power);
		audioSource.spatialBlend = 1f;
		if (!isPaused && (!audioSource.isPlaying || !loop))
		{
			audioSource.Play();
		}
	}

	public void SetTime(float newTime)
	{
		if (sources[0] != null)
		{
			sources[0].time = newTime;
		}
	}

	public void Stop(AudioSource src)
	{
		int num = sources.IndexOf(src);
		if (num != -1)
		{
			Object.Destroy(sources[num]);
			sources.RemoveAt(num);
		}
	}

	public void Stop()
	{
		int count = sources.Count;
		while (count-- > 0)
		{
			Object.Destroy(sources[count]);
			sources.RemoveAt(count);
		}
	}

	public void OnGamePause()
	{
		isPaused = true;
		int count = sources.Count;
		while (count-- > 0)
		{
			if (sources[count] != null)
			{
				sources[count].Pause();
			}
			else
			{
				sources.RemoveAt(count);
			}
		}
	}

	public void OnGameUnpause()
	{
		isPaused = false;
		int count = sources.Count;
		while (count-- > 0)
		{
			if (sources[count] != null)
			{
				sources[count].Play();
			}
			else
			{
				sources.RemoveAt(count);
			}
		}
	}

	public AudioSource CreateSource()
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = clipAudio;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.loop = loop;
		audioSource.playOnAwake = false;
		audioSource.dopplerLevel = 0f;
		audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		audioSource.maxDistance = maxVolumeDistance;
		sources.Add(audioSource);
		return audioSource;
	}

	public static void SetSourceVolume(AudioSource source, float volumeSet, AudioChannel channel)
	{
		switch (channel)
		{
		case AudioChannel.Ship:
			source.volume = GameSettings.SHIP_VOLUME * volumeSet;
			break;
		case AudioChannel.Voice:
			source.volume = GameSettings.VOICE_VOLUME * volumeSet;
			break;
		case AudioChannel.Ambient:
			source.volume = GameSettings.AMBIENCE_VOLUME * volumeSet;
			break;
		case AudioChannel.Music:
			source.volume = GameSettings.MUSIC_VOLUME * volumeSet;
			break;
		case AudioChannel.const_4:
			source.volume = GameSettings.UI_VOLUME * volumeSet;
			break;
		}
	}
}
