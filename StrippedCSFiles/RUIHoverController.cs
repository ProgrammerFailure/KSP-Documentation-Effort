using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RUIHoverController
{
	private struct CallbackID
	{
		public int id;

		public Callback callback;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CallbackID(int id, Callback callback)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CUpdateDaemon_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RUIHoverController _003C_003E4__this;

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
		public _003CUpdateDaemon_003Ed__13(int _003C_003E1__state)
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

	private bool refreshRequested1;

	private bool refreshRequested2;

	public float refreshTimeToWait;

	private bool updateDaemonRunning;

	private MonoBehaviour host;

	private Queue<CallbackID> schedule;

	private HashSet<int> scheduleIds;

	private Dictionary<int, Callback> scheduleOut;

	private int waitForHowerOutId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RUIHoverController(MonoBehaviour host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Schedule(int id, Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deschedule(int id, Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateDaemon_003Ed__13))]
	private IEnumerator UpdateDaemon()
	{
		throw null;
	}
}
