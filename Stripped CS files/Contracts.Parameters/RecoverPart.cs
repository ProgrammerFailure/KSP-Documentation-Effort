using System;
using System.Collections.Generic;
using Contracts.Templates;
using FinePrint.Utilities;

namespace Contracts.Parameters;

[Serializable]
public class RecoverPart : ContractParameter
{
	public enum CompleteCondition
	{
		None,
		AllCandidates,
		AnyCandidate
	}

	public List<uint> partsToRecover;

	public string title = "";

	public CompleteCondition failCondition;

	public CompleteCondition winCondition;

	public bool eventsAdded;

	public RecoverPart()
	{
		partsToRecover = new List<uint>();
		title = "Recover a part.";
		failCondition = CompleteCondition.AnyCandidate;
		winCondition = CompleteCondition.AllCandidates;
	}

	public RecoverPart(string title, CompleteCondition winCondition, CompleteCondition failCondition)
	{
		partsToRecover = new List<uint>();
		this.title = title;
		this.failCondition = failCondition;
		this.winCondition = winCondition;
	}

	public override string GetTitle()
	{
		return title;
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "Parameter.RecoverPart", "title", ref title, "Recover a part.");
		SystemUtilities.LoadNode(node, "Parameter.RecoverPart", "failOn", ref failCondition, CompleteCondition.AnyCandidate);
		SystemUtilities.LoadNode(node, "Parameter.RecoverPart", "winOn", ref winCondition, CompleteCondition.AnyCandidate);
		string[] values = node.GetValues("part");
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			partsToRecover.Add(uint.Parse(values[i]));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("title", title);
		node.AddValue("failOn", failCondition.ToString());
		node.AddValue("winOn", winCondition.ToString());
		int count = partsToRecover.Count;
		for (int i = 0; i < count; i++)
		{
			node.AddValue("part", partsToRecover[i]);
		}
	}

	public override void OnRegister()
	{
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onVesselRecovered.Add(OnVesselRecovered);
			GameEvents.onPartDie.Add(OnPartDie);
			GameEvents.onVesselLoaded.Add(OnVesselLoad);
			GameEvents.onFlightReady.Add(OnFlightReady);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onVesselRecovered.Remove(OnVesselRecovered);
			GameEvents.onPartDie.Remove(OnPartDie);
			GameEvents.onVesselLoaded.Remove(OnVesselLoad);
			GameEvents.onFlightReady.Remove(OnFlightReady);
		}
	}

	public void OnVesselRecovered(ProtoVessel v, bool quick)
	{
		if (winCondition == CompleteCondition.AnyCandidate)
		{
			int count = v.protoPartSnapshots.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (!partsToRecover.Contains(v.protoPartSnapshots[count].flightID));
			SetComplete();
		}
		else
		{
			if (winCondition != CompleteCondition.AllCandidates)
			{
				return;
			}
			int count2 = v.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				ProtoPartSnapshot protoPartSnapshot = v.protoPartSnapshots[count2];
				if (partsToRecover.Contains(protoPartSnapshot.flightID))
				{
					RemovePartToRecover(protoPartSnapshot.flightID);
					if (partsToRecover.Count <= 0)
					{
						SetComplete();
					}
				}
			}
		}
	}

	public void OnPartDie(Part p)
	{
		if (failCondition == CompleteCondition.AnyCandidate)
		{
			if (partsToRecover.Contains(p.flightID))
			{
				SetFailed();
			}
		}
		else if (failCondition == CompleteCondition.AllCandidates && partsToRecover.Contains(p.flightID))
		{
			RemovePartToRecover(p.flightID);
			if (partsToRecover.Count == 0)
			{
				SetFailed();
			}
		}
	}

	public void AddPartToRecover(uint partFlightId)
	{
		if (!partsToRecover.Contains(partFlightId))
		{
			partsToRecover.Add(partFlightId);
		}
	}

	public void RemovePartToRecover(uint partFlightId)
	{
		if (partsToRecover.Contains(partFlightId))
		{
			partsToRecover.Remove(partFlightId);
		}
	}

	public void OnVesselLoad(Vessel v)
	{
		if (base.Root is RecoverAsset recoverAsset)
		{
			recoverAsset.DiscoverAsset(v);
		}
	}

	public void OnFlightReady()
	{
		if (base.Root is RecoverAsset recoverAsset)
		{
			int count = FlightGlobals.VesselsLoaded.Count;
			while (count-- > 0)
			{
				recoverAsset.DiscoverAsset(FlightGlobals.VesselsLoaded[count]);
			}
		}
	}
}
