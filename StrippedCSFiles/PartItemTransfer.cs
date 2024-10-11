using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PartItemTransfer : MonoBehaviour
{
	public enum DismissAction
	{
		Cancelled,
		Interrupted,
		ItemMoved
	}

	public static PartItemTransfer Instance;

	public Part srcPart;

	public List<Part> semiValidParts;

	public List<Part> validParts;

	public string type;

	public string semiValidMessage;

	public Callback<DismissAction, Part> onDismiss;

	public ScreenMessage scMsgInstruction;

	public ScreenMessage scMsgWarning;

	public List<PartSelector> partSelectors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartItemTransfer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(Part src, string itemType, string itemName, string semiValidMsg, Callback<DismissAction, Part> onDialogDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DismissInterrupted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DismissActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DismissAlreadyRunning()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Dismiss(DismissAction dma, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	protected abstract bool IsValidPart(Part p);

	protected abstract bool IsSemiValidPart(Part p);

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AfterPartsFound()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void HookAdditionalEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UnhookAdditionalEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onVesselChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSrcPartSelect(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSemiValidPartSelect(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnPartSelect(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LateUpdate()
	{
		throw null;
	}
}
