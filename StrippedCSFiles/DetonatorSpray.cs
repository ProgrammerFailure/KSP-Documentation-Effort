using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Detonator/Object Spray")]
[RequireComponent(typeof(Detonator))]
public class DetonatorSpray : DetonatorComponent
{
	public GameObject sprayObject;

	public int count;

	public float startingRadius;

	public float minScale;

	public float maxScale;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	private Vector3 _explosionPosition;

	private float _tmpScale;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DetonatorSpray()
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
