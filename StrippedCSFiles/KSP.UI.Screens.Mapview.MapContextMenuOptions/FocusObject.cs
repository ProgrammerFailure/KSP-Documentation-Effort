using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Mapview.MapContextMenuOptions;

public class FocusObject : MapContextMenuOption
{
	public enum FocusMode
	{
		OwnedVessel,
		UnownedVessel,
		CelestialBody
	}

	private Vessel vessel;

	private CelestialBody celestialBody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FocusObject(OrbitDriver tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FocusMode GetMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSelect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnCheckEnabled(out string fbText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CheckAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TryViewInTrackingStation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTrackingStationProceed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onTrackingStationDismiss()
	{
		throw null;
	}
}
