using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Smoke")]
public class DetonatorSmoke : DetonatorComponent
{
	private const float _baseSize = 1f;

	private const float _baseDuration = 8f;

	private Color _baseColor;

	private const float _baseDamping = 0.1300004f;

	private float _scaledDuration;

	private GameObject _smokeA;

	private DetonatorBurstEmitter _smokeAEmitter;

	public Material smokeAMaterial;

	private GameObject _smokeB;

	private DetonatorBurstEmitter _smokeBEmitter;

	public Material smokeBMaterial;

	public bool drawSmokeA;

	public bool drawSmokeB;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorSmoke()
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
	public void BuildSmokeA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSmokeA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildSmokeB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSmokeB()
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
