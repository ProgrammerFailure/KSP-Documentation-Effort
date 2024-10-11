using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Camera/Camera Controller")]
public class VPCameraController : MonoBehaviour
{
	public enum Mode
	{
		AttachTo,
		SmoothFollow,
		Orbit,
		LookAt,
		Free
	}

	public enum TimeMode
	{
		Standard,
		Unscaled,
		Smooth,
		RefreshRate
	}

	public Mode mode;

	public Transform target;

	public KeyCode changeCameraKey;

	public int customCameraIndex;

	public TimeMode timeMode;

	public CameraAttachTo attachTo;

	public CameraSmoothFollow smoothFollow;

	[FormerlySerializedAs("mouseOrbit")]
	public CameraOrbit orbit;

	public CameraLookAt lookAt;

	public CameraFree free;

	private Transform m_transform;

	private CameraMode[] m_cameraModes;

	private Transform m_currentTarget;

	private Mode m_prevMode;

	private int m_prevCustomCamera;

	private VPCameraTarget m_targetConfig;

	private Transform m_viewTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPCameraController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NextCameraMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTargetConfig(VPCameraTarget targetConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPose(Vector3 position, Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MonitorTargetChanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MonitorCustomCameraChanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MonitorCustomCameraKeys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int InitializeCustomCamera(int cameraIndex)
	{
		throw null;
	}
}
