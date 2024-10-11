using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoroutineHost : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CRunAndDispose_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CoroutineHost _003C_003E4__this;

		public IEnumerator coroutine;

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
		public _003CRunAndDispose_003Ed__3(int _003C_003E1__state)
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

	private bool disposable;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CoroutineHost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CoroutineHost Create(string name, bool persistThroughSceneChanges, bool disposable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new Coroutine StartCoroutine(IEnumerator coroutine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRunAndDispose_003Ed__3))]
	private IEnumerator RunAndDispose(IEnumerator coroutine)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}
}
