using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehiclePhysics;

public class Steering
{
	public enum SteeringMode
	{
		Disabled,
		Steerable,
		Ratio,
		Reference
	}

	[Serializable]
	public class Settings
	{
		[Range(0f, 90f)]
		public float maxSteerAngle = 35f;

		[Range(-15f, 15f)]
		public float toeAngle;

		public bool ackerman;

		public Transform ackermanReference;

		public Transform ratioReference;
	}

	public class WheelData
	{
		public VehicleBase.WheelState wheelState;

		public Vector3 position;

		public SteeringMode mode;

		public float ratio;
	}

	public float steerInput;

	public Settings settings = new Settings();

	public List<WheelData> m_wheels = new List<WheelData>();

	public void AddWheel(VehicleBase.WheelState wheelState, Vector3 localPosition, SteeringMode steeringMode, float steerRatio = 1f)
	{
		if (steeringMode != 0)
		{
			WheelData wheelData = new WheelData();
			wheelData.wheelState = wheelState;
			wheelData.position = localPosition;
			wheelData.mode = steeringMode;
			wheelData.ratio = steerRatio;
			m_wheels.Add(wheelData);
			switch (steeringMode)
			{
			case SteeringMode.Steerable:
				wheelData.ratio = 1f;
				break;
			case SteeringMode.Reference:
				RecalculateRelativeSteerRatios();
				break;
			}
		}
	}

	public void AddWheel(VehicleBase.WheelState wheelState, Vector3 localPosition)
	{
		AddWheel(wheelState, localPosition, SteeringMode.Steerable);
	}

	public void RecalculateRelativeSteerRatios()
	{
		if (settings.ratioReference == null)
		{
			int i = 0;
			for (int count = m_wheels.Count; i < count; i++)
			{
				WheelData wheelData = m_wheels[i];
				if (wheelData.mode == SteeringMode.Reference)
				{
					wheelData.ratio = 1f;
				}
			}
			return;
		}
		float num = 0f;
		Vector3 localPosition = settings.ratioReference.localPosition;
		int j = 0;
		for (int count2 = m_wheels.Count; j < count2; j++)
		{
			WheelData wheelData2 = m_wheels[j];
			if (wheelData2.mode == SteeringMode.Reference)
			{
				float num2 = Mathf.Abs(wheelData2.position.z - localPosition.z);
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		int k = 0;
		for (int count3 = m_wheels.Count; k < count3; k++)
		{
			WheelData wheelData3 = m_wheels[k];
			if (wheelData3.mode == SteeringMode.Reference)
			{
				float num3 = wheelData3.position.z - localPosition.z;
				wheelData3.ratio = num3 / num;
			}
		}
	}

	public void DoUpdate()
	{
		steerInput = Mathf.Clamp(steerInput, -1f, 1f);
		float num = settings.maxSteerAngle * steerInput;
		if (settings.ackerman && settings.ackermanReference != null)
		{
			Vector3 localPosition = settings.ackermanReference.localPosition;
			int i = 0;
			for (int count = m_wheels.Count; i < count; i++)
			{
				WheelData wheelData = m_wheels[i];
				float num2 = wheelData.position.z - localPosition.z;
				float num3 = num2 / Mathf.Tan(num * wheelData.ratio * ((float)Math.PI / 180f));
				float steerAngle = Mathf.Atan(num2 / (num3 - wheelData.position.x + localPosition.x)) * 57.29578f;
				SetSteeringAngle(wheelData, steerAngle);
			}
		}
		else
		{
			int j = 0;
			for (int count2 = m_wheels.Count; j < count2; j++)
			{
				WheelData wheelData2 = m_wheels[j];
				SetSteeringAngle(wheelData2, num * wheelData2.ratio);
			}
		}
	}

	public void SetSteeringAngle(WheelData wd, float steerAngle)
	{
		wd.wheelState.steerAngle = steerAngle + ((wd.position.x < 0f) ? settings.toeAngle : (0f - settings.toeAngle));
	}
}
