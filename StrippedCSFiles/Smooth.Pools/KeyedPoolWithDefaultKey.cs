using System.Runtime.CompilerServices;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class KeyedPoolWithDefaultKey<K, T> : KeyedPool<K, T>
{
	private readonly Either<K, DelegateFunc<K>> defaultKey;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyedPoolWithDefaultKey(DelegateFunc<K, T> create, DelegateFunc<T, K> reset, K defaultKey)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyedPoolWithDefaultKey(DelegateFunc<K, T> create, DelegateFunc<T, K> reset, DelegateFunc<K> defaultKeyFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Borrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<T> BorrowDisposable()
	{
		throw null;
	}
}
