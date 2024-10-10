using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies;

[Serializable]
public class StrategySystemConfig
{
	public List<DepartmentConfig> departments = new List<DepartmentConfig>();

	public List<StrategyConfig> strategies = new List<StrategyConfig>();

	public List<DepartmentConfig> Departments => departments;

	public List<StrategyConfig> Strategies => strategies;

	public StrategySystemConfig()
	{
		LoadDepartmentConfigs();
		LoadStrategyConfigs();
	}

	public void LoadDepartmentConfigs()
	{
		departments = new List<DepartmentConfig>();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("STRATEGY_DEPARTMENT");
		int i = 0;
		for (int num = configNodes.Length; i < num; i++)
		{
			DepartmentConfig departmentConfig = DepartmentConfig.Create(configNodes[i]);
			if (departmentConfig != null)
			{
				if (GetDepartmentConfig(departmentConfig.Name) != null)
				{
					Debug.LogError("StrategyConfig: Department '" + departmentConfig.Name + "' already exists");
				}
				else
				{
					departments.Add(departmentConfig);
				}
			}
		}
	}

	public void LoadStrategyConfigs()
	{
		strategies = new List<StrategyConfig>();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("STRATEGY");
		int i = 0;
		for (int num = configNodes.Length; i < num; i++)
		{
			StrategyConfig strategyConfig = StrategyConfig.Create(configNodes[i], departments);
			if (strategyConfig != null)
			{
				if (GetStrategyConfig(strategyConfig.Name) != null)
				{
					Debug.LogError("StrategyConfig: Strategy '" + strategyConfig.Name + "' already exists");
				}
				else
				{
					strategies.Add(strategyConfig);
				}
			}
		}
	}

	public StrategyConfig GetStrategyConfig(string name)
	{
		int num = 0;
		int count = strategies.Count;
		while (true)
		{
			if (num < count)
			{
				if (strategies[num].Name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return strategies[num];
	}

	public DepartmentConfig GetDepartmentConfig(string name)
	{
		int num = 0;
		int count = departments.Count;
		while (true)
		{
			if (num < count)
			{
				if (departments[num].Name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return departments[num];
	}
}
