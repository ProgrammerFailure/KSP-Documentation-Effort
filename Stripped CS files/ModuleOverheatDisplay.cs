using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleOverheatDisplay : PartModule
{
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001487")]
	public string heatDisplay = "";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001488")]
	public string coreTempDisplay = "";

	public double oldHeat = -1.0;

	public double oldTemp = -1.0;

	public double oldGoal = -1.0;

	public List<IOverheatDisplay> _modules;

	public static string cacheAutoLOC_259510;

	public static string cacheAutoLOC_7001406;

	public override void OnStart(StartState state)
	{
		_modules = new List<IOverheatDisplay>();
		_modules.AddRange(base.part.FindModulesImplementing<IOverheatDisplay>());
		Debug.Log("Found " + _modules.Count + " overheatable modules");
		base.OnStart(state);
	}

	public override void OnUpdate()
	{
		bool flag = false;
		bool flag2 = false;
		int count = _modules.Count;
		for (int i = 0; i < count; i++)
		{
			IOverheatDisplay overheatDisplay = _modules[i];
			if (overheatDisplay.DisplayCoreHeat())
			{
				flag = true;
			}
			if (overheatDisplay.ModuleIsActive())
			{
				flag2 = true;
			}
			if (flag2 && flag)
			{
				break;
			}
		}
		double value = base.part.temperature;
		double value2 = base.part.temperature;
		if (flag)
		{
			IOverheatDisplay overheatDisplay2 = _modules[0];
			value = overheatDisplay2.GetCoreTemperature();
			value2 = overheatDisplay2.GetGoalTemperature();
		}
		if (flag && flag2)
		{
			float num = 2f;
			for (int j = 0; j < count; j++)
			{
				IOverheatDisplay overheatDisplay3 = _modules[j];
				float heatThrottle = overheatDisplay3.GetHeatThrottle();
				if (heatThrottle < num)
				{
					num = heatThrottle;
					value = overheatDisplay3.GetCoreTemperature();
					value2 = overheatDisplay3.GetGoalTemperature();
				}
			}
			double num2 = Math.Round(num * 100f, 1);
			if (num2 != oldHeat || heatDisplay == "n/a")
			{
				heatDisplay = KSPUtil.LocalizeNumber(num2, "0.0") + "%";
				oldHeat = num2;
			}
		}
		else
		{
			heatDisplay = cacheAutoLOC_259510;
		}
		double num3 = Math.Round(value, 2);
		double num4 = Math.Round(value2, 2);
		if (num3 != oldTemp || num4 != oldGoal)
		{
			string text = cacheAutoLOC_7001406;
			coreTempDisplay = KSPUtil.LocalizeNumber(num3, "0.00") + text + " / " + KSPUtil.LocalizeNumber(num4, "0.00") + text;
			oldTemp = num3;
			oldGoal = num4;
		}
		base.OnUpdate();
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_259510 = Localizer.Format("#autoLOC_259510");
		cacheAutoLOC_7001406 = Localizer.Format("#autoLOC_7001406");
	}
}
