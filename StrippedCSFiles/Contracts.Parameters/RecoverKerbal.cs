using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class RecoverKerbal : ContractParameter
{
	public enum CompleteCondition
	{
		All,
		Any
	}

	[SerializeField]
	private List<string> kerbalsToRecover;

	[SerializeField]
	private string title;

	[SerializeField]
	private CompleteCondition failCondition;

	[SerializeField]
	private CompleteCondition winCondition;

	private bool eventsAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RecoverKerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RecoverKerbal(string title, CompleteCondition winCondition = CompleteCondition.All, CompleteCondition failCondition = CompleteCondition.All)
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
	private void OnVesselRecovered(ProtoVessel v, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewKilled(EventReport evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDied(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddKerbal(string kerbalName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveKerbal(string kerbalName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTitle(string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselLoad(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlightReady()
	{
		throw null;
	}
}
