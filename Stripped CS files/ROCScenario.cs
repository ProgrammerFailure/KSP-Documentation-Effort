using System.Collections.Generic;
using Expansions;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)1066, new GameScenes[] { GameScenes.FLIGHT })]
public class ROCScenario : ScenarioModule
{
	public List<GClass0> currentROCs;

	public Dictionary<string, GClass0> destroyedROCs = new Dictionary<string, GClass0>();

	public static ROCScenario Instance { get; set; }

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(this);
			Instance = null;
		}
	}
}
