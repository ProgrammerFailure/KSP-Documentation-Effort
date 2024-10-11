using System;
using System.Runtime.CompilerServices;
using Contracts;
using KSP.UI.Screens;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class CollectROCScienceRetrieval : ContractParameter
{
	protected CelestialBody targetBody;

	protected string subjectId;

	protected string rocType;

	protected string scienceTitle;

	public CelestialBody TargetBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SubjectId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScienceTitle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectROCScienceRetrieval()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollectROCScienceRetrieval(CelestialBody targetBody, string subjectId, string rocType, string scienceTitle)
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
	private void OnVesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		throw null;
	}
}
