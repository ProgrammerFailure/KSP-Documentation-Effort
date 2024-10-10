using System;
using Smooth.Algebraics;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct GroupByContext<T, U>
{
	public readonly Lookup<T, U> lookup;

	public readonly bool release;

	public Linked<T> runner;

	public BacktrackDetector bd;

	public static readonly Mutator<Grouping<T, U>, GroupByContext<T, U>> linkedSkip = LinkedSkip;

	public static readonly Mutator<Grouping<T, U>, GroupByContext<T, U>> linkedRemove = LinkedRemove;

	public static readonly Mutator<Grouping<T, U>, GroupByContext<T, U>> linkedDispose = LinkedDispose;

	public static readonly Mutator<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> slinqedSkip = SlinqedSkip;

	public static readonly Mutator<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> slinqedRemove = SlinqedRemove;

	public static readonly Mutator<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> slinqedDispose = SlinqedDispose;

	public GroupByContext(Lookup<T, U> lookup, bool release)
	{
		this.lookup = lookup;
		this.release = release;
		runner = lookup.keys.head;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> Slinq(Lookup<T, U> lookup, bool release)
	{
		return new Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>>(slinqedSkip, slinqedRemove, slinqedDispose, new GroupByContext<T, U>(lookup, release));
	}

	public static Slinq<Grouping<T, U>, GroupByContext<T, U>> SlinqLinked(Lookup<T, U> lookup, bool release)
	{
		return new Slinq<Grouping<T, U>, GroupByContext<T, U>>(linkedSkip, linkedRemove, linkedDispose, new GroupByContext<T, U>(lookup, release));
	}

	public static void LinkedSkip(ref GroupByContext<T, U> context, out Option<Grouping<T, U>> next)
	{
		if (context.runner == null)
		{
			next = default(Option<Grouping<T, U>>);
			if (context.release)
			{
				context.lookup.DisposeInBackground();
			}
		}
		else
		{
			next = new Option<Grouping<T, U>>(new Grouping<T, U>(context.runner.value, context.release ? context.lookup.RemoveValues(context.runner.value) : context.lookup.GetValues(context.runner.value)));
			context.runner = context.runner.next;
		}
	}

	public static void LinkedRemove(ref GroupByContext<T, U> context, out Option<Grouping<T, U>> next)
	{
		throw new NotSupportedException();
	}

	public static void LinkedDispose(ref GroupByContext<T, U> context, out Option<Grouping<T, U>> next)
	{
		next = default(Option<Grouping<T, U>>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}

	public static void SlinqedSkip(ref GroupByContext<T, U> context, out Option<Grouping<T, U, LinkedContext<U>>> next)
	{
		if (context.runner == null)
		{
			next = default(Option<Grouping<T, U, LinkedContext<U>>>);
			if (context.release)
			{
				context.lookup.DisposeInBackground();
			}
		}
		else
		{
			next = new Option<Grouping<T, U, LinkedContext<U>>>(new Grouping<T, U, LinkedContext<U>>(context.runner.value, context.release ? context.lookup.RemoveValues(context.runner.value).SlinqAndDispose() : context.lookup.GetValues(context.runner.value).SlinqAndKeep()));
			context.runner = context.runner.next;
		}
	}

	public static void SlinqedRemove(ref GroupByContext<T, U> context, out Option<Grouping<T, U, LinkedContext<U>>> next)
	{
		throw new NotSupportedException();
	}

	public static void SlinqedDispose(ref GroupByContext<T, U> context, out Option<Grouping<T, U, LinkedContext<U>>> next)
	{
		next = default(Option<Grouping<T, U, LinkedContext<U>>>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}
}
