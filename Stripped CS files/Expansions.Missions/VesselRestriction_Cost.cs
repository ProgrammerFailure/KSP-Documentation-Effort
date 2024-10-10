using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions;

public class VesselRestriction_Cost : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100136")]
	public float targetCost;

	public VesselRestriction_Cost()
	{
	}

	public VesselRestriction_Cost(MENode node)
		: base(node)
	{
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorPodPicked.Add(Callback_PlacePart);
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
		return "#autoLOC_8100136";
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100138", targetCost, GetShipCurrentCost()), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100137", targetCost, GetShipCurrentCost()), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100139", targetCost, GetShipCurrentCost()), 
			_ => string.Empty, 
		};
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_Cost)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return VesselRestriction.TestFloat(GetShipCurrentCost(), targetCost, comparisonOperator);
	}

	public float GetShipCurrentCost()
	{
		float dryCost;
		float fuelCost;
		return EditorLogic.fetch.ship.GetShipCosts(out dryCost, out fuelCost, ShipConstruction.ShipManifest);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string text = value.name;
			if (!(text == "cost"))
			{
				if (text == "comparisonOperator")
				{
					node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterEqual.GreaterOrEqual);
				}
			}
			else
			{
				float.TryParse(value.value, out targetCost);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("cost", targetCost);
		node.AddValue("comparisonOperator", comparisonOperator);
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
