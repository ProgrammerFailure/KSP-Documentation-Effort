using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Smooth.Collections;

public class LinkedListStepperNodes<T> : IEnumerable<LinkedListNode<T>>, IEnumerable
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__4 : IEnumerator<LinkedListNode<T>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LinkedListNode<T> _003C_003E2__current;

		public LinkedListStepperNodes<T> _003C_003E4__this;

		private LinkedListNode<T> _003Cnode_003E5__2;

		LinkedListNode<T> IEnumerator<LinkedListNode<T>>.Current
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
		public _003CGetEnumerator_003Ed__4(int _003C_003E1__state)
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

	private readonly LinkedListNode<T> startNode;

	private readonly int step;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LinkedListStepperNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedListStepperNodes(LinkedListNode<T> startNode, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(LinkedListStepperNodes<>._003CGetEnumerator_003Ed__4))]
	public IEnumerator<LinkedListNode<T>> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
