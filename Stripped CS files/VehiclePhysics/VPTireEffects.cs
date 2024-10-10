using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Tire Effects", 2)]
public class VPTireEffects : VehicleBehaviour
{
	public class TireFxData
	{
		public VPGroundMarksRenderer lastRenderer;

		public int lastMarksIndex = -1;

		public float marksDelta;

		public VPGroundParticleEmitter lastEmitter;

		public float lastParticleTime = -1f;

		public float slipTime;
	}

	public float tireWidth = 0.2f;

	public float minSlip = 1f;

	public float maxSlip = 5f;

	[Range(0f, 2f)]
	[Header("Tire marks")]
	public float intensity = 1f;

	public float updateInterval = 0.02f;

	[Header("Smoke")]
	public float minIntensityTime = 0.5f;

	public float maxIntensityTime = 6f;

	public float limitIntensityTime = 10f;

	public TireFxData[] m_tireData = new TireFxData[0];

	public override void OnEnableVehicle()
	{
		m_tireData = new TireFxData[base.vehicle.wheelState.Length];
		for (int i = 0; i < m_tireData.Length; i++)
		{
			m_tireData[i] = new TireFxData();
		}
	}

	public override void UpdateVehicle()
	{
		int i = 0;
		for (int num = m_tireData.Length; i < num; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[i];
			TireFxData fxData = m_tireData[i];
			UpdateTireMarks(wheelState, fxData);
			UpdateTireParticles(wheelState, fxData);
		}
	}

	public override void OnReposition()
	{
		int i = 0;
		for (int num = m_tireData.Length; i < num; i++)
		{
			m_tireData[i].lastMarksIndex = -1;
		}
	}

	public void UpdateTireMarks(VehicleBase.WheelState wheelState, TireFxData fxData)
	{
		VPWheelCollider wheelCol = wheelState.wheelCol;
		if (fxData.lastMarksIndex != -1 && wheelCol.visualGrounded && fxData.marksDelta < updateInterval)
		{
			fxData.marksDelta += Time.deltaTime;
			return;
		}
		float num = fxData.marksDelta;
		if (num == 0f)
		{
			num = Time.deltaTime;
		}
		fxData.marksDelta = 0f;
		if (wheelCol.visualGrounded && !(wheelCol.visualHit.collider.attachedRigidbody != null))
		{
			VPGroundMarksRenderer vPGroundMarksRenderer = ((wheelState.groundMaterial != null) ? wheelState.groundMaterial.marksRenderer : null);
			if (vPGroundMarksRenderer != fxData.lastRenderer)
			{
				fxData.lastRenderer = vPGroundMarksRenderer;
				fxData.lastMarksIndex = -1;
			}
			if (vPGroundMarksRenderer != null)
			{
				float pressureRatio = Mathf.Clamp01(intensity * wheelState.normalizedLoad * 0.5f);
				float skidRatio = Mathf.InverseLerp(minSlip, maxSlip, wheelState.combinedTireSlip);
				fxData.lastMarksIndex = vPGroundMarksRenderer.AddMark(wheelState.wheelCol.visualHit.point - wheelState.wheelCol.cachedTransform.right * wheelState.wheelCol.center.x + wheelState.wheelVelocity * num * 0.5f, wheelState.wheelCol.visualHit.normal, pressureRatio, skidRatio, tireWidth, fxData.lastMarksIndex);
			}
		}
		else
		{
			fxData.lastMarksIndex = -1;
		}
	}

	public void UpdateTireParticles(VehicleBase.WheelState wheelState, TireFxData fxData)
	{
		if (!wheelState.wheelCol.visualGrounded)
		{
			fxData.lastParticleTime = -1f;
			fxData.slipTime -= Time.deltaTime;
			if (fxData.slipTime < 0f)
			{
				fxData.slipTime = 0f;
			}
			return;
		}
		VPGroundParticleEmitter vPGroundParticleEmitter = ((wheelState.groundMaterial != null) ? wheelState.groundMaterial.particleEmitter : null);
		if (vPGroundParticleEmitter != fxData.lastEmitter)
		{
			fxData.lastEmitter = vPGroundParticleEmitter;
			fxData.lastParticleTime = -1f;
		}
		if (vPGroundParticleEmitter != null)
		{
			Vector3 vector = wheelState.wheelCol.visualHit.point + wheelState.wheelCol.cachedTransform.up * tireWidth * 0.5f;
			Vector3 vector2 = Random.insideUnitSphere * tireWidth;
			float num = Mathf.Clamp01(wheelState.normalizedLoad);
			float num2 = Mathf.InverseLerp(minSlip, maxSlip, wheelState.combinedTireSlip);
			if (num2 > 0f && vPGroundParticleEmitter.mode == VPGroundParticleEmitter.Mode.PressureAndSkid)
			{
				fxData.slipTime += Time.deltaTime * num2 * num;
			}
			else
			{
				fxData.slipTime -= Time.deltaTime;
			}
			fxData.slipTime = Mathf.Clamp(fxData.slipTime, 0f, limitIntensityTime);
			float num3 = Mathf.InverseLerp(minIntensityTime, maxIntensityTime, fxData.slipTime);
			fxData.lastParticleTime = vPGroundParticleEmitter.EmitParticle(vector + vector2, wheelState.wheelVelocity, wheelState.tireSlip.y * wheelState.wheelCol.cachedTransform.forward, num, num2 * num3, fxData.lastParticleTime);
		}
		else
		{
			fxData.slipTime -= Time.deltaTime;
			if (fxData.slipTime < 0f)
			{
				fxData.slipTime = 0f;
			}
		}
	}
}
