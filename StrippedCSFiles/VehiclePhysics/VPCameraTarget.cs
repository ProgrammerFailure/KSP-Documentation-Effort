using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Camera/Camera Target")]
public class VPCameraTarget : MonoBehaviour
{
	[Serializable]
	public class CustomCamera
	{
		public VPCameraController.Mode mode;

		public Transform reference;

		public bool enabled;

		public string onEnableMessage;

		public KeyCode key;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CustomCamera()
		{
			throw null;
		}
	}

	public Transform lookAtPoint;

	public Transform attachToPoint;

	[Space(5f)]
	public float viewDistance;

	public float viewHeight;

	public float viewDamping;

	public float viewMinDistance;

	[FormerlySerializedAs("viewMinHeight")]
	public float viewMinAngle;

	public float targetRadius;

	[Space(5f)]
	public bool useCustomCameras;

	public int currentCustomCamera;

	public CustomCamera[] customCameras;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPCameraTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CustomCamera GetCustomCamera(ref int targetCamIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int FindEnabledCamera(int cameraIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyCode MonitorCustomCameraKeys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int[] GetCamerasByKey(KeyCode key)
	{
		throw null;
	}
}
