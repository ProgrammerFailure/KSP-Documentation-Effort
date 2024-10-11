using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class Pool<T>
{
	private readonly Stack<T> values;

	private readonly DelegateFunc<T> create;

	private readonly DelegateAction<T> reset;

	private readonly DelegateAction<T> release;

	private PoolsStatus status;

	public int Size
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Allocated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Pool()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Pool(DelegateFunc<T> create, DelegateAction<T> reset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Borrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Release(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<T> BorrowDisposable()
	{
		throw null;
	}
}
