using System;
using System.Collections.Generic;
using System.IO;
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
		public string name = "";

		public FloatCurve liftCurve = new FloatCurve();

		public FloatCurve liftMachCurve = new FloatCurve();

		public FloatCurve dragCurve = new FloatCurve();

		public FloatCurve dragMachCurve = new FloatCurve();

		public void Load(ConfigNode node)
		{
			string value = node.GetValue("name");
			if (value != null)
			{
				name = value;
			}
			ConfigNode node2 = node.GetNode("lift");
			if (node2 != null)
			{
				liftCurve.Load(node2);
			}
			node2 = node.GetNode("liftMach");
			if (node2 != null)
			{
				liftMachCurve.Load(node2);
			}
			node2 = node.GetNode("drag");
			if (node2 != null)
			{
				dragCurve.Load(node2);
			}
			node2 = node.GetNode("dragMach");
			if (node2 != null)
			{
				dragMachCurve.Load(node2);
			}
		}

		public void Save(ConfigNode node)
		{
			ConfigNode configNode = node.AddNode("LIFTING_SURFACE");
			configNode.AddValue("name", name);
			ConfigNode node2 = configNode.AddNode("lift", "Converts Sin(AoA) into a lift coefficient (Cl) then multiplied by the below mach multiplier, dynamic pressure, the wing area, and the global lift multiplier");
			liftCurve.Save(node2);
			ConfigNode node3 = configNode.AddNode("liftMach", "Converts mach number into a multiplier to Cl");
			liftMachCurve.Save(node3);
			ConfigNode node4 = configNode.AddNode("drag", "Converts Sin(AoA) into a drag coefficient (Cd) then multiplied by the below mach multiplier, dynamic pressure, the wing area, and the global lifting surface drag multiplier");
			dragCurve.Save(node4);
			ConfigNode node5 = configNode.AddNode("dragMach", "Converts mach number into a multiplier to Cd");
			dragMachCurve.Save(node5);
		}
	}

	public static PhysicsGlobals instance;

	[SerializeField]
	public string physicsDatabaseFilename = "Physics.cfg";

	[SerializeField]
	public bool aeroDataDisplay;

	[SerializeField]
	public bool aeroForceDisplay;

	[SerializeField]
	public bool aeroGUIDisplay;

	[SerializeField]
	public float aeroForceDisplayScale = 0.5f;

	[SerializeField]
	public bool autoStrutDisplay;

	[SerializeField]
	public bool roboticJointDataDisplay;

	public static double idealGasConstant = 8.31447;

	public static double boltzmannConstant = 1.3806488E-23;

	public static double stefanBoltzmanConstant = 5.670373E-08;

	public static double avogadroConstant = 6.02214129E+23;

	public const double EngineDefaultAtmDensity = 1.225000023841858;

	public static double GraviticForceMultiplier = 1.0;

	public static double GravitationalAcceleration = 9.80665;

	[SerializeField]
	public bool thermoGUIDisplay;

	[SerializeField]
	public double thermalMaxIntegrationWarp = 100.0;

	[SerializeField]
	public double spaceTemperature = 4.0;

	[SerializeField]
	public double solarLuminosity;

	[SerializeField]
	public double solarLuminosityAtHome = 1360.0;

	[SerializeField]
	public double solarInsolationAtHome = 0.15;

	[SerializeField]
	public double machConvectionDensityExponent = 0.5;

	[SerializeField]
	public double machConvectionVelocityExponent = 3.0;

	[SerializeField]
	public double newtonianDensityExponent = 0.5;

	[SerializeField]
	public double newtonianConvectionFactorBase = 8.14;

	[SerializeField]
	public double newtonianConvectionFactorTotal = 4.0;

	[SerializeField]
	public double newtonianVelocityExponent = 1.0;

	[SerializeField]
	public Gradient blackBodyRadiation = new Gradient();

	[SerializeField]
	public float blackBodyRadiationMin = 798f;

	[SerializeField]
	public float blackBodyRadiationMax = 7000f;

	[SerializeField]
	public float blackBodyRadiationAlphaMult = 0.75f;

	[SerializeField]
	public bool thermalDataDisplay;

	[SerializeField]
	public bool thermalColorsDebug;

	[SerializeField]
	public float temperatureGaugeThreshold = 0.4f;

	[SerializeField]
	public float temperatureGaugeHighlightThreshold = 0.4f;

	[SerializeField]
	public double radiationFactor = 1.0;

	[SerializeField]
	public double machConvectionFactor = 6.0;

	[SerializeField]
	public double conductionFactor = 20.0;

	[SerializeField]
	public double convectionFactorSplashed = 5000.0;

	[SerializeField]
	public double fullToCrossSectionLerpStart = 0.8;

	[SerializeField]
	public double newtonianMachTempLerpEndMach = 5.0;

	[SerializeField]
	public double newtonianMachTempLerpStartMach = 2.0;

	[SerializeField]
	public double newtonianMachTempLerpExponent = 3.0;

	[SerializeField]
	public double machTemperatureScalar = 21.0;

	[SerializeField]
	public double machTemperatureVelocityExponent = 0.75;

	[SerializeField]
	public double turbulentConvectionStart = 100.0;

	[SerializeField]
	public double turbulentConvectionEnd = 200.0;

	[SerializeField]
	public double turbulentConvectionMult = 50.0;

	[SerializeField]
	public double fullConvectionAreaMin = 0.2;

	[SerializeField]
	public double fullToCrossSectionLerpEnd = 1.5;

	[SerializeField]
	public double internalHeatProductionFactor = 0.025;

	[SerializeField]
	public double newtonianTemperatureFactor = 1.0;

	[SerializeField]
	public double shieldedConductionFactor = 0.01;

	[SerializeField]
	public bool thermalRadiationEnabled = true;

	[SerializeField]
	public bool thermalConductionEnabled = true;

	[SerializeField]
	public bool thermalConvectionEnabled = true;

	[SerializeField]
	public double standardSpecificHeatCapacity = 800.0;

	[SerializeField]
	public double skinSkinConductionFactor = 0.005;

	[SerializeField]
	public double skinInternalConductionFactor = 0.03;

	[SerializeField]
	public double thermalIntegrationMinStep = 0.014;

	[SerializeField]
	public double thermalIntegrationMaxTimeOnePass = 0.07;

	[SerializeField]
	public bool thermalIntegrationAlwaysRK2;

	[SerializeField]
	public double occlusionMinStep = 0.039;

	[SerializeField]
	public int thermalIntegrationHighMaxPasses = 20;

	[SerializeField]
	public int thermalIntegrationHighMinPasses = 1;

	[SerializeField]
	public double thermalConvergenceFactor = 0.63;

	[SerializeField]
	public double analyticLerpRateSkin = 0.003;

	[SerializeField]
	public double analyticLerpRateInternal = 0.001;

	[SerializeField]
	public double analyticConvectionSensitivityBase = 0.01;

	[SerializeField]
	public double analyticConvectionSensitivityFinal = 20.0;

	public static int temperaturePropertyID;

	[SerializeField]
	public double buoyancyScalar = 2.0;

	[SerializeField]
	public bool buoyancyUseCoBOffset;

	[SerializeField]
	public bool buoyancyApplyForceOnDie;

	[SerializeField]
	public float buoyancyForceOffsetLerp = 0.5f;

	[SerializeField]
	public double buoyancyWaterDragScalar = 4.0;

	[SerializeField]
	public double buoyancyWaterDragScalarEnd = 0.12;

	[SerializeField]
	public double buoyancyWaterDragScalarLerp = 1.0;

	[SerializeField]
	public double buoyancyWaterDragScalarLerpDotMultBase = 2.0;

	[SerializeField]
	public double buoyancyWaterDragScalarLerpDotMult = 1.25;

	[SerializeField]
	public double buoyancyWaterLiftScalarEnd = 0.1;

	[SerializeField]
	public double buoyancyWaterDragMinVel = 0.3;

	[SerializeField]
	public double buoyancyWaterDragMinVelMult = 4.0;

	[SerializeField]
	public double buoyancyWaterDragMinVelMultCOBOff = 128.0;

	[SerializeField]
	public double buoyancyWaterDragPartVelGreaterVesselMult = 1.5;

	[SerializeField]
	public double buoyancyWaterDragTimer = 3.0;

	[SerializeField]
	public double buoyancyWaterDragMultMinForMinDot = 0.25;

	[SerializeField]
	public double buoyancyWaterAngularDragScalar = 0.01;

	[SerializeField]
	public double buoyancyAngularDragMinControlSqrMag = 1.0 / 32.0;

	[SerializeField]
	public float buoyancyWaterAngularDragSlow = 20f;

	[SerializeField]
	public float buoyancyWaterDragSlow = 2f;

	[SerializeField]
	public double buoyancyWaterDragExtraRBDragAboveDot = 0.5;

	[SerializeField]
	public double buoyancyScaleAboveDepth = 0.2;

	[SerializeField]
	public double buoyancyDefaultVolume = 1.0;

	[SerializeField]
	public float buoyancyMinCrashMult = 0.1f;

	[SerializeField]
	public float buoyancyCrashToleranceMult = 1.2f;

	[SerializeField]
	public double buoyancyRange = 1000.0;

	[SerializeField]
	public float buoyancyKerbals = 0.25f;

	[SerializeField]
	public double buoyancyKerbalsRagdoll = 3.0;

	[SerializeField]
	public float cameraDepthToUnlock = -5f;

	[SerializeField]
	public float jointBreakForceFactor = 17f;

	[SerializeField]
	public float jointBreakTorqueFactor = 17f;

	[SerializeField]
	public float jointForce = 1E+20f;

	[SerializeField]
	public float rigidJointBreakForceFactor = 1f;

	[SerializeField]
	public float rigidJointBreakTorqueFactor = 1f;

	[SerializeField]
	public float maxAngularVelocity = 31.416f;

	[SerializeField]
	public float buildingImpactDamageMaxVelocityMult = 4f;

	[SerializeField]
	public bool buildingImpactDamageUseMomentum;

	[SerializeField]
	public float buildingEasingInvulnerableTime = 2f;

	[SerializeField]
	public float buildingImpactDamageMinDamageFraction = 100f;

	[SerializeField]
	public int orbitDriftFramesToWait = 5;

	[SerializeField]
	public double orbitDriftSqrThreshold = 1E-10;

	[SerializeField]
	public double orbitDriftAltThreshold = 400000000.0;

	[SerializeField]
	public float partMassMin = Mathf.Epsilon;

	[SerializeField]
	public float partRBMassMin = 0.002f;

	[SerializeField]
	public double constructionWeightLimit = GravitationalAcceleration * 60.0;

	[SerializeField]
	public double constructionWeightLimitPerKerbalCombine = GravitationalAcceleration * 60.0;

	[SerializeField]
	public bool applyDragToNonPhysicsParts = true;

	[SerializeField]
	public bool applyDragToNonPhysicsPartsAtParentCoM = true;

	[SerializeField]
	public float dragMultiplier = 8f;

	[SerializeField]
	public float bodyLiftMultiplier = 18f;

	[SerializeField]
	public float dragCubeMultiplier = 0.1f;

	[SerializeField]
	public float angularDragMultiplier = 2f;

	[SerializeField]
	public string kerbalEVADragCubeString = "Default, 0.663,0.78,0.4, 0.663,0.78,0.4, 0.5,0.7,0.4, 0.5,0.78,0.7, 0.7,0.9,0.4, 0.7,0.9,0.4, 0,0,0 0.8,1.1,0.8";

	[SerializeField]
	public float kerbalCrewMass = 0.045f;

	[SerializeField]
	public float perSeatReduction = 0.045f;

	[SerializeField]
	public float perCommandSeatReduction = 0.094f;

	[SerializeField]
	public double kerbalGOffset = 1500.0;

	[SerializeField]
	public double kerbalGPower = 4.0;

	[SerializeField]
	public double kerbalGDecayPower = 2.0;

	[SerializeField]
	public double kerbalGBraveMult = 1.5;

	[SerializeField]
	public double kerbalGBadMult = 1.5;

	[SerializeField]
	public double kerbalGClamp = 20.0;

	[SerializeField]
	public double kerbalGThresholdWarn = 30000.0;

	[SerializeField]
	public double kerbalGThresholdLOC = 60000.0;

	[SerializeField]
	public double kerbalGLOCTimeMult = 0.0001;

	[SerializeField]
	public double kerbalGLOCMaxTimeIncrement = 1.5;

	[SerializeField]
	public double kerbalGLOCBaseTime = 3.0;

	[SerializeField]
	public bool kerbalGClampGExperienced = true;

	[SerializeField]
	public PhysicMaterial kerbalEVAPhysicMaterial;

	[SerializeField]
	public float kerbalEVADynamicFriction = 1.1f;

	[SerializeField]
	public float kerbalEVAStaticFriction = 1.1f;

	[SerializeField]
	public float kerbalEVABounciness = 0.05f;

	[SerializeField]
	public PhysicMaterialCombine kerbalEVAFrictionCombine;

	[SerializeField]
	public PhysicMaterialCombine kerbalEVABounceCombine;

	[SerializeField]
	public double commNetQTimesVelForBlackoutMin = 1500.0;

	[SerializeField]
	public double commNetQTimesVelForBlackoutMax = 9500.0;

	[SerializeField]
	public double commNetTempForBlackout = 1100.0;

	[SerializeField]
	public double commNetDensityForBlackout = 0.00054;

	[SerializeField]
	public double commNetDotForBlackoutMin = -0.866;

	[SerializeField]
	public double commNetDotForBlackoutMax = -0.5;

	[SerializeField]
	public double commNetBlackoutThreshold = 0.5;

	[SerializeField]
	public bool applyDrag = true;

	[SerializeField]
	public bool dragUsesAcceleration;

	[SerializeField]
	public bool dragUsesSpherical;

	[SerializeField]
	public FloatCurve dragCurveTip = new FloatCurve(new Keyframe[4]
	{
		new Keyframe(0f, 1f, 0f, 0f),
		new Keyframe(0.85f, 1.19f, 0.6960422f, 0.6960422f),
		new Keyframe(1.1f, 2.83f, 0.730473f, 0.730473f),
		new Keyframe(5f, 4f, 0f, 0f)
	});

	[SerializeField]
	public FloatCurve dragCurveSurface = new FloatCurve(new Keyframe[7]
	{
		new Keyframe(0f, 0.02f, 0f, 0f),
		new Keyframe(0.85f, 0.02f, 0f, 0f),
		new Keyframe(0.9f, 0.0152439f, -0.07942077f, -0.07942077f),
		new Keyframe(1.1f, 0.0025f, -0.005279571f, -0.001936768f),
		new Keyframe(2f, 0.002083333f, -2.314833E-05f, -2.314833E-05f),
		new Keyframe(5f, 0.003333333f, -0.000180556f, -0.000180556f),
		new Keyframe(25f, 0.001428571f, -7.14286E-05f, 0f)
	});

	[SerializeField]
	public FloatCurve dragCurveTail = new FloatCurve(new Keyframe[6]
	{
		new Keyframe(0f, 1f, 0f, 0f),
		new Keyframe(0.85f, 1f, 0f, 0f),
		new Keyframe(1.1f, 0.25f, -0.02215106f, -0.02487721f),
		new Keyframe(1.4f, 0.22f, -0.03391732f, -0.03391732f),
		new Keyframe(5f, 0.15f, -0.001198566f, -0.001198566f),
		new Keyframe(25f, 0.14f, 0f, 0f)
	});

	[SerializeField]
	public FloatCurve dragCurveMultiplier = new FloatCurve(new Keyframe[8]
	{
		new Keyframe(0f, 0.5f, 0f, 0f),
		new Keyframe(0.85f, 0.5f, 0f, 0f),
		new Keyframe(1.1f, 1.3f, 0f, -0.008100224f),
		new Keyframe(2f, 0.7f, -0.1104858f, -0.1104858f),
		new Keyframe(5f, 0.6f, 0f, 0f),
		new Keyframe(10f, 0.85f, 0.02198264f, 0.02198264f),
		new Keyframe(14f, 0.9f, 0.007694946f, 0.007694946f),
		new Keyframe(25f, 0.95f, 0f, 0f)
	});

	[SerializeField]
	public FloatCurve dragCurveCd = new FloatCurve(new Keyframe[8]
	{
		new Keyframe(0.05f, 0.0025f, 0.15f, 0.15f),
		new Keyframe(0.4f, 0.15f, 0.3963967f, 0.3963967f),
		new Keyframe(0.7f, 0.35f, 0.9066986f, 0.9066986f),
		new Keyframe(0.75f, 0.45f, 3.213604f, 3.213604f),
		new Keyframe(0.8f, 0.66f, 3.49833f, 3.49833f),
		new Keyframe(0.85f, 0.8f, 2.212924f, 2.212924f),
		new Keyframe(0.9f, 0.89f, 1.1f, 1.1f),
		new Keyframe(1f, 1f, 1f, 1f)
	});

	[SerializeField]
	public FloatCurve dragCurveCdPower = new FloatCurve(new Keyframe[4]
	{
		new Keyframe(0f, 1f, 0f, 0.00715953f),
		new Keyframe(0.85f, 1.25f, 0.7780356f, 0.7780356f),
		new Keyframe(1.1f, 2.5f, 0.2492796f, 0.2492796f),
		new Keyframe(5f, 3f, 0f, 0f)
	});

	[SerializeField]
	public FloatCurve dragCurvePseudoReynolds = new FloatCurve(new Keyframe[10]
	{
		new Keyframe(0f, 4f, -111111.1f, -2975.412f),
		new Keyframe(0.0001f, 3f, -251.1479f, -251.1479f),
		new Keyframe(0.01f, 2f, -19.63584f, -19.63584f),
		new Keyframe(0.1f, 1.2f, -0.7846036f, -0.3912548f),
		new Keyframe(1f, 1f, 0f, 0f),
		new Keyframe(100f, 1f, 0f, 0f),
		new Keyframe(200f, 0.82f, 0f, 0f),
		new Keyframe(500f, 0.86f, 0.0001932119f, 0.0001932119f),
		new Keyframe(1000f, 0.9f, 1.54299E-05f, 1.54299E-05f),
		new Keyframe(10000f, 0.95f, 0f, 0f)
	});

	public static SurfaceCurvesList SurfaceCurves;

	[SerializeField]
	public float liftMultiplier = 0.036f;

	[SerializeField]
	public float liftDragMultiplier = 0.015f;

	[SerializeField]
	public Dictionary<string, LiftingSurfaceCurve> liftingSurfaceCurves = new Dictionary<string, LiftingSurfaceCurve>();

	[SerializeField]
	public LiftingSurfaceCurve bodyLiftCurve;

	[SerializeField]
	public float aeroFXStartThermalFX = 2f;

	[SerializeField]
	public float aeroFXFullThermalFX = 3.5f;

	[SerializeField]
	public double aeroFXVelocityExponent = 3.0;

	[SerializeField]
	public double aeroFXDensityExponent1 = 0.75;

	[SerializeField]
	public double aeroFXDensityScalar1 = 1.0;

	[SerializeField]
	public double aeroFXDensityExponent2 = 2.0;

	[SerializeField]
	public double aeroFXDensityScalar2;

	[SerializeField]
	public float aeroFXMachFXFadeStart = 0.15f;

	[SerializeField]
	public float aeroFXMachFXFadeEnd = 0.1f;

	[SerializeField]
	public double aeroFXDensityFadeStart = 0.0015;

	[SerializeField]
	public string autoStrutTechRequired = "generalConstruction";

	[SerializeField]
	public Part.ShowRigidAttachmentOption showRigidJointTweakable = Part.ShowRigidAttachmentOption.Editor;

	[SerializeField]
	public float stagingCooldownTimer = 0.5f;

	[SerializeField]
	public VesselTargetModes celestialBodyTargetingMode = VesselTargetModes.DirectionAndVelocity;

	public VesselRanges VesselRangesDefault;

	[Range(0f, 25f)]
	[SerializeField]
	public float debugGizmosMach = 5f;

	public static int debugGizmosCount = 50;

	public static float gizmosYScale = 4f;

	public static float gizmosXScale = 10f;

	public static PhysicsGlobals Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType<PhysicsGlobals>();
			}
			return instance;
		}
	}

	public static string PhysicsDatabaseFilename
	{
		get
		{
			return Instance.physicsDatabaseFilename;
		}
		set
		{
			Instance.physicsDatabaseFilename = value;
		}
	}

	public static bool AeroDataDisplay
	{
		get
		{
			return Instance.aeroDataDisplay;
		}
		set
		{
			GameSettings.DEBUG_AERO_GUI = value;
			GameSettings.SaveGameSettingsOnly();
			Instance.aeroDataDisplay = value;
		}
	}

	public static bool AeroForceDisplay
	{
		get
		{
			return Instance.aeroForceDisplay;
		}
		set
		{
			Instance.aeroForceDisplay = value;
		}
	}

	public static bool AeroGUIDisplay
	{
		get
		{
			return Instance.aeroGUIDisplay;
		}
		set
		{
			GameSettings.DEBUG_AERO_DATA_PAWS = value;
			GameSettings.SaveGameSettingsOnly();
			Instance.aeroGUIDisplay = value;
		}
	}

	public static float AeroForceDisplayScale
	{
		get
		{
			return Instance.aeroForceDisplayScale;
		}
		set
		{
			Instance.aeroForceDisplayScale = value;
		}
	}

	public static bool AutoStrutDisplay
	{
		get
		{
			return Instance.autoStrutDisplay;
		}
		set
		{
			Instance.autoStrutDisplay = value;
		}
	}

	public static bool RoboticJointDataDisplay
	{
		get
		{
			return Instance.roboticJointDataDisplay;
		}
		set
		{
			Instance.roboticJointDataDisplay = value;
		}
	}

	public static double IdealGasConstant => idealGasConstant;

	public static double BoltzmannConstant => boltzmannConstant;

	public static double StefanBoltzmanConstant => stefanBoltzmanConstant;

	public static double AvogadroConstant => avogadroConstant;

	public static double KpaToAtmospheres
	{
		get
		{
			if (Planetarium.fetch != null && Planetarium.fetch.Home != null)
			{
				return 1.0 / Planetarium.fetch.Home.atmospherePressureSeaLevel;
			}
			return 0.009869232667160128;
		}
	}

	public static bool ThermoGUIDisplay
	{
		get
		{
			return Instance.thermoGUIDisplay;
		}
		set
		{
			Instance.thermoGUIDisplay = value;
		}
	}

	public static double ThermalMaxIntegrationWarp
	{
		get
		{
			return Instance.thermalMaxIntegrationWarp;
		}
		set
		{
			Instance.thermalMaxIntegrationWarp = value;
		}
	}

	public static double SpaceTemperature
	{
		get
		{
			return Instance.spaceTemperature;
		}
		set
		{
			Instance.spaceTemperature = value;
		}
	}

	public static double SolarLuminosity => Instance.solarLuminosity;

	public static double SolarLuminosityAtHome
	{
		get
		{
			return Instance.solarLuminosityAtHome;
		}
		set
		{
			Instance.solarLuminosityAtHome = value;
		}
	}

	public static double SolarInsolationAtHome
	{
		get
		{
			return Instance.solarInsolationAtHome;
		}
		set
		{
			Instance.solarInsolationAtHome = value;
		}
	}

	public static double MachConvectionDensityExponent
	{
		get
		{
			return Instance.machConvectionDensityExponent;
		}
		set
		{
			Instance.machConvectionDensityExponent = value;
		}
	}

	public static double MachConvectionVelocityExponent
	{
		get
		{
			return Instance.machConvectionVelocityExponent;
		}
		set
		{
			Instance.machConvectionVelocityExponent = value;
		}
	}

	public static double NewtonianDensityExponent
	{
		get
		{
			return Instance.newtonianDensityExponent;
		}
		set
		{
			Instance.newtonianDensityExponent = value;
		}
	}

	public static double NewtonianConvectionFactorBase
	{
		get
		{
			return Instance.newtonianConvectionFactorBase;
		}
		set
		{
			Instance.newtonianConvectionFactorBase = value;
		}
	}

	public static double NewtonianConvectionFactorTotal
	{
		get
		{
			return Instance.newtonianConvectionFactorTotal;
		}
		set
		{
			Instance.newtonianConvectionFactorTotal = value;
		}
	}

	public static double NewtonianVelocityExponent
	{
		get
		{
			return Instance.newtonianVelocityExponent;
		}
		set
		{
			Instance.newtonianVelocityExponent = value;
		}
	}

	public static Gradient BlackBodyRadiation => Instance.blackBodyRadiation;

	public static float BlackBodyRadiationMin
	{
		get
		{
			return Instance.blackBodyRadiationMin;
		}
		set
		{
			Instance.blackBodyRadiationMin = value;
		}
	}

	public static float BlackBodyRadiationMax
	{
		get
		{
			return Instance.blackBodyRadiationMax;
		}
		set
		{
			Instance.blackBodyRadiationMax = value;
		}
	}

	public static float BlackBodyRadiationAlphaMult
	{
		get
		{
			return Instance.blackBodyRadiationAlphaMult;
		}
		set
		{
			Instance.blackBodyRadiationAlphaMult = value;
		}
	}

	public static bool ThermalDataDisplay
	{
		get
		{
			return Instance.thermalDataDisplay;
		}
		set
		{
			Instance.thermalDataDisplay = value;
		}
	}

	public static bool ThermalColorsDebug
	{
		get
		{
			return Instance.thermalColorsDebug;
		}
		set
		{
			Instance.thermalColorsDebug = value;
		}
	}

	public static float TemperatureGaugeThreshold
	{
		get
		{
			return Instance.temperatureGaugeThreshold;
		}
		set
		{
			Instance.temperatureGaugeThreshold = value;
		}
	}

	public static float TemperatureGaugeHighlightThreshold
	{
		get
		{
			return Instance.temperatureGaugeHighlightThreshold;
		}
		set
		{
			Instance.temperatureGaugeHighlightThreshold = value;
		}
	}

	public static double RadiationFactor
	{
		get
		{
			return Instance.radiationFactor;
		}
		set
		{
			Instance.radiationFactor = value;
		}
	}

	public static double MachConvectionFactor
	{
		get
		{
			return Instance.machConvectionFactor;
		}
		set
		{
			Instance.machConvectionFactor = value;
		}
	}

	public static double ConductionFactor
	{
		get
		{
			return Instance.conductionFactor;
		}
		set
		{
			Instance.conductionFactor = value;
		}
	}

	public static double ConvectionFactorSplashed
	{
		get
		{
			return Instance.convectionFactorSplashed;
		}
		set
		{
			Instance.convectionFactorSplashed = value;
		}
	}

	public static double FullToCrossSectionLerpStart
	{
		get
		{
			return Instance.fullToCrossSectionLerpStart;
		}
		set
		{
			Instance.fullToCrossSectionLerpStart = value;
		}
	}

	public static double NewtonianMachTempLerpEndMach
	{
		get
		{
			return Instance.newtonianMachTempLerpEndMach;
		}
		set
		{
			Instance.newtonianMachTempLerpEndMach = value;
		}
	}

	public static double NewtonianMachTempLerpStartMach
	{
		get
		{
			return Instance.newtonianMachTempLerpStartMach;
		}
		set
		{
			Instance.newtonianMachTempLerpStartMach = value;
		}
	}

	public static double NewtonianMachTempLerpExponent
	{
		get
		{
			return Instance.newtonianMachTempLerpExponent;
		}
		set
		{
			Instance.newtonianMachTempLerpExponent = value;
		}
	}

	public static double MachTemperatureScalar
	{
		get
		{
			return Instance.machTemperatureScalar;
		}
		set
		{
			Instance.machTemperatureScalar = value;
		}
	}

	public static double MachTemperatureVelocityExponent
	{
		get
		{
			return Instance.machTemperatureVelocityExponent;
		}
		set
		{
			Instance.machTemperatureVelocityExponent = value;
		}
	}

	public static double TurbulentConvectionStart
	{
		get
		{
			return Instance.turbulentConvectionStart;
		}
		set
		{
			Instance.turbulentConvectionStart = value;
		}
	}

	public static double TurbulentConvectionEnd
	{
		get
		{
			return Instance.turbulentConvectionEnd;
		}
		set
		{
			Instance.turbulentConvectionEnd = value;
		}
	}

	public static double TurbulentConvectionMult
	{
		get
		{
			return Instance.turbulentConvectionMult;
		}
		set
		{
			Instance.turbulentConvectionMult = value;
		}
	}

	public static double FullConvectionAreaMin
	{
		get
		{
			return Instance.fullConvectionAreaMin;
		}
		set
		{
			Instance.fullConvectionAreaMin = value;
		}
	}

	public static double FullToCrossSectionLerpEnd
	{
		get
		{
			return Instance.fullToCrossSectionLerpEnd;
		}
		set
		{
			Instance.fullToCrossSectionLerpEnd = value;
		}
	}

	public static double InternalHeatProductionFactor
	{
		get
		{
			return Instance.internalHeatProductionFactor;
		}
		set
		{
			Instance.internalHeatProductionFactor = value;
		}
	}

	public static double NewtonianTemperatureFactor
	{
		get
		{
			return Instance.newtonianTemperatureFactor;
		}
		set
		{
			Instance.newtonianTemperatureFactor = value;
		}
	}

	public static double ShieldedConductionFactor
	{
		get
		{
			return Instance.shieldedConductionFactor;
		}
		set
		{
			Instance.shieldedConductionFactor = value;
		}
	}

	public static bool ThermalRadiationEnabled
	{
		get
		{
			return Instance.thermalRadiationEnabled;
		}
		set
		{
			Instance.thermalRadiationEnabled = value;
		}
	}

	public static bool ThermalConductionEnabled
	{
		get
		{
			return Instance.thermalConductionEnabled;
		}
		set
		{
			Instance.thermalConductionEnabled = value;
		}
	}

	public static bool ThermalConvectionEnabled
	{
		get
		{
			return Instance.thermalConvectionEnabled;
		}
		set
		{
			Instance.thermalConvectionEnabled = value;
		}
	}

	public static double StandardSpecificHeatCapacity
	{
		get
		{
			return Instance.standardSpecificHeatCapacity;
		}
		set
		{
			Instance.standardSpecificHeatCapacity = value;
		}
	}

	public static double SkinSkinConductionFactor
	{
		get
		{
			return Instance.skinSkinConductionFactor;
		}
		set
		{
			Instance.skinSkinConductionFactor = value;
		}
	}

	public static double SkinInternalConductionFactor
	{
		get
		{
			return Instance.skinInternalConductionFactor;
		}
		set
		{
			Instance.skinInternalConductionFactor = value;
		}
	}

	public static double ThermalIntegrationMinStep
	{
		get
		{
			return Instance.thermalIntegrationMinStep;
		}
		set
		{
			Instance.thermalIntegrationMinStep = value;
		}
	}

	public static double ThermalIntegrationMaxTimeOnePass
	{
		get
		{
			return Instance.thermalIntegrationMaxTimeOnePass;
		}
		set
		{
			Instance.thermalIntegrationMaxTimeOnePass = value;
		}
	}

	public static bool ThermalIntegrationAlwaysRK2
	{
		get
		{
			return Instance.thermalIntegrationAlwaysRK2;
		}
		set
		{
			Instance.thermalIntegrationAlwaysRK2 = value;
		}
	}

	public static double OcclusionMinStep
	{
		get
		{
			return Instance.occlusionMinStep;
		}
		set
		{
			Instance.occlusionMinStep = value;
		}
	}

	public static int ThermalIntegrationHighMaxPasses
	{
		get
		{
			return Instance.thermalIntegrationHighMaxPasses;
		}
		set
		{
			Instance.thermalIntegrationHighMaxPasses = value;
		}
	}

	public static int ThermalIntegrationHighMinPasses
	{
		get
		{
			return Instance.thermalIntegrationHighMinPasses;
		}
		set
		{
			Instance.thermalIntegrationHighMinPasses = value;
		}
	}

	public static double ThermalConvergenceFactor
	{
		get
		{
			return Instance.thermalConvergenceFactor;
		}
		set
		{
			Instance.thermalConvergenceFactor = value;
		}
	}

	public static double AnalyticLerpRateSkin
	{
		get
		{
			return Instance.analyticLerpRateSkin;
		}
		set
		{
			Instance.analyticLerpRateSkin = value;
		}
	}

	public static double AnalyticLerpRateInternal
	{
		get
		{
			return Instance.analyticLerpRateInternal;
		}
		set
		{
			Instance.analyticLerpRateInternal = value;
		}
	}

	public static double AnalyticConvectionSensitivityBase
	{
		get
		{
			return Instance.analyticConvectionSensitivityBase;
		}
		set
		{
			Instance.analyticConvectionSensitivityBase = value;
		}
	}

	public static double AnalyticConvectionSensitivityFinal
	{
		get
		{
			return Instance.analyticConvectionSensitivityFinal;
		}
		set
		{
			Instance.analyticConvectionSensitivityFinal = value;
		}
	}

	public static int TemperaturePropertyID => temperaturePropertyID;

	public static double BuoyancyScalar
	{
		get
		{
			return Instance.buoyancyScalar;
		}
		set
		{
			Instance.buoyancyScalar = value;
		}
	}

	public static bool BuoyancyUseCoBOffset
	{
		get
		{
			return Instance.buoyancyUseCoBOffset;
		}
		set
		{
			Instance.buoyancyUseCoBOffset = value;
		}
	}

	public static bool BuoyancyApplyForceOnDie
	{
		get
		{
			return Instance.buoyancyApplyForceOnDie;
		}
		set
		{
			Instance.buoyancyApplyForceOnDie = value;
		}
	}

	public static float BuoyancyForceOffsetLerp
	{
		get
		{
			return Instance.buoyancyForceOffsetLerp;
		}
		set
		{
			Instance.buoyancyForceOffsetLerp = value;
		}
	}

	public static double BuoyancyWaterDragScalar
	{
		get
		{
			return Instance.buoyancyWaterDragScalar;
		}
		set
		{
			Instance.buoyancyWaterDragScalar = value;
		}
	}

	public static double BuoyancyWaterDragScalarEnd
	{
		get
		{
			return Instance.buoyancyWaterDragScalarEnd;
		}
		set
		{
			Instance.buoyancyWaterDragScalarEnd = value;
		}
	}

	public static double BuoyancyWaterDragScalarLerp
	{
		get
		{
			return Instance.buoyancyWaterDragScalarLerp;
		}
		set
		{
			Instance.buoyancyWaterDragScalarLerp = value;
		}
	}

	public static double BuoyancyWaterDragScalarLerpDotMultBase
	{
		get
		{
			return Instance.buoyancyWaterDragScalarLerpDotMultBase;
		}
		set
		{
			Instance.buoyancyWaterDragScalarLerpDotMultBase = value;
		}
	}

	public static double BuoyancyWaterDragScalarLerpDotMult
	{
		get
		{
			return Instance.buoyancyWaterDragScalarLerpDotMult;
		}
		set
		{
			Instance.buoyancyWaterDragScalarLerpDotMult = value;
		}
	}

	public static double BuoyancyWaterLiftScalarEnd
	{
		get
		{
			return Instance.buoyancyWaterLiftScalarEnd;
		}
		set
		{
			Instance.buoyancyWaterLiftScalarEnd = value;
		}
	}

	public static double BuoyancyWaterDragMinVel
	{
		get
		{
			return Instance.buoyancyWaterDragMinVel;
		}
		set
		{
			Instance.buoyancyWaterDragMinVel = value;
		}
	}

	public static double BuoyancyWaterDragMinVelMult
	{
		get
		{
			return Instance.buoyancyWaterDragMinVelMult;
		}
		set
		{
			Instance.buoyancyWaterDragMinVelMult = value;
		}
	}

	public static double BuoyancyWaterDragMinVelMultCOBOff
	{
		get
		{
			return Instance.buoyancyWaterDragMinVelMultCOBOff;
		}
		set
		{
			Instance.buoyancyWaterDragMinVelMultCOBOff = value;
		}
	}

	public static double BuoyancyWaterDragPartVelGreaterVesselMult
	{
		get
		{
			return Instance.buoyancyWaterDragPartVelGreaterVesselMult;
		}
		set
		{
			Instance.buoyancyWaterDragPartVelGreaterVesselMult = value;
		}
	}

	public static double BuoyancyWaterDragTimer
	{
		get
		{
			return Instance.buoyancyWaterDragTimer;
		}
		set
		{
			Instance.buoyancyWaterDragTimer = value;
		}
	}

	public static double BuoyancyWaterDragMultMinForMinDot
	{
		get
		{
			return Instance.buoyancyWaterDragMultMinForMinDot;
		}
		set
		{
			Instance.buoyancyWaterDragMultMinForMinDot = value;
		}
	}

	public static double BuoyancyWaterAngularDragScalar
	{
		get
		{
			return Instance.buoyancyWaterAngularDragScalar;
		}
		set
		{
			Instance.buoyancyWaterAngularDragScalar = value;
		}
	}

	public static double BuoyancyAngularDragMinControlSqrMag
	{
		get
		{
			return Instance.buoyancyAngularDragMinControlSqrMag;
		}
		set
		{
			Instance.buoyancyAngularDragMinControlSqrMag = value;
		}
	}

	public static float BuoyancyWaterAngularDragSlow
	{
		get
		{
			return Instance.buoyancyWaterAngularDragSlow;
		}
		set
		{
			Instance.buoyancyWaterAngularDragSlow = value;
		}
	}

	public static float BuoyancyWaterDragSlow
	{
		get
		{
			return Instance.buoyancyWaterDragSlow;
		}
		set
		{
			Instance.buoyancyWaterDragSlow = value;
		}
	}

	public static double BuoyancyWaterDragExtraRBDragAboveDot
	{
		get
		{
			return Instance.buoyancyWaterDragExtraRBDragAboveDot;
		}
		set
		{
			Instance.buoyancyWaterDragExtraRBDragAboveDot = value;
		}
	}

	public static double BuoyancyScaleAboveDepth
	{
		get
		{
			return Instance.buoyancyScaleAboveDepth;
		}
		set
		{
			Instance.buoyancyScaleAboveDepth = value;
		}
	}

	public static double BuoyancyDefaultVolume
	{
		get
		{
			return Instance.buoyancyDefaultVolume;
		}
		set
		{
			Instance.buoyancyDefaultVolume = value;
		}
	}

	public static float BuoyancyMinCrashMult
	{
		get
		{
			return Instance.buoyancyMinCrashMult;
		}
		set
		{
			Instance.buoyancyMinCrashMult = value;
		}
	}

	public static float BuoyancyCrashToleranceMult
	{
		get
		{
			return Instance.buoyancyCrashToleranceMult;
		}
		set
		{
			Instance.buoyancyCrashToleranceMult = value;
		}
	}

	public static double BuoyancyRange
	{
		get
		{
			return Instance.buoyancyRange;
		}
		set
		{
			Instance.buoyancyRange = value;
		}
	}

	public static float BuoyancyKerbals
	{
		get
		{
			return Instance.buoyancyKerbals;
		}
		set
		{
			Instance.buoyancyKerbals = value;
		}
	}

	public static double BuoyancyKerbalsRagdoll
	{
		get
		{
			return Instance.buoyancyKerbalsRagdoll;
		}
		set
		{
			Instance.buoyancyKerbalsRagdoll = value;
		}
	}

	public static float CameraDepthToUnlock
	{
		get
		{
			return Instance.cameraDepthToUnlock;
		}
		set
		{
			Instance.cameraDepthToUnlock = value;
		}
	}

	public static float JointBreakForceFactor
	{
		get
		{
			return Instance.jointBreakForceFactor;
		}
		set
		{
			Instance.jointBreakForceFactor = value;
		}
	}

	public static float JointBreakTorqueFactor
	{
		get
		{
			return Instance.jointBreakTorqueFactor;
		}
		set
		{
			Instance.jointBreakTorqueFactor = value;
		}
	}

	public static float JointForce
	{
		get
		{
			if (!(Instance.jointForce > 0f))
			{
				return float.PositiveInfinity;
			}
			return Instance.jointForce;
		}
		set
		{
			Instance.jointForce = value;
		}
	}

	public static float RigidJointBreakForceFactor
	{
		get
		{
			return Instance.rigidJointBreakForceFactor;
		}
		set
		{
			Instance.rigidJointBreakForceFactor = value;
		}
	}

	public static float RigidJointBreakTorqueFactor
	{
		get
		{
			return Instance.rigidJointBreakTorqueFactor;
		}
		set
		{
			Instance.rigidJointBreakTorqueFactor = value;
		}
	}

	public static float MaxAngularVelocity
	{
		get
		{
			return Instance.maxAngularVelocity;
		}
		set
		{
			Instance.maxAngularVelocity = value;
		}
	}

	public static float BuildingImpactDamageMaxVelocityMult
	{
		get
		{
			return Instance.buildingImpactDamageMaxVelocityMult;
		}
		set
		{
			Instance.buildingImpactDamageMaxVelocityMult = value;
		}
	}

	public static bool BuildingImpactDamageUseMomentum
	{
		get
		{
			return Instance.buildingImpactDamageUseMomentum;
		}
		set
		{
			Instance.buildingImpactDamageUseMomentum = value;
		}
	}

	public static float BuildingEasingInvulnerableTime
	{
		get
		{
			return Instance.buildingEasingInvulnerableTime;
		}
		set
		{
			Instance.buildingEasingInvulnerableTime = value;
		}
	}

	public static float BuildingImpactDamageMinDamageFraction
	{
		get
		{
			return Instance.buildingImpactDamageMinDamageFraction;
		}
		set
		{
			Instance.buildingImpactDamageMinDamageFraction = value;
		}
	}

	public static int OrbitDriftFramesToWait
	{
		get
		{
			return Instance.orbitDriftFramesToWait;
		}
		set
		{
			Instance.orbitDriftFramesToWait = value;
		}
	}

	public static double OrbitDriftSqrThreshold
	{
		get
		{
			return Instance.orbitDriftSqrThreshold;
		}
		set
		{
			Instance.orbitDriftSqrThreshold = value;
		}
	}

	public static double OrbitDriftAltThreshold
	{
		get
		{
			return Instance.orbitDriftAltThreshold;
		}
		set
		{
			Instance.orbitDriftAltThreshold = value;
		}
	}

	public static float PartMassMin
	{
		get
		{
			return Instance.partMassMin;
		}
		set
		{
			Instance.partMassMin = value;
		}
	}

	public static float PartRBMassMin
	{
		get
		{
			return Instance.partRBMassMin;
		}
		set
		{
			Instance.partRBMassMin = value;
		}
	}

	public static double ConstructionWeightLimit
	{
		get
		{
			return Instance.constructionWeightLimit;
		}
		set
		{
			Instance.constructionWeightLimit = value;
		}
	}

	public static double ConstructionWeightLimitPerKerbalCombine
	{
		get
		{
			return Instance.constructionWeightLimitPerKerbalCombine;
		}
		set
		{
			Instance.constructionWeightLimitPerKerbalCombine = value;
		}
	}

	public static bool ApplyDragToNonPhysicsParts
	{
		get
		{
			return Instance.applyDragToNonPhysicsParts;
		}
		set
		{
			Instance.applyDragToNonPhysicsParts = value;
		}
	}

	public static bool ApplyDragToNonPhysicsPartsAtParentCoM
	{
		get
		{
			return Instance.applyDragToNonPhysicsPartsAtParentCoM;
		}
		set
		{
			Instance.applyDragToNonPhysicsPartsAtParentCoM = value;
		}
	}

	public static float DragMultiplier
	{
		get
		{
			return Instance.dragMultiplier;
		}
		set
		{
			Instance.dragMultiplier = value;
		}
	}

	public static float BodyLiftMultiplier
	{
		get
		{
			return Instance.bodyLiftMultiplier;
		}
		set
		{
			Instance.bodyLiftMultiplier = value;
		}
	}

	public static float DragCubeMultiplier
	{
		get
		{
			return Instance.dragCubeMultiplier;
		}
		set
		{
			Instance.dragCubeMultiplier = value;
		}
	}

	public static float AngularDragMultiplier
	{
		get
		{
			return Instance.angularDragMultiplier;
		}
		set
		{
			Instance.angularDragMultiplier = value;
		}
	}

	public static string KerbalEVADragCubeString
	{
		get
		{
			return Instance.kerbalEVADragCubeString;
		}
		set
		{
			Instance.kerbalEVADragCubeString = value;
		}
	}

	public static ConfigNode KerbalEVADragCube
	{
		get
		{
			ConfigNode configNode = new ConfigNode("DRAG_CUBE");
			configNode.AddValue("cube", KerbalEVADragCubeString);
			return configNode;
		}
	}

	public static float KerbalCrewMass
	{
		get
		{
			return Instance.kerbalCrewMass;
		}
		set
		{
			Instance.kerbalCrewMass = value;
		}
	}

	public static float PerSeatReduction
	{
		get
		{
			return Instance.perSeatReduction;
		}
		set
		{
			Instance.perSeatReduction = value;
		}
	}

	public static float PerCommandSeatReduction
	{
		get
		{
			return Instance.perCommandSeatReduction;
		}
		set
		{
			Instance.perCommandSeatReduction = value;
		}
	}

	public static double KerbalGOffset
	{
		get
		{
			return Instance.kerbalGOffset;
		}
		set
		{
			Instance.kerbalGOffset = value;
		}
	}

	public static double KerbalGPower
	{
		get
		{
			return Instance.kerbalGPower;
		}
		set
		{
			Instance.kerbalGPower = value;
		}
	}

	public static double KerbalGDecayPower
	{
		get
		{
			return Instance.kerbalGDecayPower;
		}
		set
		{
			Instance.kerbalGDecayPower = value;
		}
	}

	public static double KerbalGBraveMult
	{
		get
		{
			return Instance.kerbalGBraveMult;
		}
		set
		{
			Instance.kerbalGBraveMult = value;
		}
	}

	public static double KerbalGBadMult
	{
		get
		{
			return Instance.kerbalGBadMult;
		}
		set
		{
			Instance.kerbalGBadMult = value;
		}
	}

	public static double KerbalGClamp
	{
		get
		{
			return Instance.kerbalGClamp;
		}
		set
		{
			Instance.kerbalGClamp = value;
		}
	}

	public static double KerbalGThresholdWarn
	{
		get
		{
			return Instance.kerbalGThresholdWarn;
		}
		set
		{
			Instance.kerbalGThresholdWarn = value;
		}
	}

	public static double KerbalGThresholdLOC
	{
		get
		{
			return Instance.kerbalGThresholdLOC;
		}
		set
		{
			Instance.kerbalGThresholdLOC = value;
		}
	}

	public static double KerbalGLOCTimeMult
	{
		get
		{
			return Instance.kerbalGLOCTimeMult;
		}
		set
		{
			Instance.kerbalGLOCTimeMult = value;
		}
	}

	public static double KerbalGLOCMaxTimeIncrement
	{
		get
		{
			return Instance.kerbalGLOCMaxTimeIncrement;
		}
		set
		{
			Instance.kerbalGLOCMaxTimeIncrement = value;
		}
	}

	public static double KerbalGLOCBaseTime
	{
		get
		{
			return Instance.kerbalGLOCBaseTime;
		}
		set
		{
			Instance.kerbalGLOCBaseTime = value;
		}
	}

	public static bool KerbalGClampGExperienced
	{
		get
		{
			return Instance.kerbalGClampGExperienced;
		}
		set
		{
			Instance.kerbalGClampGExperienced = value;
		}
	}

	public static PhysicMaterial KerbalEVAPhysicMaterial => Instance.kerbalEVAPhysicMaterial;

	public static float KerbalEVADynamicFriction
	{
		get
		{
			return Instance.kerbalEVADynamicFriction;
		}
		set
		{
			Instance.kerbalEVADynamicFriction = value;
		}
	}

	public static float KerbalEVAStaticFriction
	{
		get
		{
			return Instance.kerbalEVAStaticFriction;
		}
		set
		{
			Instance.kerbalEVAStaticFriction = value;
		}
	}

	public static float KerbalEVABounciness
	{
		get
		{
			return Instance.kerbalEVABounciness;
		}
		set
		{
			Instance.kerbalEVABounciness = value;
		}
	}

	public static PhysicMaterialCombine KerbalEVAFrictionCombine
	{
		get
		{
			return Instance.kerbalEVAFrictionCombine;
		}
		set
		{
			Instance.kerbalEVAFrictionCombine = value;
		}
	}

	public static PhysicMaterialCombine KerbalEVABounceCombine
	{
		get
		{
			return Instance.kerbalEVABounceCombine;
		}
		set
		{
			Instance.kerbalEVABounceCombine = value;
		}
	}

	public static double CommNetQTimesVelForBlackoutMin
	{
		get
		{
			return Instance.commNetQTimesVelForBlackoutMin;
		}
		set
		{
			Instance.commNetQTimesVelForBlackoutMin = value;
		}
	}

	public static double CommNetQTimesVelForBlackoutMax
	{
		get
		{
			return Instance.commNetQTimesVelForBlackoutMax;
		}
		set
		{
			Instance.commNetQTimesVelForBlackoutMax = value;
		}
	}

	public static double CommNetTempForBlackout
	{
		get
		{
			return Instance.commNetTempForBlackout;
		}
		set
		{
			Instance.commNetTempForBlackout = value;
		}
	}

	public static double CommNetDensityForBlackout
	{
		get
		{
			return Instance.commNetDensityForBlackout;
		}
		set
		{
			Instance.commNetDensityForBlackout = value;
		}
	}

	public static double CommNetDotForBlackoutMin
	{
		get
		{
			return Instance.commNetDotForBlackoutMin;
		}
		set
		{
			Instance.commNetDotForBlackoutMin = value;
		}
	}

	public static double CommNetDotForBlackoutMax
	{
		get
		{
			return Instance.commNetDotForBlackoutMax;
		}
		set
		{
			Instance.commNetDotForBlackoutMax = value;
		}
	}

	public static double CommNetBlackoutThreshold
	{
		get
		{
			return Instance.commNetBlackoutThreshold;
		}
		set
		{
			Instance.commNetBlackoutThreshold = value;
		}
	}

	public static bool ApplyDrag
	{
		get
		{
			return Instance.applyDrag;
		}
		set
		{
			Instance.applyDrag = value;
		}
	}

	public static bool DragUsesAcceleration
	{
		get
		{
			return Instance.dragUsesAcceleration;
		}
		set
		{
			Instance.dragUsesAcceleration = value;
		}
	}

	public static bool DragCubesUseSpherical
	{
		get
		{
			return Instance.dragUsesSpherical;
		}
		set
		{
			Instance.dragUsesSpherical = value;
		}
	}

	public static FloatCurve DragCurveTip => Instance.dragCurveTip;

	public static FloatCurve DragCurveSurface => Instance.dragCurveSurface;

	public static FloatCurve DragCurveTail => Instance.dragCurveTail;

	public static FloatCurve DragCurveMultiplier => Instance.dragCurveMultiplier;

	public static FloatCurve DragCurveCd => Instance.dragCurveCd;

	public static FloatCurve DragCurveCdPower => Instance.dragCurveCdPower;

	public static FloatCurve DragCurvePseudoReynolds => Instance.dragCurvePseudoReynolds;

	public static float LiftMultiplier
	{
		get
		{
			return Instance.liftMultiplier;
		}
		set
		{
			Instance.liftMultiplier = value;
		}
	}

	public static float LiftDragMultiplier
	{
		get
		{
			return Instance.liftDragMultiplier;
		}
		set
		{
			Instance.liftDragMultiplier = value;
		}
	}

	public static Dictionary<string, LiftingSurfaceCurve> LiftingSurfaceCurves => Instance.liftingSurfaceCurves;

	public static LiftingSurfaceCurve BodyLiftCurve
	{
		get
		{
			return Instance.bodyLiftCurve;
		}
		set
		{
			Instance.bodyLiftCurve = value;
		}
	}

	public static float AeroFXStartThermalFX
	{
		get
		{
			return Instance.aeroFXStartThermalFX;
		}
		set
		{
			Instance.aeroFXStartThermalFX = value;
		}
	}

	public static float AeroFXFullThermalFX
	{
		get
		{
			return Instance.aeroFXFullThermalFX;
		}
		set
		{
			Instance.aeroFXFullThermalFX = value;
		}
	}

	public static double AeroFXVelocityExponent
	{
		get
		{
			return Instance.aeroFXVelocityExponent;
		}
		set
		{
			Instance.aeroFXVelocityExponent = value;
		}
	}

	public static double AeroFXDensityExponent1
	{
		get
		{
			return Instance.aeroFXDensityExponent1;
		}
		set
		{
			Instance.aeroFXDensityExponent1 = value;
		}
	}

	public static double AeroFXDensityScalar1
	{
		get
		{
			return Instance.aeroFXDensityScalar1;
		}
		set
		{
			Instance.aeroFXDensityScalar1 = value;
		}
	}

	public static double AeroFXDensityExponent2
	{
		get
		{
			return Instance.aeroFXDensityExponent2;
		}
		set
		{
			Instance.aeroFXDensityExponent2 = value;
		}
	}

	public static double AeroFXDensityScalar2
	{
		get
		{
			return Instance.aeroFXDensityScalar2;
		}
		set
		{
			Instance.aeroFXDensityScalar2 = value;
		}
	}

	public static float AeroFXMachFXFadeStart
	{
		get
		{
			return Instance.aeroFXMachFXFadeStart;
		}
		set
		{
			Instance.aeroFXMachFXFadeStart = value;
		}
	}

	public static float AeroFXMachFXFadeEnd
	{
		get
		{
			return Instance.aeroFXMachFXFadeEnd;
		}
		set
		{
			Instance.aeroFXMachFXFadeEnd = value;
		}
	}

	public static double AeroFXDensityFadeStart
	{
		get
		{
			return Instance.aeroFXDensityFadeStart;
		}
		set
		{
			Instance.aeroFXDensityFadeStart = value;
		}
	}

	public static string AutoStrutTechRequired
	{
		get
		{
			return Instance.autoStrutTechRequired;
		}
		set
		{
			Instance.autoStrutTechRequired = value;
		}
	}

	public static Part.ShowRigidAttachmentOption ShowRigidJointTweakable
	{
		get
		{
			return Instance.showRigidJointTweakable;
		}
		set
		{
			Instance.showRigidJointTweakable = value;
		}
	}

	public static float StagingCooldownTimer
	{
		get
		{
			return Instance.stagingCooldownTimer;
		}
		set
		{
			Instance.stagingCooldownTimer = value;
		}
	}

	public static VesselTargetModes CelestialBodyTargetingMode
	{
		get
		{
			return Instance.celestialBodyTargetingMode;
		}
		set
		{
			Instance.celestialBodyTargetingMode = value;
		}
	}

	public static Color GetBlackBodyRadiation(float temperature, Part part)
	{
		if (ThermalColorsDebug)
		{
			float time = Mathf.Clamp01(temperature / 3000f);
			Color result = BlackBodyRadiation.Evaluate(time);
			result.a = 1f;
			return result;
		}
		float num = BlackBodyRadiationMin;
		float time2 = Mathf.Clamp01((temperature - num) / (BlackBodyRadiationMax - num));
		Color result2 = BlackBodyRadiation.Evaluate(time2);
		result2.a *= BlackBodyRadiationAlphaMult * part.blackBodyRadiationAlphaMult;
		return result2;
	}

	public static float DragCurveValue(SurfaceCurvesList cuves, float dotNormalized, float mach)
	{
		if (dotNormalized <= 0.5f)
		{
			float a = cuves.dragCurveTail.Evaluate(mach);
			float b = cuves.dragCurveSurface.Evaluate(mach);
			float num = cuves.dragCurveMultiplier.Evaluate(mach);
			return Mathf.Lerp(a, b, dotNormalized * 2f) * num;
		}
		float a2 = cuves.dragCurveSurface.Evaluate(mach);
		float b2 = cuves.dragCurveTip.Evaluate(mach);
		float num2 = cuves.dragCurveMultiplier.Evaluate(mach);
		return Mathf.Lerp(a2, b2, (dotNormalized - 0.5f) * 2f) * num2;
	}

	public static LiftingSurfaceCurve GetLiftingSurfaceCurve(string name)
	{
		LiftingSurfaceCurve value = null;
		if (LiftingSurfaceCurves.TryGetValue(name, out value))
		{
			return value;
		}
		return null;
	}

	public void CalculateValues()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		CelestialBody celestialBody = FlightGlobals.GetHomeBody();
		CelestialBody celestialBody2 = null;
		celestialBody2 = ((!(Planetarium.fetch != null)) ? FlightGlobals.Bodies[0] : Planetarium.fetch.Sun);
		if (celestialBody != null)
		{
			while (celestialBody.referenceBody != celestialBody2 && celestialBody.referenceBody != null)
			{
				celestialBody = celestialBody.referenceBody;
			}
			solarLuminosity = Math.Pow(celestialBody.orbit.semiMajorAxis, 2.0) * 4.0 * Math.PI * solarLuminosityAtHome;
		}
	}

	public void Awake()
	{
		temperaturePropertyID = Shader.PropertyToID("_TemperatureColor");
		instance = this;
		if (!physicsDatabaseFilename.Contains("\\") && !physicsDatabaseFilename.Contains("/"))
		{
			physicsDatabaseFilename = KSPUtil.ApplicationRootPath + physicsDatabaseFilename;
		}
		SurfaceCurvesList surfaceCurves = default(SurfaceCurvesList);
		surfaceCurves.dragCurveTail = DragCurveTail;
		surfaceCurves.dragCurveSurface = dragCurveSurface;
		surfaceCurves.dragCurveMultiplier = dragCurveMultiplier;
		surfaceCurves.dragCurveTip = dragCurveTip;
		SurfaceCurves = surfaceCurves;
	}

	public void Start()
	{
		if (!LoadDatabase())
		{
			SaveDatabase();
		}
		aeroDataDisplay = GameSettings.DEBUG_AERO_GUI;
		aeroGUIDisplay = GameSettings.DEBUG_AERO_DATA_PAWS;
		bodyLiftCurve = GetLiftingSurfaceCurve("BodyLift");
		GameEvents.OnFlightGlobalsReady.Add(OnFlightGlobalsReady);
		Part[] array = Resources.FindObjectsOfTypeAll<Part>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			Part part = array[i];
			if (part.name.Contains("kerbalEVA"))
			{
				part.DragCubes.LoadCubes(KerbalEVADragCube);
			}
		}
		UpdateKerbalEVAPhysicsMaterial(KerbalEVADynamicFriction, KerbalEVAStaticFriction, KerbalEVABounciness, KerbalEVAFrictionCombine, KerbalEVABounceCombine);
	}

	public void OnDestroy()
	{
		GameEvents.OnFlightGlobalsReady.Remove(OnFlightGlobalsReady);
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			Vector3 position = base.transform.position;
			Gizmos.DrawLine(position, position + base.transform.up * gizmosYScale);
			Gizmos.DrawLine(position, position + base.transform.right * gizmosXScale);
			Vector3 from = position;
			for (int i = 0; i <= debugGizmosCount; i++)
			{
				float num = (float)i / (float)debugGizmosCount;
				float num2 = DragCurveValue(SurfaceCurves, num, debugGizmosMach);
				Vector3 vector = position + new Vector3(num * gizmosXScale, num2 * gizmosYScale, 0f);
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
		}
	}

	public void OnFlightGlobalsReady(bool isReady)
	{
		CalculateValues();
	}

	[ContextMenu("Load Database")]
	public bool LoadDatabase()
	{
		LoadDefaultLiftingSurfaceCurves();
		if (File.Exists(physicsDatabaseFilename))
		{
			ConfigNode configNode = ConfigNode.Load(physicsDatabaseFilename);
			if (configNode != null)
			{
				Debug.Log("PhysicsGlobals: Loading database");
				LoadDatabase(configNode);
				return true;
			}
		}
		return false;
	}

	public void LoadDatabase(ConfigNode node)
	{
		string value = node.GetValue("aeroFXStartThermalFX");
		if (value != null)
		{
			aeroFXStartThermalFX = float.Parse(value);
		}
		value = node.GetValue("aeroFXFullThermalFX");
		if (value != null)
		{
			aeroFXFullThermalFX = float.Parse(value);
		}
		value = node.GetValue("aeroFXVelocityExponent");
		if (value != null)
		{
			aeroFXVelocityExponent = double.Parse(value);
		}
		value = node.GetValue("aeroFXDensityExponent1");
		if (value != null)
		{
			aeroFXDensityExponent1 = double.Parse(value);
		}
		value = node.GetValue("aeroFXDensityScalar1");
		if (value != null)
		{
			aeroFXDensityScalar1 = double.Parse(value);
		}
		value = node.GetValue("aeroFXDensityExponent2");
		if (value != null)
		{
			aeroFXDensityExponent2 = double.Parse(value);
		}
		value = node.GetValue("aeroFXDensityScalar2");
		if (value != null)
		{
			aeroFXDensityScalar2 = double.Parse(value);
		}
		value = node.GetValue("aeroFXMachFXFadeStart");
		if (value != null)
		{
			aeroFXMachFXFadeStart = float.Parse(value);
		}
		value = node.GetValue("aeroFXMachFXFadeEnd");
		if (value != null)
		{
			aeroFXMachFXFadeEnd = float.Parse(value);
		}
		value = node.GetValue("aeroFXDensityFadeStart");
		if (value != null)
		{
			aeroFXDensityFadeStart = double.Parse(value);
		}
		value = node.GetValue("blackBodyRadiationMin");
		if (value != null)
		{
			blackBodyRadiationMin = float.Parse(value);
		}
		value = node.GetValue("blackBodyRadiationMax");
		if (value != null)
		{
			blackBodyRadiationMax = float.Parse(value);
		}
		value = node.GetValue("blackBodyRadiationAlphaMult");
		if (value != null)
		{
			blackBodyRadiationAlphaMult = float.Parse(value);
		}
		value = node.GetValue("temperatureGaugeThreshold");
		if (value != null)
		{
			temperatureGaugeThreshold = float.Parse(value);
		}
		value = node.GetValue("temperatureGaugeHighlightThreshold");
		if (value != null)
		{
			temperatureGaugeHighlightThreshold = float.Parse(value);
		}
		value = node.GetValue("thermalIntegrationMinStep");
		if (value != null)
		{
			thermalIntegrationMinStep = double.Parse(value);
		}
		value = node.GetValue("thermalIntegrationMaxTimeOnePass");
		if (value != null)
		{
			thermalIntegrationMaxTimeOnePass = double.Parse(value);
		}
		value = node.GetValue("thermalIntegrationAlwaysRK2");
		if (value != null)
		{
			thermalIntegrationAlwaysRK2 = bool.Parse(value);
		}
		value = node.GetValue("occlusionMinStep");
		if (value != null)
		{
			occlusionMinStep = double.Parse(value);
		}
		value = node.GetValue("thermalIntegrationHighMaxPasses");
		if (value != null)
		{
			thermalIntegrationHighMaxPasses = int.Parse(value);
		}
		value = node.GetValue("thermalIntegrationHighMinPasses");
		if (value != null)
		{
			thermalIntegrationHighMinPasses = int.Parse(value);
		}
		value = node.GetValue("thermalConvergenceFactor");
		if (value != null)
		{
			thermalConvergenceFactor = double.Parse(value);
		}
		value = node.GetValue("standardSpecificHeatCapacity");
		if (value != null)
		{
			standardSpecificHeatCapacity = double.Parse(value);
		}
		value = node.GetValue("internalHeatProductionFactor");
		if (value != null)
		{
			internalHeatProductionFactor = double.Parse(value);
		}
		value = node.GetValue("spaceTemperature");
		if (value != null)
		{
			spaceTemperature = double.Parse(value);
		}
		value = node.GetValue("solarLuminosityAtHome");
		if (value != null)
		{
			solarLuminosityAtHome = double.Parse(value);
		}
		value = node.GetValue("solarInsolationAtHome");
		if (value != null)
		{
			solarInsolationAtHome = double.Parse(value);
		}
		value = node.GetValue("radiationFactor");
		if (value != null)
		{
			radiationFactor = double.Parse(value);
		}
		value = node.GetValue("convectionFactorSplashed");
		if (value != null)
		{
			convectionFactorSplashed = double.Parse(value);
		}
		value = node.GetValue("fullConvectionAreaMin");
		if (value != null)
		{
			fullConvectionAreaMin = double.Parse(value);
		}
		value = node.GetValue("fullToCrossSectionLerpStart");
		if (value != null)
		{
			fullToCrossSectionLerpStart = double.Parse(value);
		}
		value = node.GetValue("fullToCrossSectionLerpEnd");
		if (value != null)
		{
			fullToCrossSectionLerpEnd = double.Parse(value);
		}
		value = node.GetValue("newtonianTemperatureFactor");
		if (value != null)
		{
			newtonianTemperatureFactor = double.Parse(value);
		}
		value = node.GetValue("newtonianConvectionFactorBase");
		if (value != null)
		{
			newtonianConvectionFactorBase = double.Parse(value);
		}
		value = node.GetValue("newtonianConvectionFactorTotal");
		if (value != null)
		{
			newtonianConvectionFactorTotal = double.Parse(value);
		}
		value = node.GetValue("newtonianDensityExponent");
		if (value != null)
		{
			newtonianDensityExponent = double.Parse(value);
		}
		value = node.GetValue("newtonianVelocityExponent");
		if (value != null)
		{
			newtonianVelocityExponent = double.Parse(value);
		}
		value = node.GetValue("newtonianMachTempLerpStartMach");
		if (value != null)
		{
			newtonianMachTempLerpStartMach = double.Parse(value);
		}
		value = node.GetValue("newtonianMachTempLerpEndMach");
		if (value != null)
		{
			newtonianMachTempLerpEndMach = double.Parse(value);
		}
		value = node.GetValue("newtonianMachTempLerpExponent");
		if (value != null)
		{
			newtonianMachTempLerpExponent = double.Parse(value);
		}
		value = node.GetValue("machConvectionFactor");
		if (value != null)
		{
			machConvectionFactor = double.Parse(value);
		}
		value = node.GetValue("machConvectionDensityExponent");
		if (value != null)
		{
			machConvectionDensityExponent = double.Parse(value);
		}
		value = node.GetValue("machConvectionVelocityExponent");
		if (value != null)
		{
			machConvectionVelocityExponent = double.Parse(value);
		}
		value = node.GetValue("machTemperatureScalar");
		if (value != null)
		{
			machTemperatureScalar = double.Parse(value);
		}
		value = node.GetValue("machTemperatureVelocityExponent");
		if (value != null)
		{
			machTemperatureVelocityExponent = double.Parse(value);
		}
		value = node.GetValue("turbulentConvectionStart");
		if (value != null)
		{
			turbulentConvectionStart = double.Parse(value);
		}
		value = node.GetValue("turbulentConvectionEnd");
		if (value != null)
		{
			turbulentConvectionEnd = double.Parse(value);
		}
		value = node.GetValue("turbulentConvectionMult");
		if (value != null)
		{
			turbulentConvectionMult = double.Parse(value);
		}
		value = node.GetValue("conductionFactor");
		if (value != null)
		{
			conductionFactor = double.Parse(value);
		}
		value = node.GetValue("skinSkinConductionFactor");
		if (value != null)
		{
			skinSkinConductionFactor = double.Parse(value);
		}
		value = node.GetValue("skinInternalConductionFactor");
		if (value != null)
		{
			skinInternalConductionFactor = double.Parse(value);
		}
		value = node.GetValue("shieldedConductionFactor");
		if (value != null)
		{
			shieldedConductionFactor = double.Parse(value);
		}
		value = node.GetValue("thermalMaxIntegrationWarp");
		if (value != null)
		{
			thermalMaxIntegrationWarp = double.Parse(value);
		}
		value = node.GetValue("analyticLerpRateSkin");
		if (value != null)
		{
			analyticLerpRateSkin = double.Parse(value);
		}
		value = node.GetValue("analyticLerpRateInternal");
		if (value != null)
		{
			analyticLerpRateInternal = double.Parse(value);
		}
		value = node.GetValue("analyticConvectionSensitivityBase");
		if (value != null)
		{
			analyticConvectionSensitivityBase = double.Parse(value);
		}
		value = node.GetValue("analyticConvectionSensitivityFinal");
		if (value != null)
		{
			analyticConvectionSensitivityFinal = double.Parse(value);
		}
		value = node.GetValue("buoyancyScalar");
		if (value != null)
		{
			buoyancyScalar = double.Parse(value);
		}
		value = node.GetValue("buoyancyUseCoBOffset");
		if (value != null)
		{
			buoyancyUseCoBOffset = bool.Parse(value);
		}
		value = node.GetValue("buoyancyApplyForceOnDie");
		if (value != null)
		{
			buoyancyApplyForceOnDie = bool.Parse(value);
		}
		value = node.GetValue("buoyancyForceOffsetLerp");
		if (value != null)
		{
			buoyancyForceOffsetLerp = float.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragScalar");
		if (value != null)
		{
			buoyancyWaterDragScalar = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragScalarEnd");
		if (value != null)
		{
			buoyancyWaterDragScalarEnd = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragScalarLerp");
		if (value != null)
		{
			buoyancyWaterDragScalarLerp = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragScalarLerpDotMultBase");
		if (value != null)
		{
			buoyancyWaterDragScalarLerpDotMultBase = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragScalarLerpDotMultBase");
		if (value != null)
		{
			buoyancyWaterDragScalarLerpDotMultBase = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterLiftScalarEnd");
		if (value != null)
		{
			buoyancyWaterLiftScalarEnd = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragMinVel");
		if (value != null)
		{
			buoyancyWaterDragMinVel = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragMinVelMult");
		if (value != null)
		{
			buoyancyWaterDragMinVelMult = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragMinVelMultCOBOff");
		if (value != null)
		{
			buoyancyWaterDragMinVelMultCOBOff = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragPartVelGreaterVesselMult");
		if (value != null)
		{
			buoyancyWaterDragPartVelGreaterVesselMult = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragTimer");
		if (value != null)
		{
			buoyancyWaterDragTimer = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragMultMinForMinDot");
		if (value != null)
		{
			buoyancyWaterDragMultMinForMinDot = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterAngularDragScalar");
		if (value != null)
		{
			buoyancyWaterAngularDragScalar = double.Parse(value);
		}
		value = node.GetValue("buoyancyAngularDragMinControlSqrMag");
		if (value != null)
		{
			buoyancyAngularDragMinControlSqrMag = double.Parse(value);
		}
		value = node.GetValue("buoyancyWaterAngularDragSlow");
		if (value != null)
		{
			buoyancyWaterAngularDragSlow = float.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragSlow");
		if (value != null)
		{
			buoyancyWaterDragSlow = float.Parse(value);
		}
		value = node.GetValue("buoyancyWaterDragExtraRBDragAboveDot");
		if (value != null)
		{
			buoyancyWaterDragExtraRBDragAboveDot = double.Parse(value);
		}
		value = node.GetValue("buoyancyScaleAboveDepth");
		if (value != null)
		{
			buoyancyScaleAboveDepth = double.Parse(value);
		}
		value = node.GetValue("buoyancyDefaultVolume");
		if (value != null)
		{
			buoyancyDefaultVolume = double.Parse(value);
		}
		value = node.GetValue("buoyancyMinCrashMult");
		if (value != null)
		{
			buoyancyMinCrashMult = float.Parse(value);
		}
		value = node.GetValue("buoyancyCrashToleranceMult");
		if (value != null)
		{
			buoyancyCrashToleranceMult = float.Parse(value);
		}
		value = node.GetValue("buoyancyRange");
		if (value != null)
		{
			buoyancyRange = double.Parse(value);
		}
		value = node.GetValue("buoyancyKerbals");
		if (value != null)
		{
			buoyancyKerbals = float.Parse(value);
		}
		value = node.GetValue("buoyancyKerbalsRagdoll");
		if (value != null)
		{
			buoyancyKerbalsRagdoll = double.Parse(value);
		}
		value = node.GetValue("cameraDepthToUnlock");
		if (value != null)
		{
			cameraDepthToUnlock = float.Parse(value);
		}
		value = node.GetValue("jointBreakForceFactor");
		if (value != null)
		{
			jointBreakForceFactor = float.Parse(value);
		}
		value = node.GetValue("jointBreakTorqueFactor");
		if (value != null)
		{
			jointBreakTorqueFactor = float.Parse(value);
		}
		value = node.GetValue("rigidJointBreakForceFactor");
		if (value != null)
		{
			rigidJointBreakForceFactor = float.Parse(value);
		}
		value = node.GetValue("rigidJointBreakTorqueFactor");
		if (value != null)
		{
			rigidJointBreakTorqueFactor = float.Parse(value);
		}
		value = node.GetValue("maxAngularVelocity");
		if (value != null)
		{
			maxAngularVelocity = float.Parse(value);
		}
		value = node.GetValue("buildingImpactDamageMaxVelocityMult");
		if (value != null)
		{
			buildingImpactDamageMaxVelocityMult = float.Parse(value);
		}
		value = node.GetValue("buildingImpactDamageUseMomentum");
		if (value != null)
		{
			buildingImpactDamageUseMomentum = bool.Parse(value);
		}
		value = node.GetValue("buildingEasingInvulnerableTime");
		if (value != null)
		{
			buildingEasingInvulnerableTime = float.Parse(value);
		}
		value = node.GetValue("buildingImpactDamageMinDamageFraction");
		if (value != null)
		{
			buildingImpactDamageMinDamageFraction = float.Parse(value);
		}
		value = node.GetValue("orbitDriftFramesToWait");
		if (value != null)
		{
			orbitDriftFramesToWait = int.Parse(value);
		}
		value = node.GetValue("orbitDriftSqrThreshold");
		if (value != null)
		{
			orbitDriftSqrThreshold = double.Parse(value);
		}
		value = node.GetValue("orbitDriftAltThreshold");
		if (value != null)
		{
			orbitDriftAltThreshold = double.Parse(value);
		}
		value = node.GetValue("partMassMin");
		if (value != null)
		{
			partMassMin = float.Parse(value);
		}
		value = node.GetValue("partRBMassMin");
		if (value != null)
		{
			partRBMassMin = float.Parse(value);
		}
		value = node.GetValue("constructionWeightLimit");
		if (value != null)
		{
			constructionWeightLimit = float.Parse(value);
		}
		value = node.GetValue("constructionWeightLimitPerKerbalCombine");
		if (value != null)
		{
			constructionWeightLimitPerKerbalCombine = float.Parse(value);
		}
		value = node.GetValue("autoStrutTechRequired");
		if (value != null)
		{
			autoStrutTechRequired = value;
		}
		value = node.GetValue("showRigidJointTweakable");
		if (value != null)
		{
			showRigidJointTweakable = (Part.ShowRigidAttachmentOption)Enum.Parse(typeof(Part.ShowRigidAttachmentOption), value);
		}
		value = node.GetValue("stagingCooldownTimer");
		if (value != null)
		{
			stagingCooldownTimer = float.Parse(value);
		}
		value = node.GetValue("dragMultiplier");
		if (value != null)
		{
			dragMultiplier = float.Parse(value);
		}
		value = node.GetValue("dragCubeMultiplier");
		if (value != null)
		{
			dragCubeMultiplier = float.Parse(value);
		}
		value = node.GetValue("angularDragMultiplier");
		if (value != null)
		{
			angularDragMultiplier = float.Parse(value);
		}
		value = node.GetValue("liftMultiplier");
		if (value != null)
		{
			liftMultiplier = float.Parse(value);
		}
		value = node.GetValue("liftDragMultiplier");
		if (value != null)
		{
			liftDragMultiplier = float.Parse(value);
		}
		value = node.GetValue("bodyLiftMultiplier");
		if (value != null)
		{
			bodyLiftMultiplier = float.Parse(value);
		}
		ConfigNode node2 = node.GetNode("DRAG_TIP");
		if (node2 != null)
		{
			dragCurveTip.Load(node2);
		}
		node2 = node.GetNode("DRAG_SURFACE");
		if (node2 != null)
		{
			dragCurveSurface.Load(node2);
		}
		node2 = node.GetNode("DRAG_TAIL");
		if (node2 != null)
		{
			dragCurveTail.Load(node2);
		}
		node2 = node.GetNode("DRAG_MULTIPLIER");
		if (node2 != null)
		{
			dragCurveMultiplier.Load(node2);
		}
		node2 = node.GetNode("DRAG_CD");
		if (node2 != null)
		{
			dragCurveCd.Load(node2);
		}
		node2 = node.GetNode("DRAG_CD_POWER");
		if (node2 != null)
		{
			dragCurveCdPower.Load(node2);
		}
		node2 = node.GetNode("DRAG_PSEUDOREYNOLDS");
		if (node2 != null)
		{
			dragCurvePseudoReynolds.Load(node2);
		}
		node2 = node.GetNode("LIFTING_SURFACE_CURVES");
		if (node2 != null)
		{
			LoadLiftingSurfaceCurves(node2);
		}
		value = node.GetValue("kerbalEVADragCubeString");
		if (value != null)
		{
			kerbalEVADragCubeString = value;
		}
		value = node.GetValue("kerbalCrewMass");
		if (value != null)
		{
			kerbalCrewMass = float.Parse(value);
		}
		value = node.GetValue("perSeatReduction");
		if (value != null)
		{
			perSeatReduction = float.Parse(value);
		}
		value = node.GetValue("perCommandSeatReduction");
		if (value != null)
		{
			perCommandSeatReduction = float.Parse(value);
		}
		value = node.GetValue("kerbalGOffset");
		if (value != null)
		{
			kerbalGOffset = double.Parse(value);
		}
		value = node.GetValue("kerbalGPower");
		if (value != null)
		{
			kerbalGPower = double.Parse(value);
		}
		value = node.GetValue("kerbalGDecayPower");
		if (value != null)
		{
			kerbalGDecayPower = double.Parse(value);
		}
		value = node.GetValue("kerbalGBraveMult");
		if (value != null)
		{
			kerbalGBraveMult = double.Parse(value);
		}
		value = node.GetValue("kerbalGBadMult");
		if (value != null)
		{
			kerbalGBadMult = double.Parse(value);
		}
		value = node.GetValue("kerbalGClamp");
		if (value != null)
		{
			kerbalGClamp = double.Parse(value);
		}
		value = node.GetValue("kerbalGThresholdWarn");
		if (value != null)
		{
			kerbalGThresholdWarn = double.Parse(value);
		}
		value = node.GetValue("kerbalGThresholdLOC");
		if (value != null)
		{
			kerbalGThresholdLOC = double.Parse(value);
		}
		value = node.GetValue("kerbalGLOCTimeMult");
		if (value != null)
		{
			kerbalGLOCTimeMult = double.Parse(value);
		}
		value = node.GetValue("kerbalGLOCMaxTimeIncrement");
		if (value != null)
		{
			kerbalGLOCMaxTimeIncrement = double.Parse(value);
		}
		value = node.GetValue("kerbalGLOCBaseTime");
		if (value != null)
		{
			kerbalGLOCBaseTime = double.Parse(value);
		}
		value = node.GetValue("kerbalGClampGExperienced");
		if (value != null)
		{
			kerbalGClampGExperienced = bool.Parse(value);
		}
		value = node.GetValue("kerbalEVADynamicFriction");
		if (!string.IsNullOrEmpty(value))
		{
			kerbalEVADynamicFriction = float.Parse(value);
		}
		value = node.GetValue("kerbalEVAStaticFriction");
		if (!string.IsNullOrEmpty(value))
		{
			kerbalEVAStaticFriction = float.Parse(value);
		}
		value = node.GetValue("kerbalEVABounciness");
		if (!string.IsNullOrEmpty(value))
		{
			kerbalEVABounciness = float.Parse(value);
		}
		value = node.GetValue("kerbalEVAFrictionCombine");
		if (!string.IsNullOrEmpty(value))
		{
			kerbalEVAFrictionCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), value);
		}
		value = node.GetValue("kerbalEVABounceCombine");
		if (!string.IsNullOrEmpty(value))
		{
			kerbalEVABounceCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), value);
		}
		value = node.GetValue("commNetQTimesVelForBlackoutMin");
		if (value != null)
		{
			commNetQTimesVelForBlackoutMin = double.Parse(value);
		}
		value = node.GetValue("commNetQTimesVelForBlackoutMax");
		if (value != null)
		{
			commNetQTimesVelForBlackoutMax = double.Parse(value);
		}
		value = node.GetValue("commNetTempForBlackout");
		if (value != null)
		{
			commNetTempForBlackout = double.Parse(value);
		}
		value = node.GetValue("commNetDensityForBlackout");
		if (value != null)
		{
			commNetDensityForBlackout = double.Parse(value);
		}
		value = node.GetValue("commNetDotForBlackoutMin");
		if (value != null)
		{
			commNetDotForBlackoutMin = double.Parse(value);
		}
		value = node.GetValue("commNetDotForBlackoutMax");
		if (value != null)
		{
			commNetDotForBlackoutMax = double.Parse(value);
		}
		value = node.GetValue("commNetBlackoutThreshold");
		if (value != null)
		{
			commNetBlackoutThreshold = double.Parse(value);
		}
		ConfigNode node3 = node.GetNode("VesselRanges");
		if (node3 != null)
		{
			VesselRangesDefault.Load(node3);
		}
		value = node.GetValue("thermalDataDisplay");
		if (value != null)
		{
			thermalDataDisplay = bool.Parse(value);
		}
		value = node.GetValue("thermalColorsDebug");
		if (value != null)
		{
			thermalColorsDebug = bool.Parse(value);
		}
		value = node.GetValue("celestialBodyTargetingMode");
		if (value != null)
		{
			try
			{
				celestialBodyTargetingMode = (VesselTargetModes)Enum.Parse(typeof(VesselTargetModes), value);
			}
			catch (Exception ex)
			{
				Debug.LogError("[PhysicsGlobals]: Failed to parse " + value + " as VesselTargetModes enum, exception " + ex);
			}
		}
	}

	[ContextMenu("Save Database")]
	public void SaveDatabase()
	{
		Debug.Log("PhysicsGlobals: Saving database");
		ConfigNode configNode = new ConfigNode("PHYSICS_DATABASE");
		configNode.SetValue("aeroFXStartThermalFX", aeroFXStartThermalFX.ToString(), "Speed in mach when aeroFX starts changing from white to orange", createIfNotFound: true);
		configNode.SetValue("aeroFXFullThermalFX", aeroFXFullThermalFX.ToString(), "Speed in mach when aeroFX is fully orange", createIfNotFound: true);
		configNode.SetValue("aeroFXVelocityExponent", aeroFXVelocityExponent.ToString(), "Exponent to velocity used when calculating aeroFX strength", createIfNotFound: true);
		configNode.SetValue("aeroFXDensityScalar1", aeroFXDensityScalar1.ToString(), "The density term used for calculating aeroFX strength is (s1 * density^e1 + s2 * density^e2), this is s1", createIfNotFound: true);
		configNode.SetValue("aeroFXDensityExponent1", aeroFXDensityExponent1.ToString(), "This is e1", createIfNotFound: true);
		configNode.SetValue("aeroFXDensityScalar2", aeroFXDensityScalar2.ToString(), "This is s2", createIfNotFound: true);
		configNode.SetValue("aeroFXDensityExponent2", aeroFXDensityExponent2.ToString(), "This is e2", createIfNotFound: true);
		configNode.SetValue("aeroFXMachFXFadeStart", aeroFXMachFXFadeStart.ToString(), "Density at which condensation FX (white) starts to fade out; plasma FX never does", createIfNotFound: true);
		configNode.SetValue("aeroFXMachFXFadeEnd", aeroFXMachFXFadeEnd.ToString(), "Density at which condensation FX (white) has fully faded out", createIfNotFound: true);
		configNode.SetValue("aeroFXDensityFadeStart", aeroFXDensityFadeStart.ToString(), "Density at which aeroFX starts fading out faster--should be near the edge of the atmosphere. This creates a smooth transition rather than insta-on at high speeds.", createIfNotFound: true);
		configNode.SetValue("blackBodyRadiationMin", blackBodyRadiationMin.ToString(), "Temperature at which a part's thermal radiation becomes visibile", createIfNotFound: true);
		configNode.SetValue("blackBodyRadiationMax", blackBodyRadiationMax.ToString(), "Temperature at which the black body radiation gradient ends", createIfNotFound: true);
		configNode.SetValue("blackBodyRadiationAlphaMult", blackBodyRadiationAlphaMult.ToString(), "Multiplier to the opacity of the black body radiation glow", createIfNotFound: true);
		configNode.SetValue("temperatureGaugeThreshold", temperatureGaugeThreshold.ToString(), "When skin temp / max skin temp greater than this, gauges become visible", createIfNotFound: true);
		configNode.SetValue("temperatureGaugeHighlightThreshold", temperatureGaugeHighlightThreshold.ToString(), "When skin temp / max skin temp greater than this, edge highlighting becomes visible", createIfNotFound: true);
		configNode.SetValue("thermalIntegrationMinStep", thermalIntegrationMinStep.ToString(), "Minimum elapsed time before a thermal integration pass is run.", createIfNotFound: true);
		configNode.SetValue("thermalIntegrationMaxTimeOnePass", thermalIntegrationMaxTimeOnePass.ToString(), "Maximum time in seconds for which no more than one RK2 pass is run no matter the num-passes formula result.", createIfNotFound: true);
		configNode.SetValue("thermalIntegrationAlwaysRK2", thermalIntegrationAlwaysRK2.ToString(), "Do we always use RK2 even at 1x warp? Else we use RK1 at 1x warp.", createIfNotFound: true);
		configNode.SetValue("occlusionMinStep", occlusionMinStep.ToString(), "Minimum elapsed time before an occlusion update pass is run.", createIfNotFound: true);
		configNode.SetValue("thermalIntegrationHighMaxPasses", thermalIntegrationHighMaxPasses.ToString(), "Maximum number of thermal integration passes (Heun/RK2) when not analytic", createIfNotFound: true);
		configNode.SetValue("thermalIntegrationHighMinPasses", thermalIntegrationHighMinPasses.ToString(), "Minimum number of thermal integration passes (Heun/RK2) when not analytic", createIfNotFound: true);
		configNode.SetValue("thermalConvergenceFactor", thermalConvergenceFactor.ToString(), "Convergence factor to make Euler integration converge for the thermo systems", createIfNotFound: true);
		configNode.SetValue("standardSpecificHeatCapacity", standardSpecificHeatCapacity.ToString(), "Standard mass specific heat capacity for parts, in kJ / tonne-K", createIfNotFound: true);
		configNode.SetValue("internalHeatProductionFactor", internalHeatProductionFactor.ToString(), "Multiplier to engine heat production", createIfNotFound: true);
		configNode.SetValue("spaceTemperature", spaceTemperature.ToString(), "Temperature of the cosmic background radiation", createIfNotFound: true);
		configNode.SetValue("solarLuminosityAtHome", solarLuminosityAtHome.ToString(), "Solar flux in W/m^2 at the orbital altitude of the homeworld", createIfNotFound: true);
		configNode.SetValue("solarInsolationAtHome", solarInsolationAtHome.ToString(), "Portion of solar flux lost when transitting through an atmosphere ending at the homeworld's sea level density, with the sun assumed at zenith and the observer on the equator", createIfNotFound: true);
		configNode.SetValue("radiationFactor", radiationFactor.ToString(), "Multiplier to radiative influx and outflux", createIfNotFound: true);
		configNode.SetValue("convectionFactorSplashed", convectionFactorSplashed.ToString(), "Newtonian convection factor to use when splashed", createIfNotFound: true);
		configNode.SetValue("fullConvectionAreaMin", fullConvectionAreaMin.ToString(), "The minimum value of the interpolation between cross sectional area and full wetted area for convection", createIfNotFound: true);
		configNode.SetValue("fullToCrossSectionLerpStart", fullToCrossSectionLerpStart.ToString(), "At this mach number, convection area starts interpolating between the full wetted area and the cross-sectional area", createIfNotFound: true);
		configNode.SetValue("fullToCrossSectionLerpEnd", fullToCrossSectionLerpEnd.ToString(), "By this mach number, the interpolation will have progressed to the minimum weight for full area specified above", createIfNotFound: true);
		configNode.SetValue("newtonianTemperatureFactor", newtonianTemperatureFactor.ToString(), "Multiplier to speed in m/s used when calculating low-mach shock temperature", createIfNotFound: true);
		configNode.SetValue("newtonianConvectionFactorBase", newtonianConvectionFactorBase.ToString(), "The base convection factor for computing the low-mach convective coefficient, before forced convection bonus is applied", createIfNotFound: true);
		configNode.SetValue("newtonianConvectionFactorTotal", newtonianConvectionFactorTotal.ToString(), "The total multiplier for computing the low-mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("newtonianDensityExponent", newtonianDensityExponent.ToString(), "The exponent to density used when calculating the low-mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("newtonianVelocityExponent", newtonianVelocityExponent.ToString(), "The exponent to velocity used when calculating the low-mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("newtonianMachTempLerpStartMach", newtonianMachTempLerpStartMach.ToString(), "The mach number at which to begin lerping between thew low-mach shock temperature and convective coefficient and the high-mach ones", createIfNotFound: true);
		configNode.SetValue("newtonianMachTempLerpEndMach", newtonianMachTempLerpEndMach.ToString(), "The mach number by which only high-mach shock temperature and convective coefficient are used", createIfNotFound: true);
		configNode.SetValue("newtonianMachTempLerpExponent", newtonianMachTempLerpExponent.ToString(), "The exponent to the lerp value, powers >1 imply slow start fast finish, <1 implies fast start slow finish", createIfNotFound: true);
		configNode.SetValue("machConvectionFactor", machConvectionFactor.ToString(), "The scalar for computing the high-mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("machConvectionDensityExponent", machConvectionDensityExponent.ToString(), "The density exponent used for the high mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("machConvectionVelocityExponent", machConvectionVelocityExponent.ToString(), "The velocity exponent used for the high mach convective coefficient", createIfNotFound: true);
		configNode.SetValue("machTemperatureScalar", machTemperatureScalar.ToString(), "The scalar used when computing the high-mach shock temperature (= this * (speed in m/s)^exponent)", createIfNotFound: true);
		configNode.SetValue("machTemperatureVelocityExponent", machTemperatureVelocityExponent.ToString(), "The exponent to speed (in m/s) used when calculating the high-mach shock temperature", createIfNotFound: true);
		configNode.SetValue("turbulentConvectionStart", turbulentConvectionStart.ToString(), "The pseudo-Reynolds number (calculated as velocity * density), multiplied by the part's drag coefficient in that facing (Cd) at which to begin lerping between 1.0 and the turbulent convection multiplier below (used as a multiplier to convective coefficient)", createIfNotFound: true);
		configNode.SetValue("turbulentConvectionEnd", turbulentConvectionEnd.ToString(), "The pseudo-Reynolds number multiplied by Cd at which the multiplier becomes fully turbulentConvectionMult", createIfNotFound: true);
		configNode.SetValue("turbulentConvectionMult", turbulentConvectionMult.ToString(), "The multiplier to the convective coefficient when in turbulent flow", createIfNotFound: true);
		configNode.SetValue("conductionFactor", conductionFactor.ToString(), "Multiplier to all conduction", createIfNotFound: true);
		configNode.SetValue("skinSkinConductionFactor", skinSkinConductionFactor.ToString(), "Multiplier to skin-skin conduction, whether on the same part (exposed<->unexposed skin) or between parts' skins", createIfNotFound: true);
		configNode.SetValue("skinInternalConductionFactor", skinInternalConductionFactor.ToString(), "Multiplier to skin<->internal conduction", createIfNotFound: true);
		configNode.SetValue("shieldedConductionFactor", shieldedConductionFactor.ToString(), "Multiplier to conduction when this part's shielded status does not match the connected part's", createIfNotFound: true);
		configNode.SetValue("thermalMaxIntegrationWarp", thermalMaxIntegrationWarp.ToString(), "Maximum warp at which to use thermo integration rather than analytic thermo", createIfNotFound: true);
		configNode.SetValue("analyticLerpRateSkin", analyticLerpRateSkin.ToString(), "Lerp rate between existing and goal temperature for skin temperature", createIfNotFound: true);
		configNode.SetValue("analyticLerpRateInternal", analyticLerpRateInternal.ToString(), "Lerp rate between existing and goal temperature for internal temperature", createIfNotFound: true);
		configNode.SetValue("analyticConvectionSensitivityBase", analyticConvectionSensitivityBase.ToString(), "Convection under analytic is handled as a lerp between analytic temp and ambient temp. The lerp = 1 / ((base + (area / thermal mass)) * final * the convective coefficient [see low-mach convection above]", createIfNotFound: true);
		configNode.SetValue("analyticConvectionSensitivityFinal", analyticConvectionSensitivityFinal.ToString(), "See above", createIfNotFound: true);
		configNode.SetValue("buoyancyScalar", buoyancyScalar.ToString(), "Scalar to buoyancy force", createIfNotFound: true);
		configNode.SetValue("buoyancyUseCoBOffset", buoyancyUseCoBOffset.ToString(), "Do we offset the place where the buoyant force is applied based on current lowest point of part?", createIfNotFound: true);
		configNode.SetValue("buoyancyApplyForceOnDie", buoyancyApplyForceOnDie.ToString(), "Do we, when we die, apply that frame's buoyant force to our parent part or failing that its children? Otherwise parts that die on splashdown do not slow the rest of the vessel", createIfNotFound: true);
		configNode.SetValue("buoyancyForceOffsetLerp", buoyancyForceOffsetLerp.ToString(), "The lerp factor between last frame's offset and this one", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragScalar", buoyancyWaterDragScalar.ToString(), "Initial drag scalar for floating parts. The scalar is this when a part first splashes down, then slowly lerps to the End value. When the part is < buoyancyWaterDragMinVel, or no part on the vessel is splashed down, it lerps back to this value", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragScalarEnd", buoyancyWaterDragScalarEnd.ToString(), "The final scalar for drag for floating parts, see above", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragScalarLerp", buoyancyWaterDragScalarLerp.ToString(), "The rate at which the drag scalar lerps between the two values, see above", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragScalarLerpDotMultBase", buoyancyWaterDragScalarLerpDotMultBase.ToString(), "The base compoent when computing the lerp multiplier based on verticality of velocity", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragScalarLerpDotMult", buoyancyWaterDragScalarLerpDotMult.ToString(), "The multiplier to the verticality dot which is subtracted from the base to compute the multiplier to lerp rate", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterLiftScalarEnd", buoyancyWaterLiftScalarEnd.ToString(), "Lift lerps like drag, but between 0 (when first splashed or velocity low) and this value", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragMinVel", buoyancyWaterDragMinVel.ToString(), "The minimum velocity for drag to be enhanced (see Slow below), and multiplied by the below value for the lerping above.", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragMinVelMult", buoyancyWaterDragMinVelMult.ToString(), "The multiplier for minimum velocity for drag to be lerping down, below this it lerps up again (and vice versa for lift)", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragMinVelMultCOBOff", buoyancyWaterDragMinVelMultCOBOff.ToString(), "The multiplier for minimum velocity for CoB lerping", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragPartVelGreaterVesselMult", buoyancyWaterDragPartVelGreaterVesselMult.ToString(), "The multiplier to vessel velocity when checking part velocity > this, to enable jumpiness damping", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragTimer", buoyancyWaterDragTimer.ToString(), "The time in seconds for which 'early' splashdown drag/lift is observed (i.e. stays high for high dot, starts low for low dot)", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragMultMinForMinDot", buoyancyWaterDragMultMinForMinDot.ToString(), "The minimum multiplier to maximum drag to clamp things to during the early splashdown (the remainder is lerped by dot)", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterAngularDragScalar", buoyancyWaterAngularDragScalar.ToString(), "Scalar to angular drag for splashed parts", createIfNotFound: true);
		configNode.SetValue("buoyancyAngularDragMinControlSqrMag", buoyancyAngularDragMinControlSqrMag.ToString(), "Minimum square-magnitude of control actuation to disable extra damping angular drag", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterAngularDragSlow", buoyancyWaterAngularDragSlow.ToString(), "Unity angular drag when part velocity < MinVel", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragSlow", buoyancyWaterDragSlow.ToString(), "Unity drag addition when part velocity < MinVel", createIfNotFound: true);
		configNode.SetValue("buoyancyWaterDragExtraRBDragAboveDot", buoyancyWaterDragExtraRBDragAboveDot.ToString(), "Extra RB drag is applied (dot - this), when dot is above this value", createIfNotFound: true);
		configNode.SetValue("buoyancyScaleAboveDepth", buoyancyScaleAboveDepth.ToString(), "An easing factor. Force of buoyancy ramps between 0 and its full value as the maximum depth below sea level of the part goes from 0 to this depth", createIfNotFound: true);
		configNode.SetValue("buoyancyDefaultVolume", buoyancyDefaultVolume.ToString(), "Default volume used when part has neither dragcube nor colliders", createIfNotFound: true);
		configNode.SetValue("buoyancyMinCrashMult", buoyancyMinCrashMult.ToString(), "The downwards component of velocity is used, not the whole velocity, when seeing if a part is destoryed when splashing down. However, that component will be clamped to no lower a portion of total velocity than this value", createIfNotFound: true);
		configNode.SetValue("buoyancyCrashToleranceMult", buoyancyCrashToleranceMult.ToString(), "Multiplier to crash tolerance used when checking if a part is destroyed on splashdown", createIfNotFound: true);
		configNode.SetValue("buoyancyRange", buoyancyRange.ToString(), "If altitude of a part is greater than this above sea level, no expensive buoyancy checks are run", createIfNotFound: true);
		configNode.SetValue("buoyancyKerbals", buoyancyKerbals.ToString(), "Buoyancy multiplier for kerbals", createIfNotFound: true);
		configNode.SetValue("buoyancyKerbalsRagdoll", buoyancyKerbalsRagdoll.ToString(), "Buoyancy multiplier for kerbals when ragdolling", createIfNotFound: true);
		configNode.SetValue("cameraDepthToUnlock", cameraDepthToUnlock.ToString(), "Meters below sea level before camera rotation unlocks", createIfNotFound: true);
		configNode.SetValue("jointBreakForceFactor", jointBreakForceFactor.ToString(), "Joint break force factor", createIfNotFound: true);
		configNode.SetValue("jointBreakTorqueFactor", jointBreakTorqueFactor.ToString(), "Joint break torque factor", createIfNotFound: true);
		configNode.SetValue("rigidJointBreakForceFactor", rigidJointBreakForceFactor.ToString(), "Joint break force factor", createIfNotFound: true);
		configNode.SetValue("rigidJointBreakTorqueFactor", rigidJointBreakTorqueFactor.ToString(), "Joint break torque factor", createIfNotFound: true);
		configNode.SetValue("maxAngularVelocity", maxAngularVelocity.ToString(), "Max angular velocity of objects in radians / sec", createIfNotFound: true);
		configNode.SetValue("buildingImpactDamageMaxVelocityMult", buildingImpactDamageMaxVelocityMult.ToString(), "Max velocity multiplier (impact vs whole part and vs whole vessel velocity) for an impact velocity. Note will be sqrt of this when Use Momentum is true.", createIfNotFound: true);
		configNode.SetValue("buildingImpactDamageUseMomentum", buildingImpactDamageUseMomentum.ToString(), "By default impact damage uses kinetic energy. Set to true to use momentum (prior-to-1.1.1 behavior)", createIfNotFound: true);
		configNode.SetValue("buildingEasingInvulnerableTime", buildingEasingInvulnerableTime.ToString(), "Seconds buildings stay invulnerable for when the active vessel goes off rails, to protect against physics jerks", createIfNotFound: true);
		configNode.SetValue("buildingImpactDamageMinDamageFraction", buildingImpactDamageMinDamageFraction.ToString(), "Damage energy below this value will not be applied to buildings.", createIfNotFound: true);
		configNode.SetValue("orbitDriftFramesToWait", orbitDriftFramesToWait.ToString(), "Number of frames to wait once drift error threshold is met before drift compensation turns on", createIfNotFound: true);
		configNode.SetValue("orbitDriftSqrThreshold", orbitDriftSqrThreshold.ToString(), "Square of the magnitude of the position error vector to use as threshold for drift compensation. If the error in position between the current position and the calculated rails position is < this, then drift compensation will be engaged.", createIfNotFound: true);
		configNode.SetValue("orbitDriftAltThreshold", orbitDriftAltThreshold.ToString(), "Orbit radius threshold for drift compensation. If the orbital radius is less than this, then drift compensation will be engaged.", createIfNotFound: true);
		configNode.SetValue("partMassMin", partMassMin.ToString(), "Minimum mass that a part can have, this is the default is minimumMass is not defined in the part cfg.", createIfNotFound: true);
		configNode.SetValue("partRBMassMin", partRBMassMin.ToString(), "Minimum mass that a parts RigidBody can have - If this is too small then PhysX will not behave when it is dropped as a vessel, this is the default if minimumRBMass is not defined in the part cfg.", createIfNotFound: true);
		configNode.SetValue("constructionWeightLimit", constructionWeightLimit.ToString(), "The weight (not mass) limit a kerbal can construct with. It is the parts mass times the gravity of the situation in Newtons.", createIfNotFound: true);
		configNode.SetValue("constructionWeightLimitPerKerbalCombine", constructionWeightLimitPerKerbalCombine.ToString(), "The weight (in Newtons) each assisting kerbal provides when manipulating parts in Construction Mode", createIfNotFound: true);
		configNode.SetValue("autoStrutTechRequired", autoStrutTechRequired, "The technology required before autostruts become available (if they are on).", createIfNotFound: true);
		configNode.SetValue("showRigidJointTweakable", showRigidJointTweakable.ToString(), "Is the rigid joint tweakable displayed. Never, Editor or Always", createIfNotFound: true);
		configNode.SetValue("stagingCooldownTimer", stagingCooldownTimer.ToString(), "The time in seconds after staging during which one cannot stage again.", createIfNotFound: true);
		configNode.SetValue("kerbalEVADragCubeString", kerbalEVADragCubeString, "The drag cube kerbals use", createIfNotFound: true);
		configNode.SetValue("kerbalCrewMass", kerbalCrewMass.ToString(), "The mass of a kerbal when in a part (pod, lander can, etc). Independent of Kerbal EVA mass, not used on EVA.", createIfNotFound: true);
		configNode.SetValue("perSeatReduction", perSeatReduction.ToString(), "Mass reduction for crewable parts to account for crew member mass. This value is per available seat and includes just the kerbal.", createIfNotFound: true);
		configNode.SetValue("perCommandSeatReduction", perCommandSeatReduction.ToString(), "Mass reduction for crewable parts to account for crew member mass. This value is per available seat and includes the kerbal, parachute and jetpack.", createIfNotFound: true);
		configNode.SetValue("kerbalGOffset", kerbalGOffset, "The offset to the G increment for kerbals", createIfNotFound: true);
		configNode.SetValue("kerbalGPower", kerbalGPower, "The exponent applied to the current G force", createIfNotFound: true);
		configNode.SetValue("kerbalGDecayPower", kerbalGDecayPower, "The further exponent applied to the increment when it is negative", createIfNotFound: true);
		configNode.SetValue("kerbalGPower", kerbalGPower, "The exponent applied to the current G force", createIfNotFound: true);
		configNode.SetValue("kerbalGClamp", kerbalGClamp, "G forces above this are clamped to this for kerbal Gs", createIfNotFound: true);
		configNode.SetValue("kerbalGBraveMult", kerbalGBraveMult, "The multiplier to thresholds based on kerbal courage", createIfNotFound: true);
		configNode.SetValue("kerbalGBadMult", kerbalGBadMult, "The multiplier to thresholds based on kerbal badS", createIfNotFound: true);
		configNode.SetValue("kerbalGThresholdWarn", kerbalGThresholdWarn, "The threshold beyond which a warning is shown", createIfNotFound: true);
		configNode.SetValue("kerbalGThresholdLOC", kerbalGThresholdLOC, "The threshold beyond which the kerbal loses consciousness", createIfNotFound: true);
		configNode.SetValue("kerbalGLOCBaseTime", kerbalGLOCBaseTime, "The base time in seconds a kerbal loses consciousness", createIfNotFound: true);
		configNode.SetValue("kerbalGLOCTimeMult", kerbalGLOCTimeMult, "Multiplier to the current G experienced increment applied to unconscious time", createIfNotFound: true);
		configNode.SetValue("kerbalGLOCMaxTimeIncrement", kerbalGLOCMaxTimeIncrement, "Maximum time per second a kerbal's unconscious time can be incremented", createIfNotFound: true);
		configNode.SetValue("kerbalGClampGExperienced", kerbalGClampGExperienced, "If true, g experienced will be clamped to the LOC threshold and time unconscious will build up. If false, it will not be clamped and time uncouncsious will be just how long the kerbal is above the warn threshold, so long-sustained high Gs will last longer", createIfNotFound: true);
		configNode.SetValue("kerbalEVADynamicFriction", kerbalEVADynamicFriction, "The KerbalEVA physic material dynamic friction value, affects sliding when ragdolled", createIfNotFound: true);
		configNode.SetValue("kerbalEVAStaticFriction", kerbalEVAStaticFriction, "The KerbalEVA physic material static friction value, affects sliding when ragdolled", createIfNotFound: true);
		configNode.SetValue("kerbalEVABounciness", kerbalEVABounciness, "The KerbalEVA physic material bouncines value, affects sliding when ragdolled", createIfNotFound: true);
		configNode.SetValue("kerbalEVAFrictionCombine", kerbalEVAFrictionCombine.ToStringCached(), "The KerbalEVA physic material friction combine enumerator value, affects sliding when ragdolled", createIfNotFound: true);
		configNode.SetValue("kerbalEVABounceCombine", kerbalEVABounceCombine.ToStringCached(), "The physic KerbalEVA material bounce combine enumerator value, affects sliding when ragdolled", createIfNotFound: true);
		configNode.SetValue("commNetQTimesVelForBlackoutMin", commNetQTimesVelForBlackoutMin, "Minimum dynamic pressure * velocity for comms to start blacking out from plasma (if that option is enabled)", createIfNotFound: true);
		configNode.SetValue("commNetQTimesVelForBlackoutMax", commNetQTimesVelForBlackoutMax, "Maximum dynamic pressure * velocity for comms to start blacking out from plasma (if that option is enabled)", createIfNotFound: true);
		configNode.SetValue("commNetTempForBlackout", commNetTempForBlackout, "Minimum shock temperature for comms to start blacking out from plasma (if that option is enabled)", createIfNotFound: true);
		configNode.SetValue("commNetDensityForBlackout", commNetDensityForBlackout, "Minimum density for comms to start blacking out from plasma (if that option is enabled)", createIfNotFound: true);
		configNode.SetValue("commNetDotForBlackoutMin", commNetDotForBlackoutMin, "Minimum dot between velocity and link direction for comms to start blacking out from plasma (if that option is enabled)", createIfNotFound: true);
		configNode.SetValue("commNetDotForBlackoutMax", commNetDotForBlackoutMax, "Dot between velocity and link direction for full blackout multiplier", createIfNotFound: true);
		configNode.SetValue("commNetBlackoutThreshold", commNetBlackoutThreshold, "Threshold blackout value below which comms are entirely blacked out (if that option is enabled). Value calculated as 1 - inverse lerp of QTimesVel * inverse lerp of dot", createIfNotFound: true);
		configNode.SetValue("dragMultiplier", dragMultiplier.ToString(), "Global multiplier to drag", createIfNotFound: true);
		configNode.SetValue("dragCubeMultiplier", dragCubeMultiplier.ToString(), "Multiplier to drag from dragcubes", createIfNotFound: true);
		configNode.SetValue("angularDragMultiplier", angularDragMultiplier.ToString(), "global multiplier to angular drag", createIfNotFound: true);
		configNode.SetValue("liftMultiplier", liftMultiplier.ToString(), "Multiplier to lift from lifting/control surfaces", createIfNotFound: true);
		configNode.SetValue("liftDragMultiplier", liftDragMultiplier.ToString(), "Multiplier to drag from lifting/control surfaces", createIfNotFound: true);
		configNode.SetValue("bodyLiftMultiplier", bodyLiftMultiplier.ToString(), "Multiplier to lift from non-lifting/control-surface parts", createIfNotFound: true);
		ConfigNode node = configNode.AddNode("DRAG_TIP", "Multiplier to dragcube drag when the face points towards the velocity vector, x value is mach, y value is multiplier");
		dragCurveTip.Save(node);
		ConfigNode node2 = configNode.AddNode("DRAG_SURFACE", "Multiplier to dragcube drag when the face points orthogonal to the velocity vector, x value is mach, y value is multiplier");
		dragCurveSurface.Save(node2);
		ConfigNode node3 = configNode.AddNode("DRAG_TAIL", "Multiplier to dragcube drag when the face points away from the velocity vector, x value is mach, y value is multiplier");
		dragCurveTail.Save(node3);
		ConfigNode node4 = configNode.AddNode("DRAG_MULTIPLIER", "Overall multiplier to drag based on mach");
		dragCurveMultiplier.Save(node4);
		ConfigNode node5 = configNode.AddNode("DRAG_CD", "The final Cd of a given facing is the drag cube Cd evalauted on this curve");
		dragCurveCd.Save(node5);
		ConfigNode node6 = configNode.AddNode("DRAG_CD_POWER", "The final Cd of a given facing is then raised to this power, indexed by mach number");
		dragCurveCdPower.Save(node6);
		ConfigNode node7 = configNode.AddNode("DRAG_PSEUDOREYNOLDS", "Converts a pseudo-Reynolds number (density * velocity) into a multiplier to drag coefficient");
		dragCurvePseudoReynolds.Save(node7);
		ConfigNode node8 = configNode.AddNode("LIFTING_SURFACE_CURVES", "The lifting surface curvesets available for use");
		Dictionary<string, LiftingSurfaceCurve>.ValueCollection.Enumerator enumerator = liftingSurfaceCurves.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Save(node8);
		}
		VesselRangesDefault.Save(configNode.AddNode("VesselRanges"));
		configNode.Save(physicsDatabaseFilename);
	}

	public void LoadLiftingSurfaceCurves(ConfigNode subNode)
	{
		ConfigNode[] nodes = subNode.GetNodes("LIFTING_SURFACE");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			LiftingSurfaceCurve liftingSurfaceCurve = new LiftingSurfaceCurve();
			liftingSurfaceCurve.Load(nodes[i]);
			liftingSurfaceCurves[liftingSurfaceCurve.name] = liftingSurfaceCurve;
		}
	}

	public void LoadDefaultLiftingSurfaceCurves()
	{
		liftingSurfaceCurves.Clear();
		ConfigNode configNode = new ConfigNode();
		ConfigNode configNode2 = configNode.AddNode("LIFTING_SURFACE");
		configNode2.AddValue("name", "Default");
		ConfigNode configNode3 = configNode2.AddNode("lift");
		configNode3.AddValue("key", "0 0 0 1.965926");
		configNode3.AddValue("key", "0.258819 0.5114774 1.990092 1.905806");
		configNode3.AddValue("key", "0.5 0.9026583 0.7074468 -0.7074468");
		configNode3.AddValue("key", "0.7071068 0.5926583 -2.087948 -1.990095");
		configNode3.AddValue("key", "1 0 -2.014386 -2.014386");
		ConfigNode configNode4 = configNode2.AddNode("liftMach");
		configNode4.AddValue("key", "0 1 0 0");
		configNode4.AddValue("key", "0.3 0.5 -1.671345 -0.8273422");
		configNode4.AddValue("key", "1 0.125 -0.0005291355 -0.02625772");
		configNode4.AddValue("key", "5 0.0625 0 0");
		configNode4.AddValue("key", "25 0.05 0 0");
		ConfigNode configNode5 = configNode2.AddNode("drag");
		configNode5.AddValue("key", "0 0.01 0 0");
		configNode5.AddValue("key", "0.3420201 0.06 0.1750731 0.1750731");
		configNode5.AddValue("key", "0.5 0.24 2.60928 2.60928");
		configNode5.AddValue("key", "0.7071068 1.7 3.349777 3.349777");
		configNode5.AddValue("key", "1 2.4 1.387938 0");
		ConfigNode configNode6 = configNode2.AddNode("dragMach");
		configNode6.AddValue("key", "0 0.35 0 -0.8463008");
		configNode6.AddValue("key", "0.15 0.125 0 0");
		configNode6.AddValue("key", "0.9 0.275 0.541598 0.541598");
		configNode6.AddValue("key", "1.1 0.75 0 0");
		configNode6.AddValue("key", "1.4 0.4 -0.3626955 -0.3626955");
		configNode6.AddValue("key", "1.6 0.35 -0.1545923 -0.1545923");
		configNode6.AddValue("key", "2 0.3 -0.09013031 -0.09013031");
		configNode6.AddValue("key", "5 0.22 0 0");
		configNode6.AddValue("key", "25 0.3 0.0006807274 0");
		ConfigNode configNode7 = configNode.AddNode("LIFTING_SURFACE");
		configNode7.AddValue("name", "BodyLift");
		ConfigNode configNode8 = configNode7.AddNode("lift");
		configNode8.AddValue("key", "0 0 0 1.975376");
		configNode8.AddValue("key", "0.309017 0.5877852 1.565065 1.565065");
		configNode8.AddValue("key", "0.5877852 0.9510565 0.735902 0.735902");
		configNode8.AddValue("key", "0.7071068 1 0 0");
		configNode8.AddValue("key", "0.8910065 0.809017 -2.70827 -2.70827");
		configNode8.AddValue("key", "1 0 -11.06124 0");
		ConfigNode configNode9 = configNode7.AddNode("liftMach");
		configNode9.AddValue("key", "0.3 0.167 0 0");
		configNode9.AddValue("key", "0.8 0.167 0 -0.3904104");
		configNode9.AddValue("key", "1 0.125 -0.0005291355 -0.02625772");
		configNode9.AddValue("key", "5 0.0625 0 0");
		configNode9.AddValue("key", "25 0.05 0 0");
		configNode7.AddNode("drag").AddValue("key", "0 0 0 0");
		configNode7.AddNode("dragMach").AddValue("key", "0 0 0 0");
		ConfigNode configNode10 = configNode.AddNode("LIFTING_SURFACE");
		configNode10.AddValue("name", "CapsuleBottom");
		ConfigNode configNode11 = configNode10.AddNode("lift");
		configNode11.AddValue("key", "0 0 0 1.975376");
		configNode11.AddValue("key", "0.309017 0.5877852 1.565065 1.565065");
		configNode11.AddValue("key", "0.5877852 0.9510565 0.735902 0.735902");
		configNode11.AddValue("key", "0.7071068 1 0 0");
		configNode11.AddValue("key", "0.8910065 0.809017 -2.70827 -2.70827");
		configNode11.AddValue("key", "1 0 -11.06124 0");
		configNode10.AddNode("liftMach").AddValue("key", "0.3 0.0625 0 0");
		configNode10.AddNode("drag").AddValue("key", "0 0 0 0");
		configNode10.AddNode("dragMach").AddValue("key", "0 0 0 0");
		ConfigNode configNode12 = configNode.AddNode("LIFTING_SURFACE");
		configNode12.AddValue("name", "SpeedBrake");
		configNode12.AddNode("lift").AddValue("key", "0 0 0 0");
		configNode12.AddNode("liftMach").AddValue("key", "0 0 0 0");
		ConfigNode configNode13 = configNode12.AddNode("drag");
		configNode13.AddValue("key", "0 0.01 0 0");
		configNode13.AddValue("key", "0.3420201 0.06 0.1750731 0.1750731");
		configNode13.AddValue("key", "0.5 0.24 2.60928 2.60928");
		configNode13.AddValue("key", "0.7071068 1.7 3.349777 3.349777");
		configNode13.AddValue("key", "1 2.4 1.387938 0");
		ConfigNode configNode14 = configNode12.AddNode("dragMach");
		configNode14.AddValue("key", "0 0.35 0 -0.8463008");
		configNode14.AddValue("key", "0.15 0.125 0 0");
		configNode14.AddValue("key", "0.9 0.275 0.541598 0.541598");
		configNode14.AddValue("key", "1.1 0.75 0 0");
		configNode14.AddValue("key", "1.4 0.4 -0.3626955 -0.3626955");
		configNode14.AddValue("key", "1.6 0.35 -0.1545923 -0.1545923");
		configNode14.AddValue("key", "2 0.3 -0.09013031 -0.09013031");
		configNode14.AddValue("key", "5 0.22 0 0");
		configNode14.AddValue("key", "25 0.3 0.0006807274 0");
		LoadLiftingSurfaceCurves(configNode);
	}

	public void UpdateKerbalEVAPhysicsMaterial(float dynamicFriction, float staticFriction, float bounciness, PhysicMaterialCombine frictionCombine, PhysicMaterialCombine bounceCombine)
	{
		if (!(kerbalEVAPhysicMaterial == null))
		{
			kerbalEVADynamicFriction = dynamicFriction;
			kerbalEVAStaticFriction = staticFriction;
			kerbalEVABounciness = bounciness;
			kerbalEVAFrictionCombine = frictionCombine;
			kerbalEVABounceCombine = bounceCombine;
			kerbalEVAPhysicMaterial.dynamicFriction = kerbalEVADynamicFriction;
			kerbalEVAPhysicMaterial.staticFriction = kerbalEVAStaticFriction;
			kerbalEVAPhysicMaterial.bounciness = kerbalEVABounciness;
			kerbalEVAPhysicMaterial.frictionCombine = kerbalEVAFrictionCombine;
			kerbalEVAPhysicMaterial.bounceCombine = kerbalEVABounceCombine;
			GameEvents.onGlobalEvaPhysicMaterialChanged.Fire(kerbalEVAPhysicMaterial);
		}
	}
}
