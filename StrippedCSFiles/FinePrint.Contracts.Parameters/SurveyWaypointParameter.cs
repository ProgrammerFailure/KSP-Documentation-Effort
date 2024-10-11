using System.Runtime.CompilerServices;
using Contracts;
using FinePrint.Utilities;

namespace FinePrint.Contracts.Parameters;

public class SurveyWaypointParameter : ContractParameter
{
	public Waypoint wp;

	public WaypointClusterState clusterState;

	private string experiment;

	public string actionDescription;

	private double thresholdAltitude;

	private FlightBand band;

	private CelestialBody targetBody;

	private bool submittedWaypoint;

	private bool outerWarning;

	private bool verified;

	private bool contextual;

	private int bouncesLeft;

	private double bounceOriginLatitude;

	private double bounceOriginLongitude;

	private bool eventsAdded;

	private string contextualAnomalyName;

	private string ContextualAnomalyName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyWaypointParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyWaypointParameter(string experiment, string actionDescription, CelestialBody targetBody, Waypoint wp, FlightBand band)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyWaypointParameter(string experiment, string actionDescription, CelestialBody targetBody, Waypoint wp, FlightBand band, bool contextual, int bouncesLeft, double bounceOriginLatitude, double bounceOriginLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetMessageComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckExperimentResults(ScienceData experimentData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Bounce()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double TriggerRange(FlightBand band)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProcessWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateAltitudes(double terrainAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VerifyWaypoint(double terrainAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string SiteString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Waypoint NextWaypointInCluster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateNavWaypoint(Waypoint newWaypoint)
	{
		throw null;
	}
}
