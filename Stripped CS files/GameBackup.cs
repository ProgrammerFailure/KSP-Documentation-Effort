using System;

public class GameBackup
{
	public double UniversalTime { get; set; }

	public ConfigNode Config { get; set; }

	public int ActiveVessel { get; set; }

	public Guid ActiveVesselID { get; set; }

	public GameBackup(Game game)
	{
		UniversalTime = game.UniversalTime;
		ActiveVessel = game.flightState.activeVesselIdx;
		if (ActiveVessel >= 0 && ActiveVessel < game.flightState.protoVessels.Count)
		{
			ActiveVesselID = game.flightState.protoVessels[game.flightState.activeVesselIdx].vesselID;
		}
		else
		{
			ActiveVessel = 0;
			ActiveVesselID = Guid.Empty;
		}
		Config = new ConfigNode();
		game.Save(Config);
	}
}
