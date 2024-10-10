using System;
using ns9;
using UnityEngine;

[Serializable]
public class VesselAutopilot
{
	[Serializable]
	public class VesselSAS
	{
		[NonSerialized]
		public Vessel vessel;

		public PIDclamp pidLockedPitch;

		public PIDclamp pidLockedRoll;

		public PIDclamp pidLockedYaw;

		public float sasTuningScalar = 1f;

		public bool sasEquippedVessel;

		public bool FBWconnected;

		public bool isStarted;

		public bool dampingMode;

		public bool lockedMode = true;

		public float overrideMinimumMagnitude = 0.1f;

		public float controlDetectionThreshold = 0.05f;

		public Quaternion lockedRotation;

		public Quaternion currentRotation = Quaternion.identity;

		public Quaternion lastRotation = Quaternion.identity;

		public Vector3 rotationDelta = Vector3.zero;

		public Vector3d angularDelta = Vector3d.zero;

		public Vector3d angularDeltaRad = Vector3d.zero;

		public Vector3d sasResponse = Vector3d.zero;

		public Transform storedTransform;

		public bool autoTune = true;

		public Vector3 autoScalar;

		public Vector3 autoScalarMin = new Vector3(0.5f, 0.5f, 0.5f);

		public Vector3 autoScalarMax = new Vector3(10f, 10f, 10f);

		public Vector3 scalarFactor = new Vector3(-0.3f, -0.3f, -0.3f);

		public Vector3 scalarIntx = new Vector3(2f, 2f, 2f);

		public float scalarRate = 0.9f;

		public Vector3 torqueVector;

		public Vector3 angularAccelerationMax;

		public Vector3 posTorque;

		public Vector3 negTorque;

		public Vector3 targetOrientation;

		public float neededPitch;

		public float neededYaw;

		public float stopScalar = 1f;

		public float coastScalar = 2f;

		public bool useDecay = true;

		public bool decayLocked;

		public Vector3 decayResponseThreshold = 0.02f * Vector3.one;

		public Vector3 decayDeltaThreshold = 0.1f * Vector3.one;

		public Vector3 decayRate = 0.005f * Vector3.one;

		public Vector3 decayMin = 0.1f * Vector3.one;

		public Vector3 decayScalar = Vector3.one;

		public int dampingCooldownTimer = 20;

		public int dampingCooldown = 20;

		public bool pitchInput;

		public bool rollInput;

		public bool yawInput;

		public VesselSAS(Vessel v)
		{
			SetLockPitchPID(15000f, 0f, 2000f, 1500f);
			SetLockRollPID(9000f, 0f, 600f, 2000f);
			SetLockYawPID(15000f, 0f, 2000f, 1500f);
			SetVessel(v);
		}

		public void SetLockYawPID(float Kp, float Ki, float Kd, float clamp)
		{
			pidLockedYaw = new PIDclamp("Torque Yaw", Kp, Ki, Kd, clamp);
		}

		public void SetLockRollPID(float Kp, float Ki, float Kd, float clamp)
		{
			pidLockedRoll = new PIDclamp("Torque Roll", Kp, Ki, Kd, clamp);
		}

		public void SetLockPitchPID(float Kp, float Ki, float Kd, float clamp)
		{
			pidLockedPitch = new PIDclamp("Torque Pitch", Kp, Ki, Kd, clamp);
		}

		public void ResetAllPIDS()
		{
			pidLockedPitch.Reset();
			pidLockedRoll.Reset();
			pidLockedYaw.Reset();
		}

		public void ResetTuningScalars()
		{
			pidLockedPitch.tuningScalar = sasTuningScalar;
			pidLockedRoll.tuningScalar = sasTuningScalar;
			pidLockedYaw.tuningScalar = sasTuningScalar;
			AutoTuneReset();
		}

		public void TuneScalars(float scalar)
		{
			pidLockedPitch.tuningScalar = scalar;
			pidLockedRoll.tuningScalar = scalar;
			pidLockedYaw.tuningScalar = scalar;
		}

		public void SetTuningScalar(float scalar)
		{
			sasTuningScalar = scalar;
			ResetTuningScalars();
		}

		public void SetVessel(Vessel v)
		{
			vessel = v;
			storedTransform = v.ReferenceTransform;
		}

		public bool CanEngageSAS()
		{
			return AutopilotMode.StabilityAssist.AvailableAtLevel(vessel);
		}

		public void LockRotation(Quaternion newRotation)
		{
			lockedRotation = newRotation;
		}

