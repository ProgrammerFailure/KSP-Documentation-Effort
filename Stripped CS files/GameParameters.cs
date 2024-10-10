using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ns9;
using UnityEngine;

public class GameParameters : IConfigNode
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomParameterUI : Attribute
	{
		[SerializeField]
		public string _title;

		[SerializeField]
		public string _toolTip;

		public GameMode gameMode = GameMode.const_6;

		public bool newGameOnly;

		public bool autoPersistance = true;

		public bool unlockedDuringMission;

		public string title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = Localizer.Format(value);
			}
		}

		public string toolTip
		{
			get
			{
				return _toolTip;
			}
			set
			{
				_toolTip = Localizer.Format(value);
			}
		}

		public CustomParameterUI(string title)
		{
			this.title = Localizer.Format(title);
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomStringParameterUI : CustomParameterUI
	{
		public int lines = 1;

		public CustomStringParameterUI(string title)
			: base(title)
		{
			base.title = Localizer.Format(title);
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomIntParameterUI : CustomParameterUI
	{
		public int minValue;

		public int maxValue = 100;

		public int stepSize = 1;

		public string displayFormat = "N0";

		public CustomIntParameterUI(string title)
			: base(title)
		{
			base.title = Localizer.Format(title);
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomFloatParameterUI : CustomParameterUI
	{
		public float minValue;

		public float maxValue = 1f;

		public float logBase;

		public int stepCount = 11;

		public string displayFormat = "N0";

		public bool asPercentage;

		public bool addTextField;

		public CustomFloatParameterUI(string title)
			: base(title)
		{
			base.title = Localizer.Format(title);
		}
	}

	public class FlightParams : ParameterNode
	{
		public bool CanQuickSave = true;

		public bool CanQuickLoad = true;

		public bool CanAutoSave = true;

		public bool CanUseMap = true;

		public bool CanSwitchVesselsNear = true;

		public bool CanSwitchVesselsFar = true;

		public bool CanTimeWarpHigh = true;

		public bool CanTimeWarpLow = true;

		public bool CanEVA = true;

		public bool CanIVA = true;

		public bool CanBoard = true;

		public bool CanRestart = true;

		public bool CanLeaveToEditor = true;

		public bool CanLeaveToTrackingStation = true;

		public bool CanLeaveToSpaceCenter = true;

		public bool CanLeaveToMainMenu = true;
	}

	public class EditorParams : ParameterNode
	{
		public bool CanSave = true;

		public bool CanLoad = true;

		public bool CanStartNew = true;

		public bool CanLaunch = true;

		public bool CanLeaveToSpaceCenter = true;

		public bool CanLeaveToMainMenu;

		public int startUpMode;

		public string craftFileToLoad = "";
	}

	public class TrackingStationParams : ParameterNode
	{
		public bool CanFlyVessel = true;

		public bool CanAbortVessel = true;

		public bool CanLeaveToSpaceCenter = true;

		public bool CanLeaveToMainMenu;
	}

	public class SpaceCenterParams : ParameterNode
	{
		public bool CanGoInVAB = true;

		public bool CanGoInSPH = true;

		public bool CanGoInTrackingStation = true;

		public bool CanLaunchAtPad = true;

		public bool CanLaunchAtRunway = true;

		public bool CanGoToAdmin = true;

		public bool CanGoToAstronautC = true;

		public bool CanGoToMissionControl = true;

		public bool CanGoToRnD = true;

		public bool CanSelectFlag = true;

		public bool CanLeaveToMainMenu = true;
	}

	public class DifficultyParams : ParameterNode
	{
		public bool AutoHireCrews;

		public bool MissingCrewsRespawn = true;

		public float RespawnTimer = (float)GameSettings.DEFAULT_KERBAL_RESPAWN_TIMER;

		public bool BypassEntryPurchaseAfterResearch = true;

		public bool AllowStockVessels;

		public bool IndestructibleFacilities;

		public float ResourceAbundance = 1f;

		public float ReentryHeatScale = 1f;

		public bool EnableCommNet;

		public bool AllowOtherLaunchSites;

		public bool persistKerbalInventories;
	}

	public class CareerParams : ParameterNode
	{
		public string TechTreeUrl = "GameData/Squad/Resources/TechTree.cfg";

		public float StartingFunds = 25000f;

		public float StartingScience;

		public float StartingReputation;

		public float FundsGainMultiplier = 1f;

		public float RepGainMultiplier = 1f;

		public float ScienceGainMultiplier = 1f;

		public float FundsLossMultiplier = 1f;

		public float RepLossMultiplier = 1f;

		public float RepLossDeclined = 1f;
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
		public float KerbalGToleranceMult = 1f;

		[CustomParameterUI("#autoLOC_140960", toolTip = "#autoLoc_6002207")]
		public bool ResourceTransferObeyCrossfeed;

		[CustomParameterUI("#autoLOC_140963", newGameOnly = true, gameMode = (GameMode)12, toolTip = "#autoLoc_6002208")]
		public bool ActionGroupsAlways;

		[CustomFloatParameterUI("#autoLOC_140966", stepCount = 10, logBase = 10f, displayFormat = "F2", maxValue = 1f, minValue = 0.01f, toolTip = "#autoLOC_140967")]
		public float BuildingImpactDamageMult = 0.05f;

		public bool? enableKerbalExperience;

		[CustomParameterUI("#autoLOC_140992", gameMode = GameMode.SANDBOX, toolTip = "#autoLoc_6002211")]
		public bool PartUpgradesInSandbox;

		[CustomParameterUI("#autoLOC_140995", gameMode = (GameMode)6, toolTip = "#autoLoc_6002212")]
		public bool PartUpgradesInCareer = true;

		[CustomParameterUI("#autoLOC_8005010", gameMode = GameMode.MISSION, toolTip = "#autoLOC_8005011")]
		public bool PartUpgradesInMission;

		[CustomParameterUI("#autoLOC_6010001", gameMode = (GameMode)3, toolTip = "#autoLOC_6010002")]
		public bool EnableFullSASInSandbox;

		[CustomParameterUI("#autoLOC_6010001", gameMode = GameMode.MISSION, toolTip = "#autoLOC_6006085")]
		public bool EnableFullSASInMissions;

		public override string Title => Localizer.Format("#autoLOC_140938");

		public override string DisplaySection => "#autoLoc_6002170";

		public override string Section => "Advanced";

		public override int SectionOrder => 0;

		public override GameMode GameMode => GameMode.const_6;

		public override bool HasPresets => true;

		[CustomParameterUI("#autoLOC_140970", newGameOnly = true, gameMode = GameMode.const_6, toolTip = "#autoLoc_6002209")]
		public bool EnableKerbalExperience
		{
			get
			{
				bool? flag = enableKerbalExperience;
				if (!flag.HasValue)
				{
					if (HighLogic.CurrentGame == null)
					{
						return true;
					}
					if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
					{
						return HighLogic.CurrentGame.Mode == Game.Modes.MISSION;
					}
					return true;
				}
				return flag.GetValueOrDefault();
			}
			set
			{
				enableKerbalExperience = value;
			}
		}

		[CustomParameterUI("#autoLOC_140984", gameMode = GameMode.NOTMISSION, toolTip = "#autoLoc_6002210")]
		public bool ImmediateLevelUp { get; set; }

		public bool KerbalExperienceEnabled(Game.Modes gameMode)
		{
			bool? flag = enableKerbalExperience;
			if (!flag.HasValue)
			{
				if (gameMode != Game.Modes.CAREER)
				{
					return gameMode == Game.Modes.MISSION;
				}
				return true;
			}
			return flag.GetValueOrDefault();
		}

		public override void SetDifficultyPreset(Preset preset)
		{
			switch (preset)
			{
			case Preset.Easy:
				BuildingImpactDamageMult = 0.03f;
				ImmediateLevelUp = true;
				break;
			case Preset.Moderate:
				BuildingImpactDamageMult = 0.1f;
				AllowNegativeCurrency = true;
				ResourceTransferObeyCrossfeed = true;
				break;
			case Preset.Hard:
				BuildingImpactDamageMult = 0.2f;
				AllowNegativeCurrency = true;
				ResourceTransferObeyCrossfeed = true;
				break;
			case Preset.Normal:
				break;
			}
		}

		public override bool Interactible(MemberInfo member, GameParameters parameters)
		{
			if (member.Name == "ImmediateLevelUp")
			{
				return parameters.CustomParams<AdvancedParams>().EnableKerbalExperience;
			}
			if (member.Name == "BuildingImpactDamageMult")
			{
				return !parameters.Difficulty.IndestructibleFacilities;
			}
			if (member.Name == "KerbalGToleranceMult")
			{
				return parameters.CustomParams<AdvancedParams>().GKerbalLimits;
			}
			return base.Interactible(member, parameters);
		}

		public override void OnLoad(ConfigNode node)
		{
			if (!node.HasValue("EnableKerbalExperience") && HighLogic.CurrentGame != null)
			{
				EnableKerbalExperience = HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
			}
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

		public CustomParameterNode()
		{
		}

		public virtual void SetDifficultyPreset(Preset preset)
		{
			throw new NotImplementedException();
		}

		public virtual bool Interactible(MemberInfo member, GameParameters parameters)
		{
			return true;
		}

		public virtual bool Enabled(MemberInfo member, GameParameters parameters)
		{
			return true;
		}

		public virtual IList ValidValues(MemberInfo member)
		{
			return null;
		}
	}

	public abstract class ParameterNode : IConfigNode
	{
		public ParameterNode()
		{
		}

		public void Load(ConfigNode node)
		{
			if (node == null)
			{
				Debug.LogWarning("GameParameters: Cannot find settings");
				return;
			}
			int i = 0;
			for (int count = node.nodes.Count; i < count; i++)
			{
				ConfigNode configNode = node.nodes[i];
				FieldInfo field2 = GetType().GetField(configNode.name);
				if (!(field2 == null) && field2.GetValue(this) is IConfigNode configNode2)
				{
					configNode2.Load(configNode);
				}
			}
			foreach (ConfigNode.Value value in node.values)
			{
				MemberInfo[] member = GetType().GetMember(value.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (member.Length == 0)
				{
					continue;
				}
				MemberInfo memberInfo = member[0];
				FieldInfo field = memberInfo as FieldInfo;
				PropertyInfo property = memberInfo as PropertyInfo;
				if (property != null && (!property.CanRead || !property.CanWrite || property.GetIndexParameters().Length != 0))
				{
					continue;
				}
				Type type = ((property != null) ? property.PropertyType : field.FieldType);
				Action<object> action = delegate(object o)
				{
					if (property != null)
					{
						property.SetValue(this, o, null);
					}
					else
					{
						field.SetValue(this, o);
					}
				};
				if (type == typeof(string))
				{
					action(value.value);
				}
				else if (type == typeof(int))
				{
					action(int.Parse(value.value));
				}
				else if (type == typeof(float))
				{
					action(float.Parse(value.value));
				}
				else if (type == typeof(bool))
				{
					action(bool.Parse(value.value));
				}
				else if (type == typeof(double))
				{
					action(double.Parse(value.value));
				}
				else if (type == typeof(Vector2))
				{
					string[] array = value.value.Split(',');
					if (array.Length >= 2)
					{
						action(new Vector2(float.Parse(array[0]), float.Parse(array[1])));
					}
				}
				else if (type == typeof(Vector3))
				{
					string[] array2 = value.value.Split(',');
					if (array2.Length >= 3)
					{
						action(new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2])));
					}
				}
				else if (type.IsEnum)
				{
					object obj = null;
					try
					{
						obj = Enum.Parse(type, value.value);
					}
					catch (Exception ex)
					{
						Debug.LogWarning($"Couldn't parse value '{value.value}' for attribute {value.name} in difficulty parameter {node.name}: {ex.Message}");
					}
					if (obj != null)
					{
						action(obj);
					}
				}
			}
			OnLoad(node);
		}

		public void Save(ConfigNode node)
		{
			MemberInfo[] members = GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			foreach (MemberInfo memberInfo in members)
			{
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				PropertyInfo propertyInfo = memberInfo as PropertyInfo;
				if ((fieldInfo == null && propertyInfo == null) || (propertyInfo != null && (!propertyInfo.CanRead || !propertyInfo.CanWrite || propertyInfo.GetIndexParameters().Length != 0)))
				{
					continue;
				}
				CustomParameterUI[] array = (CustomParameterUI[])memberInfo.GetCustomAttributes(typeof(CustomParameterUI), inherit: true);
				if (array == null || array.Length == 0 || array[0].autoPersistance)
				{
					object obj = ((propertyInfo != null) ? propertyInfo.GetValue(this, null) : fieldInfo.GetValue(this));
					if (obj is IConfigNode configNode)
					{
						configNode.Save(node.AddNode(memberInfo.Name));
					}
					else if (obj != null)
					{
						string name = memberInfo.Name;
						string text = obj.ToString();
						PDebug.Log("Parameter: " + name + ", " + text, PDebug.DebugLevel.GameSettings);
						node.AddValue(name, text);
					}
				}
			}
			OnSave(node);
		}

		public virtual void OnLoad(ConfigNode node)
		{
		}

		public virtual void OnSave(ConfigNode node)
		{
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
		const_6 = 15
	}

	public FlightParams Flight;

	public EditorParams Editor;

	public TrackingStationParams TrackingStation;

	public SpaceCenterParams SpaceCenter;

	public DifficultyParams Difficulty;

	public CareerParams Career;

	public Preset preset;

	public static List<Type> ParameterTypes;

	public Dictionary<Type, CustomParameterNode> customParams = new Dictionary<Type, CustomParameterNode>();

	public static Dictionary<Preset, GameParameters> DifficultyPresets;

	static GameParameters()
	{
		ParameterTypes = null;
		DifficultyPresets = new Dictionary<Preset, GameParameters>();
		GenerateParameterTypes();
	}

	public GameParameters()
	{
		Flight = new FlightParams();
		Editor = new EditorParams();
		TrackingStation = new TrackingStationParams();
		SpaceCenter = new SpaceCenterParams();
		Difficulty = new DifficultyParams();
		Career = new CareerParams();
		preset = Preset.Normal;
		int count = ParameterTypes.Count;
		while (count-- > 0)
		{
			Type type = ParameterTypes[count];
			customParams[type] = (CustomParameterNode)Activator.CreateInstance(type);
		}
		ParameterTypes.Sort((Type a, Type b) => customParams[a].Title.CompareTo(customParams[b].Title));
	}

	public GameParameters(ConfigNode node)
		: this()
	{
		Load(node);
	}

	public static void GenerateParameterTypes()
	{
		if (ParameterTypes != null)
		{
			return;
		}
		ParameterTypes = new List<Type>();
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(CustomParameterNode)) && !t.IsAbstract)
			{
				ParameterTypes.Add(t);
				Debug.Log($"[GameParameters]: Loaded custom parameter class {t.Name}.");
			}
		});
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("preset"))
		{
			preset = (Preset)Enum.Parse(typeof(Preset), node.GetValue("preset"));
		}
		foreach (ConfigNode node2 in node.nodes)
		{
			switch (node2.name)
			{
			case "TRACKINGSTATION":
				TrackingStation.Load(node2);
				break;
			case "SPACECENTER":
				SpaceCenter.Load(node2);
				break;
			case "EDITOR":
				Editor.Load(node2);
				break;
			case "FLIGHT":
				Flight.Load(node2);
				break;
			case "CAREER":
				Career.Load(node2);
				break;
			default:
				LoadCustomNode(node2);
				break;
			case "DIFFICULTY":
				Difficulty.Load(node2);
				break;
			case "RESEARCHANDDEVELOPMENT":
				break;
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("preset", preset.ToString());
		Flight.Save(node.AddNode("FLIGHT"));
		Editor.Save(node.AddNode("EDITOR"));
		TrackingStation.Save(node.AddNode("TRACKINGSTATION"));
		SpaceCenter.Save(node.AddNode("SPACECENTER"));
		Difficulty.Save(node.AddNode("DIFFICULTY"));
		Career.Save(node.AddNode("CAREER"));
		Dictionary<Type, CustomParameterNode>.Enumerator enumerator = customParams.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ConfigNode node2 = new ConfigNode(enumerator.Current.Key.Name);
				node.AddNode(node2);
				enumerator.Current.Value.Save(node2);
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public T CustomParams<T>() where T : CustomParameterNode
	{
		if (!customParams.TryGetValue(typeof(T), out var value))
		{
			throw new ArgumentException($"Couldn't find custom parameter {typeof(T).Name}!");
		}
		return (T)value;
	}

	public CustomParameterNode CustomParams(Type type)
	{
		CustomParameterNode value = null;
		if (!(type == null) && customParams.TryGetValue(type, out value))
		{
			return value;
		}
		Debug.LogWarning(string.Format("Couldn't find custom parameter {0}!", (type == null) ? "null" : type.Name));
		return null;
	}

	public void LoadCustomNode(ConfigNode node)
	{
		Type type = null;
		int count = ParameterTypes.Count;
		while (count-- > 0)
		{
			if (ParameterTypes[count].Name == node.name)
			{
				type = ParameterTypes[count];
				break;
			}
		}
		CustomParameterNode value = null;
		if (!(type == null) && customParams.TryGetValue(type, out value))
		{
			value.Load(node);
		}
		else
		{
			Debug.LogWarning($"[GameParameters]: Couldn't find type for custom parameter {node.name}.");
		}
	}

	public static void SetDifficultyPresets()
	{
		if (DifficultyPresets == null)
		{
			DifficultyPresets = new Dictionary<Preset, GameParameters>();
		}
		if (DifficultyPresets.Keys.Count != 5)
		{
			DifficultyPresets.Clear();
			GameParameters gameParameters = new GameParameters();
			gameParameters.Difficulty.AllowStockVessels = true;
			gameParameters.Difficulty.AllowOtherLaunchSites = true;
			gameParameters.Difficulty.IndestructibleFacilities = true;
			gameParameters.Difficulty.ResourceAbundance = 1.2f;
			gameParameters.Difficulty.ReentryHeatScale = 0.5f;
			gameParameters.Difficulty.EnableCommNet = false;
			gameParameters.Difficulty.persistKerbalInventories = false;
			gameParameters.Career.StartingFunds = 250000f;
			gameParameters.Career.FundsGainMultiplier = 2f;
			gameParameters.Career.RepGainMultiplier = 2f;
			gameParameters.Career.ScienceGainMultiplier = 2f;
			gameParameters.Career.FundsLossMultiplier = 0.5f;
			gameParameters.Career.RepLossMultiplier = 0.5f;
			gameParameters.Career.RepLossDeclined = 0f;
			DifficultyPresets.Add(Preset.Easy, gameParameters);
			gameParameters = new GameParameters();
			gameParameters.Difficulty.AllowOtherLaunchSites = true;
			DifficultyPresets.Add(Preset.Normal, gameParameters);
			gameParameters.Difficulty.EnableCommNet = true;
			gameParameters = new GameParameters();
			gameParameters.Difficulty.MissingCrewsRespawn = false;
			gameParameters.Difficulty.BypassEntryPurchaseAfterResearch = false;
			gameParameters.Difficulty.ResourceAbundance = 0.8f;
			gameParameters.Difficulty.ReentryHeatScale = 1f;
			gameParameters.Difficulty.AllowOtherLaunchSites = true;
			gameParameters.Difficulty.persistKerbalInventories = false;
			gameParameters.Career.StartingFunds = 15000f;
			gameParameters.Career.FundsGainMultiplier = 0.9f;
			gameParameters.Career.RepGainMultiplier = 0.9f;
			gameParameters.Career.ScienceGainMultiplier = 0.9f;
			gameParameters.Career.FundsLossMultiplier = 1.5f;
			gameParameters.Career.RepLossMultiplier = 1.5f;
			gameParameters.Career.RepLossDeclined = 2f;
			gameParameters.Difficulty.EnableCommNet = true;
			DifficultyPresets.Add(Preset.Moderate, gameParameters);
			gameParameters = new GameParameters();
			gameParameters.Difficulty.AllowStockVessels = false;
			gameParameters.Difficulty.AllowOtherLaunchSites = true;
			gameParameters.Difficulty.MissingCrewsRespawn = false;
			gameParameters.Difficulty.BypassEntryPurchaseAfterResearch = false;
			gameParameters.Difficulty.ResourceAbundance = 0.5f;
			gameParameters.Difficulty.ReentryHeatScale = 1f;
			gameParameters.Difficulty.EnableCommNet = true;
			gameParameters.Difficulty.persistKerbalInventories = false;
			gameParameters.Flight.CanRestart = false;
			gameParameters.Flight.CanLeaveToEditor = false;
			gameParameters.Flight.CanQuickLoad = false;
			gameParameters.Career.StartingFunds = 10000f;
			gameParameters.Career.FundsGainMultiplier = 0.6f;
			gameParameters.Career.RepGainMultiplier = 0.6f;
			gameParameters.Career.ScienceGainMultiplier = 0.6f;
			gameParameters.Career.FundsLossMultiplier = 2f;
			gameParameters.Career.RepLossMultiplier = 2f;
			gameParameters.Career.RepLossDeclined = 3f;
			DifficultyPresets.Add(Preset.Hard, gameParameters);
			gameParameters = new GameParameters();
			DifficultyPresets.Add(Preset.Custom, gameParameters);
		}
	}

	public static GameParameters GetDefaultParameters(Game.Modes mode, Preset p)
	{
		GameParameters value = null;
		SetDifficultyPresets();
		if (!DifficultyPresets.TryGetValue(p, out value))
		{
			value = new GameParameters();
		}
		else
		{
			ConfigNode node = new ConfigNode("PARAMS");
			value.Save(node);
			value = new GameParameters(node);
		}
		switch (mode)
		{
		case Game.Modes.SANDBOX:
			value.Difficulty.BypassEntryPurchaseAfterResearch = true;
			value.Difficulty.AllowStockVessels = p != Preset.Hard;
			value.CustomParams<AdvancedParams>().EnableKerbalExperience = false;
			break;
		default:
			value.CustomParams<AdvancedParams>().EnableKerbalExperience = true;
			value.Difficulty.AllowOtherLaunchSites = false;
			value.Difficulty.persistKerbalInventories = false;
			break;
		case Game.Modes.SCIENCE_SANDBOX:
			value.Difficulty.BypassEntryPurchaseAfterResearch = true;
			value.CustomParams<AdvancedParams>().EnableKerbalExperience = false;
			break;
		case Game.Modes.MISSION:
			value.SpaceCenter.CanGoToRnD = false;
			value.SpaceCenter.CanGoToAdmin = false;
			value.SpaceCenter.CanGoToMissionControl = false;
			value.Difficulty.AllowOtherLaunchSites = false;
			value.Difficulty.BypassEntryPurchaseAfterResearch = true;
			value.CustomParams<AdvancedParams>().EnableKerbalExperience = true;
			value.Difficulty.persistKerbalInventories = false;
			break;
		}
		Dictionary<Type, CustomParameterNode>.Enumerator enumerator = value.customParams.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.HasPresets)
				{
					try
					{
						enumerator.Current.Value.SetDifficultyPreset(p);
					}
					catch (Exception exception)
					{
						Debug.LogError($"Error calling custom SetDifficultyPreset method in type {enumerator.Current}:");
						Debug.LogException(exception);
					}
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		value.preset = p;
		return value;
	}

	public static Color GetPresetColor(Preset p)
	{
		return p switch
		{
			Preset.Easy => XKCDColors.RadioactiveGreen, 
			Preset.Normal => XKCDColors.KSPBadassGreen, 
			Preset.Moderate => XKCDColors.GreenishYellow, 
			Preset.Hard => XKCDColors.KSPNotSoGoodOrange, 
			_ => XKCDColors.BrightCyan, 
		};
	}

	public static string GetPresetColorHex(Preset p)
	{
		return p switch
		{
			Preset.Easy => XKCDColors.HexFormat.RadioactiveGreen, 
			Preset.Normal => XKCDColors.HexFormat.KSPBadassGreen, 
			Preset.Moderate => XKCDColors.HexFormat.GreenishYellow, 
			Preset.Hard => XKCDColors.HexFormat.KSPNotSoGoodOrange, 
			_ => XKCDColors.HexFormat.BrightCyan, 
		};
	}
}
