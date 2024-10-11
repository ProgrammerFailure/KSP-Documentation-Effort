using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
	public enum Modes
	{
		HIGH,
		LOW
	}

	public enum MaxRailsRateMode
	{
		VesselAltitude,
		PeAltitude
	}

	[CompilerGenerated]
	private sealed class _003CautoWarpTo_003Ed__70 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TimeWarp _003C_003E4__this;

		public double tgtUT;

		public int rateIdx;

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
		public _003CautoWarpTo_003Ed__70(int _003C_003E1__state)
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

	public float[] warpRates;

	public float[] altitudeLimits;

	public float[] physicsWarpRates;

	public int maxPhysicsRate_index;

	public int maxModeSwitchRate_index;

	public Modes Mode;

	public float textDuration;

	private float last_rate;

	private float curr_rate;

	public int current_rate_index;

	private ScreenMessage screenText;

	public static TimeWarp fetch;

	private ScreenMessage warpMessage;

	private float LerpStartTime;

	private bool controlsLocked;

	private bool autoWarpEngaged;

	private bool showPhyWarpWarning;

	private static bool saveShowPhysWarp;

	private double shipAltitude;

	private int soiRate;

	private int cRateIndex;

	private double minPeAllowed;

	public static double GThreshold;

	public bool setAutoWarp;

	public double warpToUT;

	public double warpToMaxWarping;

	public double warpToMinWarping;

	private static string cacheAutoLOC_481056;

	private static string cacheAutoLOC_6001931;

	private static string cacheAutoLOC_6001932;

	private static string cacheAutoLOC_6001934;

	private static string cacheAutoLOC_6001936;

	public static Modes WarpMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float tgt_rate
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

	public static float CurrentRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int CurrentRateIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool CurrentRateIsTargetRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float MaxRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float MaxPhysicsRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float deltaTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float fixedDeltaTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TimeWarp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TimeWarp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PostScreenMessage(string text = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnSetHighRate(int rate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnSetLowRate(int rate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowPWarpWarning()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPWarpWarningDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateRate(float r, bool postScreenMessage = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetAltitudeLimit(int i, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRate(int rate_index, bool instant, bool postScreenMessage = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool setRate(int rateIdx, bool instantChange, bool instantIfLower = false, bool forceSwitch = true, bool postScreenMessage = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int getMaxOnRailsRateIdx(int tgtRateIdx, bool lookAhead, out ClearToSaveStatus reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetMaxRateForAltitude(double altitude, CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int ClampRateToOrbitTransitions(int rate, Orbit obt, int maxAllowedSOITransitionRate, int secondsBeforeSOItransition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void assumeWarpRate(float rate, bool instant, bool postScreenMessage = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool setMode(Modes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void WarpTo(double UT, double maxTimeWarping = 8.0, double minTimeWarping = 2.5)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CancelAutoWarp(int rateIdx = -1, bool delay_unlock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CautoWarpTo_003Ed__70))]
	private IEnumerator autoWarpTo(double tgtUT, double maxTimeWarping, double minTimeWarping, int rateIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getMaxWarpRateForTravel(double warpDeltaTime, double minTimeInWarp, double maxTimeInWarp, out int rateIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getWarpAccel(double v0, double v, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getRateChangeTravel(double v0, double v, double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
