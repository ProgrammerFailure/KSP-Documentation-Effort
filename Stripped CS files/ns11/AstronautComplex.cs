using System;
using System.Collections.Generic;
using Expansions.Missions;
using ns2;
using ns9;
using TMPro;
using UnityEngine;

namespace ns11;

public class AstronautComplex : MonoBehaviour
{
	[SerializeField]
	public CrewListItem widgetApplicants;

	[SerializeField]
	public UIList scrollListApplicants;

	[SerializeField]
	public CrewListItem widgetEnlisted;

	[SerializeField]
	public UIList scrollListAvailable;

	[SerializeField]
	public UIList scrollListAssigned;

	[SerializeField]
	public UIList scrollListKia;

	[SerializeField]
	public TextMeshProUGUI activeCrewsCount;

	[SerializeField]
	public TextMeshProUGUI nextHireCostField;

	[SerializeField]
	public TextMeshProUGUI availableCrewsTabText;

	[SerializeField]
	public TextMeshProUGUI assignedCrewsTabText;

	[SerializeField]
	public TextMeshProUGUI lostCrewsTabText;

	public int activeCrews;

	public int crewLimit;

	public VesselCrewManifest assignmentDialogManifest;

	public UIList ScrollListApplicants => scrollListApplicants;

	public UIList ScrollListAvailable => scrollListAvailable;

	public UIList ScrollListAssigned => scrollListAssigned;

	public UIList ScrollListKia => scrollListKia;

	public void Awake()
	{
	}

	public void Start()
	{
		InitiateGUI();
	}

	public void OnDestroy()
	{
	}

