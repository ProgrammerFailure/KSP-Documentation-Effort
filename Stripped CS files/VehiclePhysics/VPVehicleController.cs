using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Controller", -21)]
public class VPVehicleController : VehicleBase
{
	public Inertia.Settings inertia = new Inertia.Settings();

	public VPAxle[] axles;

	[FormerlySerializedAs("transmission")]
	public Driveline.Settings driveline = new Driveline.Settings();

	public Differential.Settings differential = new Differential.Settings();

	public Differential.Settings centerDifferential = new Differential.Settings();

	public Differential.Settings interAxleDifferential = new Differential.Settings();

	public TorqueSplitter.Settings torqueSplitter = new TorqueSplitter.Settings();

	public Steering.Settings steering = new Steering.Settings();

	public Brakes.Settings brakes = new Brakes.Settings();

	public TireFriction tireFriction = new TireFriction();

	public Engine.Settings engine = new Engine.Settings();

	public Engine.ClutchSettings clutch = new Engine.ClutchSettings();

	public Gearbox.Settings gearbox = new Gearbox.Settings();

	public Retarder.Settings retarder = new Retarder.Settings();

	public SteeringAids.Settings steeringAids = new SteeringAids.Settings();

	public SpeedControl.Settings speedControl = new SpeedControl.Settings();

	public Brakes.AbsSettings antiLock = new Brakes.AbsSettings();

	public TractionControl.Settings tractionControl = new TractionControl.Settings();

	public StabilityControl.Settings stabilityControl = new StabilityControl.Settings();

	[FormerlySerializedAs("antiSlip")]
	public AntiSpin.Settings antiSpin = new AntiSpin.Settings();

	[Range(0f, 1f)]
	public float engineReactionFactor = 1f;

	[Range(0f, 1f)]
	public float parkModeReactionFactor = 0.95f;

	public float maxSubsystemsEnergy = 100000f;

	public Inertia m_inertia;

	public Steering m_steering;

	public Brakes m_brakes;

	public Engine m_engine;

	public Gearbox m_gearbox;

	public Retarder m_retarder;

	public Driveline m_driveline;

	public StabilityControl m_stabilityControl;

	public AntiSpin m_antiSpin;

	public EnergyProvider m_energyProvider;

	public int m_firstSteerableAxle = -1;

	public float m_wheelbase = 1f;

	public VPVehicleController()
	{
		axles = new VPAxle[2];
		axles[0] = new VPAxle();
		axles[1] = new VPAxle();
		axles[0].steeringMode = Steering.SteeringMode.Steerable;
		axles[0].brakeCircuit = Brakes.BrakeCircuit.Front;
		axles[1].brakeCircuit = Brakes.BrakeCircuit.Rear;
		centerDifferential.gearRatio = 1f;
		centerDifferential.type = Differential.Type.Locked;
		interAxleDifferential.gearRatio = 1f;
		interAxleDifferential.type = Differential.Type.Locked;
		torqueSplitter.stiffness = 0.5f;
		retarder.levels = 0;
	}

