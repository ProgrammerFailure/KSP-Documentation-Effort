using System.Runtime.CompilerServices;

public class ExperimentTransfer : PartItemTransfer
{
	public IScienceDataContainer container;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperimentTransfer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ExperimentTransfer Create(Part srcPart, IScienceDataContainer cont, Callback<DismissAction, Part> onDialogDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool IsSemiValidPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool IsValidPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPartSelect(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSrcPartSelect(Part p)
	{
		throw null;
	}
}
