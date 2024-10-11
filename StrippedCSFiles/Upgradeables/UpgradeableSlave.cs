using System.Runtime.CompilerServices;
using UnityEngine;

namespace Upgradeables;

public class UpgradeableSlave : UpgradeableObject
{
	[SerializeField]
	private UpgradeableFacility[] neighbours;

	[SerializeField]
	private string[] neighbourIDs;

	private float neighbourAvgNorm;

	public UpgradeableFacility[] Neighbours
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string[] NeighbourIDs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UpgradeableSlave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Compile Neighbour IDs")]
	public void CompileIDs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnObjectLevelChange(UpgradeableObject upObj, int lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLevel()
	{
		throw null;
	}
}
