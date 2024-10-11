using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Glow")]
public class DetonatorGlow : DetonatorComponent
{
	private float _baseSize;

	private float _baseDuration;

	private Vector3 _baseVelocity;

	private Color _baseColor;

	private float _scaledDuration;

	private GameObject _glow;

	private DetonatorBurstEmitter _glowEmitter;

	public Material glowMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorGlow()
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
	public void BuildGlow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateGlow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
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
