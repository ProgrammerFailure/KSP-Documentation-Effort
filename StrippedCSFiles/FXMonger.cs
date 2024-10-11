using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.FX.Fireworks;
using UnityEngine;

public class FXMonger : MonoBehaviour
{
	private class ProtoExplosion
	{
		public Vector3d position;

		public double power;

		public List<Part> sources;

		public float timeOfExplosion;

		public float timeOfFX;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProtoExplosion(Part source, Vector3d pos, double powah)
		{
			throw null;
		}
	}

	private class ROCProtoExplosion
	{
		public Vector3d position;

		public double power;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ROCProtoExplosion(Vector3 pos, double powah)
		{
			throw null;
		}
	}

	private class DebrisProtoExplosion
	{
		public Vector3d position;

		public double power;

		public GameObject objectToFollow;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DebrisProtoExplosion(Vector3 pos, double powah, GameObject otf)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CexplosionTest_003Ed__41 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FXMonger _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CexplosionTest_003Ed__41(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private List<GameObject> debrisExplosions;

	private List<GameObject> debrisExplosionsObjectsToFollow;

	public GameObject[] explosions;

	public AudioClip[] explosionSounds;

	public GameObject[] thuds;

	public AudioClip[] crashSounds;

	public GameObject[] splashes;

	public AudioClip[] splashSounds;

	public GameObject[] debrisExplosion;

	public AudioClip[] debrisSounds;

	public float minSqrBlast;

	public float minSqrSplash;

	public float minDistanceFromOtherBlasts;

	public float minDistanceFromOtherSplashes;

	public float minTimeBetweenSplashes;

	public float queueRemovalTimeMultiplier;

	public double minPower;

	private List<ProtoExplosion> queuedExplosions;

	private List<ROCProtoExplosion> queuedROCExplosions;

	private List<DebrisProtoExplosion> queuedDebrisExplosions;

	protected List<FXObject> explosionObjects;

	private static FXMonger fetch;

	public bool splashMonger;

	private List<ProtoExplosion> queuedSplashes;

	private static ParticleSystem.Particle[] sParts;

	private List<FireworkFXComponent> fireworks;

	private static ParticleSystem.Particle[] fireworkPSs;

	[Obsolete("No longer used - please use minDistanceFromOtherBlasts")]
	public float MINIMUM_DISTANCE_FROM_OTHER_BLASTS
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXMonger()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FXMonger()
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
	public static void Explode(Part source, Vector3d blastPos, double howhard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void explode(Part source, Vector3d blastPos, double howHard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ROCExplode(Vector3d blastPos, double howhard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void rocexplode(Vector3d blastPos, double howHard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ExplodeWithDebris(Vector3d blastPos, double howHard, GameObject objectToFollow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnDebrisExplosions(Vector3d blastPos, double howHard, GameObject objectToFollow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FXObject Splash(Vector3 pos, float howHard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FXObject splash(Vector3 pos, float howHard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void splashMongered(Vector3 pos, float howHard)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CexplosionTest_003Ed__41))]
	private IEnumerator explosionTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void offsetPositions(Vector3d offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveFXOjbect(FXObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OffsetPositions(Vector3d offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void AddFireworkFX(FireworkFXComponent fx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void RemoveFireworkFX(FireworkFXComponent fx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void offsetFireworks(Vector3d offset)
	{
		throw null;
	}
}
