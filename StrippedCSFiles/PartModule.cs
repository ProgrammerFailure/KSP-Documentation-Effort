using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReflectedAttributes(Type moduleType)
		{
			throw null;
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

	[CompilerGenerated]
	private sealed class _003CUpgradeWaitForScenarioModules_003Ed__99 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PartModule _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CUpgradeWaitForScenarioModules_003Ed__99(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[SerializeField]
	[HideInInspector]
	private BaseEventList events;

	[HideInInspector]
	[SerializeField]
	private BaseFieldList fields;

	[SerializeField]
	private string guiName;

	[HideInInspector]
	[SerializeField]
	private BaseActionList actions;

	public string moduleName;

	protected static Dictionary<Type, ReflectedAttributes> reflectedAttributeCache;

	private Part _part;

	public bool isEnabled;

	private uint persistentId;

	private uint persistentActionsId;

	public ProtoPartModuleSnapshot snapshot;

	private List<AdjusterPartModuleBase> _currentModuleAdjusterList;

	private List<AdjusterPartModuleBase> moduleAdjusterListToAddOnLoad;

	private List<AdjusterPartModuleBase> moduleAdjusterListToRemoveOnLoad;

	[KSPField(isPersistant = true)]
	public bool stagingEnabled;

	[KSPField]
	public bool stagingToggleEnabledEditor;

	[KSPField]
	public bool stagingToggleEnabledFlight;

	[KSPField]
	public string stagingEnableText;

	[KSPField]
	public string stagingDisableText;

	[KSPField]
	public bool overrideStagingIconIfBlank;

	[KSPField]
	public bool moduleIsEnabled;

	public List<ConfigNode> upgrades;

	[KSPField]
	public bool upgradesApply;

	[KSPField]
	public bool upgradesAsk;

	[KSPField]
	public bool showUpgradesInModuleInfo;

	public static bool ApplyUpgradesEditorAuto;

	public List<string> upgradesApplied;

	public static string UpgradesAvailableString;

	public static string UpgradesLockedString;

	public static boolPMBoolDelegate FindUpgradesDel;

	public static voidPMNodeDelegate LoadUpgradesDel;

	public static voidPMApplyNodeDelegate ApplyUpgradeNodeDel;

	public static boolPMApplyUpgrades ApplyUpgradesDel;

	public static voidPMDelegate SetupExpansion;

	public static voidPMNodeDelegate LoadExpansionNodes;

	public static voidPMNodeDelegate SaveExpansionNodes;

	protected static Dictionary<string, ConfigNode> exclusives;

	public ModuleResourceHandler resHandler;

	public string ClassName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ClassID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseEventList Events
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseFieldList Fields
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string GUIName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseActionList Actions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ReflectedAttributes ModuleAttributes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public Part part
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

	public Vessel vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint PersistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint PersistentActionsId
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

	public bool HasAdjusters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<AdjusterPartModuleBase> CurrentModuleAdjusterList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModularSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ReflectedAttributes GetReflectedAttributes(Type partModuleType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool StagingEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateStagingToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetStaging(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_232342")]
	public void ToggleStaging()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStagingState(bool newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool StagingToggleEnabledEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool StagingToggleEnabledFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetStagingEnableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetStagingDisableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AppliedUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartUpgradeState UpgradesAvailable(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartUpgradeState UpgradesAvailable(Part part, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SaveUpgradesApplied(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LoadUpgradesApplied(List<string> applieds, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LoadUpgrades(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool FindUpgrades(bool fillApplied, ConfigNode node = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ConfigNode GetUpgrade(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ApplyUpgradeNode(List<string> appliedUps, ConfigNode node, bool doLoad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ApplyUpgrades(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetUpgradeInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string PrintUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpgradeWaitForScenarioModules_003Ed__99))]
	protected IEnumerator UpgradeWaitForScenarioModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Copy(PartModule fromModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnIconCreate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnStartBeforePartAttachJoint(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInitialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCopy(PartModule fromModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnWillBeCopied(bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnWasCopied(PartModule copyPartModule, bool asSymCounterpart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnConstructionModeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnConstructionModeFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PromoteToPhysicalPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DemoteToPhysicslessPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsValidContractObjective(string objectiveType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetCurrentColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetCurrentColor(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual List<Color> PresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnColorChanged(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnColorChanged(Color color, string pickerID = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyAdjustersOnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartModuleAdjuster(AdjusterPartModuleBase newAdjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnModuleAdjusterAddedWrapper(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjuster(AdjusterPartModuleBase oldAdjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint NewPersistentId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint GetPersistentId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearPersistentId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint GetPersistenActiontId()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal virtual void ResetWheelGroundCheck()
	{
		throw null;
	}
}
