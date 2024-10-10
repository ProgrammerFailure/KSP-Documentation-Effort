using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using UnityEngine;

namespace FinePrint;

[KSPScenario((ScenarioCreationOptions)96, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER
})]
public class ScenarioContractEvents : ScenarioModule
{
	public const float eventDelay = 0.2f;

	public static int gameSeed = -1;

	public HashSet<Guid> specificVesselCache;

	public static int GameSeed => gameSeed;

	public override void OnAwake()
	{
		GameEvents.onVesselWasModified.Add(VesselWasModifiedEvent);
		GameEvents.onPartCouple.Add(PartCoupleEvent);
		GameEvents.onVesselWillDestroy.Add(VesselWillDestroyEvent);
		GameEvents.onVesselTerminated.Add(VesselTerminatedEvent);
		GameEvents.onVesselRecovered.Add(VesselRecoveredEvent);
		GameEvents.onVesselRename.Add(VesselRenameEvent);
		specificVesselCache = new HashSet<Guid>();
	}

	public void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(VesselWasModifiedEvent);
		GameEvents.onPartCouple.Remove(PartCoupleEvent);
		GameEvents.onVesselWillDestroy.Remove(VesselWillDestroyEvent);
		GameEvents.onVesselTerminated.Remove(VesselTerminatedEvent);
		GameEvents.onVesselRecovered.Remove(VesselRecoveredEvent);
		GameEvents.onVesselRename.Remove(VesselRenameEvent);
		if (IsInvoking())
		{
			CancelInvoke();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("id", gameSeed);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ScenarioContractEvents", "id", ref gameSeed, -1, logging: false);
		ProcessGameSeed();
	}

	public void ProcessGameSeed()
	{
		if (gameSeed == -1)
		{
			KSPRandom kSPRandom = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
			while (gameSeed == -1)
			{
				gameSeed = kSPRandom.Next(int.MinValue, int.MaxValue);
			}
		}
	}

	public void VesselWasModifiedEvent(Vessel v)
	{
		InvokeCheckSpecificVessels(v.id);
	}

	public void PartCoupleEvent(GameEvents.FromToAction<Part, Part> action)
	{
		InvokeCheckSpecificVessels(action.from.vessel.id, action.to.vessel.id);
	}

	public void VesselWillDestroyEvent(Vessel v)
	{
		InvokeCheckSpecificVessels(v.id);
	}

	public void VesselTerminatedEvent(ProtoVessel pv)
	{
		InvokeCheckSpecificVessels(pv.vesselID);
	}

	public void VesselRecoveredEvent(ProtoVessel pv, bool quick)
	{
		InvokeCheckSpecificVessels(pv.vesselID);
		StartCoroutine(TouristDepartures(0.2f, pv));
	}

	public void VesselRenameEvent(GameEvents.HostedFromToAction<Vessel, string> action)
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER || ContractSystem.Instance == null)
		{
			return;
		}
		int count = ContractSystem.Instance.Contracts.Count;
		while (count-- > 0)
		{
			Contract contract = ContractSystem.Instance.Contracts[count];
			if (!(contract is ICheckSpecificVessels))
			{
				continue;
			}
			IEnumerator<ContractParameter> enumerator = contract.AllParameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is SpecificVesselParameter specificVesselParameter)
				{
					specificVesselParameter.SpecificVesselRename(action.host.id, action.to);
				}
			}
		}
	}

	public IEnumerator TouristDepartures(float delay, ProtoVessel pv)
	{
		List<ProtoCrewMember> crew = pv.GetVesselCrew();
		yield return new WaitForSeconds(delay);
		int count = crew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = crew[count];
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Tourist && protoCrewMember.hasToured)
			{
				pv?.RemoveCrew(protoCrewMember);
				HighLogic.CurrentGame.CrewRoster.Remove(protoCrewMember);
			}
		}
	}

	public void InvokeCheckSpecificVessels(params Guid[] guids)
	{
		if (IsInvoking())
		{
			CancelInvoke();
		}
		int num = guids.Length;
		while (num-- > 0)
		{
			specificVesselCache.Add(guids[num]);
		}
		Invoke("CheckSpecificVessels", 0.2f);
	}

	public void CheckSpecificVessels()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && !(ContractSystem.Instance == null))
		{
			int count = ContractSystem.Instance.Contracts.Count;
			while (count-- > 0)
			{
				Contract contract = ContractSystem.Instance.Contracts[count];
				if (!(contract is ICheckSpecificVessels))
				{
					continue;
				}
				IEnumerator<ContractParameter> enumerator = contract.AllParameters.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is SpecificVesselParameter specificVesselParameter)
					{
						specificVesselParameter.SpecificVesselScan(specificVesselCache);
					}
				}
			}
			specificVesselCache.Clear();
		}
		else
		{
			specificVesselCache.Clear();
		}
	}
}
