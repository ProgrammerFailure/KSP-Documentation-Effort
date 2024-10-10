using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class LaunchSiteClear : IPreFlightTest
{
	public string siteName;

	public string siteDisplayName;

	public string obstructingVesselName;

	public int obstructingVesselIndex;

	public Game st;

	public bool stValid;

	public LaunchSiteClear(string SiteName, string SiteDisplayName)
	{
		siteName = SiteName;
		siteDisplayName = Localizer.Format(SiteDisplayName);
	}

	public LaunchSiteClear(string SiteName, string SiteDisplayName, Game gameState)
	{
		siteName = SiteName;
		siteDisplayName = Localizer.Format(SiteDisplayName);
		st = gameState;
	}

	public bool Test()
	{
		stValid = false;
		Game game = st;
		if (game == null)
		{
			game = HighLogic.CurrentGame;
		}
		if (game == null)
		{
			game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, nullIfIncompatible: true, suppressIncompatibleMessage: true);
		}
		if (game == null)
		{
			return true;
		}
		if (game.flightState == null)
		{
			return true;
		}
		if (!game.compatible)
		{
			return true;
		}
		stValid = true;
		ShipConstruction.FindVesselsLandedAt(game.flightState, siteName, out var count, out obstructingVesselName, out obstructingVesselIndex, out var vType);
		if (count == 0)
		{
			return true;
		}
		if (vType == VesselType.Debris)
		{
			return true;
		}
		return false;
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253369", siteDisplayName);
	}

	public string GetWarningDescription()
	{
		return Localizer.Format("#autoLOC_253374", obstructingVesselName, siteDisplayName);
	}

	public string GetProceedOption()
	{
		return Localizer.Format("#autoLOC_253379", siteDisplayName, obstructingVesselName);
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253384");
	}

	public string GetObstructingVesselName()
	{
		return obstructingVesselName;
	}

	public int GetObstructingVesselIndex()
	{
		return obstructingVesselIndex;
	}

	public List<ProtoVessel> GetObstructingVessels()
	{
		Game game = st;
		if (game == null)
		{
			game = HighLogic.CurrentGame;
		}
		if (game == null)
		{
			game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, nullIfIncompatible: true, suppressIncompatibleMessage: true);
		}
		if (stValid && game != null && game.flightState != null)
		{
			return ShipConstruction.FindVesselsLandedAt(game.flightState, siteName);
		}
		return new List<ProtoVessel>();
	}

	public Game GetGameState()
	{
		return st;
	}
}
