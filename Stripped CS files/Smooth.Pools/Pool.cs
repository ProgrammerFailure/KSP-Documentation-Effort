using System;
using System.Collections.Generic;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class Pool<T>
{
	public readonly Stack<T> values = new Stack<T>();

	public readonly DelegateFunc<T> create;

	public readonly DelegateAction<T> reset;

	public readonly DelegateAction<T> release;

	public PoolsStatus status = new PoolsStatus();

	public int Size => status.maxSize;

	public int Allocated => status.allocated;

	public Pool()
	{
	}

	public Pool(DelegateFunc<T> create, DelegateAction<T> reset)
	{
		this.create = create;
		this.reset = reset;
		release = Release;
		PoolsStatus.poolsInfo.Add(typeof(T), status);
	}

	public T Borrow()
	{
		lock (values)
		{
			if (values.Count > 0)
			{
				return values.Pop();
			}
			status.allocated++;
			return create();
		}
	}

	public void Release(T value)
	{
		reset(value);
		lock (values)
		{
			values.Push(value);
			status.maxSize = Math.Max(status.maxSize, values.Count);
		}
	}

	public Disposable<T> BorrowDisposable()
	{
		return Disposable<T>.Borrow(Borrow(), release);
	}
}
