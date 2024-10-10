using System;
using System.Collections.Generic;
using Smooth.Collections;
using Smooth.Delegates;

namespace Smooth.Algebraics;

public static class Option
{
	public static Option<T> Create<T>(T value)
	{
		if (value != null)
		{
			return new Option<T>(value);
		}
		return Option<T>.None;
	}

	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	public static Option<T> None<T>(T value)
	{
		return Option<T>.None;
	}

	public static Option<T> ToOption<T>(this T value)
	{
		if (value != null)
		{
			return new Option<T>(value);
		}
		return Option<T>.None;
	}

	public static Option<T> ToSome<T>(this T value)
	{
		return new Option<T>(value);
	}

	public static Option<T> ToNone<T>(this T value)
	{
		return Option<T>.None;
	}

	public static Option<T> Flatten<T>(this Option<Option<T>> option)
	{
		return option.value;
	}
}
[Serializable]
public struct Option<T> : IComparable<Option<T>>, IEquatable<Option<T>>
{
	public static readonly Option<T> None;

	public readonly bool isSome;

	public readonly T value;

	public bool isNone => !isSome;

	public Option(T value)
	{
		isSome = true;
		this.value = value;
	}

	public U Cata<U>(DelegateFunc<T, U> someFunc, U noneValue)
	{
		if (!isSome)
		{
			return noneValue;
		}
		return someFunc(value);
	}

	public U Cata<U, V>(DelegateFunc<T, V, U> someFunc, V p, U noneValue)
	{
		if (!isSome)
		{
			return noneValue;
		}
		return someFunc(value, p);
	}

	public U Cata<U>(DelegateFunc<T, U> someFunc, DelegateFunc<U> noneFunc)
	{
		if (!isSome)
		{
			return noneFunc();
		}
		return someFunc(value);
	}

	public U Cata<U, V>(DelegateFunc<T, V, U> someFunc, V p, DelegateFunc<U> noneFunc)
	{
		if (!isSome)
		{
			return noneFunc();
		}
		return someFunc(value, p);
	}

	public U Cata<U, V, P2>(DelegateFunc<T, V, U> someFunc, V p, DelegateFunc<P2, U> noneFunc, P2 p2)
	{
		if (!isSome)
		{
			return noneFunc(p2);
		}
		return someFunc(value, p);
	}

	public bool Contains(T t)
	{
		if (isSome)
		{
			return Smooth.Collections.EqualityComparer<T>.Default.Equals(value, t);
		}
		return false;
	}

	public bool Contains(T t, IEqualityComparer<T> comparer)
	{
		if (isSome)
		{
			return comparer.Equals(value, t);
		}
		return false;
	}

	public void IfEmpty(DelegateAction action)
	{
		if (!isSome)
		{
			action();
		}
	}

	public void IfEmpty<U>(DelegateAction<U> action, U p)
	{
		if (!isSome)
		{
			action(p);
		}
	}

	public void ForEach(DelegateAction<T> action)
	{
		if (isSome)
		{
			action(value);
		}
	}

	public void ForEach<U>(DelegateAction<T, U> action, U p)
	{
		if (isSome)
		{
			action(value, p);
		}
	}

	public void ForEachOr(DelegateAction<T> someAction, DelegateAction noneAction)
	{
		if (isSome)
		{
			someAction(value);
		}
		else
		{
			noneAction();
		}
	}

	public void ForEachOr<U>(DelegateAction<T, U> someAction, U p, DelegateAction noneAction)
	{
		if (isSome)
		{
			someAction(value, p);
		}
		else
		{
			noneAction();
		}
	}

	public void ForEachOr<P2>(DelegateAction<T> someAction, DelegateAction<P2> noneAction, P2 p2)
	{
		if (isSome)
		{
			someAction(value);
		}
		else
		{
			noneAction(p2);
		}
	}

	public void ForEachOr<U, P2>(DelegateAction<T, U> someAction, U p, DelegateAction<P2> noneAction, P2 p2)
	{
		if (isSome)
		{
			someAction(value, p);
		}
		else
		{
			noneAction(p2);
		}
	}

	public Option<T> Or(Option<T> noneOption)
	{
		if (!isSome)
		{
			return noneOption;
		}
		return this;
	}

	public Option<T> Or(DelegateFunc<Option<T>> noneFunc)
	{
		if (!isSome)
		{
			return noneFunc();
		}
		return this;
	}

	public Option<T> Or<U>(DelegateFunc<U, Option<T>> noneFunc, U p)
	{
		if (!isSome)
		{
			return noneFunc(p);
		}
		return this;
	}

	public Option<U> Select<U>(DelegateFunc<T, U> selector)
	{
		if (!isSome)
		{
			return Option<U>.None;
		}
		return new Option<U>(selector(value));
	}

	public Option<U> Select<U, V>(DelegateFunc<T, V, U> selector, V p)
	{
		if (!isSome)
		{
			return Option<U>.None;
		}
		return new Option<U>(selector(value, p));
	}

	public Option<U> SelectMany<U>(DelegateFunc<T, Option<U>> selector)
	{
		if (!isSome)
		{
			return Option<U>.None;
		}
		return selector(value);
	}

	public Option<U> SelectMany<U, V>(DelegateFunc<T, V, Option<U>> selector, V p)
	{
		if (!isSome)
		{
			return Option<U>.None;
		}
		return selector(value, p);
	}

	public T ValueOr(T noneValue)
	{
		if (!isSome)
		{
			return noneValue;
		}
		return value;
	}

	public T ValueOr(DelegateFunc<T> noneFunc)
	{
		if (!isSome)
		{
			return noneFunc();
		}
		return value;
	}

	public T ValueOr<U>(DelegateFunc<U, T> noneFunc, U p)
	{
		if (!isSome)
		{
			return noneFunc(p);
		}
		return value;
	}

	public Option<T> Where(DelegateFunc<T, bool> predicate)
	{
		if (isSome && predicate(value))
		{
			return this;
		}
		return None;
	}

	public Option<T> Where<U>(DelegateFunc<T, U, bool> predicate, U p)
	{
		if (isSome && predicate(value, p))
		{
			return this;
		}
		return None;
	}

	public Option<T> WhereNot(DelegateFunc<T, bool> predicate)
	{
		if (isSome && !predicate(value))
		{
			return this;
		}
		return None;
	}

	public Option<T> WhereNot<U>(DelegateFunc<T, U, bool> predicate, U p)
	{
		if (isSome && !predicate(value, p))
		{
			return this;
		}
		return None;
	}

	public override bool Equals(object o)
	{
		if (o is Option<T>)
		{
			return Equals((Option<T>)o);
		}
		return false;
	}

	public bool Equals(Option<T> other)
	{
		if (!isSome)
		{
			return other.isNone;
		}
		return other.Contains(value);
	}

	public override int GetHashCode()
	{
		return Smooth.Collections.EqualityComparer<T>.Default.GetHashCode(value);
	}

	public int CompareTo(Option<T> other)
	{
		if (!isSome)
		{
			if (!other.isSome)
			{
				return 0;
			}
			return -1;
		}
		if (!other.isSome)
		{
			return 1;
		}
		return Smooth.Collections.Comparer<T>.Default.Compare(value, other.value);
	}

	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Option<T> lhs, Option<T> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Option<T> lhs, Option<T> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Option<T> lhs, Option<T> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Option<T> lhs, Option<T> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		if (!isSome)
		{
			return "None";
		}
		return string.Concat("Some(", value, ")");
	}
}
