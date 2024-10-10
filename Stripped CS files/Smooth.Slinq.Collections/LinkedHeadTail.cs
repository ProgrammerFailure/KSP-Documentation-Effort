using System;
using System.Collections.Generic;
using Smooth.Collections;
using Smooth.Delegates;
using Smooth.Slinq.Context;

namespace Smooth.Slinq.Collections;

public struct LinkedHeadTail<T> : IEquatable<LinkedHeadTail<T>>
{
	public Linked<T> head;

	public Linked<T> tail;

	public int count;

	public LinkedHeadTail(T value)
		: this(Linked<T>.Borrow(value))
	{
	}

	public LinkedHeadTail(Linked<T> head)
	{
		if (head == null)
		{
			this.head = null;
			tail = null;
			count = 0;
			return;
		}
		this.head = head;
		tail = head;
		count = 1;
		while (tail.next != null)
		{
			tail = tail.next;
			count++;
		}
	}

	public override bool Equals(object other)
	{
		if (other is LinkedHeadTail<T>)
		{
			return Equals((LinkedHeadTail<T>)other);
		}
		return false;
	}

	public bool Equals(LinkedHeadTail<T> other)
	{
		return head == other.head;
	}

	public override int GetHashCode()
	{
		return head.GetHashCode();
	}

	public static bool operator ==(LinkedHeadTail<T> lhs, LinkedHeadTail<T> rhs)
	{
		return lhs.head == rhs.head;
	}

	public static bool operator !=(LinkedHeadTail<T> lhs, LinkedHeadTail<T> rhs)
	{
		return lhs.head != rhs.head;
	}

	public void Append(T value)
	{
		Linked<T> next = Linked<T>.Borrow(value);
		if (head == null)
		{
			head = next;
		}
		else
		{
			tail.next = next;
		}
		tail = next;
		count++;
	}

	public void Append(Linked<T> node)
	{
		if (node != null)
		{
			if (head == null)
			{
				head = node;
			}
			else
			{
				tail.next = node;
			}
			tail = node;
			count++;
			while (tail.next != null)
			{
				tail = tail.next;
				count++;
			}
		}
	}

	public void Append(LinkedHeadTail<T> other)
	{
		if (other.count != 0)
		{
			if (head == null)
			{
				head = other.head;
				tail = other.tail;
				count = other.count;
			}
			else
			{
				tail.next = other.head;
				tail = other.tail;
				count += other.count;
			}
		}
	}

	public void DisposeInBackground()
	{
		if (head != null)
		{
			head.DisposeInBackground();
		}
		head = null;
		tail = null;
		count = 0;
	}

	public void Dispose()
	{
		if (head != null)
		{
			head.Dispose();
		}
		head = null;
		tail = null;
		count = 0;
	}

	public Slinq<T, LinkedContext<T>> SlinqAndKeep()
	{
		return LinkedContext<T>.Slinq(this, release: false);
	}

	public Slinq<T, LinkedContext<T>> SlinqAndKeep(BacktrackDetector bd)
	{
		return LinkedContext<T>.Slinq(this, bd, release: false);
	}

	public Slinq<T, LinkedContext<T>> SlinqAndDispose()
	{
		Slinq<T, LinkedContext<T>> result = LinkedContext<T>.Slinq(this, release: true);
		head = null;
		tail = null;
		count = 0;
		return result;
	}

	public Lookup<U, T> AddTo<U>(Lookup<U, T> lookup, DelegateFunc<T, U> selector)
	{
		while (head != null)
		{
			Linked<T> linked = head;
			head = head.next;
			linked.next = null;
			lookup.Add(selector(linked.value), linked);
		}
		tail = null;
		count = 0;
		return lookup;
	}

	public Lookup<U, T> AddTo<U, V>(Lookup<U, T> lookup, DelegateFunc<T, V, U> selector, V parameter)
	{
		while (head != null)
		{
			Linked<T> linked = head;
			head = head.next;
			linked.next = null;
			lookup.Add(selector(linked.value, parameter), linked);
		}
		tail = null;
		count = 0;
		return lookup;
	}

