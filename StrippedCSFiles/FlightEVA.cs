using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightEVA : MonoBehaviour
{
	public delegate KerbalEVA SpawnKerbalEVADelegate(ProtoCrewMember pcm);

	[CompilerGenerated]
	private sealed class _003CSwitchToEVAVesselWhenReady_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public KerbalEVA eva;

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
		public _003CSwitchToEVAVesselWhenReady_003Ed__19(int _003C_003E1__state)
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

	public static FlightEVA fetch;

	private float airlockStandoff;

	public static SpawnKerbalEVADelegate Spawn;

	private ProtoCrewMember pCrew;

	private Part fromPart;

	private Transform fromAirlock;

	public bool overrideEVA;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightEVA()
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
	public static KerbalEVA SpawnEVA(Kerbal crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KerbalEVA _Spawn(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Spawn EVA")]
	public void spawnEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HatchIsObstructed(Part fromPart, Transform fromAirlock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HatchIsObstructedMore(Part fromPart, Transform fromAirlock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool hatchInsideFairing(Part fromPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalEVA spawnEVA(ProtoCrewMember pCrew, Part fromPart, Transform fromAirlock, bool tryAllHatches = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KerbalEVA onGoForEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEVAAborted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSwitchToEVAVesselWhenReady_003Ed__19))]
	private IEnumerator SwitchToEVAVesselWhenReady(KerbalEVA eva)
	{
		throw null;
	}
}
