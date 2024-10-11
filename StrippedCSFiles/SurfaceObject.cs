using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SurfaceObject : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SurfaceObject _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CStart_003Ed__19(int _003C_003E1__state)
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

	public double latitude;

	public double longitude;

	public double altitude;

	public bool GrabCoordsAtStart;

	public bool PopToSceneRootAtStart;

	private CelestialBody cb;

	public KFSMUpdateMode updateMode;

	private Vector3d srfNVector;

	private Transform trf;

	private Transform originalParent;

	public int initDelay;

	private bool started;

	private bool popped;

	private Transform[] children;

	private Vector3[] pristineChildPositions;

	private Quaternion[] pristineChildRotations;

	private Vector3[] pristineChildLocalScales;

	public bool IsPopped
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurfaceObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__19))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PopToSceneRoot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReturnToParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
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
	public static SurfaceObject Create(GameObject host, CelestialBody body, int initDelay, KFSMUpdateMode updateMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SurfaceObject Create(GameObject host, CelestialBody body, double latitude, double longitude, double altitude, int initDelay, KFSMUpdateMode updateMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SurfaceObject Create(GameObject host, CelestialBody body, double latitude, double longitude, double altitude, bool grabCoordsAtStart, bool popToSceneRootAtStart, int initDelay, KFSMUpdateMode updateMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}
}
