using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq.Collections;

namespace Smooth.Slinq.Context;

public struct JoinContext<T, U, T2, V, W>
{
	public bool needsMove;

	public readonly Lookup<U, T2> lookup;

	public readonly DelegateFunc<V, U> outerSelector;

	public readonly DelegateFunc<V, T2, T> resultSelector;

	public readonly bool release;

	public Slinq<V, W> chained;

	public Linked<T2> inner;

	public BacktrackDetector bd;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W>> skip = Skip;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W>> remove = Remove;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W>> dispose = Dispose;

	public JoinContext(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, U> outerSelector, DelegateFunc<V, T2, T> resultSelector, bool release)
	{
		needsMove = false;
		this.lookup = lookup;
		this.outerSelector = outerSelector;
		this.resultSelector = resultSelector;
		chained = outer;
		this.release = release;
		inner = ((!chained.current.isSome || !lookup.dictionary.TryGetValue(outerSelector(chained.current.value), out var value)) ? null : value.head);
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W>> Join(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, U> outerSelector, DelegateFunc<V, T2, T> resultSelector, bool release)
	{
		return new Slinq<T, JoinContext<T, U, T2, V, W>>(skip, remove, dispose, new JoinContext<T, U, T2, V, W>(lookup, outer, outerSelector, resultSelector, release));
	}

	public static void Skip(ref JoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.inner = context.inner.next;
		}
		else
		{
			context.needsMove = true;
		}
		if (context.inner == null && context.chained.current.isSome)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
			while (context.chained.current.isSome)
			{
				context.inner = context.lookup.GetValues(context.outerSelector(context.chained.current.value)).head;
				if (context.inner != null)
				{
					break;
				}
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
		}
		if (context.chained.current.isSome)
		{
			next = new Option<T>(context.resultSelector(context.chained.current.value, context.inner.value));
			return;
		}
		next = default(Option<T>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}

	public static void Remove(ref JoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		context.inner = (context.chained.current.isSome ? context.lookup.GetValues(context.outerSelector(context.chained.current.value)).head : null);
		Skip(ref context, out next);
	}

	public static void Dispose(ref JoinContext<T, U, T2, V, W> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}
}
public struct JoinContext<T, U, T2, V, W, X>
{
	public bool needsMove;

	public readonly Lookup<U, T2> lookup;

	public readonly DelegateFunc<V, X, U> outerSelector;

	public readonly DelegateFunc<V, T2, X, T> resultSelector;

	public readonly X parameter;

	public readonly bool release;

	public Slinq<V, W> chained;

	public Linked<T2> inner;

	public BacktrackDetector bd;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W, X>> skip = Skip;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W, X>> remove = Remove;

	public static readonly Mutator<T, JoinContext<T, U, T2, V, W, X>> dispose = Dispose;

	public JoinContext(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, X, U> outerSelector, DelegateFunc<V, T2, X, T> resultSelector, X parameter, bool release)
	{
		needsMove = false;
		this.lookup = lookup;
		this.outerSelector = outerSelector;
		this.resultSelector = resultSelector;
		this.parameter = parameter;
		chained = outer;
		this.release = release;
		inner = ((!chained.current.isSome || !lookup.dictionary.TryGetValue(outerSelector(chained.current.value, parameter), out var value)) ? null : value.head);
		bd = BacktrackDetector.Borrow();
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W, X>> Join(Lookup<U, T2> lookup, Slinq<V, W> outer, DelegateFunc<V, X, U> outerSelector, DelegateFunc<V, T2, X, T> resultSelector, X parameter, bool release)
	{
		return new Slinq<T, JoinContext<T, U, T2, V, W, X>>(skip, remove, dispose, new JoinContext<T, U, T2, V, W, X>(lookup, outer, outerSelector, resultSelector, parameter, release));
	}

	public static void Skip(ref JoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		if (context.needsMove)
		{
			context.inner = context.inner.next;
		}
		else
		{
			context.needsMove = true;
		}
		if (context.inner == null && context.chained.current.isSome)
		{
			context.chained.skip(ref context.chained.context, out context.chained.current);
			while (context.chained.current.isSome)
			{
				context.inner = context.lookup.GetValues(context.outerSelector(context.chained.current.value, context.parameter)).head;
				if (context.inner != null)
				{
					break;
				}
				context.chained.skip(ref context.chained.context, out context.chained.current);
			}
		}
		if (context.chained.current.isSome)
		{
			next = new Option<T>(context.resultSelector(context.chained.current.value, context.inner.value, context.parameter));
			return;
		}
		next = default(Option<T>);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}

	public static void Remove(ref JoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		context.needsMove = false;
		context.chained.remove(ref context.chained.context, out context.chained.current);
		context.inner = (context.chained.current.isSome ? context.lookup.GetValues(context.outerSelector(context.chained.current.value, context.parameter)).head : null);
		Skip(ref context, out next);
	}

	public static void Dispose(ref JoinContext<T, U, T2, V, W, X> context, out Option<T> next)
	{
		next = default(Option<T>);
		context.chained.dispose(ref context.chained.context, out context.chained.current);
		if (context.release)
		{
			context.lookup.DisposeInBackground();
		}
	}
}
