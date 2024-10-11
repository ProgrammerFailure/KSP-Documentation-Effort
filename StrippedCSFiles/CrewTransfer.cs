using System.Runtime.CompilerServices;

public class CrewTransfer : PartItemTransfer
{
	public class CrewTransferData
	{
		public bool canTransfer;

		public Part sourcePart;

		public Part destPart;

		public ProtoCrewMember crewMember;

		public CrewTransfer transfer;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CrewTransferData()
		{
			throw null;
		}
	}

	public ProtoCrewMember crew;

	public Part tgtPart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewTransfer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CrewTransfer Create(Part srcPart, ProtoCrewMember crewMember, Callback<DismissAction, Part> onDialogDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HookAdditionalEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UnhookAdditionalEvents()
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
	protected override void AfterPartsFound()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPartSelect(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onGoForEva(GameEvents.HostedFromToAction<ProtoCrewMember, Part> fromto)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onChangeInactive(ProtoCrewMember pcm, bool from, bool to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void MoveCrewTo(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void waitAndCompleteTransfer()
	{
		throw null;
	}
}
