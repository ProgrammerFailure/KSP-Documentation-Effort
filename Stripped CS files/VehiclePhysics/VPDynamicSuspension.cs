using System;
using System.Collections.Generic;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Dynamic Suspension", 22)]
public class VPDynamicSuspension : VehicleBehaviour
{
	public class SuspensionData
	{
		public VehicleBase.WheelState wheelState;

		public VPWheelCollider wheelCol;

		public float ratio;

		public float springRate;

		public float targetSpringRate;
	}

	public int[] axles = new int[0];

	[Range(0.01f, 2f)]
	[Space(5f)]
	public float suspensionDistance = 0.25f;

	[Range(0f, 1f)]
	public float targetCompression = 0.5f;

	[Range(0.1f, 2f)]
	public float changeRate = 1f;

	public bool ignoreEngineState;

	[Space(5f)]
	public float minSpringRate = 40000f;

	public float maxSpringRate = 200000f;

	[Range(0.5f, 120f)]
	[Space(5f)]
	public float fastUpdateInterval = 1f;

	[Range(0.5f, 120f)]
	public float slowUpdateInterval = 60f;

	[Range(0.1f, 1f)]
	[Space(5f)]
	public float inhibitWheelSleepFactor = 0.5f;

	public bool debugLabels;

	public float m_sumForce;

	public int m_numValues;

	public float m_lastAdjustTime;

	public List<SuspensionData> m_suspension = new List<SuspensionData>();

	[NonSerialized]
	public bool disableSuspensionUpdates;

	public override void OnEnableVehicle()
	{
		m_suspension.Clear();
		for (int i = 0; i < axles.Length; i++)
		{
			int wheelsInAxle = base.vehicle.GetWheelsInAxle(axles[i]);
			for (int j = 0; j < wheelsInAxle; j++)
			{
				int wheelIndex = base.vehicle.GetWheelIndex(axles[i], (VehicleBase.WheelPos)j);
				if (wheelIndex >= 0)
				{
					AddWheel(wheelIndex);
				}
			}
		}
		float num = 0f;
		int k = 0;
		for (int count = m_suspension.Count; k < count; k++)
		{
			num += m_suspension[k].wheelCol.springRate;
		}
		if (num < 1f)
		{
			DebugLogWarning("No axles configured or suspension spring is not properly configured at the vehicle. Component disabled.");
			base.enabled = false;
			return;
		}
		int l = 0;
		for (int count2 = m_suspension.Count; l < count2; l++)
		{
			m_suspension[l].ratio = m_suspension[l].wheelCol.springRate / num;
		}
		m_sumForce = 0f;
		m_numValues = 0;
	}

	public override int GetUpdateOrder()
	{
		return -100;
	}

	public override void UpdateVehicleSuspension()
	{
		bool flag = base.vehicle.data.Get(1, 3) != 0;
		if (ignoreEngineState || flag)
		{
			int i = 0;
			for (int count = m_suspension.Count; i < count; i++)
			{
				SuspensionData suspensionData = m_suspension[i];
				m_sumForce += suspensionData.wheelState.contactDepth * suspensionData.wheelCol.effectiveSpringRate;
			}
			m_numValues++;
			if (!disableSuspensionUpdates)
			{
				float num = ((Mathf.Abs((float)base.vehicle.data.Get(1, 0) / 1000f) < 0.05f) ? fastUpdateInterval : slowUpdateInterval);
				if (Time.fixedTime - m_lastAdjustTime >= num)
				{
					AdjustSuspension();
					m_sumForce = 0f;
					m_numValues = 0;
					m_lastAdjustTime = Time.fixedTime;
				}
				int j = 0;
				for (int count2 = m_suspension.Count; j < count2; j++)
				{
					SuspensionData suspensionData2 = m_suspension[j];
					float num2 = inhibitWheelSleepFactor * suspensionData2.springRate * 0.01f;
					if (MathUtility.FastAbs(suspensionData2.springRate - suspensionData2.targetSpringRate) > num2)
					{
						base.vehicle.inhibitWheelSleep = true;
					}
					float num3 = suspensionData2.springRate * changeRate;
					suspensionData2.springRate = Mathf.MoveTowards(suspensionData2.springRate, suspensionData2.targetSpringRate, num3 * Time.deltaTime);
				}
			}
		}
		else
		{
			m_sumForce = 0f;
			m_numValues = 0;
			m_lastAdjustTime = Time.fixedTime;
		}
		int k = 0;
		for (int count3 = m_suspension.Count; k < count3; k++)
		{
			SuspensionData suspensionData3 = m_suspension[k];
			suspensionData3.wheelCol.runtimeSpringRate = suspensionData3.springRate;
		}
	}

	public void AdjustNow()
	{
		if (base.vehicle == null || !base.vehicle.initialized)
		{
			return;
		}
		bool flag = base.vehicle.data.Get(1, 3) != 0;
		if (ignoreEngineState || flag)
		{
			if (Time.fixedTime - m_lastAdjustTime >= 1f)
			{
				AdjustSuspension();
				m_sumForce = 0f;
				m_numValues = 0;
				m_lastAdjustTime = Time.fixedTime;
			}
			else
			{
				DebugLogWarning("AdjustNow ignored - not enough samples collected (min 1 sec required)");
			}
		}
	}

	public void AddWheel(int wheelIndex)
	{
		SuspensionData suspensionData = new SuspensionData();
		suspensionData.wheelState = base.vehicle.wheelState[wheelIndex];
		suspensionData.wheelCol = suspensionData.wheelState.wheelCol;
		suspensionData.springRate = suspensionData.wheelCol.springRate;
		suspensionData.targetSpringRate = suspensionData.wheelCol.springRate;
		m_suspension.Add(suspensionData);
	}

	public void AdjustSuspension()
	{
		float num = m_sumForce / (float)m_numValues / (targetCompression * suspensionDistance);
		int i = 0;
		for (int count = m_suspension.Count; i < count; i++)
		{
			m_suspension[i].targetSpringRate = Mathf.Clamp(m_suspension[i].ratio * num, minSpringRate, maxSpringRate);
		}
	}
}
