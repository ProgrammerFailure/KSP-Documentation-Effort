using FinePrint;
using ns11;
using ns25;
using UnityEngine;

public class ConsoleCommands : MonoBehaviour
{
	public static bool stockCommandsAdded;

	public void Start()
	{
		AddStockCommands();
	}

	public void AddStockCommands()
	{
		if (!stockCommandsAdded)
		{
			DebugScreenConsole.AddConsoleCommand("help", OnHelpCommand, "Shows all commands available, and help information for each one.");
			DebugScreenConsole.AddConsoleCommand("time", OnTimeCommand, "Shows the current time in both real world and kerbal variants.");
			DebugScreenConsole.AddConsoleCommand("comment", OnCommentCommand, "Allows the player to brand his log with a comment that will be very visible to modders or developers reading it. Example: \"/comment KSP is great!\"");
			DebugScreenConsole.AddConsoleCommand("info", OnInfoCommand, "Dumps a great deal of information about the current state of the game, which is helpful for debugging.");
			DebugScreenConsole.AddConsoleCommand("stacktrace", OnStackTraceCommand, "Prints a stack trace for the last error or exception.");
			DebugScreenConsole.AddConsoleCommand("vessels", OnVesselsCommand, "Flexibly searches for vessels by name and displays their status. No name will display all vessels. Example: \"/vessels Mun Satellite Alpha\"");
			DebugScreenConsole.AddConsoleCommand("switch", OnSwitchCommand, "Switches current vessel to the first vessel found with the exact name given. Example: \"/switch Duna Outpost Delta\"");
			DebugScreenConsole.AddConsoleCommand("waypoint", OnWaypointCommand, "Adds a custom waypoint to the game. Pass a position, or pass nothing to tag current position in flight. Usage: \"/waypoint\" or \"/waypoint Kerbin 52.8 22.5 My Waypoint Name\"");
			DebugScreenConsole.AddConsoleCommand("ksc", OnKSCCommand, "Shortcut to add a custom waypoint to the space center. Usage: \"/ksc\"");
			DebugScreenConsole.AddConsoleCommand("funds", OnFundsCommand, "Sets the amount of funds available in your game. Example: \"/funds 1000000\"");
			DebugScreenConsole.AddConsoleCommand("science", OnScienceCommand, "Sets the amount of science available in your game. Example: \"/science 2500\"");
			DebugScreenConsole.AddConsoleCommand("reputation", OnReputationCommand, "Sets the amount of reputation available in your game. Example: \"/reputation 1000\"");
			DebugScreenConsole.AddConsoleCommand("rendezvous", OnRendezvousCommand, "Rendezvous with a vessel. Pass a vessel index, relative position and relative velocity. Usage: \"/rendezvous  vslidx posx posy posz velx vely velz\"");
			DebugScreenConsole.AddConsoleCommand("targetv", OnTargetvCommand, "Target a vessel. Pass a vessel index. Usage: \"/targetv  vslidx\"");
			DebugScreenConsole.AddConsoleCommand("b4d455", OnBadassCommand, "Makes all of your wildest dreams come true.");
			DebugScreenConsole.AddConsoleCommand("settingsload", OnSettingsLoadCommand, "Reload the game settings from the settings.cfg.");
			DebugScreenConsole.AddConsoleCommand("settingssave", OnSettingsSaveCommand, "Force a write of the settings.cfg.");
			stockCommandsAdded = true;
		}
	}

