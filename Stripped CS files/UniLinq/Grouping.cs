using System.Collections;
using System.Collections.Generic;

namespace UniLinq;

public class Grouping<T, U> : IGrouping<T, U>, IEnumerable<U>, IEnumerable
{
	public T key;

	public IEnumerable<U> group;

	public T Key
	{
		get
		{
			return key;
		}
		set
		{
			key = value;
		}
	}

	public Grouping(T key, IEnumerable<U> group)
	{
		this.group = group;
		this.key = key;
	}

	public IEnumerator<U> GetEnumerator()
	{
		return group.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return group.GetEnumerator();
	}
}
