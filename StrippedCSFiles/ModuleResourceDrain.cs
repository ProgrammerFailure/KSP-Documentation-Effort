using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleResourceDrain : PartModule, IConstruction
{
	[Serializable]
	public class ResourceDrainStatus
	{
		public PartResourceDefinition resource;

		public bool isDraining;

		public bool showDrainFX;

		public string drainFXName;

		public int fxPriority;

		public float drainISP;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceDrainStatus()
		{
			throw null;
		}
	}

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006030")]
	public string noResourceAvailable;

	[KSPField(isPersistant = true)]
	public string resourceName;

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900321")]
	[UI_Resources]
	public int resourcesDraining;

	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 20f, minValue = 1f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(isPersistant = true, incrementalSpeed = 1f, axisGroup = KSPAxisGroup.None, guiActiveUnfocused = true, maxValue = 20f, groupDisplayName = "#autoLOC_6006010", minValue = 1f, unfocusedRange = 5f, groupName = "DrainResources", guiActiveEditor = false, guiActive = true, guiName = "#autoLOC_6006008", guiUnits = "%")]
	public float drainRate;

	public float minDrainRate;

	public float maxDrainRate;

	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006003")]
	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6006018", enabledText = "#autoLOC_6006017", affectSymCounterparts = UI_Scene.All)]
	protected bool isDraining;

	private static double epsilon;

	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001017", enabledText = "#autoLOC_6001220", affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "DrainResources", groupDisplayName = "#autoLOC_6006010", unfocusedRange = 5f, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6006015")]
	public bool flowMode;

	private bool isInactive;

	private UI_Resources UI_Resources;

	private double drainAmount;

	private double amtReceived;

	private int highestValue;

	private int highestIndex;

	private float perResourceExhaustVel;

	private float perResourceThrust;

	public List<PartResource> resourcesAvailable;

	private List<ResourceDrainStatus> resourceDrainStatus;

	private int drainedResources;

	private string loadedSavedResources;

	protected List<FXGroup> drainFXGroups;

	protected FXGroup highestPriorityFXGroup;

	private List<string> loadedResources;

	private static string cacheAutoLOC_6006002;

	private static string cacheAutoLOC_6006003;

	private static string cacheAutoLOC_6006004;

	private static string cacheAutoLOC_6006015;

	private static string cacheAutoLOC_6006016;

	private static string cacheAutoLOC_6006031;

	public bool IsDraining
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	public bool IsInactive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ModuleResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(activeEditor = false)]
	public void StartResourceDrainAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(activeEditor = false)]
	public void StopResourceDrainAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(activeEditor = false)]
	public void ToggleResourceDrainAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(activeEditor = false)]
	public void ToggleResourceDrainFlowAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleDrainPartEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TogglePartFlowMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartResourceListChanged(Part hostPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartMenuOpen(UIPartActionWindow window, Part inpPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PartPlaced(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadResourcesAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVFXToUse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FXGroup SelectFXGroupByName(string fxName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePriorityIndices()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasEnoughDrainingResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool DrainResourceNotFinished(int rdsIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourcesToRelease(List<string> resourceNames, bool exclude = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceDrainRate(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceDrainRateAndStatus(string resourceName, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceUpdateResourcePAW(string resourceName, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DrainPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PartContainsResource(string partResource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TurnOnDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckDrainingForEffects(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TurnOffDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TurnOffDrain(List<string> resourcesToStopDraining)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsResourceDraining(PartResource pR)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartResource GetPartResource(string resourceName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TogglePartResource(PartResource resource, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateResourceDrainStatus(int resourceID, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int ResourcesToDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ResourcesToDrainHaveFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetResourceDrainStatus(PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeRotated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
