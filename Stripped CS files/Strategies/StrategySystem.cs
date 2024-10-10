using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies;

[KSPScenario((ScenarioCreationOptions)96, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class StrategySystem : ScenarioModule
{
	public static List<Type> StrategyTypes;

	public static List<Type> StrategyEffectTypes;

	public StrategySystemConfig systemConfig;

	public List<Strategy> strategies = new List<Strategy>();

	public static StrategySystem Instance { get; set; }

	public StrategySystemConfig SystemConfig => systemConfig;

	public List<Strategy> Strategies => strategies;

	public IEnumerable StrategiesActive
	{
		get
		{
			int iC = strategies.Count;
			int i = 0;
			while (i < iC)
			{
				Strategy strategy = strategies[i];
				if (strategy.IsActive)
				{
					yield return strategy;
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public IEnumerable StrategiesInactive
	{
		get
		{
			int iC = strategies.Count;
			int i = 0;
			while (i < iC)
			{
				Strategy strategy = strategies[i];
				if (!strategy.IsActive)
				{
					yield return strategy;
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public List<DepartmentConfig> Departments => systemConfig.Departments;

	public static void GenerateTypes()
	{
		GenerateStrategyTypes();
		GenerateStrategyEffectTypes();
	}

	public static void GenerateStrategyTypes()
	{
		if (StrategyTypes != null)
		{
			return;
		}
		StrategyTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(Strategy)) && !(t == typeof(Strategy)))
			{
				StrategyTypes.Add(t);
			}
		});
		Debug.Log("[StrategySystem]: Found " + StrategyTypes.Count + " strategy types");
	}

	public static Type GetStrategyType(string typeName)
	{
		int num = 0;
		int count = StrategyTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (StrategyTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return StrategyTypes[num];
	}

	public static void GenerateStrategyEffectTypes()
	{
		if (StrategyEffectTypes != null)
		{
			return;
		}
		StrategyEffectTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(StrategyEffect)) && !(t == typeof(StrategyEffect)))
			{
				StrategyEffectTypes.Add(t);
			}
		});
		Debug.Log("[StrategySystem]: Found " + StrategyEffectTypes.Count + " effect types");
	}

	public static Type GetStrategyEffectType(string typeName)
	{
		int num = 0;
		int count = StrategyEffectTypes.Count;
		while (true)
		{
			if (num < count)
			{
				if (StrategyEffectTypes[num].Name == typeName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return StrategyEffectTypes[num];
	}

	public override void OnAwake()
	{
		Instance = this;
		GenerateTypes();
		systemConfig = new StrategySystemConfig();
	}

	public void OnDestroy()
	{
		Unregister();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Unregister()
	{
		int i = 0;
		for (int count = strategies.Count; i < count; i++)
		{
			strategies[i].Unregister();
		}
	}

	public override void OnLoad(ConfigNode gameNode)
	{
		StartCoroutine(OnLoadRoutine(gameNode));
	}

	public IEnumerator OnLoadRoutine(ConfigNode gameNode)
	{
		yield return null;
		strategies.Clear();
		ConfigNode node = gameNode.GetNode("STRATEGIES");
		if (node == null)
		{
			LoadStrategies(new List<ConfigNode>());
		}
		else
		{
			LoadStrategies(new List<ConfigNode>(node.GetNodes("STRATEGY")));
		}
	}

	public void LoadStrategies(List<ConfigNode> stratNodes)
	{
		for (int i = 0; i < SystemConfig.Strategies.Count; i++)
		{
			StrategyConfig strategyConfig = SystemConfig.Strategies[i];
			Type type = GetStrategyType(strategyConfig.Name);
			if (type == null)
			{
				type = typeof(Strategy);
			}
			strategies.Add(Strategy.Create(type, strategyConfig));
		}
		int j = 0;
		for (int count = Strategies.Count; j < count; j++)
		{
			Strategy strategy = Strategies[j];
			int count2 = stratNodes.Count;
			while (count2-- > 0)
			{
				if (stratNodes[count2].GetValue("name") == strategy.Config.Name)
				{
					ConfigNode node = stratNodes[count2];
					stratNodes.RemoveAt(count2);
					strategy.Load(node);
					break;
				}
			}
		}
	}

	public override void OnSave(ConfigNode gameNode)
	{
		ConfigNode configNode = gameNode.AddNode("STRATEGIES");
		int i = 0;
		for (int count = strategies.Count; i < count; i++)
		{
			Strategy strategy = strategies[i];
			if (strategy.IsActive)
			{
				ConfigNode node = configNode.AddNode("STRATEGY");
				strategy.Save(node);
			}
		}
	}

	public void Update()
	{
		int i = 0;
		for (int count = strategies.Count; i < count; i++)
		{
			if (strategies[i].IsActive)
			{
				strategies[i].Update();
			}
		}
	}

	public bool HasActiveStrategy(string department)
	{
		int num = 0;
		int count = strategies.Count;
		while (true)
		{
			if (num < count)
			{
				if (strategies[num].IsActive && strategies[num].DepartmentName == department)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool HasConflictingActiveStrategies(string[] groupTags)
	{
		List<Strategy> list = new List<Strategy>();
		int count = Strategies.Count;
		while (count-- > 0)
		{
			Strategy strategy = Strategies[count];
			if (strategy.IsActive)
			{
				list.Add(strategy);
			}
		}
		int num = 0;
		int count2 = list.Count;
		while (count2-- > 0)
		{
			bool flag = false;
			Strategy strategy = list[count2];
			int num2 = strategy.GroupTags.Length;
			while (num2-- > 0)
			{
				int num3 = groupTags.Length;
				while (num3-- > 0)
				{
					if (!(strategy.GroupTags[num2] != groupTags[num3]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				num++;
			}
			if (num >= 2)
			{
				break;
			}
		}
		int num4 = 3 - num;
		int count3 = list.Count;
		while (count3-- > 0)
		{
			Strategy strategy = strategies[count3];
			num = 0;
			int num5 = groupTags.Length;
			while (num5-- > 0)
			{
				int num6 = strategy.GroupTags.Length;
				while (num6-- > 0)
				{
					if (strategy.GroupTags[num6] == groupTags[num5])
					{
						num++;
						break;
					}
				}
				if (num >= num4)
				{
					return true;
				}
			}
		}
		return false;
	}

	public List<Strategy> GetStrategies(string department)
	{
		List<Strategy> list = new List<Strategy>();
		for (int i = 0; i < strategies.Count; i++)
		{
			if (strategies[i].DepartmentName == department)
			{
				list.Add(strategies[i]);
			}
		}
		return list;
	}
}
