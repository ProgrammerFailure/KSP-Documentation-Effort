using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class AxleSuspensionChart : PerformanceChart
{
	public DataLogger.Channel m_steerAngle;

	public DataLogger.Channel m_yawRate;

	public DataLogger.Channel m_roll;

	public DataLogger.Channel m_yawRateVsSteer;

	public DataLogger.Channel m_leftCompression;

	public DataLogger.Channel m_rightCompression;

	public DataLogger.Channel m_compressionDiff;

	public DataLogger.Channel m_leftSpring;

	public DataLogger.Channel m_rightSpring;

	public int m_monitoredAxle;

	public override string Title()
	{
		return "Axle Suspension";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 6.5f;
		base.dataLogger.bottomLimit = -0.5f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 20f, 7f);
	}

	public override void SetupChannels()
	{
		m_compressionDiff = base.dataLogger.NewChannel("Compression Diff", GColor.gray, 4.5f);
		m_compressionDiff.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 1f;
		m_compressionDiff.valueFormat = "0 mm";
		m_compressionDiff.captionPositionY = 1f;
		m_steerAngle = base.dataLogger.NewChannel("Steer Angle (avg)", GColor.Alpha(Color.Lerp(GColor.teal, GColor.green, 0.75f), 0.7f), 4.5f);
		m_steerAngle.scale = -1f / 35f;
		m_steerAngle.valueFormat = "0.00 °";
		m_steerAngle.alphaBlend = true;
		m_steerAngle.captionPositionY = 1.8f;
		m_roll = base.dataLogger.NewChannel("Roll", GColor.Alpha(GColor.teal, 0.7f), 4.5f);
		m_roll.scale = -0.1f;
		m_roll.valueFormat = "0.00 °";
		m_roll.alphaBlend = true;
		m_roll.captionPositionY = -0.100000024f;
		m_yawRate = base.dataLogger.NewChannel("Turn Rate", GColor.Alpha(GColor.red, 0.6f), 4.5f);
		m_yawRate.scale = -1f / 35f;
		m_yawRate.valueFormat = "0.0 °/s";
		m_yawRate.captionPositionY = -0.90000004f;
		m_yawRateVsSteer = base.dataLogger.NewChannel("Turn Rate vs. Steering", GColor.gray, 2f);
		m_yawRateVsSteer.scale = 1f / 15f;
		m_yawRateVsSteer.valueFormat = "0.00";
		m_leftCompression = base.dataLogger.NewChannel("Left Contact Depth", GColor.Alpha(GColor.accentYellow, 0.7f), 1f);
		m_leftCompression.alphaBlend = true;
		m_leftCompression.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 1f;
		m_leftCompression.valueFormat = "0 mm";
		m_leftCompression.captionPositionY = 1.8f;
		m_rightCompression = base.dataLogger.NewChannel("Right Contact Depth", GColor.Alpha(GColor.accentLightBlue, 0.7f), 1f);
		m_rightCompression.alphaBlend = true;
		m_rightCompression.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 1f;
		m_rightCompression.valueFormat = "0 mm";
		m_rightCompression.captionPositionY = 1f;
		m_leftSpring = base.dataLogger.NewChannel("Effective Left Spring", GColor.Alpha(GColor.yellowA100, 0.7f));
		m_leftSpring.alphaBlend = true;
		m_leftSpring.scale = 1f / (base.reference.maxSpringRate * 2f) * 0.9f;
		m_leftSpring.valueFormat = "0";
		m_leftSpring.captionPositionY = 1.8f;
		m_rightSpring = base.dataLogger.NewChannel("Effective Right Spring", GColor.Alpha(GColor.lightBlue200, 0.7f));
		m_rightSpring.alphaBlend = true;
		m_rightSpring.scale = 1f / (base.reference.maxSpringRate * 2f) * 0.9f;
		m_rightSpring.valueFormat = "0";
		m_rightSpring.captionPositionY = 1f;
	}

	public override void RecordData()
	{
		int wheelIndex = base.vehicle.GetWheelIndex(m_monitoredAxle);
		int wheelIndex2 = base.vehicle.GetWheelIndex(m_monitoredAxle, VehicleBase.WheelPos.Right);
		VehicleBase.WheelState wheelState = base.vehicle.wheelState[wheelIndex];
		VehicleBase.WheelState wheelState2 = base.vehicle.wheelState[wheelIndex2];
		float num = 0.5f * (wheelState.steerAngle + wheelState2.steerAngle);
		float num2 = base.vehicle.cachedRigidbody.angularVelocity.y * 57.29578f;
		float num3 = base.vehicle.cachedRigidbody.rotation.eulerAngles.z;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		m_steerAngle.Write(num);
		m_yawRate.Write(num2);
		if (Mathf.Abs(num3) < 15f)
		{
			m_roll.Write(num3);
		}
		if (wheelState.grounded)
		{
			m_leftCompression.Write(wheelState.contactDepth * 1000f);
		}
		if (wheelState2.grounded)
		{
			m_rightCompression.Write(wheelState2.contactDepth * 1000f);
		}
		m_compressionDiff.Write((wheelState2.contactDepth - wheelState.contactDepth) * 1000f);
		if (MathUtility.FastAbs(num) > 1f)
		{
			m_yawRateVsSteer.Write(num2 / num);
		}
		m_leftSpring.Write(wheelState.wheelCol.lastRuntimeSpringRate);
		m_rightSpring.Write(wheelState2.wheelCol.lastRuntimeSpringRate);
	}
}
