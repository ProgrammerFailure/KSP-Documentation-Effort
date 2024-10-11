using System;
using System.Runtime.CompilerServices;

namespace KSPAchievements;

[Serializable]
public class CelestialBodySubtree : ProgressNode
{
	public CelestialBody Body;

	public CelestialBodyFlyby flyBy;

	public CelestialBodyOrbit orbit;

	public CelestialBodyEscape escape;

	public CelestialBodySuborbit suborbit;

	public CelestialBodyFlight flight;

	public CelestialBodyLanding landing;

	public CelestialBodySplashdown splashdown;

	public CelestialBodyTransfer crewTransfer;

	public CelestialBodyScience science;

	public Rendezvous rendezvous;

	public Docking docking;

	public Spacewalk spacewalk;

	public SurfaceEVA surfaceEVA;

	public FlagPlant flagPlant;

	public BaseConstruction baseConstruction;

	public StationConstruction stationConstruction;

	public CelestialBodyReturn returnFromFlyby;

	public CelestialBodyReturn returnFromOrbit;

	public CelestialBodyReturn returnFromSurface;

	[NonSerialized]
	public CelestialBodySubtree parentTree;

	[NonSerialized]
	public CelestialBodySubtree[] childTrees;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodySubtree(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAchievementAchieve(ProgressNode node)
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
	private string generateSubtreeSummary(string baseID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LinkBodyHome(CelestialBodySubtree[] trees)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DebugBodyTree()
	{
		throw null;
	}
}
