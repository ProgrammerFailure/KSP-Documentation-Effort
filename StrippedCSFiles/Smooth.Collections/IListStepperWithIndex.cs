using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Smooth.Algebraics;

namespace Smooth.Collections;

public class IListStepperWithIndex<T> : IEnumerable<Smooth.Algebraics.Tuple<T, int>>, IEnumerable
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__5 : IEnumerator<Smooth.Algebraics.Tuple<T, int>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Smooth.Algebraics.Tuple<T, int> _003C_003E2__current;

		public IListStepperWithIndex<T> _003C_003E4__this;

		private int _003Ci_003E5__2;

		Smooth.Algebraics.Tuple<T, int> IEnumerator<Smooth.Algebraics.Tuple<T, int>>.Current
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
		public _003CGetEnumerator_003Ed__5(int _003C_003E1__state)
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

	private readonly IList<T> list;

	private readonly int startIndex;

	private readonly int step;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IListStepperWithIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IListStepperWithIndex(IList<T> list, int startIndex, int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(IListStepperWithIndex<>._003CGetEnumerator_003Ed__5))]
	public IEnumerator<Smooth.Algebraics.Tuple<T, int>> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
