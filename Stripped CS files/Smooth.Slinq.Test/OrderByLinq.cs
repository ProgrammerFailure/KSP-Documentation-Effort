using System.Linq;
using UnityEngine;

namespace Smooth.Slinq.Test;

public class OrderByLinq : MonoBehaviour
{
	public void Update()
	{
		for (int i = 0; i < SlinqTest.loops; i++)
		{
			SlinqTest.tuples1.OrderBy(SlinqTest.to_1f).FirstOrDefault();
		}
	}
}