	public Lookup<U, T> ToLookup<U>(DelegateFunc<T, U> selector)
	{
		return AddTo(Lookup<U, T>.Borrow(Smooth.Collections.EqualityComparer<U>.Default), selector);
	}

	public Lookup<U, T> ToLookup<U>(DelegateFunc<T, U> selector, IEqualityComparer<U> comparer)
	{
		return AddTo(Lookup<U, T>.Borrow(comparer), selector);
	}

	public Lookup<U, T> ToLookup<U, V>(DelegateFunc<T, V, U> selector, V parameter)
	{
		return AddTo(Lookup<U, T>.Borrow(Smooth.Collections.EqualityComparer<U>.Default), selector, parameter);
	}

	public Lookup<U, T> ToLookup<U, V>(DelegateFunc<T, V, U> selector, V parameter, IEqualityComparer<U> comparer)
	{
		return AddTo(Lookup<U, T>.Borrow(comparer), selector, parameter);
	}
}
public struct LinkedHeadTail<T, U> : IEquatable<LinkedHeadTail<T, U>>
{
	public Linked<T, U> head;

	public Linked<T, U> tail;

	public int count;

	public LinkedHeadTail(T key, U value)
		: this(Linked<T, U>.Borrow(key, value))
	{
	}

	public LinkedHeadTail(Linked<T, U> head)
	{
		if (head == null)
		{
			this.head = null;
			tail = null;
			count = 0;
			return;
		}
		this.head = head;
		tail = head;
		count = 1;
		while (tail.next != null)
		{
			tail = tail.next;
			count++;
		}
	}

	public override bool Equals(object other)
	{
		if (other is LinkedHeadTail<T, U>)
		{
			return Equals((LinkedHeadTail<T, U>)other);
		}
		return false;
	}

	public bool Equals(LinkedHeadTail<T, U> other)
	{
		return head == other.head;
	}

	public override int GetHashCode()
	{
		return head.GetHashCode();
	}

	public static bool operator ==(LinkedHeadTail<T, U> lhs, LinkedHeadTail<T, U> rhs)
	{
		return lhs.head == rhs.head;
	}

	public static bool operator !=(LinkedHeadTail<T, U> lhs, LinkedHeadTail<T, U> rhs)
	{
		return lhs.head != rhs.head;
	}

	public void Append(T key, U value)
	{
		Linked<T, U> next = Linked<T, U>.Borrow(key, value);
		if (head == null)
		{
			head = next;
		}
		else
		{
			tail.next = next;
		}
		tail = next;
		count++;
	}

	public void Append(Linked<T, U> node)
	{
		if (head == null)
		{
			head = node;
		}
		else
		{
			tail.next = node;
		}
		tail = node;
		count++;
		while (tail.next != null)
		{
			tail = tail.next;
			count++;
		}
	}

	public void Append(LinkedHeadTail<T, U> other)
	{
		if (other.count != 0)
		{
			if (head == null)
			{
				head = other.head;
				tail = other.tail;
				count = other.count;
			}
			else
			{
				tail.next = other.head;
				tail = other.tail;
				count += other.count;
			}
		}
	}

	public void DisposeInBackground()
	{
		if (head != null)
		{
			head.DisposeInBackground();
		}
		head = null;
		tail = null;
		count = 0;
	}

	public void Dispose()
	{
		if (head != null)
		{
			head.Dispose();
		}
		head = null;
		tail = null;
		count = 0;
	}

	public Slinq<U, LinkedContext<T, U>> SlinqAndKeep()
	{
		return LinkedContext<T, U>.Slinq(this, release: false);
	}

	public Slinq<U, LinkedContext<T, U>> SlinqAndKeep(BacktrackDetector bd)
	{
		return LinkedContext<T, U>.Slinq(this, bd, release: false);
	}

	public Slinq<U, LinkedContext<T, U>> SlinqAndDispose()
	{
		Slinq<U, LinkedContext<T, U>> result = LinkedContext<T, U>.Slinq(this, release: true);
		head = null;
		tail = null;
		count = 0;
		return result;
	}
}
