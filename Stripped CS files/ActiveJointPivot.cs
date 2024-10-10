using System;
using UnityEngine;

[Serializable]
public class ActiveJointPivot : ActiveJoint
{
	public SoftJointLimit pivotLimit;

	public SoftJointLimitSpring pivotLimitSpring;

	public float pivotRange = 60f;

	public static ActiveJointPivot Create(IActiveJointHost moduleHost, string refNodeId)
	{
		ActiveJointPivot activeJointPivot = new ActiveJointPivot();
		activeJointPivot.moduleHost = moduleHost;
		activeJointPivot.jointMode = JointMode.Pivot;
		activeJointPivot.startForRefNode(refNodeId);
		return activeJointPivot;
	}

	public static ActiveJointPivot Create(IActiveJointHost moduleHost, AttachNode refNode)
	{
		ActiveJointPivot activeJointPivot = new ActiveJointPivot();
		activeJointPivot.moduleHost = moduleHost;
		activeJointPivot.jointMode = JointMode.Pivot;
		activeJointPivot.refNode = refNode;
		activeJointPivot.startForRefNode(refNode.id);
		return activeJointPivot;
	}

	public override void onJointInit(bool jointExists)
	{
		if (jointExists)
		{
			setupPivot();
		}
	}

	public void setupPivot()
	{
		pivotLimit = base.joint.angularYLimit;
		pivotLimitSpring = base.joint.angularYZLimitSpring;
		base.joint.angularYMotion = ConfigurableJointMotion.Limited;
		base.joint.angularZMotion = ConfigurableJointMotion.Limited;
		pivotLimitSpring.spring = float.MaxValue;
		pivotLimitSpring.damper = 0f;
		pivotLimit.bounciness = 0.1f;
		pivotLimit.limit = pivotRange;
		applyPivotLimit();
	}

	public void SetPivotAngleLimit(float maxDegrees)
	{
		pivotRange = maxDegrees;
		if (base.driveMode != 0)
		{
			pivotLimit.limit = maxDegrees;
			applyPivotLimit();
		}
	}

	public void applyPivotLimit()
	{
		base.joint.angularYLimit = pivotLimit;
		base.joint.angularZLimit = pivotLimit;
		base.joint.angularYZLimitSpring = pivotLimitSpring;
	}

	public Vector3 GetControlAxis(Vector3 refAxis)
	{
		return getControlOrt(refAxis, PartSpaceMode.Pristine);
	}

	public Quaternion GetLocalPivotRotation()
	{
		return Quaternion.Inverse(moduleHost.GetLocalTransform().rotation) * base.pJoint.Host.partTransform.rotation * Quaternion.LookRotation(base.pJoint.Axis, base.pJoint.SecAxis);
	}

	public void SetLocalPivotTgtRotation(Quaternion tgtRot)
	{
		_ = base.driveMode;
	}
}