	public void OnHelpCommand(string arg)
	{
		string text = string.Empty;
		if (string.IsNullOrEmpty(arg))
		{
			Debug.Log("[DebugConsole]: Help is listing every available command, use \"/help command\" to learn more about each one...");
			int i = 0;
			for (int commandCount = DebugScreenConsole.CommandCount; i < commandCount; i++)
			{
				text = text + "/" + DebugScreenConsole.GetCommand(i).command;
				if (i != commandCount - 1)
				{
					text += " ";
				}
			}
			Debug.Log(text);
			return;
		}
		char c = arg[0];
		if (c == '/' || c == '\\')
		{
			arg = arg.Substring(1);
		}
		DebugScreenConsole.ConsoleCommand command = DebugScreenConsole.GetCommand(arg);
		if (command == null)
		{
			int j = 0;
			for (int commandCount2 = DebugScreenConsole.CommandCount; j < commandCount2; j++)
			{
				string command2 = DebugScreenConsole.GetCommand(j).command;
				if (command2.Contains(arg) || arg.Contains(command2))
				{
					text = text + "/" + command2;
					if (j != commandCount2 - 1)
					{
						text += " ";
					}
				}
			}
			if (text.Length > 0)
			{
				Debug.Log("[DebugConsole]: Help could not find command \"/" + arg + "\", but did find these similar commands...");
				Debug.Log(text);
			}
			else
			{
				Debug.Log("[DebugConsole]: Help could not find any commands similar to \"/" + arg + "\".");
			}
		}
		else
		{
			Debug.Log("[DebugConsole]: Help found command \"/" + arg + "\", displaying info...");
			Debug.Log(command.help);
		}
	}

	public void OnTimeCommand(string arg)
	{
		Debug.Log("[DebugConsole]: The current kerbal time is " + KSPUtil.PrintTimeStamp(Planetarium.GetUniversalTime()) + ". The current real world time is " + KSPUtil.SystemDateTime.DateTimeNow().ToString("T") + ".");
	}

	public void OnCommentCommand(string arg)
	{
		if (string.IsNullOrEmpty(arg))
		{
			Debug.Log("[DebugConsole]: Cannot print empty comment.");
		}
		Debug.Log("+++++++++++++++++++++ RUNTIME COMMENT +++++++++++++++++++++");
		Debug.Log("Time: " + KSPUtil.SystemDateTime.DateTimeNow().ToString("T"));
		Debug.Log("Comment: " + arg);
		Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
	}

