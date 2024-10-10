using ns9;
using UnityEngine;

public class VesselPrecalculate : MonoBehaviour
{
	public Vessel vessel;

	public bool physStatsNotDoneInUpdate = true;

	public bool ranFixedThisFrame = true;

	public bool packedInFixed = true;

	public double fDeltaTime;

	public double fDeltaTimeRecip;

	public double lastUT;

	public Vector3d worldSurfacePos;

	public QuaternionD worldSurfaceRot;

	public Vector3d gAccel;

	public Vector3d gAccelTrue;

	public Vector3d coriolis;

	public Vector3d centrifugal;

	public Vector3d integrationAccel;

	public Vector3d postIntegrationVelocityCorrection = Vector3d.zero;

	public Vector3d preIntegrationVelocityOffset = Vector3.zero;

	public static bool allowDriftCompensation = true;

	public static bool driftAlwaysPassThreshChecks = false;

	public static bool disableRunInUpdate = false;

	public bool easing;

	public bool easingLockOn;

	public double easingFrameIncrease = 0.0625;

	public bool correctDriftThisFrame;

	public bool wasCorrectingDrift;

	public Vector3d railsPosNext;

	public Vector3d railsVelNext;

	public Vector3d cpos;

	public Vector3d cvel;

	public Vector3d curRotFrameVel;

	public Vector3d rotFrameNext;

	public int framesUntilCorrect;

	public bool easeDocking;

	public bool firstStatsRunComplete;

	public OrbitDriver.UpdateMode lastMode;

	public bool railsSetPosRot = true;

	public bool updateOrbit = true;

	public bool calculateGravity = true;

	public static string cacheAutoLOC_6003101;

	public Vessel Vessel => vessel;

	public virtual bool isEasingGravity
	{
		get
		{
			return easing;
		}
		set
		{
			if (easing)
			{
				if (!value)
				{
					StopEasing();
				}
			}
			else if (value)
			{
				StartEasing();
			}
			easing = value;
		}
	}

	public virtual void Awake()
	{
		vessel = GetComponent<Vessel>();
		GameEvents.onDockingComplete.Add(onDockingComplete);
	}

	public virtual void OnDestroy()
	{
		StopEasing();
		GameEvents.onDockingComplete.Remove(onDockingComplete);
		if (vessel != null && vessel.orbitDriver != null)
		{
			vessel.orbitDriver.QueueOnce = false;
		}
	}

	public virtual void RunFirst()
	{
		if (vessel == null || vessel.transform == null || vessel.parts == null || vessel.parts.Count == 0)
		{
			return;
		}
		int count = vessel.parts.Count;
		do
		{
			if (count-- <= 0)
			{
				bool num = allowDriftCompensation;
				allowDriftCompensation = false;
				MainPhysics(doKillChecks: false);
				allowDriftCompensation = num;
				break;
			}
		}
		while (!(vessel.parts[count] == null));
	}

	public void FixedUpdate()
	{
		MainPhysics(doKillChecks: true);
	}

	public virtual void MainPhysics(bool doKillChecks)
	{
		packedInFixed = !vessel.loaded || vessel.packed;
		fDeltaTime = TimeWarp.fixedDeltaTime;
		fDeltaTimeRecip = 1.0 / fDeltaTime;
		vessel.orbitDriver.QueueOnce = !vessel.orbitDriver.QueuedUpdate;
		lastMode = vessel.orbitDriver.lastMode;
		if (vessel.orbitDriver.updateMode == OrbitDriver.UpdateMode.UPDATE && updateOrbit)
		{
			vessel.orbitDriver.UpdateOrbit();
		}
		else
		{
			SetLandedPosRot();
		}
		if (!vessel.packed)
		{
			ApplyVelocityCorrection();
		}
		wasCorrectingDrift = false;
		correctDriftThisFrame = false;
		postIntegrationVelocityCorrection.Zero();
		preIntegrationVelocityOffset.Zero();
		if (!vessel.packed && !physStatsNotDoneInUpdate)
		{
			if (calculateGravity)
			{
				CalculateGravity();
				vessel.graviticAcceleration = gAccel;
				ranFixedThisFrame = false;
			}
		}
		else
		{
			CalculatePhysicsStats();
			if (updateOrbit && vessel.orbitDriver.updateMode != OrbitDriver.UpdateMode.UPDATE)
			{
				vessel.orbitDriver.UpdateOrbit();
			}
			vessel.UpdatePosVel();
			if (calculateGravity && vessel.loaded)
			{
				CalculateGravity();
			}
			vessel.UpdateAcceleration(fDeltaTimeRecip, fromUpdate: false);
			if (calculateGravity)
			{
				vessel.graviticAcceleration = gAccel;
			}
			ranFixedThisFrame = true;
		}
		if (doKillChecks)
		{
			vessel.CheckKill();
		}
		physStatsNotDoneInUpdate = true;
	}

