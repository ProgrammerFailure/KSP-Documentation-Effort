using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct GroupJoinContext<T, U, T2, V, W>
{
	public bool needsMove;

	public readonly Lookup<U, T2> lookup;

	public readonly DelegateFunc<V, U> outerSelector;

	public readonly DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, T> resultSelector;

	public readonly bool release;

	public Slinq<V, W> chained;

	public BacktrackDetector bd;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W>> skip = Skip;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W>> remove = Remove;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W>> dispose = Dispose;

	public GroupJoinContext(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, U> outerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, T> resultSelector, bool release)
	{
		needsMove = false;
		this.lookup = lookup;
		this.outerSelector = outerSelector;
		this.resultSelector = resultSelector;
		chained = outer;
		this.release = release;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W>> GroupJoin(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, U> outerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, T> resultSelector, bool release)
	{
		return new Slinq<T, GroupJoinContext<T, U, T2, V, W>>(skip, remove, dispose, new GroupJoinContext<T, U, T2, V, W>(lookup, outer, outerSelector, resultSelector, release));
	}

	public static void Skip(ref GroupJoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.chained.current.isSome)
		{
			BacktrackDetector backtrackDetector = BacktrackDetector.Borrow();
			next = new Option<T>(context.resultSelector(context.chained.current.value, context.lookup.GetValues(context.outerSelector(context.chained.current.value)).SlinqAndKeep(backtrackDetector)));
			return;
		}
		next = default(Option<T>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}

	public static void Remove(ref GroupJoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref GroupJoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}
}
public struct GroupJoinContext<T, U, T2, V, W, X>
{
	public bool needsMove;

	public readonly Lookup<U, T2> lookup;

	public readonly DelegateFunc<V, X, U> outerSelector;

	public readonly DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, X, T> resultSelector;

	public readonly X parameter;

	public readonly bool release;

	public Slinq<V, W> chained;

	public BacktrackDetector bd;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W, X>> skip = Skip;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W, X>> remove = Remove;

	public static readonly Mutator<T, GroupJoinContext<T, U, T2, V, W, X>> dispose = Dispose;

	public GroupJoinContext(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, X, U> outerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, X, T> resultSelector, X parameter, bool release)
	{
		needsMove = false;
		this.lookup = lookup;
		this.outerSelector = outerSelector;
		this.resultSelector = resultSelector;
		this.parameter = parameter;
		chained = outer;
		this.release = release;
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W, X>> GroupJoin(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, X, U> outerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, X, T> resultSelector, X parameter, bool release)
	{
		return new Slinq<T, GroupJoinContext<T, U, T2, V, W, X>>(skip, remove, dispose, new GroupJoinContext<T, U, T2, V, W, X>(lookup, outer, outerSelector, resultSelector, parameter, release));
	}

	public static void Skip(ref GroupJoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
		}
		else
		{
			context.needsMove = true;
		}
		if (context.chained.current.isSome)
		{
			BacktrackDetector backtrackDetector = BacktrackDetector.Borrow();
			next = new Option<T>(context.resultSelector(context.chained.current.value, context.lookup.GetValues(context.outerSelector(context.chained.current.value, context.parameter)).SlinqAndKeep(backtrackDetector), context.parameter));
			return;
		}
		next = default(Option<T>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}

	public static void Remove(ref GroupJoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		Skip(ref context, out next);
	}

	public static void Dispose(ref GroupJoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}
}