	public override void OnInitialize()
	{
		SetNumberOfWheels(axles.Length * 2);
		int num = 0;
		int num2 = axles.Length;
		while (true)
		{
			if (num < num2)
			{
				VPWheelCollider leftWheel = axles[num].leftWheel;
				VPWheelCollider rightWheel = axles[num].rightWheel;
				if (leftWheel == null || rightWheel == null)
				{
					break;
				}
				int num3 = num * 2;
				int num4 = num * 2 + 1;
				base.wheelState[num3].wheelCol = leftWheel;
				base.wheelState[num4].wheelCol = rightWheel;
				bool steerable = axles[num].steeringMode != Steering.SteeringMode.Disabled;
				base.wheelState[num3].steerable = steerable;
				base.wheelState[num4].steerable = steerable;
				Wheel obj = base.wheels[num3];
				obj.tireFriction = tireFriction;
				obj.radius = leftWheel.radius;
				obj.mass = leftWheel.mass;
				Wheel obj2 = base.wheels[num4];
				obj2.tireFriction = tireFriction;
				obj2.radius = rightWheel.radius;
				obj2.mass = rightWheel.mass;
				num++;
				continue;
			}
			m_inertia = new Inertia();
			m_inertia.settings = inertia;
			m_inertia.Apply(base.cachedRigidbody);
			m_steering = new Steering();
			m_steering.settings = steering;
			m_brakes = new Brakes();
			m_brakes.settings = brakes;
			m_brakes.absSettings = antiLock;
			m_firstSteerableAxle = -1;
			int i = 0;
			for (int num5 = axles.Length; i < num5; i++)
			{
				int num6 = i * 2;
				int num7 = i * 2 + 1;
				if (axles[i].steeringMode != 0)
				{
					m_steering.AddWheel(base.wheelState[num6], GetWheelLocalPosition(base.wheelState[num6].wheelCol), axles[i].steeringMode, axles[i].steeringRatio);
					m_steering.AddWheel(base.wheelState[num7], GetWheelLocalPosition(base.wheelState[num7].wheelCol), axles[i].steeringMode, axles[i].steeringRatio);
					if (m_firstSteerableAxle == -1)
					{
						m_firstSteerableAxle = i;
					}
				}
				m_brakes.AddWheel(base.wheelState[num6], base.wheels[num6], axles[i].brakeCircuit, Brakes.LateralPosition.Left);
				m_brakes.AddWheel(base.wheelState[num7], base.wheels[num7], axles[i].brakeCircuit, Brakes.LateralPosition.Right);
			}
			m_driveline = new Driveline();
			if (driveline.drivenAxles != 0)
			{
				m_engine = new Engine();
				m_engine.settings = engine;
				m_engine.clutchSettings = clutch;
				m_gearbox = new Gearbox();
				m_gearbox.settings = gearbox;
				m_gearbox.signalSwitchingGears = GearboxSwitchingGears;
				Block.Connect(m_gearbox, 0, m_engine, 0);
				m_retarder = new Retarder();
				m_retarder.settings = retarder;
				Block.Connect(m_retarder, 0, m_gearbox, 0);
				m_driveline.settings = driveline;
				m_driveline.axleDifferential = differential;
				m_driveline.centerDifferential = centerDifferential;
				m_driveline.interAxleDifferential = interAxleDifferential;
				m_driveline.torqueSplitter = torqueSplitter;
				m_driveline.SetupDriveline(base.wheels, m_retarder);
				m_energyProvider = new EnergyProvider(m_engine);
			}
			else
			{
				m_engine = null;
				m_gearbox = null;
				m_retarder = null;
				m_energyProvider = null;
			}
			float num8 = 0f;
			float num9 = 0f;
			int j = 0;
			for (int num10 = axles.Length; j < num10; j++)
			{
				float num11 = 0.5f * (base.cachedTransform.InverseTransformPoint(axles[j].leftWheel.transform.position).z + base.cachedTransform.InverseTransformPoint(axles[j].rightWheel.transform.position).z);
				if (num11 > num9)
				{
					num9 = num11;
				}
				if (num11 < num8)
				{
					num8 = num11;
				}
			}
			m_wheelbase = num9 - num8;
			m_stabilityControl = new StabilityControl();
			m_stabilityControl.settings = stabilityControl;
			m_stabilityControl.wheelbase = m_wheelbase;
			m_antiSpin = new AntiSpin();
			m_antiSpin.settings = antiSpin;
			return;
		}
		DebugLogError("Some VPWheelCollider references are missing at the Axles property.\nAll axles should have a reference to the corresponding left-right VPWheelCollider objects.");
	}

