using System.Collections.Generic;
using UnityEngine;

public class ROCEmitter : EffectBehaviour
{
	public Transform idleEmitter;

	public Transform burstEmitter;

	public ParticleSystem[] idleEmitters;

	public ParticleSystem[] burstEmitters;

	[KSPField]
	public string idleEffectName = "idle";

	[KSPField]
	public string burstEffectName = "burst";

	public float secondsTillBurstEmission;

	public float burstEmitterMinWait = 60f;

	public float burstEmitterMaxWait = 300f;

	public float burstTime;

	public float burstTimeFraction;

	public float burstFraction;

	public bool burstEmitting;

	public ROCsSFX[] rocsSFX;

	public bool timedBurstEmissionsEnabled;

	public bool isPaused;

	public Vector2 forceRadius;

	public float forceByDistance;

	public Vector3 forceDirection;

	public List<Part> vesselPart;

	public float vfxBaseForce;

	public float forceScale;

	public float forceMagnitude;

	public Transform forceOrigin;

	public bool applyForces;

	[SerializeField]
	public GClass0 roc;

	public float forceStartTime;

	public CapsuleCollider forceTrigger;

	public bool TimedBurstEmissionsEnabled
	{
		get
		{
			return timedBurstEmissionsEnabled;
		}
		set
		{
			if (!timedBurstEmissionsEnabled && value)
			{
				ResetSecondsTillBurst();
			}
			timedBurstEmissionsEnabled = value;
		}
	}

	public void Awake()
	{
		timedBurstEmissionsEnabled = true;
		rocsSFX = GetComponents<ROCsSFX>();
	}

	public void Start()
	{
		ResetSecondsTillBurst();
		idleEmitters = idleEmitter.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		burstEmitters = burstEmitter.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		for (int i = 0; i < idleEmitters.Length; i++)
		{
			EffectBehaviour.AddParticleEmitter(idleEmitters[i]);
			idleEmitters[i].Stop();
		}
		for (int j = 0; j < burstEmitters.Length; j++)
		{
			EffectBehaviour.AddParticleEmitter(burstEmitters[j]);
			burstEmitters[j].Stop();
		}
		StartIdleEmision();
		vesselPart = new List<Part>();
		vfxBaseForce = roc.vfxBaseForce;
		applyForces = roc.applyForces;
		forceRadius = roc.vfxForceRadius;
		forceDirection = roc.forceDirection;
		if (applyForces)
		{
			forceTrigger = base.gameObject.AddComponent<CapsuleCollider>();
			forceTrigger.isTrigger = true;
			forceTrigger.center = roc.radiusCenter;
			forceTrigger.radius = forceRadius.x;
			forceTrigger.height = forceRadius.y;
			forceTrigger.enabled = true;
		}
	}

	public void Update()
	{
		if (isPaused)
		{
			return;
		}
		if (timedBurstEmissionsEnabled)
		{
			secondsTillBurstEmission -= Time.deltaTime;
			if (secondsTillBurstEmission < 0f)
			{
				StartBurstEmision();
			}
		}
		if (burstEmitting)
		{
			BurstTimer();
			if (applyForces)
			{
				ApplyForces();
			}
		}
		else
		{
			forceStartTime = Time.time;
		}
	}

	public void OnEnable()
	{
		GameEvents.onGamePause.Add(OnGamePause);
		GameEvents.onGameUnpause.Add(OnGameUnpause);
		if (applyForces && forceTrigger != null)
		{
			forceTrigger.enabled = true;
		}
	}

