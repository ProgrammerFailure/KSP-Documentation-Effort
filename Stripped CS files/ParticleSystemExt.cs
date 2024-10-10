using UnityEngine;

public static class ParticleSystemExt
{
	public static ParticleSystem.Particle[] particleBuffer = new ParticleSystem.Particle[10000];

	public static ParticleSystem.Particle[] GetParticleBuffer(this ParticleSystem particleSystem)
	{
		if (particleBuffer.Length < particleSystem.particleCount)
		{
			particleBuffer = new ParticleSystem.Particle[particleSystem.particleCount * 2];
		}
		particleSystem.GetParticles(particleBuffer);
		return particleBuffer;
	}
}
