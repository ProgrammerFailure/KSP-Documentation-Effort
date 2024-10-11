using System.Runtime.CompilerServices;
using UnityEngine;

public class CometVFXController : MonoBehaviour
{
	[SerializeField]
	private Transform dustTailFarEffect;

	[SerializeField]
	private ParticleSystem dustTailFarParticleSystem;

	[SerializeField]
	private Transform ionTailFarEffect;

	[SerializeField]
	private ParticleSystem ionTailFarParticleSystem;

	[SerializeField]
	internal SphereVolume comaEffect;

	[SerializeField]
	private float comaConstrastAtZero;

	[SerializeField]
	private float comaConstrastAtOne;

	[SerializeField]
	private float comaTextureScaleAtZero;

	[SerializeField]
	private float comaTextureScaleAtOne;

	[SerializeField]
	private float comaVisibilityAtZero;

	[SerializeField]
	private float comaVisibilityAtOne;

	internal CometParticleController ionController;

	internal CometParticleController dustController;

	private bool vfxInitialized;

	public ScaledMovement hostSM;

	public Vessel hostVessel;

	public CometVessel cometVessel;

	public float[] comaTimeWarpMovement;

	[SerializeField]
	private Part cometPart;

	[SerializeField]
	private Vector3d directionFromSun;

	[SerializeField]
	private bool vesselDead;

	private Vector3 comaMovement;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometVFXController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeSpawnVFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnVFXObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DefineDustTailParticleFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DefineIONTailParticleFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DefineComaFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselWasLoaded(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCometVesselChanged(Vessel oldVessel, Vessel newVessel, CometVessel oldCometVessel, CometVessel newCometVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartWillDie(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateComaFX()
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
	private void LateUpdate()
	{
		throw null;
	}
}
