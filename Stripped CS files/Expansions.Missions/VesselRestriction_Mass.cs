using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions;

public class VesselRestriction_Mass : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100168")]
	public float targetMass;

	public VesselRestriction_Mass()
	{
	}

	public VesselRestriction_Mass(MENode node)
		: base(node)
	{
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorShipModified.Add(Callback_ShipModified);
		GameEvents.onEditorShipCrewModified.Add(Callback_CrewModified);
		GameEvents.onEditorStarted.Add(UpdateAppEntry);
		GameEvents.onEditorLoad.Add(Callback_ShipLoad);
	}

	public override void ClearEvents()
	{
		GameEvents.onEditorPartPlaced.Remove(Callback_PlacePart);
		GameEvents.onEditorShipModified.Remove(Callback_ShipModified);
		GameEvents.onEditorShipCrewModified.Remove(Callback_CrewModified);
		GameEvents.onEditorStarted.Remove(UpdateAppEntry);
		GameEvents.onEditorLoad.Remove(Callback_ShipLoad);
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100169");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			string text = value.name;
			if (!(text == "mass"))
			{
				if (text == "comparisonOperator")
				{
					node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterEqual.GreaterOrEqual);
				}
			}
			else
			{
				float.TryParse(value.value, out targetMass);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("mass", targetMass);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_Mass)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return VesselRestriction.TestFloat(GetCurrentMass(), targetMass, comparisonOperator);
	}

	public float GetCurrentMass()
	{
		return EditorLogic.fetch.ship.GetTotalMass();
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100171", targetMass, GetCurrentMass()), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100170", targetMass, GetCurrentMass()), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100172", targetMass, GetCurrentMass()), 
			_ => Localizer.Format("#autoLOC_8100173"), 
		};
	}

	public void Callback_ShipModified(ShipConstruct ship)
	{
		UpdateAppEntry();
	}

	public void Callback_PlacePart(Part part)
	{
		UpdateAppEntry();
	}

	public void Callback_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		UpdateAppEntry();
	}

	public void Callback_CrewModified(VesselCrewManifest vcm)
	{
		UpdateAppEntry();
	}
}
