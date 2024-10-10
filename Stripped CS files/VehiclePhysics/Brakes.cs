using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehiclePhysics;

public class Brakes
{
	public enum BrakeCircuit
	{
		Neutral,
		Front,
		Rear,
		NoBrakes
	}

	public enum LateralPosition
	{
		Undefined,
		Left,
		Right
	}

	public enum AbsMode
	{
		Simple,
		MultiPosition,
		Continuous
	}

	public enum AbsTrigger
	{
		PeakSlipOffset,
		CustomSlip
	}

	public enum AbsOverride
	{
		None,
		ForceEnabled,
		ForceDisabled
	}

	[Serializable]
	public class Settings
	{
		public float maxBrakeTorque = 2000f;

		[Range(0f, 1f)]
		public float brakeBias = 0.7f;

		public float handbrakeTorque = 1500f;

		[Range(0f, 1f)]
		public float handbrakeAxle;
	}

	[Serializable]
	public class AbsSettings
	{
		public bool enabled;

		public AbsMode mode;

		public AbsTrigger trigger;

		public float minSlipOffset = 0.3f;

		public float maxSlipOffset = 1.5f;

		public float minSlip = 0.5f;

		public float maxSlip = 5f;

		[Range(0f, 1f)]
		public float minPressureRatio = 0.25f;

		[Range(2f, 8f)]
		public int valvePositions = 2;
	}

	public class WheelData
	{
		public float positionRatio;

		public Wheel wheel;

		public VehicleBase.WheelState wheelState;

		public LateralPosition lateralPosition;

		public float externalBrakeRatio;
	}

	public float brakeInput;

	public float handbrakeInput;

	public Settings settings = new Settings();

	public AbsSettings absSettings = new AbsSettings();

	public List<WheelData> m_wheelData = new List<WheelData>();

	public float m_absActivationTime = -1f;

	public bool sensorAbsEngaged => Time.time - m_absActivationTime < 0.25f;

	public AbsOverride absOverride { get; set; }

	public void AddWheel(VehicleBase.WheelState wheelState, Wheel wheel, float relPosition = 0f, LateralPosition lateralPosition = LateralPosition.Undefined)
	{
		WheelData wheelData = new WheelData();
		wheelData.positionRatio = Mathf.Clamp01(0.5f * (relPosition + 1f));
		wheelData.wheel = wheel;
		wheelData.wheelState = wheelState;
		wheelData.lateralPosition = lateralPosition;
		m_wheelData.Add(wheelData);
	}

	public void AddWheel(VehicleBase.WheelState wheelState, Wheel wheel, BrakeCircuit circuit, LateralPosition lateralPosition = LateralPosition.Undefined)
	{
		if (circuit != BrakeCircuit.NoBrakes)
		{
			float relPosition = 0f;
			switch (circuit)
			{
			case BrakeCircuit.Front:
				relPosition = 1f;
				break;
			case BrakeCircuit.Rear:
				relPosition = -1f;
				break;
			}
			AddWheel(wheelState, wheel, relPosition, lateralPosition);
		}
	}

	public void AddBrakeRatio(float ratio, BrakeCircuit circuit, LateralPosition lateralPosition)
	{
		if (ratio <= 0f)
		{
			return;
		}
		int i = 0;
		for (int count = m_wheelData.Count; i < count; i++)
		{
			WheelData wheelData = m_wheelData[i];
			if (((circuit == BrakeCircuit.Front && wheelData.positionRatio > 0.6f) || (circuit == BrakeCircuit.Rear && wheelData.positionRatio < 0.4f)) && (lateralPosition == wheelData.lateralPosition || lateralPosition == LateralPosition.Undefined))
			{
				wheelData.externalBrakeRatio += ratio;
			}
		}
	}

	public void DoUpdate()
	{
		float num = settings.handbrakeTorque * handbrakeInput;
		float num2 = Mathf.Clamp01(settings.brakeBias);
		float num3 = 1f - num2;
		float num4 = Mathf.Clamp01(2f * settings.handbrakeAxle);
		float num5 = Mathf.Clamp01(2f * (1f - settings.handbrakeAxle));
		int i = 0;
		for (int count = m_wheelData.Count; i < count; i++)
		{
			WheelData wheelData = m_wheelData[i];
			float num6 = wheelData.positionRatio * num2 + (1f - wheelData.positionRatio) * num3;
			float num7 = wheelData.positionRatio * num4 + (1f - wheelData.positionRatio) * num5;
			float num8 = Mathf.Max(brakeInput, wheelData.externalBrakeRatio);
			float num9 = settings.maxBrakeTorque * num8;
			wheelData.externalBrakeRatio = 0f;
			if (((absSettings.enabled && absOverride != AbsOverride.ForceDisabled) || absOverride == AbsOverride.ForceEnabled) && num8 > 0.1f)
			{
				float valvePressureRatio = GetValvePressureRatio(wheelData);
				if (valvePressureRatio < 1f)
				{
					num6 *= valvePressureRatio * wheelData.wheel.grip;
					m_absActivationTime = Time.time;
				}
			}
			wheelData.wheel.AddBrakeTorque(Mathf.Max(num9 * num6, num * num7));
		}
	}

	public float GetValvePressureRatio(WheelData wd)
	{
		float result = 1f;
		float num;
		float num2;
		if (absSettings.trigger == AbsTrigger.PeakSlipOffset)
		{
			float y = wd.wheel.tireFriction.GetPeakSlipBounds(wd.wheel.contactPatch).y;
			num = y + absSettings.minSlipOffset;
			num2 = y + absSettings.maxSlipOffset;
		}
		else
		{
			num = absSettings.minSlip;
			num2 = absSettings.maxSlip;
		}
		float y2 = wd.wheel.tireFriction.GetAdherentSlipBounds(wd.wheel.contactPatch).y;
		if (num < y2)
		{
			num = y2;
		}
		if (num2 < y2)
		{
			num = y2;
		}
		float y3 = wd.wheelState.tireSlip.y;
		if (y3 <= num)
		{
			return 1f;
		}
		switch (absSettings.mode)
		{
		case AbsMode.Simple:
			result = absSettings.minPressureRatio;
			break;
		case AbsMode.MultiPosition:
		{
			if (y3 >= num2)
			{
				result = absSettings.minPressureRatio;
				break;
			}
			float num3 = (num2 - num) / (float)(absSettings.valvePositions - 1);
			int num4 = (int)((y3 - num) / num3);
			float num5 = (1f - absSettings.minPressureRatio) / (float)absSettings.valvePositions;
			result = absSettings.minPressureRatio + num5 * (float)(absSettings.valvePositions - num4 - 1);
			break;
		}
		case AbsMode.Continuous:
			result = 1f - (1f - absSettings.minPressureRatio) * Mathf.InverseLerp(num, num2, wd.wheelState.tireSlip.y);
			break;
		}
		return result;
	}
}
