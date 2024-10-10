using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class KerbalSeatCollision : DesignConcernBase
{
	public List<Part> failedParts = new List<Part>();

	public override bool TestCondition()
	{
		VesselCrewManifest shipManifest = ShipConstruction.ShipManifest;
		if (shipManifest == null)
		{
			return true;
		}
		ShipConstruct ship = EditorLogic.fetch.ship;
		if (ship == null)
		{
			return true;
		}
		failedParts.Clear();
		int count = ship.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = ship[i];
			if (part.CrewCapacity <= 0)
			{
				continue;
			}
			PartCrewManifest partCrewManifest = shipManifest.GetPartCrewManifest(part.craftID);
			if (partCrewManifest != null && partCrewManifest.AnySeatTaken())
			{
				KerbalSeat kerbalSeat = part.FindModuleImplementing<KerbalSeat>();
				if (kerbalSeat != null && !kerbalSeat.EditorSeatBoundsAreClear())
				{
					failedParts.Add(part);
				}
			}
		}
		return failedParts.Count == 0;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}

	public override string GetConcernTitle()
	{
		return Localizer.Format("#autoLOC_8004252");
	}

	public override string GetConcernDescription()
	{
		return Localizer.Format("#autoLOC_8004253");
	}
}
