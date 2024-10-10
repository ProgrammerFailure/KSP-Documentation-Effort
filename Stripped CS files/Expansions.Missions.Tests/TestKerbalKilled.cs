using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Tests;

public class TestKerbalKilled : TestModule
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	public bool eventFound;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8007400");
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
		GameEvents.onCrewKilled.Add(OnKilled);
		if (missionKerbal.Kerbal != null)
		{
			ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[missionKerbal.Kerbal.name];
			if (protoCrewMember == null || (missionKerbal.TypeToShow == protoCrewMember.type && (protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Missing || protoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Dead)))
			{
				eventFound = true;
			}
		}
	}

	public override void Cleared()
	{
		GameEvents.onCrewKilled.Remove(OnKilled);
	}

	public void OnKilled(EventReport report)
	{
		if (missionKerbal.Kerbal == null)
		{
			if (HighLogic.CurrentGame.CrewRoster[report.sender].type == missionKerbal.TypeToShow)
			{
				eventFound = true;
			}
		}
		else if (report.sender == missionKerbal.Kerbal.name)
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
		return Localizer.Format("#autoLOC_8007401");
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
}
