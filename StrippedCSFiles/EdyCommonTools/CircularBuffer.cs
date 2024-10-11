using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public class CircularBuffer<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__35 : IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		public CircularBuffer<T> _003C_003E4__this;

		private int _003CbufferIndex_003E5__2;

		private int _003Ci_003E5__3;

		T IEnumerator<T>.Current
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
		public _003CGetEnumerator_003Ed__35(int _003C_003E1__state)
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

	private int m_capacity;

	private int m_size;

	private int m_head;

	private int m_tail;

	private T[] m_buffer;

	[NonSerialized]
	private object syncRoot;

	public bool allowOverflow
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public int capacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int size
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	int ICollection<T>.Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	bool ICollection<T>.IsReadOnly
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	int ICollection.Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int headIdx
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int tailIdx
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CircularBuffer(int m_capacity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CircularBuffer(int capacity, bool allowOverflow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int IndexOf(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAt(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Put(T[] src)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Put(T[] src, int srcOffset, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Put(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Skip(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] Get(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Get(T[] dst)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Get(T[] dst, int dstOffset, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Get()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Peek(int offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Head()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Tail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(T[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(T[] array, int arrayIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(CircularBuffer<>._003CGetEnumerator_003Ed__35))]
	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] GetBuffer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T[] ToArray()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void ICollection<T>.Add(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	bool ICollection<T>.Remove(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
