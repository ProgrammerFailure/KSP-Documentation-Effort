using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Mapview.MapContextMenuOptions;

public class SetAsTarget : MapContextMenuOption
{
	private readonly Func<ITargetable> getOTTarget;

	private ITargetable currTgt;

	private ITargetable tgt;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SetAsTarget(ITargetable tgt, Func<ITargetable> getOTTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnCheckEnabled(out string fbText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSelect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CheckAvailable()
	{
		throw null;
	}
}
