using System.Runtime.CompilerServices;
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

	public float emissionRate;

	[Range(0f, 1f)]
	public float emissionShuffle;

	public float maxLifetime;

	public float minVelocity;

	public float maxVelocity;

	[Range(0f, 1f)]
	public float wheelVelocityRatio;

	[Range(0f, 1f)]
	public float tireVelocityRatio;

	public Color Color1;

	public Color Color2;

	public bool randomColor;

	private ParticleSystem m_particles;

	private ParticleSystem.EmitParams m_emitParams;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPGroundParticleEmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float EmitParticle(Vector3 position, Vector3 wheelVelocity, Vector3 tireVelocity, float pressureRatio, float intensityRatio, float lastParticleTime)
	{
		throw null;
	}
}
