using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Collections;

public class FuncEnumerable<T> : IEnumerable<T>, IEnumerable
{
	public readonly T seed;

	public readonly Either<DelegateFunc<T, T>, DelegateFunc<T, Option<T>>> step;

	public FuncEnumerable()
	{
	}

	public FuncEnumerable(T seed, DelegateFunc<T, T> step)
	{
		this.seed = seed;
		this.step = Either<DelegateFunc<T, T>, DelegateFunc<T, Option<T>>>.Left(step);
	}

	public FuncEnumerable(T seed, DelegateFunc<T, Option<T>> step)
	{
		this.seed = seed;
		this.step = Either<DelegateFunc<T, T>, DelegateFunc<T, Option<T>>>.Right(step);
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (step.isLeft)
		{
			T current = seed;
			while (true)
			{
				yield return current;
				current = step.leftValue(current);
			}
		}
		Option<T> current2 = new Option<T>(seed);
		while (current2.isSome)
		{
			yield return current2.value;
			current2 = step.rightValue(current2.value);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
public class FuncEnumerable<T, U> : IEnumerable<T>, IEnumerable
{
	public readonly T seed;

	public readonly Either<DelegateFunc<T, U, T>, DelegateFunc<T, U, Option<T>>> step;

	public readonly U parameter;

	public FuncEnumerable()
	{
	}

	public FuncEnumerable(T seed, DelegateFunc<T, U, T> step, U parameter)
	{
		this.seed = seed;
		this.step = Either<DelegateFunc<T, U, T>, DelegateFunc<T, U, Option<T>>>.Left(step);
		this.parameter = parameter;
	}

	public FuncEnumerable(T seed, DelegateFunc<T, U, Option<T>> step, U parameter)
	{
		this.seed = seed;
		this.step = Either<DelegateFunc<T, U, T>, DelegateFunc<T, U, Option<T>>>.Right(step);
		this.parameter = parameter;
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (step.isLeft)
		{
			T current = seed;
			while (true)
			{
				yield return current;
				current = step.leftValue(current, parameter);
			}
		}
		Option<T> current2 = new Option<T>(seed);
		while (current2.isSome)
		{
			yield return current2.value;
			current2 = step.rightValue(current2.value, parameter);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
