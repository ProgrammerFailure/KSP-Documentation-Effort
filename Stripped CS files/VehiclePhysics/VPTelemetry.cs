using System;
using EdyCommonTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Telemetry Window", 0)]
public class VPTelemetry : VehicleBehaviour
{
	public bool showData;

	public bool contactDepthAsSuspension;

	public bool showLoadInKg;

	public Vector2 screenPosition = new Vector2(8f, 8f);

	[Space(5f)]
	public bool enableHotKey = true;

	public KeyCode hotKey = KeyCode.B;

	[Space(5f)]
	public Font font;

	[Header("Debug Gizmos")]
	public bool showCenterOfMass = true;

	public bool gizmosAtPhysicPositions;

	[FormerlySerializedAs("showGizmos")]
	[Header("Wheel Gizmos")]
	public bool showWheelGizmos = true;

	public bool showLocalFrame = true;

	public bool showContactPoints = true;

	public bool showTireSlip = true;

	public bool showTireForces = true;

	public bool showSurfaceForces;

	public bool useLogScale = true;

	public static string customData = "";

	[Header("Fuel consumption (l/100km)")]
	public float fuelConsumptionCorrection = 3.6f;

	public float fuelDensity = 0.745f;

	public GUIStyle m_smallStyle = new GUIStyle();

	public GUIStyle m_bigStyle = new GUIStyle();

	public string m_text;

	public string m_bigText;

	public int m_lines;

	public int m_frameNum;

	public Vector3 m_velocity;

	public Vector3 m_angularVelocity;

	public override void OnEnableComponent()
	{
		UpdateTextProperties();
	}

	public override void OnEnableVehicle()
	{
		m_frameNum = 0;
	}

	public void OnValidate()
	{
		UpdateTextProperties();
	}

	public override void FixedUpdateVehicle()
	{
		m_frameNum++;
		m_velocity = base.vehicle.cachedRigidbody.velocity;
		m_angularVelocity = base.vehicle.cachedRigidbody.angularVelocity;
	}

	public void FixedUpdate()
	{
		if (showData)
		{
			DoTelemetry();
		}
	}

	public void LateUpdate()
	{
		if (enableHotKey && Input.GetKeyDown(hotKey))
		{
			showData = !showData;
		}
	}

	public void OnGUI()
	{
		if (showData)
		{
			float num = 740f;
			float num2 = (float)m_lines * m_smallStyle.lineHeight + 24f + 8f + 40f;
			float num3 = ((screenPosition.x < 0f) ? ((float)Screen.width + screenPosition.x - num) : screenPosition.x);
			float y = ((screenPosition.y < 0f) ? ((float)Screen.height + screenPosition.y - num2) : screenPosition.y);
			GUI.Box(new Rect(num3, y, num, num2), "Telemetry");
			GUI.Label(new Rect(num3 + 8f, screenPosition.y + 10f, num, (float)Screen.height - num2), m_text, m_smallStyle);
			GUI.Label(new Rect(num3 + 8f, screenPosition.y + num2 + 8f - 40f, num, 200f), m_bigText, m_bigStyle);
		}
	}

	public void UpdateTextProperties()
	{
		m_smallStyle.font = font;
		m_smallStyle.fontSize = 10;
		m_smallStyle.normal.textColor = Color.white;
		m_bigStyle.font = font;
		m_bigStyle.fontSize = 20;
		m_bigStyle.normal.textColor = Color.white;
	}

	public string GetGearStr(int gear)
	{
		if (gear > 0)
		{
			return gear.ToString();
		}
		if (gear < 0)
		{
			if (gear == -1)
			{
				return "R";
			}
			return "R" + -gear;
		}
		return "N";
	}

	public string GetGearModeStr(Gearbox.AutomaticGear gearMode)
	{
		return gearMode switch
		{
			Gearbox.AutomaticGear.const_0 => "[M]P R N D L ", 
			Gearbox.AutomaticGear.const_1 => " M[P]R N D L ", 
			Gearbox.AutomaticGear.const_2 => " M P[R]N D L ", 
			Gearbox.AutomaticGear.const_3 => " M P R[N]D L ", 
			Gearbox.AutomaticGear.const_4 => " M P R N[D]L ", 
			Gearbox.AutomaticGear.const_5 => " M P R N D[L]", 
			_ => " M P R N D L ", 
		};
	}

