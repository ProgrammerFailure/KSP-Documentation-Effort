using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.TooltipTypes;

public class TooltipController_Text : TooltipController
{
	public Tooltip_Text prefab;

	[TextArea(1, 2)]
	public string textString;

	public bool continuousUpdate;

	protected Tooltip_Text tooltipInstance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TooltipController_Text()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
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
	public override bool OnTooltipUpdate(Tooltip instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTooltipDespawned(Tooltip instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetText(string textString)
	{
		throw null;
	}
}
