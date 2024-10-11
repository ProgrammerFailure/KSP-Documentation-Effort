using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.TooltipTypes;

[RequireComponent(typeof(UIRadioButton))]
public class UIRadioButtonTooltip : TooltipController
{
	public Tooltip_Text tooltipPrefab;

	public string textTrue;

	public string textFalse;

	private Tooltip_Text spawnedTooltip;

	private UIRadioButton radioButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIRadioButtonTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
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
