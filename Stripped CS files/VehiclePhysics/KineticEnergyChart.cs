using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class KineticEnergyChart : PerformanceChart
{
	public DataLogger.Channel m_totalEnergy;

	public DataLogger.Channel m_linearEnergy;

	public DataLogger.Channel m_angularEnergy;

	public DataLogger.Channel m_totalEnergyDelta;

	public DataLogger.Channel m_linearEnergyDelta;

	public DataLogger.Channel m_angularEnergyDelta;

	public float m_lastTotalEnergy;

	public float m_lastLinearEnergy;

	public float m_lastAngularEnergy;

	public override string Title()
	{
		return "Kinetic Energy";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 25f;
		base.dataLogger.bottomLimit = -12.5f;
		m_lastTotalEnergy = RigidbodyUtility.GetNormalizedKineticEnergy(base.vehicle.cachedRigidbody);
		m_lastLinearEnergy = RigidbodyUtility.GetNormalizedLinearKineticEnergy(base.vehicle.cachedRigidbody);
		m_lastAngularEnergy = RigidbodyUtility.GetNormalizedAngularKineticEnergy(base.vehicle.cachedRigidbody);
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 30f, 13.5f);
	}

	public override void SetupChannels()
	{
		float num = 1f / (0.5f * base.reference.maxSpeed * base.reference.maxSpeed) * 12f;
		float scale = num * 8f;
		string valueFormat = "0.0 J";
		m_linearEnergy = base.dataLogger.NewChannel("Linear", GColor.accentGreen);
		m_linearEnergy.scale = num;
		m_linearEnergy.valueFormat = valueFormat;
		m_angularEnergy = base.dataLogger.NewChannel("Angular", GColor.accentCyan);
		m_angularEnergy.scale = num;
		m_angularEnergy.valueFormat = valueFormat;
		m_angularEnergy.captionPositionY = 1.9f;
		m_totalEnergy = base.dataLogger.NewChannel("Total", GColor.accentRed);
		m_totalEnergy.scale = num;
		m_totalEnergy.valueFormat = valueFormat;
		m_totalEnergy.captionPositionY = 2.8f;
		m_linearEnergyDelta = base.dataLogger.NewChannel("Linear Δ", GColor.green, 8f);
		m_linearEnergyDelta.scale = scale;
		m_linearEnergyDelta.valueFormat = valueFormat;
		m_angularEnergyDelta = base.dataLogger.NewChannel("Angular Δ", GColor.cyan, 8f);
		m_angularEnergyDelta.scale = scale;
		m_angularEnergyDelta.valueFormat = valueFormat;
		m_angularEnergyDelta.captionPositionY = 1.9f;
		m_totalEnergyDelta = base.dataLogger.NewChannel("Total Δ", GColor.red, 8f);
		m_totalEnergyDelta.scale = scale;
		m_totalEnergyDelta.valueFormat = valueFormat;
		m_totalEnergyDelta.captionPositionY = 2.8f;
	}

	public override void RecordData()
	{
		float normalizedKineticEnergy = RigidbodyUtility.GetNormalizedKineticEnergy(base.vehicle.cachedRigidbody);
		m_totalEnergy.Write(normalizedKineticEnergy);
		m_totalEnergyDelta.Write(normalizedKineticEnergy - m_lastTotalEnergy);
		m_lastTotalEnergy = normalizedKineticEnergy;
		float normalizedLinearKineticEnergy = RigidbodyUtility.GetNormalizedLinearKineticEnergy(base.vehicle.cachedRigidbody);
		m_linearEnergy.Write(normalizedLinearKineticEnergy);
		m_linearEnergyDelta.Write(normalizedLinearKineticEnergy - m_lastLinearEnergy);
		m_lastLinearEnergy = normalizedLinearKineticEnergy;
		float normalizedAngularKineticEnergy = RigidbodyUtility.GetNormalizedAngularKineticEnergy(base.vehicle.cachedRigidbody);
		m_angularEnergy.Write(normalizedAngularKineticEnergy);
		m_angularEnergyDelta.Write(normalizedAngularKineticEnergy - m_lastAngularEnergy);
		m_lastAngularEnergy = normalizedAngularKineticEnergy;
	}
}
