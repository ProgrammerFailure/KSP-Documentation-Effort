using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class DeployedScience : ScenarioModule
{
	private DictionaryValueList<uint, DeployedScienceCluster> deployedScienceClusters;

	private static GameObject deployedScienceGameObject;

	private DictionaryValueList<int, float> diminishingReturns;

	public static string SeismicExperimentId;

	private DictionaryValueList<string, float> seismicEnergyRates;

	public float MinimumSeismicEnergyRequired;

	public int SeismicScienceProcessingDelay;

	public double ScienceTimeDelay;

	public double DataSendFailedTimeDelay;

	public static DeployedScience Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public static bool IsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public DictionaryValueList<uint, DeployedScienceCluster> DeployedScienceClusters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static GameObject DeployedScienceGameObject
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DictionaryValueList<int, float> DiminishingReturns
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DictionaryValueList<string, float> SeismicEnergyRates
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeployedScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DeployedScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSciencePart(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCluster(ModuleGroundExpControl controlUnit, bool replaceList, List<ModuleGroundSciencePart> deployableScienceParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRecovered(ProtoVessel vessel, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselTerminated(ProtoVessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselWillDestroy(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartExplodeGroundCollision(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselExplodeGroundCollision(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScanAndProcessSeismicEvent(Vessel vessel, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameStateLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ExpDimReturnsRate(string experimentId, string celestialBodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetDiminishingReturnsMultiplier(int numExperiments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeployedSciencePart ExpMaxScienceMultiplier(string experimentId, string celestialBodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDiminishingScienceRate(string experimentId, string celestialBodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterCluster(ModuleGroundExpControl controlUnit, List<ModuleGroundSciencePart> deployableScienceParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeRegisterCluster(uint controlUnitId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDeployedSciencePartVesselType(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveMannedScienceObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupDiminishingReturns(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSeismicEnergy(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}
}
