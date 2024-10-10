using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Collections;
using Smooth.Comparisons;
using Smooth.Delegates;
using Smooth.Dispose;
using Smooth.Pools;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;

namespace Smooth.Slinq;

public struct Slinq<T, U>
{
	public readonly Mutator<T, U> skip;

	public readonly Mutator<T, U> remove;

	public readonly Mutator<T, U> dispose;

	public U context;

	public Option<T> current;

	public Slinq(Mutator<T, U> skip, Mutator<T, U> remove, Mutator<T, U> dispose, U context)
	{
		this.skip = skip;
		this.remove = remove;
		this.dispose = dispose;
		this.context = context;
		skip(ref this.context, out current);
	}

	public void Skip()
	{
		if (current.isSome)
		{
			skip(ref context, out current);
		}
	}

	public Slinq<T, U> SkipAndReturn()
	{
		if (current.isSome)
		{
			skip(ref context, out current);
		}
		return this;
	}

	public void Remove()
	{
		if (current.isSome)
		{
			remove(ref context, out current);
		}
	}

	public Slinq<T, U> RemoveAndReturn()
	{
		if (current.isSome)
		{
			remove(ref context, out current);
		}
		return this;
	}

	public void Dispose()
	{
		if (current.isSome)
		{
			dispose(ref context, out current);
		}
	}

	public void SkipAll()
	{
		while (current.isSome)
		{
			skip(ref context, out current);
		}
	}

	public Slinq<T, U> Skip(int count)
	{
		while (current.isSome && count-- > 0)
		{
			skip(ref context, out current);
		}
		return this;
	}

	public Slinq<T, U> SkipWhile(DelegateFunc<T, bool> predicate)
	{
		while (current.isSome && predicate(current.value))
		{
			skip(ref context, out current);
		}
		return this;
	}

