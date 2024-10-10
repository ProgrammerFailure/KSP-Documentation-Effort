using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions;

public class VesselRestriction_PartCount : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100176")]
	public int targetParts;

	public VesselRestriction_PartCount()
	{
	}

	public VesselRestriction_PartCount(MENode node)
		: base(node)
	{
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPodPicked.Add(Callback_PlacePart);
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorStarted.Add(UpdateAppEntry);
		GameEvents.onEditorLoad.Add(Callback_ShipLoad);
	}

	public override void ClearEvents()
	{
		GameEvents.onEditorPartPlaced.Remove(Callback_PlacePart);
		GameEvents.onEditorPodPicked.Remove(Callback_PlacePart);
		GameEvents.onEditorStarted.Remove(UpdateAppEntry);
		GameEvents.onEditorLoad.Remove(Callback_ShipLoad);
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100177");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string text = value.name;
			if (!(text == "numberOfParts"))
			{
				if (text == "comparisonOperator")
				{
					node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterEqual.GreaterOrEqual);
				}
			}
			else
			{
				int.TryParse(value.value, out targetParts);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("numberOfParts", targetParts);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_PartCount)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return VesselRestriction.TestInt(GetCurrentParts(), targetParts, comparisonOperator);
	}

	public int GetCurrentParts()
	{
		return EditorLogic.fetch.ship.parts.Count;
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100179", targetParts, GetCurrentParts()), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100178", targetParts, GetCurrentParts()), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100180", targetParts, GetCurrentParts()), 
			_ => Localizer.Format("#autoLOC_8100173"), 
		};
	}

	public void Callback_PlacePart(Part part)
	{
		UpdateAppEntry();
	}

	public void Callback_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		UpdateAppEntry();
	}
}
