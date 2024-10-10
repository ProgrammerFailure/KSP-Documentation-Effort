using System.Linq;
using Smooth.Algebraics;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class GroupByLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			foreach (IGrouping<int, Tuple<int, int>> item in SlinqTest.tuples1.GroupBy(SlinqTest.to_1f))
			{
				item.Count();
			}
		}
	}
}
