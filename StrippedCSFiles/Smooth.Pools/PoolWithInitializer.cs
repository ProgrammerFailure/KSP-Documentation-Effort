using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class PoolWithInitializer<T, U> : Pool<T>
{
	private readonly DelegateAction<T, U> initialize;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PoolWithInitializer(DelegateFunc<T> create, DelegateAction<T> reset, DelegateAction<T, U> initialize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Borrow(U u)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Disposable<T> BorrowDisposable(U u)
	{
		throw null;
	}
}
