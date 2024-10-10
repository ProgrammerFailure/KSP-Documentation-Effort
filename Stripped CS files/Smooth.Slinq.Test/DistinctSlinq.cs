using UnityEngine;

namespace Smooth.Slinq.Test;

public class DistinctSlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().Distinct(SlinqTest.eq_1).Count();
		}
	}
}