	public virtual void Update()
	{
		if (!packedInFixed && !disableRunInUpdate)
		{
			double universalTime = Planetarium.GetUniversalTime();
			if (lastUT != universalTime)
			{
				lastUT = universalTime;
				physStatsNotDoneInUpdate = false;
				ApplyVelocityCorrection();
				preIntegrationVelocityOffset.Zero();
				CalculatePhysicsStats();
				vessel.orbitDriver.UpdateOrbit(offset: false);
				vessel.UpdatePosVel();
				vessel.UpdateAcceleration(fDeltaTimeRecip, fromUpdate: true);
			}
		}
	}

	public virtual void ApplyVelocityCorrection()
	{
		if (correctDriftThisFrame)
		{
			vessel.OffsetVelocity(postIntegrationVelocityCorrection);
		}
		correctDriftThisFrame = false;
		postIntegrationVelocityCorrection.Zero();
	}

	public virtual void GoOnRails()
	{
		if (vessel.orbitDriver.updateMode == OrbitDriver.UpdateMode.TRACK_Phys && !postIntegrationVelocityCorrection.IsZero())
		{
			ApplyVelocityCorrection();
			CalculatePhysicsStats();
			vessel.orbitDriver.TrackRigidbody(vessel.mainBody, 0.0);
		}
		correctDriftThisFrame = false;
		wasCorrectingDrift = false;
		physStatsNotDoneInUpdate = true;
		postIntegrationVelocityCorrection.Zero();
	}

	public virtual void GoOffRails()
	{
	}

