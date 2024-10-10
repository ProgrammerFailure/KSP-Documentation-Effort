using System.Linq;
using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class WhereTakeSelectAggregateLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			(from t in SlinqTest.tuples1.Where((Tuple<int, int> t) => true).Take(int.MaxValue)
				select (t)).Aggregate(0, (int acc, Tuple<int, int> t) => 0);
		}
	}
}
