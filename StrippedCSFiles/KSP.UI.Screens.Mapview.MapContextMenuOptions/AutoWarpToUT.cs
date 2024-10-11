using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Mapview.MapContextMenuOptions;

public class AutoWarpToUT : MapContextMenuOption
{
	private double destUT;

	private bool featureUnlocked;

	private PatchedConics.PatchCastHit hit;

	private int patchesAheadLimit;

	private PatchedConicRenderer pcr;

	private double tgtUT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AutoWarpToUT(double destUT, PatchedConicRenderer pcr, PatchedConics.PatchCastHit hit, int patchesAheadLimit)
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
}
