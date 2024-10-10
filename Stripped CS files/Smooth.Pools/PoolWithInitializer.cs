using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class PoolWithInitializer<T, U> : Pool<T>
{
	public readonly DelegateAction<T, U> initialize;

	public PoolWithInitializer(DelegateFunc<T> create, DelegateAction<T> reset, DelegateAction<T, U> initialize)
		: base(create, reset)
	{
		this.initialize = initialize;
	}

	public T Borrow(U u)
	{
		T val = Borrow();
		initialize(val, u);
		return val;
	}

	public Disposable<T> BorrowDisposable(U u)
	{
		Disposable<T> disposable = BorrowDisposable();
		initialize(disposable.value, u);
		return disposable;
	}
}
