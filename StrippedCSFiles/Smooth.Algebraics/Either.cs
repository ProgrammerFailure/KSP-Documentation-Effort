using System;
using System.Runtime.CompilerServices;
using Smooth.Delegates;

namespace Smooth.Algebraics;

[Serializable]
public struct Either<L, R> : IComparable<Either<L, R>>, IEquatable<Either<L, R>>
{
	public readonly bool isLeft;

	public readonly L leftValue;

	public readonly R rightValue;

	public bool isRight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Option<L> leftOption
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Option<R> rightOption
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Either(bool isLeft, L leftValue, R rightValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Either<L, R> Left(L value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Either<L, R> Right(R value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public V Cata<V>(DelegateFunc<L, V> leftFunc, DelegateFunc<R, V> rightFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public V Cata<V, P>(DelegateFunc<L, P, V> leftFunc, P p, DelegateFunc<R, V> rightFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public V Cata<V, P2>(DelegateFunc<L, V> leftFunc, DelegateFunc<R, P2, V> rightFunc, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public V Cata<V, P, P2>(DelegateFunc<L, P, V> leftFunc, P p, DelegateFunc<R, P2, V> rightFunc, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach(DelegateAction<L> leftAction, DelegateAction<R> rightAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach<P>(DelegateAction<L, P> leftAction, P p, DelegateAction<R> rightAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach<P2>(DelegateAction<L> leftAction, DelegateAction<R, P2> rightAction, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach<P, P2>(DelegateAction<L, P> leftAction, P p, DelegateAction<R, P2> rightAction, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(Either<L, R> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(Either<L, R> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator >(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator <(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator >=(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator <=(Either<L, R> lhs, Either<L, R> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