		public void Start()
		{
			GameEvents.onVesselWasModified.Add(VesselModified);
			if (vessel != null)
			{
				LockRotation(vessel.ReferenceTransform.rotation);
				ResetAllPIDS();
			}
			sasEquippedVessel = CanEngageSAS();
			isStarted = true;
		}

		public void Update()
		{
			if (!isStarted)
			{
				Start();
			}
			sasEquippedVessel = CanEngageSAS();
			if (HighLogic.LoadedSceneIsFlight)
			{
				SASUpdate();
			}
		}

		public void SASUpdate()
		{
			if (!(vessel != null))
			{
				return;
			}
			if (!sasEquippedVessel)
			{
				if (FBWconnected)
				{
					DisconnectFlyByWire();
					ResetAllPIDS();
				}
			}
			else
			{
				CheckStoredTransform();
			}
		}

		public void CheckStoredTransform()
		{
			if (storedTransform != vessel.ReferenceTransform || storedTransform == null)
			{
				storedTransform = vessel.ReferenceTransform;
				LockRotation(storedTransform.rotation);
				ResetAllPIDS();
			}
		}

		public bool CheckYawInput()
		{
			if (FlightGlobals.ActiveVessel != vessel)
			{
				return false;
			}
			bool result = false;
			if (GameSettings.AXIS_YAW.deadzone == 0f)
			{
				if (Mathf.Abs(GameSettings.AXIS_YAW.GetAxis()) > controlDetectionThreshold)
				{
					result = true;
				}
			}
			else if (GameSettings.AXIS_YAW.GetAxis() != 0f)
			{
				result = true;
			}
			if (FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice) && Mathf.Abs(SpaceNavigator.Rotation.Yaw() * GameSettings.SPACENAV_FLIGHT_SENS_ROT) > controlDetectionThreshold)
			{
				result = true;
			}
			if (ExtendedInput.GetKey(GameSettings.YAW_LEFT.primary) || ExtendedInput.GetKey(GameSettings.YAW_LEFT.secondary) || ExtendedInput.GetKey(GameSettings.YAW_RIGHT.primary) || ExtendedInput.GetKey(GameSettings.YAW_RIGHT.secondary))
			{
				result = true;
			}
			return result;
		}

