using System;
using Smooth.Algebraics;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct LinkedContext<T>
{
	public readonly LinkedHeadTail<T> list;

	public readonly bool release;

	public Linked<T> runner;

	public BacktrackDetector bd;

	public static readonly Mutator<T, LinkedContext<T>> skip = Skip;

	public static readonly Mutator<T, LinkedContext<T>> remove = Remove;

	public static readonly Mutator<T, LinkedContext<T>> dispose = Dispose;

	public LinkedContext(LinkedHeadTail<T> list, BacktrackDetector bd, bool release)
	{
		this.list = list;
		runner = list.head;
		this.release = release;
		this.bd = bd;
	}

	public static Slinq<T, LinkedContext<T>> Slinq(LinkedHeadTail<T> list, bool release)
	{
		return new Slinq<T, LinkedContext<T>>(skip, remove, dispose, new LinkedContext<T>(list, BacktrackDetector.Borrow(), release));
	}

	public static Slinq<T, LinkedContext<T>> Slinq(LinkedHeadTail<T> list, BacktrackDetector bd, bool release)
	{
		return new Slinq<T, LinkedContext<T>>(skip, remove, dispose, new LinkedContext<T>(list, bd, release));
	}

	public static void Skip(ref LinkedContext<T> context, out Option<T> next)
	{
		if (context.runner == null)
		{
			next = default(Option<T>);
			if (context.release)
			{
				context.list.DisposeInBackground();
			}
		}
		else
		{
			next = new Option<T>(context.runner.value);
			context.runner = context.runner.next;
		}
	}

	public static void Remove(ref LinkedContext<T> context, out Option<T> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref LinkedContext<T> context, out Option<T> next)
	{
		next = default(Option<T>);
		if (context.release)
		{
			context.list.DisposeInBackground();
		}
	}
}
public struct LinkedContext<T, U>
{
	public readonly LinkedHeadTail<T, U> list;

	public readonly bool release;

	public Linked<T, U> runner;

	public BacktrackDetector bd;

	public static readonly Mutator<U, LinkedContext<T, U>> skip = Skip;

	public static readonly Mutator<U, LinkedContext<T, U>> remove = Remove;

	public static readonly Mutator<U, LinkedContext<T, U>> dispose = Dispose;

	public LinkedContext(LinkedHeadTail<T, U> list, BacktrackDetector bd, bool release)
	{
		this.list = list;
		runner = list.head;
		this.release = release;
		this.bd = bd;
	}

	public static Slinq<U, LinkedContext<T, U>> Slinq(LinkedHeadTail<T, U> list, bool release)
	{
		return new Slinq<U, LinkedContext<T, U>>(skip, remove, dispose, new LinkedContext<T, U>(list, BacktrackDetector.Borrow(), release));
	}

	public static Slinq<U, LinkedContext<T, U>> Slinq(LinkedHeadTail<T, U> list, BacktrackDetector bd, bool release)
	{
		return new Slinq<U, LinkedContext<T, U>>(skip, remove, dispose, new LinkedContext<T, U>(list, bd, release));
	}

	public static void Skip(ref LinkedContext<T, U> context, out Option<U> next)
	{
		if (context.runner == null)
		{
			next = default(Option<U>);
			if (context.release)
			{
				context.list.DisposeInBackground();
			}
		}
		else
		{
			next = new Option<U>(context.runner.value);
			context.runner = context.runner.next;
		}
	}

	public static void Remove(ref LinkedContext<T, U> context, out Option<U> next)
	{
		throw new NotSupportedException();
	}

	public static void Dispose(ref LinkedContext<T, U> context, out Option<U> next)
	{
		next = default(Option<U>);
		if (context.release)
		{
			context.list.DisposeInBackground();
		}
	}
}