	public override void DoUpdateBlocks()
	{
		int[] array = data.Get(0);
		float num = (float)array[2] / 10000f;
		float num2 = (float)array[3] / 10000f;
		float num3 = (float)array[1] / 10000f;
		float clutchInput = (float)array[4] / 10000f;
		float steerInput = (float)array[0] / 10000f;
		int retarderInput = array[8];
		int manualGearInput = array[5];
		int automaticGearInput = array[6];
		int gearShiftInput = array[7];
		int ignitionInput = array[9];
		int[] array2 = data.Get(2);
		if (m_engine != null)
		{
			num3 = ApplySpeedControl(num3);
			int num4 = array2[5];
			if ((tractionControl.enabled && num4 != 2) || num4 == 1)
			{
				ApplyTractionControl();
			}
			else
			{
				m_engine.tcsRatio = 0f;
			}
			m_engine.throttleInput = num3;
			m_engine.clutchInput = clutchInput;
			m_engine.ignitionInput = ignitionInput;
			m_engine.damping = engineReactionFactor;
		}
		if (m_gearbox != null)
		{
			m_gearbox.manualGearInput = manualGearInput;
			m_gearbox.automaticGearInput = automaticGearInput;
			m_gearbox.gearShiftInput = gearShiftInput;
			m_gearbox.stateVehicleSpeed = base.speed;
			m_gearbox.stateVehicleBrakes = Mathf.Max(num, num2);
			m_gearbox.stateVehicleThrottle = num3;
			m_gearbox.damping = parkModeReactionFactor;
		}
		if (m_retarder != null)
		{
			m_retarder.retarderInput = retarderInput;
		}
		if (m_energyProvider != null)
		{
			m_energyProvider.maxEnergy = maxSubsystemsEnergy;
			m_energyProvider.DoUpdate();
		}
		if (array2[7] != 2)
		{
			ApplySteeringAids(ref steerInput);
		}
		m_steering.steerInput = steerInput;
		m_steering.DoUpdate();
		ApplyStabilityControl();
		if (m_engine != null)
		{
			ApplyAntiSpin();
		}
		m_brakes.brakeInput = num;
		m_brakes.handbrakeInput = num2;
		m_brakes.DoUpdate();
		m_driveline.differentialOverride = (Driveline.Override)array2[0];
		m_brakes.absOverride = (Brakes.AbsOverride)array2[3];
		if (m_gearbox != null)
		{
			m_gearbox.autoShiftOverride = (Gearbox.AutoShiftOverride)array2[2];
		}
		m_stabilityControl.escOverride = (StabilityControl.Override)array2[4];
		m_antiSpin.asrOverride = (AntiSpin.Override)array2[6];
		if (m_brakes.sensorAbsEngaged)
		{
			m_driveline.drivelineOverride = Driveline.Override.ForceUnlocked;
		}
		else
		{
			m_driveline.drivelineOverride = (Driveline.Override)array2[1];
		}
		m_inertia.DoUpdate(base.cachedRigidbody);
	}

	public override void DoUpdateData()
	{
		int[] array = data.Get(1);
		array[0] = (int)(base.speed * 1000f);
		if (m_engine != null)
		{
			array[1] = (int)(m_engine.sensorRpm * 1000f);
			array[2] = (m_engine.sensorStalled ? 1 : 0);
			array[3] = (m_engine.sensorWorking ? 1 : 0);
			array[4] = (m_engine.sensorStarting ? 1 : 0);
			array[5] = (m_engine.sensorRpmLimiter ? 1 : 0);
			array[7] = (int)(m_engine.sensorFlywheelTorque * 1000f);
			array[8] = (int)(m_engine.sensorPower * 1000f);
			array[6] = ((m_engine.sensorLoad < 0f) ? (-1) : ((int)(m_engine.sensorLoad * 1000f)));
			array[9] = (int)(m_engine.sensorFuelRate * 1000f);
			array[10] = (int)(m_engine.sensorOutputTorque * 1000f);
			array[11] = (int)(m_engine.sensorClutchLock * 1000f);
			array[18] = (m_engine.sensorTcsEngaged ? 1 : 0);
		}
		if (m_gearbox != null)
		{
			array[13] = m_gearbox.sensorGearMode;
			array[12] = m_gearbox.sensorEngagedGear;
			array[14] = (m_gearbox.sensorSwitchingGears ? 1 : 0);
			array[16] = (int)(m_gearbox.sensorOutputRpm * 1000f);
		}
		if (m_retarder != null)
		{
			array[15] = (int)(m_retarder.sensorRetarderTorque * 1000f);
		}
		array[17] = (m_brakes.sensorAbsEngaged ? 1 : 0);
		array[19] = (m_stabilityControl.sensorEngaged ? 1 : 0);
		array[20] = (m_antiSpin.sensorEngaged ? 1 : 0);
		array[21] = (int)(m_steering.steerInput * 10000f);
		int[] array2 = data.Get(0);
		if (m_gearbox != null)
		{
			array2[7] = m_gearbox.gearShiftInput;
			array2[5] = m_gearbox.manualGearInput;
			array2[6] = m_gearbox.automaticGearInput;
		}
		if (m_retarder != null)
		{
			array2[8] = m_retarder.retarderInput;
		}
	}

