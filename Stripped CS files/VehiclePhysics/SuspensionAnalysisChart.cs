using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class SuspensionAnalysisChart : PerformanceChart
{
	public DataLogger.Channel m_contactDepth;

	public DataLogger.Channel m_contactSpeed;

	public DataLogger.Channel m_suspensionForce;

	public DataLogger.Channel m_damperForce;

	public DataLogger.Channel m_suspensionTravel;

	public int monitoredWheel = 1;

	public override string Title()
	{
		return "Suspension Analysis";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 32f;
		base.dataLogger.bottomLimit = -24f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 20f, 8f);
	}

	public override void SetupChannels()
	{
		m_suspensionTravel = base.dataLogger.NewChannel("Suspension travel", GColor.Alpha(GColor.lime, 0.7f), 4f);
		m_suspensionTravel.alphaBlend = true;
		m_suspensionTravel.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 1f;
		m_suspensionTravel.valueFormat = "0 mm";
		m_suspensionTravel.captionPositionY = -0.100000024f;
		m_contactSpeed = base.dataLogger.NewChannel("Contact Speed", GColor.Alpha(GColor.lightBlue, 0.7f), 4f);
		m_contactSpeed.alphaBlend = true;
		m_contactSpeed.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 0.5f;
		m_contactSpeed.valueFormat = "0 mm/s";
		m_contactSpeed.captionPositionY = 1f;
		m_contactDepth = base.dataLogger.NewChannel("Contact Depth", GColor.Alpha(GColor.accentTeal, 0.7f), 4f);
		m_contactDepth.alphaBlend = true;
		m_contactDepth.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 1f;
		m_contactDepth.valueFormat = "0 mm";
		m_contactDepth.captionPositionY = 1.8f;
		m_damperForce = base.dataLogger.NewChannel("Damper force", GColor.Alpha(GColor.purple, 0.7f), 4f);
		m_damperForce.alphaBlend = true;
		m_damperForce.scale = 1f / (base.reference.maxSuspensionDistance * base.reference.maxSpringRate) * 2f;
		m_damperForce.valueFormat = "0 N";
		m_damperForce.captionPositionY = -1.7f;
		m_suspensionForce = base.dataLogger.NewChannel("Suspension force", GColor.Alpha(GColor.red, 0.7f), 4f);
		m_suspensionForce.alphaBlend = true;
		m_suspensionForce.scale = 1f / (base.reference.maxSuspensionDistance * base.reference.maxSpringRate) * 2f;
		m_suspensionForce.valueFormat = "0 N";
		m_suspensionForce.captionPositionY = -0.90000004f;
	}

	public override void RecordData()
	{
		VehicleBase.WheelState wheelState = base.vehicle.wheelState[monitoredWheel];
		if (wheelState.grounded)
		{
			m_contactDepth.Write(wheelState.contactDepth * 1000f);
			m_contactSpeed.Write(wheelState.contactSpeed * 1000f);
			m_suspensionForce.Write(wheelState.downforce);
			m_damperForce.Write(wheelState.damperForce);
		}
		m_suspensionTravel.Write(wheelState.wheelCol.lastRuntimeSuspensionTravel * 1000f);
	}
}
