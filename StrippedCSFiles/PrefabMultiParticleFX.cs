using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("PREFAB_MULTI_PARTICLE")]
public class PrefabMultiParticleFX : EffectBehaviour
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

	private List<Transform> modelParents;

	private List<ParticleSystem> emitters;

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
	public PrefabMultiParticleFX()
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
	public override void OnEvent(int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent(float power, int transformIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEmitter(int transformIdx, int minEmissionVal, int maxEmissionVal, float minEnergyVal, float maxEnergyVal, Vector3 localVelocityVal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
