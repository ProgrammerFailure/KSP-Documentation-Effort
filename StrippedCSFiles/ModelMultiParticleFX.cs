using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("MODEL_MULTI_PARTICLE")]
public class ModelMultiParticleFX : EffectBehaviour
{
	[Persistent]
	public string modelName;

	[Persistent]
	public string transformName;

	[Persistent]
	public Vector3 localRotation;

	[Persistent]
	public Vector3 localPosition;

	[Persistent]
	public Vector3 localScale;

	public FXCurve emission;

	public FXCurve energy;

	public FXCurve speed;

	private List<Transform> modelParents;

	private List<KSPParticleEmitter> emitters;

	private KSPParticleEmitter emitter;

	private float minEmission;

	private float maxEmission;

	private float minEnergy;

	private float maxEnergy;

	private Vector3 localVelocity;

	private float emissionPower;

	private float energyPower;

	private float speedPower;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModelMultiParticleFX()
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
