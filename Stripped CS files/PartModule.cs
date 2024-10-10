using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class PartModule : MonoBehaviour
{
	public class ReflectedAttributes
	{
		public string className;

		public int classID;

		public KSPModule[] kspModules;

		public PartInfo[] partInfo;

		public FieldInfo[] publicFields;

		public bool isStageable;

		public ReflectedAttributes(Type moduleType)
		{
			className = moduleType.Name;
			classID = className.GetHashCode();
			kspModules = (KSPModule[])moduleType.GetCustomAttributes(typeof(KSPModule), inherit: true);
			partInfo = (PartInfo[])moduleType.GetCustomAttributes(typeof(PartInfo), inherit: true);
			publicFields = moduleType.GetFields(BindingFlags.Instance | BindingFlags.Public);
			Type typeFromHandle = typeof(PartModule);
			MethodInfo[] methods = moduleType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			isStageable = false;
			int num = methods.Length;
			MethodInfo methodInfo;
			do
			{
				if (num-- > 0)
				{
					methodInfo = methods[num];
					continue;
				}
				return;
			}
			while (!(methodInfo.Name == "OnActive") || !(methodInfo.DeclaringType != typeFromHandle));
			isStageable = true;
		}
	}

	public delegate void voidPMDelegate(PartModule pm);

	public delegate bool boolPMBoolDelegate(PartModule pm, bool apply);

	public delegate void voidPMNodeDelegate(PartModule pm, ConfigNode node);

	public delegate void voidPMApplyNodeDelegate(PartModule pm, List<string> appliedUps, ConfigNode node, bool doLoad);

	public delegate bool boolPMApplyUpgrades(PartModule pm, StartState state);

	public enum PartUpgradeState
	{
		NONE,
		LOCKED,
		AVAILABLE
	}

	[Flags]
	public enum StartState
	{
		None = 0,
		Editor = 1,
		PreLaunch = 2,
		Landed = 4,
		Docked = 8,
		Flying = 0x10,
		Splashed = 0x20,
		SubOrbital = 0x40,
		Orbital = 0x80
	}

	[SerializeField]
	[HideInInspector]
	public BaseEventList events;

	[HideInInspector]
	[SerializeField]
	public BaseFieldList fields;

	[SerializeField]
	public string guiName;

	[HideInInspector]
	[SerializeField]
	public BaseActionList actions;

	public string moduleName;

	public static Dictionary<Type, ReflectedAttributes> reflectedAttributeCache = new Dictionary<Type, ReflectedAttributes>();

	public Part _part;

	public bool isEnabled = true;

	public uint persistentId;

	public uint persistentActionsId;

	public ProtoPartModuleSnapshot snapshot;

	public List<AdjusterPartModuleBase> _currentModuleAdjusterList;

	public List<AdjusterPartModuleBase> moduleAdjusterListToAddOnLoad;

	public List<AdjusterPartModuleBase> moduleAdjusterListToRemoveOnLoad;

	[KSPField(isPersistant = true)]
	public bool stagingEnabled = true;

	[KSPField]
	public bool stagingToggleEnabledEditor;

	[KSPField]
	public bool stagingToggleEnabledFlight;

	[KSPField]
	public string stagingEnableText = "";

	[KSPField]
	public string stagingDisableText = "";

	[KSPField]
	public bool overrideStagingIconIfBlank = true;

	[KSPField]
	public bool moduleIsEnabled = true;

	public List<ConfigNode> upgrades;

	[KSPField]
	public bool upgradesApply = true;

	[KSPField]
	public bool upgradesAsk = true;

	[KSPField]
	public bool showUpgradesInModuleInfo;

	public static bool ApplyUpgradesEditorAuto = true;

	public List<string> upgradesApplied = new List<string>();

	public static string UpgradesAvailableString = "#autoLOC_6002273";

	public static string UpgradesLockedString = "#autoLOC_6002274";

	public static boolPMBoolDelegate FindUpgradesDel;

	public static voidPMNodeDelegate LoadUpgradesDel;

	public static voidPMApplyNodeDelegate ApplyUpgradeNodeDel;

	public static boolPMApplyUpgrades ApplyUpgradesDel;

	public static voidPMDelegate SetupExpansion;

	public static voidPMNodeDelegate LoadExpansionNodes;

	public static voidPMNodeDelegate SaveExpansionNodes;

	public static Dictionary<string, ConfigNode> exclusives = new Dictionary<string, ConfigNode>();

	public ModuleResourceHandler resHandler = new ModuleResourceHandler();

	public string ClassName => ModuleAttributes.className;

	public int ClassID => ModuleAttributes.classID;

	public BaseEventList Events => events;

	public BaseFieldList Fields => fields;

	public string GUIName => guiName;

	public BaseActionList Actions => actions;

	public ReflectedAttributes ModuleAttributes { get; set; }

	public Part part
	{
		get
		{
			return _part;
		}
		set
		{
			_part = value;
		}
	}

	public Vessel vessel => part.vessel;

	public uint PersistentId => persistentId;

	public uint PersistentActionsId
	{
		get
		{
			return persistentActionsId;
		}
		set
		{
			persistentActionsId = value;
		}
	}

	public bool HasAdjusters => CurrentModuleAdjusterList.Count > 0;

	public List<AdjusterPartModuleBase> CurrentModuleAdjusterList
	{
		get
		{
			if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
			{
				return _currentModuleAdjusterList;
			}
			return new List<AdjusterPartModuleBase>();
		}
	}

	public void ModularSetup()
	{
		events = new BaseEventList(part, this);
		actions = new BaseActionList(part, this);
		fields = new BaseFieldList(this);
		if (ModuleAttributes.kspModules.Length != 0)
		{
			guiName = ModuleAttributes.kspModules[0].guiName;
		}
		else
		{
			guiName = KSPUtil.PrintModuleName(ModuleAttributes.className);
		}
		if (ModuleAttributes.partInfo.Length != 0 && !string.IsNullOrEmpty(ModuleAttributes.partInfo[0].name))
		{
			moduleName = ModuleAttributes.className;
		}
		else
		{
			moduleName = ModuleAttributes.className;
		}
		AxisGroupsManager.BuildBaseAxisFields(this);
		if (SetupExpansion != null)
		{
			SetupExpansion(this);
		}
	}

	public static ReflectedAttributes GetReflectedAttributes(Type partModuleType)
	{
		if (reflectedAttributeCache.TryGetValue(partModuleType, out var value))
		{
			return value;
		}
		value = new ReflectedAttributes(partModuleType);
		reflectedAttributeCache[partModuleType] = value;
		return value;
	}

	public virtual bool IsStageable()
	{
		return ModuleAttributes.isStageable;
	}

	public virtual bool StagingEnabled()
	{
		return stagingEnabled;
	}

	public virtual void UpdateStagingToggle()
	{
		if (stagingEnabled)
		{
			Events["ToggleStaging"].guiName = GetStagingDisableText();
		}
		else
		{
			Events["ToggleStaging"].guiName = GetStagingEnableText();
		}
		Events["ToggleStaging"].guiActiveEditor = StagingToggleEnabledEditor();
		Events["ToggleStaging"].guiActive = StagingToggleEnabledFlight();
	}

	public virtual void SetStaging(bool newValue)
	{
		stagingEnabled = newValue;
		UpdateStagingToggle();
	}

	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_232342")]
	public void ToggleStaging()
	{
		bool stagingState = !stagingEnabled;
		SetStagingState(stagingState);
		for (int i = 0; i < part.symmetryCounterparts.Count; i++)
		{
			if (!(part.symmetryCounterparts[i] == null))
			{
				object obj = part.symmetryCounterparts[i].GetComponent<ModuleDecouplerBase>();
				ModuleProceduralFairing component = part.symmetryCounterparts[i].GetComponent<ModuleProceduralFairing>();
				if (obj == null)
				{
					obj = component;
				}
				PartModule partModule = (PartModule)obj;
				if (!(partModule == null))
				{
					partModule.SetStagingState(stagingState);
				}
			}
		}
	}

	public void SetStagingState(bool newState)
	{
		stagingEnabled = newState;
		UpdateStagingToggle();
		part.UpdateStageability(propagate: true, iconUpdate: true);
	}

	public virtual bool StagingToggleEnabledEditor()
	{
		return stagingToggleEnabledEditor;
	}

	public virtual bool StagingToggleEnabledFlight()
	{
		return stagingToggleEnabledFlight;
	}

	public virtual string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return Localizer.Format("#autoLOC_232338");
	}

	public virtual string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return Localizer.Format("#autoLOC_232342");
	}

	public bool HasUpgrades()
	{
		return upgrades.Count > 0;
	}

	public bool AppliedUpgrades()
	{
		return upgradesApplied.Count > 0;
	}

	public static PartUpgradeState UpgradesAvailable(Part part)
	{
		return UpgradesAvailable(part, null);
	}

	public static PartUpgradeState UpgradesAvailable(Part part, ConfigNode node)
	{
		PartUpgradeState result = PartUpgradeState.NONE;
		int count = part.Modules.Count;
		while (true)
		{
			if (count-- > 0)
			{
				PartModule partModule = part.Modules[count];
				if (partModule.FindUpgrades(ApplyUpgradesEditorAuto, node))
				{
					break;
				}
				if (partModule.upgrades.Count > 0)
				{
					result = PartUpgradeState.LOCKED;
				}
				continue;
			}
			return result;
		}
		return PartUpgradeState.AVAILABLE;
	}

	public virtual void SaveUpgradesApplied(ConfigNode node)
	{
		int i = 0;
		for (int count = upgradesApplied.Count; i < count; i++)
		{
			node.AddValue("upgrade", upgradesApplied[i]);
		}
	}

	public virtual void LoadUpgradesApplied(List<string> applieds, ConfigNode node)
	{
		if (node != null)
		{
			applieds.Clear();
			int i = 0;
			for (int count = node.values.Count; i < count; i++)
			{
				applieds.Add(node.values[i].value);
			}
		}
	}

	public virtual void LoadUpgrades(ConfigNode node)
	{
		if (LoadUpgradesDel != null)
		{
			LoadUpgradesDel(this, node);
			return;
		}
		ConfigNode configNode = null;
		if (node != null)
		{
			configNode = node.GetNode("UPGRADES");
		}
		if (configNode != null)
		{
			if (upgrades == null)
			{
				upgrades = new List<ConfigNode>();
			}
			else
			{
				upgrades.Clear();
			}
			int i = 0;
			for (int count = configNode.nodes.Count; i < count; i++)
			{
				upgrades.Add(configNode.nodes[i]);
			}
		}
		else
		{
			if (upgrades != null && upgrades.Count != 0)
			{
				return;
			}
			if (part != null && part.partInfo != null && part.partInfo.partPrefab != null)
			{
				int num = part.Modules.IndexOf(this);
				if (num >= 0 && num < part.partInfo.partPrefab.Modules.Count && part.partInfo.partPrefab.Modules[num].GetType() == GetType())
				{
					upgrades = part.partInfo.partPrefab.Modules[num].upgrades;
				}
			}
			if (upgrades == null)
			{
				upgrades = new List<ConfigNode>();
			}
		}
	}

	public virtual bool FindUpgrades(bool fillApplied, ConfigNode node = null)
	{
		if (FindUpgradesDel != null)
		{
			return FindUpgradesDel(this, fillApplied);
		}
		LoadUpgrades(node);
		if (fillApplied)
		{
			upgradesApplied.Clear();
			if (!upgradesApply)
			{
				return false;
			}
		}
		int count;
		if (upgrades != null && (count = upgrades.Count) != 0)
		{
			exclusives.Clear();
			bool flag = false;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					ConfigNode configNode = upgrades[num];
					string value = configNode.GetValue("name__");
					if (!string.IsNullOrEmpty(value))
					{
						if (!fillApplied)
						{
							if (PartUpgradeManager.Handler.IsUnlocked(value))
							{
								break;
							}
						}
						else
						{
							string value2 = configNode.GetValue("ExclusiveWith__");
							if (!string.IsNullOrEmpty(value2) && upgradesApply && PartUpgradeManager.Handler.IsEnabled(value))
							{
								flag = true;
								exclusives[value2] = configNode;
							}
						}
					}
					num++;
					continue;
				}
				int num2 = 0;
				while (true)
				{
					if (num2 < count)
					{
						ConfigNode configNode = upgrades[num2];
						string value3 = configNode.GetValue("name__");
						if (!string.IsNullOrEmpty(value3))
						{
							if (flag)
							{
								string value4 = configNode.GetValue("ExclusiveWith__");
								if (!string.IsNullOrEmpty(value4) && exclusives.TryGetValue(value4, out var value5) && value5 != configNode)
								{
									goto IL_0170;
								}
							}
							if (fillApplied)
							{
								if (upgradesApply && PartUpgradeManager.Handler.IsEnabled(value3))
								{
									upgradesApplied.Add(value3);
								}
							}
							else if (PartUpgradeManager.Handler.IsUnlocked(value3))
							{
								break;
							}
						}
						goto IL_0170;
					}
					return upgradesApplied.Count > 0;
					IL_0170:
					num2++;
				}
				return true;
			}
			return true;
		}
		return false;
	}

	public ConfigNode GetUpgrade(string name)
	{
		int count = upgrades.Count;
		ConfigNode configNode;
		string value;
		do
		{
			if (count-- > 0)
			{
				configNode = upgrades[count];
				value = configNode.GetValue("name__");
				continue;
			}
			return null;
		}
		while (string.IsNullOrEmpty(value) || !(value == name));
		return configNode;
	}

	public virtual void ApplyUpgradeNode(List<string> appliedUps, ConfigNode node, bool doLoad)
	{
		if (ApplyUpgradeNodeDel != null)
		{
			ApplyUpgradeNodeDel(this, appliedUps, node, doLoad);
			return;
		}
		int count = upgradesApplied.Count;
		LoadUpgrades(null);
		if (count <= 0)
		{
			return;
		}
		ConfigNode configNode = new ConfigNode("CURRENTUPGRADE");
		for (int i = 0; i < count; i++)
		{
			ConfigNode upgrade = GetUpgrade(appliedUps[i]);
			if (upgrade != null && PartUpgradeManager.Handler.IsUnlocked(appliedUps[i]))
			{
				string value = upgrade.GetValue("IsExclusiveUpgrade__");
				string value2 = upgrade.GetValue("IsAdditiveUpgrade__");
				if (!string.IsNullOrEmpty(value) && bool.Parse(value))
				{
					configNode.ClearData();
					upgrade.CopyTo(configNode);
				}
				else if (!string.IsNullOrEmpty(value2) && bool.Parse(value2))
				{
					upgrade.CopyTo(configNode);
				}
				else
				{
					upgrade.CopyTo(configNode, overwrite: true);
				}
			}
		}
		if (node != null)
		{
			count = node.nodes.Count;
			for (int j = 0; j < count; j++)
			{
				ConfigNode upgrade = node.nodes[j];
				if (!configNode.HasNode(upgrade.name))
				{
					configNode.RemoveNodes(upgrade.name);
				}
			}
			count = node.values.Count;
			for (int k = 0; k < count; k++)
			{
				if (configNode.HasValue(node.values[k].name))
				{
					configNode.RemoveValues(node.values[k].name);
				}
			}
		}
		if (doLoad)
		{
			Fields.Load(configNode);
			try
			{
				OnLoad(configNode);
			}
			catch (Exception ex)
			{
				Debug.LogError("[Upgrades]: Module " + moduleName + " threw during OnLoad in editor: " + ex);
			}
			UpdateStagingToggle();
		}
		else
		{
			configNode.CopyTo(node);
		}
	}

	public virtual bool ApplyUpgrades(StartState state)
	{
		if (ApplyUpgradesDel != null)
		{
			return ApplyUpgradesDel(this, state);
		}
		if (state == StartState.Editor)
		{
			if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX) && ResearchAndDevelopment.Instance == null)
			{
				StartCoroutine(UpgradeWaitForScenarioModules());
				return false;
			}
			if (FindUpgrades(ApplyUpgradesEditorAuto))
			{
				if (ApplyUpgradesEditorAuto)
				{
					ApplyUpgradeNode(upgradesApplied, null, doLoad: true);
				}
				else
				{
					upgradesApplied.Clear();
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public virtual string GetUpgradeInfo()
	{
		string text = GetInfo();
		int i = 0;
		for (int count = upgradesApplied.Count; i < count; i++)
		{
			ConfigNode upgrade = GetUpgrade(upgradesApplied[i]);
			if (upgrade != null)
			{
				string value = null;
				if (upgrade.TryGetValue("description__", ref value))
				{
					text = text + "\n" + value;
				}
			}
		}
		return text;
	}

	public virtual string PrintUpgrades()
	{
		int count = upgrades.Count;
		if (count > 0)
		{
			string text = Localizer.Format("#autoLOC_232729");
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				PartUpgradeHandler.Upgrade upgrade = PartUpgradeManager.Handler.GetUpgrade(upgrades[i].GetValue("name__"));
				if (upgrade != null && !string.IsNullOrEmpty(upgrade.techRequired))
				{
					text = ((!flag) ? (text + " ") : (text + ", "));
					text += Localizer.Format(ResearchAndDevelopment.GetTechnologyTitle(upgrade.techRequired));
					flag = true;
				}
			}
			if (flag)
			{
				return text + ".";
			}
			return string.Empty;
		}
		return string.Empty;
	}

	public IEnumerator UpgradeWaitForScenarioModules()
	{
		while (ResearchAndDevelopment.Instance == null)
		{
			yield return null;
		}
		if (ApplyUpgrades(StartState.Editor))
		{
			part.Events["ShowUpgradeStats"].guiActiveEditor = true;
		}
	}

	public void Awake()
	{
		part = GetComponent<Part>();
		ModuleAttributes = GetReflectedAttributes(GetType());
		ModularSetup();
		resHandler.SetPartModule(this);
		resHandler.OnAwake();
		OnAwake();
		_currentModuleAdjusterList = new List<AdjusterPartModuleBase>();
		moduleAdjusterListToAddOnLoad = new List<AdjusterPartModuleBase>();
		moduleAdjusterListToRemoveOnLoad = new List<AdjusterPartModuleBase>();
	}

	public void Save(ConfigNode node)
	{
		node.ClearValues();
		node.ClearNodes();
		node.AddValue("name", ClassName);
		node.AddValue("isEnabled", isEnabled);
		if (persistentId != 0)
		{
			node.AddValue("persistentId", persistentId);
		}
		if (HasAdjusters)
		{
			ConfigNode configNode = new ConfigNode("ADJUSTERS");
			for (int i = 0; i < CurrentModuleAdjusterList.Count; i++)
			{
				CurrentModuleAdjusterList[i].Save(configNode.AddNode("ADJUSTERMODULE"));
			}
			node.AddNode(configNode);
		}
		Fields.Save(node);
		Events.OnSave(node.AddNode("EVENTS"));
		Actions.OnSave(node.AddNode("ACTIONS"));
		AxisGroupsManager.SaveAxisFieldNodes(this, node);
		if (SaveExpansionNodes != null)
		{
			SaveExpansionNodes(this, node);
		}
		OnSave(node);
		SaveUpgradesApplied(node.AddNode("UPGRADESAPPLIED"));
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("isEnabled"))
		{
			isEnabled = bool.Parse(node.GetValue("isEnabled"));
		}
		else
		{
			isEnabled = true;
		}
		persistentId = 0u;
		node.TryGetValue("persistentId", ref persistentId);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("ADJUSTERS", ref node2))
		{
			moduleAdjusterListToAddOnLoad.AddRange(AdjusterPartModuleBase.CreateModuleAdjusterList(node2));
		}
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("ADJUSTERSTOREMOVE", ref node3))
		{
			moduleAdjusterListToRemoveOnLoad.AddRange(AdjusterPartModuleBase.CreateModuleAdjusterList(node3));
		}
		LoadUpgrades(node);
		LoadUpgradesApplied(upgradesApplied, node.GetNode("UPGRADESAPPLIED"));
		if (!HighLogic.LoadedSceneIsEditor)
		{
			ApplyUpgradeNode(upgradesApplied, node, doLoad: false);
		}
		Fields.Load(node);
		if (node.HasNode("EVENTS"))
		{
			Events.OnLoad(node.GetNode("EVENTS"));
		}
		if (node.HasNode("ACTIONS"))
		{
			Actions.OnLoad(node.GetNode("ACTIONS"));
		}
		AxisGroupsManager.LoadAxisFieldNodes(this, node);
		if (LoadExpansionNodes != null)
		{
			LoadExpansionNodes(this, node);
		}
		resHandler.OnLoad(node);
		try
		{
			OnLoad(node);
		}
		catch (Exception ex)
		{
			Debug.LogError("Module " + moduleName + " threw during OnLoad: " + ex);
		}
		UpdateStagingToggle();
	}

	public void Copy(PartModule fromModule)
	{
		int count = actions.Count;
		while (count-- > 0)
		{
			actions[count].CopyAction(fromModule.actions[count]);
		}
		int count2 = Fields.Count;
		while (count2-- > 0)
		{
			Fields[count2].CopyField(fromModule.Fields[count2]);
		}
		Fields.SetOriginalValue();
		OnCopy(fromModule);
	}

	public virtual void OnAwake()
	{
	}

	public virtual void OnIconCreate()
	{
	}

	public virtual void OnStart(StartState state)
	{
	}

	public virtual void OnStartFinished(StartState state)
	{
	}

	public virtual void OnStartBeforePartAttachJoint(StartState state)
	{
	}

	public virtual void OnInitialize()
	{
	}

	public virtual void OnActive()
	{
	}

	public virtual void OnInactive()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnFixedUpdate()
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual string GetInfo()
	{
		return "";
	}

	public virtual string GetModuleDisplayName()
	{
		return "";
	}

	public virtual void OnCopy(PartModule fromModule)
	{
	}

	public virtual void OnWillBeCopied(bool asSymCounterpart)
	{
	}

	public virtual void OnWasCopied(PartModule copyPartModule, bool asSymCounterpart)
	{
	}

	public virtual bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		return false;
	}

	public virtual void OnInventoryModeDisable()
	{
	}

	public virtual void OnInventoryModeEnable()
	{
	}

	public virtual void OnConstructionModeUpdate()
	{
	}

	public virtual void OnConstructionModeFixedUpdate()
	{
	}

	public virtual void PromoteToPhysicalPart()
	{
	}

	public virtual void DemoteToPhysicslessPart()
	{
	}

	public virtual void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
	}

	public virtual void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
	}

	public bool IsValidContractObjective(string objectiveType)
	{
		if (!(this is IContractObjectiveModule contractObjectiveModule))
		{
			return false;
		}
		if (!(contractObjectiveModule.GetContractObjectiveType() != objectiveType) && contractObjectiveModule.CheckContractObjectiveValidity())
		{
			return true;
		}
		return false;
	}

	public virtual Color GetCurrentColor()
	{
		return default(Color);
	}

	public virtual Color GetCurrentColor(string fieldName)
	{
		return GetCurrentColor();
	}

	public virtual List<Color> PresetColors()
	{
		return new List<Color> { XKCDColors.Black };
	}

	public virtual void OnColorChanged(Color color)
	{
	}

	public virtual void OnColorChanged(Color color, string pickerID = "")
	{
	}

	public void ApplyAdjustersOnStart()
	{
		for (int i = 0; i < moduleAdjusterListToAddOnLoad.Count; i++)
		{
			AddPartModuleAdjuster(moduleAdjusterListToAddOnLoad[i]);
		}
		for (int j = 0; j < moduleAdjusterListToRemoveOnLoad.Count; j++)
		{
			RemovePartModuleAdjuster(moduleAdjusterListToRemoveOnLoad[j]);
		}
	}

	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < moduleAdjusters.Count; i++)
			{
				AddPartModuleAdjuster(moduleAdjusters[i]);
			}
		}
	}

	public void AddPartModuleAdjuster(AdjusterPartModuleBase newAdjuster)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory") || !MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(GetType().Name))
		{
			return;
		}
		List<Type> list = MissionsUtils.adjusterTypesSupportedByPartModule[GetType().Name];
		if (newAdjuster == null || !list.Contains(newAdjuster.GetType()))
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < CurrentModuleAdjusterList.Count; i++)
		{
			flag = flag || CurrentModuleAdjusterList[i].disableKSPFields;
			flag2 = flag2 || CurrentModuleAdjusterList[i].disableKSPActions;
			flag3 = flag3 || CurrentModuleAdjusterList[i].disableKSPEvents;
		}
		CurrentModuleAdjusterList.AddUnique(newAdjuster);
		for (int j = 0; j < CurrentModuleAdjusterList.Count; j++)
		{
			if (CurrentModuleAdjusterList[j] != newAdjuster)
			{
				CurrentModuleAdjusterList[j].OnPartModuleAdjusterListModified();
			}
		}
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		for (int k = 0; k < CurrentModuleAdjusterList.Count; k++)
		{
			flag4 = flag4 || CurrentModuleAdjusterList[k].disableKSPFields;
			flag5 = flag5 || CurrentModuleAdjusterList[k].disableKSPActions;
			flag6 = flag6 || CurrentModuleAdjusterList[k].disableKSPEvents;
			flag7 = flag7 || CurrentModuleAdjusterList[k].canBeRepaired;
		}
		if (flag4 && !flag)
		{
			int count = fields.Count;
			for (int l = 0; l < count; l++)
			{
				BaseField baseField = fields[l];
				baseField.WasActiveBeforePartWasAdjusted = baseField.guiActive;
				baseField.guiActive = false;
			}
		}
		if (flag5 && !flag2)
		{
			int count2 = actions.Count;
			for (int m = 0; m < count2; m++)
			{
				BaseAction baseAction = actions[m];
				baseAction.wasActiveBeforePartWasAdjusted = baseAction.active;
				baseAction.active = false;
			}
		}
		if (flag6 && !flag3)
		{
			int count3 = events.Count;
			for (int n = 0; n < count3; n++)
			{
				BaseEvent byIndex = events.GetByIndex(n);
				byIndex.wasActiveBeforePartWasAdjusted = byIndex.guiActive;
				byIndex.guiActive = false;
			}
		}
		newAdjuster.Setup(this);
		newAdjuster.Activate();
		GameEvents.onPartModuleAdjusterAdded.Fire(this, newAdjuster);
		OnModuleAdjusterAddedWrapper(newAdjuster);
	}

	public virtual void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
	}

	public void OnModuleAdjusterAddedWrapper(AdjusterPartModuleBase adjuster)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			OnModuleAdjusterAdded(adjuster);
		}
	}

	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory") || !MissionsUtils.adjusterTypesSupportedByPartModule.ContainsKey(GetType().Name))
		{
			return;
		}
		List<Type> list = MissionsUtils.adjusterTypesSupportedByPartModule[GetType().Name];
		for (int i = 0; i < moduleAdjusters.Count; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (moduleAdjusters[i].GetType() == list[j])
				{
					RemovePartModuleAdjuster(moduleAdjusters[i]);
				}
			}
		}
	}

	public void RemovePartModuleAdjuster(AdjusterPartModuleBase oldAdjuster)
	{
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory") || oldAdjuster == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < CurrentModuleAdjusterList.Count; i++)
		{
			flag = flag || CurrentModuleAdjusterList[i].disableKSPFields;
			flag2 = flag2 || CurrentModuleAdjusterList[i].disableKSPActions;
			flag3 = flag3 || CurrentModuleAdjusterList[i].disableKSPEvents;
		}
		for (int num = CurrentModuleAdjusterList.Count - 1; num >= 0; num--)
		{
			if (CurrentModuleAdjusterList[num].adjusterID == oldAdjuster.adjusterID)
			{
				CurrentModuleAdjusterList.RemoveAt(num);
			}
		}
		for (int j = 0; j < CurrentModuleAdjusterList.Count; j++)
		{
			CurrentModuleAdjusterList[j].OnPartModuleAdjusterListModified();
		}
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		for (int k = 0; k < CurrentModuleAdjusterList.Count; k++)
		{
			flag4 = flag4 || CurrentModuleAdjusterList[k].disableKSPFields;
			flag5 = flag5 || CurrentModuleAdjusterList[k].disableKSPActions;
			flag6 = flag6 || CurrentModuleAdjusterList[k].disableKSPEvents;
			flag7 = flag7 || CurrentModuleAdjusterList[k].canBeRepaired;
		}
		if (flag && !flag4)
		{
			int count = fields.Count;
			for (int l = 0; l < count; l++)
			{
				fields[l].guiActive = fields[l].WasActiveBeforePartWasAdjusted;
			}
		}
		if (flag2 && !flag5)
		{
			int count2 = actions.Count;
			for (int m = 0; m < count2; m++)
			{
				actions[m].active = actions[m].wasActiveBeforePartWasAdjusted;
			}
		}
		if (flag3 && !flag6)
		{
			int count3 = events.Count;
			for (int n = 0; n < count3; n++)
			{
				events.GetByIndex(n).guiActive = events.GetByIndex(n).wasActiveBeforePartWasAdjusted;
			}
		}
		oldAdjuster.CleanUp();
		oldAdjuster.Deactivate();
		GameEvents.onPartModuleAdjusterRemoved.Fire(this, oldAdjuster);
		OnModuleAdjusterRemoved(oldAdjuster);
	}

	public virtual void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
	}

	public uint NewPersistentId()
	{
		ClearPersistentId();
		return GetPersistentId();
	}

	public uint GetPersistentId()
	{
		while (persistentId == 0)
		{
			persistentId = (uint)Guid.NewGuid().GetHashCode();
			for (int i = 0; i < part.Modules.Count; i++)
			{
				if (!(part.Modules[i] == this) && part.Modules[i].persistentId == persistentId)
				{
					persistentId = 0u;
				}
			}
		}
		return persistentId;
	}

	public void ClearPersistentId()
	{
		persistentId = 0u;
	}

	public uint GetPersistenActiontId()
	{
		if (part.isClone)
		{
			for (int i = 0; i < part.originalPart.Modules.Count; i++)
			{
				if (part.originalPart.Modules[i].moduleName == moduleName)
				{
					return part.originalPart.Modules[i].persistentActionsId;
				}
			}
		}
		while (persistentActionsId == 0)
		{
			persistentActionsId = (uint)Guid.NewGuid().GetHashCode();
			for (int j = 0; j < part.Modules.Count; j++)
			{
				if (!(part.Modules[j] == this) && part.Modules[j].persistentActionsId == persistentActionsId)
				{
					persistentActionsId = 0u;
				}
			}
		}
		return persistentActionsId;
	}

	public virtual void ResetWheelGroundCheck()
	{
	}
}
