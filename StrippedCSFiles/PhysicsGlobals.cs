using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhysicsGlobals : MonoBehaviour
{
	public struct SurfaceCurvesList
	{
		public FloatCurve dragCurveTail;

		public FloatCurve dragCurveSurface;

		public FloatCurve dragCurveMultiplier;

		public FloatCurve dragCurveTip;
	}

	[Serializable]
	public class LiftingSurfaceCurve
	{
		public string name;

		public FloatCurve liftCurve;

		public FloatCurve liftMachCurve;

		public FloatCurve dragCurve;

		public FloatCurve dragMachCurve;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LiftingSurfaceCurve()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}
	}

	private static PhysicsGlobals instance;

	[SerializeField]
	private string physicsDatabaseFilename;

	[SerializeField]
	private bool aeroDataDisplay;

	[SerializeField]
	private bool aeroForceDisplay;

	[SerializeField]
	private bool aeroGUIDisplay;

	[SerializeField]
	private float aeroForceDisplayScale;

	[SerializeField]
	private bool autoStrutDisplay;

	[SerializeField]
	private bool roboticJointDataDisplay;

	private static double idealGasConstant;

	private static double boltzmannConstant;

	private static double stefanBoltzmanConstant;

	private static double avogadroConstant;

	public const double EngineDefaultAtmDensity = 1.225000023841858;

	public static double GraviticForceMultiplier;

	public static double GravitationalAcceleration;

	[SerializeField]
	private bool thermoGUIDisplay;

	[SerializeField]
	private double thermalMaxIntegrationWarp;

	[SerializeField]
	private double spaceTemperature;

	[SerializeField]
	private double solarLuminosity;

	[SerializeField]
	private double solarLuminosityAtHome;

	[SerializeField]
	private double solarInsolationAtHome;

	[SerializeField]
	private double machConvectionDensityExponent;

	[SerializeField]
	private double machConvectionVelocityExponent;

	[SerializeField]
	private double newtonianDensityExponent;

	[SerializeField]
	private double newtonianConvectionFactorBase;

	[SerializeField]
	private double newtonianConvectionFactorTotal;

	[SerializeField]
	private double newtonianVelocityExponent;

	[SerializeField]
	private Gradient blackBodyRadiation;

	[SerializeField]
	private float blackBodyRadiationMin;

	[SerializeField]
	private float blackBodyRadiationMax;

	[SerializeField]
	private float blackBodyRadiationAlphaMult;

	[SerializeField]
	private bool thermalDataDisplay;

	[SerializeField]
	private bool thermalColorsDebug;

	[SerializeField]
	private float temperatureGaugeThreshold;

	[SerializeField]
	private float temperatureGaugeHighlightThreshold;

	[SerializeField]
	private double radiationFactor;

	[SerializeField]
	private double machConvectionFactor;

	[SerializeField]
	private double conductionFactor;

	[SerializeField]
	private double convectionFactorSplashed;

	[SerializeField]
	private double fullToCrossSectionLerpStart;

	[SerializeField]
	private double newtonianMachTempLerpEndMach;

	[SerializeField]
	private double newtonianMachTempLerpStartMach;

	[SerializeField]
	private double newtonianMachTempLerpExponent;

	[SerializeField]
	private double machTemperatureScalar;

	[SerializeField]
	private double machTemperatureVelocityExponent;

	[SerializeField]
	private double turbulentConvectionStart;

	[SerializeField]
	private double turbulentConvectionEnd;

	[SerializeField]
	private double turbulentConvectionMult;

	[SerializeField]
	private double fullConvectionAreaMin;

	[SerializeField]
	private double fullToCrossSectionLerpEnd;

	[SerializeField]
	private double internalHeatProductionFactor;

	[SerializeField]
	private double newtonianTemperatureFactor;

	[SerializeField]
	private double shieldedConductionFactor;

	[SerializeField]
	private bool thermalRadiationEnabled;

	[SerializeField]
	private bool thermalConductionEnabled;

	[SerializeField]
	private bool thermalConvectionEnabled;

	[SerializeField]
	private double standardSpecificHeatCapacity;

	[SerializeField]
	private double skinSkinConductionFactor;

	[SerializeField]
	private double skinInternalConductionFactor;

	[SerializeField]
	private double thermalIntegrationMinStep;

	[SerializeField]
	private double thermalIntegrationMaxTimeOnePass;

	[SerializeField]
	private bool thermalIntegrationAlwaysRK2;

	[SerializeField]
	private double occlusionMinStep;

	[SerializeField]
	private int thermalIntegrationHighMaxPasses;

	[SerializeField]
	private int thermalIntegrationHighMinPasses;

	[SerializeField]
	private double thermalConvergenceFactor;

	[SerializeField]
	private double analyticLerpRateSkin;

	[SerializeField]
	private double analyticLerpRateInternal;

	[SerializeField]
	private double analyticConvectionSensitivityBase;

	[SerializeField]
	private double analyticConvectionSensitivityFinal;

	private static int temperaturePropertyID;

	[SerializeField]
	private double buoyancyScalar;

	[SerializeField]
	private bool buoyancyUseCoBOffset;

	[SerializeField]
	private bool buoyancyApplyForceOnDie;

	[SerializeField]
	private float buoyancyForceOffsetLerp;

	[SerializeField]
	private double buoyancyWaterDragScalar;

	[SerializeField]
	private double buoyancyWaterDragScalarEnd;

	[SerializeField]
	private double buoyancyWaterDragScalarLerp;

	[SerializeField]
	private double buoyancyWaterDragScalarLerpDotMultBase;

	[SerializeField]
	private double buoyancyWaterDragScalarLerpDotMult;

	[SerializeField]
	private double buoyancyWaterLiftScalarEnd;

	[SerializeField]
	private double buoyancyWaterDragMinVel;

	[SerializeField]
	private double buoyancyWaterDragMinVelMult;

	[SerializeField]
	private double buoyancyWaterDragMinVelMultCOBOff;

	[SerializeField]
	private double buoyancyWaterDragPartVelGreaterVesselMult;

	[SerializeField]
	private double buoyancyWaterDragTimer;

	[SerializeField]
	private double buoyancyWaterDragMultMinForMinDot;

	[SerializeField]
	private double buoyancyWaterAngularDragScalar;

	[SerializeField]
	private double buoyancyAngularDragMinControlSqrMag;

	[SerializeField]
	private float buoyancyWaterAngularDragSlow;

	[SerializeField]
	private float buoyancyWaterDragSlow;

	[SerializeField]
	private double buoyancyWaterDragExtraRBDragAboveDot;

	[SerializeField]
	private double buoyancyScaleAboveDepth;

	[SerializeField]
	private double buoyancyDefaultVolume;

	[SerializeField]
	private float buoyancyMinCrashMult;

	[SerializeField]
	private float buoyancyCrashToleranceMult;

	[SerializeField]
	private double buoyancyRange;

	[SerializeField]
	private float buoyancyKerbals;

	[SerializeField]
	private double buoyancyKerbalsRagdoll;

	[SerializeField]
	private float cameraDepthToUnlock;

	[SerializeField]
	private float jointBreakForceFactor;

	[SerializeField]
	private float jointBreakTorqueFactor;

	[SerializeField]
	private float jointForce;

	[SerializeField]
	private float rigidJointBreakForceFactor;

	[SerializeField]
	private float rigidJointBreakTorqueFactor;

	[SerializeField]
	private float maxAngularVelocity;

	[SerializeField]
	private float buildingImpactDamageMaxVelocityMult;

	[SerializeField]
	private bool buildingImpactDamageUseMomentum;

	[SerializeField]
	private float buildingEasingInvulnerableTime;

	[SerializeField]
	private float buildingImpactDamageMinDamageFraction;

	[SerializeField]
	private int orbitDriftFramesToWait;

	[SerializeField]
	private double orbitDriftSqrThreshold;

	[SerializeField]
	private double orbitDriftAltThreshold;

	[SerializeField]
	private float partMassMin;

	[SerializeField]
	private float partRBMassMin;

	[SerializeField]
	private double constructionWeightLimit;

	[SerializeField]
	private double constructionWeightLimitPerKerbalCombine;

	[SerializeField]
	private bool applyDragToNonPhysicsParts;

	[SerializeField]
	private bool applyDragToNonPhysicsPartsAtParentCoM;

	[SerializeField]
	private float dragMultiplier;

	[SerializeField]
	private float bodyLiftMultiplier;

	[SerializeField]
	private float dragCubeMultiplier;

	[SerializeField]
	private float angularDragMultiplier;

	[SerializeField]
	private string kerbalEVADragCubeString;

	[SerializeField]
	private float kerbalCrewMass;

	[SerializeField]
	private float perSeatReduction;

	[SerializeField]
	private float perCommandSeatReduction;

	[SerializeField]
	private double kerbalGOffset;

	[SerializeField]
	private double kerbalGPower;

	[SerializeField]
	private double kerbalGDecayPower;

	[SerializeField]
	private double kerbalGBraveMult;

	[SerializeField]
	private double kerbalGBadMult;

	[SerializeField]
	private double kerbalGClamp;

	[SerializeField]
	private double kerbalGThresholdWarn;

	[SerializeField]
	private double kerbalGThresholdLOC;

	[SerializeField]
	private double kerbalGLOCTimeMult;

	[SerializeField]
	private double kerbalGLOCMaxTimeIncrement;

	[SerializeField]
	private double kerbalGLOCBaseTime;

	[SerializeField]
	private bool kerbalGClampGExperienced;

	[SerializeField]
	private PhysicMaterial kerbalEVAPhysicMaterial;

	[SerializeField]
	private float kerbalEVADynamicFriction;

	[SerializeField]
	private float kerbalEVAStaticFriction;

	[SerializeField]
	private float kerbalEVABounciness;

	[SerializeField]
	private PhysicMaterialCombine kerbalEVAFrictionCombine;

	[SerializeField]
	private PhysicMaterialCombine kerbalEVABounceCombine;

	[SerializeField]
	private double commNetQTimesVelForBlackoutMin;

	[SerializeField]
	private double commNetQTimesVelForBlackoutMax;

	[SerializeField]
	private double commNetTempForBlackout;

	[SerializeField]
	private double commNetDensityForBlackout;

	[SerializeField]
	private double commNetDotForBlackoutMin;

	[SerializeField]
	private double commNetDotForBlackoutMax;

	[SerializeField]
	private double commNetBlackoutThreshold;

	[SerializeField]
	private bool applyDrag;

	[SerializeField]
	private bool dragUsesAcceleration;

	[SerializeField]
	private bool dragUsesSpherical;

	[SerializeField]
	private FloatCurve dragCurveTip;

	[SerializeField]
	private FloatCurve dragCurveSurface;

	[SerializeField]
	private FloatCurve dragCurveTail;

	[SerializeField]
	private FloatCurve dragCurveMultiplier;

	[SerializeField]
	private FloatCurve dragCurveCd;

	[SerializeField]
	private FloatCurve dragCurveCdPower;

	[SerializeField]
	private FloatCurve dragCurvePseudoReynolds;

	public static SurfaceCurvesList SurfaceCurves;

	[SerializeField]
	private float liftMultiplier;

	[SerializeField]
	private float liftDragMultiplier;

	[SerializeField]
	private Dictionary<string, LiftingSurfaceCurve> liftingSurfaceCurves;

	[SerializeField]
	private LiftingSurfaceCurve bodyLiftCurve;

	[SerializeField]
	private float aeroFXStartThermalFX;

	[SerializeField]
	private float aeroFXFullThermalFX;

	[SerializeField]
	private double aeroFXVelocityExponent;

	[SerializeField]
	private double aeroFXDensityExponent1;

	[SerializeField]
	private double aeroFXDensityScalar1;

	[SerializeField]
	private double aeroFXDensityExponent2;

	[SerializeField]
	private double aeroFXDensityScalar2;

	[SerializeField]
	private float aeroFXMachFXFadeStart;

	[SerializeField]
	private float aeroFXMachFXFadeEnd;

	[SerializeField]
	private double aeroFXDensityFadeStart;

	[SerializeField]
	private string autoStrutTechRequired;

	[SerializeField]
	private Part.ShowRigidAttachmentOption showRigidJointTweakable;

	[SerializeField]
	private float stagingCooldownTimer;

	[SerializeField]
	private VesselTargetModes celestialBodyTargetingMode;

	public VesselRanges VesselRangesDefault;

	[Range(0f, 25f)]
	[SerializeField]
	private float debugGizmosMach;

	private static int debugGizmosCount;

	private static float gizmosYScale;

	private static float gizmosXScale;

	public static PhysicsGlobals Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string PhysicsDatabaseFilename
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

	public static bool AeroDataDisplay
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

	public static bool AeroForceDisplay
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

	public static bool AeroGUIDisplay
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

	public static float AeroForceDisplayScale
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

	public static bool AutoStrutDisplay
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

	public static bool RoboticJointDataDisplay
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

	public static double IdealGasConstant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double BoltzmannConstant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double StefanBoltzmanConstant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double AvogadroConstant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double KpaToAtmospheres
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool ThermoGUIDisplay
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

	public static double ThermalMaxIntegrationWarp
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

	public static double SpaceTemperature
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

	public static double SolarLuminosity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double SolarLuminosityAtHome
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

	public static double SolarInsolationAtHome
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

	public static double MachConvectionDensityExponent
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

	public static double MachConvectionVelocityExponent
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

	public static double NewtonianDensityExponent
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

	public static double NewtonianConvectionFactorBase
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

	public static double NewtonianConvectionFactorTotal
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

	public static double NewtonianVelocityExponent
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

	public static Gradient BlackBodyRadiation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float BlackBodyRadiationMin
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

	public static float BlackBodyRadiationMax
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

	public static float BlackBodyRadiationAlphaMult
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

	public static bool ThermalDataDisplay
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

	public static bool ThermalColorsDebug
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

	public static float TemperatureGaugeThreshold
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

	public static float TemperatureGaugeHighlightThreshold
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

	public static double RadiationFactor
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

	public static double MachConvectionFactor
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

	public static double ConductionFactor
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

	public static double ConvectionFactorSplashed
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

	public static double FullToCrossSectionLerpStart
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

	public static double NewtonianMachTempLerpEndMach
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

	public static double NewtonianMachTempLerpStartMach
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

	public static double NewtonianMachTempLerpExponent
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

	public static double MachTemperatureScalar
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

	public static double MachTemperatureVelocityExponent
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

	public static double TurbulentConvectionStart
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

	public static double TurbulentConvectionEnd
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

	public static double TurbulentConvectionMult
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

	public static double FullConvectionAreaMin
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

	public static double FullToCrossSectionLerpEnd
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

	public static double InternalHeatProductionFactor
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

	public static double NewtonianTemperatureFactor
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

	public static double ShieldedConductionFactor
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

	public static bool ThermalRadiationEnabled
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

	public static bool ThermalConductionEnabled
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

	public static bool ThermalConvectionEnabled
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

	public static double StandardSpecificHeatCapacity
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

	public static double SkinSkinConductionFactor
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

	public static double SkinInternalConductionFactor
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

	public static double ThermalIntegrationMinStep
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

	public static double ThermalIntegrationMaxTimeOnePass
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

	public static bool ThermalIntegrationAlwaysRK2
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

	public static double OcclusionMinStep
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

	public static int ThermalIntegrationHighMaxPasses
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

	public static int ThermalIntegrationHighMinPasses
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

	public static double ThermalConvergenceFactor
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

	public static double AnalyticLerpRateSkin
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

	public static double AnalyticLerpRateInternal
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

	public static double AnalyticConvectionSensitivityBase
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

	public static double AnalyticConvectionSensitivityFinal
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

	public static int TemperaturePropertyID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double BuoyancyScalar
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

	public static bool BuoyancyUseCoBOffset
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

	public static bool BuoyancyApplyForceOnDie
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

	public static float BuoyancyForceOffsetLerp
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

	public static double BuoyancyWaterDragScalar
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

	public static double BuoyancyWaterDragScalarEnd
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

	public static double BuoyancyWaterDragScalarLerp
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

	public static double BuoyancyWaterDragScalarLerpDotMultBase
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

	public static double BuoyancyWaterDragScalarLerpDotMult
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

	public static double BuoyancyWaterLiftScalarEnd
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

	public static double BuoyancyWaterDragMinVel
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

	public static double BuoyancyWaterDragMinVelMult
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

	public static double BuoyancyWaterDragMinVelMultCOBOff
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

	public static double BuoyancyWaterDragPartVelGreaterVesselMult
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

	public static double BuoyancyWaterDragTimer
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

	public static double BuoyancyWaterDragMultMinForMinDot
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

	public static double BuoyancyWaterAngularDragScalar
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

	public static double BuoyancyAngularDragMinControlSqrMag
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

	public static float BuoyancyWaterAngularDragSlow
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

	public static float BuoyancyWaterDragSlow
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

	public static double BuoyancyWaterDragExtraRBDragAboveDot
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

	public static double BuoyancyScaleAboveDepth
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

	public static double BuoyancyDefaultVolume
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

	public static float BuoyancyMinCrashMult
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

	public static float BuoyancyCrashToleranceMult
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

	public static double BuoyancyRange
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

	public static float BuoyancyKerbals
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

	public static double BuoyancyKerbalsRagdoll
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

	public static float CameraDepthToUnlock
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

	public static float JointBreakForceFactor
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

	public static float JointBreakTorqueFactor
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

	public static float JointForce
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

	public static float RigidJointBreakForceFactor
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

	public static float RigidJointBreakTorqueFactor
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

	public static float MaxAngularVelocity
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

	public static float BuildingImpactDamageMaxVelocityMult
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

	public static bool BuildingImpactDamageUseMomentum
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

	public static float BuildingEasingInvulnerableTime
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

	public static float BuildingImpactDamageMinDamageFraction
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

	public static int OrbitDriftFramesToWait
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

	public static double OrbitDriftSqrThreshold
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

	public static double OrbitDriftAltThreshold
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

	public static float PartMassMin
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

	public static float PartRBMassMin
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

	public static double ConstructionWeightLimit
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

	public static double ConstructionWeightLimitPerKerbalCombine
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

	public static bool ApplyDragToNonPhysicsParts
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

	public static bool ApplyDragToNonPhysicsPartsAtParentCoM
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

	public static float DragMultiplier
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

	public static float BodyLiftMultiplier
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

	public static float DragCubeMultiplier
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

	public static float AngularDragMultiplier
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

	public static string KerbalEVADragCubeString
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

	public static ConfigNode KerbalEVADragCube
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float KerbalCrewMass
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

	public static float PerSeatReduction
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

	public static float PerCommandSeatReduction
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

	public static double KerbalGOffset
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

	public static double KerbalGPower
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

	public static double KerbalGDecayPower
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

	public static double KerbalGBraveMult
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

	public static double KerbalGBadMult
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

	public static double KerbalGClamp
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

	public static double KerbalGThresholdWarn
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

	public static double KerbalGThresholdLOC
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

	public static double KerbalGLOCTimeMult
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

	public static double KerbalGLOCMaxTimeIncrement
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

	public static double KerbalGLOCBaseTime
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

	public static bool KerbalGClampGExperienced
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

	public static PhysicMaterial KerbalEVAPhysicMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float KerbalEVADynamicFriction
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

	public static float KerbalEVAStaticFriction
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

	public static float KerbalEVABounciness
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

	public static PhysicMaterialCombine KerbalEVAFrictionCombine
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

	public static PhysicMaterialCombine KerbalEVABounceCombine
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

	public static double CommNetQTimesVelForBlackoutMin
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

	public static double CommNetQTimesVelForBlackoutMax
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

	public static double CommNetTempForBlackout
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

	public static double CommNetDensityForBlackout
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

	public static double CommNetDotForBlackoutMin
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

	public static double CommNetDotForBlackoutMax
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

	public static double CommNetBlackoutThreshold
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

	public static bool ApplyDrag
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

	public static bool DragUsesAcceleration
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

	public static bool DragCubesUseSpherical
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

	public static FloatCurve DragCurveTip
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurveSurface
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurveTail
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurveMultiplier
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurveCd
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurveCdPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FloatCurve DragCurvePseudoReynolds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float LiftMultiplier
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

	public static float LiftDragMultiplier
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

	public static Dictionary<string, LiftingSurfaceCurve> LiftingSurfaceCurves
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static LiftingSurfaceCurve BodyLiftCurve
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

	public static float AeroFXStartThermalFX
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

	public static float AeroFXFullThermalFX
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

	public static double AeroFXVelocityExponent
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

	public static double AeroFXDensityExponent1
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

	public static double AeroFXDensityScalar1
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

	public static double AeroFXDensityExponent2
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

	public static double AeroFXDensityScalar2
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

	public static float AeroFXMachFXFadeStart
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

	public static float AeroFXMachFXFadeEnd
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

	public static double AeroFXDensityFadeStart
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

	public static string AutoStrutTechRequired
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

	public static Part.ShowRigidAttachmentOption ShowRigidJointTweakable
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

	public static float StagingCooldownTimer
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

	public static VesselTargetModes CelestialBodyTargetingMode
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
	public PhysicsGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PhysicsGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GetBlackBodyRadiation(float temperature, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float DragCurveValue(SurfaceCurvesList cuves, float dotNormalized, float mach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LiftingSurfaceCurve GetLiftingSurfaceCurve(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlightGlobalsReady(bool isReady)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Load Database")]
	public bool LoadDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadDatabase(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Save Database")]
	public void SaveDatabase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadLiftingSurfaceCurves(ConfigNode subNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadDefaultLiftingSurfaceCurves()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateKerbalEVAPhysicsMaterial(float dynamicFriction, float staticFriction, float bounciness, PhysicMaterialCombine frictionCombine, PhysicMaterialCombine bounceCombine)
	{
		throw null;
	}
}
