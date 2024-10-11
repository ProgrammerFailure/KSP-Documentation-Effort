using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Fireball")]
public class DetonatorFireball : DetonatorComponent
{
	private float _baseSize;

	private float _baseDuration;

	private Color _baseColor;

	private float _scaledDuration;

	private GameObject _fireballA;

	private DetonatorBurstEmitter _fireballAEmitter;

	public Material fireballAMaterial;

	private GameObject _fireballB;

	private DetonatorBurstEmitter _fireballBEmitter;

	public Material fireballBMaterial;

	private GameObject _fireShadow;

	private DetonatorBurstEmitter _fireShadowEmitter;

	public Material fireShadowMaterial;

	public bool drawFireballA;

	public bool drawFireballB;

	public bool drawFireShadow;

	private Color _detailAdjustedColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorFireball()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FillMaterials(bool wipe)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildFireballA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFireballA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildFireballB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFireballB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildFireShadow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFireShadow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Explode()
	{
		throw null;
	}
}
