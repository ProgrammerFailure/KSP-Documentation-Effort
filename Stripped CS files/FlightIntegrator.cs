using System;
using System.Collections.Generic;
using UnityEngine;

public class FlightIntegrator : VesselModule
{
	public static FlightIntegrator ActiveVesselFI = null;

	public bool isRunning;

	public bool isKerbal;

	public float dragChangeSmoothness = 10f;

	public Transform integratorTransform;

	public Part partRef;

	public CelestialBody currentMainBody;

	public double altitude;

	public Vector3 CoM;

	public Vector3 Vel;

	public Vector3 nVel;

	public Vector3 VelUnsmoothed;

	public Vector3d CoMd;

	public Vector3d Veld;

	public Vector3d rbVeld;

	public Vector3[] VelSmoother;

	public int VelIndex;

	public static int VelSmoothLen = 10;

	public double VelSmoothLenRecip;

	public bool lastVelProvisional;

	public bool VelSpiking;

	public Vector3 lastVel;

	public int lastVelIndex;

	public double maxVelDeltaSqr = 1000000.0;

	public double spd;

	public double density;

	public double mach;

	public double staticPressurekPa;

	public double staticPressureAtm;

	public double dynamicPressurekPa;

	public double atmosphericTemperature;

	public double externalTemperature;

	public const double KPA2ATM = 0.009869232667160128;

	public Vector3 GraviticAcceleration;

	public double timeSinceLastUpdate;

	public bool firstFrame = true;

	public bool needOcclusion = true;

	public static CelestialBody sunBody;

	public double realDistanceToSun;

	public double solarFlux;

	public double solarFluxMultiplier;

	public double solarAirMass;

	public Vector3 sunVector;

	public double sunDot;

	public double convectiveCoefficient;

	public double convectiveMachLerp;

	public double convectiveMachFlux;

	public double backgroundRadiationTemp;

	public double backgroundRadiationTempExposed;

	public double atmosphereTemperatureOffset;

	public double bodyAlbedoFlux;

	public double bodyEmissiveFlux;

	public double densityThermalLerp;

	public double pseudoReynolds;

	public double pseudoReLerpTimeMult = 1.0;

	public double pseudoReDragMult = 1.0;

	public int partCount;

	public bool setupRun;

	public RaycastHit sunBodyFluxHit;

	public float cacheDragCubeMultiplier;

	public bool cacheDragCubesUseSpherical;

	public bool cacheApplyDrag;

	public bool cacheApplyDragToNonPhysicsParts;

	public bool cacheDragUsesAcceleration;

	public float cacheDragMultiplier;

	public double cacheBuoyancyWaterAngularDragScalar;

	public float cacheAngularDragMultiplier;

	public float cacheBodyLiftMultiplier;

	public double cacheRadiationFactor;

	public double cacheSpaceTemperature;

	public double cacheFullConvectionAreaMin;

	public double cacheFullToCrossSectionLerpStart;

	public double cacheFullToCrossSectionLerpEnd;

	public double cacheStefanBoltzmanConstant;

	public double cacheSkinSkinConductionFactor;

	public double cacheConductionFactor;

	public double cacheTurbulentConvectionStart;

	public double cacheTurbulentConvectionEnd;

	public double cacheTurbulentConvectionMult;

	public double cacheStandardSpecificHeatCapacity;

	public static int sunLayerMask;

	public bool recreateThermalGraph = true;

	public List<PartThermalData> partThermalDataList = new List<PartThermalData>();

	public List<PartThermalData> partThermalDataListSkin = new List<PartThermalData>();

	public int partThermalDataCount;

	public List<CompoundPart> compoundParts;

	public int compoundPartsTimer;

	public List<OcclusionData> occlusionConv = new List<OcclusionData>();

	public List<OcclusionData> occlusionSun = new List<OcclusionData>();

	public List<OcclusionData> occlusionBody = new List<OcclusionData>();

	public List<IAnalyticOverheatModule> overheatModules = new List<IAnalyticOverheatModule>();

	public List<IAnalyticPreview> previewModules = new List<IAnalyticPreview>();

	public OcclusionCone[] occludersConvection;

	public int occludersConvectionCount;

	public OcclusionCylinder[] occludersSun;

	public int occludersSunCount;

	public OcclusionCylinder[] occludersBody;

	public int occludersBodyCount;

	public bool isAnalytical;

	public double fDeltaTime;

	public double fDeltaTimeRecip;

	public double deltaTime;

	public double fTimeSinceThermo;

	public double fTimeSinceThermoRecip;

	public double timeFactor = 0.02;

	public double passesRecip = 1.0;

	public int passes;

	public bool wasMachConvectionEnabled;

	public int occlusionCounter;

	public override int GetOrder()
	{
		return 0;
	}

	public override bool ShouldBeActive()
	{
		return vessel.loaded;
	}

