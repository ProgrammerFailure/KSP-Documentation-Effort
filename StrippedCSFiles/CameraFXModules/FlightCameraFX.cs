using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CameraFXModules;

public class FlightCameraFX : MonoBehaviour
{
	[Serializable]
	public class EngineVibrations
	{
		public float Generic;

		public float SolidBooster;

		public float LiquidFuel;

		public float Piston;

		public float Turbine;

		public float ScramJet;

		public float Electric;

		public float Nuclear;

		public float MonoProp;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public EngineVibrations()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetVibration(EngineType t)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FlightCameraFX _003C_003E4__this;

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
		public _003CStart_003Ed__12(int _003C_003E1__state)
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

	private CameraFX host;

	private Wobble GForce;

	private float gForceScalar;

	private Wobble GroundRoll;

	private float groundRollScalar;

	private Wobble AtmoFlight;

	private float atmoFlightScalar;

	private Wobble EngineVibration;

	private List<IThrustProvider> vesselThrusters;

	private IThrustProvider iThr;

	private float totalThrust;

	private float thrustScalar;

	public WobbleFXParams fxGforce;

	public WobbleFXParams fxGroundRoll;

	public WobbleFXParams fxEngineVibration;

	public WobbleFXParams fxAtmoFlight;

	public FadeWobbleFXParams fxCollision;

	public FadeWobbleFXParams fxSplashdown;

	public FadeWobbleFXParams fxExplosion;

	public FadeWobbleFXParams fxOverHeat;

	public FadeWobbleFXParams fxJointBreak;

	public FadeWobbleFXParams fxSeparate;

	public MinMaxFloat rangeGroundRollSpeed;

	public MinMaxFloat rangeGForceMagnitude;

	public MinMaxFloat rangeExplosionDistance;

	public MinMaxFloat rangeSplashdownSpeed;

	public MinMaxFloat rangeCollisionRspd;

	public MinMaxFloat rangeEngineVibrationThrust;

	public MinMaxFloat rangeAtmosFlightIAS;

	public EngineVibrations engineVibrations;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightCameraFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__12))]
	private IEnumerator Start()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselEvent(EventReport evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vSt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExplosion(GameEvents.ExplosionReaction exp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselUpdate(Vessel v)
	{
		throw null;
	}
}
