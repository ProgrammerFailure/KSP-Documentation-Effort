using System.Runtime.CompilerServices;
using UnityEngine;

public class PartStatsUpgradeModule : PartModule, IModuleInfo, IPartMassModifier, IPartCostModifier
{
	public ConfigNode upgradeNode;

	public bool isUpgraded;

	public float massOffset;

	public float costOffset;

	public bool cumulativeCost;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartStatsUpgradeModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}
}
