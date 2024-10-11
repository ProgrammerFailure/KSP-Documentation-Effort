using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

[RequireComponent(typeof(PointerClickHandler))]
public class PartDropZone : TooltipController
{
	public string tooltipTitle;

	public string tooltipDefault;

	public string tooltipNoPart;

	public Tooltip_TitleAndText tooltipPrefab;

	private PointerClickHandler handler;

	public Callback<AvailablePart> onAddPart;

	public static PartDropZone Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartDropZone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ButtonInputDelegate(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTooltipSpawned(Tooltip instance)
	{
		throw null;
	}
}
