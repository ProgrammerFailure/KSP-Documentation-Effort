using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Detonator/Sparks")]
[RequireComponent(typeof(Detonator))]
public class DetonatorSparks : DetonatorComponent
{
	private float _baseSize;

	private float _baseDuration;

	private Vector3 _baseVelocity;

	private Color _baseColor;

	private Vector3 _baseForce;

	private float _scaledDuration;

	private GameObject _sparks;

	private DetonatorBurstEmitter _sparksEmitter;

	public Material sparksMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorSparks()
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
	public void BuildSparks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSparks()
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
