using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns10;

public class FireworkFXComponent : MonoBehaviour
{
	[Tooltip("Usually the particle system on the parent object.")]
	public ParticleSystem parentParticles;

	[Tooltip("These will have a Color Over Lifetime gradient applied with the first and second trail colors of the PAW.")]
	public List<ParticleSystem> trailMainColorParticles;

	[Tooltip("These will have a Color Over Lifetime gradient applied with the first and second burst colors of the PAW.")]
	public List<ParticleSystem> burstMainColorParticles;

	[Tooltip("The third color of the PAW will replace the start color of these.")]
	public List<ParticleSystem> burstColor3Particles;

	[Tooltip("These will have their Start Speed multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstSpreadParticles;

	[Tooltip("These will have their Start Lifetime multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstDurationParticles;

	[Tooltip("These will have their Start Size multiplied by the Burst Duration value of the PAW.")]
	public List<ParticleSystem> burstFlareSizeParticles;

	public FireworkEffectType fwEffectType;

	public ParticleSystem[] particleSystems;

	public AudioSource audioSource;

	public AudioClip crackleSFX;

	public int maxAmountOfCrackleInstances = 6;

	public Quaternion initialRotation = Quaternion.identity;

	public Transform objectTransform;

	public FireworkFX fwFXController;

	public bool hasStarted;

	public bool hasEnded;

	public bool burstActivated;

	public ParticleSystem.Particle[] particlePool;

	public float[] particleLifetimes;

	public float currentCrackleSFXInstances;

	public void Die()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	public void Initialize(GameObject trailGO, FireworkFX controller, FireworkEffectType type)
	{
		objectTransform = trailGO.transform;
		fwFXController = controller;
		fwEffectType = type;
		configureParticleSystems();
	}

	public void OnDestroy()
	{
		FXMonger.RemoveFireworkFX(this);
	}

	public void ActivateBurstPS(Vector3 position)
	{
		if (fwEffectType == FireworkEffectType.BURST)
		{
			int num = particleSystems.Length;
			while (num-- > 0)
			{
				if (!particleSystems[num].isPlaying)
				{
					particleSystems[num].Play();
				}
			}
			objectTransform.SetPositionAndRotation(position, initialRotation);
			if (audioSource != null && !audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		burstActivated = true;
	}

	public void configureParticleSystems()
	{
		particleLifetimes = new float[parentParticles.main.maxParticles];
		particlePool = new ParticleSystem.Particle[parentParticles.main.maxParticles];
		particleSystems = objectTransform.GetComponentsInChildren<ParticleSystem>();
		FireworkEffectType fireworkEffectType = fwEffectType;
		if (fireworkEffectType != 0 && fireworkEffectType == FireworkEffectType.BURST)
		{
			int num = particleSystems.Length;
			while (num-- > 0)
			{
				ParticleSystem.MainModule main = particleSystems[num].main;
				main.simulationSpeed = 1f;
				setParticlesCBGravity(particleSystems[num].forceOverLifetime);
			}
			int count = burstMainColorParticles.Count;
			while (count-- > 0)
			{
				ParticleSystem.MainModule main = burstMainColorParticles[count].main;
				main.startColor = XKCDColors.LightGrey;
				ParticleSystem.ColorOverLifetimeModule colorOverLifetime = burstMainColorParticles[count].colorOverLifetime;
				colorOverLifetime.color = createColorGradient(new Gradient(), colorOverLifetime.color.gradient.alphaKeys, colorOverLifetime.color.gradient.colorKeys);
			}
			int count2 = burstColor3Particles.Count;
			while (count2-- > 0)
			{
				ParticleSystem.MainModule main = burstColor3Particles[count2].main;
				main.startColor = fwFXController.burstColor3;
			}
			int count3 = burstSpreadParticles.Count;
			while (count3-- > 0)
			{
				ParticleSystem.MainModule main = burstSpreadParticles[count3].main;
				main.startSpeedMultiplier = main.startSpeed.constant * fwFXController.burstSpread;
			}
			int count4 = burstDurationParticles.Count;
			while (count4-- > 0)
			{
				ParticleSystem.MainModule main = burstDurationParticles[count4].main;
				main.startLifetimeMultiplier = main.startLifetime.constant * fwFXController.burstDuration;
			}
			int count5 = burstFlareSizeParticles.Count;
			while (count5-- > 0)
			{
				ParticleSystem.MainModule main = burstFlareSizeParticles[count5].main;
				main.startSizeMultiplier = main.startSize.constant * fwFXController.burstFlareSize;
			}
			if (fwFXController.randomizeBurstOrientation)
			{
				Vector3 insideUnitSphere = Random.insideUnitSphere;
				int num2 = particleSystems.Length;
				while (num2-- > 0)
				{
					ParticleSystem.ShapeModule shape = particleSystems[num2].shape;
					shape.rotation = Quaternion.LookRotation(objectTransform.position + insideUnitSphere, base.transform.up).eulerAngles;
				}
			}
		}
		else
		{
			objectTransform.parent = fwFXController?.transform;
			objectTransform.localPosition = Vector3.zero;
			objectTransform.localRotation = initialRotation;
			ParticleSystem.MainModule main = parentParticles.main;
			if (!(fwFXController.minTrailLifeTime < 0f) && fwFXController.maxTrailLifeTime >= 0f)
			{
				main.startLifetime = Mathf.Clamp(fwFXController.flightDuration, fwFXController.minTrailLifeTime, fwFXController.maxTrailLifeTime);
			}
			else
			{
				main.startLifetime = fwFXController.flightDuration;
			}
			main.simulationSpeed = 1f;
			int count6 = trailMainColorParticles.Count;
			while (count6-- > 0)
			{
				main = trailMainColorParticles[count6].main;
				main.startColor = XKCDColors.LightGrey;
				ParticleSystem.ColorOverLifetimeModule colorOverLifetime = trailMainColorParticles[count6].colorOverLifetime;
				Gradient baseGradient = new Gradient();
				colorOverLifetime.color = createColorGradient(baseGradient, colorOverLifetime.color.gradient.alphaKeys, colorOverLifetime.color.gradient.colorKeys);
			}
		}
		FXMonger.AddFireworkFX(this);
	}

	public ParticleSystem.MinMaxGradient createColorGradient(Gradient baseGradient, GradientAlphaKey[] alphaKeys, GradientColorKey[] colorKeys)
	{
		Color color = ((fwEffectType == FireworkEffectType.TRAIL) ? fwFXController.trailColor1 : fwFXController.burstColor1);
		Color color2 = ((fwEffectType == FireworkEffectType.TRAIL) ? fwFXController.trailColor2 : fwFXController.burstColor2);
		if (colorKeys.Length > 1)
		{
			colorKeys[0].color = color;
			colorKeys[1].color = color2;
		}
		else
		{
			colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(color2, 1f)
			};
		}
		baseGradient.SetKeys(colorKeys, alphaKeys);
		return new ParticleSystem.MinMaxGradient(baseGradient);
	}

	public bool particleSystemsPlaying()
	{
		int num = particleSystems.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (!particleSystems[num].isPlaying);
		return true;
	}

	public void setParticlesCBGravity(ParticleSystem.ForceOverLifetimeModule folm)
	{
		folm.enabled = true;
		Vector3 vector = ((!(FlightGlobals.ActiveVessel != null) || !(FlightGlobals.ActiveVessel.mainBody != null) || !(FlightGlobals.ActiveVessel.altitude < FlightGlobals.ActiveVessel.mainBody.minOrbitalDistance - FlightGlobals.ActiveVessel.mainBody.Radius)) ? ((Vector3)(FlightGlobals.ActiveVessel.geeForce * FlightGlobals.ActiveVessel.graviticAcceleration)) : ((Vector3)FlightGlobals.ActiveVessel.graviticAcceleration));
		folm.x = vector.x;
		folm.y = vector.y;
		folm.z = vector.z;
	}

	public void Update()
	{
		if (fwEffectType == FireworkEffectType.TRAIL)
		{
			if (burstActivated && particleSystemsPlaying())
			{
				parentParticles.Stop();
				ParticleSystem.MainModule main = parentParticles.main;
				main.startSizeMultiplier = 0f;
			}
		}
		else if (burstActivated)
		{
			if (!hasStarted && particleSystemsPlaying())
			{
				hasStarted = true;
			}
			else if (hasStarted && !hasEnded && !particleSystemsPlaying())
			{
				hasEnded = true;
			}
			else if (hasStarted && hasEnded && audioSource != null && !audioSource.isPlaying)
			{
				StopAllCoroutines();
				Die();
			}
		}
	}

	public void LateUpdate()
	{
		if (!burstActivated)
		{
			return;
		}
		parentParticles.GetParticles(particlePool);
		for (int i = 0; i < particlePool.Length; i++)
		{
			if (particleLifetimes[i] < particlePool[i].remainingLifetime && particlePool[i].remainingLifetime > 0f)
			{
				StartCoroutine(PlaySFXOnParticleDeath(particlePool[i].remainingLifetime));
			}
			particleLifetimes[i] = particlePool[i].remainingLifetime;
		}
	}

	public IEnumerator PlaySFXOnParticleDeath(float lifetime)
	{
		yield return new WaitForSeconds(lifetime);
		if (crackleSFX != null && currentCrackleSFXInstances <= (float)maxAmountOfCrackleInstances)
		{
			if (FlightGlobals.ActiveVessel.orbit.referenceBody.atmosphere && FlightGlobals.ActiveVessel.altitude < FlightGlobals.ActiveVessel.orbit.referenceBody.atmosphereDepth)
			{
				audioSource.PlayOneShot(crackleSFX);
			}
			currentCrackleSFXInstances += 1f;
		}
	}
}
