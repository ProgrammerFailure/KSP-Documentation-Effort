using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns11;
using ns9;

namespace Expansions.Missions;

public class VesselRestriction_Resource : VesselRestriction
{
	[MEGUI_Dropdown(SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100185")]
	public float targetAmount;

	public VesselRestriction_Resource()
	{
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		enumerator.MoveNext();
		resourceName = enumerator.Current.name;
	}

	public VesselRestriction_Resource(MENode node)
		: base(node)
	{
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		enumerator.MoveNext();
		resourceName = enumerator.Current.name;
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
		return Localizer.Format("#autoLOC_8100186");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "comparisonOperator":
				node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonLessGreaterEqual.GreaterOrEqual);
				break;
			case "resourceName":
				resourceName = value.value;
				break;
			case "amount":
				float.TryParse(value.value, out targetAmount);
				break;
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("amount", targetAmount);
		node.AddValue("resourceName", resourceName);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_Resource)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return VesselRestriction.TestFloat((float)GetCurrentResourceValue(), targetAmount, comparisonOperator);
	}

	public double GetCurrentResourceValue()
	{
		double num = 0.0;
		List<Part> parts = EditorLogic.fetch.ship.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			PartResourceList resources = parts[i].Resources;
			for (int j = 0; j < resources.Count; j++)
			{
				if (resources[j].resourceName == resourceName)
				{
					num += resources[j].amount;
				}
			}
		}
		return num;
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100188", targetAmount.ToString("N1"), resourceName, GetCurrentResourceValue().ToString("N1")), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100187", targetAmount.ToString("N1"), resourceName, GetCurrentResourceValue().ToString("N1")), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100189", targetAmount.ToString("N1"), resourceName, GetCurrentResourceValue().ToString("N1")), 
			_ => string.Empty, 
		};
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

	public void Callback_ShipModified(ShipConstruct ship)
	{
		UpdateAppEntry();
	}

	public void Callback_CrewModified(VesselCrewManifest vcm)
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
}
