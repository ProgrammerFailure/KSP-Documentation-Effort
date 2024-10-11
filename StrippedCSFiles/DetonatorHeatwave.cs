using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Heatwave (Pro Only)")]
public class DetonatorHeatwave : DetonatorComponent
{
	private GameObject _heatwave;

	private float s;

	private float _startSize;

	private float _maxSize;

	private float _baseDuration;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public float zOffset;

	public float distortion;

	private float _elapsedTime;

	private float _normalizedTime;

	public Material heatwaveMaterial;

	private Material _material;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorHeatwave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Explode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}
}
