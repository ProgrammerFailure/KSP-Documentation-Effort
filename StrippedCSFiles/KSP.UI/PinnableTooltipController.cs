using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace KSP.UI;

public class PinnableTooltipController : TooltipController, IPinnableTooltipController, ITooltipController, IPointerClickHandler, IEventSystemHandler
{
	protected bool pinned;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PinnableTooltipController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTooltipPinned()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTooltipUnpinned()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsPinned()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unpin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	string ITooltipController.get_name()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	void ITooltipController.set_name(string value)
	{
		throw null;
	}
}
