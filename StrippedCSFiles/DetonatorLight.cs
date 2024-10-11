using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Detonator/Light")]
[RequireComponent(typeof(Detonator))]
public class DetonatorLight : DetonatorComponent
{
	private float _baseIntensity;

	private Color _baseColor;

	private float _scaledDuration;

	private float _explodeTime;

	private GameObject _light;

	private Light _lightComponent;

	public float intensity;

	private float _reduceAmount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorLight()
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
