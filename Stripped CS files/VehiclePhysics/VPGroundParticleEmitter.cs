using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Ground Materials/Ground Particle Emitter")]
[RequireComponent(typeof(ParticleSystem))]
public class VPGroundParticleEmitter : MonoBehaviour
{
	public enum Mode
	{
		PressureAndSkid,
		PressureAndVelocity
	}

	public Mode mode;

	public float emissionRate = 10f;

	[Range(0f, 1f)]
	public float emissionShuffle;

	public float maxLifetime = 7f;

	public float minVelocity = 1f;

	public float maxVelocity = 15f;

	[Range(0f, 1f)]
	public float wheelVelocityRatio = 0.1f;

	[Range(0f, 1f)]
	public float tireVelocityRatio = 0.5f;

	public Color Color1 = Color.white;

	public Color Color2 = Color.gray;

	public bool randomColor;

	public ParticleSystem m_particles;

	public ParticleSystem.EmitParams m_emitParams;

	public void OnEnable()
	{
		m_particles = GetComponent<ParticleSystem>();
		m_particles.Stop();
	}

	public float EmitParticle(Vector3 position, Vector3 wheelVelocity, Vector3 tireVelocity, float pressureRatio, float intensityRatio, float lastParticleTime)
	{
		if (!base.isActiveAndEnabled)
		{
			return -1f;
		}
		if (lastParticleTime < 0f)
		{
			lastParticleTime = Time.time - 1f / emissionRate;
		}
		int num = (int)((Time.time - lastParticleTime) * emissionRate);
		if (num <= 0)
		{
			return lastParticleTime;
		}
		float num2 = 0f;
		switch (mode)
		{
		case Mode.PressureAndVelocity:
		{
			float value = tireVelocity.magnitude + wheelVelocity.magnitude;
			num2 = pressureRatio * maxLifetime * Mathf.InverseLerp(minVelocity, maxVelocity, value);
			break;
		}
		case Mode.PressureAndSkid:
			num2 = pressureRatio * intensityRatio * maxLifetime;
			break;
		}
		if (num2 <= 0f)
		{
			return -1f;
		}
		for (int i = 0; i < num; i++)
		{
			Vector3 velocity = wheelVelocity * wheelVelocityRatio + tireVelocity * tireVelocityRatio;
			float num3 = num2 * Random.Range(0.6f, 1.4f);
			float startSize = num3 / maxLifetime * Random.Range(0.8f, 1.4f);
			float rotation = Random.Range(0f, 360f);
			Color color = (randomColor ? Color.Lerp(Color1, Color2, Random.value) : Color1);
			uint randomSeed = (uint)Random.Range(0, 20000);
			m_emitParams.position = position;
			m_emitParams.rotation = rotation;
			m_emitParams.velocity = velocity;
			m_emitParams.angularVelocity = 0f;
			m_emitParams.startLifetime = num3;
			m_emitParams.startSize = startSize;
			m_emitParams.startColor = color;
			m_emitParams.randomSeed = randomSeed;
			m_particles.Emit(m_emitParams, 1);
		}
		return Time.time + Random.Range(0f - emissionShuffle, emissionShuffle) / emissionRate;
	}
}
