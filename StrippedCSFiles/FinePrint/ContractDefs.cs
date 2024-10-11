using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FinePrint.Utilities;
using UnityEngine;

namespace FinePrint;

[KSPAddon(KSPAddon.Startup.MainMenu, true)]
public class ContractDefs : MonoBehaviour
{
	public static class ARM
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			public static float SolarEjectionMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			public static float SolarEjectionMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			public static float SolarEjectionMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static float SignificantSolarEjectionChance;

		public static float ExceptionalSolarEjectionChance;

		public static float HomeLandingChance;

		public static bool AllowSolarEjections;

		public static bool AllowHomeLandings;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static ARM()
		{
			throw null;
		}
	}

	public static class Base
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			public static float MobileMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			public static float MobileMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			public static float MobileMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static float ContextualChance;

		public static float ContextualAssets;

		public static float TrivialMobileChance;

		public static float SignificantMobileChance;

		public static float ExceptionalMobileChance;

		public static bool AllowMobile;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Base()
		{
			throw null;
		}
	}

	public static class Flag
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Flag()
		{
			throw null;
		}
	}

	public static class Grand
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static float Rarity;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Grand()
		{
			throw null;
		}
	}

	public static class ISRU
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static ISRU()
		{
			throw null;
		}
	}

	public static class Progression
	{
		public static class Funds
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static bool DisableTutorialContracts;

		public static bool DisableProgressionContracts;

		public static double MaxDepthRecord;

		public static double MaxDistanceRecord;

		public static double MaxSpeedRecord;

		public static float OutlierMilestoneMultiplier;

		public static float PassiveBaseRatio;

		public static float PassiveBodyRatio;

		public static int RecordSplit;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Progression()
		{
			throw null;
		}
	}

	public static class Recovery
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static bool AllowKerbalRescue;

		public static bool AllowPartRecovery;

		public static bool AllowCompoundRecovery;

		public static bool AllowLandedVacuum;

		public static bool AllowLandedAtmosphere;

		public static float HighOrbitDifficulty;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Recovery()
		{
			throw null;
		}
	}

	public static class Satellite
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			public static float HomeMultiplier;

			public static float PolarMultiplier;

			public static float SynchronousMultiplier;

			public static float StationaryMultiplier;

			public static float TundraMultiplier;

			public static float KolniyaMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			public static float HomeMultiplier;

			public static float PolarMultiplier;

			public static float SynchronousMultiplier;

			public static float StationaryMultiplier;

			public static float TundraMultiplier;

			public static float KolniyaMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			public static float HomeMultiplier;

			public static float PolarMultiplier;

			public static float SynchronousMultiplier;

			public static float StationaryMultiplier;

			public static float TundraMultiplier;

			public static float KolniyaMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static float AnimationDuration;

		public static float ContextualChance;

		public static float ContextualAssets;

		public static float ContextualHomeAssets;

		public static int NetworkMinimum;

		public static int NetworkMaximum;

		public static double MinimumDeviationWindow;

		public static double TrivialDeviation;

		public static double SignificantDeviation;

		public static double ExceptionalDeviation;

		public static float TrivialAltitudeDifficulty;

		public static float TrivialInclinationDifficulty;

		public static float SignificantAltitudeDifficulty;

		public static float SignificantInclinationDifficulty;

		public static float ExceptionalAltitudeDifficulty;

		public static float ExceptionalInclinationDifficulty;

		public static bool PreferHome;

		public static bool AllowSolar;

		public static bool AllowEquatorial;

		public static bool AllowPolar;

		public static bool AllowSynchronous;

		public static bool AllowStationary;

		public static bool AllowTundra;

		public static bool AllowKolniya;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Satellite()
		{
			throw null;
		}
	}

	public static class Research
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Research()
		{
			throw null;
		}
	}

	public static class Sentinel
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Trivial
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static int MinAsteroids;

			public static int MaxAsteroids;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Trivial()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static int MinAsteroids;

			public static int MaxAsteroids;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static int MinAsteroids;

			public static int MaxAsteroids;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static float ScanTypeBaseMultiplier;

		public static float ScanTypeClassMultiplier;

		public static float ScanTypeInclinationMultiplier;

		public static float ScanTypeEccentricityMultiplier;

		public static int TrivialDeviation;

		public static int SignificantDeviation;

		public static int ExceptionalDeviation;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Sentinel()
		{
			throw null;
		}
	}

	public static class CometDetection
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static CometDetection()
		{
			throw null;
		}
	}

	public static class Station
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			public static float AsteroidMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			public static float AsteroidMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			public static float AsteroidMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static float ContextualChance;

		public static float ContextualAssets;

		public static float TrivialAsteroidChance;

		public static float SignificantAsteroidChance;

		public static float ExceptionalAsteroidChance;

		public static bool AllowAsteroid;

		public static bool AllowSolar;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Station()
		{
			throw null;
		}
	}

	public static class Survey
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float DefaultAdvance;

			public static float DefaultReward;

			public static float DefaultFailure;

			public static float WaypointDefaultReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float DefaultReward;

			public static float WaypointDefaultReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float DefaultReward;

			public static float DefaultFailure;

			public static float WaypointDefaultReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static float ContextualChance;

		public static float ContextualAssets;

		public static int TrivialWaypoints;

		public static int SignificantWaypoints;

		public static int ExceptionalWaypoints;

		public static float HomeNearbyProgressCap;

		public static double TrivialRange;

		public static double SignificantRange;

		public static double ExceptionalRange;

		public static double MinimumTriggerRange;

		public static double MaximumTriggerRange;

		public static double MinimumThreshold;

		public static double MaximumThreshold;

		public static float ThresholdDeviancy;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Survey()
		{
			throw null;
		}
	}

	public static class Test
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static int SubjectsToRepeat;

		public static bool AllowHauls;

		public static int TrivialHaulChance;

		public static int SignificantHaulChance;

		public static int ExceptionalHaulChance;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Test()
		{
			throw null;
		}
	}

	public static class Tour
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float DefaultFare;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static bool FailOnInactive;

		public static float GeeAdventureChance;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Tour()
		{
			throw null;
		}
	}

	public static class DeployedScience
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static float SciencePercentage;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static DeployedScience()
		{
			throw null;
		}
	}

	public static class ROCScienceArm
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float SimpleArmAdvance;

			public static float SimpleArmReward;

			public static float SimpleArmFailure;

			public static float ComplexArmAdvance;

			public static float ComplexArmReward;

			public static float ComplexArmFailure;

			public static float AdvancedArmAdvance;

			public static float AdvancedArmReward;

			public static float AdvancedArmFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float SimpleArmReward;

			public static float ComplexArmReward;

			public static float AdvancedArmReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float SimpleArmReward;

			public static float SimpleArmFailure;

			public static float ComplexArmReward;

			public static float ComplexArmFailure;

			public static float AdvancedArmReward;

			public static float AdvancedArmFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		public static float SimpleArmPercentage;

		public static float ComplexArmPercentage;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static ROCScienceArm()
		{
			throw null;
		}
	}

	public static class ROCScienceRetrieval
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float Advance;

			public static float Reward;

			public static float Failure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float Reward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float Reward;

			public static float Failure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static int MaximumExistent;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static ROCScienceRetrieval()
		{
			throw null;
		}
	}

	public static class CometSample
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Trivial
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Trivial()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static float SciencePercentage;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static CometSample()
		{
			throw null;
		}
	}

	public static class RoverConstruction
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Trivial
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float WayPointMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Trivial()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float WayPointMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float WayPointMultiplier;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static float WayPointMinDistance;

		public static float WayPointMaxDistance;

		public static float WayPointTriggerRange;

		public static float MinimumGeeASL;

		public static float MaximumGeeASL;

		public static string TechNodeRequired;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static RoverConstruction()
		{
			throw null;
		}
	}

	public static class VesselRepair
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Trivial
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Trivial()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static VesselRepair()
		{
			throw null;
		}
	}

	public static class OrbitalConstruction
	{
		public static class Expire
		{
			public static int MinimumExpireDays;

			public static int MaximumExpireDays;

			public static int DeadlineDays;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Expire()
			{
				throw null;
			}
		}

		public static class Funds
		{
			public static float BaseAdvance;

			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Funds()
			{
				throw null;
			}
		}

		public static class Science
		{
			public static float BaseReward;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Science()
			{
				throw null;
			}
		}

		public static class Reputation
		{
			public static float BaseReward;

			public static float BaseFailure;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Reputation()
			{
				throw null;
			}
		}

		public static class Trivial
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Trivial()
			{
				throw null;
			}
		}

		public static class Significant
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Significant()
			{
				throw null;
			}
		}

		public static class Exceptional
		{
			public static float FundsMultiplier;

			public static float ScienceMultiplier;

			public static float ReputationMultiplier;

			public static float MaxOrbitEccentricityFactor;

			public static float OrbitAltitudeFactor;

			public static float OrbitInclinationFactor;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Exceptional()
			{
				throw null;
			}
		}

		public static int MaximumAvailable;

		public static int MaximumActive;

		public static string TechNodeRequired;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static OrbitalConstruction()
		{
			throw null;
		}
	}

	public static ConfigNode config;

	private static ContractDefs instance;

	public static SpriteMap sprites;

	public const string textureBaseUrl = "Squad/Contracts/Icons/";

	public const string configPath = "GameData/Squad/Contracts/Contracts.cfg";

	public const string ObjectiveAntenna = "Antenna";

	public const string ObjectiveBattery = "Battery";

	public const string ObjectiveDock = "Dock";

	public const string ObjectiveGenerator = "Generator";

	public const string ObjectiveGrapple = "Grapple";

	public const string ObjectiveWheel = "Wheel";

	public static int WeightDefault;

	public static int WeightMinimum;

	public static int WeightMaximum;

	public static int WeightAcceptDelta;

	public static int WeightDeclineDelta;

	public static int WeightWithdrawReadDelta;

	public static int WeightWithdrawSeenDelta;

	public static bool DisplayOfferedOrbits;

	public static bool DisplayOfferedWaypoints;

	public static bool SurveyNavigationGhosting;

	public static int AverageAvailableContracts;

	public static float FacilityProgressionFactor;

	public static float SolarOrbitHeatTolerance;

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

	private static List<SurveyDefinition> surveyDefinitions;

	private static bool loadedContractCraftDefs;

	private static bool loadedConstructionPartsList;

	public static ContractDefs Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<SurveyDefinition> SurveyDefinitions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractDefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ContractDefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadDefinitions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void TechResearched(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> result)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadContractCraftDefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadConstructionPartsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PreBuiltCraft GetPreBuiltCraft(string className, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DictionaryValueList<string, List<PreBuiltCraftPosition>> GetPreBuiltCraftPositions(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateDefaultConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LoadConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddSurveyDefinitions(ConfigNode surveyNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddPartRequests(ConfigNode topNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddResourceRequests(ConfigNode topNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddCrewRequests(ConfigNode topNode)
	{
		throw null;
	}
}
