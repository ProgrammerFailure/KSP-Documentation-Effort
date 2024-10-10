using Smooth.Algebraics;
using Smooth.Slinq.Context;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class GroupJoinSlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().GroupJoin(SlinqTest.tuples2.Slinq(), SlinqTest.to_1, SlinqTest.to_1, (Tuple<int, int> a, Slinq<Tuple<int, int>, LinkedContext<Tuple<int, int>>> bs) => bs.Count()).Count();
		}
	}
}
