using System.Collections.Generic;
using ns9;
using UnityEngine;

public class CrewTransfer : PartItemTransfer
{
	public class CrewTransferData
	{
		public bool canTransfer = true;

		public Part sourcePart;

		public Part destPart;

		public ProtoCrewMember crewMember;

		public CrewTransfer transfer;
	}

	public ProtoCrewMember crew;

	public Part tgtPart;

	public static CrewTransfer Create(Part srcPart, ProtoCrewMember crewMember, Callback<DismissAction, Part> onDialogDismiss)
	{
		CrewTransfer crewTransfer = new GameObject("Crew Transfer Host").AddComponent<CrewTransfer>();
		crewTransfer.crew = crewMember;
		crewTransfer.Setup(srcPart, "#autoLOC_6002372", crewMember.name, Localizer.Format("#autoLOC_111558"), onDialogDismiss);
		return crewTransfer;
	}

	public override void HookAdditionalEvents()
	{
		GameEvents.onCrewTransferred.Add(onGoForEva);
		GameEvents.onKerbalInactiveChange.Add(onChangeInactive);
	}

	public override void UnhookAdditionalEvents()
	{
		GameEvents.onCrewTransferred.Remove(onGoForEva);
		GameEvents.onKerbalInactiveChange.Remove(onChangeInactive);
	}

	public override bool IsSemiValidPart(Part p)
	{
		if (p.CrewCapacity > 0 && p.protoModuleCrew.Count >= p.CrewCapacity)
		{
			return p.crewTransferAvailable;
		}
		return false;
	}

	public override bool IsValidPart(Part p)
	{
		if (p.CrewCapacity > 0 && p.protoModuleCrew.Count < p.CrewCapacity)
		{
			return p.crewTransferAvailable;
		}
		return false;
	}

	public override void AfterPartsFound()
	{
		GameEvents.onCrewTransferPartListCreated.Fire(new GameEvents.HostedFromToAction<Part, List<Part>>(srcPart, validParts, semiValidParts));
	}

	public override void OnPartSelect(Part p)
	{
		CrewTransferData crewTransferData = new CrewTransferData();
		crewTransferData.canTransfer = true;
		crewTransferData.crewMember = crew;
		crewTransferData.sourcePart = srcPart;
		crewTransferData.destPart = p;
		crewTransferData.transfer = this;
		GameEvents.onCrewTransferSelected.Fire(crewTransferData);
		if (crewTransferData.canTransfer)
		{
			MoveCrewTo(p);
		}
	}

	public void onGoForEva(GameEvents.HostedFromToAction<ProtoCrewMember, Part> fromto)
	{
		if (fromto.host == crew && fromto.to.GetComponent<KerbalEVA>() != null)
		{
			DismissInterrupted();
		}
	}

	public void onChangeInactive(ProtoCrewMember pcm, bool from, bool to)
	{
		if (pcm == crew && to)
		{
			DismissInterrupted();
		}
	}

	public virtual void MoveCrewTo(Part p)
	{
		srcPart.RemoveCrewmember(crew);
		p.AddCrewmember(crew);
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_111636", crew.name, p.partInfo.title), scMsgWarning);
		GameEvents.onCrewTransferred.Fire(new GameEvents.HostedFromToAction<ProtoCrewMember, Part>(crew, srcPart, p));
		Vessel.CrewWasModified(srcPart.vessel, p.vessel);
		tgtPart = p;
		FlightGlobals.ActiveVessel.DespawnCrew();
		StartCoroutine(CallbackUtil.DelayedCallback(1, waitAndCompleteTransfer));
	}

	public virtual void waitAndCompleteTransfer()
	{
		FlightGlobals.ActiveVessel.SpawnCrew();
		Dismiss(DismissAction.ItemMoved, tgtPart);
	}
}
