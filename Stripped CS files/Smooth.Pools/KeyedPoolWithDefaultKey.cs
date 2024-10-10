using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class KeyedPoolWithDefaultKey<T, U> : KeyedPool<T, U>
{
	public readonly Either<T, DelegateFunc<T>> defaultKey;

	public KeyedPoolWithDefaultKey(DelegateFunc<T, U> create, DelegateFunc<U, T> reset, T defaultKey)
		: base(create, reset)
	{
		this.defaultKey = Either<T, DelegateFunc<T>>.Left(defaultKey);
	}

	public KeyedPoolWithDefaultKey(DelegateFunc<T, U> create, DelegateFunc<U, T> reset, DelegateFunc<T> defaultKeyFunc)
		: base(create, reset)
	{
		defaultKey = Either<T, DelegateFunc<T>>.Right(defaultKeyFunc);
	}

	public U Borrow()
	{
		return Borrow(defaultKey.isLeft ? defaultKey.leftValue : defaultKey.rightValue());
	}

	public Disposable<U> BorrowDisposable()
	{
		return BorrowDisposable(defaultKey.isLeft ? defaultKey.leftValue : defaultKey.rightValue());
	}
}