	public void OnInfoCommand(string arg)
	{
		Debug.Log("[DebugConsole] Dumping current game state information...");
		if (AssemblyLoader.loadedAssemblies != null && AssemblyLoader.loadedAssemblies.Count > 0)
		{
			string text = string.Empty;
			int i = 0;
			for (int count = AssemblyLoader.loadedAssemblies.Count; i < count; i++)
			{
				text += AssemblyLoader.loadedAssemblies[i].name;
				if (i != count - 1)
				{
					text += ", ";
				}
			}
			Debug.Log("Assemblies: " + text);
		}
		if (HighLogic.CurrentGame != null)
		{
			Debug.Log("Save: " + HighLogic.CurrentGame.Title);
			Debug.Log("Mode: " + HighLogic.CurrentGame.Mode);
		}
		Debug.Log("Scene: " + HighLogic.LoadedScene);
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			Debug.Log("Vessel: " + activeVessel.GetDisplayName());
			Debug.Log("Body: " + activeVessel.mainBody.name);
			Debug.Log("Situation: " + activeVessel.situation);
			Debug.Log("Latitude: " + activeVessel.latitude);
			Debug.Log("Longitude: " + activeVessel.longitude);
			Debug.Log("Altitude: " + activeVessel.altitude);
		}
	}

	public void OnStackTraceCommand(string arg)
	{
		if (KSPLog.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot display stack trace as the log does not exist.");
			return;
		}
		Debug.Log("[DebugConsole]: Displaying a stack trace for the last error or exception...");
		Debug.Log(KSPLog.Instance.LastError);
		Debug.Log(KSPLog.Instance.LastStackTrace);
	}

	public void OnVesselsCommand(string arg)
	{
		if (FlightGlobals.fetch == null)
		{
			Debug.Log("[DebugConsole]: Cannot perform a vessel search, flight globals does not exist.");
			return;
		}
		if (string.IsNullOrEmpty(arg))
		{
			Debug.Log("[DebugConsole]: Displaying all current vessels.");
		}
		else
		{
			Debug.Log("[DebugConsole]: Searching for vessels with names matching \"" + arg + "\".");
		}
		int num = 0;
		int i = 0;
		for (int count = FlightGlobals.Vessels.Count; i < count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			string text = (vessel.loaded ? vessel.GetDisplayName() : vessel.protoVessel.GetDisplayName());
			if (string.IsNullOrEmpty(arg) || text.Contains(arg) || arg.Contains(text))
			{
				Debug.Log("(" + i + ") " + text + ": " + Vessel.GetSituationString(vessel));
				num++;
			}
		}
		if (num <= 0)
		{
			Debug.Log("[DebugConsole]: No vessels found.");
		}
	}

	public void OnSwitchCommand(string arg)
	{
		int num = 0;
		int count = FlightGlobals.Vessels.Count;
		Vessel vessel;
		while (true)
		{
			if (num < count)
			{
				vessel = FlightGlobals.Vessels[num];
				if ((vessel.loaded ? vessel.GetDisplayName() : vessel.protoVessel.GetDisplayName()) == arg)
				{
					break;
				}
				num++;
				continue;
			}
			Debug.Log("[DebugConsole]: Cannot switch to \"" + arg + "\" as the vessel could not be found.");
			return;
		}
		Debug.Log("[DebugConsole]: Vessel \"" + arg + "\" found. Switching to vessel.");
		if (!(FlightGlobals.fetch == null) && FlightGlobals.ready && HighLogic.LoadedSceneIsFlight)
		{
			FlightGlobals.ForceSetActiveVessel(vessel);
			return;
		}
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		FlightDriver.StartAndFocusVessel("persistent", FlightGlobals.Vessels.IndexOf(vessel));
	}

	public void OnWaypointCommand(string arg)
	{
		if (ScenarioCustomWaypoints.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot add waypoint, the custom waypoint scenario does not exist. Try from the flight or tracking station scenes.");
			return;
		}
		if (string.IsNullOrEmpty(arg))
		{
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			if (!HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready || activeVessel == null)
			{
				Debug.Log("[DebugConsole]: Cannot add waypoint at current position, as there is no active vessel loaded in a flight scene.");
				return;
			}
			arg = activeVessel.mainBody.name + " " + activeVessel.latitude + " " + activeVessel.longitude + " " + activeVessel.GetDisplayName() + " " + KSPUtil.PrintDateCompact(Planetarium.GetUniversalTime(), includeTime: true, includeSeconds: true);
		}
		string[] array = arg.Trim().Split(' ');
		if (array.Length < 4)
		{
			Debug.Log("[DebugConsole]: Not enough arguments to add a waypoint. Please see /help waypoint");
			return;
		}
		Waypoint waypoint = new Waypoint();
		waypoint.name = string.Empty;
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			switch (i)
			{
			default:
				waypoint.name += array[i];
				if (i < array.Length - 1)
				{
					waypoint.name += " ";
				}
				break;
			case 0:
			{
				waypoint.celestialName = array[i];
				bool flag = false;
				int num2 = FlightGlobals.Bodies.Count - 1;
				while (num2 >= 0)
				{
					CelestialBody celestialBody = FlightGlobals.Bodies[num2];
					if (!(celestialBody.name.ToLowerInvariant() == waypoint.celestialName.ToLowerInvariant()))
					{
						num2--;
						continue;
					}
					waypoint.celestialName = celestialBody.name;
					flag = true;
					break;
				}
				if (!flag)
				{
					Debug.Log("[DebugConsole]: Could not find celestial body \"" + waypoint.celestialName + "\" while adding waypoint.");
					return;
				}
				break;
			}
			case 1:
				if (!double.TryParse(array[i], out waypoint.latitude))
				{
					Debug.Log("[DebugConsole]: Error parsing latitude while adding waypoint.");
					return;
				}
				break;
			case 2:
				if (!double.TryParse(array[i], out waypoint.longitude))
				{
					Debug.Log("[DebugConsole]: Error parsing longitude while adding waypoint.");
					return;
				}
				break;
			}
		}
		ScenarioCustomWaypoints.AddWaypoint(waypoint);
		Debug.Log("[DebugConsole]: Added waypoint " + waypoint.name + ".");
	}

	public void OnKSCCommand(string arg)
	{
		if (SpaceCenter.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot add KSC waypoint, as KSC does not currently exist.");
			return;
		}
		string arg2 = FlightGlobals.GetHomeBodyName() + " " + SpaceCenter.Instance.Latitude + " " + SpaceCenter.Instance.Longitude + " Kerbal Space Center";
		OnWaypointCommand(arg2);
	}

	public void OnFundsCommand(string arg)
	{
		if (Funding.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot set funds, funding scenario not available.");
			return;
		}
		double result = 0.0;
		if (!double.TryParse(arg, out result))
		{
			Debug.Log("[DebugConsole]: Cannot set funds, amount not recognized.");
			return;
		}
		Funding.Instance.SetFunds(result, TransactionReasons.Cheating);
		if (RDController.Instance != null)
		{
			RDController.Instance.techTree.RefreshUI();
		}
		Debug.Log("[DebugConsole]: Funds set to " + result + ".");
	}

	public void OnScienceCommand(string arg)
	{
		if (ResearchAndDevelopment.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot set science, Research and Development scenario not available.");
			return;
		}
		float result = 0f;
		if (!float.TryParse(arg, out result))
		{
			Debug.Log("[DebugConsole]: Cannot set science, amount not recognized.");
			return;
		}
		ResearchAndDevelopment.Instance.SetScience(result, TransactionReasons.Cheating);
		if (RDController.Instance != null)
		{
			RDController.Instance.techTree.RefreshUI();
		}
		Debug.Log("[DebugConsole]: Science set to " + result + ".");
	}

	public void OnReputationCommand(string arg)
	{
		if (Reputation.Instance == null)
		{
			Debug.Log("[DebugConsole]: Cannot set reputation, reputation scenario not available.");
			return;
		}
		float result = 0f;
		if (!float.TryParse(arg, out result))
		{
			Debug.Log("[DebugConsole]: Cannot set reputation, amount not recognized.");
			return;
		}
		result = Mathf.Clamp(result, 0f - Reputation.RepRange, Reputation.RepRange);
		Reputation.Instance.SetReputation(result, TransactionReasons.Cheating);
		Debug.Log("[DebugConsole]: Reputation set to " + result + ".");
	}

	public void OnRendezvousCommand(string arg)
	{
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && !(FlightGlobals.ActiveVessel == null))
		{
			string[] array = arg.Trim().Split(' ');
			if (array.Length < 7)
			{
				Debug.Log("[DebugConsole]: Not enough arguments to rendezvous with a vessel. Please see /help rendezvous");
				return;
			}
			Vessel vessel = null;
			Vector3d zero = Vector3d.zero;
			Vector3d zero2 = Vector3d.zero;
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				switch (i)
				{
				case 0:
				{
					if (int.TryParse(array[i], out var result))
					{
						if (result >= 0 && result < FlightGlobals.Vessels.Count)
						{
							vessel = FlightGlobals.Vessels[result];
							if (vessel == FlightGlobals.ActiveVessel)
							{
								Debug.Log("[DebugConsole]: Unable to rendezvous with self.");
								return;
							}
							break;
						}
						Debug.Log("[DebugConsole]: Invalid vessel index while rendezvousing.");
						return;
					}
					Debug.Log("[DebugConsole]: Error parsing vessel index while rendezvousing.");
					return;
				}
				case 1:
					if (!double.TryParse(array[i], out zero.x))
					{
						Debug.Log("[DebugConsole]: Error parsing position x while rendezvousing.");
						return;
					}
					break;
				case 2:
					if (!double.TryParse(array[i], out zero.y))
					{
						Debug.Log("[DebugConsole]: Error parsing position y while rendezvousing.");
						return;
					}
					break;
				case 3:
					if (!double.TryParse(array[i], out zero.z))
					{
						Debug.Log("[DebugConsole]: Error parsing position z while rendezvousing.");
						return;
					}
					break;
				case 4:
					if (!double.TryParse(array[i], out zero2.x))
					{
						Debug.Log("[DebugConsole]: Error parsing velocity x while rendezvousing.");
						return;
					}
					break;
				case 5:
					if (!double.TryParse(array[i], out zero2.y))
					{
						Debug.Log("[DebugConsole]: Error parsing velocity y while rendezvousing.");
						return;
					}
					break;
				case 6:
					if (!double.TryParse(array[i], out zero2.z))
					{
						Debug.Log("[DebugConsole]: Error parsing velocity z while rendezvousing.");
						return;
					}
					break;
				}
			}
			Debug.Log(string.Concat("[DebugConsole]: rendezvousing with ", vessel.name, ", ", zero, ", ", zero2, "."));
			FlightGlobals.fetch.SetShipOrbitRendezvous(vessel, zero, zero2);
		}
		else
		{
			Debug.Log("[DebugConsole]: Cannot rendezvous, as there is no active vessel loaded in a flight scene.");
		}
	}

	public void OnTargetvCommand(string arg)
	{
		if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && !(FlightGlobals.ActiveVessel == null))
		{
			string[] array = arg.Trim().Split(' ');
			if (array.Length < 1)
			{
				Debug.Log("[DebugConsole]: Not enough arguments to target a vessel. Please see /help targetv");
				return;
			}
			Vessel vessel = null;
			_ = Vector3d.zero;
			_ = Vector3d.zero;
			int num = 0;
			int num2 = array.Length;
			while (true)
			{
				if (num < num2)
				{
					if (num == 0)
					{
						if (!int.TryParse(array[num], out var result))
						{
							Debug.Log("[DebugConsole]: Error parsing vessel index while rendezvousing.");
							return;
						}
						if (result < 0 || result >= FlightGlobals.Vessels.Count)
						{
							break;
						}
						vessel = FlightGlobals.Vessels[result];
						if (vessel == FlightGlobals.ActiveVessel)
						{
							Debug.Log("[DebugConsole]: Unable to rendezvous with self.");
							return;
						}
					}
					num++;
					continue;
				}
				Debug.Log("[DebugConsole]: targeting " + vessel.name);
				FlightGlobals.fetch.SetVesselTarget(vessel);
				return;
			}
			Debug.Log("[DebugConsole]: Invalid vessel index while rendezvousing.");
		}
		else
		{
			Debug.Log("[DebugConsole]: Cannot target, as there is no active vessel loaded in a flight scene.");
		}
	}

	public void OnBadassCommand(string arg)
	{
		ScreenMessages.PostScreenMessage("That's one small step for a kerbal, one giant leap for kerbalkind", 10f, ScreenMessageStyle.UPPER_CENTER);
		if ((bool)Funding.Instance)
		{
			Funding.Instance.SetFunds(31415926.0, TransactionReasons.Cheating);
		}
		if ((bool)ResearchAndDevelopment.Instance)
		{
			ResearchAndDevelopment.Instance.SetScience(1337f, TransactionReasons.Cheating);
			ResearchAndDevelopment.Instance.CheatTechnology();
		}
		if ((bool)Reputation.Instance)
		{
			Reputation.Instance.SetReputation(Reputation.RepRange, TransactionReasons.Cheating);
		}
		if ((bool)ScenarioUpgradeableFacilities.Instance)
		{
			ScenarioUpgradeableFacilities.Instance.CheatFacilities();
		}
		if ((bool)ProgressTracking.Instance)
		{
			ProgressTracking.Instance.CheatEarlyProgression();
		}
		KerbalRoster.CheatExperience();
		if (!HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels)
		{
			HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels = true;
		}
		if ((bool)SpaceCenter.Instance && (bool)ScenarioCustomWaypoints.Instance)
		{
			string arg2 = FlightGlobals.GetHomeBodyName() + " " + SpaceCenter.Instance.Latitude + " " + SpaceCenter.Instance.Longitude + " Kerbal Space Center";
			OnWaypointCommand(arg2);
		}
	}

	public void OnSettingsLoadCommand(string arg)
	{
		Debug.Log("[DebugConsole]: Loading Settings.cfg");
		GameSettings.LoadGameSettingsOnly();
	}

	public void OnSettingsSaveCommand(string arg)
	{
		Debug.Log("[DebugConsole]: Saving settings to settings.cfg.");
		GameSettings.SaveGameSettingsOnly();
	}
}