	public override object GetInternalObject(Type type)
	{
		if (type == typeof(Gearbox))
		{
			return m_gearbox;
		}
		if (type == typeof(Engine))
		{
			return m_engine;
		}
		if (type == typeof(StabilityControl))
		{
			return m_stabilityControl;
		}
		if (type == typeof(AntiSpin))
		{
			return m_antiSpin;
		}
		if (type == typeof(Inertia))
		{
			return m_inertia;
		}
		if (type == typeof(Retarder))
		{
			return m_retarder;
		}
		if (type == typeof(Engine.Settings))
		{
			return engine;
		}
		if (type == typeof(EnergyProvider))
		{
			return m_energyProvider;
		}
		return null;
	}

	public void GearboxSwitchingGears()
	{
		if (m_gearbox.settings.type == Gearbox.Type.Manual)
		{
			m_engine.throttleInput = 0f;
		}
	}

	public void ApplyTractionControl()
	{
		int num = driveline.firstDrivenAxle * 2;
		int wheelIndex = num + 1;
		float wheelFinalRatio = GetWheelFinalRatio(num);
		if (float.IsNaN(wheelFinalRatio))
		{
			m_engine.tcsRatio = 0f;
			return;
		}
		float num2 = 0f;
		float num3 = 0f;
		switch (tractionControl.mode)
		{
		case TractionControl.Mode.Street:
			num2 = GetWheelAdherentSlip(num).y;
			num3 = GetWheelAdherentSlip(wheelIndex).y;
			break;
		case TractionControl.Mode.Sport:
			num2 = GetWheelPeakSlip(num).y;
			num3 = GetWheelPeakSlip(wheelIndex).y;
			break;
		case TractionControl.Mode.CustomSlip:
			num2 = tractionControl.customSlip;
			num3 = tractionControl.customSlip;
			break;
		}
		if (wheelFinalRatio < 0f)
		{
			num2 = 0f - num2;
			num3 = 0f - num3;
		}
		float wheelAngularVelocityForSlip = GetWheelAngularVelocityForSlip(num, num2);
		float wheelAngularVelocityForSlip2 = GetWheelAngularVelocityForSlip(wheelIndex, num3);
		if (wheelAngularVelocityForSlip != 0f && wheelAngularVelocityForSlip2 != 0f)
		{
			m_engine.tcsRpms = (wheelAngularVelocityForSlip + wheelAngularVelocityForSlip2) * 0.5f * wheelFinalRatio * Block.WToRpm;
		}
		else if (wheelAngularVelocityForSlip != 0f)
		{
			m_engine.tcsRpms = wheelAngularVelocityForSlip * wheelFinalRatio * Block.WToRpm;
		}
		else if (wheelAngularVelocityForSlip2 != 0f)
		{
			m_engine.tcsRpms = wheelAngularVelocityForSlip2 * wheelFinalRatio * Block.WToRpm;
		}
		else
		{
			m_engine.tcsRpms = 0f;
		}
		m_engine.tcsRatio = tractionControl.ratio;
	}

	public void ApplySteeringAids(ref float steerInput)
	{
		if (steeringAids.priority == SteeringAids.Priority.PreferDrifting)
		{
			ApplySteeringHelp(ref steerInput);
			ApplySteeringLimit(ref steerInput);
		}
		else
		{
			ApplySteeringLimit(ref steerInput);
			ApplySteeringHelp(ref steerInput);
		}
	}

	public void ApplySteeringHelp(ref float steerInput)
	{
		if (steeringAids.helpMode != 0 && steeringAids.helpRatio > 0f)
		{
			float num = Mathf.InverseLerp(0.1f, 3f, base.speed);
			float num2 = Mathf.Clamp(base.speedAngle * steeringAids.helpRatio * num * Mathf.InverseLerp(2f, 3f, Mathf.Abs(base.speedAngle)) / steering.maxSteerAngle, -1f, 1f);
			steerInput = num2 + steerInput;
		}
	}