	public string GetWheelTelemetryStr(VehicleBase.WheelState wheel)
	{
		if (wheel.grounded)
		{
			return string.Format("{0,-12}{1,4:0.}{12}|{2,5:0.0} |{3,5:0.00} |{4,6:0.} | Td:{5,6:0.} Tb:{6,6:0.} Tr:{7,6:0.} | Fx:{8,6:0.} Fy:{9,6:0.} | Sx:{10,5:0.00} Sy:{11,5:0.00} Sc:{13,5:0.00}\n", (wheel.wheelCol != null) ? wheel.wheelCol.gameObject.name : "?", wheel.angularVelocity * Block.WToRpm, wheel.steerAngle, contactDepthAsSuspension ? wheel.contactDepth : wheel.suspensionCompression, showLoadInKg ? (wheel.downforce * (1f / base.vehicle.gravity.Magnitude)) : wheel.downforce, wheel.driveTorque, wheel.brakeTorque, wheel.reactionTorque, wheel.tireForce.x, wheel.tireForce.y, wheel.tireSlip.x, wheel.tireSlip.y, wheel.wheelCol.canSleep ? "·" : " ", wheel.combinedTireSlip);
		}
		return string.Format("{0,-12}{1,4:0.} |{2,5:0.0} |{3,5:0.00} |{4,6:0.} | Td:{5,6:0.} Tb:{6,6:0.} Tr:{7,6:0.} |    {8,6:0.}    {9,6:0.} |    {10,5:0.00}    {11,5:0.00}\n", (wheel.wheelCol != null) ? wheel.wheelCol.gameObject.name : "?", wheel.angularVelocity * Block.WToRpm, wheel.steerAngle, contactDepthAsSuspension ? wheel.contactDepth : wheel.suspensionCompression, "", wheel.driveTorque, wheel.brakeTorque, wheel.reactionTorque, "", "", "", "");
	}

	public void DoTelemetry()
	{
		string text = (contactDepthAsSuspension ? "m" : "%");
		string text2 = (showLoadInKg ? "Kg" : " N");
		m_text = $"{base.vehicle.gameObject.name}\n{m_frameNum,8}    Spin |Steer | Susp | Load  |  Torque (drive/brake/react)   |  Force (lat/long)   |  Slip (lat/long/comb)\n             rpm |   °  |   {text}  |  {text2}   |    Nm                         |   N                 |   m/s\n";
		m_lines = 2;
		VehicleBase.WheelState[] wheelState = base.vehicle.wheelState;
		float num = 0f;
		int i = 0;
		for (int num2 = wheelState.Length; i < num2; i++)
		{
			if (!wheelState[i].wheelCol.hidden)
			{
				m_text += GetWheelTelemetryStr(wheelState[i]);
				num += wheelState[i].downforce;
				m_lines++;
			}
		}
		float num3 = num * (1f / base.vehicle.gravity.Magnitude);
		m_text += $"                       ∑ {num,6:0.} ({num3:0.} Kg)\n";
		m_lines++;
		float speed = base.vehicle.speed;
		float num4 = Vector3.Dot(m_velocity, base.vehicle.cachedTransform.right);
		float num5 = ComputeSlope();
		float num6 = Mathf.Tan(num5 * ((float)Math.PI / 180f)) * 100f;
		Vector3 eulerAngles = base.vehicle.cachedTransform.rotation.eulerAngles;
		float num7 = MathUtility.ClampAngle(eulerAngles.x);
		float num8 = MathUtility.ClampAngle(eulerAngles.z);
		m_text += string.Format("\nSpeed (long/lat/abs)     {0,6:0.00} {1,6:0.00} {2,6:0.00}  m/s    {3,5:0.0} km/h {4,5:0.0} mph   {5,6:0.0}°\nAcceleration             {6,6:0.00} {7,6:0.00}  m/s²           {8,4:0.0} G\nYaw rate                 {9,6:0.00}  rads/s\nAngles (pitch/roll)      {10,6:0.00}°{11,6:0.00}°                 Road grade:{12,5:0.0}% {13,6}°\n", speed, num4, m_velocity.magnitude, speed * 3.6f, speed * 2.237f, base.vehicle.speedAngle, base.vehicle.localAcceleration.z, base.vehicle.localAcceleration.x, base.vehicle.localAcceleration.magnitude / base.vehicle.gravity.Magnitude, m_angularVelocity.y, num7, num8, num6, "(" + num5.ToString("0.0") + ")");
		m_lines += 5;
		int[] array = base.vehicle.data.Get(1);
		bool flag = array[2] == 0 && array[3] == 0;
		bool flag2 = array[2] != 0;
		float num9 = (float)array[7] / 1000f;
		float num10 = (float)array[8] / 1000f;
		float num11 = (float)array[6] / 1000f;
		float num12 = (float)array[9] / 1000f;
		float num13 = (float)array[10] / 1000f;
		float num14 = (float)array[11] / 1000f;
		float num15 = (float)array[16] / 1000f;
		int num16 = base.vehicle.data.Get(0, 8);
		float num17 = (float)array[15] / 1000f;
		m_text += string.Format("\nEngine (load/torque/pwr) {0}{1,7:0.0} Nm{2,7:0.0} kW     {3}\n", (num11 < 0f) ? "  n/a " : $"{num11 * 100f,5:0.0}%", num9, num10, flag ? "[--OFF--]" : (flag2 ? "[STALLED]" : "[--ON---]"));
		m_text += string.Format("Clutch (lock/torque)     {0}{1,7:0.0} Nm\n", (num14 < 0f) ? "  n/a " : $"{num14 * 100f,5:0.0}%", num13);
		m_text += $"Transmission (rpm)      {num15,6:0.}\n";
		m_text += ((num16 > 0) ? $"Retarder torque               {num17,6:0.0} Nm\n" : "\n");
		float num18 = 100f * num12 / speed / fuelDensity;
		num18 /= fuelConsumptionCorrection;
		m_text += string.Format("Fuel consumption         {0,5:0.0} g/s  {1} l/100km\n", (num12 < 0f) ? 0f : num12, (!(num12 >= 0f) || speed <= 5.555556f) ? "  -.-" : $"{num18,5:0.0}");
		m_lines += 5;
		StabilityControl stabilityControl = (StabilityControl)base.vehicle.GetInternalObject(typeof(StabilityControl));
		if (stabilityControl != null)
		{
			m_text += $"Stability Control (ESC)    Understeer: {stabilityControl.sensorUndersteerL,5:0.00}L {stabilityControl.sensorUndersteerR,5:0.00}R  Oversteer: {stabilityControl.sensorOversteerL,5:0.00}L {stabilityControl.sensorOversteerR,5:0.00}R";
			m_lines++;
		}
		AntiSpin antiSpin = (AntiSpin)base.vehicle.GetInternalObject(typeof(AntiSpin));
		if (antiSpin != null)
		{
			m_text += $"\nAnti Spin (ASR)            Rpm:{antiSpin.stateAngularVelocityL * Block.WToRpm,5:0.}L{antiSpin.stateAngularVelocityR * Block.WToRpm,5:0.}R  Diff:{(antiSpin.stateAngularVelocityL - antiSpin.stateAngularVelocityR) * Block.WToRpm,5:0.}";
			m_lines++;
		}
		if (!string.IsNullOrEmpty(customData))
		{
			m_text = m_text + "\n\n\n\n" + customData;
			customData = "";
		}
		float num19 = (float)array[1] / 1000f;
		int gear = array[12];
		Gearbox.AutomaticGear gearMode = (Gearbox.AutomaticGear)array[13];
		bool flag3 = array[4] != 0;
		bool flag4 = array[14] != 0;
		m_bigText = string.Format("{0,6:0.}{1}rpm  {2}{3} {4}", num19, flag3 ? "·" : " ", GetGearStr(gear), flag4 ? "·" : " ", GetGearModeStr(gearMode));
		if (num16 > 0)
		{
			m_bigText = m_bigText + "  RET" + num16;
		}
		if (array[17] != 0)
		{
			m_bigText += "  ABS";
		}
		if (array[18] != 0)
		{
			m_bigText += "  TCS";
		}
		if (array[19] != 0)
		{
			m_bigText += "  ESC";
		}
		if (array[20] != 0)
		{
			m_bigText += "  ASR";
		}
	}

