using System;
using System.Collections.Generic;

public static class WarpTransitionCalculator
{
	public static List<WarpTransition> WarpRateTransitionPeriods = new List<WarpTransition>();

	public static float[] warpRates;

	public static double UTToRateDownOne => UTToRateDown(TimeWarp.CurrentRateIndex, Math.Max(TimeWarp.CurrentRateIndex - 1, 0));

	public static double UTToRateTimesOne => UTToRateDown(TimeWarp.CurrentRateIndex, 0);

	public static bool CheckForTransitionChanges()
	{
		if (TimeWarp.fetch == null)
		{
			return false;
		}
		float[] array = TimeWarp.fetch.warpRates;
		bool flag = false;
		if (warpRates == null)
		{
			flag = true;
		}
		else if (warpRates.Length != array.Length)
		{
			flag = true;
		}
		else
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (warpRates[i] != array[i])
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			warpRates = new float[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				warpRates[j] = array[j];
			}
			CalcWarpRateTransitions();
		}
		return flag;
	}

	public static void CalcWarpRateTransitions()
	{
		if (TimeWarp.fetch == null)
		{
			return;
		}
		WarpRateTransitionPeriods = new List<WarpTransition>();
		for (int i = 0; i < TimeWarp.fetch.warpRates.Length; i++)
		{
			WarpTransition warpTransition = new WarpTransition(i, TimeWarp.fetch.warpRates[i]);
			if (i > 0)
			{
				warpTransition.UTToRateDown = (TimeWarp.fetch.warpRates[i] + TimeWarp.fetch.warpRates[i - 1]) / 2f;
			}
			if (i < TimeWarp.fetch.warpRates.Length - 1)
			{
				warpTransition.UTToRateUp = (TimeWarp.fetch.warpRates[i] + TimeWarp.fetch.warpRates[i + 1]) / 2f;
			}
			WarpRateTransitionPeriods.Add(warpTransition);
		}
		for (int j = 0; j < WarpRateTransitionPeriods.Count; j++)
		{
			WarpTransition warpTransition2 = WarpRateTransitionPeriods[j];
			warpTransition2.UTTo1Times = 0.0;
			for (int k = 0; k < WarpRateTransitionPeriods.Count; k++)
			{
				WarpTransition warpTransition3 = WarpRateTransitionPeriods[k];
				if (warpTransition3.Index <= warpTransition2.Index)
				{
					warpTransition2.UTTo1Times += warpTransition3.UTToRateDown;
				}
			}
		}
	}

	public static double UTToRateDown(int FromIndex, int ToIndex)
	{
		double num = 0.0;
		int i = 0;
		for (int count = WarpRateTransitionPeriods.Count; i < count; i++)
		{
			if (WarpRateTransitionPeriods[i].Index == FromIndex)
			{
				num = WarpRateTransitionPeriods[i].UTTo1Times;
			}
		}
		int j = 0;
		for (int count2 = WarpRateTransitionPeriods.Count; j < count2; j++)
		{
			if (WarpRateTransitionPeriods[j].Index == ToIndex)
			{
				num -= WarpRateTransitionPeriods[j].UTTo1Times;
			}
		}
		return num;
	}

	public static int SafeRateToUTPeriod(double seconds)
	{
		int count = WarpRateTransitionPeriods.Count;
		do
		{
			if (count-- <= 0)
			{
				return 0;
			}
		}
		while (WarpRateTransitionPeriods[count].UTTo1Times >= seconds);
		return Math.Min(count + 1, WarpRateTransitionPeriods.Count - 1);
	}
}
