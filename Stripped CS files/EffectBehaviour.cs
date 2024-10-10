using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
	public static List<KSPParticleEmitter> kspEmitters = new List<KSPParticleEmitter>();

	public static List<ParticleSystem> emitters = new List<ParticleSystem>();

	public Part hostPart;

	public string effectName = "";

	public string instanceName = "";

	public static void AddParticleEmitter(KSPParticleEmitter emitter)
	{
		if (!kspEmitters.Contains(emitter))
		{
			kspEmitters.Add(emitter);
		}
	}

	public static void RemoveParticleEmitter(KSPParticleEmitter emitter)
	{
		kspEmitters.Remove(emitter);
	}

	public static void AddParticleEmitter(ParticleSystem emitter)
	{
		if (!emitters.Contains(emitter))
		{
			emitters.Add(emitter);
		}
	}

	public static void RemoveParticleEmitter(ParticleSystem emitter)
	{
		emitters.Remove(emitter);
	}

	public static void OffsetParticles(Vector3d offset)
	{
		Vector3d vector3d = Vector3d.zero;
		int count = emitters.Count;
		while (count-- > 0)
		{
			ParticleSystem particleSystem = emitters[count];
			ParticleSystem.MainModule main = particleSystem.main;
			if (particleSystem == null)
			{
				emitters.RemoveAt(count);
			}
			else
			{
				if (main.simulationSpace != ParticleSystemSimulationSpace.World || particleSystem.particleCount == 0)
				{
					continue;
				}
				bool flag = false;
				Rigidbody componentInParent = particleSystem.GetComponentInParent<Rigidbody>();
				if (componentInParent != null)
				{
					flag = true;
					vector3d = componentInParent.velocity + Krakensbane.GetFrameVelocity();
				}
				ParticleSystem.Particle[] particleBuffer = particleSystem.GetParticleBuffer();
				int particleCount = particleSystem.particleCount;
				int num = particleCount;
				while (num-- > 0)
				{
					ParticleSystem.Particle particle = particleBuffer[num];
					Vector3d vector3d2 = particle.position;
					if (flag)
					{
						vector3d2 -= vector3d * Random.value * Time.deltaTime;
					}
					particle.position = vector3d2 + offset;
				}
				particleSystem.SetParticles(particleBuffer, particleCount);
			}
		}
		count = kspEmitters.Count;
		while (count-- > 0)
		{
			KSPParticleEmitter kSPParticleEmitter = kspEmitters[count];
			if (kSPParticleEmitter == null)
			{
				kspEmitters.RemoveAt(count);
			}
			else
			{
				if (!kSPParticleEmitter.useWorldSpace || kSPParticleEmitter.ps.particleCount == 0)
				{
					continue;
				}
				bool flag2 = false;
				Rigidbody componentInParent2 = kSPParticleEmitter.GetComponentInParent<Rigidbody>();
				if (componentInParent2 != null)
				{
					flag2 = true;
					vector3d = componentInParent2.velocity + Krakensbane.GetFrameVelocity();
				}
				ParticleSystem.Particle[] particleBuffer2 = kSPParticleEmitter.ps.GetParticleBuffer();
				int particleCount2 = kSPParticleEmitter.ps.particleCount;
				int num = particleCount2;
				while (num-- > 0)
				{
					ParticleSystem.Particle particle2 = particleBuffer2[num];
					Vector3d vector3d3 = particle2.position;
					if (flag2)
					{
						vector3d3 -= vector3d * Random.value * Time.deltaTime;
					}
					particle2.position = vector3d3 + offset;
					particleBuffer2[num] = particle2;
				}
				kSPParticleEmitter.ps.SetParticles(particleBuffer2, particleCount2);
			}
		}
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnInitialize()
	{
	}

	public virtual void OnEvent()
	{
	}

	public virtual void OnEvent(int transformIdx)
	{
		OnEvent();
	}

	public virtual void OnEvent(float power)
	{
	}

	public virtual void OnEvent(float power, int transformIdx)
	{
		OnEvent(power);
	}
}
