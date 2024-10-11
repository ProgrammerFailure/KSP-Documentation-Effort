using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameParameters : IConfigNode
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomParameterUI : Attribute
	{
		[SerializeField]
		private string _title;

		[SerializeField]
		private string _toolTip;

		public GameMode gameMode;

		public bool newGameOnly;

		public bool autoPersistance;

		public bool unlockedDuringMission;

		public string title
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

		public string toolTip
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
		public CustomParameterUI(string title)
		{
			throw null;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomStringParameterUI : CustomParameterUI
	{
		public int lines;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CustomStringParameterUI(string title)
		{
			throw null;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomIntParameterUI : CustomParameterUI
	{
		public int minValue;

		public int maxValue;

		public int stepSize;

		public string displayFormat;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CustomIntParameterUI(string title)
		{
			throw null;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomFloatParameterUI : CustomParameterUI
	{
		public float minValue;

		public float maxValue;

		public float logBase;

		public int stepCount;

		public string displayFormat;

		public bool asPercentage;

		public bool addTextField;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CustomFloatParameterUI(string title)
		{
			throw null;
		}
	}

	public class FlightParams : ParameterNode
	{
		public bool CanQuickSave;

		public bool CanQuickLoad;

		public bool CanAutoSave;

		public bool CanUseMap;

		public bool CanSwitchVesselsNear;

		public bool CanSwitchVesselsFar;

		public bool CanTimeWarpHigh;

		public bool CanTimeWarpLow;

		public bool CanEVA;

		public bool CanIVA;

		public bool CanBoard;

		public bool CanRestart;

		public bool CanLeaveToEditor;

		public bool CanLeaveToTrackingStation;

		public bool CanLeaveToSpaceCenter;

		public bool CanLeaveToMainMenu;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FlightParams()
		{
			throw null;
		}
	}

	public class EditorParams : ParameterNode
	{
		public bool CanSave;

		public bool CanLoad;

		public bool CanStartNew;

		public bool CanLaunch;

		public bool CanLeaveToSpaceCenter;

		public bool CanLeaveToMainMenu;

		public int startUpMode;

		public string craftFileToLoad;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public EditorParams()
		{
			throw null;
		}
	}

	public class TrackingStationParams : ParameterNode
	{
		public bool CanFlyVessel;

		public bool CanAbortVessel;

		public bool CanLeaveToSpaceCenter;

		public bool CanLeaveToMainMenu;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TrackingStationParams()
		{
			throw null;
		}
	}

	public class SpaceCenterParams : ParameterNode
	{
		public bool CanGoInVAB;

		public bool CanGoInSPH;

		public bool CanGoInTrackingStation;

		public bool CanLaunchAtPad;

		public bool CanLaunchAtRunway;

		public bool CanGoToAdmin;

		public bool CanGoToAstronautC;

		public bool CanGoToMissionControl;

		public bool CanGoToRnD;

		public bool CanSelectFlag;

		public bool CanLeaveToMainMenu;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpaceCenterParams()
		{
			throw null;
		}
	}

	public class DifficultyParams : ParameterNode
	{
		public bool AutoHireCrews;

		public bool MissingCrewsRespawn;

		public float RespawnTimer;

		public bool BypassEntryPurchaseAfterResearch;

		public bool AllowStockVessels;

		public bool IndestructibleFacilities;

		public float ResourceAbundance;

		public float ReentryHeatScale;

		public bool EnableCommNet;

		public bool AllowOtherLaunchSites;

		public bool persistKerbalInventories;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DifficultyParams()
		{
			throw null;
		}
	}

	public class CareerParams : ParameterNode
	{
		public string TechTreeUrl;

		public float StartingFunds;

		public float StartingScience;

		public float StartingReputation;

		public float FundsGainMultiplier;

		public float RepGainMultiplier;

		public float ScienceGainMultiplier;

		public float FundsLossMultiplier;

		public float RepLossMultiplier;

		public float RepLossDeclined;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CareerParams()
		{
			throw null;
		}
	}

	public class AdvancedParams : CustomParameterNode
	{
		[CustomParameterUI("#autoLOC_140944", gameMode = (GameMode)14, toolTip = "#autoLoc_6002203")]
		public bool AllowNegativeCurrency;

		[CustomParameterUI("#autoLOC_140947", toolTip = "#autoLoc_6002204")]
		public bool PressurePartLimits;

		[CustomParameterUI("#autoLOC_140950", toolTip = "#autoLoc_6002205")]
		public bool GPartLimits;

		[CustomParameterUI("#autoLOC_140953", toolTip = "#autoLoc_6002206")]
		public bool GKerbalLimits;

		[CustomFloatParameterUI("#autoLOC_140956", stepCount = 10, logBase = 10f, displayFormat = "F2", maxValue = 10f, minValue = 0.01f, toolTip = "#autoLOC_140957")]
		public float KerbalGToleranceMult;

		[CustomParameterUI("#autoLOC_140960", toolTip = "#autoLoc_6002207")]
		public bool ResourceTransferObeyCrossfeed;

		[CustomParameterUI("#autoLOC_140963", newGameOnly = true, gameMode = (GameMode)12, toolTip = "#autoLoc_6002208")]
		public bool ActionGroupsAlways;

		[CustomFloatParameterUI("#autoLOC_140966", stepCount = 10, logBase = 10f, displayFormat = "F2", maxValue = 1f, minValue = 0.01f, toolTip = "#autoLOC_140967")]
		public float BuildingImpactDamageMult;

		private bool? enableKerbalExperience;

		[CustomParameterUI("#autoLOC_140992", gameMode = GameMode.SANDBOX, toolTip = "#autoLoc_6002211")]
		public bool PartUpgradesInSandbox;

		[CustomParameterUI("#autoLOC_140995", gameMode = (GameMode)6, toolTip = "#autoLoc_6002212")]
		public bool PartUpgradesInCareer;

		[CustomParameterUI("#autoLOC_8005010", gameMode = GameMode.MISSION, toolTip = "#autoLOC_8005011")]
		public bool PartUpgradesInMission;

		[CustomParameterUI("#autoLOC_6010001", gameMode = (GameMode)3, toolTip = "#autoLOC_6010002")]
		public bool EnableFullSASInSandbox;

		[CustomParameterUI("#autoLOC_6010001", gameMode = GameMode.MISSION, toolTip = "#autoLOC_6006085")]
		public bool EnableFullSASInMissions;

		public override string Title
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public override string DisplaySection
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public override string Section
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public override int SectionOrder
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public override GameMode GameMode
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public override bool HasPresets
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[CustomParameterUI("#autoLOC_140970", newGameOnly = true, gameMode = GameMode.ANY, toolTip = "#autoLoc_6002209")]
		public bool EnableKerbalExperience
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

		[CustomParameterUI("#autoLOC_140984", gameMode = GameMode.NOTMISSION, toolTip = "#autoLoc_6002210")]
		public bool ImmediateLevelUp
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AdvancedParams()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool KerbalExperienceEnabled(Game.Modes gameMode)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void SetDifficultyPreset(Preset preset)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override bool Interactible(MemberInfo member, GameParameters parameters)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void OnLoad(ConfigNode node)
		{
			throw null;
		}
	}

	public abstract class CustomParameterNode : ParameterNode
	{
		public abstract string Title { get; }

		public abstract string DisplaySection { get; }

		public abstract string Section { get; }

		public abstract int SectionOrder { get; }

		public abstract GameMode GameMode { get; }

		public abstract bool HasPresets { get; }

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected CustomParameterNode()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void SetDifficultyPreset(Preset preset)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual bool Interactible(MemberInfo member, GameParameters parameters)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual bool Enabled(MemberInfo member, GameParameters parameters)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual IList ValidValues(MemberInfo member)
		{
			throw null;
		}
	}

	public abstract class ParameterNode : IConfigNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected ParameterNode()
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void OnLoad(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void OnSave(ConfigNode node)
		{
			throw null;
		}
	}

	public enum Preset
	{
		[Description("#autoLOC_7000039")]
		Easy,
		[Description("#autoLOC_7000040")]
		Normal,
		[Description("#autoLOC_7000041")]
		Moderate,
		[Description("#autoLOC_7000042")]
		Hard,
		[Description("#autoLOC_7000043")]
		Custom
	}

	public enum GameMode
	{
		NONE = 0,
		SANDBOX = 1,
		SCIENCE = 2,
		CAREER = 4,
		MISSION = 8,
		NOTMISSION = 7,
		ANY = 15
	}

	public FlightParams Flight;

	public EditorParams Editor;

	public TrackingStationParams TrackingStation;

	public SpaceCenterParams SpaceCenter;

	public DifficultyParams Difficulty;

	public CareerParams Career;

	public Preset preset;

	public static List<Type> ParameterTypes;

	private Dictionary<Type, CustomParameterNode> customParams;

	public static Dictionary<Preset, GameParameters> DifficultyPresets;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GameParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameParameters(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateParameterTypes()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T CustomParams<T>() where T : CustomParameterNode
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CustomParameterNode CustomParams(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadCustomNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetDifficultyPresets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameParameters GetDefaultParameters(Game.Modes mode, Preset p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GetPresetColor(Preset p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPresetColorHex(Preset p)
	{
		throw null;
	}
}
