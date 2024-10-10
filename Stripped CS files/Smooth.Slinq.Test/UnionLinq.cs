using System.Linq;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class UnionLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Union(SlinqTest.tuples2, SlinqTest.eq_1).Count();
		}
	}
}