	public virtual void StartEasing()
	{
		vessel.gravityMultiplier = 0.0;
		easing = true;
		GameEvents.onPhysicsEaseStart.Fire(vessel);
		if (vessel == FlightGlobals.ActiveVessel)
		{
			easingLockOn = true;
			InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "PhysicsEase");
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6003101, (float)(1.0 / easingFrameIncrease * 0.02 + 0.5), ScreenMessageStyle.UPPER_RIGHT);
		}
	}

	public virtual void StopEasing()
	{
		easing = false;
		if (vessel != null)
		{
			vessel.gravityMultiplier = 1.0;
		}
		GameEvents.onPhysicsEaseStop.Fire(vessel);
		if (easingLockOn)
		{
			InputLockManager.RemoveControlLock("PhysicsEase");
		}
	}

	public virtual void CalculateGravity()
	{
		vessel.gravityTrue = (gAccelTrue = FlightGlobals.getGeeForceAtPosition(vessel.CoMD, vessel.mainBody));
		if (!vessel.packed)
		{
			if (lastMode != 0)
			{
				vessel.IgnoreGForces(10);
			}
			if ((GameSettings.PHYSICS_EASE && !easing && lastMode == OrbitDriver.UpdateMode.IDLE && vessel.orbitDriver.updateMode == OrbitDriver.UpdateMode.TRACK_Phys) || easeDocking)
			{
				StartEasing();
				easeDocking = false;
			}
			if (easing)
			{
				vessel.gravityMultiplier += easingFrameIncrease;
				if (vessel.gravityMultiplier >= 1.0)
				{
					StopEasing();
				}
			}
			if (GameSettings.ORBIT_DRIFT_COMPENSATION && allowDriftCompensation && vessel.loaded && !vessel.packed && !vessel.LandedOrSplashed && vessel.mainBody == vessel.lastBody && vessel.staticPressurekPa == 0.0 && (!vessel.mainBody.atmosphere || vessel.altitude > vessel.mainBody.atmosphereDepth) && vessel.orbit.radius < PhysicsGlobals.OrbitDriftAltThreshold)
			{
				Vector3d vector3d = vessel.CoMD - vessel.orbit.referenceBody.position;
				vessel.orbit.GetOrbitalStateVectorsAtUT(vessel.orbitDriver.lastTrackUT, out cpos, out cvel);
				curRotFrameVel = vessel.orbit.GetRotFrameVelAtPos(vessel.mainBody, cpos);
				cpos.Swizzle();
				cvel.Swizzle();
				curRotFrameVel.Swizzle();
				if (!driftAlwaysPassThreshChecks && (!((cpos - vector3d).sqrMagnitude < PhysicsGlobals.OrbitDriftSqrThreshold) || (cvel - curRotFrameVel - vessel.velocityD).sqrMagnitude >= PhysicsGlobals.OrbitDriftSqrThreshold))
				{
					framesUntilCorrect = PhysicsGlobals.OrbitDriftFramesToWait;
				}
				else if (framesUntilCorrect > 0)
				{
					framesUntilCorrect--;
				}
				else
				{
					wasCorrectingDrift = true;
					correctDriftThisFrame = true;
					double double_ = vessel.orbitDriver.lastTrackUT + fDeltaTime;
					vessel.orbit.GetOrbitalStateVectorsAtUT(double_, out railsPosNext, out railsVelNext);
					rotFrameNext = vessel.orbit.GetRotFrameVelAtPos(vessel.orbit.referenceBody, railsPosNext);
					railsPosNext.Swizzle();
					railsVelNext.Swizzle();
					rotFrameNext.Swizzle();
					preIntegrationVelocityOffset = (railsPosNext + vessel.orbit.referenceBody.position - vessel.CoMD - vessel.velocityD * fDeltaTime) * fDeltaTimeRecip;
					postIntegrationVelocityCorrection = railsVelNext - vessel.velocityD - rotFrameNext - preIntegrationVelocityOffset;
					preIntegrationVelocityOffset *= PhysicsGlobals.GraviticForceMultiplier * vessel.gravityMultiplier;
					postIntegrationVelocityCorrection *= PhysicsGlobals.GraviticForceMultiplier * vessel.gravityMultiplier;
					integrationAccel.Zero();
					gAccel = (railsVelNext - vessel.velocityD - curRotFrameVel) * fDeltaTimeRecip;
					vessel.gravityForPos = preIntegrationVelocityOffset * fDeltaTimeRecip;
					if (preIntegrationVelocityOffset.IsInvalid() || postIntegrationVelocityCorrection.IsInvalid())
					{
						wasCorrectingDrift = false;
						preIntegrationVelocityOffset.Zero();
						postIntegrationVelocityCorrection.Zero();
					}
				}
			}
			if (!wasCorrectingDrift)
			{
				integrationAccel = gAccelTrue;
				if (vessel.mainBody.inverseRotation)
				{
					coriolis = FlightGlobals.getCoriolisAcc(vessel.velocityD, vessel.mainBody);
					centrifugal = FlightGlobals.getCentrifugalAcc(vessel.CoMD, vessel.mainBody);
					integrationAccel += coriolis + centrifugal;
					integrationAccel *= PhysicsGlobals.GraviticForceMultiplier * vessel.gravityMultiplier;
					vessel.gravityForPos = integrationAccel;
					Vector3d vector3d2 = vessel.velocityD + integrationAccel * fDeltaTime;
					Vector3d refPos = vessel.CoMD + vector3d2 * fDeltaTime - vessel.mainBody.position;
					refPos.Swizzle();
					Vector3d rotFrameVel = vessel.orbit.GetRotFrameVel(vessel.mainBody);
					rotFrameVel.Swizzle();
					Vector3d rotFrameVelAtPos = vessel.orbit.GetRotFrameVelAtPos(vessel.mainBody, refPos);
					rotFrameVelAtPos.Swizzle();
					gAccel = vector3d2 + rotFrameVelAtPos - (vessel.velocityD + rotFrameVel);
					gAccel *= 1.0 / fDeltaTime;
				}
				else
				{
					integrationAccel *= PhysicsGlobals.GraviticForceMultiplier * vessel.gravityMultiplier;
					vessel.gravityForPos = (gAccel = integrationAccel);
				}
			}
		}
		else if (easing)
		{
			StopEasing();
		}
	}

	public virtual void SetLandedPosRot()
	{
		if (vessel.LandedOrSplashed && vessel.packed && railsSetPosRot)
		{
			worldSurfaceRot = vessel.mainBody.bodyTransform.rotation * vessel.srfRelRotation;
			vessel.SetRotation(worldSurfaceRot, setPos: false);
			worldSurfacePos = vessel.mainBody.GetWorldSurfacePosition(vessel.latitude, vessel.longitude, vessel.altitude);
			vessel.SetPosition(worldSurfacePos);
		}
	}

	public virtual void CalculatePhysicsStats()
	{
		bool flag = true;
		if (vessel.loaded)
		{
			Transform referenceTransform = vessel.ReferenceTransform;
			int count = vessel.Parts.Count;
			QuaternionD quaternionD = QuaternionD.Inverse(referenceTransform.rotation);
			Vector3d zero = Vector3d.zero;
			Vector3d zero2 = Vector3d.zero;
			Vector3d zero3 = Vector3d.zero;
			double num = 0.0;
			if (vessel.packed && count > 0)
			{
				Physics.SyncTransforms();
			}
			int index = count;
			while (index-- > 0)
			{
				Part part = vessel.parts[index];
				if (part.rb != null)
				{
					double physicsMass = part.physicsMass;
					zero += (Vector3d)part.rb.worldCenterOfMass * physicsMass;
					Vector3d vector3d = (Vector3d)part.rb.velocity * physicsMass;
					zero2 += vector3d;
					zero3 += quaternionD * part.rb.angularVelocity * physicsMass;
					num += physicsMass;
				}
			}
			if (num > 0.0)
			{
				flag = false;
				vessel.totalMass = num;
				double num2 = 1.0 / num;
				vessel.CoMD = zero * num2;
				vessel.rb_velocityD = zero2 * num2;
				vessel.velocityD = vessel.rb_velocityD + Krakensbane.GetFrameVelocity();
				vessel.CoM = vessel.CoMD;
				vessel.localCoM = vessel.vesselTransform.InverseTransformPoint(vessel.CoM);
				vessel.rb_velocity = vessel.rb_velocityD;
				vessel.angularVelocityD = zero3 * num2;
				vessel.angularVelocity = vessel.angularVelocityD;
				if (!(vessel.angularVelocityD != Vector3d.zero) && vessel.packed)
				{
					vessel.vector3_0.Zero();
					vessel.angularMomentum.Zero();
				}
				else
				{
					Matrix4x4 left = Matrix4x4.zero;
					Matrix4x4 m = Matrix4x4.identity;
					Matrix4x4 m2 = Matrix4x4.identity;
					Matrix4x4 m3 = Matrix4x4.identity;
					Quaternion quaternion = quaternionD;
					for (int i = 0; i < count; i++)
					{
						Part part2 = vessel.parts[i];
						if (part2.rb != null)
						{
							KSPUtil.ToDiagonalMatrix2(part2.rb.inertiaTensor, ref m);
							Quaternion quaternion2 = quaternion * part2.transform.rotation * part2.rb.inertiaTensorRotation;
							Quaternion q = Quaternion.Inverse(quaternion2);
							Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, quaternion2, Vector3.one);
							Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
							Matrix4x4 right = matrix4x * m * matrix4x2;
							KSPUtil.Add(ref left, ref right);
							Vector3 vector = referenceTransform.InverseTransformDirection(part2.rb.position - vessel.CoMD);
							KSPUtil.ToDiagonalMatrix2(part2.rb.mass * vector.sqrMagnitude, ref m2);
							KSPUtil.Add(ref left, ref m2);
							KSPUtil.OuterProduct2(vector, (0f - part2.rb.mass) * vector, ref m3);
							KSPUtil.Add(ref left, ref m3);
						}
					}
					vessel.vector3_0 = KSPUtil.Diag(left);
					vessel.angularMomentum.x = (float)((double)vessel.vector3_0.x * vessel.angularVelocityD.x);
					vessel.angularMomentum.y = (float)((double)vessel.vector3_0.y * vessel.angularVelocityD.y);
					vessel.angularMomentum.z = (float)((double)vessel.vector3_0.z * vessel.angularVelocityD.z);
				}
			}
		}
		if (flag)
		{
			if (vessel.packed)
			{
				if (vessel.LandedOrSplashed)
				{
					vessel.CoMD = worldSurfacePos + worldSurfaceRot * vessel.localCoM;
				}
				else
				{
					if (!vessel.orbitDriver.Ready)
					{
						vessel.orbitDriver.orbit.Init();
						vessel.orbitDriver.updateFromParameters(setPosition: false);
					}
					vessel.CoMD = vessel.mainBody.position + vessel.orbitDriver.pos;
				}
			}
			else
			{
				vessel.CoMD = vessel.vesselTransform.TransformPoint(vessel.localCoM);
			}
			vessel.CoM = vessel.CoMD;
			if (vessel.rootPart != null && vessel.rootPart.rb != null)
			{
				vessel.rb_velocity = vessel.rootPart.rb.GetPointVelocity(vessel.CoM);
				vessel.rb_velocityD = vessel.rb_velocity;
				vessel.velocityD = (Vector3d)vessel.rb_velocity + Krakensbane.GetFrameVelocity();
				vessel.angularVelocityD = (vessel.angularVelocity = Quaternion.Inverse(vessel.ReferenceTransform.rotation) * vessel.rootPart.rb.angularVelocity);
			}
			else
			{
				vessel.rb_velocity.Zero();
				vessel.rb_velocityD.Zero();
				vessel.velocityD.Zero();
				vessel.angularVelocity.Zero();
				vessel.angularVelocityD.Zero();
			}
			vessel.vector3_0.Zero();
			vessel.angularMomentum.Zero();
		}
		firstStatsRunComplete = true;
	}

	public void onDockingComplete(GameEvents.FromToAction<Part, Part> FromTo)
	{
		if (FromTo.from.vessel == vessel || FromTo.to.vessel == vessel)
		{
			easeDocking = true;
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003101 = Localizer.Format("#autoLOC_6003101");
	}
}
