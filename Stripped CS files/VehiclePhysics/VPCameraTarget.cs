using System;
using System.Collections.Generic;
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

		public bool enabled = true;

		public string onEnableMessage;

		public KeyCode key;
	}

	public Transform lookAtPoint;

	public Transform attachToPoint;

	[Space(5f)]
	public float viewDistance = 10f;

	public float viewHeight = 3.5f;

	public float viewDamping = 3f;

	public float viewMinDistance = 3.8f;

	[FormerlySerializedAs("viewMinHeight")]
	public float viewMinAngle;

	public float targetRadius = 5f;

	[Space(5f)]
	public bool useCustomCameras;

	public int currentCustomCamera;

	public CustomCamera[] customCameras = new CustomCamera[0];

	public CustomCamera GetCustomCamera(ref int targetCamIndex)
	{
		if (targetCamIndex < 0)
		{
			targetCamIndex = 0;
		}
		if (targetCamIndex >= customCameras.Length)
		{
			targetCamIndex = customCameras.Length - 1;
		}
		if (targetCamIndex >= 0)
		{
			return customCameras[targetCamIndex];
		}
		return null;
	}

	public int FindEnabledCamera(int cameraIndex)
	{
		if (customCameras.Length == 0)
		{
			return -1;
		}
		int num = customCameras.Length;
		while (true)
		{
			if (num > 0)
			{
				if (cameraIndex < 0 || cameraIndex >= customCameras.Length)
				{
					cameraIndex = 0;
				}
				if (customCameras[cameraIndex].enabled)
				{
					break;
				}
				cameraIndex++;
				num--;
				continue;
			}
			return -1;
		}
		return cameraIndex;
	}

	public KeyCode MonitorCustomCameraKeys()
	{
		int num = 0;
		CustomCamera customCamera;
		while (true)
		{
			if (num < customCameras.Length)
			{
				customCamera = customCameras[num];
				if (customCamera.enabled && Input.GetKeyDown(customCamera.key))
				{
					break;
				}
				num++;
				continue;
			}
			return KeyCode.None;
		}
		return customCamera.key;
	}

	public int[] GetCamerasByKey(KeyCode key)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < customCameras.Length; i++)
		{
			CustomCamera customCamera = customCameras[i];
			if (customCamera.enabled && customCamera.key == key)
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}
}
