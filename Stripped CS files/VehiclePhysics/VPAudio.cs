using System;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Audio", 0)]
public class VPAudio : VehicleBehaviour
{
	[Serializable]
	public class Engine
	{
		public AudioSource audioSource;

		public float audioBaseRpm = 5000f;

		[Space(5f)]
		public float volumeAtRest = 0.4f;

		public float volumeAtFullLoad = 0.8f;

		public float volumeChangeRateUp = 24f;

		public float volumeChangeRateDown = 8f;
	}

	[Serializable]
	public class EngineExtras
	{
		public AudioSource turboAudioSource;

		public float turboMinRpm = 3500f;

		public float turboMaxRpm = 5500f;

		[Range(0f, 3f)]
		public float turboMinPitch = 0.8f;

		[Range(0f, 3f)]
		public float turboMaxPitch = 1.5f;

		[Range(0f, 1f)]
		public float turboMaxVolume = 1f;

		[Space(5f)]
		public AudioSource transmissionAudioSource;

		public float transmissionMinRpm = 500f;

		public float transmissionMaxRpm = 5500f;

		[Range(0f, 3f)]
		public float transmissionMinPitch = 0.2f;

		[Range(0f, 3f)]
		public float transmissionMaxPitch = 1.1f;

		[Range(0f, 1f)]
		public float transmissionMinVolume = 0.1f;

		[Range(0f, 1f)]
		public float transmissionMaxVolume = 0.2f;

		[NonSerialized]
		public float turboRatioChangeRate = 8f;
	}

	[Serializable]
	public class Wheels
	{
		public AudioSource skidAudioSource;

		public float skidMinSlip = 2f;

		public float skidMaxSlip = 7f;

		[Range(0f, 3f)]
		public float skidMinPitch = 0.9f;

		[Range(0f, 3f)]
		public float skidMaxPitch = 0.8f;

		[Range(0f, 1f)]
		public float skidMaxVolume = 0.75f;

		[Range(0f, 1f)]
		public float skidIntensity = 0.5f;

		[Space(5f)]
		public AudioSource offroadAudioSource;

		public float offroadMinSpeed = 1f;

		public float offroadMaxSpeed = 20f;

		[Range(0f, 3f)]
		public float offroadMinPitch = 0.3f;

		[Range(0f, 3f)]
		public float offroadMaxPitch = 2.5f;

		[Range(0f, 1f)]
		public float offroadMinVolume = 0.3f;

		[Range(0f, 1f)]
		public float offroadMaxVolume = 0.6f;

		[Space(5f)]
		public AudioClip bumpAudioClip;

		public float bumpMinForceDelta = 2000f;

		public float bumpMaxForceDelta = 18000f;

		[Range(0f, 1f)]
		public float bumpMinVolume = 0.2f;

		[Range(0f, 1f)]
		public float bumpMaxVolume = 0.6f;

		[NonSerialized]
		public float skidRatioChangeRate = 40f;

		[NonSerialized]
		public float offroadSpeedChangeRate = 20f;

		[NonSerialized]
		public float offroadCutoutSpeed = 0.02f;
	}

	[Serializable]
	public class Impacts
	{
		[Space(5f)]
		public AudioClip hardImpactAudioClip;

		public AudioClip softImpactAudioClip;

		public float minSpeed = 0.1f;

		public float maxSpeed = 10f;

		[Range(0f, 3f)]
		public float minPitch = 0.3f;

		[Range(0f, 3f)]
		public float maxPitch = 0.6f;

		[Range(0f, 3f)]
		public float randomPitch = 0.2f;

		[Range(0f, 1f)]
		public float minVolume = 0.7f;

		[Range(0f, 1f)]
		public float maxVolume = 1f;

		[Range(0f, 1f)]
		public float randomVolume = 0.2f;
	}

	[Serializable]
	public class Drags
	{
		public AudioSource hardDragAudioSource;

		public AudioSource softDragAudioSource;

		public float minSpeed = 2f;

		public float maxSpeed = 20f;

		[Range(0f, 3f)]
		public float minPitch = 0.6f;

		[Range(0f, 3f)]
		public float maxPitch = 0.8f;

		[Range(0f, 1f)]
		public float minVolume = 0.8f;

		[Range(0f, 1f)]
		public float maxVolume = 1f;

