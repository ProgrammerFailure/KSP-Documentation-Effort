using System.ComponentModel;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

public class TestVesselActiveCount : TestModule
{
	public enum VesselType
	{
		[Description("#autoLOC_900712")]
		All,
		[Description("#autoLOC_8001049")]
		PlayerControlled,
		[Description("#autoLOC_308682")]
		Asteroids,
		[Description("#autoLOC_6006050")]
		Comets,
		[Description("#autoLOC_901070")]
		Unowned
	}

	[MEGUI_Dropdown(canBePinned = false, resetValue = "All", guiName = "#autoLOC_8001042", Tooltip = "#autoLOC_8001043")]
	public VesselType vesselType;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonOperator comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, resetValue = "0", guiName = "#autoLOC_8001044", Tooltip = "#autoLOC_8001045")]
	public int vesselCount;

	public int countCompare;

	public string operatorString;

	public override void Awake()
	{
		base.Awake();
		title = "#autoLOC_8001038";
	}

	public override void Initialized()
	{
		base.Initialized();
		countCompare = 0;
		GameEvents.onFlightGlobalsAddVessel.Add(OnAddVessel);
		GameEvents.onFlightGlobalsRemoveVessel.Add(OnRemoveVessel);
		int i = 0;
		for (int count = FlightGlobals.Vessels.Count; i < count; i++)
		{
			UpdateVesselCount(FlightGlobals.Vessels[i], 1);
		}
	}

	public override void Cleared()
	{
		base.Cleared();
		GameEvents.onFlightGlobalsAddVessel.Add(OnAddVessel);
		GameEvents.onFlightGlobalsRemoveVessel.Add(OnRemoveVessel);
	}

	public void OnAddVessel(Vessel v)
	{
		UpdateVesselCount(v, 1);
	}

	public void OnRemoveVessel(Vessel v)
	{
		UpdateVesselCount(v, -1);
	}

	public void UpdateVesselCount(Vessel v, int value)
	{
		if (v.vesselType == global::VesselType.const_11 || v.vesselType == global::VesselType.Flag || v.vesselType == global::VesselType.DeployedSciencePart || v.vesselType == global::VesselType.DeployedScienceController)
		{
			return;
		}
		switch (vesselType)
		{
		case VesselType.All:
			countCompare += value;
			break;
		case VesselType.PlayerControlled:
			if (v.DiscoveryInfo.Level == DiscoveryLevels.Owned)
			{
				countCompare += value;
			}
			break;
		case VesselType.Asteroids:
			if (v.FindPartModuleImplementing<ModuleAsteroid>() != null)
			{
				countCompare += value;
			}
			break;
		case VesselType.Comets:
			if (v.FindPartModuleImplementing<ModuleComet>() != null)
			{
				countCompare += value;
			}
			break;
		case VesselType.Unowned:
			if (v.DiscoveryInfo.Level == DiscoveryLevels.Unowned)
			{
				countCompare += value;
			}
			break;
		}
	}

	public override bool Test()
	{
		base.Test();
		return comparisonOperator switch
		{
			TestComparisonOperator.LessThan => countCompare < vesselCount, 
			TestComparisonOperator.LessThanorEqual => countCompare <= vesselCount, 
			TestComparisonOperator.Equal => countCompare == vesselCount, 
			TestComparisonOperator.GreaterThanorEqual => countCompare >= vesselCount, 
			TestComparisonOperator.GreaterThan => countCompare > vesselCount, 
			_ => false, 
		};
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "vesselCount")
		{
			operatorString = "";
			switch (comparisonOperator)
			{
			case TestComparisonOperator.LessThan:
				operatorString = "<";
				break;
			case TestComparisonOperator.LessThanorEqual:
				operatorString = "<=";
				break;
			case TestComparisonOperator.Equal:
				operatorString = "=";
				break;
			case TestComparisonOperator.GreaterThanorEqual:
				operatorString = ">=";
				break;
			case TestComparisonOperator.GreaterThan:
				operatorString = ">";
				break;
			}
			return Localizer.Format("#autoLOC_8100154", field.guiName, operatorString, vesselCount.ToString("0"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8001039");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("vesselType", vesselType);
		node.AddValue("comparisonOperator", comparisonOperator);
		node.AddValue("vesselCount", vesselCount);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("vesselType", ref vesselType, VesselType.All);
		node.TryGetEnum("comparisonOperator", ref comparisonOperator, TestComparisonOperator.GreaterThan);
		node.TryGetValue("vesselCount", ref vesselCount);
	}
}
