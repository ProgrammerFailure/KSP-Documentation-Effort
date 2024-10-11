using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RDTech : MonoBehaviour
{
	public enum State
	{
		Unavailable,
		Available
	}

	public enum OperationResult
	{
		Successful,
		NotEnoughFunds,
		ScienceCostLimitExceeded,
		Failure
	}

	public string techID;

	public int scienceCost;

	public string title;

	public string description;

	public List<AvailablePart> partsAssigned;

	public List<AvailablePart> partsPurchased;

	public State state;

	public ResearchAndDevelopment host;

	private ProtoTechNode techState;

	public bool hideIfNoParts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Warmup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OperationResult ResearchTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnlockTech(bool updateGameState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PartIsPurchased(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PurchasePart(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void HandlePurchase(AvailablePart partInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AutoPurchaseAllParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