		[Space(5f)]
		public AudioClip scratchAudioClip;

		public float scratchRandomThreshold = 0.02f;

		public float scratchMinSpeed = 2f;

		public float scratchMinInterval = 0.2f;

		[Range(0f, 3f)]
		public float scratchMinPitch = 0.7f;

		[Range(0f, 3f)]
		public float scratchMaxPitch = 1.1f;

		[Range(0f, 1f)]
		public float scratchMinVolume = 0.9f;

		[Range(0f, 1f)]
		public float scratchMaxVolume = 1f;

		[NonSerialized]
		public float cutoutSpeed = 0.01f;
	}

	[Serializable]
	public class Wind
	{
		public AudioSource windAudioSource;

		public float minSpeed = 3f;

		public float maxSpeed = 30f;

		[Range(0f, 3f)]
		public float minPitch = 0.5f;

		[Range(0f, 3f)]
		public float maxPitch = 1f;

		[Range(0f, 1f)]
		public float maxVolume = 0.5f;
	}

	public class WheelAudioData
	{
		public float lastDownforce;

		public float lastWheelBumpTime;
	}

	[Tooltip("AudioSource to be used with the one-time audio effects (impacts, etc)")]
	public AudioSource audioClipTemplate;

	[Space(2f)]
	public Engine engine = new Engine();

	[Space(2f)]
	public EngineExtras engineExtras = new EngineExtras();

	[Space(2f)]
	public Wheels wheels = new Wheels();

	[Space(2f)]
	public Impacts impacts = new Impacts();

	[Space(2f)]
	public Drags drags = new Drags();

	[Space(2f)]
	public Wind wind = new Wind();

	[Space(2f)]
	public InterpolatedFloat m_engineRpm = new InterpolatedFloat();

	public InterpolatedFloat m_engineLoadRatio = new InterpolatedFloat();

	public bool m_prevEngineLimiter;

	public bool m_engineLimiterActive;

	public float m_engineLimiterTime;

	public float m_skidRatio;

	public float m_offroadSpeed;

	public float m_lastScratchTime;

	public float m_turboRatio;

	public WheelAudioData[] m_audioData = new WheelAudioData[0];

