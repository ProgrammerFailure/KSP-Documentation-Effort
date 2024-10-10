using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public sealed class MissionScore : DynamicModuleList
{
	public static List<Type> globalScoreModules { get; set; }

	public IScoreableObjective scoreableObjective
	{
		get
		{
			if (base.node != null && base.node.dockParentNode != null)
			{
				return base.node.dockParentNode.testGroups[0].testModules[0] as IScoreableObjective;
			}
			return null;
		}
	}

	public MissionScore(MENode node)
		: base(node)
	{
		if (globalScoreModules == null)
		{
			SetDefaultGlobalScoreModules();
		}
		rootNodeName = "SCOREMODULES";
		moduleNodeName = "SCOREMODULE";
	}

	public void AwardScore(Mission mission)
	{
		for (int i = 0; i < activeModules.Count; i++)
		{
			if (activeModules[i] is ScoreModule scoreModule)
			{
				mission.currentScore = scoreModule.AwardScore(mission.currentScore);
			}
		}
	}

	public string ScoreDescription()
	{
		string text = "";
		for (int i = 0; i < activeModules.Count; i++)
		{
			if (activeModules[i] is ScoreModule scoreModule)
			{
				text = text + Localizer.Format("#autoLOC_8005404") + " " + scoreModule.ScoreDescription();
			}
		}
		return text;
	}

	public string AwarededScoreDescription()
	{
		string text = "";
		for (int i = 0; i < activeModules.Count; i++)
		{
			if (activeModules[i] is ScoreModule scoreModule)
			{
				string text2 = scoreModule.AwarededScoreDescription();
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n" + text2;
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text.Substring(1);
		}
		return text;
	}

	public static void SetDefaultGlobalScoreModules()
	{
		globalScoreModules = new List<Type>();
		globalScoreModules.Add(typeof(ScoreModule_Completion));
		globalScoreModules.Add(typeof(ScoreModule_Modifier));
	}

	public static void AddGlobalScoreModule(Type scoreModule)
	{
		if (globalScoreModules == null)
		{
			SetDefaultGlobalScoreModules();
		}
		if (!scoreModule.IsSubclassOf(typeof(ScoreModule)))
		{
			Debug.LogWarning("[MissionScore]: The supplied type " + scoreModule.Name + " is not a sub class of the ScoreModule class.");
		}
		else if (globalScoreModules.Contains(scoreModule))
		{
			Debug.LogWarning("[MissionScore]: The supplied score module " + scoreModule.Name + " has been already defined as a global score module.");
		}
		else
		{
			globalScoreModules.Add(scoreModule);
		}
	}

	public override List<Type> GetSupportedTypes()
	{
		IScoreableObjective scoreableObjective = this.scoreableObjective;
		if (scoreableObjective != null)
		{
			supportedTypes = new List<Type>();
			supportedTypes.AddRange(globalScoreModules);
			MEScoreModule[] array = (MEScoreModule[])scoreableObjective.GetType().GetCustomAttributes(typeof(MEScoreModule), inherit: false);
			if (array != null && array.Length != 0)
			{
				supportedTypes.AddRange(array[0].AllowedSystems);
			}
			return supportedTypes;
		}
		return globalScoreModules;
	}
}
