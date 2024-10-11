using System;
using System.Runtime.CompilerServices;
using Smooth.Delegates;
using Smooth.Pools;

namespace Smooth.Dispose;

public class Disposable<T> : IDisposable
{
	private static readonly Pool<Disposable<T>> _pool;

	private DelegateAction<T> _dispose;

	public T value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Disposable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Disposable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Disposable<T> Borrow(T value, DelegateAction<T> dispose)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisposeInBackground()
	{
		throw null;
	}
}
