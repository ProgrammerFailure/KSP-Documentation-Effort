using System.Collections;
using System.Collections.Generic;

namespace Smooth.Collections;

public class LinkedListStepperNodes<T> : IEnumerable<LinkedListNode<T>>, IEnumerable
{
	public readonly LinkedListNode<T> startNode;

	public readonly int step;

	public LinkedListStepperNodes()
	{
	}

	public LinkedListStepperNodes(LinkedListNode<T> startNode, int step)
	{
		this.startNode = startNode;
		this.step = step;
	}

	public IEnumerator<LinkedListNode<T>> GetEnumerator()
	{
		LinkedListNode<T> node = startNode;
		while (node != null)
		{
			yield return node;
			int i = step;
			while (i > 0 && node != null)
			{
				node = node.Next;
				i--;
			}
			for (; i < 0; i++)
			{
				if (node == null)
				{
					break;
				}
				node = node.Previous;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