	public override void OnEnableVehicle()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Combine(vehicleBase.onImpact, new Action(ProcessImpactsAudio));
		m_audioData = new WheelAudioData[base.vehicle.wheelState.Length];
		for (int i = 0; i < m_audioData.Length; i++)
		{
			m_audioData[i] = new WheelAudioData();
		}
		SetupLoopAudioSource(engine.audioSource);
		SetupLoopAudioSource(engineExtras.turboAudioSource);
		SetupLoopAudioSource(engineExtras.transmissionAudioSource);
		SetupLoopAudioSource(wheels.skidAudioSource);
		SetupLoopAudioSource(wheels.offroadAudioSource);
		SetupLoopAudioSource(wind.windAudioSource);
		SetupLoopAudioSource(drags.hardDragAudioSource);
		SetupLoopAudioSource(drags.softDragAudioSource);
	}

	public override void OnDisableVehicle()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Remove(vehicleBase.onImpact, new Action(ProcessImpactsAudio));
		StopAudio(engine.audioSource);
		StopAudio(engineExtras.turboAudioSource);
		StopAudio(engineExtras.transmissionAudioSource);
		StopAudio(wheels.skidAudioSource);
		StopAudio(wheels.offroadAudioSource);
		StopAudio(wind.windAudioSource);
		StopAudio(drags.hardDragAudioSource);
		StopAudio(drags.softDragAudioSource);
	}

	public override void FixedUpdateVehicle()
	{
		if (!base.vehicle.paused)
		{
			ProcessEngineAudio();
			PrcessEngineExtraAudio();
			ProcessWheelBumpAudio();
			ProcessBodyDragAudio();
			ProcessWindAudio();
		}
	}

	public override void UpdateVehicle()
	{
		if (!base.vehicle.paused)
		{
			ProcessInterpolatedEngineAudio();
			ProcessTireAudio();
		}
	}

	public override void OnEnterPause()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Remove(vehicleBase.onImpact, new Action(ProcessImpactsAudio));
		StopAudio(engine.audioSource);
		StopAudio(engineExtras.turboAudioSource);
		StopAudio(engineExtras.transmissionAudioSource);
		StopAudio(wheels.skidAudioSource);
		StopAudio(wheels.offroadAudioSource);
		StopAudio(wind.windAudioSource);
		StopAudio(drags.hardDragAudioSource);
		StopAudio(drags.softDragAudioSource);
	}

	public override void OnLeavePause()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onImpact = (Action)Delegate.Combine(vehicleBase.onImpact, new Action(ProcessImpactsAudio));
	}

	public void ProcessEngineAudio()
	{
		if (!(engine.audioSource == null))
		{
			int[] array = base.vehicle.data.Get(1);
			m_engineRpm.Set(Mathf.Abs((float)array[1] / 1000f));
			m_engineLoadRatio.Set(Mathf.Abs((float)array[6] / 1000f));
			bool flag = array[5] != 0;
			if (!m_prevEngineLimiter && flag)
			{
				m_engineLimiterActive = true;
				m_engineLimiterTime = Time.time;
			}
			m_prevEngineLimiter = flag;
			if (m_engineLimiterActive && Time.time - m_engineLimiterTime >= 0.15f)
			{
				m_engineLimiterActive = false;
			}
		}
	}

	public void ProcessInterpolatedEngineAudio()
	{
		float frameRatio = InterpolatedFloat.GetFrameRatio();
		AdjustVolumeWithRatio(engine.audioSource, engine.volumeAtRest, engine.volumeAtFullLoad, m_engineLoadRatio.GetInterpolated(frameRatio), m_engineLimiterActive ? 1000f : engine.volumeChangeRateUp, m_engineLimiterActive ? 1000f : engine.volumeChangeRateDown);
		PlayContinuousAudio(engine.audioSource, engine.audioBaseRpm, m_engineRpm.GetInterpolated(frameRatio));
	}

	public void PrcessEngineExtraAudio()
	{
		float b = Mathf.InverseLerp(engineExtras.turboMinRpm, engineExtras.turboMaxRpm, m_engineRpm.Get()) * m_engineLoadRatio.Get();
		m_turboRatio = Mathf.Lerp(m_turboRatio, b, engineExtras.turboRatioChangeRate * Time.deltaTime);
		PlayContinuousAudio(engineExtras.turboAudioSource, m_turboRatio, engineExtras.turboMinPitch, engineExtras.turboMaxPitch, 0f, engineExtras.turboMaxVolume);
		float value = (float)base.vehicle.data.Get(1, 16) / 1000f;
		PlayContinuousAudioWithFallOff(engineExtras.transmissionAudioSource, value, engineExtras.transmissionMinRpm, engineExtras.transmissionMaxRpm, engineExtras.transmissionMinPitch, engineExtras.transmissionMaxPitch, engineExtras.transmissionMinVolume, engineExtras.transmissionMaxVolume);
	}

	public void ProcessTireAudio()
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		VehicleBase.WheelState[] wheelState = base.vehicle.wheelState;
		foreach (VehicleBase.WheelState wheelState2 in wheelState)
		{
			if (wheelState2.groundMaterial != null && wheelState2.groundMaterial.surfaceType != 0)
			{
				if (wheelState2.grounded)
				{
					num2 += wheelState2.wheelVelocity.magnitude + Mathf.Abs(wheelState2.tireSlip.y);
					num3++;
				}
			}
			else
			{
				num += Mathf.InverseLerp(wheels.skidMinSlip, wheels.skidMaxSlip, wheelState2.combinedTireSlip * wheelState2.normalizedLoad);
			}
		}
		m_skidRatio = Mathf.Lerp(m_skidRatio, num * wheels.skidIntensity, wheels.skidRatioChangeRate * Time.deltaTime);
		PlayContinuousAudio(wheels.skidAudioSource, m_skidRatio, wheels.skidMinPitch, wheels.skidMaxPitch, 0f, wheels.skidMaxVolume);
		if (num3 > 1)
		{
			num2 /= (float)num3;
		}
		m_offroadSpeed = Mathf.Lerp(m_offroadSpeed, num2, wheels.offroadSpeedChangeRate * Time.deltaTime);
		PlaySpeedBasedAudio(wheels.offroadAudioSource, m_offroadSpeed, wheels.offroadCutoutSpeed, wheels.offroadMinSpeed, wheels.offroadMaxSpeed, 0f, wheels.offroadMinPitch, wheels.offroadMaxPitch, wheels.offroadMinVolume, wheels.offroadMaxVolume);
	}

	public void ProcessWheelBumpAudio()
	{
		if (wheels.bumpAudioClip == null)
		{
			return;
		}
		int i = 0;
		for (int num = base.vehicle.wheelState.Length; i < num; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[i];
			WheelAudioData wheelAudioData = m_audioData[i];
			float num2 = wheelState.downforce - wheelAudioData.lastDownforce;
			wheelAudioData.lastDownforce = wheelState.downforce;
			if (num2 > wheels.bumpMinForceDelta && Time.fixedTime - wheelAudioData.lastWheelBumpTime > 0.03f)
			{
				PlayWheelBumpAudio(num2, wheelState.wheelCol.cachedTransform.position);
				wheelAudioData.lastWheelBumpTime = Time.fixedTime;
			}
		}
	}

	public void ProcessWindAudio()
	{
		float ratio = Mathf.InverseLerp(wind.minSpeed, wind.maxSpeed, base.vehicle.cachedRigidbody.velocity.magnitude);
		PlayContinuousAudio(wind.windAudioSource, ratio, wind.minPitch, wind.maxPitch, 0f, wind.maxVolume);
	}

	public void ProcessImpactsAudio()
	{
		if (!(impacts.hardImpactAudioClip != null) && !(impacts.softImpactAudioClip != null))
		{
			return;
		}
		float magnitude = base.vehicle.localImpactVelocity.magnitude;
		if (magnitude > impacts.minSpeed)
		{
			float t = Mathf.InverseLerp(impacts.minSpeed, impacts.maxSpeed, magnitude);
			AudioClip audioClip = null;
			audioClip = ((!(impacts.softImpactAudioClip == null)) ? (base.vehicle.isHardImpact ? impacts.hardImpactAudioClip : impacts.softImpactAudioClip) : impacts.hardImpactAudioClip);
			if (audioClip != null)
			{
				PlayOneTime(audioClip, base.vehicle.cachedTransform.TransformPoint(base.vehicle.localImpactPosition), Mathf.Lerp(impacts.minVolume, impacts.maxVolume, t) + UnityEngine.Random.Range(0f - impacts.randomVolume, impacts.randomVolume), Mathf.Lerp(impacts.minPitch, impacts.maxPitch, t) + UnityEngine.Random.Range(0f - impacts.randomPitch, impacts.randomPitch));
			}
		}
	}

	public void ProcessBodyDragAudio()
	{
		float magnitude = base.vehicle.localDragVelocity.magnitude;
		float speed = (base.vehicle.isHardDrag ? magnitude : 0f);
		float speed2 = (base.vehicle.isHardDrag ? 0f : magnitude);
		PlaySpeedBasedAudio(drags.hardDragAudioSource, speed, drags.cutoutSpeed, drags.minSpeed, drags.maxSpeed, 0f, drags.minPitch, drags.maxPitch, drags.minVolume, drags.maxVolume);
		PlaySpeedBasedAudio(drags.softDragAudioSource, speed2, drags.cutoutSpeed, drags.minSpeed, drags.maxSpeed, 0f, drags.minPitch, drags.maxPitch, drags.minVolume, drags.maxVolume);
		if (drags.scratchAudioClip != null && magnitude > drags.scratchMinSpeed && base.vehicle.isHardDrag && UnityEngine.Random.value < drags.scratchRandomThreshold && Time.fixedTime - m_lastScratchTime > drags.scratchMinInterval)
		{
			PlayOneTime(drags.scratchAudioClip, base.vehicle.cachedTransform.TransformPoint(base.vehicle.localDragPosition), UnityEngine.Random.Range(drags.scratchMinVolume, drags.scratchMaxVolume), UnityEngine.Random.Range(drags.scratchMinPitch, drags.scratchMaxPitch));
			m_lastScratchTime = Time.fixedTime;
		}
	}

	public void SetupLoopAudioSource(AudioSource audio)
	{
		if (audio != null)
		{
			audio.loop = true;
			audio.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
		}
	}

	public void PlayContinuousAudio(AudioSource audio, float baseValue, float value)
	{
		if (!(audio == null))
		{
			audio.pitch = value / baseValue;
			MuteZeroPitch(audio);
			if (!audio.isPlaying)
			{
				audio.Play();
			}
		}
	}

	public void PlayContinuousAudio(AudioSource audio, float ratio, float minPitch, float maxPitch, float minVolume, float maxVolume)
	{
		if (!(audio == null))
		{
			audio.pitch = Mathf.Lerp(minPitch, maxPitch, ratio);
			audio.volume = Mathf.Lerp(minVolume, maxVolume, ratio);
			MuteZeroPitch(audio);
			if (!audio.isPlaying)
			{
				audio.Play();
			}
		}
	}

	public void PlayContinuousAudioWithFallOff(AudioSource audio, float value, float minValue, float maxValue, float minPitch, float maxPitch, float minVolume, float maxVolume)
	{
		PlaySpeedBasedAudio(audio, value, minValue * 0.01f, minValue, maxValue, 0f, minPitch, maxPitch, minVolume, maxVolume);
	}

	public void AdjustVolumeWithRatio(AudioSource audio, float minVolume, float maxVolume, float ratio, float changeRateUp, float changeRateDown)
	{
		if (!(audio == null))
		{
			float num = Mathf.Lerp(minVolume, maxVolume, ratio);
			float num2 = ((num > audio.volume) ? changeRateUp : changeRateDown);
			audio.volume = Mathf.Lerp(audio.volume, num, Time.deltaTime * num2);
		}
	}

	public void PlaySpeedBasedAudio(AudioSource audio, float speed, float cutoutSpeed, float minSpeed, float maxSpeed, float cutoutPitch, float minPitch, float maxPitch, float minVolume, float maxVolume)
	{
		if (audio == null)
		{
			return;
		}
		if (speed < cutoutSpeed)
		{
			if (audio.isPlaying)
			{
				audio.Stop();
			}
			return;
		}
		if (speed < minSpeed)
		{
			float t = Mathf.InverseLerp(cutoutSpeed, minSpeed, speed);
			audio.pitch = Mathf.Lerp(cutoutPitch, minPitch, t);
			audio.volume = Mathf.Lerp(0f, minVolume, t);
		}
		else
		{
			float t2 = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
			audio.pitch = Mathf.Lerp(minPitch, maxPitch, t2);
			audio.volume = Mathf.Lerp(minVolume, maxVolume, t2);
		}
		MuteZeroPitch(audio);
		if (!audio.isPlaying)
		{
			audio.Play();
		}
	}

	public void MuteZeroPitch(AudioSource audio)
	{
		float num = MathUtility.FastAbs(audio.pitch);
		if (num < 0.05f)
		{
			audio.volume *= num / 0.05f;
		}
	}

	public void StopAudio(AudioSource audio)
	{
		if (audio != null)
		{
			audio.Stop();
		}
	}

	public void PlayWheelBumpAudio(float suspensionForceDelta, Vector3 position)
	{
		float t = Mathf.InverseLerp(wheels.bumpMinForceDelta, wheels.bumpMaxForceDelta, suspensionForceDelta);
		PlayOneTime(wheels.bumpAudioClip, position, Mathf.Lerp(wheels.bumpMinVolume, wheels.bumpMaxVolume, t));
	}

	public void PlayOneTime(AudioClip clip, Vector3 position, float volume)
	{
		PlayOneTime(clip, position, volume, 1f);
	}

	public void PlayOneTime(AudioClip clip, Vector3 position, float volume, float pitch)
	{
		if (!(clip == null) && !(pitch < 0.01f) && volume >= 0.01f)
		{
			GameObject gameObject;
			AudioSource audioSource;
			if (audioClipTemplate != null)
			{
				gameObject = UnityEngine.Object.Instantiate(audioClipTemplate.gameObject, position, Quaternion.identity);
				audioSource = gameObject.GetComponent<AudioSource>();
				gameObject.transform.parent = audioClipTemplate.transform.parent;
			}
			else
			{
				gameObject = new GameObject("One shot audio");
				gameObject.transform.parent = base.vehicle.cachedTransform;
				gameObject.transform.position = position;
				audioSource = null;
			}
			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.spatialBlend = 1f;
			}
			audioSource.clip = clip;
			audioSource.volume = volume;
			audioSource.pitch = pitch;
			audioSource.dopplerLevel = 0f;
			MuteZeroPitch(audioSource);
			audioSource.Play();
			UnityEngine.Object.Destroy(gameObject, clip.length / pitch);
		}
	}
}
