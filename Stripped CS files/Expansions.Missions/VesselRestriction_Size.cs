using Expansions.Missions.Editor;
using ns11;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class VesselRestriction_Size : VesselRestriction
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterOrEqual", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterEqual comparisonOperator = TestComparisonLessGreaterEqual.GreaterOrEqual;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100191")]
	public float tempInputY;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100192")]
	public float tempInputX;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, canBePinned = false, resetValue = "0", canBeReset = true, guiName = "#autoLOC_8100193")]
	public float tempInputZ;

	public Vector3 targetSize;

	public VesselRestriction_Size()
	{
		tempInputX = 0f;
		tempInputY = 0f;
		tempInputZ = 0f;
		targetSize = new Vector3(tempInputX, tempInputY, tempInputZ);
	}

	public VesselRestriction_Size(MENode node)
		: base(node)
	{
		tempInputX = 0f;
		tempInputY = 0f;
		tempInputZ = 0f;
		targetSize = new Vector3(tempInputX, tempInputY, tempInputZ);
	}

	public override void SuscribeToEvents()
	{
		GameEvents.onEditorPartPlaced.Add(Callback_PlacePart);
		GameEvents.onEditorPodPicked.Add(Callback_PlacePart);
		GameEvents.onEditorStarted.Add(UpdateAppEntry);
		GameEvents.onEditorLoad.Add(CallBack_ShipLoad);
	}

	public override void ClearEvents()
	{
		GameEvents.onEditorPartPlaced.Remove(Callback_PlacePart);
		GameEvents.onEditorPodPicked.Remove(Callback_PlacePart);
		GameEvents.onEditorStarted.Remove(UpdateAppEntry);
		GameEvents.onEditorLoad.Remove(CallBack_ShipLoad);
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100194");
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
			case "sizeZ":
				float.TryParse(value.value, out tempInputZ);
				break;
			case "sizeY":
				float.TryParse(value.value, out tempInputY);
				break;
			case "sizeX":
				float.TryParse(value.value, out tempInputX);
				break;
			}
		}
		targetSize = new Vector3(tempInputX, tempInputY, tempInputZ);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("sizeX", tempInputX);
		node.AddValue("sizeY", tempInputY);
		node.AddValue("sizeZ", tempInputZ);
		node.AddValue("comparisonOperator", comparisonOperator);
	}

	public override bool SameComparator(VesselRestriction otherRestriction)
	{
		return comparisonOperator == ((VesselRestriction_Size)otherRestriction).comparisonOperator;
	}

	public override bool IsComplete()
	{
		return Check(GetShipSize(), targetSize, comparisonOperator);
	}

	public bool Check(Vector3 currentSize, Vector3 targetSize, TestComparisonLessGreaterEqual comparisonOperator)
	{
		if (VesselRestriction.TestFloat(currentSize.x, targetSize.x, comparisonOperator) && VesselRestriction.TestFloat(currentSize.y, targetSize.y, comparisonOperator))
		{
			return VesselRestriction.TestFloat(currentSize.z, targetSize.z, comparisonOperator);
		}
		return false;
	}

	public Vector3 GetShipSize()
	{
		return ShipConstruction.CalculateCraftSize(EditorLogic.fetch.ship);
	}

	public override string GetStateMessage()
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => Localizer.Format("#autoLOC_8100196", GetShipDimensionsString(targetSize), GetShipDimensionsString(GetShipSize())), 
			TestComparisonLessGreaterEqual.Equal => Localizer.Format("#autoLOC_8100195", GetShipDimensionsString(targetSize), GetShipDimensionsString(GetShipSize())), 
			TestComparisonLessGreaterEqual.GreaterOrEqual => Localizer.Format("#autoLOC_8100197", GetShipDimensionsString(targetSize), GetShipDimensionsString(GetShipSize())), 
			_ => Localizer.Format("#autoLOC_8100173"), 
		};
	}

	public string GetShipDimensionsString(Vector3 dimensions)
	{
		string empty = string.Empty;
		empty = empty + "\t<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + Localizer.Format("#autoLOC_8100191") + ":</color> " + dimensions.y.ToString("N2") + Localizer.Format("#autoLOC_7001411") + "\n";
		empty = empty + "\t<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + Localizer.Format("#autoLOC_8100192") + ":</color> " + dimensions.x.ToString("N2") + Localizer.Format("#autoLOC_7001411") + "\n";
		return empty + "\t<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + Localizer.Format("#autoLOC_8100193") + ":</color> " + dimensions.z.ToString("N2") + Localizer.Format("#autoLOC_7001411") + "\n";
	}

	public void Callback_PlacePart(Part part)
	{
		UpdateAppEntry();
	}

	public void CallBack_ShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		UpdateAppEntry();
	}
}
