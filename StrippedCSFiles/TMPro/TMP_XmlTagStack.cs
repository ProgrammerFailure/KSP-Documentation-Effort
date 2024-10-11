using System.Runtime.CompilerServices;

namespace TMPro;

public struct TMP_XmlTagStack<T>
{
	public T[] itemStack;

	public int index;

	private int m_capacity;

	private T m_defaultItem;

	private const int k_defaultCapacity = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_XmlTagStack(T[] tagStack)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefault(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Remove()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Push(T item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Pop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T CurrentItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T PreviousItem()
	{
		throw null;
	}
}
