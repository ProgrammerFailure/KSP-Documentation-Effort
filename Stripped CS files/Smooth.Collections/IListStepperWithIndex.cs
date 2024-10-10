using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;

namespace Smooth.Collections;

public class IListStepperWithIndex<T> : IEnumerable<Tuple<T, int>>, IEnumerable
{
	public readonly IList<T> list;

	public readonly int startIndex;

	public readonly int step;

	public IListStepperWithIndex()
	{
	}

	public IListStepperWithIndex(IList<T> list, int startIndex, int step)
	{
		this.list = list;
		this.startIndex = startIndex;
		this.step = step;
	}

	public IEnumerator<Tuple<T, int>> GetEnumerator()
	{
		for (int i = startIndex; 0 <= i && i < list.Count; i += step)
		{
			yield return new Tuple<T, int>(list[i], i);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
