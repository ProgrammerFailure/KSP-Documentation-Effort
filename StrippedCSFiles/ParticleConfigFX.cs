using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[EffectDefinition("PARTICLE_CONFIG")]
public class ParticleConfigFX : EffectBehaviour
{
	[Serializable]
	public class PFXMaterial
	{
		public enum MaterialType
		{
			Additive,
			AlphaBlended
		}

		[Persistent]
		public MaterialType type;

		[Persistent]
		public Color color;

		[Persistent]
		public string texture;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PFXMaterial()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Material CreateMaterial()
		{
			throw null;
		}
	}

	public GameObject pHost;

	public ParticleSystem ps;

	[Persistent]
	public float power;

	[Persistent]
	public bool useWorldSpace;

	[Persistent]
	public Vector3 ellipsoid;

	public FXCurve minSize;

	public FXCurve maxSize;

	public FXCurve minEnergy;

	public FXCurve maxEnergy;

	public FXCurve minEmission;

	public FXCurve maxEmission;

	[Persistent]
	public Vector3 worldVelocity;

	[Persistent]
	public Vector3 localVelocity;

	[Persistent]
	public Vector3 rndVelocity;

	[Persistent]
	public float emitterVelocityScale;

	[Persistent]
	public Vector3 tangentVelocity;

	[Persistent]
	public float angularVelocity;

	[Persistent]
	public float rndAngularVelocity;

	[Persistent]
	public bool rndRotation;

	[Persistent]
	public bool oneShot;

	[Persistent]
	public bool doesAnimateColor;

	[Persistent]
	public Color[] colorAnimation;

	private static float dC;

	[Persistent]
	public Vector3 worldRotationAxis;

	[Persistent]
	public Vector3 localRotationAxis;

	public FXCurve sizeGrow;

	[Persistent]
	public Vector3 rndForce;

	[Persistent]
	public Vector3 force;

	[Persistent]
	public float damping;

	[Persistent]
	public bool autodestruct;

	[Persistent]
	public bool castShadows;

	[Persistent]
	public bool recieveShadows;

	public FXCurve lengthScale;

	[Persistent]
	public float velocityScale;

	[Persistent]
	public float maxParticleSize;

	[Persistent]
	public ParticleSystemRenderMode particleRenderModeNewSystem;

	[Persistent]
	public int uvAnimationXTile;

	[Persistent]
	public int uvAnimationYTile;

	[Persistent]
	public int uvAnimationCycles;

	[Persistent]
	public PFXMaterial material;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ParticleConfigFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ParticleConfigFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeComponents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeParticleSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateComponents()
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
	public override void OnEvent(float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEvent()
	{
		throw null;
	}
}
