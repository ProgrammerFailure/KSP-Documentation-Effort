using System.Runtime.CompilerServices;
using UnityEngine;

public static class ConfigurableJointExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetTargetRotationInternal(this ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Quaternion GetExternalToJointSpaceRotation(this ConfigurableJoint joint)
	{
		throw null;
	}
}
