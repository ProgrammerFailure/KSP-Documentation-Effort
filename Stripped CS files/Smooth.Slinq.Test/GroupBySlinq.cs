using Smooth.Algebraics;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class GroupBySlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().GroupBy(SlinqTest.to_1).ForEach(delegate(Grouping<int, Tuple<int, int>, LinkedContext<Tuple<int, int>>> g)
			{
				g.values.Count();
			});
		}
	}
}
