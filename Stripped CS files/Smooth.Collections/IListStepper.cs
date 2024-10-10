using System.Collections;
using System.Collections.Generic;

namespace Smooth.Collections;

public class IListStepper<T> : IEnumerable<T>, IEnumerable
{
	public readonly IList<T> list;

	public readonly int startIndex;

	public readonly int step;

	public IListStepper()
	{
	}

	public IListStepper(IList<T> list, int startIndex, int step)
	{
		this.list = list;
		this.startIndex = startIndex;
		this.step = step;
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = startIndex; 0 <= i && i < list.Count; i += step)
		{
			yield return list[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