	public Slinq<T, U> SkipWhile<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		while (current.isSome && predicate(current.value, parameter))
		{
			skip(ref context, out current);
		}
		return this;
	}

	public V SkipWhile<V>(V seed, DelegateFunc<V, T, Option<V>> selector)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value);
			if (!option.isSome)
			{
				break;
			}
			skip(ref context, out current);
			seed = option.value;
		}
		return seed;
	}

	public V SkipWhile<V, W>(V seed, DelegateFunc<V, T, W, Option<V>> selector, W parameter)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value, parameter);
			if (!option.isSome)
			{
				break;
			}
			skip(ref context, out current);
			seed = option.value;
		}
		return seed;
	}

	public int RemoveAll()
	{
		int num = 0;
		while (current.isSome)
		{
			remove(ref context, out current);
			num++;
		}
		return num;
	}

	public int RemoveAll(DelegateAction<T> then)
	{
		int num = 0;
		while (current.isSome)
		{
			T value = current.value;
			remove(ref context, out current);
			then(value);
			num++;
		}
		return num;
	}

	public int RemoveAll<V>(DelegateAction<T, V> then, V thenParameter)
	{
		int num = 0;
		while (current.isSome)
		{
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
			num++;
		}
		return num;
	}

	public Slinq<T, U> Remove(int count)
	{
		while (current.isSome && count-- > 0)
		{
			remove(ref context, out current);
		}
		return this;
	}

	public Slinq<T, U> Remove(int count, DelegateAction<T> then)
	{
		while (current.isSome && count-- > 0)
		{
			T value = current.value;
			remove(ref context, out current);
			then(value);
		}
		return this;
	}

	public Slinq<T, U> Remove<V>(int count, DelegateAction<T, V> then, V thenParameter)
	{
		while (current.isSome && count-- > 0)
		{
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile(DelegateFunc<T, bool> predicate)
	{
		while (current.isSome && predicate(current.value))
		{
			remove(ref context, out current);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile(DelegateFunc<T, bool> predicate, DelegateAction<T> then)
	{
		while (current.isSome && predicate(current.value))
		{
			T value = current.value;
			remove(ref context, out current);
			then(value);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile<V>(DelegateFunc<T, bool> predicate, DelegateAction<T, V> then, V thenParameter)
	{
		while (current.isSome && predicate(current.value))
		{
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		while (current.isSome && predicate(current.value, parameter))
		{
			remove(ref context, out current);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile<V>(DelegateFunc<T, V, bool> predicate, V parameter, DelegateAction<T> then)
	{
		while (current.isSome && predicate(current.value, parameter))
		{
			T value = current.value;
			remove(ref context, out current);
			then(value);
		}
		return this;
	}

	public Slinq<T, U> RemoveWhile<V, P2>(DelegateFunc<T, V, bool> predicate, V parameter, DelegateAction<T, P2> then, P2 thenParameter)
	{
		while (current.isSome && predicate(current.value, parameter))
		{
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
		}
		return this;
	}

	public V RemoveWhile<V>(V seed, DelegateFunc<V, T, Option<V>> selector)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value);
			if (!option.isSome)
			{
				break;
			}
			remove(ref context, out current);
			seed = option.value;
		}
		return seed;
	}

	public V RemoveWhile<V>(V seed, DelegateFunc<V, T, Option<V>> selector, DelegateAction<T> then)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value);
			if (!option.isSome)
			{
				break;
			}
			T value = current.value;
			remove(ref context, out current);
			then(value);
			seed = option.value;
		}
		return seed;
	}

	public V RemoveWhile<V, W>(V seed, DelegateFunc<V, T, Option<V>> selector, DelegateAction<T, W> then, W thenParameter)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value);
			if (!option.isSome)
			{
				break;
			}
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
			seed = option.value;
		}
		return seed;
	}

	public V RemoveWhile<V, W>(V seed, DelegateFunc<V, T, W, Option<V>> selector, W parameter)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value, parameter);
			if (!option.isSome)
			{
				break;
			}
			remove(ref context, out current);
			seed = option.value;
		}
		return seed;
	}

	public V RemoveWhile<V, W>(V seed, DelegateFunc<V, T, W, Option<V>> selector, W parameter, DelegateAction<T> then)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value, parameter);
			if (!option.isSome)
			{
				break;
			}
			T value = current.value;
			remove(ref context, out current);
			then(value);
			seed = option.value;
		}
		return seed;
	}

	public V RemoveWhile<V, W, P2>(V seed, DelegateFunc<V, T, W, Option<V>> selector, W parameter, DelegateAction<T, P2> then, P2 thenParameter)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value, parameter);
			if (!option.isSome)
			{
				break;
			}
			T value = current.value;
			remove(ref context, out current);
			then(value, thenParameter);
			seed = option.value;
		}
		return seed;
	}

	public T Aggregate(DelegateFunc<T, T, T> selector)
	{
		return AggregateOrNone(selector).ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public Option<T> AggregateOrNone(DelegateFunc<T, T, T> selector)
	{
		if (current.isSome)
		{
			T val = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				val = selector(val, current.value);
				skip(ref context, out current);
			}
			return new Option<T>(val);
		}
		return current;
	}

	public Option<T> AggregateOrNone<V>(DelegateFunc<T, T, V, T> selector, V parameter)
	{
		if (current.isSome)
		{
			T val = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				val = selector(val, current.value, parameter);
				skip(ref context, out current);
			}
			return new Option<T>(val);
		}
		return current;
	}

	public V Aggregate<V>(V seed, DelegateFunc<V, T, V> selector)
	{
		while (current.isSome)
		{
			seed = selector(seed, current.value);
			skip(ref context, out current);
		}
		return seed;
	}

	public W Aggregate<V, W>(V seed, DelegateFunc<V, T, V> selector, DelegateFunc<V, W> resultSelector)
	{
		return resultSelector(Aggregate(seed, selector));
	}

	public V Aggregate<V, W>(V seed, DelegateFunc<V, T, W, V> selector, W parameter)
	{
		while (current.isSome)
		{
			seed = selector(seed, current.value, parameter);
			skip(ref context, out current);
		}
		return seed;
	}

	public V AggregateWhile<V>(V seed, DelegateFunc<V, T, Option<V>> selector)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value);
			if (option.isSome)
			{
				skip(ref context, out current);
				seed = option.value;
				continue;
			}
			dispose(ref context, out current);
			break;
		}
		return seed;
	}

	public V AggregateWhile<V, W>(V seed, DelegateFunc<V, T, W, Option<V>> selector, W parameter)
	{
		while (current.isSome)
		{
			Option<V> option = selector(seed, current.value, parameter);
			if (option.isSome)
			{
				skip(ref context, out current);
				seed = option.value;
				continue;
			}
			dispose(ref context, out current);
			break;
		}
		return seed;
	}

	public bool All(DelegateFunc<T, bool> predicate)
	{
		while (true)
		{
			if (current.isSome)
			{
				if (!predicate(current.value))
				{
					break;
				}
				skip(ref context, out current);
				continue;
			}
			return true;
		}
		dispose(ref context, out current);
		return false;
	}

	public bool All<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		while (true)
		{
			if (current.isSome)
			{
				if (!predicate(current.value, parameter))
				{
					break;
				}
				skip(ref context, out current);
				continue;
			}
			return true;
		}
		dispose(ref context, out current);
		return false;
	}

	public bool Any()
	{
		if (current.isSome)
		{
			dispose(ref context, out current);
			return true;
		}
		return false;
	}

	public bool Any(DelegateFunc<T, bool> predicate)
	{
		while (true)
		{
			if (current.isSome)
			{
				if (predicate(current.value))
				{
					break;
				}
				skip(ref context, out current);
				continue;
			}
			return false;
		}
		dispose(ref context, out current);
		return true;
	}

	public bool Any<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		while (true)
		{
			if (current.isSome)
			{
				if (predicate(current.value, parameter))
				{
					break;
				}
				skip(ref context, out current);
				continue;
			}
			return false;
		}
		dispose(ref context, out current);
		return true;
	}

	public bool Contains(T value)
	{
		return Contains(value, Smooth.Collections.EqualityComparer<T>.Default);
	}

	public bool Contains(T value, IEqualityComparer<T> comparer)
	{
		while (true)
		{
			if (current.isSome)
			{
				if (comparer.Equals(value, current.value))
				{
					break;
				}
				skip(ref context, out current);
				continue;
			}
			return false;
		}
		dispose(ref context, out current);
		return true;
	}

	public int Count()
	{
		int num = 0;
		while (current.isSome)
		{
			num++;
			skip(ref context, out current);
		}
		return num;
	}

	public T ElementAt(int index)
	{
		return ElementAtOrNone(index).ValueOr(delegate
		{
			throw new ArgumentOutOfRangeException();
		});
	}

	public T ElementAtOrDefault(int index)
	{
		return ElementAtOrNone(index).value;
	}

	public Option<T> ElementAtOrNone(int index)
	{
		if (index > 0)
		{
			return Skip(index - 1).FirstOrNone();
		}
		if (index == 0)
		{
			return FirstOrNone();
		}
		Dispose();
		return Option<T>.None;
	}

	public T First()
	{
		return FirstOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public T First(DelegateFunc<T, bool> predicate)
	{
		return FirstOrNone(predicate).ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public T FirstOrDefault()
	{
		return FirstOrNone().value;
	}

	public T FirstOrDefault(DelegateFunc<T, bool> predicate)
	{
		return FirstOrNone(predicate).value;
	}

	public Option<T> FirstOrNone()
	{
		if (current.isSome)
		{
			Option<T> result = current;
			dispose(ref context, out current);
			return result;
		}
		return current;
	}

	public Option<T> FirstOrNone(DelegateFunc<T, bool> predicate)
	{
		while (current.isSome && !predicate(current.value))
		{
			skip(ref context, out current);
		}
		return FirstOrNone();
	}

	public Option<T> FirstOrNone<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		while (current.isSome && !predicate(current.value, parameter))
		{
			skip(ref context, out current);
		}
		return FirstOrNone();
	}

	public void ForEach(DelegateAction<T> action)
	{
		while (current.isSome)
		{
			action(current.value);
			skip(ref context, out current);
		}
	}

	public void ForEach<V>(DelegateAction<T, V> action, V parameter)
	{
		while (current.isSome)
		{
			action(current.value, parameter);
			skip(ref context, out current);
		}
	}

	public T Last()
	{
		return LastOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public T Last(DelegateFunc<T, bool> predicate)
	{
		return LastOrNone(predicate).ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public T LastOrDefault()
	{
		return LastOrNone().value;
	}

	public T LastOrDefault(DelegateFunc<T, bool> predicate)
	{
		return LastOrNone(predicate).value;
	}

	public Option<T> LastOrNone()
	{
		Option<T> none = Option<T>.None;
		while (current.isSome)
		{
			none = current;
			skip(ref context, out current);
		}
		return none;
	}

	public Option<T> LastOrNone(DelegateFunc<T, bool> predicate)
	{
		Option<T> none = Option<T>.None;
		while (current.isSome)
		{
			if (predicate(current.value))
			{
				none = current;
			}
			skip(ref context, out current);
		}
		return none;
	}

	public Option<T> LastOrNone<V>(DelegateFunc<T, V, bool> predicate, V parameter)
	{
		Option<T> none = Option<T>.None;
		while (current.isSome)
		{
			if (predicate(current.value, parameter))
			{
				none = current;
			}
			skip(ref context, out current);
		}
		return none;
	}

	public T Max()
	{
		return MaxOrNone().ValueOr(delegate
		{
			if (!typeof(T).IsClass)
			{
				throw new InvalidOperationException();
			}
			return default(T);
		});
	}

	public Option<T> MaxOrNone()
	{
		return MaxOrNone(Comparisons<T>.Default);
	}

	public Option<T> MaxOrNone(IComparer<T> comparer)
	{
		return MaxOrNone(Comparisons<T>.ToComparison(comparer));
	}

	public Option<T> MaxOrNone(Comparison<T> comparison)
	{
		if (current.isSome)
		{
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				if (comparison(value, current.value) < 0)
				{
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public Option<T> MaxOrNone<V>(DelegateFunc<T, V> selector)
	{
		return MaxOrNone(selector, Comparisons<V>.Default);
	}

	public Option<T> MaxOrNone<V>(DelegateFunc<T, V> selector, IComparer<V> comparer)
	{
		return MaxOrNone(selector, Comparisons<V>.ToComparison(comparer));
	}

	public Option<T> MaxOrNone<V>(DelegateFunc<T, V> selector, Comparison<V> comparison)
	{
		if (current.isSome)
		{
			V x = selector(current.value);
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				V val = selector(current.value);
				if (comparison(x, val) < 0)
				{
					x = val;
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public Option<T> MaxOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter)
	{
		return MaxOrNone(selector, parameter, Comparisons<V>.Default);
	}

	public Option<T> MaxOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter, IComparer<V> comparer)
	{
		return MaxOrNone(selector, parameter, Comparisons<V>.ToComparison(comparer));
	}

	public Option<T> MaxOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter, Comparison<V> comparison)
	{
		if (current.isSome)
		{
			V x = selector(current.value, parameter);
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				V val = selector(current.value, parameter);
				if (comparison(x, val) < 0)
				{
					x = val;
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public T Min()
	{
		return MinOrNone().ValueOr(delegate
		{
			if (!typeof(T).IsClass)
			{
				throw new InvalidOperationException();
			}
			return default(T);
		});
	}

	public Option<T> MinOrNone()
	{
		return MinOrNone(Comparisons<T>.Default);
	}

	public Option<T> Min(IComparer<T> comparer)
	{
		return MinOrNone(Comparisons<T>.ToComparison(comparer));
	}

	public Option<T> MinOrNone(Comparison<T> comparison)
	{
		if (current.isSome)
		{
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				if (comparison(value, current.value) > 0)
				{
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public Option<T> MinOrNone<V>(DelegateFunc<T, V> selector)
	{
		return MinOrNone(selector, Comparisons<V>.Default);
	}

	public Option<T> MinOrNone<V>(DelegateFunc<T, V> selector, IComparer<V> comparer)
	{
		return MinOrNone(selector, Comparisons<V>.ToComparison(comparer));
	}

	public Option<T> MinOrNone<V>(DelegateFunc<T, V> selector, Comparison<V> comparison)
	{
		if (current.isSome)
		{
			V x = selector(current.value);
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				V val = selector(current.value);
				if (comparison(x, val) > 0)
				{
					x = val;
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public Option<T> MinOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter)
	{
		return MinOrNone(selector, parameter, Comparisons<V>.Default);
	}

	public Option<T> MinOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter, IComparer<V> comparer)
	{
		return MinOrNone(selector, parameter, Comparisons<V>.ToComparison(comparer));
	}

	public Option<T> MinOrNone<V, W>(DelegateFunc<T, W, V> selector, W parameter, Comparison<V> comparison)
	{
		if (current.isSome)
		{
			V x = selector(current.value, parameter);
			T value = current.value;
			skip(ref context, out current);
			while (current.isSome)
			{
				V val = selector(current.value, parameter);
				if (comparison(x, val) > 0)
				{
					x = val;
					value = current.value;
				}
				skip(ref context, out current);
			}
			return new Option<T>(value);
		}
		return current;
	}

	public bool SequenceEqual<C2>(Slinq<T, C2> other)
	{
		return SequenceEqual(other, Comparisons<T>.ToPredicate(Smooth.Collections.EqualityComparer<T>.Default));
	}

	public bool SequenceEqual<C2>(Slinq<T, C2> other, System.Collections.Generic.EqualityComparer<T> equalityComparer)
	{
		return SequenceEqual(other, Comparisons<T>.ToPredicate(equalityComparer));
	}

	public bool SequenceEqual<T2, C2>(Slinq<T2, C2> other, DelegateFunc<T, T2, bool> predicate)
	{
		while (current.isSome && other.current.isSome)
		{
			if (predicate(current.value, other.current.value))
			{
				skip(ref context, out current);
				other.skip(ref other.context, out other.current);
				continue;
			}
			dispose(ref context, out current);
			other.dispose(ref other.context, out other.current);
			return false;
		}
		if (current.isSome)
		{
			dispose(ref context, out current);
			return false;
		}
		if (other.current.isSome)
		{
			other.dispose(ref other.context, out other.current);
			return false;
		}
		return true;
	}

	public T Single()
	{
		return SingleOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public T SingleOrDefault()
	{
		if (current.isSome)
		{
			return SingleOrNone().ValueOr(delegate
			{
				throw new InvalidOperationException();
			});
		}
		return default(T);
	}

	public Option<T> SingleOrNone()
	{
		if (current.isSome)
		{
			Option<T> result = current;
			skip(ref context, out current);
			if (current.isSome)
			{
				dispose(ref context, out current);
				return default(Option<T>);
			}
			return result;
		}
		return current;
	}

	public Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>> SplitRight(int count)
	{
		if (current.isSome)
		{
			if (count > 0)
			{
				LinkedHeadTail<T> item = default(LinkedHeadTail<T>);
				LinkedHeadTail<T> item2 = new LinkedHeadTail<T>(current.value);
				skip(ref context, out current);
				while (current.isSome && item2.count <= count)
				{
					item2.tail.next = Linked<T>.Borrow(current.value);
					item2.tail = item2.tail.next;
					item2.count++;
					skip(ref context, out current);
				}
				if (item2.count > count)
				{
					item.head = item2.head;
					item.tail = item2.head;
					item.count = 1;
					item2.head = item2.head.next;
					item2.count--;
					while (current.isSome)
					{
						item2.tail.next = Linked<T>.Borrow(current.value);
						item2.tail = item2.tail.next;
						item.tail = item2.head;
						item2.head = item2.head.next;
						item.count++;
						skip(ref context, out current);
					}
					item.tail.next = null;
				}
				return new Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>>(item, item2);
			}
			return new Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>>(ToLinked(), default(LinkedHeadTail<T>));
		}
		return default(Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>>);
	}

	public V AddTo<V>(V collection) where V : ICollection<T>
	{
		while (current.isSome)
		{
			collection.Add(current.value);
			skip(ref context, out current);
		}
		return collection;
	}

	public Disposable<V> AddTo<V>(Disposable<V> collection) where V : ICollection<T>
	{
		AddTo(collection.value);
		return collection;
	}

	public W AddTo<V, W>(W collection, DelegateFunc<T, V> selector) where W : ICollection<V>
	{
		while (current.isSome)
		{
			collection.Add(selector(current.value));
			skip(ref context, out current);
		}
		return collection;
	}

	public Disposable<W> AddTo<V, W>(Disposable<W> collection, DelegateFunc<T, V> selector) where W : ICollection<V>
	{
		AddTo(collection.value, selector);
		return collection;
	}

	public W AddTo<V, W, X>(W collection, DelegateFunc<T, X, V> selector, X parameter) where W : ICollection<V>
	{
		while (current.isSome)
		{
			collection.Add(selector(current.value, parameter));
			skip(ref context, out current);
		}
		return collection;
	}

	public Disposable<W> AddTo<V, W, X>(Disposable<W> collection, DelegateFunc<T, X, V> selector, X parameter) where W : ICollection<V>
	{
		AddTo(collection.value, selector, parameter);
		return collection;
	}

	public LinkedHeadTail<T> AddTo(LinkedHeadTail<T> list)
	{
		if (current.isSome)
		{
			list.Append(current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				list.tail.next = Linked<T>.Borrow(current.value);
				list.tail = list.tail.next;
				list.count++;
				skip(ref context, out current);
			}
		}
		return list;
	}

	public LinkedHeadTail<T> AddToReverse(LinkedHeadTail<T> list)
	{
		if (current.isSome)
		{
			LinkedHeadTail<T> other = new LinkedHeadTail<T>(current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				Linked<T> linked = Linked<T>.Borrow(current.value);
				linked.next = other.head;
				other.head = linked;
				other.count++;
				skip(ref context, out current);
			}
			list.Append(other);
			return list;
		}
		return list;
	}

	public LinkedHeadTail<V, T> AddTo<V>(LinkedHeadTail<V, T> list, DelegateFunc<T, V> selector)
	{
		if (current.isSome)
		{
			list.Append(selector(current.value), current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				list.tail.next = Linked<V, T>.Borrow(selector(current.value), current.value);
				list.tail = list.tail.next;
				list.count++;
				skip(ref context, out current);
			}
		}
		return list;
	}

	public LinkedHeadTail<V, T> AddTo<V, W>(LinkedHeadTail<V, T> list, DelegateFunc<T, W, V> selector, W parameter)
	{
		if (current.isSome)
		{
			list.Append(selector(current.value, parameter), current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				list.tail.next = Linked<V, T>.Borrow(selector(current.value, parameter), current.value);
				list.tail = list.tail.next;
				list.count++;
				skip(ref context, out current);
			}
		}
		return list;
	}

	public LinkedHeadTail<V, T> AddToReverse<V>(LinkedHeadTail<V, T> list, DelegateFunc<T, V> selector)
	{
		if (current.isSome)
		{
			LinkedHeadTail<V, T> other = new LinkedHeadTail<V, T>(selector(current.value), current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				Linked<V, T> linked = Linked<V, T>.Borrow(selector(current.value), current.value);
				linked.next = other.head;
				other.head = linked;
				other.count++;
				skip(ref context, out current);
			}
			list.Append(other);
			return list;
		}
		return list;
	}

	public LinkedHeadTail<V, T> AddToReverse<V, W>(LinkedHeadTail<V, T> list, DelegateFunc<T, W, V> selector, W parameter)
	{
		if (current.isSome)
		{
			LinkedHeadTail<V, T> other = new LinkedHeadTail<V, T>(selector(current.value, parameter), current.value);
			skip(ref context, out current);
			while (current.isSome)
			{
				Linked<V, T> linked = Linked<V, T>.Borrow(selector(current.value, parameter), current.value);
				linked.next = other.head;
				other.head = linked;
				other.count++;
				skip(ref context, out current);
			}
			list.Append(other);
			return list;
		}
		return list;
	}

	public Lookup<V, T> AddTo<V>(Lookup<V, T> lookup, DelegateFunc<T, V> selector)
	{
		while (current.isSome)
		{
			lookup.Add(selector(current.value), current.value);
			skip(ref context, out current);
		}
		return lookup;
	}

	public Lookup<V, T> AddTo<V, W>(Lookup<V, T> lookup, DelegateFunc<T, W, V> selector, W parameter)
	{
		while (current.isSome)
		{
			lookup.Add(selector(current.value, parameter), current.value);
			skip(ref context, out current);
		}
		return lookup;
	}

	public LinkedHeadTail<T> ToLinked()
	{
		return AddTo(default(LinkedHeadTail<T>));
	}

	public LinkedHeadTail<T> ToLinkedReverse()
	{
		return AddToReverse(default(LinkedHeadTail<T>));
	}

	public LinkedHeadTail<V, T> ToLinked<V>(DelegateFunc<T, V> selector)
	{
		return AddTo(default(LinkedHeadTail<V, T>), selector);
	}

	public LinkedHeadTail<V, T> ToLinked<V, W>(DelegateFunc<T, W, V> selector, W parameter)
	{
		return AddTo(default(LinkedHeadTail<V, T>), selector, parameter);
	}

	public LinkedHeadTail<V, T> ToLinkedReverse<V>(DelegateFunc<T, V> selector)
	{
		return AddToReverse(default(LinkedHeadTail<V, T>), selector);
	}

	public LinkedHeadTail<V, T> ToLinkedReverse<V, W>(DelegateFunc<T, W, V> selector, W parameter)
	{
		return AddToReverse(default(LinkedHeadTail<V, T>), selector, parameter);
	}

	public Lookup<V, T> ToLookup<V>(DelegateFunc<T, V> selector)
	{
		return AddTo(Lookup<V, T>.Borrow(Smooth.Collections.EqualityComparer<V>.Default), selector);
	}

	public Lookup<V, T> ToLookup<V>(DelegateFunc<T, V> selector, IEqualityComparer<V> comparer)
	{
		return AddTo(Lookup<V, T>.Borrow(comparer), selector);
	}

	public Lookup<V, T> ToLookup<V, W>(DelegateFunc<T, W, V> selector, W parameter)
	{
		return AddTo(Lookup<V, T>.Borrow(Smooth.Collections.EqualityComparer<V>.Default), selector, parameter);
	}

	public Lookup<V, T> ToLookup<V, W>(DelegateFunc<T, W, V> selector, W parameter, IEqualityComparer<V> comparer)
	{
		return AddTo(Lookup<V, T>.Borrow(comparer), selector, parameter);
	}

	public List<T> ToList()
	{
		return AddTo(ListPool<T>.Instance.Borrow());
	}
}
public static class Slinq
{
	public static Slinq<T, AggregateContext<T, U, V>> AggregateRunning<T, U, V>(this Slinq<U, V> slinq, T seed, DelegateFunc<T, U, T> selector)
	{
		return AggregateContext<T, U, V>.AggregateRunning(slinq, seed, selector);
	}

	public static Slinq<T, AggregateContext<T, U, V, W>> AggregateRunning<T, U, V, W>(this Slinq<U, V> slinq, T seed, DelegateFunc<T, U, W, T> selector, W parameter)
	{
		return AggregateContext<T, U, V, W>.AggregateRunning(slinq, seed, selector, parameter);
	}

	public static Slinq<T, ConcatContext<C2, T, U>> Concat<C2, T, U>(this Slinq<T, U> first, Slinq<T, C2> second)
	{
		return ConcatContext<C2, T, U>.Concat(first, second);
	}

	public static Slinq<T, EitherContext<OptionContext<T>, T, U>> DefaultIfEmpty<T, U>(this Slinq<T, U> slinq)
	{
		if (!slinq.current.isSome)
		{
			return EitherContext<OptionContext<T>, T, U>.Right(OptionContext<T>.Slinq(new Option<T>(default(T))));
		}
		return EitherContext<OptionContext<T>, T, U>.Left(slinq);
	}

	public static Slinq<T, EitherContext<OptionContext<T>, T, U>> DefaultIfEmpty<T, U>(this Slinq<T, U> slinq, T defaultValue)
	{
		if (!slinq.current.isSome)
		{
			return EitherContext<OptionContext<T>, T, U>.Right(OptionContext<T>.Slinq(new Option<T>(defaultValue)));
		}
		return EitherContext<OptionContext<T>, T, U>.Left(slinq);
	}

	public static Slinq<T, HashSetContext<T, U>> Distinct<T, U>(this Slinq<T, U> slinq)
	{
		return HashSetContext<T, U>.Distinct(slinq, HashSetPool<T>.Instance.BorrowDisposable(), release: true);
	}

	public static Slinq<T, HashSetContext<T, U>> Distinct<T, U>(this Slinq<T, U> slinq, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U>.Distinct(slinq, HashSetPool<T>.Instance.BorrowDisposable(comparer), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Distinct<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return HashSetContext<T, U, V>.Distinct(slinq, selector, HashSetPool<T>.Instance.BorrowDisposable(), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Distinct<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V>.Distinct(slinq, selector, HashSetPool<T>.Instance.BorrowDisposable(comparer), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Distinct<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return HashSetContext<T, U, V, W>.Distinct(slinq, selector, parameter, HashSetPool<T>.Instance.BorrowDisposable(), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Distinct<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V, W>.Distinct(slinq, selector, parameter, HashSetPool<T>.Instance.BorrowDisposable(comparer), release: true);
	}

	public static Slinq<T, HashSetContext<T, U>> Except<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other)
	{
		return HashSetContext<T, U>.Except(slinq, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable()), release: true);
	}

	public static Slinq<T, HashSetContext<T, U>> Except<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U>.Except(slinq, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer)), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Except<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector)
	{
		return HashSetContext<T, U, V>.Except(slinq, selector, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(), selector), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Except<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V>.Except(slinq, selector, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer), selector), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Except<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter)
	{
		return HashSetContext<T, U, V, W>.Except(slinq, selector, parameter, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(), selector, parameter), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Except<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V, W>.Except(slinq, selector, parameter, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer), selector, parameter), release: true);
	}

	public static Slinq<T, FlattenContext<T, C1, C2>> Flatten<T, C1, C2>(this Slinq<Slinq<T, C1>, C2> slinq)
	{
		return FlattenContext<T, C1, C2>.Flatten(slinq);
	}

	public static Slinq<T, FlattenContext<T, U>> Flatten<T, U>(this Slinq<Option<T>, U> slinq)
	{
		return FlattenContext<T, U>.Flatten(slinq);
	}

	public static Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> GroupBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return slinq.ToLookup(selector, Smooth.Collections.EqualityComparer<T>.Default).SlinqAndDispose();
	}

	public static Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> GroupBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> comparer)
	{
		return slinq.ToLookup(selector, comparer).SlinqAndDispose();
	}

	public static Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> GroupBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.ToLookup(selector, parameter, Smooth.Collections.EqualityComparer<T>.Default).SlinqAndDispose();
	}

	public static Slinq<Grouping<T, U, LinkedContext<U>>, GroupByContext<T, U>> GroupBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> comparer)
	{
		return slinq.ToLookup(selector, parameter, comparer).SlinqAndDispose();
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W>> GroupJoin<T, U, T2, C2, V, W>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, U> outerSelector, DelegateFunc<T2, U> innerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, T> resultSelector)
	{
		return inner.ToLookup(innerSelector, Smooth.Collections.EqualityComparer<U>.Default).GroupJoinAndDispose(outer, outerSelector, resultSelector);
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W>> GroupJoin<T, U, T2, C2, V, W>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, U> outerSelector, DelegateFunc<T2, U> innerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, T> resultSelector, IEqualityComparer<U> comparer)
	{
		return inner.ToLookup(innerSelector, comparer).GroupJoinAndDispose(outer, outerSelector, resultSelector);
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W, X>> GroupJoin<T, U, T2, C2, V, W, X>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, X, U> outerSelector, DelegateFunc<T2, X, U> innerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, X, T> resultSelector, X parameter)
	{
		return inner.ToLookup(innerSelector, parameter, Smooth.Collections.EqualityComparer<U>.Default).GroupJoinAndDispose(outer, outerSelector, resultSelector, parameter);
	}

	public static Slinq<T, GroupJoinContext<T, U, T2, V, W, X>> GroupJoin<T, U, T2, C2, V, W, X>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, X, U> outerSelector, DelegateFunc<T2, X, U> innerSelector, DelegateFunc<V, Slinq<T2, LinkedContext<T2>>, X, T> resultSelector, X parameter, IEqualityComparer<U> comparer)
	{
		return inner.ToLookup(innerSelector, parameter, comparer).GroupJoinAndDispose(outer, outerSelector, resultSelector, parameter);
	}

	public static Slinq<T, HashSetContext<T, U>> Intersect<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other)
	{
		return HashSetContext<T, U>.Intersect(slinq, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable()), release: true);
	}

	public static Slinq<T, HashSetContext<T, U>> Intersect<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U>.Intersect(slinq, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer)), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Intersect<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector)
	{
		return HashSetContext<T, U, V>.Intersect(slinq, selector, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(), selector), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V>> Intersect<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V>.Intersect(slinq, selector, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer), selector), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Intersect<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter)
	{
		return HashSetContext<T, U, V, W>.Intersect(slinq, selector, parameter, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(), selector, parameter), release: true);
	}

	public static Slinq<U, HashSetContext<T, U, V, W>> Intersect<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> comparer)
	{
		return HashSetContext<T, U, V, W>.Intersect(slinq, selector, parameter, other.AddTo(HashSetPool<T>.Instance.BorrowDisposable(comparer), selector, parameter), release: true);
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W>> Join<T, U, T2, C2, V, W>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, U> outerSelector, DelegateFunc<T2, U> innerSelector, DelegateFunc<V, T2, T> resultSelector)
	{
		return inner.ToLookup(innerSelector, Smooth.Collections.EqualityComparer<U>.Default).JoinAndDispose(outer, outerSelector, resultSelector);
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W>> Join<T, U, T2, C2, V, W>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, U> outerSelector, DelegateFunc<T2, U> innerSelector, DelegateFunc<V, T2, T> resultSelector, IEqualityComparer<U> comparer)
	{
		return inner.ToLookup(innerSelector, comparer).JoinAndDispose(outer, outerSelector, resultSelector);
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W, X>> Join<T, U, T2, C2, V, W, X>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, X, U> outerSelector, DelegateFunc<T2, X, U> innerSelector, DelegateFunc<V, T2, X, T> resultSelector, X parameter)
	{
		return inner.ToLookup(innerSelector, parameter, Smooth.Collections.EqualityComparer<U>.Default).JoinAndDispose(outer, outerSelector, resultSelector, parameter);
	}

	public static Slinq<T, JoinContext<T, U, T2, V, W, X>> Join<T, U, T2, C2, V, W, X>(this Slinq<V, W> outer, Slinq<T2, C2> inner, DelegateFunc<V, X, U> outerSelector, DelegateFunc<T2, X, U> innerSelector, DelegateFunc<V, T2, X, T> resultSelector, X parameter, IEqualityComparer<U> comparer)
	{
		return inner.ToLookup(innerSelector, parameter, comparer).JoinAndDispose(outer, outerSelector, resultSelector, parameter);
	}

	public static Slinq<T, LinkedContext<T>> OrderBy<T, U>(this Slinq<T, U> slinq)
	{
		return slinq.OrderBy(Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: true);
	}

	public static Slinq<T, LinkedContext<T>> OrderBy<T, U>(this Slinq<T, U> slinq, IComparer<T> comparer)
	{
		return slinq.OrderBy(Comparisons<T>.ToComparison(comparer), ascending: true);
	}

	public static Slinq<T, LinkedContext<T>> OrderBy<T, U>(this Slinq<T, U> slinq, Comparison<T> comparison)
	{
		return slinq.OrderBy(comparison, ascending: true);
	}

	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, U>(this Slinq<T, U> slinq)
	{
		return slinq.OrderBy(Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: false);
	}

	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, U>(this Slinq<T, U> slinq, IComparer<T> comparer)
	{
		return slinq.OrderBy(Comparisons<T>.ToComparison(comparer), ascending: false);
	}

	public static Slinq<T, LinkedContext<T>> OrderByDescending<T, U>(this Slinq<T, U> slinq, Comparison<T> comparison)
	{
		return slinq.OrderBy(comparison, ascending: false);
	}

	public static Slinq<T, LinkedContext<T>> OrderBy<T, U>(this Slinq<T, U> slinq, Comparison<T> comparison, bool ascending)
	{
		if (!slinq.current.isSome)
		{
			return default(Slinq<T, LinkedContext<T>>);
		}
		return Linked.Sort(slinq.ToLinked(), comparison, ascending).SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return slinq.OrderBy(selector, Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IComparer<T> comparer)
	{
		return slinq.OrderBy(selector, Comparisons<T>.ToComparison(comparer), ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, Comparison<T> comparison)
	{
		return slinq.OrderBy(selector, comparison, ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return slinq.OrderBy(selector, Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IComparer<T> comparer)
	{
		return slinq.OrderBy(selector, Comparisons<T>.ToComparison(comparer), ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, Comparison<T> comparison)
	{
		return slinq.OrderBy(selector, comparison, ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, Comparison<T> comparison, bool ascending)
	{
		if (!slinq.current.isSome)
		{
			return default(Slinq<U, LinkedContext<T, U>>);
		}
		return Linked.Sort(slinq.ToLinked(selector), comparison, ascending).SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.OrderBy(selector, parameter, Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IComparer<T> comparer)
	{
		return slinq.OrderBy(selector, parameter, Comparisons<T>.ToComparison(comparer), ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Comparison<T> comparison)
	{
		return slinq.OrderBy(selector, parameter, comparison, ascending: true);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.OrderBy(selector, parameter, Comparisons<T>.ToComparison(Smooth.Collections.Comparer<T>.Default), ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IComparer<T> comparer)
	{
		return slinq.OrderBy(selector, parameter, Comparisons<T>.ToComparison(comparer), ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderByDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Comparison<T> comparison)
	{
		return slinq.OrderBy(selector, parameter, comparison, ascending: false);
	}

	public static Slinq<U, LinkedContext<T, U>> OrderBy<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, Comparison<T> comparison, bool ascending)
	{
		if (!slinq.current.isSome)
		{
			return default(Slinq<U, LinkedContext<T, U>>);
		}
		return Linked.Sort(slinq.ToLinked(selector, parameter), comparison, ascending).SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return slinq.ToLookup(selector, Smooth.Collections.EqualityComparer<T>.Default).SortKeys(Comparisons<T>.Default, ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
	{
		return slinq.ToLookup(selector, equalityComparer).SortKeys(Comparisons<T>.ToComparison(comparer), ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> equalityComparer, Comparison<T> comparison)
	{
		return slinq.ToLookup(selector, equalityComparer).SortKeys(comparison, ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return slinq.ToLookup(selector, Smooth.Collections.EqualityComparer<T>.Default).SortKeys(Comparisons<T>.Default, ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
	{
		return slinq.ToLookup(selector, equalityComparer).SortKeys(Comparisons<T>.ToComparison(comparer), ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector, IEqualityComparer<T> equalityComparer, Comparison<T> comparison)
	{
		return slinq.ToLookup(selector, equalityComparer).SortKeys(comparison, ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.ToLookup(selector, parameter, Smooth.Collections.EqualityComparer<T>.Default).SortKeys(Comparisons<T>.Default, ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
	{
		return slinq.ToLookup(selector, parameter, equalityComparer).SortKeys(Comparisons<T>.ToComparison(comparer), ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroup<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> equalityComparer, Comparison<T> comparison)
	{
		return slinq.ToLookup(selector, parameter, equalityComparer).SortKeys(comparison, ascending: true).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.ToLookup(selector, parameter, Smooth.Collections.EqualityComparer<T>.Default).SortKeys(Comparisons<T>.Default, ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
	{
		return slinq.ToLookup(selector, parameter, equalityComparer).SortKeys(Comparisons<T>.ToComparison(comparer), ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<U, LinkedContext<U>> OrderByGroupDescending<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> equalityComparer, Comparison<T> comparison)
	{
		return slinq.ToLookup(selector, parameter, equalityComparer).SortKeys(comparison, ascending: false).FlattenAndDispose()
			.SlinqAndDispose();
	}

	public static Slinq<T, LinkedContext<T>> Reverse<T, U>(this Slinq<T, U> slinq)
	{
		if (!slinq.current.isSome)
		{
			return default(Slinq<T, LinkedContext<T>>);
		}
		return slinq.ToLinkedReverse().SlinqAndDispose();
	}

	public static Slinq<T, SelectContext<T, U, V>> Select<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, T> selector)
	{
		return SelectContext<T, U, V>.Select(slinq, selector);
	}

	public static Slinq<T, SelectContext<T, U, V, W>> Select<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, T> selector, W parameter)
	{
		return SelectContext<T, U, V, W>.Select(slinq, selector, parameter);
	}

	public static Slinq<T, SelectSlinqContext<T, U, V, W>> SelectMany<T, U, V, W>(this Slinq<V, W> slinq, DelegateFunc<V, Slinq<T, U>> selector)
	{
		return SelectSlinqContext<T, U, V, W>.SelectMany(slinq, selector);
	}

	public static Slinq<T, SelectSlinqContext<T, U, V, W, X>> SelectMany<T, U, V, W, X>(this Slinq<V, W> slinq, DelegateFunc<V, X, Slinq<T, U>> selector, X parameter)
	{
		return SelectSlinqContext<T, U, V, W, X>.SelectMany(slinq, selector, parameter);
	}

	public static Slinq<T, SelectOptionContext<T, U, V>> SelectMany<T, U, V>(this Slinq<U, V> slinq, DelegateFunc<U, Option<T>> selector)
	{
		return SelectOptionContext<T, U, V>.SelectMany(slinq, selector);
	}

	public static Slinq<T, SelectOptionContext<T, U, V, W>> SelectMany<T, U, V, W>(this Slinq<U, V> slinq, DelegateFunc<U, W, Option<T>> selector, W parameter)
	{
		return SelectOptionContext<T, U, V, W>.SelectMany(slinq, selector, parameter);
	}

	public static Slinq<T, IntContext<T, U>> Take<T, U>(this Slinq<T, U> slinq, int count)
	{
		return IntContext<T, U>.Take(slinq, count);
	}

	public static Slinq<T, LinkedContext<T>> TakeRight<T, U>(this Slinq<T, U> slinq, int count)
	{
		if (slinq.current.isSome)
		{
			Smooth.Algebraics.Tuple<LinkedHeadTail<T>, LinkedHeadTail<T>> tuple = slinq.SplitRight(count);
			tuple.Item1.DisposeInBackground();
			return tuple.Item2.SlinqAndDispose();
		}
		return default(Slinq<T, LinkedContext<T>>);
	}

	public static Slinq<T, PredicateContext<T, U>> TakeWhile<T, U>(this Slinq<T, U> slinq, DelegateFunc<T, bool> predicate)
	{
		return PredicateContext<T, U>.TakeWhile(slinq, predicate);
	}

	public static Slinq<T, PredicateContext<T, U, V>> TakeWhile<T, U, V>(this Slinq<T, U> slinq, DelegateFunc<T, V, bool> predicate, V parameter)
	{
		return PredicateContext<T, U, V>.TakeWhile(slinq, predicate, parameter);
	}

	public static Slinq<T, PredicateContext<T, U>> Where<T, U>(this Slinq<T, U> slinq, DelegateFunc<T, bool> predicate)
	{
		return PredicateContext<T, U>.Where(slinq, predicate);
	}

	public static Slinq<T, PredicateContext<T, U, V>> Where<T, U, V>(this Slinq<T, U> slinq, DelegateFunc<T, V, bool> predicate, V parameter)
	{
		return PredicateContext<T, U, V>.Where(slinq, predicate, parameter);
	}

	public static Slinq<T, HashSetContext<T, ConcatContext<C2, T, U>>> Union<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other)
	{
		return slinq.Concat(other).Distinct(Smooth.Collections.EqualityComparer<T>.Default);
	}

	public static Slinq<T, HashSetContext<T, ConcatContext<C2, T, U>>> Union<C2, T, U>(this Slinq<T, U> slinq, Slinq<T, C2> other, IEqualityComparer<T> comparer)
	{
		return slinq.Concat(other).Distinct(comparer);
	}

	public static Slinq<U, HashSetContext<T, U, ConcatContext<C2, U, V>>> Union<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector)
	{
		return slinq.Concat(other).Distinct(selector, Smooth.Collections.EqualityComparer<T>.Default);
	}

	public static Slinq<U, HashSetContext<T, U, ConcatContext<C2, U, V>>> Union<T, C2, U, V>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, T> selector, IEqualityComparer<T> comparer)
	{
		return slinq.Concat(other).Distinct(selector, comparer);
	}

	public static Slinq<U, HashSetContext<T, U, ConcatContext<C2, U, V>, W>> Union<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter)
	{
		return slinq.Concat(other).Distinct(selector, parameter, Smooth.Collections.EqualityComparer<T>.Default);
	}

	public static Slinq<U, HashSetContext<T, U, ConcatContext<C2, U, V>, W>> Union<T, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<U, C2> other, DelegateFunc<U, W, T> selector, W parameter, IEqualityComparer<T> comparer)
	{
		return slinq.Concat(other).Distinct(selector, parameter, comparer);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, T2>, ZipContext<T2, C2, T, U>> Zip<T2, C2, T, U>(this Slinq<T, U> slinq, Slinq<T2, C2> with)
	{
		return ZipContext<T2, C2, T, U>.Zip(slinq, with, ZipRemoveFlags.Left);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, T2>, ZipContext<T2, C2, T, U>> Zip<T2, C2, T, U>(this Slinq<T, U> slinq, Slinq<T2, C2> with, ZipRemoveFlags removeFlags)
	{
		return ZipContext<T2, C2, T, U>.Zip(slinq, with, removeFlags);
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V>> Zip<T, T2, C2, U, V>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<U, T2, T> selector)
	{
		return ZipContext<T, T2, C2, U, V>.Zip(slinq, with, selector, ZipRemoveFlags.Left);
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V>> Zip<T, T2, C2, U, V>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<U, T2, T> selector, ZipRemoveFlags removeFlags)
	{
		return ZipContext<T, T2, C2, U, V>.Zip(slinq, with, selector, removeFlags);
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V, W>> Zip<T, T2, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<U, T2, W, T> selector, W parameter)
	{
		return ZipContext<T, T2, C2, U, V, W>.Zip(slinq, with, selector, parameter, ZipRemoveFlags.Left);
	}

	public static Slinq<T, ZipContext<T, T2, C2, U, V, W>> Zip<T, T2, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<U, T2, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		return ZipContext<T, T2, C2, U, V, W>.Zip(slinq, with, selector, parameter, removeFlags);
	}

	public static Slinq<Smooth.Algebraics.Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> ZipAll<T2, C2, T, U>(this Slinq<T, U> slinq, Slinq<T2, C2> with)
	{
		return ZipAllContext<T2, C2, T, U>.ZipAll(slinq, with, ZipRemoveFlags.Left);
	}

	public static Slinq<Smooth.Algebraics.Tuple<Option<T>, Option<T2>>, ZipAllContext<T2, C2, T, U>> ZipAll<T2, C2, T, U>(this Slinq<T, U> slinq, Slinq<T2, C2> with, ZipRemoveFlags removeFlags)
	{
		return ZipAllContext<T2, C2, T, U>.ZipAll(slinq, with, removeFlags);
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V>> ZipAll<T, T2, C2, U, V>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<Option<U>, Option<T2>, T> selector)
	{
		return ZipAllContext<T, T2, C2, U, V>.ZipAll(slinq, with, selector, ZipRemoveFlags.Left);
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V>> ZipAll<T, T2, C2, U, V>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<Option<U>, Option<T2>, T> selector, ZipRemoveFlags removeFlags)
	{
		return ZipAllContext<T, T2, C2, U, V>.ZipAll(slinq, with, selector, removeFlags);
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V, W>> ZipAll<T, T2, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<Option<U>, Option<T2>, W, T> selector, W parameter)
	{
		return ZipAllContext<T, T2, C2, U, V, W>.ZipAll(slinq, with, selector, parameter, ZipRemoveFlags.Left);
	}

	public static Slinq<T, ZipAllContext<T, T2, C2, U, V, W>> ZipAll<T, T2, C2, U, V, W>(this Slinq<U, V> slinq, Slinq<T2, C2> with, DelegateFunc<Option<U>, Option<T2>, W, T> selector, W parameter, ZipRemoveFlags removeFlags)
	{
		return ZipAllContext<T, T2, C2, U, V, W>.ZipAll(slinq, with, selector, parameter, removeFlags);
	}

	public static Slinq<Smooth.Algebraics.Tuple<T, int>, ZipContext<int, FuncContext<int, int>, T, U>> ZipWithIndex<T, U>(this Slinq<T, U> slinq)
	{
		return ZipContext<int, FuncContext<int, int>, T, U>.Zip(slinq, Slinqable.Sequence(0, 1), ZipRemoveFlags.Left);
	}

	public static double Average<T>(this Slinq<int, T> slinq)
	{
		return slinq.AverageOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public static double Average<T>(this Slinq<long, T> slinq)
	{
		return slinq.AverageOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public static float Average<T>(this Slinq<float, T> slinq)
	{
		return slinq.AverageOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public static double Average<T>(this Slinq<double, T> slinq)
	{
		return slinq.AverageOrNone().ValueOr(delegate
		{
			throw new InvalidOperationException();
		});
	}

	public static Option<double> AverageOrNone<T>(this Slinq<int, T> slinq)
	{
		if (slinq.current.isSome)
		{
			int num = 0;
			double num2 = 0.0;
			while (slinq.current.isSome)
			{
				num2 += (double)slinq.current.value;
				slinq.skip(ref slinq.context, out slinq.current);
				num++;
			}
			return new Option<double>(num2 / (double)num);
		}
		return default(Option<double>);
	}

	public static Option<double> AverageOrNone<T>(this Slinq<long, T> slinq)
	{
		if (slinq.current.isSome)
		{
			int num = 0;
			double num2 = 0.0;
			while (slinq.current.isSome)
			{
				num2 += (double)slinq.current.value;
				slinq.skip(ref slinq.context, out slinq.current);
				num++;
			}
			return new Option<double>(num2 / (double)num);
		}
		return default(Option<double>);
	}

	public static Option<float> AverageOrNone<T>(this Slinq<float, T> slinq)
	{
		if (slinq.current.isSome)
		{
			int num = 0;
			float num2 = 0f;
			while (slinq.current.isSome)
			{
				num2 += slinq.current.value;
				slinq.skip(ref slinq.context, out slinq.current);
				num++;
			}
			return new Option<float>(num2 / (float)num);
		}
		return default(Option<float>);
	}

	public static Option<double> AverageOrNone<T>(this Slinq<double, T> slinq)
	{
		if (slinq.current.isSome)
		{
			int num = 0;
			double num2 = 0.0;
			while (slinq.current.isSome)
			{
				num2 += slinq.current.value;
				slinq.skip(ref slinq.context, out slinq.current);
				num++;
			}
			return new Option<double>(num2 / (double)num);
		}
		return default(Option<double>);
	}

	public static int Sum<T>(this Slinq<int, T> slinq)
	{
		int num = 0;
		while (slinq.current.isSome)
		{
			num = checked(num + slinq.current.value);
			slinq.skip(ref slinq.context, out slinq.current);
		}
		return num;
	}

	public static long Sum<T>(this Slinq<long, T> slinq)
	{
		long num = 0L;
		while (slinq.current.isSome)
		{
			num = checked(num + slinq.current.value);
			slinq.skip(ref slinq.context, out slinq.current);
		}
		return num;
	}

	public static float Sum<T>(this Slinq<float, T> slinq)
	{
		float num = 0f;
		while (slinq.current.isSome)
		{
			num += slinq.current.value;
			slinq.skip(ref slinq.context, out slinq.current);
		}
		return num;
	}

	public static double Sum<T>(this Slinq<double, T> slinq)
	{
		double num = 0.0;
		while (slinq.current.isSome)
		{
			num += slinq.current.value;
			slinq.skip(ref slinq.context, out slinq.current);
		}
		return num;
	}
}
