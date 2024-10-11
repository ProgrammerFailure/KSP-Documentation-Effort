using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OrbitPhysicsManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public OrbitPhysicsManager _003C_003E4__this;

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
		public _003CStart_003Ed__15(int _003C_003E1__state)
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

	private static OrbitPhysicsManager fetch;

	public CelestialBody dominantBody;

	[Obsolete("Use Vessel.distancePackThreshold instead")]
	public float distantPartPackThreshold;

	[Obsolete("Use Vessel.distanceUnpackThreshold instead")]
	public float distantPartUnpackThreshold;

	[Obsolete("Use Vessel.distanceLandedPackThreshold instead")]
	public float distantLandedPartPackThreshold;

	[Obsolete("Use Vessel.distanceLandedUnpackThreshold instead")]
	public float distantLandedPartUnpackThreshold;

	public bool toggleDominantBodyRotation;

	public bool degub;

	public bool holdVesselUnpack;

	private int releaseUnpackIn;

	private bool started;

	public static CelestialBody DominantBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitPhysicsManager()
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
	[IteratorStateMachine(typeof(_003CStart_003Ed__15))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void HoldVesselUnpack(int releaseAfter = 1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setDominantBody(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetDominantBody(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetReverseOrbit(OrbitDriver orbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setRotatingFrame(bool rotatingFrameState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Toggle Rotating Frame (Force Debug)")]
	public void ToggleRotatingFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetRotatingFrame(bool rotating)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void checkReferenceFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CheckReferenceFrame()
	{
		throw null;
	}
}
