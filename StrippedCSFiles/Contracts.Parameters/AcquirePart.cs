using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class AcquirePart : ContractParameter
{
	public enum CompleteCondition
	{
		All,
		Any
	}

	[SerializeField]
	private List<uint> partsToRecover;

	[SerializeField]
	private string title;

	[SerializeField]
	private CompleteCondition winCondition;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AcquirePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AcquirePart(string title, CompleteCondition winCondition = CompleteCondition.All)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTitle(string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCouple(GameEvents.FromToAction<Part, Part> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDied(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPart(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePart(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScanVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckNewPart(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyPart(uint id)
	{
		throw null;
	}
}
