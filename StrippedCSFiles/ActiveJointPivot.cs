using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ActiveJointPivot : ActiveJoint
{
	private SoftJointLimit pivotLimit;

	private SoftJointLimitSpring pivotLimitSpring;

	private float pivotRange;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActiveJointPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ActiveJointPivot Create(IActiveJointHost moduleHost, string refNodeId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ActiveJointPivot Create(IActiveJointHost moduleHost, AttachNode refNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void onJointInit(bool jointExists)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPivotAngleLimit(float maxDegrees)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void applyPivotLimit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetControlAxis(Vector3 refAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion GetLocalPivotRotation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLocalPivotTgtRotation(Quaternion tgtRot)
	{
		throw null;
	}
}
