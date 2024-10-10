using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUIParameterVessel : MEGUICompoundParameter
{
	public GameObject vesselCameraSetup;

	public GAPUtil_VesselFrame vesselFramePrefab;

	public Mission mission;

	public List<VesselSituation> VesselList
	{
		get
		{
			if (mission != null)
			{
				return mission.GetAllVesselSituations();
			}
			return new List<VesselSituation>();
		}
	}

	public List<MissionMappedVessel> MappedVesselList
	{
		get
		{
			if (mission != null)
			{
				return mission.mappedVessels;
			}
			return new List<MissionMappedVessel>();
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		if (field.host is TestModule)
		{
			mission = (field.host as TestModule).node.mission;
		}
		else if (field.host is ActionModule)
		{
			mission = (field.host as ActionModule).node.mission;
		}
		else if (MissionEditorLogic.Instance != null)
		{
			mission = MissionEditorLogic.Instance.EditorMission;
		}
	}

	public virtual void OnChangePartSelection(Part part)
	{
	}

	public virtual void OnNextVessel()
	{
	}

	public virtual void OnPrevVessel()
	{
	}

	public void SetMission(Mission newMission)
	{
		mission = newMission;
	}
}
