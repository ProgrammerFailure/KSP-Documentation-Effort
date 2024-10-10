using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class ScoreModule_Resource : ScoreModule
{
	[MEGUI_Dropdown(SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName = "LiquidFuel";

	[MEGUI_ScoreRangeList(ContentType = MEGUI_ScoreRangeList.RangeContentType.IntegerNumber, guiName = "#autoLOC_8004155")]
	public List<ScoreRange> scoreRanges;

	public ScoreModule_Resource()
	{
		scoreRanges = new List<ScoreRange>();
	}

	public ScoreModule_Resource(MENode node)
		: base(node)
	{
		scoreRanges = new List<ScoreRange>();
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100133");
	}

	public override float AwardScore(float currentScore)
	{
		Vessel vessel = base.scoreableObjective.GetScoreModifier(GetType()) as Vessel;
		if (vessel == null)
		{
			vessel = FlightGlobals.ActiveVessel;
		}
		if (vessel != null)
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (definition.id != -1)
			{
				vessel.GetConnectedResourceTotals(definition.id, out var amount, out var _);
				for (int i = 0; i < scoreRanges.Count; i++)
				{
					if (scoreRanges[i].isValueInRange(amount))
					{
						awardedScoreDescription = Localizer.Format("#autoLOC_8006058", amount.ToString("00"), scoreRanges[i].score);
						return currentScore += scoreRanges[i].score;
					}
				}
			}
		}
		return currentScore;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "scoreRanges")
		{
			return ScoreDescription();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string ScoreDescription()
	{
		string text = Localizer.Format("#autoLOC_8100157", GetDisplayName());
		int i = 0;
		for (int count = scoreRanges.Count; i < count; i++)
		{
			text += Localizer.Format("#autoLOC_8100160", scoreRanges[i].minRange, scoreRanges[i].maxRange, scoreRanges[i].score);
		}
		return text;
	}

	public List<MEGUIDropDownItem> SetResourceNames()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PartResourceDefinition current = enumerator.Current;
			list.Add(new MEGUIDropDownItem(current.name, current.name, current.displayName));
		}
		return list;
	}

	public override List<string> GetDefaultPinnedParameters()
	{
		return new List<string> { "resourceName", "scoreRanges" };
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreModule_Resource scoreModule_Resource))
		{
			return false;
		}
		if (base.name.Equals(scoreModule_Resource.name) && resourceName.Equals(scoreModule_Resource.resourceName))
		{
			return MissionCheckpointValidator.CompareObjectLists(scoreRanges, scoreModule_Resource.scoreRanges);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004157");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("resourceName", ref resourceName);
		if (node.HasNode("SCORERANGES"))
		{
			ConfigNode[] nodes = node.GetNode("SCORERANGES").GetNodes("SCORERANGE");
			foreach (ConfigNode configNode in nodes)
			{
				ScoreRange scoreRange = new ScoreRange();
				scoreRange.Load(configNode);
				scoreRanges.Add(scoreRange);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("resourceName", resourceName);
		ConfigNode configNode = node.AddNode("SCORERANGES");
		int i = 0;
		for (int count = scoreRanges.Count; i < count; i++)
		{
			scoreRanges[i].Save(configNode.AddNode("SCORERANGE"));
		}
	}
}
