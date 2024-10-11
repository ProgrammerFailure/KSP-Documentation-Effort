using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class ReachFlightEnvelope : ContractParameter
{
	[CompilerGenerated]
	private sealed class _003CTrackVessel_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Vessel v;

		public ReachFlightEnvelope _003C_003E4__this;

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
		public _003CTrackVessel_003Ed__23(int _003C_003E1__state)
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

	public CelestialBody Destination;

	public Vessel.Situations Situation;

	public string BiomeName;

	public bool useAltLimits;

	public bool useSpdLimits;

	public double maxAltitude;

	public double minAltitude;

	public double maxSpeed;

	public double minSpeed;

	protected string title;

	protected Vessel tgtVessel;

	private bool trackerActive;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachFlightEnvelope()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachFlightEnvelope(CelestialBody dest, Vessel.Situations sit, string biomeName, float maxAlt, float minAlt, float maxSpd, float minSpd, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSituationStringShort(Vessel.Situations sit, CelestialBody body, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlightReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CTrackVessel_003Ed__23))]
	private IEnumerator TrackVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool checkVesselWithinBiome(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkVesselWithinFlightEnvelope(Vessel v)
	{
		throw null;
	}
}
