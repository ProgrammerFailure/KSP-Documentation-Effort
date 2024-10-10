using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class AbsDiagnosticsChart : PerformanceChart
{
	public DataLogger.Channel m_speed;

	public DataLogger.Channel m_brakePedal;

	public DataLogger.Channel m_brakeTorque;

	public DataLogger.Channel m_wheelSpin;

	public DataLogger.Channel m_longitudinalG;

	public DataLogger.Channel m_slip;

	public int monitoredWheel;

	public float maxBrakeTorque = 3200f;

	public override string Title()
	{
		return "Abs Diagnostics";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 12f;
		base.dataLogger.bottomLimit = -0.5f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 30f, 12.5f);
	}

	public override void SetupChannels()
	{
		m_slip = base.dataLogger.NewChannel("Slip", GColor.Alpha(GColor.gray, 0.2f), 3f);
		m_slip.scale = 0.1f;
		m_slip.valueFormat = "0.0 m/s";
		m_brakePedal = base.dataLogger.NewChannel("Brake pedal", GColor.Alpha(GColor.red, 0.6f));
		m_brakePedal.alphaBlend = true;
		m_brakePedal.valueFormat = "0 %";
		m_brakePedal.scale = 2.5f;
		m_brakePedal.captionPositionY = 1.9f;
		m_brakeTorque = base.dataLogger.NewChannel("Brake torque", GColor.Alpha(GColor.orange, 1f));
		m_brakeTorque.alphaBlend = true;
		m_brakeTorque.valueFormat = "0 Nm";
		m_brakeTorque.scale = 1f / maxBrakeTorque * 2.5f;
		m_longitudinalG = base.dataLogger.NewChannel("Acceleration", GColor.blue, 10f);
		m_longitudinalG.scale = 1f / base.reference.maxAccelerationG * 3f;
		m_longitudinalG.valueFormat = "0.00 G";
		m_longitudinalG.captionPositionY = -0.2f;
		VehicleBase.WheelState wheelState = base.vehicle.wheelState[monitoredWheel];
		m_wheelSpin = base.dataLogger.NewChannel(wheelState.wheelCol.name, GColor.Alpha(PerformanceChart.wheelChartColors[monitoredWheel], 0.7f), 4f);
		m_wheelSpin.alphaBlend = true;
		m_wheelSpin.scale = 1f / (base.reference.maxSpeed * 3.6f) * 5f;
		m_wheelSpin.valueFormat = "0.0 km/h";
		m_wheelSpin.captionPositionY = 1.9f;
		m_speed = base.dataLogger.NewChannel("Reference Speed", GColor.cyan, 4f);
		m_speed.scale = 1f / (base.reference.maxSpeed * 3.6f) * 5f;
		m_speed.valueFormat = "0.0 km/h";
	}

	public override void RecordData()
	{
		float num = (float)base.vehicle.data.Get(1)[0] / 1000f;
		m_speed.Write(num * 3.6f);
		float value = (float)base.vehicle.data.Get(0)[2] / 10000f;
		m_brakePedal.Write(value);
		VehicleBase.WheelState wheelState = base.vehicle.wheelState[monitoredWheel];
		m_wheelSpin.Write(wheelState.angularVelocity * wheelState.wheelCol.radius * 3.6f);
		m_brakeTorque.Write(wheelState.brakeTorque);
		m_longitudinalG.Write(base.vehicle.localAcceleration.z / 9.807f);
		if (wheelState.tireSlip.y > -0.1f)
		{
			m_slip.Write(wheelState.tireSlip.y);
		}
	}
}
