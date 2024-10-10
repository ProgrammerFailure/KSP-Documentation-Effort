using System.Collections.Generic;
using System.IO;
using Expansions;
using FinePrint.Utilities;
using ns9;
using UniLinq;
using UnityEngine;

namespace FinePrint;

[KSPAddon(KSPAddon.Startup.MainMenu, true)]
public class ContractDefs : MonoBehaviour
{
	public static class GClass6
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2982;
		}

		public static class Funds
		{
			public static float BaseAdvance = 46000f;

			public static float BaseReward = 120000f;

			public static float BaseFailure = 0f;

			public static float SolarEjectionMultiplier = 1.25f;
		}

		public static class Science
		{
			public static float BaseReward = 16f;

			public static float SolarEjectionMultiplier = 1f;
		}

		public static class Reputation
		{
			public static float BaseReward = 26f;

			public static float BaseFailure = 13f;

			public static float SolarEjectionMultiplier = 1.25f;
		}

		public static int MaximumExistent = 9999;

		public static float SignificantSolarEjectionChance = 10f;

		public static float ExceptionalSolarEjectionChance = 20f;

		public static float HomeLandingChance = 20f;

		public static bool AllowSolarEjections = true;

		public static bool AllowHomeLandings = true;
	}

	public static class Base
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2982;
		}

		public static class Funds
		{
			public static float BaseAdvance = 36000f;

			public static float BaseReward = 91000f;

			public static float BaseFailure = 0f;

			public static float MobileMultiplier = 1.5f;
		}

		public static class Science
		{
			public static float BaseReward = 0f;

			public static float MobileMultiplier = 1.25f;
		}

		public static class Reputation
		{
			public static float BaseReward = 21f;

			public static float BaseFailure = 14f;

			public static float MobileMultiplier = 1.5f;
		}

		public static int MaximumExistent = 9999;

		public static float ContextualChance = 75f;

		public static float ContextualAssets = 2f;

		public static float TrivialMobileChance = 0f;

		public static float SignificantMobileChance = 10f;

		public static float ExceptionalMobileChance = 30f;

		public static bool AllowMobile = true;
	}

	public static class Flag
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 23000f;

			public static float BaseReward = 58000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 14f;

			public static float BaseFailure = 7f;
		}

		public static int MaximumExistent = 9999;
	}

	public static class Grand
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 7;

			public static int MaximumExpireDays = 28;

			public static int DeadlineDays = 2982;
		}

		public static class Funds
		{
			public static float BaseAdvance = 10000f;

			public static float BaseReward = 58000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 16f;

			public static float BaseFailure = 8f;
		}

		public static float Rarity = 5f;
	}

	public static class ISRU
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2982;
		}

		public static class Funds
		{
			public static float BaseAdvance = 31000f;

			public static float BaseReward = 79000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 22f;

			public static float BaseFailure = 11f;
		}

		public static int MaximumExistent = 9999;
	}

	public static class Progression
	{
		public static class Funds
		{
			public static float BaseReward = 80000f;
		}

		public static class Science
		{
			public static float BaseReward = 8f;
		}

		public static class Reputation
		{
			public static float BaseReward = 16f;
		}

		public static bool DisableTutorialContracts = false;

		public static bool DisableProgressionContracts = false;

		public static double MaxDepthRecord = 750.0;

		public static double MaxDistanceRecord = 100000.0;

		public static double MaxSpeedRecord = 2500.0;

		public static float OutlierMilestoneMultiplier = 1.5f;

		public static float PassiveBaseRatio = 0.2f;

		public static float PassiveBodyRatio = 0.3f;

		public static int RecordSplit = 5;
	}

	public static class Recovery
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 26000f;

			public static float BaseReward = 66000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 15f;

			public static float BaseFailure = 15f;
		}

		public static int MaximumAvailable = 9999;

		public static int MaximumActive = 9999;

		public static bool AllowKerbalRescue = true;

		public static bool AllowPartRecovery = true;

		public static bool AllowCompoundRecovery = true;

		public static bool AllowLandedVacuum = true;

		public static bool AllowLandedAtmosphere = true;

		public static float HighOrbitDifficulty = 0.3f;
	}

	public static class Satellite
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 16000f;

			public static float BaseReward = 41000f;

			public static float BaseFailure = 0f;

			public static float HomeMultiplier = 2f;

			public static float PolarMultiplier = 1f;

			public static float SynchronousMultiplier = 1.1f;

			public static float StationaryMultiplier = 1.2f;

			public static float TundraMultiplier = 1.2f;

			public static float KolniyaMultiplier = 1.2f;
		}

		public static class Science
		{
			public static float BaseReward = 0f;

			public static float HomeMultiplier = 0.5f;

			public static float PolarMultiplier = 1f;

			public static float SynchronousMultiplier = 1.1f;

			public static float StationaryMultiplier = 1.2f;

			public static float TundraMultiplier = 1.2f;

			public static float KolniyaMultiplier = 1.2f;
		}

		public static class Reputation
		{
			public static float BaseReward = 10f;

			public static float BaseFailure = 5f;

			public static float HomeMultiplier = 1.25f;

			public static float PolarMultiplier = 1f;

			public static float SynchronousMultiplier = 1.1f;

			public static float StationaryMultiplier = 1.2f;

			public static float TundraMultiplier = 1.2f;

			public static float KolniyaMultiplier = 1.2f;
		}

		public static int MaximumAvailable = 9999;

		public static int MaximumActive = 9999;

		public static float AnimationDuration = 8f;

		public static float ContextualChance = 50f;

		public static float ContextualAssets = 2f;

		public static float ContextualHomeAssets = 4f;

		public static int NetworkMinimum = 3;

		public static int NetworkMaximum = 4;

		public static double MinimumDeviationWindow = 750.0;

		public static double TrivialDeviation = 7.0;

		public static double SignificantDeviation = 5.0;

		public static double ExceptionalDeviation = 3.0;

		public static float TrivialAltitudeDifficulty = 0.2f;

		public static float TrivialInclinationDifficulty = 0.2f;

		public static float SignificantAltitudeDifficulty = 0.4f;

		public static float SignificantInclinationDifficulty = 0.4f;

		public static float ExceptionalAltitudeDifficulty = 0.8f;

		public static float ExceptionalInclinationDifficulty = 0.8f;

		public static bool PreferHome = true;

		public static bool AllowSolar = true;

		public static bool AllowEquatorial = true;

		public static bool AllowPolar = true;

		public static bool AllowSynchronous = true;

		public static bool AllowStationary = true;

		public static bool AllowTundra = true;

		public static bool AllowKolniya = true;
	}

	public static class Research
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 9000f;

			public static float BaseReward = 22000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward = 3f;
		}

		public static class Reputation
		{
			public static float BaseReward = 6f;

			public static float BaseFailure = 3f;
		}

		public static int MaximumExistent = 9999;
	}

	public static class Sentinel
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 9000f;

			public static float BaseReward = 22000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward = 3f;
		}

		public static class Reputation
		{
			public static float BaseReward = 6f;

			public static float BaseFailure = 3f;
		}

		public static class Trivial
		{
			public static float FundsMultiplier = 1.15f;

			public static float ScienceMultiplier = 1.05f;

			public static float ReputationMultiplier = 1.05f;

			public static int MinAsteroids = 5;

			public static int MaxAsteroids = 11;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 1.15f;

			public static float ScienceMultiplier = 1.05f;

			public static float ReputationMultiplier = 1.05f;

			public static int MinAsteroids = 10;

			public static int MaxAsteroids = 16;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 1.15f;

			public static float ScienceMultiplier = 1.05f;

			public static float ReputationMultiplier = 1.05f;

			public static int MinAsteroids = 15;

			public static int MaxAsteroids = 21;
		}

		public static int MaximumAvailable = 2;

		public static int MaximumActive = 3;

		public static float ScanTypeBaseMultiplier = 0.1f;

		public static float ScanTypeClassMultiplier = 0.4f;

		public static float ScanTypeInclinationMultiplier = 0.5f;

		public static float ScanTypeEccentricityMultiplier = 0.5f;

		public static int TrivialDeviation = 7;

		public static int SignificantDeviation = 5;

		public static int ExceptionalDeviation = 3;
	}

	public static class CometDetection
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 18000f;

			public static float BaseReward = 26000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward = 6f;
		}

		public static class Reputation
		{
			public static float BaseReward = 10f;

			public static float BaseFailure = 5f;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 2f;

			public static float ScienceMultiplier = 1.5f;

			public static float ReputationMultiplier = 1.5f;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 3f;

			public static float ScienceMultiplier = 2f;

			public static float ReputationMultiplier = 2f;
		}

		public static int MaximumAvailable = 1;

		public static int MaximumActive = 1;
	}

	public static class Station
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2982;
		}

		public static class Funds
		{
			public static float BaseAdvance = 32000f;

			public static float BaseReward = 83000f;

			public static float BaseFailure = 0f;

			public static float AsteroidMultiplier = 1.2f;
		}

		public static class Science
		{
			public static float BaseReward = 0f;

			public static float AsteroidMultiplier = 1.3f;
		}

		public static class Reputation
		{
			public static float BaseReward = 18f;

			public static float BaseFailure = 9f;

			public static float AsteroidMultiplier = 1.3f;
		}

		public static int MaximumExistent = 9999;

		public static float ContextualChance = 75f;

		public static float ContextualAssets = 2f;

		public static float TrivialAsteroidChance = 0f;

		public static float SignificantAsteroidChance = 10f;

		public static float ExceptionalAsteroidChance = 20f;

		public static bool AllowAsteroid = true;

		public static bool AllowSolar = true;
	}

	public static class Survey
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float DefaultAdvance = 30000f;

			public static float DefaultReward = 53000f;

			public static float DefaultFailure = 0f;

			public static float WaypointDefaultReward = 8000f;
		}

		public static class Science
		{
			public static float DefaultReward = 0f;

			public static float WaypointDefaultReward = 3f;
		}

		public static class Reputation
		{
			public static float DefaultReward = 9f;

			public static float DefaultFailure = 9f;

			public static float WaypointDefaultReward = 3f;
		}

		public static int MaximumAvailable = 9999;

		public static int MaximumActive = 9999;

		public static float ContextualChance = 50f;

		public static float ContextualAssets = 2f;

		public static int TrivialWaypoints = 1;

		public static int SignificantWaypoints = 2;

		public static int ExceptionalWaypoints = 3;

		public static float HomeNearbyProgressCap = 7f;

		public static double TrivialRange = 2000.0;

		public static double SignificantRange = 4000.0;

		public static double ExceptionalRange = 6000.0;

		public static double MinimumTriggerRange = 500.0;

		public static double MaximumTriggerRange = 15000.0;

		public static double MinimumThreshold = 1400.0;

		public static double MaximumThreshold = 30000.0;

		public static float ThresholdDeviancy = 10f;
	}

	public static class Test
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 3;

			public static int DeadlineDays = 426;
		}

		public static class Funds
		{
			public static float BaseAdvance = 18000f;

			public static float BaseReward = 45000f;

			public static float BaseFailure = 18000f;
		}

		public static class Science
		{
			public static float BaseReward = 6f;
		}

		public static class Reputation
		{
			public static float BaseReward = 12f;

			public static float BaseFailure = 9f;
		}

		public static int MaximumExistent = 9999;

		public static int SubjectsToRepeat = 6;

		public static bool AllowHauls = true;

		public static int TrivialHaulChance = 50;

		public static int SignificantHaulChance = 25;

		public static int ExceptionalHaulChance = 0;
	}

	public static class Tour
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float DefaultFare = 13000f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 20f;

			public static float BaseFailure = 10f;
		}

		public static int MaximumAvailable = 9999;

		public static int MaximumActive = 9999;

		public static bool FailOnInactive = true;

		public static float GeeAdventureChance = 0.15f;
	}

	public static class DeployedScience
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float BaseAdvance = 10000f;

			public static float BaseReward = 21000f;

			public static float BaseFailure = 12000f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 6f;

			public static float BaseFailure = 3f;
		}

		public static int MaximumExistent = 5;

		public static float SciencePercentage = 50f;
	}

	public static class ROCScienceArm
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float SimpleArmAdvance = 8000f;

			public static float SimpleArmReward = 17000f;

			public static float SimpleArmFailure = 10000f;

			public static float ComplexArmAdvance = 10000f;

			public static float ComplexArmReward = 21000f;

			public static float ComplexArmFailure = 12000f;

			public static float AdvancedArmAdvance = 12000f;

			public static float AdvancedArmReward = 25000f;

			public static float AdvancedArmFailure = 17000f;
		}

		public static class Science
		{
			public static float SimpleArmReward;

			public static float ComplexArmReward;

			public static float AdvancedArmReward;
		}

		public static class Reputation
		{
			public static float SimpleArmReward = 2f;

			public static float SimpleArmFailure = 2f;

			public static float ComplexArmReward = 2f;

			public static float ComplexArmFailure = 2f;

			public static float AdvancedArmReward = 2f;

			public static float AdvancedArmFailure = 2f;
		}

		public static int MaximumExistent = 3;

		public static float SimpleArmPercentage = 40f;

		public static float ComplexArmPercentage = 70f;
	}

	public static class ROCScienceRetrieval
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 2130;
		}

		public static class Funds
		{
			public static float Advance = 22000f;

			public static float Reward = 35000f;

			public static float Failure = 27000f;
		}

		public static class Science
		{
			public static float Reward;
		}

		public static class Reputation
		{
			public static float Reward = 2f;

			public static float Failure = 2f;
		}

		public static int MaximumExistent = 2;
	}

	public static class CometSample
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 7200;
		}

		public static class Funds
		{
			public static float BaseAdvance = 22000f;

			public static float BaseReward = 40000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward = 25f;
		}

		public static class Reputation
		{
			public static float BaseReward = 25f;

			public static float BaseFailure = 10f;
		}

		public static class Trivial
		{
			public static float FundsMultiplier = 1f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 1f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 2f;

			public static float ScienceMultiplier = 1.5f;

			public static float ReputationMultiplier = 1.5f;
		}

		public static int MaximumAvailable = 1;

		public static int MaximumActive = 1;

		public static float SciencePercentage = 50f;
	}

	public static class RoverConstruction
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 852;
		}

		public static class Funds
		{
			public static float BaseAdvance = 32000f;

			public static float BaseReward = 100000f;

			public static float BaseFailure = 0f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 2f;

			public static float BaseFailure = 1f;
		}

		public static class Trivial
		{
			public static float FundsMultiplier = 1f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float WayPointMultiplier = 1f;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 1.25f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float WayPointMultiplier = 2f;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 2f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 2f;

			public static float WayPointMultiplier = 3f;
		}

		public static int MaximumAvailable = 1;

		public static int MaximumActive = 1;

		public static float WayPointMinDistance = 900f;

		public static float WayPointMaxDistance = 1100f;

		public static float WayPointTriggerRange = 50f;

		public static float MinimumGeeASL = 0.02f;

		public static float MaximumGeeASL = 0.4f;

		public static string TechNodeRequired = "advExploration";
	}

	public static class VesselRepair
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 852;
		}

		public static class Funds
		{
			public static float BaseAdvance = 22000f;

			public static float BaseReward = 40000f;

			public static float BaseFailure = 30000f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 2f;

			public static float BaseFailure = 1f;
		}

		public static class Trivial
		{
			public static float FundsMultiplier = 1f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.33f;

			public static float OrbitInclinationFactor = 0.25f;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 1.25f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.5f;

			public static float OrbitInclinationFactor = 1f;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 2f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 2f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.8f;

			public static float OrbitInclinationFactor = 1f;
		}

		public static int MaximumAvailable = 1;

		public static int MaximumActive = 1;
	}

	public static class OrbitalConstruction
	{
		public static class Expire
		{
			public static int MinimumExpireDays = 1;

			public static int MaximumExpireDays = 7;

			public static int DeadlineDays = 852;
		}

		public static class Funds
		{
			public static float BaseAdvance = 23000f;

			public static float BaseReward = 42000f;

			public static float BaseFailure = 25000f;
		}

		public static class Science
		{
			public static float BaseReward;
		}

		public static class Reputation
		{
			public static float BaseReward = 2f;

			public static float BaseFailure = 1f;
		}

		public static class Trivial
		{
			public static float FundsMultiplier = 1f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.33f;

			public static float OrbitInclinationFactor = 0.25f;
		}

		public static class Significant
		{
			public static float FundsMultiplier = 1.25f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 1f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.5f;

			public static float OrbitInclinationFactor = 1f;
		}

		public static class Exceptional
		{
			public static float FundsMultiplier = 2f;

			public static float ScienceMultiplier = 1f;

			public static float ReputationMultiplier = 2f;

			public static float MaxOrbitEccentricityFactor = 0.05f;

			public static float OrbitAltitudeFactor = 0.8f;

			public static float OrbitInclinationFactor = 1f;
		}

		public static int MaximumAvailable = 1;

		public static int MaximumActive = 1;

		public static string TechNodeRequired = "advConstruction";
	}

	public static ConfigNode config;

	public static ContractDefs instance;

	public static SpriteMap sprites;

	public const string textureBaseUrl = "Squad/Contracts/Icons/";

	public const string configPath = "GameData/Squad/Contracts/Contracts.cfg";

	public const string ObjectiveAntenna = "Antenna";

	public const string ObjectiveBattery = "Battery";

	public const string ObjectiveDock = "Dock";

	public const string ObjectiveGenerator = "Generator";

	public const string ObjectiveGrapple = "Grapple";

	public const string ObjectiveWheel = "Wheel";

	public static int WeightDefault = 30;

	public static int WeightMinimum = 10;

	public static int WeightMaximum = 90;

	public static int WeightAcceptDelta = 12;

	public static int WeightDeclineDelta = -8;

	public static int WeightWithdrawReadDelta = -2;

	public static int WeightWithdrawSeenDelta = -1;

	public static bool DisplayOfferedOrbits = true;

	public static bool DisplayOfferedWaypoints = true;

	public static bool SurveyNavigationGhosting = false;

	public static int AverageAvailableContracts = 10;

	public static float FacilityProgressionFactor = 0.2f;

	public static float SolarOrbitHeatTolerance = 800f;

	public static string PolarOrbitName;

	public static string EquatorialOrbitName;

	public static string TundraOrbitName;

	public static string SunStationaryName;

	public static string HomeStationaryName;

	public static string OtherStationaryName;

	public static string SunSynchronousName;

	public static string HomeSynchronousName;

	public static string OtherSynchronousName;

	public static string MolniyaName;

	public static PreBuiltCraft PreBuiltCraftDefs;

	public static DictionaryValueList<string, List<PreBuiltCraftPosition>> PreBuiltCraftPositions;

	public static ConstructionParts constructionParts;

	public static List<SurveyDefinition> surveyDefinitions;

	public static bool loadedContractCraftDefs = false;

	public static bool loadedConstructionPartsList = false;

	public static ContractDefs Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			instance = new GameObject("ContractDefs").AddComponent<ContractDefs>();
			return instance;
		}
	}

	public static List<SurveyDefinition> SurveyDefinitions
	{
		get
		{
			if (surveyDefinitions == null)
			{
				surveyDefinitions = new List<SurveyDefinition>();
				ConfigNode node = config.GetNode("Survey");
				if (node != null)
				{
					ConfigNode[] nodes = node.GetNodes("SURVEY_DEFINITION");
					int num = nodes.Length;
					while (num-- > 0)
					{
						surveyDefinitions.Add(new SurveyDefinition(nodes[num]));
					}
				}
			}
			List<SurveyDefinition> list = new List<SurveyDefinition>();
			int count = surveyDefinitions.Count;
			while (count-- > 0)
			{
				list.Add(new SurveyDefinition(surveyDefinitions[count]));
			}
			return list;
		}
	}

	public void LoadDefinitions()
	{
		PolarOrbitName = Localizer.Format("#autoLOC_7001202");
		EquatorialOrbitName = Localizer.Format("#autoLOC_7001201");
		TundraOrbitName = Localizer.Format("#autoLOC_7001204");
		SunStationaryName = Localizer.Format("#autoLOC_7001205");
		HomeStationaryName = Localizer.Format("#autoLOC_7001206");
		OtherStationaryName = Localizer.Format("#autoLOC_7001207");
		SunSynchronousName = Localizer.Format("#autoLOC_7001208");
		HomeSynchronousName = Localizer.Format("#autoLOC_7001209");
		OtherSynchronousName = Localizer.Format("#autoLOC_7001210");
		MolniyaName = Localizer.Format("#autoLOC_7001203");
	}

	public void Awake()
	{
		sprites = new SpriteMap();
		instance = this;
		Object.DontDestroyOnLoad(this);
		config = new ConfigNode();
		PreBuiltCraftPositions = new DictionaryValueList<string, List<PreBuiltCraftPosition>>();
		if (!File.Exists(KSPUtil.ApplicationRootPath + "GameData/Squad/Contracts/Contracts.cfg"))
		{
			CreateDefaultConfig();
		}
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("Contracts");
		int num = configNodes.Length;
		while (num-- > 0)
		{
			configNodes[num].CopyTo(config);
		}
		LoadConfig();
		LoadDefinitions();
		GameEvents.OnTechnologyResearched.Add(TechResearched);
	}

	public void OnDestroy()
	{
		GameEvents.OnTechnologyResearched.Remove(TechResearched);
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void TechResearched(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> result)
	{
		loadedContractCraftDefs = false;
		LoadContractCraftDefs();
	}

	public static void LoadContractCraftDefs()
	{
		if (!loadedContractCraftDefs)
		{
			ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("CONTRACTCRAFT");
			ConfigNode node = new ConfigNode();
			int num = configNodes.Length;
			while (num-- > 0)
			{
				configNodes[num].CopyTo(node);
			}
			PreBuiltCraftPositions = GetPreBuiltCraftPositions(node);
			PreBuiltCraftDefs = GetPreBuiltCraft("ContractDefs", node);
			loadedContractCraftDefs = true;
		}
	}

	public static void LoadConstructionPartsList()
	{
		if (loadedConstructionPartsList)
		{
			return;
		}
		List<ConstructionPart> list = new List<ConstructionPart>();
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("CONTRACT_CONSTRUCTION_PARTS");
		ConfigNode configNode = new ConfigNode();
		int num = configNodes.Length;
		while (num-- > 0)
		{
			configNodes[num].CopyTo(configNode);
		}
		ConfigNode[] nodes = configNode.GetNodes("CONSTRUCTION_PART");
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = "";
			nodes[i].TryGetValue("name", ref value);
			string[] values = nodes[i].GetValues("contract");
			if (string.IsNullOrEmpty(value) || values.Length == 0)
			{
				continue;
			}
			value = value.Replace('_', '.');
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(value);
			bool flag = false;
			if (partInfoByName != null && partInfoByName.partPrefab != null)
			{
				ModuleCargoPart moduleCargoPart = partInfoByName.partPrefab.FindModuleImplementing<ModuleCargoPart>();
				if (moduleCargoPart != null && moduleCargoPart.packedVolume >= 0f)
				{
					if (!list.ContainsPart(value))
					{
						list.Add(new ConstructionPart(value, partInfoByName, values.ToList()));
					}
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogWarningFormat("[ContractDefs]: Ignored invalid construction contract part: {0},", value);
			}
		}
		constructionParts = new ConstructionParts(list);
		loadedConstructionPartsList = true;
	}

	public static PreBuiltCraft GetPreBuiltCraft(string className, ConfigNode node)
	{
		ConfigNode[] nodes = node.GetNodes("PREBUILTCRAFT");
		List<PreBuiltCraftDefinition> list = new List<PreBuiltCraftDefinition>();
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = "";
			bool value2 = false;
			if (!nodes[i].TryGetValue("url", ref value))
			{
				continue;
			}
			value = KSPUtil.ApplicationRootPath + value;
			nodes[i].TryGetValue("brokenParts", ref value2);
			PreBuiltCraftDefinition preBuiltCraftDefinition = SystemUtilities.ValidateLoadCraftNode(value, className, value2, bypassTechCheck: true);
			if (preBuiltCraftDefinition != null)
			{
				nodes[i].TryGetValue("AllowGround", ref preBuiltCraftDefinition.allowGround);
				nodes[i].TryGetValue("AllowOrbit", ref preBuiltCraftDefinition.allowOrbit);
				nodes[i].TryGetValue("AllowWater", ref preBuiltCraftDefinition.allowWater);
				nodes[i].TryGetValue("UsePreBuiltPos", ref preBuiltCraftDefinition.usePreBuiltPositions);
				string[] values = nodes[i].GetValues("ContractType");
				if (values.Length != 0)
				{
					preBuiltCraftDefinition.contractTypes = values.ToList();
					list.Add(preBuiltCraftDefinition);
				}
			}
		}
		return new PreBuiltCraft(list);
	}

	public static DictionaryValueList<string, List<PreBuiltCraftPosition>> GetPreBuiltCraftPositions(ConfigNode node)
	{
		DictionaryValueList<string, List<PreBuiltCraftPosition>> dictionaryValueList = new DictionaryValueList<string, List<PreBuiltCraftPosition>>();
		ConfigNode[] nodes = node.GetNodes("PREBUILTPOSITIONS");
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = "";
			if (nodes[i].TryGetValue("Body", ref value) && FlightGlobals.GetBodyByName(value) != null)
			{
				PreBuiltCraftPosition preBuiltCraftPosition = new PreBuiltCraftPosition();
				nodes[i].TryGetValue("CenterLatitude", ref preBuiltCraftPosition.CenterLatitude);
				nodes[i].TryGetValue("CenterLongitude", ref preBuiltCraftPosition.CenterLongitude);
				nodes[i].TryGetValue("SearchRadius", ref preBuiltCraftPosition.SearchRadius);
				if (!dictionaryValueList.ContainsKey(value))
				{
					dictionaryValueList.Add(value, new List<PreBuiltCraftPosition>());
				}
				List<PreBuiltCraftPosition> val = null;
				if (dictionaryValueList.TryGetValue(value, out val))
				{
					val.Add(preBuiltCraftPosition);
				}
			}
		}
		return dictionaryValueList;
	}

	public static void CreateDefaultConfig()
	{
		config = new ConfigNode();
		ConfigNode configNode = config.AddNode(new ConfigNode("Contracts", "Below you will find many career mode options, and most are commented"));
		ConfigNode configNode2 = configNode.AddNode(new ConfigNode("ARM", "Asteroid Recovery Contracts"));
		ConfigNode configNode3 = configNode2.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode4 = configNode2.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode5 = configNode2.AddNode(new ConfigNode("Science"));
		ConfigNode configNode6 = configNode2.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode7 = configNode.AddNode(new ConfigNode("Base", "Outpost Construction Contracts"));
		ConfigNode configNode8 = configNode7.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode9 = configNode7.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode10 = configNode7.AddNode(new ConfigNode("Science"));
		ConfigNode configNode11 = configNode7.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode12 = configNode.AddNode(new ConfigNode("Flag", "Flag Plant Contracts"));
		ConfigNode configNode13 = configNode12.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode14 = configNode12.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode15 = configNode12.AddNode(new ConfigNode("Science"));
		ConfigNode configNode16 = configNode12.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode17 = configNode.AddNode(new ConfigNode("Grand", "Grand Tour Contracts"));
		ConfigNode configNode18 = configNode17.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode19 = configNode17.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode20 = configNode17.AddNode(new ConfigNode("Science"));
		ConfigNode configNode21 = configNode17.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode22 = configNode.AddNode(new ConfigNode("ISRU", "In Situ Resource Extraction Contracts"));
		ConfigNode configNode23 = configNode22.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode24 = configNode22.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode25 = configNode22.AddNode(new ConfigNode("Science"));
		ConfigNode configNode26 = configNode22.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode27 = configNode.AddNode(new ConfigNode("Progression", "World First Contracts and Milestones"));
		ConfigNode configNode28 = configNode27.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode29 = configNode27.AddNode(new ConfigNode("Science"));
		ConfigNode configNode30 = configNode27.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode31 = configNode.AddNode(new ConfigNode("Recovery", "Rescue and Recovery Contracts"));
		ConfigNode configNode32 = configNode31.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode33 = configNode31.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode34 = configNode31.AddNode(new ConfigNode("Science"));
		ConfigNode configNode35 = configNode31.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode36 = configNode.AddNode(new ConfigNode("Satellite", "Satellite Deployment Contracts"));
		ConfigNode configNode37 = configNode36.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode38 = configNode36.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode39 = configNode36.AddNode(new ConfigNode("Science"));
		ConfigNode configNode40 = configNode36.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode41 = configNode.AddNode(new ConfigNode("Science", "Science Contracts"));
		ConfigNode configNode42 = configNode41.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode43 = configNode41.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode44 = configNode41.AddNode(new ConfigNode("Science"));
		ConfigNode configNode45 = configNode41.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode46 = configNode.AddNode(new ConfigNode("Sentinel", "Sentinel Contracts"));
		ConfigNode configNode47 = configNode41.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode48 = configNode41.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode49 = configNode41.AddNode(new ConfigNode("Science"));
		ConfigNode configNode50 = configNode41.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode51 = configNode41.AddNode(new ConfigNode("Trivial", "Easy Contracts"));
		ConfigNode configNode52 = configNode41.AddNode(new ConfigNode("Significant", "Medium Contracts"));
		ConfigNode configNode53 = configNode41.AddNode(new ConfigNode("Exceptional", "Difficult Contracts"));
		ConfigNode configNode54 = configNode.AddNode(new ConfigNode("Station", "Station Construction Contracts"));
		ConfigNode configNode55 = configNode54.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode56 = configNode54.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode57 = configNode54.AddNode(new ConfigNode("Science"));
		ConfigNode configNode58 = configNode54.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode59 = configNode.AddNode(new ConfigNode("Survey", "Survey Contracts"));
		ConfigNode configNode60 = configNode59.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode61 = configNode59.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode62 = configNode59.AddNode(new ConfigNode("Science"));
		ConfigNode configNode63 = configNode59.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode64 = configNode.AddNode(new ConfigNode("Test", "Part Test Contracts"));
		ConfigNode configNode65 = configNode64.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode66 = configNode64.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode67 = configNode64.AddNode(new ConfigNode("Science"));
		ConfigNode configNode68 = configNode64.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode69 = configNode.AddNode(new ConfigNode("Tour", "Tourism Contracts"));
		ConfigNode configNode70 = configNode69.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode71 = configNode69.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode72 = configNode69.AddNode(new ConfigNode("Science"));
		ConfigNode configNode73 = configNode69.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode74 = configNode.AddNode(new ConfigNode("CometSample", "Comet Sample Contracts"));
		ConfigNode configNode75 = configNode74.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode76 = configNode74.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode77 = configNode74.AddNode(new ConfigNode("Science"));
		ConfigNode configNode78 = configNode74.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode79 = configNode74.AddNode(new ConfigNode("Trivial", "Easy Contracts"));
		ConfigNode configNode80 = configNode74.AddNode(new ConfigNode("Significant", "Medium Contracts"));
		ConfigNode configNode81 = configNode74.AddNode(new ConfigNode("Exceptional", "Difficult Contracts"));
		ConfigNode configNode82 = configNode.AddNode(new ConfigNode("RoverConstruction", "Rover Construction Contracts"));
		ConfigNode configNode83 = configNode82.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode84 = configNode82.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode85 = configNode82.AddNode(new ConfigNode("Science"));
		ConfigNode configNode86 = configNode82.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode87 = configNode82.AddNode(new ConfigNode("Trivial", "Easy Contracts"));
		ConfigNode configNode88 = configNode82.AddNode(new ConfigNode("Significant", "Medium Contracts"));
		ConfigNode configNode89 = configNode82.AddNode(new ConfigNode("Exceptional", "Difficult Contracts"));
		ConfigNode configNode90 = configNode.AddNode(new ConfigNode("VesselRepair", "Vessel Repair Contracts"));
		ConfigNode configNode91 = configNode90.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode92 = configNode90.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode93 = configNode90.AddNode(new ConfigNode("Science"));
		ConfigNode configNode94 = configNode90.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode95 = configNode90.AddNode(new ConfigNode("Trivial", "Easy Contracts"));
		ConfigNode configNode96 = configNode90.AddNode(new ConfigNode("Significant", "Medium Contracts"));
		ConfigNode configNode97 = configNode90.AddNode(new ConfigNode("Exceptional", "Difficult Contracts"));
		ConfigNode configNode98 = configNode.AddNode(new ConfigNode("OrbitalConstruction", "Orbital Construction Contracts"));
		ConfigNode configNode99 = configNode98.AddNode(new ConfigNode("Expiration"));
		ConfigNode configNode100 = configNode98.AddNode(new ConfigNode("Funds"));
		ConfigNode configNode101 = configNode98.AddNode(new ConfigNode("Science"));
		ConfigNode configNode102 = configNode98.AddNode(new ConfigNode("Reputation"));
		ConfigNode configNode103 = configNode98.AddNode(new ConfigNode("Trivial", "Easy Contracts"));
		ConfigNode configNode104 = configNode98.AddNode(new ConfigNode("Significant", "Medium Contracts"));
		ConfigNode configNode105 = configNode98.AddNode(new ConfigNode("Exceptional", "Difficult Contracts"));
		configNode.AddValue("WeightDefault", WeightDefault, "The default weight new contract types start with");
		configNode.AddValue("WeightMinimum", WeightMinimum, "The minimum weight that contracts can have from declining them");
		configNode.AddValue("WeightMaximum", WeightMaximum, "The maximum weight that contracts can have from accepting them");
		configNode.AddValue("WeightAcceptDelta", WeightAcceptDelta, "How much accepting contracts affects their weight");
		configNode.AddValue("WeightDeclineDelta", WeightDeclineDelta, "How much declining contracts affects their weight");
		configNode.AddValue("WeightWithdrawReadDelta", WeightWithdrawReadDelta, "How much an expired contract that was read affects their weight");
		configNode.AddValue("WeightWithdrawSeenDelta", WeightWithdrawSeenDelta, "How much an expired contract that was seen (showed up in mission control) affects their weight");
		configNode.AddValue("DisplayOfferedOrbits", DisplayOfferedOrbits, "Display inactive contract orbits in the Tracking Station");
		configNode.AddValue("DisplayOfferedWaypoints", DisplayOfferedWaypoints, "Display inactive contract waypoints in the Tracking Station");
		configNode.AddValue("SurveyNavigationGhosting", SurveyNavigationGhosting, "Survey navigation icons go transparent behind the navball");
		configNode.AddValue("AverageAvailableContracts", AverageAvailableContracts, "The average number of contracts in Mission Control");
		configNode.AddValue("FacilityProgressionFactor", FacilityProgressionFactor, "How much facility upgrades affect contract progression");
		configNode.AddValue("SolarOrbitHeatTolerance", SolarOrbitHeatTolerance, "The temperature to avoid when placing orbits near the Sun");
		configNode.AddValue("SunStationaryName", SunStationaryName, "What contracts call heliostationary orbits of the Sun");
		configNode.AddValue("HomeStationaryName", HomeStationaryName, "What contracts call geostationary orbits of the home planet");
		configNode.AddValue("OtherStationaryName", OtherStationaryName, "What contracts call stationary orbits of any other planet");
		configNode.AddValue("SunSynchronousName", SunSynchronousName, "What contracts call heliosynchronous orbits of the Sun");
		configNode.AddValue("HomeSynchronousName", HomeSynchronousName, "What contracts call geosynchronous orbits of the home planet");
		configNode.AddValue("OtherSynchronousName", OtherSynchronousName, "What contracts call synchronous orbits of any other planet");
		configNode.AddValue("MolniyaName", MolniyaName, "What contracts call Molniya orbits");
		configNode2.AddValue("MaximumExistent", GClass6.MaximumExistent, "The maximum amount of active or inactive ARM contracts");
		configNode2.AddValue("SignificantSolarEjectionChance", GClass6.SignificantSolarEjectionChance, "The percent chance for a solar ejection objective at medium difficulty");
		configNode2.AddValue("ExceptionalSolarEjectionChance", GClass6.ExceptionalSolarEjectionChance, "The percent chance for a solar ejection objective at hard difficulty");
		configNode2.AddValue("HomeLandingChance", GClass6.HomeLandingChance, "The percent chance for a landing request on the home planet");
		configNode2.AddValue("AllowSolarEjections", GClass6.AllowSolarEjections, "Whether to allow solar ejections at all");
		configNode2.AddValue("AllowHomeLandings", GClass6.AllowHomeLandings, "Whether to allow landing requests on the home planet at all");
		configNode3.AddValue("MinimumExpireDays", GClass6.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode3.AddValue("MaximumExpireDays", GClass6.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode3.AddValue("DeadlineDays", GClass6.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode4.AddValue("BaseAdvance", GClass6.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode4.AddValue("BaseReward", GClass6.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode4.AddValue("BaseFailure", GClass6.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode4.AddValue("SolarEjectionMultiplier", GClass6.Funds.SolarEjectionMultiplier, "How much fund rewards are multiplied by on a solar ejection request");
		configNode5.AddValue("BaseReward", GClass6.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode5.AddValue("SolarEjectionMultiplier", GClass6.Science.SolarEjectionMultiplier, "How much science rewards are multiplied by on a solar ejection request");
		configNode6.AddValue("BaseReward", GClass6.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode6.AddValue("BaseFailure", GClass6.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode6.AddValue("SolarEjectionMultiplier", GClass6.Reputation.SolarEjectionMultiplier, "How much reputation rewards are multiplied by on a solar ejection request");
		configNode7.AddValue("MaximumExistent", Base.MaximumExistent, "The maximum amount of active or inactive outpost contracts");
		configNode7.AddValue("ContextualChance", Base.ContextualChance, "The maximum chance for an outpost expansion request");
		configNode7.AddValue("ContextualAssets", Base.ContextualAssets, "The amount of outposts required on the planet to reach that chance");
		configNode7.AddValue("TrivialMobileChance", Base.TrivialMobileChance, "The percent chance to receive a mobile outpost request on easy contracts");
		configNode7.AddValue("SignificantMobileChance", Base.SignificantMobileChance, "The percent chance to receive a mobile outpost request on medium contracts");
		configNode7.AddValue("ExceptionalMobileChance", Base.ExceptionalMobileChance, "The percent chance to receive a mobile outpost request on hard contracts");
		configNode7.AddValue("AllowMobile", Base.AllowMobile, "Whether to allow mobile outpost requests at all");
		configNode8.AddValue("MinimumExpireDays", Base.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode8.AddValue("MaximumExpireDays", Base.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode8.AddValue("DeadlineDays", Base.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode9.AddValue("BaseAdvance", Base.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode9.AddValue("BaseReward", Base.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode9.AddValue("BaseFailure", Base.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode9.AddValue("MobileMultiplier", Base.Funds.MobileMultiplier, "How much fund rewards are multiplied by on a mobile outpost request");
		configNode10.AddValue("BaseReward", Base.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode10.AddValue("MobileMultiplier", Base.Science.MobileMultiplier, "How much science rewards are multiplied by on a mobile outpost request");
		configNode11.AddValue("BaseReward", Base.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode11.AddValue("BaseFailure", Base.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode11.AddValue("MobileMultiplier", Base.Reputation.MobileMultiplier, "How much reputation rewards are multiplied by on a mobile outpost request");
		configNode12.AddValue("MaximumExistent", Flag.MaximumExistent, "The maximum amount of active or inactive flag plant contracts");
		configNode13.AddValue("MinimumExpireDays", Flag.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode13.AddValue("MaximumExpireDays", Flag.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode13.AddValue("DeadlineDays", Flag.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode14.AddValue("BaseAdvance", Flag.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode14.AddValue("BaseReward", Flag.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode14.AddValue("BaseFailure", Flag.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode15.AddValue("BaseReward", Flag.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode16.AddValue("BaseReward", Flag.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode16.AddValue("BaseFailure", Flag.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode17.AddValue("Rarity", Grand.Rarity, "The chance for a Grand Tour to appear in relation to other contracts");
		configNode18.AddValue("MinimumExpireDays", Grand.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode18.AddValue("MaximumExpireDays", Grand.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode18.AddValue("DeadlineDays", Grand.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode19.AddValue("BaseAdvance", Grand.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode19.AddValue("BaseReward", Grand.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode19.AddValue("BaseFailure", Grand.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode20.AddValue("BaseReward", Grand.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode21.AddValue("BaseReward", Grand.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode21.AddValue("BaseFailure", Grand.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode22.AddValue("MaximumExistent", ISRU.MaximumExistent, "The maximum amount of active or inactive ISRU contracts");
		configNode23.AddValue("MinimumExpireDays", ISRU.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode23.AddValue("MaximumExpireDays", ISRU.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode23.AddValue("DeadlineDays", ISRU.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode24.AddValue("BaseAdvance", ISRU.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode24.AddValue("BaseReward", ISRU.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode24.AddValue("BaseFailure", ISRU.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode25.AddValue("BaseReward", ISRU.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode26.AddValue("BaseReward", ISRU.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode26.AddValue("BaseFailure", ISRU.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode27.AddValue("DisableTutorialContracts", Progression.DisableTutorialContracts, "Whether World First ignores first launch, reach space, and orbit home");
		configNode27.AddValue("DisableProgressionContracts", Progression.DisableProgressionContracts, "Whether World First ignores the line of contracts after the tutorial");
		configNode27.AddValue("MaxDepthRecord", Progression.MaxDepthRecord, "The maximum depth to reach in the depth record track");
		configNode27.AddValue("MaxDistanceRecord", Progression.MaxDistanceRecord, "The maximum distance to reach in the distance record track");
		configNode27.AddValue("MaxSpeedRecord", Progression.MaxSpeedRecord, "The maximum speed to reach in the speed record track");
		configNode27.AddValue("OutlierMilestoneMultiplier", Progression.OutlierMilestoneMultiplier, "What to multiply rewards by on hard requests like returning from atmospheric landings");
		configNode27.AddValue("PassiveBaseRatio", Progression.PassiveBaseRatio, "What base portion of the total World First contract reward is granted passively");
		configNode27.AddValue("PassiveBodyRatio", Progression.PassiveBodyRatio, "How much the body multiplier affects passive progression");
		configNode27.AddValue("RecordSplit", Progression.RecordSplit, "How many intervals are in the speed, altitude, and distance record tracks");
		configNode28.AddValue("BaseReward", Progression.Funds.BaseReward, "The base funds reward of a progress node including passive and contract income");
		configNode29.AddValue("BaseReward", Progression.Science.BaseReward, "The base science reward of a progress node including passive and contract income");
		configNode30.AddValue("BaseReward", Progression.Reputation.BaseReward, "The base reputation reward of a progress node including passive and contract income");
		configNode31.AddValue("MaximumAvailable", Recovery.MaximumAvailable, "The maximum amount of inactive recovery contracts in Mission Control");
		configNode31.AddValue("MaximumActive", Recovery.MaximumActive, "The maximum amount of active recovery contracts that have been accepted");
		configNode31.AddValue("AllowKerbalRescue", Recovery.AllowKerbalRescue, "Whether to allow kerbal only rescues at all");
		configNode31.AddValue("AllowPartRecovery", Recovery.AllowPartRecovery, "Whether to allow part only recoveries at all");
		configNode31.AddValue("AllowCompoundRecovery", Recovery.AllowCompoundRecovery, "Whether to allow compound recoveries of kerbals and parts at all");
		configNode31.AddValue("AllowLandedVacuum", Recovery.AllowLandedVacuum, "Whether to allow landed recoveries on vacuum planets at all");
		configNode31.AddValue("AllowLandedAtmosphere", Recovery.AllowLandedAtmosphere, "Whether to allow landed recoveries on atmospheric planets at all");
		configNode31.AddValue("HighOrbitDifficulty", Recovery.HighOrbitDifficulty, "How eccentric and inclined recoveries in high orbit can get");
		configNode32.AddValue("MinimumExpireDays", Recovery.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode32.AddValue("MaximumExpireDays", Recovery.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode32.AddValue("DeadlineDays", Recovery.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode33.AddValue("BaseAdvance", Recovery.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode33.AddValue("BaseReward", Recovery.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode33.AddValue("BaseFailure", Recovery.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode34.AddValue("BaseReward", Recovery.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode35.AddValue("BaseReward", Recovery.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode35.AddValue("BaseFailure", Recovery.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode36.AddValue("MaximumAvailable", Satellite.MaximumAvailable, "The maximum amount of inactive satellite contracts in Mission Control");
		configNode36.AddValue("MaximumActive", Satellite.MaximumActive, "The maximum amount of active satellite contracts that have been accepted");
		configNode36.AddValue("AnimationDuration", Satellite.AnimationDuration, "How long it takes the orbit render animation to loop");
		configNode36.AddValue("ContextualChance", Satellite.ContextualChance, "The maximum chance for a satellite adjustment or network request");
		configNode36.AddValue("ContextualAssets", Satellite.ContextualAssets, "The amount of satellites required on non-home planets to reach that chance");
		configNode36.AddValue("ContextualHomeAssets", Satellite.ContextualHomeAssets, "The amount of satellites required on the home planet to reach that chance");
		configNode36.AddValue("NetworkMinimum", Satellite.NetworkMinimum, "The minimum amount of satellites that are considered a network");
		configNode36.AddValue("NetworkMaximum", Satellite.NetworkMaximum, "The maximum amount of satellites that are considered a network");
		configNode36.AddValue("MinimumDeviationWindow", Satellite.MinimumDeviationWindow, "The lowest that an altitude deviation can get at the periapsis");
		configNode36.AddValue("TrivialDeviation", Satellite.TrivialDeviation, "The percent difference allowed for altitude and relevant angles to trigger easy contracts");
		configNode36.AddValue("SignificantDeviation", Satellite.SignificantDeviation, "The percent difference allowed for altitude and relevant angles to trigger medium contracts");
		configNode36.AddValue("ExceptionalDeviation", Satellite.ExceptionalDeviation, "The percent difference allowed for altitude and relevant angles to trigger hard contracts");
		configNode36.AddValue("TrivialAltitudeDifficulty", Satellite.TrivialAltitudeDifficulty, "A ratio representing how eccentric orbits can get on easy contracts");
		configNode36.AddValue("TrivialInclinationDifficulty", Satellite.TrivialInclinationDifficulty, "A ratio representing how inclined orbits can get on easy contracts");
		configNode36.AddValue("SignificantAltitudeDifficulty", Satellite.SignificantAltitudeDifficulty, "A ratio representing how eccentric orbits can get on medium contracts");
		configNode36.AddValue("SignificantInclinationDifficulty", Satellite.SignificantInclinationDifficulty, "A ratio representing how inclined orbits can get on medium contracts");
		configNode36.AddValue("ExceptionalAltitudeDifficulty", Satellite.ExceptionalAltitudeDifficulty, "A ratio representing how eccentric orbits can get on hard contracts");
		configNode36.AddValue("ExceptionalInclinationDifficulty", Satellite.ExceptionalInclinationDifficulty, "A ratio representing how inclined orbits can get on hard contracts");
		configNode36.AddValue("PreferHome", Satellite.PreferHome, "Whether to utilize home override chances to prefer home satellites");
		configNode36.AddValue("AllowSolar", Satellite.AllowSolar, "Whether to allow satellite requests for solar orbits at all");
		configNode36.AddValue("AllowEquatorial", Satellite.AllowEquatorial, "Whether to allow equatorial orbits at all");
		configNode36.AddValue("AllowPolar", Satellite.AllowPolar, "Whether to allow polar orbits at all");
		configNode36.AddValue("AllowSynchronous", Satellite.AllowSynchronous, "Whether to allow synchronous orbits at all");
		configNode36.AddValue("AllowStationary", Satellite.AllowStationary, "Whether to allow stationary orbits at all");
		configNode36.AddValue("AllowTundra", Satellite.AllowTundra, "Whether to allow tundra orbits at all");
		configNode36.AddValue("AllowKolniya", Satellite.AllowKolniya, "Whether to allow Molniya orbits at all");
		configNode37.AddValue("MinimumExpireDays", Satellite.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode37.AddValue("MaximumExpireDays", Satellite.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode37.AddValue("DeadlineDays", Satellite.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode38.AddValue("BaseAdvance", Satellite.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode38.AddValue("BaseReward", Satellite.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode38.AddValue("BaseFailure", Satellite.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode38.AddValue("HomeMultiplier", Satellite.Funds.HomeMultiplier, "How much funds rewards are multiplied by on satellite requests at home");
		configNode38.AddValue("PolarMultiplier", Satellite.Funds.PolarMultiplier, "How much funds rewards are multiplied by on polar satellite requests");
		configNode38.AddValue("SynchronousMultiplier", Satellite.Funds.SynchronousMultiplier, "How much funds rewards are multiplied by on synchronous satellite requests");
		configNode38.AddValue("StationaryMultiplier", Satellite.Funds.StationaryMultiplier, "How much funds rewards are multiplied by on stationary satellite requests");
		configNode38.AddValue("TundraMultiplier", Satellite.Funds.TundraMultiplier, "How much funds rewards are multiplied by on tundra satellite requests");
		configNode38.AddValue("KolniyaMultiplier", Satellite.Funds.KolniyaMultiplier, "How much funds rewards are multiplied by on Molniya satellite requests");
		configNode39.AddValue("BaseReward", Satellite.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode39.AddValue("HomeMultiplier", Satellite.Science.HomeMultiplier, "How much science rewards are multiplied by on satellite requests at home");
		configNode39.AddValue("PolarMultiplier", Satellite.Science.PolarMultiplier, "How much science rewards are multiplied by on polar satellite requests");
		configNode39.AddValue("SynchronousMultiplier", Satellite.Science.SynchronousMultiplier, "How much science rewards are multiplied by on synchronous satellite requests");
		configNode39.AddValue("StationaryMultiplier", Satellite.Science.StationaryMultiplier, "How much science rewards are multiplied by on stationary satellite requests");
		configNode39.AddValue("TundraMultiplier", Satellite.Science.TundraMultiplier, "How much science rewards are multiplied by on tundra satellite requests");
		configNode39.AddValue("KolniyaMultiplier", Satellite.Science.KolniyaMultiplier, "How much science rewards are multiplied by on Molniya satellite requests");
		configNode40.AddValue("BaseReward", Satellite.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode40.AddValue("BaseFailure", Satellite.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode40.AddValue("HomeMultiplier", Satellite.Reputation.HomeMultiplier, "How much reputation rewards are multiplied by on satellite requests at home");
		configNode40.AddValue("PolarMultiplier", Satellite.Reputation.PolarMultiplier, "How much reputation rewards are multiplied by on polar satellite requests");
		configNode40.AddValue("SynchronousMultiplier", Satellite.Reputation.SynchronousMultiplier, "How much reputation rewards are multiplied by on synchronous satellite requests");
		configNode40.AddValue("StationaryMultiplier", Satellite.Reputation.StationaryMultiplier, "How much reputation rewards are multiplied by on stationary satellite requests");
		configNode40.AddValue("TundraMultiplier", Satellite.Reputation.TundraMultiplier, "How much reputation rewards are multiplied by on tundra satellite requests");
		configNode40.AddValue("KolniyaMultiplier", Satellite.Reputation.KolniyaMultiplier, "How much reputation rewards are multiplied by on Molniya satellite requests");
		configNode41.AddValue("MaximumExistent", Research.MaximumExistent, "The maximum amount of active or inactive science contracts");
		configNode42.AddValue("MinimumExpireDays", Research.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode42.AddValue("MaximumExpireDays", Research.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode42.AddValue("DeadlineDays", Research.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode43.AddValue("BaseAdvance", Research.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode43.AddValue("BaseReward", Research.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode43.AddValue("BaseFailure", Research.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode44.AddValue("BaseReward", Research.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode45.AddValue("BaseReward", Research.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode45.AddValue("BaseFailure", Research.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode46.AddValue("MaximumActive", Sentinel.MaximumActive, "The maximum amount of active sentinel contracts");
		configNode46.AddValue("MaximumAvailable", Sentinel.MaximumAvailable, "The maximum amount of available sentinel contracts");
		configNode46.AddValue("ScanTypeBaseMultiplier", Sentinel.ScanTypeBaseMultiplier, "Multiplier applied to basic types of scan contract (no specific asteroid class, inclination or eccentricity)");
		configNode46.AddValue("ScanTypeClassMultiplier", Sentinel.ScanTypeClassMultiplier, "Multiplier applied to scan contract requiring specific class of asteroid.");
		configNode46.AddValue("ScanTypeInclinationMultiplier", Sentinel.ScanTypeInclinationMultiplier, "Multiplier applied to scan contract requiring inclination.");
		configNode46.AddValue("ScanTypeEccentricityMultiplier", Sentinel.ScanTypeEccentricityMultiplier, "Multiplier applied to scan contract requiring eccentricity.");
		configNode46.AddValue("TrivialDeviation", Sentinel.TrivialDeviation, "The percent difference allowed for altitude and relevant angles to trigger easy contracts");
		configNode46.AddValue("SignificantDeviation", Sentinel.SignificantDeviation, "The percent difference allowed for altitude and relevant angles to trigger medium contracts");
		configNode46.AddValue("ExceptionalDeviation", Sentinel.ExceptionalDeviation, "The percent difference allowed for altitude and relevant angles to trigger hard contracts");
		configNode47.AddValue("MinimumExpireDays", Sentinel.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode47.AddValue("MaximumExpireDays", Sentinel.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode47.AddValue("DeadlineDays", Sentinel.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode48.AddValue("BaseAdvance", Sentinel.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode48.AddValue("BaseReward", Sentinel.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode48.AddValue("BaseFailure", Sentinel.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode49.AddValue("BaseReward", Sentinel.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode50.AddValue("BaseReward", Sentinel.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode50.AddValue("BaseFailure", Sentinel.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode51.AddValue("FundsMultiplier", Sentinel.Trivial.FundsMultiplier, "The multiplier this request places on funds in Trivial contracts");
		configNode51.AddValue("ScienceMultiplier", Sentinel.Trivial.ScienceMultiplier, "The multiplier this request places on science in Trivial contracts");
		configNode51.AddValue("ReputationMultiplier", Sentinel.Trivial.ReputationMultiplier, "The multiplier this request places on reputation in Trivial contracts");
		configNode51.AddValue("MinAsteroids", Sentinel.Trivial.MinAsteroids, "The minimum number of asteroids to scan");
		configNode51.AddValue("MaxAsteroids", Sentinel.Trivial.MaxAsteroids, "The maximum number of asteroids to scan");
		configNode52.AddValue("FundsMultiplier", Sentinel.Significant.FundsMultiplier, "The multiplier this request places on funds in Significant contracts");
		configNode52.AddValue("ScienceMultiplier", Sentinel.Significant.ScienceMultiplier, "The multiplier this request places on science in Significant contracts");
		configNode52.AddValue("ReputationMultiplier", Sentinel.Significant.ReputationMultiplier, "The multiplier this request places on reputation in Significant contracts");
		configNode52.AddValue("MinAsteroids", Sentinel.Significant.MinAsteroids, "The minimum number of asteroids to scan");
		configNode52.AddValue("MaxAsteroids", Sentinel.Significant.MaxAsteroids, "The maximum number of asteroids to scan");
		configNode53.AddValue("FundsMultiplier", Sentinel.Exceptional.FundsMultiplier, "The multiplier this request places on funds in Exceptional contracts");
		configNode53.AddValue("ScienceMultiplier", Sentinel.Exceptional.ScienceMultiplier, "The multiplier this request places on science in Exceptional contracts");
		configNode53.AddValue("ReputationMultiplier", Sentinel.Exceptional.ReputationMultiplier, "The multiplier this request places on reputation in Exceptional contracts");
		configNode53.AddValue("MinAsteroids", Sentinel.Exceptional.MinAsteroids, "The minimum number of asteroids to scan");
		configNode53.AddValue("MaxAsteroids", Sentinel.Exceptional.MaxAsteroids, "The maximum number of asteroids to scan");
		configNode54.AddValue("MaximumExistent", Station.MaximumExistent, "The maximum amount of active or inactive station contracts");
		configNode54.AddValue("ContextualChance", Station.ContextualChance, "The maximum chance for a station expansion request");
		configNode54.AddValue("ContextualAssets", Station.ContextualAssets, "The amount of stations required around the planet to reach that chance");
		configNode54.AddValue("TrivialAsteroidChance", Station.TrivialAsteroidChance, "The percent chance to receive an asteroid embedded station request on easy contracts");
		configNode54.AddValue("SignificantAsteroidChance", Station.SignificantAsteroidChance, "The percent chance to receive an asteroid embedded station request on medium contracts");
		configNode54.AddValue("ExceptionalAsteroidChance", Station.ExceptionalAsteroidChance, "The percent chance to receive an asteroid embedded station request on hard contracts");
		configNode54.AddValue("AllowAsteroid", Station.AllowAsteroid, "Whether to allow asteroid embedded station requests at all");
		configNode54.AddValue("AllowSolar", Station.AllowSolar, "Whether to allow station requests around the sun at all");
		configNode55.AddValue("MinimumExpireDays", Station.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode55.AddValue("MaximumExpireDays", Station.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode55.AddValue("DeadlineDays", Station.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode56.AddValue("BaseAdvance", Station.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode56.AddValue("BaseReward", Station.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode56.AddValue("BaseFailure", Station.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode56.AddValue("AsteroidMultiplier", Station.Funds.AsteroidMultiplier, "How much funds rewards are multiplied by on an asteroid station request");
		configNode57.AddValue("BaseReward", Station.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode57.AddValue("AsteroidMultiplier", Station.Science.AsteroidMultiplier, "How much science rewards are multiplied by on an asteroid station request");
		configNode58.AddValue("BaseReward", Station.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode58.AddValue("BaseFailure", Station.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode58.AddValue("AsteroidMultiplier", Station.Reputation.AsteroidMultiplier, "How much reputation rewards are multiplied by on an asteroid station request");
		configNode59.AddValue("MaximumAvailable", Survey.MaximumAvailable, "The maximum amount of inactive survey contracts in Mission Control");
		configNode59.AddValue("MaximumActive", Survey.MaximumActive, "The maximum amount of active survey contracts that have been accepted");
		configNode59.AddValue("ContextualChance", Survey.ContextualChance, "The maximum chance for a contextual survey trail request");
		configNode59.AddValue("ContextualAssets", Survey.ContextualAssets, "The amount of ground assets required on the planet to reach that chance");
		configNode59.AddValue("TrivialWaypoints", Survey.TrivialWaypoints, "The base amount of waypoints per survey on easy contracts before planet modifiers");
		configNode59.AddValue("SignificantWaypoints", Survey.SignificantWaypoints, "The base amount of waypoints per survey on medium contracts before planet modifiers");
		configNode59.AddValue("ExceptionalWaypoints", Survey.ExceptionalWaypoints, "The base amount of waypoints per survey on hard contracts before planet modifiers");
		configNode59.AddValue("HomeNearbyProgressCap", Survey.HomeNearbyProgressCap, "The progress level at which home surveys stop appearing near the space center");
		configNode59.AddValue("TrivialRange", Survey.TrivialRange, "The range that waypoints can spawn from the center of the survey on easy surveys");
		configNode59.AddValue("SignificantRange", Survey.SignificantRange, "The range that waypoints can spawn from the center of the survey on medium surveys");
		configNode59.AddValue("ExceptionalRange", Survey.ExceptionalRange, "The range that waypoints can spawn from the center of the survey on hard surveys");
		configNode59.AddValue("MinimumTriggerRange", Survey.MinimumTriggerRange, "The minimum range to trigger waypoints on ground surveys before gravitational adjustments");
		configNode59.AddValue("MaximumTriggerRange", Survey.MaximumTriggerRange, "The minimum range to trigger waypoints on high altitude surveys");
		configNode59.AddValue("MinimumThreshold", Survey.MinimumThreshold, "The minimum threshold between low and high altitude on non-atmospheric planets");
		configNode59.AddValue("MaximumThreshold", Survey.MaximumThreshold, "The maximum threshold between low and high altitude on non-atmospheric planets");
		configNode59.AddValue("ThresholdDeviancy", Survey.ThresholdDeviancy, "How much to wobble this threshold randomly so that the briefings aren't all the same");
		configNode60.AddValue("MinimumExpireDays", Survey.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode60.AddValue("MaximumExpireDays", Survey.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode60.AddValue("DeadlineDays", Survey.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode61.AddValue("DefaultAdvance", Survey.Funds.DefaultAdvance, "The base funds advance for the contract before planet and difficulty modifiers");
		configNode61.AddValue("DefaultReward", Survey.Funds.DefaultReward, "The base funds completion reward for the contract before planet and difficulty modifiers");
		configNode61.AddValue("DefaultFailure", Survey.Funds.DefaultFailure, "The base funds failure penalty for the contract before planet and difficulty modifiers");
		configNode61.AddValue("WaypointDefaultReward", Survey.Funds.WaypointDefaultReward, "The base funds completion reward for each waypoint before planet and difficulty modifiers");
		configNode62.AddValue("DefaultReward", Survey.Science.DefaultReward, "The base science completion reward for the contract before difficulty modifiers");
		configNode62.AddValue("WaypointDefaultReward", Survey.Science.WaypointDefaultReward, "The base science completion reward for each waypoint before difficulty modifiers");
		configNode63.AddValue("DefaultReward", Survey.Reputation.DefaultReward, "The base reputation completion reward for the contract before difficulty modifiers");
		configNode63.AddValue("DefaultFailure", Survey.Reputation.DefaultFailure, "The base reputation failure penalty for the contract before difficulty modifiers");
		configNode63.AddValue("WaypointDefaultReward", Survey.Reputation.WaypointDefaultReward, "The base reputation completion reward for each waypoint before difficulty modifiers");
		configNode64.AddValue("MaximumExistent", Test.MaximumExistent, "The maximum amount of active or inactive part test contracts");
		configNode64.AddValue("SubjectsToRepeat", Test.SubjectsToRepeat, "How slowly to start repeating previously completed tests");
		configNode64.AddValue("AllowHauls", Test.AllowHauls, "Whether to allow payload haul variants at all");
		configNode64.AddValue("TrivialHaulChance", Test.TrivialHaulChance, "The percent chance to become a payload contract on easy contracts");
		configNode64.AddValue("SignificantHaulChance", Test.SignificantHaulChance, "The percent chance to become a payload contract on medium contracts");
		configNode64.AddValue("ExceptionalHaulChance", Test.ExceptionalHaulChance, "The percent chance to become a payload contract on hard contracts");
		configNode65.AddValue("MinimumExpireDays", Test.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode65.AddValue("MaximumExpireDays", Test.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode65.AddValue("DeadlineDays", Test.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode66.AddValue("BaseAdvance", Test.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode66.AddValue("BaseReward", Test.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode66.AddValue("BaseFailure", Test.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode67.AddValue("BaseReward", Test.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode68.AddValue("BaseReward", Test.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode68.AddValue("BaseFailure", Test.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode69.AddValue("MaximumAvailable", Tour.MaximumAvailable, "The maximum amount of inactive tourism contracts in Mission Control");
		configNode69.AddValue("MaximumActive", Tour.MaximumActive, "The maximum amount of active tourism contracts that have been accepted");
		configNode69.AddValue("FailOnInactive", Tour.FailOnInactive, "Whether to fail parameters for tourists rendered inactive (from gee forces)");
		configNode69.AddValue("GeeAdventureChance", Tour.GeeAdventureChance, "Chance of generating a high-gee adventure contract");
		configNode70.AddValue("MinimumExpireDays", Tour.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode70.AddValue("MaximumExpireDays", Tour.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode70.AddValue("DeadlineDays", Tour.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode71.AddValue("DefaultFare", Tour.Funds.DefaultFare, "The base funds completion reward for each itinerary destination before planet and difficulty modifiers");
		configNode72.AddValue("BaseReward", Tour.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode73.AddValue("BaseReward", Tour.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode73.AddValue("BaseFailure", Tour.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		AddSurveyDefinitions(configNode59);
		AddPartRequests(configNode);
		AddResourceRequests(configNode);
		AddCrewRequests(configNode);
		configNode74.AddValue("MaximumAvailable", CometSample.MaximumAvailable, "The maximum amount of inactive comet sample contracts in Mission Control");
		configNode74.AddValue("MaximumActive", CometSample.MaximumActive, "The maximum amount of active comet sample contracts that have been accepted");
		configNode75.AddValue("MinimumExpireDays", CometSample.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode75.AddValue("MaximumExpireDays", CometSample.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode75.AddValue("DeadlineDays", CometSample.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode76.AddValue("BaseAdvance", CometSample.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode76.AddValue("BaseReward", CometSample.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode76.AddValue("BaseFailure", CometSample.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode77.AddValue("BaseReward", CometSample.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode78.AddValue("BaseReward", CometSample.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode78.AddValue("BaseFailure", CometSample.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode79.AddValue("FundsMultiplier", CometSample.Trivial.FundsMultiplier, "The multiplier this request places on funds in Trivial contracts");
		configNode79.AddValue("ScienceMultiplier", CometSample.Trivial.ScienceMultiplier, "The multiplier this request places on science in Trivial contracts");
		configNode79.AddValue("ReputationMultiplier", CometSample.Trivial.ReputationMultiplier, "The multiplier this request places on reputation in Trivial contracts");
		configNode80.AddValue("FundsMultiplier", CometSample.Significant.FundsMultiplier, "The multiplier this request places on funds in Significant contracts");
		configNode80.AddValue("ScienceMultiplier", CometSample.Significant.ScienceMultiplier, "The multiplier this request places on science in Significant contracts");
		configNode80.AddValue("ReputationMultiplier", CometSample.Significant.ReputationMultiplier, "The multiplier this request places on reputation in Significant contracts");
		configNode81.AddValue("FundsMultiplier", CometSample.Exceptional.FundsMultiplier, "The multiplier this request places on funds in Exceptional contracts");
		configNode81.AddValue("ScienceMultiplier", CometSample.Exceptional.ScienceMultiplier, "The multiplier this request places on science in Exceptional contracts");
		configNode81.AddValue("ReputationMultiplier", CometSample.Exceptional.ReputationMultiplier, "The multiplier this request places on reputation in Exceptional contracts");
		configNode82.AddValue("MaximumAvailable", RoverConstruction.MaximumAvailable, "The maximum amount of Rover Construction contracts in Mission Control");
		configNode82.AddValue("MaximumActive", RoverConstruction.MaximumActive, "The maximum amount of active Rover Construction contracts that have been accepted");
		configNode82.AddValue("WayPointMinDistance", RoverConstruction.WayPointMinDistance, "The WayPoint minimum distance in meters between start and finish positions for the contract");
		configNode82.AddValue("WayPointMinDistance", RoverConstruction.WayPointMaxDistance, "The WayPoint maximum distance in meters between start and finish positions for the contract");
		configNode82.AddValue("WayPointTriggerRange", RoverConstruction.WayPointTriggerRange, "The WayPoint distance in meters the rover must be within to complete the contract");
		configNode82.AddValue("MinimumGeeASL", RoverConstruction.MinimumGeeASL, "The minimum GeeASL a celestial body must have to be selected for this contract");
		configNode82.AddValue("MaximumGeeASL", RoverConstruction.MaximumGeeASL, "The maximum GeeASL a celestial body must have to be selected for this contract");
		configNode82.AddValue("TechNodeRequired", RoverConstruction.TechNodeRequired, "The Tech tree node that must be unlocked to be offered this contract");
		configNode83.AddValue("MinimumExpireDays", RoverConstruction.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode83.AddValue("MaximumExpireDays", RoverConstruction.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode83.AddValue("DeadlineDays", RoverConstruction.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode84.AddValue("BaseAdvance", RoverConstruction.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode84.AddValue("BaseReward", RoverConstruction.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode84.AddValue("BaseFailure", RoverConstruction.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode85.AddValue("BaseReward", RoverConstruction.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode86.AddValue("BaseReward", RoverConstruction.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode86.AddValue("BaseFailure", RoverConstruction.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode87.AddValue("FundsMultiplier", RoverConstruction.Trivial.FundsMultiplier, "The multiplier this request places on funds in Trivial contracts");
		configNode87.AddValue("ScienceMultiplier", RoverConstruction.Trivial.ScienceMultiplier, "The multiplier this request places on science in Trivial contracts");
		configNode87.AddValue("ReputationMultiplier", RoverConstruction.Trivial.ReputationMultiplier, "The multiplier this request places on reputation in Trivial contracts");
		configNode87.AddValue("WayPointMultiplier", RoverConstruction.Trivial.WayPointMultiplier, "The multiplier this request places on the WayPoint distance from starting point in Trivial contracts");
		configNode88.AddValue("FundsMultiplier", RoverConstruction.Significant.FundsMultiplier, "The multiplier this request places on funds in Significant contracts");
		configNode88.AddValue("ScienceMultiplier", RoverConstruction.Significant.ScienceMultiplier, "The multiplier this request places on science in Significant contracts");
		configNode88.AddValue("ReputationMultiplier", RoverConstruction.Significant.ReputationMultiplier, "The multiplier this request places on reputation in Significant contracts");
		configNode88.AddValue("WayPointMultiplier", RoverConstruction.Significant.WayPointMultiplier, "The multiplier this request places on the WayPoint distance from starting point in Significant contracts");
		configNode89.AddValue("FundsMultiplier", RoverConstruction.Exceptional.FundsMultiplier, "The multiplier this request places on funds in Exceptional contracts");
		configNode89.AddValue("ScienceMultiplier", RoverConstruction.Exceptional.ScienceMultiplier, "The multiplier this request places on science in Exceptional contracts");
		configNode89.AddValue("ReputationMultiplier", RoverConstruction.Exceptional.ReputationMultiplier, "The multiplier this request places on reputation in Exceptional contracts");
		configNode89.AddValue("WayPointMultiplier", RoverConstruction.Exceptional.WayPointMultiplier, "The multiplier this request places on the WayPoint distance from starting point in Exceptional contracts");
		configNode90.AddValue("MaximumAvailable", VesselRepair.MaximumAvailable, "The maximum amount of inactive Vessel Repair contracts in Mission Control");
		configNode90.AddValue("MaximumActive", VesselRepair.MaximumActive, "The maximum amount of active Vessel Repair contracts that have been accepted");
		configNode91.AddValue("MinimumExpireDays", VesselRepair.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode91.AddValue("MaximumExpireDays", VesselRepair.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode91.AddValue("DeadlineDays", VesselRepair.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode92.AddValue("BaseAdvance", VesselRepair.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode92.AddValue("BaseReward", VesselRepair.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode92.AddValue("BaseFailure", VesselRepair.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode93.AddValue("BaseReward", VesselRepair.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode94.AddValue("BaseReward", VesselRepair.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode94.AddValue("BaseFailure", VesselRepair.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode95.AddValue("FundsMultiplier", VesselRepair.Trivial.FundsMultiplier, "The multiplier this request places on funds in Trivial contracts");
		configNode95.AddValue("ScienceMultiplier", VesselRepair.Trivial.ScienceMultiplier, "The multiplier this request places on science in Trivial contracts");
		configNode95.AddValue("ReputationMultiplier", VesselRepair.Trivial.ReputationMultiplier, "The multiplier this request places on reputation in Trivial contracts");
		configNode95.AddValue("MaxOrbitEccentricityFactor", VesselRepair.Trivial.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Trivial contracts");
		configNode95.AddValue("OrbitAltitudeFactor", VesselRepair.Trivial.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Trivial contracts");
		configNode95.AddValue("OrbitInclinationFactor", VesselRepair.Trivial.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Trivial contracts");
		configNode96.AddValue("FundsMultiplier", VesselRepair.Significant.FundsMultiplier, "The multiplier this request places on funds in Significant contracts");
		configNode96.AddValue("ScienceMultiplier", VesselRepair.Significant.ScienceMultiplier, "The multiplier this request places on science in Significant contracts");
		configNode96.AddValue("ReputationMultiplier", VesselRepair.Significant.ReputationMultiplier, "The multiplier this request places on reputation in Significant contracts");
		configNode96.AddValue("MaxOrbitEccentricityFactor", VesselRepair.Significant.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Significant contracts");
		configNode96.AddValue("OrbitAltitudeFactor", VesselRepair.Significant.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Significant contracts");
		configNode96.AddValue("OrbitInclinationFactor", VesselRepair.Significant.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Significant contracts");
		configNode97.AddValue("FundsMultiplier", VesselRepair.Exceptional.FundsMultiplier, "The multiplier this request places on funds in Exceptional contracts");
		configNode97.AddValue("ScienceMultiplier", VesselRepair.Exceptional.ScienceMultiplier, "The multiplier this request places on science in Exceptional contracts");
		configNode97.AddValue("ReputationMultiplier", VesselRepair.Exceptional.ReputationMultiplier, "The multiplier this request places on reputation in Exceptional contracts");
		configNode97.AddValue("MaxOrbitEccentricityFactor", VesselRepair.Exceptional.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Exceptional contracts");
		configNode97.AddValue("OrbitAltitudeFactor", VesselRepair.Exceptional.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Exceptional contracts");
		configNode97.AddValue("OrbitInclinationFactor", VesselRepair.Exceptional.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Exceptional contracts");
		configNode98.AddValue("MaximumAvailable", OrbitalConstruction.MaximumAvailable, "The maximum amount of inactive Orbital Construction contracts in Mission Control");
		configNode98.AddValue("MaximumActive", OrbitalConstruction.MaximumActive, "The maximum amount of active Orbital Construction contracts that have been accepted");
		configNode98.AddValue("TechNodeRequired", OrbitalConstruction.TechNodeRequired, "The Tech tree node that must be unlocked to be offered this contract");
		configNode99.AddValue("MinimumExpireDays", OrbitalConstruction.Expire.MinimumExpireDays, "The minimum time in days the contract will remain in Mission Control before expiring");
		configNode99.AddValue("MaximumExpireDays", OrbitalConstruction.Expire.MaximumExpireDays, "The maximum time in days the contract will remain in Mission Control before expiring");
		configNode99.AddValue("DeadlineDays", OrbitalConstruction.Expire.DeadlineDays, "How many days the player gets to complete the contract");
		configNode100.AddValue("BaseAdvance", OrbitalConstruction.Funds.BaseAdvance, "The base funds advance before planet and difficulty modifiers");
		configNode100.AddValue("BaseReward", OrbitalConstruction.Funds.BaseReward, "The base funds completion reward before planet and difficulty modifiers");
		configNode100.AddValue("BaseFailure", OrbitalConstruction.Funds.BaseFailure, "The base funds failure penalty before planet and difficulty modifiers");
		configNode101.AddValue("BaseReward", OrbitalConstruction.Science.BaseReward, "The base science completion reward before difficulty modifiers");
		configNode102.AddValue("BaseReward", OrbitalConstruction.Reputation.BaseReward, "The base reputation completion reward before difficulty modifiers");
		configNode102.AddValue("BaseFailure", OrbitalConstruction.Reputation.BaseFailure, "The base reputation failure penalty before difficulty modifiers");
		configNode103.AddValue("FundsMultiplier", OrbitalConstruction.Trivial.FundsMultiplier, "The multiplier this request places on funds in Trivial contracts");
		configNode103.AddValue("ScienceMultiplier", OrbitalConstruction.Trivial.ScienceMultiplier, "The multiplier this request places on science in Trivial contracts");
		configNode103.AddValue("ReputationMultiplier", OrbitalConstruction.Trivial.ReputationMultiplier, "The multiplier this request places on reputation in Trivial contracts");
		configNode103.AddValue("MaxOrbitEccentricityFactor", OrbitalConstruction.Trivial.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Trivial contracts");
		configNode103.AddValue("OrbitAltitudeFactor", OrbitalConstruction.Trivial.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Trivial contracts");
		configNode103.AddValue("OrbitInclinationFactor", OrbitalConstruction.Trivial.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Trivial contracts");
		configNode104.AddValue("FundsMultiplier", OrbitalConstruction.Significant.FundsMultiplier, "The multiplier this request places on funds in Significant contracts");
		configNode104.AddValue("ScienceMultiplier", OrbitalConstruction.Significant.ScienceMultiplier, "The multiplier this request places on science in Significant contracts");
		configNode104.AddValue("ReputationMultiplier", OrbitalConstruction.Significant.ReputationMultiplier, "The multiplier this request places on reputation in Significant contracts");
		configNode104.AddValue("MaxOrbitEccentricityFactor", OrbitalConstruction.Significant.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Significant contracts");
		configNode104.AddValue("OrbitAltitudeFactor", OrbitalConstruction.Significant.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Significant contracts");
		configNode104.AddValue("OrbitInclinationFactor", OrbitalConstruction.Significant.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Significant contracts");
		configNode105.AddValue("FundsMultiplier", OrbitalConstruction.Exceptional.FundsMultiplier, "The multiplier this request places on funds in Exceptional contracts");
		configNode105.AddValue("ScienceMultiplier", OrbitalConstruction.Exceptional.ScienceMultiplier, "The multiplier this request places on science in Exceptional contracts");
		configNode105.AddValue("ReputationMultiplier", OrbitalConstruction.Exceptional.ReputationMultiplier, "The multiplier this request places on reputation in Exceptional contracts");
		configNode105.AddValue("MaxOrbitEccentricityFactor", OrbitalConstruction.Exceptional.MaxOrbitEccentricityFactor, "The multiplier this request places on the orbit eccentricity in Exceptional contracts");
		configNode105.AddValue("OrbitAltitudeFactor", OrbitalConstruction.Exceptional.OrbitAltitudeFactor, "The multiplier this request places on the orbit altitude in Exceptional contracts");
		configNode105.AddValue("OrbitInclinationFactor", OrbitalConstruction.Exceptional.OrbitInclinationFactor, "The multiplier this request places on the orbit inclination in Exceptional contracts");
		config.Save(KSPUtil.ApplicationRootPath + "GameData/Squad/Contracts/Contracts.cfg");
		config = configNode;
	}

	public static void LoadConfig()
	{
		ConfigNode configNode = null;
		ConfigNode configNode2 = null;
		ConfigNode configNode3 = null;
		ConfigNode configNode4 = null;
		ConfigNode configNode5 = null;
		ConfigNode configNode6 = null;
		ConfigNode configNode7 = null;
		ConfigNode configNode8 = null;
		ConfigNode configNode9 = null;
		ConfigNode configNode10 = null;
		ConfigNode configNode11 = null;
		ConfigNode configNode12 = null;
		ConfigNode configNode13 = null;
		ConfigNode configNode14 = null;
		ConfigNode configNode15 = null;
		ConfigNode configNode16 = null;
		ConfigNode configNode17 = null;
		ConfigNode node = null;
		ConfigNode node2 = null;
		ConfigNode node3 = null;
		ConfigNode node4 = null;
		ConfigNode node5 = null;
		ConfigNode node6 = null;
		ConfigNode node7 = null;
		ConfigNode node8 = null;
		ConfigNode node9 = null;
		ConfigNode node10 = null;
		ConfigNode node11 = null;
		ConfigNode node12 = null;
		ConfigNode node13 = null;
		ConfigNode node14 = null;
		ConfigNode node15 = null;
		ConfigNode node16 = null;
		ConfigNode node17 = null;
		ConfigNode node18 = null;
		ConfigNode node19 = null;
		ConfigNode node20 = null;
		ConfigNode node21 = null;
		ConfigNode node22 = null;
		ConfigNode node23 = null;
		ConfigNode node24 = null;
		ConfigNode node25 = null;
		ConfigNode node26 = null;
		ConfigNode node27 = null;
		ConfigNode node28 = null;
		ConfigNode node29 = null;
		ConfigNode node30 = null;
		ConfigNode node31 = null;
		ConfigNode node32 = null;
		ConfigNode node33 = null;
		ConfigNode node34 = null;
		ConfigNode node35 = null;
		ConfigNode node36 = null;
		ConfigNode node37 = null;
		ConfigNode node38 = null;
		ConfigNode node39 = null;
		ConfigNode node40 = null;
		ConfigNode node41 = null;
		ConfigNode node42 = null;
		ConfigNode node43 = null;
		ConfigNode node44 = null;
		ConfigNode node45 = null;
		ConfigNode node46 = null;
		ConfigNode node47 = null;
		ConfigNode node48 = null;
		ConfigNode node49 = null;
		ConfigNode node50 = null;
		ConfigNode node51 = null;
		ConfigNode node52 = null;
		ConfigNode node53 = null;
		ConfigNode node54 = null;
		ConfigNode node55 = null;
		ConfigNode node56 = null;
		ConfigNode node57 = null;
		ConfigNode node58 = null;
		ConfigNode node59 = null;
		ConfigNode node60 = null;
		ConfigNode node61 = null;
		ConfigNode node62 = null;
		ConfigNode node63 = null;
		ConfigNode node64 = null;
		ConfigNode node65 = null;
		ConfigNode node66 = null;
		ConfigNode node67 = null;
		ConfigNode node68 = null;
		ConfigNode node69 = null;
		ConfigNode node70 = null;
		ConfigNode configNode18 = null;
		ConfigNode node71 = null;
		ConfigNode node72 = null;
		ConfigNode node73 = null;
		ConfigNode node74 = null;
		ConfigNode node75 = null;
		ConfigNode node76 = null;
		ConfigNode node77 = null;
		ConfigNode configNode19 = null;
		ConfigNode node78 = null;
		ConfigNode node79 = null;
		ConfigNode node80 = null;
		ConfigNode node81 = null;
		ConfigNode node82 = null;
		ConfigNode node83 = null;
		ConfigNode node84 = null;
		ConfigNode configNode20 = null;
		ConfigNode node85 = null;
		ConfigNode node86 = null;
		ConfigNode node87 = null;
		ConfigNode node88 = null;
		ConfigNode node89 = null;
		ConfigNode node90 = null;
		ConfigNode node91 = null;
		ConfigNode configNode21 = null;
		ConfigNode node92 = null;
		ConfigNode node93 = null;
		ConfigNode node94 = null;
		ConfigNode node95 = null;
		ConfigNode node96 = null;
		ConfigNode node97 = null;
		ConfigNode node98 = null;
		configNode = config.GetNode("ARM");
		if (configNode != null)
		{
			node = configNode.GetNode("Expiration");
			node2 = configNode.GetNode("Funds");
			node3 = configNode.GetNode("Science");
			node4 = configNode.GetNode("Reputation");
		}
		configNode2 = config.GetNode("Base");
		if (configNode2 != null)
		{
			node5 = configNode2.GetNode("Expiration");
			node6 = configNode2.GetNode("Funds");
			node7 = configNode2.GetNode("Science");
			node8 = configNode2.GetNode("Reputation");
		}
		configNode3 = config.GetNode("Flag");
		if (configNode3 != null)
		{
			node9 = configNode3.GetNode("Expiration");
			node10 = configNode3.GetNode("Funds");
			node11 = configNode3.GetNode("Science");
			node12 = configNode3.GetNode("Reputation");
		}
		configNode4 = config.GetNode("Grand");
		if (configNode4 != null)
		{
			node13 = configNode4.GetNode("Expiration");
			node14 = configNode4.GetNode("Funds");
			node15 = configNode4.GetNode("Science");
			node16 = configNode4.GetNode("Reputation");
		}
		configNode5 = config.GetNode("ISRU");
		if (configNode5 != null)
		{
			node17 = configNode5.GetNode("Expiration");
			node18 = configNode5.GetNode("Funds");
			node19 = configNode5.GetNode("Science");
			node20 = configNode5.GetNode("Reputation");
		}
		configNode6 = config.GetNode("Progression");
		if (configNode6 != null)
		{
			node21 = configNode6.GetNode("Funds");
			node22 = configNode6.GetNode("Science");
			node23 = configNode6.GetNode("Reputation");
		}
		configNode7 = config.GetNode("Recovery");
		if (configNode7 != null)
		{
			node24 = configNode7.GetNode("Expiration");
			node25 = configNode7.GetNode("Funds");
			node26 = configNode7.GetNode("Science");
			node27 = configNode7.GetNode("Reputation");
		}
		configNode8 = config.GetNode("Satellite");
		if (configNode8 != null)
		{
			node28 = configNode8.GetNode("Expiration");
			node29 = configNode8.GetNode("Funds");
			node30 = configNode8.GetNode("Science");
			node31 = configNode8.GetNode("Reputation");
		}
		configNode9 = config.GetNode("Science");
		if (configNode9 != null)
		{
			node32 = configNode9.GetNode("Expiration");
			node33 = configNode9.GetNode("Funds");
			node34 = configNode9.GetNode("Science");
			node35 = configNode9.GetNode("Reputation");
		}
		configNode10 = config.GetNode("Sentinel");
		if (configNode10 != null)
		{
			node36 = configNode10.GetNode("Expiration");
			node37 = configNode10.GetNode("Funds");
			node38 = configNode10.GetNode("Science");
			node39 = configNode10.GetNode("Reputation");
			node40 = configNode10.GetNode("Trivial");
			node41 = configNode10.GetNode("Significant");
			node42 = configNode10.GetNode("Exceptional");
		}
		configNode11 = config.GetNode("Station");
		if (configNode11 != null)
		{
			node43 = configNode11.GetNode("Expiration");
			node44 = configNode11.GetNode("Funds");
			node45 = configNode11.GetNode("Science");
			node46 = configNode11.GetNode("Reputation");
		}
		configNode12 = config.GetNode("Survey");
		if (configNode12 != null)
		{
			node47 = configNode12.GetNode("Expiration");
			node48 = configNode12.GetNode("Funds");
			node49 = configNode12.GetNode("Science");
			node50 = configNode12.GetNode("Reputation");
		}
		configNode13 = config.GetNode("Test");
		if (configNode13 != null)
		{
			node51 = configNode13.GetNode("Expiration");
			node52 = configNode13.GetNode("Funds");
			node53 = configNode13.GetNode("Science");
			node54 = configNode13.GetNode("Reputation");
		}
		configNode14 = config.GetNode("Tour");
		if (configNode14 != null)
		{
			node55 = configNode14.GetNode("Expiration");
			node56 = configNode14.GetNode("Funds");
			node57 = configNode14.GetNode("Science");
			node58 = configNode14.GetNode("Reputation");
		}
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			configNode15 = config.GetNode("DeployedScience");
			if (configNode15 != null)
			{
				node59 = configNode15.GetNode("Expiration");
				node60 = configNode15.GetNode("Funds");
				node61 = configNode15.GetNode("Science");
				node62 = configNode15.GetNode("Reputation");
			}
			configNode16 = config.GetNode("ROCScienceArm");
			if (configNode16 != null)
			{
				node63 = configNode16.GetNode("Expiration");
				node64 = configNode16.GetNode("Funds");
				node65 = configNode16.GetNode("Science");
				node66 = configNode16.GetNode("Reputation");
			}
			configNode17 = config.GetNode("ROCScienceRetrieval");
			if (configNode17 != null)
			{
				node67 = configNode17.GetNode("Expiration");
				node68 = configNode17.GetNode("Funds");
				node69 = configNode17.GetNode("Science");
				node70 = configNode17.GetNode("Reputation");
			}
		}
		configNode18 = config.GetNode("CometSample");
		if (configNode18 != null)
		{
			node71 = configNode18.GetNode("Expiration");
			node72 = configNode18.GetNode("Funds");
			node73 = configNode18.GetNode("Science");
			node74 = configNode18.GetNode("Reputation");
			node75 = configNode18.GetNode("Trivial");
			node76 = configNode18.GetNode("Significant");
			node77 = configNode18.GetNode("Exceptional");
		}
		configNode19 = config.GetNode("RoverConstruction");
		if (configNode19 != null)
		{
			node78 = configNode19.GetNode("Expiration");
			node79 = configNode19.GetNode("Funds");
			node80 = configNode19.GetNode("Science");
			node81 = configNode19.GetNode("Reputation");
			node82 = configNode19.GetNode("Trivial");
			node83 = configNode19.GetNode("Significant");
			node84 = configNode19.GetNode("Exceptional");
		}
		configNode20 = config.GetNode("VesselRepair");
		if (configNode20 != null)
		{
			node85 = configNode20.GetNode("Expiration");
			node86 = configNode20.GetNode("Funds");
			node87 = configNode20.GetNode("Science");
			node88 = configNode20.GetNode("Reputation");
			node89 = configNode20.GetNode("Trivial");
			node90 = configNode20.GetNode("Significant");
			node91 = configNode20.GetNode("Exceptional");
		}
		configNode21 = config.GetNode("OrbitalConstruction");
		if (configNode21 != null)
		{
			node92 = configNode21.GetNode("Expiration");
			node93 = configNode21.GetNode("Funds");
			node94 = configNode21.GetNode("Science");
			node95 = configNode21.GetNode("Reputation");
			node96 = configNode21.GetNode("Trivial");
			node97 = configNode21.GetNode("Significant");
			node98 = configNode21.GetNode("Exceptional");
		}
		SystemUtilities.LoadNode(config, "ContractDefs", "WeightDefault", ref WeightDefault, WeightDefault);
		SystemUtilities.LoadNode(config, "ContractDefs", "WeightMinimum", ref WeightMinimum, WeightMinimum);
		SystemUtilities.LoadNode(config, "ContractDefs", "WeightMaximum", ref WeightMaximum, WeightMaximum);
		SystemUtilities.LoadNode(config, "ContractDefs", "WeightAcceptDelta", ref WeightAcceptDelta, WeightAcceptDelta);
		SystemUtilities.LoadNode(config, "ContractDefs", "WeightDeclineDelta", ref WeightDeclineDelta, WeightDeclineDelta);
		SystemUtilities.LoadNode(config, "ContractDefs", "DisplayOfferedOrbits", ref DisplayOfferedOrbits, DisplayOfferedOrbits);
		SystemUtilities.LoadNode(config, "ContractDefs", "DisplayOfferedWaypoints", ref DisplayOfferedWaypoints, DisplayOfferedWaypoints);
		SystemUtilities.LoadNode(config, "ContractDefs", "SurveyNavigationGhosting", ref SurveyNavigationGhosting, SurveyNavigationGhosting);
		SystemUtilities.LoadNode(config, "ContractDefs", "AverageAvailableContracts", ref AverageAvailableContracts, AverageAvailableContracts);
		SystemUtilities.LoadNode(config, "ContractDefs", "FacilityProgressionFactor", ref FacilityProgressionFactor, FacilityProgressionFactor);
		SystemUtilities.LoadNode(config, "ContractDefs", "SolarOrbitHeatTolerance", ref SolarOrbitHeatTolerance, SolarOrbitHeatTolerance);
		SystemUtilities.LoadNode(config, "ContractDefs", "SunStationaryName", ref SunStationaryName, SunStationaryName);
		SystemUtilities.LoadNode(config, "ContractDefs", "HomeStationaryName", ref HomeStationaryName, HomeStationaryName);
		SystemUtilities.LoadNode(config, "ContractDefs", "OtherStationaryName", ref OtherStationaryName, OtherStationaryName);
		SystemUtilities.LoadNode(config, "ContractDefs", "SunSynchronousName", ref SunSynchronousName, SunSynchronousName);
		SystemUtilities.LoadNode(config, "ContractDefs", "HomeSynchronousName", ref HomeSynchronousName, HomeSynchronousName);
		SystemUtilities.LoadNode(config, "ContractDefs", "OtherSynchronousName", ref OtherSynchronousName, OtherSynchronousName);
		SystemUtilities.LoadNode(config, "ContractDefs", "MolniyaName", ref MolniyaName, MolniyaName);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.MaximumExistent", ref GClass6.MaximumExistent, GClass6.MaximumExistent);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.SignificantSolarEjectionChance", ref GClass6.SignificantSolarEjectionChance, GClass6.SignificantSolarEjectionChance);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.ExceptionalSolarEjectionChance", ref GClass6.ExceptionalSolarEjectionChance, GClass6.ExceptionalSolarEjectionChance);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.HomeLandingChance", ref GClass6.HomeLandingChance, GClass6.HomeLandingChance);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.AllowSolarEjections", ref GClass6.AllowSolarEjections, GClass6.AllowSolarEjections);
		SystemUtilities.LoadNode(configNode, "ContractDefs", "ARM.AllowHomeLandings", ref GClass6.AllowHomeLandings, GClass6.AllowHomeLandings);
		SystemUtilities.LoadNode(node, "ContractDefs", "ARM.Expire.MinimumExpireDays", ref GClass6.Expire.MinimumExpireDays, GClass6.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node, "ContractDefs", "ARM.Expire.MaximumExpireDays", ref GClass6.Expire.MaximumExpireDays, GClass6.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node, "ContractDefs", "ARM.Expire.DeadlineDays", ref GClass6.Expire.DeadlineDays, GClass6.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node2, "ContractDefs", "ARM.Funds.BaseAdvance", ref GClass6.Funds.BaseAdvance, GClass6.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node2, "ContractDefs", "ARM.Funds.BaseReward", ref GClass6.Funds.BaseReward, GClass6.Funds.BaseReward);
		SystemUtilities.LoadNode(node2, "ContractDefs", "ARM.Funds.BaseFailure", ref GClass6.Funds.BaseFailure, GClass6.Funds.BaseFailure);
		SystemUtilities.LoadNode(node2, "ContractDefs", "ARM.Funds.SolarEjectionMultiplier", ref GClass6.Funds.SolarEjectionMultiplier, GClass6.Funds.SolarEjectionMultiplier);
		SystemUtilities.LoadNode(node3, "ContractDefs", "ARM.Science.BaseReward", ref GClass6.Science.BaseReward, GClass6.Science.BaseReward);
		SystemUtilities.LoadNode(node3, "ContractDefs", "ARM.Science.SolarEjectionMultiplier", ref GClass6.Science.SolarEjectionMultiplier, GClass6.Science.SolarEjectionMultiplier);
		SystemUtilities.LoadNode(node4, "ContractDefs", "ARM.Reputation.BaseReward", ref GClass6.Reputation.BaseReward, GClass6.Reputation.BaseReward);
		SystemUtilities.LoadNode(node4, "ContractDefs", "ARM.Reputation.BaseFailure", ref GClass6.Reputation.BaseFailure, GClass6.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node4, "ContractDefs", "ARM.Reputation.SolarEjectionMultiplier", ref GClass6.Reputation.SolarEjectionMultiplier, GClass6.Reputation.SolarEjectionMultiplier);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.MaximumExistent", ref Base.MaximumExistent, Base.MaximumExistent);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.ContextualChance", ref Base.ContextualChance, Base.ContextualChance);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.ContextualAssets", ref Base.ContextualAssets, Base.ContextualAssets);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.TrivialMobileChance", ref Base.TrivialMobileChance, Base.TrivialMobileChance);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.SignificantMobileChance", ref Base.SignificantMobileChance, Base.SignificantMobileChance);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.ExceptionalMobileChance", ref Base.ExceptionalMobileChance, Base.ExceptionalMobileChance);
		SystemUtilities.LoadNode(configNode2, "ContractDefs", "Base.AllowMobile", ref Base.AllowMobile, Base.AllowMobile);
		SystemUtilities.LoadNode(node5, "ContractDefs", "Base.Expire.MinimumExpireDays", ref Base.Expire.MinimumExpireDays, Base.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node5, "ContractDefs", "Base.Expire.MaximumExpireDays", ref Base.Expire.MaximumExpireDays, Base.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node5, "ContractDefs", "Base.Expire.DeadlineDays", ref Base.Expire.DeadlineDays, Base.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node6, "ContractDefs", "Base.Funds.BaseAdvance", ref Base.Funds.BaseAdvance, Base.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node6, "ContractDefs", "Base.Funds.BaseReward", ref Base.Funds.BaseReward, Base.Funds.BaseReward);
		SystemUtilities.LoadNode(node6, "ContractDefs", "Base.Funds.BaseFailure", ref Base.Funds.BaseFailure, Base.Funds.BaseFailure);
		SystemUtilities.LoadNode(node6, "ContractDefs", "Base.Funds.MobileMultiplier", ref Base.Funds.MobileMultiplier, Base.Funds.MobileMultiplier);
		SystemUtilities.LoadNode(node7, "ContractDefs", "Base.Science.BaseReward", ref Base.Science.BaseReward, Base.Science.BaseReward);
		SystemUtilities.LoadNode(node7, "ContractDefs", "Base.Science.MobileMultiplier", ref Base.Science.MobileMultiplier, Base.Science.MobileMultiplier);
		SystemUtilities.LoadNode(node8, "ContractDefs", "Base.Reputation.BaseReward", ref Base.Reputation.BaseReward, Base.Reputation.BaseReward);
		SystemUtilities.LoadNode(node8, "ContractDefs", "Base.Reputation.BaseFailure", ref Base.Reputation.BaseFailure, Base.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node8, "ContractDefs", "Base.Reputation.MobileMultiplier", ref Base.Reputation.MobileMultiplier, Base.Reputation.MobileMultiplier);
		SystemUtilities.LoadNode(configNode3, "ContractDefs", "Flag.MaximumExistent", ref Flag.MaximumExistent, Flag.MaximumExistent);
		SystemUtilities.LoadNode(node9, "ContractDefs", "Flag.Expire.MinimumExpireDays", ref Flag.Expire.MinimumExpireDays, Flag.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node9, "ContractDefs", "Flag.Expire.MaximumExpireDays", ref Flag.Expire.MaximumExpireDays, Flag.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node9, "ContractDefs", "Flag.Expire.DeadlineDays", ref Flag.Expire.DeadlineDays, Flag.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node10, "ContractDefs", "Flag.Funds.BaseAdvance", ref Flag.Funds.BaseAdvance, Flag.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node10, "ContractDefs", "Flag.Funds.BaseReward", ref Flag.Funds.BaseReward, Flag.Funds.BaseReward);
		SystemUtilities.LoadNode(node10, "ContractDefs", "Flag.Funds.BaseFailure", ref Flag.Funds.BaseFailure, Flag.Funds.BaseFailure);
		SystemUtilities.LoadNode(node11, "ContractDefs", "Flag.Science.BaseReward", ref Flag.Science.BaseReward, Flag.Science.BaseReward);
		SystemUtilities.LoadNode(node12, "ContractDefs", "Flag.Reputation.BaseReward", ref Flag.Reputation.BaseReward, Flag.Reputation.BaseReward);
		SystemUtilities.LoadNode(node12, "ContractDefs", "Flag.Reputation.BaseFailure", ref Flag.Reputation.BaseFailure, Flag.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode4, "ContractDefs", "Grand.Rarity", ref Grand.Rarity, Grand.Rarity);
		SystemUtilities.LoadNode(node13, "ContractDefs", "Grand.Expire.MinimumExpireDays", ref Grand.Expire.MinimumExpireDays, Grand.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node13, "ContractDefs", "Grand.Expire.MaximumExpireDays", ref Grand.Expire.MaximumExpireDays, Grand.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node13, "ContractDefs", "Grand.Expire.DeadlineDays", ref Grand.Expire.DeadlineDays, Grand.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node14, "ContractDefs", "Grand.Funds.BaseAdvance", ref Grand.Funds.BaseAdvance, Grand.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node14, "ContractDefs", "Grand.Funds.BaseReward", ref Grand.Funds.BaseReward, Grand.Funds.BaseReward);
		SystemUtilities.LoadNode(node14, "ContractDefs", "Grand.Funds.BaseFailure", ref Grand.Funds.BaseFailure, Grand.Funds.BaseFailure);
		SystemUtilities.LoadNode(node15, "ContractDefs", "Grand.Science.BaseReward", ref Grand.Science.BaseReward, Grand.Science.BaseReward);
		SystemUtilities.LoadNode(node16, "ContractDefs", "Grand.Reputation.BaseReward", ref Grand.Reputation.BaseReward, Grand.Reputation.BaseReward);
		SystemUtilities.LoadNode(node16, "ContractDefs", "Grand.Reputation.BaseFailure", ref Grand.Reputation.BaseFailure, Grand.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode5, "ContractDefs", "ISRU.MaximumExistent", ref ISRU.MaximumExistent, ISRU.MaximumExistent);
		SystemUtilities.LoadNode(node17, "ContractDefs", "ISRU.Expire.MinimumExpireDays", ref ISRU.Expire.MinimumExpireDays, ISRU.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node17, "ContractDefs", "ISRU.Expire.MaximumExpireDays", ref ISRU.Expire.MaximumExpireDays, ISRU.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node17, "ContractDefs", "ISRU.Expire.DeadlineDays", ref ISRU.Expire.DeadlineDays, ISRU.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node18, "ContractDefs", "ISRU.Funds.BaseAdvance", ref ISRU.Funds.BaseAdvance, ISRU.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node18, "ContractDefs", "ISRU.Funds.BaseReward", ref ISRU.Funds.BaseReward, ISRU.Funds.BaseReward);
		SystemUtilities.LoadNode(node18, "ContractDefs", "ISRU.Funds.BaseFailure", ref ISRU.Funds.BaseFailure, ISRU.Funds.BaseFailure);
		SystemUtilities.LoadNode(node19, "ContractDefs", "ISRU.Science.BaseReward", ref ISRU.Science.BaseReward, ISRU.Science.BaseReward);
		SystemUtilities.LoadNode(node20, "ContractDefs", "ISRU.Reputation.BaseReward", ref ISRU.Reputation.BaseReward, ISRU.Reputation.BaseReward);
		SystemUtilities.LoadNode(node20, "ContractDefs", "ISRU.Reputation.BaseFailure", ref ISRU.Reputation.BaseFailure, ISRU.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.DisableTutorialContracts", ref Progression.DisableTutorialContracts, Progression.DisableTutorialContracts);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.DisableProgressionContracts", ref Progression.DisableProgressionContracts, Progression.DisableProgressionContracts);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.MaxDepthRecord", ref Progression.MaxDepthRecord, Progression.MaxDepthRecord);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.MaxDistanceRecord", ref Progression.MaxDistanceRecord, Progression.MaxDistanceRecord);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.MaxSpeedRecord", ref Progression.MaxSpeedRecord, Progression.MaxSpeedRecord);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.OutlierMilestoneMultiplier", ref Progression.OutlierMilestoneMultiplier, Progression.OutlierMilestoneMultiplier);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.PassiveBaseRatio", ref Progression.PassiveBaseRatio, Progression.PassiveBaseRatio);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.PassiveBodyRatio", ref Progression.PassiveBodyRatio, Progression.PassiveBodyRatio);
		SystemUtilities.LoadNode(configNode6, "ContractDefs", "Progression.RecordSplit", ref Progression.RecordSplit, Progression.RecordSplit);
		SystemUtilities.LoadNode(node21, "ContractDefs", "Progression.Funds.BaseReward", ref Progression.Funds.BaseReward, Progression.Funds.BaseReward);
		SystemUtilities.LoadNode(node22, "ContractDefs", "Progression.Science.BaseReward", ref Progression.Science.BaseReward, Progression.Science.BaseReward);
		SystemUtilities.LoadNode(node23, "ContractDefs", "Progression.Reputation.BaseReward", ref Progression.Reputation.BaseReward, Progression.Reputation.BaseReward);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.MaximumAvailable", ref Recovery.MaximumAvailable, Recovery.MaximumAvailable);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.MaximumActive", ref Recovery.MaximumActive, Recovery.MaximumActive);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.AllowKerbalRescue", ref Recovery.AllowKerbalRescue, Recovery.AllowKerbalRescue);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.AllowPartRecovery", ref Recovery.AllowPartRecovery, Recovery.AllowPartRecovery);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.AllowCompoundRecovery", ref Recovery.AllowCompoundRecovery, Recovery.AllowCompoundRecovery);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.AllowLandedVacuum", ref Recovery.AllowLandedVacuum, Recovery.AllowLandedVacuum);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.AllowLandedAtmosphere", ref Recovery.AllowLandedAtmosphere, Recovery.AllowLandedAtmosphere);
		SystemUtilities.LoadNode(configNode7, "ContractDefs", "Recovery.HighOrbitDifficulty", ref Recovery.HighOrbitDifficulty, Recovery.HighOrbitDifficulty);
		SystemUtilities.LoadNode(node24, "ContractDefs", "Recovery.Expire.MinimumExpireDays", ref Recovery.Expire.MinimumExpireDays, Recovery.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node24, "ContractDefs", "Recovery.Expire.MaximumExpireDays", ref Recovery.Expire.MaximumExpireDays, Recovery.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node24, "ContractDefs", "Recovery.Expire.DeadlineDays", ref Recovery.Expire.DeadlineDays, Recovery.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node25, "ContractDefs", "Recovery.Funds.BaseAdvance", ref Recovery.Funds.BaseAdvance, Recovery.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node25, "ContractDefs", "Recovery.Funds.BaseReward", ref Recovery.Funds.BaseReward, Recovery.Funds.BaseReward);
		SystemUtilities.LoadNode(node25, "ContractDefs", "Recovery.Funds.BaseFailure", ref Recovery.Funds.BaseFailure, Recovery.Funds.BaseFailure);
		SystemUtilities.LoadNode(node26, "ContractDefs", "Recovery.Science.BaseReward", ref Recovery.Science.BaseReward, Recovery.Science.BaseReward);
		SystemUtilities.LoadNode(node27, "ContractDefs", "Recovery.Reputation.BaseReward", ref Recovery.Reputation.BaseReward, Recovery.Reputation.BaseReward);
		SystemUtilities.LoadNode(node27, "ContractDefs", "Recovery.Reputation.BaseFailure", ref Recovery.Reputation.BaseFailure, Recovery.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.MaximumAvailable", ref Satellite.MaximumAvailable, Satellite.MaximumAvailable);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.MaximumActive", ref Satellite.MaximumActive, Satellite.MaximumActive);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AnimationDuration", ref Satellite.AnimationDuration, Satellite.AnimationDuration);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ContextualChance", ref Satellite.ContextualChance, Satellite.ContextualChance);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ContextualAssets", ref Satellite.ContextualAssets, Satellite.ContextualAssets);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ContextualHomeAssets", ref Satellite.ContextualHomeAssets, Satellite.ContextualHomeAssets);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.NetworkMinimum", ref Satellite.NetworkMinimum, Satellite.NetworkMinimum);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.NetworkMaximum", ref Satellite.NetworkMaximum, Satellite.NetworkMaximum);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.MinimumDeviationWindow", ref Satellite.MinimumDeviationWindow, Satellite.MinimumDeviationWindow);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.TrivialDeviation", ref Satellite.TrivialDeviation, Satellite.TrivialDeviation);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.SignificantDeviation", ref Satellite.SignificantDeviation, Satellite.SignificantDeviation);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ExceptionalDeviation", ref Satellite.ExceptionalDeviation, Satellite.ExceptionalDeviation);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.TrivialAltitudeDifficulty", ref Satellite.TrivialAltitudeDifficulty, Satellite.TrivialAltitudeDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.TrivialInclinationDifficulty", ref Satellite.TrivialInclinationDifficulty, Satellite.TrivialInclinationDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.SignificantAltitudeDifficulty", ref Satellite.SignificantAltitudeDifficulty, Satellite.SignificantAltitudeDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.SignificantInclinationDifficulty", ref Satellite.SignificantInclinationDifficulty, Satellite.SignificantInclinationDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ExceptionalAltitudeDifficulty", ref Satellite.ExceptionalAltitudeDifficulty, Satellite.ExceptionalAltitudeDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.ExceptionalInclinationDifficulty", ref Satellite.ExceptionalInclinationDifficulty, Satellite.ExceptionalInclinationDifficulty);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.PreferHome", ref Satellite.PreferHome, Satellite.PreferHome);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowSolar", ref Satellite.AllowSolar, Satellite.AllowSolar);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowEquatorial", ref Satellite.AllowEquatorial, Satellite.AllowEquatorial);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowPolar", ref Satellite.AllowPolar, Satellite.AllowPolar);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowSynchronous", ref Satellite.AllowSynchronous, Satellite.AllowSynchronous);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowStationary", ref Satellite.AllowStationary, Satellite.AllowStationary);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowTundra", ref Satellite.AllowTundra, Satellite.AllowTundra);
		SystemUtilities.LoadNode(configNode8, "ContractDefs", "Satellite.AllowKolniya", ref Satellite.AllowKolniya, Satellite.AllowKolniya);
		SystemUtilities.LoadNode(node28, "ContractDefs", "Satellite.Expire.MinimumExpireDays", ref Satellite.Expire.MinimumExpireDays, Satellite.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node28, "ContractDefs", "Satellite.Expire.MaximumExpireDays", ref Satellite.Expire.MaximumExpireDays, Satellite.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node28, "ContractDefs", "Satellite.Expire.DeadlineDays", ref Satellite.Expire.DeadlineDays, Satellite.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.BaseAdvance", ref Satellite.Funds.BaseAdvance, Satellite.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.BaseReward", ref Satellite.Funds.BaseReward, Satellite.Funds.BaseReward);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.BaseFailure", ref Satellite.Funds.BaseFailure, Satellite.Funds.BaseFailure);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.HomeMultiplier", ref Satellite.Funds.HomeMultiplier, Satellite.Funds.HomeMultiplier);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.PolarMultiplier", ref Satellite.Funds.PolarMultiplier, Satellite.Funds.PolarMultiplier);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.SynchronousMultiplier", ref Satellite.Funds.SynchronousMultiplier, Satellite.Funds.SynchronousMultiplier);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.StationaryMultiplier", ref Satellite.Funds.StationaryMultiplier, Satellite.Funds.StationaryMultiplier);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.TundraMultiplier", ref Satellite.Funds.TundraMultiplier, Satellite.Funds.TundraMultiplier);
		SystemUtilities.LoadNode(node29, "ContractDefs", "Satellite.Funds.KolniyaMultiplier", ref Satellite.Funds.KolniyaMultiplier, Satellite.Funds.KolniyaMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.BaseReward", ref Satellite.Science.BaseReward, Satellite.Science.BaseReward);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.HomeMultiplier", ref Satellite.Science.HomeMultiplier, Satellite.Science.HomeMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.PolarMultiplier", ref Satellite.Science.PolarMultiplier, Satellite.Science.PolarMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.SynchronousMultiplier", ref Satellite.Science.SynchronousMultiplier, Satellite.Science.SynchronousMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.StationaryMultiplier", ref Satellite.Science.StationaryMultiplier, Satellite.Science.StationaryMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.TundraMultiplier", ref Satellite.Science.TundraMultiplier, Satellite.Science.TundraMultiplier);
		SystemUtilities.LoadNode(node30, "ContractDefs", "Satellite.Science.KolniyaMultiplier", ref Satellite.Science.KolniyaMultiplier, Satellite.Science.KolniyaMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.BaseReward", ref Satellite.Reputation.BaseReward, Satellite.Reputation.BaseReward);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.BaseFailure", ref Satellite.Reputation.BaseFailure, Satellite.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.HomeMultiplier", ref Satellite.Reputation.HomeMultiplier, Satellite.Reputation.HomeMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.PolarMultiplier", ref Satellite.Reputation.PolarMultiplier, Satellite.Reputation.PolarMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.SynchronousMultiplier", ref Satellite.Reputation.SynchronousMultiplier, Satellite.Reputation.SynchronousMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.StationaryMultiplier", ref Satellite.Reputation.StationaryMultiplier, Satellite.Reputation.StationaryMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.TundraMultiplier", ref Satellite.Reputation.TundraMultiplier, Satellite.Reputation.TundraMultiplier);
		SystemUtilities.LoadNode(node31, "ContractDefs", "Satellite.Reputation.KolniyaMultiplier", ref Satellite.Reputation.KolniyaMultiplier, Satellite.Reputation.KolniyaMultiplier);
		SystemUtilities.LoadNode(configNode9, "ContractDefs", "Science.MaximumExistent", ref Research.MaximumExistent, Research.MaximumExistent);
		SystemUtilities.LoadNode(node32, "ContractDefs", "Science.Expire.MinimumExpireDays", ref Research.Expire.MinimumExpireDays, Research.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node32, "ContractDefs", "Science.Expire.MaximumExpireDays", ref Research.Expire.MaximumExpireDays, Research.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node32, "ContractDefs", "Science.Expire.DeadlineDays", ref Research.Expire.DeadlineDays, Research.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node33, "ContractDefs", "Science.Funds.BaseAdvance", ref Research.Funds.BaseAdvance, Research.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node33, "ContractDefs", "Science.Funds.BaseReward", ref Research.Funds.BaseReward, Research.Funds.BaseReward);
		SystemUtilities.LoadNode(node33, "ContractDefs", "Science.Funds.BaseFailure", ref Research.Funds.BaseFailure, Research.Funds.BaseFailure);
		SystemUtilities.LoadNode(node34, "ContractDefs", "Science.Science.BaseReward", ref Research.Science.BaseReward, Research.Science.BaseReward);
		SystemUtilities.LoadNode(node35, "ContractDefs", "Science.Reputation.BaseReward", ref Research.Reputation.BaseReward, Research.Reputation.BaseReward);
		SystemUtilities.LoadNode(node35, "ContractDefs", "Science.Reputation.BaseFailure", ref Research.Reputation.BaseFailure, Research.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.MaximumActive", ref Sentinel.MaximumActive, Sentinel.MaximumActive);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.MaximumAvailable", ref Sentinel.MaximumAvailable, Sentinel.MaximumAvailable);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.ScanTypeBaseMultiplier", ref Sentinel.ScanTypeBaseMultiplier, Sentinel.ScanTypeBaseMultiplier);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.ScanTypeClassMultiplier", ref Sentinel.ScanTypeClassMultiplier, Sentinel.ScanTypeClassMultiplier);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.ScanTypeInclinationMultiplier", ref Sentinel.ScanTypeInclinationMultiplier, Sentinel.ScanTypeInclinationMultiplier);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.ScanTypeEccentricityMultiplier", ref Sentinel.ScanTypeEccentricityMultiplier, Sentinel.ScanTypeEccentricityMultiplier);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.TrivialDeviation", ref Sentinel.TrivialDeviation, Sentinel.TrivialDeviation);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.SignificantDeviation", ref Sentinel.SignificantDeviation, Sentinel.SignificantDeviation);
		SystemUtilities.LoadNode(configNode10, "ContractDefs", "Sentinel.ExceptionalDeviation", ref Sentinel.ExceptionalDeviation, Sentinel.ExceptionalDeviation);
		SystemUtilities.LoadNode(node36, "ContractDefs", "Sentinel.Expire.MinimumExpireDays", ref Sentinel.Expire.MinimumExpireDays, Sentinel.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node36, "ContractDefs", "Sentinel.Expire.MaximumExpireDays", ref Sentinel.Expire.MaximumExpireDays, Sentinel.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node36, "ContractDefs", "Sentinele.Expire.DeadlineDays", ref Sentinel.Expire.DeadlineDays, Sentinel.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node37, "ContractDefs", "Sentinel.Funds.BaseAdvance", ref Sentinel.Funds.BaseAdvance, Sentinel.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node37, "ContractDefs", "Sentinel.Funds.BaseReward", ref Sentinel.Funds.BaseReward, Sentinel.Funds.BaseReward);
		SystemUtilities.LoadNode(node37, "ContractDefs", "Sentinel.Funds.BaseFailure", ref Sentinel.Funds.BaseFailure, Sentinel.Funds.BaseFailure);
		SystemUtilities.LoadNode(node38, "ContractDefs", "Sentinel.Science.BaseReward", ref Sentinel.Science.BaseReward, Sentinel.Science.BaseReward);
		SystemUtilities.LoadNode(node39, "ContractDefs", "Sentinel.Reputation.BaseReward", ref Sentinel.Reputation.BaseReward, Sentinel.Reputation.BaseReward);
		SystemUtilities.LoadNode(node39, "ContractDefs", "Sentinel.Reputation.BaseFailure", ref Sentinel.Reputation.BaseFailure, Sentinel.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node40, "ContractDefs", "Sentinel.Trivial.FundsMultiplier", ref Sentinel.Trivial.FundsMultiplier, Sentinel.Trivial.FundsMultiplier);
		SystemUtilities.LoadNode(node40, "ContractDefs", "Sentinel.Trivial.ScienceMultiplier", ref Sentinel.Trivial.ScienceMultiplier, Sentinel.Trivial.ScienceMultiplier);
		SystemUtilities.LoadNode(node40, "ContractDefs", "Sentinel.Trivial.ReputationMultiplier", ref Sentinel.Trivial.ReputationMultiplier, Sentinel.Trivial.ReputationMultiplier);
		SystemUtilities.LoadNode(node40, "ContractDefs", "Sentinel.Trivial.MinAsteroids", ref Sentinel.Trivial.MinAsteroids, Sentinel.Trivial.MinAsteroids);
		SystemUtilities.LoadNode(node40, "ContractDefs", "Sentinel.Trivial.MaxAsteroids", ref Sentinel.Trivial.MaxAsteroids, Sentinel.Trivial.MaxAsteroids);
		SystemUtilities.LoadNode(node41, "ContractDefs", "Sentinel.Significant.FundsMultiplier", ref Sentinel.Significant.FundsMultiplier, Sentinel.Significant.FundsMultiplier);
		SystemUtilities.LoadNode(node41, "ContractDefs", "Sentinel.Significant.ScienceMultiplier", ref Sentinel.Significant.ScienceMultiplier, Sentinel.Significant.ScienceMultiplier);
		SystemUtilities.LoadNode(node41, "ContractDefs", "Sentinel.Significant.ReputationMultiplier", ref Sentinel.Significant.ReputationMultiplier, Sentinel.Significant.ReputationMultiplier);
		SystemUtilities.LoadNode(node41, "ContractDefs", "Sentinel.Significant.MinAsteroids", ref Sentinel.Significant.MinAsteroids, Sentinel.Significant.MinAsteroids);
		SystemUtilities.LoadNode(node41, "ContractDefs", "Sentinel.Significant.MaxAsteroids", ref Sentinel.Significant.MaxAsteroids, Sentinel.Significant.MaxAsteroids);
		SystemUtilities.LoadNode(node42, "ContractDefs", "Sentinel.Exceptional.FundsMultiplier", ref Sentinel.Exceptional.FundsMultiplier, Sentinel.Exceptional.FundsMultiplier);
		SystemUtilities.LoadNode(node42, "ContractDefs", "Sentinel.Exceptional.ScienceMultiplier", ref Sentinel.Exceptional.ScienceMultiplier, Sentinel.Exceptional.ScienceMultiplier);
		SystemUtilities.LoadNode(node42, "ContractDefs", "Sentinel.Exceptional.ReputationMultiplier", ref Sentinel.Exceptional.ReputationMultiplier, Sentinel.Exceptional.ReputationMultiplier);
		SystemUtilities.LoadNode(node42, "ContractDefs", "Sentinel.Exceptional.MinAsteroids", ref Sentinel.Exceptional.MinAsteroids, Sentinel.Exceptional.MinAsteroids);
		SystemUtilities.LoadNode(node42, "ContractDefs", "Sentinel.Exceptional.MaxAsteroids", ref Sentinel.Exceptional.MaxAsteroids, Sentinel.Exceptional.MaxAsteroids);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.MaximumExistent", ref Station.MaximumExistent, Station.MaximumExistent);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.ContextualChance", ref Station.ContextualChance, Station.ContextualChance);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.ContextualAssets", ref Station.ContextualAssets, Station.ContextualAssets);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.TrivialAsteroidChance", ref Station.TrivialAsteroidChance, Station.TrivialAsteroidChance);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.SignificantAsteroidChance", ref Station.SignificantAsteroidChance, Station.SignificantAsteroidChance);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.ExceptionalAsteroidChance", ref Station.ExceptionalAsteroidChance, Station.ExceptionalAsteroidChance);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.AllowAsteroid", ref Station.AllowAsteroid, Station.AllowAsteroid);
		SystemUtilities.LoadNode(configNode11, "ContractDefs", "Station.AllowSolar", ref Station.AllowSolar, Station.AllowSolar);
		SystemUtilities.LoadNode(node43, "ContractDefs", "Station.Expire.MinimumExpireDays", ref Station.Expire.MinimumExpireDays, Station.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node43, "ContractDefs", "Station.Expire.MaximumExpireDays", ref Station.Expire.MaximumExpireDays, Station.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node43, "ContractDefs", "Station.Expire.DeadlineDays", ref Station.Expire.DeadlineDays, Station.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node44, "ContractDefs", "Station.Funds.BaseAdvance", ref Station.Funds.BaseAdvance, Station.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node44, "ContractDefs", "Station.Funds.BaseReward", ref Station.Funds.BaseReward, Station.Funds.BaseReward);
		SystemUtilities.LoadNode(node44, "ContractDefs", "Station.Funds.BaseFailure", ref Station.Funds.BaseFailure, Station.Funds.BaseFailure);
		SystemUtilities.LoadNode(node44, "ContractDefs", "Station.Funds.AsteroidMultiplier", ref Station.Funds.AsteroidMultiplier, Station.Funds.AsteroidMultiplier);
		SystemUtilities.LoadNode(node45, "ContractDefs", "Station.Science.BaseReward", ref Station.Science.BaseReward, Station.Science.BaseReward);
		SystemUtilities.LoadNode(node45, "ContractDefs", "Station.Science.AsteroidMultiplier", ref Station.Science.AsteroidMultiplier, Station.Science.AsteroidMultiplier);
		SystemUtilities.LoadNode(node46, "ContractDefs", "Station.Reputation.BaseReward", ref Station.Reputation.BaseReward, Station.Reputation.BaseReward);
		SystemUtilities.LoadNode(node46, "ContractDefs", "Station.Reputation.BaseFailure", ref Station.Reputation.BaseFailure, Station.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node46, "ContractDefs", "Station.Reputation.AsteroidMultiplier", ref Station.Reputation.AsteroidMultiplier, Station.Reputation.AsteroidMultiplier);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MaximumAvailable", ref Survey.MaximumAvailable, Survey.MaximumAvailable);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MaximumActive", ref Survey.MaximumActive, Survey.MaximumActive);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.ContextualChance", ref Survey.ContextualChance, Survey.ContextualChance);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.ContextualAssets", ref Survey.ContextualAssets, Survey.ContextualAssets);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.TrivialWaypoints", ref Survey.TrivialWaypoints, Survey.TrivialWaypoints);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.SignificantWaypoints", ref Survey.SignificantWaypoints, Survey.SignificantWaypoints);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.ExceptionalWaypoints", ref Survey.ExceptionalWaypoints, Survey.ExceptionalWaypoints);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.HomeNearbyProgressCap", ref Survey.HomeNearbyProgressCap, Survey.HomeNearbyProgressCap);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.TrivialRange", ref Survey.TrivialRange, Survey.TrivialRange);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.SignificantRange", ref Survey.SignificantRange, Survey.SignificantRange);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.ExceptionalRange", ref Survey.ExceptionalRange, Survey.ExceptionalRange);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MinimumTriggerRange", ref Survey.MinimumTriggerRange, Survey.MinimumTriggerRange);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MaximumTriggerRange", ref Survey.MaximumTriggerRange, Survey.MaximumTriggerRange);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MinimumThreshold", ref Survey.MinimumThreshold, Survey.MinimumThreshold);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.MaximumThreshold", ref Survey.MaximumThreshold, Survey.MaximumThreshold);
		SystemUtilities.LoadNode(configNode12, "ContractDefs", "Survey.ThresholdDeviancy", ref Survey.ThresholdDeviancy, Survey.ThresholdDeviancy);
		SystemUtilities.LoadNode(node47, "ContractDefs", "Survey.Expire.MinimumExpireDays", ref Survey.Expire.MinimumExpireDays, Survey.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node47, "ContractDefs", "Survey.Expire.MaximumExpireDays", ref Survey.Expire.MaximumExpireDays, Survey.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node47, "ContractDefs", "Survey.Expire.DeadlineDays", ref Survey.Expire.DeadlineDays, Survey.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node48, "ContractDefs", "Survey.Funds.DefaultAdvance", ref Survey.Funds.DefaultAdvance, Survey.Funds.DefaultAdvance);
		SystemUtilities.LoadNode(node48, "ContractDefs", "Survey.Funds.DefaultReward", ref Survey.Funds.DefaultReward, Survey.Funds.DefaultReward);
		SystemUtilities.LoadNode(node48, "ContractDefs", "Survey.Funds.DefaultFailure", ref Survey.Funds.DefaultFailure, Survey.Funds.DefaultFailure);
		SystemUtilities.LoadNode(node48, "ContractDefs", "Survey.Funds.WaypointDefaultReward", ref Survey.Funds.WaypointDefaultReward, Survey.Funds.WaypointDefaultReward);
		SystemUtilities.LoadNode(node49, "ContractDefs", "Survey.Science.DefaultReward", ref Survey.Science.DefaultReward, Survey.Science.DefaultReward);
		SystemUtilities.LoadNode(node49, "ContractDefs", "Survey.Science.WaypointDefaultReward", ref Survey.Science.WaypointDefaultReward, Survey.Science.WaypointDefaultReward);
		SystemUtilities.LoadNode(node50, "ContractDefs", "Survey.Reputation.DefaultReward", ref Survey.Reputation.DefaultReward, Survey.Reputation.DefaultReward);
		SystemUtilities.LoadNode(node50, "ContractDefs", "Survey.Reputation.DefaultFailure", ref Survey.Reputation.DefaultFailure, Survey.Reputation.DefaultFailure);
		SystemUtilities.LoadNode(node50, "ContractDefs", "Survey.Reputation.WaypointDefaultReward", ref Survey.Reputation.WaypointDefaultReward, Survey.Reputation.WaypointDefaultReward);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.MaximumExistent", ref Test.MaximumExistent, Test.MaximumExistent);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.SubjectsToRepeat", ref Test.SubjectsToRepeat, Test.SubjectsToRepeat);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.AllowHauls", ref Test.AllowHauls, Test.AllowHauls);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.TrivialHaulChance", ref Test.TrivialHaulChance, Test.TrivialHaulChance);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.SignificantHaulChance", ref Test.SignificantHaulChance, Test.SignificantHaulChance);
		SystemUtilities.LoadNode(configNode13, "ContractDefs", "Test.ExceptionalHaulChance", ref Test.ExceptionalHaulChance, Test.ExceptionalHaulChance);
		SystemUtilities.LoadNode(node51, "ContractDefs", "Test.Expire.MinimumExpireDays", ref Test.Expire.MinimumExpireDays, Test.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node51, "ContractDefs", "Test.Expire.MaximumExpireDays", ref Test.Expire.MaximumExpireDays, Test.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node51, "ContractDefs", "Test.Expire.DeadlineDays", ref Test.Expire.DeadlineDays, Test.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node52, "ContractDefs", "Test.Funds.BaseAdvance", ref Test.Funds.BaseAdvance, Test.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node52, "ContractDefs", "Test.Funds.BaseReward", ref Test.Funds.BaseReward, Test.Funds.BaseReward);
		SystemUtilities.LoadNode(node52, "ContractDefs", "Test.Funds.BaseFailure", ref Test.Funds.BaseFailure, Test.Funds.BaseFailure);
		SystemUtilities.LoadNode(node53, "ContractDefs", "Test.Science.BaseReward", ref Test.Science.BaseReward, Test.Science.BaseReward);
		SystemUtilities.LoadNode(node54, "ContractDefs", "Test.Reputation.BaseReward", ref Test.Reputation.BaseReward, Test.Reputation.BaseReward);
		SystemUtilities.LoadNode(node54, "ContractDefs", "Test.Reputation.BaseFailure", ref Test.Reputation.BaseFailure, Test.Reputation.BaseFailure);
		SystemUtilities.LoadNode(configNode14, "ContractDefs", "Tour.MaximumActive", ref Tour.MaximumActive, Tour.MaximumActive);
		SystemUtilities.LoadNode(configNode14, "ContractDefs", "Tour.FailOnInactive", ref Tour.FailOnInactive, Tour.FailOnInactive);
		SystemUtilities.LoadNode(node55, "ContractDefs", "Tour.Expire.MinimumExpireDays", ref Tour.Expire.MinimumExpireDays, Tour.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(configNode14, "ContractDefs", "Tour.GeeAdventureChance", ref Tour.GeeAdventureChance, Tour.GeeAdventureChance);
		SystemUtilities.LoadNode(node55, "ContractDefs", "Tour.Expire.MaximumExpireDays", ref Tour.Expire.MaximumExpireDays, Tour.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node55, "ContractDefs", "Tour.Expire.DeadlineDays", ref Tour.Expire.DeadlineDays, Tour.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node56, "ContractDefs", "Tour.Funds.DefaultFare", ref Tour.Funds.DefaultFare, Tour.Funds.DefaultFare);
		SystemUtilities.LoadNode(node57, "ContractDefs", "Tour.Science.BaseReward", ref Tour.Science.BaseReward, Tour.Science.BaseReward);
		SystemUtilities.LoadNode(node58, "ContractDefs", "Tour.Reputation.BaseReward", ref Tour.Reputation.BaseReward, Tour.Reputation.BaseReward);
		SystemUtilities.LoadNode(node58, "ContractDefs", "Tour.Reputation.BaseFailure", ref Tour.Reputation.BaseFailure, Tour.Reputation.BaseFailure);
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			SystemUtilities.LoadNode(configNode15, "ContractDefs", "DeployedScience.MaximumExistent", ref DeployedScience.MaximumExistent, DeployedScience.MaximumExistent);
			SystemUtilities.LoadNode(configNode15, "ContractDefs", "DeployedScience.SciencePercentage", ref DeployedScience.SciencePercentage, DeployedScience.SciencePercentage);
			SystemUtilities.LoadNode(node59, "ContractDefs", "DeployedScience.Expire.MinimumExpireDays", ref DeployedScience.Expire.MinimumExpireDays, DeployedScience.Expire.MinimumExpireDays);
			SystemUtilities.LoadNode(node59, "ContractDefs", "DeployedScience.Expire.MaximumExpireDays", ref DeployedScience.Expire.MaximumExpireDays, DeployedScience.Expire.MaximumExpireDays);
			SystemUtilities.LoadNode(node59, "ContractDefs", "DeployedScience.Expire.DeadlineDays", ref DeployedScience.Expire.DeadlineDays, DeployedScience.Expire.DeadlineDays);
			SystemUtilities.LoadNode(node60, "ContractDefs", "DeployedScience.Funds.BaseAdvance", ref DeployedScience.Funds.BaseAdvance, DeployedScience.Funds.BaseAdvance);
			SystemUtilities.LoadNode(node60, "ContractDefs", "DeployedScience.Funds.BaseReward", ref DeployedScience.Funds.BaseReward, DeployedScience.Funds.BaseReward);
			SystemUtilities.LoadNode(node60, "ContractDefs", "DeployedScience.Funds.BaseFailure", ref DeployedScience.Funds.BaseFailure, DeployedScience.Funds.BaseFailure);
			SystemUtilities.LoadNode(node61, "ContractDefs", "DeployedScience.Science.BaseReward", ref DeployedScience.Science.BaseReward, DeployedScience.Science.BaseReward);
			SystemUtilities.LoadNode(node62, "ContractDefs", "DeployedScience.Reputation.BaseReward", ref DeployedScience.Reputation.BaseReward, DeployedScience.Reputation.BaseReward);
			SystemUtilities.LoadNode(node62, "ContractDefs", "DeployedScience.Reputation.BaseFailure", ref DeployedScience.Reputation.BaseFailure, DeployedScience.Reputation.BaseFailure);
			SystemUtilities.LoadNode(configNode16, "ContractDefs", "ROCScienceArm.MaximumExistent", ref ROCScienceArm.MaximumExistent, ROCScienceArm.MaximumExistent);
			SystemUtilities.LoadNode(configNode16, "ContractDefs", "ROCScienceArm.SimpleArmPercentage", ref ROCScienceArm.SimpleArmPercentage, ROCScienceArm.SimpleArmPercentage);
			SystemUtilities.LoadNode(configNode16, "ContractDefs", "ROCScienceArm.ComplexArmPercentage", ref ROCScienceArm.ComplexArmPercentage, ROCScienceArm.ComplexArmPercentage);
			SystemUtilities.LoadNode(node63, "ContractDefs", "ROCScienceArm.Expire.MinimumExpireDays", ref ROCScienceArm.Expire.MinimumExpireDays, ROCScienceArm.Expire.MinimumExpireDays);
			SystemUtilities.LoadNode(node63, "ContractDefs", "ROCScienceArm.Expire.MaximumExpireDays", ref ROCScienceArm.Expire.MaximumExpireDays, ROCScienceArm.Expire.MaximumExpireDays);
			SystemUtilities.LoadNode(node63, "ContractDefs", "ROCScienceArm.Expire.DeadlineDays", ref ROCScienceArm.Expire.DeadlineDays, ROCScienceArm.Expire.DeadlineDays);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.SimpleArmAdvance", ref ROCScienceArm.Funds.SimpleArmAdvance, ROCScienceArm.Funds.SimpleArmAdvance);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.ComplexArmAdvance", ref ROCScienceArm.Funds.ComplexArmAdvance, ROCScienceArm.Funds.ComplexArmAdvance);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.AdvancedArmAdvance", ref ROCScienceArm.Funds.AdvancedArmAdvance, ROCScienceArm.Funds.AdvancedArmAdvance);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.SimpleArmReward", ref ROCScienceArm.Funds.SimpleArmReward, ROCScienceArm.Funds.SimpleArmReward);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.ComplexArmReward", ref ROCScienceArm.Funds.ComplexArmReward, ROCScienceArm.Funds.ComplexArmReward);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.AdvancedArmReward", ref ROCScienceArm.Funds.AdvancedArmReward, ROCScienceArm.Funds.AdvancedArmReward);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.SimpleArmFailure", ref ROCScienceArm.Funds.SimpleArmFailure, ROCScienceArm.Funds.SimpleArmFailure);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.ComplexArmFailure", ref ROCScienceArm.Funds.ComplexArmFailure, ROCScienceArm.Funds.ComplexArmFailure);
			SystemUtilities.LoadNode(node64, "ContractDefs", "ROCScienceArm.Funds.AdvancedArmFailure", ref ROCScienceArm.Funds.AdvancedArmFailure, ROCScienceArm.Funds.AdvancedArmFailure);
			SystemUtilities.LoadNode(node65, "ContractDefs", "ROCScienceArm.Science.SimpleArmReward", ref ROCScienceArm.Science.SimpleArmReward, ROCScienceArm.Science.SimpleArmReward);
			SystemUtilities.LoadNode(node65, "ContractDefs", "ROCScienceArm.Science.ComplexArmReward", ref ROCScienceArm.Science.ComplexArmReward, ROCScienceArm.Science.ComplexArmReward);
			SystemUtilities.LoadNode(node65, "ContractDefs", "ROCScienceArm.Science.AdvancedArmReward", ref ROCScienceArm.Science.AdvancedArmReward, ROCScienceArm.Science.AdvancedArmReward);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.SimpleArmReward", ref ROCScienceArm.Reputation.SimpleArmReward, ROCScienceArm.Reputation.SimpleArmReward);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.ComplexArmReward", ref ROCScienceArm.Reputation.ComplexArmReward, ROCScienceArm.Reputation.ComplexArmReward);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.AdvancedArmReward", ref ROCScienceArm.Reputation.AdvancedArmReward, ROCScienceArm.Reputation.AdvancedArmReward);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.SimpleArmFailure", ref ROCScienceArm.Reputation.SimpleArmFailure, ROCScienceArm.Reputation.SimpleArmFailure);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.ComplexArmFailure", ref ROCScienceArm.Reputation.ComplexArmFailure, ROCScienceArm.Reputation.ComplexArmFailure);
			SystemUtilities.LoadNode(node66, "ContractDefs", "ROCScienceArm.Reputation.AdvancedArmFailure", ref ROCScienceArm.Reputation.AdvancedArmFailure, ROCScienceArm.Reputation.AdvancedArmFailure);
			SystemUtilities.LoadNode(configNode17, "ContractDefs", "ROCScienceRetrieval.MaximumExistent", ref ROCScienceRetrieval.MaximumExistent, ROCScienceRetrieval.MaximumExistent);
			SystemUtilities.LoadNode(node67, "ContractDefs", "ROCScienceRetrieval.Expire.MinimumExpireDays", ref ROCScienceRetrieval.Expire.MinimumExpireDays, ROCScienceRetrieval.Expire.MinimumExpireDays);
			SystemUtilities.LoadNode(node67, "ContractDefs", "ROCScienceRetrieval.Expire.MaximumExpireDays", ref ROCScienceRetrieval.Expire.MaximumExpireDays, ROCScienceRetrieval.Expire.MaximumExpireDays);
			SystemUtilities.LoadNode(node67, "ContractDefs", "ROCScienceRetrieval.Expire.DeadlineDays", ref ROCScienceRetrieval.Expire.DeadlineDays, ROCScienceRetrieval.Expire.DeadlineDays);
			SystemUtilities.LoadNode(node68, "ContractDefs", "ROCScienceRetrieval.Funds.Advance", ref ROCScienceRetrieval.Funds.Advance, ROCScienceRetrieval.Funds.Advance);
			SystemUtilities.LoadNode(node68, "ContractDefs", "ROCScienceRetrieval.Funds.Reward", ref ROCScienceRetrieval.Funds.Reward, ROCScienceRetrieval.Funds.Reward);
			SystemUtilities.LoadNode(node68, "ContractDefs", "ROCScienceRetrieval.Funds.Failure", ref ROCScienceRetrieval.Funds.Failure, ROCScienceRetrieval.Funds.Failure);
			SystemUtilities.LoadNode(node69, "ContractDefs", "ROCScienceRetrieval.Science.Reward", ref ROCScienceRetrieval.Science.Reward, ROCScienceRetrieval.Science.Reward);
			SystemUtilities.LoadNode(node70, "ContractDefs", "ROCScienceRetrieval.Reputation.Reward", ref ROCScienceRetrieval.Reputation.Reward, ROCScienceRetrieval.Reputation.Reward);
			SystemUtilities.LoadNode(node70, "ContractDefs", "ROCScienceRetrieval.Reputation.Failure", ref ROCScienceRetrieval.Reputation.Failure, ROCScienceRetrieval.Reputation.Failure);
		}
		SystemUtilities.LoadNode(configNode18, "ContractDefs", "CometSample.MaximumAvailable", ref CometSample.MaximumAvailable, CometSample.MaximumAvailable);
		SystemUtilities.LoadNode(configNode18, "ContractDefs", "CometSample.MaximumActive", ref CometSample.MaximumActive, CometSample.MaximumActive);
		SystemUtilities.LoadNode(node71, "ContractDefs", "CometSample.Expire.MinimumExpireDays", ref CometSample.Expire.MinimumExpireDays, CometSample.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node71, "ContractDefs", "CometSample.Expire.MaximumExpireDays", ref CometSample.Expire.MaximumExpireDays, CometSample.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node71, "ContractDefs", "CometSample.Expire.DeadlineDays", ref CometSample.Expire.DeadlineDays, CometSample.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node72, "ContractDefs", "CometSample.Funds.BaseAdvance", ref CometSample.Funds.BaseAdvance, CometSample.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node72, "ContractDefs", "CometSample.Funds.BaseReward", ref CometSample.Funds.BaseReward, CometSample.Funds.BaseReward);
		SystemUtilities.LoadNode(node72, "ContractDefs", "CometSample.Funds.BaseFailure", ref CometSample.Funds.BaseFailure, CometSample.Funds.BaseFailure);
		SystemUtilities.LoadNode(node73, "ContractDefs", "CometSample.Science.BaseReward", ref CometSample.Science.BaseReward, CometSample.Science.BaseReward);
		SystemUtilities.LoadNode(node74, "ContractDefs", "CometSample.Reputation.BaseReward", ref CometSample.Reputation.BaseReward, CometSample.Reputation.BaseReward);
		SystemUtilities.LoadNode(node74, "ContractDefs", "CometSample.Reputation.BaseFailure", ref CometSample.Reputation.BaseFailure, CometSample.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node75, "ContractDefs", "CometSample.Trivial.FundsMultiplier", ref CometSample.Trivial.FundsMultiplier, CometSample.Trivial.FundsMultiplier);
		SystemUtilities.LoadNode(node75, "ContractDefs", "CometSample.Trivial.ScienceMultiplier", ref CometSample.Trivial.ScienceMultiplier, CometSample.Trivial.ScienceMultiplier);
		SystemUtilities.LoadNode(node75, "ContractDefs", "CometSample.Trivial.ReputationMultiplier", ref CometSample.Trivial.ReputationMultiplier, CometSample.Trivial.ReputationMultiplier);
		SystemUtilities.LoadNode(node76, "ContractDefs", "CometSample.Significant.FundsMultiplier", ref CometSample.Significant.FundsMultiplier, CometSample.Significant.FundsMultiplier);
		SystemUtilities.LoadNode(node76, "ContractDefs", "CometSample.Significant.ScienceMultiplier", ref CometSample.Significant.ScienceMultiplier, CometSample.Significant.ScienceMultiplier);
		SystemUtilities.LoadNode(node76, "ContractDefs", "CometSample.Significant.ReputationMultiplier", ref CometSample.Significant.ReputationMultiplier, CometSample.Significant.ReputationMultiplier);
		SystemUtilities.LoadNode(node77, "ContractDefs", "CometSample.Exceptional.FundsMultiplier", ref CometSample.Exceptional.FundsMultiplier, CometSample.Exceptional.FundsMultiplier);
		SystemUtilities.LoadNode(node77, "ContractDefs", "CometSample.Exceptional.ScienceMultiplier", ref CometSample.Exceptional.ScienceMultiplier, CometSample.Exceptional.ScienceMultiplier);
		SystemUtilities.LoadNode(node77, "ContractDefs", "CometSample.Exceptional.ReputationMultiplier", ref CometSample.Exceptional.ReputationMultiplier, CometSample.Exceptional.ReputationMultiplier);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.MaximumAvailable", ref RoverConstruction.MaximumAvailable, RoverConstruction.MaximumAvailable);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.MaximumActive", ref RoverConstruction.MaximumActive, RoverConstruction.MaximumActive);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.WayPointMinDistance", ref RoverConstruction.WayPointMinDistance, RoverConstruction.WayPointMinDistance);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.WayPointMaxDistance", ref RoverConstruction.WayPointMaxDistance, RoverConstruction.WayPointMaxDistance);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.WayPointTriggerRange", ref RoverConstruction.WayPointTriggerRange, RoverConstruction.WayPointTriggerRange);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.MinimumGeeASL", ref RoverConstruction.MinimumGeeASL, RoverConstruction.MinimumGeeASL);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.MaximumGeeASL", ref RoverConstruction.MaximumGeeASL, RoverConstruction.MaximumGeeASL);
		SystemUtilities.LoadNode(configNode19, "ContractDefs", "RoverConstruction.TechNodeRequired", ref RoverConstruction.TechNodeRequired, RoverConstruction.TechNodeRequired);
		SystemUtilities.LoadNode(node78, "ContractDefs", "RoverConstruction.Expire.MinimumExpireDays", ref RoverConstruction.Expire.MinimumExpireDays, RoverConstruction.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node78, "ContractDefs", "RoverConstruction.Expire.MaximumExpireDays", ref RoverConstruction.Expire.MaximumExpireDays, RoverConstruction.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node78, "ContractDefs", "RoverConstruction.Expire.DeadlineDays", ref RoverConstruction.Expire.DeadlineDays, RoverConstruction.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node79, "ContractDefs", "RoverConstruction.Funds.BaseAdvance", ref RoverConstruction.Funds.BaseAdvance, RoverConstruction.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node79, "ContractDefs", "RoverConstruction.Funds.BaseReward", ref RoverConstruction.Funds.BaseReward, RoverConstruction.Funds.BaseReward);
		SystemUtilities.LoadNode(node79, "ContractDefs", "RoverConstruction.Funds.BaseFailure", ref RoverConstruction.Funds.BaseFailure, RoverConstruction.Funds.BaseFailure);
		SystemUtilities.LoadNode(node80, "ContractDefs", "RoverConstruction.Science.BaseReward", ref RoverConstruction.Science.BaseReward, RoverConstruction.Science.BaseReward);
		SystemUtilities.LoadNode(node81, "ContractDefs", "RoverConstruction.Reputation.BaseReward", ref RoverConstruction.Reputation.BaseReward, RoverConstruction.Reputation.BaseReward);
		SystemUtilities.LoadNode(node81, "ContractDefs", "RoverConstruction.Reputation.BaseFailure", ref RoverConstruction.Reputation.BaseFailure, RoverConstruction.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node82, "ContractDefs", "RoverConstruction.Trivial.FundsMultiplier", ref RoverConstruction.Trivial.FundsMultiplier, RoverConstruction.Trivial.FundsMultiplier);
		SystemUtilities.LoadNode(node82, "ContractDefs", "RoverConstruction.Trivial.ScienceMultiplier", ref RoverConstruction.Trivial.ScienceMultiplier, RoverConstruction.Trivial.ScienceMultiplier);
		SystemUtilities.LoadNode(node82, "ContractDefs", "RoverConstruction.Trivial.ReputationMultiplier", ref RoverConstruction.Trivial.ReputationMultiplier, RoverConstruction.Trivial.ReputationMultiplier);
		SystemUtilities.LoadNode(node82, "ContractDefs", "RoverConstruction.Trivial.WayPointMultiplier", ref RoverConstruction.Trivial.WayPointMultiplier, RoverConstruction.Trivial.WayPointMultiplier);
		SystemUtilities.LoadNode(node83, "ContractDefs", "RoverConstruction.Significant.FundsMultiplier", ref RoverConstruction.Significant.FundsMultiplier, RoverConstruction.Significant.FundsMultiplier);
		SystemUtilities.LoadNode(node83, "ContractDefs", "RoverConstruction.Significant.ScienceMultiplier", ref RoverConstruction.Significant.ScienceMultiplier, RoverConstruction.Significant.ScienceMultiplier);
		SystemUtilities.LoadNode(node83, "ContractDefs", "RoverConstruction.Significant.ReputationMultiplier", ref RoverConstruction.Significant.ReputationMultiplier, RoverConstruction.Significant.ReputationMultiplier);
		SystemUtilities.LoadNode(node83, "ContractDefs", "RoverConstruction.Significant.WayPointMultiplier", ref RoverConstruction.Significant.WayPointMultiplier, RoverConstruction.Significant.WayPointMultiplier);
		SystemUtilities.LoadNode(node84, "ContractDefs", "RoverConstruction.Exceptional.FundsMultiplier", ref RoverConstruction.Exceptional.FundsMultiplier, RoverConstruction.Exceptional.FundsMultiplier);
		SystemUtilities.LoadNode(node84, "ContractDefs", "RoverConstruction.Exceptional.ScienceMultiplier", ref RoverConstruction.Exceptional.ScienceMultiplier, RoverConstruction.Exceptional.ScienceMultiplier);
		SystemUtilities.LoadNode(node84, "ContractDefs", "RoverConstruction.Exceptional.ReputationMultiplier", ref RoverConstruction.Exceptional.ReputationMultiplier, RoverConstruction.Exceptional.ReputationMultiplier);
		SystemUtilities.LoadNode(node84, "ContractDefs", "RoverConstruction.Exceptional.WayPointMultiplier", ref RoverConstruction.Exceptional.WayPointMultiplier, RoverConstruction.Exceptional.WayPointMultiplier);
		SystemUtilities.LoadNode(configNode20, "ContractDefs", "VesselRepair.MaximumAvailable", ref VesselRepair.MaximumAvailable, VesselRepair.MaximumAvailable);
		SystemUtilities.LoadNode(configNode20, "ContractDefs", "VesselRepair.MaximumActive", ref VesselRepair.MaximumActive, VesselRepair.MaximumActive);
		SystemUtilities.LoadNode(node85, "ContractDefs", "VesselRepair.Expire.MinimumExpireDays", ref VesselRepair.Expire.MinimumExpireDays, VesselRepair.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node85, "ContractDefs", "VesselRepair.Expire.MaximumExpireDays", ref VesselRepair.Expire.MaximumExpireDays, VesselRepair.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node85, "ContractDefs", "VesselRepair.Expire.DeadlineDays", ref VesselRepair.Expire.DeadlineDays, VesselRepair.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node86, "ContractDefs", "VesselRepair.Funds.BaseAdvance", ref VesselRepair.Funds.BaseAdvance, VesselRepair.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node86, "ContractDefs", "VesselRepair.Funds.BaseReward", ref VesselRepair.Funds.BaseReward, VesselRepair.Funds.BaseReward);
		SystemUtilities.LoadNode(node86, "ContractDefs", "VesselRepair.Funds.BaseFailure", ref VesselRepair.Funds.BaseFailure, VesselRepair.Funds.BaseFailure);
		SystemUtilities.LoadNode(node87, "ContractDefs", "VesselRepair.Science.BaseReward", ref VesselRepair.Science.BaseReward, VesselRepair.Science.BaseReward);
		SystemUtilities.LoadNode(node88, "ContractDefs", "VesselRepair.Reputation.BaseReward", ref VesselRepair.Reputation.BaseReward, VesselRepair.Reputation.BaseReward);
		SystemUtilities.LoadNode(node88, "ContractDefs", "VesselRepair.Reputation.BaseFailure", ref VesselRepair.Reputation.BaseFailure, VesselRepair.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.FundsMultiplier", ref VesselRepair.Trivial.FundsMultiplier, VesselRepair.Trivial.FundsMultiplier);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.ScienceMultiplier", ref VesselRepair.Trivial.ScienceMultiplier, VesselRepair.Trivial.ScienceMultiplier);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.ReputationMultiplier", ref VesselRepair.Trivial.ReputationMultiplier, VesselRepair.Trivial.ReputationMultiplier);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.MaxOrbitEccentricityFactor", ref VesselRepair.Trivial.MaxOrbitEccentricityFactor, VesselRepair.Trivial.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.OrbitAltitudeFactor", ref VesselRepair.Trivial.OrbitAltitudeFactor, VesselRepair.Trivial.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node89, "ContractDefs", "VesselRepair.Trivial.OrbitInclinationFactor", ref VesselRepair.Trivial.OrbitInclinationFactor, VesselRepair.Trivial.OrbitInclinationFactor);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.FundsMultiplier", ref VesselRepair.Significant.FundsMultiplier, VesselRepair.Significant.FundsMultiplier);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.ScienceMultiplier", ref VesselRepair.Significant.ScienceMultiplier, VesselRepair.Significant.ScienceMultiplier);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.ReputationMultiplier", ref VesselRepair.Significant.ReputationMultiplier, VesselRepair.Significant.ReputationMultiplier);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.MaxOrbitEccentricityFactor", ref VesselRepair.Significant.MaxOrbitEccentricityFactor, VesselRepair.Significant.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.OrbitAltitudeFactor", ref VesselRepair.Significant.OrbitAltitudeFactor, VesselRepair.Significant.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node90, "ContractDefs", "VesselRepair.Significant.OrbitInclinationFactor", ref VesselRepair.Significant.OrbitInclinationFactor, VesselRepair.Significant.OrbitInclinationFactor);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.FundsMultiplier", ref VesselRepair.Exceptional.FundsMultiplier, VesselRepair.Exceptional.FundsMultiplier);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.ScienceMultiplier", ref VesselRepair.Exceptional.ScienceMultiplier, VesselRepair.Exceptional.ScienceMultiplier);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.ReputationMultiplier", ref VesselRepair.Exceptional.ReputationMultiplier, VesselRepair.Exceptional.ReputationMultiplier);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.MaxOrbitEccentricityFactor", ref VesselRepair.Exceptional.MaxOrbitEccentricityFactor, VesselRepair.Exceptional.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.OrbitAltitudeFactor", ref VesselRepair.Exceptional.OrbitAltitudeFactor, VesselRepair.Exceptional.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node91, "ContractDefs", "VesselRepair.Exceptional.OrbitInclinationFactor", ref VesselRepair.Exceptional.OrbitInclinationFactor, VesselRepair.Exceptional.OrbitInclinationFactor);
		SystemUtilities.LoadNode(configNode21, "ContractDefs", "OrbitalConstruction.MaximumAvailable", ref OrbitalConstruction.MaximumAvailable, OrbitalConstruction.MaximumAvailable);
		SystemUtilities.LoadNode(configNode21, "ContractDefs", "OrbitalConstruction.MaximumActive", ref OrbitalConstruction.MaximumActive, OrbitalConstruction.MaximumActive);
		SystemUtilities.LoadNode(configNode21, "ContractDefs", "OrbitalConstruction.TechNodeRequired", ref OrbitalConstruction.TechNodeRequired, OrbitalConstruction.TechNodeRequired);
		SystemUtilities.LoadNode(node92, "ContractDefs", "OrbitalConstruction.Expire.MinimumExpireDays", ref OrbitalConstruction.Expire.MinimumExpireDays, OrbitalConstruction.Expire.MinimumExpireDays);
		SystemUtilities.LoadNode(node92, "ContractDefs", "OrbitalConstruction.Expire.MaximumExpireDays", ref OrbitalConstruction.Expire.MaximumExpireDays, OrbitalConstruction.Expire.MaximumExpireDays);
		SystemUtilities.LoadNode(node92, "ContractDefs", "OrbitalConstruction.Expire.DeadlineDays", ref OrbitalConstruction.Expire.DeadlineDays, OrbitalConstruction.Expire.DeadlineDays);
		SystemUtilities.LoadNode(node93, "ContractDefs", "OrbitalConstruction.Funds.BaseAdvance", ref OrbitalConstruction.Funds.BaseAdvance, OrbitalConstruction.Funds.BaseAdvance);
		SystemUtilities.LoadNode(node93, "ContractDefs", "OrbitalConstruction.Funds.BaseReward", ref OrbitalConstruction.Funds.BaseReward, OrbitalConstruction.Funds.BaseReward);
		SystemUtilities.LoadNode(node93, "ContractDefs", "OrbitalConstruction.Funds.BaseFailure", ref OrbitalConstruction.Funds.BaseFailure, OrbitalConstruction.Funds.BaseFailure);
		SystemUtilities.LoadNode(node94, "ContractDefs", "OrbitalConstruction.Science.BaseReward", ref OrbitalConstruction.Science.BaseReward, OrbitalConstruction.Science.BaseReward);
		SystemUtilities.LoadNode(node95, "ContractDefs", "OrbitalConstruction.Reputation.BaseReward", ref OrbitalConstruction.Reputation.BaseReward, OrbitalConstruction.Reputation.BaseReward);
		SystemUtilities.LoadNode(node95, "ContractDefs", "OrbitalConstruction.Reputation.BaseFailure", ref OrbitalConstruction.Reputation.BaseFailure, OrbitalConstruction.Reputation.BaseFailure);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.FundsMultiplier", ref OrbitalConstruction.Trivial.FundsMultiplier, OrbitalConstruction.Trivial.FundsMultiplier);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.ScienceMultiplier", ref OrbitalConstruction.Trivial.ScienceMultiplier, OrbitalConstruction.Trivial.ScienceMultiplier);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.ReputationMultiplier", ref OrbitalConstruction.Trivial.ReputationMultiplier, OrbitalConstruction.Trivial.ReputationMultiplier);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.MaxOrbitEccentricityFactor", ref OrbitalConstruction.Trivial.MaxOrbitEccentricityFactor, OrbitalConstruction.Trivial.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.OrbitAltitudeFactor", ref OrbitalConstruction.Trivial.OrbitAltitudeFactor, OrbitalConstruction.Trivial.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node96, "ContractDefs", "OrbitalConstruction.Trivial.OrbitInclinationFactor", ref OrbitalConstruction.Trivial.OrbitInclinationFactor, OrbitalConstruction.Trivial.OrbitInclinationFactor);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.FundsMultiplier", ref OrbitalConstruction.Significant.FundsMultiplier, OrbitalConstruction.Significant.FundsMultiplier);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.ScienceMultiplier", ref OrbitalConstruction.Significant.ScienceMultiplier, OrbitalConstruction.Significant.ScienceMultiplier);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.ReputationMultiplier", ref OrbitalConstruction.Significant.ReputationMultiplier, OrbitalConstruction.Significant.ReputationMultiplier);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.MaxOrbitEccentricityFactor", ref OrbitalConstruction.Significant.MaxOrbitEccentricityFactor, OrbitalConstruction.Significant.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.OrbitAltitudeFactor", ref OrbitalConstruction.Significant.OrbitAltitudeFactor, OrbitalConstruction.Significant.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node97, "ContractDefs", "OrbitalConstruction.Significant.OrbitInclinationFactor", ref OrbitalConstruction.Significant.OrbitInclinationFactor, OrbitalConstruction.Significant.OrbitInclinationFactor);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.FundsMultiplier", ref OrbitalConstruction.Exceptional.FundsMultiplier, OrbitalConstruction.Exceptional.FundsMultiplier);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.ScienceMultiplier", ref OrbitalConstruction.Exceptional.ScienceMultiplier, OrbitalConstruction.Exceptional.ScienceMultiplier);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.ReputationMultiplier", ref OrbitalConstruction.Exceptional.ReputationMultiplier, OrbitalConstruction.Exceptional.ReputationMultiplier);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.MaxOrbitEccentricityFactor", ref OrbitalConstruction.Exceptional.MaxOrbitEccentricityFactor, OrbitalConstruction.Exceptional.MaxOrbitEccentricityFactor);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.OrbitAltitudeFactor", ref OrbitalConstruction.Exceptional.OrbitAltitudeFactor, OrbitalConstruction.Exceptional.OrbitAltitudeFactor);
		SystemUtilities.LoadNode(node98, "ContractDefs", "OrbitalConstruction.Exceptional.OrbitInclinationFactor", ref OrbitalConstruction.Exceptional.OrbitInclinationFactor, OrbitalConstruction.Exceptional.OrbitInclinationFactor);
	}

	public static void AddSurveyDefinitions(ConfigNode surveyNode)
	{
		ConfigNode configNode = surveyNode.AddNode(new ConfigNode("SURVEY_DEFINITION", "Visual Survey Definition"));
		configNode.AddValue("DataName", "observational", "Plain text adjective describing survey data");
		configNode.AddValue("AnomalyName", "inconsistencies", "Plain text description of survey anomalies");
		configNode.AddValue("ResultName", "reports", "Plain text noun describing survey results");
		configNode.AddValue("FundsReward", 53000, "The base funds completion reward before difficulty modifiers");
		configNode.AddValue("FundsFailure", 0, "The base funds failure penalty before difficulty modifiers");
		configNode.AddValue("ScienceReward", 0, "The base science completion reward before difficulty modifiers");
		configNode.AddValue("ReputationReward", 9, "The base reputation completion reward before difficulty modifiers");
		configNode.AddValue("ReputationFailure", 9, "The base reputation failure penalty before difficulty modifiers");
		ConfigNode configNode2 = configNode.AddNode(new ConfigNode("PARAM", "EVA Report Survey Objective"));
		configNode2.AddValue("Experiment", "evaReport", "The experiment ID");
		configNode2.AddValue("Description", "Take an EVA report", "Plain text description of the action involved in the objective");
		configNode2.AddValue("Texture", "eva", "The waypoint texture");
		configNode2.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode2.AddValue("AllowLow", value: false, "Whether survey can spawn at low altitudes");
		configNode2.AddValue("AllowHigh", value: false, "Whether survey can spawn at high altitudes including orbit");
		configNode2.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode2.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode2.AddValue("EVARequired", value: true, "Whether survey requires an EVA");
		configNode2.AddValue("FundsMultiplier", 0.9, "The base funds reward per waypoint");
		configNode2.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode2.AddValue("ReputationMultiplier", 1.1, "The base reputation reward per waypoint");
		ConfigNode configNode3 = configNode.AddNode(new ConfigNode("PARAM", "Crew Report Survey Objective"));
		configNode3.AddValue("Experiment", "crewReport", "The experiment ID");
		configNode3.AddValue("Description", "Take a crew report", "Plain text description of the action involved in the objective");
		configNode3.AddValue("Texture", "report", "The waypoint texture");
		configNode3.AddValue("AllowGround", value: false, "Whether survey can spawn on the ground");
		configNode3.AddValue("AllowLow", value: true, "Whether survey can spawn at low altitudes");
		configNode3.AddValue("AllowHigh", value: true, "Whether survey can spawn at high altitudes including orbit");
		configNode3.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode3.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode3.AddValue("CrewRequired", value: true, "Whether survey requires crew presence");
		configNode3.AddValue("FundsMultiplier", 1.1, "The base funds reward per waypoint");
		configNode3.AddValue("ScienceMultiplier", 0.9, "The base science reward per waypoint");
		configNode3.AddValue("ReputationMultiplier", 1, "The base reputation reward per waypoint");
		ConfigNode configNode4 = configNode.AddNode(new ConfigNode("PARAM", "Surface Sample Survey Objective"));
		configNode4.AddValue("Experiment", "surfaceSample", "The experiment ID");
		configNode4.AddValue("Description", "Take a surface sample", "Plain text description of the action involved in the objective");
		configNode4.AddValue("Texture", "sample", "The waypoint texture");
		configNode4.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode4.AddValue("AllowLow", value: false, "Whether survey can spawn at low altitudes");
		configNode4.AddValue("AllowHigh", value: false, "Whether survey can spawn at high altitudes including orbit");
		configNode4.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode4.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode4.AddValue("EVARequired", value: true, "Whether survey requires an EVA");
		configNode4.AddValue("FundsMultiplier", 1, "The base funds reward per waypoint");
		configNode4.AddValue("ScienceMultiplier", 1.1, "The base science reward per waypoint");
		configNode4.AddValue("ReputationMultiplier", 0.9, "The base reputation reward per waypoint");
		ConfigNode configNode5 = surveyNode.AddNode(new ConfigNode("SURVEY_DEFINITION", "Seismic Survey Definition"));
		configNode5.AddValue("DataName", "seismic", "Plain text adjective describing survey data");
		configNode5.AddValue("AnomalyName", "disturbances", "Plain text description of survey anomalies");
		configNode5.AddValue("ResultName", "readings", "Plain text noun describing survey results");
		configNode5.AddValue("FundsReward", 53000, "The base funds completion reward before difficulty modifiers");
		configNode5.AddValue("FundsFailure", 0, "The base funds failure penalty before difficulty modifiers");
		configNode5.AddValue("ScienceReward", 0, "The base science completion reward before difficulty modifiers");
		configNode5.AddValue("ReputationReward", 9, "The base reputation completion reward before difficulty modifiers");
		configNode5.AddValue("ReputationFailure", 9, "The base reputation failure penalty before difficulty modifiers");
		ConfigNode configNode6 = configNode5.AddNode(new ConfigNode("PARAM", "Seismometer Survey Objective"));
		configNode6.AddValue("Experiment", "seismicScan", "The experiment ID");
		configNode6.AddValue("Description", "Take seismic readings", "Plain text description of the action involved in the objective");
		configNode6.AddValue("Texture", "seismic", "The waypoint texture");
		configNode6.AddValue("Tech", "sensorAccelerometer", "Technology research required to appear");
		configNode6.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode6.AddValue("AllowLow", value: false, "Whether survey can spawn at low altitudes");
		configNode6.AddValue("AllowHigh", value: false, "Whether survey can spawn at high altitudes including orbit");
		configNode6.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode6.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode6.AddValue("FundsMultiplier", 1.1, "The base funds reward per waypoint");
		configNode6.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode6.AddValue("ReputationMultiplier", 0.9, "The base reputation reward per waypoint");
		ConfigNode configNode7 = surveyNode.AddNode(new ConfigNode("SURVEY_DEFINITION", "Temperature Survey Definition"));
		configNode7.AddValue("DataName", "temperature", "Plain text adjective describing survey data");
		configNode7.AddValue("AnomalyName", "fluctuations", "Plain text description of survey anomalies");
		configNode7.AddValue("ResultName", "measurements", "Plain text noun describing survey results");
		configNode7.AddValue("FundsReward", 53000, "The base funds completion reward before difficulty modifiers");
		configNode7.AddValue("FundsFailure", 0, "The base funds failure penalty before difficulty modifiers");
		configNode7.AddValue("ScienceReward", 0, "The base science completion reward before difficulty modifiers");
		configNode7.AddValue("ReputationReward", 9, "The base reputation completion reward before difficulty modifiers");
		configNode7.AddValue("ReputationFailure", 9, "The base reputation failure penalty before difficulty modifiers");
		ConfigNode configNode8 = configNode7.AddNode(new ConfigNode("PARAM", "Thermometer Survey Objective"));
		configNode8.AddValue("Experiment", "temperatureScan", "The experiment ID");
		configNode8.AddValue("Description", "Measure the temperature", "Plain text description of the action involved in the objective");
		configNode8.AddValue("Texture", "thermometer", "The waypoint texture");
		configNode8.AddValue("Tech", "sensorThermometer", "Technology research required to appear");
		configNode8.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode8.AddValue("AllowLow", value: true, "Whether survey can spawn at low altitudes");
		configNode8.AddValue("AllowHigh", value: true, "Whether survey can spawn at high altitudes including orbit");
		configNode8.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode8.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode8.AddValue("FundsMultiplier", 1.1, "The base funds reward per waypoint");
		configNode8.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode8.AddValue("ReputationMultiplier", 0.9, "The base reputation reward per waypoint");
		ConfigNode configNode9 = surveyNode.AddNode(new ConfigNode("SURVEY_DEFINITION", "Atmospheric Survey Definition"));
		configNode9.AddValue("DataName", "atmospheric", "Plain text adjective describing survey data");
		configNode9.AddValue("AnomalyName", "abnormalities", "Plain text description of survey anomalies");
		configNode9.AddValue("ResultName", "readings", "Plain text noun describing survey results");
		configNode9.AddValue("FundsReward", 53000, "The base funds completion reward before difficulty modifiers");
		configNode9.AddValue("FundsFailure", 0, "The base funds failure penalty before difficulty modifiers");
		configNode9.AddValue("ScienceReward", 0, "The base science completion reward before difficulty modifiers");
		configNode9.AddValue("ReputationReward", 9, "The base reputation completion reward before difficulty modifiers");
		configNode9.AddValue("ReputationFailure", 9, "The base reputation failure penalty before difficulty modifiers");
		ConfigNode configNode10 = configNode9.AddNode(new ConfigNode("PARAM", "Barometer Survey Objective"));
		configNode10.AddValue("Experiment", "barometerScan", "The experiment ID");
		configNode10.AddValue("Description", "Take pressure readings", "Plain text description of the action involved in the objective");
		configNode10.AddValue("Texture", "pressure", "The waypoint texture");
		configNode10.AddValue("Tech", "sensorBarometer", "Technology research required to appear");
		configNode10.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode10.AddValue("AllowLow", value: true, "Whether survey can spawn at low altitudes");
		configNode10.AddValue("AllowHigh", value: true, "Whether survey can spawn at high altitudes including orbit");
		configNode10.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode10.AddValue("AllowVacuum", value: false, "Whether survey can spawn in vacuum");
		configNode10.AddValue("FundsMultiplier", 1.1, "The base funds reward per waypoint");
		configNode10.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode10.AddValue("ReputationMultiplier", 0.9, "The base reputation reward per waypoint");
		ConfigNode configNode11 = configNode9.AddNode(new ConfigNode("PARAM", "Atmospheric Analysis Survey Objective"));
		configNode11.AddValue("Experiment", "atmosphereAnalysis", "The experiment ID");
		configNode11.AddValue("Description", "Perform atmospheric analysis", "Plain text description of the action involved in the objective");
		configNode11.AddValue("Texture", "balloon", "The waypoint texture");
		configNode11.AddValue("Tech", "sensorAtmosphere", "Technology research required to appear");
		configNode11.AddValue("AllowGround", value: false, "Whether survey can spawn on the ground");
		configNode11.AddValue("AllowLow", value: true, "Whether survey can spawn at low altitudes");
		configNode11.AddValue("AllowHigh", value: true, "Whether survey can spawn at high altitudes including orbit");
		configNode11.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode11.AddValue("AllowVacuum", value: false, "Whether survey can spawn in vacuum");
		configNode11.AddValue("FundsMultiplier", 0.9, "The base funds reward per waypoint");
		configNode11.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode11.AddValue("ReputationMultiplier", 1.1, "The base reputation reward per waypoint");
		ConfigNode configNode12 = surveyNode.AddNode(new ConfigNode("SURVEY_DEFINITION", "Gravimetric Survey Definition"));
		configNode12.AddValue("DataName", "gravimetric", "Plain text adjective describing survey data");
		configNode12.AddValue("AnomalyName", "anomalies", "Plain text description of survey anomalies");
		configNode12.AddValue("ResultName", "readings", "Plain text noun describing survey results");
		configNode12.AddValue("FundsReward", 53000, "The base funds completion reward before difficulty modifiers");
		configNode12.AddValue("FundsFailure", 0, "The base funds failure penalty before difficulty modifiers");
		configNode12.AddValue("ScienceReward", 0, "The base science completion reward before difficulty modifiers");
		configNode12.AddValue("ReputationReward", 9, "The base reputation completion reward before difficulty modifiers");
		configNode12.AddValue("ReputationFailure", 9, "The base reputation failure penalty before difficulty modifiers");
		ConfigNode configNode13 = configNode12.AddNode(new ConfigNode("PARAM", "Gravioli Detector Survey Objective"));
		configNode13.AddValue("Experiment", "gravityScan", "The experiment ID");
		configNode13.AddValue("Description", "Record gravitational forces", "Plain text description of the action involved in the objective");
		configNode13.AddValue("Texture", "gravity", "The waypoint texture");
		configNode13.AddValue("Tech", "sensorGravimeter", "Technology research required to appear");
		configNode13.AddValue("AllowGround", value: true, "Whether survey can spawn on the ground");
		configNode13.AddValue("AllowLow", value: false, "Whether survey can spawn at low altitudes");
		configNode13.AddValue("AllowHigh", value: true, "Whether survey can spawn at high altitudes including orbit");
		configNode13.AddValue("AllowWater", value: false, "Whether survey can spawn in the ocean");
		configNode13.AddValue("AllowVacuum", value: true, "Whether survey can spawn in vacuum");
		configNode13.AddValue("FundsMultiplier", 0.9, "The base funds reward per waypoint");
		configNode13.AddValue("ScienceMultiplier", 1, "The base science reward per waypoint");
		configNode13.AddValue("ReputationMultiplier", 1.1, "The base reputation reward per waypoint");
	}

	public static void AddPartRequests(ConfigNode topNode)
	{
		if (topNode != null)
		{
			ConfigNode node = topNode.GetNode("Base");
			ConfigNode node2 = topNode.GetNode("Station");
			ConfigNode node3 = topNode.GetNode("Satellite");
			if (node != null)
			{
				ConfigNode configNode = node.AddNode("PART_REQUEST", "A potential science lab request for outposts");
				ConfigNode configNode2 = configNode.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode3 = configNode.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode4 = configNode.AddNode("Exceptional", "Hard Contracts");
				configNode.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode.AddValue("PartDescription", "research lab", "Plain speech description of part for request");
				configNode.AddValue("VesselDescription", "outpost", "Plain speech description of vessel for request");
				configNode.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode.AddValue("Part", "Large_Crewed_Lab", "A part ID that triggers this request");
				configNode.AddValue("Module", "ModuleScienceLab", "A part module ID that triggers this request");
				configNode.AddValue("MinimumScience", 15, "A minimum value this request places on science rewards");
				configNode2.AddValue("Weight", 20, "How common this request is in easy contracts");
				configNode2.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in easy contracts");
				configNode2.AddValue("ScienceMultiplier", 1.5, "The multiplier this request places on science in easy contracts");
				configNode2.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in easy contracts");
				configNode3.AddValue("Weight", 30, "How common this request is in medium contracts");
				configNode3.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in medium contracts");
				configNode3.AddValue("ScienceMultiplier", 1.5, "The multiplier this request places on science in medium contracts");
				configNode3.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in medium contracts");
				configNode4.AddValue("Weight", 40, "How common this request is in hard contracts");
				configNode4.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in hard contracts");
				configNode4.AddValue("ScienceMultiplier", 1.5, "The multiplier this request places on science in hard contracts");
				configNode4.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode5 = node.AddNode("PART_REQUEST", "A potential cupola request for outposts");
				ConfigNode configNode6 = configNode5.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode7 = configNode5.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode8 = configNode5.AddNode("Exceptional", "Hard Contracts");
				configNode5.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode5.AddValue("PartDescription", "viewing cupola", "Plain speech description of part for request");
				configNode5.AddValue("VesselDescription", "outpost", "Plain speech description of vessel for request");
				configNode5.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode5.AddValue("Part", "cupola", "A part ID that triggers this request");
				configNode6.AddValue("Weight", 20, "How common this request is in easy contracts");
				configNode6.AddValue("FundsMultiplier", 1.25, "The multiplier this request places on funds in easy contracts");
				configNode6.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in easy contracts");
				configNode6.AddValue("ReputationMultiplier", 1.25, "The multiplier this request places on reputation in easy contracts");
				configNode7.AddValue("Weight", 30, "How common this request is in medium contracts");
				configNode7.AddValue("FundsMultiplier", 1.25, "The multiplier this request places on funds in medium contracts");
				configNode7.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in medium contracts");
				configNode7.AddValue("ReputationMultiplier", 1.25, "The multiplier this request places on reputation in medium contracts");
				configNode8.AddValue("Weight", 40, "How common this request is in hard contracts");
				configNode8.AddValue("FundsMultiplier", 1.25, "The multiplier this request places on funds in hard contracts");
				configNode8.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in hard contracts");
				configNode8.AddValue("ReputationMultiplier", 1.25, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode9 = node.AddNode("PART_REQUEST", "A potential ISRU module request for outposts");
				ConfigNode configNode10 = configNode9.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode11 = configNode9.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode12 = configNode9.AddNode("Exceptional", "Hard Contracts");
				configNode9.AddValue("Article", "an", "Article (A/An) to be used prior to part description");
				configNode9.AddValue("PartDescription", "ISRU resource conversion unit", "Plain speech description of part for request");
				configNode9.AddValue("VesselDescription", "outpost", "Plain speech description of vessel for request");
				configNode9.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode9.AddValue("Part", "ISRU", "A part ID that triggers this request");
				configNode9.AddValue("Part", "MiniISRU", "A part ID that triggers this request");
				configNode10.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode10.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in easy contracts");
				configNode10.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
				configNode10.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode11.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode11.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in medium contracts");
				configNode11.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in medium contracts");
				configNode11.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode12.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode12.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in hard contracts");
				configNode12.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in hard contracts");
				configNode12.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
			}
			if (node2 != null)
			{
				ConfigNode configNode13 = node2.AddNode("PART_REQUEST", "A potential science lab request for stations");
				ConfigNode configNode14 = configNode13.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode15 = configNode13.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode16 = configNode13.AddNode("Exceptional", "Hard Contracts");
				configNode13.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode13.AddValue("PartDescription", "research lab", "Plain speech description of part for request");
				configNode13.AddValue("VesselDescription", "station", "Plain speech description of vessel for request");
				configNode13.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode13.AddValue("Part", "Large_Crewed_Lab", "A part ID that triggers this request");
				configNode13.AddValue("Module", "ModuleScienceLab", "A part module ID that triggers this request");
				configNode13.AddValue("MinimumScience", 13, "A minimum value this request places on science rewards");
				configNode14.AddValue("Weight", 20, "How common this request is in easy contracts");
				configNode14.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in easy contracts");
				configNode14.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in easy contracts");
				configNode14.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in easy contracts");
				configNode15.AddValue("Weight", 30, "How common this request is in medium contracts");
				configNode15.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in medium contracts");
				configNode15.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in medium contracts");
				configNode15.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in medium contracts");
				configNode16.AddValue("Weight", 40, "How common this request is in hard contracts");
				configNode16.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in hard contracts");
				configNode16.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in hard contracts");
				configNode16.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode17 = node2.AddNode("PART_REQUEST", "A potential cupola request for stations");
				ConfigNode configNode18 = configNode17.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode19 = configNode17.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode20 = configNode17.AddNode("Exceptional", "Hard Contracts");
				configNode17.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode17.AddValue("PartDescription", "viewing cupola", "Plain speech description of part for request");
				configNode17.AddValue("VesselDescription", "station", "Plain speech description of vessel for request");
				configNode17.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode17.AddValue("Part", "cupola", "A part ID that triggers this request");
				configNode18.AddValue("Weight", 20, "How common this request is in easy contracts");
				configNode18.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in easy contracts");
				configNode18.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in easy contracts");
				configNode18.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in easy contracts");
				configNode19.AddValue("Weight", 30, "How common this request is in medium contracts");
				configNode19.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in medium contracts");
				configNode19.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in medium contracts");
				configNode19.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in medium contracts");
				configNode20.AddValue("Weight", 40, "How common this request is in hard contracts");
				configNode20.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
				configNode20.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in hard contracts");
				configNode20.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode21 = node2.AddNode("PART_REQUEST", "A potential ISRU module request for stations");
				ConfigNode configNode22 = configNode21.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode23 = configNode21.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode24 = configNode21.AddNode("Exceptional", "Hard Contracts");
				configNode21.AddValue("Article", "an", "Article (A/An) to be used prior to part description");
				configNode21.AddValue("PartDescription", "ISRU resource conversion unit", "Plain speech description of part for request");
				configNode21.AddValue("VesselDescription", "station", "Plain speech description of vessel for request");
				configNode21.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode21.AddValue("Part", "ISRU", "A part ID that triggers this request");
				configNode21.AddValue("Part", "MiniISRU", "A part ID that triggers this request");
				configNode22.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode22.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in easy contracts");
				configNode22.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
				configNode22.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode23.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode23.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in medium contracts");
				configNode23.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in medium contracts");
				configNode23.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode24.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode24.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in hard contracts");
				configNode24.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in hard contracts");
				configNode24.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
			}
			if (node3 != null)
			{
				ConfigNode configNode25 = node3.AddNode("PART_REQUEST", "A potential mystery goo request for satellites");
				ConfigNode configNode26 = configNode25.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode27 = configNode25.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode28 = configNode25.AddNode("Exceptional", "Hard Contracts");
				configNode25.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode25.AddValue("PartDescription", "mystery goo unit", "Plain speech description of part for request");
				configNode25.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode25.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode25.AddValue("Part", "GooExperiment", "A part ID that triggers this request");
				configNode25.AddValue("MinimumScience", 7, "A minimum value this request places on science rewards");
				configNode26.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode26.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
				configNode26.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in easy contracts");
				configNode26.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode27.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode27.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in medium contracts");
				configNode27.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in medium contracts");
				configNode27.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode28.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode28.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in hard contracts");
				configNode28.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in hard contracts");
				configNode28.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode29 = node3.AddNode("PART_REQUEST", "A potential thermometer request for satellites");
				ConfigNode configNode30 = configNode29.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode31 = configNode29.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode32 = configNode29.AddNode("Exceptional", "Hard Contracts");
				configNode29.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode29.AddValue("PartDescription", "thermometer", "Plain speech description of part for request");
				configNode29.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode29.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode29.AddValue("Part", "sensorThermometer", "A part ID that triggers this request");
				configNode29.AddValue("MinimumScience", 7, "A minimum value this request places on science rewards");
				configNode30.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode30.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
				configNode30.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in easy contracts");
				configNode30.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode31.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode31.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in medium contracts");
				configNode31.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in medium contracts");
				configNode31.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode32.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode32.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in hard contracts");
				configNode32.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in hard contracts");
				configNode32.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode33 = node3.AddNode("PART_REQUEST", "A potential gravimetric sensor request for satellites");
				ConfigNode configNode34 = configNode33.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode35 = configNode33.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode36 = configNode33.AddNode("Exceptional", "Hard Contracts");
				configNode33.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode33.AddValue("PartDescription", "gravioli detector", "Plain speech description of part for request");
				configNode33.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode33.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode33.AddValue("Part", "sensorGravimeter", "A part ID that triggers this request");
				configNode33.AddValue("MinimumScience", 7, "A minimum value this request places on science rewards");
				configNode34.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode34.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
				configNode34.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in easy contracts");
				configNode34.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode35.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode35.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in medium contracts");
				configNode35.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in medium contracts");
				configNode35.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode36.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode36.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in hard contracts");
				configNode36.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in hard contracts");
				configNode36.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode37 = node3.AddNode("PART_REQUEST", "A potential materials bay request for satellites");
				ConfigNode configNode38 = configNode37.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode39 = configNode37.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode40 = configNode37.AddNode("Exceptional", "Hard Contracts");
				configNode37.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode37.AddValue("PartDescription", "materials bay", "Plain speech description of part for request");
				configNode37.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode37.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode37.AddValue("Part", "science_module", "A part ID that triggers this request");
				configNode37.AddValue("MinimumScience", 7, "A minimum value this request places on science rewards");
				configNode38.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode38.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
				configNode38.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in easy contracts");
				configNode38.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode39.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode39.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in medium contracts");
				configNode39.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in medium contracts");
				configNode39.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode40.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode40.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in hard contracts");
				configNode40.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in hard contracts");
				configNode40.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode41 = node3.AddNode("PART_REQUEST", "A potential accelerometer request for satellites");
				ConfigNode configNode42 = configNode41.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode43 = configNode41.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode44 = configNode41.AddNode("Exceptional", "Hard Contracts");
				configNode41.AddValue("Article", "an", "Article (A/An) to be used prior to part description");
				configNode41.AddValue("PartDescription", "accelerometer", "Plain speech description of part for request");
				configNode41.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode41.AddValue("Keyword", "Scientific", "Contract briefing keyword");
				configNode41.AddValue("Part", "sensorAccelerometer", "A part ID that triggers this request");
				configNode41.AddValue("MinimumScience", 7, "A minimum value this request places on science rewards");
				configNode42.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode42.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
				configNode42.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in easy contracts");
				configNode42.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode43.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode43.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in medium contracts");
				configNode43.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in medium contracts");
				configNode43.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode44.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode44.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in hard contracts");
				configNode44.AddValue("ScienceMultiplier", 1.25, "The multiplier this request places on science in hard contracts");
				configNode44.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
				ConfigNode configNode45 = node3.AddNode("PART_REQUEST", "A potential survey scanner request for satellites");
				ConfigNode configNode46 = configNode45.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode47 = configNode45.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode48 = configNode45.AddNode("Exceptional", "Hard Contracts");
				configNode45.AddValue("Article", "a", "Article (A/An) to be used prior to part description");
				configNode45.AddValue("PartDescription", "resource survey scanner", "Plain speech description of part for request");
				configNode45.AddValue("VesselDescription", "satellite", "Plain speech description of vessel for request");
				configNode45.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode45.AddValue("Part", "SurveyScanner", "A part ID that triggers this request");
				configNode46.AddValue("Weight", 5, "How common this request is in easy contracts");
				configNode46.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in easy contracts");
				configNode46.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
				configNode46.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
				configNode47.AddValue("Weight", 15, "How common this request is in medium contracts");
				configNode47.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in medium contracts");
				configNode47.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in medium contracts");
				configNode47.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in medium contracts");
				configNode48.AddValue("Weight", 25, "How common this request is in hard contracts");
				configNode48.AddValue("FundsMultiplier", 1.15, "The multiplier this request places on funds in hard contracts");
				configNode48.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in hard contracts");
				configNode48.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in hard contracts");
			}
		}
	}

	public static void AddResourceRequests(ConfigNode topNode)
	{
		if (topNode != null)
		{
			ConfigNode node = topNode.GetNode("Base");
			ConfigNode node2 = topNode.GetNode("Station");
			ConfigNode node3 = topNode.GetNode("ISRU");
			ConfigNode configNode = new ConfigNode("RESOURCE_REQUEST", "A potential request for ore possession");
			ConfigNode configNode2 = configNode.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode3 = configNode.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode4 = configNode.AddNode("Exceptional", "Hard Contracts");
			configNode.AddValue("Name", "Ore", "The resource ID");
			configNode.AddValue("Title", "ore", "Lower case plain text resource name");
			configNode.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode.AddValue("Module", "ModuleResourceHarvester", "Must research a part with this module to appear");
			configNode.AddValue("ChanceMultiplier", 1, "Reward multiplier on ISRU delivery contract");
			configNode2.AddValue("Amount", 1000, "How much to have in easy contracts");
			configNode2.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode2.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in easy contracts");
			configNode2.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in easy contracts");
			configNode2.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in easy contracts");
			configNode3.AddValue("Amount", 2000, "How much to have in medium contracts");
			configNode3.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode3.AddValue("FundsMultiplier", 1.2, "The multiplier this request places on funds in medium contracts");
			configNode3.AddValue("ScienceMultiplier", 1.2, "The multiplier this request places on science in medium contracts");
			configNode3.AddValue("ReputationMultiplier", 1.2, "The multiplier this request places on reputation in medium contracts");
			configNode4.AddValue("Amount", 5000, "How much to have in hard contracts");
			configNode4.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode4.AddValue("FundsMultiplier", 1.3, "The multiplier this request places on funds in hard contracts");
			configNode4.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in hard contracts");
			configNode4.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode5 = new ConfigNode("RESOURCE_REQUEST", "A potential request for fuel possession");
			ConfigNode configNode6 = configNode5.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode7 = configNode5.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode8 = configNode5.AddNode("Exceptional", "Hard Contracts");
			configNode5.AddValue("Name", "LiquidFuel", "The resource ID");
			configNode5.AddValue("Title", "liquid fuel", "Lower case plain text resource name");
			configNode5.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode5.AddValue("Part", "fuelTankSmallFlat", "Must research this part to appear");
			configNode5.AddValue("ChanceMultiplier", 1, "Reward multiplier on ISRU delivery contract");
			configNode6.AddValue("Amount", 2000, "How much to have in easy contracts");
			configNode6.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode6.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in easy contracts");
			configNode6.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in easy contracts");
			configNode6.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in easy contracts");
			configNode7.AddValue("Amount", 4000, "How much to have in medium contracts");
			configNode7.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode7.AddValue("FundsMultiplier", 1.2, "The multiplier this request places on funds in medium contracts");
			configNode7.AddValue("ScienceMultiplier", 1.2, "The multiplier this request places on science in medium contracts");
			configNode7.AddValue("ReputationMultiplier", 1.2, "The multiplier this request places on reputation in medium contracts");
			configNode8.AddValue("Amount", 6000, "How much to have in hard contracts");
			configNode8.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode8.AddValue("FundsMultiplier", 1.3, "The multiplier this request places on funds in hard contracts");
			configNode8.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in hard contracts");
			configNode8.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode9 = new ConfigNode("RESOURCE_REQUEST", "A potential request for monopropellant possession");
			ConfigNode configNode10 = configNode9.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode11 = configNode9.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode12 = configNode9.AddNode("Exceptional", "Hard Contracts");
			configNode9.AddValue("Name", "MonoPropellant", "The resource ID");
			configNode9.AddValue("Title", "monopropellant", "Lower case plain text resource name");
			configNode9.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode9.AddValue("Part", "rcsTankMini", "Must research this part to appear");
			configNode9.AddValue("ChanceMultiplier", 1, "Reward multiplier on ISRU delivery contract");
			configNode10.AddValue("Amount", 500, "How much to have in easy contracts");
			configNode10.AddValue("Weight", 2, "How common this request is in easy contracts");
			configNode10.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode10.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode10.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode11.AddValue("Amount", 1000, "How much to have in medium contracts");
			configNode11.AddValue("Weight", 4, "How common this request is in medium contracts");
			configNode11.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode11.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode11.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode12.AddValue("Amount", 1500, "How much to have in hard contracts");
			configNode12.AddValue("Weight", 6, "How common this request is in hard contracts");
			configNode12.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode12.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode12.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode13 = new ConfigNode("RESOURCE_REQUEST", "A potential request for xenon possession");
			ConfigNode configNode14 = configNode13.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode15 = configNode13.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode16 = configNode13.AddNode("Exceptional", "Hard Contracts");
			configNode13.AddValue("Name", "XenonGas", "The resource ID");
			configNode13.AddValue("Title", "xenon gas", "Lower case plain text resource name");
			configNode13.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode13.AddValue("Part", "xenonTank", "Must research this part to appear");
			configNode13.AddValue("ChanceMultiplier", 1, "Reward multiplier on ISRU delivery contract");
			configNode14.AddValue("Amount", 500, "How much to have in easy contracts");
			configNode14.AddValue("Weight", 2, "How common this request is in easy contracts");
			configNode14.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode14.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode14.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode15.AddValue("Amount", 1000, "How much to have in medium contracts");
			configNode15.AddValue("Weight", 4, "How common this request is in medium contracts");
			configNode15.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode15.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode15.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode16.AddValue("Amount", 2000, "How much to have in hard contracts");
			configNode16.AddValue("Weight", 6, "How common this request is in hard contracts");
			configNode16.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode16.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode16.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode17 = new ConfigNode("RESOURCE_REQUEST", "A potential request for electric charge possession");
			ConfigNode configNode18 = configNode17.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode19 = configNode17.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode20 = configNode17.AddNode("Exceptional", "Hard Contracts");
			configNode17.AddValue("Name", "ElectricCharge", "The resource ID");
			configNode17.AddValue("Title", "electric charge", "Lower case plain text resource name");
			configNode17.AddValue("Keyword", "Scientific", "Contract briefing keyword");
			configNode17.AddValue("Part", "batteryPack", "Must research this part to appear");
			configNode17.AddValue("ChanceMultiplier", 1, "Reward multiplier on ISRU delivery contract");
			configNode18.AddValue("Amount", 2500, "How much to have in easy contracts");
			configNode18.AddValue("Weight", 2, "How common this request is in easy contracts");
			configNode18.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode18.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode18.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode19.AddValue("Amount", 5000, "How much to have in medium contracts");
			configNode19.AddValue("Weight", 4, "How common this request is in medium contracts");
			configNode19.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode19.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode19.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode20.AddValue("Amount", 7500, "How much to have in hard contracts");
			configNode20.AddValue("Weight", 6, "How common this request is in hard contracts");
			configNode20.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode20.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode20.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			if (node != null)
			{
				node.AddNode(configNode5);
				node.AddNode(configNode);
				node.AddNode(configNode17);
				node.AddNode(configNode9);
				node.AddNode(configNode13);
			}
			if (node2 != null)
			{
				node2.AddNode(configNode5);
				node2.AddNode(configNode);
				node2.AddNode(configNode17);
				node2.AddNode(configNode9);
				node2.AddNode(configNode13);
			}
			if (node3 != null)
			{
				ConfigNode configNode21 = node3.AddNode("RESOURCE_REQUEST", "A potential request for ore extraction");
				ConfigNode configNode22 = configNode21.AddNode("Trivial", "Easy Contracts");
				ConfigNode configNode23 = configNode21.AddNode("Significant", "Medium Contracts");
				ConfigNode configNode24 = configNode21.AddNode("Exceptional", "Hard Contracts");
				configNode21.AddValue("Name", "Ore", "The resource ID");
				configNode21.AddValue("Title", "ore", "Lower case plain text resource name");
				configNode21.AddValue("Keyword", "Commercial", "Contract briefing keyword");
				configNode21.AddValue("Module", "ModuleResourceHarvester", "A part with this module must be researched for this request to appear");
				configNode21.AddValue("DeliveryMultiplier", 1.8, "Reward multiplier on ISRU delivery contract");
				configNode21.AddValue("Forbidden", "Sun", "A body this resource cannot be extracted from");
				configNode21.AddValue("Forbidden", "Jool", "A body this resource cannot be extracted from");
				configNode22.AddValue("Amount", 500, "How much to extract in easy contracts");
				configNode22.AddValue("DeliveryChance", 50, "The percent chance for a delivery in easy contracts");
				configNode22.AddValue("FundsMultiplier", 1, "The multiplier this request places on funds in easy contracts");
				configNode22.AddValue("ScienceMultiplier", 1, "The multiplier this request places on science in easy contracts");
				configNode22.AddValue("ReputationMultiplier", 1, "The multiplier this request places on reputation in easy contracts");
				configNode23.AddValue("Amount", 1000, "How much to extract in medium contracts");
				configNode23.AddValue("DeliveryChance", 65, "The percent chance for a delivery in medium contracts");
				configNode23.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in medium contracts");
				configNode23.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in medium contracts");
				configNode23.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in medium contracts");
				configNode24.AddValue("Amount", 2500, "How much to extract in hard contracts");
				configNode24.AddValue("DeliveryChance", 80, "The percent chance for a delivery in hard contracts");
				configNode24.AddValue("FundsMultiplier", 1.3, "The multiplier this request places on funds in hard contracts");
				configNode24.AddValue("ScienceMultiplier", 1.3, "The multiplier this request places on science in hard contracts");
				configNode24.AddValue("ReputationMultiplier", 1.3, "The multiplier this request places on reputation in hard contracts");
			}
		}
	}

	public static void AddCrewRequests(ConfigNode topNode)
	{
		if (topNode != null)
		{
			ConfigNode node = topNode.GetNode("Base");
			ConfigNode node2 = topNode.GetNode("Station");
			ConfigNode configNode = new ConfigNode("CREW_REQUEST", "A request for pilot crew members");
			ConfigNode configNode2 = configNode.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode3 = configNode.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode4 = configNode.AddNode("Exceptional", "Hard Contracts");
			configNode.AddValue("Trait", "Pilot", "The crew trait ID");
			configNode.AddValue("Keyword", "Pioneer", "Contract briefing keyword");
			configNode.AddValue("Crew", 1, "How many desired per target part");
			configNode.AddValue("Module", "ModuleCommand", "Modules that encourage this request");
			configNode2.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode2.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode2.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode2.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode3.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode3.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode3.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode3.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode4.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode4.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode4.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode4.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode5 = new ConfigNode("CREW_REQUEST", "A request for engineer crew members");
			ConfigNode configNode6 = configNode5.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode7 = configNode5.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode8 = configNode5.AddNode("Exceptional", "Hard Contracts");
			configNode5.AddValue("Trait", "Engineer", "The crew trait ID");
			configNode5.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode5.AddValue("Crew", 1, "How many desired per target part");
			configNode5.AddValue("Part", "ISRU", "Parts that encourage this request");
			configNode5.AddValue("Part", "MiniISRU", "Parts that encourage this request");
			configNode6.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode6.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode6.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode6.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode7.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode7.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode7.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode7.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode8.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode8.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode8.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode8.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode9 = new ConfigNode("CREW_REQUEST", "A request for scientist crew members");
			ConfigNode configNode10 = configNode9.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode11 = configNode9.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode12 = configNode9.AddNode("Exceptional", "Hard Contracts");
			configNode9.AddValue("Trait", "Scientist", "The crew trait ID");
			configNode9.AddValue("Keyword", "Scientific", "Contract briefing keyword");
			configNode9.AddValue("Crew", 1, "How many desired per target part");
			configNode9.AddValue("Part", "Large.Crewed.Lab", "Parts that encourage this request");
			configNode10.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode10.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode10.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode10.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode11.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode11.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode11.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode11.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode12.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode12.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode12.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode12.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			ConfigNode configNode13 = new ConfigNode("CREW_REQUEST", "A request for tourist visitors");
			ConfigNode configNode14 = configNode13.AddNode("Trivial", "Easy Contracts");
			ConfigNode configNode15 = configNode13.AddNode("Significant", "Medium Contracts");
			ConfigNode configNode16 = configNode13.AddNode("Exceptional", "Hard Contracts");
			configNode13.AddValue("Trait", "Tourist", "The crew trait ID");
			configNode13.AddValue("Keyword", "Commercial", "Contract briefing keyword");
			configNode13.AddValue("Crew", 1, "How many desired per target part");
			configNode13.AddValue("Part", "cupola", "Parts that encourage this request");
			configNode14.AddValue("Weight", 5, "How common this request is in easy contracts");
			configNode14.AddValue("FundsMultiplier", 1.05, "The multiplier this request places on funds in easy contracts");
			configNode14.AddValue("ScienceMultiplier", 1.05, "The multiplier this request places on science in easy contracts");
			configNode14.AddValue("ReputationMultiplier", 1.05, "The multiplier this request places on reputation in easy contracts");
			configNode15.AddValue("Weight", 10, "How common this request is in medium contracts");
			configNode15.AddValue("FundsMultiplier", 1.075, "The multiplier this request places on funds in medium contracts");
			configNode15.AddValue("ScienceMultiplier", 1.075, "The multiplier this request places on science in medium contracts");
			configNode15.AddValue("ReputationMultiplier", 1.075, "The multiplier this request places on reputation in medium contracts");
			configNode16.AddValue("Weight", 15, "How common this request is in hard contracts");
			configNode16.AddValue("FundsMultiplier", 1.1, "The multiplier this request places on funds in hard contracts");
			configNode16.AddValue("ScienceMultiplier", 1.1, "The multiplier this request places on science in hard contracts");
			configNode16.AddValue("ReputationMultiplier", 1.1, "The multiplier this request places on reputation in hard contracts");
			if (node != null)
			{
				node.AddNode(configNode);
				node.AddNode(configNode5);
				node.AddNode(configNode9);
				node.AddNode(configNode13);
			}
			if (node2 != null)
			{
				node2.AddNode(configNode);
				node2.AddNode(configNode5);
				node2.AddNode(configNode9);
				node2.AddNode(configNode13);
			}
		}
	}
}
