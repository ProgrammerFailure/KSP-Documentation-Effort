using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestKerbalRescue : TestModule, IScoreableObjective
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = true, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8002025", Tooltip = "#autoLOC_8002026")]
	public bool mustBoardVessel;

	public bool eventFound;

	public List<ProtoCrewMember> pcm;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000154");
		missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		missionKerbal.Initialize(null, testGroup.node);
		mustBoardVessel = false;
	}

	public override void Initialized()
	{
		eventFound = false;
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
			for (int i = 0; i < vesselCrew.Count; i++)
			{
				if (missionKerbal.IsValid(vesselCrew[i]) && vessel.DiscoveryInfo.Level == DiscoveryLevels.Owned)
				{
					if (!mustBoardVessel || (mustBoardVessel && !vessel.isEVA))
					{
						eventFound = true;
					}
					break;
				}
			}
		}
		if (!eventFound)
		{
			if (mustBoardVessel)
			{
				GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
				GameEvents.onCrewTransferred.Add(OnCrewTransferred);
			}
			else
			{
				GameEvents.onKnowledgeChanged.Add(onKnowledgeChanged);
			}
		}
	}

	public override void Cleared()
	{
		GameEvents.onCrewBoardVessel.Remove(OnCrewBoardVessel);
		GameEvents.onCrewTransferred.Remove(OnCrewTransferred);
		GameEvents.onKnowledgeChanged.Remove(onKnowledgeChanged);
	}

	public void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> action)
	{
		CheckNewKerbal(action.from.vessel.vesselName);
	}

	public void OnCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> action)
	{
		CheckNewKerbal(action.host.name);
	}

	public void CheckNewKerbal(string rescuedName)
	{
		if (missionKerbal == null || missionKerbal.Kerbal == null || missionKerbal.Kerbal.name == rescuedName)
		{
			eventFound = true;
		}
	}

	public void onKnowledgeChanged(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		Vessel vessel = kChg.host as Vessel;
		if (!(vessel != null))
		{
			return;
		}
		List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
		for (int i = 0; i < vesselCrew.Count; i++)
		{
			if (vesselCrew[i].name == missionKerbal.Kerbal.name && kChg.to == DiscoveryLevels.Owned)
			{
				eventFound = true;
			}
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004019");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "missionKerbal")
		{
			return missionKerbal.GetNodeBodyParameterString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (missionKerbal.Kerbal != null && base.node.mission.situation.crewRoster[missionKerbal.Kerbal.name].rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
		{
			validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002033", missionKerbal.Name));
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		missionKerbal.Save(node);
		node.AddValue("mustBoardVessel", mustBoardVessel);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (missionKerbal == null)
		{
			missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		}
		missionKerbal.Load(node);
		node.TryGetValue("mustBoardVessel", ref mustBoardVessel);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			return FlightGlobals.ActiveVessel;
		}
		return null;
	}
}
