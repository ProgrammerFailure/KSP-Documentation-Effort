using System.Runtime.CompilerServices;
using UnityEngine;

public class LaunchPadFX : MonoBehaviour
{
	[SerializeField]
	protected ParticleSystem[] ps;

	[SerializeField]
	protected Material smokeParticleMaterial;

	[Range(0f, 1f)]
	[SerializeField]
	protected float fxScale;

	[SerializeField]
	private float maxFX;

	private float totalFX;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LaunchPadFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFX(float fx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