	public void ApplySteeringLimit(ref float steerInput)
	{
		if (steeringAids.limitMode == SteeringAids.LimitMode.Disabled || !(steeringAids.limitRatio > 0f))
		{
			return;
		}
		int num = m_firstSteerableAxle * 2;
		int wheelIndex = num + 1;
		float num2 = 0f;
		switch (steeringAids.limitMode)
		{
		case SteeringAids.LimitMode.Street:
			num2 = (GetWheelAdherentSlip(num).x + GetWheelAdherentSlip(wheelIndex).x) * 0.5f;
			break;
		case SteeringAids.LimitMode.Sport:
			num2 = (GetWheelPeakSlip(num).x + GetWheelPeakSlip(wheelIndex).x) * 0.5f;
			break;
		case SteeringAids.LimitMode.CustomSlip:
			num2 = steeringAids.limitCustomSlip;
			break;
		}
		float num3 = base.speed;
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		float num4;
		float num5 = 0f - (num4 = Mathf.Asin(Mathf.Clamp01(num2 / (num3 * steeringAids.limitRatio))) * 57.29578f);
		if (base.speedAngle > 0f)
		{
			if (num4 < base.speedAngle)
			{
				num4 = base.speedAngle;
			}
		}
		else if (base.speedAngle < num5)
		{
			num5 = base.speedAngle;
		}
		if (num4 > steering.maxSteerAngle)
		{
			num4 = steering.maxSteerAngle;
		}
		if (num5 < 0f - steering.maxSteerAngle)
		{
			num5 = 0f - steering.maxSteerAngle;
		}
		float num6 = num4 / steering.maxSteerAngle;
		float num7 = num5 / steering.maxSteerAngle;
		float a = Mathf.Clamp(steerInput, num7, num6);
		float b = ((steerInput >= 0f) ? (steerInput * num6) : (steerInput *= 0f - num7));
		steerInput = Mathf.Lerp(a, b, steeringAids.limitProportionality);
	}

	public float ApplySpeedControl(float throttleInput)
	{
		if (!speedControl.cruiseControl && !speedControl.speedLimiter)
		{
			return throttleInput;
		}
		int[] array = data.Get(1);
		float num = (float)array[0] / 1000f;
		bool flag = array[3] != 0;
		int num2 = array[12];
		bool flag2 = array[14] != 0 && m_gearbox.settings.type == Gearbox.Type.Manual;
		bool flag3 = array[15] > 0;
		int[] array2 = data.Get(0);
		bool flag4 = array2[2] > 1000;
		bool flag5 = array2[4] > 1000;
		float b = 0f;
		if (speedControl.cruiseControl && num >= speedControl.minSpeedForCC && flag && num2 > 0 && !flag2 && !flag3 && !flag4 && !flag5)
		{
			b = Mathf.Clamp01((speedControl.cruiseSpeed - num) * speedControl.throttleSlope);
		}
		float b2 = 1f;
		if (speedControl.speedLimiter)
		{
			b2 = Mathf.Clamp01((speedControl.speedLimit - num) * speedControl.throttleSlope);
		}
		return Mathf.Min(Mathf.Max(throttleInput, b), b2);
	}

