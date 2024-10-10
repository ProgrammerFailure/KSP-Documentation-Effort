using System.Collections.Generic;
using System.Linq;
using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class GroupJoinLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.GroupJoin(SlinqTest.tuples2, SlinqTest.to_1f, SlinqTest.to_1f, (Tuple<int, int> a, IEnumerable<Tuple<int, int>> bs) => bs.Count()).Count();
		}
	}
}
