using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightIntegrator : VesselModule
{
	public static FlightIntegrator ActiveVesselFI;

	public bool isRunning;

	public bool isKerbal;

	private float dragChangeSmoothness;

	protected Transform integratorTransform;

	protected Part partRef;

	protected CelestialBody currentMainBody;

	public double altitude;

	public Vector3 CoM;

	public Vector3 Vel;

	public Vector3 nVel;

	public Vector3 VelUnsmoothed;

	public Vector3d CoMd;

	public Vector3d Veld;

	public Vector3d rbVeld;

	protected Vector3[] VelSmoother;

	protected int VelIndex;

	protected static int VelSmoothLen;

	protected double VelSmoothLenRecip;

	protected bool lastVelProvisional;

	protected bool VelSpiking;

	protected Vector3 lastVel;

	protected int lastVelIndex;

	protected double maxVelDeltaSqr;

	public double spd;

	public double density;

	public double mach;

	public double staticPressurekPa;

	public double staticPressureAtm;

	public double dynamicPressurekPa;

	public double atmosphericTemperature;

	public double externalTemperature;

	protected const double KPA2ATM = 0.009869232667160128;

	public Vector3 GraviticAcceleration;

	public double timeSinceLastUpdate;

	public bool firstFrame;

	public bool needOcclusion;

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

	protected double densityThermalLerp;

	public double pseudoReynolds;

	public double pseudoReLerpTimeMult;

	public double pseudoReDragMult;

	protected int partCount;

	protected bool setupRun;

	private RaycastHit sunBodyFluxHit;

	private float cacheDragCubeMultiplier;

	private bool cacheDragCubesUseSpherical;

	private bool cacheApplyDrag;

	private bool cacheApplyDragToNonPhysicsParts;

	private bool cacheDragUsesAcceleration;

	private float cacheDragMultiplier;

	private double cacheBuoyancyWaterAngularDragScalar;

	private float cacheAngularDragMultiplier;

	private float cacheBodyLiftMultiplier;

	private double cacheRadiationFactor;

	private double cacheSpaceTemperature;

	private double cacheFullConvectionAreaMin;

	private double cacheFullToCrossSectionLerpStart;

	private double cacheFullToCrossSectionLerpEnd;

	private double cacheStefanBoltzmanConstant;

	private double cacheSkinSkinConductionFactor;

	private double cacheConductionFactor;

	private double cacheTurbulentConvectionStart;

	private double cacheTurbulentConvectionEnd;

	private double cacheTurbulentConvectionMult;

	private double cacheStandardSpecificHeatCapacity;

	protected static int sunLayerMask;

	protected bool recreateThermalGraph;

	public List<PartThermalData> partThermalDataList;

	public List<PartThermalData> partThermalDataListSkin;

	protected int partThermalDataCount;

	public List<CompoundPart> compoundParts;

	public int compoundPartsTimer;

	private List<OcclusionData> occlusionConv;

	private List<OcclusionData> occlusionSun;

	private List<OcclusionData> occlusionBody;

	protected List<IAnalyticOverheatModule> overheatModules;

	protected List<IAnalyticPreview> previewModules;

	public OcclusionCone[] occludersConvection;

	public int occludersConvectionCount;

	public OcclusionCylinder[] occludersSun;

	public int occludersSunCount;

	public OcclusionCylinder[] occludersBody;

	public int occludersBodyCount;

	public bool isAnalytical;

	protected double fDeltaTime;

	protected double fDeltaTimeRecip;

	protected double deltaTime;

	protected double fTimeSinceThermo;

	protected double fTimeSinceThermoRecip;

	protected double timeFactor;

	protected double passesRecip;

	public int passes;

	protected bool wasMachConvectionEnabled;

	private int occlusionCounter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightIntegrator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightIntegrator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ShouldBeActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoadVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUnloadVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void HookVesselEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UnhookVesselEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartEvent(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnVesselEvent(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartEventTargetAction(GameEvents.HostTargetAction<Part, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartEventFromToAction(GameEvents.FromToAction<Part, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateMassStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SmoothVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ThermoPrecalculate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetPhysicslessChildMass(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Integrate(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void IntegratePhysicalObjects(List<physicalObject> pObjs, double atmDensity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculatePressure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateSunBodyFlux()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateDensityThermalLerp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateBackgroundRadiationTemperature(double ambientTemp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateConstantsVacuum()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CalculateConstantsAtmosphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateShockTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double CalculateAtmosphericDensity(double pres, double temp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalcConvectiveCoefficient(Vessel.Situations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateConvectiveCoefficient()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateConvectiveCoefficient(Vessel.Situations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateConvectiveCoefficientNewtonian()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateConvectiveCoefficientMach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CheckThermalGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateThermodynamics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ThermalIntegrationPass(bool averageWithPrevious)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateAnalyticTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PrecalcConduction(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateConduction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UnifySkinTemp(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateThermalGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateCompoundParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetSkinProperties(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PrecalcConvection(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateConvection(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PrecalcRadiation(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateRadiation(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetSunArea(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetBodyArea(PartThermalData ptd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetSkinThermalMass(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ApplyAeroDrag(Part part, Rigidbody rbPossible, ForceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ApplyAeroLift(Part part, Rigidbody rbPossible, ForceMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateAerodynamics(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateDragValue(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateDragValue_Spherical(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateDragValue_Cylindrical(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateDragValue_Conic(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateDragValue_Cube(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateAerodynamicArea(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateAreaExposed(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double CalculateAreaRadiative(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DragCubeSetupAndPartAeroStats(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateOcclusion(bool all)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateOcclusionConvection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateOcclusionSolar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateOcclusionBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDrawGizmosSelected()
	{
		throw null;
	}
}
