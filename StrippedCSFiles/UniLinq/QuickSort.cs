using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UniLinq;

internal class QuickSort<TElement>
{
	[CompilerGenerated]
	private sealed class _003CSort_003Ed__6 : IEnumerable<TElement>, IEnumerable, IEnumerator<TElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<TElement> source;

		public IEnumerable<TElement> _003C_003E3__source;

		private SortContext<TElement> context;

		public SortContext<TElement> _003C_003E3__context;

		private QuickSort<TElement> _003Csorter_003E5__2;

		private int _003Ci_003E5__3;

		TElement IEnumerator<TElement>.Current
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
		public _003CSort_003Ed__6(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	private TElement[] elements;

	private int[] indexes;

	private SortContext<TElement> context;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private QuickSort(IEnumerable<TElement> source, SortContext<TElement> context)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int[] CreateIndexes(int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PerformSort()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(QuickSort<>._003CSort_003Ed__6))]
	public static IEnumerable<TElement> Sort(IEnumerable<TElement> source, SortContext<TElement> context)
	{
		throw null;
	}
}
