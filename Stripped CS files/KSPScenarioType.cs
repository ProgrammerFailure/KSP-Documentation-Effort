using System;
using System.Collections.Generic;
using Expansions;
using UnityEngine;

public class KSPScenarioType
{
	public Type ModuleType { get; set; }

	public KSPScenario ScenarioAttributes { get; set; }

	public KSPScenarioType(Type ModuleType, KSPScenario ScenarioAttributes)
	{
		this.ModuleType = ModuleType;
		this.ScenarioAttributes = ScenarioAttributes;
	}

	public static List<KSPScenarioType> GetAllScenarioTypesInAssemblies()
	{
		List<KSPScenarioType> scnTypes = new List<KSPScenarioType>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(ScenarioModule)) && !(t == typeof(ScenarioModule)))
			{
				KSPScenario[] array = (KSPScenario[])t.GetCustomAttributes(typeof(KSPScenario), inherit: true);
				if (array.Length != 0)
				{
					int count = scnTypes.Count;
					do
					{
						if (count-- <= 0)
						{
							if (t.Name == "DeployedScience")
							{
								if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
								{
									scnTypes.Add(new KSPScenarioType(t, array[0]));
								}
							}
							else
							{
								scnTypes.Add(new KSPScenarioType(t, array[0]));
							}
							return;
						}
					}
					while (!(scnTypes[count].ModuleType.Name == t.Name));
					Debug.LogError("[ScenarioTypes]: Loaded Scenario definitions already contains a scenario with name '" + t.Name + "'");
				}
			}
		});
		Debug.Log("[ScenarioTypes]: List Created " + scnTypes.Count + " scenario types loaded from " + AssemblyLoader.loadedAssemblies.Count + " loaded assemblies.");
		return scnTypes;
	}
}
