using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Shockwave")]
public class DetonatorShockwave : DetonatorComponent
{
	private float _baseSize;

	private float _baseDuration;

	private Vector3 _baseVelocity;

	private Color _baseColor;

	private GameObject _shockwave;

	private DetonatorBurstEmitter _shockwaveEmitter;

	public Material shockwaveMaterial;

	public ParticleSystemRenderMode renderModeNewSystem;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorShockwave()
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
	public void BuildShockwave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateShockwave()
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
