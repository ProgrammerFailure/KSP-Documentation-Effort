using System;
using System.Collections.Generic;
using FinePrint.Utilities;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class AcquireCrew : ContractParameter
{
	public enum CompleteCondition
	{
		All,
		Any
	}

	[SerializeField]
	public List<string> kerbalsToRecover = new List<string>();

	[SerializeField]
	public string title = "";

	[SerializeField]
	public CompleteCondition winCondition;

	public bool eventsAdded;

	public AcquireCrew()
	{
	}

	public AcquireCrew(string title, CompleteCondition winCondition = CompleteCondition.All)
	{
		this.title = title;
		kerbalsToRecover = new List<string>();
		this.winCondition = winCondition;
	}

	public void SetTitle(string title)
	{
		this.title = title;
	}

	public override string GetTitle()
	{
		return title;
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "Parameter.AcquireCrew", "title", ref title, "");
		SystemUtilities.LoadNode(node, "Parameter.AcquireCrew", "winOn", ref winCondition, CompleteCondition.Any);
		string[] values = node.GetValues("kerbal");
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			kerbalsToRecover.Add(values[i]);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("title", title);
		node.AddValue("winOn", (int)winCondition);
		int count = kerbalsToRecover.Count;
		for (int i = 0; i < count; i++)
		{
			node.AddValue("kerbal", kerbalsToRecover[i]);
		}
	}

	public override void OnRegister()
	{
		eventsAdded = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
			GameEvents.onCrewTransferred.Add(OnCrewTransferred);
			GameEvents.onPartCouple.Add(OnCouple);
			GameEvents.onCrewKilled.Add(OnCrewKilled);
			GameEvents.onPartDie.Add(OnPartDied);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onCrewBoardVessel.Remove(OnCrewBoardVessel);
			GameEvents.onCrewTransferred.Remove(OnCrewTransferred);
			GameEvents.onPartCouple.Remove(OnCouple);
			GameEvents.onCrewKilled.Remove(OnCrewKilled);
			GameEvents.onPartDie.Remove(OnPartDied);
		}
	}

	public override void OnReset()
	{
		if (SystemUtilities.FlightIsReady(checkVessel: true))
		{
			ScanVessel(FlightGlobals.ActiveVessel);
		}
	}

	public void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> action)
	{
		CheckNewKerbal(action.from.vessel.vesselName);
	}

	public void OnCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> action)
	{
		CheckNewKerbal(action.host.name);
	}

	public void OnCouple(GameEvents.FromToAction<Part, Part> action)
	{
		ScanVessel(action.from.vessel);
	}

	public void OnCrewKilled(EventReport evt)
	{
		KillKerbal(evt.sender);
	}

	public void OnPartDied(Part p)
	{
		int count = p.protoModuleCrew.Count;
		while (count-- > 0)
		{
			KillKerbal(p.protoModuleCrew[count].name);
		}
	}

	public void AddKerbal(string kerbalName)
	{
		if (!kerbalsToRecover.Contains(kerbalName))
		{
			kerbalsToRecover.Add(kerbalName);
		}
	}

	public void RemoveKerbal(string kerbalName)
	{
		if (kerbalsToRecover.Contains(kerbalName))
		{
			kerbalsToRecover.Remove(kerbalName);
		}
	}

	public void ScanVessel(Vessel v)
	{
		int count = v.parts.Count;
		while (count-- > 0)
		{
			Part part = v.parts[count];
			int count2 = part.protoModuleCrew.Count;
			while (count2-- > 0)
			{
				CheckNewKerbal(part.protoModuleCrew[count2].name);
			}
		}
	}

	public void CheckNewKerbal(string kerbalName)
	{
		if (kerbalsToRecover.Contains(kerbalName))
		{
			kerbalsToRecover.RemoveAll((string name) => name == kerbalName);
			if (winCondition == CompleteCondition.Any)
			{
				SetComplete();
			}
			else if (winCondition == CompleteCondition.All && kerbalsToRecover.Count <= 0)
			{
				SetComplete();
			}
		}
	}

	public void KillKerbal(string kerbalName)
	{
		if (kerbalsToRecover.Contains(kerbalName))
		{
			kerbalsToRecover.RemoveAll((string name) => name == kerbalName);
			if (winCondition == CompleteCondition.All)
			{
				SetFailed();
			}
			else if (winCondition == CompleteCondition.Any && kerbalsToRecover.Count <= 0)
			{
				SetFailed();
			}
		}
	}
}
