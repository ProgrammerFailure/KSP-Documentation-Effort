using System.Linq;
using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class JoinLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.Join(SlinqTest.tuples2, SlinqTest.to_1f, SlinqTest.to_1f, (Tuple<int, int> a, Tuple<int, int> b) => 0).Count();
		}
	}
}
