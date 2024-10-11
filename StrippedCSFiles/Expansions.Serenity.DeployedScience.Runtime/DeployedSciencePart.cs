using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

public class DeployedSciencePart : MonoBehaviour, IConfigNode
{
	[SerializeField]
	private uint partId;

	[SerializeField]
	private bool deployedOnGround;

	[SerializeField]
	private bool moduleEnabled;

	[SerializeField]
	private int powerUnitsProduced;

	[SerializeField]
	private int actualPowerUnitsProduced;

	[SerializeField]
	private int powerUnitsRequired;

	[SerializeField]
	private bool isSolarPanel;

	[SerializeField]
	private bool isAntenna;

	[SerializeField]
	private double antennaBoosterPower;

	public DeployedScienceExperiment Experiment;

	[SerializeField]
	private uint controllerId;

	public DeployedScienceCluster Cluster;

	private ModuleGroundSciencePart loadedPartModule;

	public uint PartId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool DeployedOnGround
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Enabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int PowerUnitsProduced
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ActualPowerUnitsProduced
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int PowerUnitsRequired
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsSolarPanel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAntenna
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double AntennaBoosterPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint ControllerId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeployedSciencePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static DeployedSciencePart Spawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedSciencePart Spawn(ModuleGroundSciencePart part, ModuleGroundExpControl controlUnit, DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedSciencePart SpawnandLoad(ConfigNode node, DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundSciencePartRemoved(ModuleGroundSciencePart sciencePartRemoved)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSciencePart(ModuleGroundSciencePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part PartIsLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
