using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class VesselAutopilot
{
	[Serializable]
	public class VesselSAS
	{
		[NonSerialized]
		private Vessel vessel;

		public PIDclamp pidLockedPitch;

		public PIDclamp pidLockedRoll;

		public PIDclamp pidLockedYaw;

		private float sasTuningScalar;

		private bool sasEquippedVessel;

		private bool FBWconnected;

		private bool isStarted;

		public bool dampingMode;

		public bool lockedMode;

		public float overrideMinimumMagnitude;

		public float controlDetectionThreshold;

		public Quaternion lockedRotation;

		private Quaternion currentRotation;

		private Quaternion lastRotation;

		private Vector3 rotationDelta;

		private Vector3d angularDelta;

		private Vector3d angularDeltaRad;

		private Vector3d sasResponse;

		private Transform storedTransform;

		public bool autoTune;

		private Vector3 autoScalar;

		public Vector3 autoScalarMin;

		public Vector3 autoScalarMax;

		public Vector3 scalarFactor;

		public Vector3 scalarIntx;

		public float scalarRate;

		private Vector3 torqueVector;

		private Vector3 angularAccelerationMax;

		public Vector3 posTorque;

		public Vector3 negTorque;

		public Vector3 targetOrientation;

		private float neededPitch;

		private float neededYaw;

		public float stopScalar;

		public float coastScalar;

		public bool useDecay;

		private bool decayLocked;

		public Vector3 decayResponseThreshold;

		public Vector3 decayDeltaThreshold;

		public Vector3 decayRate;

		public Vector3 decayMin;

		private Vector3 decayScalar;

		private int dampingCooldownTimer;

		public int dampingCooldown;

		private bool pitchInput;

		private bool rollInput;

		private bool yawInput;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselSAS(Vessel v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetLockYawPID(float Kp, float Ki, float Kd, float clamp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetLockRollPID(float Kp, float Ki, float Kd, float clamp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetLockPitchPID(float Kp, float Ki, float Kd, float clamp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ResetAllPIDS()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ResetTuningScalars()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void TuneScalars(float scalar)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetTuningScalar(float scalar)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SetVessel(Vessel v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool CanEngageSAS()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void LockRotation(Quaternion newRotation)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Start()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SASUpdate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CheckStoredTransform()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckYawInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckRollInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckPitchInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool UpdatePitchInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool UpdateRollInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool UpdateYawInput()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float EvaluateScalar(float ratio)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AutoTuneScalar()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AutoTuneReset()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 GetTotalVesselTorque(Vessel v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateMaximumAcceleration()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void UpdateVesselTorque(FlightCtrlState s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetDampingMode(bool isEnabled)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ConnectFlyByWire(bool reset)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DisconnectFlyByWire()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetTargetOrientation(Vector3 tgtOrientation, bool reset)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 PitchYawAngle(Transform t, Vector3 v2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void PitchYawAngle(Transform t, Vector3 v2, out float pitch, out float yaw)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CheckCoasting()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void StabilityDecay()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void KillAngularVelocity()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ResetDecay()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CheckDamping()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Quaternion GetRotationDelta()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static float AngularTrim(float angle)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ControlUpdate(FlightCtrlState s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Destroy()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void VesselModified(Vessel v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ModuleSetup()
		{
			throw null;
		}
	}

	public enum AutopilotMode
	{
		StabilityAssist,
		Prograde,
		Retrograde,
		Normal,
		Antinormal,
		RadialIn,
		RadialOut,
		Target,
		AntiTarget,
		Maneuver
	}

	private VesselSAS sas;

	private Vessel vessel;

	private AutopilotMode mode;

	private bool enabled;

	public VesselSAS SAS
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel Vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public AutopilotMode Mode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Enabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselAutopilot(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~VesselAutopilot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Enable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Enable(AutopilotMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Disable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetAutopilot(bool initialize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetMode(AutopilotMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanSetMode(AutopilotMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetAutopilotOrbit(bool initialize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetAutopilotSurface(bool initialize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetAutopilotTarget(bool initialize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VectorLockInvalid(Vessel v, float threshold)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TargetLockInvalid(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ManeuverLockInvalid(PatchedConicSolver pcs)
	{
		throw null;
	}
}
