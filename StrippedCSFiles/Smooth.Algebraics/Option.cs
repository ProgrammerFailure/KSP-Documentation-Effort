using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Delegates;

namespace Smooth.Algebraics;

public static class Option
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> Create<T>(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> Some<T>(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> None<T>(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> ToOption<T>(this T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> ToSome<T>(this T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> ToNone<T>(this T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Option<T> Flatten<T>(this Option<Option<T>> option)
	{
		throw null;
	}
}
[Serializable]
public struct Option<T> : IComparable<Option<T>>, IEquatable<Option<T>>
{
	public static readonly Option<T> None;

	public readonly bool isSome;

	public readonly T value;

	public bool isNone
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option(T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Option()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Cata<U>(DelegateFunc<T, U> someFunc, U noneValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Cata<U, P>(DelegateFunc<T, P, U> someFunc, P p, U noneValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Cata<U>(DelegateFunc<T, U> someFunc, DelegateFunc<U> noneFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Cata<U, P>(DelegateFunc<T, P, U> someFunc, P p, DelegateFunc<U> noneFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public U Cata<U, P, P2>(DelegateFunc<T, P, U> someFunc, P p, DelegateFunc<P2, U> noneFunc, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(T t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(T t, IEqualityComparer<T> comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IfEmpty(DelegateAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IfEmpty<P>(DelegateAction<P> action, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach(DelegateAction<T> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEach<P>(DelegateAction<T, P> action, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEachOr(DelegateAction<T> someAction, DelegateAction noneAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEachOr<P>(DelegateAction<T, P> someAction, P p, DelegateAction noneAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEachOr<P2>(DelegateAction<T> someAction, DelegateAction<P2> noneAction, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForEachOr<P, P2>(DelegateAction<T, P> someAction, P p, DelegateAction<P2> noneAction, P2 p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Or(Option<T> noneOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Or(DelegateFunc<Option<T>> noneFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Or<P>(DelegateFunc<P, Option<T>> noneFunc, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<U> Select<U>(DelegateFunc<T, U> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<U> Select<U, P>(DelegateFunc<T, P, U> selector, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<U> SelectMany<U>(DelegateFunc<T, Option<U>> selector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<U> SelectMany<U, P>(DelegateFunc<T, P, Option<U>> selector, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T ValueOr(T noneValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T ValueOr(DelegateFunc<T> noneFunc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T ValueOr<P>(DelegateFunc<P, T> noneFunc, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Where(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> Where<P>(DelegateFunc<T, P, bool> predicate, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> WhereNot(DelegateFunc<T, bool> predicate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Option<T> WhereNot<P>(DelegateFunc<T, P, bool> predicate, P p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Equals(Option<T> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(Option<T> other)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator >(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator <(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator >=(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool operator <=(Option<T> lhs, Option<T> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
