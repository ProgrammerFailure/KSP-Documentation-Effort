using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CometVessel : VesselModule
{
	[CompilerGenerated]
	private sealed class _003CWaitAndUpdateVFXDimensions_003Ed__57 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CometVessel _003C_003E4__this;

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
		public _003CWaitAndUpdateVFXDimensions_003Ed__57(int _003C_003E1__state)
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

	[KSPField(isPersistant = true)]
	public string typeName;

	[KSPField(isPersistant = true)]
	public uint cometPartId;

	private CometOrbitType cometType;

	[KSPField(isPersistant = true)]
	public double vfxStartDistance;

	[KSPField(isPersistant = true)]
	public double comaWidthRatio;

	[KSPField(isPersistant = true)]
	public double tailMaxWidthRatio;

	[KSPField(isPersistant = true)]
	public double tailMaxLengthRatio;

	[KSPField(isPersistant = true)]
	public double ionTailMaxWidthRatio;

	[KSPField(isPersistant = true)]
	public double comaWidth;

	[KSPField(isPersistant = true)]
	public double tailWidth;

	[KSPField(isPersistant = true)]
	public double tailLength;

	[KSPField(isPersistant = true)]
	public double ionTailWidth;

	[KSPField(isPersistant = true)]
	public bool hasName;

	[KSPField(isPersistant = true)]
	internal float radius;

	[KSPField(isPersistant = true)]
	internal int seed;

	[KSPField(isPersistant = true)]
	public int numGeysers;

	[KSPField(isPersistant = true)]
	public int numNearDustEmitters;

	[KSPField(isPersistant = true)]
	public bool optimizedCollider;

	[KSPField(isPersistant = true)]
	public float fragmentDynamicPressureModifier;

	[SerializeField]
	private double homeBodySMA;

	private bool isCometTypeReady;

	internal CometVFXController cometVFX;

	private WaitForEndOfFrame startWait;

	[SerializeField]
	private double lastUpdateUT;

	[SerializeField]
	internal double currentDistanceToSun;

	[SerializeField]
	internal float distanceRatio;

	[SerializeField]
	internal float vfxRatio;

	[SerializeField]
	internal bool vfxRatioSet;

	[SerializeField]
	internal float nearDustVFXRatio;

	[SerializeField]
	internal float geyserVFXRatio;

	[SerializeField]
	internal float atmosphereVFXRatio;

	[SerializeField]
	internal bool overrideVFXRatio;

	public CometOrbitType CometType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double ComaRadius
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ComaWidthScaledSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ComaRadiusScaledSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float TailWidthScaledSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float TailLengthScaledSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float IonTailWidthScaledSpace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSituationRanges(VesselRanges.Situation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartPersistentIdChanged(uint vesselId, uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDecouple(Vessel oldVessel, Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MoveCometVesselModule(uint newId, Part cometPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKnowledgeChange(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVFXDimensions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndUpdateVFXDimensions_003Ed__57))]
	internal IEnumerator WaitAndUpdateVFXDimensions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetCollider(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetGeyser(bool enabled, float emissionRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDust(bool enabled, float emissionRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetComa(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetIonTail(bool enabled, float emissionRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDustTail(bool enabled, float emissionRate, float size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ReprimeParticles()
	{
		throw null;
	}
}
