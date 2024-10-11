using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
	private static List<KSPParticleEmitter> kspEmitters;

	private static List<ParticleSystem> emitters;

	public Part hostPart;

	public string effectName;

	public string instanceName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EffectBehaviour()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EffectBehaviour()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddParticleEmitter(KSPParticleEmitter emitter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveParticleEmitter(KSPParticleEmitter emitter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddParticleEmitter(ParticleSystem emitter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveParticleEmitter(ParticleSystem emitter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OffsetParticles(Vector3d offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEvent(int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEvent(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEvent(float power, int transformIdx)
	{
		throw null;
	}
}
