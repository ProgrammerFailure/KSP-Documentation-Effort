using System.Collections.Generic;

namespace Smooth.Pools;

public static class LinkedListNodePool<T>
{
	public static readonly PoolWithInitializer<LinkedListNode<T>, T> _Instance = new PoolWithInitializer<LinkedListNode<T>, T>(() => new LinkedListNode<T>(default(T)), delegate(LinkedListNode<T> node)
	{
		node.Value = default(T);
	}, delegate(LinkedListNode<T> node, T value)
	{
		node.Value = value;
	});

	public static PoolWithInitializer<LinkedListNode<T>, T> Instance => _Instance;
}