	public virtual void Setup()
	{
		integratorTransform = base.transform;
		vessel = GetComponent<Vessel>();
		partRef = GetComponent<Part>();
		staticPressureAtm = 1.0;
		staticPressurekPa = 101.325;
		dynamicPressurekPa = 0.0;
		altitude = 0.0;
		currentMainBody = null;
		atmosphericTemperature = 0.0;
		externalTemperature = 0.0;
		if ((bool)Planetarium.fetch && (bool)Planetarium.fetch.Sun)
		{
			sunBody = Planetarium.fetch.Sun;
		}
		else
		{
			sunBody = FlightGlobals.Bodies[0];
		}
		if (vessel == null || partRef == null || !partRef.started)
		{
			return;
		}
		setupRun = true;
		VelSmoothLenRecip = 1.0 / (double)VelSmoothLen;
		VelSmoother = new Vector3[VelSmoothLen];
		int velSmoothLen = VelSmoothLen;
		while (velSmoothLen-- > 0)
		{
			VelSmoother[velSmoothLen] = Vector3.zero;
		}
		isKerbal = partRef.Modules.Contains("KerbalEVA");
		partCount = 0;
		if ((bool)vessel)
		{
			partCount = vessel.parts.Count;
			int index = partCount;
			while (index-- > 0)
			{
				Part part = vessel.parts[index];
				part.resourceMass = part.GetResourceMass(out part.resourceThermalMass);
				if (part.Modules.Contains("ModuleAsteroid"))
				{
					if ((part.Modules["ModuleAsteroid"] as ModuleAsteroid).forceProceduralDrag)
					{
						part.DragCubes.Procedural = true;
					}
					part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming: true);
					part.DragCubes.SetDragWeights();
				}
				if (part.Modules.Contains("ModuleComet"))
				{
					if ((part.Modules["ModuleComet"] as ModuleComet).forceProceduralDrag)
					{
						part.DragCubes.Procedural = true;
					}
					part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming: true);
					part.DragCubes.SetDragWeights();
				}
				if (part.DragCubes.None)
				{
					continue;
				}
				if (part == vessel.rootPart)
				{
					bool flag = true;
					int num = 5;
					while (num >= 0)
					{
						if (part.DragCubes.WeightedArea[num] <= 0f)
						{
							num--;
							continue;
						}
						flag = false;
						break;
					}
					if (flag && !part.DragCubes.Procedural)
					{
						if (part.Modules.Contains("KerbalEVA"))
						{
							part.DragCubes.LoadCubes(PhysicsGlobals.KerbalEVADragCube);
						}
						else if (part.partInfo != null && part.partInfo.partPrefab != null)
						{
							Part partPrefab = part.partInfo.partPrefab;
							if (!partPrefab.DragCubes.None)
							{
								while (part.DragCubes.Cubes.Count < partPrefab.DragCubes.Cubes.Count)
								{
									part.DragCubes.Cubes.Add(new DragCube());
								}
								for (int i = 0; i < partPrefab.DragCubes.Cubes.Count; i++)
								{
									part.DragCubes.Cubes[i].Load(partPrefab.DragCubes.Cubes[i].SaveToString().Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
								}
								part.DragCubes.ForceUpdate(weights: true, occlusion: true);
								part.DragCubes.SetDragWeights();
								part.DragCubes.SetPartOcclusion();
								int count = part.children.Count;
								while (count-- > 0)
								{
									if (!part.children[count].DragCubes.None)
									{
										part.children[count].DragCubes.SetPartOcclusion();
									}
								}
								Debug.Log("[FlightIntegrator]: Reloaded drag cube for zeroed cube root part " + part.name + " on vessel " + vessel.GetDisplayName());
							}
							flag = true;
							int num2 = 5;
							while (num2 >= 0)
							{
								if (part.DragCubes.WeightedArea[num2] <= 0f)
								{
									num2--;
									continue;
								}
								flag = false;
								break;
							}
							if (flag && vessel.vesselType != VesselType.Flag && vessel.vesselType != VesselType.DroppedPart)
							{
								Debug.LogError("[FlightIntegrator]: Root part's drag cube is still zeroed!");
							}
						}
					}
				}
				part.radiativeArea = part.DragCubes.PostOcclusionArea;
			}
			cacheStandardSpecificHeatCapacity = PhysicsGlobals.StandardSpecificHeatCapacity;
			UpdateMassStats();
		}
		firstFrame = true;
		needOcclusion = true;
		recreateThermalGraph = true;
	}

	public override void OnStart()
	{
		sunLayerMask = 1 << LayerMask.NameToLayer("Scaled Scenery");
		HookVesselEvents();
		Setup();
	}

	public override void OnLoadVessel()
	{
		partRef = GetComponent<Part>();
		if (!setupRun && partRef != null && partRef.started)
		{
			Setup();
		}
	}

	public override void OnUnloadVessel()
	{
		setupRun = false;
	}

	public virtual void OnDestroy()
	{
		UnhookVesselEvents();
	}

	public virtual void HookVesselEvents()
	{
		GameEvents.onPartAttach.Add(OnPartEventTargetAction);
		GameEvents.onPartRemove.Add(OnPartEventTargetAction);
		GameEvents.onPartCouple.Add(OnPartEventFromToAction);
		GameEvents.onPartDie.Add(OnPartEvent);
		GameEvents.onPartUndock.Add(OnPartEvent);
		GameEvents.onVesselWasModified.Add(OnVesselEvent);
	}

	public virtual void UnhookVesselEvents()
	{
		GameEvents.onPartAttach.Remove(OnPartEventTargetAction);
		GameEvents.onPartRemove.Remove(OnPartEventTargetAction);
		GameEvents.onPartCouple.Remove(OnPartEventFromToAction);
		GameEvents.onPartDie.Remove(OnPartEvent);
		GameEvents.onPartUndock.Remove(OnPartEvent);
		GameEvents.onVesselWasModified.Remove(OnVesselEvent);
	}

	public void OnPartEvent(Part part)
	{
		if (part.vessel == vessel)
		{
			recreateThermalGraph = true;
		}
	}

	public void OnVesselEvent(Vessel vessel)
	{
		if (base.vessel == vessel)
		{
			recreateThermalGraph = true;
		}
	}

	public void OnPartEventTargetAction(GameEvents.HostTargetAction<Part, Part> data)
	{
		if (((object)data.host != null && data.host.vessel == vessel) || ((object)data.host != null && data.target.vessel == vessel))
		{
			recreateThermalGraph = true;
		}
	}

	public void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> data)
	{
		if (((object)data.from != null && data.from.vessel == vessel) || ((object)data.to != null && data.to.vessel == vessel))
		{
			recreateThermalGraph = true;
		}
	}

	public virtual void UpdateMassStats()
	{
		int index = partCount;
		while (index-- > 0)
		{
			Part part = vessel.parts[index];
			part.resourceMass = part.GetResourceMass(out part.resourceThermalMass);
			part.thermalMass = (double)part.mass * cacheStandardSpecificHeatCapacity * part.thermalMassModifier + part.resourceThermalMass;
			SetSkinThermalMass(part);
			part.thermalMass = Math.Max(part.thermalMass - part.skinThermalMass, 0.1);
			part.thermalMassReciprocal = 1.0 / part.thermalMass;
		}
		int index2 = partCount;
		while (index2-- > 0)
		{
			Part part = vessel.parts[index2];
			if (part.rb != null)
			{
				float num = part.mass + part.resourceMass + GetPhysicslessChildMass(part);
				num = Mathf.Clamp(num, part.partInfo.MinimumMass, Mathf.Abs(num));
				part.physicsMass = num;
				float num2 = Mathf.Max(part.partInfo.MinimumRBMass, num);
				if (!part.packed)
				{
					if (part.servoRb != null)
					{
						part.rb.mass = num2 / 2f;
						part.servoRb.mass = num2 / 2f;
					}
					else
					{
						part.rb.mass = num2;
					}
					part.rb.centerOfMass = part.CoMOffset;
				}
			}
			else
			{
				part.physicsMass = 0.0;
			}
		}
	}

	public virtual void SmoothVelocity()
	{
		VelUnsmoothed = Vel;
		int num = VelIndex - 1;
		if (num < 0)
		{
			num = VelSmoothLen - 1;
		}
		Vector3 vector = VelSmoother[num];
		if ((double)(Vel - vector).sqrMagnitude > maxVelDeltaSqr * fDeltaTime * fDeltaTime && Vel.sqrMagnitude > vector.sqrMagnitude)
		{
			if (VelSpiking)
			{
				VelSmoother[VelIndex++] = Vel;
			}
			else if (lastVelProvisional)
			{
				VelSmoother[VelIndex++] = Vel;
				VelSmoother[lastVelIndex] = lastVel;
				VelSpiking = true;
			}
			else
			{
				lastVelProvisional = true;
				lastVel = Vel;
				lastVelIndex = VelIndex;
				VelSmoother[VelIndex++] = vector;
			}
		}
		else
		{
			VelSmoother[VelIndex++] = Vel;
			VelSpiking = false;
			lastVelProvisional = false;
		}
		if (VelIndex >= VelSmoothLen)
		{
			VelIndex = 0;
		}
		Vector3d zero = Vector3d.zero;
		int velSmoothLen = VelSmoothLen;
		while (velSmoothLen-- > 0)
		{
			zero += VelSmoother[velSmoothLen];
		}
		zero *= VelSmoothLenRecip;
		Vel = zero;
	}

	public virtual void FixedUpdate()
	{
		if (!setupRun)
		{
			Setup();
			if (!setupRun)
			{
				return;
			}
		}
		fDeltaTime = TimeWarp.fixedDeltaTime;
		fDeltaTimeRecip = 1.0 / fDeltaTime;
		if (vessel == FlightGlobals.ActiveVessel)
		{
			ActiveVesselFI = this;
		}
		isRunning = false;
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (partRef == null)
		{
			partRef = GetComponent<Part>();
		}
		if (partRef == null || !partRef.rb)
		{
			return;
		}
		cacheDragCubeMultiplier = PhysicsGlobals.DragCubeMultiplier;
		cacheDragCubesUseSpherical = PhysicsGlobals.DragCubesUseSpherical;
		cacheApplyDrag = PhysicsGlobals.ApplyDrag;
		cacheApplyDragToNonPhysicsParts = PhysicsGlobals.ApplyDragToNonPhysicsParts;
		cacheDragUsesAcceleration = PhysicsGlobals.DragUsesAcceleration;
		cacheDragMultiplier = PhysicsGlobals.DragMultiplier;
		cacheBuoyancyWaterAngularDragScalar = PhysicsGlobals.BuoyancyWaterAngularDragScalar;
		cacheAngularDragMultiplier = PhysicsGlobals.AngularDragMultiplier;
		cacheBodyLiftMultiplier = PhysicsGlobals.BodyLiftMultiplier;
		cacheRadiationFactor = PhysicsGlobals.RadiationFactor;
		cacheSpaceTemperature = PhysicsGlobals.SpaceTemperature;
		cacheFullConvectionAreaMin = PhysicsGlobals.FullConvectionAreaMin;
		cacheFullToCrossSectionLerpStart = PhysicsGlobals.FullToCrossSectionLerpStart;
		cacheFullToCrossSectionLerpEnd = PhysicsGlobals.FullToCrossSectionLerpEnd;
		cacheStefanBoltzmanConstant = PhysicsGlobals.StefanBoltzmanConstant;
		cacheSkinSkinConductionFactor = PhysicsGlobals.SkinSkinConductionFactor;
		cacheConductionFactor = PhysicsGlobals.ConductionFactor;
		cacheTurbulentConvectionStart = PhysicsGlobals.TurbulentConvectionStart;
		cacheTurbulentConvectionEnd = PhysicsGlobals.TurbulentConvectionEnd;
		cacheTurbulentConvectionMult = PhysicsGlobals.TurbulentConvectionMult;
		cacheStandardSpecificHeatCapacity = PhysicsGlobals.StandardSpecificHeatCapacity;
		isRunning = true;
		int count = vessel.parts.Count;
		if (partCount != count)
		{
			partCount = count;
			recreateThermalGraph = true;
		}
		rbVeld = vessel.rb_velocityD;
		Veld = vessel.velocityD;
		Vel = Veld;
		CoMd = vessel.CoMD;
		CoM = vessel.CoM;
		UpdateMassStats();
		SmoothVelocity();
		if (vessel.IgnoreSpeedActive)
		{
			spd = 0.0;
		}
		else
		{
			spd = Vel.magnitude;
		}
		nVel = ((spd != 0.0) ? (Vel / (float)spd) : Vector3.zero);
		currentMainBody = vessel.mainBody;
		GraviticAcceleration = vessel.graviticAcceleration;
		altitude = FlightGlobals.getAltitudeAtPos(CoMd, currentMainBody);
		CalculatePressure();
		vessel.speed = spd;
		vessel.staticPressurekPa = staticPressurekPa;
		vessel.indicatedAirSpeed = spd * staticPressureAtm;
		vessel.distanceToSun = (vessel.vesselTransform.position - sunBody.bodyTransform.position).magnitude;
		if (firstFrame)
		{
			firstFrame = false;
			density = vessel.atmDensity;
			ThermoPrecalculate();
		}
		if (staticPressurekPa > 0.0)
		{
			CalculateConstantsAtmosphere();
		}
		else
		{
			CalculateConstantsVacuum();
		}
		densityThermalLerp = CalculateDensityThermalLerp();
		backgroundRadiationTemp = CalculateBackgroundRadiationTemperature(atmosphericTemperature);
		backgroundRadiationTempExposed = CalculateBackgroundRadiationTemperature(externalTemperature);
		DragCubeSetupAndPartAeroStats(vessel);
		CheckThermalGraph();
		if (needOcclusion)
		{
			UpdateOcclusion(all: true);
		}
		UpdateThermodynamics();
		if (partRef.packed)
		{
			int count2 = vessel.parts.Count;
			while (count2-- > 0)
			{
				Part part = vessel.parts[count2];
				part.forces.Clear();
				part.force.Zero();
				part.torque.Zero();
			}
			return;
		}
		if (FlightGlobals.ActiveVessel == vessel && FlightGlobals.physicalObjects.Count > 0)
		{
			IntegratePhysicalObjects(FlightGlobals.physicalObjects, density);
		}
		if (vessel.precalc.preIntegrationVelocityOffset != Vector3d.zero)
		{
			vessel.OffsetVelocity(vessel.precalc.preIntegrationVelocityOffset);
		}
		Integrate(partRef);
	}

	public virtual void Update()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (!setupRun)
		{
			Setup();
			if (!setupRun)
			{
				return;
			}
		}
		ThermoPrecalculate();
		deltaTime += TimeWarp.deltaTime;
		if (deltaTime > PhysicsGlobals.OcclusionMinStep)
		{
			UpdateOcclusion(all: false);
			deltaTime = 0.0;
		}
	}

	public virtual void ThermoPrecalculate()
	{
		CalculateSunBodyFlux();
	}

	public float GetPhysicslessChildMass(Part part)
	{
		float num = 0f;
		int count = part.children.Count;
		while (count-- > 0)
		{
			Part part2 = part.children[count];
			if (part2.rb == null)
			{
				num += part2.mass + part2.resourceMass + GetPhysicslessChildMass(part2);
			}
		}
		return num;
	}

	public virtual void Integrate(Part part)
	{
		if (part.rb != null)
		{
			part.rb.AddForce(vessel.precalc.integrationAccel, ForceMode.Acceleration);
			part.rb.AddForce(part.force);
			part.rb.AddTorque(part.torque);
			int count = part.forces.Count;
			while (count-- > 0)
			{
				part.rb.AddForceAtPosition(part.forces[count].force, part.forces[count].pos);
			}
		}
		if (part.servoRb != null)
		{
			part.servoRb.AddForce(vessel.precalc.integrationAccel, ForceMode.Acceleration);
		}
		part.forces.Clear();
		part.force.Zero();
		part.torque.Zero();
		UpdateAerodynamics(part);
		int count2 = part.children.Count;
		for (int i = 0; i < count2; i++)
		{
			Part part2 = part.children[i];
			if (part2.isAttached)
			{
				Integrate(part2);
			}
		}
	}

	public virtual void IntegratePhysicalObjects(List<physicalObject> pObjs, double atmDensity)
	{
		double num = 0.0;
		Vector3 zero = Vector3.zero;
		double num2 = 0.0;
		Vector3 force = (FlightGlobals.getGeeForceAtPosition(CoMd, currentMainBody) + FlightGlobals.getCoriolisAcc(Veld, currentMainBody) + FlightGlobals.getCentrifugalAcc(CoMd, currentMainBody)) * PhysicsGlobals.GraviticForceMultiplier;
		int count = pObjs.Count;
		while (count-- > 0)
		{
			physicalObject physicalObject2 = pObjs[count];
			if (physicalObject2 == null)
			{
				continue;
			}
			Rigidbody rb = physicalObject2.rb;
			rb.AddForce(force, ForceMode.Acceleration);
			if (!PhysicsGlobals.ApplyDrag || !(atmDensity > 0.0))
			{
				continue;
			}
			num = physicalObject2.origDrag;
			num2 = 0.0005 * pseudoReDragMult * atmDensity * num * (rb.velocity + Krakensbane.GetFrameVelocity()).sqrMagnitude * (double)PhysicsGlobals.DragMultiplier;
			if (!double.IsNaN(num2) && !double.IsInfinity(num2))
			{
				zero = -(rb.velocity + Krakensbane.GetFrameVelocity()).normalized * num2;
				if (PhysicsGlobals.DragUsesAcceleration)
				{
					rb.AddForce(zero, ForceMode.Acceleration);
				}
				else
				{
					rb.AddForce(zero, ForceMode.Force);
				}
			}
		}
	}

	public virtual void CalculatePressure()
	{
		if (currentMainBody.atmosphere && altitude <= currentMainBody.atmosphereDepth)
		{
			staticPressurekPa = currentMainBody.GetPressure(altitude);
			staticPressureAtm = staticPressurekPa * 0.009869232667160128;
		}
		else
		{
			staticPressurekPa = 0.0;
			staticPressureAtm = 0.0;
		}
	}

	public virtual void CalculateSunBodyFlux()
	{
		if (vessel == null || vessel.state == Vessel.State.DEAD || currentMainBody == null)
		{
			return;
		}
		vessel.directSunlight = false;
		Vector3d vector3d = ScaledSpace.LocalToScaledSpace(integratorTransform.position);
		Vector3d vector3d2 = sunBody.scaledBody.transform.position;
		Vector3d vector3d3 = vector3d2 - vector3d;
		double num = (vector3d2 - vector3d).magnitude;
		if (num == 0.0)
		{
			num = 1.0;
		}
		sunVector = vector3d3 / num;
		Ray ray = new Ray(vector3d, sunVector);
		solarFlux = 0.0;
		sunDot = Vector3d.Dot(sunVector, vessel.upAxis);
		currentMainBody.GetAtmoThermalStats(getBodyFlux: true, sunBody, sunVector, sunDot, vessel.upAxis, altitude, out atmosphereTemperatureOffset, out bodyEmissiveFlux, out bodyAlbedoFlux);
		if (Physics.Raycast(ray, out sunBodyFluxHit, float.MaxValue, sunLayerMask))
		{
			if (sunBodyFluxHit.transform.GetComponent<ScaledMovement>().celestialBody == sunBody)
			{
				realDistanceToSun = (double)ScaledSpace.ScaleFactor * (double)sunBodyFluxHit.distance;
				vessel.directSunlight = true;
			}
		}
		else
		{
			vessel.directSunlight = true;
			realDistanceToSun = num * (double)ScaledSpace.ScaleFactor - sunBody.Radius;
		}
		if (vessel.directSunlight)
		{
			solarFlux = PhysicsGlobals.SolarLuminosity / (Math.PI * 4.0 * realDistanceToSun * realDistanceToSun);
		}
	}

	public virtual double CalculateDensityThermalLerp()
	{
		double num = density;
		if (mach > 1.0)
		{
			double num2 = mach * mach;
			num = (currentMainBody.atmosphereAdiabaticIndex + 1.0) * num2 / (2.0 + (currentMainBody.atmosphereAdiabaticIndex - 1.0) * num2) * num;
		}
		if (num < 0.0625)
		{
			return 1.0 - Math.Sqrt(Math.Sqrt(num));
		}
		if (num < 0.25)
		{
			return 0.75 - Math.Sqrt(num);
		}
		return 0.0625 / num;
	}

	public virtual double CalculateBackgroundRadiationTemperature(double ambientTemp)
	{
		return UtilMath.Lerp(ambientTemp, PhysicsGlobals.SpaceTemperature, densityThermalLerp);
	}

	public virtual void CalculateConstantsVacuum()
	{
		vessel.atmosphericTemperature = (atmosphericTemperature = PhysicsGlobals.SpaceTemperature);
		vessel.externalTemperature = (externalTemperature = PhysicsGlobals.SpaceTemperature);
		solarFluxMultiplier = 1.0;
		Vessel obj = vessel;
		double num = 0.0;
		obj.convectiveCoefficient = 0.0;
		convectiveCoefficient = num;
		Vessel obj2 = vessel;
		num = 0.0;
		obj2.convectiveMachFlux = 0.0;
		convectiveMachFlux = num;
		Vessel obj3 = vessel;
		num = 0.0;
		density = 0.0;
		obj3.atmDensity = num;
		Vessel obj4 = vessel;
		num = 0.0;
		dynamicPressurekPa = 0.0;
		obj4.dynamicPressurekPa = num;
		Vessel obj5 = vessel;
		num = 0.0;
		mach = 0.0;
		obj5.mach = num;
		vessel.speedOfSound = 0.0;
		vessel.solarFlux = solarFlux;
		pseudoReynolds = 0.0;
		pseudoReLerpTimeMult = 1.0;
		pseudoReDragMult = 1.0;
	}

	public virtual void CalculateConstantsAtmosphere()
	{
		currentMainBody.GetSolarAtmosphericEffects(sunDot, density, out solarAirMass, out solarFluxMultiplier);
		vessel.solarFlux = (solarFlux *= solarFluxMultiplier);
		vessel.atmosphericTemperature = (atmosphericTemperature = currentMainBody.GetFullTemperature(altitude, atmosphereTemperatureOffset));
		vessel.atmDensity = (density = CalculateAtmosphericDensity(staticPressurekPa, atmosphericTemperature));
		vessel.dynamicPressurekPa = (dynamicPressurekPa = 0.0005 * density * spd * spd);
		vessel.speedOfSound = currentMainBody.GetSpeedOfSound(staticPressurekPa, density);
		if (vessel.speedOfSound > 0.0)
		{
			vessel.mach = (mach = spd / vessel.speedOfSound);
		}
		else
		{
			Vessel obj = vessel;
			double num = 0.0;
			mach = 0.0;
			obj.mach = num;
		}
		convectiveMachLerp = Math.Pow(UtilMath.Clamp01((mach - PhysicsGlobals.NewtonianMachTempLerpStartMach) / (PhysicsGlobals.NewtonianMachTempLerpEndMach - PhysicsGlobals.NewtonianMachTempLerpStartMach)), PhysicsGlobals.NewtonianMachTempLerpExponent);
		vessel.externalTemperature = (externalTemperature = Math.Max(atmosphericTemperature, CalculateShockTemperature()));
		vessel.convectiveCoefficient = (convectiveCoefficient = CalculateConvectiveCoefficient());
		pseudoReynolds = density * spd;
		pseudoReLerpTimeMult = 1.0 / (PhysicsGlobals.TurbulentConvectionEnd - PhysicsGlobals.TurbulentConvectionStart);
		pseudoReDragMult = PhysicsGlobals.DragCurvePseudoReynolds.Evaluate((float)pseudoReynolds);
	}

	public virtual double CalculateShockTemperature()
	{
		double num = spd * PhysicsGlobals.NewtonianTemperatureFactor;
		if (convectiveMachLerp > 0.0)
		{
			double b = PhysicsGlobals.MachTemperatureScalar * Math.Pow(spd, PhysicsGlobals.MachTemperatureVelocityExponent);
			num = UtilMath.LerpUnclamped(num, b, convectiveMachLerp);
		}
		return num * (double)HighLogic.CurrentGame.Parameters.Difficulty.ReentryHeatScale * currentMainBody.shockTemperatureMultiplier;
	}

	public double CalculateAtmosphericDensity(double pres, double temp)
	{
		return currentMainBody.GetDensity(pres, temp);
	}

	public virtual double CalcConvectiveCoefficient(Vessel.Situations situation)
	{
		return CalculateConvectiveCoefficient(situation);
	}

	public virtual double CalculateConvectiveCoefficient()
	{
		return CalculateConvectiveCoefficient(vessel.situation);
	}

	public virtual double CalculateConvectiveCoefficient(Vessel.Situations situation)
	{
		double num = 0.0;
		num = ((situation == Vessel.Situations.SPLASHED) ? ((PhysicsGlobals.ConvectionFactorSplashed * PhysicsGlobals.NewtonianConvectionFactorBase + Math.Pow(spd, PhysicsGlobals.NewtonianVelocityExponent) * 10.0) * PhysicsGlobals.NewtonianConvectionFactorTotal) : ((convectiveMachLerp == 0.0) ? CalculateConvectiveCoefficientNewtonian() : ((convectiveMachLerp != 1.0) ? UtilMath.LerpUnclamped(CalculateConvectiveCoefficientNewtonian(), CalculateConvectiveCoefficientMach(), convectiveMachLerp) : CalculateConvectiveCoefficientMach())));
		return num * currentMainBody.convectionMultiplier;
	}

	public virtual double CalculateConvectiveCoefficientNewtonian()
	{
		return ((density > 1.0) ? density : Math.Pow(density, PhysicsGlobals.NewtonianDensityExponent)) * (PhysicsGlobals.NewtonianConvectionFactorBase + Math.Pow(spd, PhysicsGlobals.NewtonianVelocityExponent)) * PhysicsGlobals.NewtonianConvectionFactorTotal;
	}

	public virtual double CalculateConvectiveCoefficientMach()
	{
		return 1E-07 * PhysicsGlobals.MachConvectionFactor * ((density > 1.0) ? density : Math.Pow(density, PhysicsGlobals.MachConvectionDensityExponent)) * Math.Pow(spd, PhysicsGlobals.MachConvectionVelocityExponent);
	}

	public void CheckThermalGraph()
	{
		if (recreateThermalGraph)
		{
			UpdateThermalGraph();
			needOcclusion = true;
			recreateThermalGraph = false;
		}
		UpdateCompoundParts();
	}

	public virtual void UpdateThermodynamics()
	{
		fTimeSinceThermo += fDeltaTime;
		if (!(fTimeSinceThermo > PhysicsGlobals.ThermalIntegrationMinStep))
		{
			return;
		}
		double universalTime = Planetarium.GetUniversalTime();
		timeSinceLastUpdate = universalTime - vessel.lastUT;
		if (vessel.lastUT >= 0.0)
		{
			if (timeSinceLastUpdate < 0.0)
			{
				timeSinceLastUpdate = fTimeSinceThermo;
			}
		}
		else
		{
			timeSinceLastUpdate = double.MaxValue;
		}
		vessel.lastUT = universalTime;
		isAnalytical = (double)TimeWarp.CurrentRate > PhysicsGlobals.ThermalMaxIntegrationWarp || timeSinceLastUpdate > PhysicsGlobals.ThermalMaxIntegrationWarp * 0.04;
		fTimeSinceThermoRecip = 1.0 / fTimeSinceThermo;
		double num = 0.0;
		bool flag = true;
		if (isAnalytical)
		{
			num = CalculateAnalyticTemperature();
			double num2 = fTimeSinceThermo;
			if (timeSinceLastUpdate > fTimeSinceThermo * 1.1)
			{
				num2 = timeSinceLastUpdate;
				MonoBehaviour.print("[FlightIntegrator]: Vessel " + vessel.GetDisplayName() + " has been unloaded " + timeSinceLastUpdate + ", applying analytic temperature " + num);
			}
			double num3 = PhysicsGlobals.AnalyticLerpRateInternal * num2;
			double num4 = PhysicsGlobals.AnalyticLerpRateSkin * num2;
			if (!double.IsNaN(num))
			{
				int index = partThermalDataCount;
				while (index-- > 0)
				{
					PartThermalData partThermalData = partThermalDataList[index];
					Part part = partThermalDataList[index].part;
					double num5 = num4 * part.analyticSkinInsulationFactor;
					double num6 = num3 * part.analyticInternalInsulationFactor;
					if (num5 > 1.0)
					{
						num5 = 1.0;
					}
					if (num6 > 1.0)
					{
						num6 = 1.0;
					}
					double num7 = UtilMath.LerpUnclamped(part.skinTemperature, num, num5);
					double num8 = UtilMath.LerpUnclamped(part.temperature, num, num6);
					if (partThermalData.analyticModifier != null)
					{
						IAnalyticTemperatureModifier analyticModifier = partThermalData.analyticModifier;
						analyticModifier.SetAnalyticTemperature(this, num, num8, num7);
						bool lerp;
						double internalTemperature = analyticModifier.GetInternalTemperature(out lerp);
						bool lerp2;
						double skinTemperature = analyticModifier.GetSkinTemperature(out lerp2);
						if (lerp)
						{
							part.temperature = UtilMath.LerpUnclamped(part.temperature, internalTemperature, num6);
						}
						else
						{
							part.temperature = internalTemperature;
						}
						if (lerp2)
						{
							part.skinTemperature = UtilMath.LerpUnclamped(part.skinTemperature, skinTemperature, num5);
						}
						else
						{
							part.skinTemperature = skinTemperature;
						}
					}
					else
					{
						part.skinTemperature = num7;
						part.temperature = num8;
					}
				}
			}
			else if (GameSettings.FI_LOG_TEMP_ERROR)
			{
				Debug.LogError("[FlightIntegrator]: Analytic temp is NaN for vessel " + vessel.GetName());
			}
		}
		else
		{
			int index2 = partThermalDataCount;
			while (index2-- > 0)
			{
				PartThermalData partThermalData2 = partThermalDataList[index2];
				Part part2 = partThermalData2.part;
				part2.thermalConductionFlux = 0.0;
				part2.skinToInternalFlux = 0.0;
				part2.thermalConvectionFlux = 0.0;
				part2.thermalRadiationFlux = 0.0;
				partThermalData2.previousTemperature = partThermalData2.part.temperature;
				partThermalData2.previousSkinTemperature = partThermalData2.part.skinTemperature;
				partThermalData2.previousSkinUnexposedTemperature = partThermalData2.part.skinUnexposedTemperature;
				if (part2.temperature < cacheSpaceTemperature || part2.skinTemperature < cacheSpaceTemperature)
				{
					if (flag)
					{
						num = CalculateAnalyticTemperature();
						flag = false;
					}
					if (!double.IsNaN(num))
					{
						part2.temperature = (part2.skinTemperature = (part2.skinUnexposedTemperature = num));
					}
				}
				SetSkinProperties(partThermalData2);
				PrecalcConduction(partThermalData2);
				PrecalcConvection(partThermalData2);
				PrecalcRadiation(partThermalData2);
				part2.thermalInternalFluxPrevious = part2.thermalInternalFlux * fTimeSinceThermoRecip;
				part2.thermalInternalFlux = 0.0;
				part2.thermalSkinFluxPrevious = part2.thermalSkinFlux * fTimeSinceThermoRecip;
				part2.thermalSkinFlux = 0.0;
				part2.thermalExposedFluxPrevious = part2.thermalExposedFlux * fTimeSinceThermoRecip;
				part2.thermalExposedFlux = 0.0;
			}
			if (!(TimeWarp.CurrentRate > 1f) && !PhysicsGlobals.ThermalIntegrationAlwaysRK2)
			{
				timeFactor = fTimeSinceThermo;
				ThermalIntegrationPass(averageWithPrevious: false);
			}
			else if (fTimeSinceThermo > PhysicsGlobals.ThermalIntegrationMaxTimeOnePass)
			{
				double num9 = Math.Sqrt(fTimeSinceThermo * 50.0 / PhysicsGlobals.ThermalMaxIntegrationWarp);
				num9 = Math.Sqrt(num9) * num9;
				passes = (int)Math.Round(num9 * (double)PhysicsGlobals.ThermalIntegrationHighMaxPasses);
				if (passes < PhysicsGlobals.ThermalIntegrationHighMinPasses)
				{
					passes = PhysicsGlobals.ThermalIntegrationHighMinPasses;
				}
				passesRecip = 1.0 / (double)passes;
				timeFactor = fTimeSinceThermo * passesRecip;
				int num10 = passes;
				while (num10-- > 0)
				{
					ThermalIntegrationPass(averageWithPrevious: false);
					ThermalIntegrationPass(averageWithPrevious: true);
				}
			}
			else
			{
				passes = 1;
				timeFactor = fTimeSinceThermo;
				ThermalIntegrationPass(averageWithPrevious: false);
				ThermalIntegrationPass(averageWithPrevious: true);
			}
		}
		fTimeSinceThermo = 0.0;
	}

	public virtual void ThermalIntegrationPass(bool averageWithPrevious)
	{
		if (PhysicsGlobals.ThermalConductionEnabled)
		{
			UpdateConduction();
		}
		double thermalConvergenceFactor = PhysicsGlobals.ThermalConvergenceFactor;
		bool thermalConvectionEnabled = PhysicsGlobals.ThermalConvectionEnabled;
		bool thermalRadiationEnabled = PhysicsGlobals.ThermalRadiationEnabled;
		int index = partThermalDataCount;
		while (index-- > 0)
		{
			PartThermalData partThermalData = partThermalDataList[index];
			Part part = partThermalData.part;
			if (!averageWithPrevious)
			{
				partThermalData.previousTemperature = part.temperature;
				partThermalData.previousSkinTemperature = part.skinTemperature;
				partThermalData.previousSkinUnexposedTemperature = part.skinUnexposedTemperature;
			}
			double num = 0.0;
			partThermalData.convectionFlux = 0.0;
			double unexpRadiationFlux = num;
			num = 0.0;
			partThermalData.unexpRadiationFlux = unexpRadiationFlux;
			partThermalData.radiationFlux = num;
			if (thermalConvectionEnabled)
			{
				UpdateConvection(partThermalData);
			}
			if (thermalRadiationEnabled)
			{
				UpdateRadiation(partThermalData);
			}
			double num2 = part.skinThermalMassRecip * part.skinExposedMassMult * timeFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num2))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " expMassFactor NaN");
			}
			double num3 = 0.0;
			if (part.skinExposedAreaFrac > 0.0)
			{
				num3 = part.skinThermalMassRecip * part.skinUnexposedMassMult * timeFactor;
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num3))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " unexpMassFactor NaN");
			}
			double num4 = part.thermalMassReciprocal * timeFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num4))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " partMassFactor NaN");
			}
			part.temperature += part.thermalInternalFluxPrevious * num4;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.thermalInternalFluxPrevious))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.thermalInternalFlux NaN");
			}
			double num5 = part.thermalSkinFluxPrevious * part.skinThermalMassRecip * timeFactor;
			if (part.skinExposedAreaFrac > 0.0)
			{
				part.skinTemperature += num5;
			}
			if (part.skinExposedAreaFrac < 1.0)
			{
				part.skinUnexposedTemperature += num5;
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.thermalSkinFluxPrevious))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.thermalSkinFlux NaN");
			}
			if (part.skinExposedAreaFrac > 0.0)
			{
				part.skinTemperature += part.thermalExposedFluxPrevious * num2;
			}
			else
			{
				part.skinUnexposedTemperature += part.thermalExposedFluxPrevious * num3;
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.thermalExposedFluxPrevious))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.thermalExposedFlux NaN");
			}
			part.skinToInternalFlux = (partThermalData.skinInteralConductionFlux + partThermalData.unexpSkinInternalConductionFlux) * thermalConvergenceFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinToInternalFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.skinToInternalFlux NaN");
			}
			part.thermalConductionFlux = partThermalData.intConductionFlux * thermalConvergenceFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.thermalConductionFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.thermalConductionFlux NaN");
			}
			part.temperature += (part.thermalConductionFlux + part.skinToInternalFlux) * num4;
			part.skinTemperature += (partThermalData.skinConductionFlux * part.skinExposedAreaFrac - partThermalData.skinSkinConductionFlux - partThermalData.skinInteralConductionFlux) * num2 * thermalConvergenceFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(partThermalData.skinConductionFlux * part.skinExposedAreaFrac - partThermalData.skinSkinConductionFlux - partThermalData.skinInteralConductionFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " expSkin NaN");
				Debug.LogError("SCF " + partThermalData.skinConductionFlux + ", SSCF " + partThermalData.skinSkinConductionFlux + ", SICF " + partThermalData.skinInteralConductionFlux + ", USICF " + partThermalData.unexpSkinInternalConductionFlux);
			}
			part.skinUnexposedTemperature += (partThermalData.skinConductionFlux * (1.0 - part.skinExposedAreaFrac) + partThermalData.skinSkinConductionFlux - partThermalData.unexpSkinInternalConductionFlux) * num3 * thermalConvergenceFactor;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(partThermalData.skinConductionFlux * (1.0 - part.skinExposedAreaFrac) + partThermalData.skinSkinConductionFlux - partThermalData.unexpSkinInternalConductionFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " unexpSkin NaN");
			}
			part.thermalConvectionFlux = partThermalData.convectionFlux * thermalConvergenceFactor;
			part.skinTemperature += part.thermalConvectionFlux * num2;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.thermalConvectionFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " part.thermalConvectionFlux NaN");
			}
			part.skinTemperature += partThermalData.radiationFlux * num2;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(partThermalData.radiationFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " expRad NaN");
			}
			part.skinUnexposedTemperature += partThermalData.unexpRadiationFlux * num3;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(partThermalData.unexpRadiationFlux))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + " unexpRad NaN");
			}
			part.thermalRadiationFlux = partThermalData.radiationFlux + partThermalData.unexpRadiationFlux;
			if (double.IsNaN(part.temperature))
			{
				if (GameSettings.FI_LOG_TEMP_ERROR)
				{
					Debug.LogError("[FlightIntegrator]: For part " + part.name + " temp NaN");
				}
				part.temperature = partThermalData.previousTemperature;
			}
			if (double.IsNaN(part.skinTemperature))
			{
				if (GameSettings.FI_LOG_TEMP_ERROR)
				{
					Debug.LogError("[FlightIntegrator]: For part " + part.name + " skinTemp NaN");
				}
				part.skinTemperature = partThermalData.previousSkinTemperature;
			}
			if (double.IsNaN(part.skinUnexposedTemperature))
			{
				if (GameSettings.FI_LOG_TEMP_ERROR)
				{
					Debug.LogError("[FlightIntegrator]: For part " + part.name + " skinUnexpTemp NaN");
				}
				part.skinUnexposedTemperature = partThermalData.previousSkinUnexposedTemperature;
			}
			if (averageWithPrevious)
			{
				part.temperature = (partThermalData.previousTemperature + part.temperature) * 0.5;
				part.skinTemperature = (part.skinTemperature + partThermalData.previousSkinTemperature) * 0.5;
				part.skinUnexposedTemperature = (part.skinUnexposedTemperature + partThermalData.previousSkinUnexposedTemperature) * 0.5;
			}
			if (part.temperature < cacheSpaceTemperature)
			{
				part.temperature = cacheSpaceTemperature;
			}
			if (part.skinTemperature < cacheSpaceTemperature)
			{
				part.skinTemperature = cacheSpaceTemperature;
			}
			if (part.skinUnexposedTemperature < cacheSpaceTemperature)
			{
				part.skinUnexposedTemperature = cacheSpaceTemperature;
			}
			if (GameSettings.FI_LOG_OVERTEMP && (part.temperature > part.maxTemp || part.skinTemperature > part.skinMaxTemp))
			{
				MonoBehaviour.print("[FlightIntegrator]: *** Part " + part.name + " over-temp. \nPrevious were " + partThermalData.previousTemperature + "/" + partThermalData.previousSkinTemperature + ". \nRad " + part.radiativeArea + ", expFrac " + part.skinExposedAreaFrac + ".\n TMs " + part.thermalMass + "/" + part.skinThermalMass + ". Factors are " + num4 + "/" + num2 + ".\nFluxes are: \nAdd: " + part.thermalInternalFluxPrevious + " to int, " + (part.thermalExposedFluxPrevious + part.thermalSkinFluxPrevious) + "\nRad: " + partThermalData.radiationFlux + "\nCnv: " + partThermalData.convectionFlux + "\nCdI: " + part.thermalConductionFlux + "\nSC:  " + partThermalData.skinConductionFlux * thermalConvergenceFactor + "\nSIC: " + partThermalData.skinInteralConductionFlux * thermalConvergenceFactor + "\nSSC: " + partThermalData.skinSkinConductionFlux * thermalConvergenceFactor + "\nUIC: " + partThermalData.unexpSkinInternalConductionFlux * thermalConvergenceFactor);
			}
		}
	}

	public virtual double CalculateAnalyticTemperature()
	{
		double num = 1.0 / fTimeSinceThermo;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		double num7 = 0.0;
		double num8 = 0.0;
		double num9 = double.MaxValue;
		double num10 = 0.0;
		double num11 = 0.0;
		if (partThermalDataCount == 0)
		{
			UpdateThermalGraph();
		}
		for (int i = 0; i < partThermalDataCount; i++)
		{
			Part part = partThermalDataList[i].part;
			if (double.IsNaN(part.temperature))
			{
				part.temperature = cacheSpaceTemperature;
			}
			if (double.IsNaN(part.skinTemperature))
			{
				part.skinTemperature = cacheSpaceTemperature;
			}
			num11 = Math.Max(num11, Math.Max(part.skinTemperature, part.temperature));
			part.thermalInternalFluxPrevious = part.thermalInternalFlux * num;
			num2 += part.thermalInternalFluxPrevious;
			part.thermalInternalFlux = 0.0;
			part.thermalSkinFluxPrevious = part.thermalSkinFlux * num;
			num2 += part.thermalSkinFluxPrevious;
			part.thermalSkinFlux = 0.0;
			part.thermalExposedFluxPrevious = part.thermalExposedFlux * num;
			num2 += part.thermalExposedFluxPrevious;
			part.thermalExposedFlux = 0.0;
			double num12 = 0.0;
			part.skinToInternalFlux = 0.0;
			double thermalRadiationFlux = num12;
			num12 = 0.0;
			part.thermalRadiationFlux = thermalRadiationFlux;
			double thermalConvectionFlux = num12;
			num12 = 0.0;
			part.thermalConvectionFlux = thermalConvectionFlux;
			part.thermalConductionFlux = num12;
			if (!part.ShieldedFromAirstream)
			{
				num3 += GetSunArea(partThermalDataList[i]) * part.absorptiveConstant;
				num4 += GetBodyArea(partThermalDataList[i]) * part.absorptiveConstant;
				num5 += part.radiativeArea;
				num6 += part.radiativeArea * part.emissiveConstant;
				num7 += part.radiativeArea * part.absorptiveConstant;
			}
			num8 += part.thermalMass + part.skinThermalMass;
			if (part.maxTemp < num9)
			{
				num9 = part.maxTemp;
			}
		}
		if (num6 <= 0.0)
		{
			num6 = 0.01;
		}
		if (num7 <= 0.0)
		{
			num7 = 0.01;
		}
		if (num5 < num6)
		{
			num5 = num6;
		}
		double num13 = num7 / num5;
		double num14 = 0.0;
		double num15 = backgroundRadiationTemp;
		num15 *= num15;
		num15 *= num15;
		num15 *= num13;
		double num16 = 0.0;
		if (vessel.directSunlight)
		{
			num16 = num3 * solarFlux * cacheRadiationFactor;
		}
		double num17 = bodyEmissiveFlux + bodyAlbedoFlux;
		if (num17 > 0.0)
		{
			num17 = num4 * UtilMath.Lerp(0.0, num17, densityThermalLerp) * PhysicsGlobals.RadiationFactor;
		}
		double num18 = num16 + num17;
		int count = overheatModules.Count;
		while (count-- > 0)
		{
			num10 += overheatModules[count].GetFlux();
		}
		double num19 = 0.0;
		int count2 = previewModules.Count;
		while (count2-- > 0)
		{
			IAnalyticPreview analyticPreview = previewModules[count2];
			analyticPreview.AnalyticInfo(this, num18, backgroundRadiationTemp, num6, num13, num2, convectiveCoefficient, atmosphereTemperatureOffset, num11);
			num19 += analyticPreview.InternalFluxAdjust();
		}
		num2 += num19;
		double num20 = 1.0 / (cacheStefanBoltzmanConstant * num6 * cacheRadiationFactor);
		double num21 = (num18 + num2 * 1000.0) * num20 + num15;
		double num22 = cacheSpaceTemperature;
		num22 *= num22;
		num22 *= num22;
		num14 = ((!(num21 > num22)) ? cacheSpaceTemperature : Math.Sqrt(Math.Sqrt(num21)));
		double t = 1.0;
		if (density > 0.0)
		{
			double num23 = num5 / num8;
			t = 1.0 / ((PhysicsGlobals.AnalyticConvectionSensitivityBase + num23) * PhysicsGlobals.AnalyticConvectionSensitivityFinal * convectiveCoefficient);
			num14 = UtilMath.Lerp(atmosphericTemperature, num14, t);
		}
		if (num14 > num9 - 3.0)
		{
			double num24 = Math.Sqrt(Math.Sqrt(num21 - num10));
			if (density > 0.0)
			{
				num24 = UtilMath.Lerp(atmosphericTemperature, num24, t);
			}
			if (num24 < num9 - 10.0)
			{
				double num25 = num9 - 10.0;
				num25 *= num25;
				num25 *= num25;
				num25 = num21 - num25;
				num25 /= num10;
				int count3 = overheatModules.Count;
				for (int j = 0; j < count3; j++)
				{
					overheatModules[j].OnOverheat(num25);
				}
				num14 = num9 - 10.0;
			}
		}
		return num14;
	}

	public virtual void PrecalcConduction(PartThermalData ptd)
	{
		Part part = ptd.part;
		ptd.conductionMult = cacheConductionFactor * 10.0 * part.heatConductivity;
		ptd.skinSkinTransfer = part.skinSkinConductionMult * cacheSkinSkinConductionFactor * Math.Min(part.skinExposedAreaFrac, 1.0 - part.skinExposedAreaFrac) * 2.0 * Math.Sqrt(part.radiativeArea);
	}

	public virtual void UpdateConduction()
	{
		bool flag = false;
		bool flag2 = false;
		int num = partThermalDataCount - 1;
		while (true)
		{
			PartThermalData partThermalData = partThermalDataList[num];
			PartThermalData partThermalData2 = partThermalData;
			PartThermalData partThermalData3 = partThermalData;
			PartThermalData partThermalData4 = partThermalData;
			PartThermalData partThermalData5 = partThermalData;
			PartThermalData partThermalData6 = partThermalData;
			PartThermalData partThermalData7 = partThermalData;
			PartThermalData partThermalData8 = partThermalData;
			double num2 = 0.0;
			partThermalData8.unexpSkinInternalConductionFlux = 0.0;
			double skinInteralConductionFlux = num2;
			num2 = 0.0;
			partThermalData7.skinInteralConductionFlux = skinInteralConductionFlux;
			double skinSkinConductionFlux = num2;
			num2 = 0.0;
			partThermalData6.skinSkinConductionFlux = skinSkinConductionFlux;
			double localIntConduction = num2;
			num2 = 0.0;
			partThermalData5.localIntConduction = localIntConduction;
			double intConductionFlux = num2;
			num2 = 0.0;
			partThermalData4.intConductionFlux = intConductionFlux;
			double localSkinConduction = num2;
			num2 = 0.0;
			partThermalData3.localSkinConduction = localSkinConduction;
			partThermalData2.skinConductionFlux = num2;
			partThermalData.unifiedTemp = partThermalData.GetUnifiedSkinTemp();
			num--;
			if (num < 0)
			{
				break;
			}
			if (flag || partThermalData.part.temperature < partThermalDataList[num].part.temperature)
			{
				flag = true;
			}
			if (flag2 || partThermalDataListSkin[num + 1].unifiedTemp < partThermalDataListSkin[num].unifiedTemp)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			partThermalDataList.Sort(PartThermalData.intComparer);
		}
		for (int num3 = partThermalDataCount - 2; num3 >= 0; num3--)
		{
			PartThermalData partThermalData = partThermalDataList[num3];
			Part part = partThermalData.part;
			double num4 = part.thermalMass * part.temperature;
			double num5 = part.thermalMass;
			int thermalLinkCount = partThermalData.thermalLinkCount;
			while (thermalLinkCount-- > 0)
			{
				ThermalLink thermalLink = partThermalData.thermalLinks[thermalLinkCount];
				PartThermalData ptd = thermalLink.remotePart.ptd;
				if (ptd == null)
				{
					continue;
				}
				thermalLink.temperatureDelta = thermalLink.remotePart.temperature - part.temperature;
				if (thermalLink.temperatureDelta > 0.0)
				{
					num4 += thermalLink.remotePart.thermalMass * thermalLink.remotePart.temperature;
					num5 += thermalLink.remotePart.thermalMass;
					double num6 = partThermalData.conductionMult * thermalLink.remotePart.heatConductivity;
					if (part.ShieldedFromAirstream != thermalLink.remotePart.ShieldedFromAirstream)
					{
						num6 *= PhysicsGlobals.ShieldedConductionFactor;
					}
					double num7 = thermalLink.contactArea * num6 * thermalLink.temperatureDelta;
					ptd.localIntConduction -= num7;
					partThermalData.localIntConduction += num7;
				}
			}
			if (partThermalData.localIntConduction != 0.0)
			{
				double num8 = num4 / num5;
				double num9 = 1.0;
				double num10 = partThermalData.localIntConduction * part.thermalMassReciprocal;
				double num11 = part.temperature + num10 - num8;
				if (num11 > num10)
				{
					if (GameSettings.FI_LOG_TEMP_ERROR)
					{
						Debug.Log("For part " + part.name + " overflow > change! System temp " + num8 + ", our temp " + part.temperature + ", unclamped delta " + num10);
					}
					num9 = 0.0;
				}
				else if (num11 > 0.0 && num10 > 0.0)
				{
					num9 = (num10 - num11) / num10;
					partThermalData.localIntConduction *= num9;
				}
				int thermalLinkCount2 = partThermalData.thermalLinkCount;
				while (thermalLinkCount2-- > 0)
				{
					ThermalLink thermalLink = partThermalData.thermalLinks[thermalLinkCount2];
					PartThermalData ptd = thermalLink.remotePart.ptd;
					if (ptd == null)
					{
						continue;
					}
					ptd.localIntConduction *= num9;
					if (ptd.localIntConduction < 0.0)
					{
						num10 = ptd.localIntConduction * thermalLink.remotePart.thermalMassReciprocal;
						num11 = thermalLink.remotePart.temperature + num10 - num8;
						if (num11 < 0.0 && num10 < 0.0)
						{
							double num12 = num11 * thermalLink.remotePart.thermalMass;
							if (ptd.localIntConduction < num12)
							{
								partThermalData.localIntConduction += num12;
								ptd.localIntConduction -= num12;
							}
							else
							{
								partThermalData.localIntConduction += ptd.localIntConduction;
								ptd.localIntConduction = 0.0;
							}
						}
					}
					ptd.intConductionFlux += ptd.localIntConduction;
					ptd.localIntConduction = 0.0;
				}
				if (partThermalData.localIntConduction > 0.0)
				{
					partThermalData.intConductionFlux += partThermalData.localIntConduction;
				}
				partThermalData.localIntConduction = 0.0;
			}
		}
		if (flag2)
		{
			partThermalDataListSkin.Sort(PartThermalData.skinComparer);
		}
		double skinInternalConductionFactor = PhysicsGlobals.SkinInternalConductionFactor;
		int num13 = partThermalDataCount;
		while (num13-- > 0)
		{
			PartThermalData partThermalData = partThermalDataListSkin[num13];
			Part part = partThermalData.part;
			if (!(part.radiativeArea > 0.0))
			{
				continue;
			}
			double num14 = partThermalData.conductionMult * 0.5;
			double num15 = part.temperature * part.thermalMass;
			double num16;
			double num22;
			if (partThermalData.exposed && part.skinExposedAreaFrac < 1.0)
			{
				num16 = part.skinTemperature - part.skinUnexposedTemperature;
				if (num16 > 0.001 || num16 < -0.001)
				{
					double num17 = num14 * partThermalData.skinSkinTransfer;
					double val = 1.0;
					double val2 = 1.0;
					double num18 = (0.0 - num16) * num17 * part.skinThermalMassRecip * part.skinExposedMassMult;
					double num19 = num16 * num17 * part.skinThermalMassRecip * part.skinUnexposedMassMult;
					double num20 = part.skinTemperature + num18 - partThermalData.unifiedTemp;
					double num21 = part.skinUnexposedTemperature + num19 - partThermalData.unifiedTemp;
					if (num16 > 0.0)
					{
						if (num20 < 0.0)
						{
							val = (num18 - num20) / num18;
						}
						if (num21 > 0.0)
						{
							val2 = (num19 - num21) / num19;
						}
					}
					else
					{
						if (num20 > 0.0)
						{
							val = (num18 - num20) / num18;
						}
						if (num21 < 0.0)
						{
							val2 = (num19 - num21) / num19;
						}
					}
					num22 = Math.Max(0.0, Math.Min(val, val2));
					partThermalData.skinSkinConductionFlux = num17 * num22 * num16;
				}
				num16 = part.skinUnexposedTemperature - part.temperature;
				if (num16 > 0.001 || num16 < -0.001)
				{
					double num17 = part.radiativeArea * (1.0 - part.skinExposedAreaFrac) * num14 * skinInternalConductionFactor * part.skinInternalConductionMult;
					double num23 = part.skinThermalMass * (1.0 - part.skinExposedAreaFrac);
					double num24 = (num15 + part.skinUnexposedTemperature * num23) / (part.thermalMass + num23);
					double num25 = (0.0 - num16) * num17 * part.skinThermalMassRecip * part.skinUnexposedMassMult;
					double num26 = num16 * num17 * part.thermalMassReciprocal;
					double num27 = part.skinUnexposedTemperature + num25 - num24;
					double num28 = part.temperature + num26 - num24;
					double val3 = 1.0;
					double val4 = 1.0;
					if (num16 > 0.0)
					{
						if (num27 < 0.0)
						{
							val3 = (num25 - num27) / num25;
						}
						if (num28 > 0.0)
						{
							val4 = (num26 - num28) / num26;
						}
					}
					else
					{
						if (num27 > 0.0)
						{
							val3 = (num25 - num27) / num25;
						}
						if (num28 < 0.0)
						{
							val4 = (num26 - num28) / num26;
						}
					}
					num22 = Math.Max(0.0, Math.Min(val4, val3));
					partThermalData.unexpSkinInternalConductionFlux = num17 * num22 * num16;
				}
			}
			num16 = part.skinTemperature - part.temperature;
			if (num16 > 0.001 || num16 < -0.001)
			{
				double num17 = part.skinExposedAreaFrac * part.radiativeArea * num14 * skinInternalConductionFactor * part.skinInternalConductionMult;
				double num29 = part.skinThermalMass * part.skinExposedAreaFrac;
				double num30 = (num15 + part.skinTemperature * num29) / (part.thermalMass + num29);
				double num25 = (0.0 - num16) * num17 * part.skinThermalMassRecip * part.skinExposedMassMult;
				double num26 = num16 * num17 * part.thermalMassReciprocal;
				double num27 = part.skinTemperature + num25 - num30;
				double num28 = part.temperature + num26 - num30;
				double val3 = 1.0;
				double val4 = 1.0;
				if (num16 > 0.0)
				{
					if (num27 < 0.0)
					{
						val3 = (num25 - num27) / num25;
					}
					if (num28 > 0.0)
					{
						val4 = (num26 - num28) / num26;
					}
				}
				else
				{
					if (num27 > 0.0)
					{
						val3 = (num25 - num27) / num25;
					}
					if (num28 < 0.0)
					{
						val4 = (num26 - num28) / num26;
					}
				}
				num22 = Math.Max(0.0, Math.Min(val4, val3));
				partThermalData.skinInteralConductionFlux = num17 * num22 * num16;
			}
			if (num13 >= partThermalDataCount - 1)
			{
				continue;
			}
			double num31 = part.skinThermalMass * partThermalData.unifiedTemp;
			double num32 = part.skinThermalMass;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(partThermalData.unifiedTemp))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", unified skin temp is NaN");
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num14))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", conductionMult is NaN");
			}
			int thermalLinkCount3 = partThermalData.thermalLinkCount;
			while (thermalLinkCount3-- > 0)
			{
				ThermalLink thermalLink = partThermalData.thermalLinks[thermalLinkCount3];
				PartThermalData ptd = thermalLink.remotePart.ptd;
				if (ptd == null)
				{
					continue;
				}
				if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(ptd.unifiedTemp))
				{
					Debug.LogError("[FlightIntegrator]: For remote part " + ptd.part.name + ", remote unified skin temp is NaN");
				}
				if (part.ShieldedFromAirstream != thermalLink.remotePart.ShieldedFromAirstream)
				{
					continue;
				}
				double num33 = ptd.unifiedTemp - partThermalData.unifiedTemp;
				if (!(num33 <= 0.0) && !(part.skinTemperature >= Math.Max(ptd.part.skinTemperature, ptd.part.skinUnexposedTemperature)))
				{
					num31 += thermalLink.remotePart.skinThermalMass * ptd.unifiedTemp;
					num32 += thermalLink.remotePart.skinThermalMass;
					double num34 = num14 * thermalLink.remotePart.heatConductivity * part.skinSkinConductionMult * thermalLink.remotePart.skinSkinConductionMult * cacheSkinSkinConductionFactor * thermalLink.contactAreaSqrt * 200.0 * num33;
					if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num34))
					{
						Debug.LogError("[FlightIntegrator]: For part " + part.name + " and remote " + ptd.part.name + ", energy is NaN");
					}
					ptd.localSkinConduction = 0.0 - num34;
					partThermalData.localSkinConduction += num34;
				}
			}
			if (partThermalData.localSkinConduction == 0.0)
			{
				continue;
			}
			double num35 = num31 / num32;
			num22 = 1.0;
			double num36 = partThermalData.localSkinConduction * part.skinThermalMassRecip;
			double num37 = partThermalData.unifiedTemp + num36 - num35;
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num35))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", system temp is NaN");
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num36))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", tempDelta is NaN");
			}
			if (num37 > num36)
			{
				if (GameSettings.FI_LOG_TEMP_ERROR)
				{
					Debug.LogError("[FlightIntegrator]: For part " + part.name + " overflow > change! System temp " + num35 + ", our unified " + partThermalData.unifiedTemp + ", unclamped delta " + num36);
				}
				num22 = 0.0;
			}
			else if (num37 > 0.0 && num36 > 0.0)
			{
				num22 = (num36 - num37) / num36;
				partThermalData.localSkinConduction *= num22;
			}
			int thermalLinkCount4 = partThermalData.thermalLinkCount;
			while (thermalLinkCount4-- > 0)
			{
				ThermalLink thermalLink = partThermalData.thermalLinks[thermalLinkCount4];
				PartThermalData ptd = thermalLink.remotePart.ptd;
				if (ptd == null)
				{
					continue;
				}
				ptd.localSkinConduction *= num22;
				if (ptd.localSkinConduction < 0.0)
				{
					num36 = ptd.localSkinConduction * thermalLink.remotePart.skinThermalMassRecip;
					if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num36))
					{
						Debug.LogError("[FlightIntegrator]: For remote part " + ptd.part.name + ", tempDelta is NaN");
					}
					num37 = ptd.unifiedTemp + num36 - num35;
					if (num37 < 0.0 && num36 < 0.0)
					{
						double num38 = num37 * thermalLink.remotePart.skinThermalMass;
						if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(num38))
						{
							Debug.LogError("[FlightIntegrator]: For remote part " + ptd.part.name + ", offset is NaN");
						}
						if (ptd.localSkinConduction < num38)
						{
							partThermalData.localSkinConduction += num38;
							ptd.localSkinConduction -= num38;
						}
						else
						{
							partThermalData.localSkinConduction += ptd.localSkinConduction;
							ptd.localSkinConduction = 0.0;
						}
					}
				}
				ptd.skinConductionFlux += ptd.localSkinConduction;
				ptd.localSkinConduction = 0.0;
			}
			if (partThermalData.localSkinConduction > 0.0)
			{
				partThermalData.skinConductionFlux += partThermalData.localSkinConduction;
			}
			partThermalData.localSkinConduction = 0.0;
		}
	}

	public virtual void UnifySkinTemp(PartThermalData ptd)
	{
		Part part = ptd.part;
		part.skinTemperature = ptd.GetUnifiedSkinTemp();
		ptd.exposed = false;
		part.skinExposedArea = part.radiativeArea;
		double skinUnexposedMassMult = 0.0;
		part.skinUnexposedTemperature = 0.0;
		part.skinUnexposedMassMult = skinUnexposedMassMult;
		skinUnexposedMassMult = 1.0;
		part.skinExposedMassMult = 1.0;
		part.skinExposedAreaFrac = skinUnexposedMassMult;
	}

	public virtual void UpdateThermalGraph()
	{
		Dictionary<Part, double> dictionary = new Dictionary<Part, double>(partCount);
		int index = partThermalDataCount;
		while (index-- > 0)
		{
			PartThermalData partThermalData = partThermalDataList[index];
			Part part = partThermalData.part;
			double value = ((!partThermalData.exposed) ? 0.0 : part.skinExposedArea);
			dictionary[part] = value;
		}
		partThermalDataList.Clear();
		if (partThermalDataList.Capacity < partCount || partThermalDataList.Capacity > partCount * 2)
		{
			partThermalDataList.Capacity = partCount;
		}
		partThermalDataListSkin.Clear();
		if (partThermalDataListSkin.Capacity < partCount || partThermalDataListSkin.Capacity > partCount * 2)
		{
			partThermalDataListSkin.Capacity = partCount;
		}
		partThermalDataCount = 0;
		occlusionConv.Clear();
		if (occlusionConv.Capacity < partCount || occlusionConv.Capacity > partCount * 2)
		{
			occlusionConv.Capacity = partCount;
		}
		occlusionSun.Clear();
		if (occlusionSun.Capacity < partCount || occlusionSun.Capacity > partCount * 2)
		{
			occlusionSun.Capacity = partCount;
		}
		occlusionBody.Clear();
		if (occlusionBody.Capacity < partCount || occlusionBody.Capacity > partCount * 2)
		{
			occlusionBody.Capacity = partCount;
		}
		overheatModules.Clear();
		previewModules.Clear();
		compoundParts = new List<CompoundPart>();
		for (int i = 0; i < partCount; i++)
		{
			Part part2 = vessel.parts[i];
			part2.DragCubes.RequestOcclusionUpdate();
			if (part2.thermalMass == 0.0)
			{
				continue;
			}
			PartThermalData partThermalData2 = (part2.ptd = new PartThermalData(part2));
			overheatModules.AddRange(partThermalData2.overheatModules);
			previewModules.AddRange(partThermalData2.analyticPreviewModules);
			dictionary.TryGetValue(part2, out part2.skinExposedArea);
			if (part2.skinExposedArea > 0.0)
			{
				partThermalData2.exposed = true;
			}
			if (part2.skinTemperature < PhysicsGlobals.SpaceTemperature)
			{
				part2.skinTemperature = part2.temperature;
			}
			if (part2.parent != null && part2.parent.thermalMass != 0.0)
			{
				partThermalData2.thermalLinks.Add(new ThermalLink(part2, part2.parent));
				partThermalData2.thermalLinkCount++;
			}
			int count = part2.children.Count;
			for (int j = 0; j < count; j++)
			{
				Part part3 = part2.children[j];
				if (part3.thermalMass != 0.0)
				{
					partThermalData2.thermalLinks.Add(new ThermalLink(part2, part3));
					partThermalData2.thermalLinkCount++;
				}
			}
			if (part2 is CompoundPart)
			{
				compoundParts.Add(part2 as CompoundPart);
			}
			partThermalDataList.Add(partThermalData2);
			partThermalDataListSkin.Add(partThermalData2);
			occlusionConv.Add(partThermalData2.convectionData);
			occlusionSun.Add(partThermalData2.sunData);
			occlusionBody.Add(partThermalData2.bodyData);
			partThermalDataCount++;
		}
		if (compoundParts.Count == 0)
		{
			compoundParts = null;
		}
		occludersConvection = new OcclusionCone[partThermalDataCount];
		occludersSun = new OcclusionCylinder[partThermalDataCount];
		occludersBody = new OcclusionCylinder[partThermalDataCount];
	}

	public virtual void UpdateCompoundParts()
	{
		if (compoundParts == null)
		{
			return;
		}
		compoundPartsTimer++;
		if (compoundPartsTimer <= 2)
		{
			return;
		}
		int count = compoundParts.Count;
		while (count-- > 0)
		{
			CompoundPart compoundPart = compoundParts[count];
			Part part = compoundPart;
			PartThermalData ptd = part.ptd;
			if (ptd == null)
			{
				continue;
			}
			Part target = compoundPart.target;
			if (target != null)
			{
				PartThermalData ptd2 = target.ptd;
				if (ptd2 != null)
				{
					ptd2.thermalLinks.Add(new ThermalLink(target, part));
					ptd2.thermalLinkCount++;
					ptd.thermalLinks.Add(new ThermalLink(part, target));
					ptd.thermalLinkCount++;
				}
			}
		}
		compoundPartsTimer = 0;
		compoundParts = null;
	}

	public virtual void SetSkinProperties(PartThermalData ptd)
	{
		Part part = ptd.part;
		if (part.skinUnexposedTemperature < cacheSpaceTemperature)
		{
			part.skinUnexposedTemperature = part.skinTemperature;
		}
		if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinTemperature))
		{
			Debug.LogError("[FlightIntegrator]: For part " + part.name + " in SSP, skintemp NaN");
		}
		double num = Math.Max(part.radiativeArea, 0.001);
		ptd.radAreaRecip = 1.0 / num;
		if (num == 0.0)
		{
			ptd.radAreaRecip = 1.0;
		}
		bool flag = false;
		double num2 = 1.0;
		if (!part.ShieldedFromAirstream && part.atmDensity > 0.0)
		{
			ptd.convectionArea = UtilMath.Lerp(num, part.exposedArea, 0.0 - cacheFullConvectionAreaMin + (mach - cacheFullToCrossSectionLerpStart) / (cacheFullToCrossSectionLerpEnd - cacheFullToCrossSectionLerpStart)) * ptd.convectionAreaMultiplier;
			double num3 = ptd.convectionArea * ptd.radAreaRecip;
			if (num3 < 0.999 && !double.IsNaN(num3))
			{
				if (num3 > 0.001)
				{
					num2 = num3;
					flag = true;
				}
				else
				{
					num2 = 0.0;
				}
			}
			else
			{
				num2 = 1.0;
				flag = true;
			}
		}
		if (flag)
		{
			if (!ptd.exposed || num2 == 1.0)
			{
				part.skinUnexposedTemperature = part.skinTemperature;
				ptd.exposed = true;
			}
			part.skinExposedMassMult = 1.0 / num2;
			if (num2 < 1.0)
			{
				part.skinUnexposedMassMult = 1.0 / (1.0 - num2);
			}
			else
			{
				part.skinUnexposedMassMult = 0.0;
			}
			if (part.skinExposedAreaFrac != num2 && part.skinUnexposedTemperature != part.skinTemperature && part.skinExposedAreaFrac > 0.0 && part.skinExposedAreaFrac < 1.0 && num2 < 1.0)
			{
				double num4 = 1.0 - part.skinExposedAreaFrac;
				double num5 = part.skinTemperature * part.skinExposedAreaFrac * part.skinThermalMass;
				double num6 = part.skinUnexposedTemperature * num4 * part.skinThermalMass;
				double num7 = num2 - part.skinExposedAreaFrac;
				double num8 = 0.0;
				num8 = ((!(num7 > 0.0)) ? (num7 / part.skinExposedAreaFrac * num5) : (num7 / num4 * num6));
				part.skinTemperature = (num5 + num8) * part.skinExposedMassMult * part.skinThermalMassRecip;
				part.skinUnexposedTemperature = (num6 - num8) * part.skinUnexposedMassMult * part.skinThermalMassRecip;
				if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinTemperature))
				{
					Debug.LogError("[FlightIntegrator]: For part " + part.name + " in SSP, now NaN, transfer " + num8 + ", delta " + num7 + ", old unexp " + num4 + ", exp " + part.skinExposedAreaFrac + ", new exp " + num2 + ", recips " + part.skinExposedMassMult + "/" + part.skinUnexposedMassMult);
				}
			}
			part.skinExposedAreaFrac = num2;
			part.skinExposedArea = num2 * num;
		}
		else
		{
			if (ptd.exposed)
			{
				UnifySkinTemp(ptd);
			}
			ptd.exposed = false;
			double num9 = 0.0;
			part.skinUnexposedMassMult = 0.0;
			double skinExposedArea = num9;
			num9 = 0.0;
			part.skinExposedArea = skinExposedArea;
			ptd.convectionArea = num9;
			num9 = 1.0;
			part.skinExposedMassMult = 1.0;
			part.skinExposedAreaFrac = num9;
		}
	}

	public virtual void PrecalcConvection(PartThermalData ptd)
	{
		if (ptd.convectionArea <= 0.0)
		{
			return;
		}
		Part part = ptd.part;
		ptd.postShockExtTemp = UtilMath.LerpUnclamped(atmosphericTemperature, externalTemperature, ptd.convectionTempMultiplier);
		ptd.finalCoeff = convectiveCoefficient * ptd.convectionArea * 0.001 * part.heatConvectiveConstant * ptd.convectionCoeffMultiplier;
		ptd.partPseudoRe = pseudoReynolds * (double)part.DragCubes.DragCoeff;
		if (ptd.partPseudoRe > cacheTurbulentConvectionStart)
		{
			if (ptd.partPseudoRe > cacheTurbulentConvectionEnd)
			{
				ptd.finalCoeff *= cacheTurbulentConvectionMult;
			}
			else
			{
				ptd.finalCoeff *= UtilMath.LerpUnclamped(1.0, cacheTurbulentConvectionMult, (ptd.partPseudoRe - cacheTurbulentConvectionStart) * pseudoReLerpTimeMult);
			}
		}
		ptd.finalCoeff = Math.Min(ptd.finalCoeff, part.skinThermalMass * part.skinExposedAreaFrac);
	}

	public virtual void UpdateConvection(PartThermalData ptd)
	{
		if (!(ptd.convectionArea <= 0.0))
		{
			ptd.convectionFlux = (ptd.postShockExtTemp - ptd.part.skinTemperature) * ptd.finalCoeff;
		}
	}

	public virtual void PrecalcRadiation(PartThermalData ptd)
	{
		Part part = ptd.part;
		double num = cacheRadiationFactor * 0.001;
		ptd.emissScalar = part.emissiveConstant * num;
		ptd.absorbScalar = part.absorptiveConstant * num;
		ptd.sunFlux = 0.0;
		ptd.bodyFlux = bodyEmissiveFlux + bodyAlbedoFlux;
		double num2 = part.radiativeArea * (1.0 - part.skinExposedAreaFrac);
		ptd.expFlux = 0.0;
		ptd.unexpFlux = 0.0;
		ptd.brtUnexposed = backgroundRadiationTemp;
		ptd.brtExposed = UtilMath.Lerp(backgroundRadiationTemp, backgroundRadiationTempExposed, ptd.convectionTempMultiplier);
		if (part.ShieldedFromAirstream)
		{
			return;
		}
		if (vessel.directSunlight && !part.ShieldedFromAirstream)
		{
			double sunArea = GetSunArea(ptd);
			if (sunArea > 0.0)
			{
				ptd.sunFlux = ptd.absorbScalar * solarFlux;
				if (ptd.exposed)
				{
					double num3 = ((double)Vector3.Dot(sunVector, nVel) + 1.0) * 0.5;
					double num4 = Math.Min(sunArea, part.skinExposedArea * num3);
					double num5 = Math.Min(sunArea - num4, num2 * (1.0 - num3));
					ptd.expFlux += ptd.sunFlux * num4;
					ptd.unexpFlux += ptd.sunFlux * num5;
				}
				else
				{
					ptd.expFlux += ptd.sunFlux * sunArea;
				}
			}
		}
		if (!(ptd.bodyFlux > 0.0) || part.ShieldedFromAirstream)
		{
			return;
		}
		double bodyArea = GetBodyArea(ptd);
		if (bodyArea > 0.0)
		{
			ptd.bodyFlux = UtilMath.Lerp(0.0, ptd.bodyFlux, densityThermalLerp) * ptd.absorbScalar;
			if (ptd.exposed)
			{
				double num6 = (Vector3.Dot(-vessel.upAxis, nVel) + 1f) * 0.5f;
				double num7 = Math.Min(bodyArea, part.skinExposedArea * num6);
				double num8 = Math.Min(bodyArea - num7, num2 * (1.0 - num6));
				ptd.expFlux += ptd.bodyFlux * num7;
				ptd.unexpFlux += ptd.bodyFlux * num8;
			}
			else
			{
				ptd.expFlux += ptd.bodyFlux * bodyArea;
			}
		}
	}

	public virtual void UpdateRadiation(PartThermalData ptd)
	{
		Part part = ptd.part;
		double skinTemperature = part.skinTemperature;
		double skinUnexposedTemperature = part.skinUnexposedTemperature;
		double num = part.radiativeArea * part.skinExposedAreaFrac;
		double num2 = part.radiativeArea * (1.0 - part.skinExposedAreaFrac);
		if (!part.ShieldedFromAirstream)
		{
			if (skinUnexposedTemperature > 0.0 && num2 > 0.0)
			{
				double brtUnexposed = ptd.brtUnexposed;
				brtUnexposed *= brtUnexposed;
				brtUnexposed *= brtUnexposed;
				skinUnexposedTemperature *= skinUnexposedTemperature;
				skinUnexposedTemperature *= skinUnexposedTemperature;
				ptd.unexpFlux -= (skinUnexposedTemperature - brtUnexposed) * cacheStefanBoltzmanConstant * ptd.emissScalar * num2;
			}
			if (skinTemperature > 0.0 && num > 0.0)
			{
				double brtExposed = ptd.brtExposed;
				brtExposed *= brtExposed;
				brtExposed *= brtExposed;
				skinTemperature *= skinTemperature;
				skinTemperature *= skinTemperature;
				ptd.expFlux -= (skinTemperature - brtExposed) * cacheStefanBoltzmanConstant * ptd.emissScalar * num;
			}
			ptd.radiationFlux += ptd.expFlux;
			ptd.unexpRadiationFlux += ptd.unexpFlux;
		}
	}

	public virtual double GetSunArea(PartThermalData ptd)
	{
		Part part = ptd.part;
		if (part.DragCubes.None)
		{
			return 0.0;
		}
		Vector3 dir = part.partTransform.InverseTransformDirection(sunVector);
		return (double)part.DragCubes.GetCubeAreaDir(dir) * ptd.sunAreaMultiplier;
	}

	public virtual double GetBodyArea(PartThermalData ptd)
	{
		Part part = ptd.part;
		if (part.DragCubes.None)
		{
			return 0.0;
		}
		Vector3 dir = part.partTransform.InverseTransformDirection(-vessel.upAxis);
		return (double)part.DragCubes.GetCubeAreaDir(dir) * ptd.bodyAreaMultiplier;
	}

	public virtual void SetSkinThermalMass(Part part)
	{
		double num = cacheStandardSpecificHeatCapacity * part.thermalMassModifier;
		part.skinThermalMass = Math.Max(0.1, Math.Min(0.001 * part.skinMassPerArea * part.skinThermalMassModifier * part.radiativeArea * num, (double)part.mass * num * 0.5));
		part.skinThermalMassRecip = 1.0 / part.skinThermalMass;
	}

	public void ApplyAeroDrag(Part part, Rigidbody rbPossible, ForceMode mode)
	{
		Vector3 zero = Vector3.zero;
		rbPossible.AddForceAtPosition(position: (!(rbPossible != part.rb) || !PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM) ? part.partTransform.TransformPoint(part.CoPOffset) : rbPossible.worldCenterOfMass, force: -part.dragVectorDir * part.dragScalar, mode: mode);
	}

	public void ApplyAeroLift(Part part, Rigidbody rbPossible, ForceMode mode)
	{
		Vector3 zero = Vector3.zero;
		zero = ((!(rbPossible != part.rb) || !PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM) ? part.partTransform.TransformPoint(part.CoLOffset) : rbPossible.worldCenterOfMass);
		Vector3 vector = part.transform.rotation * (part.bodyLiftScalar * part.DragCubes.LiftForce);
		vector = Vector3.ProjectOnPlane(vector, -part.dragVectorDir);
		part.bodyLiftLocalPosition = part.partTransform.InverseTransformPoint(zero);
		part.bodyLiftLocalVector = part.partTransform.InverseTransformDirection(vector);
		rbPossible.AddForceAtPosition(vector, zero, mode);
	}

	public virtual void UpdateAerodynamics(Part part)
	{
		Rigidbody rigidbody = part.Rigidbody;
		if ((object)rigidbody == null)
		{
			return;
		}
		bool flag = part.rb != null;
		bool flag2 = part.servoRb != null;
		part.aerodynamicArea = 0.0;
		part.exposedArea = CalculateAreaExposed(part);
		double num = 0.0;
		part.submergedDynamicPressurekPa = 0.0;
		part.dynamicPressurekPa = num;
		if (flag && part.angularDragByFI)
		{
			part.rb.angularDrag = 0f;
		}
		if (flag2 && part.angularDragByFI)
		{
			part.servoRb.angularDrag = 0f;
		}
		part.dragVector = rigidbody.velocity + Krakensbane.GetFrameVelocity();
		part.dragVectorSqrMag = part.dragVector.sqrMagnitude;
		bool flag3 = false;
		if (part.dragVectorSqrMag != 0f)
		{
			flag3 = true;
			part.dragVectorMag = Mathf.Sqrt(part.dragVectorSqrMag);
			part.dragVectorDir = part.dragVector / part.dragVectorMag;
			part.dragVectorDirLocal = -part.partTransform.InverseTransformDirection(part.dragVectorDir);
			part.dragScalar = 0f;
		}
		else
		{
			part.dragVectorMag = 0f;
			part.dragVectorDir = Vector3.zero;
			part.dragVectorDirLocal = Vector3.zero;
			part.dragScalar = 0f;
		}
		double submergedPortion = part.submergedPortion;
		if (part.ShieldedFromAirstream || (!(part.atmDensity > 0.0) && !(submergedPortion > 0.0)))
		{
			return;
		}
		if (!part.DragCubes.None)
		{
			part.DragCubes.SetDrag(part.dragVectorDirLocal, (float)mach);
		}
		part.aerodynamicArea = CalculateAerodynamicArea(part);
		if (!(cacheApplyDrag && flag3) || (!(rigidbody == part.rb) && !cacheApplyDragToNonPhysicsParts))
		{
			return;
		}
		double num2 = 1.0;
		bool flag4 = false;
		double num4;
		if (currentMainBody.ocean)
		{
			if (submergedPortion > 0.0)
			{
				flag4 = true;
				double num3 = currentMainBody.oceanDensity * 1000.0;
				if (submergedPortion >= 1.0)
				{
					num2 = 0.0;
					part.submergedDynamicPressurekPa = num3;
					num4 = num3 * cacheBuoyancyWaterAngularDragScalar * (double)part.waterAngularDragMultiplier;
				}
				else
				{
					num2 = 1.0 - submergedPortion;
					part.submergedDynamicPressurekPa = num3;
					num4 = part.staticPressureAtm * num2 + submergedPortion * num3 * cacheBuoyancyWaterAngularDragScalar * (double)part.waterAngularDragMultiplier;
				}
			}
			else
			{
				part.dynamicPressurekPa = part.atmDensity;
				num4 = part.staticPressureAtm;
			}
		}
		else
		{
			part.dynamicPressurekPa = part.atmDensity;
			num4 = part.staticPressureAtm;
		}
		double num5 = 0.0005 * (double)part.dragVectorSqrMag;
		part.dynamicPressurekPa *= num5;
		part.submergedDynamicPressurekPa *= num5;
		if (flag && part.angularDragByFI)
		{
			if (flag4)
			{
				num4 += part.dynamicPressurekPa * 0.009869232667160128 * num2;
				num4 += part.submergedDynamicPressurekPa * 0.009869232667160128 * cacheBuoyancyWaterAngularDragScalar * (double)part.waterAngularDragMultiplier * submergedPortion;
			}
			else
			{
				num4 = part.dynamicPressurekPa * 0.009869232667160128;
			}
			if (num4 <= 0.0)
			{
				num4 = 0.0;
			}
			part.rb.angularDrag = part.angularDrag * (float)num4 * cacheAngularDragMultiplier;
			if (flag2)
			{
				part.servoRb.angularDrag = part.rb.angularDrag;
			}
		}
		double num6 = CalculateDragValue(part) * pseudoReDragMult;
		if (!double.IsNaN(num6) && num6 != 0.0)
		{
			part.dragScalar = (float)(part.dynamicPressurekPa * num6 * num2) * cacheDragMultiplier;
			ApplyAeroDrag(part, rigidbody, cacheDragUsesAcceleration ? ForceMode.Acceleration : ForceMode.Force);
		}
		else
		{
			part.dragScalar = 0f;
		}
		if (!part.hasLiftModule && (!part.bodyLiftOnlyUnattachedLiftActual || part.bodyLiftOnlyProvider == null || !part.bodyLiftOnlyProvider.IsLifting))
		{
			part.bodyLiftScalar = (float)(flag4 ? (part.dynamicPressurekPa * num2 + part.submergedDynamicPressurekPa * part.submergedLiftScalar * submergedPortion) : part.dynamicPressurekPa) * part.bodyLiftMultiplier * cacheBodyLiftMultiplier * PhysicsGlobals.BodyLiftCurve.liftMachCurve.Evaluate((float)mach);
			if (part.bodyLiftScalar != 0f && part.DragCubes.LiftForce != Vector3.zero && !part.DragCubes.LiftForce.IsInvalid())
			{
				ApplyAeroLift(part, rigidbody, cacheDragUsesAcceleration ? ForceMode.Acceleration : ForceMode.Force);
			}
		}
	}

	public virtual double CalculateDragValue(Part part)
	{
		switch (part.dragModel)
		{
		case Part.DragModel.DEFAULT:
			return CalculateDragValue_Cube(part);
		case Part.DragModel.CONIC:
			return CalculateDragValue_Conic(part);
		case Part.DragModel.CYLINDRICAL:
			return CalculateDragValue_Cylindrical(part);
		case Part.DragModel.SPHERICAL:
			return CalculateDragValue_Spherical(part);
		case Part.DragModel.CUBE:
			if (!cacheDragCubesUseSpherical && !part.DragCubes.None)
			{
				return CalculateDragValue_Cube(part);
			}
			return CalculateDragValue_Spherical(part);
		default:
			return 0.0;
		}
	}

	public virtual double CalculateDragValue_Spherical(Part part)
	{
		return part.maximum_drag;
	}

	public virtual double CalculateDragValue_Cylindrical(Part part)
	{
		return Mathf.Lerp(part.minimum_drag, part.maximum_drag, Mathf.Abs(Vector3.Dot(part.partTransform.TransformDirection(part.dragReferenceVector), part.dragVectorDir)));
	}

	public virtual double CalculateDragValue_Conic(Part part)
	{
		return Mathf.Lerp(part.minimum_drag, part.maximum_drag, Vector3.Angle(part.partTransform.TransformDirection(part.dragReferenceVector), part.dragVectorDir) / 180f);
	}

	public virtual double CalculateDragValue_Cube(Part part)
	{
		return part.DragCubes.AreaDrag * cacheDragCubeMultiplier;
	}

	public virtual double CalculateAerodynamicArea(Part part)
	{
		if (!part.DragCubes.None)
		{
			return part.DragCubes.Area;
		}
		switch (part.dragModel)
		{
		case Part.DragModel.CONIC:
			return part.maximum_drag;
		case Part.DragModel.CYLINDRICAL:
			return part.maximum_drag;
		case Part.DragModel.SPHERICAL:
			return part.maximum_drag;
		case Part.DragModel.DEFAULT:
		case Part.DragModel.CUBE:
			if (!cacheDragCubesUseSpherical && !part.DragCubes.None)
			{
				return part.DragCubes.Area;
			}
			return part.maximum_drag;
		default:
			return part.maximum_drag;
		}
	}

	public virtual double CalculateAreaExposed(Part part)
	{
		if (!part.DragCubes.None)
		{
			return part.DragCubes.ExposedArea;
		}
		switch (part.dragModel)
		{
		case Part.DragModel.CONIC:
			return part.maximum_drag;
		case Part.DragModel.CYLINDRICAL:
			return part.maximum_drag;
		case Part.DragModel.SPHERICAL:
			return part.maximum_drag;
		case Part.DragModel.DEFAULT:
		case Part.DragModel.CUBE:
			if (!cacheDragCubesUseSpherical && !part.DragCubes.None)
			{
				return part.DragCubes.ExposedArea;
			}
			return part.maximum_drag;
		default:
			return part.maximum_drag;
		}
	}

	public virtual double CalculateAreaRadiative(Part part)
	{
		if (!part.DragCubes.None)
		{
			return part.DragCubes.PostOcclusionArea;
		}
		switch (part.dragModel)
		{
		case Part.DragModel.CONIC:
			return part.maximum_drag;
		case Part.DragModel.CYLINDRICAL:
			return part.maximum_drag;
		case Part.DragModel.SPHERICAL:
			return part.maximum_drag;
		case Part.DragModel.DEFAULT:
		case Part.DragModel.CUBE:
			if (!cacheDragCubesUseSpherical && !part.DragCubes.None)
			{
				return part.DragCubes.PostOcclusionArea;
			}
			return part.maximum_drag;
		default:
			return part.maximum_drag;
		}
	}

	public void DragCubeSetupAndPartAeroStats(Vessel v)
	{
		for (int i = 0; i < partCount; i++)
		{
			if (!v.parts[i].DragCubes.None)
			{
				v.parts[i].DragCubes.SetDragWeights();
			}
		}
		for (int j = 0; j < partCount; j++)
		{
			if (!v.parts[j].DragCubes.None)
			{
				v.parts[j].DragCubes.SetPartOcclusion();
			}
		}
		double magnitude = vessel.gravityTrue.magnitude;
		for (int k = 0; k < partCount; k++)
		{
			Part part = vessel.parts[k];
			part.radiativeArea = CalculateAreaRadiative(v.parts[k]);
			double altitudeAtPos = FlightGlobals.getAltitudeAtPos((Vector3d)part.partTransform.position, currentMainBody);
			part.staticPressureAtm = currentMainBody.GetPressure(altitudeAtPos);
			part.atmDensity = density;
			if (currentMainBody.ocean && altitudeAtPos < 0.0)
			{
				part.staticPressureAtm += magnitude * (0.0 - altitudeAtPos) * currentMainBody.oceanDensity;
			}
			part.staticPressureAtm *= 0.009869232667160128;
			part.machNumber = mach;
		}
	}

	public virtual void UpdateOcclusion(bool all)
	{
		if (all)
		{
			UpdateOcclusionConvection();
			UpdateOcclusionSolar();
			UpdateOcclusionBody();
			needOcclusion = false;
			return;
		}
		switch (occlusionCounter)
		{
		default:
			UpdateOcclusionBody();
			occlusionCounter = 0;
			break;
		case 1:
			UpdateOcclusionSolar();
			occlusionCounter++;
			break;
		case 0:
			UpdateOcclusionConvection();
			occlusionCounter++;
			break;
		}
	}

	public void UpdateOcclusionConvection()
	{
		bool flag = true;
		if (mach > 1.0)
		{
			flag = false;
		}
		double num;
		if (flag)
		{
			if (wasMachConvectionEnabled)
			{
				for (int i = 0; i < partThermalDataCount; i++)
				{
					PartThermalData partThermalData = partThermalDataList[i];
					PartThermalData partThermalData2 = partThermalDataList[i];
					PartThermalData partThermalData3 = partThermalDataList[i];
					num = 1.0;
					partThermalData3.convectionCoeffMultiplier = 1.0;
					double convectionAreaMultiplier = num;
					num = 1.0;
					partThermalData2.convectionAreaMultiplier = convectionAreaMultiplier;
					partThermalData.convectionTempMultiplier = num;
				}
				wasMachConvectionEnabled = false;
			}
			return;
		}
		bool flag2 = false;
		if (partThermalDataCount != partThermalDataList.Count)
		{
			recreateThermalGraph = true;
			CheckThermalGraph();
		}
		int num2 = partThermalDataCount - 1;
		int num3 = partThermalDataCount;
		while (num3-- > 0)
		{
			occlusionConv[num3].Update(nVel);
			if (num3 < num2 && !flag2 && occlusionConv[num3 + 1].maximumDot < occlusionConv[num3].maximumDot)
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			occlusionConv.Sort();
		}
		double num4 = Math.Sqrt(mach);
		double num5 = Math.Asin(1.0 / num4);
		double num6 = 0.7957 * (1.0 - 1.0 / (mach * num4));
		OcclusionData occlusionData = occlusionConv[num2];
		PartThermalData ptd = occlusionData.ptd;
		PartThermalData ptd2 = occlusionData.ptd;
		PartThermalData ptd3 = occlusionData.ptd;
		num = 1.0;
		ptd3.convectionCoeffMultiplier = 1.0;
		double convectionAreaMultiplier2 = num;
		num = 1.0;
		ptd2.convectionAreaMultiplier = convectionAreaMultiplier2;
		ptd.convectionTempMultiplier = num;
		occlusionData.convCone.Setup(occlusionData, num4, num5, num6);
		occludersConvection[0] = occlusionData.convCone;
		occludersConvectionCount = 1;
		FXCamera.Instance.ApplyObliqueness((float)occludersConvection[0].shockAngle);
		int index = num2;
		while (index-- > 0)
		{
			occlusionData = occlusionConv[index];
			PartThermalData ptd4 = occlusionData.ptd;
			PartThermalData ptd5 = occlusionData.ptd;
			PartThermalData ptd6 = occlusionData.ptd;
			num = 1.0;
			ptd6.convectionCoeffMultiplier = 1.0;
			double convectionAreaMultiplier3 = num;
			num = 1.0;
			ptd5.convectionAreaMultiplier = convectionAreaMultiplier3;
			ptd4.convectionTempMultiplier = num;
			for (int j = 0; j < occludersConvectionCount; j++)
			{
				double shockStats = occlusionData.GetShockStats(occludersConvection[j], ref occlusionData.ptd.convectionTempMultiplier, ref occlusionData.ptd.convectionCoeffMultiplier);
				occlusionData.ptd.convectionAreaMultiplier *= shockStats;
				if (!(occlusionData.ptd.convectionAreaMultiplier > 0.001))
				{
					occlusionData.ptd.convectionAreaMultiplier = 0.0;
					break;
				}
			}
			if (occlusionData.ptd.convectionAreaMultiplier > 0.0)
			{
				double num7 = 1.0;
				occlusionData.convCone.Setup(occlusionData, num4 * num7, num5 * num7, num6 * num7);
				occludersConvection[occludersConvectionCount] = occlusionData.convCone;
				occludersConvectionCount++;
			}
		}
	}

	public void UpdateOcclusionSolar()
	{
		bool flag = false;
		if (partThermalDataCount != partThermalDataList.Count)
		{
			recreateThermalGraph = true;
			CheckThermalGraph();
		}
		int num = partThermalDataCount - 1;
		int num2 = partThermalDataCount;
		while (num2-- > 0)
		{
			occlusionSun[num2].Update(sunVector);
			if (num2 < num && !flag && occlusionSun[num2 + 1].maximumDot < occlusionSun[num2].maximumDot)
			{
				flag = true;
			}
		}
		if (flag)
		{
			occlusionSun.Sort();
		}
		OcclusionData occlusionData = occlusionSun[num];
		occlusionData.ptd.sunAreaMultiplier = 1.0;
		occlusionData.sunCyl.Setup(occlusionData);
		occludersSun[0] = occlusionData.sunCyl;
		occludersSunCount = 1;
		int index = num;
		while (index-- > 0)
		{
			occlusionData = occlusionSun[index];
			occlusionData.ptd.sunAreaMultiplier = 1.0;
			for (int i = 0; i < occludersSunCount; i++)
			{
				occlusionData.ptd.sunAreaMultiplier -= occlusionData.GetCylinderOcclusion(occludersSun[i]);
				if (!(occlusionData.ptd.sunAreaMultiplier > 0.001))
				{
					occlusionData.ptd.sunAreaMultiplier = 0.0;
					break;
				}
			}
			if (occlusionData.ptd.sunAreaMultiplier > 0.0)
			{
				occlusionData.sunCyl.Setup(occlusionData);
				occludersSun[occludersSunCount] = occlusionData.sunCyl;
				occludersSunCount++;
			}
		}
	}

	public void UpdateOcclusionBody()
	{
		Vector3 velocity = -vessel.upAxis;
		bool flag = false;
		if (partThermalDataCount != partThermalDataList.Count)
		{
			recreateThermalGraph = true;
			CheckThermalGraph();
		}
		int num = partThermalDataCount - 1;
		int num2 = partThermalDataCount;
		while (num2-- > 0)
		{
			occlusionBody[num2].Update(velocity);
			if (num2 < num && !flag && occlusionBody[num2 + 1].maximumDot < occlusionBody[num2].maximumDot)
			{
				flag = true;
			}
		}
		if (flag)
		{
			occlusionBody.Sort();
		}
		OcclusionData occlusionData = occlusionBody[num];
		occlusionData.ptd.bodyAreaMultiplier = 1.0;
		occlusionData.bodyCyl.Setup(occlusionData);
		occludersBody[0] = occlusionData.bodyCyl;
		occludersBodyCount = 1;
		int index = num;
		while (index-- > 0)
		{
			occlusionData = occlusionBody[index];
			occlusionData.ptd.bodyAreaMultiplier = 1.0;
			for (int i = 0; i < occludersBodyCount; i++)
			{
				occlusionData.ptd.bodyAreaMultiplier -= occlusionData.GetCylinderOcclusion(occludersBody[i]);
				if (!(occlusionData.ptd.bodyAreaMultiplier > 0.001))
				{
					occlusionData.ptd.bodyAreaMultiplier = 0.0;
					break;
				}
			}
			if (occlusionData.ptd.bodyAreaMultiplier > 0.0)
			{
				occlusionData.bodyCyl.Setup(occlusionData);
				occludersBody[occludersBodyCount] = occlusionData.bodyCyl;
				occludersBodyCount++;
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.gray;
		Gizmos.DrawSphere(CoM, 0.1f);
	}
}
