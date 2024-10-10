using System;
using System.Collections.Generic;

namespace TMPro;

public class FastAction
{
	public LinkedList<Action> delegates = new LinkedList<Action>();

	public Dictionary<Action, LinkedListNode<Action>> lookup = new Dictionary<Action, LinkedListNode<Action>>();

	public void Add(Action rhs)
	{
		if (!lookup.ContainsKey(rhs))
		{
			lookup[rhs] = delegates.AddLast(rhs);
		}
	}

	public void Remove(Action rhs)
	{
		if (lookup.TryGetValue(rhs, out var value))
		{
			lookup.Remove(rhs);
			delegates.Remove(value);
		}
	}

	public void Call()
	{
		for (LinkedListNode<Action> linkedListNode = delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value();
		}
	}
}
public class FastAction<T>
{
	public LinkedList<Action<T>> delegates = new LinkedList<Action<T>>();

	public Dictionary<Action<T>, LinkedListNode<Action<T>>> lookup = new Dictionary<Action<T>, LinkedListNode<Action<T>>>();

	public void Add(Action<T> rhs)
	{
		if (!lookup.ContainsKey(rhs))
		{
			lookup[rhs] = delegates.AddLast(rhs);
		}
	}

	public void Remove(Action<T> rhs)
	{
		if (lookup.TryGetValue(rhs, out var value))
		{
			lookup.Remove(rhs);
			delegates.Remove(value);
		}
	}

	public void Call(T a)
	{
		for (LinkedListNode<Action<T>> linkedListNode = delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value(a);
		}
	}
}
public class FastAction<T, U>
{
	public LinkedList<Action<T, U>> delegates = new LinkedList<Action<T, U>>();

	public Dictionary<Action<T, U>, LinkedListNode<Action<T, U>>> lookup = new Dictionary<Action<T, U>, LinkedListNode<Action<T, U>>>();

	public void Add(Action<T, U> rhs)
	{
		if (!lookup.ContainsKey(rhs))
		{
			lookup[rhs] = delegates.AddLast(rhs);
		}
	}

	public void Remove(Action<T, U> rhs)
	{
		if (lookup.TryGetValue(rhs, out var value))
		{
			lookup.Remove(rhs);
			delegates.Remove(value);
		}
	}

	public void Call(T a, U b)
	{
		for (LinkedListNode<Action<T, U>> linkedListNode = delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value(a, b);
		}
	}
}
public class FastAction<T, U, V>
{
	public LinkedList<Action<T, U, V>> delegates = new LinkedList<Action<T, U, V>>();

	public Dictionary<Action<T, U, V>, LinkedListNode<Action<T, U, V>>> lookup = new Dictionary<Action<T, U, V>, LinkedListNode<Action<T, U, V>>>();

	public void Add(Action<T, U, V> rhs)
	{
		if (!lookup.ContainsKey(rhs))
		{
			lookup[rhs] = delegates.AddLast(rhs);
		}
	}

	public void Remove(Action<T, U, V> rhs)
	{
		if (lookup.TryGetValue(rhs, out var value))
		{
			lookup.Remove(rhs);
			delegates.Remove(value);
		}
	}

	public void Call(T a, U b, V c)
	{
		for (LinkedListNode<Action<T, U, V>> linkedListNode = delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value(a, b, c);
		}
	}
}
