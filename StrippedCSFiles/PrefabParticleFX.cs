using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("PREFAB_PARTICLE")]
public class PrefabParticleFX : EffectBehaviour
{
	[Persistent]
	public string prefabName;

	[Persistent]
	public string transformName;

	public FXCurve emission;

	public FXCurve energy;

	public FXCurve speed;

	[Persistent]
	public Vector3 localOffset;

	[Persistent]
	public Vector4 localRotation;

	[Persistent]
	public Vector3 localScale;

	[Persistent]
	public bool oneShot;

	private Transform modelParent;

	private ParticleSystem emitter;

	private float minEmission;

	private float maxEmission;

	private float minEnergy;

	private float maxEnergy;

	private Vector3 velocity;

	private float emissionPower;

	private float energyPower;

	private float speedPower;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PrefabParticleFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
