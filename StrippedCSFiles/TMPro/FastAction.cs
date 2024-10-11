using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TMPro;

public class FastAction
{
	private LinkedList<Action> delegates;

	private Dictionary<Action, LinkedListNode<Action>> lookup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(Action rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(Action rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Call()
	{
		throw null;
	}
}
public class FastAction<A>
{
	private LinkedList<Action<A>> delegates;

	private Dictionary<Action<A>, LinkedListNode<Action<A>>> lookup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(Action<A> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(Action<A> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Call(A a)
	{
		throw null;
	}
}
public class FastAction<A, B>
{
	private LinkedList<Action<A, B>> delegates;

	private Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>> lookup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(Action<A, B> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(Action<A, B> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Call(A a, B b)
	{
		throw null;
	}
}
public class FastAction<A, B, C>
{
	private LinkedList<Action<A, B, C>> delegates;

	private Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>> lookup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(Action<A, B, C> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(Action<A, B, C> rhs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Call(A a, B b, C c)
	{
		throw null;
	}
}
