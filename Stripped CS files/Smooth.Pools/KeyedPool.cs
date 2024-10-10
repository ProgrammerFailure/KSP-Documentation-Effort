using System.Collections.Generic;
using Smooth.Delegates;
using Smooth.Dispose;

namespace Smooth.Pools;

public class KeyedPool<T, U>
{
	public readonly Dictionary<T, Stack<U>> keyToValues = new Dictionary<T, Stack<U>>();

	public readonly DelegateFunc<T, U> create;

	public readonly DelegateFunc<U, T> reset;

	public readonly DelegateAction<U> release;

	public KeyedPool()
	{
	}

	public KeyedPool(DelegateFunc<T, U> create, DelegateFunc<U, T> reset)
	{
		this.create = create;
		this.reset = reset;
		release = Release;
	}

	public U Borrow(T key)
	{
		lock (keyToValues)
		{
			Stack<U> value;
			return (!keyToValues.TryGetValue(key, out value) || value.Count <= 0) ? create(key) : value.Pop();
		}
	}

	public void Release(U value)
	{
		T key = reset(value);
		lock (keyToValues)
		{
			if (!keyToValues.TryGetValue(key, out var value2))
			{
				value2 = new Stack<U>();
				keyToValues[key] = value2;
			}
			value2.Push(value);
		}
	}

	public Disposable<U> BorrowDisposable(T key)
	{
		return Disposable<U>.Borrow(Borrow(key), release);
	}
}
