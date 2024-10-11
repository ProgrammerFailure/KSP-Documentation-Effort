using System.Runtime.CompilerServices;
using Contracts;
using KSP.UI.Screens;

namespace FinePrint.Contracts.Parameters;

public class KerbalTourParameter : ContractParameter
{
	public string kerbalName;

	private ProtoCrewMember.Gender kerbalGender;

	private bool eventsAdded;

	private bool kerbalInactive;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalTourParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalTourParameter(string kerbalName, ProtoCrewMember.Gender kerbalGender)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetTitle()
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
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewKilled(EventReport evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnParameterStateChange(ContractParameter p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselRecovered(ProtoVessel v, MissionRecoveryDialog mrd, float x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnKerbalInactiveChange(ProtoCrewMember pcm, bool oldValue, bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetMessageFailed()
	{
		throw null;
	}
}
