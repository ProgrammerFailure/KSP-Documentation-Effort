using System;
using UnityEngine;

public class PartBuoyancy : MonoBehaviour
{
	public Vector3 centerOfBuoyancy;

	public Vector3 centerOfDisplacement;

	public Vector3d maxCoords;

	public Vector3d minCoords;

	public float maxDimension;

	public Bounds bounds;

	public Vector3[] boundCoords = new Vector3[8];

	public Vector3 boundsCenter;

	public double[] depths = new double[8];

	public bool early;

	public bool allSplashed;

	public bool slow;

	public bool wasSplashed;

	public bool dead;

	public static bool overrideCubeOnChutesIfUnspecified = true;

	public double waterLevel;

	public CelestialBody body;

	public double depth;

	public double maxDepth;

	public double minDepth;

	public double dragScalar;

	public double splashedCounter;

	public double liftScalar;

	public double drag;

	public double submergedPortion;

	public Vector3 effectiveForce;

	public double buoyantGeeForce;

	public DragCube overrideCube;

	public double localVelocityMag;

	public Vector3 lastBuoyantForce;

	public Vector3 lastForcePosition;

	public bool physWarp;

	public double xPortion;

	public double yPortion;

	public double zPortion;

	public double xzPortion;

	public Part part;

	public bool splashed;

	public double displacement;

	public bool canForceUpdate;

	public Vector3 pLast;

	public Vector3 vector3_0;

	public Vector3 downAxis;

	public float dH;

	public bool isKerbal;

	public Rigidbody _rigidbody;

	public void Start()
	{
		part = GetComponent<Part>();
		if (part != null)
		{
			canForceUpdate = true;
			centerOfBuoyancy = part.partTransform.position + part.partTransform.rotation * part.CenterOfBuoyancy;
			centerOfDisplacement = part.partTransform.position + part.partTransform.rotation * part.CenterOfDisplacement;
			pLast = centerOfBuoyancy;
			if (part.GetComponent<KerbalEVA>() != null)
			{
				isKerbal = true;
			}
			if (overrideCubeOnChutesIfUnspecified && string.IsNullOrEmpty(part.buoyancyUseCubeNamed) && part.Modules.Contains("ModuleParachute") && part.DragCubes.Cubes.Find((DragCube c) => c.Name == "PACKED") != null)
			{
				part.buoyancyUseCubeNamed = "PACKED";
			}
			part.submergedDragScalar = (dragScalar = PhysicsGlobals.BuoyancyWaterDragScalar);
			Part obj = part;
			double num = 0.0;
			obj.submergedPortion = 0.0;
			submergedPortion = num;
			liftScalar = 0.0;
			FindOverrideCube();
			UpdateDisplacement();
			lastBuoyantForce.Zero();
			lastForcePosition = centerOfBuoyancy;
		}
		else
		{
			Debug.LogError("[PartBouyancy] Can't be added because it depends of Part component.");
			UnityEngine.Object.Destroy(this);
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (boundCoords != null && boundCoords.Length == 8)
		{
			Gizmos.color = XKCDColors.Red;
			for (int i = 0; i < 8; i++)
			{
				Gizmos.DrawWireSphere(boundCoords[i], 0.5f);
			}
			Gizmos.color = XKCDColors.Cyan;
			Gizmos.DrawWireSphere(boundsCenter, 0.5f);
		}
	}

	public void FindOverrideCube()
	{
		overrideCube = null;
		if (string.IsNullOrEmpty(part.buoyancyUseCubeNamed))
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < part.DragCubes.Cubes.Count)
			{
				if (part.DragCubes.Cubes[num].Name == part.buoyancyUseCubeNamed)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		overrideCube = part.DragCubes.Cubes[num];
	}

	public void UpdateDisplacement()
	{
		displacement = PhysicsGlobals.BuoyancyDefaultVolume;
		maxDimension = 1f;
		if (!part.DragCubes.None)
		{
			if (part.DragCubes.WeightedSize.x == 0f && canForceUpdate && overrideCube == null)
			{
				part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming: true);
				canForceUpdate = false;
				return;
			}
			float[] array;
			Vector3 vector;
			if (overrideCube != null)
			{
				array = overrideCube.Area;
				vector = overrideCube.Size;
			}
			else
			{
				array = part.DragCubes.WeightedArea;
				vector = part.DragCubes.WeightedSize;
			}
			displacement = vector.x * vector.y * vector.z;
			maxDimension = vector.x;
			if (vector.y > maxDimension)
			{
				maxDimension = vector.y;
			}
			if (vector.y > maxDimension)
			{
				maxDimension = vector.z;
			}
			xPortion = array[0] / (vector.y * vector.z);
			yPortion = array[2] / (vector.x * vector.z);
			zPortion = array[4] / (vector.x * vector.y);
			double num = xPortion + yPortion + zPortion;
			if (!double.IsNaN(num) && num > 0.0)
			{
				xzPortion = (Math.Min(xPortion, zPortion) + 2.0 * (xPortion * zPortion)) * (1.0 / 3.0);
				displacement *= xzPortion * yPortion;
			}
		}
		else if ((bool)part.collider)
		{
			Vector3 size = part.collider.bounds.size;
			displacement = (double)(size.x * size.y * size.z) * 0.75;
			maxDimension = size.x;
			if (size.y > maxDimension)
			{
				maxDimension = size.y;
			}
			if (size.y > maxDimension)
			{
				maxDimension = size.z;
			}
		}
	}

