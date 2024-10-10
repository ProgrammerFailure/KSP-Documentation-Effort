using System;
using Smooth.Delegates;
using Smooth.Pools;

namespace Smooth.Dispose;

public class Disposable<T> : IDisposable
{
	public static readonly Pool<Disposable<T>> _pool = new Pool<Disposable<T>>(() => new Disposable<T>(), delegate(Disposable<T> wrapper)
	{
		wrapper._dispose(wrapper.value);
		wrapper._dispose = delegate
		{
		};
		wrapper.value = default(T);
	});

	public DelegateAction<T> _dispose;

	public T value { get; set; }

	public static Disposable<T> Borrow(T value, DelegateAction<T> dispose)
	{
		Disposable<T> disposable = _pool.Borrow();
		disposable.value = value;
		disposable._dispose = dispose;
		return disposable;
	}

	public void Dispose()
	{
		_pool.Release(this);
	}

	public void DisposeInBackground()
	{
		DisposalQueue.Enqueue(this);
	}
}
