using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.TooltipTypes;

[RequireComponent(typeof(UIStateButton))]
public class UIStateButtonTooltip : TooltipController
{
	public Tooltip_Text tooltipPrefab;

	public ButtonStateTooltip[] tooltipStates;

	private Tooltip_Text spawnedTooltip;

	private UIStateButton stateButton;

	private ButtonStateTooltip currentTooltipState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateButtonTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ButtonStateTooltip FindButtonStateTooltip(string state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChanged(UIStateButton button)
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
}
