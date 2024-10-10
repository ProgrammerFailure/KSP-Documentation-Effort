using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions;

public class VesselRestriction_Stages : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, maxValue = 20f, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100203")]
	public int targetStages;

	public VesselRestriction_Stages()
	{
	}

	public VesselRestriction_Stages(MENode node)
		: base(node)
	{
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorPartPicked.Add(Callback_PlacePart);
		GameEvents.StageManager.OnGUIStageSequenceModified.Add(Callback_ModifyStage);
		GameEvents.StageManager.OnGUIStageAdded.Add(Callback_AddStage);
		GameEvents.StageManager.OnGUIStageRemoved.Add(Callback_RemoveStage);
		GameEvents.onEditorStarted.Add(UpdateAppEntry);
	}

	public override void ClearEvents()
	{
		GameEvents.onEditorPartPlaced.Remove(Callback_PlacePart);
		GameEvents.onEditorPartPicked.Remove(Callback_PlacePart);
		GameEvents.StageManager.OnGUIStageSequenceModified.Remove(Callback_ModifyStage);
		GameEvents.StageManager.OnGUIStageAdded.Remove(Callback_AddStage);
		GameEvents.StageManager.OnGUIStageRemoved.Remove(Callback_RemoveStage);
		GameEvents.onEditorStarted.Remove(UpdateAppEntry);
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100204");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string text = value.name;
			if (!(text == "stages"))
			{
				if (text == "comparisonOperator")
				{
					node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterEqual.GreaterOrEqual);
				}
			}
			else
			{
				int.TryParse(value.value, out targetStages);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("stages", targetStages);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_Stages)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return VesselRestriction.TestInt(GetCurrentStageCount(), targetStages, comparisonOperator);
	}

	public int GetCurrentStageCount()
	{
		return StageManager.StageCount;
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100206", targetStages, GetCurrentStageCount()), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100205", targetStages, GetCurrentStageCount()), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100207", targetStages, GetCurrentStageCount()), 
			_ => string.Empty, 
		};
	}

	public void Callback_PlacePart(Part part)
	{
		UpdateAppEntry();
	}

	public void Callback_AddStage(int index)
	{
		UpdateAppEntry();
	}

	public void Callback_ModifyStage()
	{
		UpdateAppEntry();
	}

	public void Callback_RemoveStage(int index)
	{
		UpdateAppEntry();
	}
}