	public void OnDisable()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
		if (applyForces && forceTrigger != null)
		{
			forceTrigger.enabled = false;
		}
	}

	public void AddForceTarget(GameObject target)
	{
		Part componentInParent = target.GetComponentInParent<Part>();
		if (!(componentInParent == null) && !vesselPart.Contains(componentInParent))
		{
			vesselPart.Add(componentInParent);
		}
	}

	public void RemoveForceTarget(GameObject target)
	{
		Part componentInParent = target.GetComponentInParent<Part>();
		if (!(componentInParent == null) && vesselPart.Contains(componentInParent))
		{
			vesselPart.Remove(componentInParent);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (applyForces)
		{
			AddForceTarget(other.gameObject);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		RemoveForceTarget(other.gameObject);
	}

	public void OnGamePause()
	{
		for (int i = 0; i < idleEmitters.Length; i++)
		{
			idleEmitters[i].Pause();
		}
		for (int j = 0; j < burstEmitters.Length; j++)
		{
			burstEmitters[j].Pause();
		}
		isPaused = true;
	}

	public void OnGameUnpause()
	{
		for (int i = 0; i < idleEmitters.Length; i++)
		{
			idleEmitters[i].Play();
		}
		for (int j = 0; j < burstEmitters.Length; j++)
		{
			burstEmitters[j].Play();
		}
		isPaused = false;
	}

	public void SetBurstEmitterMinWait(float value)
	{
		burstEmitterMinWait = value;
		timedBurstEmissionsEnabled = value > 0f;
	}

	public void SetBurstEmitterMaxWait(float value)
	{
		burstEmitterMaxWait = value;
		timedBurstEmissionsEnabled = value > 0f;
	}

	public void ResetSecondsTillBurst()
	{
		secondsTillBurstEmission = Random.Range(burstEmitterMinWait, burstEmitterMaxWait);
	}

	public void StartIdleEmision()
	{
		if (idleEmitters == null)
		{
			Debug.LogWarning("[ROCEmitter]: Cant play idle - no emitters found");
			return;
		}
		for (int i = 0; i < idleEmitters.Length; i++)
		{
			idleEmitters[i].Play();
		}
	}

	public void StopIdleEmision()
	{
		if (idleEmitters == null)
		{
			return;
		}
		for (int i = 0; i < idleEmitters.Length; i++)
		{
			if (idleEmitters[i].isPlaying)
			{
				idleEmitters[i].Stop();
			}
		}
	}

	public void StartBurstEmision(bool resetTimer = true)
	{
		if (burstEmitters == null)
		{
			Debug.LogWarning("[ROCEmitter]: Cant play burst - no emitters found");
			return;
		}
		for (int i = 0; i < burstEmitters.Length; i++)
		{
			burstEmitters[i].Play();
			burstEmitting = true;
		}
		if (rocsSFX != null)
		{
			for (int j = 0; j < rocsSFX.Length; j++)
			{
				if (rocsSFX[j].effectName == idleEffectName)
				{
					rocsSFX[j].FadeOutPlayback();
					continue;
				}
				burstTime = rocsSFX[j].PlayBurstSFX();
				burstTimeFraction = burstTime - burstTime * burstFraction;
			}
		}
		ResetSecondsTillBurst();
	}

	public void BurstTimer()
	{
		if (burstTime > burstTimeFraction)
		{
			burstTime -= Time.deltaTime;
			SetForceScale(Time.time - forceStartTime);
			return;
		}
		if (rocsSFX != null)
		{
			for (int i = 0; i < rocsSFX.Length; i++)
			{
				if (rocsSFX[i].effectName == idleEffectName)
				{
					rocsSFX[i].FadeInPlayback();
				}
			}
		}
		burstEmitting = false;
	}

	public void StopBurstEmision()
	{
		if (burstEmitters == null)
		{
			return;
		}
		for (int i = 0; i < burstEmitters.Length; i++)
		{
			if (burstEmitters[i].isPlaying)
			{
				burstEmitters[i].Stop();
			}
		}
	}

	public void SetSFXPower(float value)
	{
		if (rocsSFX != null)
		{
			for (int i = 0; i < rocsSFX.Length; i++)
			{
				rocsSFX[i].SetSFXVolume(value);
			}
		}
	}

	public void SetSFXClipPath(string idlepClip, string burstClip)
	{
		if (rocsSFX != null)
		{
			for (int i = 0; i < rocsSFX.Length; i++)
			{
				rocsSFX[i].SetClipsPath(idlepClip, burstClip);
			}
		}
	}

	public void ApplyForces()
	{
		if (vesselPart == null)
		{
			return;
		}
		for (int i = 0; i < vesselPart.Count; i++)
		{
			if (vesselPart[i] != null)
			{
				forceByDistance = Vector3.Distance(vesselPart[i].transform.position, forceOrigin.position);
				forceMagnitude = vfxBaseForce * forceScale / forceByDistance;
				vesselPart[i].AddForce(forceOrigin.transform.TransformDirection(forceDirection) * forceMagnitude);
			}
		}
	}

	public void SetForceScale(float burstClipTime)
	{
		forceScale = roc.GetVFXForceScale(burstClipTime);
	}
}
