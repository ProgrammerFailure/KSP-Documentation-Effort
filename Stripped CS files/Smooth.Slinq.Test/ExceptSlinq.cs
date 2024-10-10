using UnityEngine;

namespace Smooth.Slinq.Test;

public class ExceptSlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().Except(SlinqTest.tuples2.Slinq(), SlinqTest.eq_1).Count();
		}
	}
}
