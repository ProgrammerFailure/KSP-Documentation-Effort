using System;
using ns9;

namespace PreFlightTests;

[Obsolete("because NoControlSources handles all cases this doesn't")]
public class NoCrewInMannedVessel : DesignConcernBase, IPreFlightTest
{
	public VesselCrewManifest manifest;

	public bool updateManifest;

	[Obsolete("because NoControlSources handles all cases this doesn't")]
	public NoCrewInMannedVessel(VesselCrewManifest crewManifest)
	{
		manifest = crewManifest;
	}

	[Obsolete("because NoControlSources handles all cases this doesn't")]
	public NoCrewInMannedVessel()
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
		bool flag = false;
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
				if (!partCrewManifest.AnySeatTaken())
				{
					if (partCrewManifest.PartInfo.partPrefab.isControlSource > Vessel.ControlLevel.NONE)
					{
						if (partCrewManifest.PartInfo.partPrefab.CrewCapacity == 0)
						{
							break;
						}
						flag = true;
					}
					continue;
				}
				return true;
			}
			if (flag)
			{
				return false;
			}
			return true;
		}
		return true;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.CRITICAL;
	}

	public override string GetConcernTitle()
	{
		return Localizer.Format("#autoLOC_253586");
	}

	public override string GetConcernDescription()
	{
		return Localizer.Format("#autoLOC_253591");
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253598");
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_253603");
	}

	public string GetProceedOption()
	{
		return Localizer.Format("#autoLOC_253608");
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253613");
	}

	public string GetTestName()
	{
		return Localizer.Format("#autoLOC_253618");
	}
}
