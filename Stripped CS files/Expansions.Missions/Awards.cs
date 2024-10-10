using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Awards : MonoBehaviour
{
	[SerializeField]
	public List<AwardDefinition> internalAwards = new List<AwardDefinition>();

	public Dictionary<string, AwardDefinition> awardsDictionary;

	public List<AwardDefinition> AwardDefinitions { get; set; }

	public void Awake()
	{
		awardsDictionary = new Dictionary<string, AwardDefinition>();
		AwardDefinitions = new List<AwardDefinition>();
		SetupAwards();
	}

	public void SetupAwards()
	{
		AwardDefinitions.AddRange(internalAwards);
		int i = 0;
		for (int count = internalAwards.Count; i < count; i++)
		{
			awardsDictionary.Add(internalAwards[i].id, internalAwards[i]);
		}
	}

	public AwardDefinition GetAwardDefinition(string id)
	{
		if (awardsDictionary.ContainsKey(id))
		{
			return awardsDictionary[id];
		}
		return null;
	}
}
