public class VesselTripLog
{
	public FlightLog Log;

	public VesselTripLog()
	{
		Log = new FlightLog();
	}

	public static VesselTripLog FromVessel(Vessel v)
	{
		VesselTripLog vesselTripLog = new VesselTripLog();
		int i = 0;
		for (int count = v.parts.Count; i < count; i++)
		{
			Part part = v.parts[i];
			if (part.Modules.Contains("ModuleTripLogger"))
			{
				ModuleTripLogger moduleTripLogger = part.Modules["ModuleTripLogger"] as ModuleTripLogger;
				vesselTripLog.Log.MergeWith(moduleTripLogger.Log);
			}
		}
		return vesselTripLog;
	}

	public static VesselTripLog FromProtoVessel(ProtoVessel pv)
	{
		VesselTripLog vesselTripLog = new VesselTripLog();
		int count = pv.protoPartSnapshots.Count;
		for (int i = 0; i < count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = pv.protoPartSnapshots[i];
			int count2 = protoPartSnapshot.modules.Count;
			for (int j = 0; j < count2; j++)
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[j];
				if (protoPartModuleSnapshot.moduleName == "ModuleTripLogger")
				{
					VesselTripLog vesselTripLog2 = new VesselTripLog();
					vesselTripLog2.Log.Load(protoPartModuleSnapshot.moduleValues.GetNode("Log"));
					vesselTripLog.Log.MergeWith(vesselTripLog2.Log);
				}
			}
		}
		return vesselTripLog;
	}
}
