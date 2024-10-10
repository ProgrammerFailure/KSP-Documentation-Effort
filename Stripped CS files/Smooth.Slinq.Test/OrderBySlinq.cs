using UnityEngine;

namespace Smooth.Slinq.Test;

public class OrderBySlinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Slinq().OrderBy(SlinqTest.to_1).FirstOrDefault();
		}
	}
}
