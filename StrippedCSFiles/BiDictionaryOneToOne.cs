using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BiDictionaryOneToOne<TFirst, TSecond>
{
	public IDictionary<TFirst, TSecond> firstToSecond;

	public IDictionary<TSecond, TFirst> secondToFirst;

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BiDictionaryOneToOne()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(TFirst first, TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TSecond GetByFirst(TFirst first)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TFirst GetBySecond(TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveByFirst(TFirst first)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveBySecond(TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryAdd(TFirst first, TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetByFirst(TFirst first, out TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetBySecond(TSecond second, out TFirst first)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryRemoveByFirst(TFirst first)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryRemoveBySecond(TSecond second)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}
}
