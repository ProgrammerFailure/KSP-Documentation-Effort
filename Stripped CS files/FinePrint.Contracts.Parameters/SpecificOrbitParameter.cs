using System;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class SpecificOrbitParameter : ContractParameter
{
	public double deviationWindow;

	public ContractOrbitRenderer orbitRenderer;

	public int successCounter;

	public OrbitType orbitType;

	public Orbit orbit;

	public double inclination;

	public double eccentricity;

	public double sma;

	public double lan;

	public double argumentOfPeriapsis;

	public double meanAnomalyAtEpoch;

	public double epoch;

	public CelestialBody TargetBody;

	public Orbit Orbit => orbit;

	public SpecificOrbitParameter()
	{
		deviationWindow = 10.0;
		successCounter = 0;
		orbitType = OrbitType.RANDOM;
		inclination = 0.0;
		eccentricity = 0.0;
		sma = 10000000.0;
		lan = 0.0;
		argumentOfPeriapsis = 0.0;
		meanAnomalyAtEpoch = 0.0;
		epoch = 0.0;
		TargetBody = Planetarium.fetch.Home;
		orbit = null;
	}

	public SpecificOrbitParameter(OrbitType orbitType, double inclination, double eccentricity, double sma, double lan, double argumentOfPeriapsis, double meanAnomalyAtEpoch, double epoch, CelestialBody targetBody, double deviationWindow)
	{
		this.deviationWindow = deviationWindow;
		successCounter = 0;
		this.orbitType = orbitType;
		this.inclination = inclination;
		this.eccentricity = eccentricity;
		this.sma = sma;
		this.lan = lan;
		this.argumentOfPeriapsis = argumentOfPeriapsis;
		this.meanAnomalyAtEpoch = meanAnomalyAtEpoch;
		this.epoch = epoch;
		TargetBody = targetBody;
		orbit = new Orbit(inclination, eccentricity, sma, lan, argumentOfPeriapsis, meanAnomalyAtEpoch, epoch, TargetBody);
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		string text = Localizer.Format("#autoLOC_284812");
		switch (base.Root.Prestige)
		{
		case Contract.ContractPrestige.Trivial:
			text = Localizer.Format("#autoLOC_284817");
			break;
		case Contract.ContractPrestige.Significant:
			text = Localizer.Format("#autoLOC_284820");
			break;
		case Contract.ContractPrestige.Exceptional:
			text = Localizer.Format("#autoLOC_284823");
			break;
		}
		switch (orbitType)
		{
		default:
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001091", TargetBody.displayName, text));
		case OrbitType.SYNCHRONOUS:
			if (TargetBody == Planetarium.fetch.Sun)
			{
				return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.SunSynchronousName, TargetBody.displayName, text));
			}
			if (TargetBody == Planetarium.fetch.Home)
			{
				return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.HomeSynchronousName, TargetBody.displayName, text));
			}
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.OtherSynchronousName, TargetBody.displayName, text));
		case OrbitType.STATIONARY:
			if (TargetBody == Planetarium.fetch.Sun)
			{
				return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001090", ContractDefs.SunStationaryName, TargetBody.displayName, text));
			}
			if (TargetBody == Planetarium.fetch.Home)
			{
				return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001090", ContractDefs.HomeStationaryName, TargetBody.displayName, text));
			}
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001090", ContractDefs.OtherStationaryName, TargetBody.displayName, text));
		case OrbitType.POLAR:
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.PolarOrbitName, TargetBody.displayName, text));
		case OrbitType.EQUATORIAL:
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.EquatorialOrbitName, TargetBody.displayName, text));
		case OrbitType.KOLNIYA:
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", StringUtilities.TitleCase(ContractDefs.MolniyaName), TargetBody.displayName, text));
		case OrbitType.TUNDRA:
			return StringBuilderCache.Format(Localizer.Format("#autoLOC_7001089", ContractDefs.TundraOrbitName, TargetBody.displayName, text));
		}
	}

	public override string GetNotes()
	{
		double a = sma * (1.0 - eccentricity) - TargetBody.Radius;
		double a2 = sma * (1.0 + eccentricity) - TargetBody.Radius;
		string text = "";
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			text += "\n";
		}
		text += StringBuilderCache.Format("{0}:\n{1}\n{2}\n{3}\n", Localizer.Format("#autoLOC_7003228"), Localizer.Format("#autoLOC_7003229", Convert.ToDecimal(Math.Round(a2)).ToString("#,###")), Localizer.Format("#autoLOC_7003230", Convert.ToDecimal(Math.Round(a)).ToString("#,###")), Localizer.Format("#autoLOC_7003231", Math.Round(inclination, 1)));
		if (Math.Abs(inclination) % 180.0 >= 1.0)
		{
			text = text + "\n" + Localizer.Format("#autoLOC_7003232", Math.Round(lan, 1).ToString());
		}
		if (eccentricity > 0.05)
		{
			text = text + "\n" + Localizer.Format("#autoLOC_7003233", Math.Round(argumentOfPeriapsis, 1).ToString());
		}
		return text;
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
	}

	public override void OnUnregister()
	{
		CleanupRenderer();
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnSave(ConfigNode node)
	{
		if (orbit == null)
		{
			orbit = new Orbit(inclination, eccentricity, sma, lan, argumentOfPeriapsis, meanAnomalyAtEpoch, epoch, TargetBody);
		}
		ValidateOrbit();
		node.AddValue("TargetBody", TargetBody.flightGlobalsIndex);
		node.AddValue("deviationWindow", deviationWindow);
		node.AddValue("orbitType", (int)orbitType);
		node.AddValue("inclination", inclination);
		node.AddValue("eccentricity", eccentricity);
		node.AddValue("sma", sma);
		node.AddValue("lan", lan);
		node.AddValue("argumentOfPeriapsis", argumentOfPeriapsis);
		node.AddValue("meanAnomalyAtEpoch", meanAnomalyAtEpoch);
		node.AddValue("epoch", epoch);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "TargetBody", ref TargetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "deviationWindow", ref deviationWindow, 10.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "orbitType", ref orbitType, OrbitType.RANDOM);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "inclination", ref inclination, 0.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "eccentricity", ref eccentricity, 0.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "sma", ref sma, 1000000.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "lan", ref lan, 0.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "argumentOfPeriapsis", ref argumentOfPeriapsis, 0.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "meanAnomalyAtEpoch", ref meanAnomalyAtEpoch, 0.0);
		SystemUtilities.LoadNode(node, "SpecificOrbitParameter", "epoch", ref epoch, 0.0);
		if (orbit == null)
		{
			orbit = new Orbit(inclination, eccentricity, sma, lan, argumentOfPeriapsis, meanAnomalyAtEpoch, epoch, TargetBody);
		}
		ValidateOrbit();
		if (HighLogic.LoadedSceneIsFlight && base.Root.ContractState == Contract.State.Active)
		{
			SetupRenderer();
		}
		if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && base.State == ParameterState.Incomplete && (base.Root.ContractState == Contract.State.Active || (!base.Root.IsFinished() && ContractDefs.DisplayOfferedOrbits)))
		{
			SetupRenderer();
		}
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active, checkVessel: true))
		{
			return;
		}
		bool flag = orbitRenderer != null && orbitRenderer.driver != null && VesselUtilities.VesselAtOrbit(orbitRenderer.driver.orbit, deviationWindow);
		if (base.State == ParameterState.Incomplete)
		{
			if (orbitRenderer != null)
			{
				orbitRenderer.activeDraw = true;
			}
			if (flag)
			{
				successCounter++;
			}
			else
			{
				successCounter = 0;
			}
			if (successCounter >= 5)
			{
				SetComplete();
			}
		}
		if (base.State != ParameterState.Complete)
		{
			return;
		}
		if (flag)
		{
			if (orbitRenderer != null)
			{
				orbitRenderer.activeDraw = false;
			}
			return;
		}
		if (orbitRenderer != null)
		{
			orbitRenderer.activeDraw = true;
		}
		SetIncomplete();
	}

	public void SetupRenderer()
	{
		if (!(orbitRenderer != null) && orbit != null)
		{
			orbitRenderer = ContractOrbitRenderer.Setup(base.Root, orbit);
		}
	}

	public void CleanupRenderer()
	{
		if (orbitRenderer != null)
		{
			orbitRenderer.Cleanup();
		}
	}

	public void ValidateOrbit()
	{
		if (base.Root != null && !base.Root.IsFinished() && !(TargetBody == null) && orbit != null)
		{
			double altitudeDifficulty;
			double inclinationDifficulty;
			switch (base.Root.Prestige)
			{
			default:
				altitudeDifficulty = ContractDefs.Satellite.SignificantAltitudeDifficulty;
				inclinationDifficulty = ContractDefs.Satellite.SignificantInclinationDifficulty;
				break;
			case Contract.ContractPrestige.Exceptional:
				altitudeDifficulty = ContractDefs.Satellite.ExceptionalAltitudeDifficulty;
				inclinationDifficulty = ContractDefs.Satellite.ExceptionalInclinationDifficulty;
				break;
			case Contract.ContractPrestige.Trivial:
				altitudeDifficulty = ContractDefs.Satellite.TrivialAltitudeDifficulty;
				inclinationDifficulty = ContractDefs.Satellite.TrivialInclinationDifficulty;
				break;
			}
			if (!OrbitUtilities.ValidateOrbit(base.Root.MissionSeed, ref orbit, orbitType, altitudeDifficulty, inclinationDifficulty, "contract parameter \"" + GetTitle() + "\""))
			{
				inclination = orbit.inclination;
				eccentricity = orbit.eccentricity;
				sma = orbit.semiMajorAxis;
				lan = orbit.double_0;
				argumentOfPeriapsis = orbit.argumentOfPeriapsis;
				meanAnomalyAtEpoch = orbit.meanAnomalyAtEpoch;
				epoch = orbit.epoch;
			}
		}
	}
}
