using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FXGroup
{
	public List<ParticleSystem> fxEmittersNewSystem = new List<ParticleSystem>();

	public List<Light> lights = new List<Light>();

	public AudioClip sfx;

	public AudioSource audio;

	public List<float> initSizeValuesNewSystem;

	public List<float> initLifeValuesNewSystem;

	public List<float> initLightValues;

	public List<float> initSizeValuesNewSystemVariation;

	public List<float> initLifeValuesNewSystemVariation;

	public bool valid;

	public string name;

	public float _minVisualPower;

	public float _maxVisualPower = 1f;

	public bool clampVisualPower;

	public bool active;

	public IEnumerator fadeCoroutine;

	public CoroutineHost fadeCoroutineHost;

	public float power;

	public float powerVariation;

	public bool activeLatch;

	public bool isValid => valid;

	public float minVisualPower
	{
		get
		{
			return _minVisualPower;
		}
		set
		{
			_minVisualPower = value;
			clampVisualPower = true;
		}
	}

	public float maxVisualPower
	{
		get
		{
			return _maxVisualPower;
		}
		set
		{
			_maxVisualPower = value;
			clampVisualPower = true;
		}
	}

	public bool Active => active;

	public float Power
	{
		get
		{
			return power;
		}
		set
		{
			SetPower(value);
		}
	}

	public FXGroup(string groupID)
	{
		name = groupID;
		_minVisualPower = 0f;
		_maxVisualPower = 1f;
	}

	public void begin(AudioSource audioRef)
	{
		valid = true;
		initSizeValuesNewSystem = new List<float>();
		initLifeValuesNewSystem = new List<float>();
		initLightValues = new List<float>();
		initSizeValuesNewSystemVariation = new List<float>();
		initLifeValuesNewSystemVariation = new List<float>();
		for (int i = 0; i < lights.Count; i++)
		{
			Light light = lights[i];
			initLightValues.Add(light.intensity);
			light.enabled = false;
		}
		for (int j = 0; j < fxEmittersNewSystem.Count; j++)
		{
			ParticleSystem particleSystem = fxEmittersNewSystem[j];
			ParticleSystem.MainModule main = particleSystem.main;
			initSizeValuesNewSystem.Add(Mathf.Max(main.startSize.constantMax, main.startSize.constantMin));
			initLifeValuesNewSystem.Add(Mathf.Max(main.startLifetime.constantMax, main.startLifetime.constantMin));
			initSizeValuesNewSystemVariation.Add(Mathf.Abs(main.startSize.constantMax - main.startSize.constantMin));
			initLifeValuesNewSystemVariation.Add(Mathf.Abs(main.startLifetime.constantMax - main.startLifetime.constantMin));
			particleSystem.Stop();
		}
		if (audio == null)
		{
			audio = audioRef;
		}
		if (sfx != null && audio != null)
		{
			audio.clip = sfx;
		}
		active = false;
	}

	public void setActive(bool value)
	{
		if (!valid)
		{
			return;
		}
		for (int i = 0; i < lights.Count; i++)
		{
			lights[i].enabled = value;
		}
		for (int j = 0; j < fxEmittersNewSystem.Count; j++)
		{
			if (value)
			{
				fxEmittersNewSystem[j].Play();
			}
			else
			{
				fxEmittersNewSystem[j].Stop();
			}
		}
		if (sfx != null && audio != null)
		{
			audio.clip = sfx;
			if (value)
			{
				if (!audio.isPlaying)
				{
					audio.time = UnityEngine.Random.Range(0f, audio.clip.length);
					audio.Play();
				}
			}
			else
			{
				audio.Stop();
			}
		}
		active = value;
	}

	public void setActiveWithAudioFade(bool value, float audioFadeLength)
	{
		if (!valid)
		{
			return;
		}
		for (int i = 0; i < lights.Count; i++)
		{
			lights[i].enabled = value;
		}
		for (int j = 0; j < fxEmittersNewSystem.Count; j++)
		{
			if (value)
			{
				fxEmittersNewSystem[j].Play();
			}
			else
			{
				fxEmittersNewSystem[j].Stop();
			}
		}
		if (sfx != null && audio != null)
		{
			audio.clip = sfx;
			if (fadeCoroutine != null && fadeCoroutineHost != null)
			{
				fadeCoroutineHost.StopCoroutine(fadeCoroutine);
			}
			if (value)
			{
				fadeCoroutine = VolumeFade(audio, GameSettings.SHIP_VOLUME, audioFadeLength);
				fadeCoroutineHost = CoroutineHost.Create("FXGroupVolumeFade", persistThroughSceneChanges: false, disposable: true);
				fadeCoroutineHost.StartCoroutine(fadeCoroutine);
				if (!audio.isPlaying)
				{
					audio.time = UnityEngine.Random.Range(0f, audio.clip.length);
					audio.Play();
				}
			}
			else
			{
				fadeCoroutine = VolumeFade(audio, 0f, audioFadeLength);
				fadeCoroutineHost = CoroutineHost.Create("FXGroupVolumeFade", persistThroughSceneChanges: false, disposable: true);
				fadeCoroutineHost.StartCoroutine(fadeCoroutine);
			}
		}
		active = value;
	}

	public IEnumerator VolumeFade(AudioSource audioSource, float endVolume, float fadeLength)
	{
		float startTime = Time.unscaledTime;
		float startVolume = audioSource.volume;
		while (audioSource.isPlaying && !(Time.unscaledTime >= startTime + fadeLength))
		{
			float num = (startTime + fadeLength - Time.unscaledTime) / fadeLength;
			num *= num;
			audioSource.volume = num * startVolume + endVolume * (1f - num);
			yield return null;
		}
		audioSource.volume = endVolume;
		if (endVolume == 0f)
		{
			audioSource.Stop();
		}
	}

	public void Burst()
	{
		if (valid)
		{
			for (int i = 0; i < fxEmittersNewSystem.Count; i++)
			{
				fxEmittersNewSystem[i].Play();
			}
			if (sfx != null && audio != null)
			{
				audio.PlayOneShot(sfx);
			}
		}
	}

	public void SetVisualMinMax(float min, float max)
	{
		_minVisualPower = min;
		_maxVisualPower = max;
		clampVisualPower = true;
	}

	public void SetLatch(bool latch)
	{
		if (valid && latch)
		{
			setActive(value: true);
			activeLatch = true;
		}
	}

	public void SetPower(float pwr)
	{
		if (clampVisualPower)
		{
			power = Mathf.Lerp(minVisualPower, maxVisualPower, pwr);
		}
		else
		{
			power = pwr;
		}
		if (valid)
		{
			int i = 0;
			for (int count = lights.Count; i < count; i++)
			{
				lights[i].intensity = initLightValues[i] * pwr;
			}
			int j = 0;
			for (int count2 = fxEmittersNewSystem.Count; j < count2; j++)
			{
				ParticleSystem.MainModule main = fxEmittersNewSystem[j].main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.constantMin = (initLifeValuesNewSystem[j] - initLifeValuesNewSystemVariation[j]) * power;
				startLifetime.constantMax = initLifeValuesNewSystem[j] * power;
				startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.constantMin = (initSizeValuesNewSystem[j] - initSizeValuesNewSystemVariation[j]) * power;
				startSize.constantMax = initSizeValuesNewSystem[j] * power;
				startSize.mode = ParticleSystemCurveMode.TwoConstants;
				main.startSize = startSize;
			}
			if (sfx != null && audio != null)
			{
				audio.pitch = pwr;
			}
		}
	}

	public void SetPowerLatch(float pwr)
	{
		if (!activeLatch)
		{
			return;
		}
		if (clampVisualPower)
		{
			power = Mathf.Lerp(minVisualPower, maxVisualPower, pwr);
		}
		else
		{
			power = Mathf.Clamp01(pwr);
		}
		if (valid)
		{
			int i = 0;
			for (int count = lights.Count; i < count; i++)
			{
				Light light = lights[i];
				light.intensity = Mathf.Max(initLightValues[i] * pwr, light.intensity);
			}
			int j = 0;
			for (int count2 = fxEmittersNewSystem.Count; j < count2; j++)
			{
				ParticleSystem.MainModule main = fxEmittersNewSystem[j].main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.constantMin = Mathf.Max(initLifeValuesNewSystem[j] * power - powerVariation * 0.5f, startLifetime.constantMin);
				startLifetime.constantMax = Mathf.Max(initLifeValuesNewSystem[j] * power + powerVariation * 0.5f, startLifetime.constantMax);
				startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.constantMin = Mathf.Max(initSizeValuesNewSystem[j] * power - powerVariation * 0.5f, startSize.constantMin);
				startSize.constantMax = Mathf.Max(initSizeValuesNewSystem[j] * power + powerVariation * 0.5f, startSize.constantMax);
				startSize.mode = ParticleSystemCurveMode.TwoConstants;
				main.startSize = startSize;
			}
			if (sfx != null && audio != null)
			{
				audio.pitch = Mathf.Max(pwr, audio.pitch);
			}
		}
	}

	public void Unlatch()
	{
		if (valid && activeLatch)
		{
			setActive(value: false);
			activeLatch = false;
			Power = power;
		}
	}
}
