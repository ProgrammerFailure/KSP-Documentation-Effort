using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SurfaceFX : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem srfCloud;

	private ParticleSystem.MainModule srfCloudMain;

	private ParticleSystem.Particle[] cloudParticles;

	private int cloudCount;

	[SerializeField]
	private ParticleSystem srfDust;

	private ParticleSystem.MainModule srfDustMain;

	private ParticleSystem.Particle[] dustParticles;

	private int dustCount;

	[SerializeField]
	private ParticleSystem srfWake;

	private ParticleSystem.MainModule srfWakeMain;

	private int wakeCount;

	private float fxScale;

	[SerializeField]
	private float pushThreshold;

	[SerializeField]
	private float pushScale;

	[SerializeField]
	private float linger;

	private List<ModuleSurfaceFX> sources;

	public GameObject prefab;

	private bool atmosphere;

	private static float mergeThreshold;

	private static List<SurfaceFX> fxs;

	private Transform trf;

	private Vector3 Vsrf;

	private Vector3 upAxis;

	private Vector3 barycenter;

	private float lastUpdate;

	public ModuleSurfaceFX leadSource
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float ScaledFX
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SurfaceFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SurfaceFX FindNearestFX(ModuleSurfaceFX src, Vector3 wPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSource(ModuleSurfaceFX src)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveSource(ModuleSurfaceFX src)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateParticle(ref ParticleSystem.Particle p, float lifeTimeThreshold)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetWeightedAvgVector(Func<int, Vector3> getVector, Func<int, float> getWeight)
	{
		throw null;
	}
}