	public void ApplyStabilityControl()
	{
		m_stabilityControl.stateVehicleSpeed = base.speed;
		m_stabilityControl.stateVehicleSpeedAngle = base.speedAngle;
		m_stabilityControl.stateVehicleRotationRate = base.cachedRigidbody.angularVelocity.y;
		m_stabilityControl.stateVehicleSteeringAngle = steering.maxSteerAngle * m_steering.steerInput;
		m_stabilityControl.DoUpdate();
		if (!stabilityControl.enabled)
		{
			return;
		}
		m_brakes.AddBrakeRatio(m_stabilityControl.sensorBrakeFL, Brakes.BrakeCircuit.Front, Brakes.LateralPosition.Left);
		m_brakes.AddBrakeRatio(m_stabilityControl.sensorBrakeFR, Brakes.BrakeCircuit.Front, Brakes.LateralPosition.Right);
		m_brakes.AddBrakeRatio(m_stabilityControl.sensorBrakeRL, Brakes.BrakeCircuit.Rear, Brakes.LateralPosition.Left);
		m_brakes.AddBrakeRatio(m_stabilityControl.sensorBrakeRR, Brakes.BrakeCircuit.Rear, Brakes.LateralPosition.Right);
		if (m_engine != null)
		{
			float a = Mathf.Max(m_stabilityControl.sensorBrakeFL, m_stabilityControl.sensorBrakeFR);
			float b = Mathf.Max(m_stabilityControl.sensorBrakeRL, m_stabilityControl.sensorBrakeRR);
			float num = Mathf.Clamp01(Mathf.Max(a, b));
			if (num > 0f)
			{
				m_engine.throttleInput *= 1f - num;
				m_engine.clutchInput = Mathf.Max(m_engine.clutchInput, num);
			}
		}
	}

	public void ApplyAntiSpin()
	{
		int num = driveline.firstDrivenAxle * 2;
		int num2 = num + 1;
		m_antiSpin.stateVehicleSpeed = base.speed;
		m_antiSpin.stateAngularVelocityL = base.wheelState[num].angularVelocity;
		m_antiSpin.stateAngularVelocityR = base.wheelState[num2].angularVelocity;
		m_antiSpin.DoUpdate();
		if (antiSpin.enabled)
		{
			base.wheels[num].AddBrakeTorque(m_antiSpin.sensorBrakeTorqueL);
			base.wheels[num2].AddBrakeTorque(m_antiSpin.sensorBrakeTorqueR);
		}
	}

	public float GetOptimalGearShiftRatio()
	{
		if (base.initialized && m_engine != null && m_gearbox != null)
		{
			int num = m_gearbox.settings.forwardGearRatios.Length;
			if (num < 2)
			{
				return 0f;
			}
			int sensorEngagedGear = m_gearbox.sensorEngagedGear;
			if (sensorEngagedGear <= 0)
			{
				return 0f;
			}
			if (m_engine.throttleInput < 0.5f)
			{
				return 0f;
			}
			float sensorRpm = m_engine.sensorRpm;
			float num2 = m_engine.CalculateTorque(sensorRpm, m_engine.throttleInput);
			if (num2 <= 0f)
			{
				return 0f;
			}
			float axleFinalRatio = m_driveline.GetAxleFinalRatio(driveline.firstDrivenAxle);
			float gearRatio = m_gearbox.GetGearRatio(sensorEngagedGear);
			float num3 = num2 * axleFinalRatio * gearRatio;
			float num4 = sensorRpm / axleFinalRatio / gearRatio;
			float num5 = 0f;
			float num6 = 0f;
			if (sensorEngagedGear < num)
			{
				float gearRatio2 = m_gearbox.GetGearRatio(sensorEngagedGear + 1);
				float rpm = num4 * axleFinalRatio * gearRatio2;
				num5 = axleFinalRatio * gearRatio2 * m_engine.CalculateTorque(rpm, m_engine.throttleInput) / num3;
			}
			if (sensorEngagedGear > 1)
			{
				float gearRatio3 = m_gearbox.GetGearRatio(sensorEngagedGear - 1);
				float rpm2 = num4 * axleFinalRatio * gearRatio3;
				num6 = axleFinalRatio * gearRatio3 * m_engine.CalculateTorque(rpm2, m_engine.throttleInput) / num3;
			}
			if (num6 > num5)
			{
				return 0f - num6;
			}
			return num5;
		}
		return 0f;
	}

	public float GetWheelFinalRatio(int wheelIndex, int gear = 0)
	{
		if (base.initialized && wheelIndex >= 0 && wheelIndex < base.wheels.Length)
		{
			float num = m_driveline.GetAxleFinalRatio(wheelIndex / 2);
			if (m_gearbox != null)
			{
				num = ((gear != 0) ? (num * m_gearbox.GetGearRatio(gear)) : (num * m_gearbox.GetCurrentGearRatio()));
			}
			return num;
		}
		return 0f;
	}

	public void OnDrawGizmos()
	{
	}
}
