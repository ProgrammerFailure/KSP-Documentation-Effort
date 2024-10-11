using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class ActionGroupList
{
	public List<bool>[] groupStates;

	public List<bool> groups;

	public List<double> cooldownTimes;

	public Vessel v;

	public bool this[KSPActionGroup group]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionGroupList(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleGroup(KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGroup(KSPActionGroup group, bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double CooldownTimeRemaining(KSPActionGroup group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyFrom(ActionGroupList acl)
	{
		throw null;
	}
}
