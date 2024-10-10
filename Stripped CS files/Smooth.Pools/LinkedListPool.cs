using System.Collections.Generic;

namespace Smooth.Pools;

public static class LinkedListPool<T>
{
	public static readonly Pool<LinkedList<T>> _Instance = new Pool<LinkedList<T>>(() => new LinkedList<T>(), delegate(LinkedList<T> list)
	{
		for (LinkedListNode<T> first = list.First; first != null; first = list.First)
		{
			list.RemoveFirst();
			LinkedListNodePool<T>.Instance.Release(first);
		}
	});

	public static Pool<LinkedList<T>> Instance => _Instance;
}