		public bool CheckRollInput()
		{
			if (FlightGlobals.ActiveVessel != vessel)
			{
				return false;
			}
			bool result = false;
			if (GameSettings.AXIS_ROLL.deadzone == 0f)
			{
				if (Mathf.Abs(GameSettings.AXIS_ROLL.GetAxis()) > controlDetectionThreshold)
				{
					result = true;
				}
			}
			else if (GameSettings.AXIS_ROLL.GetAxis() != 0f)
			{
				result = true;
			}
			if (FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice) && Mathf.Abs(SpaceNavigator.Rotation.Roll() * GameSettings.SPACENAV_FLIGHT_SENS_ROT) > controlDetectionThreshold)
			{
				result = true;
			}
			if (ExtendedInput.GetKey(GameSettings.ROLL_LEFT.primary) || ExtendedInput.GetKey(GameSettings.ROLL_LEFT.secondary) || ExtendedInput.GetKey(GameSettings.ROLL_RIGHT.primary) || ExtendedInput.GetKey(GameSettings.ROLL_RIGHT.secondary))
			{
				result = true;
			}
			return result;
		}

		public bool CheckPitchInput()
		{
			if (FlightGlobals.ActiveVessel != vessel)
			{
				return false;
			}
			bool result = false;
			if (GameSettings.AXIS_PITCH.deadzone == 0f)
			{
				if (Mathf.Abs(GameSettings.AXIS_PITCH.GetAxis()) > controlDetectionThreshold)
				{
					result = true;
				}
			}
			else if (GameSettings.AXIS_PITCH.GetAxis() != 0f)
			{
				result = true;
			}
			if (FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL && SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice) && Mathf.Abs(SpaceNavigator.Rotation.Pitch() * GameSettings.SPACENAV_FLIGHT_SENS_ROT) > controlDetectionThreshold)
			{
				result = true;
			}
			if (ExtendedInput.GetKey(GameSettings.PITCH_UP.primary) || ExtendedInput.GetKey(GameSettings.PITCH_UP.secondary) || ExtendedInput.GetKey(GameSettings.PITCH_DOWN.primary) || ExtendedInput.GetKey(GameSettings.PITCH_DOWN.secondary))
			{
				result = true;
			}
			return result;
		}

		public bool UpdatePitchInput()
		{
			if (CheckPitchInput() && vessel.ctrlState.pitch != 0f)
			{
				if (vessel.ctrlState.pitch < 0f)
				{
					if (!(sasResponse.x < 0.0))
					{
						sasResponse.x = vessel.ctrlState.pitch;
						return true;
					}
					if ((double)vessel.ctrlState.pitch < sasResponse.x)
					{
						sasResponse.x = vessel.ctrlState.pitch;
						return true;
					}
				}
				else
				{
					if (!(sasResponse.x > 0.0))
					{
						sasResponse.x = vessel.ctrlState.pitch;
						return true;
					}
					if ((double)vessel.ctrlState.pitch > sasResponse.x)
					{
						sasResponse.x = vessel.ctrlState.pitch;
						return true;
					}
				}
			}
			return false;
		}

		public bool UpdateRollInput()
		{
			if (CheckRollInput() && vessel.ctrlState.roll != 0f)
			{
				if (vessel.ctrlState.roll < 0f)
				{
					if (!(sasResponse.y < 0.0))
					{
						sasResponse.y = vessel.ctrlState.roll;
						return true;
					}
					if ((double)vessel.ctrlState.roll < sasResponse.y)
					{
						sasResponse.y = vessel.ctrlState.roll;
						return true;
					}
				}
				else
				{
					if (!(sasResponse.y > 0.0))
					{
						sasResponse.y = vessel.ctrlState.roll;
						return true;
					}
					if ((double)vessel.ctrlState.roll > sasResponse.y)
					{
						sasResponse.y = vessel.ctrlState.roll;
						return true;
					}
				}
			}
			return false;
		}

		public bool UpdateYawInput()
		{
			if (CheckYawInput() && vessel.ctrlState.yaw != 0f)
			{
				if (vessel.ctrlState.yaw < 0f)
				{
					if (!(sasResponse.z < 0.0))
					{
						sasResponse.z = vessel.ctrlState.yaw;
						return true;
					}
					if ((double)vessel.ctrlState.yaw < sasResponse.z)
					{
						sasResponse.z = vessel.ctrlState.yaw;
						return true;
					}
				}
				else
				{
					if (!(sasResponse.z > 0.0))
					{
						sasResponse.z = vessel.ctrlState.yaw;
						return true;
					}
					if ((double)vessel.ctrlState.yaw > sasResponse.z)
					{
						sasResponse.z = vessel.ctrlState.yaw;
						return true;
					}
				}
			}
			return false;
		}

		public float EvaluateScalar(float ratio)
		{
			if (ratio <= 0f)
			{
				return 10f;
			}
			if ((double)ratio <= 0.1)
			{
				return -90f * ratio + 10f;
			}
			if (ratio <= 10f)
			{
				return 1f;
			}
			if (ratio <= 50f)
			{
				return -0.0167f * ratio + 1.1667f;
			}
			return 0.5f;
		}

		public void AutoTuneScalar()
		{
			if (autoTune)
			{
				float value = Mathf.Min(autoScalarMax.x, EvaluateScalar(angularAccelerationMax.x));
				autoScalar.x = Mathf.Clamp(value, autoScalarMin.x, autoScalarMax.x);
				value = Mathf.Min(autoScalarMax.y, EvaluateScalar(angularAccelerationMax.y));
				autoScalar.y = Mathf.Clamp(value, autoScalarMin.y, autoScalarMax.y);
				value = Mathf.Min(autoScalarMax.z, EvaluateScalar(angularAccelerationMax.z));
				autoScalar.z = Mathf.Clamp(value, autoScalarMin.z, autoScalarMax.z);
				value = autoScalar.x;
				pidLockedPitch.tuningScalar = sasTuningScalar * value / TimeWarp.CurrentRate;
				pidLockedPitch.clampScalar = TimeWarp.CurrentRate / decayScalar.x;
				value = autoScalar.y;
				pidLockedRoll.tuningScalar = sasTuningScalar * value / TimeWarp.CurrentRate;
				pidLockedRoll.clampScalar = TimeWarp.CurrentRate / decayScalar.y;
				value = autoScalar.z;
				pidLockedYaw.tuningScalar = sasTuningScalar * value / TimeWarp.CurrentRate;
				pidLockedYaw.clampScalar = TimeWarp.CurrentRate / decayScalar.z;
			}
			else
			{
				TuneScalars(sasTuningScalar / TimeWarp.CurrentRate);
			}
		}

		public void AutoTuneReset()
		{
			autoScalar = Vector3.one;
			pidLockedPitch.kdScalar = 1f;
			pidLockedRoll.kdScalar = 1f;
			pidLockedYaw.kdScalar = 1f;
		}

		public Vector3 GetTotalVesselTorque(Vessel v)
		{
			Vector3 zero = Vector3.zero;
			posTorque = Vector3.zero;
			negTorque = Vector3.zero;
			int count = vessel.parts.Count;
			while (count-- > 0)
			{
				Part part = vessel.parts[count];
				int count2 = part.Modules.Count;
				while (count2-- > 0)
				{
					if (part.Modules[count2] is ITorqueProvider torqueProvider)
					{
						torqueProvider.GetPotentialTorque(out var pos, out var neg);
						posTorque += pos;
						negTorque += neg;
					}
				}
			}
			zero.x = Mathf.Max(posTorque.x, negTorque.x);
			zero.y = Mathf.Max(posTorque.y, negTorque.y);
			zero.z = Mathf.Max(posTorque.z, negTorque.z);
			return zero;
		}

		public void UpdateMaximumAcceleration()
		{
			angularAccelerationMax.x = Mathf.Max(torqueVector.x / vessel.vector3_0.x, 0.0001f);
			angularAccelerationMax.y = Mathf.Max(torqueVector.y / vessel.vector3_0.y, 0.0001f);
			angularAccelerationMax.z = Mathf.Max(torqueVector.z / vessel.vector3_0.z, 0.0001f);
		}

		public void UpdateVesselTorque(FlightCtrlState s)
		{
			torqueVector = GetTotalVesselTorque(vessel);
		}

		public void SetDampingMode(bool isEnabled)
		{
			dampingMode = isEnabled;
		}

		public void ConnectFlyByWire(bool reset)
		{
			if (!FBWconnected)
			{
				Vessel obj = vessel;
				obj.OnAutopilotUpdate = (FlightInputCallback)Delegate.Combine(obj.OnAutopilotUpdate, new FlightInputCallback(ControlUpdate));
				Vessel obj2 = vessel;
				obj2.OnAutopilotUpdate = (FlightInputCallback)Delegate.Combine(obj2.OnAutopilotUpdate, new FlightInputCallback(UpdateVesselTorque));
				FBWconnected = true;
				if (!(storedTransform != vessel.ReferenceTransform) && !(storedTransform == null))
				{
					LockRotation(storedTransform.rotation);
				}
				else
				{
					storedTransform = vessel.ReferenceTransform;
					LockRotation(storedTransform.rotation);
				}
			}
			if (reset)
			{
				ResetAllPIDS();
			}
		}

		public void DisconnectFlyByWire()
		{
			targetOrientation = Vector3.zero;
			lockedRotation = Quaternion.identity;
			ResetAllPIDS();
			if (FBWconnected)
			{
				Vessel obj = vessel;
				obj.OnAutopilotUpdate = (FlightInputCallback)Delegate.Remove(obj.OnAutopilotUpdate, new FlightInputCallback(ControlUpdate));
				Vessel obj2 = vessel;
				obj2.OnAutopilotUpdate = (FlightInputCallback)Delegate.Remove(obj2.OnAutopilotUpdate, new FlightInputCallback(UpdateVesselTorque));
				FBWconnected = false;
			}
		}

		public void SetTargetOrientation(Vector3 tgtOrientation, bool reset)
		{
			ConnectFlyByWire(reset);
			targetOrientation = tgtOrientation.normalized;
			CheckStoredTransform();
		}

		public Vector3 PitchYawAngle(Transform t, Vector3 v2)
		{
			Vector3 vector = t.InverseTransformDirection(v2);
			Vector3 result = default(Vector3);
			result.x = 90f - Mathf.Atan2(vector.y, vector.z) * 57.29578f;
			if (result.x > 180f)
			{
				result.x -= 360f;
			}
			result.y = 0f;
			result.z = Mathf.Atan2(vector.x, vector.y) * 57.29578f;
			return result;
		}

		public void PitchYawAngle(Transform t, Vector3 v2, out float pitch, out float yaw)
		{
			Vector3 vector = t.InverseTransformDirection(v2);
			pitch = 90f - Mathf.Atan2(vector.y, vector.z) * 57.29578f;
			if (pitch > 180f)
			{
				pitch -= 360f;
			}
			yaw = Mathf.Atan2(vector.x, vector.y) * 57.29578f;
		}

		public void CheckCoasting()
		{
			if (angularAccelerationMax.x > 0f)
			{
				float num = Mathf.Abs(Time.deltaTime + vessel.angularVelocity.x / angularAccelerationMax.x);
				float num2 = Mathf.Abs(rotationDelta.x * ((float)Math.PI / 180f) / vessel.angularVelocity.x);
				if (num * stopScalar > num2 && Math.Sign(rotationDelta.x) != Math.Sign(vessel.angularVelocity.x))
				{
					pidLockedPitch.Reset();
					rotationDelta.x = 0f - rotationDelta.x;
				}
				else if (num * coastScalar > num2 && Math.Sign(rotationDelta.x) != Math.Sign(vessel.angularVelocity.x))
				{
					pidLockedPitch.Reset();
					rotationDelta.x = 0f;
				}
			}
			if (angularAccelerationMax.z > 0f)
			{
				float num3 = Mathf.Abs(Time.deltaTime + vessel.angularVelocity.z / angularAccelerationMax.z);
				float num4 = Mathf.Abs(rotationDelta.z * ((float)Math.PI / 180f) / vessel.angularVelocity.z);
				if (num3 * stopScalar > num4 && Math.Sign(rotationDelta.z) != Math.Sign(vessel.angularVelocity.z))
				{
					pidLockedYaw.Reset();
					rotationDelta.z = 0f - rotationDelta.z;
				}
				else if (num3 * coastScalar > num4 && Math.Sign(rotationDelta.z) != Math.Sign(vessel.angularVelocity.z))
				{
					pidLockedYaw.Reset();
					rotationDelta.z = 0f;
				}
			}
		}

		public void StabilityDecay()
		{
			if (useDecay)
			{
				if (Math.Abs(sasResponse.x) < (double)decayResponseThreshold.x && angularDeltaRad.x < (double)decayDeltaThreshold.x)
				{
					decayScalar.x = Mathf.Max(decayScalar.x - decayRate.x, decayMin.x);
				}
				else
				{
					decayScalar.x = Mathf.Min(decayScalar.x + 0.1f, 1f);
				}
				if (Math.Abs(sasResponse.y) < (double)decayResponseThreshold.y && angularDeltaRad.y < (double)decayDeltaThreshold.y)
				{
					decayScalar.y = Mathf.Max(decayScalar.y - decayRate.y, decayMin.y);
				}
				else
				{
					decayScalar.y = Mathf.Min(decayScalar.y + 0.1f, 1f);
				}
				if (Math.Abs(sasResponse.z) < (double)decayResponseThreshold.z && angularDeltaRad.z < (double)decayDeltaThreshold.z)
				{
					decayScalar.z = Mathf.Max(decayScalar.z - decayRate.z, decayMin.z);
				}
				else
				{
					decayScalar.z = Mathf.Min(decayScalar.z + 0.1f, 1f);
				}
				if (decayScalar == decayMin)
				{
					if (!decayLocked)
					{
						decayLocked = true;
						LockRotation(currentRotation);
						KillAngularVelocity();
						ResetAllPIDS();
					}
				}
				else
				{
					decayLocked = false;
				}
			}
			else
			{
				decayScalar = Vector3.one;
			}
		}

		public void KillAngularVelocity()
		{
			int count = vessel.parts.Count;
			while (count-- > 0)
			{
				if (!vessel.parts[count].isRoboticRotor() && (!vessel.parts[count].isChildOfRoboticRotor() || !vessel.parts[count].isControlSurface()))
				{
					Rigidbody rb = vessel.parts[count].rb;
					if (rb != null)
					{
						rb.angularVelocity = Vector3.zero;
					}
				}
			}
		}

		public void ResetDecay()
		{
			decayScalar = 1f * Vector3.one;
		}

		public void CheckDamping()
		{
			dampingMode = false;
			pitchInput = UpdatePitchInput();
			rollInput = UpdateRollInput();
			yawInput = UpdateYawInput();
			if (pitchInput)
			{
				LockRotation(currentRotation);
				pidLockedPitch.Reset();
				dampingMode = true;
			}
			if (rollInput)
			{
				LockRotation(currentRotation);
				pidLockedRoll.Reset();
				dampingMode = true;
			}
			if (yawInput)
			{
				LockRotation(currentRotation);
				pidLockedYaw.Reset();
				dampingMode = true;
			}
			if (!pitchInput && !rollInput && !yawInput)
			{
				if (--dampingCooldownTimer < 0)
				{
					dampingCooldownTimer = 0;
				}
			}
			else
			{
				dampingCooldownTimer = dampingCooldown;
			}
			if (Mathf.Abs(vessel.angularVelocity.x) > overrideMinimumMagnitude)
			{
				dampingMode = true;
				LockRotation(lastRotation);
				if (lockedMode)
				{
					if (dampingCooldownTimer > 0)
					{
						pidLockedPitch.Reset();
					}
					else
					{
						ResetAllPIDS();
					}
				}
			}
			if (Mathf.Abs(vessel.angularVelocity.y) > overrideMinimumMagnitude)
			{
				dampingMode = true;
				LockRotation(lastRotation);
				if (dampingCooldownTimer > 0)
				{
					pidLockedRoll.Reset();
				}
				else
				{
					ResetAllPIDS();
				}
			}
			if (!(Mathf.Abs(vessel.angularVelocity.z) > overrideMinimumMagnitude))
			{
				return;
			}
			dampingMode = true;
			LockRotation(lastRotation);
			if (lockedMode)
			{
				if (dampingCooldownTimer > 0)
				{
					pidLockedYaw.Reset();
				}
				else
				{
					ResetAllPIDS();
				}
			}
		}

		public Quaternion GetRotationDelta()
		{
			return Quaternion.Inverse(currentRotation) * lockedRotation;
		}

		public static float AngularTrim(float angle)
		{
			if (!(angle > 180f))
			{
				return angle;
			}
			return angle - 360f;
		}

		public void ControlUpdate(FlightCtrlState s)
		{
			if (!(storedTransform == null))
			{
				currentRotation = storedTransform.rotation;
				UpdateMaximumAcceleration();
				rotationDelta = Quaternion.Inverse(GetRotationDelta()).eulerAngles;
				if (!lockedMode)
				{
					PitchYawAngle(vessel.ReferenceTransform, targetOrientation, out neededPitch, out neededYaw);
					rotationDelta.x = 0f - neededPitch;
					rotationDelta.z = neededYaw;
					CheckCoasting();
				}
				else if (!dampingMode)
				{
					CheckCoasting();
				}
				angularDelta.x = AngularTrim(rotationDelta.x);
				angularDelta.y = AngularTrim(rotationDelta.y);
				angularDelta.z = AngularTrim(rotationDelta.z);
				angularDeltaRad = angularDelta * 0.01745329238474369;
				StabilityDecay();
				AutoTuneScalar();
				sasResponse.x = pidLockedPitch.Update(angularDeltaRad.x, TimeWarp.deltaTime) / pidLockedPitch.clamp;
				sasResponse.y = pidLockedRoll.Update(angularDeltaRad.y, TimeWarp.deltaTime) / pidLockedRoll.clamp;
				sasResponse.z = pidLockedYaw.Update(angularDeltaRad.z, TimeWarp.deltaTime) / pidLockedYaw.clamp;
				CheckDamping();
				sasResponse.x = UtilMath.Clamp(sasResponse.x, -1.0, 1.0);
				sasResponse.y = UtilMath.Clamp(sasResponse.y, -1.0, 1.0);
				sasResponse.z = UtilMath.Clamp(sasResponse.z, -1.0, 1.0);
				s.pitch = (float)sasResponse.x;
				s.roll = (float)sasResponse.y;
				s.yaw = (float)sasResponse.z;
				lastRotation = currentRotation;
			}
		}

		public void Destroy()
		{
			GameEvents.onVesselWasModified.Remove(VesselModified);
			DisconnectFlyByWire();
		}

		public void VesselModified(Vessel v)
		{
			sasEquippedVessel = CanEngageSAS();
			if (vessel == v && !lockedMode)
			{
				DisconnectFlyByWire();
			}
		}

		public void ModuleSetup()
		{
			sasEquippedVessel = CanEngageSAS();
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

	public VesselSAS sas;

	public Vessel vessel;

	public AutopilotMode mode;

	public bool enabled;

	public VesselSAS VesselSAS_0 => sas;

	public Vessel Vessel => vessel;

	public AutopilotMode Mode => mode;

	public bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			if (!enabled && value)
			{
				Enable();
			}
			else if (enabled && !value)
			{
				Disable();
			}
		}
	}

	public VesselAutopilot(Vessel vessel)
	{
		this.vessel = vessel;
		sas = new VesselSAS(vessel);
	}

	~VesselAutopilot()
	{
	}

	public void Destroy()
	{
		sas.Destroy();
	}

	public void SetupModules()
	{
		sas.ModuleSetup();
	}

	public void Update()
	{
		if (vessel.ActionGroups[KSPActionGroup.flag_6])
		{
			if (!enabled)
			{
				if (!Enable())
				{
					vessel.ActionGroups.SetGroup(KSPActionGroup.flag_6, active: false);
					if (FlightGlobals.ActiveVessel == vessel)
					{
						ScreenMessages.PostScreenMessage("<color=orange><b>" + Localizer.Format("#autoLOC_6003109", vessel.GetDisplayName()) + "</b></color>", 5f);
					}
				}
			}
			else if (!CanSetMode(mode))
			{
				if (mode != 0 && CanSetMode(AutopilotMode.StabilityAssist))
				{
					SetMode(AutopilotMode.StabilityAssist);
				}
				else
				{
					vessel.ActionGroups.SetGroup(KSPActionGroup.flag_6, active: false);
					if (FlightGlobals.ActiveVessel == vessel)
					{
						ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_137027"), 5f, ScreenMessageStyle.UPPER_CENTER);
					}
					else
					{
						ScreenMessages.PostScreenMessage("<color=orange><b>" + Localizer.Format("#autoLOC_6003110", vessel.GetDisplayName()) + "</b></color>", 5f, ScreenMessageStyle.UPPER_LEFT);
					}
				}
			}
		}
		else if (enabled)
		{
			Disable();
		}
		if (enabled)
		{
			SetAutopilot(initialize: false);
		}
	}

	public bool Enable()
	{
		return Enable(AutopilotMode.StabilityAssist);
	}

	public bool Enable(AutopilotMode mode)
	{
		if (enabled)
		{
			Disable();
		}
		this.mode = mode;
		sas.lockedMode = mode == AutopilotMode.StabilityAssist;
		if (!SetAutopilot(initialize: true))
		{
			return false;
		}
		enabled = true;
		return true;
	}

	public bool Disable()
	{
		sas.DisconnectFlyByWire();
		enabled = false;
		vessel.ctrlState.NeutralizeStick();
		return true;
	}

	public bool SetAutopilot(bool initialize)
	{
		return FlightGlobals.speedDisplayMode switch
		{
			FlightGlobals.SpeedDisplayModes.Surface => SetAutopilotSurface(initialize), 
			FlightGlobals.SpeedDisplayModes.Target => SetAutopilotTarget(initialize), 
			_ => SetAutopilotOrbit(initialize), 
		};
	}

	public bool SetMode(AutopilotMode mode)
	{
		if (!CanSetMode(mode))
		{
			return false;
		}
		if (enabled)
		{
			return Enable(mode);
		}
		this.mode = mode;
		sas.lockedMode = mode == AutopilotMode.StabilityAssist;
		return true;
	}

	public bool CanSetMode(AutopilotMode mode)
	{
		if (!mode.AvailableAtLevel(vessel))
		{
			return false;
		}
		switch (mode)
		{
		case AutopilotMode.Prograde:
		case AutopilotMode.Retrograde:
			if (VectorLockInvalid(vessel, 1f))
			{
				return false;
			}
			break;
		case AutopilotMode.Normal:
		case AutopilotMode.Antinormal:
		case AutopilotMode.RadialIn:
		case AutopilotMode.RadialOut:
			if (FlightGlobals.speedDisplayMode == FlightGlobals.SpeedDisplayModes.Orbit && VectorLockInvalid(vessel, 1f))
			{
				return false;
			}
			break;
		case AutopilotMode.Target:
		case AutopilotMode.AntiTarget:
			if (TargetLockInvalid(vessel))
			{
				return false;
			}
			break;
		case AutopilotMode.Maneuver:
			if (ManeuverLockInvalid(vessel.patchedConicSolver))
			{
				return false;
			}
			break;
		}
		return true;
	}

	public bool SetAutopilotOrbit(bool initialize)
	{
		switch (mode)
		{
		case AutopilotMode.StabilityAssist:
			if (initialize)
			{
				if (!VesselSAS_0.CanEngageSAS())
				{
					return false;
				}
				sas.ConnectFlyByWire(reset: true);
			}
			else
			{
				sas.Update();
			}
			goto default;
		case AutopilotMode.Prograde:
			sas.SetTargetOrientation(vessel.obt_velocity, initialize);
			goto default;
		case AutopilotMode.Retrograde:
			sas.SetTargetOrientation(-vessel.obt_velocity, initialize);
			goto default;
		case AutopilotMode.Normal:
			sas.SetTargetOrientation(vessel.orbit.h.xzy, initialize);
			goto default;
		case AutopilotMode.Antinormal:
			sas.SetTargetOrientation(-vessel.orbit.h.xzy, initialize);
			goto default;
		case AutopilotMode.RadialIn:
			sas.SetTargetOrientation(-Vector3.Cross(vessel.obt_velocity, vessel.orbit.h.xzy), initialize);
			goto default;
		case AutopilotMode.RadialOut:
			sas.SetTargetOrientation(Vector3.Cross(vessel.obt_velocity, vessel.orbit.h.xzy), initialize);
			goto default;
		case AutopilotMode.Target:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.targetObject.GetTransform().position - vessel.ReferenceTransform.position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.AntiTarget:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.ReferenceTransform.position - vessel.targetObject.GetTransform().position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.Maneuver:
			if (!(vessel.patchedConicSolver != null) || vessel.patchedConicSolver.maneuverNodes.Count <= 0)
			{
				break;
			}
			sas.SetTargetOrientation(vessel.patchedConicSolver.maneuverNodes[0].GetBurnVector(vessel.orbit), initialize);
			goto default;
		default:
			return true;
		}
		return false;
	}

	public bool SetAutopilotSurface(bool initialize)
	{
		switch (mode)
		{
		case AutopilotMode.StabilityAssist:
			if (initialize)
			{
				if (!sas.CanEngageSAS())
				{
					return false;
				}
				sas.ConnectFlyByWire(reset: true);
			}
			else
			{
				sas.Update();
			}
			goto default;
		case AutopilotMode.Prograde:
			sas.SetTargetOrientation(vessel.srf_velocity, initialize);
			goto default;
		case AutopilotMode.Retrograde:
			sas.SetTargetOrientation(-vessel.srf_velocity, initialize);
			goto default;
		case AutopilotMode.Normal:
			sas.SetTargetOrientation(vessel.mainBody.RotationAxis, initialize);
			goto default;
		case AutopilotMode.Antinormal:
			sas.SetTargetOrientation(-vessel.mainBody.RotationAxis, initialize);
			goto default;
		case AutopilotMode.RadialIn:
			sas.SetTargetOrientation(vessel.upAxis, initialize);
			goto default;
		case AutopilotMode.RadialOut:
			sas.SetTargetOrientation(-vessel.upAxis, initialize);
			goto default;
		case AutopilotMode.Target:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.targetObject.GetTransform().position - vessel.ReferenceTransform.position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.AntiTarget:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.ReferenceTransform.position - vessel.targetObject.GetTransform().position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.Maneuver:
			if (!(vessel.patchedConicSolver != null) || vessel.patchedConicSolver.maneuverNodes.Count <= 0)
			{
				break;
			}
			sas.SetTargetOrientation(vessel.patchedConicSolver.maneuverNodes[0].GetBurnVector(vessel.orbit), initialize);
			goto default;
		default:
			return true;
		}
		return false;
	}

	public bool SetAutopilotTarget(bool initialize)
	{
		if (sas == null)
		{
			return false;
		}
		switch (mode)
		{
		case AutopilotMode.StabilityAssist:
			if (initialize)
			{
				if (!sas.CanEngageSAS())
				{
					return false;
				}
				sas.ConnectFlyByWire(reset: true);
			}
			else
			{
				sas.Update();
			}
			goto default;
		case AutopilotMode.Prograde:
			sas.SetTargetOrientation(vessel.obt_velocity - vessel.targetObject.GetObtVelocity(), initialize);
			goto default;
		case AutopilotMode.Retrograde:
			sas.SetTargetOrientation(-(vessel.obt_velocity - vessel.targetObject.GetObtVelocity()), initialize);
			goto default;
		case AutopilotMode.Normal:
			sas.SetTargetOrientation(vessel.mainBody.RotationAxis, initialize);
			goto default;
		case AutopilotMode.Antinormal:
			sas.SetTargetOrientation(-vessel.mainBody.RotationAxis, initialize);
			goto default;
		case AutopilotMode.RadialIn:
			sas.SetTargetOrientation(vessel.upAxis, initialize);
			goto default;
		case AutopilotMode.RadialOut:
			sas.SetTargetOrientation(-vessel.upAxis, initialize);
			goto default;
		case AutopilotMode.Target:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.targetObject.GetTransform().position - vessel.ReferenceTransform.position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.AntiTarget:
			if (vessel.targetObject != null)
			{
				sas.SetTargetOrientation((vessel.ReferenceTransform.position - vessel.targetObject.GetTransform().position).normalized, initialize);
				goto default;
			}
			return false;
		case AutopilotMode.Maneuver:
			if (!(vessel.patchedConicSolver != null) || vessel.patchedConicSolver.maneuverNodes.Count <= 0)
			{
				break;
			}
			sas.SetTargetOrientation(vessel.patchedConicSolver.maneuverNodes[0].GetBurnVector(vessel.orbit), initialize);
			goto default;
		default:
			return true;
		}
		return false;
	}

	public bool VectorLockInvalid(Vessel v, float threshold)
	{
		return FlightGlobals.GetDisplayVelocity().sqrMagnitude < (double)threshold;
	}

	public bool TargetLockInvalid(Vessel v)
	{
		return v.targetObject == null;
	}

	public bool ManeuverLockInvalid(PatchedConicSolver pcs)
	{
		if (!(pcs == null) && pcs.maneuverNodes.Count != 0)
		{
			if (pcs.maneuverNodes[0].DeltaV.sqrMagnitude > 1.0)
			{
				return pcs.maneuverNodes[0].GetBurnVector(vessel.orbit).sqrMagnitude < 1.0;
			}
			return false;
		}
		return true;
	}
}
