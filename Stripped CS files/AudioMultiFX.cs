using System.Collections.Generic;
using UnityEngine;

[EffectDefinition("AUDIO_MULTI")]
public class AudioMultiFX : EffectBehaviour
{
	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip = "";

	public FXCurve volume = new FXCurve("volume", 1f);

	public FXCurve pitch = new FXCurve("pitch", 1f);

	[Persistent]
	public string transformName = "";

	public AudioClip clipAudio;

	public AudioSource source;

	public float[] volumes;

	public List<Transform> modelParents;

	public bool isPaused;

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Update()
	{
		if (!(source == null) && !isPaused)
		{
			float num = 0f;
			int num2 = volumes.Length;
			while (num2-- > 0)
			{
				num = Mathf.Max(num, volumes[num2]);
			}
			AudioFX.SetSourceVolume(source, num, channel);
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
		modelParents = new List<Transform>(hostPart.FindModelTransforms(transformName));
		if (modelParents.Count == 0)
		{
			Debug.LogError("AudioFX: Cannot find transform of name '" + transformName + "'");
			clipAudio = null;
			return;
		}
		volumes = new float[modelParents.Count];
		if (HighLogic.LoadedScene != 0)
		{
			GameEvents.onGamePause.Add(OnGamePause);
			GameEvents.onGameUnpause.Add(OnGameUnpause);
		}
	}

	public override void OnEvent(float power, int transformIdx)
	{
		if (volumes == null)
		{
			return;
		}
		if (transformIdx >= 0 && transformIdx < volumes.Length)
		{
			Play(power, transformIdx);
			return;
		}
		int i = 0;
		for (int num = volumes.Length; i < num; i++)
		{
			Play(power, i);
		}
	}

	public override void OnEvent(int transformIdx)
	{
		OnEvent(1f, transformIdx);
	}

	public void Play(float power, int transformIdx)
	{
		if (clipAudio == null)
		{
			return;
		}
		if (source == null)
		{
			source = CreateSource(modelParents[0]);
		}
		volumes[transformIdx] = volume.Value(power);
		float num = 0f;
		int num2 = volumes.Length;
		while (num2-- > 0)
		{
			num = Mathf.Max(num, volumes[num2]);
		}
		AudioFX.SetSourceVolume(source, num, channel);
		source.pitch = pitch.Value(power);
		source.spatialBlend = 1f;
		if (!isPaused)
		{
			if (!source.isPlaying && num > 0f)
			{
				source.Play();
			}
			else if (source.isPlaying && num <= 0f)
			{
				source.Stop();
			}
		}
	}

	public void OnGamePause()
	{
		isPaused = true;
		if (source != null)
		{
			source.Pause();
		}
	}

	public void OnGameUnpause()
	{
		isPaused = false;
		if (source != null)
		{
			source.Play();
		}
	}

	public AudioSource CreateSource(Transform transform)
	{
		AudioSource audioSource = transform.gameObject.AddComponent<AudioSource>();
		audioSource.clip = clipAudio;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.loop = false;
		audioSource.dopplerLevel = 0f;
		return audioSource;
	}
}
