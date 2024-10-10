using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace EdyCommonTools;

public class CircularBuffer<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection
{
	public int m_capacity;

	public int m_size;

	public int m_head;

	public int m_tail;

	public T[] m_buffer;

	[NonSerialized]
	public object syncRoot;

	public bool allowOverflow { get; set; }

	public int capacity
	{
		get
		{
			return m_capacity;
		}
		set
		{
			if (value != m_capacity)
			{
				if (value < m_size)
				{
					throw new ArgumentOutOfRangeException("Capacity too small (" + value + "). Buffer contains " + m_size + " items.");
				}
				T[] array = new T[value];
				if (m_size > 0)
				{
					CopyTo(array);
				}
				m_buffer = array;
				m_head = 0;
				m_tail = m_size;
				m_capacity = value;
			}
		}
	}

	public int size => m_size;

	int ICollection<T>.Count => size;

	bool ICollection<T>.IsReadOnly => false;

	int ICollection.Count => size;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot
	{
		get
		{
			if (syncRoot == null)
			{
				Interlocked.CompareExchange(ref syncRoot, new object(), null);
			}
			return syncRoot;
		}
	}

	public int headIdx => m_head;

	public int tailIdx => m_tail;

	public CircularBuffer(int m_capacity)
		: this(m_capacity, allowOverflow: true)
	{
	}

	public CircularBuffer(int capacity, bool allowOverflow)
	{
		if (m_capacity < 0)
		{
			throw new ArgumentException("Capacity must be non-negative");
		}
		m_capacity = capacity;
		m_size = 0;
		m_head = 0;
		m_tail = 0;
		m_buffer = new T[m_capacity];
		this.allowOverflow = allowOverflow;
	}

	public int IndexOf(T item)
	{
		int num = m_head;
		EqualityComparer<T> @default = EqualityComparer<T>.Default;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_size)
			{
				if (num == m_capacity)
				{
					num = 0;
				}
				if (item != null || m_buffer[num] != null)
				{
					if (m_buffer[num] != null && @default.Equals(m_buffer[num], item))
					{
						break;
					}
					num2++;
					num++;
					continue;
				}
				return num2;
			}
			return -1;
		}
		return num2;
	}

	public void RemoveAt(int index)
	{
		if (index >= 0 && index < m_size)
		{
			int num = m_head + index;
			if (num >= m_capacity)
			{
				num -= m_capacity;
			}
			int num2 = m_size - index - 1;
			int num3 = 0;
			while (num3 < num2)
			{
				if (num == m_capacity)
				{
					num = 0;
				}
				int num4 = num + 1;
				if (num4 == m_capacity)
				{
					num4 = 0;
				}
				m_buffer[num] = m_buffer[num4];
				num3++;
				num++;
			}
			m_size--;
			m_tail--;
			if (m_tail < 0)
			{
				m_tail = m_capacity - 1;
			}
			return;
		}
		throw new ArgumentOutOfRangeException("Index out of range");
	}

	public bool Contains(T item)
	{
		return IndexOf(item) >= 0;
	}

	public void Clear()
	{
		m_size = 0;
		m_head = 0;
		m_tail = 0;
	}

	public int Put(T[] src)
	{
		return Put(src, 0, src.Length);
	}

	public int Put(T[] src, int srcOffset, int count)
	{
		if (!allowOverflow && count > m_capacity - m_size)
		{
			throw new InvalidOperationException("This operation would cause buffer overflow");
		}
		int num = srcOffset;
		int num2 = 0;
		while (num2 < count)
		{
			if (m_tail == m_capacity)
			{
				m_tail = 0;
			}
			m_buffer[m_tail++] = src[num];
			if (m_size == m_capacity)
			{
				if (m_head == m_capacity)
				{
					m_head = 0;
				}
				m_head++;
			}
			else
			{
				m_size++;
			}
			num2++;
			num++;
		}
		return count;
	}

	public void Put(T item)
	{
		if (!allowOverflow && m_size == m_capacity)
		{
			throw new InvalidOperationException("This operation would cause buffer overflow");
		}
		if (m_tail == m_capacity)
		{
			m_tail = 0;
		}
		m_buffer[m_tail++] = item;
		if (m_size == m_capacity)
		{
			if (m_head == m_capacity)
			{
				m_head = 0;
			}
			m_head++;
		}
		else
		{
			m_size++;
		}
	}

	public void Skip(int count)
	{
		if (count > m_size)
		{
			count = m_size;
		}
		m_head += count;
		if (m_head > m_capacity)
		{
			m_head -= m_capacity;
		}
		m_size -= count;
	}

	public T[] Get(int count)
	{
		if (count > m_size)
		{
			count = m_size;
		}
		T[] array = new T[count];
		Get(array);
		return array;
	}

	public int Get(T[] dst)
	{
		return Get(dst, 0, dst.Length);
	}

	public int Get(T[] dst, int dstOffset, int count)
	{
		if (count > m_size)
		{
			count = m_size;
		}
		int num = dstOffset;
		int num2 = 0;
		while (num2 < count)
		{
			if (m_head == m_capacity)
			{
				m_head = 0;
			}
			dst[num] = m_buffer[m_head++];
			num2++;
			num++;
		}
		m_size -= count;
		return count;
	}

	public T Get()
	{
		if (m_size == 0)
		{
			throw new InvalidOperationException("Buffer is empty");
		}
		if (m_head == m_capacity)
		{
			m_head = 0;
		}
		T result = m_buffer[m_head++];
		m_size--;
		return result;
	}

	public T Peek(int offset)
	{
		if (offset >= m_size)
		{
			throw new InvalidOperationException("Offset overflow");
		}
		int num = m_head + offset;
		if (num >= m_capacity)
		{
			num -= m_capacity;
		}
		return m_buffer[num];
	}

	public T Head()
	{
		if (m_size == 0)
		{
			throw new InvalidOperationException("Buffer is empty");
		}
		int num = m_head;
		if (num == m_capacity)
		{
			num = 0;
		}
		return m_buffer[num];
	}

	public T Tail()
	{
		if (m_size == 0)
		{
			throw new InvalidOperationException("Buffer is empty");
		}
		int num = m_tail - 1;
		if (num < 0)
		{
			num = m_capacity - 1;
		}
		return m_buffer[num];
	}

	public void CopyTo(T[] array)
	{
		CopyTo(array, 0);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		CopyTo(0, array, arrayIndex, m_size);
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		if (count > m_size)
		{
			throw new ArgumentOutOfRangeException("Count too large");
		}
		int num = m_head;
		int num2 = 0;
		while (num2 < count)
		{
			if (num == m_capacity)
			{
				num = 0;
			}
			array[arrayIndex] = m_buffer[num];
			num2++;
			num++;
			arrayIndex++;
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		int bufferIndex = m_head;
		int i = 0;
		while (i < m_size)
		{
			if (bufferIndex == m_capacity)
			{
				bufferIndex = 0;
			}
			yield return m_buffer[bufferIndex];
			i++;
			bufferIndex++;
		}
	}

	public T[] GetBuffer()
	{
		return m_buffer;
	}

	public T[] ToArray()
	{
		T[] array = new T[m_size];
		CopyTo(array);
		return array;
	}

	void ICollection<T>.Add(T item)
	{
		Put(item);
	}

	bool ICollection<T>.Remove(T item)
	{
		int num = IndexOf(item);
		if (num >= 0)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		CopyTo((T[])array, arrayIndex);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