	public float ComputeSlope()
	{
		if (!base.vehicle.initialized)
		{
			return 0f;
		}
		int groundedWheelIndex = GetGroundedWheelIndex(0);
		int groundedWheelIndex2 = GetGroundedWheelIndex(base.vehicle.GetAxleCount() - 1);
		if (groundedWheelIndex >= 0 && groundedWheelIndex2 >= 0)
		{
			Vector3 point = base.vehicle.wheelState[groundedWheelIndex].hit.point;
			Vector3 point2 = base.vehicle.wheelState[groundedWheelIndex2].hit.point;
			point = MathUtility.ClosestPointOnPlane(base.vehicle.cachedTransform.position, base.vehicle.cachedTransform.right, point);
			point2 = MathUtility.ClosestPointOnPlane(base.vehicle.cachedTransform.position, base.vehicle.cachedTransform.right, point2);
			return 90f - Vector3.Angle(point - point2, Vector3.up);
		}
		return 0f;
	}

	public int GetGroundedWheelIndex(int axle)
	{
		int wheelIndex = base.vehicle.GetWheelIndex(axle);
		if (wheelIndex < 0)
		{
			return -1;
		}
		if (base.vehicle.wheelState[wheelIndex].grounded)
		{
			return wheelIndex;
		}
		wheelIndex = base.vehicle.GetWheelIndex(axle, VehicleBase.WheelPos.Right);
		if (!base.vehicle.wheelState[wheelIndex].grounded)
		{
			return -1;
		}
		return wheelIndex;
	}
}
