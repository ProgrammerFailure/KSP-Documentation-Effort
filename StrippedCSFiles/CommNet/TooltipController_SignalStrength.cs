using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;

namespace CommNet;

public class TooltipController_SignalStrength : TooltipController
{
	public Tooltip_SignalStrength prefab;

	public Tooltip_SignalStrengthItem itemPrefab;

	protected Tooltip_SignalStrength tooltip;

	private List<Tooltip_SignalStrengthItem> items;

	private static string cacheAutoLOC_121470;

	protected static string NoSignal;

	protected static string NoSignalPlasma;

	protected static string NoSignalControl;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TooltipController_SignalStrength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TooltipController_SignalStrength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool OnTooltipAboutToSpawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTooltipSpawned(Tooltip instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTooltipDespawned(Tooltip instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool OnTooltipUpdate(Tooltip instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ClearList()
	{
		throw null;
	}
}
