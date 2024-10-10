using System.Collections.Generic;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_VesselsUsed : AwardModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, guiName = "#autoLOC_8100023")]
	public int vesselsUsed;

	public List<uint> vesselsUsedList;

	public AwardModule_VesselsUsed(MENode node)
		: base(node)
	{
		vesselsUsedList = new List<uint>();
	}

	public AwardModule_VesselsUsed(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
		vesselsUsedList = new List<uint>();
	}

	public override bool EvaluateCondition(Mission mission)
	{
		return vesselsUsed == vesselsUsedList.Count;
	}

	public override void StartTracking()
	{
		GameEvents.onVesselChange.Add(OnVesselChange);
	}

	public override void StopTracking()
	{
		GameEvents.onVesselChange.Remove(OnVesselChange);
	}

	public void OnVesselChange(Vessel data)
	{
		vesselsUsedList.AddUnique(data.persistentId);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("vesselsUsed", ref vesselsUsed);
		ConfigNode configNode = new ConfigNode();
		if (node.TryGetNode("PROGRESS", ref configNode))
		{
			List<string> valuesList = configNode.GetValuesList("vesselUsedID");
			int i = 0;
			for (int count = valuesList.Count; i < count; i++)
			{
				vesselsUsedList.Add(uint.Parse(valuesList[i]));
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("vesselsUsed", vesselsUsed);
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			ConfigNode configNode = node.AddNode("PROGRESS");
			for (int i = 0; i < vesselsUsedList.Count; i++)
			{
				configNode.AddValue("vesselUsedID", vesselsUsedList[i]);
			}
		}
	}
}
