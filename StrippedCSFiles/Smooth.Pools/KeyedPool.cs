using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class KeyedPool<K, T>
{
	private readonly Dictionary<K, Stack<T>> keyToValues;

	private readonly DelegateFunc<K, T> create;

	private readonly DelegateFunc<T, K> reset;

	private readonly DelegateAction<T> release;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private KeyedPool()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyedPool(DelegateFunc<K, T> create, DelegateFunc<T, K> reset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Borrow(K key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Release(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<T> BorrowDisposable(K key)
	{
		throw null;
	}
}
