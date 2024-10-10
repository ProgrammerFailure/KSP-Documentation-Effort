using ns9;

public class ModuleTripLogger : PartModule
{
	public FlightLog Log;

	public const string moduleString = "ModuleTripLogger";

	public const string logString = "Log";

	public static string cacheAutoLOC_6003061;

	public override void OnAwake()
	{
		Log = new FlightLog();
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("Surfaced"))
		{
			LoadDeprecated("Surfaced", FlightLog.EntryType.Land, node);
		}
		if (node.HasNode("Flew"))
		{
			LoadDeprecated("Flew", FlightLog.EntryType.Flight, node);
		}
		if (node.HasNode("FlewBy"))
		{
			LoadDeprecated("FlewBy", FlightLog.EntryType.Flyby, node);
		}
		if (node.HasNode("Orbited"))
		{
			LoadDeprecated("Orbited", FlightLog.EntryType.Orbit, node);
		}
		if (node.HasNode("SubOrbited"))
		{
			LoadDeprecated("SubOrbited", FlightLog.EntryType.Suborbit, node);
		}
		Log.Load(node.GetNode("Log"));
	}

	public void LoadDeprecated(string oldName, FlightLog.EntryType newName, ConfigNode node)
	{
		string[] values = node.GetNode(oldName).GetValues("at");
		int i = 0;
		for (int num = values.Length; i < num; i++)
		{
			Log.AddEntry(newName, values[i]);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		Log.Save(node.AddNode("Log"));
	}

	public override void OnStart(StartState state)
	{
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChanged);
		GameEvents.onVesselSOIChanged.Add(OnVesselSOIChanged);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChanged);
		GameEvents.onVesselSOIChanged.Remove(OnVesselSOIChanged);
		if (IsInvoking())
		{
			CancelInvoke();
		}
	}

	public void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		if (!(vs.host != base.vessel))
		{
			InvokeLogVessel(0.2f);
		}
	}

	public void OnVesselSOIChanged(GameEvents.HostedFromToAction<Vessel, CelestialBody> vc)
	{
		if (!(vc.host != base.vessel))
		{
			InvokeLogVessel(0.2f);
		}
	}

	public void InvokeLogVessel(float delay)
	{
		if (IsInvoking())
		{
			CancelInvoke();
		}
		Invoke("LogVesselSituation", delay);
	}

	public void LogVesselSituation()
	{
		LogNewSituation(base.vessel.situation, base.vessel.mainBody);
	}

	public void LogNewSituation(Vessel.Situations sit, CelestialBody where)
	{
		switch (sit)
		{
		case Vessel.Situations.FLYING:
			LogSituation(FlightLog.EntryType.Flight, where.name);
			break;
		case Vessel.Situations.LANDED:
		case Vessel.Situations.SPLASHED:
			LogSituation(FlightLog.EntryType.Land, where.name);
			break;
		case Vessel.Situations.ORBITING:
			LogSituation(FlightLog.EntryType.Orbit, where.name);
			break;
		case Vessel.Situations.SUB_ORBITAL:
			LogSituation(FlightLog.EntryType.Suborbit, where.name);
			break;
		case Vessel.Situations.ESCAPING:
			LogSituation(FlightLog.EntryType.Escape, where.name);
			break;
		}
	}

	public void LogSituation(FlightLog.EntryType type, string target)
	{
		Log.AddEntryUnique(type, target);
		int i = 0;
		for (int count = base.part.protoModuleCrew.Count; i < count; i++)
		{
			base.part.protoModuleCrew[i].flightLog.AddEntryUnique(type, target);
			base.part.protoModuleCrew[i].UpdateExperience();
		}
		GameEvents.OnFlightLogRecorded.Fire(base.vessel);
	}

	public static void ForceVesselLog(Vessel v, FlightLog.EntryType type, CelestialBody body)
	{
		if (!(v == null) && !(body == null))
		{
			if (v.loaded)
			{
				ForceLogEntry(v, type, body);
			}
			else
			{
				ForceLogEntry(v.protoVessel, type, body);
			}
			GameEvents.OnFlightLogRecorded.Fire(v);
		}
	}

	public static void ForceLogEntry(Vessel v, FlightLog.EntryType type, CelestialBody body)
	{
		int count = v.Parts.Count;
		while (count-- > 0)
		{
			Part part = v.Parts[count];
			int count2 = part.protoModuleCrew.Count;
			while (count2-- > 0)
			{
				part.protoModuleCrew[count2].flightLog.AddEntryUnique(type, body.name);
				part.protoModuleCrew[count2].UpdateExperience();
			}
			int count3 = part.Modules.Count;
			while (count3-- > 0)
			{
				ModuleTripLogger moduleTripLogger = part.Modules[count3] as ModuleTripLogger;
				if (!(moduleTripLogger == null))
				{
					moduleTripLogger.Log.AddEntryUnique(type, body.name);
				}
			}
		}
	}

	public static void ForceLogEntry(ProtoVessel pv, FlightLog.EntryType type, CelestialBody body)
	{
		int count = pv.protoPartSnapshots.Count;
		while (count-- > 0)
		{
			ProtoPartSnapshot protoPartSnapshot = pv.protoPartSnapshots[count];
			int count2 = protoPartSnapshot.modules.Count;
			while (count2-- > 0)
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[count2];
				if (!(protoPartModuleSnapshot.moduleName != "ModuleTripLogger"))
				{
					FlightLog flightLog = new FlightLog();
					flightLog.Load(protoPartModuleSnapshot.moduleValues.GetNode("Log"));
					protoPartModuleSnapshot.moduleValues.RemoveNode("Log");
					flightLog.AddEntryUnique(type, body.name);
					flightLog.Save(protoPartModuleSnapshot.moduleValues.AddNode("Log"));
					int i = 0;
					for (int count3 = protoPartSnapshot.protoModuleCrew.Count; i < count3; i++)
					{
						protoPartSnapshot.protoModuleCrew[i].flightLog.AddEntryUnique(type, body.name);
						protoPartSnapshot.protoModuleCrew[i].UpdateExperience();
					}
				}
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003061;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003061 = Localizer.Format("#autoLoc_6003061");
	}
}
