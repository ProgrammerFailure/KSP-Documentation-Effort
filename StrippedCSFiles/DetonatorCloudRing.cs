using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
public class DetonatorCloudRing : DetonatorComponent
{
	private float _baseSize;

	private float _baseDuration;

	private Vector3 _baseVelocity;

	private Color _baseColor;

	private Vector3 _baseForce;

	private GameObject _cloudRing;

	private DetonatorBurstEmitter _cloudRingEmitter;

	public Material cloudRingMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorCloudRing()
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
	public void BuildCloudRing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCloudRing()
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
