using System;
using UnityEngine;

namespace VehiclePhysics;

public class AntiSpin
{
	[Serializable]
	public class Settings
	{
		public bool enabled;

		public float maxSpeed = 5.555556f;

		public float minRotationDiffRpm = 100f;

		public float maxRotationDiffRpm = 400f;

		public float maxBrakeTorque = 1000f;
	}

	public enum Override
	{
		None,
		ForceEnabled,
		ForceDisabled
	}

	public Settings settings = new Settings();

	public float stateVehicleSpeed;

	public float stateAngularVelocityL;

	public float stateAngularVelocityR;

	public bool sensorEngaged { get; set; }

	public float sensorBrakeTorqueL { get; set; }

	public float sensorBrakeTorqueR { get; set; }

	public Override asrOverride { get; set; }

	public void DoUpdate()
	{
		bool flag = false;
		float num = 0f;
		float num2 = 0f;
		if (((settings.enabled && asrOverride != Override.ForceDisabled) || asrOverride == Override.ForceEnabled) && Mathf.Abs(stateVehicleSpeed) < settings.maxSpeed)
		{
			float num3 = 0.5f * (stateAngularVelocityL + stateAngularVelocityR) * Block.WToRpm;
			float num4 = (stateAngularVelocityL - stateAngularVelocityR) * Block.WToRpm;
			float num5 = Mathf.Abs(num4);
			if (num5 > settings.minRotationDiffRpm)
			{
				float num6 = Mathf.InverseLerp(settings.minRotationDiffRpm, settings.maxRotationDiffRpm, num5);
				if (num3 < 0f)
				{
					num4 = 0f - num4;
				}
				if (num4 > 0f)
				{
					num = num6;
				}
				else
				{
					num2 = num6;
				}
				flag = true;
			}
		}
		sensorEngaged = flag;
		sensorBrakeTorqueL = num * settings.maxBrakeTorque;
		sensorBrakeTorqueR = num2 * settings.maxBrakeTorque;
	}
}
