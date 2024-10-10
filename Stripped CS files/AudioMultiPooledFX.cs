using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Pools;
using UnityEngine;

[EffectDefinition("AUDIO_MULTI_POOL")]
public class AudioMultiPooledFX : EffectBehaviour
{
	public class PooledAudioSource
	{
		public static Pool<PooledAudioSource> Pool = new Pool<PooledAudioSource>(Create, Reset);

		public static int maxAudioSource = 64;

		public GameObject host;

		public AudioSource source;

		public Action onRelease;

		public PooledAudioSource()
		{
			host = new GameObject("PooledAudioSource");
			UnityEngine.Object.DontDestroyOnLoad(host);
			source = host.AddComponent<AudioSource>();
			source.volume = 0f;
			source.pitch = 0f;
			source.spatialBlend = 1f;
			source.loop = false;
			source.dopplerLevel = 0f;
			host.SetActive(value: false);
		}

		public static PooledAudioSource Create()
		{
			return new PooledAudioSource();
		}

		public static void Reset(PooledAudioSource pooledAudioSource)
		{
			pooledAudioSource.onRelease();
			pooledAudioSource.source.enabled = false;
			pooledAudioSource.source.clip = null;
			pooledAudioSource.host.transform.parent = null;
			pooledAudioSource.host.SetActive(value: false);
		}

		public static PooledAudioSource BorrowAndPlay(Transform parent, AudioClip clip, float volume, float pitch, AudioFX.AudioChannel channel, Action onRelease)
		{
			if (Pool.Size == 0 && Pool.Allocated > maxAudioSource)
			{
				return null;
			}
			PooledAudioSource pooledAudioSource = null;
			while (pooledAudioSource == null || pooledAudioSource.host == null)
			{
				pooledAudioSource = Pool.Borrow();
			}
			pooledAudioSource.onRelease = onRelease;
			pooledAudioSource.host.transform.parent = parent;
			pooledAudioSource.host.SetActive(value: true);
			pooledAudioSource.source.enabled = true;
			pooledAudioSource.source.clip = clip;
			pooledAudioSource.source.pitch = pitch;
			AudioFX.SetSourceVolume(pooledAudioSource.source, volume, channel);
			if (pooledAudioSource.source != null && pooledAudioSource.source.isActiveAndEnabled)
			{
				pooledAudioSource.source.Play();
			}
			HighLogic.fetch.StartCoroutine(pooledAudioSource.CheckEnd());
			return pooledAudioSource;
		}

		public void Update(float volume, AudioFX.AudioChannel channel)
		{
			source.volume = volume;
			AudioFX.SetSourceVolume(source, volume, channel);
		}

		public void Update(float volume, float pitch, AudioFX.AudioChannel channel)
		{
			source.volume = volume;
			source.pitch = pitch;
			AudioFX.SetSourceVolume(source, volume, channel);
		}

		public void StopAndRelease()
		{
			source.Stop();
			Pool.Release(this);
		}

		public IEnumerator CheckEnd()
		{
			while (source != null && source.isPlaying)
			{
				yield return null;
			}
			if (host.transform.parent != null)
			{
				Pool.Release(this);
			}
		}
	}

	[Persistent]
	public AudioFX.AudioChannel channel;

	[Persistent]
	public string clip = "";

	public FXCurve volume = new FXCurve("volume", 1f);

	public FXCurve pitch = new FXCurve("pitch", 1f);

	[Persistent]
	public string transformName = "";

	public AudioClip clipAudio;

	public PooledAudioSource source;

	public float[] volumes;

	public List<Transform> modelParents;

	public bool isPaused;

	public void OnDestroy()
	{
		if (source != null)
		{
			source.StopAndRelease();
			Debug.Log("AudioMultiPooledFX OnDestroy StopAndRelease ");
		}
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void OnDisable()
	{
		if (source != null)
		{
			source.StopAndRelease();
			Debug.Log("AudioMultiPooledFX OnDestroy OnDisable ");
		}
	}

	public void Update()
	{
		if (source != null && !isPaused)
		{
			float a = 0f;
			int num = volumes.Length;
			while (num-- > 0)
			{
				a = Mathf.Max(a, volumes[num]);
			}
			source.Update(a, channel);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
		volume.Load("volume", node);
		pitch.Load("pitch", node);
		SetupAudioClip();
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode.CreateConfigFromObject(this, node);
		volume.Save(node);
		pitch.Save(node);
	}

	public override void OnInitialize()
	{
		SetupAudioClip();
		if (HighLogic.LoadedScene != 0)
		{
			GameEvents.onGamePause.Add(OnGamePause);
			GameEvents.onGameUnpause.Add(OnGameUnpause);
		}
	}

	public void SetupAudioClip()
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
		}
		else
		{
			volumes = new float[modelParents.Count];
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
		volumes[transformIdx] = volume.Value(power);
		float num = 0f;
		int num2 = volumes.Length;
		while (num2-- > 0)
		{
			num = Mathf.Max(num, volumes[num2]);
		}
		if (isPaused)
		{
			return;
		}
		Transform parent = modelParents[0];
		for (int i = 0; i < modelParents.Count; i++)
		{
			if (modelParents[i].gameObject.activeInHierarchy)
			{
				parent = modelParents[i];
				break;
			}
		}
		if (source == null && num > 0f)
		{
			source = PooledAudioSource.BorrowAndPlay(parent, clipAudio, num, pitch.Value(power), channel, delegate
			{
				source = null;
			});
		}
		else if (source != null && num > 0f)
		{
			source.Update(num, pitch.Value(power), channel);
		}
		else if (source != null && num <= 0f)
		{
			source.StopAndRelease();
		}
	}

	public void OnGamePause()
	{
		if (source != null)
		{
			source.StopAndRelease();
			isPaused = true;
		}
	}

	public void OnGameUnpause()
	{
		if (isPaused)
		{
			isPaused = false;
		}
	}
}
