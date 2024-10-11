using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("MODEL_PARTICLE")]
public class ModelParticleFX : EffectBehaviour
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

	private Transform modelParent;

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
	public ModelParticleFX()
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
