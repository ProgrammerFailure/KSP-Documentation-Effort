using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using FinePrint.Utilities;

namespace FinePrint.Contracts;

public class SurveyContract : Contract
{
	public CelestialBody targetBody;

	public double centerLatitude;

	public double centerLongitude;

	public string dataName;

	public string anomalyName;

	public string resultName;

	public string locationName;

	private bool focusedSurvey;

	public FlightBand targetBand;

	private bool contextual;

	private string vesselName;

	private string cardinalName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SurveyContract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanBeDeclined()
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
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetSynopsys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string MessageCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetNotes()
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
	public override bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override List<CelestialBody> GetWeightBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupParam(int paramIndex, KeyValuePair<SurveyDefinitionParameter, FlightBand> pair, int totalParameters, Vessel contextualVessel = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<KeyValuePair<Vessel, List<string>>> CachePossibleVessels(CelestialBody body, List<SurveyDefinition> originalDefinitions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected List<SurveyDefinition> PossibleDefinitions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double ParamRange(ContractPrestige prestige, CelestialBody body, FlightBand band)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RandomizeNearKSC(ContractPrestige prestigeLevel, bool allowWater, Random generator = null)
	{
		throw null;
	}
}
