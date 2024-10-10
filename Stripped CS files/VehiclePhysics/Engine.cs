using System;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class Engine : Block
{
	public enum IdleControlType
	{
		Passive,
		Active
	}

	public enum ClutchType
	{
		LockRatio,
		DiskFriction,
		TorqueConverter,
		TorqueConverterLimited
	}

	public enum RpmLimiterMode
	{
		InjectionCut,
		InjectionLimit
	}

	[Serializable]
	public class Settings : ISerializationCallbackReceiver
	{
		public float idleRpm = 1000f;

		public float peakRpm = 4200f;

		public float maxRpm = 6500f;

		public float idleRpmTorque = 135f;

		public float peakRpmTorque = 188f;

		[Range(0f, 1f)]
		public float idleRpmCurveBias;

		[Range(0f, 1f)]
		public float peakRpmCurveBias;

		public float inertia = 0.5f;

		public float frictionTorque = 20f;

		[Range(0f, 3f)]
		public float rotationalFriction = 0.1f;

		[Range(0f, 0.2f)]
		public float viscousFriction = 0.01f;

		public bool torqueCap;

		public float torqueCapLimit = 200f;

		public bool rpmLimiter;

		public RpmLimiterMode rpmLimiterMode;

		public float rpmLimiterMax = 6000f;

		[Range(0f, 1f)]
		public float rpmLimiterCutoffTime = 0.075f;

		public IdleControlType idleControl = IdleControlType.Active;

		[Range(0f, 1f)]
		public float maxIdleThrottle = 1f;

		[Range(0f, 1f)]
		public float activeIdleRange = 0.5f;

		[Range(0.0001f, 0.9999f)]
		public float activeIdleBias = 0.25f;

		public bool canStall;

		[Range(0f, 1f)]
		public float stallBias = 0.5f;

		public float stalledFrictionTorque = 25f;

		[Range(0f, 1f)]
		public float starterMotorBias = 0.6f;

		public float maxFuelPerRev = 0.16f;

		public void ApplyConstraints()
		{
			idleRpm = Mathf.Max(10f, idleRpm);
			peakRpm = Mathf.Max(idleRpm + 10f, peakRpm);
			maxRpm = Mathf.Max(peakRpm + 10f, maxRpm);
			idleRpmTorque = Mathf.Max(0f, idleRpmTorque);
			peakRpmTorque = Mathf.Max(0f, peakRpmTorque);
			torqueCapLimit = Mathf.Max(0f, torqueCapLimit);
			inertia = Mathf.Max(0.01f, inertia);
			frictionTorque = Mathf.Max(0f, frictionTorque);
			rotationalFriction = Mathf.Max(0f, rotationalFriction);
			viscousFriction = Mathf.Max(0f, viscousFriction);
			maxFuelPerRev = Mathf.Max(0f, maxFuelPerRev);
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			ApplyConstraints();
		}
	}

	[Serializable]
	public class ClutchSettings
	{
		public ClutchType type = ClutchType.TorqueConverter;

		public float maxTorqueTransfer = 300f;

		public float lockRpm = 2000f;

		[Range(0.001f, 0.999f)]
		public float lockRatioBias = 0.1f;
	}

	public struct EngineSpecs
	{
		public float idleRpm;

		public float maxTorqueAtIdle;

		public float frictionTorqueAtIdle;

		public float peakRpm;

		public float maxTorqueAtPeak;

		public float frictionTorqueAtPeak;

		public float specificFuelConsumption;

		public float specificFuelConsumptionRpm;

		public float maxPowerRpm;

		public float maxPowerInKw;

		public float maxPowerInHp;

		public float limitRpm;

		public float frictionTorqueAtLimit;

		public float stallRpm;

		public float frictionTorqueAtStall;

		public bool malformedTorque;

		public bool malformedRawPower;
	}

	[Serializable]
	public struct StateVars
	{
		public float float_0;

		public float Treaction;

		public bool rpmLimiterActive;

		public float rpmLimiterTime;

		public float tcsActivationTime;
	}

	public float throttleInput;

	public float clutchInput;

	public int ignitionInput;

	public float allowedFuelRatio = 1f;

	public float tcsRpms = -1f;

	public float tcsRatio = 1f;

	public float tcsThrottleFactor = 1f;

	public bool autoRpms;

	public float targetRpms = -1f;

	public Settings settings = new Settings();

	public ClutchSettings clutchSettings = new ClutchSettings();

	public static float KwToHp = 1.341022f;

	public float damping = 1f;

	public Connection m_output;

	public bool m_sensorStalled = true;

	public float m_sensorFlywheelTorque;

	public float m_sensorLoad;

	public float m_sensorPower;

	public float m_sensorClutchLock;

	public float m_sensorFuelRate;

	public bool m_sensorTcsEngaged;

	public StateVars m_stateVars = new StateVars
	{
		float_0 = 0f,
		Treaction = 0f,
		rpmLimiterActive = false,
		rpmLimiterTime = 0f,
		tcsActivationTime = -1f
	};

	public float float_0;

	public float Tr;

	public float m_throttle;

	public BiasedRatio m_lockRatioBias = new BiasedRatio();

	public BiasedRatio m_throttleMapBias = new BiasedRatio();

	public float m_extraFrictionTorque;

	public float tempDebug1;

	public float tempDebug2;

	public float sensorRpm => m_stateVars.float_0 / settings.inertia * Block.WToRpm;

	public bool sensorStalled
	{
		get
		{
			if (m_sensorStalled)
			{
				return ignitionInput >= 0;
			}
			return false;
		}
	}

	public bool sensorWorking
	{
		get
		{
			if (!m_sensorStalled)
			{
				return ignitionInput >= 0;
			}
			return false;
		}
	}

	public bool sensorStarting
	{
		get
		{
			if (m_sensorStalled)
			{
				return ignitionInput == 1;
			}
			return false;
		}
	}

	public float sensorFlywheelTorque => m_sensorFlywheelTorque;

	public float sensorOutputTorque
	{
		get
		{
			if (m_output == null)
			{
				return 0f;
			}
			return m_output.outTd;
		}
	}

	public float sensorPower => m_sensorPower;

	public bool sensorRpmLimiter => m_stateVars.rpmLimiterActive;

	public bool sensorTcsEngaged => m_sensorTcsEngaged;

	public float sensorFuelRate => m_sensorFuelRate;

	public float sensorLoad => m_sensorLoad;

	public float sensorClutchLock => m_sensorClutchLock;

	public void GetEngineSpecifications(ref EngineSpecs data, float deltaRpm = 5f)
	{
		data.idleRpm = settings.idleRpm;
		data.maxTorqueAtIdle = CalculateTorque(settings.idleRpm);
		data.frictionTorqueAtIdle = GetFrictionTorque(settings.idleRpm);
		data.malformedTorque = false;
		data.malformedRawPower = false;
		data.peakRpm = 0f;
		data.maxTorqueAtPeak = 0f;
		data.frictionTorqueAtPeak = 0f;
		data.maxPowerRpm = 0f;
		data.maxPowerInKw = 0f;
		data.maxPowerInHp = 0f;
		data.stallRpm = -1f;
		data.frictionTorqueAtStall = 0f;
		float num = 0f;
		float maxRpm = settings.maxRpm;
		float num2 = CalculateTorque(num);
		float num3 = CalculatePowerInKw(num);
		float num4 = GetMaxPowerTorque(num);
		while (num < maxRpm)
		{
			float num5 = CalculateTorque(num + deltaRpm);
			float num6 = CalculatePowerInKw(num + deltaRpm);
			float maxPowerTorque = GetMaxPowerTorque(num + deltaRpm);
			if (num > settings.idleRpm && num2 < 0f)
			{
				data.malformedTorque = true;
			}
			if (!settings.torqueCap && num > settings.peakRpm && maxPowerTorque > num4)
			{
				data.malformedRawPower = true;
			}
			if (num2 - data.maxTorqueAtPeak > 5E-05f)
			{
				data.maxTorqueAtPeak = num2;
				data.peakRpm = num;
				data.frictionTorqueAtPeak = GetFrictionTorque(num);
				data.specificFuelConsumption = GetMaxFuelRate(num) / num3 * 3600f;
				data.specificFuelConsumptionRpm = num;
			}
			if (num3 > data.maxPowerInKw)
			{
				data.maxPowerInKw = num3;
				data.maxPowerRpm = num;
				data.maxPowerInHp = num3 * KwToHp;
			}
			if (data.stallRpm < 0f && num5 > 0f)
			{
				data.stallRpm = num;
				data.frictionTorqueAtStall = GetFrictionTorque(num) + settings.stalledFrictionTorque;
			}
			num += deltaRpm;
			num2 = num5;
			num3 = num6;
			num4 = maxPowerTorque;
		}
		if (settings.torqueCap && !data.malformedTorque && !data.malformedRawPower)
		{
			num = settings.idleRpm;
			maxRpm = settings.maxRpm - deltaRpm;
			float num7 = CalculateSpecificFuelConsumption(num);
			while (num < maxRpm)
			{
				float num8 = CalculateSpecificFuelConsumption(num + deltaRpm);
				if (!(num8 < num7))
				{
					break;
				}
				data.specificFuelConsumption = num8;
				data.specificFuelConsumptionRpm = num + deltaRpm;
				num += deltaRpm;
				num7 = num8;
			}
		}
		data.limitRpm = settings.maxRpm;
		data.frictionTorqueAtLimit = GetFrictionTorque(data.limitRpm);
		if (data.malformedTorque || data.malformedRawPower)
		{
			return;
		}
		num = settings.maxRpm;
		while (true)
		{
			if (num < 25000f)
			{
				if (!(GetMaxPowerTorque(num) >= 0.001f))
				{
					break;
				}
				num += deltaRpm;
				continue;
			}
			return;
		}
		data.limitRpm = num;
		data.frictionTorqueAtLimit = GetFrictionTorque(num);
	}

	public float CalculateTorque(float rpm, float throttle = 1f)
	{
		float num = GetMaxPowerTorque(rpm);
		float num2 = GetFrictionTorque(rpm);
		if (rpm < settings.idleRpm)
		{
			if (settings.canStall)
			{
				if (num < num2)
				{
					num = 0f;
					num2 += settings.stalledFrictionTorque;
				}
			}
			else
			{
				num = GetMaxPowerTorque(settings.idleRpm);
			}
		}
		return throttle * num - num2;
	}

	public float CalculatePowerInKw(float rpm)
	{
		return (GetMaxPowerTorque(rpm) - GetFrictionTorque(rpm)) * rpm * Block.RpmToW * 0.001f;
	}

	public float CalculateSpecificFuelConsumption(float rpm)
	{
		float maxPowerTorque = GetMaxPowerTorque(rpm);
		float maxPowerTorqueRaw = GetMaxPowerTorqueRaw(rpm);
		return maxPowerTorque / maxPowerTorqueRaw * GetMaxFuelRate(rpm) / CalculatePowerInKw(rpm) * 3600f;
	}

	public float AddFrictionTorque(float frictionTorque)
	{
		m_extraFrictionTorque += frictionTorque;
		if (m_extraFrictionTorque < 0f)
		{
			m_extraFrictionTorque = 0f;
		}
		return m_extraFrictionTorque;
	}

	public void ResetFrictionTorque()
	{
		m_extraFrictionTorque = 0f;
	}

	public StateVars GetStateVars()
	{
		return m_stateVars;
	}

	public void SetStateVars(StateVars stateVars)
	{
		m_stateVars = stateVars;
	}

	public float GetMaxPowerTorqueRaw(float rpm)
	{
		float num = MathUtility.FastAbs(rpm);
		float num2;
		if (!(num < settings.idleRpm))
		{
			num2 = ((!(num < settings.peakRpm)) ? MathUtility.TangentLerp(settings.peakRpm, GetFrictionTorque(settings.peakRpm) + settings.peakRpmTorque, settings.maxRpm, GetFrictionTorque(settings.maxRpm), 0f, 1f, num) : MathUtility.TangentLerp(settings.idleRpm, GetFrictionTorque(settings.idleRpm) + settings.idleRpmTorque, settings.peakRpm, GetFrictionTorque(settings.peakRpm) + settings.peakRpmTorque, settings.idleRpmCurveBias, 0f - settings.peakRpmCurveBias, num));
		}
		else
		{
			float num3 = settings.idleRpm * 0.5f * settings.stallBias;
			num2 = ((!(num < num3)) ? MathUtility.TangentLerp(num3, 0f, settings.idleRpm, GetFrictionTorque(settings.idleRpm) + settings.idleRpmTorque, 0f, settings.stallBias, num) : 0f);
		}
		if (!(num2 > 0f))
		{
			return 0f;
		}
		return num2;
	}

	public float ClampPowerTorque(float powerTorque, float rpm)
	{
		if (settings.torqueCap)
		{
			float frictionTorque = GetFrictionTorque(rpm);
			if (powerTorque - frictionTorque > settings.torqueCapLimit)
			{
				powerTorque = frictionTorque + settings.torqueCapLimit;
			}
			if (!(powerTorque > 0f))
			{
				return 0f;
			}
			return powerTorque;
		}
		return powerTorque;
	}

	public float GetMaxPowerTorque(float rpm)
	{
		return ClampPowerTorque(GetMaxPowerTorqueRaw(rpm), rpm);
	}

	public float GetFrictionTorque(float rpm)
	{
		float num = MathUtility.FastAbs(rpm) * Block.RpmToW;
		return settings.frictionTorque + num * (settings.rotationalFriction + num * settings.viscousFriction * settings.viscousFriction);
	}

	public float GetMaxFuelRate(float rpm)
	{
		return rpm / 60f * settings.maxFuelPerRev;
	}

	public float ThrottleMap(float throttleInput, float rpm)
	{
		if (settings.activeIdleRange <= 0f)
		{
			return throttleInput;
		}
		if (throttleInput < 0.01f)
		{
			return 0f;
		}
		float frictionTorque = GetFrictionTorque(settings.idleRpm);
		float maxPowerTorque = GetMaxPowerTorque(settings.idleRpm);
		float num = frictionTorque / maxPowerTorque;
		float x = Mathf.InverseLerp(settings.idleRpm + (settings.maxRpm - settings.idleRpm) * settings.activeIdleRange, settings.idleRpm, rpm);
		return Mathf.Lerp(m_throttleMapBias.BiasedLerp(x, settings.activeIdleBias) * num, 1f, throttleInput);
	}

	public override void Initialize()
	{
		SetInputs(0);
		SetOutputs(1);
	}

	public override bool CheckConnections()
	{
		m_output = base.outputs[0];
		return m_output != null;
	}

	public override void PreStep()
	{
		float num = m_stateVars.float_0 / settings.inertia * Block.WToRpm;
		if (settings.idleControl == IdleControlType.Active)
		{
			m_throttle = Mathf.Clamp01(ThrottleMap(throttleInput, num));
		}
		else
		{
			m_throttle = Mathf.Clamp01(throttleInput);
		}
		if (settings.rpmLimiter)
		{
			if (settings.rpmLimiterMode == RpmLimiterMode.InjectionCut)
			{
				if (num > settings.rpmLimiterMax && !m_stateVars.rpmLimiterActive)
				{
					m_stateVars.rpmLimiterActive = true;
					m_stateVars.rpmLimiterTime = Solver.time;
				}
				if (m_stateVars.rpmLimiterActive)
				{
					m_throttle = 0f;
					if (Solver.time - m_stateVars.rpmLimiterTime >= settings.rpmLimiterCutoffTime)
					{
						m_stateVars.rpmLimiterActive = false;
					}
				}
			}
			else
			{
				float num2 = (settings.rpmLimiterMax * Block.RpmToW * settings.inertia - m_stateVars.float_0) / Solver.deltaTime - m_stateVars.Treaction;
				float maxPowerTorque = GetMaxPowerTorque(num);
				float num3 = Mathf.Clamp01(num2 / maxPowerTorque);
				if (m_throttle > num3)
				{
					m_throttle = num3;
				}
			}
		}
		if ((tcsRatio > 0f) & (tcsRpms > 0f))
		{
			float num4 = m_output.float_0 / m_output.float_1 * Block.WToRpm;
			float num5 = num - num4;
			float num6 = Mathf.Lerp(settings.maxRpm, tcsRpms + num5, tcsRatio);
			tempDebug1 = num4;
			tempDebug2 = num5;
			if (num6 > settings.idleRpm)
			{
				float num7 = (num6 * Block.RpmToW * settings.inertia - m_stateVars.float_0) / Solver.deltaTime - m_stateVars.Treaction;
				float maxPowerTorque2 = GetMaxPowerTorque(num);
				float num8 = Mathf.Clamp01(num7 / maxPowerTorque2) * tcsThrottleFactor;
				if (m_throttle > num8)
				{
					m_throttle = num8;
					m_stateVars.tcsActivationTime = Solver.time;
				}
			}
		}
		if (autoRpms)
		{
			float num9 = (targetRpms * Block.RpmToW * settings.inertia - m_stateVars.float_0) / Solver.deltaTime - m_stateVars.Treaction;
			float maxPowerTorque3 = GetMaxPowerTorque(num);
			m_throttle = Mathf.Clamp01(num9 / maxPowerTorque3);
		}
		m_sensorPower = m_sensorFlywheelTorque * num * Block.RpmToW * 0.001f;
		m_sensorFuelRate = GetMaxFuelRate(num) * m_sensorLoad;
		m_sensorTcsEngaged = Solver.time - m_stateVars.tcsActivationTime < 0.25f;
	}

	public override void GetState(ref State state_0)
	{
		state_0.float_0 = m_stateVars.float_0;
		state_0.Lr = 0f;
	}

	public override void SetSubstepState(State state_0)
	{
		m_stateVars.float_0 = state_0.float_0;
	}

	public override void EvaluateTorqueDownstream()
	{
		float num = m_stateVars.float_0 / settings.inertia * Block.WToRpm;
		float num2 = GetMaxPowerTorqueRaw(num);
		float num3 = ClampPowerTorque(num2, num);
		float num4 = GetFrictionTorque(num);
		m_sensorStalled = false;
		if (num < settings.idleRpm)
		{
			if (settings.canStall)
			{
				if (num3 < num4)
				{
					m_sensorStalled = true;
					num4 += settings.stalledFrictionTorque;
					if (ignitionInput > 0)
					{
						float num5 = 0.5f + settings.starterMotorBias * 0.7f;
						float num6 = Mathf.PerlinNoise(Solver.time * 4f, 0f);
						num3 = num4 * (num5 + num6);
					}
					else
					{
						num3 = 0f;
					}
				}
				else
				{
					float num7 = GetMaxPowerTorque(settings.idleRpm) - GetFrictionTorque(settings.idleRpm);
					if (num3 < num7)
					{
						num3 = num7;
					}
				}
			}
			else
			{
				float maxPowerTorque = GetMaxPowerTorque(settings.idleRpm);
				if (num3 < maxPowerTorque)
				{
					num3 = maxPowerTorque;
				}
			}
			if (num2 < num3)
			{
				num2 = num3;
			}
		}
		if (ignitionInput < 0)
		{
			num3 = 0f;
		}
		float num8 = 0f;
		switch (settings.idleControl)
		{
		case IdleControlType.Active:
			num8 = (settings.idleRpm * Block.RpmToW * settings.inertia - m_stateVars.float_0) / Solver.deltaTime + GetFrictionTorque(settings.idleRpm);
			num8 = Mathf.Clamp(num8, 0f, num3 * settings.maxIdleThrottle);
			break;
		case IdleControlType.Passive:
			num8 = GetFrictionTorque(settings.idleRpm);
			break;
		}
		float num9 = 0f;
		if (m_sensorStalled)
		{
			num9 = num3;
		}
		else
		{
			num3 *= allowedFuelRatio;
			num9 = ((!(num3 > num8)) ? num3 : (num8 + (num3 - num8) * m_throttle));
		}
		m_sensorLoad = num9 / num2;
		float num10 = 0f - m_stateVars.float_0 / Solver.deltaTime - m_stateVars.Treaction * damping;
		float num11 = ((m_stateVars.float_0 > 0f) ? Mathf.Max(num10, 0f - (num4 + m_extraFrictionTorque)) : num10);
		float num12 = (m_sensorFlywheelTorque = num9 + num11);
		float num13 = 0f;
		float num14 = 0f;
		float num15 = Mathf.Clamp01(1f - clutchInput);
		switch (clutchSettings.type)
		{
		case ClutchType.LockRatio:
		{
			float viscousLockingDt = Solver.GetViscousLockingDt(num15);
			num13 = ComputeLockingTorque(num12, viscousLockingDt);
			num14 = num13 * num15;
			break;
		}
		case ClutchType.DiskFriction:
		{
			num13 = ComputeLockingTorque(num12, Solver.deltaTime);
			float num18 = clutchSettings.maxTorqueTransfer * num15;
			num14 = Mathf.Clamp(num13, 0f - num18, num18);
			break;
		}
		case ClutchType.TorqueConverter:
		case ClutchType.TorqueConverterLimited:
		{
			float num16 = m_output.float_0 / m_output.float_1 * Block.WToRpm;
			float num17 = Mathf.Max(MathUtility.FastAbs(num - num16), Mathf.Max(num, MathUtility.FastAbs(num16)));
			num15 = Mathf.Min(m_lockRatioBias.BiasedLerp(num17 / clutchSettings.lockRpm, clutchSettings.lockRatioBias), num15);
			float viscousLockingDt = Solver.GetViscousLockingDt(num15);
			num13 = ComputeLockingTorque(num12, viscousLockingDt);
			num14 = num13 * num15;
			if (clutchSettings.type == ClutchType.TorqueConverterLimited)
			{
				num14 = Mathf.Clamp(num14, 0f - clutchSettings.maxTorqueTransfer, clutchSettings.maxTorqueTransfer);
			}
			break;
		}
		}
		float_0 = num12 - num14;
		m_output.outTd = num14;
		m_sensorClutchLock = MathUtility.FastAbs(num14 / num13);
		Tr = float_0 - num9;
	}

	public override void GetSubstepDerivative(ref Derivative derivative_0)
	{
		derivative_0.float_0 = float_0;
		derivative_0.Tr = Tr;
	}

	public override void SetState(State state_0)
	{
		m_stateVars.float_0 = state_0.float_0;
		m_stateVars.Treaction = state_0.Lr / Solver.deltaTime;
		m_extraFrictionTorque = 0f;
	}

	public float ComputeLockingTorque(float flywheelTorque, float dt)
	{
		return 1f / ((settings.inertia + m_output.float_1) * dt) * (m_output.float_1 * (m_stateVars.float_0 + flywheelTorque * dt) - settings.inertia * (m_output.float_0 + m_output.Tr * damping * dt));
	}
}
