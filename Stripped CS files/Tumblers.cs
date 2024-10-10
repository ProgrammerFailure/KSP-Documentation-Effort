using System;
using UnityEngine;

public class Tumblers : MonoBehaviour
{
	public Tumbler[] tumblers;

	public Tumbler unitTumbler;

	public double currValue;

	public int lastUnit;

	public double value => currValue;

	public void Awake()
	{
		currValue = 0.0;
	}

	public void setValue(double val)
	{
		val = Math.Abs(val);
		if (unitTumbler != null)
		{
			int num = 0;
			while (val >= 1000000.0)
			{
				val /= 1000.0;
				num++;
			}
			unitTumbler.tumbleTo(num, (num <= lastUnit) ? Tumbler.TumbleDirection.Down : Tumbler.TumbleDirection.Up);
			lastUnit = num;
		}
		int num2 = tumblers.Length;
		while (num2-- > 0)
		{
			tumblers[num2].tumbleTo(val / Math.Pow(10.0, num2), (!(val > currValue)) ? Tumbler.TumbleDirection.Down : Tumbler.TumbleDirection.Up);
		}
		currValue = val;
	}
}
