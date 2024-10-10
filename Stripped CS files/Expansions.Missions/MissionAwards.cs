using System;
using System.Collections.Generic;
using System.Reflection;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using UnityEngine;

namespace Expansions.Missions;

public class MissionAwards : DynamicModuleList
{
	public List<string> awardedAwards;

	public MissionAwards(MENode node)
		: base(node)
	{
		rootNodeName = "AWARDMODULES";
		moduleNodeName = "AWARDMODULE";
		awardedAwards = new List<string>();
	}

	public void FillAwardsList()
	{
		Awards awards = (HighLogic.LoadedSceneIsMissionBuilder ? MissionEditorLogic.Instance.awardDefinitions : MissionSystem.awardDefinitions);
		if (!(awards != null))
		{
			return;
		}
		int i = 0;
		for (int count = awards.AwardDefinitions.Count; i < count; i++)
		{
			Type classByName = AssemblyLoader.GetClassByName(typeof(DynamicModule), awards.AwardDefinitions[i].name);
			if (classByName != null && !ContainsDefinition(awards.AwardDefinitions[i]))
			{
				DynamicModule item = (DynamicModule)Activator.CreateInstance(classByName, base.node, awards.AwardDefinitions[i]);
				activeModules.Add(item);
			}
		}
	}

	public bool ContainsDefinition(AwardDefinition definition)
	{
		int count = activeModules.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(activeModules[count] is AwardModule { Definition: not null } awardModule) || !(awardModule.Definition.id == definition.id));
		return true;
	}

	public override List<Type> GetSupportedTypes()
	{
		if (supportedTypes.Count == 0)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			int i = 0;
			for (int num = assemblies.Length; i < num; i++)
			{
				Type[] types = assemblies[i].GetTypes();
				int j = 0;
				for (int num2 = types.Length; j < num2; j++)
				{
					if (types[j].IsSubclassOf(typeof(AwardModule)))
					{
						supportedTypes.Add(types[j]);
					}
				}
			}
		}
		return supportedTypes;
	}

	public void EvaluateAwards(Mission mission)
	{
		StopTracking();
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			if (activeModules[i] is AwardModule { enabled: not false } awardModule)
			{
				awardModule.EvaluateAward(mission, ref awardedAwards);
			}
		}
	}

	public void StartTracking()
	{
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			if (activeModules[i] is AwardModule { enabled: not false } awardModule)
			{
				awardModule.StartTracking();
			}
		}
		Debug.Log("[MissionAwards]: Awards tracking started.");
	}

	public void StopTracking()
	{
		int i = 0;
		for (int count = activeModules.Count; i < count; i++)
		{
			if (activeModules[i] is AwardModule { enabled: not false } awardModule)
			{
				awardModule.StopTracking();
			}
		}
		Debug.Log("[MissionAwards]: Awards tracking stopped.");
	}
}
