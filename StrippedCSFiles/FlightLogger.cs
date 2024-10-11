using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightLogger : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CwaitAndEndFlight_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FlightLogger _003C_003E4__this;

		public Vessel v;

		public FlightEndModes endMode;

		private float _003Ct_003E5__2;

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
		public _003CwaitAndEndFlight_003Ed__56(int _003C_003E1__state)
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

	private double highestAltitude;

	private double groundDistance;

	private double totalDistance;

	private double highestGee;

	private double instantGee;

	private double sustainedGee;

	private double highestSpeed;

	private double highestSpeedOverLand;

	public double missionTime;

	private bool liftOff;

	private bool missionEnd;

	private int partsLost;

	private int kerbalsKilled;

	private FlightEndModes flightEndMode;

	private Dictionary<FlightEndModes, string> endMessages;

	public static List<string> eventLog;

	public static FlightLogger fetch;

	private bool okToSetSpeed;

	public static bool LogGees;

	private static string lastReport;

	public float endFlightWait;

	public float attemptSwitchWait;

	public static bool LiftOff
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double met
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private string missionTimestamp
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private string missionTimestampWithDay
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightLogger()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightLogger()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void IgnoreGeeForces(float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResumeGForceReading()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string getMissionStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onJointBreak(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrash(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrashSplashdown(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCollision(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOverheat(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOverPressure(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOverG(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onStageSeparation(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrewOnEva(GameEvents.FromToAction<Part, Part> fv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrewKilled(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrewBoardVessel(GameEvents.FromToAction<Part, Part> fp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLaunch(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUndock(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSplashDamage(EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LogEvent(string message, EventReport report)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LogEvent(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool checkMissionEnd(Part p, FlightEndModes endMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AttemptVesselSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndEndFlight_003Ed__56))]
	public IEnumerator waitAndEndFlight(Vessel v, FlightEndModes endMode)
	{
		throw null;
	}
}
