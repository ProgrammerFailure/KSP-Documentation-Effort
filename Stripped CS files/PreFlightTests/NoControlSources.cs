using ns9;

namespace PreFlightTests;

public class NoControlSources : DesignConcernBase, IPreFlightTest
{
	public VesselCrewManifest manifest;

	public bool updateManifest;

	public KerbalRoster roster;

	public NoControlSources(VesselCrewManifest crewManifest)
	{
		manifest = crewManifest;
	}

	public NoControlSources(VesselCrewManifest crewManifest, KerbalRoster roster)
	{
		manifest = crewManifest;
		this.roster = roster;
	}

	public NoControlSources()
	{
		manifest = ShipConstruction.ShipManifest;
		updateManifest = true;
	}

	public override bool TestCondition()
	{
		if (updateManifest)
		{
			manifest = ShipConstruction.ShipManifest;
		}
		if (manifest == null)
		{
			return false;
		}
		int count = manifest.PartManifests.Count;
		while (true)
		{
			if (count-- > 0)
			{
				PartCrewManifest partCrewManifest = manifest.PartManifests[count];
				if (partCrewManifest.PartInfo.partPrefab.isControlSource > Vessel.ControlLevel.NONE)
				{
					ModuleCommand moduleCommand = partCrewManifest.PartInfo.partPrefab.FindModuleImplementing<ModuleCommand>();
					if (!(moduleCommand != null))
					{
						return true;
					}
					if (moduleCommand.minimumCrew <= partCrewManifest.CountCrewNotType(ProtoCrewMember.KerbalType.Tourist, (roster == null) ? HighLogic.CurrentGame.CrewRoster : roster))
					{
						return true;
					}
				}
				else if (!partCrewManifest.NoSeats() && partCrewManifest.CountCrewNotType(ProtoCrewMember.KerbalType.Tourist, (roster == null) ? HighLogic.CurrentGame.CrewRoster : roster) > 0 && partCrewManifest.PartInfo.partPrefab.FindModuleImplementing<KerbalSeat>() != null)
				{
					break;
				}
				continue;
			}
			return false;
		}
		return true;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.CRITICAL;
	}

	public override string GetConcernTitle()
	{
		return Localizer.Format("#autoLOC_253474");
	}

	public override string GetConcernDescription()
	{
		return Localizer.Format("#autoLOC_253479");
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253486");
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_253491");
	}

	public string GetProceedOption()
	{
		return Localizer.Format("#autoLOC_253496");
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253501");
	}

	public string GetTestName()
	{
		return "No Control Sources";
	}
}
