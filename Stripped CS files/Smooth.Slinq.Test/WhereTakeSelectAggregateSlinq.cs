using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class WhereTakeSelectAggregateSlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			(from t in (from t in SlinqTest.tuples1.Slinq()
					where true
					select t).Take(int.MaxValue)
				select (t)).Aggregate(0, (int acc, Tuple<int, int> t) => 0);
		}
	}
}