using System;
using Smooth.Collections;
using Smooth.Delegates;

namespace Smooth.Algebraics;

[Serializable]
public struct Either<T, U> : IComparable<Either<T, U>>, IEquatable<Either<T, U>>
{
	public readonly bool isLeft;

	public readonly T leftValue;

	public readonly U rightValue;

	public bool isRight => !isLeft;

	public Option<T> leftOption
	{
		get
		{
			if (!isLeft)
			{
				return default(Option<T>);
			}
			return new Option<T>(leftValue);
		}
	}

	public Option<U> rightOption
	{
		get
		{
			if (!isLeft)
			{
				return new Option<U>(rightValue);
			}
			return default(Option<U>);
		}
	}

	public Either(bool isLeft, T leftValue, U rightValue)
	{
		this.isLeft = isLeft;
		this.leftValue = leftValue;
		this.rightValue = rightValue;
	}

	public static Either<T, U> Left(T value)
	{
		return new Either<T, U>(isLeft: true, value, default(U));
	}

	public static Either<T, U> Right(U value)
	{
		return new Either<T, U>(isLeft: false, default(T), value);
	}

	public V Cata<V>(DelegateFunc<T, V> leftFunc, DelegateFunc<U, V> rightFunc)
	{
		if (!isLeft)
		{
			return rightFunc(rightValue);
		}
		return leftFunc(leftValue);
	}

	public V Cata<V, W>(DelegateFunc<T, W, V> leftFunc, W p, DelegateFunc<U, V> rightFunc)
	{
		if (!isLeft)
		{
			return rightFunc(rightValue);
		}
		return leftFunc(leftValue, p);
	}

	public V Cata<V, P2>(DelegateFunc<T, V> leftFunc, DelegateFunc<U, P2, V> rightFunc, P2 p2)
	{
		if (!isLeft)
		{
			return rightFunc(rightValue, p2);
		}
		return leftFunc(leftValue);
	}

	public V Cata<V, W, P2>(DelegateFunc<T, W, V> leftFunc, W p, DelegateFunc<U, P2, V> rightFunc, P2 p2)
	{
		if (!isLeft)
		{
			return rightFunc(rightValue, p2);
		}
		return leftFunc(leftValue, p);
	}

	public void ForEach(DelegateAction<T> leftAction, DelegateAction<U> rightAction)
	{
		if (isLeft)
		{
			leftAction(leftValue);
		}
		else
		{
			rightAction(rightValue);
		}
	}

	public void ForEach<V>(DelegateAction<T, V> leftAction, V p, DelegateAction<U> rightAction)
	{
		if (isLeft)
		{
			leftAction(leftValue, p);
		}
		else
		{
			rightAction(rightValue);
		}
	}

	public void ForEach<P2>(DelegateAction<T> leftAction, DelegateAction<U, P2> rightAction, P2 p2)
	{
		if (isLeft)
		{
			leftAction(leftValue);
		}
		else
		{
			rightAction(rightValue, p2);
		}
	}

	public void ForEach<V, P2>(DelegateAction<T, V> leftAction, V p, DelegateAction<U, P2> rightAction, P2 p2)
	{
		if (isLeft)
		{
			leftAction(leftValue, p);
		}
		else
		{
			rightAction(rightValue, p2);
		}
	}

	public override bool Equals(object o)
	{
		if (o is Either<T, U>)
		{
			return Equals((Either<T, U>)o);
		}
		return false;
	}

	public bool Equals(Either<T, U> other)
	{
		if (!isLeft)
		{
			if (!other.isLeft)
			{
				return EqualityComparer<U>.Default.Equals(rightValue, other.rightValue);
			}
			return false;
		}
		if (other.isLeft)
		{
			return EqualityComparer<T>.Default.Equals(leftValue, other.leftValue);
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (!isLeft)
		{
			return EqualityComparer<U>.Default.GetHashCode(rightValue);
		}
		return EqualityComparer<T>.Default.GetHashCode(leftValue);
	}

	public int CompareTo(Either<T, U> other)
	{
		if (!isLeft)
		{
			if (!other.isLeft)
			{
				return Comparer<U>.Default.Compare(rightValue, other.rightValue);
			}
			return 1;
		}
		if (!other.isLeft)
		{
			return -1;
		}
		return Comparer<T>.Default.Compare(leftValue, other.leftValue);
	}

	public static bool operator ==(Either<T, U> lhs, Either<T, U> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Either<T, U> lhs, Either<T, U> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Either<T, U> lhs, Either<T, U> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Either<T, U> lhs, Either<T, U> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Either<T, U> lhs, Either<T, U> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Either<T, U> lhs, Either<T, U> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		if (!isLeft)
		{
			return string.Concat("[Right: ", rightValue, " ]");
		}
		return string.Concat("[Left: ", leftValue, " ]");
	}
}
