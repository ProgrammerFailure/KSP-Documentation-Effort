using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class ReachSituation : ContractParameter
{
	public Vessel.Situations Situation;

	protected string title;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReachSituation(Vessel.Situations sit, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetTitleStringShort(Vessel.Situations sit)
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
	private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TrackVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool checkVesselSituation(Vessel v)
	{
		throw null;
	}
}