	public void InitiateGUI()
	{
		HighLogic.CurrentGame.CrewRoster.Update(Planetarium.GetUniversalTime());
		CreateApplicantList();
		CreateAvailableList();
		CreateAssignedList();
		CreateKiaList();
		if (Funding.Instance == null)
		{
			nextHireCostField.transform.parent.gameObject.SetActive(value: false);
		}
		else if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && !HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().astronautHiresAreFree)
		{
			nextHireCostField.transform.parent.gameObject.SetActive(value: false);
		}
		StartCoroutine(CallbackUtil.DelayedCallback(1, UpdateCrewCounts));
	}

	public void CreateApplicantList()
	{
		scrollListApplicants.Clear(destroyElements: true);
		IEnumerator<ProtoCrewMember> enumerator = HighLogic.CurrentGame.CrewRoster.Applicants.GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddItem_Applicants(enumerator.Current);
		}
	}

	public void CreateAvailableList()
	{
		if (CrewAssignmentDialog.Instance != null)
		{
			assignmentDialogManifest = CrewAssignmentDialog.Instance.GetManifest();
		}
		scrollListAvailable.Clear(destroyElements: true);
		IEnumerator<ProtoCrewMember> enumerator = HighLogic.CurrentGame.CrewRoster.Kerbals(ProtoCrewMember.KerbalType.Crew, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddItem_Available(enumerator.Current);
		}
		enumerator = HighLogic.CurrentGame.CrewRoster.Kerbals(ProtoCrewMember.KerbalType.Tourist, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddItem_Available(enumerator.Current);
		}
	}

	public void CreateKiaList()
	{
		scrollListKia.Clear(destroyElements: true);
		IEnumerator<ProtoCrewMember> enumerator = HighLogic.CurrentGame.CrewRoster.Kerbals(ProtoCrewMember.KerbalType.Crew, ProtoCrewMember.RosterStatus.Dead, ProtoCrewMember.RosterStatus.Missing).GetEnumerator();
		while (enumerator.MoveNext())
		{
			AddItem_Kia(enumerator.Current);
		}
	}

	public void CreateAssignedList()
	{
		scrollListAssigned.Clear(destroyElements: true);
		if (HighLogic.CurrentGame.flightState == null)
		{
			return;
		}
		int count = HighLogic.CurrentGame.flightState.protoVessels.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoVessel protoVessel = HighLogic.CurrentGame.flightState.protoVessels[i];
			if (protoVessel.vesselType == VesselType.const_11)
			{
				ProtoCrewMember protoCrewMember = protoVessel.GetVesselCrew()[0];
				AddItem_Assigned(protoCrewMember.name, protoCrewMember.courage, protoCrewMember.stupidity, CrewListItem.KerbalTypes.const_2, Localizer.Format("#autoLOC_445419"), protoVessel.GetVesselCrew()[0]);
				continue;
			}
			int count2 = protoVessel.protoPartSnapshots.Count;
			for (int j = 0; j < count2; j++)
			{
				ProtoPartSnapshot protoPartSnapshot = protoVessel.protoPartSnapshots[j];
				int count3 = protoPartSnapshot.protoModuleCrew.Count;
				for (int k = 0; k < count3; k++)
				{
					ProtoCrewMember protoCrewMember2 = protoPartSnapshot.protoModuleCrew[k];
					string text = Localizer.Format("#autoLOC_445437");
					InternalModel internalPart = PartLoader.GetInternalPart(protoPartSnapshot.partInfo.internalConfig.GetValue("name"));
					if (internalPart != null && internalPart.seats != null && internalPart.seats.Count > protoCrewMember2.seatIdx && protoCrewMember2.seatIdx >= 0)
					{
						text = internalPart.seats[protoCrewMember2.seatIdx].displayseatName;
					}
					string label = Localizer.Format("#autoLOC_445441", protoVessel.GetDisplayName(), text, RUIutils.CutString(protoPartSnapshot.partInfo.title, 45, "..."));
					if (protoCrewMember2.veteran)
					{
						AddItem_Assigned(protoCrewMember2.name, protoCrewMember2.courage, protoCrewMember2.stupidity, CrewListItem.KerbalTypes.BADASS, label, protoCrewMember2);
					}
					else
					{
						AddItem_Assigned(protoCrewMember2.name, protoCrewMember2.courage, protoCrewMember2.stupidity, CrewListItem.KerbalTypes.AVAILABLE, label, protoCrewMember2);
					}
				}
			}
		}
	}

	public void AddItem_Applicants(ProtoCrewMember crew)
	{
		CrewListItem crewListItem = AddItem(scrollListApplicants, widgetApplicants);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetName(crew.name);
		crewListItem.SetButton(CrewListItem.ButtonTypes.const_1);
		crewListItem.AddButtonInputDelegate(Vbutton);
		crewListItem.SetXP(crew);
		crewListItem.SetStats(crew);
		crewListItem.SetLabel(Localizer.Format("#autoLOC_445466"));
		crewListItem.SetKerbal(crew, CrewListItem.KerbalTypes.RECRUIT);
		crewListItem.SetTooltip(crew);
	}

	public void AddItem_Available(ProtoCrewMember crew)
	{
		CrewListItem crewListItem = AddItem(scrollListAvailable, widgetEnlisted);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetName(crew.name);
		if (crew.type != ProtoCrewMember.KerbalType.Tourist)
		{
			if (assignmentDialogManifest != null && assignmentDialogManifest.Contains(crew))
			{
				crewListItem.SetLabel(Localizer.Format("#autoLOC_445493"));
				crewListItem.MouseoverEnabled = false;
			}
			else
			{
				crewListItem.SetButton(CrewListItem.ButtonTypes.const_0);
				crewListItem.AddButtonInputDelegate(Xbutton_AvailableCrew);
				crewListItem.SetLabel(Localizer.Format("#autoLOC_445500"));
			}
		}
		else
		{
			crewListItem.SetLabel(Localizer.Format("#autoLOC_445505"));
			crewListItem.MouseoverEnabled = false;
		}
		crewListItem.SetXP(crew);
		crewListItem.SetStats(crew);
		crewListItem.SetKerbalAsApplicableType(crew);
		crewListItem.SetTooltip(crew);
	}

	public void AddItem_Assigned(string name, float courage, float stupidity, CrewListItem.KerbalTypes type, string label, ProtoCrewMember crew)
	{
		CrewListItem crewListItem = AddItem(scrollListAssigned, widgetEnlisted);
		crewListItem.SetName(name);
		crewListItem.SetXP(crew);
		crewListItem.SetStats(crew);
		crewListItem.SetKerbal(crew, type);
		crewListItem.SetLabel(label);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetTooltip(crew);
		crewListItem.MouseoverEnabled = false;
	}

	public void AddItem_Kia(ProtoCrewMember crew)
	{
		CrewListItem crewListItem = AddItem(scrollListKia, widgetEnlisted);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetName(crew.name);
		crewListItem.SetXP(crew);
		crewListItem.SetStats(crew);
		if (crew.rosterStatus == ProtoCrewMember.RosterStatus.Dead)
		{
			crewListItem.SetLabel(Localizer.Format("#autoLOC_445546"));
			crewListItem.MouseoverEnabled = false;
		}
		else
		{
			crewListItem.SetLabel(Localizer.Format("#autoLOC_445551"));
			crewListItem.SetButton(CrewListItem.ButtonTypes.const_0);
			crewListItem.AddButtonInputDelegate(Xbutton_MIA);
			crewListItem.MouseoverEnabled = true;
		}
		crewListItem.SetKerbalAsApplicableType(crew);
		crewListItem.SetTooltip(crew);
	}

	public CrewListItem AddItem(UIList list, CrewListItem widget)
	{
		CrewListItem crewListItem = UnityEngine.Object.Instantiate(widget);
		list.AddItem(crewListItem.GetComponent<UIListItem>());
		return crewListItem;
	}

	public void Vbutton(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.CrewRecruited, 0f - GameVariables.Instance.GetRecruitHireCost(activeCrews), 0f, 0f);
		if ((HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().astronautHiresAreFree) || currencyModifierQuery.CanAfford(delegate(Currency c)
		{
			ScreenMessages.PostScreenMessage(StringBuilderCache.Format(Localizer.Format("#autoLOC_445572", c.displayDescription())), 6f, ScreenMessageStyle.UPPER_LEFT);
		}))
		{
			HireRecruit(scrollListApplicants, scrollListAvailable, clickItem.GetComponent<UIListItem>());
		}
	}

	public void Xbutton_AvailableCrew(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		scrollListAvailable.RemoveItem(clickItem.GetComponent<UIListItem>());
		scrollListAvailable.Controller.RefreshCurrent();
		HighLogic.CurrentGame.CrewRoster.SackAvailable(clickItem.GetCrewRef());
		CreateApplicantList();
		UpdateCrewCounts();
	}

	public void Xbutton_MIA(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		scrollListKia.RemoveItem(clickItem.GetComponent<UIListItem>());
		scrollListAvailable.Controller.RefreshCurrent();
		if (clickItem.GetCrewRef().rosterStatus == ProtoCrewMember.RosterStatus.Missing)
		{
			HighLogic.CurrentGame.CrewRoster.RemoveMIA(clickItem.GetCrewRef());
		}
		else if (clickItem.GetCrewRef().rosterStatus == ProtoCrewMember.RosterStatus.Dead)
		{
			HighLogic.CurrentGame.CrewRoster.RemoveDead(clickItem.GetCrewRef());
		}
		CreateApplicantList();
		UpdateCrewCounts();
	}

	public void HireRecruit(UIList fromlist, UIList tolist, UIListItem listItem)
	{
		if (fromlist == scrollListApplicants && activeCrews < crewLimit)
		{
			CrewListItem component = listItem.GetComponent<CrewListItem>();
			ProtoCrewMember crewRef = component.GetCrewRef();
			string text = component.GetName();
			fromlist.RemoveItem(listItem, deleteItem: true);
			CrewListItem crewListItem = AddItem(tolist, widgetEnlisted);
			crewListItem.SetCrewRef(crewRef);
			crewListItem.SetName(text);
			crewListItem.SetXP(crewRef);
			crewListItem.SetStats(crewRef);
			crewListItem.SetLabel(Localizer.Format("#autoLOC_445625"));
			crewListItem.SetButton(CrewListItem.ButtonTypes.const_0);
			crewListItem.AddButtonInputDelegate(Xbutton_AvailableCrew);
			crewListItem.SetTooltip(crewRef);
			tolist.Controller.RefreshCurrent();
			HighLogic.CurrentGame.CrewRoster.HireApplicant(crewRef);
			crewListItem.SetKerbalAsApplicableType(crewRef);
			UpdateCrewCounts();
		}
	}

	public void UpdateCrewCounts()
	{
		activeCrews = HighLogic.CurrentGame.CrewRoster.GetActiveCrewCount();
		crewLimit = GameVariables.Instance.GetActiveCrewLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		if (activeCrews < crewLimit)
		{
			if (crewLimit < int.MaxValue)
			{
				activeCrewsCount.text = Localizer.Format("#autoLOC_445648", activeCrews.ToString(), crewLimit.ToString());
			}
			else
			{
				activeCrewsCount.text = Localizer.Format("#autoLOC_445652", activeCrews.ToString());
			}
			SetApplicantsListUnlocked(unlocked: true);
		}
		else
		{
			activeCrewsCount.text = Localizer.Format("#autoLOC_445658", XKCDColors.HexFormat.BrightOrange, activeCrews.ToString(), crewLimit.ToString());
			string text = Localizer.Format("#autoLOC_7001009");
			string text2 = Localizer.Format("#autoLOC_7001010");
			SetApplicantsListUnlocked(unlocked: false, "<color=" + XKCDColors.HexFormat.BrightOrange + ">" + text + "</color>", "<color=" + XKCDColors.HexFormat.BrightOrange + ">" + text2 + "</color>");
		}
		availableCrewsTabText.text = Localizer.Format("#autoLOC_445662", HighLogic.CurrentGame.CrewRoster.GetAvailableCrewCount());
		assignedCrewsTabText.text = Localizer.Format("#autoLOC_445663", HighLogic.CurrentGame.CrewRoster.GetAssignedCrewCount());
		assignedCrewsTabText.text = Localizer.Format("#autoLOC_445663", HighLogic.CurrentGame.CrewRoster.GetAssignedCrewCount());
		int num = (HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn ? HighLogic.CurrentGame.CrewRoster.GetMissingCrewCount() : HighLogic.CurrentGame.CrewRoster.GetKIACrewCount());
		lostCrewsTabText.text = Localizer.Format("#autoLOC_445665", num, Convert.ToInt32(HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn));
		UpdateCrewCosts();
	}

	public void UpdateCrewCosts()
	{
		string costLine = CurrencyModifierQuery.RunQuery(TransactionReasons.CrewRecruited, 0f - GameVariables.Instance.GetRecruitHireCost(activeCrews), 0f, 0f).GetCostLine();
		if (activeCrews >= crewLimit)
		{
			nextHireCostField.text = Localizer.Format("#autoLOC_445678", XKCDColors.HexFormat.KSPNeutralUIGrey, costLine);
		}
		else
		{
			nextHireCostField.text = Localizer.Format("#autoLOC_445682", costLine);
		}
	}

	public void SetApplicantsListUnlocked(bool unlocked, string lockReasonTitle = "", string lockReasonCaption = "")
	{
		int count = scrollListApplicants.Count;
		while (count-- > 0)
		{
			scrollListApplicants.GetUilistItemAt(count).GetComponent<CrewListItem>().SetButtonEnabled(unlocked, lockReasonTitle, lockReasonCaption);
		}
	}
}
