using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class JoinSlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().Join(SlinqTest.tuples2.Slinq(), SlinqTest.to_1, SlinqTest.to_1, (Tuple<int, int> a, Tuple<int, int> b) => 0).Count();
		}
	}
}
