using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalCameraSwitch : InternalModule
{
	[KSPField]
	public string colliderTransformName;

	[KSPField]
	public string cameraTransformName;

	public Transform colliderTransform;

	public Transform cameraTransform;

	private Transform oldParent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalCameraSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Button_OnDoubleTap()
	{
		throw null;
	}
}
