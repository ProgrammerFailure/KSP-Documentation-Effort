using System.Linq;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class ExceptLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Except(SlinqTest.tuples2, SlinqTest.eq_1).Count();
		}
	}
}
