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

	public Mode mode = Mode.SmoothFollow;

	public Transform target;

	public KeyCode changeCameraKey = KeyCode.C;

	public int customCameraIndex;

	public TimeMode timeMode = TimeMode.Unscaled;

	public CameraAttachTo attachTo = new CameraAttachTo();

	public CameraSmoothFollow smoothFollow = new CameraSmoothFollow();

	[FormerlySerializedAs("mouseOrbit")]
	public CameraOrbit orbit = new CameraOrbit();

	public CameraLookAt lookAt = new CameraLookAt();

	public CameraFree free = new CameraFree();

	public Transform m_transform;

	public CameraMode[] m_cameraModes = new CameraMode[0];

	public Transform m_currentTarget;

	public Mode m_prevMode;

	public int m_prevCustomCamera = -1;

	public VPCameraTarget m_targetConfig;

	public Transform m_viewTarget;

	public void OnEnable()
	{
		m_transform = GetComponent<Transform>();
		m_cameraModes = new CameraMode[5] { attachTo, smoothFollow, orbit, lookAt, free };
		CameraMode[] cameraModes = m_cameraModes;
		for (int i = 0; i < cameraModes.Length; i++)
		{
			cameraModes[i].Initialize(m_transform);
		}
		MonitorTargetChanges();
		m_cameraModes[(int)mode].OnEnable(m_transform);
		m_cameraModes[(int)mode].Reset(m_transform, m_viewTarget);
		m_prevMode = mode;
	}

	public void OnDisable()
	{
		m_cameraModes[(int)mode].OnDisable(m_transform);
		m_currentTarget = null;
		m_viewTarget = null;
	}

	public void Update()
	{
		MonitorTargetChanges();
		if (Input.GetKeyDown(changeCameraKey))
		{
			NextCameraMode();
		}
		else if (m_targetConfig != null && m_targetConfig.useCustomCameras)
		{
			MonitorCustomCameraKeys();
		}
		else
		{
			for (int i = 0; i < m_cameraModes.Length; i++)
			{
				if (Input.GetKeyDown(m_cameraModes[i].hotKey))
				{
					mode = (Mode)i;
				}
			}
		}
		MonitorCustomCameraChanges();
	}

	public void LateUpdate()
	{
		if (mode != m_prevMode)
		{
			m_cameraModes[(int)m_prevMode].OnDisable(m_transform);
			m_cameraModes[(int)mode].OnEnable(m_transform);
			m_cameraModes[(int)mode].Reset(m_transform, m_viewTarget);
			m_prevMode = mode;
		}
		float deltaTime = ((timeMode == TimeMode.Unscaled) ? Time.unscaledDeltaTime : ((timeMode == TimeMode.Smooth) ? Time.smoothDeltaTime : ((timeMode != TimeMode.RefreshRate) ? Time.deltaTime : (1f / (float)Screen.currentResolution.refreshRate))));
		m_cameraModes[(int)mode].Update(m_transform, m_viewTarget, deltaTime);
	}

	public void NextCameraMode()
	{
		if (!base.enabled)
		{
			return;
		}
		if (m_targetConfig != null && m_targetConfig.useCustomCameras)
		{
			customCameraIndex++;
			return;
		}
		mode++;
		if ((int)mode >= m_cameraModes.Length)
		{
			mode = Mode.AttachTo;
		}
	}

	public void ResetCamera()
	{
		if (base.enabled)
		{
			m_cameraModes[(int)mode].Reset(m_transform, m_viewTarget);
		}
	}

	public void SetTargetConfig(VPCameraTarget targetConfig)
	{
		CameraMode[] cameraModes = m_cameraModes;
		for (int i = 0; i < cameraModes.Length; i++)
		{
			cameraModes[i].SetTargetConfig(targetConfig);
		}
	}

	public void SetPose(Vector3 position, Quaternion rotation)
	{
		if (base.enabled)
		{
			m_cameraModes[(int)mode].SetPose(m_transform, position, rotation);
		}
	}

	public void MonitorTargetChanges()
	{
		if (target != m_currentTarget)
		{
			if (target != null)
			{
				InitializeTarget();
				ResetCamera();
			}
			else
			{
				m_viewTarget = null;
				m_targetConfig = null;
			}
			m_currentTarget = target;
		}
	}

	public void MonitorCustomCameraChanges()
	{
		if (customCameraIndex == m_prevCustomCamera)
		{
			return;
		}
		if (m_targetConfig != null && m_targetConfig.useCustomCameras)
		{
			m_viewTarget = null;
			customCameraIndex = InitializeCustomCamera(customCameraIndex);
			if (m_viewTarget == null)
			{
				m_viewTarget = ((m_targetConfig.lookAtPoint != null) ? m_targetConfig.lookAtPoint : target);
			}
		}
		m_prevCustomCamera = customCameraIndex;
	}

	public void MonitorCustomCameraKeys()
	{
		KeyCode keyCode = m_targetConfig.MonitorCustomCameraKeys();
		if (keyCode == KeyCode.None)
		{
			return;
		}
		int[] camerasByKey = m_targetConfig.GetCamerasByKey(keyCode);
		if (camerasByKey.Length == 1)
		{
			customCameraIndex = camerasByKey[0];
		}
		else
		{
			if (camerasByKey.Length <= 1)
			{
				return;
			}
			int num = camerasByKey[0];
			for (int i = 0; i < camerasByKey.Length; i++)
			{
				if (customCameraIndex == camerasByKey[i])
				{
					if (i < camerasByKey.Length - 1)
					{
						num = camerasByKey[i + 1];
					}
					break;
				}
			}
			customCameraIndex = num;
		}
	}

	public void InitializeTarget()
	{
		m_targetConfig = target.GetComponent<VPCameraTarget>();
		if (m_targetConfig != null)
		{
			m_viewTarget = null;
			if (m_targetConfig.useCustomCameras)
			{
				customCameraIndex = InitializeCustomCamera(m_targetConfig.currentCustomCamera);
				m_prevCustomCamera = customCameraIndex;
			}
			if (m_viewTarget == null)
			{
				m_viewTarget = ((m_targetConfig.lookAtPoint != null) ? m_targetConfig.lookAtPoint : target);
			}
			SetTargetConfig(m_targetConfig);
		}
		else
		{
			m_viewTarget = target;
		}
	}

	public int InitializeCustomCamera(int cameraIndex)
	{
		cameraIndex = m_targetConfig.FindEnabledCamera(cameraIndex);
		VPCameraTarget.CustomCamera customCamera = m_targetConfig.GetCustomCamera(ref cameraIndex);
		if (customCamera != null)
		{
			mode = customCamera.mode;
			m_viewTarget = customCamera.reference;
			if (m_viewTarget != null && !string.IsNullOrEmpty(customCamera.onEnableMessage))
			{
				m_viewTarget.SendMessage(customCamera.onEnableMessage, SendMessageOptions.DontRequireReceiver);
			}
		}
		return cameraIndex;
	}
}
