using System;
using UnityEngine;

namespace VehiclePhysics;

public class StabilityControl
{
	[Serializable]
	public class Settings
	{
		public bool enabled;

		public float minSpeed = 6f;

		public float understeerMinRate = 0.5f;

		public float understeerMaxRate = 1.5f;

		public float understeerMinSpeed = 6f;

		public float oversteerMinAngle = 4f;

		public float oversteerMaxAngle = 12f;

		public float oversteerMinSpeed = 10f;
	}

	public enum Override
	{
		None,
		ForceEnabled,
		ForceDisabled
	}

	public Settings settings = new Settings();

	public float wheelbase = 1f;

	public float stateVehicleSpeed;

	public float stateVehicleSpeedAngle;

	public float stateVehicleRotationRate;

	public float stateVehicleSteeringAngle;

	public float m_espBrakeFL;

	public float m_espBrakeFR;

	public float m_espBrakeRL;

	public float m_espBrakeRR;

	public bool sensorEngaged { get; set; }

	public float sensorUndersteerL { get; set; }

	public float sensorUndersteerR { get; set; }

	public float sensorOversteerL { get; set; }

	public float sensorOversteerR { get; set; }

	public float sensorBrakeFL { get; set; }

	public float sensorBrakeFR { get; set; }

	public float sensorBrakeRL { get; set; }

	public float sensorBrakeRR { get; set; }

	public Override escOverride { get; set; }

	public void DoUpdate()
	{
		if ((!settings.enabled || escOverride == Override.ForceDisabled) && escOverride != Override.ForceEnabled)
		{
			sensorUndersteerL = 0f;
			sensorUndersteerR = 0f;
			sensorOversteerL = 0f;
			sensorOversteerR = 0f;
			sensorBrakeFL = 0f;
			sensorBrakeFR = 0f;
			sensorBrakeRL = 0f;
			sensorBrakeRR = 0f;
			sensorEngaged = false;
			return;
		}
		float num = stateVehicleSpeed * Mathf.Sin(stateVehicleSteeringAngle * ((float)Math.PI / 180f)) / wheelbase;
		if (stateVehicleSpeed > settings.understeerMinSpeed)
		{
			sensorUndersteerL = num - Mathf.Max(0f, stateVehicleRotationRate);
			if (sensorUndersteerL < 0f)
			{
				sensorUndersteerL = 0f;
			}
			sensorUndersteerR = Mathf.Min(0f, stateVehicleRotationRate) - num;
			if (sensorUndersteerR < 0f)
			{
				sensorUndersteerR = 0f;
			}
		}
		else
		{
			sensorUndersteerL = 0f;
			sensorUndersteerR = 0f;
		}
		if (stateVehicleSpeed > settings.oversteerMinSpeed)
		{
			sensorOversteerL = stateVehicleSpeedAngle;
			if (sensorOversteerL < 0f)
			{
				sensorOversteerL = 0f;
			}
			sensorOversteerR = 0f - stateVehicleSpeedAngle;
			if (sensorOversteerR < 0f)
			{
				sensorOversteerR = 0f;
			}
		}
		else
		{
			sensorOversteerL = 0f;
			sensorOversteerR = 0f;
		}
		m_espBrakeFL = Mathf.InverseLerp(settings.oversteerMinAngle, settings.oversteerMaxAngle, sensorOversteerR);
		m_espBrakeFR = Mathf.InverseLerp(settings.oversteerMinAngle, settings.oversteerMaxAngle, sensorOversteerL);
		m_espBrakeRL = Mathf.InverseLerp(settings.understeerMinRate, settings.understeerMaxRate, sensorUndersteerR);
		m_espBrakeRR = Mathf.InverseLerp(settings.understeerMinRate, settings.understeerMaxRate, sensorUndersteerL);
		m_espBrakeRL = Mathf.Max(0f, m_espBrakeRL - m_espBrakeFR);
		m_espBrakeRR = Mathf.Max(0f, m_espBrakeRR - m_espBrakeFL);
		sensorBrakeFL = m_espBrakeFL;
		sensorBrakeFR = m_espBrakeFR;
		sensorBrakeRL = m_espBrakeRL;
		sensorBrakeRR = m_espBrakeRR;
		sensorEngaged = m_espBrakeFL > 0f || m_espBrakeFR > 0f || m_espBrakeRL > 0f || m_espBrakeRR > 0f;
	}
}
