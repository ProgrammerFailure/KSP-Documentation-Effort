using KSPAchievements;

namespace FinePrint.Utilities;

public class ProgressMilestone
{
	public CelestialBody body;

	public ProgressType type;

	public MannedStatus manned;

	public CelestialBodySubtree progressTree;

	public CelestialBodySubtree progress
	{
		get
		{
			if (progressTree != null)
			{
				return progressTree;
			}
			if (body == null)
			{
				return null;
			}
			progressTree = body.progressTree;
			if (progressTree != null)
			{
				return progressTree;
			}
			int num = ProgressTracking.Instance.celestialBodyNodes.Length;
			while (num-- > 0)
			{
				CelestialBodySubtree celestialBodySubtree = ProgressTracking.Instance.celestialBodyNodes[num];
				if (!(celestialBodySubtree.Body != body))
				{
					progressTree = celestialBodySubtree;
					break;
				}
			}
			return progressTree;
		}
	}

	public bool complete
	{
		get
		{
			if (!possible)
			{
				return false;
			}
			switch (type)
			{
			case ProgressType.SPEEDRECORD:
				return CheckCompletion("RecordsSpeed");
			case ProgressType.REACHSPACE:
				return CheckCompletion("ReachedSpace");
			case ProgressType.ALTITUDERECORD:
				return CheckCompletion("RecordsAltitude");
			case ProgressType.CREWRECOVERY:
				return CheckCompletion("FirstCrewToSurvive");
			case ProgressType.DEPTHRECORD:
				return CheckCompletion("RecordsDepth");
			case ProgressType.DISTANCERECORD:
				return CheckCompletion("RecordsDistance");
			default:
				if (progress != null && progress.IsReached)
				{
					return type switch
					{
						ProgressType.BASECONSTRUCTION => CheckCompletion(progress.baseConstruction), 
						ProgressType.CREWTRANSFER => CheckCompletion(progress.crewTransfer), 
						ProgressType.DOCKING => CheckCompletion(progress.docking), 
						ProgressType.ESCAPE => CheckCompletion(progress.escape), 
						ProgressType.FLAGPLANT => CheckCompletion(progress.flagPlant), 
						ProgressType.FLIGHT => CheckCompletion(progress.flight), 
						ProgressType.FLYBY => CheckCompletion(progress.flyBy), 
						ProgressType.FLYBYRETURN => CheckCompletion(progress.returnFromFlyby), 
						ProgressType.LANDING => CheckCompletion(progress.landing), 
						ProgressType.LANDINGRETURN => CheckCompletion(progress.returnFromSurface), 
						ProgressType.ORBIT => CheckCompletion(progress.orbit), 
						ProgressType.ORBITRETURN => CheckCompletion(progress.returnFromOrbit), 
						ProgressType.RENDEZVOUS => CheckCompletion(progress.rendezvous), 
						ProgressType.SCIENCE => CheckCompletion(progress.science), 
						ProgressType.SPACEWALK => CheckCompletion(progress.spacewalk), 
						ProgressType.SPLASHDOWN => CheckCompletion(progress.splashdown), 
						ProgressType.STATIONCONSTRUCTION => CheckCompletion(progress.stationConstruction), 
						ProgressType.SUBORBIT => CheckCompletion(progress.suborbit), 
						ProgressType.SURFACEEVA => CheckCompletion(progress.surfaceEVA), 
						_ => false, 
					};
				}
				return false;
			case ProgressType.FIRSTLAUNCH:
				return CheckCompletion("FirstLaunch");
			}
		}
	}

	public bool possible
	{
		get
		{
			if (type == ProgressType.SPLASHDOWN && (body == null || !body.ocean))
			{
				return false;
			}
			if (type == ProgressType.FLIGHT && (body == null || !body.atmosphere))
			{
				return false;
			}
			if (manned == MannedStatus.UNMANNED && impliesManned)
			{
				return false;
			}
			if (CelestialUtilities.IsGasGiant(body))
			{
				switch (type)
				{
				case ProgressType.BASECONSTRUCTION:
				case ProgressType.FLAGPLANT:
				case ProgressType.LANDING:
				case ProgressType.LANDINGRETURN:
				case ProgressType.SURFACEEVA:
					return false;
				}
			}
			return true;
		}
	}

	public bool bodySensitive
	{
		get
		{
			switch (type)
			{
			default:
				return false;
			case ProgressType.BASECONSTRUCTION:
			case ProgressType.CREWTRANSFER:
			case ProgressType.DOCKING:
			case ProgressType.ESCAPE:
			case ProgressType.FLAGPLANT:
			case ProgressType.FLIGHT:
			case ProgressType.FLYBY:
			case ProgressType.FLYBYRETURN:
			case ProgressType.LANDING:
			case ProgressType.LANDINGRETURN:
			case ProgressType.ORBIT:
			case ProgressType.ORBITRETURN:
			case ProgressType.RENDEZVOUS:
			case ProgressType.SCIENCE:
			case ProgressType.SPACEWALK:
			case ProgressType.SPLASHDOWN:
			case ProgressType.STATIONCONSTRUCTION:
			case ProgressType.SUBORBIT:
			case ProgressType.SURFACEEVA:
				return true;
			}
		}
	}

	public bool crewSensitive
	{
		get
		{
			switch (type)
			{
			default:
				return false;
			case ProgressType.BASECONSTRUCTION:
			case ProgressType.DOCKING:
			case ProgressType.ESCAPE:
			case ProgressType.FIRSTLAUNCH:
			case ProgressType.FLIGHT:
			case ProgressType.FLYBY:
			case ProgressType.FLYBYRETURN:
			case ProgressType.LANDING:
			case ProgressType.LANDINGRETURN:
			case ProgressType.ORBIT:
			case ProgressType.ORBITRETURN:
			case ProgressType.REACHSPACE:
			case ProgressType.RENDEZVOUS:
			case ProgressType.SCIENCE:
			case ProgressType.SPLASHDOWN:
			case ProgressType.STATIONCONSTRUCTION:
			case ProgressType.SUBORBIT:
				return true;
			}
		}
	}

	public bool impliesManned
	{
		get
		{
			switch (type)
			{
			default:
				return false;
			case ProgressType.CREWTRANSFER:
			case ProgressType.CREWRECOVERY:
			case ProgressType.FLAGPLANT:
			case ProgressType.SPACEWALK:
			case ProgressType.SURFACEEVA:
				return true;
			}
		}
	}

	public ProgressMilestone()
	{
		body = Planetarium.fetch.Sun;
		type = ProgressType.SPLASHDOWN;
		manned = MannedStatus.const_2;
	}

	public ProgressMilestone(CelestialBody body, ProgressType type, MannedStatus manned = MannedStatus.const_2)
	{
		this.body = body;
		this.type = type;
		this.manned = manned;
	}

	public bool CheckCompletion(ProgressNode node)
	{
		if (node == null)
		{
			return false;
		}
		return manned switch
		{
			MannedStatus.UNMANNED => node.IsCompleteUnmanned, 
			MannedStatus.MANNED => node.IsCompleteManned, 
			_ => node.IsComplete, 
		};
	}

	public bool CheckCompletion(string nodeID)
	{
		if (ProgressTracking.Instance == null)
		{
			return false;
		}
		return manned switch
		{
			MannedStatus.UNMANNED => ProgressTracking.Instance.NodeCompleteUnmanned(nodeID), 
			MannedStatus.MANNED => ProgressTracking.Instance.NodeCompleteManned(nodeID), 
			_ => ProgressTracking.Instance.NodeComplete(nodeID), 
		};
	}
}