	public void FixedUpdate()
	{
		dead = false;
		if (!FlightGlobals.ready || part.vessel == null || part.vessel.mainBody == null || !(body = part.vessel.mainBody).ocean || part.rb == null)
		{
			return;
		}
		bool shieldedFromAirstream = part.ShieldedFromAirstream;
		double num = part.vessel.waterOffset + waterLevel;
		float fixedDeltaTime = TimeWarp.fixedDeltaTime;
		centerOfBuoyancy = part.partTransform.position + part.partTransform.rotation * part.CenterOfBuoyancy;
		centerOfDisplacement = part.partTransform.position + part.partTransform.rotation * part.CenterOfDisplacement;
		depth = 0.0 - FlightGlobals.getAltitudeAtPos((Vector3d)centerOfDisplacement, body) + num;
		if (!shieldedFromAirstream && !part.DragCubes.None && depth > 0.0 - PhysicsGlobals.BuoyancyRange && part.vessel.terrainAltitude < 20.0 && !isKerbal)
		{
			minDepth = double.MaxValue;
			maxDepth = double.MinValue;
			FindOverrideCube();
			if (overrideCube != null)
			{
				bounds = new Bounds(overrideCube.Center, overrideCube.Size);
				boundsCenter = overrideCube.Center;
			}
			else
			{
				bounds = new Bounds(part.DragCubes.WeightedCenter, part.DragCubes.WeightedSize);
				boundsCenter = part.DragCubes.WeightedCenter;
			}
			Vector3d vector3d = bounds.min;
			Vector3d vector3d2 = bounds.max;
			Matrix4x4 localToWorldMatrix = part.partTransform.localToWorldMatrix;
			boundsCenter = localToWorldMatrix.MultiplyPoint3x4(boundsCenter);
			boundCoords[0] = new Vector3d(vector3d.x, vector3d.y, vector3d.z);
			boundCoords[0] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[0]);
			depths[0] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[0], body)) + num;
			if (depths[0] > maxDepth)
			{
				maxDepth = depths[0];
				maxCoords = boundCoords[0];
			}
			if (depths[0] < minDepth)
			{
				minDepth = depths[0];
				minCoords = boundCoords[0];
			}
			boundCoords[1] = new Vector3d(vector3d.x, vector3d.y, vector3d2.z);
			boundCoords[1] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[1]);
			depths[1] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[1], body)) + num;
			if (depths[1] > maxDepth)
			{
				maxDepth = depths[1];
				maxCoords = boundCoords[1];
			}
			if (depths[1] < minDepth)
			{
				minDepth = depths[1];
				minCoords = boundCoords[1];
			}
			boundCoords[2] = new Vector3d(vector3d.x, vector3d2.y, vector3d.z);
			boundCoords[2] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[2]);
			depths[2] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[2], body)) + num;
			if (depths[2] > maxDepth)
			{
				maxDepth = depths[2];
				maxCoords = boundCoords[2];
			}
			if (depths[2] < minDepth)
			{
				minDepth = depths[2];
				minCoords = boundCoords[2];
			}
			boundCoords[3] = new Vector3d(vector3d.x, vector3d2.y, vector3d2.z);
			boundCoords[3] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[3]);
			depths[3] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[3], body)) + num;
			if (depths[3] > maxDepth)
			{
				maxDepth = depths[3];
				maxCoords = boundCoords[3];
			}
			if (depths[3] < minDepth)
			{
				minDepth = depths[3];
				minCoords = boundCoords[3];
			}
			boundCoords[4] = new Vector3d(vector3d2.x, vector3d.y, vector3d.z);
			boundCoords[4] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[4]);
			depths[4] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[4], body)) + num;
			if (depths[4] > maxDepth)
			{
				maxDepth = depths[4];
				maxCoords = boundCoords[4];
			}
			if (depths[4] < minDepth)
			{
				minDepth = depths[4];
				minCoords = boundCoords[4];
			}
			boundCoords[5] = new Vector3d(vector3d2.x, vector3d.y, vector3d2.z);
			boundCoords[5] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[5]);
			depths[5] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[5], body)) + num;
			if (depths[5] > maxDepth)
			{
				maxDepth = depths[5];
				maxCoords = boundCoords[5];
			}
			if (depths[5] < minDepth)
			{
				minDepth = depths[5];
				minCoords = boundCoords[5];
			}
			boundCoords[6] = new Vector3d(vector3d2.x, vector3d2.y, vector3d.z);
			boundCoords[6] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[6]);
			depths[6] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[6], body)) + num;
			if (depths[6] > maxDepth)
			{
				maxDepth = depths[6];
				maxCoords = boundCoords[6];
			}
			if (depths[6] < minDepth)
			{
				minDepth = depths[6];
				minCoords = boundCoords[6];
			}
			boundCoords[7] = new Vector3d(vector3d2.x, vector3d2.y, vector3d2.z);
			boundCoords[7] = localToWorldMatrix.MultiplyPoint3x4(boundCoords[7]);
			depths[7] = (double)(0f - FlightGlobals.getAltitudeAtPos(boundCoords[7], body)) + num;
			if (depths[7] > maxDepth)
			{
				maxDepth = depths[7];
				maxCoords = boundCoords[7];
			}
			if (depths[7] < minDepth)
			{
				minDepth = depths[7];
				minCoords = boundCoords[7];
			}
		}
		else
		{
			minDepth = (maxDepth = depth);
			minCoords = centerOfBuoyancy;
			maxCoords = centerOfBuoyancy;
		}
		part.depth = depth;
		part.minDepth = minDepth;
		part.maxDepth = maxDepth;
		localVelocityMag = (double)part.rb.velocity.magnitude + Krakensbane.GetFrameVelocity().magnitude;
		downAxis = -FlightGlobals.getUpAxis(body, centerOfBuoyancy);
		float num2 = ((localVelocityMag == 0.0) ? 1f : Vector3.Dot((part.rb.velocity + Krakensbane.GetFrameVelocity()) / (float)localVelocityMag, downAxis));
		double num3 = (VectorExtensions.IsZero(part.vessel.srf_vel_direction) ? 1.0 : ((double)Math.Max(Math.Abs(num2), Math.Abs(Vector3.Dot(part.vessel.srf_vel_direction, downAxis)))));
		double num4 = PhysicsGlobals.BuoyancyWaterDragScalarLerpDotMultBase - num3 * PhysicsGlobals.BuoyancyWaterDragScalarLerpDotMult;
		early = splashedCounter < PhysicsGlobals.BuoyancyWaterDragTimer;
		allSplashed = splashed || part.vessel.Splashed;
		slow = localVelocityMag < PhysicsGlobals.BuoyancyWaterDragMinVel * PhysicsGlobals.BuoyancyWaterDragMinVelMult;
		double num5 = PhysicsGlobals.BuoyancyWaterDragScalar;
		if (early)
		{
			num5 *= 0.5 + num3;
		}
		if (splashed && !slow)
		{
			if (!early)
			{
				if (dragScalar > PhysicsGlobals.BuoyancyWaterDragScalarEnd)
				{
					dragScalar += (PhysicsGlobals.BuoyancyWaterDragScalarEnd - dragScalar) * PhysicsGlobals.BuoyancyWaterDragScalarLerp * num4 * (double)fixedDeltaTime;
					if (dragScalar < PhysicsGlobals.BuoyancyWaterDragScalarEnd)
					{
						dragScalar = PhysicsGlobals.BuoyancyWaterDragScalarEnd;
					}
				}
				else
				{
					dragScalar = PhysicsGlobals.BuoyancyWaterDragScalarEnd;
				}
				if (liftScalar < PhysicsGlobals.BuoyancyWaterLiftScalarEnd)
				{
					if (early)
					{
						liftScalar = 0.0;
					}
					else
					{
						liftScalar += (PhysicsGlobals.BuoyancyWaterLiftScalarEnd - liftScalar) * PhysicsGlobals.BuoyancyWaterDragScalarLerp * (double)fixedDeltaTime;
						if (liftScalar > PhysicsGlobals.BuoyancyWaterLiftScalarEnd)
						{
							liftScalar = PhysicsGlobals.BuoyancyWaterLiftScalarEnd;
						}
					}
				}
				else
				{
					liftScalar = PhysicsGlobals.BuoyancyWaterLiftScalarEnd;
				}
			}
		}
		else
		{
			if (dragScalar < num5)
			{
				dragScalar += (num5 - dragScalar) * PhysicsGlobals.BuoyancyWaterDragScalarLerp * (double)fixedDeltaTime;
				if (dragScalar > num5)
				{
					dragScalar = num5;
				}
			}
			else
			{
				dragScalar = num5;
			}
			if (liftScalar > 0.0)
			{
				liftScalar -= liftScalar * PhysicsGlobals.BuoyancyWaterDragScalarLerp * (double)fixedDeltaTime;
				if (liftScalar < 0.0)
				{
					liftScalar = 0.0;
				}
			}
		}
		if (splashed)
		{
			if (early)
			{
				splashedCounter += fixedDeltaTime;
			}
		}
		else
		{
			splashedCounter = 0.0;
		}
		if (!shieldedFromAirstream && maxDepth > 0.0 && part.State != PartStates.DEAD)
		{
			UpdateDisplacement();
			if (!splashed)
			{
				pLast = centerOfBuoyancy - Krakensbane.GetFrameVelocityV3f() * fixedDeltaTime;
				vector3_0 = centerOfBuoyancy - pLast;
				dH = Vector3.Dot(vector3_0, downAxis);
				float num6 = num2 * (float)localVelocityMag;
				if (localVelocityMag > 2.0 && Vector3.Distance(part.partTransform.position, FlightGlobals.camera_position) < 5000f)
				{
					double num7 = 0.0;
					if ((bool)body.pqsController && body.pqsController.ChildSpheres.Length != 0)
					{
						num7 = GameSettings.WATERLEVEL_BASE_OFFSET - (double)body.pqsController.ChildSpheres[0].maxLevel * GameSettings.WATERLEVEL_MAXLEVEL_MULT;
					}
					FXMonger.Splash(centerOfBuoyancy + (Vector3)((Vector3d)downAxis * (maxDepth - depth - (double)dH + num7)), num6 * 0.1f * maxDimension);
				}
				splashed = true;
				if (Mathf.Max(PhysicsGlobals.BuoyancyMinCrashMult * (float)localVelocityMag, num6) > part.crashTolerance * PhysicsGlobals.BuoyancyCrashToleranceMult)
				{
					GameEvents.onCrashSplashdown.Fire(new EventReport(FlightEvents.SPLASHDOWN_CRASH, part, part.partInfo.title));
					MonoBehaviour.print("[PartBuoyancy]: Part " + part.partInfo.title + " splashed at " + localVelocityMag + " m/s total, " + num6 + " down, effective " + Mathf.Max(PhysicsGlobals.BuoyancyMinCrashMult * (float)localVelocityMag, num6) + ", with tolerance " + part.crashTolerance * PhysicsGlobals.BuoyancyCrashToleranceMult);
					dead = true;
				}
				else
				{
					if (IsInvoking())
					{
						CancelInvoke();
					}
					Invoke("ReportSplashDown", 2f);
				}
			}
			part.WaterContact = true;
			part.vessel.Splashed = true;
			submergedPortion = 1.0;
			Vector3 vector = centerOfBuoyancy;
			if (minDepth < 0.0 && minDepth < maxDepth)
			{
				if (depth <= 0.0 && depth != maxDepth)
				{
					submergedPortion = maxDepth / (maxDepth - depth) * 0.5;
				}
				else
				{
					submergedPortion = 0.5 + depth / (depth - minDepth) * 0.5;
				}
				if (part.buoyancyUseSine)
				{
					submergedPortion *= Math.PI;
					submergedPortion -= Math.PI / 2.0;
					submergedPortion = Math.Sin(submergedPortion) * 0.5 + 0.5;
				}
				if (PhysicsGlobals.BuoyancyUseCoBOffset)
				{
					Vector3 zero = Vector3.zero;
					float num8 = 0f;
					for (int num9 = 7; num9 >= 0; num9--)
					{
						float num10 = (float)depths[num9];
						if (num10 > 0f)
						{
							zero += boundCoords[num9] * num10;
							num8 += num10;
						}
					}
					zero /= num8;
					vector = centerOfBuoyancy + (zero + part.partTransform.rotation * part.CenterOfBuoyancy - centerOfBuoyancy) * (float)(1.0 - submergedPortion);
				}
			}
			part.submergedPortion = submergedPortion;
			buoyantGeeForce = displacement * submergedPortion * body.oceanDensity * ((maxDepth < PhysicsGlobals.BuoyancyScaleAboveDepth) ? (maxDepth / PhysicsGlobals.BuoyancyScaleAboveDepth) : 1.0) * PhysicsGlobals.BuoyancyScalar * (double)part.buoyancy;
			effectiveForce = -FlightGlobals.getGeeForceAtPosition(vector) * buoyantGeeForce * part.vessel.gravityMultiplier * PhysicsGlobals.GraviticForceMultiplier;
			if (early && !wasSplashed)
			{
				dragScalar = Math.Min(dragScalar, (PhysicsGlobals.BuoyancyWaterDragMultMinForMinDot + num3 * (1.0 - PhysicsGlobals.BuoyancyWaterDragMultMinForMinDot)) * (num5 - PhysicsGlobals.BuoyancyWaterDragScalarEnd) + PhysicsGlobals.BuoyancyWaterDragScalarEnd);
			}
			if (localVelocityMag < PhysicsGlobals.BuoyancyWaterDragMinVel * PhysicsGlobals.BuoyancyWaterDragMinVelMultCOBOff)
			{
				vector = Vector3.Lerp(lastForcePosition + vector3_0, vector, PhysicsGlobals.BuoyancyForceOffsetLerp);
			}
			if (!(localVelocityMag < PhysicsGlobals.BuoyancyWaterDragMinVel) && localVelocityMag <= part.vessel.srfSpeed * PhysicsGlobals.BuoyancyWaterDragPartVelGreaterVesselMult)
			{
				if (!isKerbal)
				{
					part.angularDragByFI = true;
					part.rb.angularDrag = 0f;
					double num11 = (part.DragCubes.None ? 1.0 : ((double)part.DragCubes.AreaDrag));
					double num12 = 0.5 * body.oceanDensity * localVelocityMag * num11 * (double)PhysicsGlobals.DragCubeMultiplier * dragScalar * submergedPortion * (double)PhysicsGlobals.DragMultiplier;
					part.rb.drag = (float)(num12 / (double)((part.rb.mass != 0f) ? part.rb.mass : 0.1f));
					if (part.servoRb != null)
					{
						part.servoRb.angularDrag = 0f;
						part.servoRb.drag = (float)(num12 / (double)((part.servoRb.mass != 0f) ? part.servoRb.mass : 0.1f));
					}
				}
				else
				{
					part.rb.drag = PhysicsGlobals.BuoyancyWaterDragSlow;
				}
			}
			else
			{
				if (!isKerbal)
				{
					double num13 = part.vessel.ctrlState.pitch;
					double num14 = part.vessel.ctrlState.yaw;
					double num15 = part.vessel.ctrlState.roll;
					if (num13 * num13 + num14 * num14 + num15 * num15 < PhysicsGlobals.BuoyancyAngularDragMinControlSqrMag)
					{
						part.angularDragByFI = false;
						part.rb.angularDrag = PhysicsGlobals.BuoyancyWaterAngularDragSlow;
						if (part.servoRb != null)
						{
							part.servoRb.angularDrag = PhysicsGlobals.BuoyancyWaterAngularDragSlow;
						}
					}
					else
					{
						part.angularDragByFI = true;
						part.rb.angularDrag = 0f;
						if (part.servoRb != null)
						{
							part.servoRb.angularDrag = 0f;
						}
					}
				}
				part.rb.drag = PhysicsGlobals.BuoyancyWaterDragSlow;
				if (part.servoRb != null)
				{
					part.servoRb.drag = PhysicsGlobals.BuoyancyWaterDragSlow;
				}
			}
			physWarp = TimeWarp.CurrentRate > 1f && TimeWarp.WarpMode == TimeWarp.Modes.const_1;
			if (isKerbal && physWarp)
			{
				part.rb.drag += fixedDeltaTime * 6.25f;
				if (TimeWarp.CurrentRate > 2f)
				{
					effectiveForce = Vector3.Lerp(lastBuoyantForce, effectiveForce, 1.25f - TimeWarp.CurrentRate * 0.25f);
				}
			}
			if (early)
			{
				if (physWarp && !isKerbal)
				{
					part.rb.drag += (float)((dragScalar - PhysicsGlobals.BuoyancyWaterDragScalarEnd) * (double)fixedDeltaTime * 50.0);
					if (part.servoRb != null)
					{
						part.servoRb.drag += (float)((dragScalar - PhysicsGlobals.BuoyancyWaterDragScalarEnd) * (double)fixedDeltaTime * 50.0);
					}
					if (TimeWarp.CurrentRate > 2f)
					{
						effectiveForce = Vector3.Lerp(lastBuoyantForce, effectiveForce, 1.25f - TimeWarp.CurrentRate * 0.25f);
					}
				}
				else if (num3 > PhysicsGlobals.BuoyancyWaterDragExtraRBDragAboveDot)
				{
					part.rb.drag += (float)(num3 - PhysicsGlobals.BuoyancyWaterDragExtraRBDragAboveDot);
					if (part.servoRb != null)
					{
						part.servoRb.drag += (float)(num3 - PhysicsGlobals.BuoyancyWaterDragExtraRBDragAboveDot);
					}
				}
			}
			if (effectiveForce.IsInvalid())
			{
				effectiveForce.Zero();
			}
			lastBuoyantForce = effectiveForce;
			lastForcePosition = vector;
			if (dead)
			{
				if (PhysicsGlobals.BuoyancyApplyForceOnDie && part.parent != null)
				{
					part.parent.AddForceAtPosition(effectiveForce, vector);
				}
				part.Die();
			}
			else
			{
				part.AddForceAtPosition(effectiveForce, vector);
			}
		}
		else
		{
			if (IsInvoking())
			{
				CancelInvoke();
			}
			part.WaterContact = false;
			if (splashed)
			{
				part.vessel.checkSplashed();
				splashed = false;
				this.GetComponentCached(ref _rigidbody).drag = 0f;
			}
			Part obj = part;
			double num16 = 0.0;
			submergedPortion = 0.0;
			obj.submergedPortion = num16;
			drag = 0.0;
			lastBuoyantForce.Zero();
			lastForcePosition = centerOfBuoyancy;
		}
		part.submergedDragScalar = dragScalar;
		part.submergedLiftScalar = liftScalar;
		wasSplashed = splashed;
	}

	public void ReportSplashDown()
	{
		part.OnSplashDown();
	}
}
