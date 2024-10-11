using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

public abstract class KbAppVessel : KbApp
{
	protected Vessel vessel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected KbAppVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPanelHeaderTap(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksChange(GameEvents.FromToAction<ControlTypes, ControlTypes> iChg)
	{
		throw null;
	}
}
