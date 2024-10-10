using System;
using ns9;

namespace Contracts.Predicates;

[Serializable]
public class LaunchID : ContractPredicate
{
	public uint id;

	public double date = -1.0;

	public uint UInt32_0 => UInt32_0;

	public double Date => date;

	public LaunchID(IContractParameterHost parent)
		: base(parent)
	{
		parent.Root.OnStateChange.Add(OnContractStateChange);
	}

	~LaunchID()
	{
		if (base.Root != null)
		{
			base.Root.OnStateChange.Remove(OnContractStateChange);
		}
	}

	public void OnContractStateChange(Contract.State state)
	{
		if (state == Contract.State.Active)
		{
			id = HighLogic.CurrentGame.launchID;
			date = HighLogic.CurrentGame.UniversalTime;
		}
	}

	public override bool Test(Vessel vessel)
	{
		return vessel.launchTime >= date;
	}

	public override bool Test(ProtoVessel vessel)
	{
		return vessel.launchTime >= date;
	}

	public bool ContainsLaunchID_GreaterThanOrEqualTo(Vessel vessel, uint launchID)
	{
		int num = 0;
		while (true)
		{
			if (num < vessel.parts.Count)
			{
				if (vessel.parts[num].launchID >= launchID)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool ContainsLaunchID_GreaterThanOrEqualTo(ProtoVessel vessel, uint launchID)
	{
		int num = 0;
		while (true)
		{
			if (num < vessel.protoPartSnapshots.Count)
			{
				if (vessel.protoPartSnapshots[num].launchID >= launchID)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_271214");
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("launchID"))
		{
			id = uint.Parse(node.GetValue("launchID"));
		}
		if (node.HasValue("date"))
		{
			date = double.Parse(node.GetValue("date"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("launchID", id);
		node.AddValue("date", date);
	}
}
