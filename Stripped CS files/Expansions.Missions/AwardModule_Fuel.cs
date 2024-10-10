using System.Collections.Generic;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_Fuel : AwardModule
{
	[MEGUI_Dropdown(SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014")]
	public string resourceName = "LiquidFuel";

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, guiName = "#autoLOC_8100141")]
	public double resourceAmount;

	public DictionaryValueList<Vessel, double> vesselResource;

	public double totalSpentResource;

	public PartResourceDefinition resourceDefinition;

	public AwardModule_Fuel(MENode node)
		: base(node)
	{
		vesselResource = new DictionaryValueList<Vessel, double>();
	}

	public AwardModule_Fuel(MENode node, AwardDefinition definition)
		: base(node, definition)
	{
		vesselResource = new DictionaryValueList<Vessel, double>();
	}

	public override bool EvaluateCondition(Mission mission)
	{
		int i = 0;
		for (int count = vesselResource.Count; i < count; i++)
		{
			Vessel v = vesselResource.KeyAt(i);
			totalSpentResource += vesselResource.At(i) - GetCurrentResourceFromVessel(v);
		}
		return totalSpentResource <= resourceAmount;
	}

	public override void StartTracking()
	{
		resourceDefinition = PartResourceLibrary.Instance.GetDefinition(resourceName);
		if (resourceDefinition.id != -1)
		{
			GameEvents.onNewVesselCreated.Add(OnVesselCreated);
			GameEvents.onVesselWillDestroy.Add(OnVesselWillDestroy);
		}
	}

	public override void StopTracking()
	{
		GameEvents.onNewVesselCreated.Remove(OnVesselCreated);
		GameEvents.onVesselWillDestroy.Remove(OnVesselWillDestroy);
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

	public double GetCurrentResourceFromVessel(Vessel v)
	{
		double amount = 0.0;
		double maxAmount = 0.0;
		if (resourceDefinition.id != -1)
		{
			v.GetConnectedResourceTotals(resourceDefinition.id, out amount, out maxAmount);
		}
		return amount;
	}

	public void OnVesselWillDestroy(Vessel data)
	{
		if (vesselResource.ContainsKey(data))
		{
			totalSpentResource += vesselResource[data] - GetCurrentResourceFromVessel(data);
			vesselResource.Remove(data);
		}
	}

	public void OnVesselCreated(Vessel data)
	{
		if (!vesselResource.ContainsKey(data))
		{
			vesselResource.Add(data, GetCurrentResourceFromVessel(data));
		}
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("resourceName", ref resourceName);
		node.TryGetValue("resourceAmount", ref resourceAmount);
		ConfigNode configNode = new ConfigNode();
		if (!node.TryGetNode("PROGRESS", ref configNode))
		{
			return;
		}
		configNode.TryGetValue("totalSpentResource", ref totalSpentResource);
		ConfigNode[] nodes = configNode.GetNodes("VESSELRESOURCE");
		uint value = 0u;
		double value2 = 0.0;
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			if (nodes[i].TryGetValue("vesselID", ref value) && FlightGlobals.FindVessel(value, out var vessel))
			{
				nodes[i].TryGetValue("currentResource", ref value2);
				vesselResource.Add(vessel, value2);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("resourceName", resourceName);
		node.AddValue("resourceAmount", resourceAmount);
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			return;
		}
		ConfigNode configNode = node.AddNode("PROGRESS");
		configNode.AddValue("totalSpentResource", totalSpentResource);
		int i = 0;
		for (int count = vesselResource.Count; i < count; i++)
		{
			Vessel vessel = vesselResource.KeyAt(i);
			if (vessel != null)
			{
				ConfigNode configNode2 = configNode.AddNode("VESSELRESOURCE");
				configNode2.AddValue("vesselID", vessel.persistentId);
				configNode2.AddValue("currentResource", vesselResource[vessel]);
			}
		}
	}
}
