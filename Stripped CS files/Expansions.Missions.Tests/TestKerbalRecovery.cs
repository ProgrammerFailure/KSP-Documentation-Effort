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
public class TestKerbalRecovery : TestModule, IScoreableObjective
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	public bool eventFound;

	public List<ProtoCrewMember> pcm;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000150");
		missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
	}

	public override void Initialize(TestGroup testGroup, string title)
	{
		base.Initialize(testGroup, title);
		missionKerbal.Initialize(null, testGroup.node);
	}

	public override void Initialized()
	{
		eventFound = false;
		if (missionKerbal != null && missionKerbal.Kerbal != null)
		{
			checkAlreadyRecovered();
		}
		if (!eventFound)
		{
			GameEvents.onVesselRecovered.Add(OnRecovered);
		}
	}

	public override void Cleared()
	{
		GameEvents.onVesselRecovered.Remove(OnRecovered);
	}

	public void checkAlreadyRecovered()
	{
		if (HighLogic.CurrentGame != null)
		{
			ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[missionKerbal.Kerbal.name];
			if (protoCrewMember != null && protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available)
			{
				eventFound = true;
			}
		}
	}

	public void OnRecovered(ProtoVessel pv, bool enabled)
	{
		pcm = pv.GetVesselCrew();
		if (pcm.Count <= 0)
		{
			return;
		}
		if (missionKerbal != null && missionKerbal.Kerbal != null)
		{
			int count = pcm.Count;
			while (count-- > 0)
			{
				if (missionKerbal.IsValid(pcm[count]))
				{
					eventFound = true;
				}
			}
		}
		else
		{
			eventFound = true;
		}
	}

	public override bool Test()
	{
		return eventFound;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004018");
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
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (missionKerbal == null)
		{
			missionKerbal = new MissionKerbal(null, base.node, UpdateNodeBodyUI);
		}
		missionKerbal.Load(node);
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
