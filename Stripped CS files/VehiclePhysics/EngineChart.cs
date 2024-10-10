using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class EngineChart : PerformanceChart
{
	public DataLogger.Channel m_rpm;

	public DataLogger.Channel m_load;

	public DataLogger.Channel m_speed;

	public DataLogger.Channel m_gear;

	public DataLogger.Channel m_torque;

	public DataLogger.Channel m_power;

	public DataLogger.Channel m_fuelRate;

	public override string Title()
	{
		return "Engine Performance";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 10f;
		base.dataLogger.bottomLimit = -0.5f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 30f, 10.5f);
	}

	public override void SetupChannels()
	{
		m_load = base.dataLogger.NewChannel("Load", GColor.lightGreen);
		m_load.valueFormat = "0 %";
		m_load.scale = 0.9f;
		m_fuelRate = base.dataLogger.NewChannel("Fuel", GColor.indigo, 1f);
		m_fuelRate.valueFormat = "0.0 g/kWh";
		m_fuelRate.scale = 0.005f;
		m_power = base.dataLogger.NewChannel("Power", GColor.lightBlue * 0.8f, 1f);
		m_power.scale = 1f / base.reference.maxPower * 7f;
		m_power.valueFormat = "0.0  kW";
		m_power.captionPositionY = 1.8f;
		m_torque = base.dataLogger.NewChannel("Torque", GColor.green, 1f);
		m_torque.scale = 1f / base.reference.maxTorque * 3f;
		m_torque.valueFormat = "0.0  Nm";
		m_torque.captionPositionY = 2.6f;
		m_rpm = base.dataLogger.NewChannel("RPM", GColor.red, 1f);
		m_rpm.scale = 1f / base.reference.maxRpm * 7f;
		m_rpm.valueFormat = "0.0 rpm";
		m_rpm.captionPositionY = 3.4f;
		m_gear = base.dataLogger.NewChannel("Gear", GColor.indigo, 8f);
		m_gear.showSegmentBegin = true;
		m_gear.scale = 1f / (float)base.reference.numGears * 0.9f;
		m_gear.valueFormat = "0";
		m_gear.captionPositionY = 1.8f;
		m_speed = base.dataLogger.NewChannel("Speed", GColor.cyan, 8f);
		m_speed.scale = 1f / (base.reference.maxSpeed * 3.6f) * 2f;
		m_speed.valueFormat = "0.0 km/h";
	}

	public override void RecordData()
	{
		int[] array = base.vehicle.data.Get(1);
		float num = (float)array[6] / 1000f;
		float num2 = (float)array[9] / 1000f;
		float value = (float)array[1] / 1000f;
		float num3 = (float)array[7] / 1000f;
		float num4 = (float)array[8] / 1000f;
		bool flag = array[14] != 0;
		int num5 = array[12];
		float num6 = (float)array[0] / 1000f;
		if (num >= 0f)
		{
			m_load.Write(num);
		}
		float num7 = num2 / num4 * 3600f;
		if (num7 > 0f && num7 < 500f)
		{
			m_fuelRate.Write(num7);
		}
		m_rpm.Write(value);
		if (num3 >= 0f)
		{
			m_torque.Write(num3);
		}
		if (num4 >= 0f)
		{
			m_power.Write(num4);
		}
		if (!flag)
		{
			m_gear.Write(num5);
		}
		m_speed.Write(num6 * 3.6f);
	}
}
