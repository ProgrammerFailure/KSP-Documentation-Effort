using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class AxisGroupsModule : VesselModule
{
	public Dictionary<int, BaseAxisFieldList> AxisGroups;

	private int groupOverride;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisGroupsModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddAxis(KSPAxisGroup axisGroup, BaseAxisField axisField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildAxisGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselWasModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselOverrideGroupChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Activation GetActivation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoadVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ShouldBeActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAxisGroup(KSPAxisGroup axisGroup, float axisValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IncrementAxisGroup(KSPAxisGroup axisGroup, float axisRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateAxisGroup(KSPAxisGroup axisGroup, float axisValue)
	{
		throw null;
	}
}
