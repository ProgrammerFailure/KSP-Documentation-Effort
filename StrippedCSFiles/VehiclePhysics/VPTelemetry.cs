using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Telemetry Window", 0)]
public class VPTelemetry : VehicleBehaviour
{
	public bool showData;

	public bool contactDepthAsSuspension;

	public bool showLoadInKg;

	public Vector2 screenPosition;

	[Space(5f)]
	public bool enableHotKey;

	public KeyCode hotKey;

	[Space(5f)]
	public Font font;

	[Header("Debug Gizmos")]
	public bool showCenterOfMass;

	public bool gizmosAtPhysicPositions;

	[FormerlySerializedAs("showGizmos")]
	[Header("Wheel Gizmos")]
	public bool showWheelGizmos;

	public bool showLocalFrame;

	public bool showContactPoints;

	public bool showTireSlip;

	public bool showTireForces;

	public bool showSurfaceForces;

	public bool useLogScale;

	public static string customData;

	[Header("Fuel consumption (l/100km)")]
	public float fuelConsumptionCorrection;

	public float fuelDensity;

	private GUIStyle m_smallStyle;

	private GUIStyle m_bigStyle;

	private string m_text;

	private string m_bigText;

	private int m_lines;

	private int m_frameNum;

	private Vector3 m_velocity;

	private Vector3 m_angularVelocity;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPTelemetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VPTelemetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetGearStr(int gear)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetGearModeStr(Gearbox.AutomaticGear gearMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetWheelTelemetryStr(VehicleBase.WheelState wheel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoTelemetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ComputeSlope()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetGroundedWheelIndex(int axle)
	{
		throw null;
	}
}
