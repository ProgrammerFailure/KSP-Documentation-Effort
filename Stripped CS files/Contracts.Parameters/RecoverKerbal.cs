using System;
using System.Collections.Generic;
using Contracts.Templates;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class RecoverKerbal : ContractParameter
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
	public CompleteCondition failCondition;

	[SerializeField]
	public CompleteCondition winCondition;

	public bool eventsAdded;

	public RecoverKerbal()
	{
	}

	public RecoverKerbal(string title, CompleteCondition winCondition = CompleteCondition.All, CompleteCondition failCondition = CompleteCondition.All)
	{
		this.title = title;
		kerbalsToRecover = new List<string>();
		this.failCondition = failCondition;
		this.winCondition = winCondition;
	}

	public override string GetTitle()
	{
		return title;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
		if (node.HasValue("failOn"))
		{
			failCondition = (CompleteCondition)Enum.Parse(typeof(CompleteCondition), node.GetValue("failOn"));
		}
		if (node.HasValue("winOn"))
		{
			winCondition = (CompleteCondition)Enum.Parse(typeof(CompleteCondition), node.GetValue("winOn"));
		}
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
		node.AddValue("failOn", failCondition.ToString());
		node.AddValue("winOn", winCondition.ToString());
		int i = 0;
		for (int count = kerbalsToRecover.Count; i < count; i++)
		{
			node.AddValue("kerbal", kerbalsToRecover[i]);
		}
	}

	public override void OnRegister()
	{
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onVesselRecovered.Add(OnVesselRecovered);
			GameEvents.onCrewKilled.Add(OnCrewKilled);
			GameEvents.onPartDie.Add(OnPartDied);
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
			GameEvents.onCrewKilled.Remove(OnCrewKilled);
			GameEvents.onPartDie.Remove(OnPartDied);
			GameEvents.onVesselLoaded.Remove(OnVesselLoad);
			GameEvents.onFlightReady.Remove(OnFlightReady);
		}
	}

	public void OnVesselRecovered(ProtoVessel v, bool quick)
	{
		List<ProtoCrewMember> vesselCrew;
		switch (winCondition)
		{
		case CompleteCondition.All:
		{
			vesselCrew = v.GetVesselCrew();
			int count = vesselCrew.Count;
			while (count-- > 0)
			{
				int count2 = kerbalsToRecover.Count;
				while (count2-- > 0)
				{
					if (vesselCrew[count].name == kerbalsToRecover[count2])
					{
						kerbalsToRecover.RemoveAt(count2);
					}
				}
				if (kerbalsToRecover.Count == 0)
				{
					SetComplete();
				}
			}
			return;
		}
		}
		vesselCrew = v.GetVesselCrew();
		int count3 = vesselCrew.Count;
		while (count3-- > 0)
		{
			bool flag = false;
			int count4 = kerbalsToRecover.Count;
			while (count4-- > 0)
			{
				if (vesselCrew[count3].name == kerbalsToRecover[count4])
				{
					flag = true;
					SetComplete();
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
	}

	public void OnCrewKilled(EventReport evt)
	{
		switch (failCondition)
		{
		case CompleteCondition.Any:
		{
			int count2 = kerbalsToRecover.Count;
			do
			{
				if (count2-- <= 0)
				{
					return;
				}
			}
			while (!(evt.sender == kerbalsToRecover[count2]));
			SetFailed();
			break;
		}
		case CompleteCondition.All:
		{
			int count = kerbalsToRecover.Count;
			while (true)
			{
				if (count-- <= 0)
				{
					return;
				}
				if (evt.sender == kerbalsToRecover[count])
				{
					kerbalsToRecover.RemoveAt(count);
					if (kerbalsToRecover.Count == 0)
					{
						break;
					}
				}
			}
			SetFailed();
			break;
		}
		}
	}

	public void OnPartDied(Part p)
	{
		int count = p.protoModuleCrew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = p.protoModuleCrew[count];
			while (kerbalsToRecover.Contains(protoCrewMember.name))
			{
				if (failCondition != CompleteCondition.Any)
				{
					kerbalsToRecover.Remove(protoCrewMember.name);
					continue;
				}
				SetFailed();
				return;
			}
			if (failCondition == CompleteCondition.All && kerbalsToRecover.Count <= 0)
			{
				SetFailed();
			}
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

	public void SetTitle(string title)
	{
		this.title = title;
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
